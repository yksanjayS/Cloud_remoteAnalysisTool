using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VibExchange.Areas.RemoteAnalysis.Models;
using VibExchange.Filters;
using VibExchange.Areas.RemoteAnalysis;
using VibExchange.Models;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Configuration;
using RestSharp;
using VibExchange.Areas.Helper;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using Microsoft.Reporting.WebForms;


namespace VibExchange.Areas.RemoteAnalysis.Controllers
{
    [InitializeSimpleMembershipAttribute]
    public class UploadController : Controller
    {
        string[] ChannelInput = { "Int 1", "Int 2", "Int 3", "Off" };
        string[] AmpMode = { "Lin A", "Int1 V", "Int2 S", "Env" };
        string[] WType = { "Rectangular", "Hanning" };
        string[] TrigMode = { "Free Run", "Internal", "External" };
        string[] AvgMode = { "Off", "Linear Frequency Domain", "Exponantial Frequency Domain", "Linear Time Domain", "Exponantial Time Domain" };
        int[] EnvelopingFrequency = { 2, 4, 8, 16, 32 };
        private string fileLocation = "~/Files/";
        [HttpGet]
        public ActionResult DataUpload(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult DataUpload(FormCollection formCollection, HttpPostedFileBase[] files, string ReturnUrl)
        {
            bool sts = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Files["UploadedFile"];
                if (file == null)
                {
                    ModelState.AddModelError("FileName", "Please Upload Your file");
                }
                else if (file.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 150; //150 MB
                    if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("FileName", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB. Please zip your file and upload again.");
                    }
                    else
                    {
                        using (DBClass context = new DBClass())
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            string fname = Session["UserName"].ToString() + "@" + fileName;
                            var path = Path.Combine(Server.MapPath("~/Files"), fname);

                            if (System.IO.File.Exists(path))
                            {
                                ModelState.AddModelError("File", "This file already exist!");
                                ViewBag.errorsOccurred = "You have already upload this file before ! Please rename your file or upload another file.";
                                TempData["Error"] = "You have already upload this file before ! Please rename your file or upload another file.";
                                return View();
                            }
                            context.AddParameter("@UserID", Session["UserName"].ToString());
                            context.AddParameter("@FileName", fname);
                            context.AddParameter("@FileType", formCollection["btnRadio"]);
                            context.AddParameter("@InstrumentUsed", "Kohtect-C911");
                            context.AddParameter("@AnalysisMethod", "Remote Analysis");
                            context.AddParameter("@ImagePath", path);
                            context.AddParameter("@Description", formCollection["Description"]);
                            context.AddParameter("@id", null, SqlDbType.Int, ParameterDirection.Output);
                            int count = Convert.ToInt32(context.ExecuteNonQuery("addUploadDataRAM", CommandType.StoredProcedure, ParameterDirection.Output, "@id"));
                            if (count > 0)
                            {
                                file.SaveAs(path);
                                Session["CurrentFilePath"] = path;
                                Session["CurrentFileID"] = count;
                                TempData["FileID"] = count;
                                ViewBag.Message = "File uploaded successfully";
                                return RedirectToAction("GetAnalysisKohtect", "Kohtect", new { fileid = count });
                            }
                            else
                            {
                                ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");
                            }
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult GetAnalysisKohtect(FileDetail _file)
        {
            try
            { }
            catch { }
            return View();
        }

        [HttpPost]
        public ActionResult RouteDetail(string param)
        {
            try
            {
                PlantData plant = new PlantData();
                PointData point = new PointData();
                ArrayList lstffBearings = new ArrayList();
                int row;
                int a = getIndexofNumber(param);
                string NodeNumber = param.Substring(a, param.Length - a);
                row = Convert.ToInt32(NodeNumber);
                string NodeType = param.Substring(0, a);


                switch (NodeType)
                {
                    case "Plant":
                        {
                            using (DBClass context = new DBClass())
                            {
                                context.AddParameter("@Nodeid", param);
                                DataTable dtPlant = context.getData("GetPlantDetail", CommandType.StoredProcedure);
                                if (dtPlant.Rows.Count > 0)
                                {
                                    foreach (DataRow plantitem in dtPlant.Rows)
                                    {
                                        plant.PlantID = Convert.ToString(plantitem["NodeID"]);
                                        plant.PlantName = Convert.ToString(plantitem["PlantName"]);
                                        plant.PlantCategory = Convert.ToString(plantitem["Category"]);
                                        plant.PlantDetail = Convert.ToString(plantitem["PlantDetail"]);
                                    }
                                    ViewBag.NodeType = "Plant";
                                    Session["CurrentNodeID"] = plant.PlantID;
                                    return PartialView("_DisplayPlant", plant);
                                }
                                else
                                {
                                    TempData["NodeAvailability"] = "There is no any 'Plant' for display ! Please click on 'Add New Plant' button for create new 'Plant'.";
                                    return PartialView("_PlantDetail", plant);
                                }
                            }
                        }
                    case "Area":
                        {
                            using (DBClass context = new DBClass())
                            {
                                AreaData area = new AreaData();
                                context.AddParameter("@Nodeid", param);
                                DataTable dtArea = context.getData("GetAreaDetail", CommandType.StoredProcedure);
                                if (dtArea.Rows.Count > 0)
                                {
                                    foreach (DataRow areaItem in dtArea.Rows)
                                    {
                                        area.AreaID = Convert.ToString(areaItem["NodeID"]);
                                        area.AreaName = Convert.ToString(areaItem["AreaName"]);
                                        area.AreaDetail = Convert.ToString(areaItem["AreaDetail"]);
                                    }
                                    ViewBag.NodeType = "Area";
                                    Session["CurrentNodeID"] = area.AreaID;
                                    return PartialView("_DisplayArea", area);
                                }
                                else
                                {
                                    TempData["NodeAvailability"] = "There is no any 'Area' for display ! ";
                                    return PartialView("_DisplayArea", area);
                                }
                            }
                        }
                    case "Train":
                        {
                            using (DBClass context = new DBClass())
                            {
                                TrainData train = new TrainData();
                                context.AddParameter("@Nodeid", param);
                                DataTable dtTrain = context.getData("GetTrainDetail", CommandType.StoredProcedure);
                                if (dtTrain.Rows.Count > 0)
                                {
                                    foreach (DataRow trainItem in dtTrain.Rows)
                                    {
                                        train.TrainID = Convert.ToString(trainItem["NodeID"]);
                                        train.TrainName = Convert.ToString(trainItem["TrainName"]);
                                        train.TrainDetail = Convert.ToString(trainItem["TrainDetail"]);
                                        train.NoOfMachine = Convert.ToString(trainItem["NoOfMachine"]);
                                    }
                                    ViewBag.NodeType = "Train";
                                    Session["CurrentNodeID"] = train.TrainID;
                                    return PartialView("_DisplayTrain", train);
                                }
                                else
                                {
                                    TempData["NodeAvailability"] = "There is no any 'Train' for display ! ";
                                    return PartialView("_DisplayTrain", train);
                                }
                            }
                        }
                    case "Machine":
                        {
                            using (DBClass context = new DBClass())
                            {
                                MachineData machine = new MachineData();
                                context.AddParameter("@Nodeid", param);
                                DataTable dtMachine = context.getData("GetMachineDetail", CommandType.StoredProcedure);
                                if (dtMachine.Rows.Count > 0)
                                {
                                    foreach (DataRow machineitem in dtMachine.Rows)
                                    {
                                        machine.MachineID = Convert.ToString(machineitem["NodeID"]);
                                        machine.MachineName = Convert.ToString(machineitem["MachineName"]);
                                        machine.MachineClass = Convert.ToString(machineitem["MachineClass"]);
                                        machine.RPMDriven = Convert.ToInt32(machineitem["RPMDriven"]);
                                        machine.PulseRevolution = Convert.ToInt32(machineitem["PulseRevolution"]);
                                        machine.MachineDetail = Convert.ToString(machineitem["MachineDetail"]);
                                    }
                                    ViewBag.NodeType = "Machine";
                                    Session["CurrentNodeID"] = machine.MachineID;
                                    return PartialView("_DisplayMachine", machine);
                                }
                                else
                                {
                                    TempData["NodeAvailability"] = "There is no any 'Machine' for display !";
                                    return PartialView("_DisplayMachine", machine);
                                }
                            }
                        }
                    case "Point":
                        {
                            using (DBClass context = new DBClass())
                            {
                                context.AddParameter("@Nodeid", param);
                                DataTable dtPoint = context.getData("GetPointDetail", CommandType.StoredProcedure);
                                if (dtPoint.Rows.Count > 0)
                                {
                                    foreach (DataRow pointitem in dtPoint.Rows)
                                    {
                                        DataColumnCollection column = dtPoint.Columns;

                                        point.PointID = Convert.ToString(pointitem["NodeID"]);
                                        point.PointName = Convert.ToString(pointitem["PointName"]);
                                        point.PointDetail = Convert.ToString(pointitem["PointDetail"]);
                                        point.ChannelA = ChannelInput[Convert.ToInt32(pointitem["Channel1"])];
                                        point.ChannelB = ChannelInput[Convert.ToInt32(pointitem["Channel2"])];
                                        point.RadioA = AmpMode[Convert.ToInt32(pointitem["SelectRadio1"])];
                                        point.RadioB = AmpMode[Convert.ToInt32(pointitem["SelectRadio2"])];
                                        point.EnvelopingFreq = EnvelopingFrequency[Convert.ToInt32(pointitem["EVFrequency"])];
                                        point.SpectralLines = Convert.ToString(pointitem["SpectrumLineNo"]);
                                        point.WindowType = WType[Convert.ToInt32(pointitem["WindowType"])];
                                        point.Fmin = Convert.ToInt32(pointitem["Fmin"]);
                                        point.Fmax = Convert.ToInt32(pointitem["Fmax"]);
                                        point.TrigerMode = TrigMode[Convert.ToInt32(pointitem["TriggerMode"])];
                                        point.AverageMode = AvgMode[Convert.ToInt32(pointitem["AvgMode"])];
                                        point.N = Convert.ToInt32(pointitem["N"]);
                                        point.Comments = Convert.ToString(pointitem["Comment"]);
                                        point.lstFaultFrequency = getFaultFrequencyList(point.PointID, Session["UserName"].ToString());
                                        point.lstBearingFrequency = getBearingFrequencyList(point.PointID, Session["UserName"].ToString());
                                        ViewBag.lstBearingMake = getBearingManufacture();
                                        if (point.BearingMake != null)
                                        {
                                            ViewBag.BearingModel = getBearingModel(point.BearingMake);
                                        }

                                    }
                                    ViewBag.NodeType = "Point";
                                    Session["CurrentNodeID"] = point.PointID;
                                    TempData["Nodeid"] = point.PointID;
                                }
                                else
                                {
                                    TempData["NodeAvailability"] = "There is no any 'Point' for display !";
                                }
                            }
                            return PartialView("_DisplayPoint", point);
                        }
                    default:
                        {
                            return PartialView("_DisplayPlant", plant);
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult UpdatePlant(PlantData plant)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@Nodeid", plant.PlantID);
                        context.AddParameter("@PlantName", plant.PlantName);
                        context.AddParameter("@PlantCategory", plant.PlantCategory);
                        context.AddParameter("@PlantDetail", plant.PlantDetail);
                        int i = context.ExecuteNonQuery("UpdatePlant", CommandType.StoredProcedure);
                        if (i > 0)
                        {
                            return PartialView("_DisplayPlant", plant);
                            //return RedirectToAction("RouteDetail", "Upload", new { param = plant.PlantID });
                        }
                    }
                    return PartialView("_DisplayPlant", plant);
                }
                else
                {
                    ModelState.AddModelError("", "Your data is not changed successfully! Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_DisplayPlant", plant);
        }

        [HttpPost]
        public ActionResult UpdateArea(AreaData area)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@Nodeid", area.AreaID);
                        context.AddParameter("@AreaName", area.AreaName);
                        context.AddParameter("@AreaDetail", area.AreaDetail);
                        int i = context.ExecuteNonQuery("UpdateArea", CommandType.StoredProcedure);
                        if (i > 0)
                        {
                            return PartialView("_DisplayArea", area);
                        }

                    }
                    return PartialView("_DisplayPlant", area);
                }
                else
                {
                    ModelState.AddModelError("", "Your data is not changed successfully! Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_DisplayPlant", area);
        }

        [HttpPost]
        public ActionResult UpdateTrain(TrainData train)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@Nodeid", train.TrainID);
                        context.AddParameter("@TrainName", train.TrainName);
                        context.AddParameter("@NoOfMachine", train.NoOfMachine);
                        context.AddParameter("@TrainDetail", train.TrainDetail);
                        int i = context.ExecuteNonQuery("UpdateTrain", CommandType.StoredProcedure);
                        if (i > 0)
                        {
                            return PartialView("_DisplayTrain", train);
                        }

                    }
                    return PartialView("_DisplayTrain", train);
                }
                else
                {
                    ModelState.AddModelError("", "Your data is not changed successfully! Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_DisplayTrain", train);
        }

        [HttpPost]
        public ActionResult UpdateMachine(MachineData machine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@Nodeid", machine.MachineID);
                        context.AddParameter("@MachineName", machine.MachineName);
                        context.AddParameter("@MachineClass", machine.MachineClass);
                        context.AddParameter("@RPMDriven", machine.RPMDriven);
                        context.AddParameter("@PulseRevolution", machine.PulseRevolution);
                        context.AddParameter("@MachineDetail", machine.MachineDetail);
                        int i = context.ExecuteNonQuery("UpdateMachine", CommandType.StoredProcedure);
                        if (i > 0)
                        {
                            return PartialView("_DisplayMachine", machine);
                        }

                    }
                    return PartialView("_DisplayMachine", machine);
                }
                else
                {
                    ModelState.AddModelError("", "Your data is not changed successfully! Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_DisplayMachine", machine);
        }

        [HttpPost]
        public ActionResult UpdatePoint(PointData point)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@Nodeid", point.PointID);
                        context.AddParameter("@PointName", point.PointName);
                        context.AddParameter("@PointDetail", point.PointDetail);
                        int i = context.ExecuteNonQuery("UpdatePoint", CommandType.StoredProcedure);
                        if (i > 0)
                        {
                            return PartialView("_DisplayPoint", point);
                        }

                    }
                    return PartialView("_DisplayPoint", point);
                }
                else
                {
                    ModelState.AddModelError("", "Your data is not changed successfully! Please try again.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_DisplayPoint", point);
        }

