using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LavaYa.Entidades
{
    [Serializable]
    public class LavPuntosVentasDetalle : Objeto
    {
        int _idPuntoVentaDetalle;

        public int IdPuntoVenta { get; set; }

        public int IdDia{ get; set; }
        public string Dia { get; set; }

        TimeSpan _horaDesde;
        TimeSpan _horaHasta;
        public LavPuntosVentasDetalle()
        {
        }

        [PrimaryKey]
        public int IdPuntoVentaDetalle
        {
            get { return _idPuntoVentaDetalle; }
            set { _idPuntoVentaDetalle= value; }
        }


        [Auditoria()]
        public TimeSpan HoraDesde
        {
            get { return _horaDesde; }
            set { _horaDesde = value; }
        }
        [Auditoria()]
        public TimeSpan HoraHasta
        {
            get { return _horaHasta; }
            set { _horaHasta = value; }
        }
    }
}
