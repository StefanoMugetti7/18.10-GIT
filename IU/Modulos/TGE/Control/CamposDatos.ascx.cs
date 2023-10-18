using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE.Control
{
    public partial class CamposDatos : ControlesSeguros
    {
        private TGECampos MiCampo
        {
            get { return (TGECampos)Session[this.MiSessionPagina + "CamposDatosMiCampo"]; }
            set { Session[this.MiSessionPagina + "CamposDatosMiCampo"] = value; }
        }

        private List<TGEListasValoresDetalles> MisTablasParametros
        {
            get { return (List<TGEListasValoresDetalles>)Session[this.MiSessionPagina + "CamposDatosMisTablasParametros"]; }
            set { Session[this.MiSessionPagina + "CamposDatosMisTablasParametros"] = value; }
        }

        public delegate void CamposDatosAceptarEventHandler(object sender, TGECampos e);
        public event CamposDatosAceptarEventHandler CampoModificarDatosAceptar;

        public delegate void CamposDatosCancelarEventHandler();
        public event CamposDatosCancelarEventHandler CampoModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MiCampo == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una operacion
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TGECampos pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCampo = pParametro;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ddlEstado.Enabled = false;
                    this.ctrComentarios.IniciarControl(this.MiCampo, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiCampo = TGEGeneralesF.CamposObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiCampo);
                    break;
                case Gestion.Consultar:
                    this.MiCampo = TGEGeneralesF.CamposObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiCampo);
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlTablaValor.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TablasCamposDinamicos);
            this.ddlTablaValor.DataValueField = "Descripcion";
            this.ddlTablaValor.DataTextField = "Descripcion";
            this.ddlTablaValor.DataBind();

            this.ddlCampoTipo.DataSource = TGEGeneralesF.CamposTiposObtenerListaFiltro(new TGECamposTipos());
            this.ddlCampoTipo.DataValueField = "IdCampoTipo";
            this.ddlCampoTipo.DataTextField = "CampoTipo";
            this.ddlCampoTipo.DataBind();

            this.ddlTabla.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TablasParametrosCamposDinamicos);
            this.ddlTabla.DataValueField = "Descripcion";
            this.ddlTabla.DataTextField = "Descripcion";
            this.ddlTabla.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTabla, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlIdRefTabla, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlRefCampo.DataSource = TGEGeneralesF.CamposObtenerRefCamposCombo();
            this.ddlRefCampo.DataValueField = "IdRefCampo";
            this.ddlRefCampo.DataTextField = "TituloRefCampo";
            this.ddlRefCampo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlRefCampo, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarComboOpcionesTabla(TGEListasValoresDetalles pParametro)
        {
            this.ddlIdRefTabla.Items.Clear();
            this.ddlIdRefTabla.SelectedValue = null;
            this.ddlIdRefTabla.DataSource = TGEGeneralesF.ListasValoresObtenerConsultaDinamicaCombos(pParametro);
            this.ddlIdRefTabla.DataValueField = "IdListaValorDetalle";
            this.ddlIdRefTabla.DataTextField = "Descripcion";
            this.ddlIdRefTabla.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlIdRefTabla, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlTabla_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTabla.SelectedValue))
            {
                TGEListasValoresDetalles filtro = new TGEListasValoresDetalles();
                filtro.Descripcion = this.ddlTabla.SelectedItem.Text;
                filtro.ListaValor.IdListaValor = (int)EnumTGEListasValoresSistemas.TablasParametrosCamposDinamicos;
                filtro = TGEGeneralesF.ListasValoresDetallesObtenerPorFiltro(filtro);
                this.CargarComboOpcionesTabla(filtro);
            }
        }
        protected void ddlCampoTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlCampoTipo.SelectedValue))
            {
                this.pnlListaValor.Visible = false;
                this.pnlStoredProcedure.Visible = false;
                this.pnlConsultaDinamica.Visible = false;

                if (Convert.ToInt32(this.ddlCampoTipo.SelectedValue) == (int)EnumCamposTipos.DropDownList)
                {
                    this.pnlListaValor.Visible = true;
                    this.ddlListaValores.DataSource = TGEGeneralesF.ListasValoresObtenerListaFiltro(new TGEListasValores());
                    this.ddlListaValores.DataValueField = "IdListaValor";
                    this.ddlListaValores.DataTextField = "ListaValor";
                    this.ddlListaValores.DataBind();
                }
                else if (Convert.ToInt32(this.ddlCampoTipo.SelectedValue) == (int)EnumCamposTipos.GrillaDinamicaAB)
                {
                    this.pnlStoredProcedure.Visible = true;
                    this.pnlConsultaDinamica.Visible = true;
                }
                else if (Convert.ToInt32(this.ddlCampoTipo.SelectedValue) == (int)EnumCamposTipos.DropDownListQuery)
                {
                    this.pnlConsultaDinamica.Visible = true;
                }
                else
                {
                    this.pnlStoredProcedure.Visible = true;
                }
            }
        }
        private void MapearObjetoAControles(TGECampos pParametro)
        {
            this.txtNombre.Text = pParametro.Nombre;
            this.txtTitulo.Text = pParametro.Titulo;
            ListItem item = this.ddlTablaValor.Items.FindByValue(pParametro.TablaValor);
            if (item == null)
                this.ddlTablaValor.Items.Add(new ListItem(pParametro.TablaValor, pParametro.TablaValor));
            this.ddlTablaValor.SelectedValue = pParametro.TablaValor;

            item = this.ddlTabla.Items.FindByValue(pParametro.Tabla);
            if (item == null)
                this.ddlTabla.Items.Add(new ListItem(pParametro.Tabla, pParametro.Tabla));
            this.ddlTabla.SelectedValue = pParametro.Tabla;

            if (pParametro.Tabla != string.Empty)
            {
                TGEListasValoresDetalles filtro = new TGEListasValoresDetalles();
                filtro.Descripcion = this.ddlTabla.SelectedItem.Text;
                filtro.ListaValor.IdListaValor = (int)EnumTGEListasValoresSistemas.TablasParametrosCamposDinamicos;
                filtro = TGEGeneralesF.ListasValoresDetallesObtenerPorFiltro(filtro);
                this.CargarComboOpcionesTabla(filtro);

                item = this.ddlIdRefTabla.Items.FindByValue(pParametro.IdRefTabla.ToString());
                if (item == null && pParametro.IdRefTabla > 0)
                    this.ddlIdRefTabla.Items.Add(new ListItem(pParametro.IdRefTabla.ToString(), pParametro.IdRefTabla.ToString()));
                this.ddlIdRefTabla.SelectedValue = pParametro.IdRefTabla.ToString();
            }

            if (pParametro.EventoJavaScript != string.Empty)
            {
                this.txtEventoJavaScript.Text = pParametro.EventoJavaScript;
            }
            this.chkRequerido.Checked = pParametro.Requerido;
            this.chkMostrarWordpress.Checked = pParametro.wpmostrar;
            this.txtOrden.Text = pParametro.Orden.ToString();
            this.txtTamanioMinimo.Text = pParametro.TamanioMinimo.ToString();
            this.txtTamanioMaximo.Text = pParametro.TamanioMaximo.ToString();
            this.txtStoredProcedure.Text = pParametro.StoredProcedure;
            this.txtConsultaDinamica.Text = pParametro.ConsultaDinamica;
            this.txtStoredProcedureLeyenda.Text = pParametro.StoredProcedureLeyenda;
            this.txtClase.Text = pParametro.Clase;

            item = this.ddlCampoTipo.Items.FindByValue(pParametro.CampoTipo.IdCampoTipo.ToString());
            if (item == null)
                this.ddlCampoTipo.Items.Add(new ListItem(pParametro.CampoTipo.CampoTipo.ToString(), pParametro.CampoTipo.IdCampoTipo.ToString()));
            this.ddlCampoTipo.SelectedValue = pParametro.CampoTipo.IdCampoTipo.ToString();
            this.ddlCampoTipo_SelectedIndexChanged(null, EventArgs.Empty);

            if (pParametro.ListaValor.IdListaValor > 0)
            {
                //this.ddlCampoTipo_SelectedIndexChanged(null, EventArgs.Empty);
                item = this.ddlListaValores.Items.FindByValue(pParametro.ListaValor.IdListaValor.ToString());
                if (item == null)
                    this.ddlListaValores.Items.Add(new ListItem(pParametro.ListaValor.ListaValor, pParametro.ListaValor.IdListaValor.ToString()));
                this.ddlListaValores.SelectedValue = pParametro.ListaValor.IdListaValor.ToString();
            }
            this.chkSaltoLinea.Checked = pParametro.SaltoLinea;
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.chkDeshabilitado.Checked = pParametro.Deshabilitado;
            this.txtStoredProcedureValidaciones.Text = pParametro.StoredProcedureValidaciones;

            if (pParametro.IdRefCampo.HasValue)
            {
                item = this.ddlRefCampo.Items.FindByValue(pParametro.IdRefCampo.Value.ToString());
                if (item == null)
                    this.ddlRefCampo.Items.Add(new ListItem(pParametro.TituloRefCampo, pParametro.IdRefCampo.Value.ToString()));
                this.ddlRefCampo.SelectedValue = pParametro.IdRefCampo.Value.ToString();
            }
            if (pParametro.IdCampoDependiente.HasValue)
            {
                item = this.ddlCampoDependiente.Items.FindByValue(pParametro.IdCampoDependiente.Value.ToString());
                if (item == null)
                    this.ddlCampoDependiente.Items.Add(new ListItem(pParametro.NombreCampoDependiente, pParametro.IdCampoDependiente.Value.ToString()));
                this.ddlCampoDependiente.SelectedValue = pParametro.IdCampoDependiente.Value.ToString();
            }
            this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pParametro);
        }
        private void MapearControlesAObjeto(TGECampos pParametro)
        {
            pParametro.EventoJavaScript = this.txtEventoJavaScript.Text;
            pParametro.Nombre = this.txtNombre.Text;
            pParametro.Titulo = this.txtTitulo.Text;
            pParametro.TablaValor = this.ddlTablaValor.SelectedValue;
            pParametro.Tabla = this.ddlTabla.SelectedValue;
            pParametro.IdRefTabla = this.ddlIdRefTabla.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlIdRefTabla.SelectedValue);
            pParametro.Requerido = this.chkRequerido.Checked;
            pParametro.wpmostrar = this.chkMostrarWordpress.Checked;
            pParametro.Orden = this.txtOrden.Text == string.Empty ? 0 : Convert.ToInt32(this.txtOrden.Text);
            pParametro.TamanioMinimo = this.txtTamanioMinimo.Text == string.Empty ? 0 : Convert.ToInt32(this.txtTamanioMinimo.Text);
            pParametro.TamanioMaximo = this.txtTamanioMaximo.Text == string.Empty ? 0 : Convert.ToInt32(this.txtTamanioMaximo.Text);
            pParametro.CampoTipo.IdCampoTipo = Convert.ToInt32(this.ddlCampoTipo.SelectedValue);
            pParametro.ListaValor.IdListaValor = this.ddlListaValores.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlListaValores.SelectedValue);
            pParametro.SaltoLinea = this.chkSaltoLinea.Checked;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Comentarios = ctrComentarios.ObtenerLista();
            pParametro.StoredProcedure = this.txtStoredProcedure.Text;
            pParametro.StoredProcedureLeyenda = this.txtStoredProcedureLeyenda.Text;
            pParametro.ConsultaDinamica = this.txtConsultaDinamica.Text;
            pParametro.StoredProcedureValidaciones = txtStoredProcedureValidaciones.Text;
            pParametro.Deshabilitado = this.chkDeshabilitado.Checked;
            pParametro.IdRefCampo = this.ddlRefCampo.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlRefCampo.SelectedValue);
            pParametro.IdCampoDependiente = this.ddlCampoDependiente.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlCampoDependiente.SelectedValue);
            pParametro.NombreCampoDependiente = this.ddlCampoDependiente.SelectedValue == string.Empty ? string.Empty : ddlCampoDependiente.SelectedItem.Text;
            pParametro.Clase = this.txtClase.Text;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiCampo);
            this.MiCampo.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCampo.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.CamposAgregar(this.MiCampo);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.CamposModificar(this.MiCampo);
                    break;
                default:
                    break;
            }
            if (guardo)
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCampo.CodigoMensaje));
            else
                this.MostrarMensaje(this.MiCampo.CodigoMensaje, true, this.MiCampo.CodigoMensajeArgs);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.CampoModificarDatosCancelar != null)
                this.CampoModificarDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.CampoModificarDatosAceptar != null)
                this.CampoModificarDatosAceptar(null, this.MiCampo);
        }
        protected void ddlTablaValor_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTablaValor.SelectedValue))
            {
                TGECampos lista = new TGECampos();
                lista.TablaValor = this.ddlTablaValor.SelectedValue;

                this.ddlCampoDependiente.DataSource = TGEGeneralesF.CamposObtenerDependientes(lista);
                this.ddlCampoDependiente.DataValueField = "IdCampo";
                this.ddlCampoDependiente.DataTextField = "Titulo";
                this.ddlCampoDependiente.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCampoDependiente, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                this.UpdatePanel1.Update();
            }
        }
    }
}