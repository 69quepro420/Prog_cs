using System;
using System.Collections.Generic;

namespace Project;

class Oggetto
{
    public string nome { get; set; }
    public string descr { get; set; }
    public float peso { get; set; }
    public bool mobile { get; set; }

    // --- 1. COSTRUTTORE VUOTO (Risolve l'errore del Terminale CS7036) ---
    public Oggetto() { }

    // --- 2. COSTRUTTORE CON PARAMETRI (Risolve l'errore dell'oggetto di test precedente) ---
    public Oggetto(string nome, string descr, float peso, bool mobile)
    {
        this.nome = nome;
        this.descr = descr;
        this.peso = peso;
        this.mobile = mobile;
    }

    /// <summary>
    /// Metodo base per l'interazione. Viene sovrascritto (override) dal Terminale.
    /// </summary>
    public virtual void Usa(Giocatore player)
    {
        Console.WriteLine("Non c'è niente di speciale da fare con questo oggetto.");
    }
}

/// <summary>
/// Cassa: contenitore che può essere bloccato, sbloccato o saccheggiato.
/// Quando viene aperta in stato Sbloccata rilascia il contenuto nella stanza.
/// </summary>
class Cassa : Oggetto
{
    public enum StatoCassa { Bloccata, Sbloccata, Saccheggiata }
    public StatoCassa stato { get; set; }
    public Oggetto? contenuto { get; set; }

    public Cassa() { }

    public Cassa(string nome, string descr, Oggetto? contenuto, StatoCassa stato = StatoCassa.Bloccata)
    {
        this.nome = nome;
        this.descr = descr;
        this.peso = 5f; // valore di default
        this.mobile = false; // la cassa è fissa
        this.contenuto = contenuto;
        this.stato = stato;
    }

    /// <summary>
    /// Interazione diretta con la cassa.
    /// - Bloccata: mostra messaggio.
    /// - Sbloccata: rilascia il contenuto nella stanza e diventa Saccheggiata.
    /// - Saccheggiata: informa che è già stata svuotata.
    /// </summary>
    public override void Usa(Giocatore player)
    {
        Console.Clear();
        if (stato == StatoCassa.Bloccata)
        {
            Console.WriteLine($"{nome} è bloccata da un sistema elettronico. Non puoi aprirla manualmente.");
        }
        else if (stato == StatoCassa.Sbloccata)
        {
            if (contenuto != null)
            {
                Console.WriteLine($"Apri {nome}. Trovi {contenuto.nome} all'interno. (Viene posizionato nella stanza)");
                // Aggiungiamo il contenuto alla stanza corrente
                player.stanza!.lista.Add(contenuto);
                // svuotiamo la cassa
                contenuto = null;
            }
            else
            {
                Console.WriteLine($"Apri {nome}. Non c'è niente all'interno.");
            }
            stato = StatoCassa.Saccheggiata;
        }
        else // Saccheggiata
        {
            Console.WriteLine($"{nome} è già stata saccheggiata.");
        }

        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Metodo per sbloccare la cassa (utilizzabile dal Terminale o da chiavi).
    /// </summary>
    public void Sblocca()
    {
        if (stato == StatoCassa.Bloccata)
            stato = StatoCassa.Sbloccata;
    }
}

/// <summary>
/// Strumento: oggetto che, usato nella stanza giusta, apre una porta.
/// Usato altrove non produce alcun effetto. Non si consuma con l'uso.
/// </summary>
class Strumento : Oggetto
{
    public string stanzaUso;      // nome della stanza in cui lo strumento funziona
    public Porta porta;           // porta che viene aperta
    public string messaggioUso;   // messaggio mostrato quando l'uso riesce

    public Strumento(string nome, string descr, float peso, string stanzaUso, Porta porta, string messaggioUso)
    {
        this.nome = nome;
        this.descr = descr;
        this.peso = peso;
        this.mobile = true;
        this.stanzaUso = stanzaUso;
        this.porta = porta;
        this.messaggioUso = messaggioUso;
    }

