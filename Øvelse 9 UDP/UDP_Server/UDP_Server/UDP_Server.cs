using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Server
{
    public class UDP_Server 
    {
        private const int ServerPort = 9000;

        private static void StartServer() 
        {
			//Klargør UDP server
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var server = new UdpClient(ServerPort);
            var serverInfo = new IPEndPoint(IPAddress.Any,ServerPort);

            try 
            {
				//Venter på connection og kommando fra client
                while (true) 
                {
                    Console.WriteLine("Waiting for command from  UDP client");
                    byte[] bytes = server.Receive(ref serverInfo);
                    var receivedCommand = Encoding.ASCII.GetString(bytes,0,bytes.Length);
                    Console.WriteLine("UDP server has received the command: " + receivedCommand + " from " + serverInfo.Address);

                    //Sender servers connection detaljer
                    var responseEp = new IPEndPoint(serverInfo.Address, ServerPort);

					//Sender loadaverage eller uptime efter modtaget karakter
                    switch (receivedCommand)
                    {
                        case "U":
                        case "u":
                            using (var sr = new StreamReader ("/proc/uptime"))
                            {
                                var line = sr.ReadToEnd();
                                Console.WriteLine("Sending server uptime: " + line);
                                byte[] sendUbuf = Encoding.ASCII.GetBytes(line);

                                socket.SendTo(sendUbuf, responseEp);
                            }
                            break;
                        case "L":
                        case "l":
                            using (var sr = new StreamReader ("/proc/loadavg"))
                            {
                                var line = sr.ReadToEnd();
                                Console.WriteLine("Sending server load average: " + line);

                                byte[] sendLbuf = Encoding.ASCII.GetBytes(line);
                                socket.SendTo(sendLbuf, responseEp);
                            }
                            break;
                        default:
                            break;
                    }
                }
            } 
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                server.Close();
            }
        }

		static int Main() 
        {
			StartServer();

            return 0;
        }
    }
}