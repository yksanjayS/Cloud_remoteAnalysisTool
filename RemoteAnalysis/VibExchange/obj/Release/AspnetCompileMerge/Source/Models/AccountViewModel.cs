using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace VibExchange.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("ConnectionString")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class UserRegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string uName { get; set; }

        [Required]
        [System.Web.Mvc.Remote("UserNameExist", "Home", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        [Display(Name = "User Name")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Spaces is not allowed in UserName !")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [System.Web.Mvc.Remote("EmailIDExist", "Home", HttpMethod = "POST", ErrorMessage = "This EmailID has been already register.Please enter another emailid ! ")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string uCompany { get; set; }

        [Display(Name = "Address")]
        public string uCompanyAddress { get; set; }

        public virtual string Country { get; set; }

        public virtual string State { get; set; }

        public virtual string City { get; set; }

        [Display(Name = "Fax No.")]
        public string uFax { get; set; }

        [Required]
        [Display(Name = "Contact No.")]
        public string uMobile { get; set; }

        [Display(Name = "Company Website")]
        public string compWebsite { get; set; }

        public bool isTrue
        { get { return true; } }

        [Required]
        //[Display(Name = "I agree to the terms and conditions")]
        [Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
        public bool TermsAndConditions { get; set; }

        //[Range(typeof(bool), "true", "true", ErrorMessage = "Please accept Term & Conditions !")]
        //public bool TermsAndConditions { get; set; }	
    }

    public class ForgotPassword
    {
        [Display(Name = "EmailID ")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [System.Web.Mvc.Remote("EmailIDVerify", "Home", HttpMethod = "POST", ErrorMessage = "Please enter register Email Address ! ")]
        public string EmailID { get; set; }
    }

    public class ForgotPasswordUser
    {
        [Display(Name = "EmailID ")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [System.Web.Mvc.Remote("EmailIDVerify", "Home", HttpMethod = "POST", ErrorMessage = "Please enter register Email Address ! ")]
        public string EmailID { get; set; }
    }


    public class ResetPasswordViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [System.Web.Mvc.Remote("EmailIDVerify", "Home", HttpMethod = "POST", ErrorMessage = "Please enter registered Email Address ! ")]
        public string EmailID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [System.Web.Mvc.Remote("CurrentPasswordVerify", "Home", HttpMethod = "POST", ErrorMessage = "Please enter correct password ! ")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class EditUserProfile
    {
        ///<summary>
        /// For Personal Details of Customers , Employee and Consultant also...................... 
        ///</summary>

        [Display(Name = "Full Name")]
        public string fullName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Contact No.")]
        public string Phone { get; set; }

        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [System.Web.Mvc.AllowHtml]
        public string Contents { get; set; }
        public byte[] Image { get; set; }
        public string imageSrc { get; set; }

        public string Address { get; set; }

        public virtual string Country { get; set; }

        public virtual string State { get; set; }

        public virtual string City { get; set; }

        public string MimeType { get; set; }


        /// <summary>
        /// For Qualification Details of Customers , Employee and Consultant also...................... 
        /// </summary>

        [Display(Name = "Academic Education")]
        public string Academic { get; set; }

        [Display(Name = "Graduation")]
        public string graduation { get; set; }

        [Display(Name = "Post Graduation")]
        public string postgraduation { get; set; }

        [Display(Name = "Certificate (if any)")]
        public string certification { get; set; }

        /// <summary>
        /// For Professional Details of Customers , Employee and Consultant also...................... 
        /// </summary>

        [Display(Name = "Specialization")]
        public string specialization { get; set; }

        public string Experience { get; set; }

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

        [Display(Name = "Office Phone No.")]
        public string OfficeNo { get; set; }

        [Display(Name = "Website")]
        public string cmpWebsite { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Date Of Zoining")]
        public DateTime ZoiningDate { get; set; }

    }

    public class HomePage
    {
        public List<Enquiry> EnquiryListData { get; set; }
    }
    public class Slider
    {
        public string src { get; set; }
        public string title { get; set; }
    }

}