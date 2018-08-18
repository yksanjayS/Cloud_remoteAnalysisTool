using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using VibExchange.Models;

namespace VibExchange.Areas.RemoteAnalysis.Models
{
    /// <summary>
    /// This Class contain the field related to uploaded file.Also contain the information about bearing that will used for calculate bearing frequency.
    /// Auther : Vimal Kumar Yadav
    /// </summary>
    /// 

    public class FileDetail
    {
        public int FileID { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        [FileExtensions(Extensions = "txt,doc,docx,pdf,JPEG,JFIF,JPEG,Exif,TIFF,GIF,,BMP,PNG,PPM,PGM,PBM,PNM,BAT,BPG,RAR", ErrorMessage = "Please upload valid file format")]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "Data Type")]
        public string FileType { get; set; }

        [Required]
        [Display(Name = "File Formate")]
        public string FileFormate { get; set; }

        [Required]
        [Display(Name = "Instrument Used")]
        public string InstrumentUsed { get; set; }

        [Display(Name = "Machine Count")]
        [Required(ErrorMessage = "Value should be greater than 0")]
        [Range(1, 1000)]
        public int NoOfMachines { get; set; }

        [Display(Name = "Point Count")]
        [Required(ErrorMessage = "Value should be greater than 0")]
        [Range(1, 100)]
        public int NoOfPoints { get; set; }

        [Display(Name = "Analysis Method")]
        public string AnalysisMethod { get; set; }

        [Required]
        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public string UploadDate { get; set; }

        public List<PlantData> lstPlant { get; set; }
        public List<AreaData> lstArea { get; set; }
        public List<TrainData> lstTrain { get; set; }
        public List<MachineData> lstMachine { get; set; }
        public List<PointData> lstPoint { get; set; }
        public List<ReturnData> lstReturnData { get; set; }

