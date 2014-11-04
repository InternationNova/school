using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Classes
{
    public class reportPrimaPdf
    {
        public string titulo { get; set; }
        public string subtitulo { get; set; }


        public List<reportPrimaFirst> reportPrimaFirst { get; set; }
        public List<reportPrimaSecond> reportPrimaSecond { get; set; }

    }
    public class reportPrimaFirst
    {
        public string codigo_smk { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public string calculo { get; set; }
        public int quantidade { get; set; }
        public double mdf { get; set; }
        public double laminado { get; set; }

    }

    public class reportPrimaSecond
    {
        public string codigo_smk { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public string calculo { get; set; }
        public int quantidade { get; set; }
        public double mdf { get; set; }
        public double laminado { get; set; }

    }

}