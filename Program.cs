using System;

namespace Project;

class Program
{
    static void Main(string[] args)
    {
        // 1. Introduzione testuale (saltabile con un tasto)
        AnimazioneIntro.Gioca();

        // 2. Menu principale
        while (true)
        {
            Menu menuPrincipale = new Menu(
                new[] { "Nuova Partita", "Carica Partita", "Esci" },
                "MENU PRINCIPALE");

            int scelta = menuPrincipale.Selezione();

            if (scelta == 0) // Nuova Partita
            {
                Giocatore player = new Giocatore(ChiediNome());
                Global.Inizializza(player);
                Comando.Start(player);
            }
            else if (scelta == 1) // Carica Partita
            {
                if (!Salvataggio.Esiste())
                {
                    Mostra("Nessun salvataggio trovato.");
                    continue;
                }

                Giocatore player = new Giocatore("Comandante");
                if (Salvataggio.Carica(player))
                {
                    Comando.Start(player);
                }
                else
                {
                    Mostra("Il file di salvataggio è corrotto o illeggibile.");
                }
            }
            else // Esci (o ESC)
            {
                Console.Clear();
                return;
            }
        }
    }

    /// <summary>
    /// Chiede il nome del giocatore. Vuoto = "Comandante";
    /// massimo 20 caratteri per non rompere l'allineamento dell'HUD.
    /// </summary>
    private static string ChiediNome()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== NUOVA PARTITA ===\n");
            Console.Write("Inserisci il tuo nome (max 20 caratteri, invio per 'Comandante'): ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) return "Comandante";
            if (input.Length <= 20) return input;

            Mostra("Nome troppo lungo, massimo 20 caratteri.");
        }
    }

    private static void Mostra(string messaggio)
    {
        Console.Clear();
        Console.WriteLine($"\n{messaggio}");
        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey(true);
    }
}
