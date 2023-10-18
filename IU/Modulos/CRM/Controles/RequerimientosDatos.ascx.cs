using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using CRM;
using CRM.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using static CRM.Entidades.CRMRequerimientos;

namespace IU.Modulos.CRM.Controles
{
    public partial class RequerimientosDatos : ControlesSeguros
    {
        public CRMRequerimientos MiRequerimiento
        {
            get { return this.PropiedadObtenerValor<CRMRequerimientos>("RequerimientosDatosMisRequerimientos"); }
            set { this.PropiedadGuardarValor("RequerimientosDatosMisRequerimientos", value); }
        }
        private string MenuCards
        {
            get
            {
                if (Session["MaestraMenuHtml"] != null)
                { return (string)Session["MaestraMenuHtml"]; }
                else
                { return string.Empty; }
            }
            set { Session["MaestraMenuHtml"] = value; }
        }
        public delegate void ControlDatosAceptarEventHandler(object sender, CRMRequerimientos e);
        public event ControlDatosAceptarEventHandler ControlModificarDatosAceptar;
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        private StringBuilder CardsBootStrap;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                this.CardsBootStrap = new StringBuilder();
                this.CargarCardsBootStrap();
                this.MenuCards = this.CardsBootStrap == null ? string.Empty : this.CardsBootStrap.ToString();
                this.ltrCards.Text = this.MenuCards;
                this.ctrCamposValores.IniciarControl(MiRequerimiento, new Objeto(), this.GestionControl);
            }
        }
        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosConsultar.aspx"),true);
        }
        #region REQUERIMIENTOS-INCIDENTES
        public void IniciarControl(CRMRequerimientos pParametro, Gestion pGestion, AfiAfiliados miAfiliado)
        {
            this.dvDatosCliente.Visible = false;
            this.hdfIdAfiliado.Value = miAfiliado.IdAfiliado.ToString();
            this.IniciarControl(pParametro, pGestion);
        }
        public void IniciarControl(CRMRequerimientos pParametro, Gestion pGestion)
        {
            this.MiRequerimiento = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrCamposValores.IniciarControl(this.MiRequerimiento, new Objeto(), this.GestionControl);
                    this.ddlEstado.Enabled = false;
                    this.tpArchivos.Visible = true;
                    this.btnIncidente.Visible = false;
                    this.btnDocumentoListar.Visible = false;
                    this.ctrArchivos.IniciarControl(new CRMRequerimientos(), this.GestionControl);
                    this.Botonera.Visible = false;
                    this.btnIntervencion.Visible = false;
                    this.txtFechaRequerimiento.Text = DateTime.Now.ToShortDateString();
                    if (pParametro.Afiliado.IdAfiliado > 0)
                    {
                        this.dvDatosCliente.Visible = false;
                    }
                    break;
                case Gestion.Modificar:
                    this.MiRequerimiento = RequerimientosF.RequerimientosObtenerDatosCompletos(this.MiRequerimiento);
                    this.MapearObjetoAControles(this.MiRequerimiento);
                    this.Submenu.Visible = true;
                    this.txtUsuarioAlta.Visible = true;
                    this.lblUsuarioAlta.Visible = true;
                    this.ctrArchivos.IniciarControl(this.MiRequerimiento, this.GestionControl);
                    this.btnAceptar.Visible = true;
                    this.btnCancelar.Visible = true;
                    this.ddlEstado.Enabled = true;
                    this.btnIntervencion_Click(null, new EventArgs());
                    break;
                case Gestion.Consultar:
                    this.MiRequerimiento = RequerimientosF.RequerimientosObtenerDatosCompletos(this.MiRequerimiento);
                    this.MapearObjetoAControles(this.MiRequerimiento);
                    this.CKEditor1.Enabled = false;
                    this.txtNombre.Enabled = false;
                    this.ddlCategoria.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.ddlTipoRequerimiento.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.txtFechaResolucion.Enabled = false;
                    this.txtFechaRequerimiento.Enabled = false;
                    this.txtFechaInternaResolucion.Enabled = false;
                    this.ddlPrioridad.Enabled = false;
                    this.ddlOrigenSolicitud.Enabled = false;
                    this.chkEsPrivado.Enabled = false;
                    this.Submenu.Visible = true;
                    this.ddlTecnico.Enabled = false;
                    this.txtUsuarioAlta.Visible = true;
                    this.lblUsuarioAlta.Visible = true;
                    this.ctrArchivos.IniciarControl(this.MiRequerimiento, this.GestionControl);
                    this.ddlNumeroSocio.Enabled = false;
                    this.Botonera.Visible = false;
                    this.btnIntervencion_Click(null, new EventArgs());
                    break;
                default:
                    break;
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiRequerimiento);
            this.MiRequerimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    MiRequerimiento.IdRequerimiento = 0;
                    guardo = RequerimientosF.RequerimientosAgregar(this.MiRequerimiento);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiRequerimiento.CodigoMensaje, false);
                        this.IniciarControl(this.MiRequerimiento, Gestion.Modificar);
                    }
                    break;
                case Gestion.Anular:
                    this.MiRequerimiento.Estado.IdEstado = (int)Estados.Baja;
                    guardo = RequerimientosF.RequerimientosModificar(this.MiRequerimiento);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiRequerimiento.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = RequerimientosF.RequerimientosModificar(this.MiRequerimiento);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiRequerimiento.CodigoMensaje, false);
                        this.IniciarControl(this.MiRequerimiento, Gestion.Modificar);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiRequerimiento.CodigoMensaje, true, this.MiRequerimiento.CodigoMensajeArgs);
                if (this.MiRequerimiento.dsResultado != null)
                {
                    this.MiRequerimiento.dsResultado = null;
                }
            }
            else
            {
                this.btnIncidente_Click(this.btnIncidente, EventArgs.Empty);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlCategoria.DataSource = RequerimientosF.RequerimientosObtenerCategorias();
            this.ddlCategoria.DataValueField = "IdCategoria";
            this.ddlCategoria.DataTextField = "Categoria";
            this.ddlCategoria.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlCategoria, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlPrioridad.DataSource = RequerimientosF.RequerimientosObtenerPrioridades();
            this.ddlPrioridad.DataValueField = "IdPrioridad";
            this.ddlPrioridad.DataTextField = "Prioridad";
            this.ddlPrioridad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrioridad, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoRequerimiento.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CRMRequerimientos);
            this.ddlTipoRequerimiento.DataValueField = "IdListaValorDetalle";
            this.ddlTipoRequerimiento.DataTextField = "Descripcion";
            this.ddlTipoRequerimiento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoRequerimiento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlOrigenSolicitud.DataSource = RequerimientosF.RequerimientosObtenerOrigenSolicitud();
            this.ddlOrigenSolicitud.DataValueField = "IdOrigenSolicitud";
            this.ddlOrigenSolicitud.DataTextField = "OrigenSolicitud";
            this.ddlOrigenSolicitud.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlOrigenSolicitud, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTecnico.DataSource = RequerimientosF.RequerimientosObtenerTecnicos();
            this.ddlTecnico.DataValueField = "IdTecnico";
            this.ddlTecnico.DataTextField = "Tecnico";
            this.ddlTecnico.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTecnico, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(CRMRequerimientos pParametro)
        {
            this.txtNombre.Text = pParametro.Nombre;
            this.CKEditor1.Text = pParametro.Descripcion;

            this.txtFechaRequerimiento.Text = pParametro.FechaRequerimiento.ToString() == string.Empty ? "" : pParametro.FechaRequerimiento.ToShortDateString();
            this.chkEsPrivado.Checked = pParametro.EsPrivado;

            this.ddlPrioridad.SelectedValue = pParametro.IdPrioridad == 0 ? "-1" : pParametro.IdPrioridad.ToString();
            this.ddlTipoRequerimiento.SelectedValue = pParametro.RequerimientosTipos.IdTipoRequerimiento.ToString();

            this.txtFechaInternaResolucion.Text = pParametro.FechaInternaResolucion.HasValue == false ? "" : pParametro.FechaInternaResolucion.Value.ToShortDateString();
            this.txtFechaResolucion.Text = pParametro.FechaResolucion.HasValue == false ? "" : pParametro.FechaResolucion.Value.ToShortDateString();
            this.txtUsuarioAlta.Text = pParametro.UsuarioAlta;

            ListItem item1 = ddlEstado.Items.FindByValue(pParametro.Estado.IdEstado.ToString());
            if (item1 == null)
                ddlEstado.Items.Add(new ListItem(pParametro.Estado.Descripcion, pParametro.Estado.IdEstado.ToString()));
            ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();

            ListItem item = ddlTipoRequerimiento.Items.FindByValue(pParametro.RequerimientosTipos.IdTipoRequerimiento.ToString());
            if (item == null)
                ddlTipoRequerimiento.Items.Add(new ListItem(pParametro.RequerimientosTipos.Descripcion, pParametro.RequerimientosTipos.IdTipoRequerimiento.ToString()));
            ddlTipoRequerimiento.SelectedValue = pParametro.RequerimientosTipos.IdTipoRequerimiento.ToString();

            if (pParametro.IdOrigenSolicitud > 0)
            {
                ListItem item3 = ddlOrigenSolicitud.Items.FindByValue(pParametro.IdOrigenSolicitud.ToString());
                if (item3 == null)
                    ddlOrigenSolicitud.Items.Add(new ListItem(pParametro.OrigenSolicitud, pParametro.IdOrigenSolicitud.ToString()));
                ddlOrigenSolicitud.SelectedValue = pParametro.IdOrigenSolicitud.ToString();
            }

            if (pParametro.IdTecnico > 0)
            {
                ListItem item4 = ddlTecnico.Items.FindByValue(pParametro.IdTecnico.ToString());
                if (item4 == null)
                    ddlTecnico.Items.Add(new ListItem(pParametro.Tecnico, pParametro.IdTecnico.ToString()));
                ddlTecnico.SelectedValue = pParametro.IdTecnico.ToString();
            }

            if (pParametro.IdCategoria.HasValue && pParametro.IdCategoria > 0)
            {
                ListItem item2 = ddlCategoria.Items.FindByValue(pParametro.IdCategoria.ToString());
                if (item2 == null)
                    ddlCategoria.Items.Add(new ListItem(pParametro.Categoria, pParametro.IdCategoria.ToString()));
                ddlCategoria.SelectedValue = pParametro.IdCategoria.ToString();
            }

            if (pParametro.Tabla == "AfiAfiliados" && pParametro.IdRefTabla.HasValue)
            {
                this.hdfIdAfiliado.Value = pParametro.IdRefTabla.ToString();
                this.button_Click(null, new EventArgs());
            }

            this.btnSeguimiento.Visible = this.ValidarPermiso("AgregarSeguimiento.aspx");
            this.btnDocumento.Visible = this.ValidarPermiso("AgregarDocumento.aspx");
            this.btnSolucion.Visible = this.ValidarPermiso("AgregarSolucion.aspx");

            if (this.GestionControl == Gestion.Modificar)
            {
                if(this.MiRequerimiento.IdUsuarioAlta == this.UsuarioActivo.IdUsuario)
                {
                    this.btnSolucion.Visible = this.ValidarPermiso("AgregarSolucionPropio.aspx");
                }
                if(this.MiRequerimiento.IdTecnico == this.UsuarioActivo.IdUsuario)
                {
                    this.btnSolucion.Visible = true; //si es el tecnico asignado, lo puede solucionar

                }
                this.CKEditor1.Enabled = this.ValidarPermiso("ModificarRequerimiento.aspx");
                this.txtNombre.Enabled = this.CKEditor1.Enabled;

                if (pParametro.IdUsuarioAlta == this.UsuarioActivo.IdUsuario)
                {
                    this.CKEditor1.Enabled = this.ValidarPermiso("ModificarRequerimientoPropio.aspx");
                    this.txtNombre.Enabled = this.CKEditor1.Enabled;
                }
            }

            this.Botonera.Visible = this.btnSeguimiento.Visible || this.btnDocumento.Visible || this.btnSolucion.Visible;
            this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);
            this.ctrCamposValores.IniciarControl(pParametro, pParametro.RequerimientosTipos, this.GestionControl);
        }
        private void MapearControlesAObjeto(CRMRequerimientos pParametro)
        {
            pParametro.Descripcion = this.CKEditor1.Text;
            pParametro.FechaRequerimiento = (DateTime)(this.txtFechaRequerimiento.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaRequerimiento.Text));
            pParametro.FechaResolucion = this.txtFechaResolucion.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaResolucion.Text);
            pParametro.FechaInternaResolucion = this.txtFechaInternaResolucion.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaInternaResolucion.Text);
            pParametro.IdCategoria = this.ddlCategoria.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCategoria.SelectedValue);
            pParametro.IdPrioridad = this.ddlPrioridad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPrioridad.SelectedValue);
            pParametro.IdTecnico = this.ddlTecnico.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTecnico.SelectedValue);
            pParametro.EsPrivado = this.chkEsPrivado.Checked;
            pParametro.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Nombre = this.txtNombre.Text;
            pParametro.IdOrigenSolicitud = this.ddlOrigenSolicitud.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlOrigenSolicitud.SelectedValue);
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
            pParametro.RequerimientosTipos.IdTipoRequerimiento = Convert.ToInt32(this.ddlTipoRequerimiento.SelectedValue);
            pParametro.Archivos = ctrArchivos.ObtenerLista();
            if (!string.IsNullOrEmpty(this.hdfIdAfiliado.Value))
            {
                pParametro.Tabla = "AfiAfiliados";
                pParametro.IdRefTabla = Convert.ToInt32(this.hdfIdAfiliado.Value);
            }
        }
        protected void button_Click(object sender, EventArgs e)
        {
            string txtNumeroSocio = this.hdfIdAfiliado.Value;
            AfiAfiliados miAfiliado = new AfiAfiliados();
            miAfiliado.IdAfiliado = txtNumeroSocio == string.Empty ? 0 : Convert.ToInt32(txtNumeroSocio);
            miAfiliado = AfiliadosF.AfiliadosObtenerDatos(miAfiliado);
            if (miAfiliado.IdAfiliado != 0)
            {
                this.MapearObjetoAControlesAfiliado(miAfiliado);
            }
            else
            {
                this.txtCUIT.Text = string.Empty;
                this.txtEstadoAfiliado.Text = string.Empty;
                this.txtCondicionFiscal.Text = string.Empty;
                miAfiliado.CodigoMensaje = "El cliente no existe";
                this.MostrarMensaje(miAfiliado.CodigoMensaje, true);
            }
        }
        private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        {
            this.ddlNumeroSocio.Items.Add(new ListItem(pAfiliado.DescripcionAfiliado, pAfiliado.IdAfiliado.ToString()));
            this.ddlNumeroSocio.SelectedValue = pAfiliado.IdAfiliado.ToString();
            this.lblCUIT.Text = pAfiliado.TipoDocumento.TipoDocumento;
            this.txtCUIT.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtDetalle.Text = pAfiliado.Detalle;
            this.txtEstadoAfiliado.Text = pAfiliado.Estado.Descripcion;
            this.txtCondicionFiscal.Text = pAfiliado.CondicionFiscal.Descripcion;
        }
        protected void btnIncidente_Click(object sender, EventArgs e)
        {
            this.TituloAgregarDocumentos.Visible = false;
            this.Botonera.Visible = this.btnSeguimiento.Visible || this.btnDocumento.Visible || this.btnSolucion.Visible;
            this.tpIncidentes.Visible = true;
            this.tpIntervenciones.Visible = false;
            this.tpHistorico.Visible = false;
            this.tpEstadisticas.Visible = false;
            this.tpTareas.Visible = false;
            this.tpArchivos.Visible = false;

            //this.CKEditorIntervencion.Visible = true;
            //this.CKEditorSolucion.Visible = false;
        }
        protected void ddlTipoOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoRequerimiento.SelectedValue))
            {
                CRMRequerimientosTipos aux = new CRMRequerimientosTipos
                {
                    IdTipoRequerimiento = Convert.ToInt32(this.ddlTipoRequerimiento.SelectedValue)
                };
                this.ctrCamposValores.IniciarControl(this.MiRequerimiento, aux, this.GestionControl);
            }
        }
        #endregion
        #region INTERVENCIONES
        protected void btnIntervencion_Click(object sender, EventArgs e)
        {
            this.TituloAgregarDocumentos.Visible = false;
            this.TituloAgregar.Visible = false;
            this.DatosSolucion.Visible = false;
            this.DatosSeguimiento.Visible = false;
            this.Botonera.Visible = this.btnSeguimiento.Visible || this.btnDocumento.Visible || this.btnSolucion.Visible;
            this.tpIncidentes.Visible = false;
            this.tpHistorico.Visible = false;
            this.tpEstadisticas.Visible = false;
            this.tpTareas.Visible = false;
            this.tpIntervenciones.Visible = true;
            this.tpArchivos.Visible = false;
            if (this.GestionControl == Gestion.Consultar)
            {
                this.Botonera.Visible = false;
            }
        }
        protected void btnSeguimiento_Click(object sender, EventArgs e)
        {
            this.TituloAgregarDocumentos.Visible = false;
            this.tpIncidentes.Visible = false;
            this.DatosSolucion.Visible = false;
            this.tpIntervenciones.Visible = true;
            this.DatosSeguimiento.Visible = true;
            this.CKEditorIntervencion.Visible = true;
            this.btnAceptarSeguimiento.Visible = true;
            this.btnCancelarSeguimiento.Visible = true;
            this.tpArchivos.Visible = false;
            this.TituloAgregar.Visible = true;
            this.TituloAgregar.InnerText = "AGREGAR SEGUIMIENTO";
        }
        protected void btnAgregarSeguimiento_Click(object sender, EventArgs e)
        {
            CRMSeguimientos crm = new CRMSeguimientos();
            crm.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            crm.IdRequerimiento = MiRequerimiento.IdRequerimiento;
            crm.Descripcion = this.CKEditorIntervencion.Text;
            this.CKEditorIntervencion.Text = string.Empty;
            bool guardo = RequerimientosF.RequerimientosAgregarSeguimiento(crm);
            if (guardo)
            {
                this.CardsBootStrap = new StringBuilder();
                this.CargarCardsBootStrap();
                this.MenuCards = this.CardsBootStrap == null ? string.Empty : this.CardsBootStrap.ToString();
                this.ltrCards.Text = this.MenuCards;
                this.DatosSeguimiento.Visible = false;
                this.TituloAgregar.Visible = false;
                this.MostrarMensaje("Seguimiento agregado con exito.", false);
            }
            else
                this.MostrarMensaje("Error al agregar un seguimiento.", true);

        }
        protected void btnCancelarSeguimiento_Click(object sender, EventArgs e)
        {
            this.DatosSeguimiento.Visible = false;
            this.DatosSolucion.Visible = false;
            this.btnAceptarSeguimiento.Visible = false;
            this.btnCancelarSeguimiento.Visible = false;
            this.CKEditorIntervencion.Visible = false;
            this.TituloAgregar.Visible = false;
        }
        protected void btnSolucion_Click(object sender, EventArgs e)
        {
            this.TituloAgregarDocumentos.Visible = false;
            this.tpIncidentes.Visible = false;
            this.tpIntervenciones.Visible = true;
            this.DatosSolucion.Visible = true;
            this.DatosSeguimiento.Visible = false;
            this.CKEditorSolucion.Visible = true;
            this.btnAceptarSolucion.Visible = true;
            this.btnCancelarSolucion.Visible = true;
            this.tpArchivos.Visible = false;
            this.TituloAgregar.Visible = true;
            this.TituloAgregar.InnerText = "AGREGAR SOLUCION";
        }
        protected void btnAgregarSolucion_Click(object sender, EventArgs e)
        {
            CRMSeguimientos crm = new CRMSeguimientos();
            crm.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            crm.IdRequerimiento = this.MiRequerimiento.IdRequerimiento;
            crm.Descripcion = this.CKEditorSolucion.Text;
            crm.Estado.IdEstado = (int)EstadosRequerimientos.Solucionado;
            this.CKEditorSolucion.Text = string.Empty;
            bool guardo = RequerimientosF.RequerimientosAgregarSolucion(crm);
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje("Solucion agregada con exito.");
            }
            else
                this.MostrarMensaje("Error al agregar la solucion.", true);

        }
        protected void btnCancelarSolucion_Click(object sender, EventArgs e)
        {
            this.DatosSolucion.Visible = false;
            this.DatosSeguimiento.Visible = false;
            this.btnAceptarSolucion.Visible = false;
            this.btnCancelarSolucion.Visible = false;
            this.CKEditorSolucion.Visible = false;
            this.TituloAgregar.Visible = false;
        }
        private void CargarCardsBootStrap()
        {
            CRMRequerimientos crm = new CRMRequerimientos();
            crm.IdRequerimiento = MiRequerimiento.IdRequerimiento;
            DataTable cards = RequerimientosF.RequerimientosCargarCardsBootStrap(crm);
            if (cards.Rows.Count > 0)
            {
                foreach (DataRow fila in cards.Rows)
                {
                    //     this.CardsBootStrap.AppendLine(" <div class=\"card card-prestamos  \" >");
                    this.CardsBootStrap.AppendLine(" <div class=\"col-lg-10 col-md-10 col-sm-10\">");
                    this.CardsBootStrap.AppendLine(" <div class=\"card  \" >");
                    // this.CardsBootStrap.AppendLine(" <div class=\"card-group  \" >");     
                    this.CardsBootStrap.AppendFormat(" <div class=\"card-body {0} \" >", fila["Color"]);
                    this.CardsBootStrap.AppendFormat("<p class=\"card-requerimientos card-text\">{0}</p>", fila["Descripcion"]);
                    this.CardsBootStrap.AppendLine("</div>");
                    this.CardsBootStrap.AppendLine("</div>");
                    this.CardsBootStrap.AppendLine("</div>");
                }
            }
        }
        #endregion
        #region DOCUMENTOS
        protected void btnDocumentoListar_Click(object sender, EventArgs e)
        {
            this.TituloAgregarDocumentos.Visible = false;
            this.Botonera.Visible = this.btnSeguimiento.Visible || this.btnDocumento.Visible || this.btnSolucion.Visible;
            this.tpIncidentes.Visible = false;
            this.tpIntervenciones.Visible = false;
            this.tpHistorico.Visible = false;
            this.tpEstadisticas.Visible = false;
            this.tpTareas.Visible = false;
            this.tpArchivos.Visible = true;
            this.ctrArchivos.IniciarControl(this.MiRequerimiento, Gestion.Consultar);
            this.TituloAgregar.Visible = true;
            this.TituloAgregar.InnerText = "DOCUMENTOS";
        }
        protected void btnDocumento_Click(object sender, EventArgs e)
        {
            //this.Botonera.Visible = false;
            this.TituloAgregarDocumentos.Visible = true;
            this.tpIncidentes.Visible = false;
            this.tpIntervenciones.Visible = false;
            this.tpHistorico.Visible = false;
            this.tpEstadisticas.Visible = false;
            this.tpTareas.Visible = false;
            this.tpArchivos.Visible = true;
            this.ctrArchivos.IniciarControl(this.MiRequerimiento, Gestion.Agregar);
        }
        #endregion
    }
}