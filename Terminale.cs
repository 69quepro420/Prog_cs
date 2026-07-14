using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Project;

public enum StatoTerminale { Sbloccato, Bloccato, Criptato }

class Terminale : Oggetto
{
    public StatoTerminale stato;
    public string password;
    public List<string> logs;

    // Lista di nomi delle porte che questo terminale può controllare
    public List<string> porteControllate { get; set; } = new List<string>();

    public string asciiArt = @"
    d8'                    MP""""""`MM                                        MMP""""""YMM MP""""""`MM
    d8'                     M  mmmmm..M                                        M' .mmm. `M M  mmmmm..M
    d8'                      M.      `YM 88d888b. .d8888b. .d8888b. .d8888b.    M  MMMMM  M M.      `YM
    d8'                       MMMMMMM.  M 88'  `88 88'  `88 88'  `"""" 88ooood8    M  MMMMM  M MMMMMMM.  M
    d8'                        M. .MMM'  M 88.  .88 88.  .88 88.  ... 88.  ...    M. `MMM' .M M. .MMM'  M
    88                          Mb.     .dM 88Y888P' `88888P8 `88888P' `88888P'    MMb     dMM Mb.     .dM
    oooooooooooo       MMMMMMMMMMM 88                                     MMMMMMMMMMM MMMMMMMMMMM
    dP                                                                 ";

    // Nuovo costruttore che può ricevere la lista delle porte controllate
    public Terminale(string nome, string descr, StatoTerminale stato, string password, List<string>? porteControllate = null)
    {
        this.nome = nome;
        this.descr = descr;
        this.peso = 0f;
        this.mobile = false; // L'oggetto non può essere messo nell'inventario
        this.stato = stato;
        this.password = password;
        this.logs = new List<string>();
        if (porteControllate != null) this.porteControllate = porteControllate;
    }

    public override void Usa(Giocatore player)
    {
        Console.Clear();
        if (stato == StatoTerminale.Bloccato) RichiediPassword(player);
        else if (stato == StatoTerminale.Criptato) AvviaMinigioco(player);
        else SistemaOperativo(player);
    }

    private void RichiediPassword(Giocatore player)
    {
        Console.WriteLine("======================================");
        Console.WriteLine("TERMINALE BLOCCATO. RICHIESTA PASSWORD.");
        Console.WriteLine("======================================");
        Console.Write("> ");
        string? input = Console.ReadLine();

        if (input == password)
        {
            Console.WriteLine("\nPASSWORD ACCETTATA. SBLOCCO IN CORSO...");
            Thread.Sleep(1500);
            this.stato = StatoTerminale.Sbloccato;
            SistemaOperativo(player);
        }
        else
        {
            Console.WriteLine("\nPASSWORD ERRATA. ACCESSO NEGATO.");
            Console.WriteLine("Premi un tasto per disconnetterti...");
            Console.ReadKey(true);
        }
    }

    private void AvviaMinigioco(Giocatore player)
    {
        string[] target = { "1C", "BD", "55" };
        string[,] matrix = {
            { "55", "1C", "FF", "BD" },
            { "FF", "55", "1C", "E9" },
            { "1C", "BD", "55", "FF" },
            { "BD", "E9", "FF", "1C" }
        };

        List<string> buffer = new List<string>();
        bool turnoRiga = true;
        int bloccoIndice = 0;
        Stopwatch sw = new Stopwatch();
        sw.Start();

        while (true)
        {
            Console.Clear();
            int tempoRimasto = 30 - (int)sw.Elapsed.TotalSeconds;
            if (tempoRimasto <= 0)
            {
                Console.WriteLine("\nTEMPO SCADUTO! ACCESSO DI SICUREZZA NEGATO.");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine($"--- BREACH PROTOCOL --- TEMPO RIMASTO: {tempoRimasto}s\n");
            Console.WriteLine("SEQUENZA BERSAGLIO: " + string.Join(" - ", target));
            Console.WriteLine("IL TUO BUFFER:      " + string.Join(" - ", buffer) + "\n");

            Console.WriteLine("    0   1   2   3");
            Console.WriteLine("  +---------------+");
            for (int r = 0; r < 4; r++)
            {
                Console.Write(r + " | ");
                for (int c = 0; c < 4; c++)
                {
                    if (matrix[r, c] == "--") Console.Write("--  ");
                    else Console.Write(matrix[r, c] + "  ");
                }
                Console.WriteLine();
            }

            if (buffer.Count >= 3)
            {
                // Controllo se il buffer combacia con il target
                bool vittoria = true;
                for(int i = 0; i < 3; i++) {
                    if (buffer[i] != target[i]) vittoria = false;
                }

                if (vittoria)
                {
                    Console.WriteLine("\nVIOLAZIONE COMPLETATA. ACCESSO GARANTITO.");
                    this.stato = StatoTerminale.Sbloccato;
                    Console.ReadKey(true);
                    SistemaOperativo(player);
                    return;
                }
                else
                {
                    Console.WriteLine("\nSEQUENZA ERRATA. ACCESSO NEGATO.");
                    Console.ReadKey(true);
                    return;
                }
            }

            Console.WriteLine($"\nE' il turno della {(turnoRiga ? "RIGA" : "COLONNA")} {bloccoIndice}.");
            Console.Write("Inserisci l'indice (0-3) della " + (turnoRiga ? "colonna" : "riga") + " da incrociare: ");

            while (!Console.KeyAvailable)
            {
                if (30 - (int)sw.Elapsed.TotalSeconds <= 0) break;
                Thread.Sleep(50);
            }
            if (30 - (int)sw.Elapsed.TotalSeconds <= 0) continue;

            string? input = Console.ReadLine();
            if (int.TryParse(input, out int scelta) && scelta >= 0 && scelta <= 3)
            {
                int r = turnoRiga ? bloccoIndice : scelta;
                int c = turnoRiga ? scelta : bloccoIndice;

                if (matrix[r, c] != "--")
                {
                    buffer.Add(matrix[r, c]);
                    matrix[r, c] = "--";
                    turnoRiga = !turnoRiga;
                    bloccoIndice = scelta;
                }
            }
        }
    }

    private void SistemaOperativo(Giocatore(player))
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine(asciiArt);
            Console.WriteLine("\n 1. Leggi i Log di sistema");
            Console.WriteLine(" 2. Rete Locale");
            Console.WriteLine(" 3. Disconnetti");
            Console.Write("\nSeleziona operazione: ");

            var tasto = Console.ReadKey(true).Key;
            if (tasto == ConsoleKey.D1 || tasto == ConsoleKey.NumPad1)
            {
                Console.Clear();
                Console.WriteLine("--- LOG SALVATI ---\n");
                if (logs.Count == 0) Console.WriteLine("Nessun log presente in questo terminale.");
                foreach (string log in logs) Console.WriteLine("- " + log);
                Console.WriteLine("\nPremi un tasto per tornare indietro...");
                Console.ReadKey(true);
            }
            else if (tasto == ConsoleKey.D2 || tasto == ConsoleKey.NumPad2)
            {
                // Rete Locale: mostra solo le porte e le casse controllabili da questo terminale
                Console.Clear();
                Console.WriteLine("--- RETE LOCALE ---\n");

                if (player.stanza == null)
                {
                    Console.WriteLine("Nessuna stanza associata al giocatore.");
                    Console.WriteLine("\nPremi un tasto per tornare indietro...");
                    Console.ReadKey(true);
                    continue;
                }

                List<string> menuVoci = new List<string>();
                List<object> riferimenti = new List<object>();

                void TryAddPort(Porta? p)
                {
                    if (p != null && porteControllate.Contains(p.nome))
                    {
                        menuVoci.Add("[Porta] " + p.nome);
                        riferimenti.Add(p);
                    }
                }

                TryAddPort(player.stanza.portaNord);
                TryAddPort(player.stanza.portaSud);
                TryAddPort(player.stanza.portaEst);
                TryAddPort(player.stanza.portaOvest);

                // Aggiungiamo le casse presenti nella stanza
                foreach (var o in player.stanza.lista)
                {
                    if (o is Cassa c)
                    {
                        menuVoci.Add("[Cassa] " + c.nome);
                        riferimenti.Add(c);
                    }
                }

                if (menuVoci.Count == 0)
                {
                    Console.WriteLine("Nessun dispositivo rilevabile nella rete locale controllabile da questo terminale.");
                    Console.WriteLine("\nPremi un tasto per tornare indietro...");
                    Console.ReadKey(true);
                    continue;
                }

                for (int i = 0; i < menuVoci.Count; i++) Console.WriteLine($"{i+1}. {menuVoci[i]}");
                Console.WriteLine("\nSeleziona un dispositivo (0 per uscire): ");
                string? sel = Console.ReadLine();
                if (!int.TryParse(sel, out int idx) || idx < 0 || idx > menuVoci.Count) continue;
                if (idx == 0) continue;

                object target = riferimenti[idx - 1];

                if (target is Porta porta)
                {
                    Console.WriteLine($"\nDispositivo selezionato: Porta '{porta.nome}'. Stato attuale: {porta.stato}");
                    Console.WriteLine("1. Apri porta");
                    Console.WriteLine("2. Torna indietro");
                    Console.Write("> ");
                    string? a = Console.ReadLine();
                    if (a == "1")
                    {
                        porta.CambiaStato(Porta.StatoPorta.Aperta);
                        Console.WriteLine($"\nLa porta '{porta.nome}' è stata aperta dal terminale.");
                    }
                    Console.WriteLine("\nPremi un tasto per tornare indietro...");
                    Console.ReadKey(true);
                    continue;
                }

                if (target is Cassa cassa)
                {
                    Console.WriteLine($"\nDispositivo selezionato: {cassa.nome}. Stato attuale: {cassa.stato}");
                    Console.WriteLine("1. Sblocca");
                    Console.WriteLine("2. Apri");
                    Console.WriteLine("3. Torna indietro");
                    Console.Write("> ");
                    string? a = Console.ReadLine();
                    if (a == "1")
                    {
                        if (cassa.Sblocca()) Console.WriteLine($"\n{cassa.nome} è stata sbloccata.");
                        else Console.WriteLine($"\n{cassa.nome} non può essere sbloccata o è già sbloccata.");
                    }
                    else if (a == "2")
                    {
                        cassa.Usa(player);
                    }
                    Console.WriteLine("\nPremi un tasto per tornare indietro...");
                    Console.ReadKey(true);
                    continue;
                }
            }
            else if (tasto == ConsoleKey.D3 || tasto == ConsoleKey.NumPad3)
            {
                return;
            }
        }
    }
}
