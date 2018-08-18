
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using VibExchange.Filters;
using VibExchange.Models;

namespace VibExchange.Controllers
{
    [InitializeSimpleMembershipAttribute]
    public class ClientsController : Controller
    {
        UploadFile client = new UploadFile();
        private UsersContext db = new UsersContext();
        public ActionResult ClientList()
        {
            var clients = ClientsRecord.GetClients();
            ViewBag.ClientsList = clients;
            return View(clients);
        }

        public ActionResult AllUserList()
        {
            return View();
        }

        public ActionResult getUserData()
        {
            var UserList = ClientsRecord.AllUserList();
            var UserDat = Json(UserList, JsonRequestBehavior.AllowGet);
            return UserDat;
        }

        public ActionResult ShowUserDetail(string UserName)
        {
            UserDetail ud = new UserDetail();
            using (DBClass context = new DBClass())
            {
                context.AddParameter("@UserName",UserName);
              DataTable dtUser = context.getData("getUserData", CommandType.StoredProcedure);
              if (dtUser.Rows.Count > 0)
              {
                  ud.FullName = Convert.ToString(dtUser.Rows[0]["FullName"]);
                  ud.EmailID = Convert.ToString(dtUser.Rows[0]["EmailID"]);
                  ud.Phone = Convert.ToString(dtUser.Rows[0]["Phone"]);
                  ud.Company = Convert.ToString(dtUser.Rows[0]["Company"]);
                  ud.UserRole = Convert.ToString(dtUser.Rows[0]["Role"]);
                  ud.Address = Convert.ToString(dtUser.Rows[0]["Address"]);
                  ud.City = Convert.ToString(dtUser.Rows[0]["City"]);
                  ud.State = Convert.ToString(dtUser.Rows[0]["State"]);
                  ud.Country = Convert.ToString(dtUser.Rows[0]["Country"]);
              }
            }
            return View("_ShowUserDetail", ud);
        }

