using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;
using Compras;
using System.Data;

namespace IU.Modulos.Compras
{
    public partial class BuscarProductos : PaginaSegura
    {
        private DataTable BuscarProductosListas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CMPListasPreciosDetallesSeleccionarBuscarProducto"]; }
            set { Session[this.MiSessionPagina + "CMPListasPreciosDetallesSeleccionarBuscarProducto"] = value; }
        }

        //private BuscarProductos MiUltimaAgregada
        //{
        //    get { return (BuscarProductos)Session[this.MiSessionPagina + "ListasPreciosListarMiUltimaAgregada"]; }
        //    set { Session[this.MiSessionPagina + "ListasPreciosListarMiUltimaAgregada"] = value; }
        //}

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoProducto, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcionProducto, this.btnBuscar);
                CargarCombos(); 


                //BuscarProductos parametros = this.BusquedaParametrosObtenerValor<BuscarProductos>();
                //if (parametros.BusquedaParametros)
                //{

                //}
            }
        }


        private void CargarCombos()
        {
            this.ddlIvas.DataSource = TGEGeneralesF.TGEIVAAlicuotaObtenerLista();
            this.ddlIvas.DataValueField = "IdIVA";
            this.ddlIvas.DataTextField = "Descripcion";
            this.ddlIvas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlIvas, this.ObtenerMensajeSistema("SeleccioneOpcion"));


        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CMPListasPreciosDetalles parametros = this.BusquedaParametrosObtenerValor<CMPListasPreciosDetalles>();
            CargarLista(parametros);
        }

       

        private void CargarLista(CMPListasPreciosDetalles pParametro)
        {


            pParametro.IVAFiltro.IdIVA = this.ddlIvas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlIvas.SelectedValue);
            pParametro.Producto.IdProducto = this.txtCodigoProducto.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoProducto.Text);
            pParametro.Producto.Descripcion = this.txtDescripcionProducto.Text.Trim();
            //VER PARAMETROS QUE RECIBE EL STORE
            //pParametros.IdListaPrecioDetalle = this.txtCodigoProducto.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoProducto.Text);
            //pParametros.ListaPrecio = this..Text  ;
            
            //pParametros.RazonSocial = this.txtRazonSocial.Text.Trim();
            //pParametros.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            //pParametros.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CMPListasPreciosDetalles>(pParametro);
            this.BuscarProductosListas = ComprasF.CMPListasPreciosDetallesSeleccionarBuscarProducto(pParametro);
            //this.MiUltimaAgregada = this.MisListasPrecios.Find(x=>x.FechaAlta == (this.MisListasPrecios.Max(y => y.FechaAlta)));
            this.gvItems.DataSource = this.BuscarProductosListas;
            this.gvItems.PageIndex = pParametro.IndiceColeccion;
            this.gvItems.DataBind();
        }

        #region "Grilla"

        protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CMPListasPreciosDetalles parametros = this.BusquedaParametrosObtenerValor<CMPListasPreciosDetalles>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CMPListasPreciosDetalles>(parametros);

            gvItems.PageIndex = e.NewPageIndex;
            gvItems.DataSource = this.BuscarProductosListas;
            gvItems.DataBind();
        }



        protected void gvItems_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.BuscarProductosListas = this.OrdenarGrillaDatos<CMPListasPreciosDetalles>(this.BuscarProductosListas, e);
            this.gvItems.DataSource = this.BuscarProductosListas;
            this.gvItems.DataBind();
        }

        #endregion
    }
}