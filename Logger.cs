using System;
using System.IO;

namespace Project;

/// <summary>
/// Semplice meccanismo di logging: registra gli eventi di gioco su un file
/// di testo (partita.log) con timestamp. Serve a tracciare l'andamento della
/// partita (movimenti, azioni, eventi, finali).
/// </summary>
static class Logger
{
    static readonly string percorso = Path.Combine(AppContext.BaseDirectory, "partita.log");

    /// <summary>
    /// Scrive una riga nel file di log. Il logging non deve mai interrompere
    /// il gioco: eventuali errori di I/O vengono ignorati.
    /// </summary>
    public static void Log(string messaggio)
    {
        try
        {
            string riga = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {messaggio}";
            File.AppendAllText(percorso, riga + Environment.NewLine);
        }
        catch
        {
            // Ignoriamo volutamente gli errori: il log è un supporto, non un blocco.
        }
    }
}
