// Giulio Zangheri 4H 21/12/2023
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static System.Console;
using System.Reflection;

namespace ConsoleAppServer
{
    internal class ProgramServer
    {
        static void Main(string[] args)
        {
            string data = null;
            //Data buffer for incoming data
            byte[] bytes = new Byte[1024];

            Console.WriteLine("\nPROGRAMMA SERVER\n");

            //Establish the local endpoint for the socket
            //Dns.GetHostName returns the name of the
            //host running the application

            //I 3 modi sono indifferenti, funzionano tutti quanti
            //MODO 1
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[1];

            //MODO 2
            //IpAddress ipAddress = IPAddress.Any; //Fornisce un indirizzo IP che il server deve attendere l'attività del client su tutte le interfaccie di rete. Questo campo è di sola lettura.

            //MODO 3
            IPAddress ipAddress = IPAddress.Parse("192.168.56.1"); //varia in base al pc in cui si è


            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and
            // listen for the incoming connection.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    data = null;

                    int byteSent;
                    string message = "";
                    Console.WriteLine("in attesa di un messaggio");
                    do
                    {
                        //in attesa di un messaggio
                        data = Encoding.ASCII.GetString(bytes, 0, handler.Receive(bytes));
                        Console.WriteLine($"messaggio ricevuto: {data}");
                        //risposta
                        if (data == "ciao" && message == "ciao") { break; }
                        Console.Write("Scrivere messaggio da inviare -> ");
                        //Chiedi in input quello da mandare
                        message = Console.ReadLine();
                        byteSent = handler.Send(Encoding.ASCII.GetBytes(message));
                        Console.WriteLine("Messaggio inviato, in attesa di risposta");
                    } while (data != "ciao" || message != "ciao");

                    Console.WriteLine();
                    Console.WriteLine("Connessione chiusa");
                    data = null;
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                    
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
