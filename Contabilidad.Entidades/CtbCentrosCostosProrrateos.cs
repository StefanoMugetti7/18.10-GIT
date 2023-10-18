using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbCentrosCostosProrrateos : Objeto
    {
        int? _idCentroCostoProrrateo;
        string _centroCostoProrrateo;
        bool? _noVisible;
        TGEFiliales _filial;
        List<CtbCentrosCostosProrrateosDetalles> _centrosCostosProrrateosDetalles;

        [PrimaryKey()]
        [Auditoria()]
        public int? IdCentroCostoProrrateo
        {
            get { return _idCentroCostoProrrateo; }
            set { _idCentroCostoProrrateo = value; }
        }

        [Auditoria()]
        public string CentroCostoProrrateo
        {
            get { return _centroCostoProrrateo==null? string.Empty : _centroCostoProrrateo; }
            set { _centroCostoProrrateo = value; }
        }

        public bool? NoVisible
        {
            get { return _noVisible; }
            set { _noVisible = value; }
        }

        [Auditoria()]
        public TGEFiliales Filial
        {
            get { return _filial==null ? (_filial=new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public List<CtbCentrosCostosProrrateosDetalles> CentrosCostosProrrateosDetalles
        {
            get { return _centrosCostosProrrateosDetalles == null ? (_centrosCostosProrrateosDetalles = new List<CtbCentrosCostosProrrateosDetalles>()) : _centrosCostosProrrateosDetalles; }
            set { _centrosCostosProrrateosDetalles = value; }
        }
    }
}
