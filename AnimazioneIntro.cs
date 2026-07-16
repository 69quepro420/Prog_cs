using System;
using System.Collections.Generic;
using System.Threading;

namespace Project;

class AnimazioneIntro
{
    static readonly string[] segmenti = {
        "Anno 2097  -  25 dicembre  -  ore 5:30 a.m.",

        "Un altro Natale, l'ennesimo che passi nella solitudine dello spazio. " +
        "Sarebbe stato il primo Natale da passare insieme alla tua famiglia, se " +
        "solo non avessi beccato una pioggia di asteroidi che ti ha messo fuori " +
        "uso due dei tre motori della navicella.",

        "Con un solo motore inizi a pensare che la tua velocità assomigli a quella " +
        "delle vecchie autovetture che i tuoi nonni terrestri usavano fino a 50 " +
        "anni fa. Ancora 20 giorni per raggiungere il wormhole verso Marte: " +
        "pensare che dovevi arrivarci già 5 giorni fa.",

        "Almeno viaggiare così lentamente ti fa godere la meraviglia dello spazio, " +
        "di pianeti mai visti, di una navicella motherboard... aspetta, una motherboard?",

        "Due turbopompe e un ugello: solo di questo hai bisogno. Forse anche di " +
        "qualche Milky Way bar (potevi non mangiarle tutte in una settimana!).",

        "Forse non tutte le speranze sono perdute. Mentre ti avvicini al porto " +
        "d'attracco, un oggetto sbatte contro la vetrata principale. UN CADAVERE!",

        "Noti che ci sono diversi corpi fluttuanti attorno al porto ma, anche se " +
        "impaurito, la mancanza di casa è più forte di qualsiasi minaccia.",

        "Attracchi, slacci le cinture, indossi la tuta, prendi il tuo spacepack " +
        "(bella l'idea dello zaino fluttuante, se non fosse che è strettissimo) e " +
        "ti prepari: pronto contro ogni pericolo!"
    };

    /// <summary>
    /// Mostra l'introduzione una schermata alla volta. Ogni schermata resta
    /// finché il giocatore non preme un tasto per continuare.
    /// </summary>
    public static void Gioca()
    {
        Console.CursorVisible = false;

        foreach (string segmento in segmenti)
        {
            Console.Clear();
            Console.WriteLine("\n");
            Console.WriteLine("  ===========================================================");
            Console.WriteLine();
            foreach (string riga in MandaACapo(segmento, 54))
                Console.WriteLine("    " + riga);
            Console.WriteLine();
            Console.WriteLine("  ===========================================================");
            Console.WriteLine("\n  (Premi un tasto per continuare...)");

            Console.ReadKey(true);
        }

        Console.CursorVisible = true;
        Console.Clear();
    }

    /// <summary>
    /// Mostra un dialogo forzato: una frase alla volta, un tasto (o il timeout)
    /// fa avanzare alla successiva.
    /// </summary>
    public static void MostraDialogo(string[] frasi)
    {
        Console.CursorVisible = false;

        foreach (string frase in frasi)
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            Console.WriteLine("===========================================================");
            Console.WriteLine($" {frase}");
            Console.WriteLine("===========================================================");
            Console.WriteLine("\n(Premi un tasto per continuare)");

            AspettaOTastoPremuto(4000);
        }

        Console.CursorVisible = true;
        Console.Clear();
    }

    /// <summary>
    /// Spezza un testo su più righe senza tagliare le parole, entro la larghezza indicata.
    /// </summary>
    private static List<string> MandaACapo(string testo, int larghezza)
    {
        List<string> righe = new();
        string corrente = "";

        foreach (string parola in testo.Split(' '))
        {
            if (corrente.Length == 0)
                corrente = parola;
            else if (corrente.Length + 1 + parola.Length <= larghezza)
                corrente += " " + parola;
            else
            {
                righe.Add(corrente);
                corrente = parola;
            }
        }

        if (corrente.Length > 0) righe.Add(corrente);
        return righe;
    }

    /// <summary>
    /// Attende i millisecondi indicati controllando l'input a piccoli intervalli.
    /// Ritorna true se l'utente ha premuto un tasto.
    /// </summary>
    private static bool AspettaOTastoPremuto(int millisecondi)
    {
        int trascorsi = 0;
        while (trascorsi < millisecondi)
        {
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true);
                return true;
            }
            Thread.Sleep(50);
            trascorsi += 50;
        }
        return false;
    }
}
