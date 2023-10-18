using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestamos.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Generales.Entidades;
using System.Xml;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PrestamosCesionesDatos : ControlesSeguros
    {
        private PrePrestamosCesiones MiPrestamoCesion
        {
            get
            {
                return (Session[this.MiSessionPagina + "PrestamosCesionesDatosMiPrestamoCesion"] == null ?
                    (PrePrestamosCesiones)(Session[this.MiSessionPagina + "PrestamosCesionesDatosMiPrestamoCesion"] = new PrePrestamosCesiones()) : (PrePrestamosCesiones)Session[this.MiSessionPagina + "PrestamosCesionesDatosMiPrestamoCesion"]);
            }
            set { Session[this.MiSessionPagina + "PrestamosCesionesDatosMiPrestamoCesion"] = value; }
        }

        public delegate void PrestamosCesionesDatosAceptarEventHandler(object sender, PrePrestamosCesiones e);
        public event PrestamosCesionesDatosAceptarEventHandler ModificarDatosAceptar;

        public delegate void PrestamosCesionesDatosCancelarEventHandler();
        public event PrestamosCesionesDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);

            //Type t = this.GetType();
            //if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, "PrestamosCesionesDatos"))
            //    Page.ClientScript.RegisterClientScriptInclude(t, "PrestamosCesionesDatos", "~/Modulos/Prestamos/Controles/PrestamosCesionesDatos.js");
            if (!this.IsPostBack)
            {

            }
        }

        public void IniciarControl(PrePrestamosCesiones pParametro, Gestion pGestion)
        {
            this.MiPrestamoCesion = pParametro;
            this.GestionControl = pGestion;

            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPrestamoCesion.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.PresamosCesiones;
                    this.ddlCesionario.Enabled = true;
                    this.txtDescripcion.Enabled = true;
                    this.ddlEstado.Enabled = false;
                    this.txtTasa.Enabled = true;
                    this.pnlBuscar.Visible = true;
                    this.btnCalcularVAN.Visible = true;
                    //this.MiPrestamoCesion.PrestamosCesionesDetalles = PrePrestamosF.PrestamosCesionesObtenerPrestamosDisponibles(new PrePrestamosCesionesDetalles());
                    this.ctrComentarios.IniciarControl(this.MiPrestamoCesion, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiPrestamoCesion, this.GestionControl);
                    break;
                   case Gestion.Autorizar:
                    this.MiPrestamoCesion = PrePrestamosF.PrestamosCesionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiPrestamoCesion);
                    this.btnBuscar.Visible = false;
                    break;
                case Gestion.Anular:
                    this.MiPrestamoCesion = PrePrestamosF.PrestamosCesionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiPrestamoCesion);
                    break;
                case Gestion.Consultar:
                    this.MiPrestamoCesion = PrePrestamosF.PrestamosCesionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiPrestamoCesion);
                    this.btnAceptar.Visible = false;
                    this.btnBuscar.Visible = false;
                    break;
                default:
                    break;
            }

            this.ctrComentarios.IniciarControl(pParametro, pGestion);
            this.ctrArchivos.IniciarControl(pParametro, pGestion);
            this.ctrAuditoria.IniciarControl(pParametro);
        }

        private void CargarCombos()
        {          
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestamos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.SelectedValue = ((int)EstadosPrestamos.Activo).ToString();

            this.ddlCesionario.DataSource = PrePrestamosF.CesionariosObtenerListaFiltro(new PreCesionarios());
            this.ddlCesionario.DataValueField = "IdCesionario";
            this.ddlCesionario.DataTextField = "MiProveedorRazonSocial";
            this.ddlCesionario.DataBind();

            PrePrestamosPlanes planFiltro = new PrePrestamosPlanes();
            planFiltro.Estado.IdEstado = (int)Estados.Activo;
            planFiltro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.PrestamosLargoPlazo;
            this.ddlPlan.DataSource = PrePrestamosF.PrestamosPlanesObtenerLista(planFiltro);
            this.ddlPlan.DataValueField = "IdPrestamoPlan";
            this.ddlPlan.DataTextField = "Descripcion";
            this.ddlPlan.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearControlesAObjeto(PrePrestamosCesiones pParametro)
        {
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Cesionario.IdCesionario = Convert.ToInt32(this.ddlCesionario.SelectedValue);
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.Tasa = this.txtTasa.Decimal;
            pParametro.Comentarios = ctrComentarios.ObtenerLista();
            pParametro.Archivos = ctrArchivos.ObtenerLista();
        }

        private void MapearObjetoAControles(PrePrestamosCesiones pParametro)
        {
            this.ddlCesionario.SelectedValue = pParametro.Cesionario.IdCesionario.ToString();
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.txtDescripcion.Text = pParametro.Descripcion;
            this.txtCantidad.Text = pParametro.Cantidad.ToString();
            this.txtTotalAmortizacion.Text = pParametro.TotalAmortizacion.ToString("N2");
            this.txtTotalInteres.Text = pParametro.TotalInteres.ToString("N2");
            this.txtTasa.Text = pParametro.Tasa.ToString("N2");
            this.txtVAN.Text = pParametro.VAN.ToString("N2");
            
            AyudaProgramacion.CargarGrillaListas<PrePrestamosCesionesDetalles>(pParametro.PrestamosCesionesDetalles, false, this.gvDatos, true);

            this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            this.ctrArchivos.IniciarControl(pParametro, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pParametro);
        }

        private void PersistirLista()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                CheckBox check = ((CheckBox)fila.FindControl("chkIncluir"));
                this.MiPrestamoCesion.PrestamosCesionesDetalles[fila.DataItemIndex].Incluir = check.Checked;
                if (check.Checked)
                {
                    this.MiPrestamoCesion.PrestamosCesionesDetalles[fila.DataItemIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiPrestamoCesion.PrestamosCesionesDetalles[fila.DataItemIndex], this.GestionControl);
                }
                else
                {
                    this.MiPrestamoCesion.PrestamosCesionesDetalles[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.SinCambio;
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PrePrestamosCesionesDetalles filtro = new PrePrestamosCesionesDetalles();
            filtro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            filtro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            filtro.RiesgoCrediticio = Convert.ToInt32(this.txtRiesgoCrediticio.Decimal);
            filtro.CantidadCuotas = Convert.ToInt32(this.txtCantidadCuotasPendientes.Decimal);
            filtro.IdPrestamoPlan = this.ddlPlan.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPlan.SelectedValue);
            this.MiPrestamoCesion.PrestamosCesionesDetalles = PrePrestamosF.PrestamosCesionesObtenerPrestamosDisponibles(filtro);
            AyudaProgramacion.CargarGrillaListas<PrePrestamosCesionesDetalles>(this.MiPrestamoCesion.PrestamosCesionesDetalles, false, this.gvDatos, true );
        }

        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                this.MiPrestamoCesion.PrestamosCesionesDetalles[fila.DataItemIndex].Incluir = chkIncluir.Checked;
                if (chkIncluir.Checked)
                    this.MiPrestamoCesion.PrestamosCesionesDetalles[fila.DataItemIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiPrestamoCesion.PrestamosCesionesDetalles[fila.DataItemIndex], GestionControl);
                else
                    this.MiPrestamoCesion.PrestamosCesionesDetalles[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.SinCambio;
            }
        }

        protected void btnCalcularVAN_Click(object sender, EventArgs e)
        {
            
            this.PersistirDatosGrilla();
            this.MiPrestamoCesion.Tasa = this.txtTasa.Decimal;
            PrePrestamosF.PrestamosCesionesCalcularVAN(this.MiPrestamoCesion);
            this.txtVAN.Text = this.MiPrestamoCesion.VAN.ToString("C2");
            this.upCalcularVAN.Update();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiPrestamoCesion);
            this.MiPrestamoCesion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    int cantidadPrevia = this.MiPrestamoCesion.PrestamosCesionesDetalles.Count(x => x.Incluir);
                    this.PersistirDatosGrilla();
                    if (cantidadPrevia != this.MiPrestamoCesion.PrestamosCesionesDetalles.Count(x => x.Incluir))
                    {
                        this.btnCalcularVAN_Click(null, EventArgs.Empty);
                        this.MostrarMensaje("ValidarCalculoVAN", true);
                        return;
                    }
                    this.MiPrestamoCesion.PrestamosCesionesDetalles = this.MiPrestamoCesion.PrestamosCesionesDetalles.Where(x => x.Incluir).ToList();
                    guardo = PrePrestamosF.PrestamosCesionesAgregar(this.MiPrestamoCesion);
                    break;
                case Gestion.Autorizar:
                    this.MiPrestamoCesion.Estado.IdEstado = (int)EstadosCesiones.Autorizado;
                    this.MiPrestamoCesion.FechaAutorizar = DateTime.Now;
                    this.MiPrestamoCesion.IdUsuarioAutorizar = this.UsuarioActivo.IdUsuario;
                    guardo = PrePrestamosF.PrestamosCesionesAutorizar(this.MiPrestamoCesion);
                    break;
                case Gestion.Anular:
                    this.MiPrestamoCesion.Estado.IdEstado = (int)EstadosCesiones.Baja;
                    this.MiPrestamoCesion.FechaBaja = DateTime.Now;
                    guardo = PrePrestamosF.PrestamosCesionesModificar(this.MiPrestamoCesion);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiPrestamoCesion.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiPrestamoCesion.CodigoMensaje, true, this.MiPrestamoCesion.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar(null, this.MiPrestamoCesion);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox chkTodos = (CheckBox)e.Row.FindControl("chkTodos");
                    chkTodos.Visible = true;
                    //chkTodos.Attributes.Add("onclick", "MarcarTodos(this);");
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    //ibtnConsultar.Attributes.Add("onclick", "CalcularTotales();");
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalAmortizacion = (Label)e.Row.FindControl("lblTotalAmortizacion");
                lblTotalAmortizacion.Text = this.MiPrestamoCesion.PrestamosCesionesDetalles.Sum(x => x.ImporteAmortizacion).ToString("C2");
                Label lblTotalInteres = (Label)e.Row.FindControl("lblTotalInteres");
                lblTotalInteres.Text = this.MiPrestamoCesion.PrestamosCesionesDetalles.Sum(x => x.ImporteInteres).ToString("C2");
                Label lblGrillaTotalRegistros = (Label)e.Row.FindControl("lblGrillaTotalRegistros");
                lblGrillaTotalRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiPrestamoCesion.PrestamosCesionesDetalles.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //PrePrestamosPlanes parametros = this.BusquedaParametrosObtenerValor<PrePrestamosPlanes>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<PrePrestamosPlanes>(parametros);
            this.PersistirDatosGrilla();
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiPrestamoCesion.PrestamosCesionesDetalles;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiPrestamoCesion.PrestamosCesionesDetalles = this.OrdenarGrillaDatos<PrePrestamosCesionesDetalles>(this.MiPrestamoCesion.PrestamosCesionesDetalles, e);
            AyudaProgramacion.CargarGrillaListas<PrePrestamosCesionesDetalles>(this.MiPrestamoCesion.PrestamosCesionesDetalles, false, this.gvDatos, true);
        }
    }
}