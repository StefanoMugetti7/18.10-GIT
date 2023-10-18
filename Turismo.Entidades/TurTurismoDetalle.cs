using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Entidades
{
    [Serializable]
    public class TurTurismoDetalle : Objeto
    {
        [PrimaryKey]
        public int IdTurismoDetalle { get; set; }
        public int IdTipoServicio { get; set; }
        public string TipoServicio { get; set; }
        public string Proveedor { get; set; }
        public int? IdProveedor{ get; set; }
        public decimal Costo { get; set; }
        public decimal Importe { get; set; }
        List<TGECampos> _serviciosCampos;
        public List<TGECampos> ServiciosCampos
        {
            get { return _serviciosCampos == null ? (_serviciosCampos = new List<TGECampos>()) : _serviciosCampos; }
            set { _serviciosCampos = value; }
        }
        //public string Destino { get; set; }
        //public string Transporte { get; set; }
        //public int? IdTransporte { get; set; }            
        //public string RegimenPension { get; set; }
        //public int? IdRegimenPension { get; set; }      
        //public string Hotel { get; set; }
        //public string Habitacion { get; set; }
        //public int? IdHabitacion { get; set; }
        //public decimal? Costo { get; set; }
        //public decimal? ImpuestoPais { get; set; }
        //public int? CantidadNoches { get; set; }
        //public DateTime? FechaIngreso { get; set; }
        //public DateTime? FechaSalida{ get; set; }
        //public DateTime? FechaEgreso{ get; set; }
 

    }
}
