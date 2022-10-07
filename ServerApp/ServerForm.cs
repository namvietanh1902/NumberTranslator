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
using ServerApp.Translator;

namespace ServerApp
{
    public partial class ServerForm : Form
    {   
        
        public ServerForm()
        {
            InitializeComponent(); 
            serverHandler = new ServerHandler();
            serverHandler.updateView = new ServerHandler.UpdateView(UpdateView);
            serverHandler.clients = new List<Client>();
            dgvClient.DataSource = serverHandler.clients;
            
            
        }
        bool isConnected = false;
        ServerHandler serverHandler;
        
    
       

        

        
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
            if (!isConnected)
            {
                serverHandler.Connect();
                isConnected = true;
                connectBtn.Text = "Disconnect";
            }
            else
            {
                serverHandler.CloseThread();
                isConnected = false;
                connectBtn.Text = "Connect";

            }
            
        }

        private void btnDis_Click(object sender, EventArgs e)
        {
           

        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            serverHandler.CloseThread();

        }
    }
}
