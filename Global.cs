namespace Project;

class Global
{
    public static Stanza[][] map = new Stanza[][]{};

    // Obiettivo del gioco: installare i componenti di ricambio nella navetta
    public const int componentiNavettaTotali = 3;
    public static int componentiNavettaInstallati = 0;
    public static bool partitaVinta = false;

    // Personaggi (posizionati in stanze casuali) e riferimenti utili al salvataggio
    public static Personaggio? ryan;
    public static Personaggio? iaOstile;
    public static Antidolorifici? antidolorifici;
    public static string stanzaRyan = "";

    /// <summary>Ritorna la stanza con il nome indicato, o null se non esiste.</summary>
    public static Stanza? TrovaStanza(string nome)
    {
        foreach (Stanza[] riga in map)
            foreach (Stanza? s in riga)
                if (s != null && s.nome == nome) return s;
        return null;
    }

    /// <summary>
    /// Posiziona i personaggi nelle stanze indicate (usato sia alla nuova
    /// partita, con stanze casuali, sia al caricamento, con le stanze salvate).
    /// Azzera prima ogni assegnazione precedente.
    /// </summary>
    public static void PosizionaPersonaggi(string nomeStanzaRyan, string nomeStanzaIA)
    {
        foreach (Stanza[] riga in map)
            foreach (Stanza? s in riga)
                if (s != null) s.personaggio = null;

        Stanza? sr = TrovaStanza(nomeStanzaRyan);
        if (sr != null && ryan != null)
        {
            sr.personaggio = ryan;
            stanzaRyan = sr.nome;
            if (antidolorifici != null) antidolorifici.stanzaUso = sr.nome; // gli antidolorifici si usano dov'è Ryan
        }

        Stanza? si = TrovaStanza(nomeStanzaIA);
        if (si != null && iaOstile != null) si.personaggio = iaOstile;
    }

