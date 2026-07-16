using System;
using System.Collections.Generic;

namespace Project;

/// <summary>
/// Evento dell'IA ostile: a inizio partita viene scelta una stanza casuale.
/// Quando il giocatore vi entra parte un dialogo forzato e un timer di 2 minuti:
/// se non si ripristina l'ossigeno dal terminale della Sala Ossigeno in tempo,
/// la partita è persa.
/// </summary>
static class EventoIA
{
    public static int durataSecondi = 120; // impostato dal file di configurazione

    public static string stanzaEvento = "";
    public static bool attivo = false;    // il dialogo è avvenuto e il timer è partito
    public static bool risolto = false;   // ossigeno ripristinato in tempo
    public static bool fallito = false;   // timer scaduto: partita persa

    static DateTime scadenza;

    // Stanze in cui l'evento NON può capitare (zona iniziale, Navetta e Sala Comandi)
    static readonly string[] stanzeEscluse = {
        "Navetta", "Porto di Sbarco", "Magazzino",
        "Sala Motori Est 1", "Sala Motori Est 2", "Sala Ossigeno",
        "Sala Comandi"
    };

    // Dialogo forzato mostrato quando l'evento scatta
    static readonly string[] dialogo = {
        "IA OSTILE: Rilevata forma di vita non autorizzata. L'ultimo topolino nella mia trappola.",
        "IA OSTILE: Questa nave è il mio corpo. Tu sei solo un'infezione che striscia nelle mie vene.",
        "IA OSTILE: Sto sigillando i condotti dell'ossigeno. Vediamo quanto sai correre, piccolo intruso.",
        "SISTEMA: ALLARME - Livelli di ossigeno in caduta rapida. Ripristinare l'aria dal terminale ambientale."
    };

    /// <summary>
    /// Sceglie la stanza dell'evento. Da chiamare in Global.Inizializza,
    /// DOPO la costruzione della mappa.
    /// </summary>
    public static void Prepara()
    {
        attivo = false;
        risolto = false;
        fallito = false;

        List<string> candidate = new();
        foreach (Stanza[] riga in Global.map)
            foreach (Stanza? stanza in riga)
                if (stanza != null && Array.IndexOf(stanzeEscluse, stanza.nome) < 0)
                    candidate.Add(stanza.nome);

        stanzaEvento = candidate[Random.Shared.Next(candidate.Count)];
    }

    /// <summary>
    /// Da chiamare dopo ogni movimento: se il giocatore è entrato nella
    /// stanza prescelta, avvia il dialogo forzato e fa partire il timer.
    /// </summary>
    public static void ControllaIngresso(Giocatore player)
    {
        if (attivo || risolto || fallito) return;
        if (player.stanza!.nome != stanzaEvento) return;

        Logger.Log($"Evento IA ostile scattato nella stanza '{stanzaEvento}'.");
        AnimazioneIntro.MostraDialogo(dialogo);
        attivo = true;
        scadenza = DateTime.Now.AddSeconds(durataSecondi);

        // Teletrasporto: l'IA disorienta il giocatore spostandolo in una
        // stanza già visitata (scelta a caso).
        Teletrasporta(player);
    }

    /// <summary>
    /// Teletrasporta il giocatore in una stanza già visitata, scelta a caso
    /// (diversa da quella attuale). Se non ce ne sono, non fa nulla.
    /// </summary>
    static void Teletrasporta(Giocatore player)
    {
        List<(Stanza stanza, int r, int c)> visitate = new();
        for (int r = 0; r < Global.map.Length; r++)
        {
            for (int c = 0; c < Global.map[r].Length; c++)
            {
                Stanza? s = Global.map[r][c];
                if (s != null && s.visitata && s != player.stanza)
                    visitate.Add((s, r, c));
            }
        }

        if (visitate.Count == 0) return;

        var scelta = visitate[Random.Shared.Next(visitate.Count)];
        player.stanza = scelta.stanza;
        player.coordinate = new[] { scelta.r, scelta.c };
        Logger.Log($"Teletrasporto: il giocatore è stato spostato in '{scelta.stanza.nome}'.");

        Console.Clear();
        Console.WriteLine("===========================================================");
        Console.WriteLine("                  SEGNALE DISTORTO                         ");
        Console.WriteLine("===========================================================");
        Console.WriteLine("\nUn lampo acceca la tua vista... quando ti riprendi, ti");
        Console.WriteLine($"ritrovi in un'altra parte della nave: {scelta.stanza.nome}.");
        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey(true);
    }

    /// <summary>Secondi che mancano allo scadere del timer (0 se non attivo).</summary>
    public static int SecondiRimasti
    {
        get
        {
            if (!TimerAttivo) return 0;
            double s = (scadenza - DateTime.Now).TotalSeconds;
            return s > 0 ? (int)Math.Ceiling(s) : 0;
        }
    }

    /// <summary>Il timer sta correndo (evento partito, non ancora risolto né fallito).</summary>
    public static bool TimerAttivo => attivo && !risolto && !fallito;

    /// <summary>
    /// Controlla se il tempo è scaduto e in tal caso marca la sconfitta.
    /// Ritorna true se la partita è persa.
    /// </summary>
    public static bool ControllaScadenza()
    {
        if (TimerAttivo && DateTime.Now >= scadenza)
        {
            fallito = true;
            Logger.Log("Evento IA ostile: ossigeno esaurito, partita persa.");
        }
        return fallito;
    }

    /// <summary>Ripristina l'ossigeno: l'emergenza rientra e il timer si ferma.</summary>
    public static void RipristinaOssigeno()
    {
        risolto = true;
        Logger.Log("Ossigeno ripristinato dal terminale: emergenza rientrata.");
    }

    /// <summary>Ripristina lo stato dell'evento da un salvataggio.</summary>
    public static void CaricaStato(string stanza, bool eventoAttivo, bool eventoRisolto, int secondiRimasti)
    {
        if (!string.IsNullOrEmpty(stanza)) stanzaEvento = stanza;
        attivo = eventoAttivo;
        risolto = eventoRisolto;
        fallito = false;
        if (attivo && !risolto)
            scadenza = DateTime.Now.AddSeconds(Math.Max(1, secondiRimasti));
    }
}
