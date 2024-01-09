using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATShipD365TS.Models
{
    public class JsonResponse
    {
        //public Message Message { get; set; }
        public string id { get; set; }
        public int Status { get; set; }
        public string ReturnMsg { get; set; }
        public string Submitted { get; set; }
    }

    public class Message
    {
        public string id { get; set; }
        public int Status { get; set; }
        public string ReturnMsg { get; set; }
        public string Submitted { get; set; }
    }
}


