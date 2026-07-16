using System;

namespace Project;

/// <summary>
/// Personaggio non giocante presente in una stanza (es. un membro
/// dell'equipaggio o l'IA ostile). Ha un nome e una descrizione; ci si
/// parla col tasto [T]. Può essere vivo o morto: da morto non risponde più.
/// </summary>
class Personaggio
{
    public string nome;
    public string descr;
    public bool vivo = true;
    public string battutaViva;
    public string battutaMorto;

    // Righe mostrate nell'HUD per segnalare la presenza del personaggio
    public string presenzaViva;
    public string presenzaMorto;

    public Personaggio(string nome, string descr, string battutaViva, string battutaMorto,
                       string? presenzaViva = null, string? presenzaMorto = null)
    {
        this.nome = nome;
        this.descr = descr;
        this.battutaViva = battutaViva;
        this.battutaMorto = battutaMorto;
        this.presenzaViva = presenzaViva ?? $"{nome} è qui. Premi [T] per parlare.";
        this.presenzaMorto = presenzaMorto ?? $"Il corpo senza vita di {nome} giace a terra.";
    }

    /// <summary>Riga da mostrare nell'HUD in base allo stato del personaggio.</summary>
    public string RigaPresenza() => vivo ? presenzaViva : presenzaMorto;

    public void Parla()
    {
        Console.Clear();
        Console.WriteLine($"--- {nome} ---\n");
        Console.WriteLine(vivo ? battutaViva : battutaMorto);
        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey(true);
    }
}
