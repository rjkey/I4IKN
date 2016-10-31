using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDP_Client 
{

	static void Main(string[] args)
	{
		//Klargør UDP Client
		int serverPort = 9000;
		var client = new UdpClient (serverPort);
		var socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		//Tager imod serverIP og kommando
		var serverIpAddress = IPAddress.Parse (args[0]);
		string sendCommand = args[1];

		//Sender kommando
		byte[] sendbufferBytes = Encoding.ASCII.GetBytes (sendCommand);
		var destination = new IPEndPoint (serverIpAddress, serverPort);
		socket.SendTo (sendbufferBytes, destination);


		//Venter på svar fra server og skriver resultat ud i konsoll.
	    switch (args[1])
	    {
            case "U":
            case "u":
                byte[] uBytes = client.Receive(ref destination);
                Console.WriteLine("Server uptime: {0}", Encoding.ASCII.GetString(uBytes, 0, uBytes.Length));
	            break;

	        case "L":
            case "l":
                byte[] lBytes = client.Receive(ref destination);
                Console.WriteLine("Server load average: {0}", Encoding.ASCII.GetString(lBytes, 0, lBytes.Length));
                break;

            default:
	            break;

	    }
		
	}
}
