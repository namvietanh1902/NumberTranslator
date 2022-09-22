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

namespace ServerApp
{
    public partial class ServerForm : Form
    {   
        
        public ServerForm()
        {
            InitializeComponent();
            
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        IPEndPoint IP;
       
        TcpListener server;
        Thread listen;
        bool isStopping = false;


      

    
        void Connect()
        {
            try
            {   

                IP = new IPEndPoint(IPAddress.Any, 9000);
                
                server = new TcpListener(IP);
                server.Start();
                listen = new Thread(Receive);
                listen.Start();
                listen.IsBackground = true;
                    
                


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
                   
                    Thread handleThread = new Thread(() =>
                       {
                           try
                           {
                               var client = server.AcceptTcpClient();
                               Stream stream = client.GetStream();
                           
                               while (TestConnection(client))
                               {
                                   var reader = new StreamReader(stream);
                                   string str = reader.ReadLine();
                                   listView1.Items.Add(str);
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
                    handleThread.IsBackground = true;
                    handleThread.Start(); }
               
                } 
                
            
            catch (Exception ex)
            {
                
            }
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
