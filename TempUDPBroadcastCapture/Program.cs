using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TempUDPBroadcastCapture
{
    class Program
    {
        private const int port = 5005;

        static void Main(string[] args)
        {
            Start();
            Console.ReadKey();
        }

        private static void Start()
        {
            using (var reciever = new TempServiceReference1.TempServiceClient("BasicHttpBinding_ITempService"))
            {
                using (UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, port)))
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(0, 0);
                    while (true)
                    {
                        Console.WriteLine("Waiting for broadcast {0}", client.Client.LocalEndPoint);
                        byte[] datagramRecieved = client.Receive(ref remoteEndPoint);

                        string str = Encoding.ASCII.GetString(datagramRecieved, 0, datagramRecieved.Length);
                        Console.WriteLine("Recieves {0} bytes from {1} port {2} message {3}", datagramRecieved.Length,
                            remoteEndPoint.Address, remoteEndPoint.Port, str);

                        reciever.PostTempToDB(str); // Denne sætning sender data til databasen
                    }
                }
            }
        }
    }
}
