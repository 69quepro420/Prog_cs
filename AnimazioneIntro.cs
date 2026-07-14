using System;
using System.Threading;

namespace Project;

class AnimazioneIntro
{
    /// <summary>
    /// Avvia l'animazione introduttiva della navicella spaziale.
    /// </summary>
    public static void Gioca()
    {
        Console.CursorVisible = false;
        Console.Clear();

        string frameSu = @"
        /_\
        /   \
        /_____\
        |  O  |
        /|     |\
        / |_____| \
        /   \
        /_____\
        | | |
        - - -
        ";

        string frameGiu = @"

        /_\
        /   \
        /_____\
        |  O  |
        /|     |\
        / |_____| \
        /   \
        /_____\
        | | |
        - - -
        ";

        string[] testiFiller = {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit...",
            "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua...",
            "Ut enim ad minim veniam, quis nostrud exercitation ullamco...",
            "Duis aute irure dolor in reprehenderit in voluptate velit esse...",
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa..."
        };

        int fps = 12; // Animazione un po' più lenta per la navicella
        int durataSecondi = 10; // 2 secondi per ogni frase (5 frasi)
        int frameTotali = fps * durataSecondi;
        int delayMs = 1000 / fps;

        for (int i = 0; i < frameTotali; i++)
        {
            Console.SetCursorPosition(0, 0);

            if ((i / 4) % 2 == 0)
                Console.WriteLine(frameSu);
            else
                Console.WriteLine(frameGiu);

            Console.WriteLine("\n===========================================================");

            // Cambia testo ogni 2 secondi (fps * 2 frame)
            int indiceTesto = (i / (fps * 2)) % testiFiller.Length;
            // PadRight assicura che le stringhe più corte non lascino residui di quelle più lunghe
            Console.WriteLine($" {testiFiller[indiceTesto].PadRight(60)}");
            Console.WriteLine("===========================================================");

            Thread.Sleep(delayMs);
        }

        Console.CursorVisible = true;
        Console.Clear();
    }
}

