using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicina.Entidades
{
    [Serializable]
    public class MedEstudios : Objeto
    {
        public MedEstudios() {
            TipoEstudio = string.Empty;
            InformeEstudio = string.Empty;
            VistaPreviaEstudio = string.Empty;
        }
        [PrimaryKey]
        public int IdEstudio { get; set; }
        public int IdTipoEstudio { get; set; }
        public string TipoEstudio { get; set; }
        public DateTime FechaAlta { get; set; }
        [Auditoria]
        public string InformeEstudio { get; set; }
        public string VistaPreviaEstudio { get; set; }
        public DateTime FechaEstudio { get; set; }
        public int IdUsuarioAlta { get; set; }
        public int IdUsuarioEvento { get; set; }

        private AfiPacientes _afiliados;
        private MedPrestadores _prestador;
        private List<TGEArchivos> _archivos;

        public AfiPacientes Afiliado
        {
            get { return _afiliados == null ? (_afiliados = new AfiPacientes()) : _afiliados; }
            set { _afiliados = value; }
        }

        public MedPrestadores Prestador
        {
            get { return _prestador == null ? (_prestador = new MedPrestadores()) : _prestador; }
            set { _prestador = value; }
        }

        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }
    }
}
