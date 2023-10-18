using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subsidios.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Subsidios;
using Generales.Entidades;

namespace IU.Modulos.Subsidios.Controles
{
    public partial class SubsidiosDatos : ControlesSeguros
    {

        private SubSubsidios MiSolicitud
        {
            get { return (SubSubsidios)Session[this.MiSessionPagina + "SubsidiosDatosMiSolicitud"]; }
            set { Session[this.MiSessionPagina + "SubsidiosDatosMiSolicitud"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        public delegate void SubsidiosDatosAceptarEventHandler(object sender, SubSubsidios e);
        public event SubsidiosDatosAceptarEventHandler SubsidiosModificarDatosAceptar;

        public delegate void SubsidiosDatosCancelarEventHandler();
        public event SubsidiosDatosCancelarEventHandler SubsidiosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrEscalas.EscalasModificarDatosAceptar += new SubEscalasDatosPopUp.SubEscalasDatosEventHandler(ctrEscalas_EscalasModificarDatosAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiSolicitud == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        public void IniciarControl(SubSubsidios pSolicitud, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            //this.MisSubsidios = SubsidiosF.SubsidiosObtenerListaFiltro(new SubSubsidios());
            this.CargarCombos();
            this.MiSolicitud = pSolicitud;

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrCamposValores.IniciarControl(this.MiSolicitud, new Objeto(), this.GestionControl);
                    AyudaProgramacion.CargarGrillaListas<SubEscalas>(this.MiSolicitud.Escalas, false, this.gvDatos, true);
                    break;
                case Gestion.Modificar:
                    this.MiSolicitud = SubsidiosF.SubsidiosObtenerDatosCompletos(this.MiSolicitud);
                    this.MapearObjetoControles(this.MiSolicitud);
                    break;
                case Gestion.Consultar:
                    this.MiSolicitud = SubsidiosF.SubsidiosObtenerDatosCompletos(this.MiSolicitud);
                    this.MapearObjetoControles(this.MiSolicitud);
                    this.btnAceptar.Visible = false;
                    this.btnAgregarEscalas.Visible = false;
                    break;
            }
        }

        private void CargarCombos()
        {
            //this.ddlSubsidio.DataSource = null;// TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.TiposFacturas);
            //this.ddlSubsidio.DataValueField = "IdListaValorDetalle";
            //this.ddlSubsidio.DataTextField = "Descripcion";
            //this.ddlSubsidio.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlSubsidio, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
         

            this.ddlTipoSubsidio.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposSubsidios);
            this.ddlTipoSubsidio.DataValueField = "IdListaValorDetalle";
            this.ddlTipoSubsidio.DataTextField = "Descripcion";
            this.ddlTipoSubsidio.DataBind();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.SubsidiosModificarDatosAceptar != null)
                this.SubsidiosModificarDatosAceptar(null, this.MiSolicitud);
        }

        protected void MapearControlesAObjeto(SubSubsidios pSolicitud)
        {
            pSolicitud.Descripcion = this.txtDescripcion.Text;
            pSolicitud.CantidadMaxima = this.txtCantidadMaxima.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadMaxima.Text);
            pSolicitud.FrecuenciaAnual = this.txtFrecuenciaAnual.Text == string.Empty ? 0 : Convert.ToInt32(this.txtFrecuenciaAnual.Text);
           // pSolicitud.MesesCarencia = this.txtMesesCarencia.Text == string.Empty ? 0 : Convert.ToInt32(this.txtMesesCarencia.Text);
            pSolicitud.SubsidioTipo.IdSubsidioTipo = this.ddlTipoSubsidio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoSubsidio.SelectedValue);
            pSolicitud.Campos = this.ctrCamposValores.ObtenerLista();
            pSolicitud.Estado.IdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            pSolicitud.MesesValidezEvento = txtMesesValidezEvento.Text == string.Empty ? 0 : Convert.ToInt32(this.txtMesesValidezEvento.Text);
            pSolicitud.ModificaImporte = chkModificaImporte.Checked;
        }

        protected void MapearObjetoControles(SubSubsidios pSolicitud)
        {this.ddlEstado.SelectedValue = pSolicitud.Estado.IdEstado.ToString();
            this.txtDescripcion.Text = pSolicitud.Descripcion;
            this.txtCantidadMaxima.Text = pSolicitud.CantidadMaxima.ToString();
            this.txtFrecuenciaAnual.Text = pSolicitud.FrecuenciaAnual.ToString();
          //  this.txtMesesCarencia.Text = pSolicitud.MesesCarencia.ToString();
            this.ddlTipoSubsidio.SelectedValue = pSolicitud.SubsidioTipo.IdSubsidioTipo.ToString();
            this.txtMesesValidezEvento.Text = pSolicitud.MesesValidezEvento.ToString();
            this.chkModificaImporte.Checked = pSolicitud.ModificaImporte;
            AyudaProgramacion.CargarGrillaListas<SubEscalas>(pSolicitud.Escalas, false, this.gvDatos, true);
            this.ctrCamposValores.IniciarControl(this.MiSolicitud, new Objeto(), this.GestionControl);
        }

        #region "Grilla Solicitud Detalles"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrEscalas.IniciarControl(this.MiSolicitud.Escalas[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrEscalas.IniciarControl(this.MiSolicitud.Escalas[indiceColeccion], Gestion.Consultar);
            }
            else if (e.CommandName == "Borrar")
            {
                this.MiSolicitud.Escalas.RemoveAt(indiceColeccion);
                this.MiSolicitud.Escalas = AyudaProgramacion.AcomodarIndices<SubEscalas>(this.MiSolicitud.Escalas);
                AyudaProgramacion.CargarGrillaListas<SubEscalas>(this.MiSolicitud.Escalas, false, this.gvDatos, true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SubEscalas item = (SubEscalas)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.Cells[4].FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("DiasHorasConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        void ctrEscalas_EscalasModificarDatosAceptar(SubEscalas e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiSolicitud.Escalas.Add(e);
                    e.IndiceColeccion = this.MiSolicitud.Escalas.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiSolicitud.Escalas[this.MiIndiceDetalleModificar] = e;
                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiSolicitud.Escalas, false, this.gvDatos, true);
            this.upEscalas.Update();
        }

        protected void btnAgregarEscalas_Click(object sender, EventArgs e)
        {
            SubEscalas escala = new SubEscalas();
            escala.IdSubsidio = this.MiSolicitud.IdSubsidio;
            this.ctrEscalas.IniciarControl(escala, Gestion.Agregar);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.PersistirDatos();
            this.Page.Validate("AfiliadosModificarDatosAceptar");
            if (!this.Page.IsValid)
                return;

            bool guardo = false;

            this.MapearControlesAObjeto(this.MiSolicitud);
            //this.ActualizarGrilla();
            this.MiSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = SubsidiosF.SubsidiosAgregar(this.MiSolicitud);
                    break;
                case Gestion.Modificar:
                    guardo = SubsidiosF.SubsidiosModificar(this.MiSolicitud);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiSolicitud.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiSolicitud.CodigoMensaje, true, this.MiSolicitud.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.SubsidiosModificarDatosCancelar != null)
                this.SubsidiosModificarDatosCancelar();
        }

    }
}