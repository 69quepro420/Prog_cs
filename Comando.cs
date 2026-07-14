using System;
using System.Collections.Generic;
using System.Linq;

namespace Project;

class Comando
{
    public static void Start(Giocatore player)
    {
        while (true)
        {
            player.DisegnaHUD();

            // Ascolta il tasto premuto senza mostrarlo a schermo (true)
            ConsoleKeyInfo infoTasto = Console.ReadKey(true);
            string tasto = infoTasto.Key.ToString().ToUpper();

            switch (tasto)
            {
                case "W":
                case "A":
                case "S":
                case "D":
                    player.Muoviti(tasto);
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
                    Console.WriteLine("\nNon c'è nessuno con cui parlare qui. (Premi un tasto)");
                    Console.ReadKey(true);
                    break;
            }
        }
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
            else
            {
                // Aggiungiamo all'inventario (per ora senza controllo peso)
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
        Menu menuInv = new Menu(nomiOggetti, "INVENTARIO - Seleziona un oggetto (ESC per uscire)");

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
        }

        // 3. Reinseriamo l'oggetto Target nell'inventario (se non è stato scartato/consumato)
        if (sceltaAzione != 2) player.inventario.Push(oggettoTarget);

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
