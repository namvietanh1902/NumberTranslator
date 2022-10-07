using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class ClientHandler
    {
        public IPEndPoint IP { get; set; }
        Socket socket { get; set; }
        TcpClient client { get; set; }

        Stream stream { get; set; }
    }
}
