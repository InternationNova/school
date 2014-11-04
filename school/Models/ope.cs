using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    public class ope
    {

        public int id { get; set; }
        public string numero_processo { get; set; }
        public string numero_projeto { get; set; }
        public string data_abertura { get; set; }
        public string data_chegada { get; set; }
        public int escolas_id { get; set; }
        public string observacao { get; set; }

    }
}