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

namespace ClientApp
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();

        }
        IPEndPoint IP; 
        Socket socket;
        TcpClient client;
        Stream stream;

        private void btnSend_Click(object sender, EventArgs e)
        {
            Send();
        }
        void Connect() 
        {
            try
            {
                IP =  new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client = new TcpClient();
                client.Connect(IP);
                stream = client.GetStream();
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
            client.Dispose();
            client.Close();
        }
        void Send()
        {
            if (txtNumber.Text != String.Empty)
            {
                
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;

                writer.WriteLine(txtNumber.Text);
                if (txtNumber.Text == "Bye")
                {
                    this.Close();
                }
                txtNumber.Text = String.Empty;

                
                
                
            }
           
        }
        
        void Receive()
        {
            try
            {
                while (true)
                {
                    var reader = new StreamReader(stream);
                    string str = reader.ReadLine();
                    txtResult.Text = str;
                }
            }
            catch(Exception ex)
            {
                Close();
            }
        }
        byte[] Serialize() { return null; }
        object Deserialize(byte[] data) { return null; }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseThread();
        }
    }
}
