using Afiliados;
using Afiliados.Entidades;
using Cargos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Nichos;
using Nichos.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Nichos
{
    public partial class NichosListarGeneral : PaginaSegura
    {
        private DataTable CardsBootStrapDataTable
        {
            get { return (DataTable)Session[this.MiSessionPagina + "SociosListarNichosGenerales"]; }
            set { Session[this.MiSessionPagina + "SociosListarNichosGenerales"] = value; }
        }
        private DataTable MisNichosAfiliadosGeneral
        {
            get { return (DataTable)Session[this.MiSessionPagina + "NichosListarGeneralMisNichosAfiliadosGeneral"]; }
            set { Session[this.MiSessionPagina + "NichosListarGeneralMisNichosAfiliadosGeneral"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtSocio, this.btnBuscar);
                this.CardsBootStrap = new StringBuilder();
                this.CargarCardsBootStrap();
                this.MenuCards = this.CardsBootStrap == null ? string.Empty : this.CardsBootStrap.ToString();
                this.ltrCards.Text = this.MenuCards;
                CargarCombos();

                NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();
                if (parametros.BusquedaParametros)
                {
                    this.CargarLista(parametros);
                }             
            }
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }
        private string MenuCards
        {
            get
            {
                if (Session["MaestraMenuHtml"] != null)
                { return (string)Session["MaestraMenuHtml"]; }
                else
                { return string.Empty; }
            }
            set { Session["MaestraMenuHtml"] = value; }
        }
        private StringBuilder CardsBootStrap;
        private void CargarCardsBootStrap()
        {
            string accion = string.Empty;
            string appPath = string.Empty;
            string target = string.Empty;

            NCHNichos filtro = new NCHNichos();
            DataTable cards = NichosF.NichosCargarCardsBootStrap(filtro);

            if (cards.Rows.Count > 0)
            {
                foreach (DataRow fila in cards.Rows)
                {
                    this.CardsBootStrap.AppendFormat("     <a href=\"#\" class=\"link-prestamos\" onclick=\"javascript:EjecutarFiltro({0},{1});\">", fila["IdCementerio"], fila["IdEstado"]);
                    this.CardsBootStrap.AppendLine(" <div class=\"card \" >");
                    this.CardsBootStrap.AppendFormat(" <div class=\"card-body {0} \">", fila["Color"]);
                    this.CardsBootStrap.AppendFormat(" <h5 class=\"card-title text-left card-prestamosh5\" >{0}</h5>", fila["Cantidad"]);
                    this.CardsBootStrap.AppendFormat("<p class=\"card-prestamosp card-text text-left\">{0} &nbsp;{1} </p>", fila["CementerioDescripcion"], fila["EstadoDescripcion"]);
                    this.CardsBootStrap.AppendLine("</div>");
                    this.CardsBootStrap.AppendLine("</div>");
                    this.CardsBootStrap.AppendLine("</a>");
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());


            CarTiposCargosAfiliadosFormasCobros cargoAfiliado = new CarTiposCargosAfiliadosFormasCobros();
            cargoAfiliado.IdTipoCargoAfiliadoFormaCobro = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoCargoAfiliadoFormaCobro"].ToString());
            this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(cargoAfiliado);


            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", cargoAfiliado.IdTipoCargoAfiliadoFormaCobro);

            AfiAfiliados afiliado = new AfiAfiliados();
            afiliado.IdAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());
            afiliado = AfiliadosF.AfiliadosObtenerDatos(afiliado);
            PaginaAfiliados paginaAfi = new PaginaAfiliados();
            paginaAfi.Guardar(this.MiSessionPagina, afiliado);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();               
                parametrosFiltros.BusquedaParametros = true;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
                parametrosFiltros.BusquedaParametros = true;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {    
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CarCargosAfiliadosFormasCobros, "CarTiposCargosAfiliadosFormasCobros", cargoAfiliado, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, UpdatePanel1, "CarTiposCargosAfiliadosFormasCobros", this.UsuarioActivo);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            NCHNichos nicho = BusquedaParametrosObtenerValor<NCHNichos>();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;

                if (!string.IsNullOrEmpty(dr.Row["NumeroSocio"].ToString()))
                {
                    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                    ibtnConsultar.Visible = this.ValidarPermiso("NichosConsultar.aspx");
                    ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                    ibtnModificar.Visible = this.ValidarPermiso("NichosModificar.aspx");
                    ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                    ibtnImprimir.Visible = true;
                }              
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisNichosAfiliadosGeneral.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisNichosAfiliadosGeneral = this.OrdenarGrillaDatos<DataTable>(this.MisNichosAfiliadosGeneral, e);
            this.gvDatos.DataSource = this.MisNichosAfiliadosGeneral;
            this.gvDatos.DataBind();
        }
        private void CargarCombos()
        {
            this.ddlCementerio.DataSource = CementeriosF.CementeriosObtenerListaActiva(new NCHCementerios());
            this.ddlCementerio.DataValueField = "IdCementerio";
            this.ddlCementerio.DataTextField = "Descripcion";
            this.ddlCementerio.DataBind();
            if (ddlCementerio.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCementerio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
       }

        private void CargarLista(NCHNichos pParametro)
        {
            pParametro.Filtro = this.txtSocio.Text.Trim();
            pParametro.Panteon.Cementerio.IdCementerio = ddlCementerio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlCementerio.SelectedValue);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = UsuarioActivo.PageSize > 0 ? UsuarioActivo.PageSize : 25;
            gvDatos.PageSize = UsuarioActivo.PageSize > 0 ? UsuarioActivo.PageSize : 25;
            
            this.BusquedaParametrosGuardarValor<NCHNichos>(pParametro);
            this.MisNichosAfiliadosGeneral = NichosF.NichosObtenerAfiliados(pParametro); 
            this.gvDatos.DataSource = this.MisNichosAfiliadosGeneral;
            this.gvDatos.VirtualItemCount = MisNichosAfiliadosGeneral.Rows.Count > 0 ? Convert.ToInt32(MisNichosAfiliadosGeneral.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
        }
    }
}