using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace VibExchange.Models
{
    public class Login
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string Query { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        //[Required]
        public string Designation { get; set; }

       // [Required(ErrorMessage = "Please select a Deprtment !")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Email ID")]
        [EmailAddress]
        [System.Web.Mvc.Remote("EmailIDExist", "Home", HttpMethod = "POST", ErrorMessage = "This EmailID has been already register.Please enter another emailid ! ")]
        public string Email { get; set; }

        [Required]
        [System.Web.Mvc.Remote("UserNameExist", "Home", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
      
        [Display(Name = "Phone No.")]
        public string phoneno { get; set; }

        public string  Experience { get; set; }

        public string Address { get; set; }

        public virtual string Country { get; set; }

        public virtual string State { get; set; }

        public virtual string City { get; set; }

         [Display(Name = "Academic Education")]
        public string Academic { get; set; }

         [Display(Name = "Graduation")]
        public string graduation { get; set; }

         [Display(Name = "Post Graduation")]
        public string postraduation { get; set; }

         [Display(Name = "Certificate (if any)")]
        public string certification { get; set; }

         [Display(Name = "Specialization")]
         public List<System.Web.Mvc.SelectListItem> specialization { get; set; }

         public string specs { get; set; }
         public string EmpID { get; set; }
         public string MimeType { get; set; } 

         [System.Web.Mvc.AllowHtml]
         public string Contents { get; set; }
         public byte[] Image { get; set; }
         public string imageSrc { get; set; }

        //[Required(ErrorMessage = "Please select one from them.")]
        [Display(Name = "Have you company ?")]
        public string companyif { get; set; }

        [Display(Name = "Company Name")]
        public string companyName { get; set; }

        [Display(Name = "Address")]
        public string compAddress { get; set; }

        [Display(Name = "City")]
        public string cmpCity { get; set; }

        [Display(Name = "State")]
        public string cmpState { get; set; }

        [Display(Name = "Country")]
        public string cmpCountry { get; set; }

        [Display(Name="Office Phone No.")]
        public string  OfficeNo { get; set; }

        [Display(Name = "Website")]
        public string cmpWebsite { get; set; }

        public bool isTrue
        { get { return true; } }

        [Required]
        [Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
        public bool TermsAndConditions { get; set; }

        public List<RegisterModel> AllConsultant { get; set; }
    }
}