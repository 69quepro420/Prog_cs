using System;

namespace Project;

class Oggetto
{
    public string nome { get; set; }
    public string descr { get; set; }
    public float peso { get; set; }
    public bool mobile { get; set; }

    // --- 1. COSTRUTTORE VUOTO (Risolve l'errore del Terminale CS7036) ---
    public Oggetto() { }

    // --- 2. COSTRUTTORE CON PARAMETRI (Risolve l'errore dell'oggetto di test precedente) ---
    public Oggetto(string nome, string descr, float peso, bool mobile)
    {
        this.nome = nome;
        this.descr = descr;
        this.peso = peso;
        this.mobile = mobile;
    }

    /// <summary>
    /// Metodo base per l'interazione. Viene sovrascritto (override) dal Terminale.
    /// </summary>
    public virtual void Usa(Giocatore player)
    {
        Console.WriteLine("Non c'è niente di speciale da fare con questo oggetto.");
    }
}
