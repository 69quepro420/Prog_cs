using System;

namespace Project;

/// <summary>
/// Personaggio non giocante presente in una stanza (es. un membro
/// dell'equipaggio). Ci si parla col tasto [T]. Può essere vivo o morto:
/// da morto non risponde più.
/// </summary>
class Personaggio
{
    public string nome;
    public bool vivo = true;
    public string battutaViva;
    public string battutaMorto;

    public Personaggio(string nome, string battutaViva, string battutaMorto)
    {
        this.nome = nome;
        this.battutaViva = battutaViva;
        this.battutaMorto = battutaMorto;
    }

    public void Parla()
    {
        Console.Clear();
        Console.WriteLine($"--- {nome} ---\n");
        Console.WriteLine(vivo ? battutaViva : battutaMorto);
        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey(true);
    }
}
