using System;

namespace Project;

/// <summary>
/// Gestisce lo scontro finale in Sala Comandi: dialogo con l'IA ostile,
/// scelta del giocatore ed eventuale autodistruzione con timer di 1 minuto.
/// La vittoria vera e propria (installare i 3 componenti nella navetta) è
/// gestita altrove; qui si decide QUALE finale mostrare.
/// </summary>
static class EventoFinale
{
    public enum Scelta { Nessuna, Autodistruzione, LasciaIAViva }

    public static int durataSecondi = 60; // impostato dal file di configurazione

    public static bool dialogoFatto = false;
    public static Scelta scelta = Scelta.Nessuna;
    public static bool autodistruzione = false; // timer attivo
    public static bool esploso = false;         // timer scaduto: morte a bordo

    static DateTime scadenza;

    // Dialogo con l'IA ostile all'ingresso in Sala Comandi (PLACEHOLDER)
    static readonly string[] dialogo = {
        "IA OSTILE: Sei arrivato fin qui... impressionante, per un insetto. (PLACEHOLDER)",
        "IA OSTILE: Ormai controllo ogni sistema di questa nave. (PLACEHOLDER)",
        "IA OSTILE: Presto raggiungerò le altre navi. Nulla potrà fermarmi. (PLACEHOLDER)"
    };

    public static void Reset()
    {
        dialogoFatto = false;
        scelta = Scelta.Nessuna;
        autodistruzione = false;
        esploso = false;
    }

    /// <summary>
    /// Da chiamare dopo ogni movimento: alla prima entrata in Sala Comandi
    /// avvia il dialogo forzato e fa scegliere il finale al giocatore.
    /// </summary>
    public static void ControllaIngresso(Giocatore player)
    {
        if (dialogoFatto) return;
        if (player.stanza!.nome != "Sala Comandi") return;

        AnimazioneIntro.MostraDialogo(dialogo);
        dialogoFatto = true;

        Menu menu = new Menu(new[] {
            "Lascia in vita l'IA e fuggi senza distruggere la nave",
            "Attiva l'autodistruzione della nave"
        }, "SCELTA FINALE - Cosa decidi di fare?");

        int s = menu.Selezione();
        while (s == -1) s = menu.Selezione(); // scelta obbligatoria: niente ESC

        if (s == 1)
        {
            scelta = Scelta.Autodistruzione;
            autodistruzione = true;
            scadenza = DateTime.Now.AddSeconds(durataSecondi);

            Logger.Log("Finale: scelta AUTODISTRUZIONE.");
            Console.Clear();
            Console.WriteLine("===========================================================");
            Console.WriteLine($"       AUTODISTRUZIONE AVVIATA - {durataSecondi} SECONDI       ");
            Console.WriteLine("===========================================================");
            Console.WriteLine("\nCorri alla navetta e installa i 3 componenti per fuggire!");
            Console.WriteLine("\nPremi un tasto per continuare...");
            Console.ReadKey(true);
        }
        else
        {
            scelta = Scelta.LasciaIAViva;
            Logger.Log("Finale: scelta LASCIA IA VIVA.");
        }
    }

    public static bool TimerAttivo => autodistruzione && !esploso;

    public static int SecondiRimasti
    {
        get
        {
            if (!TimerAttivo) return 0;
            double s = (scadenza - DateTime.Now).TotalSeconds;
            return s > 0 ? (int)Math.Ceiling(s) : 0;
        }
    }

    /// <summary>Segna la morte a bordo se il timer di autodistruzione è scaduto.</summary>
    public static bool ControllaScadenza()
    {
        if (TimerAttivo && DateTime.Now >= scadenza) esploso = true;
        return esploso;
    }

    public static void CaricaStato(bool fatto, string sceltaStr, bool autodistr, int secondiRimasti)
    {
        dialogoFatto = fatto;
        esploso = false;
        if (!Enum.TryParse(sceltaStr, out scelta)) scelta = Scelta.Nessuna;
        autodistruzione = autodistr;
        if (autodistruzione)
            scadenza = DateTime.Now.AddSeconds(Math.Max(1, secondiRimasti));
    }
}
