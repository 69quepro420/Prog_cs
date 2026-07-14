using System;
using System.Collections.Generic;

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
/// Cassa: contenitore che può essere bloccato, sbloccato o saccheggiato.
/// Quando viene aperta in stato Sbloccata rilascia il contenuto nella stanza.
/// </summary>
class Cassa : Oggetto
{
    public enum StatoCassa { Bloccata, Sbloccata, Saccheggiata }
    public StatoCassa stato { get; set; }
    public Oggetto? contenuto { get; set; }

    public Cassa() { }

    public Cassa(string nome, string descr, Oggetto? contenuto, StatoCassa stato = StatoCassa.Bloccata)
    {
        this.nome = nome;
        this.descr = descr;
        this.peso = 5f; // valore di default
        this.mobile = false; // la cassa è fissa
        this.contenuto = contenuto;
        this.stato = stato;
    }

    /// <summary>
    /// Interazione diretta con la cassa.
    /// - Bloccata: mostra messaggio.
    /// - Sbloccata: rilascia il contenuto nella stanza e diventa Saccheggiata.
    /// - Saccheggiata: informa che è già stata svuotata.
    /// </summary>
    public override void Usa(Giocatore player)
    {
        Console.Clear();
        if (stato == StatoCassa.Bloccata)
        {
            Console.WriteLine($"{nome} è bloccata da un sistema elettronico. Non puoi aprirla manualmente.");
        }
        else if (stato == StatoCassa.Sbloccata)
        {
            if (contenuto != null)
            {
                Console.WriteLine($"Apri {nome}. Trovi {contenuto.nome} all'interno. (Viene posizionato nella stanza)");
                // Aggiungiamo il contenuto alla stanza corrente
                player.stanza!.lista.Add(contenuto);
                // svuotiamo la cassa
                contenuto = null;
            }
            else
            {
                Console.WriteLine($"Apri {nome}. Non c'è niente all'interno.");
            }
            stato = StatoCassa.Saccheggiata;
        }
        else // Saccheggiata
        {
            Console.WriteLine($"{nome} è già stata saccheggiata.");
        }

        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Metodo per sbloccare la cassa (utilizzabile dal Terminale o da chiavi).
    /// </summary>
    public void Sblocca()
    {
        if (stato == StatoCassa.Bloccata)
            stato = StatoCassa.Sbloccata;
    }
}
