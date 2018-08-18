using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VibExchange.Models
{
    public class Blogs
    {
        public int blogID { get; set; }

        [Required]
        [Display(Name="Title")]
        public string blogTitle { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string blogImage { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string blogText { get; set; }

        public int likes { get; set; }
        public string CreatedBy { get; set; }
        public string CreateDate { get; set; }
        public List<Blogs> blogsList { get; set; }
    }
}