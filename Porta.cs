using System;

namespace Project;

/// <summary>
/// Rappresenta una porta tra due stanze.
/// Può essere: Aperta, Bloccata o Rotta
/// </summary>
class Porta
{
    public enum StatoPorta
    {
        Aperta,      // Passaggio libero
        Bloccata,    // Richiede una chiave o azione
        Rotta        // Impossibile passare
    }

    public string nome { get; set; }
    public StatoPorta stato { get; set; }

    /// <summary>
    /// Costruttore della porta
    /// </summary>
    public Porta(string nome, StatoPorta stato = StatoPorta.Aperta)
    {
        this.nome = nome;
        this.stato = stato;
    }

    /// <summary>
    /// Ritorna il messaggio appropriato quando il giocatore prova a passare
    /// </summary>
    public string GetMessaggioPassaggio()
    {
        return stato switch
        {
            StatoPorta.Aperta => "",  // Passaggio libero, nessun messaggio
            StatoPorta.Bloccata => $"\n⛔ La porta '{nome}' è bloccata. Non puoi passare.",
            StatoPorta.Rotta => $"\n🔨 La porta '{nome}' è rotta. Non esiste alcun modo per passare.",
            _ => "Errore sconosciuto"
        };
    }

    /// <summary>
    /// Controlla se il passaggio è consentito
    /// </summary>
    public bool PermettePassaggio()
    {
        return stato == StatoPorta.Aperta;
    }

    /// <summary>
    /// Cambia lo stato della porta
    /// </summary>
    public void CambiaStato(StatoPorta nuovoStato)
    {
        stato = nuovoStato;
    }
}
