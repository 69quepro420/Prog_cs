using System;
using System.Collections.Generic;
using System.Linq;

namespace Project;

class Giocatore
{
    public string nome { get; set; }
    public int[] coordinate;
    public Stanza? stanza;
    public Stack<Oggetto> inventario { get; set; }
    public int vita = 100;
    public int contatorePassi = 0;

    /// <summary>
    /// Sfrutta il polimorfismo chiamando il metodo Usa dell'oggetto specifico.
    /// </summary>
    public void UsaOggetto(Oggetto obj)
    {
        obj.Usa(this);
    }

    public Giocatore(string nome)
    {
        this.nome = nome;
        coordinate = new int[] { 0, 0 };
        inventario = new Stack<Oggetto>();
    }

    public void DisegnaHUD()
    {
        Console.Clear();
        int blocchiVita = vita / 10;
        string barraVita = new string('█', blocchiVita).PadRight(10, '-');

        string infoUtente = $"{nome} | HP: [{barraVita}]".PadRight(43);
        string contatore = $"COUNTER: {contatorePassi:D4}";

        Console.WriteLine("+-----------------------------------------------------------+");
        Console.WriteLine($"| {infoUtente} {contatore} |");
        Console.WriteLine("+-------------------------------------------+---------------+");
        Console.WriteLine("| AZIONI                                    | INVENTARIO    |");
        Console.WriteLine("|                                           |               |");

        string[] azioni = {
            " [WASD] Muoviti",
            " [E] INVENTARIO",
            " [C] CERCA OGGETTI",
            " [T] PARLA",
            " [M] MAPPA"
        };

        List<string> invTesto = inventario.Select(o => "- " + o.nome).ToList();

        for (int i = 0; i < 8; i++)
        {
            string colonnaSx = i < azioni.Length ? azioni[i].PadRight(41) : "".PadRight(41);
            string colonnaDx = i < invTesto.Count ? invTesto[i].PadRight(13) : "".PadRight(13);

            Console.WriteLine($"| {colonnaSx} | {colonnaDx} |");
        }

        Console.WriteLine("+-------------------------------------------+---------------+");
        Console.WriteLine($"\nSei in: {stanza!.nome}");
        Console.WriteLine($"{stanza.descr}\n");
    }

    public void Muoviti(string direzione)
    {
        // Specifichiamo gli indici dell'array con [0] per la riga e [1] per la colonna
        int r = coordinate[0];
        int c = coordinate[1];
        
        // Determina quale porta controllare
        Porta? porta = null;
        
        switch (direzione)
        {
            case "W": r--; porta = stanza?.portaNord; break;
            case "S": r++; porta = stanza?.portaSud; break;
            case "A": c--; porta = stanza?.portaOvest; break;
            case "D": c++; porta = stanza?.portaEst; break;
        }

        // PRIMO CONTROLLO: Verifica se le coordinate sono valide nella mappa
        if(r < 0 || r >= Global.map.Length || c < 0 || c >= Global.map[r].Length || Global.map[r][c] == null)
        {
            Console.WriteLine("\nC'è un muro in quella direzione. (Premi un tasto)");
            Console.ReadKey(true);
            return;
        }

        // SECONDO CONTROLLO: Verifica se la porta esiste e se è aperta
        if (porta == null || !porta.PermettePassaggio())
        {
            if (porta == null)
            {
                Console.WriteLine("\nNon c'è alcun passaggio in quella direzione. (Premi un tasto)");
            }
            else
            {
                Console.WriteLine(porta.GetMessaggioPassaggio());
            }
            Console.ReadKey(true);
            return;
        }

        // Se tutti i controlli passano, esegui il movimento
        coordinate = new int[] { r, c };
        stanza = Global.map[r][c];
        contatorePassi++;
    }

    public void MostraMappa()
    {
        Console.Clear();
        // Stampa la stringa ASCII esatta della stanza attuale
        Console.WriteLine(stanza.mappaAscii);

        Console.WriteLine("\nPremi un tasto per tornare all'HUD...");
        Console.ReadKey(true);
    }
}
