using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WumingIMServer
{
    class Settings
    {
        public string serverPort { get; set; }
        public string sqlServerIP { get; set; }
        public string sqlAccessAccount { get; set; }
        public string sqlAccessPassword { get; set; }
        public string RSAkey { get; set; }
        public string SqlDBName { get; set; }
    }
}