        public ActionResult Detail(int id)
        {
            UploadFile uploadMachine = new UploadFile();
            using (DBClass context = new DBClass())
            {
                context.AddParameter("@FileID", id);
                DataTable dt = context.getData("getCurrentFileDetailByID", CommandType.StoredProcedure);

                //if (Convert.ToString(dt.Rows[0]["FileType"]) == "Data")
                //{
                //    filetype = true;
                //}
                uploadMachine.FileID = Convert.ToInt32(dt.Rows[0]["ID"]);
                uploadMachine.fileName1 = Convert.ToString(dt.Rows[0]["FileName"]);
                uploadMachine.Instrument = Convert.ToString(dt.Rows[0]["InstrumentUsed"]);
                uploadMachine.AnalysisType = Convert.ToString(dt.Rows[0]["AnalysisType"]);
                uploadMachine.Description = Convert.ToString(dt.Rows[0]["Description"]);
            }
            ViewBag.FileID = uploadMachine.FileID;
            return View(uploadMachine);
            //using (DBClass context = new DBClass())
            //{
            //    DataTable data = context.getData("select * from UploadData where ID = '" + id + "'", CommandType.Text);
            //    if (Request.IsAjaxRequest())
            //    {
            //        upload.fileName1 = Convert.ToString(data.Rows[0]["FileName"]);
            //        upload.Instrument = Convert.ToString(data.Rows[0]["InstrumentUsed"]);
            //        upload.machine1 = upload.GetList(id);
            //        upload.Description = Convert.ToString(data.Rows[0]["Description"]);
            //        upload.AnalysisType = Convert.ToString(data.Rows[0]["AnalysisType"]);
            //        ViewBag.Message = "Your File Detail";
            //        var machineDetail = upload;
            //        ViewBag.MachineDetail = machineDetail;
            //        return PartialView("_FileDetail", upload);
            //    }
            //    else
            //    { return PartialView("_FileDetail", upload); }
            //}
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            UploadFile upload = new UploadFile();
            EditFile client = new EditFile();
            using (DBClass context = new DBClass())
            {
                DataTable data = context.getData("select * from UploadData where ID = '" + id + "'", CommandType.Text);
                //HttpPostedFileBase filename = upload.fileName;
                string fileName = Convert.ToString(data.Rows[0]["FileName"]);
                string fname = Session["UserName"].ToString() + "@" + Convert.ToString(data.Rows[0]["FileName"]);
                var path = Path.Combine(Server.MapPath("~/Files"), fname);
                //if (System.IO.File.Exists(path))
                //{
                //    ModelState.AddModelError("File", "This file is already exist!");
                //}

                var filecontent = GetFileFromDisk(fname);
                string filetype = null;
                if (Request.IsAjaxRequest())
                {
                    filetype = Convert.ToString(data.Rows[0]["FileType"]);
                    if (filetype == "Data")
                    {
                        ViewBag.FileType = "Data";
                        //upload.FileType = ViewBag.FileType;
                    }
                    upload.TotalPoint = Convert.ToInt32(data.Rows[0]["PointCount"]);
                    upload.currentfile = Convert.ToString(data.Rows[0]["FileName"]);
                    upload.Instrument = Convert.ToString(data.Rows[0]["InstrumentUsed"]);
                    upload.AnalysisType = Convert.ToString(data.Rows[0]["AnalysisType"]);
                    upload.Description = Convert.ToString(data.Rows[0]["Description"]);
                    ViewBag.Message = "Edit Your File";
                    return PartialView("_UploadData", upload);

                }
                else
                {
                    filetype = Convert.ToString(data.Rows[0]["FileType"]);
                    if (filetype == "Image")
                    {
                        ViewBag.FileType = "Image";
                        //upload.FileType = ViewBag.FileType;
                    }
                    upload.TotalPoint = Convert.ToInt32(data.Rows[0]["PointCount"]);
                    upload.currentfile = Convert.ToString(data.Rows[0]["FileName"]);
                    upload.Instrument = Convert.ToString(data.Rows[0]["InstrumentUsed"]);
                    upload.AnalysisType = Convert.ToString(data.Rows[0]["AnalysisType"]);
                    upload.Description = Convert.ToString(data.Rows[0]["Description"]);
                    ViewBag.Message = "Edit Your File";
                    return PartialView("_UploadData", upload);
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formcollection)
        {
            UploadFile upload = new UploadFile();
            var file = HttpContext.Request.Files["FiletoChange"];
            string fileName = null; ;
            if (file.ContentLength > 0)
            {
                fileName = Path.GetFileName(file.FileName);
            }
            else
            {
                fileName = formcollection["currentfile"];
            }
            string fname = Session["UserName"].ToString() + "@" + fileName;
            double TotalCost = 0.0;
            var path = Path.Combine(Server.MapPath("~/Files"), fname);
            using (DBClass context = new DBClass())
            {
                var fil = formcollection["fileType"];
                var analysis = formcollection["AnalysisType"];
                string analysisType = null;
                int totalPoint = 0;
                totalPoint = Convert.ToInt32(formcollection["PointCount"]);

                switch (analysis)
                {
                    case "Indicative Analysis":
                        {
                            analysisType = "Indicative Analysis";

                            DataTable dt = context.getData("Select * from AnalysisCost where ServiceType='" + analysisType + "'", CommandType.Text);
                            if (fil == "Data")
                            { TotalCost = Convert.ToDouble(totalPoint) * Convert.ToDouble(dt.Rows[0]["CostPerPoint"]); }
                            else { TotalCost = Convert.ToDouble(dt.Rows[0]["CostPerGraphOrImage"]); }
                            break;

                        }
                    case "VibAnalyst Analysis":
                        {
                            analysisType = "VibAnalyst Analysis";
                            DataTable dt = context.getData("Select * from AnalysisCost where ServiceType='" + analysisType + "'", CommandType.Text);
                            if (fil == "Data")
                            { TotalCost = Convert.ToDouble(totalPoint) * Convert.ToDouble(dt.Rows[0]["CostPerPoint"]); }
                            else
                            { TotalCost = Convert.ToDouble(dt.Rows[0]["CostPerGraphOrImage"]); }
                            break;
                        }
                    case "Expert Analysis":
                        {
                            analysisType = "Expert Analysis";
                            DataTable dt = context.getData("Select * from AnalysisCost where ServiceType='" + analysisType + "'", CommandType.Text);
                            if (fil == "Data")
                            { TotalCost = Convert.ToDouble(totalPoint) * Convert.ToDouble(dt.Rows[0]["CostPerPoint"]); }
                            else
                            { TotalCost = Convert.ToDouble(dt.Rows[0]["CostPerGraphOrImage"]); }
                            break;
                        }
                }
                context.AddParameter("@ID", id);
                context.AddParameter("@FileName", fileName);
                context.AddParameter("@InstrumentUsed", formcollection["Instrument"]);
                context.AddParameter("@Description", formcollection["Description"]);
                context.AddParameter("@AnalysisType", formcollection["AnalysisType"]);
                context.AddParameter("@UploadDate", DateTime.Now);
                context.AddParameter("@AnalysisCost", TotalCost);
                context.AddParameter("@PointCount", totalPoint);
                context.AddParameter("@FileType", fil);
                if (context.ExecuteNonQuery("updateUploadFile", CommandType.StoredProcedure) > 0)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                        file.SaveAs(path);
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";
                        return RedirectToAction("ClientList", "Clients");
                    }
                    else
                    {
                        file.SaveAs(path);
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";
                        return RedirectToAction("ClientList", "Clients");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");
                }
            }
            return RedirectToAction("ClientList", "Clients");
        }

        public FileResult GetFileFromDisk(string fname)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "UploadDownloadData/Files/";
            return File(path + fname, "application/octet-stream", fname);
        }

