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
        }
        IPEndPoint IP;
        Socket socket;
        TcpListener server;
        Stream stream;

        private void btnSend_Click(object sender, EventArgs e)
        {
            Send();
        }
        void Connect()
        {
            try
            {
                IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);
                
                server = new TcpListener(IP);
                server.Start();
                socket = server.AcceptSocket();
                stream = new NetworkStream(socket);
                
                Thread listen = new Thread(Receive);
                listen.IsBackground = true;
                listen.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void CloseThread()
        {   

            socket.Close();
            stream.Close();
            server.Stop();
        }
        void Send()
        {

        }

        void Receive()
        {
            try
            {
                while (true)
                {
                    var reader = new StreamReader(stream);
                    string str = reader.ReadLine();
                    listView1.Items.Add(str);
                }
            }
            catch (Exception ex)
            {
                Close();
            }
        }
        byte[] Serialize() { return null; }
        object Deserialize(byte[] data) { return null; }


        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseThread();
            Dispose();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect();
        }
    }
}
