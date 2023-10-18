using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Expedientes.Entidades;
using Generales.FachadaNegocio;
using Expedientes;
using Generales.Entidades;

namespace IU.Modulos.Expedientes.Controles
{
    public partial class ExpedientesModificarDatos : ControlesSeguros
    {
        private ExpExpedientes MiExpediente
        {
            get { return (ExpExpedientes)Session[this.MiSessionPagina + "ExpedientesModificarDatosMiExpediente"]; }
            set { Session[this.MiSessionPagina + "ExpedientesModificarDatosMiExpediente"] = value; }
        }
        public delegate void ExpedientesDatosCancelarEventHandler();
        public event ExpedientesDatosCancelarEventHandler ExpedienteModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (this.IsPostBack)
            {
                if (this.MiExpediente == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        public void IniciarControl(ExpExpedientes pExpediente, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiExpediente = pExpediente;
                    this.ddlEstado.SelectedValue = ((int)EstadosExpedientes.Activo).ToString();
                    this.ddlEstado.Enabled = false;
                    this.CargarCombosDerivarExpediente(this.MiExpediente);
                    this.pnlDerivaciones.Visible = true;
                    this.ctrArchivos.IniciarControl(pExpediente, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiExpediente = ExpedientesF.ExpedientesObtenerDatosCompletos(pExpediente);
                    this.CargarCombosDerivarExpediente(this.MiExpediente);
                    this.MapearObjetoAControles(this.MiExpediente);
                    this.pnlDerivaciones.Visible = true;
                    break;
                case Gestion.Aceptar:
                    this.MiExpediente = ExpedientesF.ExpedientesObtenerDatosCompletos(pExpediente);
                    this.CargarCombosDerivarExpediente(this.MiExpediente);
                    this.MapearObjetoAControles(this.MiExpediente);
                    this.pnlDerivaciones.Visible = false;
                    this.btnAceptar.Text = this.ObtenerMensajeSistema("AceptarDerivacion");
                    break;
                case Gestion.Rechazar:
                    this.MiExpediente = ExpedientesF.ExpedientesObtenerDatosCompletos(pExpediente);
                    this.CargarCombosDerivarExpediente(this.MiExpediente);
                    this.MapearObjetoAControles(this.MiExpediente);
                    this.pnlDerivaciones.Visible = false;
                    this.btnAceptar.Text = this.ObtenerMensajeSistema("RechazarDerivacion");
                    this.txtTitulo.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.ddlTipoExpediente.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    break;
                case Gestion.Consultar:
                    this.MiExpediente = ExpedientesF.ExpedientesObtenerDatosCompletos(pExpediente);
                    this.MapearObjetoAControles(this.MiExpediente);
                    this.txtTitulo.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.ddlTipoExpediente.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAgregaTracking.Visible = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlTipoExpediente.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.ExpedientesTipos);
            this.ddlTipoExpediente.DataValueField = "IdListaValorDetalle";
            this.ddlTipoExpediente.DataTextField = "Descripcion";
            this.ddlTipoExpediente.DataBind();
            if (this.ddlTipoExpediente.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoExpediente, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosExpedientes));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }
        private void CargarCombosDerivarExpediente(ExpExpedientes pExpediente)
        {
            List<TGEFiliales> filiales = ExpedientesF.ExpedientesObtenerFilialesDerivar(pExpediente, this.UsuarioActivo.FilialPredeterminada, this.UsuarioActivo.SectorPredeterminado);
            this.ddlFilial.DataSource = filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            if (this.ddlFilial.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            
            this.ddlFilial_SelectedIndexChanged(null, EventArgs.Empty);
        }
        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFilial.SelectedValue))
            {
                TGEFiliales filial = new TGEFiliales();
                filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
                this.ddlSector.DataSource = ExpedientesF.ExpedientesObtenerSectoresDerivar(this.MiExpediente, filial);
                this.ddlSector.DataValueField = "IdSector";
                this.ddlSector.DataTextField = "Sector";
                this.ddlSector.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlSector, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                this.ddlSector.Items.Clear();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlSector, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        private void MapearObjetoAControles(ExpExpedientes pExpediente)
        {
            this.txtNumeroExpediente.Text = pExpediente.IdExpediente.ToString();
            this.txtTitulo.Text = pExpediente.Titulo;
            this.txtDescripcion.Text = pExpediente.Descripcion;
            this.ddlEstado.SelectedValue = pExpediente.Estado.IdEstado.ToString();
            this.ddlTipoExpediente.SelectedValue = pExpediente.ExpedienteTipo.IdExpedienteTipo.ToString();
            AyudaProgramacion.CargarGrillaListas<ExpExpedientesTracking>(this.MiExpediente.ExpedientesTracking, false, this.gvDatos, true);

            this.ctrComentarios.IniciarControl(pExpediente, this.GestionControl);
            this.ctrArchivos.IniciarControl(pExpediente, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pExpediente);
        }
        private void MapearControlesAObjeto(ExpExpedientes pExpediente)
        {
            pExpediente.Titulo = this.txtTitulo.Text;
            pExpediente.Descripcion = this.txtDescripcion.Text;
            pExpediente.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pExpediente.ExpedienteTipo.IdExpedienteTipo = Convert.ToInt32(this.ddlTipoExpediente.SelectedValue);
            pExpediente.Comentarios = ctrComentarios.ObtenerLista();
            pExpediente.Archivos = ctrArchivos.ObtenerLista();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiExpediente);
            this.MiExpediente.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ExpedientesF.ExpedientesAgregar(this.MiExpediente, this.UsuarioActivo.SectorPredeterminado);
                    break;
                case Gestion.Modificar:
                    guardo = ExpedientesF.ExpedientesModificar(this.MiExpediente);
                    break;
                case Gestion.Aceptar:
                    guardo = ExpedientesF.ExpedientesAceptarDerivacion(this.MiExpediente, this.UsuarioActivo.SectorPredeterminado);
                    break;
                case Gestion.Rechazar:
                    guardo = ExpedientesF.ExpedientesRechazarDerivacion(this.MiExpediente, this.UsuarioActivo.SectorPredeterminado);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.ctrPopUpComprobantes.CargarReporte(this.MiExpediente, EnumTGEComprobantes.ExpExpedientes); 
                this.btnAceptar.Visible = false;
                this.btnImprimir.Visible = true;
            }
            else
            {
                this.MostrarMensaje(this.MiExpediente.CodigoMensaje, true, this.MiExpediente.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ExpedienteModificarDatosCancelar != null)
                this.ExpedienteModificarDatosCancelar();
        }
        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            this.ctrPopUpComprobantes.CargarReporte(this.MiExpediente, EnumTGEComprobantes.ExpExpedientes);
        }
        #region Expedientes Tracking
        protected void btnAgregaTracking_Click(object sender, EventArgs e)
        {
            ExpExpedientesTracking expTracking = new ExpExpedientesTracking();
            expTracking.Fecha = DateTime.Now;
            expTracking.Sector.IdSector = Convert.ToInt32(this.ddlSector.SelectedValue);
            expTracking.Sector.Sector = this.ddlSector.SelectedItem.Text;
            expTracking.Sector.Filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
            expTracking.Sector.Filial.Filial = this.ddlFilial.SelectedItem.Text;
            expTracking.Estado.IdEstado = (int)EstadosExpedientesTracking.Derivado;
            expTracking.Estado = TGEGeneralesF.TGEEstadosObtener(expTracking.Estado);
            expTracking.EstadoColeccion = EstadoColecciones.Agregado;

            if (this.MiExpediente.ExpedientesTracking.Exists(
                x => x.Sector.IdSector == expTracking.Sector.IdSector
                && x.Sector.Filial.IdFilial == expTracking.Sector.Filial.IdFilial
                && x.EstadoColeccion == EstadoColecciones.Agregado))
            {
                this.MostrarMensaje("ValidarFilialSectorIngresado", true);
                return;
            }

            this.MiExpediente.ExpedientesTracking.Add(expTracking);
            expTracking.IndiceColeccion = this.MiExpediente.ExpedientesTracking.IndexOf(expTracking);
            this.MiExpediente.ExpedientesTracking = this.MiExpediente.ExpedientesTracking.OrderByDescending(x => x.Fecha).ToList();
            this.MiExpediente.ExpedientesTracking = AyudaProgramacion.AcomodarIndices<ExpExpedientesTracking>(this.MiExpediente.ExpedientesTracking);
            AyudaProgramacion.CargarGrillaListas<ExpExpedientesTracking>(this.MiExpediente.ExpedientesTracking, true, this.gvDatos, true);

            if (this.GestionControl==Gestion.Modificar)
                this.pnlDerivaciones.Visible = false;
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiExpediente.ExpedientesTracking.RemoveAt(indiceColeccion);
                AyudaProgramacion.CargarGrillaListas<ExpExpedientesTracking>(this.MiExpediente.ExpedientesTracking, true, this.gvDatos, true);
                if (this.GestionControl == Gestion.Modificar)
                   this.pnlDerivaciones.Visible = true;
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ExpExpedientesTracking item = (ExpExpedientesTracking)e.Row.DataItem;

                switch (this.GestionControl)
                {
                    case Gestion.Modificar:
                    case Gestion.Agregar:
                        if (item.EstadoColeccion == EstadoColecciones.Agregado)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                            bool permisoModificar = this.ValidarPermiso("ExpedientesModificar.aspx");
                            ibtn.Visible = permisoModificar;

                            string mensaje = this.ObtenerMensajeSistema("ExpedienteTrackingConfirmarBaja");
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiExpediente.ExpedientesTracking;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiExpediente.ExpedientesTracking = this.OrdenarGrillaDatos<ExpExpedientesTracking>(this.MiExpediente.ExpedientesTracking, e);
            this.gvDatos.DataSource = this.MiExpediente.ExpedientesTracking;
            this.gvDatos.DataBind();
        }
        #endregion
    }
}