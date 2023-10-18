using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Facturas;
using SKP.ASP.Controls;

namespace IU.Modulos.Facturas.Controles
{
    public partial class RemitosBuscarPopUp : ControlesSeguros 
    {

        //solo se usa para devolver los detalles dentro de un remito(vacio)
        private VTARemitos MiRemito
        {
            get { return (VTARemitos)Session[this.MiSessionPagina + "RemitosBuscarMiRemito"]; }
            set { Session[this.MiSessionPagina + "RemitosBuscarMiRemito"] = value; }
        }

        private int MiIndice
        {
            get { return (int)Session[this.MiSessionPagina + "RemitosListarMiIndice"]; }
            set { Session[this.MiSessionPagina + "RemitosListarMiIndice"] = value; }
        }

        private List<VTARemitosDetalles> MisDetalles //dato a grillar
        {
            get { return (List<VTARemitosDetalles>)Session[this.MiSessionPagina + "RemitosListarMisDetalles"]; }
            set { Session[this.MiSessionPagina + "RemitosListarMisDetalles"] = value; }
        }

        public delegate void RemitosBuscarEventHandler(VTARemitos e);
        public event RemitosBuscarEventHandler RemitosBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                this.MiIndice = 0;
                //this.CargarCombo();
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoRemito, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroRemitoPrefijo, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroRemitoSuFijo, this.btnBuscar);
            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }

        //public void IniciarControl()
        //{
        //    //this.CargarCombo();
        //    this.txtCodigoRemito.Text = string.Empty;
        //    this.txtNumeroRemitoPrefijo.Text = string.Empty;
        //    this.txtNumeroRemitoSuFijo.Text = string.Empty;
        //   // this.txtNumeroSocio.Text = string.Empty;
        //    this.mpePopUp.Show();
        //}

        public void IniciarControl(VTARemitos pRemito)
        {
            //this.txtNumeroSocio.Text = pRemito.Afiliado.IdAfiliado.ToString();
            //this.txtNumeroSocio.Enabled = false;
            this.MiRemito = new VTARemitos();
            pRemito.NumeroRemitoPrefijo = string.Empty; //para que ande el SP, de lo contrario los parametros pasan como NULL
            pRemito.NumeroRemitoSuFijo = string.Empty;
            //USO MiRemito para guardar el IdAfiliado / y al Aceptar se usa para devolver un remito vacio con todos los detalles seleccionados
            this.MiRemito.Afiliado.IdAfiliado = pRemito.Afiliado.IdAfiliado;
            this.MiRemito.Moneda.IdMoneda = pRemito.Moneda.IdMoneda;
            CargarLista(this.MiRemito);
            //this.MisDetalles = FacturasF.RemitosObtenerListaFiltroPopUp(pRemito);
            //AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MisDetalles, false, this.gvDatos, false);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarRemito();", true);
        } 

        /*private void CargarCombo()
        {
 	        this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosRemitos));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }*/

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            VTARemitos parametros = this.BusquedaParametrosObtenerValor<VTARemitos>();
            CargarLista(parametros);
        }

        #region Grilla

        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                    this.MisDetalles[fila.DataItemIndex].Incluir = chkIncluir.Checked;
                }
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (!(e.CommandName == Gestion.Consultar.ToString()))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //VTARemitos remito= this.MisRemitos[indiceColeccion];
            //remito = FacturasF.RemitosObtenerDatosCompletos(remito);

            //if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    if (this.RemitosBuscarSeleccionar != null)
            //    {
            //        this.RemitosBuscarSeleccionar(remito);
            //        this.mpePopUp.Hide();
            //    }
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                CheckBox incluir = (CheckBox)e.Row.FindControl("chkIncluir");
              
                VTARemitosDetalles remDet = (VTARemitosDetalles)e.Row.DataItem;
                
                //precio.Enabled = false;
                //if (Convert.ToInt32(precio.Decimal) == 0)
                //{
                //    incluir.Enabled = false;
                //}
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDetalles.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

         protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //VTARemitos parametros = this.BusquedaParametrosObtenerValor<VTARemitos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<VTARemitos>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisDetalles;
            gvDatos.DataBind();

        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisDetalles = this.OrdenarGrillaDatos<VTARemitosDetalles>(this.MisDetalles, e);
            this.gvDatos.DataSource = this.MisDetalles;
            this.gvDatos.DataBind();
        }
        
        #endregion

        private void CargarLista(VTARemitos pRemito)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pRemito.Afiliado.IdAfiliado = this.MiRemito.Afiliado.IdAfiliado;
            pRemito.IdRemito = this.txtCodigoRemito.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoRemito.Text);
            pRemito.NumeroRemitoSuFijo = this.txtNumeroRemitoSuFijo == null ? string.Empty : this.txtNumeroRemitoSuFijo.Text;
            pRemito.NumeroRemitoPrefijo = this.txtNumeroRemitoPrefijo == null ? string.Empty : this.txtNumeroRemitoPrefijo.Text;
            pRemito.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //pRemito.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            
            pRemito.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pRemito.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pRemito.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            pRemito.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<VTARemitos>(pRemito);
            this.MisDetalles = FacturasF.RemitosObtenerListaFiltroPopUp(pRemito);
            this.gvDatos.DataSource = this.MisDetalles;
            this.gvDatos.PageIndex = pRemito.IndiceColeccion;
            this.gvDatos.DataBind();

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MapearChecked();
            if(this.MiRemito.RemitosDetalles.Count > 0)
            {
                if (this.RemitosBuscarSeleccionar != null)
                {
                    this.RemitosBuscarSeleccionar(this.MiRemito);
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarRemitoto();", true);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarRemitoto();", true);
            }

        }

        private void MapearChecked()
        {
            this.MiRemito = new VTARemitos();
            this.MiRemito.RemitosDetalles.AddRange(this.MisDetalles.Where(x => x.Incluir).ToList());
            this.MiRemito.RemitosDetalles = AyudaProgramacion.AcomodarIndices<VTARemitosDetalles>(this.MiRemito.RemitosDetalles);
            //VTARemitosDetalles detalle = new VTARemitosDetalles();
            //CheckBox incluir;
            //this.MiRemito = new VTARemitos();
            //foreach (GridViewRow fila in this.gvDatos.Rows)
            //{

            //    if (fila.RowType == DataControlRowType.DataRow)
            //    {
            //        detalle = this.MisDetalles[fila.DataItemIndex];
            //        incluir = (CheckBox)fila.FindControl("chkIncluir");

            //        if (incluir.Checked)
            //        {
            //            this.MiRemito.RemitosDetalles.Add(detalle);
            //            detalle.IndiceColeccion = this.MiRemito.RemitosDetalles.IndexOf(detalle);
            //        }
            //    }
            //}
        }
    }
}