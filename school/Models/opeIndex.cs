using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class opeIndex
    {
        [Key]
        public int id { get; set; }
        public string nome { get; set; }
        public ope ope { get; set; }

    }
}