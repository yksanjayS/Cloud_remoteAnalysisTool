using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace VibExchange.Models
{
    public class Order
    {
        [Required]
        [Display(Name = "Name")]
        public string fullName { get; set; }

       
        [Display(Name = "EmailAddress")]
        [Required(ErrorMessage = "The email address is required")]
        [System.Web.Mvc.Remote("EmailIDExist", "Home", HttpMethod = "POST", ErrorMessage = "This EmailID has been already register.Please login if you have already registered ! ")]
        public string emailID { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Choose Password")]
        public string password { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string phone { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string EnquirySubject { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string EnquiryCategory { get; set; }

        [Required]
        [Display(Name = "Brief Your Requirnment")]
        public string EnquiryDetail { get; set; }

        public bool isTrue
        { get { return true; } }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
        public bool TermsAndConditions { get; set; }
    }

    public class Enquiry
    {
        [Display(Name = "ID")]
        public string EnquiryID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Company Name")]
        public string company { get; set; }

        [Display(Name = "Phone No.")]
        public string phone { get; set; }

        [Display(Name = "EmaiilID")]
        public string emailid { get; set; }

        [Display(Name = "Category")]
        public string Enq_Category { get; set; }

        [Display(Name = "Subject")]
        public string Enq_Subject { get; set; }

        [Display(Name = "Create Date")]
        public string Enq_CreateDate { get; set; }

        [Required]
        [Display(Name = "Brief Your Requirnment")]
        public string EnquiryDetail1 { get; set; }

        public int Buyer_Count { get; set; }

        public string eStatus { get; set; }

        public List<Enquiry> enquiryList { get; set; }


        public static List<Enquiry> getAllEnquiry(string UserRole)
        {
            List<Enquiry> enquiryList = new List<Enquiry>();  // creating list of model.
            string username = HttpContext.Current.Session["UserName"].ToString();
            DataTable dt = new DataTable();
            using (DBClass context = new DBClass())
            {
                if (UserRole == "Client")
                {
                    context.AddParameter("@UserName", username);
                    dt = context.getData("getEnquiryByUser", CommandType.StoredProcedure);
                }
                else
                {
                    dt = context.getData("getAllEnquiry", CommandType.StoredProcedure);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    string EnquiryStatus = "NO";
                    context.AddParameter("@EnquiryID", Convert.ToString(dr["ID"]));
                    context.AddParameter("@UserName", username);
                    DataSet dtStatus = context.ExecuteDataSet("getEnquiryStatus", CommandType.StoredProcedure);
                    if (dtStatus.Tables[0].Rows.Count > 0)
                    {
                        EnquiryStatus = "YES";
                    }

                    enquiryList.Add(new Enquiry
                    {
                        EnquiryID = Convert.ToString(dr["ID"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        Enq_Category = Convert.ToString(dr["EnquiryCategory"]),
                        Enq_Subject = Convert.ToString(dr["EnquirySubject"]),
                        EnquiryDetail1 = Convert.ToString(dr["EnquiryDetail"]),
                        Enq_CreateDate = Convert.ToString(dr["CreateDate"]),
                        Buyer_Count = Convert.ToInt32(dr["Buyer_Count"]),
                        eStatus = EnquiryStatus
                    });
                }
                return enquiryList;
            }
        }

        public List<Enquiry> getAllEnquiryData()
        {
            List<Enquiry> enquiryList = new List<Enquiry>();  // creating list of model.
            DataTable dt = new DataTable();
            using (DBClass context = new DBClass())
            {
                dt = context.getData("getAllEnquiry", CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    enquiryList.Add(new Enquiry
                    {
                        EnquiryID = Convert.ToString(dr["ID"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        Enq_Category = Convert.ToString(dr["EnquiryCategory"]),
                        Enq_Subject = Convert.ToString(dr["EnquirySubject"]),
                        EnquiryDetail1 = Convert.ToString(dr["EnquiryDetail"]),
                        Enq_CreateDate = Convert.ToString(dr["CreateDate"]),
                        Buyer_Count = Convert.ToInt32(dr["Buyer_Count"])
                    });
                }
                return enquiryList;
            }
        }

    }
}