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

namespace IU.Modulos.Compras
{
    public partial class ListasPreciosListar : PaginaSegura
    {
        private List<CMPListasPrecios> MisListasPrecios
        {
            get { return (List<CMPListasPrecios>)Session[this.MiSessionPagina + "ListasPreciosListarCMPListasPrecios"]; }
            set { Session[this.MiSessionPagina + "ListasPreciosListarCMPListasPrecios"] = value; }
        }

        //private CMPListasPrecios MiUltimaAgregada
        //{
        //    get { return (CMPListasPrecios)Session[this.MiSessionPagina + "ListasPreciosListarMiUltimaAgregada"]; }
        //    set { Session[this.MiSessionPagina + "ListasPreciosListarMiUltimaAgregada"] = value; }
        //}

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
           
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoLista, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("ListasPreciosAgregar.aspx");
                this.CargarCombos();

                CMPListasPrecios parametros = this.BusquedaParametrosObtenerValor<CMPListasPrecios>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCodigoLista.Text = parametros.IdListaPrecio == 0 ? String.Empty : parametros.IdListaPrecio.ToString();
                    //this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtDescripcion.Text = parametros.Descripcion;
                    this.txtRazonSocial.Text = parametros.RazonSocial;
                    this.CargarLista(parametros);
                }
            }
        }

        
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosListasPrecios));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CMPListasPrecios parametros = this.BusquedaParametrosObtenerValor<CMPListasPrecios>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ListasPreciosAgregar.aspx"), true);
        }

        private void CargarLista(CMPListasPrecios pParametros)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pParametros.IdListaPrecio = this.txtCodigoLista.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoLista.Text);
            pParametros.Descripcion = this.txtDescripcion.Text;
            pParametros.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametros.RazonSocial = this.txtRazonSocial.Text.Trim();
            pParametros.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametros.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametros.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CMPListasPrecios>(pParametros);
            this.MisListasPrecios = ComprasF.ListasPreciosObtenerListaFiltro(pParametros);
            //this.MiUltimaAgregada = this.MisListasPrecios.Find(x=>x.FechaAlta == (this.MisListasPrecios.Max(y => y.FechaAlta)));
            this.gvDatos.DataSource = this.MisListasPrecios;
            this.gvDatos.PageIndex = pParametros.IndiceColeccion;
            this.gvDatos.DataBind();
        }

        #region "Grilla"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CMPListasPrecios pListaPrecio = this.MisListasPrecios[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdListaPrecio", pListaPrecio.IdListaPrecio);
            //CONTROL DE ULTIMA LISTA AGREGADA
            //if (pListaPrecio.UltimaAgregada)
            //{
            //    this.MisParametrosUrl.Add("UltimaAgregada", pListaPrecio.UltimaAgregada);
            //}

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ListasPreciosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ListasPreciosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ListasPreciosModificar.aspx"), true);
            }
            //else if (e.CommandName == Gestion.Impresion.ToString())
            //{
            //    this.ctrPopUpComprobantes.CargarReporte(pSolicitud, EnumTGEComprobantes.CapOrdenesPagos);
            //    this.UpdatePanel1.Update();
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("ListasPreciosConsultar.aspx");

                CMPListasPrecios listaPrecio = (CMPListasPrecios)e.Row.DataItem;

                switch (listaPrecio.Estado.IdEstado)
                {
                    case (int)EstadosListasPrecios.Activo:
                        if(this.ValidarFecha(listaPrecio))
                            ibtnModificar.Visible = this.ValidarPermiso("ListasPreciosModificar.aspx");
                        ibtnAnular.Visible = this.ValidarPermiso("ListasPreciosAnular.aspx");
                        //ibtnAutorizar.Visible = this.ValidarPermiso("ListasPreciosAutorizar.aspx");
                        break;
                    
                    default:
                        break;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisListasPrecios.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        private bool ValidarFecha(CMPListasPrecios listaPrecio)
        {
            if (listaPrecio.FechaFinVigencia > DateTime.Now )
            {
                //CONTROL DE ULTIMA LISTA AGREGADA
                //if (listaPrecio.IdListaPrecio == this.MiUltimaAgregada.IdListaPrecio)
                //    listaPrecio.UltimaAgregada = true;
                return true;
            }
            else
                return false;
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CMPListasPrecios parametros = this.BusquedaParametrosObtenerValor<CMPListasPrecios>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CMPListasPrecios>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisListasPrecios;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisListasPrecios = this.OrdenarGrillaDatos<CMPListasPrecios>(this.MisListasPrecios, e);
            this.gvDatos.DataSource = this.MisListasPrecios;
            this.gvDatos.DataBind();
        }

        #endregion
    }
}