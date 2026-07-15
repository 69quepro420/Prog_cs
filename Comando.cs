using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Project;

class Comando
{
    public static void Start(Giocatore player)
    {
        while (true)
        {
            player.DisegnaHUD();

            // Attesa dell'input non bloccante: durante l'evento dell'IA ostile
            // l'HUD viene ridisegnato ogni secondo per far scorrere il countdown
            ConsoleKeyInfo? infoTasto = AttendiTasto(player);
            if (infoTasto == null) // un timer è scaduto durante l'attesa
            {
                if (GestisciFinePartita(player)) return;
                continue;
            }

            string tasto = infoTasto.Value.Key.ToString().ToUpper();

            switch (tasto)
            {
                case "W":
                case "A":
                case "S":
                case "D":
                    player.Muoviti(tasto);
                    EventoIA.ControllaIngresso(player);     // stanza casuale: IA ostile
                    EventoFinale.ControllaIngresso(player); // Sala Comandi: scontro finale
                    break;

                case "M":
                    player.MostraMappa();
                    break;

                case "C":
                    CercaOggetti(player);
                    break;

                case "E":
                    GestisciInventarioLIFO(player);
                    break;

                case "T":
                    Parla(player);
                    break;

                case "ESCAPE":
                    if (MenuPausa(player)) return; // torna al menu principale
                    break;
            }

            // Controllo di fine partita (vittoria/sconfitta) dopo ogni azione
            if (GestisciFinePartita(player)) return;
        }
    }