        public static List<FileDetail> getPreviousFileData(string UserRole)
        {
            List<FileDetail> fileList = new List<FileDetail>();  // creating list of model.
            string username = HttpContext.Current.Session["UserName"].ToString();
            using (DBClass context = new DBClass())
            {
                context.AddParameter("@UserID", username);
                DataTable dt = context.getData("getPreviousDataByUserID", CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    string[] fName = Convert.ToString(dr["FileName"]).Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                    fileList.Add(new FileDetail
                    {
                        FileID = Convert.ToInt16(dr["FileID"]),
                        UserName = Convert.ToString(dr["UserID"]),
                        UploadDate = Convert.ToString(dr["DateOfUpload"]),
                        FileType = Convert.ToString(dr["FileType"]),
                        FileName = Convert.ToString(fName[1]),
                        InstrumentUsed = Convert.ToString(dr["InstrumentUsed"])
                    });
                }
                return fileList;
            }
        }
    }

    public partial class TFileStructure
    {
        public List<TreeViewModel> Childs { get; set; }
    }

    public class TreeViewModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
    }

    public class PlantData
    {
        public string PlantID { get; set; }

        [Required]
        [Display(Name = "Plant Name")]
        public string PlantName { get; set; }

        [Display(Name = "Plant Detail")]
        public string PlantDetail { get; set; }

        [Display(Name = "Category")]
        public string PlantCategory { get; set; }

        public string FileID { get; set; }

    }

    public class AreaData
    {
        public string AreaID { get; set; }

        [Required]
        [Display(Name = "Area Name")]
        public string AreaName { get; set; }

        [Display(Name = "Area Detail")]
        public string AreaDetail { get; set; }

        public string FileID { get; set; }

        public string ParentID { get; set; }
    }

    public class TrainData
    {
        public string TrainID { get; set; }
        [Required]
        [Display(Name = "Train Name")]
        public string TrainName { get; set; }

        [Required]
        [Display(Name = "Train Detail")]
        public string TrainDetail { get; set; }

        [Display(Name = "No of Machine")]
        public string NoOfMachine { get; set; }

        public string FileID { get; set; }

        public string ParentID { get; set; }
    }

    public class MachineData
    {
        public string MachineID { get; set; }

        [Required]
        [Display(Name = "Machine Name")]
        public string MachineName { get; set; }

        [Required]
        [Display(Name = "Machine Class")]
        public string MachineClass { get; set; }


        [Display(Name = "Machine Detail")]
        public string MachineDetail { get; set; }

        [Required]
        [Display(Name = "RPM Driven")]
        public int RPMDriven { get; set; }

        [Display(Name = "Pulse Revolution")]
        public float PulseRevolution { get; set; }

        public string FileID { get; set; }

        public string ParentID { get; set; }

    }

    public class PointData
    {
        public string PointID { get; set; }

        [Required]
        [Display(Name = "Point Name")]
        public string PointName { get; set; }
        [Display(Name = "Point Description")]
        public string PointDetail { get; set; }
        public string BearingID { get; set; }
        public string ParentID { get; set; }
        public string FileID { get; set; }
        public int NoofChannel { get; set; }

        [Display(Name = "Channel Status")]
        public string ChannelA { get; set; }

        [Display(Name = "Amplifier Mode")]
        public string RadioA { get; set; }

        [Display(Name = "Channel 2")]
        public string ChannelB { get; set; }

        [Display(Name = "Amplifier Mode")]
        public string RadioB { get; set; }

        [Display(Name = "Enveloping Frequency")]
        public int EnvelopingFreq { get; set; }

        [Display(Name = "Window Type")]
        public string WindowType { get; set; }

        [Display(Name = "Spectrul Lines")]
        public string SpectralLines { get; set; }

        [Display(Name = "Fmin(HPF)")]
        public int Fmin { get; set; }

        [Display(Name = "Fmax(LPF)")]
        public int Fmax { get; set; }

        [Display(Name = "Trigger Mode")]
        public string TrigerMode { get; set; }

        [Display(Name = "Averaging Mode")]
        public string AverageMode { get; set; }

        [Display(Name = "N")]
        public int N { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "General Alarms")]
        public string GenralAlarm { get; set; }

        [Display(Name = "Band Alarms")]
        public string BandAlarm { get; set; }

        /////////// Bearing fiels detail
        public string BearingStatus { get; set; }

        [Display(Name = "Manufacturer ")]
        public string BearingMake { get; set; }

        [Display(Name = "Bearing Number")]
        public string BearingModel { get; set; }

        [Display(Name = "Inner Diameter")]
        public float InnerDia { get; set; }

        [Display(Name = "Outer Diameter")]
        public float OuterDia { get; set; }

        [Display(Name = "Contact Angel")]
        public float ContactAngel { get; set; }

        [Display(Name = "Rolling Element Diameter")]
        public float RollingDia { get; set; }

        [Display(Name = "NO of Rolling Element")]
        public int NoOfRollingElement { get; set; }

        public int? Balls { get; set; }

        public double? FTF { get; set; }

        public double? BSF { get; set; }

        public double? BPFO { get; set; }

        public double? BPFI { get; set; }

        public int? AlarmID { get; set; }

        [Required]
        [Display(Name = "Alarm Name")]
        public string AlarmName { get; set; }

        [Required]
        [Display(Name = "Alarm Type")]
        public string AlarmType { get; set; }

        [Required]
        [Display(Name = "High Value")]
        public float HighValue { get; set; }

        [Required]
        [Display(Name = "Low Value")]
        public float LowValue { get; set; }


        public List<FaultFrequency> lstFaultFrequency { get; set; }
        public List<PointData> lstBearingMake { get; set; }
        public List<PointData> lstBearingNumber { get; set; }
        public List<BearingData> lstBearingFrequency { get; set; }
        //public List<Alarm> lstAlarmDetail { get; set; }
    }

    public class BearingData
    {
        public string PointID { get; set; }
        public string BearingID { get; set; }

        [Display(Name = "Manufacturer ")]
        public string BearingMake { get; set; }

        [Display(Name = "Bearing Number")]
        public string BearingNumber { get; set; }

        [Display(Name = "Inner Diameter")]
        public double? InnerDia { get; set; }

        [Display(Name = "Outer Diameter")]
        public double? OuterDia { get; set; }

        [Display(Name = "Contact Angel")]
        public double? ContactAngel { get; set; }

        [Display(Name = "Rolling Element Diameter")]
        public double? RollingDia { get; set; }

        [Display(Name = "NO of Rolling Element")]
        public int? NoOfRollingElement { get; set; }

        public int? Balls { get; set; }

        public double? FTF { get; set; }

        public double? BSF { get; set; }

        public double? BPFO { get; set; }

        public double? BPFI { get; set; }

        public string BearingStatus { get; set; }
        public List<BearingData> BearingList { get; set; }
        public List<BearingData> lstBearingMake { get; set; }
        public List<BearingData> lstBearingNumber { get; set; }
    }

    public class FaultFrequency
    {
        public int FaultFreqID { get; set; }

        public string PID { get; set; }

        [Required]
        [Display(Name = "Frequency Name")]
        public string FaultFreqName { get; set; }

        [Required]
        [Display(Name = "Frequency Value")]
        public string FaultFreqValue { get; set; }
    }

    public class Alarm
    {

        public int AlarmID { get; set; }

        [Required]
        [Display(Name = "Alarm Name")]
        public string AlarmName { get; set; }

        [Required]
        [Display(Name = "Alarm Type")]
        public string AlarmType { get; set; }

        [Required]
        [Display(Name = "High Value")]
        public float HighValue { get; set; }

        [Required]
        [Display(Name = "Low Value")]
        public float LowValue { get; set; }

    }

    public class Unit
    {
        public int UnitID { get; set; }

        [Required]
        [Display(Name = "Unit Type")]
        public string UnitType { get; set; }

        [Required]
        [Display(Name = "Unit")]
        public string UnitName { get; set; }

    }

    public class ReturnData
    {
        public bool Status { get; set; }
        public int FileID { get; set; }
        public string UserID { get; set; }
        public DateTime? MeasurementTime { get; set; }
        public DateTime UploadTime { get; set; }
    }

}