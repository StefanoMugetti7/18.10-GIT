using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using Comunes.Entidades;
using System.Collections;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Bancos;
using Reportes.FachadaNegocio;
using Facturas.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class CobranzasExternasConciliacionesListar : PaginaSegura
    {

        private List<TESCobranzasExternasConciliaciones> MisCobranzas
        {
            get { return (List<TESCobranzasExternasConciliaciones>)Session[this.MiSessionPagina + "CobranzasListarMisCobranzas"]; }
            set { Session[this.MiSessionPagina + "CobranzasListarMisCobranzas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("BancosCuentasAgregar.aspx");
                this.CargarCombos();
                TESCobranzasExternasConciliaciones parametros = this.BusquedaParametrosObtenerValor<TESCobranzasExternasConciliaciones>();
                if (parametros.BusquedaParametros)
                {
                    
                    this.ddlFormaCobro.SelectedValue = parametros.FormaCobro.IdFormaCobro.ToString();
                    this.ddlRefFormaCobro.SelectedValue = parametros.IdRefFormaCobro.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        private void CargarCombos()
        {
            this.ddlFormaCobro.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
            this.ddlFormaCobro.DataValueField = "IdFormaCobro";
            this.ddlFormaCobro.DataTextField = "FormaCobro";
            this.ddlFormaCobro.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)EstadosTodos.Todos).ToString(); //ESTADOS TODOS 

            //ddl REf Formas Cobros
            AyudaProgramacion.AgregarItemSeleccione(this.ddlRefFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlFormaCobro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlFormaCobro.SelectedValue))
                return;

            if (Convert.ToInt32(ddlFormaCobro.SelectedValue) == (int)EnumTGEFormasCobros.TarjetaCredito)
            {
                //CARGAR COMBO TARJETAS CREDITO
                this.ddlRefFormaCobro.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TarjetasCredito);
                this.ddlRefFormaCobro.DataValueField = "IdListaValorSistemaDetalle";
                this.ddlRefFormaCobro.DataTextField = "Descripcion";
                this.ddlRefFormaCobro.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlRefFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.ddlRefFormaCobro.Enabled = true;
                this.btnBuscar.Visible = true;
                this.upRefFormaCobro.Update();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/CobranzasExternasConciliacionesAgregar.aspx"), true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TESCobranzasExternasConciliaciones parametros = this.BusquedaParametrosObtenerValor<TESCobranzasExternasConciliaciones>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == "Movimientos"
                || e.CommandName == "Conciliar"
                || e.CommandName == Gestion.Impresion.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCobranzasExternasConciliaciones pCobranza = this.MisCobranzas[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCobranzaExternaConciliacion", pCobranza.IdCobranzaExternaConciliacion);

            //if (e.CommandName == Gestion.Modificar.ToString())
            //{
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasModificar.aspx"), true);
            //}
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/CobranzasExternasConciliacionesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, "CobranzasExternas", pCobranza, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this, "CobranzasExternas", this.UsuarioActivo);

            }
            else if (e.CommandName == Gestion.Anular.ToString() & pCobranza.Estado.IdEstado == (int)Estados.Activo)
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/CobranzasExternasConciliacionesAnular.aspx"), true);
            }
            //else if (e.CommandName == "Movimientos")
            //{
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosListar.aspx"), true);
            //}
            //else if (e.CommandName == "Conciliar")
            //{
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosConciliar.aspx"), true);
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TESCobranzasExternasConciliaciones pCobranza = (TESCobranzasExternasConciliaciones)e.Row.DataItem;
                //ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                consultar.Visible = this.ValidarPermiso("CobranzasExternasConciliacionesConsultar.aspx");

                //modificar.Visible = this.ValidarPermiso("BancosCuentasModificar.aspx");
                if(pCobranza.Estado.IdEstado == (int)Estados.Activo)
                {
                    ImageButton baja = (ImageButton)e.Row.FindControl("btnAnular");
                    baja.Visible = this.ValidarPermiso("CobranzasExternasConciliacionesAnular.aspx");
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCobranzas.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESCobranzasExternasConciliaciones parametros = this.BusquedaParametrosObtenerValor<TESCobranzasExternasConciliaciones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESCobranzasExternasConciliaciones>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCobranzas;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCobranzas = this.OrdenarGrillaDatos<TESCobranzasExternasConciliaciones>(this.MisCobranzas, e);
            this.gvDatos.DataSource = this.MisCobranzas;
            this.gvDatos.DataBind();
        }

        private void CargarLista(TESCobranzasExternasConciliaciones pParametro)
        {
            pParametro.FormaCobro.IdFormaCobro = this.ddlFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            pParametro.IdRefFormaCobro = this.ddlRefFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlRefFormaCobro.SelectedValue);
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TESCobranzasExternasConciliaciones>(pParametro);
            this.MisCobranzas = BancosF.CobranzasExternasConciliacionesObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisCobranzas;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}