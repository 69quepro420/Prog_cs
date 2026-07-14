namespace Project;

class Global
{
    public static Stanza[][] map = new Stanza[][]{};

    public static void Inizializza(Giocatore player)
    {
        // ====================================================================
        // 1. MAPPE CON CARATTERI FILLER (PUNTINI STELLARI) PER BLOCCARE L'ALLINEAMENTO
        // ====================================================================

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


        // ====================================================================
        // 2. CREAZIONE DELLE STANZE CON LA MAPPA CORRISPONDENTE
        // ====================================================================
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
        
        // === AGGIUNTA DEI TERMINALI - TEST DI TUTTI E TRE GLI STATI ===
        
        // TERMINALE 1: BLOCCATO (richiede password "1234")
        Terminale terminaleMagazzino = new Terminale("Terminale Logistico", "Un terminale bloccato da password.", StatoTerminale.Bloccato, "1234");
        terminaleMagazzino.logs.Add("MEMO: Smettetela di usare '1234' come password per il magazzino!");
        terminaleMagazzino.logs.Add("AVVISO: Accesso limitato - Verificare credenziali.");
        magazzino.lista.Add(terminaleMagazzino);

        // TERMINALE 2: CRIPTATO (richiede il minigioco Breach Protocol)
        Terminale terminaleOssigeno = new Terminale("Terminale Ambientale", "Il quadro dei comandi vitale. Il sistema è criptato e richiede una violazione.", StatoTerminale.Criptato, "");
        terminaleOssigeno.logs.Add("AVVISO SISTEMA: Rilevato blocco porte.");
        terminaleOssigeno.logs.Add("AUTORITÀ: Protezione crittografica attivata.");
        terminaleOssigeno.logs.Add("NOTA: Tempo limite di vulnerabilità: 30 secondi.");
        salaOssigeno.lista.Add(terminaleOssigeno);

        // TERMINALE 3: SBLOCCATO (accesso immediato)
        Terminale terminaleArchivio = new Terminale("Terminale Archivi", "Un vecchio terminale già sbloccato dal precedente operatore.", StatoTerminale.Sbloccato, "");
        terminaleArchivio.logs.Add("ULTIMO ACCESSO: 72 ore fa");
        terminaleArchivio.logs.Add("STATO: Sistema operativo attivo");
        terminaleArchivio.logs.Add("DATABASE: Dati di navigazione, coordinate stellari, rapporti storici");
        terminaleArchivio.logs.Add("AVVISO: Backup programmato per domani 06:00");
        archivio.lista.Add(terminaleArchivio);

        // ====================================================================
        // 3. CONFIGURAZIONE DELLE PORTE - SECONDO LO SCHEMA SPECIFICATO
        // ====================================================================
        
        // SCHEMA COLLEGAMENTI STANZE:
        // ✅ 12 Collegamenti tra Stanze Normali:
        //    - Navetta ↔ Porto
        //    - Porto ↔ Magazzino
        //    - Magazzino ↔ Sala Motori Ovest (portaOvest)
        //    - Magazzino ↔ Sala Motori Est (portaEst)
        //    - Sala Motori Est 1 ↔ Sala Ossigeno
        //    - Sala Motori Est 2 ↔ Corridoio Est Sud
        //    - Corridoio Est Nord ↔ Archivio
        //    - Corridoio Centrale Nord ↔ Ripostiglio
        //    - Ripostiglio ↔ Corridoio Ovest Nord
        //    - Corridoio Ovest Sud ↔ Sala Medica
        //    - Corridoio Centrale Nord ↔ Sala Comandi
        //    - Archivio ↔ Corridoio Centrale Nord
        //
        // ✅ 5 Collegamenti tra Stanze Lunghe:
        //    - Sala Motori Ovest 1 ↔ Sala Motori Ovest 2
        //    - Sala Motori Est 1 ↔ Sala Motori Est 2
        //    - Corridoio Est Nord ↔ Corridoio Est Sud
        //    - Corridoio Centrale Nord ↔ Corridoio Centrale Sud
        //    - Corridoio Ovest Nord ↔ Corridoio Ovest Sud

        // Navetta ↔ Porto
        Porta portaNavettaPorto = new Porta("Porta Navetta-Porto", Porta.StatoPorta.Aperta);
        navetta.portaNord = portaNavettaPorto;
        porto.portaSud = portaNavettaPorto;

        // Porto ↔ Magazzino
        Porta portaPortoMagazzino = new Porta("Porta Porto-Magazzino", Porta.StatoPorta.Aperta);
        porto.portaNord = portaPortoMagazzino;
        magazzino.portaSud = portaPortoMagazzino;

        // Magazzino ↔ Sala Motori Ovest 2 (portaOvest)
        Porta portaMagazzinoMotoriOvest2 = new Porta("Porta Magazzino-Sala Motori Ovest 2", Porta.StatoPorta.Aperta);
        magazzino.portaOvest = portaMagazzinoMotoriOvest2;
        motoriOvest2.portaEst = portaMagazzinoMotoriOvest2;

        // Magazzino ↔ Sala Motori Est 1 (portaEst)
        Porta portaMagazzinoMotoriEst1 = new Porta("Porta Magazzino-Sala Motori Est 1", Porta.StatoPorta.Aperta);
        magazzino.portaEst = portaMagazzinoMotoriEst1;
        motoriEst1.portaOvest = portaMagazzinoMotoriEst1;

        // Sala Motori Est 1 ↔ Sala Ossigeno
        Porta portaMotoriEst1SalaOssigeno = new Porta("Porta Sala Motori Est 1-Sala Ossigeno", Porta.StatoPorta.Aperta);
        motoriEst1.portaNord = portaMotoriEst1SalaOssigeno;
        salaOssigeno.portaSud = portaMotoriEst1SalaOssigeno;

        // Sala Motori Est 2 ↔ Corridoio Est Sud
        Porta portaMotoriEst2CorrEstSud = new Porta("Porta Sala Motori Est 2-Corridoio Est Sud", Porta.StatoPorta.Aperta);
        motoriEst2.portaNord = portaMotoriEst2CorrEstSud;
        corrEstS.portaSud = portaMotoriEst2CorrEstSud;

        // Corridoio Est Nord ↔ Archivio
        Porta portaCorrEstNordArchivio = new Porta("Porta Corridoio Est Nord-Archivio", Porta.StatoPorta.Aperta);
        corrEstN.portaOvest = portaCorrEstNordArchivio;
        archivio.portaEst = portaCorrEstNordArchivio;

        // Corridoio Centrale Nord ↔ Ripostiglio
        Porta portaCorrCentraleNRipostiglio = new Porta("Porta Corridoio Centrale Nord-Ripostiglio", Porta.StatoPorta.Aperta);
        corrCentraleN.portaOvest = portaCorrCentraleNRipostiglio;
        ripostiglio.portaEst = portaCorrCentraleNRipostiglio;

        // Ripostiglio ↔ Corridoio Ovest Nord
        Porta portaRipostCorrOvestNord = new Porta("Porta Ripostiglio-Corridoio Ovest Nord", Porta.StatoPorta.Aperta);
        ripostiglio.portaOvest = portaRipostCorrOvestNord;
        corrOvestN.portaEst = portaRipostCorrOvestNord;

        // Corridoio Ovest Sud ↔ Sala Medica
        Porta portaCorrOvestSudSalaMedica = new Porta("Porta Corridoio Ovest Sud-Sala Medica", Porta.StatoPorta.Aperta);
        corrOvestS.portaEst = portaCorrOvestSudSalaMedica;
        salaMedica.portaOvest = portaCorrOvestSudSalaMedica;

        // Corridoio Centrale Nord ↔ Sala Comandi
        Porta portaCorrCentraleNSalaComandi = new Porta("Porta Corridoio Centrale Nord-Sala Comandi", Porta.StatoPorta.Aperta);
        corrCentraleN.portaNord = portaCorrCentraleNSalaComandi;
        salaComandi.portaSud = portaCorrCentraleNSalaComandi;

        // Archivio ↔ Corridoio Centrale Nord
        Porta portaArchivioCorriCentraleN = new Porta("Porta Archivio-Corridoio Centrale Nord", Porta.StatoPorta.Aperta);
        archivio.portaOvest = portaArchivioCorriCentraleN;
        corrCentraleN.portaEst = portaArchivioCorriCentraleN;

        // CORRIDOI LUNGHI (5 collegamenti)
        
        // Sala Motori Ovest 1 ↔ Sala Motori Ovest 2
        Porta portaMotoriOvest1Ovest2 = new Porta("Porta Sala Motori Ovest 1-2", Porta.StatoPorta.Aperta);
        motoriOvest1.portaEst = portaMotoriOvest1Ovest2;
        motoriOvest2.portaOvest = portaMotoriOvest1Ovest2;

        // Sala Motori Est 1 ↔ Sala Motori Est 2
        Porta portaMotoriEst1Est2 = new Porta("Porta Sala Motori Est 1-2", Porta.StatoPorta.Aperta);
        motoriEst1.portaEst = portaMotoriEst1Est2;
        motoriEst2.portaOvest = portaMotoriEst1Est2;

        // Corridoio Est Nord ↔ Corridoio Est Sud
        Porta portaCorrEstNordSud = new Porta("Porta Corridoio Est Nord-Sud", Porta.StatoPorta.Aperta);
        corrEstN.portaSud = portaCorrEstNordSud;
        corrEstS.portaNord = portaCorrEstNordSud;

        // Corridoio Centrale Nord ↔ Corridoio Centrale Sud
        Porta portaCorrCentraleNordSud = new Porta("Porta Corridoio Centrale Nord-Sud", Porta.StatoPorta.Aperta);
        corrCentraleN.portaSud = portaCorrCentraleNordSud;
        corrCentraleS.portaNord = portaCorrCentraleNordSud;

        // Corridoio Ovest Nord ↔ Corridoio Ovest Sud
        Porta portaCorrOvestNordSud = new Porta("Porta Corridoio Ovest Nord-Sud", Porta.StatoPorta.Aperta);
        corrOvestN.portaSud = portaCorrOvestNordSud;
        corrOvestS.portaNord = portaCorrOvestNordSud;

        // ====================================================================
        // 4. INSERIMENTO NELLA GRIGLIA LOGICA (6 Righe, 5 Colonne)
        // ====================================================================
        map = new Stanza[][] {
            /* Riga 0 */ new Stanza[] { null, null, salaComandi, null, null },
            /* Riga 1 */ new Stanza[] { corrOvestN, ripostiglio, corrCentraleN, archivio, corrEstN },
            /* Riga 2 */ new Stanza[] { corrOvestS, salaMedica, corrCentraleS, salaOssigeno, corrEstS },
            /* Riga 3 */ new Stanza[] { motoriOvest1, motoriOvest2, magazzino, motoriEst1, motoriEst2 },
            /* Riga 4 */ new Stanza[] { null, null, porto, null, null },
            /* Riga 5 */ new Stanza[] { null, null, navetta, null, null }
        };

        // Oggetto iniziale di test
        player.inventario.Push(new Oggetto("Chiave Inglese", "Pesante e arrugginita.", 1.5f, true));

        // Partiamo dalla navetta (Riga 5, Colonna 2)
        player.coordinate = new int[] { 5, 2 };
        player.stanza = navetta;
    }
}
