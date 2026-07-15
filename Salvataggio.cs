using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Project;

/// <summary>
/// Dati serializzati su file. Il mondo viene sempre ricostruito da
/// Global.Inizializza(): qui salviamo solo ciò che può cambiare durante
/// la partita, identificando porte/casse/terminali/oggetti per nome
/// (i nomi devono quindi restare unici nella mappa).
/// </summary>
class DatiSalvataggio
{
    public string nome { get; set; } = "Comandante";
    public int vita { get; set; } = 100;
    public int contatorePassi { get; set; }
    public int componentiNavetta { get; set; }
    public int[] coordinate { get; set; } = new[] { 5, 2 };

    // Stato dell'evento dell'IA ostile
    public string eventoStanza { get; set; } = "";
    public bool eventoAttivo { get; set; }
    public bool eventoRisolto { get; set; }
    public int eventoSecondiRimasti { get; set; }

    // Nomi degli oggetti nell'inventario, dal fondo alla cima della pila
    public List<string> inventario { get; set; } = new();

    // Stati per nome
    public Dictionary<string, string> porte { get; set; } = new();
    public Dictionary<string, string> terminali { get; set; } = new();
    public Dictionary<string, string> casse { get; set; } = new();
    public Dictionary<string, bool> casseConContenuto { get; set; } = new();

    // Nome stanza -> nomi degli oggetti mobili presenti sul pavimento
    public Dictionary<string, List<string>> oggettiNelleStanze { get; set; } = new();

    // Nome stanza -> il personaggio presente è vivo?
    public Dictionary<string, bool> personaggiVivi { get; set; } = new();
}

static class Salvataggio
{
    public const string FileSalvataggio = "salvataggio.json";

    public static bool Esiste() => File.Exists(FileSalvataggio);

