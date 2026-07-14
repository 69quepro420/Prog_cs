using System.Collections.Generic;

namespace Project;

class Stanza
{
    public string nome { get; set; }
    public string descr { get; set; }
    public bool visitata { get; set; }
    public string mappaAscii { get; set; } // <--- Nuova proprietà per la cartina
    public List<Oggetto> lista { get; set; }
    
    // Porte in 4 direzioni
    public Porta? portaNord { get; set; }
    public Porta? portaSud { get; set; }
    public Porta? portaEst { get; set; }
    public Porta? portaOvest { get; set; }

    /// <summary>
    /// Costruttore della stanza con mappa inclusa.
    /// </summary>
    public Stanza(string nome, string descr, string mappaAscii)
    {
        this.nome = nome;
        this.descr = descr;
        this.mappaAscii = mappaAscii;
        this.visitata = false;
        this.lista = new List<Oggetto>();
        
        // Inizialmente tutte le porte sono null (non esistono)
        portaNord = null;
        portaSud = null;
        portaEst = null;
        portaOvest = null;
    }
}
