namespace Project;

class Program
{
    static void Main(string[] args)
    {
        // 1. Avvia la cutscene iniziale
        AnimazioneIntro.Gioca();

        // 2. Inizializza il giocatore
        Giocatore player = new Giocatore("Comandante");

        // 3. Genera il mondo e posiziona il giocatore
        Global.Inizializza(player);

        // 4. Avvia il ciclo di input principale (HUD)
        Comando.Start(player);
    }
}
