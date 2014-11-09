using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class sub_produtos_materia_primas
    {
        [Key]
        public int sub_produtos_id {get;set;}
        public int materia_primas_id {get;set;}
        public double quantidade{get;set;}
        public string calculo {get;set;}
        public Nullable<int> superior { get; set; }
        public Nullable<int> inferior { get; set; }
        public Nullable<int> esquerda { get; set; }
        public Nullable<int> direita { get; set; }
        public Nullable<int> frente { get; set; }
        public Nullable<int> verso { get; set; }
 
    }
}