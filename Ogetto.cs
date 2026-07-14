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

/// <summary>
/// Classe Cassa: oggetto interattivo con 3 stati (Bloccata, Sbloccata, Saccheggiata)
/// Contiene un singolo Oggetto che, quando la cassa viene aperta (stato Sbloccata), viene lasciato nella stanza.
/// I messaggi di interazione usano direttamente la proprietà `nome` per essere personalizzabili.
/// </summary>
class Cassa : Oggetto
{
    public enum StatoCassa { Bloccata, Sbloccata, Saccheggiata }

    public StatoCassa stato { get; private set; }
    public Oggetto? contenuto { get; private set; }

    public Cassa() { }

    public Cassa(string nome, Oggetto? contenuto, StatoCassa stato = StatoCassa.Bloccata)
    {
        this.nome = nome;
        this.descr = "Una cassa metallica.";
        this.mobile = false;
        this.peso = 0f;
        this.contenuto = contenuto;
        this.stato = stato;
    }

    /// <summary>
    /// Interazione principale con la cassa (apri)
    /// </summary>
    public override void Usa(Giocatore player)
    {
        // Assicuriamoci che la stanza del giocatore sia definita
        if (player.stanza == null)
        {
            Console.WriteLine("Errore: giocatore non in una stanza.");
            return;
        }

        switch (stato)
        {
            case StatoCassa.Bloccata:
                Console.WriteLine($"{nome} è bloccata da un sistema elettronico.");
                break;

            case StatoCassa.Sbloccata:
                if (contenuto != null)
                {
                    // Mettiamo l'oggetto contenuto nella stanza
                    player.stanza.lista.Add(contenuto);
                    var trovato = contenuto;
                    contenuto = null;
                    stato = StatoCassa.Saccheggiata;
                    Console.WriteLine($"Hai aperto {nome} e trovato: {trovato.nome}. L'oggetto è stato lasciato nella stanza.");
                }
                else
                {
                    stato = StatoCassa.Saccheggiata;
                    Console.WriteLine($"{nome} sembra vuoto.");
                }
                break;

            case StatoCassa.Saccheggiata:
                Console.WriteLine($"{nome} è saccheggiata.");
                break;
        }

        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Sblocca la cassa (semplice comportamento: sblocca solo se era bloccata)
    /// </summary>
    public bool Sblocca()
    {
        if (stato == StatoCassa.Bloccata)
        {
            stato = StatoCassa.Sbloccata;
            return true;
        }
        return false;
    }
}
