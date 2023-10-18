using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Compras;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class ImportarRemitoPopUp : ControlesSeguros
    {
        //solo se usa para devolver los detalles dentro de un remito(vacio)
        private CmpInformesRecepciones MiInforme
        {
            get { return (CmpInformesRecepciones)Session[this.MiSessionPagina + "RemitosBuscarMiInforme"]; }
            set { Session[this.MiSessionPagina + "RemitosBuscarMiInforme"] = value; }
        }

        private int MiIndice
        {
            get { return (int)Session[this.MiSessionPagina + "RemitosListarMiIndice"]; }
            set { Session[this.MiSessionPagina + "RemitosListarMiIndice"] = value; }
        }

        private List<CmpInformesRecepcionesDetalles> MisDetalles //dato a grillar
        {
            get { return (List<CmpInformesRecepcionesDetalles>)Session[this.MiSessionPagina + "RemitosListarMisDetalles"]; }
            set { Session[this.MiSessionPagina + "RemitosListarMisDetalles"] = value; }
        }

        public delegate void InformesBuscarEventHandler(CmpInformesRecepciones e);
        public event InformesBuscarEventHandler InformesBuscarSeleccionar;

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

        public void IniciarControl(CmpInformesRecepciones pInforme)
        {
            //this.txtNumeroSocio.Text = pRemito.Afiliado.IdAfiliado.ToString();
            //this.txtNumeroSocio.Enabled = false;
            this.MiInforme = new CmpInformesRecepciones();
            pInforme.NumeroRemitoPrefijo = string.Empty; //para que ande el SP, de lo contrario los parametros pasan como NULL
            pInforme.NumeroRemitoSufijo = string.Empty;
            //USO MiRemito para guardar el IdAfiliado / y al Aceptar se usa para devolver un remito vacio con todos los detalles seleccionados
            this.MiInforme.Proveedor.IdProveedor = pInforme.Proveedor.IdProveedor;
            this.MisDetalles = ComprasF.InformesRecepcionesObtenerDetallesPendientesFiltroPorProveedor(pInforme);
            this.MisDetalles = this.MisDetalles.Where(x=> !pInforme.InformesRecepcionesDetalles.Any(x2=>x2.IdInformeRecepcionDetalle==x.IdInformeRecepcionDetalle)).ToList();
            this.MisDetalles = AyudaProgramacion.AcomodarIndices<CmpInformesRecepcionesDetalles>(this.MisDetalles);
            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MisDetalles, false, this.gvDatos, false);
            //this.mpePopUp.Show();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalImportarRemito();", true);
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
            CmpInformesRecepciones parametros = this.BusquedaParametrosObtenerValor<CmpInformesRecepciones>();
            CargarLista(parametros);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalImportarRemito();", true);
            //this.mpePopUp.Show();
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
                //Evol.Controls.CurrencyTextBox precio = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtPrecio");
                //CheckBox incluir = (CheckBox)e.Row.FindControl("chkIncluir");
                //precio.Enabled = false;
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
                 ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalImportarRemito();", true);


            //this.mpePopUp.Show();

        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisDetalles = this.OrdenarGrillaDatos<CmpInformesRecepcionesDetalles>(this.MisDetalles, e);
            this.gvDatos.DataSource = this.MisDetalles;
            this.gvDatos.DataBind();
            //this.mpePopUp.Show();
        }
        #endregion

        private void CargarLista(CmpInformesRecepciones pInforme)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pInforme.Proveedor.IdProveedor= this.MiInforme.Proveedor.IdProveedor;
            pInforme.IdInformeRecepcion = this.txtCodigoRemito.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoRemito.Text);
            pInforme.NumeroRemitoSufijo = this.txtNumeroRemitoSuFijo == null ? string.Empty : this.txtNumeroRemitoSuFijo.Text;
            pInforme.NumeroRemitoPrefijo = this.txtNumeroRemitoPrefijo == null ? string.Empty : this.txtNumeroRemitoPrefijo.Text;
            //pRemito.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);

            pInforme.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pInforme.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);

            pInforme.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CmpInformesRecepciones>(pInforme);
            this.MisDetalles = ComprasF.InformesRecepcionesObtenerDetallesPendientesFiltroPorProveedor(pInforme);
            this.gvDatos.DataSource = this.MisDetalles;
            this.gvDatos.PageIndex = pInforme.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MapearChecked();
            if (this.MiInforme.InformesRecepcionesDetalles.Count > 0)
            {
                if (this.InformesBuscarSeleccionar != null)
                {
                    this.InformesBuscarSeleccionar(this.MiInforme);
                    //this.mpePopUp.Hide();
                }
            }
            else
            {
                //this.mpePopUp.Hide();
            }

        }

        private void MapearChecked()
        {
            CmpInformesRecepcionesDetalles detalle = new CmpInformesRecepcionesDetalles();
            //CheckBox incluir;
            this.MiInforme = new CmpInformesRecepciones();
            this.MiInforme.InformesRecepcionesDetalles.AddRange(this.MisDetalles.Where(x => x.Incluir).ToList());
            this.MiInforme.InformesRecepcionesDetalles = AyudaProgramacion.AcomodarIndices<CmpInformesRecepcionesDetalles>(this.MiInforme.InformesRecepcionesDetalles);
            //foreach (GridViewRow fila in this.gvDatos.Rows)
            //{

            //    if (fila.RowType == DataControlRowType.DataRow)
            //    {
            //        detalle = this.MisDetalles[fila.DataItemIndex];
            //        incluir = (CheckBox)fila.FindControl("chkIncluir");

            //        if (incluir.Checked)
            //        {
            //            this.MiInforme.InformesRecepcionesDetalles.Add(detalle);
            //            detalle.IndiceColeccion = this.MiInforme.InformesRecepcionesDetalles.IndexOf(detalle);
            //        }
            //    }
            //}
        }
    }
}