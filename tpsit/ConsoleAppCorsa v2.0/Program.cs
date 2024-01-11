//Giulio Zangheri 11/01/2024
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;
using System.Runtime.CompilerServices;

namespace ConsoleAppCorsa_v2._0
{
    internal class Program
    {
        //le posizioni dei cursori che permettono di spampare Andrea Baldo e Carlo sempre più lontani dalla posizione standard del cursore

        static int classifica = 0; //assegna le varie variabili globabi neccessarie
        static string primo = ""; //questa variabile serve alla funzione vittoria per capire quale vincitore stampare
        static string comando = "";

        static Thread thAndrea;
        static Thread thBaldo; //assegna i vari Thread
        static Thread thCarlo;

        static Object _lock = new object(); //il lock permette di stampare Andrea, Baldo e Carlo Non utilizzando risorse comuni

        static void Main(string[] args)
        {

            thAndrea = new Thread(Corridore);
            thBaldo = new Thread(Corridore); //si assegnano i corridori
            thCarlo = new Thread(Corridore);

            Title = "Giulio Zangheri 4H 2023/10/05";

            CursorVisible = false; //cosi il cursore non da fastidio alla visione della corsa

            Pronti();

            Menu(0);

            //fa iniziare tutti quanti
            thAndrea.Start(2);
            thAndrea.Name = "Andrea";
            thBaldo.Start(6); //viene passato il numero della riga tramite un object
            thBaldo.Name = "Baldo";
            thCarlo.Start(10);
            thCarlo.Name = "Carlo"; //diamo un nome ai vari Thread per distinguerli

            do //il Thread "Main" continua a eseguire chiedendo una key per il menù
            {
                Stato();

                if (Console.KeyAvailable) //verifica che si stia premendo un qualsiasi tasto
                    SceltaMenu();
            } while (thAndrea.IsAlive || thBaldo.IsAlive || thCarlo.IsAlive); //verifica che i vari thread non siano in join o abort per continuar
            Stato();

            Vittoria();

            Console.ReadLine(); //aspetta prima di chiudere la console
        }

        static void Pronti() 
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
                        Scrivi(3, 20, "Carlo  (C)                   Abort       (A)");
                        Scrivi(3, 21, "                             Join        (J)");
                        break;
                    case 2: //Baldo
                        Scrivi(3, 16, "MENU'                        AZIONE SU Baldo");
                        Scrivi(3, 17, "                                      ");
                        Scrivi(3, 18, "Andrea (A)                   Sospendere  (S)");
                        Scrivi(3, 19, "Baldo  (B)                   Riprendere  (R)");
                        Scrivi(3, 20, "Carlo  (C)                   Abort       (A)");
                        Scrivi(3, 21, "                             Join        (J)");
                        break;
                    case 3: //Carlo
                        Scrivi(3, 16, "MENU'                        AZIONE SU Carlo");
                        Scrivi(3, 17, "                                      ");
                        Scrivi(3, 18, "Andrea (A)                   Sospendere  (S)");
                        Scrivi(3, 19, "Baldo  (B)                   Riprendere  (R)");
                        Scrivi(3, 20, "Carlo  (C)                   Abort       (A)");
                        Scrivi(3, 21, "                             Join        (J)");
                        break;
                    default:
                        break;
                }
            }
        }

        static void SceltaMenu()
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
                                thAndrea.Abort();
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

        static void Corridore(object riga)
        {
            int posCorridore = 0;
            int corsia = (int)riga;
            do
            {
                if (comando.Length == 3) //join
                {
                    switch (Thread.CurrentThread.Name) //verifica quale thread è questo
                    {
                        case "Andrea":
                            if (comando[0] == 'A') //verifica chi far aspettare ad andrea
                            {
                                switch (comando[2])
                                {
                                    case 'B': //verifica la cofica che abbiamo assegnato
                                        thBaldo.Join();
                                        break;
                                    case 'C':
                                        thCarlo.Join();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case "Baldo":
                            if (comando[0] == 'B') //verifica chi far aspettare a baldo
                            {
                                switch (comando[2])
                                {
                                    case 'A': //verifica la cofica che abbiamo assegnato
                                        thAndrea.Join();
                                        break;
                                    case 'C':
                                        thCarlo.Join();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case "Carlo":
                            if (comando[0] == 'C') //verifica chi far aspettare a carlo
                            {
                                switch (comando[2])
                                {
                                    case 'A': //verifica la cofica che abbiamo assegnato
                                        thAndrea.Join();
                                        break;
                                    case 'B':
                                        thBaldo.Join();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                } //join

                posCorridore++;
                Thread.Sleep(150); //Velocità dei corridori
                lock (_lock) //il lock utilizza la risorsa e appena viene liberato, la risorsa viene utilizzata da un altro lock
                {
                    switch (Thread.CurrentThread.Name) //verifica il thread corrente 
                    {
                        case "Andrea":                 // e spampa andrea, baldo o carlo
                            Scrivi(posCorridore, corsia + 1,  "  []");
                            Scrivi(posCorridore, corsia + 2, @" /▓▓\");
                            Scrivi(posCorridore, corsia + 3, @"  /\");
                            break;
                        case "Baldo":
                            Scrivi(posCorridore, corsia + 1,  "  ()");
                            Scrivi(posCorridore, corsia + 2, @" /▒▒\");
                            Scrivi(posCorridore, corsia + 3, @"  └└");
                            break;
                        case "Carlo":
                            Scrivi(posCorridore, corsia + 1,  "  <>");
                            Scrivi(posCorridore, corsia + 2, @" /██\");
                            Scrivi(posCorridore, corsia + 3, @"  ||");
                            break;
                        default:
                            break;
                    }
                }

            } while (posCorridore < 115);

            lock (_lock) //dentro a un altro lock si aumenta la classifica per scrivere l'ordine di arrivo
            {
                classifica++;
                Scrivi(115, corsia, $"{classifica}");
                if (classifica == 1) //controlla se Andrea è il primo, nel caso, alla fine stampa il suo nome
                    primo = Thread.CurrentThread.Name;
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
