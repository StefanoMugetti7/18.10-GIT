using Comunes.Entidades;
using System;

namespace ProcesosDatos.Entidades
{
    [Serializable]
    public partial class CarTiposCargosLotesEnviadosDetalles : Objeto
    {
        int _idTipoCargoLoteEnviadoDetalle;
        int? _idCuentaCorriente;
        CarTiposCargosLotesEnviados _tipoCargoLoteEnviado;
        int _idAfiliado;
        string _apellidoNombre;
        int _orden;
        string _registroEnviado;
        decimal _importeEnviado;
        decimal _importeCobrado;
        decimal? _importeAplicar;
        int? _idTipoCargoLoteTipoRegistro;
        string _tipoDocumento;
        long? _numeroDocumento;
        string _numeroSocio;
        long? _matriculaIAF;
        string _observaciones;
        int? _idProcesoProcesamientoDevolucion;
        string _descripcion;
        string _periodo;
        string _concepto;
        string _fechaMovimiento;
        string _descripcionEstado;
        decimal _importeLEDCC;
        decimal _importeAAplicarLEDCC;
        int _idFormaCobro;
       
        [PrimaryKey]
        public int IdTipoCargoLoteEnviadoDetalle { get => _idTipoCargoLoteEnviadoDetalle; set => _idTipoCargoLoteEnviadoDetalle = value; }
        public int? IdCuentaCorriente { get => _idCuentaCorriente; set => _idCuentaCorriente = value; }
        public CarTiposCargosLotesEnviados TipoCargoLoteEnviado
        {
            get { return _tipoCargoLoteEnviado == null ? (_tipoCargoLoteEnviado = new CarTiposCargosLotesEnviados()) : _tipoCargoLoteEnviado; }
            set { _tipoCargoLoteEnviado = value; }
        }

        public int IdAfiliado { get => _idAfiliado; set => _idAfiliado = value; }
        public int Orden { get => _orden; set => _orden = value; }
        public string RegistroEnviado { get => _registroEnviado; set => _registroEnviado = value; }
        public decimal ImporteEnviado { get => _importeEnviado; set => _importeEnviado = value; }
        public decimal ImporteCobrado { get => _importeCobrado; set => _importeCobrado = value; }
        public decimal? ImporteAAplicar { get => _importeAplicar; set => _importeAplicar = value; }
        public int? IdTipoCargoLoteTipoRegistro { get => _idTipoCargoLoteTipoRegistro; set => _idTipoCargoLoteTipoRegistro = value; }
        public string TipoDocumento { get => _tipoDocumento; set => _tipoDocumento = value; }
        public long? NumeroDocumento { get => _numeroDocumento; set => _numeroDocumento = value; }
        public string NumeroSocio { get => _numeroSocio; set => _numeroSocio = value; }
        public long? MatriculaIAF { get => _matriculaIAF; set => _matriculaIAF = value; }
        public string Observaciones { get => _observaciones; set => _observaciones = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public string ApellidoNombre { get => _apellidoNombre; set => _apellidoNombre = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public string Periodo { get => _periodo; set => _periodo = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public string Concepto { get => _concepto; set => _concepto = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public string FechaMovimiento { get => _fechaMovimiento; set => _fechaMovimiento = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public string Descripcion { get => _descripcion; set => _descripcion = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public string DescripcionEstado { get => _descripcionEstado; set => _descripcionEstado = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public decimal ImporteLEDCC { get => _importeLEDCC; set => _importeLEDCC = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public decimal ImporteCC { get => _importeLEDCC; set => _importeLEDCC = value; }
        /// <summary>
        /// PROPIEDADES SOLO PARA GRILLA 
        /// </summary>
        public decimal ImporteAAplicarLEDCC { get => _importeAAplicarLEDCC; set => _importeAAplicarLEDCC = value; }
        public int IdFormaCobro { get => _idFormaCobro; set => _idFormaCobro = value; }
        public int? IdProcesoProcesamientoDevolucion { get => _idProcesoProcesamientoDevolucion; set => _idProcesoProcesamientoDevolucion = value; }
    }
}