using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elecciones.Entidades
{
    [Serializable]
    public class EleEleccionesEtapas : Objeto
    {
        int _idEleccionEtapa;
        int _idEtapa;
        string _etapa;
        string _codigoEtapa;
        int _idEleccion;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;

        [PrimaryKey]
        public int IdEleccionEtapa { get => _idEleccionEtapa; set => _idEleccionEtapa = value; }
        public int IdEtapa { get => _idEtapa; set => _idEtapa = value; }
        public string Etapa { get => _etapa; set => _etapa = value; }
        public int IdEleccion { get => _idEleccion; set => _idEleccion = value; }
        public DateTime? FechaDesde { get => _fechaDesde; set => _fechaDesde = value; }
        public DateTime? FechaHasta { get => _fechaHasta; set => _fechaHasta = value; }
        public string CodigoEtapa { get => _codigoEtapa; set => _codigoEtapa = value; }
    }
}
