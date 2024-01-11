// Giulio Zangheri 4H 21/12/2023
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ConsoleAppClient
{
    internal class ProgramClient
    {
        static void Main(string[] args)
        {
            string data = null;
            // Data buffer for incoming data
            byte[] bytes = new Byte[1024];

            Console.WriteLine("\nPROGRAMMA CLIENT\n");

            //Connect to a remote device
            try
            {
                //Establish the remote endpoint for the socket
                //This example uses port 11000 on the local computer

                //I 3 modi sono indifferenti, funzionano tutti quanti
                //MODO 1
                //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddress = ipHostInfo.AddressList[1];

                //MODO 2
                //IpAddress ipAddress = IPAddress.Any; //Fornisce un indirizzo IP che il server deve attendere l'attività del client su tutte le interfaccie di rete. Questo campo è di sola lettura.

                //MODO 3
                IPAddress ipAddress = IPAddress.Parse("192.168.56.1"); //ip del server


                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000); //porta del server

                // Create a TCP/IP socket.
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                //console.WriteLine(ipAddress.AddressFamily);
                //console.WriteLine(SocketType.Stream);
                //console.WriteLine(ProtocolType.Tcp);

                //Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine($"Socket connected to {sender.RemoteEndPoint.ToString()}");

                    int byteSent;
                    string message;

                    do
                    {
                        Console.Write("Scrivere messaggio da inviare -> ");
                        //Chiedi in input quello da mandare
                        message = Console.ReadLine();
                        byteSent = sender.Send(Encoding.ASCII.GetBytes(message));
                        Console.WriteLine("Messaggio inviato, in attesa di risposta");
                        if (data == "ciao" && message == "ciao") { break; }
                        //aspetta che riceva e riceve
                        data = Encoding.ASCII.GetString(bytes, 0, sender.Receive(bytes));
                        Console.WriteLine($"messaggio ricevuto: {data}");
                        //verifica il ricevuto
                    } while (data != "ciao" || message != "ciao");

                    Console.WriteLine();
                    Console.WriteLine("Connessione chiusa");

                    //Release the socket
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("ArgumentNullException");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
