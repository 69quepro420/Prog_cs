using System;
using System.Threading;

namespace Project;

class AnimazioneIntro
{
    // Testi mostrati in sequenza durante l'introduzione
    static readonly string[] testi = {
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit...",
        "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua...",
        "Ut enim ad minim veniam, quis nostrud exercitation ullamco...",
        "Duis aute irure dolor in reprehenderit in voluptate velit esse...",
        "Excepteur sint occaecat cupidatat non proident, sunt in culpa..."
    };

    const int millisecondiPerFrase = 2500;

    /// <summary>
    /// Mostra l'introduzione testuale: una frase alla volta, che cambia
    /// automaticamente. Un tasto qualsiasi salta il resto dell'introduzione.
    /// </summary>
    public static void Gioca()
    {
        Console.CursorVisible = false;

        foreach (string testo in testi)
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            Console.WriteLine("===========================================================");
            Console.WriteLine($" {testo}");
            Console.WriteLine("===========================================================");
            Console.WriteLine("\n(Premi un tasto per saltare l'introduzione)");

            if (AspettaOTastoPremuto(millisecondiPerFrase)) break;
        }

        Console.CursorVisible = true;
        Console.Clear();
    }

    /// <summary>
    /// Mostra un dialogo forzato riusando lo stesso sistema dell'intro:
    /// una frase alla volta, un tasto (o il timeout) fa avanzare alla successiva.
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
    /// Attende i millisecondi indicati controllando l'input a piccoli intervalli,
    /// così non blocca la CPU con ridisegni continui.
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
