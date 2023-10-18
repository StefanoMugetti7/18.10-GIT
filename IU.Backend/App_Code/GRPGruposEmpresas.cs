using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IU.Backend.App_Code
{
    [Serializable]
    public class GRPGruposEmpresas : Objeto
    {
        public string Grupo { get; set; }

        public string Empresa { get; set; }

        public string BaseDatos { get; set; }

        public string URLSistema { get; set; }

        public string NombreAplicacion { get; set; }

        public string SourcePath { get; set; }
        public string TargetPath { get; set; }

        public string Version { get; set; }
    }
}