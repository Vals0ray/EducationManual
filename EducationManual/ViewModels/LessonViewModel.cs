using EducationManual.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationManual.ViewModels
{
    public class LessonViewModel
    {
        [Key]
        public int LessonId { get; set; }

        public string Name { get; set; }

        public int? TestId { get; set; }

        public Student Student { get; set; }
    }
}