using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace VibExchange.Models
{
    public class UploadFile
    {

        [Required(ErrorMessage = "Please Upload File")]
        [FileExtensions(Extensions = "txt,doc,docx,pdf", ErrorMessage = "Please upload valid file format")]
        [Display(Name = "File")]
        public HttpContext fileName { get; set; }

        [Required]
        [Display(Name = "Instrument Used")]
        public string Instrument { get; set; }

        [Required]
        [Display(Name = "Analysis Type")]
        public string AnalysisType { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Analysis Method")]
        public string analysisMethodType { get; set; }

        [Display(Name = "Fill Your Machine Data :-")]
        public virtual string MachineLabel { get; set; }

        /// <summary>
        /// These properties are used to get all detail about driveunit and non-drive unit about machine and all coupling that are mounted with that perticular machine.
        /// </summary>
        /// 
        public int FileID { get; set; }

        public int MachineID { get; set; }

        [Display(Name="ID")]
        public int DrivenUnitID { get; set; }

        [Required]
        [Display(Name = "Machine Name")]
        public string TrainName { get; set; }

        [Required]
        [Display(Name = "Machine Orientation")]
        public string MachineOrientaion { get; set; }

        [Required]
        [Display(Name = "Mounting Type")]
        public string MountingType { get; set; }

        [Required]
        [Display(Name = "Transmission Type")]
        public string TransmissionType { get; set; }

        [Required]
        [Display(Name = "Drive Unit")]
        public string DEUnitType { get; set; }

        [Required]
        [Display(Name = "Drive Unit RPM")]
        public string DURPM { get; set; }

        [Display(Name="No. of Poles")]
        public int NoOfPoles { get; set; }

         [Display(Name = "Point Count")]
        public int Point_Count_DE { get; set; }

        ///End of Machine Detail of both type drive unit and intermediate or non drive unit...

        [Required]
        [Display(Name = "Machine Name")]
        public string NDEUnit1 { get; set; }

        [Display(Name = "Machine Name")]
        public string NDEUnit2 { get; set; }

        [Display(Name = "Machine Name")]
        public string NDEUnit3 { get; set; }

        [Display(Name="Unit Type")]
        public string NDEType { get; set; }
        [Display(Name="RPM")]
        public double NDERPM { get; set; }

        [Display(Name="Input RPM")]
        public double InputRPM { get; set; }

        [Display(Name = "Output RPM")]
        public double OutputRPM { get; set; }

        [Display(Name = "Gear Ratio")]
        public string GearRatio { get; set; }

        [Display(Name = "Teeth Input")]
        public int TeethCountInput { get; set; }

        [Display(Name = "Teeth Idle")]
        public int TeethCountIdle { get; set; }

        [Display(Name = "Teeth Output")]
        public int TeethCountOutput { get; set; }

        [Display(Name = "Belt Count")]
        public int BeltCount { get; set; }

        [Display(Name = "Belt Type")]
        public string BeltType { get; set; }

        [Display(Name = "Belt Length")]
        public double BeltLength { get; set; }

        [Display(Name = "Pitch Diameter")]
        public double PitchDia { get; set; }

        [Display(Name = "Teeth Count")]
        public int TeethCount { get; set; }

        [Display(Name = "No Of Blades/Fins")]
        public int BladesorFins { get; set; }

        [Display(Name = "Point Count")]
        public int Point_Count_NDE { get; set; }

        public IEnumerable MachienDataList { get; set; }

        /// <returns></returns>
        /// ///End of Machine Detail of both type drive unit and intermediate or non drive unit...

        /// <summary>
        ///  /// Bearing Related data //////////////////////
        /// </summary>
        [Display(Name = "Lubrication Type")]
        public string BearingLubrication { get; set; }

        [Display(Name = "Bearing Make")]
        public string bearingMake { get; set; }

        [Display(Name = "Bearing DE")]
        public string bearingNoDE { get; set; }

        [Display(Name = "Bearing NDE")]
        public string bearingNoNDE { get; set; }

        [Display(Name="Bearing DE Input")]
        public string bearingDEIn { get; set; }

        [Display(Name = "Bearing NDE Input")]
        public string bearingNDEIn { get; set; }

        [Display(Name = "Bearing DE Output")]
        public string bearingDEOut { get; set; }

        [Display(Name = "Bearing NDE Output")]
        public string bearingNDEOut { get; set; }

        [Display(Name = "Bearing DE Idler")]
        public string bearingDEIdler { get; set; }

        [Display(Name = "Bearing NDE Idler")]
        public string bearingNDEIdler { get; set; }

        /// <returns></returns>




        [Required(ErrorMessage = "Please enter atleast one machine !")]
        [Display(Name = "Machine")]
        public string machine { get; set; }


        [Display(Name = "Coupling")]
        public string coupling1 { get; set; }


        [Display(Name = "Coupling")]
        public string coupling2 { get; set; }


        [Display(Name = "Coupling")]
        public string coupling3 { get; set; }

       


        [Display(Name = "RPM/WINGS")]
        public string rpm1 { get; set; }


        [Display(Name = "RPM/WINGS")]
        public string rpm2 { get; set; }


        [Display(Name = "RPM/WINGS")]
        public string rpm3 { get; set; }

       
        [Display(Name = "Total Points")]
        public int TotalPoint { get; set; }

        public string filelabel { get; set; }

        public string fileName1 { get; set; }

        public DateTime Uploaddate { get; set; }

        [Required(ErrorMessage = "Please select file type.")]
        public bool FileType { get; set; }

        public IEnumerable machine1 { get; set; }

        public IEnumerable CostList { get; set; }

        public string sr { get; set; }

        public string costperpoint { get; set; }

        public string costpergraph { get; set; }
        public string AnalysisTime { get; set; }

        [FileExtensions(Extensions = "txt,doc,docx,pdf", ErrorMessage = "Please upload valid file format")]
        [Display(Name = "New File")]
        public HttpContext Changefile { get; set; }

        [Display(Name = "Your File:")]
        public string currentfile { get; set; }

        public string BearingType { get; set; }


        //////////////Machine Detail Data //////////////////////////////////////////////

        public int chkId { get; set; }
        public string chkName { get; set; }
        public bool chkStatus { get; set; }
        public object Tags { get; set; }

       
        //////////////////////////Cost Table Data///////////////////////////
        //public string AnalysisType { get; set; }
        //public IEnumerable CostList { get; set; }
        

        public List<UploadFile> GetList(int id)
        {
            List<UploadFile> allList = new List<UploadFile>();

            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("select * from UploadData where ID = '" + id + "'", CommandType.Text);

                string mac = Convert.ToString(dt.Rows[0]["Machine"]);
                string[] macname = mac.Split(',');

                string rpm = Convert.ToString(dt.Rows[0]["RPM"]);
                string[] rpm10 = rpm.Split(',');

                string mac1 = Convert.ToString(dt.Rows[0]["Coupling1"]);
                string[] mac11 = mac1.Split(',');

                string rpm1 = Convert.ToString(dt.Rows[0]["RPMWINGS1"]);
                string[] rpm11 = rpm1.Split(',');

                string mac2 = Convert.ToString(dt.Rows[0]["Coupling2"]);
                string[] macname22 = mac2.Split(',');

                string rpm2 = Convert.ToString(dt.Rows[0]["RPMWINGS2"]);
                string[] rpm22 = rpm2.Split(',');

                string mac3 = Convert.ToString(dt.Rows[0]["Coupling3"]);
                string[] macname33 = mac3.Split(',');

                string rpm3 = Convert.ToString(dt.Rows[0]["RPMWINGS3"]);
                string[] rpm33 = rpm3.Split(',');
                string pointcount = Convert.ToString(dt.Rows[0]["PointCount"]);
                string[] point = pointcount.Split(',');
                for (int i = 0; i < macname.Length; i++)
                {
                    allList.Add(new UploadFile
                    {
                        machine = Convert.ToString(macname[i]),
                       // rpm = Convert.ToString(rpm10[i]),
                        coupling1 = Convert.ToString(mac11[i]),
                        rpm1 = Convert.ToString(rpm11[i]),
                        coupling2 = Convert.ToString(macname22[i]),
                        rpm2 = Convert.ToString(rpm22[i]),
                        coupling3 = Convert.ToString(macname33[i]),
                        rpm3 = Convert.ToString(rpm33[i]),
                        TotalPoint = Convert.ToInt32(point[i])
                    });
                }

                return allList;
            }

        }

    }
    public class AnalysisMethod
    {
        public string AnalysisType { get; set; }
        public string sr { get; set; }

        public string costperpoint { get; set; }

        public string costpergraph { get; set; }
        public string AnalysisTime { get; set; }
        public IEnumerable CostList { get; set; }
    }

    public class BearingType
    {
        public string BearingMode { get; set; }

        [Display(Name = "Diameter of Inner Race")]
        public string bearingDOIR { get; set; }

        [Display(Name = "Diameter of Outer Race")]
        public string bearingDOOR { get; set; }

        [Display(Name = "Contact Angle(Degree)")]
        public string bearingCA { get; set; }

        [Display(Name = "Diameter of Rolling Element")]
        public string bearingDORE { get; set; }

        [Display(Name = "Number of Rolling Elements")]
        public int bearingNORE { get; set; }

        [Display(Name = "Bearing Number")]
        public string bearingManufacture { get; set; }

        [Display(Name = "Bearing Model No.")]
        public string bearingModel { get; set; }

    }

}