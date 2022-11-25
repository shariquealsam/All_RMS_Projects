using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS_Fleet.Models
{
    public class Mail
    {
        public int RecId { get; set; }
        public string VehicleNo { get; set; }
        public string Branch { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public string status { get; set; }
        public string Remarks { get; set; }
        
    }
}