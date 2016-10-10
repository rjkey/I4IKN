using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCP_Client
{
	class file_client
	{
		static void Main(string[] args)
		{
			try
			{
				int PORT = 9000;
				string FILENAME, IP;

				if(args.Length < 2)
				{
					Console.WriteLine("Start the program by writing the following line: ./TCP_Client.exe <ip> <filename>");
					return;
				}

				IP = args[0];
				FILENAME=args[1];

				TcpClient client = new TcpClient(IP, PORT);

				byte[] data = Encoding.ASCII.GetBytes(FILENAME);

				NetworkStream stream = client.GetStream();

				//Sender filnavn til server
				stream.Write(data, 0, data.Length);

				data = new byte[1000];

				//Læser svar fra server. Er det et 0 tal, skrives det i console at filen ikke findes, er det 1 tal findes filen og den modtager
				stream.Read(data, 0 , 1);

				if(Encoding.ASCII.GetString(data).StartsWith("0"))
				{
					Console.WriteLine("These are not the files you are looking for");
					Console.WriteLine("You can go about your business..");
					Console.WriteLine("Move along");
					stream.Close();
					client.Close();

					return;
				}
				else
				{
					Console.WriteLine("File found! Transfering now.. {0}", Encoding.ASCII.GetString(data));

				}

				//Det gøres klar til at modtage en ny file
				string file = Path.GetFileName(FILENAME);

				var writer = new BinaryWriter(File.Open(file,FileMode.Create));

				int i, counter = 1;

				//Modtager den nye fil i bakker. 1000bytes per bakke
				do
				{
					i=stream.Read(data, 0, data.Length);
					writer.Write(data, 0, i);

					Console.WriteLine("Receiving package #{0} - Received {1} bytes", counter, i);


					counter++;

				} while(i > 0);

				writer.Close();
				stream.Close();
				client.Close();
			}
			catch(ArgumentNullException e) 
			{
				Console.WriteLine ("ArgumentNullException: {0}", e);
			}

			catch(SocketException e) 
			{
				Console.WriteLine ("SocketException: {0}", e);
			}

			Console.WriteLine ("\nPress enter to continue...");

			Console.Read();
		}
	}
}