using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    
    public class escolas
    {
        
        public int id { get; set; }
        public string nome { get; set; }
        public string endereco { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string cep { get; set; }
        public string telefone { get; set; }
        public int estados_id { get; set; }

    }
}