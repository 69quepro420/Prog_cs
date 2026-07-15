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
    public const int durataSecondi = 120;

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

    // Dialogo forzato mostrato quando l'evento scatta (PLACEHOLDER)
    static readonly string[] dialogo = {
        "??? : Rilevata forma di vita non autorizzata. (PLACEHOLDER)",
        "IA OSTILE: Questa stazione è sotto il MIO controllo. (PLACEHOLDER)",
        "IA OSTILE: Sto sigillando i condotti dell'ossigeno. Ti restano 2 minuti. (PLACEHOLDER)",
        "SISTEMA: ALLARME! Livelli di ossigeno in caduta. Ripristinare dal terminale ambientale."
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

        AnimazioneIntro.MostraDialogo(dialogo);
        attivo = true;
        scadenza = DateTime.Now.AddSeconds(durataSecondi);
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
        if (TimerAttivo && DateTime.Now >= scadenza) fallito = true;
        return fallito;
    }

    /// <summary>Ripristina l'ossigeno: l'emergenza rientra e il timer si ferma.</summary>
    public static void RipristinaOssigeno()
    {
        risolto = true;
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
