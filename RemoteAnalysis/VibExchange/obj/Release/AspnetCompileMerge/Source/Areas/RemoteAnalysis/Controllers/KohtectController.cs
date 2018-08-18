using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VibExchange.Areas.RemoteAnalysis.Models;
using VibExchange.Models;
using System.Data;
using NUnrar.Archive;
using System.IO;
using NUnrar.Common;


namespace VibExchange.Areas.RemoteAnalysis.Controllers
{
    public class KohtectController : Controller
    {
        [HttpGet]
        public ActionResult GetAnalysisKohtect(int fileid)
        {
            FileDetail _file = new FileDetail();
            int noofmachine = _file.NoOfMachines;
            bool chkFile = CheckDuplicateFileRecord(fileid, Session["UserName"].ToString());
            if (!chkFile)
            {
                try
                {
                    _file.lstReturnData = GetFileToRead(fileid, Session["UserName"].ToString());
                    ViewBag.ReturnList = _file.lstReturnData;
                    Session["FileID"] = fileid;
                }
                catch { }
            }
            else { }
            return View();
        }
        public List<ReturnData> GetFileToRead(int FileID, string Userid)
        {
            bool status = false;
            RemoteAnalysisKohtect _kohtect = new RemoteAnalysisKohtect();
            List<ReturnData> rList = new List<ReturnData>();
            string filePath = null;
            string newpath = null;
            string filetype = null;
            try
            {
                if (Userid != null)
                {
                    using (DBClass _dbConnection = new DBClass())
                    {
                        _dbConnection.AddParameter("@FileID", FileID);
                        _dbConnection.AddParameter("UserID", Userid);
                        DataTable dtPath = _dbConnection.getData("GetFilePathbyFileID", CommandType.StoredProcedure);
                        filePath = Convert.ToString(dtPath.Rows[0]["FilePath"]);
                        filetype = Convert.ToString(dtPath.Rows[0]["FileType"]);
                        if (filetype != "File")
                        {
                            newpath = ExtractRARFile(filePath, Userid, FileID);
                            status = _kohtect.FileDataSave(FileID, Userid, newpath, filetype);
                        }
                        else
                        {
                            status = _kohtect.FileDataSave(FileID, Userid, filePath, filetype);
                        }
                        rList.Add(new ReturnData
                        {
                            Status = status,
                            FileID = FileID,
                            UserID = Userid,
                            MeasurementTime = _kohtect.MeasureTime,
                            UploadTime = DateTime.Now
                        });
                    }
                }
                else
                {
                    status = false;
                    ModelState.AddModelError("UserIDNotFound", "Please Logout and Login again to complete your process !");
                }
            }
            catch
            {
            }

            return rList;
        }

        public string ExtractRARFile(string filePath, string UserID, int Fileid)
        {
            try
            {
                string[] FileDetail = filePath.ToString().Split(new string[] { "\\", ".rar" }, StringSplitOptions.RemoveEmptyEntries);
                string fileName = FileDetail[FileDetail.Length - 1];
                string newPath = null;
                RarArchive file = RarArchive.Open(filePath);
                newPath = Path.Combine(Server.MapPath("~/Files"), fileName);
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@FileID", Fileid);
                    context.AddParameter("@UserID", UserID);
                    context.AddParameter("@FileName", fileName);
                    context.AddParameter("@NewPath", newPath);
                    context.ExecuteNonQuery("UpdateFileData", CommandType.StoredProcedure);
                }
                foreach (RarArchiveEntry rarFile in file.Entries)
                {
                    rarFile.WriteToDirectory(newPath, ExtractOptions.ExtractFullPath);
                }
                //if(System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}
                return newPath;
            }
            catch (Exception e) { throw e; }
        }

        public JsonResult CreateHierarchy()
        {
            List<TreeViewModel> TreeList = new List<TreeViewModel>();
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@FileID", Convert.ToInt32(Session["FileID"]));
                    DataTable dtHierarchy = context.getData("GetHierarchyData", CommandType.StoredProcedure);
                    ViewBag.FirstNodeID = Convert.ToString(dtHierarchy.Rows[0]["NodeID"]);
                    if (dtHierarchy.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtHierarchy.Rows)
                        {
                            TreeList.Add(new TreeViewModel
                            {
                                id = Convert.ToString(dr["NodeID"]),
                                text = Convert.ToString(dr["NodeText"]),
                                parent = Convert.ToString(dr["ParentID"])
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Json(TreeList, "True", JsonRequestBehavior.AllowGet);

        }
      
        private bool CheckDuplicateFileRecord(int fileid, string userid)
        {
            bool sts = false;
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@Fileid", fileid);
                    DataTable dt = context.getData("GetRecordedDataByFileID", CommandType.StoredProcedure);
                    if (dt.Rows.Count > 0) { sts = true; }
                }
            }
            catch (Exception ex)
            { throw ex; }
            return sts;
        }

        public ActionResult ReAnalysisKohtect(int FileID)
        {
            try
            {
                FileDetail _file = new FileDetail();
                Session["FileID"] = FileID;
                int noofmachine = _file.NoOfMachines;
                _file.lstReturnData = GetFileToAnalyse(FileID, Session["UserName"].ToString());
                ViewBag.ReturnList = _file.lstReturnData;
                Session["FileID"] = FileID;
            }
            catch (Exception ex) { throw ex; }
            return View("GetAnalysisKohtect");
        }

        public List<ReturnData> GetFileToAnalyse(int FileID, string Userid)
        {
            bool status = false;
            RemoteAnalysisKohtect _kohtect = new RemoteAnalysisKohtect();
            List<ReturnData> rList = new List<ReturnData>();
            try
            {
                if (Userid != null)
                {
                    using (DBClass _dbConnection = new DBClass())
                    {
                        _dbConnection.AddParameter("@FileID", FileID);
                        _dbConnection.AddParameter("UserID", Userid);
                        DataTable dtPath = _dbConnection.getData("GetFileData", CommandType.StoredProcedure);
                        rList.Add(new ReturnData
                        {
                            Status = true,
                            FileID = FileID,
                            UserID = Userid,
                            MeasurementTime = Convert.ToDateTime(dtPath.Rows[0]["MeasureTime"]),
                            UploadTime = Convert.ToDateTime(dtPath.Rows[0]["DateOfUpload"]),
                        });
                    }
                }
                else
                {
                    status = false;
                    ModelState.AddModelError("UserIDNotFound", "Please Logout and Login again to complete your process !");
                }
            }
            catch
            {
            }

            return rList;
        }
    }
}
