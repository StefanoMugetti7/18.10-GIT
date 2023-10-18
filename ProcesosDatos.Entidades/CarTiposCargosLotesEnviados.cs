using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ProcesosDatos.Entidades
{
    [Serializable]
    public partial class CarTiposCargosLotesEnviados : Objeto
    {
        int _idTipoCargoLoteEnviado;
        int? _idFormaCobro;
        int _formaCobro;
        int _cantidadRegistros;
        decimal _importeTotal;
        int _idProcesoArchivo;
        DateTime _fechaAlta;
        int? _periodo;
        int? _idRefFormaCobro;
        int _idProcesoProcesamiento;
        XmlDocumentSerializationWrapper _loteDetalles;
        List<CarTiposCargosLotesEnviadosDetalles> _detalles;

        [PrimaryKey]
        public int IdTipoCargoLoteEnviado { get => _idTipoCargoLoteEnviado; set => _idTipoCargoLoteEnviado = value; }
        public int? IdFormaCobro { get => _idFormaCobro; set => _idFormaCobro = value; }
        public int CantidadRegistros { get => _cantidadRegistros; set => _cantidadRegistros = value; }
        public decimal ImporteTotal { get => _importeTotal; set => _importeTotal = value; }
        public int IdProcesoArchivo { get => _idProcesoArchivo; set => _idProcesoArchivo = value; }
        public DateTime FechaAlta { get => _fechaAlta; set => _fechaAlta = value; }
        public int? Periodo { get => _periodo; set => _periodo = value; }
        public int? IdRefFormaCobro { get => _idRefFormaCobro; set => _idRefFormaCobro = value; }
        public int FormaCobro { get => _formaCobro; set => _formaCobro = value; }
        public List<CarTiposCargosLotesEnviadosDetalles> Detalles
        {
            get { return _detalles == null ? (_detalles = new List<CarTiposCargosLotesEnviadosDetalles>()) : _detalles; }
            set { _detalles = value; }
        }
        public XmlDocument LoteDetalles
        {
            get
            {
                return _loteDetalles;
            }
            set
            {
                _loteDetalles = value;
            }
        }

        public int IdProcesoProcesamiento { get => _idProcesoProcesamiento; set => _idProcesoProcesamiento = value; }
        public DateTime FechaCobro { get; set; }
    }
}
