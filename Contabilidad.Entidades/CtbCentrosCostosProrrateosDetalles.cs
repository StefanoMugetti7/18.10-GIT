using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbCentrosCostosProrrateosDetalles : Objeto
    {
        int _idCentroCostoProrrateoDetalle;
        int _idCentroCostoProrrateo;
        int _idCentroCosto;
        string _centroCosto;
        decimal _porcentaje;

        [PrimaryKey()]
        public int IdCentroCostoProrrateoDetalle
        {
            get { return _idCentroCostoProrrateoDetalle; }
            set { _idCentroCostoProrrateoDetalle = value; }
        }

        public int IdCentroCostoProrrateo
        {
            get { return _idCentroCostoProrrateo; }
            set { _idCentroCostoProrrateo = value; }
        }

        [Auditoria()]
        public int IdCentroCosto
        {
            get { return _idCentroCosto; }
            set { _idCentroCosto = value; }
        }

        [Auditoria()]
        public string CentroCosto
        {
            get { return _centroCosto == null ? string.Empty : _centroCosto; }
            set { _centroCosto = value; }
        }

        [Auditoria()]
        public decimal Porcentaje
        {
            get { return _porcentaje; }
            set { _porcentaje = value; }
        }
    }
}
