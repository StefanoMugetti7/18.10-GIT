using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTASectoresPuntosVentas : Objeto
    {
        
        int _idSectorPuntoVenta;
        TGESectores _sector;
        TGEFiliales _filial;
        int _PuntoVenta;

        [PrimaryKey]
        public int IdSectorPuntoVenta { get => _idSectorPuntoVenta; set => _idSectorPuntoVenta = value; }
        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }
        public TGESectores Sector
        {
            get { return _sector == null ? (_sector = new TGESectores()) : _sector; }
            set { _sector = value; }
        }
        public int PuntoVenta { get => _PuntoVenta; set => _PuntoVenta = value; }
       
    }
}
