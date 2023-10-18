using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using System.Xml;

namespace Tesorerias.Entidades
{
    [Serializable]
    public class TESCajasMovimientosConceptosContables : Objeto
    {
        int _idCajaMovimientoConceptoContable;
        int _idCajaMovimiento;
        string _detalle;
        decimal _importe;
        CtbCentrosCostosProrrateos _centroCostoProrrateo;
        CtbConceptosContables _conceptoContable;
        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCajasMovimientosValores;
        string _descripcion;


        [PrimaryKey]
        public int IdCajaMovimientoConceptoContable
        {
            get { return _idCajaMovimientoConceptoContable; }
            set { _idCajaMovimientoConceptoContable=value; }
        }

        public int IdCajaMovimiento
        {
            get { return _idCajaMovimiento; }
            set { _idCajaMovimiento = value; }
        }
        public string Detalle
        {
            get { return _detalle; }
            set { _detalle=value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set {  _importe=value; }
        }

        public CtbCentrosCostosProrrateos CentroCostoProrrateo
        {
            get { return _centroCostoProrrateo == null ? (_centroCostoProrrateo = new CtbCentrosCostosProrrateos()) : _centroCostoProrrateo; }
            set { _centroCostoProrrateo = value; }
        }

        public CtbConceptosContables ConceptoContable
        {
            get { return _conceptoContable == null ? (_conceptoContable = new CtbConceptosContables()) : _conceptoContable; }
            set { _conceptoContable = value; }
        }
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }
        public XmlDocument LoteCajasMovimientosValores
        {
            get { return _loteCajasMovimientosValores; }
            set { _loteCajasMovimientosValores = value; }
        }
        public string DescripcionCampos
        {
            get
            {
                string r = string.Empty;
                foreach (TGECampos p in this.Campos)
                {
                    r = string.Concat(r, p.Titulo, ": ", p.CampoValor.ListaValor);
                }

                return r;
            }
        }
    }
}
