
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiAfiliadosBase : Objeto
    {
        int _idAfiliado;
        string _nombre;
        string _apellido;
        AfiTiposDocumentos _tipoDocumento;
        long _numeroDocumento;
        AfiAfiliadosTipos _afiliadoTipo;
        string _Proveedor;

        [PrimaryKey()]
        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }

        [Auditoria()]
        public string Nombre
        {
            get { return _nombre == null ? string.Empty : _nombre; }
            set { _nombre = value; }
        }
        [Auditoria()]
        public string Apellido
        {
            get { return _apellido == null ? string.Empty : _apellido; }
            set { _apellido = value; }
        }
        public string Proveedor
        {
            get { return _Proveedor == null ? string.Empty : _Proveedor; }
            set { _Proveedor = value; }
        }

        [Auditoria()]
        public AfiTiposDocumentos TipoDocumento
        {
            get { return _tipoDocumento == null ? (_tipoDocumento = new AfiTiposDocumentos()) : _tipoDocumento; }
            set { _tipoDocumento = value; }
        }

        [Auditoria()]
        public long NumeroDocumento
        {
            get { return _numeroDocumento; }
            set { _numeroDocumento = value; }
        }
        public AfiAfiliadosTipos AfiliadoTipo
        {
            get { return _afiliadoTipo == null ? (_afiliadoTipo = new AfiAfiliadosTipos()) : _afiliadoTipo; }
            set { _afiliadoTipo = value; }
        }

        public bool CalculaNumeroSocio { get; set; }
    }

    [Serializable]
    public partial class AfiAfiliados : AfiAfiliadosBase
    {
        #region "Private Members"	
        int _idAfiliadoRef;
        List<AfiAfiliados> _familiares;
        List<AfiAfiliados> _apoderados;
        DateTime _fechaAlta;
        string _correoElectronico;
        string _detalle;
        string _antecedentesFamiliares;
        string _antecedentesPersonales;
        long _cUIL;
        string _numeroSocio;
        AfiSexos _sexo;
        DateTime? _fechaNacimiento;
        DateTime? _fechaIngreso;
        AfiEstadoCivil _estadoCivil;
        long _matriculaIAF;
        AfiCategorias _categoria;
        AfiGruposSanguineos _grupoSanguieno;
        AfiGrados _grado;
        DateTime? _fechaRetiro;
        DateTime? _fechaFallecimiento;
        DateTime? _fechaBaja;
        bool _comprobanteExento;
        byte[] _foto;
        byte[] _firma;
        TGEFiliales _filial;
        UsuariosAlta _usuarioAlta;
        List<AfiDomicilios> _domicilios;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        List<AfiTelefonos> _telefonos;
        AfiParentesco _parentesco;
        List<AfiAlertasTipos> _alertasTipos;
        AfiTiposApoderados _tipoApoderado;
        int _cantidadParticipantes;
        int _idAfiliadoFallecido;
        string _numeroSocioFallecido;
        AfiAlertasTipos _alertaTipo;
        DateTime? _fechaSupervivencia;
        bool _confirmarBajaFamiliares;
        string _codigoZonaGrupo;
        decimal _importeExcedido;
        List<TGECampos> _campos;
        TGECondicionesFiscales _condicionFiscal;
        TGEZonasGrupos _zonaGrupo;
        bool _mostrarMensajesAlertas;
        //AGREGADO PARA PODER FILTRAR
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        string _razonSocial;
        decimal _saldoActual;
        bool _tieneSaldo;
        XmlDocumentSerializationWrapper _LoteObtenerFamiliares;
        XmlDocumentSerializationWrapper _LoteObtenerDomicilios;
        XmlDocumentSerializationWrapper _loteFamiliares;
        XmlDocumentSerializationWrapper _loteDomicilios;
        XmlDocumentSerializationWrapper _loteCamposValores;
        XmlDocumentSerializationWrapper _loteDomiciliosLaborales;
        XmlDocumentSerializationWrapper _loteTelefonos;
        string _tabla;
        Int64? _idRefTabla;
        AfiNacionalidades _nacionalidad;
        int _idTipoPersona;
        string _informacionFamiliar;
        bool _desvincular;
        #endregion

        #region "Constructors"
        public AfiAfiliados()
        {
        }
        #endregion

        #region "Public Properties"

        public int IdAfiliadoRef
        {
            get { return _idAfiliadoRef; }
            set { _idAfiliadoRef = value; }
        }
        public List<AfiAfiliados> Familiares
        {
            get { return _familiares == null ? (_familiares = new List<AfiAfiliados>()) : _familiares; }
            set { _familiares = value; }
        }

        public List<AfiAfiliados> Apoderados
        {
            get { return _apoderados == null ? (_apoderados = new List<AfiAfiliados>()) : _apoderados; }
            set { _apoderados = value; }
        }


        [Auditoria()]
        public string Detalle
        {
            get { return _detalle == null ? string.Empty : _detalle; }
            set { _detalle = value; }
        }

        public string ApellidoNombre
        {
            get { return this.IdAfiliado > 0 ? string.Concat(this.Apellido, this.Nombre.Length > 0 ? ", " : string.Empty, this.Nombre) : string.Empty; }
            set { }
        }

        public string AntecedentesFamiliares
        {
            get { return _antecedentesFamiliares == null ? string.Empty : _antecedentesFamiliares; }
            set { _antecedentesFamiliares = value; }
        }


        public string AntecedentesPersonales
        {
            get { return _antecedentesPersonales == null ? string.Empty : _antecedentesPersonales; }
            set { _antecedentesPersonales = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        [Auditoria()]
        public string CorreoElectronico
        {
            get { return _correoElectronico == null ? string.Empty : _correoElectronico; }
            set { _correoElectronico = value; }
        }

        [Auditoria()]
        public long CUIL
        {
            get { return _cUIL; }
            set { _cUIL = value; }
        }

        [Auditoria()]
        public string CUILFormateado
        {
            get { return this.CUIL.ToString(); }// >= 11 ? string.Concat(this.CUIL.ToString().Substring(0, 2), "-", this.CUIL.ToString().Substring(2, 9), "-", this.CUIL.ToString().Substring(10, 1)) : string.Empty; }
            set { }
        }

        [Auditoria()]
        public string NumeroSocio
        {
            get { return _numeroSocio == null ? string.Empty : _numeroSocio; }
            set { _numeroSocio = value; }
        }
        [Auditoria()]
        public AfiSexos Sexo
        {
            get { return _sexo == null ? (_sexo = new AfiSexos()) : _sexo; }
            set { _sexo = value; }
        }
        [Auditoria()]
        public DateTime? FechaNacimiento
        {
            get { return _fechaNacimiento; }
            set { _fechaNacimiento = value; }
        }
        [Auditoria()]
        public DateTime? FechaIngreso
        {
            get { return _fechaIngreso; }
            set { _fechaIngreso = value; }
        }
        [Auditoria()]
        public AfiEstadoCivil EstadoCivil
        {
            get { return _estadoCivil == null ? (_estadoCivil = new AfiEstadoCivil()) : _estadoCivil; }
            set { _estadoCivil = value; }
        }
        [Auditoria()]
        public long MatriculaIAF
        {
            get { return _matriculaIAF; }
            set { _matriculaIAF = value; }
        }

        [Auditoria()]
        public AfiCategorias Categoria
        {
            get { return _categoria == null ? (_categoria = new AfiCategorias()) : _categoria; }
            set { _categoria = value; }
        }
        [Auditoria()]
        public AfiGruposSanguineos GrupoSanguieno
        {
            get { return _grupoSanguieno == null ? (_grupoSanguieno = new AfiGruposSanguineos()) : _grupoSanguieno; }
            set { _grupoSanguieno = value; }
        }
        [Auditoria()]
        public AfiGrados Grado
        {
            get { return _grado == null ? (_grado = new AfiGrados()) : _grado; }
            set { _grado = value; }
        }
        [Auditoria()]
        public DateTime? FechaRetiro
        {
            get { return _fechaRetiro; }
            set { _fechaRetiro = value; }
        }
        [Auditoria()]
        public DateTime? FechaFallecimiento
        {
            get { return _fechaFallecimiento; }
            set { _fechaFallecimiento = value; }
        }

        [Auditoria()]
        public DateTime? FechaBaja
        {
            get { return _fechaBaja; }
            set { _fechaBaja = value; }
        }

        public bool ComprobanteExento
        {
            get { return _comprobanteExento; }
            set { _comprobanteExento = value; }
        }

        public byte[] Foto
        {
            get { return _foto; }
            set { _foto = value; }
        }

        public byte[] Firma
        {
            get { return _firma; }
            set { _firma = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }
        public List<AfiDomicilios> Domicilios
        {
            get { return _domicilios == null ? (_domicilios = new List<AfiDomicilios>()) : _domicilios; }
            set { _domicilios = value; }
        }

        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        public List<TGEComentarios> Comentarios
        {
            get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
            set { _comentarios = value; }
        }

        public List<AfiTelefonos> Telefonos
        {
            get { return _telefonos == null ? (_telefonos = new List<AfiTelefonos>()) : _telefonos; }
            set { _telefonos = value; }
        }

        public AfiParentesco Parentesco
        {
            get { return _parentesco == null ? (_parentesco = new AfiParentesco()) : _parentesco; }
            set { _parentesco = value; }
        }

        public List<AfiAlertasTipos> AlertasTipos
        {
            get { return _alertasTipos == null ? (_alertasTipos = new List<AfiAlertasTipos>()) : _alertasTipos; }
            set { _alertasTipos = value; }
        }

        public string DomicilioPredeterminado
        {
            get { return this.Domicilios.Exists(x => x.Predeterminado) ? this.Domicilios.Find(x => x.Predeterminado).DomicilioCompleto : string.Empty; }
            set { }
        }

        public string TelefonoPredeterminado
        {
            get { return this.Telefonos.Count > 0 ? string.Concat(/*this.Telefonos[0].Prefijo, " ", */this.Telefonos[0].Numero) : string.Empty; }
            set { }
        }

        [Auditoria()]
        public AfiTiposApoderados TipoApoderado
        {
            get { return _tipoApoderado == null ? (_tipoApoderado = new AfiTiposApoderados()) : _tipoApoderado; }
            set { _tipoApoderado = value; }
        }

        public string DatosAfiliado
        {
            get { return string.Concat(this.NumeroSocio, " - ", this.TipoDocumento.TipoDocumento, " ", this.NumeroDocumento, " ", this.ApellidoNombre); }
            set { }
        }

        public int CantidadParticipantes
        {
            get
            {
                return _cantidadParticipantes;
                //return this.Familiares.Count > 0 ?
                //    this.Familiares.Where(x => x.Estado.IdEstado == (int)EstadosAfiliados.AvisodeFallecido
                //  || x.Estado.IdEstado == (int)EstadosAfiliados.Moroso
                //  || x.Estado.IdEstado == (int)EstadosAfiliados.Normal
                //  || x.Estado.IdEstado == (int)EstadosAfiliados.Suspendido
                //  || x.Estado.IdEstado == (int)EstadosAfiliados.Vitalicio).ToList().Count
                //  : 0;
            }
            set { _cantidadParticipantes = value; }
        }

        public int IdAfiliadoFallecido
        {
            get { return _idAfiliadoFallecido; }
            set { _idAfiliadoFallecido = value; }
        }

        public int IdTipoPersona
        {
            get { return _idTipoPersona; }
            set { _idTipoPersona = value; }
        }


        public string NumeroSocioFallecido
        {
            get { return _numeroSocioFallecido == null ? string.Empty : _numeroSocioFallecido; }
            set { _numeroSocioFallecido = value; }
        }

        public AfiAlertasTipos AlertaTipo
        {
            get { return _alertaTipo == null ? (_alertaTipo = new AfiAlertasTipos()) : _alertaTipo; }
            set { _alertaTipo = value; }
        }

        [Auditoria()]
        public DateTime? FechaSupervivencia
        {
            get { return _fechaSupervivencia; }
            set { _fechaSupervivencia = value; }
        }

        public bool ConfirmarBajaFamiliares
        {
            get { return _confirmarBajaFamiliares; }
            set { _confirmarBajaFamiliares = value; }
        }

        //[Auditoria()]
        public string CodigoZonaGrupo
        {
            //get { return _codigoZonaGrupo == null ? string.Empty : _codigoZonaGrupo; }
            //set { _codigoZonaGrupo = value; }
            get { return this.ZonaGrupo.IdZonaGrupo.ToString(); }
            set { }
        }

        public decimal ImporteExcedido
        {
            get { return _importeExcedido; }
            set { _importeExcedido = value; }
        }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        [Auditoria()]
        public TGECondicionesFiscales CondicionFiscal
        {
            get { return _condicionFiscal == null ? (_condicionFiscal = new TGECondicionesFiscales()) : _condicionFiscal; }
            set { _condicionFiscal = value; }
        }

        [Auditoria()]
        public TGEZonasGrupos ZonaGrupo
        {
            get { return _zonaGrupo == null ? (_zonaGrupo = new TGEZonasGrupos()) : _zonaGrupo; }
            set { _zonaGrupo = value; }
        }

        public bool MostrarMensajesAlertas
        {
            get { return _mostrarMensajesAlertas; }
            set { _mostrarMensajesAlertas = value; }
        }

        public DateTime? FechaDesde
        {
            get { return _fechaDesde; }
            set { _fechaDesde = value; }
        }

        public DateTime? FechaHasta
        {
            get { return _fechaHasta; }
            set { _fechaHasta = value; }
        }

        public string RazonSocial
        {
            get { return _razonSocial == null ? string.Empty : _razonSocial; }
            set { _razonSocial = value; }
        }

        public decimal SaldoActual
        {
            get { return _saldoActual; }
            set { _saldoActual = value; }
        }

        public bool TieneSaldo
        {
            get { return _tieneSaldo; }
            set { _tieneSaldo = value; }
        }

        public XmlDocument LoteDomicilios
        {
            get { return _loteDomicilios; }
            set { _loteDomicilios = value; }
        }
        public XmlDocument LoteDomiciliosLaborales
        {
            get { return _loteDomiciliosLaborales; }
            set { _loteDomiciliosLaborales = value; }
        }
        public XmlDocument LoteTelefonos
        {
            get { return _loteTelefonos; }
            set { _loteTelefonos = value; }
        }
        public XmlDocument LoteFamiliares
        {
            get { return _loteFamiliares; }
            set { _loteFamiliares = value; }
        }
        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }//  ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }

        public string Tabla
        {
            get { return _tabla == null ? string.Empty : _tabla; }
            set { _tabla = value; }
        }

        public Int64? IdRefTabla
        {
            get { return _idRefTabla; }
            set { _idRefTabla = value; }
        }

        public string DescripcionAfiliado { get; set; }

        [Auditoria()]
        public AfiNacionalidades Nacionalidad
        {
            get { return _nacionalidad == null ? (_nacionalidad = new AfiNacionalidades()) : _nacionalidad; }
            set { _nacionalidad = value; }
        }

        public string PrefijoNumero { get; set; }
        public string CBU { get; set; }
        public string InformacionFamiliar { get => _informacionFamiliar; set => _informacionFamiliar = value; }
        #endregion

        public XmlDocument ObtenerFamiliares()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement raiz = doc.CreateElement(string.Empty, "Familiares", string.Empty);
            doc.AppendChild(raiz);
            foreach (AfiAfiliados fam in Familiares.Where(x => x.NumeroDocumento > 0).ToList())
            {
                //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                //XmlElement root = doc.DocumentElement;
                //doc.InsertBefore(xmlDeclaration, root);
                XmlElement Familiarxml = doc.CreateElement(string.Empty, "Familiar", string.Empty);
                raiz.AppendChild(Familiarxml);
                XmlElement Apellidoxml = doc.CreateElement(string.Empty, "Apellido", string.Empty);
                XmlText txtApellido = doc.CreateTextNode(fam.Apellido.ToString());
                Familiarxml.AppendChild(Apellidoxml);
                Apellidoxml.AppendChild(txtApellido);
                XmlElement Sexoxml = doc.CreateElement(string.Empty, "IdSexo", string.Empty);
                XmlText txtSexo = doc.CreateTextNode(fam.Sexo.IdSexo.ToString());
                Familiarxml.AppendChild(Sexoxml);
                Sexoxml.AppendChild(txtSexo);
                XmlElement Nombrexml = doc.CreateElement(string.Empty, "Nombre", string.Empty);
                XmlText txtNombre = doc.CreateTextNode(fam.Nombre.ToString());
                Familiarxml.AppendChild(Nombrexml);
                Nombrexml.AppendChild(txtNombre);
                XmlElement NumDocumentoxml = doc.CreateElement(string.Empty, "NumeroDocumento", string.Empty);
                XmlText txtNumeroDocumento = doc.CreateTextNode(fam.NumeroDocumento.ToString());
                Familiarxml.AppendChild(NumDocumentoxml);
                NumDocumentoxml.AppendChild(txtNumeroDocumento);
                XmlElement Parentescoxml = doc.CreateElement(string.Empty, "IdParentesco", string.Empty);
                XmlText txtParentesco = doc.CreateTextNode(fam.Parentesco.IdParentesco.ToString());
                Familiarxml.AppendChild(Parentescoxml);
                Parentescoxml.AppendChild(txtParentesco);


            }
            return doc;
        }
        public XmlDocument ObtenerDomicilios()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement lista = doc.CreateElement(string.Empty, "Domicilios", string.Empty);
            doc.AppendChild(lista);
            foreach (AfiDomicilios dom in Domicilios)
            {
                //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                //XmlElement root = doc.DocumentElement;
                //doc.InsertBefore(xmlDeclaration, root);
                XmlElement Domicilioxml = doc.CreateElement(string.Empty, "Domicilio", string.Empty);
                lista.AppendChild(Domicilioxml);
                XmlElement IdDomicilio = doc.CreateElement(string.Empty, "IdDomicilio", string.Empty);
                XmlText txtIdDomicilio = doc.CreateTextNode(dom.IdDomicilio.ToString());
                Domicilioxml.AppendChild(IdDomicilio);
                IdDomicilio.AppendChild(txtIdDomicilio);

                XmlElement IdDomicilioTipo = doc.CreateElement(string.Empty, "IdDomicilioTipo", string.Empty);
                XmlText txtIdDomicilioTipo = doc.CreateTextNode(dom.DomicilioTipo.IdDomicilioTipo.ToString());
                Domicilioxml.AppendChild(IdDomicilioTipo);
                IdDomicilioTipo.AppendChild(txtIdDomicilioTipo);

                XmlElement Provinciaxml = doc.CreateElement(string.Empty, "Provincia", string.Empty);
                XmlText txtProvincia = doc.CreateTextNode(dom.Localidad.Provincia.IdProvincia.ToString());
                Domicilioxml.AppendChild(Provinciaxml);
                Provinciaxml.AppendChild(txtProvincia);
                XmlElement Nacionalidadxml = doc.CreateElement(string.Empty, "Nacionalidad", string.Empty);
                XmlText txtNacionalidad = doc.CreateTextNode("");
                Domicilioxml.AppendChild(Nacionalidadxml);
                Nacionalidadxml.AppendChild(txtNacionalidad);
                XmlElement Localidadxml = doc.CreateElement(string.Empty, "Localidad", string.Empty);
                XmlText txtLocalidad = doc.CreateTextNode(dom.Localidad.IdCodigoPostal.ToString());
                Domicilioxml.AppendChild(Localidadxml);
                Localidadxml.AppendChild(txtLocalidad);
                XmlElement CodigoPostalxml = doc.CreateElement(string.Empty, "CodigoPostal", string.Empty);
                XmlText txtCodigoPostal = doc.CreateTextNode(dom.CodigoPostal.ToString());
                Domicilioxml.AppendChild(CodigoPostalxml);
                CodigoPostalxml.AppendChild(txtCodigoPostal);
                XmlElement Callexml = doc.CreateElement(string.Empty, "Calle", string.Empty);
                XmlText txtCalle = doc.CreateTextNode(dom.Calle.ToString());
                Domicilioxml.AppendChild(Callexml);
                Callexml.AppendChild(txtCalle);
                XmlElement Numeroxml = doc.CreateElement(string.Empty, "Numero", string.Empty);
                XmlText txtNumero = doc.CreateTextNode(dom.Numero.ToString());
                Domicilioxml.AppendChild(Numeroxml);
                Numeroxml.AppendChild(txtNumero);
                XmlElement Departamentoxml = doc.CreateElement(string.Empty, "Departamento", string.Empty);
                XmlText txtDepartamento = doc.CreateTextNode(dom.Departamento.ToString());
                Domicilioxml.AppendChild(Departamentoxml);
                Departamentoxml.AppendChild(txtDepartamento);
                XmlElement Pisoxml = doc.CreateElement(string.Empty, "Piso", string.Empty);
                XmlText txtPiso = doc.CreateTextNode(dom.Piso.ToString());
                Domicilioxml.AppendChild(Pisoxml);
                Pisoxml.AppendChild(txtPiso);

                XmlElement IdEstado = doc.CreateElement(string.Empty, "IdEstado", string.Empty);
                XmlText txtIdEstado = doc.CreateTextNode(dom.Estado.IdEstado.ToString());
                Domicilioxml.AppendChild(IdEstado);
                IdEstado.AppendChild(txtIdEstado);

            }
            return doc;

        }
        public XmlDocument ObtenerTelefonos()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement lista = doc.CreateElement(string.Empty, "Telefonos", string.Empty);
            doc.AppendChild(lista);
            foreach (AfiTelefonos tel in Telefonos)
            {
                XmlElement Telefonooxml = doc.CreateElement(string.Empty, "Telefono", string.Empty);
                lista.AppendChild(Telefonooxml);
                XmlElement IdTelefono = doc.CreateElement(string.Empty, "IdTelefono", string.Empty);
                XmlText txtIdTelefono = doc.CreateTextNode(tel.IdTelefono.ToString());
                Telefonooxml.AppendChild(IdTelefono);
                IdTelefono.AppendChild(txtIdTelefono);

                XmlElement IdTelefonoTipo = doc.CreateElement(string.Empty, "IdTelefonoTipo", string.Empty);
                XmlText txtIdTelefonoTipo = doc.CreateTextNode(tel.TelefonoTipo.IdTelefonoTipo.ToString());
                Telefonooxml.AppendChild(IdTelefonoTipo);
                IdTelefonoTipo.AppendChild(txtIdTelefonoTipo);

                //XmlElement Prefijo = doc.CreateElement(string.Empty, "Prefijo", string.Empty);
                //XmlText txtPrefijo = doc.CreateTextNode(tel.Prefijo.ToString());
                //Telefonooxml.AppendChild(Prefijo);
                //Prefijo.AppendChild(txtPrefijo);

                XmlElement Numero = doc.CreateElement(string.Empty, "Numero", string.Empty);
                XmlText txtNumero = doc.CreateTextNode(tel.Numero.ToString());
                Telefonooxml.AppendChild(Numero);
                Numero.AppendChild(txtNumero);

                XmlElement Interno = doc.CreateElement(string.Empty, "Interno", string.Empty);
                XmlText txtInterno = doc.CreateTextNode(tel.Interno.ToString());
                Telefonooxml.AppendChild(Interno);
                Interno.AppendChild(txtInterno);

                XmlElement IdEstado = doc.CreateElement(string.Empty, "IdEstado", string.Empty);
                XmlText txtIdEstado = doc.CreateTextNode(tel.Estado.IdEstado.ToString());
                Telefonooxml.AppendChild(IdEstado);
                IdEstado.AppendChild(txtIdEstado);
            }
            return doc;

        }
        //public XmlDocument CamposValores()
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlElement lista = doc.CreateElement(string.Empty, "Domicilios", string.Empty);
        //    doc.AppendChild(lista);
        //    foreach (TGECamposValores cps in Domicilios)
        //    {
        //        //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //        //XmlElement root = doc.DocumentElement;
        //        //doc.InsertBefore(xmlDeclaration, root);
        //        XmlElement Domicilioxml = doc.CreateElement(string.Empty, "Domicilio", string.Empty);
        //        lista.AppendChild(Domicilioxml);
        //        XmlElement Provinciaxml = doc.CreateElement(string.Empty, "Provincia", string.Empty);
        //        XmlText txtProvincia = doc.CreateTextNode(dom.Localidad.Provincia.IdProvincia.ToString());
        //        Domicilioxml.AppendChild(Provinciaxml);
        //        Provinciaxml.AppendChild(txtProvincia);
        //        XmlElement Nacionalidadxml = doc.CreateElement(string.Empty, "Nacionalidad", string.Empty);
        //        XmlText txtNacionalidad = doc.CreateTextNode("");
        //        Domicilioxml.AppendChild(Nacionalidadxml);
        //        Nacionalidadxml.AppendChild(txtNacionalidad);
        //        XmlElement Localidadxml = doc.CreateElement(string.Empty, "Localidad", string.Empty);
        //        XmlText txtLocalidad = doc.CreateTextNode(dom.Localidad.IdCodigoPostal.ToString());
        //        Domicilioxml.AppendChild(Localidadxml);
        //        Localidadxml.AppendChild(txtLocalidad);




        //    }
        //    return doc;

        //}

    }



    [Serializable]
    public partial class AfiAfiliadosDTO
    {
        [PrimaryKey]
        public int IdAfiliado { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public int IdTipoDocumento { get; set; }
        public Int64 NumeroDocumento { get; set; }

        public string TipoDocumentoDescripcion { get; set; }

        public string DescripcionCombo { get; set; }

        public int IdAfiliadoTipo { get; set; }

        public int IdCondicionFiscal { get; set; }

        public string CondicionFiscalDescripcion { get; set; }

        public string RazonSocial { get; set; }

        public string EstadoDescripcion { get; set; }

        public string Detalle { get; set; }

        public string DescripcionAfiliado { get; set; }

        public long Cuit { get; set; }

        public int Edad { get; set; }

        public string CategoriaDescripcion { get; set; }
        public string CorreoElectronico { get; set; }
        public string FechaNacimientoTexto { get; set; }

        public int? IdFormaCobro { get; set; }
    }
}