        public int getIndexofNumber(string cell)
        {
            int indexofNum = -1;
            foreach (char c in cell)
            {
                indexofNum++;
                if (Char.IsDigit(c))
                {
                    return indexofNum;
                }
            }
            return indexofNum;
        }

        public ActionResult DisplayGraph(string nodeid)
        {
            //ChartData.NodeID = Convert.ToString(TempData["Nodeid"]);
            ViewBag.NodeID = Convert.ToString(TempData["Nodeid"]);
            ViewBag.xUnit = ChartData.xAxisUnit;
            ViewBag.yUnit = ChartData.yAxisUnit;
            ViewBag.Overall = ChartData.overallValue;
            ViewBag.amplitudeUnit = ChartData.amplitudeUnit;
            ViewBag.NodeType = "Graph";
            ViewBag.Title = Session["PointName"];
            return PartialView("_DisplayGraph");
        }

        public JsonResult GetChartData(string FileID)
        {
            ArrayList BearingFaultFrequency = new ArrayList();
            BearingFaultFrequency = GetBearingFaultFrequencies(FileID);
            var data = ChartData.GetData(FileID, Session["UserName"].ToString(), BearingFaultFrequency);
            return Json(data, "True", JsonRequestBehavior.AllowGet);
        }
        BearingFF_Interface _BFFInterface = new BearingFF_Control();
        public ArrayList GetBearingFaultFrequencies(string spointid)
        {
            string BDIR = "1";
            string BDOR = "10";
            string BCA = "1";
            string BDRE = "1";
            string BNRE = "1";
            string Manufacture = "0";
            string BearingNumber = "0";
            string _BPFO = null;
            string _BPFI = null;
            string _BSF = null;
            string _FTF = null;
            string Sstatus = null;
            ArrayList BearingFaultFrequencies = new ArrayList();
            try
            {
                DataTable dt = new DataTable();
                using (DBClass context = new DBClass())
                {
                    dt = context.getData("select * from tblBearingDetail where PointID='" + spointid + "'", CommandType.Text);

                    foreach (DataRow dr in dt.Rows)
                    {
                        _BPFO = dr["BearingBPFO"].ToString();
                        _BPFI = dr["BearingBPFI"].ToString();
                        _BSF = dr["BearingBSF"].ToString();
                        _FTF = dr["BearingFTF"].ToString();
                        Sstatus = dr["Status"].ToString();
                    }
                }
                double NumberOfBalls = Convert.ToDouble(BNRE.ToString());
                double BearingPitchDiameter = Convert.ToDouble(((Convert.ToDouble(BDIR.ToString()) + Convert.ToDouble(BDOR.ToString())) / 2));
                double RollingElementDiameter = Convert.ToDouble(BDRE.ToString());
                double ContactAngle = Convert.ToDouble(BCA.ToString());
                double ShaftSpeed = 0;
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@Pointid", spointid);
                    DataTable dtMachineInfo = context.getData("getMachineRPMByPointID", CommandType.StoredProcedure);

                    int iRPM = Convert.ToInt32(dtMachineInfo.Rows[0]["RPMDriven"]);
                    int iPulse = Convert.ToInt32(dtMachineInfo.Rows[0]["PulseRevolution"]);
                    ShaftSpeed = (double)((double)iRPM / (double)(iPulse));
                }
                if (Sstatus == "true")
                {
                    BearingFaultFrequencies = _BFFInterface.CalculateBearingFaultFrequencies(ShaftSpeed, NumberOfBalls, BearingPitchDiameter, RollingElementDiameter, ContactAngle);
                }
                else
                {
                    BearingFaultFrequencies.Add("BPFO = " + Convert.ToString(Convert.ToDouble(Convert.ToDouble(_BPFO.ToString()) * ShaftSpeed) / 60));
                    BearingFaultFrequencies.Add("BPFI = " + Convert.ToString(Convert.ToDouble(Convert.ToDouble(_BPFI.ToString()) * ShaftSpeed) / 60));
                    BearingFaultFrequencies.Add("BSF = " + Convert.ToString(Convert.ToDouble(Convert.ToDouble(_BSF.ToString()) * ShaftSpeed) / 60));
                    BearingFaultFrequencies.Add("FTF = " + Convert.ToString(Convert.ToDouble(Convert.ToDouble(_FTF.ToString()) * ShaftSpeed) / 60));
                }
                //for (int i = 0; i < 4; i++)
                //{
                //    string[] ExtractFreqSingle = BearingFaultFrequencies[i].ToString().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                //    double Comparator = Convert.ToDouble(ExtractFreqSingle[1]);
                //    double dcomp = Comparator;

                //    for (int j = 0; j < iBearingHarmonics - 1; j++)
                //    {
                //        Comparator = dcomp * (j + 2);
                //        BearingFaultFrequencies.Add(ExtractFreqSingle[0].ToString() + Convert.ToString(j + 2) + "x =" + Comparator.ToString());
                //    }
                //}
            }
            catch (Exception ex)
            {
            }
            return BearingFaultFrequencies;
        }

        public double[] FindNearestPointIndex(double[] actualvalue, double[] xData)
        {
            double[] pointIndex = new double[actualvalue.Length];
            double minDistance = 0;
            int minIndex;
            double lValue = 0, rValue = 0, diff;
            for (int j = 0; j < actualvalue.Length; j++)
            {
                minIndex = Array.IndexOf(xData, actualvalue[j]);
                if (minIndex == -1)
                {
                    for (int i = 0; i < xData.Length; i++)
                    {
                        diff = xData[i] - actualvalue[j];
                        if (diff >= 0)
                        {
                            if (i == 0) { lValue = xData[i]; }
                            else { lValue = xData[i - 1]; }
                            lValue = xData[i - 1];
                            rValue = xData[i];
                            break;
                        }
                    }
                    if (Math.Abs(actualvalue[j] - lValue) > Math.Abs(actualvalue[j] - rValue))
                    {
                        minIndex = Array.IndexOf(xData, rValue);
                    }
                    else
                    {
                        minIndex = Array.IndexOf(xData, lValue);
                    }
                    pointIndex[j] = minIndex;
                }
                else
                {
                    pointIndex[j] = minIndex;
                }


            }
            return pointIndex;
        }

        [HttpGet]
        public ActionResult ChangeUnit(string id)
        {
            Unit unit = new Unit();

            if (id == "xUnit")
            {
                return PartialView("_changeXUnit", unit);
            }
            else { return PartialView("_changeYUnit", unit); }

        }

        public ActionResult GetUnitByType(string unitType)
        {
            List<Unit> lstUnit = new List<Unit>();
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@UnitType", unitType);
                    DataTable dtUnit = context.getData("GetUnitByType", CommandType.StoredProcedure);
                    if (dtUnit.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtUnit.Rows)
                        {
                            lstUnit.Add(new Unit
                            {
                                UnitID = Convert.ToInt32(dr["UnitID"]),
                                UnitName = Convert.ToString(dr["ParameterType"])
                            });
                        }
                    }
                }
                return Json(lstUnit, JsonRequestBehavior.AllowGet);
            }
            catch { return Json(lstUnit, JsonRequestBehavior.AllowGet); }
        }

        [HttpGet]
        public ActionResult AddFF(string PID)
        {
            FaultFrequency ff = new FaultFrequency();
            ff.PID = PID;
            return PartialView("_addFaultFrequency", ff);
        }

        [HttpPost]
        public ActionResult AddFF(string Pointid, string FName, string FValue)
        {
            string[] frequency = new string[4];
            if (Pointid != "" && FName != "" && FValue != "")
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@PointID", Pointid);
                    context.AddParameter("@FName", FName);
                    context.AddParameter("@FValue", FValue);
                    context.AddParameter("@UserID", Session["UserName"].ToString());
                    if (context.ExecuteNonQuery("addFaultFrequency", CommandType.StoredProcedure) > 0)
                    {
                        DataTable dt = context.getData("Select Top 1* from tblFaultFrequency where PointID = '" + Pointid + "' and UserID = '" + Session["UserName"].ToString() + "' order by FrequencyID desc;", CommandType.Text);
                        frequency[0] = Convert.ToString(dt.Rows[0]["FrequencyID"]);
                        frequency[1] = (string)dt.Rows[0]["FrequencyName"];
                        frequency[2] = (string)dt.Rows[0]["FrequencyValue"];
                        frequency[3] = "Success";
                        return Json(frequency, "True", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        frequency[3] = "Failed";
                        return Json(frequency, "True", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                frequency[3] = "Incomplete";
                return Json(frequency, "True", JsonRequestBehavior.AllowGet);
            }

        }

        private List<FaultFrequency> getFaultFrequencyList(string pointid, string userid)
        {
            List<FaultFrequency> lstFF = new List<FaultFrequency>();
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@PointID", pointid);
                    context.AddParameter("@UserName", userid);
                    DataTable dtFF = context.getData("getFalutFrequencyListbyPointID", CommandType.StoredProcedure);
                    if (dtFF.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtFF.Rows)
                        {
                            lstFF.Add(new FaultFrequency
                            {
                                FaultFreqID = Convert.ToInt16(dr["FrequencyID"]),
                                FaultFreqName = Convert.ToString(dr["FrequencyName"]),
                                FaultFreqValue = Convert.ToString(dr["FrequencyValue"])
                            });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstFF;
        }

        public List<BearingData> getBearingFrequencyList(string pointid, string userid)
        {
            List<BearingData> lstBFF = new List<BearingData>();
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@PointID", pointid);
                    DataTable dtBFF = context.getData("getBearingFrequencyListbyPointID", CommandType.StoredProcedure);
                    if (dtBFF.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtBFF.Rows)
                        {
                            lstBFF.Add(new BearingData
                            {
                                BearingID = Convert.ToString(dr["ID"]),
                                BearingMake = Convert.ToString(dr["BearingMake"]),
                                BearingNumber = Convert.ToString(dr["BearingModel"]),
                                Balls = Convert.ToInt32(dr["BearingBalls"]),
                                FTF = Convert.ToDouble(dr["BearingFTF"]),
                                BSF = Convert.ToDouble(dr["BearingBSF"]),
                                BPFO = Convert.ToDouble(dr["BearingBPFO"]),
                                BPFI = Convert.ToDouble(dr["BearingBPFI"]),
                                BearingStatus = Convert.ToString(dr["Status"])
                            });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstBFF;
        }

        [HttpGet]
        public ActionResult AddBearingFrequency()
        {
            BearingData _berData = new BearingData();
            ViewBag.lstBearingMake = getBearingManufacture();
            return PartialView("_addBearing", _berData);
        }
        private List<BearingData> getBearingManufacture()
        {
            List<BearingData> lstBearingMake = new List<BearingData>();
            using (DBClass context = new DBClass())
            {
                DataTable dtBearingMake = context.getData("Select distinct BearingMake from tblBearingMaster order by BearingMake asc", CommandType.Text);
                foreach (DataRow bearing in dtBearingMake.Rows)
                {
                    lstBearingMake.Add(new BearingData
                    {
                        BearingMake = Convert.ToString(bearing["BearingMake"])          
                    });
                }
            }
            return lstBearingMake;
        }
        public List<string> getBearingModel(string Make)
        {
            List<string> bNumber = new List<string>();
            using (DBClass context = new DBClass())
            {
                DataTable dtNumber = context.getData("Select distinct BearingModel from tblBearingMaster where BearingMake = '" + Make + "'", CommandType.Text);
                if (dtNumber.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNumber.Rows.Count; i++)
                    {
                        bNumber.Add(Convert.ToString(dtNumber.Rows[i]["BearingModel"]));
                    }
                }
            }
            return bNumber;
        }

        public ActionResult getBearingNumber(string Make)
        {
            List<string> bNumber = new List<string>();
            using (DBClass context = new DBClass())
            {
                DataTable dtNumber = context.getData("Select distinct BearingModel from tblBearingMaster where BearingMake = '" + Make + "'", CommandType.Text);
                if (dtNumber.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNumber.Rows.Count; i++)
                    {
                        bNumber.Add(Convert.ToString(dtNumber.Rows[i]["BearingModel"]));
                    }
                }
            }
            return Json(bNumber, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadRoute(string FileID)
        {
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("Select * from tblFileData where FileID = " + FileID + "", CommandType.Text);
                string fileName = null;
                if (Convert.ToString(dt.Rows[0]["FileType"]) == "Route")
                {
                    fileName = Convert.ToString(dt.Rows[0]["FileName"]) + ".rar";
                }
                else
                {
                    fileName = Convert.ToString(dt.Rows[0]["FileName"]);
                }
                var sDocument = Server.MapPath(fileLocation + fileName);
                if (!System.IO.File.Exists(sDocument))
                {
                    return HttpNotFound();
                }
                return File(sDocument, "application/rar", fileName);
            }
        }

        public ActionResult getBearingData(string Make, string numberID, string PointID)
        {
            string[] bDetail = new string[9];
            try
            {
                using (DBClass context = new DBClass())
                {
                    DataTable dtBearingData = context.getData("Select * from tblBearingMaster where BearingMake = '" + Make + "' and BearingModel ='" + numberID + "'", CommandType.Text);
                    if (dtBearingData.Rows.Count > 0)
                    {
                        bDetail[0] = Convert.ToString(dtBearingData.Rows[0]["BearingID"]);
                        bDetail[1] = Convert.ToString(dtBearingData.Rows[0]["BearingMake"]);
                        bDetail[2] = Convert.ToString(dtBearingData.Rows[0]["BearingModel"]);
                        bDetail[3] = Convert.ToString(dtBearingData.Rows[0]["BearingBalls"]);
                        bDetail[4] = Convert.ToString(dtBearingData.Rows[0]["BearingFTF"]);
                        bDetail[5] = Convert.ToString(dtBearingData.Rows[0]["BearingBSF"]);
                        bDetail[6] = Convert.ToString(dtBearingData.Rows[0]["BearingBPFO"]);
                        bDetail[7] = Convert.ToString(dtBearingData.Rows[0]["BearingBPFI"]);
                        bDetail[8] = "Success";
                    }
                    else
                    {
                        bDetail[8] = "Failed";
                    }
                }
                if (bDetail[8] == "Success")
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@Pointid", PointID);
                        context.AddParameter("@BearingMake", Convert.ToString(bDetail[1]));
                        context.AddParameter("@BearingModel", Convert.ToString(bDetail[2]));
                        context.AddParameter("@BearingBalls", Convert.ToInt16(bDetail[3]));
                        context.AddParameter("@BearingFTF", Convert.ToDouble(bDetail[4]));
                        context.AddParameter("@BearingBSF", Convert.ToDouble(bDetail[5]));
                        context.AddParameter("@BearingBPFO", Convert.ToDouble(bDetail[6]));
                        context.AddParameter("@BearingBPFI", Convert.ToDouble(bDetail[7]));
                        context.AddParameter("@Status", "false");
                        context.ExecuteNonQuery("AddBearingDataByPointID", CommandType.StoredProcedure);
                        return Json(bDetail, "True", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(bDetail, "True", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {

            }
            return Json(bDetail, "True", JsonRequestBehavior.AllowGet);
        }

        public ActionResult getBearingDataPhysical(string PointID, string InDia, string OutDia, string ConAnagel, string rDia, string rElement)
        {
            string[] bDetail = new string[9];
            try
            {
                double NumberOfBalls = Convert.ToDouble(rElement);
                double BearingPitchDiameter = Convert.ToDouble((Convert.ToDouble(InDia) + Convert.ToDouble(OutDia)) / 2);
                double RollingElementDiameter = Convert.ToDouble(rDia);
                double ContactAngle = Convert.ToDouble(ConAnagel);
                double ShaftSpeed = 1;

                ArrayList BearingFaultFrequencies = _BFFInterface.CalculateBearingFaultFrequencies(ShaftSpeed, NumberOfBalls, BearingPitchDiameter, RollingElementDiameter, ContactAngle);
                bDetail[0] = null;
                bDetail[1] = "Default";
                bDetail[2] = "Default";
                bDetail[3] = Convert.ToString(NumberOfBalls);
                bDetail[4] = Convert.ToString(Math.Round(_BFFInterface._FTF, 2).ToString());
                bDetail[5] = Convert.ToString(Math.Round(_BFFInterface._BSF).ToString());
                bDetail[6] = Convert.ToString(Math.Round(_BFFInterface._BPFO).ToString());
                bDetail[7] = Convert.ToString(Math.Round(_BFFInterface._BPFI).ToString());
                bDetail[8] = "Success";
                if (bDetail[8] == "Success")
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@Pointid", PointID);
                        context.AddParameter("@BearingMake", Convert.ToString(bDetail[1]));
                        context.AddParameter("@BearingModel", Convert.ToString(bDetail[2]));
                        context.AddParameter("@BearingBalls", Convert.ToInt16(bDetail[3]));
                        context.AddParameter("@BearingFTF", Convert.ToDouble(bDetail[4]));
                        context.AddParameter("@BearingBSF", Convert.ToDouble(bDetail[5]));
                        context.AddParameter("@BearingBPFO", Convert.ToDouble(bDetail[6]));
                        context.AddParameter("@BearingBPFI", Convert.ToDouble(bDetail[7]));
                        context.AddParameter("@Status", "true");
                        context.ExecuteNonQuery("AddBearingDataByPointID", CommandType.StoredProcedure);
                        return Json(bDetail, "True", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(bDetail, "True", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {

            }
            return Json(bDetail, "True", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPreviosData()
        {
            return View();
        }

        public ActionResult GetFileData()
        {
            var FileData = Json(FileDetail.getPreviousFileData(Convert.ToString(Session["UserRole"])), JsonRequestBehavior.AllowGet);
            return FileData;
        }

        public ActionResult getAlarmData(string AlarmName, string PointID)
        {
            string[] AlarmDetail = new string[5];
            using (DBClass context = new DBClass())
            {
                DataTable dtAlarm = context.getData("Select * from tblAlarmMaster where AlarmName = '" + AlarmName + "'", CommandType.Text);
                if (dtAlarm.Rows.Count > 0)
                {
                    AlarmDetail[0] = Convert.ToString(dtAlarm.Rows[0]["AlarmID"]);
                    AlarmDetail[1] = Convert.ToString(dtAlarm.Rows[0]["AlarmName"]);
                    AlarmDetail[2] = Convert.ToString(dtAlarm.Rows[0]["HighValue"]);
                    AlarmDetail[3] = Convert.ToString(dtAlarm.Rows[0]["LowValue"]);
                    AlarmDetail[4] = "Success";
                }
            }
            if (AlarmDetail[4] == "Success")
            {
                using (DBClass context = new DBClass())
                {
                    context.ExecuteNonQuery("Update tblPoint set PointAlarmID = " + Convert.ToInt32(AlarmDetail[0]) + " where NodeID = '" + PointID + "'", CommandType.Text);
                }
            }
            return Json(AlarmDetail, "True", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateReport(string ReportName)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Areas/RemoteAnalysis/rptAlarm.rdlc");
            List<clsReport> lstOverallData = new List<clsReport>();

            using (DBClass context = new DBClass())
            {
                context.AddParameter("@FileID", Convert.ToInt32(Session["FileID"]));
                context.AddParameter("@UserID", Session["UserName"]);
                DataTable dtReportData = context.getData("GetRouteDetailForReport", CommandType.StoredProcedure);
                if (dtReportData.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtReportData.Rows)
                    {
                        try
                        {
                            lstOverallData.Add(new clsReport
                            {
                                FileID = Convert.ToInt32(dr["Fileid"]),
                                PlantID = Convert.ToString(dr["PlantID"]),
                                PlantName = Convert.ToString(dr["PlantName"]),
                                AreaID = Convert.ToString(dr["AreaID"]),
                                AreaName = Convert.ToString(dr["AreaName"]),
                                TrainID = Convert.ToString(dr["TrainID"]),
                                TrainName = Convert.ToString(dr["TrainName"]),
                                MachineID = Convert.ToString(dr["MachineID"]),
                                MachineName = Convert.ToString(dr["MachineName"]),
                                PointID = Convert.ToString(dr["PointID"]),
                                PointName = Convert.ToString(dr["PointName"]),
                                AalrmID = Convert.ToInt32(dr["PointAlarmID"]),
                                AlarmName = Convert.ToString(dr["AlarmName"]),
                                HighValue = Convert.ToDouble(dr["HighValue"]),
                                LowValue = Convert.ToDouble(dr["LowValue"]),
                                MeasurementTime = Convert.ToDateTime("2018-04-17 14:44:02.293"),
                                OverallChnlA = Convert.ToDouble(dr["OverallValueChnlA"]),
                                UnitChnlA = Convert.ToString(dr["OverallUnitChnlA"]),
                                OverallChnlB = Convert.ToDouble(dr["OverallValueChnlB"]),
                                UnitChnlB = Convert.ToString(dr["OverallUnitChnlB"]),
                                DataID = Convert.ToInt32(dr["DataID"])
                            });
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                }
            }
            ReportDataSource reportDataSource = new ReportDataSource("dsOverall", lstOverallData);
            //reportDataSource.Name = "dsOverall";
            //reportDataSource.Value = lstOverallData;

            localReport.DataSources.Add(reportDataSource);
            localReport.Refresh();
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
            string deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "<PageTitle> Report</PageTitle>" +
                "  <PageWidth>9.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0.5in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 
            ViewBag.Title = "Alarm Report";
            return File(renderedBytes, mimeType);
            //if (format == null)
            //{
            //    return File(renderedBytes, "image/jpeg");
            //}
            //else if (format == "PDF")
            //{
            //    return File(renderedBytes, mimeType);
            //}
            //else
            //{
            //    return File(renderedBytes, "image/jpeg");
            //}
        }
    }
}
