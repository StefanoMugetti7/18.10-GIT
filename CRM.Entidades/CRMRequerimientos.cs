using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entidades
{
    [Serializable]
    public class CRMRequerimientos:Objeto
    {
        [PrimaryKey]
        public int IdRequerimiento { get; set; }
        
        public int? IdCategoria { get; set; }
        public string Categoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRequerimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public int? IdPrioridad { get; set; }
        public string Prioridad { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public DateTime? FechaInternaResolucion { get; set; }
        public DateTime? FechaCierre{ get; set; }
        public bool EsPrivado { get; set; }
        public int IdOrigenSolicitud { get; set; }
        public string OrigenSolicitud { get; set; }
        public string Tabla { get; set; }
        public int? IdRefTabla { get; set; }
       
        public int IdTecnico { get; set; }
        public int IdUsuarioAlta { get; set; }
        public string Tecnico { get; set; }
        public string UsuarioAlta { get; set; }
        public string Cliente { get; set; }
        AfiAfiliados _afiliado;

        private CRMRequerimientosTipos _requerimientosTipos;
        private List<TGEArchivos> _archivos;

        private List<TGECampos> _campos;
        //List<CRMSeguimientos> _seguimientos;

        //public List<CRMSeguimientos> PuntosVentasDetalles
        //{
        //    get { return _seguimientos == null ? (_seguimientos = new List<CRMSeguimientos>()) : _seguimientos; }
        //    set { _seguimientos = value; }
        //}
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }
        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }
        public CRMRequerimientosTipos RequerimientosTipos
        {
            get { return _requerimientosTipos == null ? (_requerimientosTipos = new CRMRequerimientosTipos()) : _requerimientosTipos; }
            set { _requerimientosTipos = value; }
        }
        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }
        public enum EstadosRequerimientos
        {
            Activo = 1,
            Baja = 0,
            Solucionado = 115,
        }
    }
}
