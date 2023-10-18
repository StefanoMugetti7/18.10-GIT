using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bancos.Entidades
{
    [Serializable]
    public class TESBancosLotesEnviados : Objeto
    {
        [PrimaryKey]
        public int IdBancoLoteEnvio { get; set; }
        public int IdFilial{ get; set; }
        public int CantidadRegistros{ get; set; }
        public int CantidadRegistrosConciliado{ get; set; }
        public decimal ImporteTotal{ get; set; }
        public decimal ImporteTotalConciliado{ get; set; }
        public string Observacion { get; set; }
        public int IdBancoCuenta{ get; set; }
        public string BancoCuenta { get; set; } 
        public string NumeroTramite { get; set; } 
        public int? IdTipoArchivo{ get; set; }
        public string TipoArchivo { get; set; }
        public new string Filtro { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdUsuarioAlta { get; set; }
        public DateTime? FechaAutorizacion{ get; set; }
        public int? IdUsuarioAutorizar{ get; set; }
        public DateTime? FechaPago{ get; set; }
        UsuariosAlta _usuarioAlta;
        List<TESBancosLotesEnviadosDetalle> _detalles;

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int IdTipoOperacionFiltro { get; set; }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        public List<TESBancosLotesEnviadosDetalle> Detalles
        {
            get { return _detalles == null ? (_detalles = new List<TESBancosLotesEnviadosDetalle>()) : _detalles; }
            set { _detalles = value; }
        }

        
    }
}
