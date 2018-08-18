using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VibExchange.Areas.RemoteAnalysis.Models
{
    public class clsReport
    {
        public int FileID { get; set; }
        public string PlantID { get; set; }
        public string PlantName { get; set; }
        public string AreaID { get; set; }
        public string AreaName { get; set; }
        public string TrainID { get; set; }
        public string TrainName { get; set; }
        public string MachineID { get; set; }
        public string MachineName { get; set; }
        public string PointID { get; set; }
        public string PointName { get; set; }
        public int AalrmID { get; set; }
        public string AlarmName { get; set; }
        public int DataID { get; set; }
        public DateTime? MeasurementTime { get; set; }
        public double OverallChnlA { get; set; }
        public string UnitChnlA { get; set; }
        public double OverallChnlB { get; set; }
        public string UnitChnlB { get; set; }
        public double HighValue { get; set; }
        public double LowValue { get; set; }
       
    }
}