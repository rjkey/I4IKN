using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCP_Server
{
	class file_server
	{
		public static void Main (string[] args)
		{
			TcpListener server= null;

			try
			{
				Int32 PORT = 9000;
				IPAddress localAddr = IPAddress.Parse("10.0.0.1");

				server = new TcpListener(localAddr, PORT);

				server.Start();

				Byte[] bytes = new Byte[1000];
				string data = null;

				while(true)
				{

					Console.WriteLine("Waiting for a new connection...");

					TcpClient client = server.AcceptTcpClient();
					Console.WriteLine("Connected!");

					data=null;

					NetworkStream stream = client.GetStream();

					int i = stream.Read(bytes, 0, bytes.Length);

					//Læser ved hvad client sender over streamen og viser det i console.
					data = Encoding.ASCII.GetString(bytes,0,i);
					Console.WriteLine("Received: {0}", data);

					
					//Om filen eksisterer, deles filen op i et array, og sender derefter med 1000bytes ved hver dowhile løkke
					//Clienten modtager henholdvis et 1 tal om filen findes, og et 0 tal om server ikke finder filen.
					if(File.Exists(data))
					{
						var reader = new BinaryReader(File.Open(data, FileMode.Open));

						int counter = 1;
						byte[] array;

						array = Encoding.ASCII.GetBytes("1");
						stream.Write(array, 0, array.Length);
					

						do
						{
							array = reader.ReadBytes(1000);
							stream.Write(array,0,array.Length);

							Console.WriteLine("Sending package #{0} - Sent {1} bytes", counter, array.Length);

							counter++;
						} while(array.Length > 0);

						reader.Close();
					}
					//Sender et 0 tal til client fordi filen ikke eksiterer hos server
					else
					{
						byte[] toSend = Encoding.ASCII.GetBytes("0");
						stream.Write(toSend, 0, toSend.Length);
					}
					stream.Close();
					client.Close();
				}
			}
			catch(SocketException e)
			{
				Console.WriteLine("SocketException: {0}", e);
			}
			finally
			{
				server.Stop();
			}
		}
	}
}
