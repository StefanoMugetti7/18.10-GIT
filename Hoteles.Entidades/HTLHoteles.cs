//using Afiliados.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLHoteles : Objeto
    {
        [PrimaryKey]
        public int IdHotel { get; set; }
        public string Descripcion { get; set; }
        public TimeSpan? HorarioIngreso { get; set; }
        public TimeSpan? HorarioEgreso { get; set; }
        public string HoraIngresoStr { get; set; }
        public string HoraEgresoStr { get; set; }
        TGEFiliales _filial;
        public TGEFiliales Filial { get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; } set { _filial = value; } }
        //AfiAfiliados _afiliado { get; set; }
        //public AfiAfiliados Afiliado { get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; } set { _afiliado = value; } }
    }
}