    public static void Inizializza(Giocatore player)
    {
        componentiNavettaInstallati = 0;
        partitaVinta = false;

        // Mappe ASCII di ogni stanza (i puntini fanno da riempimento per l'allineamento)

        string mapSComandi = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../         >SEI QUI<         \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapCNord = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   >SEI QUI<   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapCSud = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   >SEI QUI<   |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapONord = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |>SEI QUI<| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapOSud = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |>SEI QUI<| SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapENord = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |>SEI QUI<|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapESud = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|>SEI QUI<|
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapRipostiglio = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO|  >SEI QUI<  |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapMedica = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  |  >SEI QUI<  |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapArchivio = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  >SEI QUI<  |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapOssigeno = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |  >SEI QUI<  |   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapMotoriO1 = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        |>SEI QUI<:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapMotoriO2 = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  >SEI QUI<  |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapMagazzino = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   >SEI QUI<   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapMotoriE1 = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  >SEI QUI<  : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapMotoriE2 = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   :>SEI QUI<|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapPorto = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|   >SEI QUI<   |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(               )
        ........................(    NAVETTA    )
        .........................'-------------'";

        string mapNavetta = @"
        .............................../^\
        ............................../   \
        ............................./     \
        ....................+-------/       \-------+
        .................../      SALA COMANDI       \
        ................../                           \
        +---------+-------------+---------------+-------------+---------+
        |         |             |               |             |         |
        |CORRIDOIO| RIPOSTIGLIO |   CORRIDOIO   |  ARCHIVIO   |CORRIDOIO|
        |         |             |               |             |         |
        |  -   -  +-------------+   -   -   -   +-------------+  -   -  |
        |         |             |               |             |         |
        |  OVEST  | SALA MEDICA |   CENTRALE    |SALA OSSIGENO|   EST   |
        |         |             |               |             |         |
        +---------+-------------+---------------+-------------+---------+
        |         :             |               |             :         |
        | S.MOTORI:  S.MOTORI   |   MAGAZZINO   |  S.MOTORI   : S.MOTORI|
        | OVEST 1 :   OVEST 2   |               |    EST 1    :  EST 2  |
        |         :             |               |             :         |
        +---------+-------------+---------------+-------------+---------+
        ........................|               |
        ........................|     PORTO     |
        ........................|               |
        ........................+-------+-------+
        ................................|
        ..........................-------------.
        ........................(   >SEI QUI<   )
        ........................(    NAVETTA    )
        .........................'-------------'";


        // Creazione delle stanze
        Stanza salaComandi = new Stanza("Sala Comandi", "Il ponte di comando della nave. Un'enorme vetrata mostra lo spazio siderale.", mapSComandi);

        Stanza corrOvestN = new Stanza("Corridoio Ovest (Nord)", "La parte nord del corridoio di babordo.", mapONord);
        Stanza ripostiglio = new Stanza("Ripostiglio", "Una piccola stanza piena di attrezzi per la manutenzione.", mapRipostiglio);
        Stanza corrCentraleN = new Stanza("Corridoio Centrale (Nord)", "L'arteria principale della nave.", mapCNord);
        Stanza archivio = new Stanza("Archivio", "Server spenti e faldoni digitali coprono le pareti.", mapArchivio);
        Stanza corrEstN = new Stanza("Corridoio Est (Nord)", "La parte nord del corridoio di tribordo.", mapENord);

        Stanza corrOvestS = new Stanza("Corridoio Ovest (Sud)", "La parte sud del corridoio di babordo.", mapOSud);
        Stanza salaMedica = new Stanza("Sala Medica", "Lettini d'emergenza e macchinari medici.", mapMedica);
        Stanza corrCentraleS = new Stanza("Corridoio Centrale (Sud)", "Il corridoio principale. Da qui si scende verso la stiva.", mapCSud);
        Stanza salaOssigeno = new Stanza("Sala Ossigeno", "Enormi ventole purificano l'aria della nave.", mapOssigeno);
        Stanza corrEstS = new Stanza("Corridoio Est (Sud)", "La parte sud del corridoio di tribordo.", mapESud);

        Stanza motoriOvest1 = new Stanza("Sala Motori Ovest 1", "La parte più esterna del reattore di sinistra.", mapMotoriO1);
        Stanza motoriOvest2 = new Stanza("Sala Motori Ovest 2", "La parte interna del reattore di sinistra, vicino al magazzino.", mapMotoriO2);
        Stanza magazzino = new Stanza("Magazzino", "Casse impilate l'una sull'altra bloccano parzialmente la visuale.", mapMagazzino);
        Stanza motoriEst1 = new Stanza("Sala Motori Est 1", "La parte interna del reattore di destra, vicino al magazzino.", mapMotoriE1);
        Stanza motoriEst2 = new Stanza("Sala Motori Est 2", "La parte più esterna del reattore di destra.", mapMotoriE2);

        Stanza porto = new Stanza("Porto di Sbarco", "La camera di compensazione e attracco navette.", mapPorto);
        Stanza navetta = new Stanza("Navetta", "Il veicolo con cui siete arrivati. I motori sono spenti.", mapNavetta);
        
        // Porte tra le stanze

        // Navetta - Porto
        Porta portaNavettaPorto = new Porta("Porta Navetta-Porto", Porta.StatoPorta.Aperta);
        navetta.portaNord = portaNavettaPorto;
        porto.portaSud = portaNavettaPorto;

        // Porto - Magazzino - BLOCCATA: si apre usando l'IA Tascabile nel Porto
        Porta portaPortoMagazzino = new Porta("Porta Porto-Magazzino", Porta.StatoPorta.Bloccata,
            "Accanto alla porta noti un foro USB per l'ingresso di un dispositivo.");
        porto.portaNord = portaPortoMagazzino;
        magazzino.portaSud = portaPortoMagazzino;

        // Magazzino - Sala Motori Ovest 2 (portaOvest) - BLOCCATA: si apre col Piede di Porco usato in Magazzino
        Porta portaMagazzinoMotoriOvest2 = new Porta("Porta Magazzino-Sala Motori Ovest 2", Porta.StatoPorta.Bloccata,
            "La porta elettronica si è bloccata. Forse si potrebbe forzare con l'attrezzo giusto.");
        magazzino.portaOvest = portaMagazzinoMotoriOvest2;
        motoriOvest2.portaEst = portaMagazzinoMotoriOvest2;

        // Magazzino - Sala Motori Est 1 (portaEst) - BLOCCATA: controllata dal Terminale di Magazzino
        Porta portaMagazzinoMotoriEst1 = new Porta("Porta Magazzino-Sala Motori Est 1", Porta.StatoPorta.Bloccata);
        magazzino.portaEst = portaMagazzinoMotoriEst1;
        motoriEst1.portaOvest = portaMagazzinoMotoriEst1;

        // Sala Motori Est 1 - Sala Ossigeno
        Porta portaMotoriEst1SalaOssigeno = new Porta("Porta Sala Motori Est 1-Sala Ossigeno", Porta.StatoPorta.Aperta);
        motoriEst1.portaNord = portaMotoriEst1SalaOssigeno;
        salaOssigeno.portaSud = portaMotoriEst1SalaOssigeno;

        // Sala Motori Est 2 - Corridoio Est Sud - BLOCCATA: controllata dal Terminale di Sala Motori Est 2
        Porta portaMotoriEst2CorrEstSud = new Porta("Porta Sala Motori Est 2-Corridoio Est Sud", Porta.StatoPorta.Bloccata);
        motoriEst2.portaNord = portaMotoriEst2CorrEstSud;
        corrEstS.portaSud = portaMotoriEst2CorrEstSud;

        // Corridoio Est Nord - Archivio
        Porta portaCorrEstNordArchivio = new Porta("Porta Corridoio Est Nord-Archivio", Porta.StatoPorta.Aperta);
        corrEstN.portaOvest = portaCorrEstNordArchivio;
        archivio.portaEst = portaCorrEstNordArchivio;

        // Corridoio Centrale Nord - Ripostiglio
        Porta portaCorrCentraleNRipostiglio = new Porta("Porta Corridoio Centrale Nord-Ripostiglio", Porta.StatoPorta.Aperta);
        corrCentraleN.portaOvest = portaCorrCentraleNRipostiglio;
        ripostiglio.portaEst = portaCorrCentraleNRipostiglio;

        // Ripostiglio - Corridoio Ovest Nord
        Porta portaRipostCorrOvestNord = new Porta("Porta Ripostiglio-Corridoio Ovest Nord", Porta.StatoPorta.Aperta);
        ripostiglio.portaOvest = portaRipostCorrOvestNord;
        corrOvestN.portaEst = portaRipostCorrOvestNord;

        // Corridoio Ovest Sud - Sala Medica - BLOCCATA: si apre col Martello di Emergenza usato in Corridoio Ovest Sud
        Porta portaCorrOvestSudSalaMedica = new Porta("Porta Corridoio Ovest Sud-Sala Medica", Porta.StatoPorta.Bloccata,
            "Delle lamiere contorte bloccano il passaggio. Bisognerebbe sfondarle per entrare.");
        corrOvestS.portaEst = portaCorrOvestSudSalaMedica;
        salaMedica.portaOvest = portaCorrOvestSudSalaMedica;

        // Corridoio Centrale Nord - Sala Comandi - BLOCCATA: controllata dal Terminale in Corridoio Centrale Sud
        Porta portaCorrCentraleNSalaComandi = new Porta("Porta Corridoio Centrale Nord-Sala Comandi", Porta.StatoPorta.Bloccata);
        corrCentraleN.portaNord = portaCorrCentraleNSalaComandi;
        salaComandi.portaSud = portaCorrCentraleNSalaComandi;

        // Archivio - Corridoio Centrale Nord - BLOCCATA: controllata dal Terminale di Sala Motori Ovest
        Porta portaArchivioCorriCentraleN = new Porta("Porta Archivio-Corridoio Centrale Nord", Porta.StatoPorta.Bloccata);
        archivio.portaOvest = portaArchivioCorriCentraleN;
        corrCentraleN.portaEst = portaArchivioCorriCentraleN;

        // CORRIDOI LUNGHI (5 collegamenti)
        
        // Sala Motori Ovest 1 - Sala Motori Ovest 2
        Porta portaMotoriOvest1Ovest2 = new Porta("Porta Sala Motori Ovest 1-2", Porta.StatoPorta.Aperta);
        motoriOvest1.portaEst = portaMotoriOvest1Ovest2;
        motoriOvest2.portaOvest = portaMotoriOvest1Ovest2;

        // Sala Motori Est 1 - Sala Motori Est 2
        Porta portaMotoriEst1Est2 = new Porta("Porta Sala Motori Est 1-2", Porta.StatoPorta.Aperta);
        motoriEst1.portaEst = portaMotoriEst1Est2;
        motoriEst2.portaOvest = portaMotoriEst1Est2;

        // Corridoio Est Nord - Corridoio Est Sud
        Porta portaCorrEstNordSud = new Porta("Porta Corridoio Est Nord-Sud", Porta.StatoPorta.Aperta);
        corrEstN.portaSud = portaCorrEstNordSud;
        corrEstS.portaNord = portaCorrEstNordSud;

        // Corridoio Centrale Nord - Corridoio Centrale Sud
        Porta portaCorrCentraleNordSud = new Porta("Porta Corridoio Centrale Nord-Sud", Porta.StatoPorta.Aperta);
        corrCentraleN.portaSud = portaCorrCentraleNordSud;
        corrCentraleS.portaNord = portaCorrCentraleNordSud;

        // Corridoio Ovest Nord - Corridoio Ovest Sud
        Porta portaCorrOvestNordSud = new Porta("Porta Corridoio Ovest Nord-Sud", Porta.StatoPorta.Aperta);
        corrOvestN.portaSud = portaCorrOvestNordSud;
        corrOvestS.portaNord = portaCorrOvestNordSud;

        // Casse, terminali e oggetti.
        // Le porte e le casse collegate a un terminale nascono bloccate.

        // --- OGGETTI CHIAVE (componenti della navetta: usali nella Navetta per vincere) ---
        OggettoChiave carburatoreSonico = new OggettoChiave("Turbopompa Primaria", "Un componente vitale della navetta. Vibra leggermente.", 2.5f);
        OggettoChiave iniettoreCarburante = new OggettoChiave("Turbopompa Secondaria", "Un componente della navetta. Puzza di cherosene.", 2.5f);
        OggettoChiave antimateriaNeurale = new OggettoChiave("Ugello", "Il componente terminale del propulsore della navetta.", 2.5f);

        // --- STRUMENTI (aprono una porta se usati nella stanza giusta) ---
        Strumento piedeDiPorco = new Strumento("Piede di Porco", "Una robusta leva d'acciaio. Perfetta per forzare porte.", 3f,
            "Magazzino", portaMagazzinoMotoriOvest2,
            "Infili il piede di porco nella fessura e fai leva con tutte le tue forze.");

        // Il martello pesa quanto l'intero limite di trasporto: per usarlo bisogna scartare tutto il resto
        Strumento martelloEmergenza = new Strumento("Martello di Emergenza", "Un enorme martello anti-incendio. Pesa una tonnellata: per portarlo devi avere le mani libere.", Giocatore.pesoMassimo,
            "Corridoio Ovest (Sud)", portaCorrOvestSudSalaMedica,
            "Con un colpo tremendo sfondi il pannello di blocco della porta.");

        // --- CASSE ---

        // Cassa in Sala Motori Est 1 - SBLOCCATA: interagibile senza terminale
        Cassa botolaParete = new Cassa("Botola nella parete", "Una botola di servizio semiaperta nella parete.", carburatoreSonico, Cassa.StatoCassa.Sbloccata);
        motoriEst1.lista.Add(botolaParete);

        // Cassa in Archivio - BLOCCATA: controllata dal Terminale di Archivio
        Cassa cassaArchivio = new Cassa("Cassetto pieno di scartoffie", "Un cassetto d'archivio traboccante di documenti. La serratura è elettronica.", piedeDiPorco, Cassa.StatoCassa.Bloccata);
        archivio.lista.Add(cassaArchivio);

        // Cassa in Sala Medica - BLOCCATA: controllata dal Terminale di Sala Medica.
        // Il contenuto (Antidolorifici) viene assegnato più sotto, quando esistono
        // sia il ferito Ryan sia il terminale del corridoio centrale.
        Cassa cassaMedica = new Cassa("Cassa Medica", "Un contenitore di forniture mediche sigillato elettronicamente.", null, Cassa.StatoCassa.Bloccata);
        salaMedica.lista.Add(cassaMedica);

        // Cassa in Ripostiglio - SBLOCCATA: contiene la Turbopompa Secondaria (chiave 2)
        Cassa cassaRipostiglio = new Cassa("Cassa del Ripostiglio", "Una vecchia cassa senza serratura.", iniettoreCarburante, Cassa.StatoCassa.Sbloccata);
        ripostiglio.lista.Add(cassaRipostiglio);

        // --- INDIZIO PASSWORD DEL TERMINALE DI SALA MOTORI EST 2 (in Sala Ossigeno) ---
        // La scritta sul muro disegna "H2O": H=8, 2, O=15 -> password "8215".
        string asciiH2O = @"
____    ____                  ____
`MM'    `MM'                 6MMMMb
 MM      MM                 8P    Y8
 MM      MM         ____   6M      Mb
 MM      MM        6MMMMb  MM      MM
 MMMMMMMMMM       MM'  `Mb MM      MM
 MM      MM            ,MM MM      MM
 MM      MM           ,MM' MM      MM
 MM      MM         ,M'    YM      M9
 MM      MM       ,M'       8b    d8
_MM_    _MM_      MMMMMMMM   YMMMM9
";
        Oggetto scrittaMuro = new Oggetto("Scritta sul muro vicino a un tubo", asciiH2O, 0f, false);
        salaOssigeno.lista.Add(scrittaMuro);

        Oggetto notaOssigeno = new Oggetto("Nota appuntata",
            "Ricordati che il computer di sala motori accetta soltanto numeri, quindi non " +
            "ribloccarmi il computer di nuovo perché devo lavorarci, Pier. Quante volte dovrò " +
            "ricordartelo.. P.S. se non ti ricordi la password come le ultime due volte, non " +
            "perdere tempo a disturbarmi: è scritta sul muro.", 0f, false);
        salaOssigeno.lista.Add(notaOssigeno);

        // --- OGGETTI SUL PAVIMENTO ---
        corrEstS.lista.Add(martelloEmergenza);        // Corridoio Est (Sud)
        salaComandi.lista.Add(antimateriaNeurale);    // Sala Comandi

        // Nota di Ryan nel Porto: introduce l'IA Tascabile e la meccanica delle backdoor
        Oggetto notaRyan = new Oggetto("Nota di Ryan",
@"
Io ci ho provato in ogni modo a fermarla, ma era già troppo tardi.
Gli altri non mi hanno voluto ascoltare: pensavano fossi pazzo.

Se mai arriverà qualcuno, voglio dirti che tu sei l'unica speranza.
Non per questa nave, che ormai è morta, ma per il mondo intero.

L'intelligenza artificiale della nave è impazzita: ha ucciso tutto
l'equipaggio (pace alle loro anime) e ora ha il completo controllo
della nave.

Vicino a questa nota avrai forse notato uno strano aggeggio: è
un'IA tascabile, del tutto indipendente dall'IA principale della
nave. L'ho costruita io, ingegnere informatico di bordo, per tentare
di contrastare l'IA ormai fuori controllo. È stato inutile: era già
diventata troppo potente. Ma sono certo che, nel momento del bisogno,
se le parlerai riuscirà a salvarti la vita.

Ho inserito una backdoor in molti terminali: da lì potrai prendere il
controllo manuale delle porte e farti strada nella nave attraverso la
rete locale dei computer.

Per accedere alla backdoor dovrai far combaciare i buffer: avrai tre
coppie di lettere e numeri e dovrai selezionare il buffer prima dalla
riga, poi dalla colonna corrispondente e infine di nuovo dalla riga.
Con un po' di pratica ci riuscirai.

Non deludermi. Non lasciare che altre navi facciano la fine di questa.
Credo in te.

                                                              - Ryan", 0f, false);
        porto.lista.Add(notaRyan);

        // IA amichevole nel Porto: usata lì, sblocca la porta verso il Magazzino
        porto.lista.Add(new IATascabile(portaPortoMagazzino, "Porto di Sbarco"));

        // --- TERMINALI ---

        // TERMINALE MAGAZZINO: CRIPTATO -> porta Magazzino-Sala Motori Est 1
        Terminale terminaleMagazzino = new Terminale("Terminale di Magazzino", "Un terminale logistico. Il sistema è criptato e richiede una violazione.", StatoTerminale.Criptato, "");
        terminaleMagazzino.logs.Add("PLACEHOLDER");
        terminaleMagazzino.porteControllate.Add(portaMagazzinoMotoriEst1);
        magazzino.lista.Add(terminaleMagazzino);

        // TERMINALE SALA MOTORI EST 2: BLOCCATO (password "8215": H=8, 2, O=15, dalla
        // scritta "H2O" sul muro della Sala Ossigeno) -> porta Sala Motori Est 2-Corridoio Est Sud
        Terminale terminaleMotoriEst2 = new Terminale("Terminale di Sala Motori Est 2", "Un terminale di manutenzione bloccato da password.", StatoTerminale.Bloccato, "8215");
        terminaleMotoriEst2.logs.Add("PLACEHOLDER");
        terminaleMotoriEst2.porteControllate.Add(portaMotoriEst2CorrEstSud);
        motoriEst2.lista.Add(terminaleMotoriEst2);

        // TERMINALE SALA OSSIGENO: BLOCCATO (password "1234") -> sistema di ripristino ossigeno
        Terminale terminaleOssigeno = new Terminale("Terminale di Sala Ossigeno", "Il quadro dei comandi vitali, bloccato da password.", StatoTerminale.Bloccato, "1234");
        terminaleOssigeno.logs.Add("PLACEHOLDER");
        terminaleOssigeno.sistemaOssigeno = true;
        salaOssigeno.lista.Add(terminaleOssigeno);

        // TERMINALE ARCHIVIO: CRIPTATO -> cassa dell'Archivio
        Terminale terminaleArchivio = new Terminale("Terminale di Archivio", "Un terminale dati protetto da crittografia.", StatoTerminale.Criptato, "");
        terminaleArchivio.logs.Add("PLACEHOLDER");
        terminaleArchivio.casseControllate.Add(cassaArchivio);
        archivio.lista.Add(terminaleArchivio);

        // TERMINALE CORRIDOIO CENTRALE SUD: BLOCCATO (password "9832", rivelata solo da Ryan)
        // -> porta Corridoio Centrale Nord-Sala Comandi
        Terminale terminaleCorrCentraleS = new Terminale("Terminale di Corridoio Centrale", "Un terminale di sicurezza incassato nella parete, bloccato da password.", StatoTerminale.Bloccato, "9832");
        terminaleCorrCentraleS.logs.Add("PLACEHOLDER");
        terminaleCorrCentraleS.porteControllate.Add(portaCorrCentraleNSalaComandi);
        corrCentraleS.lista.Add(terminaleCorrCentraleS);

        // TERMINALE SALA MOTORI OVEST: BLOCCATO (password "1234") -> porta Archivio-Corridoio Centrale Nord
        Terminale terminaleMotoriOvest = new Terminale("Terminale di Sala Motori Ovest", "Un terminale di manutenzione impolverato, bloccato da password.", StatoTerminale.Bloccato, "1234");
        terminaleMotoriOvest.logs.Add("PLACEHOLDER");
        terminaleMotoriOvest.porteControllate.Add(portaArchivioCorriCentraleN);
        motoriOvest1.lista.Add(terminaleMotoriOvest);

        // TERMINALE SALA MEDICA: CRIPTATO -> cassa Medica
        Terminale terminaleMedica = new Terminale("Terminale di Sala Medica", "Un terminale medico protetto da crittografia.", StatoTerminale.Criptato, "");
        terminaleMedica.logs.Add("PLACEHOLDER");
        terminaleMedica.casseControllate.Add(cassaMedica);
        salaMedica.lista.Add(terminaleMedica);

        // --- RYAN: membro dell'equipaggio ferito (NPC "buono", stanza casuale) ---
        ryan = new Personaggio("Ryan",
            "Un membro dell'equipaggio, ferito e allo stremo delle forze.",
            "\"Aaah... la ferita... fa un male cane... ti prego... trova qualcosa per il dolore...\" (PLACEHOLDER)",
            $"Ti inginocchi accanto al corpo di Ryan. Ripensi alle sue ultime parole: la password del terminale del corridoio centrale era {terminaleCorrCentraleS.password}.",
            "Ryan giace ferito a terra. Premi [T] per parlargli.");

        // Gli Antidolorifici nella cassa medica: somministrati a Ryan rivelano
        // la password del terminale del corridoio centrale, poi lui muore.
        // La stanza d'uso viene impostata al piazzamento casuale di Ryan.
        antidolorifici = new Antidolorifici(ryan, terminaleCorrCentraleS, "");
        cassaMedica.contenuto = antidolorifici;

        // --- IA OSTILE: NPC "cattivo", piazzato nella stanza casuale dell'evento ---
        iaOstile = new Personaggio("IA Ostile",
            "L'intelligenza artificiale che ha preso il controllo della stazione. Fredda e onnipresente.",
            "\"Sei ancora qui? Patetico. Nulla può fermare ciò che ho iniziato.\" (PLACEHOLDER)",
            "",
            "Un occhio rosso ti scruta da ogni telecamera: l'IA Ostile è qui. Premi [T] per parlare.");

        // Griglia logica della mappa (6 righe, 5 colonne)
        map = new Stanza[][] {
            /* Riga 0 */ new Stanza[] { null, null, salaComandi, null, null },
            /* Riga 1 */ new Stanza[] { corrOvestN, ripostiglio, corrCentraleN, archivio, corrEstN },
            /* Riga 2 */ new Stanza[] { corrOvestS, salaMedica, corrCentraleS, salaOssigeno, corrEstS },
            /* Riga 3 */ new Stanza[] { motoriOvest1, motoriOvest2, magazzino, motoriEst1, motoriEst2 },
            /* Riga 4 */ new Stanza[] { null, null, porto, null, null },
            /* Riga 5 */ new Stanza[] { null, null, navetta, null, null }
        };

        // Oggetto iniziale nell'inventario
        player.inventario.Push(new Oggetto("Chiave Inglese", "Pesante e arrugginita.", 1.5f, true));

        // Partiamo dalla navetta (Riga 5, Colonna 2)
        player.coordinate = new int[] { 5, 2 };
        player.stanza = navetta;

        // Scelta della stanza casuale per l'evento dell'IA ostile
        // (va fatta DOPO la costruzione della mappa)
        EventoIA.Prepara();

        // Reset dello scontro finale in Sala Comandi
        EventoFinale.Reset();

        // Piazzamento casuale dei personaggi:
        // - IA Ostile nella stanza dell'evento
        // - Ryan in una stanza casuale diversa da Navetta, Porto, Sala Comandi
        //   e dalla stanza dell'IA (una stanza ospita un solo personaggio)
        string[] escluseRyan = { "Navetta", "Porto di Sbarco", "Sala Comandi", EventoIA.stanzaEvento };
        List<Stanza> candidateRyan = new();
        foreach (Stanza[] riga in map)
            foreach (Stanza? s in riga)
                if (s != null && Array.IndexOf(escluseRyan, s.nome) < 0)
                    candidateRyan.Add(s);

        string nomeStanzaRyan = candidateRyan[Random.Shared.Next(candidateRyan.Count)].nome;
        PosizionaPersonaggi(nomeStanzaRyan, EventoIA.stanzaEvento);
    }
}