        [HttpGet]
        public ActionResult UploadReport(int id)
        {
            UploadReport report = new UploadReport();
            using (DBClass context = new DBClass())
            {
                DataTable data = context.getData("select ud.FileName ,us.uCompanyName from UploadData ud left join UserDetail us on ud.UserName=us.UserName where ud.ID = '" + id + "'", CommandType.Text);
                report.FileName = Convert.ToString(data.Rows[0]["FileName"]);
                report.Company = Convert.ToString(data.Rows[0]["uCompanyName"]);
                ViewBag.Message = "Upload Report";
            }
            return PartialView("_UploadReport", report);
        }

        [HttpPost]
        public ActionResult UploadReport(int id, FormCollection formcollection)
        {
            bool sts = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Files["UploadedReport"];
                if (file == null)
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
                else if (file.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 100; //100 MB
                    string[] AllowedFileExtensions = new string[] { ".pdf", ".docx", ".txt", ".jpg", ".png", ".doc" };
                    string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));

                    }
                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB. Please zip your file and upload agail.");
                    }
                    else
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        string fname = id + "@" + fileName;
                        var path = Path.Combine(Server.MapPath("~/Reports"), fname);
                        using (DBClass context = new DBClass())
                        {
                            context.AddParameter("@ID", id);
                            context.AddParameter("@EmpID", Session["UserName"].ToString());
                            context.AddParameter("@ReportFile", fileName);
                            if (context.ExecuteNonQuery("addReport", CommandType.StoredProcedure) > 0)
                            {
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                    file.SaveAs(path);
                                    ModelState.Clear();
                                    ViewBag.Message = "Report uploaded successfully";
                                    return RedirectToAction("ClientList", "Clients");
                                    //ModelState.AddModelError("File", "This file is already exist!");
                                }
                                else
                                {
                                    file.SaveAs(path);
                                    ModelState.Clear();
                                    ViewBag.Message = "Report uploaded successfully";
                                    return RedirectToAction("ClientList", "Clients");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");
                            }
                        }
                    }
                }
            }
            UploadReport report = new UploadReport();
            using (DBClass context = new DBClass())
            {
                DataTable data = context.getData("select ud.FileName ,us.uCompanyName from UploadData ud left join UserDetail us on ud.UserName=us.UserName where ud.ID = '" + id + "'", CommandType.Text);
                report.FileName = Convert.ToString(data.Rows[0]["FileName"]);
                report.Company = Convert.ToString(data.Rows[0]["uCompanyName"]);
                ViewBag.Message = "Upload Report";
            }
            return PartialView("_UploadReport", report);
        }

        //[HttpPost]
        //public ActionResult EditFileDeatil(ClientsRecord client, string Command)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return PartialView("_EditEmployee", emp);
        //    }
        //    else
        //    {
        //        Employee empObj = new Employee();
        //        empObj.ID = emp.Id;
        //        empObj.Emp_ID = emp.Emp_ID;
        //        empObj.Name = emp.Name;
        //        empObj.Dept = emp.Dept;
        //        empObj.City = emp.City;
        //        empObj.State = emp.State;
        //        empObj.Country = emp.Country;
        //        empObj.Mobile = emp.Mobile;

        //        bool IsSuccess = mobjModel.UpdateEmployee(empObj);
        //        if (IsSuccess)
        //        {
        //            TempData["OperStatus"] = "Employee updated succeessfully";
        //            ModelState.Clear();
        //            return RedirectToAction("SearchEmployee", "ManageEmployee");
        //        }
        //    }

        //    return PartialView("_EditEmployee");
        //}

        //public ActionResult ViewFileDetail(int id)
        //{
        //    var data = mobjModel.GetEmployeeDetail(id);
        //    if (Request.IsAjaxRequest())
        //    {
        //        EmployeeModel empObj = new EmployeeModel();

        //        empObj.Emp_ID = data.Emp_ID;
        //        empObj.Name = data.Name;
        //        empObj.Dept = data.Dept;
        //        empObj.City = data.City;
        //        empObj.State = data.State;
        //        empObj.Country = data.Country;
        //        empObj.Mobile = data.Mobile;

        //        return View("_EmployeeDetail", empObj);
        //    }
        //    else

        //        return View(data);
        //}

        public ActionResult Delete(int id)
        {
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("Select UserName,FileName from UploadData where ID = '" + id + "'", CommandType.Text);
                string fileName = Convert.ToString(dt.Rows[0]["UserName"]) + '@' + Convert.ToString(dt.Rows[0]["FileName"]);
                var path = Path.Combine(Server.MapPath("~/Files"), fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            bool check = ClientsRecord.DeleteFile(id, Session["UserName"].ToString());
            var clients = ClientsRecord.GetClients();
            ViewBag.ClientsList = clients;
            return RedirectToAction("ClientList");

        }

        private string fileLocation = "~/Files/";
        public ActionResult DownloadFile(int id)
        {
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("Select UserName,FileName from UploadData where ID = '" + id + "'", CommandType.Text);

                string fileName = Convert.ToString(dt.Rows[0]["UserName"]) + '@' + Convert.ToString(dt.Rows[0]["FileName"]);
                var sDocument = Server.MapPath(fileLocation + fileName);
                if (!System.IO.File.Exists(sDocument))
                {
                    return HttpNotFound();
                }
                return File(sDocument, "application/docx", Convert.ToString(dt.Rows[0]["FileName"]));
            }
        }

        private string reportLocation = "~/Reports/";
        public ActionResult DownloadReport(int id)
        {
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("Select * from AnalysisReportData where FileID = '" + id + "'", CommandType.Text);

                string fileName = Convert.ToString(id) + '@' + Convert.ToString(dt.Rows[0]["ReportFile"]);
                var sDocument = Server.MapPath(reportLocation + fileName);
                if (!System.IO.File.Exists(sDocument))
                {
                    return HttpNotFound();
                }
                return File(sDocument, "application/docx", Convert.ToString(dt.Rows[0]["ReportFile"]));
            }
        }

        public ActionResult ShowDriveUnit(string MachineID)
        {
            UploadFile upload = new UploadFile();
            upload.DrivenUnitID = Convert.ToInt32(MachineID);
            TempData["MachineID"] = MachineID.ToString();
            ViewBag.DriveID = Convert.ToInt32(MachineID);

            using (DBClass context = new DBClass())
            {
                DataTable dt = new DataTable();
                context.AddParameter("@MachineID", MachineID);
                dt = context.getData("getDriveUnitByID", CommandType.StoredProcedure);
                upload.TrainName = Convert.ToString(dt.Rows[0]["MachineName"]);
                upload.MachineOrientaion = Convert.ToString(dt.Rows[0]["MachineOrientation"]);
                upload.MountingType = Convert.ToString(dt.Rows[0]["MountingType"]);
                upload.DEUnitType = Convert.ToString(dt.Rows[0]["DUName"]);
                upload.DURPM = Convert.ToString(dt.Rows[0]["RPM"]);
                upload.BearingLubrication = Convert.ToString(dt.Rows[0]["Ber_Lubrication"]);
                upload.bearingMake = Convert.ToString(dt.Rows[0]["Ber_Manufacture"]);
                upload.bearingNoDE = Convert.ToString(dt.Rows[0]["Ber_NumberDE"]);
                upload.bearingNoNDE = Convert.ToString(dt.Rows[0]["Ber_NumberNDE"]);
                upload.NoOfPoles = Convert.ToInt32(dt.Rows[0]["PolesCount"]);
                upload.Point_Count_DE = Convert.ToInt32(dt.Rows[0]["Point_Count"]);
            }
            return View("_DriveDetail", upload);
        }

        public ActionResult ShowDrivenUnit(string MachineID)
        {
            UploadFile upload = new UploadFile();
            using (DBClass context = new DBClass())
            {
                DataTable dt = new DataTable();
                context.AddParameter("@MachineID", MachineID);
                dt = context.getData("getDrivenUnitByID", CommandType.StoredProcedure);

                upload.NDEUnit1 = Convert.ToString(dt.Rows[0]["Machine_Name"]);
                upload.NDEType = Convert.ToString(dt.Rows[0]["Machine_Type"]);
                upload.TransmissionType = Convert.ToString(dt.Rows[0]["Transmission_Type"]);
                upload.BearingLubrication = Convert.ToString(dt.Rows[0]["Ber_Lubrication"]);
                upload.bearingMake = Convert.ToString(dt.Rows[0]["Ber_Manufacture"]);
                upload.bearingNoDE = Convert.ToString(dt.Rows[0]["Ber_DE"]);
                upload.bearingNoNDE = Convert.ToString(dt.Rows[0]["Ber_NDE"]);
                upload.bearingDEIn = Convert.ToString(dt.Rows[0]["Ber_DE_IN"]);
                upload.bearingDEOut = Convert.ToString(dt.Rows[0]["Ber_DE_OUT"]);
                upload.bearingNDEIn = Convert.ToString(dt.Rows[0]["Ber_NDE_IN"]);
                upload.bearingNDEOut = Convert.ToString(dt.Rows[0]["Ber_NDE_OUT"]);
                upload.bearingDEIdler = Convert.ToString(dt.Rows[0]["Ber_DE_Idle"]);
                upload.bearingNDEIdler = Convert.ToString(dt.Rows[0]["Ber_NDE_Idle"]);
                upload.InputRPM = Convert.ToDouble(dt.Rows[0]["Input_RPM"]);
                upload.OutputRPM = Convert.ToDouble(dt.Rows[0]["Output_RPM"]);
                upload.GearRatio = Convert.ToString(dt.Rows[0]["Gear_Ratio"]);
                upload.TeethCountInput = Convert.ToInt32(dt.Rows[0]["Teeth_Input_Count"]);
                upload.TeethCountOutput = Convert.ToInt32(dt.Rows[0]["Teeth_Output_Count"]);
                upload.TeethCountIdle = Convert.ToInt32(dt.Rows[0]["Teeth_Idler_Count"]);
                upload.NDERPM = Convert.ToDouble(dt.Rows[0]["RPM"]);
                upload.BladesorFins = Convert.ToInt32(dt.Rows[0]["BladesORFins"]);
                upload.BeltType = Convert.ToString(dt.Rows[0]["Belt_Type"]);
                upload.BeltCount = Convert.ToInt32(dt.Rows[0]["Belt_Count"]);
                upload.BeltLength = Convert.ToDouble(dt.Rows[0]["Belt_Length"]);
                upload.PitchDia = Convert.ToDouble(dt.Rows[0]["Pitch_Dia"]);
                upload.TeethCount = Convert.ToInt32(dt.Rows[0]["Teeth_Count"]);
                upload.Point_Count_NDE = Convert.ToInt32(dt.Rows[0]["Point_Count"]);
            }
            return View("_DrivenDetail", upload);
        }

        //public ActionResult Update(int id)
        //{
        //    if (HttpContext.Request.RequestType == "POST")
        //    {
        //        // Request is Post type; must be a submit
        //        var name = Request.Form["name"];
        //        var address = Request.Form["address"];
        //        var trusted = Request.Form["trusted"];

        //        // Get all of the clients
        //        var clints = ClientsRecord.GetClients();

        //        foreach (ClientsRecord client in clints)
        //        {
        //            // Find the client
        //            if (client.ID == id)
        //            {
        //                // Client found, now update his properties and save it.
        //                client.Name = name;
        //                client.Address = address;
        //                client.Trusted = Convert.ToBoolean(trusted);
        //                // Break through the loop
        //                break;
        //            }
        //        }

        //        // Update the clients in the disk
        //        System.IO.File.WriteAllText(ClientsRecord.ClientFile, JsonConvert.SerializeObject(clints));

        //        // Add the details to the View
        //        Response.Redirect("~/Client/Index?Message=Client_Updated");
        //    }


        //    // Create a model object.
        //    var clnt = new ClientsRecord();
        //    // Get the list of clients
        //    var clients = ClientsRecord.GetClients();
        //    // Search within the clients
        //    foreach (ClientsRecord client in clients)
        //    {
        //        // If the client's ID matches
        //        if (client.ID == id)
        //        {
        //            clnt = client;
        //        }
        //        // No need to further run the loop
        //        break;
        //    }
        //    if (clnt == null)
        //    {
        //        // No client was found
        //        ViewBag.Message = "No client was found.";
        //    }
        //    return View(clnt);
        //}


        public void Payment(int id, string amt)
        {
            string firstName = Session["UserName"].ToString();
            string amount = amt;
            string productInfo = Convert.ToString(id);
            string email = "";
            string phone = "";
            RemotePost myremotepost = new RemotePost();
            string key = "HKi5X9";
            string salt = "hUtCMrYk";
            myremotepost.Url = "https://secure.payu.in/_payment";
            myremotepost.Add("key", "HKi5X9");
            string txnid = Generatetxnid();
            myremotepost.Add("txnid", txnid);
            myremotepost.Add("amount", amount);
            myremotepost.Add("productinfo", productInfo);
            myremotepost.Add("firstname", firstName);
            myremotepost.Add("phone", phone);
            myremotepost.Add("email", email);
            myremotepost.Add("surl", "http://www.vibration-service.com/Clients/Return");//Change the success url here depending upon the port number of your local system.
            myremotepost.Add("furl", "http://www.vibration-service.com/Clients/Return");//Change the failure url here depending upon the port number of your local system.
            myremotepost.Add("service_provider", "payu_paisa");
            string hashString = key + "|" + txnid + "|" + amount + "|" + productInfo + "|" + firstName + "|" + email + "|||||||||||" + salt;
            string hash = Generatehash512(hashString);
            myremotepost.Add("hash", hash);
            myremotepost.Post();
        }

        //////////This method for convert all type of currency///////////////////////////

        public string[] CurrencyConvert(string Amount, string convertFrom, string ConvertTo)
        {
            string[] finalData = new string[2];
            //string convertRate = getConversionRate(convertFrom, ConvertTo);


            return finalData;
        }

        [HttpPost]
        public ActionResult Return(FormCollection form)
        {
            try
            {
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                string order_id = string.Empty;
                string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                if (form["status"].ToString() == "success")
                {
                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = ConfigurationManager.AppSettings["SALT"] + "|" + form["status"].ToString();
                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (form[merc_hash_var] != null ? form[merc_hash_var] : "");
                    }
                    Response.Write(merc_hash_string);
                    merc_hash = Generatehash512(merc_hash_string).ToLower();
                    if (merc_hash != form["hash"])
                    {
                        using (DBClass context = new DBClass())
                        {
                            int i = context.ExecuteNonQuery("Update PaymentDetail set Status = 'True' Where FileID =" + form["productinfo"].ToString() + " ", CommandType.Text);
                        }
                        return RedirectToAction("ClientList", "Clients");

                    }
                    else
                    {
                        order_id = Request.Form["txnid"];
                        using (DBClass context = new DBClass())
                        {
                            int i = context.ExecuteNonQuery("Update PaymentDetail set Status = 'True' Where FileID =" + form["productinfo"].ToString() + " ", CommandType.Text);
                        }
                        TempData["Error"] = "Your Payment has been done successfully.";
                        return RedirectToAction("ClientList", "Clients");
                    }
                }
                else
                {
                    TempData["Error"] = "Your Payment has not been done. Please try again !";
                    return RedirectToAction("ClientList", "Clients");
                }
            }
            catch
            {
                // return RedirectToAction("ClientList", "Clients");
            }

            return RedirectToAction("ClientList", "Clients");
        }

        public class RemotePost
        {
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();
            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
            }
        }

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }


        public string Generatetxnid()
        {
            Random rnd = new Random();
            string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            string txnid1 = strHash.ToString().Substring(0, 20);

            return txnid1;
        }

        public ActionResult SomeAction()
        {
            // ...
            return Json(
                new { Message = "Success Message!" },
                JsonRequestBehavior.AllowGet
            );
        }

    }
}
