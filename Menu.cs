using System;

namespace Project;

class Menu
{
    string[] opzioni;
    int selezione = 0;
    string titolo;

    /// <summary>
    /// Costruttore del menù a freccette.
    /// </summary>
    public Menu(string[] opzioni, string titolo = "")
    {
        this.opzioni = opzioni;
        this.titolo = titolo;
    }

    /// <summary>
    /// Mostra il menu e permette la navigazione con le frecce.
    /// Supporta l'uscita con il tasto ESC.
    /// </summary>
    public int Selezione()
    {
        ConsoleKey tasto;
        do
        {
            Console.Clear();
            if (!string.IsNullOrEmpty(titolo))
                Console.WriteLine($"=== {titolo} ===\n");

            for (int i = 0; i < opzioni.Length; i++)
            {
                if (i == selezione)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($">> {opzioni[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {opzioni[i]}");
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            tasto = keyInfo.Key;

            if (tasto == ConsoleKey.UpArrow)
            {
                selezione--;
                if (selezione < 0) selezione = opzioni.Length - 1;
            }
            else if (tasto == ConsoleKey.DownArrow)
            {
                selezione++;
                if (selezione > opzioni.Length - 1) selezione = 0;
            }
            else if (tasto == ConsoleKey.Escape)
            {
                return -1; // -1 indica l'annullamento
            }

        } while (tasto != ConsoleKey.Enter);

        return selezione;
    }
}
