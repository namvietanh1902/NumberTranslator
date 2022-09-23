using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Models
{
    public class Client
    {   
        [DisplayName("IP Address")]
        public string IP { get; set; }
        [DisplayName("Connected At")]

        public DateTime ConnectedAt { get; set; } = DateTime.Now;
        [DisplayName("Language")]
        public string Language { get; set; }
        [DisplayName("Request Number")]
        public string Request { get; set; }
    }
}
