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
using ClientApp.Model;

namespace ClientApp
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            setCBB();
           

        }
        private void setCBB()
        {
            cbbLanguage.Items.AddRange(new LanguageCBB[]
            {
                new LanguageCBB{value = "vi", description = "Tiếng Việt"},
                new LanguageCBB{value = "en",description="Tiếng Anh (English)"},
                new LanguageCBB{value = "france",description="Tiếng Pháp (Francais)"}
            });
            cbbLanguage.SelectedIndex = 0;
        }
        IPEndPoint IP; 
        Socket socket;
        TcpClient client;
        Stream stream;
        bool isConnected = false;
        

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (socket != null)
            {
                Send();
            }
            else MessageBox.Show("Chưa kết nối server");
        }
        void Connect() 
        {
            try
            {
                IP =  new IPEndPoint(IPAddress.Parse("10.10.56.148"), 9000);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client = new TcpClient();
                client.Connect(IP);
                stream = client.GetStream();
                Task listen = new Task(Receive);
                listen.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot connect to server");
                
            }
        }
        void CloseThread()
        {   
            if (socket != null)
            {
                socket.Close();
                client.Close();
            }
            else if (stream != null)
            {
                stream.Close();
            }
        }
        void Send()
        {   
            if (txtInput.Text != String.Empty)
            {
                var validator = new ClientApp.Validator.NumberValidator();
                var txt = txtInput.Text;
                var writer = new StreamWriter(stream);
                if (validator.checkNumber(txt))
                {
                    txt += "*" + (cbbLanguage.SelectedItem as LanguageCBB).value;
                    writer.AutoFlush = true;

                    writer.WriteLine(txt);
                }
                else
                {
                    MessageBox.Show("Số nhập vào không đúng format", "Lỗi người dùng");
                }
                txtInput.Text = String.Empty;
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
                    Invoke(new MethodInvoker(() =>
                    {
                        txtResult.Text = str;
                    }));
                }
            }
            catch(Exception ex)
            {
                Close();
            }
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseThread();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {   
            if (!isConnected)
            {
                isConnected = true;
                btnConnect.Text = "Disconnect";
                Connect();
                btnConnect.FlatAppearance.BorderColor = System.Drawing.Color.Green;
               
            }
            else
            {   
                isConnected = false;
                btnConnect.Text = "Connect";
                btnConnect.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                CloseThread();
            }
        }

        private void txtInput_Leave(object sender, EventArgs e)
        {
            
        }
    }
}
