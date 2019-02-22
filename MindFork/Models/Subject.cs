using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MindFork.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Subject Name is Required")]
        [Display(Name = "Subject")]
        [Remote("IsSubNameAvailable", "Subjects", ErrorMessage = "Subject Name already exist.")]
        public string SubjectName { get; set; }
    }
}