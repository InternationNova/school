using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Classes
{
    public class reportComprasPdf
    {
        public List<reportComprasFirst> reportComprasFirst { get; set; }
        public List<reportComprasSecond> reportComprasSecond { get; set; }
    }
    public class reportComprasFirst
    {
        public string memo { get; set; }
        public string codigo_smk { get; set; }
        public string unidade { get; set; }
        public double quantidade { get; set; }
        public string codigo_smk1 { get; set; }

    }


    public class reportComprasSecond
    {
        public string memo { get; set; }
        public string codigo_smk { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public string codigo_smk1 { get; set; }

    }
}