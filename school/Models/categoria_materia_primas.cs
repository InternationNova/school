using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class categoria_materia_primas
    {
        public int id { get; set; }
        public string descricao { get; set; }
    }
}