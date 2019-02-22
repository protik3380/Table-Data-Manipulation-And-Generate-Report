using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MindFork.Models
{
    public class Student
    {
        [Key]
        public int S_Id { get; set; }

        [Required(ErrorMessage = "Please Enter Student ID")]
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "Please Enter Student Name")]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Please Select Subject")]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual  Subject Subject { get; set; }

        [Required(ErrorMessage = "Please Enter marks")]
        public int  Mark { get; set; }





    }
}