
using Comunes.Entidades;
using Generales.Entidades;
using System;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiDomicilios : Objeto
    {

        #region "Private Members"
        int _idDomicilio;
        int _idAfiliado;
        AfiDomiciliosTipos _domicilioTipo;
        string _calle;
        int _numero;
        int _piso;
        string _departamento;
        string _codigoPostal;
        TGECodigosPostales _localidad;

        bool _predeterminado;
        DateTime _fechaAlta;
        #endregion

        #region "Constructors"
        public AfiDomicilios()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdDomicilio
        {
            get { return _idDomicilio; }
            set { _idDomicilio = value; }
        }
        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }
        [Auditoria()]
        public AfiDomiciliosTipos DomicilioTipo
        {
            get { return _domicilioTipo == null ? (_domicilioTipo = new AfiDomiciliosTipos()) : _domicilioTipo; }
            set { _domicilioTipo = value; }
        }
        [Auditoria()]
        public string Calle
        {
            get { return _calle == null ? string.Empty : _calle; }
            set { _calle = value; }
        }
        [Auditoria()]
        public int Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }
        [Auditoria()]
        public int Piso
        {
            get { return _piso; }
            set { _piso = value; }
        }
        [Auditoria()]
        public string Departamento
        {
            get { return _departamento == null ? string.Empty : _departamento; }
            set { _departamento = value; }
        }

        [Auditoria()]
        public string CodigoPostal
        {
            get { return _codigoPostal == null ? string.Empty : _codigoPostal; }
            set { _codigoPostal = value; }
        }

        [Auditoria()]
        public TGECodigosPostales Localidad
        {
            get { return _localidad == null ? (_localidad = new TGECodigosPostales()) : _localidad; }
            set { _localidad = value; }
        }
        [Auditoria()]
        public bool Predeterminado
        {
            get { return _predeterminado; }
            set { _predeterminado = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public string DomicilioCompleto
        {
            get
            {
                return string.Concat(DomicilioCalleCompleto, " ", CodigoPostal.Length > 0 ? "(" + CodigoPostal + ") " : string.Empty,
                Localidad.Descripcion.Length > 0 ? " - " + Localidad.Descripcion : string.Empty,
                    Localidad.Provincia.Descripcion.Length > 0 ? ", " + Localidad.Provincia.Descripcion : string.Empty);
            }
            set { }
        }

        public string DomicilioCalleCompleto
        {
            get
            {
                return string.Concat(this.Calle, " ", Numero > 0 ? Numero.ToString() : string.Empty,
                    Piso > 0 ? " - " + "Piso: " + Piso.ToString() : string.Empty,
                    Departamento.Length > 0 && Departamento.ToString() != "0" ? " - " + "Dpto: " + Departamento.ToString() : string.Empty);
            }
            set { }
        }

        #endregion
    }
}
