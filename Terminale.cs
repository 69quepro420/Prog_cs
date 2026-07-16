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

    // Liste di dispositivi che questo terminale può controllare (impostate dallo sviluppatore)
    public List<Porta> porteControllate = new List<Porta>();
    public List<Cassa> casseControllate = new List<Cassa>();

    // Se true, la Rete Locale offre il comando di ripristino dell'ossigeno
    // (usato dal terminale della Sala Ossigeno durante l'evento dell'IA ostile)
    public bool sistemaOssigeno = false;

    public string asciiArt =
@"
  ____
 6MMMMb\
6M'    `
MM      __ ____      ___     ____     ____           _____     ____
YM.     `M6MMMMb   6MMMMb   6MMMMb.  6MMMMb         6MMMMMb   6MMMMb\
 YMMMMb  MM'  `Mb 8M'  `Mb 6M'   Mb 6M'  `Mb       6M'   `Mb MM'    `
     `Mb MM    MM     ,oMM MM    `' MM    MM       MM     MM YM.
      MM MM    MM ,6MM9'MM MM       MMMMMMMM       MM     MM  YMMMMb
      MM MM    MM MM'   MM MM       MM             MM     MM      `Mb
L    ,M9 MM.  ,M9 MM.  ,MM YM.   d9 YM    d9       YM.   ,M9 L    ,MM
MYMMMM9  MMYMMM9  `YMMM9'Yb.YMMMM9   YMMMM9         YMMMMM9  MYMMMM9
         MM
         MM
        _MM_";

    public Terminale(string nome, string descr, StatoTerminale stato, string password)
    {
        this.nome = nome;
        this.descr = descr;
        this.peso = 0f;
        this.mobile = false; // L'oggetto non può essere messo nell'inventario
        this.stato = stato;
        this.password = password;
        this.logs = new List<string>();
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

            // Attesa dell'input con countdown dinamico: la riga del timer
            // viene riscritta ogni secondo senza ridisegnare tutto lo schermo
            int ultimoSecondo = -1;
            while (!Console.KeyAvailable)
            {
                int rimasto = 30 - (int)sw.Elapsed.TotalSeconds;
                if (rimasto <= 0) break;

                if (rimasto != ultimoSecondo)
                {
                    ultimoSecondo = rimasto;
                    (int col, int riga) = Console.GetCursorPosition();
                    Console.SetCursorPosition(0, 0);
                    Console.Write($"--- BREACH PROTOCOL --- TEMPO RIMASTO: {rimasto}s   ");
                    Console.SetCursorPosition(col, riga);
                }
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

    private void SistemaOperativo(Giocatore player)
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
                // Rete Locale come "chiave" per dispositivi pre-selezionati dallo sviluppatore
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("--- RETE LOCALE ---\n");

                    int totaleDispositivi = porteControllate.Count + casseControllate.Count + (sistemaOssigeno ? 1 : 0);
                    if (totaleDispositivi == 0)
                    {
                        Console.WriteLine("Nessun dispositivo controllato da questo terminale.");
                        Console.WriteLine("\nPremi un tasto per tornare indietro...");
                        Console.ReadKey(true);
                        break;
                    }

                    Dictionary<int, (string tipo, object riferimento)> mappa = new Dictionary<int, (string, object)>();
                    int idx = 1;

                    Console.WriteLine("Dispositivi controllati:");
                    foreach (var p in porteControllate)
                    {
                        Console.WriteLine($"{idx}. [PORTA] {p.nome} - Stato: {p.stato}");
                        mappa[idx] = ("porta", p);
                        idx++;
                    }
                    foreach (var c in casseControllate)
                    {
                        Console.WriteLine($"{idx}. [CASSA] {c.nome} - Stato: {c.stato}");
                        mappa[idx] = ("cassa", c);
                        idx++;
                    }
                    if (sistemaOssigeno)
                    {
                        string statoOssigeno = EventoIA.TimerAttivo ? "EMERGENZA" : "Normale";
                        Console.WriteLine($"{idx}. [SISTEMA] Ripristino Ossigeno - Stato: {statoOssigeno}");
                        mappa[idx] = ("ossigeno", this);
                        idx++;
                    }

                    Console.WriteLine("\nSeleziona il numero del dispositivo da gestire (0 per uscire): ");
                    string? input = Console.ReadLine();
                    if (!int.TryParse(input, out int sel)) continue;
                    if (sel == 0) break;
                    if (!mappa.ContainsKey(sel)) continue;

                    var entry = mappa[sel];
                    if (entry.tipo == "ossigeno")
                    {
                        if (EventoIA.attivo && !EventoIA.risolto)
                        {
                            if (EventoIA.ControllaScadenza())
                            {
                                Console.WriteLine("\nTROPPO TARDI: i condotti sono ormai sigillati...");
                            }
                            else
                            {
                                EventoIA.RipristinaOssigeno();
                                Console.WriteLine("\nOSSIGENO RIPRISTINATO. I condotti tornano a sibilare regolarmente.");
                                Console.WriteLine("L'emergenza è rientrata.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nI livelli di ossigeno sono nella norma.");
                        }
                        Console.WriteLine("\nPremi un tasto per tornare alla rete locale..."); Console.ReadKey(true);
                    }
                    else if (entry.tipo == "porta")
                    {
                        var p = (Porta)entry.riferimento;
                        if (p.stato == Porta.StatoPorta.Aperta)
                        {
                            Console.WriteLine($"\nLa porta '{p.nome}' è già aperta.");
                        }
                        else
                        {
                            Console.WriteLine($"\nVuoi aprire la porta '{p.nome}'? (S/N)");
                            var k = Console.ReadKey(true).Key;
                            if (k == ConsoleKey.S) { p.CambiaStato(Porta.StatoPorta.Aperta); Console.WriteLine($"Porta '{p.nome}' aperta."); }
                            else Console.WriteLine("Operazione annullata.");
                        }
                        Console.WriteLine("\nPremi un tasto per tornare alla rete locale..."); Console.ReadKey(true);
                    }
                    else // cassa
                    {
                        var c = (Cassa)entry.riferimento;
                        if (c.stato == Cassa.StatoCassa.Bloccata)
                        {
                            Console.WriteLine($"\nLa cassa '{c.nome}' è bloccata. Vuoi sbloccarla via terminale? (S/N)");
                            var k = Console.ReadKey(true).Key;
                            if (k == ConsoleKey.S) { c.Sblocca(); Console.WriteLine($"Cassa '{c.nome}' sbloccata."); }
                            else Console.WriteLine("Operazione annullata.");
                        }
                        else if (c.stato == Cassa.StatoCassa.Sbloccata)
                        {
                            Console.WriteLine($"\nVuoi aprire la cassa '{c.nome}'? (S/N)");
                            var k = Console.ReadKey(true).Key;
                            if (k == ConsoleKey.S) { c.Usa(player); }
                            else Console.WriteLine("Operazione annullata.");
                        }
                        else // Saccheggiata
                        {
                            Console.WriteLine($"\nLa cassa '{c.nome}' è già stata saccheggiata.");
                        }
                        Console.WriteLine("\nPremi un tasto per tornare alla rete locale..."); Console.ReadKey(true);
                    }
                }
            }
            else if (tasto == ConsoleKey.D3 || tasto == ConsoleKey.NumPad3)
            {
                return;
            }
        }
    }
}