    public override void Usa(Giocatore player)
    {
        if (player.stanza!.nome != stanzaUso)
        {
            Console.WriteLine($"\nQui {nome} non serve a niente.");
        }
        else if (porta.stato == Porta.StatoPorta.Aperta)
        {
            Console.WriteLine($"\nLa porta '{porta.nome}' è già aperta.");
        }
        else
        {
            porta.CambiaStato(Porta.StatoPorta.Aperta);
            Console.WriteLine($"\n{messaggioUso}");
            Console.WriteLine($"La porta '{porta.nome}' ora è aperta!");
        }
    }
}

/// <summary>
/// IA Tascabile: intelligenza artificiale amichevole portatile.
/// Se è nell'inventario si può parlarle col tasto [T]; quando l'evento
/// dell'IA ostile è attivo rivela (sotto forma di indovinello) la password
/// del terminale della Sala Ossigeno.
/// </summary>
class IATascabile : Oggetto
{
    // Porta che l'IA è in grado di sbloccare e stanza in cui può farlo
    public Porta? portaCollegata;
    public string stanzaSblocco = "";

    public IATascabile(Porta? portaCollegata = null, string stanzaSblocco = "")
    {
        this.nome = "IA Tascabile";
        this.descr = "Un piccolo dispositivo con un occhio luminoso. Sembra amichevole.";
        this.peso = 0.5f;
        this.mobile = true;
        this.portaCollegata = portaCollegata;
        this.stanzaSblocco = stanzaSblocco;
    }

    public override void Usa(Giocatore player)
    {
        // Se siamo nella stanza giusta e la porta collegata è ancora chiusa,
        // l'IA la sblocca; in tutti gli altri casi si limita a parlare
        if (portaCollegata != null
            && player.stanza!.nome == stanzaSblocco
            && portaCollegata.stato != Porta.StatoPorta.Aperta)
        {
            Console.Clear();
            Console.WriteLine("--- IA TASCABILE ---\n");
            Console.WriteLine("\"*bzzt* Serratura elettronica rilevata. Lascia fare a me...\"");
            portaCollegata.CambiaStato(Porta.StatoPorta.Aperta);
            Console.WriteLine($"\nLa porta '{portaCollegata.nome}' ora è aperta!");
            Console.WriteLine("\nPremi un tasto per continuare...");
            Console.ReadKey(true);
            return;
        }

        Parla(player);
    }

    public void Parla(Giocatore player)
    {
        Console.Clear();
        Console.WriteLine("--- IA TASCABILE ---\n");

        if (!EventoIA.attivo)
        {
            Console.WriteLine("\"Sistemi in standby. Nessuna minaccia rilevata... per ora.\"");
        }
        else if (EventoIA.risolto)
        {
            Console.WriteLine("\"Ottimo lavoro! I livelli di ossigeno sono di nuovo stabili.\"");
        }
        else
        {
            // Indovinello che rivela la password del Terminale di Sala Ossigeno (PLACEHOLDER)
            Console.WriteLine("\"*bzzt* Presto! Il terminale della Sala Ossigeno può fermare tutto!\"");
            Console.WriteLine();
            Console.WriteLine("\"La password? Te la dico a modo mio... (PLACEHOLDER INDOVINELLO)\"");
            Console.WriteLine("\"Sali quattro gradini, uno alla volta, partendo dal primo.\"");
        }

        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey(true);
    }
}

/// <summary>
/// Oggetto chiave: componente di ricambio della navetta.
/// Usato dentro la Navetta viene installato (sparisce dall'inventario);
/// quando tutti i componenti sono installati il giocatore vince.
/// </summary>
class OggettoChiave : Oggetto
{
    public bool installato = false;

    public OggettoChiave(string nome, string descr, float peso)
    {
        this.nome = nome;
        this.descr = descr;
        this.peso = peso;
        this.mobile = true;
    }

    public override void Usa(Giocatore player)
    {
        if (player.stanza!.nome != "Navetta")
        {
            Console.WriteLine($"\n{nome} è un componente della navetta: va installato lì.");
            return;
        }

        installato = true;
        Global.componentiNavettaInstallati++;
        Console.WriteLine($"\nInstalli {nome} nella navetta. " +
            $"({Global.componentiNavettaInstallati}/{Global.componentiNavettaTotali} componenti)");

        if (Global.componentiNavettaInstallati >= Global.componentiNavettaTotali)
        {
            Global.partitaVinta = true;
        }
        else
        {
            Console.WriteLine("La navetta ha ancora bisogno di altri componenti...");
        }
    }
}