    /// <summary>
    /// Salva lo stato corrente della partita su file JSON.
    /// Ritorna true se il salvataggio è andato a buon fine.
    /// </summary>
    public static bool Salva(Giocatore player)
    {
        try
        {
            DatiSalvataggio dati = new DatiSalvataggio
            {
                nome = player.nome,
                vita = player.vita,
                contatorePassi = player.contatorePassi,
                componentiNavetta = Global.componentiNavettaInstallati,
                eventoStanza = EventoIA.stanzaEvento,
                eventoAttivo = EventoIA.attivo,
                eventoRisolto = EventoIA.risolto,
                eventoSecondiRimasti = EventoIA.SecondiRimasti,
                coordinate = new[] { player.coordinate[0], player.coordinate[1] },
                // Stack enumera dalla cima al fondo: invertiamo per salvare dal fondo
                inventario = player.inventario.Select(o => o.nome).Reverse().ToList()
            };

            foreach (Stanza stanza in TutteLeStanze())
            {
                foreach (Porta? porta in PorteDella(stanza))
                {
                    if (porta != null) dati.porte[porta.nome] = porta.stato.ToString();
                }

                if (stanza.personaggio != null)
                    dati.personaggiVivi[stanza.nome] = stanza.personaggio.vivo;

                List<string> mobili = new();
                foreach (Oggetto obj in stanza.lista)
                {
                    if (obj is Terminale term)
                    {
                        dati.terminali[term.nome] = term.stato.ToString();
                    }
                    else if (obj is Cassa cassa)
                    {
                        dati.casse[cassa.nome] = cassa.stato.ToString();
                        dati.casseConContenuto[cassa.nome] = cassa.contenuto != null;
                    }
                    else if (obj.mobile)
                    {
                        mobili.Add(obj.nome);
                    }
                }
                if (mobili.Count > 0) dati.oggettiNelleStanze[stanza.nome] = mobili;
            }

            string json = JsonSerializer.Serialize(dati, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileSalvataggio, json);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nErrore durante il salvataggio: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Carica la partita dal file: ricostruisce il mondo con Global.Inizializza
    /// e poi riapplica gli stati salvati. Ritorna false se il file manca o è corrotto.
    /// </summary>
    public static bool Carica(Giocatore player)
    {
        DatiSalvataggio? dati;
        try
        {
            dati = JsonSerializer.Deserialize<DatiSalvataggio>(File.ReadAllText(FileSalvataggio));
            if (dati == null) return false;
        }
        catch
        {
            return false;
        }

        // 1. Mondo pulito (imposta anche inventario/posizione di partenza, che sovrascriviamo)
        Global.Inizializza(player);

        // 2. Cataloghiamo per nome tutto ciò che esiste nel mondo appena creato
        Dictionary<string, Stanza> stanzePerNome = new();
        Dictionary<string, Porta> portePerNome = new();
        Dictionary<string, Terminale> terminaliPerNome = new();
        Dictionary<string, Cassa> cassePerNome = new();
        Dictionary<string, Oggetto> oggettiMobili = new();

        // Gli oggetti di partenza nell'inventario entrano nel catalogo,
        // poi la pila viene svuotata e ripopolata dai dati salvati
        foreach (Oggetto obj in player.inventario) oggettiMobili[obj.nome] = obj;
        player.inventario.Clear();

        foreach (Stanza stanza in TutteLeStanze())
        {
            stanzePerNome[stanza.nome] = stanza;

            foreach (Porta? porta in PorteDella(stanza))
            {
                if (porta != null) portePerNome[porta.nome] = porta;
            }

            // Gli oggetti mobili vengono tolti dalle stanze: li ripiazzeremo
            // dove dice il salvataggio (stanza o inventario)
            for (int i = stanza.lista.Count - 1; i >= 0; i--)
            {
                Oggetto obj = stanza.lista[i];
                if (obj is Terminale term)
                {
                    terminaliPerNome[term.nome] = term;
                }
                else if (obj is Cassa cassa)
                {
                    cassePerNome[cassa.nome] = cassa;
                    if (cassa.contenuto != null) oggettiMobili[cassa.contenuto.nome] = cassa.contenuto;
                }
                else if (obj.mobile)
                {
                    oggettiMobili[obj.nome] = obj;
                    stanza.lista.RemoveAt(i);
                }
            }
        }

        // 3. Riapplichiamo gli stati salvati
        foreach (var (nome, stato) in dati.porte)
        {
            if (portePerNome.TryGetValue(nome, out Porta? porta) && Enum.TryParse(stato, out Porta.StatoPorta s))
                porta.CambiaStato(s);
        }
        foreach (var (nome, stato) in dati.terminali)
        {
            if (terminaliPerNome.TryGetValue(nome, out Terminale? term) && Enum.TryParse(stato, out StatoTerminale s))
                term.stato = s;
        }
        foreach (var (nome, stato) in dati.casse)
        {
            if (cassePerNome.TryGetValue(nome, out Cassa? cassa) && Enum.TryParse(stato, out Cassa.StatoCassa s))
                cassa.stato = s;
        }
        foreach (var (nome, haContenuto) in dati.casseConContenuto)
        {
            if (!haContenuto && cassePerNome.TryGetValue(nome, out Cassa? cassa))
                cassa.contenuto = null;
        }

        // 4. Rimettiamo gli oggetti mobili dove erano stati lasciati
        foreach (var (nomeStanza, nomi) in dati.oggettiNelleStanze)
        {
            if (!stanzePerNome.TryGetValue(nomeStanza, out Stanza? stanza)) continue;
            foreach (string nomeObj in nomi)
            {
                if (oggettiMobili.TryGetValue(nomeObj, out Oggetto? obj)) stanza.lista.Add(obj);
            }
        }
        foreach (string nomeObj in dati.inventario)
        {
            if (oggettiMobili.TryGetValue(nomeObj, out Oggetto? obj)) player.inventario.Push(obj);
        }

        // 5. Stato del giocatore e della navetta
        player.nome = dati.nome;
        player.vita = dati.vita;
        player.contatorePassi = dati.contatorePassi;
        Global.componentiNavettaInstallati = dati.componentiNavetta;
        EventoIA.CaricaStato(dati.eventoStanza, dati.eventoAttivo, dati.eventoRisolto, dati.eventoSecondiRimasti);

        foreach (var (nomeStanza, vivo) in dati.personaggiVivi)
        {
            if (stanzePerNome.TryGetValue(nomeStanza, out Stanza? stanza) && stanza.personaggio != null)
                stanza.personaggio.vivo = vivo;
        }

        int r = dati.coordinate[0], c = dati.coordinate[1];
        if (r >= 0 && r < Global.map.Length && c >= 0 && c < Global.map[r].Length && Global.map[r][c] != null)
        {
            player.coordinate = new[] { r, c };
            player.stanza = Global.map[r][c];
        }

        return true;
    }

    private static IEnumerable<Stanza> TutteLeStanze()
    {
        foreach (Stanza[] riga in Global.map)
            foreach (Stanza? stanza in riga)
                if (stanza != null) yield return stanza;
    }

    private static IEnumerable<Porta?> PorteDella(Stanza s)
    {
        yield return s.portaNord;
        yield return s.portaSud;
        yield return s.portaEst;
        yield return s.portaOvest;
    }
}
