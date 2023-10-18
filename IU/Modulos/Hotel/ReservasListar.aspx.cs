using Afiliados;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Hoteles;
using Hoteles.Entidades;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel
{
    public partial class ReservasListar : PaginaSegura
    {
        private DataTable MisReservas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ReservasListarMisReservas"]; }
            set { Session[this.MiSessionPagina + "ReservasListarMisReservas"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroReserva, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaIngreso, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaIngresoHasta, this.btnBuscar);
                this.CargarCombos();
                HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroReserva.Text = parametros.IdReserva == 0 ? string.Empty : parametros.IdReserva.ToString();
                    this.ddlReservaTipoDocumento.SelectedValue = parametros.IdTipoDocumento.HasValue ? parametros.IdTipoDocumento.Value.ToString() : string.Empty;
                    this.txtReservaNumeroDocumento.Text = parametros.NumeroDocumento;
                    this.txtApellido.Text = parametros.Apellido;
                    this.txtFechaIngreso.Text = parametros.FechaIngreso.HasValue ? parametros.FechaIngreso.Value.ToShortDateString() : string.Empty;
                    this.txtFechaIngresoHasta.Text = parametros.FechaIngresoHasta.HasValue ? parametros.FechaIngresoHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlHoteles.SelectedValue = parametros.IdHotel == 0 ? string.Empty : parametros.IdHotel.ToString();
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado == -1 ? string.Empty : parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasAgregar.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            HTLReservas reserva = new HTLReservas();
            reserva.IdReserva = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdReserva"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdReserva", reserva.IdReserva }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("ReservasConsultar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;

                if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosReservas.Finalizada
                    && !this.UsuarioActivo.EsAdministradorGeneral)
                    modificar.Visible = false;

            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<HTLReservas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisReservas;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisReservas = this.OrdenarGrillaDatos<DataTable>(this.MisReservas, e);
            this.gvDatos.DataSource = this.MisReservas;
            this.gvDatos.DataBind();
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisReservas;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosReservas));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlReservaTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlReservaTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlReservaTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlReservaTipoDocumento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlReservaTipoDocumento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            HTLHoteles hotel = new HTLHoteles();
            hotel.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlHoteles.DataSource = HotelesF.HotelesObtenerListaActiva(hotel);
            this.ddlHoteles.DataValueField = "IdHotel";
            this.ddlHoteles.DataTextField = "Descripcion";
            this.ddlHoteles.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlHoteles, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(HTLReservas pReserva)
        {
            pReserva.IdReserva = this.txtNumeroReserva.Text == string.Empty ? default(Int64) : Convert.ToInt64(this.txtNumeroReserva.Decimal);
            pReserva.IdTipoDocumento = this.ddlReservaTipoDocumento.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlReservaTipoDocumento.SelectedValue);
            pReserva.NumeroDocumento = this.txtReservaNumeroDocumento.Text;
            pReserva.Apellido = this.txtApellido.Text;
            pReserva.FechaIngreso = this.txtFechaIngreso.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaIngreso.Text);
            pReserva.FechaIngresoHasta = this.txtFechaIngresoHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaIngresoHasta.Text);
            pReserva.IdHotel = this.ddlHoteles.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlHoteles.SelectedValue);
            pReserva.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlEstados.SelectedValue);
            pReserva.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pReserva.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<HTLReservas>(pReserva);
            this.MisReservas = HotelesF.ReservasObtenerListaGrilla(pReserva);
            this.gvDatos.DataSource = this.MisReservas;
            this.gvDatos.PageIndex = pReserva.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatos);

            if (this.MisReservas.Rows.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }
    }
}
