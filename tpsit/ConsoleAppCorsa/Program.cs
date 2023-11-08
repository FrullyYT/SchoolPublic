using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using System.Windows.Input;

namespace ConsoleAppCorsa
{
    internal class Program
    {
        //le posizioni dei cursori che permettono di spampare Andrea Baldo e Carlo sempre più lontani dalla posizione standard del cursore
        static int posAndrea = 0;
        static int posBaldo = 0;
        static int posCarlo = 0;
        static int classifica = 0;
        static string primo = "";
        static string comando = "";

        static Thread thAndrea;
        static Thread thBaldo; //inizializza i vari Thread
        static Thread thCarlo;

        static Object _lock = new object(); //il lock permette di stampare Andrea, Baldo e Carlo Non utilizzando risorse comuni

        static void Pronti() //Stampa Andrea Baldo e Carlo a console su diverse righe posizionandoli spaziati 0
        {
            Scrivi(0, 2, "Andrea");
            Scrivi(0, 3, "  []");
            Scrivi(0, 4, @" /▓▓\");
            Scrivi(0, 5, @"  /\");
            Scrivi(0, 6, "Baldo");
            Scrivi(0, 7, "  ()");       //Stampa Andrea Baldo e Carlo a console su diverse righe posizionandoli spaziati 0
            Scrivi(0, 8, @" /▒▒\");
            Scrivi(0, 9, @"  └└");
            Scrivi(0, 10, "Carlo");
            Scrivi(0, 11, "  <>");
            Scrivi(0, 12, @" /██\");
            Scrivi(0, 13, @"  ||");

            Scrivi(20, 14, "Premere ENTER per iniziare");

            while (true) // il ciclo infinito si interrompe in una condizione dentro a esso
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(); //legge se l'input da tastiera è enter

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Scrivi(20, 14, "                                           ");//quando enter viene premuto pulisce la scritta di inizio
                    break;
                }
            }
        }

        static void Menu(int i)
        {
            lock (_lock)
            {
                switch (i) //tramite una codifica custom di int capisce quale menu deve spampare, inerente a Andrea o Baldo o Carlo o il menù iniziale
                {
                    case 0:
                        Scrivi(3, 16, "MENU'                                            ");
                        Scrivi(3, 17, "                                                 ");
                        Scrivi(3, 18, "Andrea (A)                                       ");
                        Scrivi(3, 19, "Baldo  (B)                                       ");
                        Scrivi(3, 20, "Carlo  (C)                                       ");
                        Scrivi(3, 21, "                                                                                                 ");
                        Scrivi(3, 22, "                                                                                                 ");
                        break;
                    case 1: //Andrea
                        Scrivi(3, 16, "MENU'                        AZIONE SU Andrea");
                        Scrivi(3, 17, "                                      ");
                        Scrivi(3, 18, "Andrea (A)                   Sospendere  (S)");
                        Scrivi(3, 19, "Baldo  (B)                   Riprendere  (R)");
                        Scrivi(3, 20, "Carlo (C)                    Abort       (A)");
                        Scrivi(3, 21, "                             Join        (J)");
                        break;
                    case 2: //Baldo
                        Scrivi(3, 16, "MENU'                        AZIONE SU Baldo");
                        Scrivi(3, 17, "                                      ");
                        Scrivi(3, 18, "Andrea (A)                   Sospendere  (S)");
                        Scrivi(3, 19, "Baldo  (B)                   Riprendere  (R)");
                        Scrivi(3, 20, "Carlo (C)                    Abort       (A)");
                        Scrivi(3, 21, "                             Join        (J)");
                        break;
                    case 3: //Carlo
                        Scrivi(3, 16, "MENU'                        AZIONE SU Carlo");
                        Scrivi(3, 17, "                                      ");
                        Scrivi(3, 18, "Andrea (A)                   Sospendere  (S)");
                        Scrivi(3, 19, "Baldo  (B)                   Riprendere  (R)");
                        Scrivi(3, 20, "Carlo (C)                    Abort       (A)");
                        Scrivi(3, 21, "                             Join        (J)");
                        break;
                    default:
                        break;
                }
            }
        }

        static void SceltaMenu1()
        {
            char sceltaMenu = ReadKey().KeyChar; //assegna il carattere premuto

            switch (sceltaMenu) //verifica che il carattere premuto sia fra le scelte del menù dei giocatori
            {
                case 'a':
                    Menu(1);
                    sceltaMenu = ReadKey().KeyChar; //dopo aver stampato il menu nuovo, inerente ad andrea, richiede nuovamente il carattere
                    switch (sceltaMenu) //verifica il nuovo carattere che sia fra le scelte del nuovo menu
                    {
                        case 's':
                            thAndrea.Suspend();
                            break;
                        case 'r':
                            if (thAndrea.ThreadState == ThreadState.Suspended) //prima di riprendere verifica che non sia ne join ne abort
                            {
                                thAndrea.Resume();
                            }
                            break;
                        case 'a':
                            if (thAndrea.ThreadState == ThreadState.Suspended) //verifica se il thread è sospeso
                            {
                                thAndrea.Resume(); //se è sospeso lo risprende e po aborta se no da errore
                                thAndrea.Abort();
                            }
                            else
                            {
                                thAndrea.Abort();
                            }
                            break;
                        case 'j':
                            Scrivi(3, 21, "                             Join        (J)        Baldo (B)");
                            Scrivi(3, 22, "                                                    Carlo (C)");
                            sceltaMenu = ReadKey().KeyChar; //richiede un altro carattere per definire quale persona aspettare
                            switch (sceltaMenu)
                            {
                                case 'b':
                                    comando = "AJB"; //definisce una codifica custom
                                    break;
                                case 'c':
                                    comando = "AJC";
                                    break;
                                default:
                                    break;
                            }
                            if (thAndrea.ThreadState == ThreadState.Suspended) //se joina mentre è in suspend lo riprende e poi joina, se non non funziona il join
                            {
                                thAndrea.Resume();
                            }
                            break;
                        default:
                            break;
                    }
                    Menu(0);
                    break;
                case 'b':
                    Menu(2);
                    sceltaMenu = ReadKey().KeyChar; //dopo aver stampato il menu nuovo, inerente ad andrea, richiede nuovamente il carattere
                    switch (sceltaMenu) //verifica il nuovo carattere che sia fra le scelte del nuovo menu
                    {
                        case 's':
                            thBaldo.Suspend();
                            break;
                        case 'r':
                            if (thBaldo.ThreadState == ThreadState.Suspended) //prima di riprendere verifica che non sia ne join ne abort
                            {
                                thBaldo.Resume();
                            }
                            break;
                        case 'a':
                            if (thBaldo.ThreadState == ThreadState.Suspended) //verifica se il thread è sospeso
                            {
                                thBaldo.Resume(); //se è sospeso lo risprende e po aborta se no da errore
                                thBaldo.Abort();
                            }
                            else
                            {
                                thBaldo.Abort();
                            }
                            break;
                        case 'j':
                            Scrivi(3, 21, "                             Join        (J)        Andrea (A)");
                            Scrivi(3, 22, "                                                    Carlo (C)");
                            sceltaMenu = ReadKey().KeyChar; //richiede un altro carattere per definire quale persona aspettare
                            switch (sceltaMenu)
                            {
                                case 'a':
                                    comando = "BJA"; //definisce una codifica custom
                                    break;
                                case 'c':
                                    comando = "BJC";
                                    break;
                                default:
                                    break;
                            }
                            if (thBaldo.ThreadState == ThreadState.Suspended) //se joina mentre è in suspend lo riprende e poi joina, se non non funziona il join
                            {
                                thBaldo.Resume();
                            }
                            break;
                        default:
                            break;
                    }
                    Menu(0);
                    break;
                case 'c':
                    Menu(3);
                    sceltaMenu = ReadKey().KeyChar; //dopo aver stampato il menu nuovo, inerente ad andrea, richiede nuovamente il carattere
                    switch (sceltaMenu) //verifica il nuovo carattere che sia fra le scelte del nuovo menu
                    {
                        case 's':
                            thCarlo.Suspend();
                            break;
                        case 'r':
                            if (thCarlo.ThreadState == ThreadState.Suspended) //prima di riprendere verifica che non sia ne join ne abort
                            {
                                thCarlo.Resume();
                            }
                            break;
                        case 'a':
                            if (thCarlo.ThreadState == ThreadState.Suspended) //verifica se il thread è sospeso
                            {
                                thCarlo.Resume(); //se è sospeso lo risprende e po aborta se no da errore
                                thCarlo.Abort();
                            }
                            else
                            {
                                thCarlo.Abort();
                            }
                            break;
                        case 'j':
                            Scrivi(3, 21, "                             Join        (J)        Andrea (A)");
                            Scrivi(3, 22, "                                                    Baldo (B)");
                            sceltaMenu = ReadKey().KeyChar; //richiede un altro carattere per definire quale persona aspettare
                            switch (sceltaMenu)
                            {
                                case 'a':
                                    comando = "CJA"; //definisce una codifica custom
                                    break;
                                case 'b':
                                    comando = "CJB";
                                    break;
                                default:
                                    break;
                            }
                            if (thCarlo.ThreadState == ThreadState.Suspended) //se joina mentre è in suspend lo riprende e poi joina, se non non funziona il join
                            {
                                thCarlo.Resume();
                            }
                            break;
                        default:
                            break;
                    }
                    Menu(0);
                    break;
                default:
                    Menu(0);
                    break;
            }
        }

        static void Main(string[] args)
        {
            thAndrea = new Thread(Andrea);
            thBaldo = new Thread(Baldo); //si assegnano i corridori
            thCarlo = new Thread(Carlo);

            Title = "Giulio Zangheri 4H 2023/10/05";

            CursorVisible = false;

            Pronti();

            Menu(0);

            thBaldo.Start();
            thAndrea.Start(); //fa iniziare tutti quanti
            thCarlo.Start();

            do
            {
                Stato();

                if (Console.KeyAvailable)
                    SceltaMenu1();
            } while (thAndrea.IsAlive || thBaldo.IsAlive || thCarlo.IsAlive);
            Stato();

            Vittoria();

            Console.ReadLine();
        }

        static void Stato()
        {
            Scrivi(0, 2, "Andrea -> " + thAndrea.ThreadState + "                        ");
            Scrivi(50, 2, "Is alive = " + thAndrea.IsAlive + "                        ");
            Scrivi(0, 6, "Baldo -> " + thBaldo.ThreadState + "                        "); //scrive i vari stati di Andrea, Baldo e Carlo
            Scrivi(50, 6, "Is alive = " + thBaldo.IsAlive + "                        "); // tanti spazi servono per cancellare eventuali scritte rimaste
            Scrivi(0, 10, "Carlo -> " + thCarlo.ThreadState + "                        ");
            Scrivi(50, 10, "Is alive = " + thCarlo.IsAlive + "                        ");
        }

        static void Scrivi(int col, int rig, string st)
        {
            lock (_lock)
            {
                SetCursorPosition(col, rig); //assegna il cursore
                Write(@st); //scrive la stringa in input dal sottoprogramma
            }
        }

        static void Andrea() //Muove Andrea ogni 150ms
        {
            do
            {
                if (comando.Length == 3) //join
                {
                    if ((comando[1] == 'J') && (comando[0] == 'A'))
                    {
                        switch (comando[2])
                        {
                            case 'B':
                                thBaldo.Join();
                                break;
                            case 'C':
                                thCarlo.Join();
                                break;
                            default:
                                break;
                        }
                    }
                }
                posAndrea++;
                Thread.Sleep(150); //Velocità di Andrea
                lock (_lock) //il lock utilizza la risorsa e appena viene liberato, la risorsa viene utilizzata da un altro lock
                {
                    Scrivi(posAndrea, 4, "  []");
                    Scrivi(posAndrea, 5, @" /▓▓\");
                    Scrivi(posAndrea, 6, @"  /\");
                }
            } while (posAndrea < 115);

            lock (_lock) //dentro a un altro lock si aumenta la classifica per scrivere l'ordine di arrivo
            {
                classifica++;
                Scrivi(115, 2, $"{classifica}");
                if (classifica == 1) //controlla se Andrea è il primo, nel caso, alla fine stampa il suo nome
                    primo = "Andrea";
            }
        }
        static void Baldo() //Muove Baldo ogni 150ms
        {
            do
            {
                if (comando.Length == 3) //join
                {
                    if ((comando[1] == 'J') && (comando[0] == 'B'))
                    {
                        switch (comando[2])
                        {
                            case 'a':
                                thAndrea.Join();
                                break;
                            case 'C':
                                thCarlo.Join();
                                break;
                            default:
                                break;
                        }
                    }
                }
                posBaldo++;
                Thread.Sleep(150); //Velocità di Baldo
                lock (_lock) //il lock utilizza la risorsa e appena viene liberato, la risorsa viene utilizzata da un altro lock
                {
                    Scrivi(posBaldo, 7, "  ()");
                    Scrivi(posBaldo, 8, @" /▒▒\");
                    Scrivi(posBaldo, 9, @"  └└");
                }
            } while (posBaldo < 115);

            lock (_lock) //dentro a un altro lock si aumenta la classifica per scrivere l'ordine di arrivo
            {
                classifica++;
                Scrivi(115, 6, $"{classifica}");
                if (classifica == 1)//controlla se Baldo è il primo, nel caso, alla fine stampa il suo nome
                    primo = "Baldo";
            }
        }
        static void Carlo() //Muove Carlo ogni 150ms
        {
            do
            {
                if (comando.Length == 3) //join
                {
                    if ((comando[1] == 'J') && (comando[0] == 'C'))
                    {
                        switch (comando[2])
                        {
                            case 'A':
                                thAndrea.Join();
                                break;
                            case 'B':
                                thBaldo.Join();
                                break;
                            default:
                                break;
                        }
                    }
                }
                posCarlo++;
                Thread.Sleep(150); //Velocità di Carlo
                lock (_lock) //il lock utilizza la risorsa e appena viene liberato, la risorsa viene utilizzata da un altro lock
                {
                    Scrivi(posCarlo, 11, "  <>");
                    Scrivi(posCarlo, 12, @" /██\");
                    Scrivi(posCarlo, 13, @"  ||");
                }
                
            } while (posCarlo < 115);

            lock (_lock) //dentro a un altro lock si aumenta la classifica per scrivere l'ordine di arrivo
            {
                classifica++;
                Scrivi(115, 10, $"{classifica}");
                if (classifica == 1)//controlla se Carlo è il primo, nel caso, alla fine stampa il suo nome
                    primo = "Carlo";
            }
        }

        static void Vittoria() //stampa il vincitore in ascii
        {
            int cursor = 0;
            switch (primo) //capisce chi ha vinto
            {
                case "Andrea":
                    cursor = (Console.WindowWidth - 107) / 2; // cambia il cursore in base alla lunghezza della scritta di chi 
                    break;                                    // ha vinto per avere la scritta ascii sempre al centro
                case "Baldo":
                    cursor = (Console.WindowWidth - 97) / 2;
                    break;
                case "Carlo":
                    cursor = (Console.WindowWidth - 98) / 2;
                    break;
                default:
                    break;
            }
            int totDist = 54 + 1 + cursor; //stampa vittoria
            lock (_lock)
            {
                Scrivi(cursor, 22, "  _    _           __      _______ _   _ _______ ____  ");
                Scrivi(cursor, 23, " | |  | |   /\\     \\ \\    / /_   _| \\ | |__   __/ __ \\ ");
                Scrivi(cursor, 24, " | |__| |  /  \\     \\ \\  / /  | | |  \\| |  | | | |  | |"); //54
                Scrivi(cursor, 25, " |  __  | / /\\ \\     \\ \\/ /   | | | . ` |  | | | |  | |");
                Scrivi(cursor, 26, " | |  | |/ ____ \\     \\  /   _| |_| |\\  |  | | | |__| |");
                Scrivi(cursor, 27, " |_|  |_/_/    \\_\\     \\/   |_____|_| \\_|  |_|  \\____/ ");
            }

            switch (primo) //Sceglie il vincitore
            {
                case "Andrea": //stampa Andrea
                    lock (_lock)//per farlo stampare tutto insieme e non spezzato
                    {
                        Scrivi(totDist, 22, "             _   _ _____  _____  ______            _ ");
                        Scrivi(totDist, 23, "       /\\   | \\ | |  __ \\|  __ \\|  ____|   /\\     | |");
                        Scrivi(totDist, 24, "      /  \\  |  \\| | |  | | |__) | |__     /  \\    | |");
                        Scrivi(totDist, 25, "     / /\\ \\ | . ` | |  | |  _  /|  __|   / /\\ \\   | |");
                        Scrivi(totDist, 26, "    / ____ \\| |\\  | |__| | | \\ \\| |____ / ____ \\  |_|");
                        Scrivi(totDist, 27, "   /_/    \\_\\_| \\_|_____/|_|  \\_\\______/_/    \\_\\ (_)");
                    }
                    break;
                case "Baldo": //stampa Baldo
                    lock (_lock)//per farlo stampare tutto insieme e non spezzato
                    {
                        Scrivi(totDist, 22, "    ____          _      _____   ____    _ ");
                        Scrivi(totDist, 23, "   |  _ \\   /\\   | |    |  __ \\ / __ \\  | |");
                        Scrivi(totDist, 24, "   | |_) | /  \\  | |    | |  | | |  | | | |");
                        Scrivi(totDist, 25, "   |  _ < / /\\ \\ | |    | |  | | |  | | | |");
                        Scrivi(totDist, 26, "   | |_) / ____ \\| |____| |__| | |__| | |_|");
                        Scrivi(totDist, 27, "   |____/_/    \\_\\______|_____/ \\____/  (_)");
                    }
                    break;
                case "Carlo": //stampa Carlo
                    lock (_lock)//per farlo stampare tutto insieme e non spezzato
                    {
                        Scrivi(totDist, 22, "     _____          _____  _      ____    _ ");
                        Scrivi(totDist, 23, "    / ____|   /\\   |  __ \\| |    / __ \\  | |");
                        Scrivi(totDist, 24, "   | |       /  \\  | |__) | |   | |  | | | |");
                        Scrivi(totDist, 25, "   | |      / /\\ \\ |  _  /| |   | |  | | | |");
                        Scrivi(totDist, 26, "   | |____ / ____ \\| | \\ \\| |___| |__| | |_|");
                        Scrivi(totDist, 27, "    \\_____/_/    \\_\\_|  \\_\\______\\____/  (_)");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