    /// <summary>
    /// Valuta le condizioni di fine partita e mostra il finale appropriato.
    /// Ritorna true se la partita è terminata (bisogna uscire dal ciclo).
    /// </summary>
    private static bool GestisciFinePartita(Giocatore player)
    {
        // 1. Vittoria: i 3 componenti sono installati nella navetta.
        //    Ha priorità: se sei fuggito in tempo, i timer non contano più.
        if (Global.partitaVinta)
        {
            if (EventoFinale.scelta == EventoFinale.Scelta.Autodistruzione)
                FinaleFugaAutodistruzione(player);
            else if (EventoFinale.scelta == EventoFinale.Scelta.LasciaIAViva)
                FinaleIAViva(player);
            else
                SchermataVittoria(player); // fallback (non dovrebbe accadere)
            return true;
        }

        // 2. Morte per esaurimento ossigeno
        if (EventoIA.ControllaScadenza())
        {
            SchermataSconfitta(player);
            return true;
        }

        // 3. Morte a bordo per autodistruzione
        if (EventoFinale.ControllaScadenza())
        {
            FinaleMorteAutodistruzione(player);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Attende un tasto ridisegnando l'HUD a ogni secondo quando un timer
    /// (ossigeno o autodistruzione) è attivo. Ritorna null se un timer scade.
    /// </summary>
    private static ConsoleKeyInfo? AttendiTasto(Giocatore player)
    {
        int ultimoSecondo = -1;
        while (!Console.KeyAvailable)
        {
            if (EventoIA.ControllaScadenza() || EventoFinale.ControllaScadenza()) return null;

            if (EventoIA.TimerAttivo || EventoFinale.TimerAttivo)
            {
                int s = EventoFinale.TimerAttivo ? EventoFinale.SecondiRimasti : EventoIA.SecondiRimasti;
                if (s != ultimoSecondo)
                {
                    ultimoSecondo = s;
                    player.DisegnaHUD();
                }
            }
            Thread.Sleep(50);
        }
        return Console.ReadKey(true);
    }

    /// <summary>
    /// Tasto [T]: parla con il personaggio presente nella stanza e/o con
    /// l'IA Tascabile, se è nell'inventario. Se entrambi sono disponibili,
    /// il giocatore sceglie con chi parlare.
    /// </summary>
    private static void Parla(Giocatore player)
    {
        Personaggio? npc = player.stanza!.personaggio;
        IATascabile? ia = player.inventario.OfType<IATascabile>().FirstOrDefault();

        if (npc != null && ia != null)
        {
            Menu menu = new Menu(new[] { npc.nome, "IA Tascabile" }, "Con chi vuoi parlare? (ESC per annullare)");
            int scelta = menu.Selezione();
            if (scelta == 0) npc.Parla();
            else if (scelta == 1) ia.Parla(player);
        }
        else if (npc != null)
        {
            npc.Parla();
        }
        else if (ia != null)
        {
            ia.Parla(player);
        }
        else
        {
            Console.WriteLine("\nNon c'è nessuno con cui parlare qui. (Premi un tasto)");
            Console.ReadKey(true);
        }
    }

    /// <summary>
    /// Schermata di sconfitta: l'ossigeno è finito.
    /// </summary>
    private static void SchermataSconfitta(Giocatore player)
    {
        Console.Clear();
        Console.WriteLine("===========================================================");
        Console.WriteLine("                   OSSIGENO ESAURITO                       ");
        Console.WriteLine("===========================================================");
        Console.WriteLine();
        Console.WriteLine(" L'aria si fa sempre più sottile... il buio ti avvolge.");
        Console.WriteLine($" La stazione ha reclamato anche te, {player.nome}.");
        Console.WriteLine();
        Console.WriteLine("                      HAI PERSO");
        Console.WriteLine();
        Console.WriteLine("===========================================================");
        Console.WriteLine("\nPremi un tasto per tornare al menu principale...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Schermata finale mostrata quando la navetta è stata riparata.
    /// </summary>
    private static void SchermataVittoria(Giocatore player)
    {
        Console.Clear();
        Console.WriteLine("===========================================================");
        Console.WriteLine("              LA NAVETTA E' STATA RIPARATA!                ");
        Console.WriteLine("===========================================================");
        Console.WriteLine();
        Console.WriteLine($" I motori si riaccendono con un rombo possente.");
        Console.WriteLine($" Complimenti, {player.nome}: hai lasciato la stazione!");
        Console.WriteLine();
        Console.WriteLine($" Passi compiuti: {player.contatorePassi}");
        Console.WriteLine();
        Console.WriteLine("===========================================================");
        Console.WriteLine("\nPremi un tasto per tornare al menu principale...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// FINALE 1: autodistruzione attivata e fuga riuscita in tempo.
    /// </summary>
    private static void FinaleFugaAutodistruzione(Giocatore player)
    {
        Console.Clear();
        Console.WriteLine("===========================================================");
        Console.WriteLine("                    FUGA ALL'ULTIMO SECONDO                ");
        Console.WriteLine("===========================================================");
        Console.WriteLine();
        Console.WriteLine(" La navetta si stacca mentre la stazione esplode alle tue spalle.");
        Console.WriteLine(" L'IA ostile - e con lei la minaccia per le altre navi - è");
        Console.WriteLine(" incenerita nel silenzio dello spazio.");
        Console.WriteLine($" Ce l'hai fatta, {player.nome}. Sei un eroe. (PLACEHOLDER)");
        Console.WriteLine();
        Console.WriteLine("                   HAI VINTO - FINALE EROE");
        Console.WriteLine();
        Console.WriteLine("===========================================================");
        Console.WriteLine("\nPremi un tasto per tornare al menu principale...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// FINALE 2: autodistruzione attivata ma timer scaduto a bordo.
    /// </summary>
    private static void FinaleMorteAutodistruzione(Giocatore player)
    {
        Console.Clear();
        Console.WriteLine("===========================================================");
        Console.WriteLine("                      SACRIFICIO FINALE                    ");
        Console.WriteLine("===========================================================");
        Console.WriteLine();
        Console.WriteLine(" Non hai raggiunto la navetta in tempo.");
        Console.WriteLine(" La stazione si squarcia in un lampo accecante.");
        Console.WriteLine($" Sei morto, {player.nome}, ma l'IA ostile è morta con te:");
        Console.WriteLine(" le altre navi sono salve. (PLACEHOLDER)");
        Console.WriteLine();
        Console.WriteLine("              FINE - SACRIFICIO (l'IA è distrutta)");
        Console.WriteLine();
        Console.WriteLine("===========================================================");
        Console.WriteLine("\nPremi un tasto per tornare al menu principale...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// FINALE 3: l'IA viene lasciata in vita e il giocatore fugge.
    /// </summary>
    private static void FinaleIAViva(Giocatore player)
    {
        Console.Clear();
        Console.WriteLine("===========================================================");
        Console.WriteLine("                     FUGA NELL'OMBRA                       ");
        Console.WriteLine("===========================================================");
        Console.WriteLine();
        Console.WriteLine(" La navetta si allontana dalla stazione intatta.");
        Console.WriteLine(" Alle tue spalle, l'IA ostile è ancora viva e libera:");
        Console.WriteLine(" i suoi segnali si propagano già verso le altre navi.");
        Console.WriteLine($" Ti sei salvato, {player.nome}, ma a quale prezzo? (PLACEHOLDER)");
        Console.WriteLine();
        Console.WriteLine("            FINE - FUGA (l'IA è ancora là fuori)");
        Console.WriteLine();
        Console.WriteLine("===========================================================");
        Console.WriteLine("\nPremi un tasto per tornare al menu principale...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Menu di pausa: salvataggio e uscita al menu principale.
    /// Ritorna true se il giocatore vuole uscire dalla partita.
    /// </summary>
    private static bool MenuPausa(Giocatore player)
    {
        Menu menu = new Menu(
            new[] { "Riprendi", "Salva Partita", "Salva ed Esci", "Esci senza Salvare" },
            "PAUSA");

        int scelta = menu.Selezione();

        if (scelta == 1 || scelta == 2)
        {
            Console.Clear();
            if (Salvataggio.Salva(player))
                Console.WriteLine("\nPartita salvata!");
            else
                Console.WriteLine("\nSalvataggio non riuscito.");
            Console.WriteLine("\nPremi un tasto per continuare...");
            Console.ReadKey(true);
        }

        return scelta == 2 || scelta == 3;
    }

    /// <summary>
    /// Permette al giocatore di cercare e interagire con gli oggetti nella stanza.
    /// </summary>
    private static void CercaOggetti(Giocatore player)
    {
        if (player.stanza!.lista.Count == 0)
        {
            Console.WriteLine("\nNon c'è niente di interessante in questa stanza. (Premi un tasto)");
            Console.ReadKey(true);
            return;
        }

        // Creiamo un menu con i nomi degli oggetti
        string[] nomiOggetti = player.stanza.lista.Select(o => o.nome).ToArray();
        Menu menuOggetti = new Menu(nomiOggetti, "CERCA - Seleziona un oggetto da esaminare (ESC per uscire)");

        int scelta = menuOggetti.Selezione();

        if (scelta == -1) return; // Ha premuto ESC

        // Prendiamo l'oggetto selezionato
        Oggetto oggettoSelezionato = player.stanza.lista[scelta];

        // Mostriamo il menù delle azioni
        Menu menuAzioni = new Menu(
            new string[] { "Usa", "Esamina", "Prendi (se possibile)" },
            $"Cosa vuoi fare con: {oggettoSelezionato.nome}? (ESC per uscire)"
        );

        int sceltaAzione = menuAzioni.Selezione();

        if (sceltaAzione == -1) return;

        Console.Clear();

        // Eseguiamo l'azione
        if (sceltaAzione == 0)
        {
            // USA - Chiama il metodo polimorfo Usa() dell'oggetto
            oggettoSelezionato.Usa(player);

            // Un oggetto consumato (componente installato, antidolorifici usati...)
            // sparisce dalla stanza
            if (oggettoSelezionato.Consumato)
                player.stanza.lista.Remove(oggettoSelezionato);
        }
        else if (sceltaAzione == 1)
        {
            // ESAMINA
            Console.WriteLine($"\n{oggettoSelezionato.nome}");
            Console.WriteLine($"Descrizione: {oggettoSelezionato.descr}");
            if (oggettoSelezionato.mobile)
                Console.WriteLine($"Peso: {oggettoSelezionato.peso} kg (Oggetto portatile)");
            else
                Console.WriteLine("(Oggetto troppo grande o fisso per essere spostato)");
        }
        else if (sceltaAzione == 2)
        {
            // PRENDI
            if (!oggettoSelezionato.mobile)
            {
                Console.WriteLine($"\n{oggettoSelezionato.nome} è troppo grande o fisso per essere preso.");
            }
            else if (player.PesoInventario() + oggettoSelezionato.peso > Giocatore.pesoMassimo)
            {
                Console.WriteLine($"\n{oggettoSelezionato.nome} pesa {oggettoSelezionato.peso:0.#} kg: troppo!");
                Console.WriteLine($"Stai trasportando {player.PesoInventario():0.#}/{Giocatore.pesoMassimo:0.#} kg. Scarta qualcosa prima.");
            }
            else
            {
                player.inventario.Push(oggettoSelezionato);
                player.stanza.lista.RemoveAt(scelta);
                Console.WriteLine($"\nHai preso {oggettoSelezionato.nome} e lo hai messo nell'inventario!");
            }
        }

        Console.WriteLine("\nPremi un tasto per tornare all'HUD...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Gestisce l'apertura dell'inventario e il rispetto del vincolo LIFO (Estrazione e Reinserimento).
    /// </summary>
    private static void GestisciInventarioLIFO(Giocatore player)
    {
        if (player.inventario.Count == 0)
        {
            Console.WriteLine("\nIl tuo inventario è vuoto! (Premi un tasto)");
            Console.ReadKey(true);
            return;
        }

        // Convertiamo la pila in array solo per visualizzare il menù (senza alterare la struttura)
        string[] nomiOggetti = player.inventario.Select(o => o.nome).ToArray();
        Menu menuInv = new Menu(nomiOggetti, $"INVENTARIO ({player.PesoInventario():0.#}/{Giocatore.pesoMassimo:0.#} kg) - Seleziona un oggetto (ESC per uscire)");

        int sceltOggetto = menuInv.Selezione();

        if (sceltOggetto == -1) return; // Ha premuto ESC

        // Ora apriamo il sottomenu delle azioni per l'oggetto scelto
        Menu menuAzione = new Menu(new string[] { "Usa", "Esamina", "Scarta" }, $"Cosa vuoi fare con: {nomiOggetti[sceltOggetto]}? (ESC per uscire)");
        int sceltaAzione = menuAzione.Selezione();

        if (sceltaAzione == -1) return;

        // == LOGICA OBBLIGATORIA DEL PROFESSORE (ESTRAZIONE LIFO) ==
        Console.Clear();
        Stack<Oggetto> appoggio = new Stack<Oggetto>();

        // 1. Estraiamo gli oggetti sovrastanti uno a uno
        Console.WriteLine("Avvio procedura di estrazione LIFO...");
        for (int i = 0; i < sceltOggetto; i++)
        {
            Oggetto estratto = player.inventario.Pop();
            Console.WriteLine($"[Spostato temporaneamente: {estratto.nome}]");
            appoggio.Push(estratto);
        }

        // 2. Ora l'oggetto che vogliamo è in cima alla pila. Lo prendiamo (Pop).
        Oggetto oggettoTarget = player.inventario.Pop();

        // Eseguiamo l'azione
        if (sceltaAzione == 0)
        {
            // USA - Chiama il metodo polimorfo Usa() dell'oggetto
            oggettoTarget.Usa(player);
        }
        else if (sceltaAzione == 1)
        {
            Console.WriteLine($"\nDESCRIZIONE: {oggettoTarget.descr}");
        }
        else if (sceltaAzione == 2)
        {
            Console.WriteLine($"\nHai SCARTATO {oggettoTarget.nome} (gettato sul pavimento).");
            // Aggiungiamo l'oggetto nella stanza corrente
            player.stanza!.lista.Add(oggettoTarget);
        }

        // 3. Reinseriamo l'oggetto Target nell'inventario
        //    (a meno che non sia stato scartato o consumato dall'uso)
        if (sceltaAzione != 2 && !oggettoTarget.Consumato) player.inventario.Push(oggettoTarget);

        // 4. Reinseriamo gli oggetti temporanei mantenendo l'ordine originario
        Console.WriteLine("\nReinserimento oggetti nella Pila...");
        while (appoggio.Count > 0)
        {
            player.inventario.Push(appoggio.Pop());
        }

        Console.WriteLine("\nOperazione completata. (Premi un tasto per tornare all'HUD)");
        Console.ReadKey(true);
    }
}
