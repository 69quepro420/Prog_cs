using System;
using System.IO;
using System.Text.Json;

namespace Project;

/// <summary>
/// Parametri di gioco letti a runtime da un file di configurazione esterno
/// (config.json). Se il file manca o è illeggibile, si usano i valori di
/// default definiti qui sotto.
/// </summary>
static class Config
{
    public static float pesoMassimo = 10f;
    public static int durataOssigenoSecondi = 120;
    public static int durataAutodistruzioneSecondi = 60;
    public static string nomeDefault = "Comandante";

    /// <summary>
    /// Struttura usata solo per la deserializzazione del file JSON.
    /// I campi nullable permettono di lasciare fuori dal file i parametri
    /// che si vogliono mantenere al valore di default.
    /// </summary>
    private class DatiConfig
    {
        public float? pesoMassimo { get; set; }
        public int? durataOssigenoSecondi { get; set; }
        public int? durataAutodistruzioneSecondi { get; set; }
        public string? nomeDefault { get; set; }
    }

    /// <summary>
    /// Carica i parametri dal file di configurazione e li propaga alle classi
    /// che li utilizzano. Da chiamare una volta all'avvio del gioco.
    /// </summary>
    public static void Carica()
    {
        try
        {
            string? percorso = TrovaFile();
            if (percorso == null)
            {
                Logger.Log("Config: file config.json non trovato, uso i valori di default.");
            }
            else
            {
                DatiConfig? d = JsonSerializer.Deserialize<DatiConfig>(File.ReadAllText(percorso));
                if (d != null)
                {
                    if (d.pesoMassimo.HasValue) pesoMassimo = d.pesoMassimo.Value;
                    if (d.durataOssigenoSecondi.HasValue) durataOssigenoSecondi = d.durataOssigenoSecondi.Value;
                    if (d.durataAutodistruzioneSecondi.HasValue) durataAutodistruzioneSecondi = d.durataAutodistruzioneSecondi.Value;
                    if (!string.IsNullOrWhiteSpace(d.nomeDefault)) nomeDefault = d.nomeDefault!;
                    Logger.Log($"Config caricata da {percorso}.");
                }
            }
        }
        catch (Exception e)
        {
            Logger.Log($"Config: errore di lettura ({e.Message}), uso i valori di default.");
        }

        // Propaghiamo i parametri alle classi che li usano
        Giocatore.pesoMassimo = pesoMassimo;
        EventoIA.durataSecondi = durataOssigenoSecondi;
        EventoFinale.durataSecondi = durataAutodistruzioneSecondi;
    }

    /// <summary>
    /// Cerca config.json prima accanto all'eseguibile, poi nella cartella
    /// di lavoro corrente. Ritorna null se non lo trova.
    /// </summary>
    private static string? TrovaFile()
    {
        string accantoEseguibile = Path.Combine(AppContext.BaseDirectory, "config.json");
        if (File.Exists(accantoEseguibile)) return accantoEseguibile;
        if (File.Exists("config.json")) return "config.json";
        return null;
    }
}
