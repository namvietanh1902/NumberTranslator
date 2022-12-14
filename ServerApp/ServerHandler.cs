using ServerApp.Models;
using ServerApp.Translator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public class ServerHandler
    {
        public IPEndPoint IP { get; set; }

        public TcpListener server { get; set; }

        bool isStopping = false;

        public List<Client> clients { get; set; }
        public List<TcpClient> sockets = new List<TcpClient> { };
        public delegate void UpdateView(List<Client> clients) ;
        public UpdateView updateView { get; set; }
        public ServerHandler()
        {
            IP = new IPEndPoint(IPAddress.Any, 9000);
            server = new TcpListener(IP);
        }

        public void Connect()
        {
            try
            {

                
                server.Start();
                Task listen = new Task(Receive);
                listen.Start();




            }
            catch (Exception ex)
            {
               
            }
        }
        public void CloseThread()
        {
            if(server != null)
            {
                isStopping = true;
                foreach (TcpClient socket in sockets)
                {
                    if (socket != null)
                    {
                        socket.Close();
                    }

                }
                server.Stop();

            }


        }
        
        void Send(Stream stream, string number, string language)
        {
            if (number != null)
            {
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;
                ITranslator translator;
                if (language == "en")
                {
                    translator = new EnTranslator();
                    writer.WriteLine(translator.Translate(number));
                }
                else if (language == "vi")
                {
                    translator = new VNTranslator();
                    writer.WriteLine(translator.Translate(number));

                }
                else if (language == "france")
                {
                    translator = new FranceTranslator();
                    writer.WriteLine(translator.Translate(number));

                }



            }

        }
        private static bool TestConnection(TcpClient client)
        {
            bool sConnected = true;

            if (client.Client.Poll(0, SelectMode.SelectRead))
            {
                if (!client.Connected) sConnected = false;
                else
                {
                    byte[] b = new byte[1];
                    try
                    {
                        if (client.Client.Receive(b, SocketFlags.Peek) == 0)
                        {
                            // Client disconnected
                            sConnected = false;
                        }
                    }
                    catch { sConnected = false; }
                }
            }
            return sConnected;
        }
        void Receive()
        {
            isStopping = false;
            try
            {
                while (true)
                {

                    Task handleThread = new Task(() =>
                    {
                        try
                        {
                            var client = server.AcceptTcpClient();
                            sockets.Add(client);
                            Stream stream = client.GetStream();
                            


                            while (TestConnection(client))
                            {
                                var reader = new StreamReader(stream);
                                string str = reader.ReadLine();
                                string language = null;
                                string number = null;
                                
                                if (str == null)
                                {
                                    clients.Add(new Client
                                    {
                                        IP = client.Client.RemoteEndPoint.ToString(),
                                        Language = "notify",
                                        Request = "Disconnected"

                                    });
                                }
                                else
                                {
                                    language = str.Split('*')[1];
                                    number = str.Split('*')[0];

                                    clients.Add(new Client
                                    {
                                        IP = client.Client.RemoteEndPoint.ToString(),
                                        Language = language,
                                        Request = String.Join(".", new VNTranslator().FormatValue(number).Reverse())

                                    });
                                }
                                Send(stream, number, language);
                                updateView(clients);

                            }
                            sockets.Remove(client);
                            stream.Close();
                            client.Close();
                        }


                        catch (Exception err)
                        {
                            //if (!isStopping) throw;
                        }
                    });

                    handleThread.Start();
                }

            }


            catch (Exception ex)
            {

            }
        }
    }
}
