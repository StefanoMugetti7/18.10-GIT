using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Hoteles;
using Hoteles.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel
{
    public partial class DescuentosListar : PaginaSegura
    {
        private DataTable MisDescuentos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "DescuentosListarMisDescuentos"]; }
            set { Session[this.MiSessionPagina + "DescuentosListarMisDescuentos"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                HTLDescuentosFiltros parametros = this.BusquedaParametrosObtenerValor<HTLDescuentosFiltros>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlHoteles.SelectedValue = parametros.IdHotel.ToString();
                    this.ddlTipoDescuento.SelectedValue = parametros.IdTipoDescuento.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            HTLDescuentosFiltros parametros = this.BusquedaParametrosObtenerValor<HTLDescuentosFiltros>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/DescuentosAgregar.aspx"), true);
        }
        private void CargarCombos()
        {
            HTLHoteles hotel = new HTLHoteles();
            hotel.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlHoteles.DataSource = HotelesF.HotelesObtenerListaActiva(hotel);
            this.ddlHoteles.DataValueField = "IdHotel";
            this.ddlHoteles.DataTextField = "Descripcion";
            this.ddlHoteles.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlHoteles, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoDescuento.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.HotelTiposDescuentos);
            this.ddlTipoDescuento.DataValueField = "IdListaValorDetalle";
            this.ddlTipoDescuento.DataTextField = "Descripcion";
            this.ddlTipoDescuento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoDescuento, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        #region grilla
        private void CargarLista(HTLDescuentosFiltros pDescuentos)
        {
            pDescuentos.IdHotel = this.ddlHoteles.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlHoteles.SelectedValue);
            pDescuentos.IdTipoDescuento = ddlTipoDescuento.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoDescuento.SelectedValue);
            pDescuentos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pDescuentos.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<HTLDescuentos>(pDescuentos);
            List<HTLDescuentos> descuentos = HotelesF.ObtenerLista(pDescuentos);
            this.gvDatos.DataSource = descuentos;
            this.gvDatos.PageIndex = pDescuentos.IndiceColeccion;
            this.gvDatos.DataBind();

        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            HTLDescuentos Descuento = new HTLDescuentos();
            Descuento.IdDescuento = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdDescuento"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdDescuento", Descuento.IdDescuento }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/DescuentosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/DescuentosConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnConsultar.Visible = this.ValidarPermiso("ReservasConsultar.aspx");
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            HTLDescuentos parametros = this.BusquedaParametrosObtenerValor<HTLDescuentos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<HTLDescuentos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisDescuentos;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisDescuentos = this.OrdenarGrillaDatos<DataTable>(this.MisDescuentos, e);
            this.gvDatos.DataSource = this.MisDescuentos;
            this.gvDatos.DataBind();
        }
        #endregion
    }
}