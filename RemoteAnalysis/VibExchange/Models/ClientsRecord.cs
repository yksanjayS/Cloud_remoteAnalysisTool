
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace VibExchange.Models
{
    public class ClientsRecord
    {
        // public static string ClientFile = HttpContext.Current.Server.MapPath("~/App_Data/Clients.json");

        [Required]
        [Display(Name = "ID")]
        public int Serial { get; set; }

        
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Name { get; set; }
        public string UserRole { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        public string EmailID { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        [Display(Name = "File Name")]
        public string FileName { get; set; }


        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Payment")]
        public bool PaymentStatus { get; set; }

        [Required]
        [Display(Name = "Report")]
        public bool ReportStatus { get; set; }

        [Required]
        [Display(Name = "Analysis Cost")]
        public double AnalysisCost { get; set; }

        [Required]
        [Display(Name="Analysis Mode")]
        public string AnalysisMode { get; set; }

        [Required]
        [Display(Name = "Reporting Date")]
        public DateTime ReportingDate { get; set; }


        public static List<ClientsRecord> GetClients()
        {
            List<ClientsRecord> clientList = new List<ClientsRecord>();  // creating list of model.

            DataSet ds = new DataSet();
            char[] delimiterChars = { '@' };
            //string[] RolesForCurrentUser = Roles.GetAllRoles();
            //string[] user = Roles.GetUsersInRole(RolesForCurrentUser[0]);
            // string userName = Roles.GetRolesForUser();
            //string username = HttpContext.Current.User.Identity.Name;
            string username = "";
            if (HttpContext.Current.Session["UserName"] != null)
            {
                username = HttpContext.Current.Session["UserName"].ToString();
            }
            else
            {
                username = ""; 
            }
            using (DBClass context = new DBClass())
            {
                context.AddParameter("@username", username);
                ds = context.ExecuteDataSet("getUserRole", CommandType.StoredProcedure);
            }
            using(DBClass context = new DBClass())
            {
                context.AddParameter("@UserRole", ds.Tables[0].Rows[0]["UserRole"].ToString());
                context.AddParameter("@UserName", username);
                DataTable dt = context.getData("getFileWithStatus", CommandType.StoredProcedure);
                // loop for adding add from dataset to list<modeldata>
                foreach (DataRow dr in dt.Rows)
                {
                    //string[] fName = Convert.ToString(dr["FilePath"]).Split(delimiterChars);
                    clientList.Add(new ClientsRecord
                    {
                        Serial = Convert.ToInt32(dr["ID"]),
                        UserRole = ds.Tables[0].Rows[0]["UserRole"].ToString(),
                        Company = Convert.ToString(dr["uCompanyName"]),
                        Phone = Convert.ToString(dr["uMobile_No"]),
                        EmailID = Convert.ToString(dr["Email_ID"]),
                        FileName = Convert.ToString(dr["FileName"]),
                        PaymentStatus = Convert.ToBoolean(dr["Status"]),
                        ReportStatus = Convert.ToBoolean(dr["UploadStatus"]),
                        AnalysisCost = Convert.ToDouble(dr["Amount"]),
                        ReportingDate = Convert.ToDateTime(dr["ReportingTime"])
                    });
                }
                return clientList;
            }
        }

        public static List<ClientsRecord> AllUserList()
        {
            List<ClientsRecord> UserList = new List<ClientsRecord>();  
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("getAllUserList", CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    UserList.Add(new ClientsRecord
                    {
                        UserName = Convert.ToString(dr["UserName"]),
                        Name = dr["Name"].ToString(),
                        //Company = Convert.ToString(dr["Company"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        EmailID = Convert.ToString(dr["Email_ID"]),
                        CreatedDate = Convert.ToDateTime(dr["CreationDate"]),
                        UserRole = Convert.ToString(dr["UserRole"]),
                        
                    });
                }
                return UserList;
            }
        }
        public static bool DeleteFile(int fileID,string username)
        {
            bool sts = false;
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@FileID",fileID);
                    context.AddParameter("@userName", username);
                    if ((context.ExecuteNonQuery("deleteUploadFile", CommandType.StoredProcedure)) > 0)
                    { sts = true; }
                    else
                    { sts = false; }
                }
            }
            catch
            {
                return sts;
            }
            return sts;
        }
    }

    public class UploadReport
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        [Display(Name = "Analysis Report")]
        public HttpPostedFileBase Report { get; set; }
    }

    public class EditFile
    {
        [Required(ErrorMessage = "Please Upload File")]
        [FileExtensions(Extensions = "txt,doc,docx,pdf", ErrorMessage = "Please upload valid file format")]
        [Display(Name = "File")]
        public HttpContext EditFileName { get; set; }

        [Required]
        [Display(Name = "Instrument Used")]
        public string EditInstrument { get; set; }

        [Required]
        [Display(Name = "Analysis Type")]
        public string EditAnalysisType { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string EditDescription { get; set; }
    }

    public class UserDetail
    {
        [Display(Name="Full Name")]
        public string  FullName { get; set; }

        [Display(Name="Email Address")]
        public string EmailID { get; set; }

        public string Phone { get; set; }

        [Display(Name="User Role")]
        public string UserRole { get; set; }

        public string Company { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }


}