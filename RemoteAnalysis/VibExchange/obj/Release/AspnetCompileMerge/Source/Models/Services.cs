using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VibExchange.Models
{
    public class Services
    {
        public string ServiceName { get; set; }
        public string ServiceDepartment { get; set; }
        public string ServiceType { get; set; }
        public float ServiceCost { get; set; }
        public string ServiceDuration { get; set; }
        public bool IsEnabled { get; set; }
    }
}