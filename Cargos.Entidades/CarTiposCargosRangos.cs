using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargos.Entidades
{
    [Serializable]
    public class CarTiposCargosRangos : Objeto
    {
        [PrimaryKey]
        public int IdTipoCargoRango { get; set; }

        public int IdTipoCargo { get; set; }

        CarTiposRangos _tipoRango;
        [Auditoria]
        public CarTiposRangos TipoRango { get { return _tipoRango == null ? (_tipoRango = new CarTiposRangos()) : _tipoRango; } set { _tipoRango = value; } }
        [Auditoria]
        public decimal Desde { get; set; }
        [Auditoria]
        public decimal Hasta { get; set; }
        [Auditoria]
        public decimal Importe { get; set; }

        public DateTime FechaAlta { get; set; }

        public DateTime FechaVigenciaDesde { get; set; }
        public int IdUsuarioAlta { get; set; }
        
    }
}
