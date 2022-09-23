using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerApp.Models;

namespace ServerApp
{
    public partial class ServerForm : Form
    {   
        
        public ServerForm()
        {
            InitializeComponent();
            
         
            dgvClient.DataSource = clients;
            Connect();
            
        }
        IPEndPoint IP;
       
        TcpListener server;
        
        bool isStopping = false;
        
        List<Client> clients = new List<Client>();
        


      

    
        void Connect()
        {
            try
            {   

                IP = new IPEndPoint(IPAddress.Any, 9000);
                
                server = new TcpListener(IP);
                server.Start();
                Task listen = new Task(Receive);
                listen.Start();
                    
                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void CloseThread()
        {   

            
            isStopping = true;
            
            server.Stop();
            
            
        }
        void Send(Stream stream,string str)
        {
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            writer.WriteLine(str);

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
            try
            {
                while (true)
                {
                   
                    Task handleThread = new Task(() =>
                       {
                           try
                           {
                               var client = server.AcceptTcpClient();
                               Stream stream = client.GetStream();
                               
                           
                               while (TestConnection(client))
                               {
                                   var reader = new StreamReader(stream);
                                   string str = reader.ReadLine();
                                   
                                   
                                   if (str == null)
                                   {
                                       clients.Add(new Client
                                       {
                                           IP = client.Client.RemoteEndPoint.ToString(),
                                           Language = "vi",
                                           Request = "Disconnected"

                                       });
                                   }
                                   else
                                   {
                                       clients.Add(new Client
                                       {
                                           IP = client.Client.RemoteEndPoint.ToString(),
                                           Language = "vi",
                                           Request = str

                                       });
                                   }
                                   UpdateView(clients);
                                   Send(stream,str);

                               }
                               client.Close();
                               stream.Close();
                           }

                           catch (Exception err)
                           {    
                               if(!isStopping) throw;
                           }
                        });
                    
                    handleThread.Start(); }
               
                } 
                
            
            catch (Exception ex)
            {
                
            }
        }
        private void UpdateView(List<Client> clients)
        {
            Invoke(new MethodInvoker(() =>
            {
                dgvClient.DataSource = null;
                dgvClient.DataSource = clients;
            }));
        }
        


        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void btnDis_Click(object sender, EventArgs e)
        {
           

        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseThread();

        }
    }
}
