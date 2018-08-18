using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace VibExchange.Models
{
    public class Contact
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        
    }
    public class Career
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name="Email Address")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Experience { get; set; }

        [Required(ErrorMessage = "Please Upload Resume")]
        public HttpContext Resume { get; set; }

    }

    public class CurrentOpenings
    {
        public int JobID { get; set; }
        public string JobTitle { get; set; }
        public string JobCategory { get; set; }
        public string JobDescription { get; set; }
        public string ExperienceRequired { get; set; }

        public List<CurrentOpenings> AllOpenings { get; set; }

        public List<CurrentOpenings> GetAllOpenings()
        {
            List<CurrentOpenings> OpeningList = new List<CurrentOpenings>();
            using (DBClass context = new DBClass())
            {
                DataTable dt = context.getData("GetAllOpening", CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    OpeningList.Add(new CurrentOpenings
                    {
                        JobID = Convert.ToInt32(dr["JobID"]),
                        JobTitle = Convert.ToString(dr["JobTitle"]),
                        JobCategory = Convert.ToString(dr["JobCategory"]),
                        JobDescription = Convert.ToString(dr["Description"]),
                        ExperienceRequired = Convert.ToString(dr["ExperienceRequired"])

                    });
                }
                return OpeningList;
            }
        }
    }

}