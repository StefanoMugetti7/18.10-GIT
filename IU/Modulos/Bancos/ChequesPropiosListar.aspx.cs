using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using System.Collections;
using Comunes.Entidades;
using Generales.Entidades;
using CuentasPagar.Entidades;
using Generales.FachadaNegocio;
using Bancos;

namespace IU.Modulos.Bancos
{
    public partial class ChequesPropiosListar : PaginaSegura
    {
        private List<TESCheques> MisCheques
        {
            get { return (List<TESCheques>)Session[this.MiSessionPagina + "ChequesPropiosListarMisCheques"]; }
            set { Session[this.MiSessionPagina + "ChequesPropiosListarMisCheques"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.txtFechaDesde.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                this.txtFechaHasta.Text = DateTime.Now.ToShortDateString();

                this.CargarCombos();
                TESCheques parametros = this.BusquedaParametrosObtenerValor<TESCheques>();
                if (parametros.BusquedaParametros)
                {
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.txtFechaDiferidoDesde.Text = parametros.FechaDiferidoDesde.HasValue ? parametros.FechaDiferidoDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaDiferidoHasta.Text = parametros.FechaDiferidoHasta.HasValue ? parametros.FechaDiferidoHasta.Value.ToShortDateString() : string.Empty;
                    this.txtNumeroCheque.Text = parametros.NumeroCheque;
                    this.ddlBancos.SelectedValue = parametros.Banco.IdBanco.ToString();
                    this.ddlFiliales.SelectedValue = parametros.Filial.IdFilial.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtTitularCheque.Text = parametros.TitularCheque;
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TESCheques parametros = this.BusquedaParametrosObtenerValor<TESCheques>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCheques cheque = this.MisCheques[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCheque", cheque.IdCheque);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                if (cheque.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagos)
                {
                    CapOrdenesPagos pReporte = new CapOrdenesPagos();
                    pReporte.IdOrdenPago = cheque.IdRefTipoOperacion;
                    this.ctrPopUpComprobantes.CargarReporte(pReporte, EnumTGEComprobantes.CapOrdenesPagos);
                    this.UpdatePanel1.Update();
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TESCheques bancoCuenta = (TESCheques)e.Row.DataItem;

                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                consultar.Visible = this.ValidarPermiso("ChequesConsultar.aspx");
                ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                imprimir.Visible = false;

                switch (bancoCuenta.Estado.IdEstado)
                {
                    case (int)EstadosCheques.EnCaja:
                    case (int)EstadosCheques.EnTesoreria:
                        imprimir.Visible = true;
                        break;
                    case (int)EstadosCheques.EnviadoSectorBancos:
                    case (int)EstadosCheques.EnSectorBancos:
                        imprimir.Visible = true;
                        modificar.Visible = this.ValidarPermiso("ChequesModificar.aspx");
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label grillaTotal = (Label)e.Row.FindControl("lblGrillaTotalRegistros");
                grillaTotal.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCheques.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESCheques parametros = this.BusquedaParametrosObtenerValor<TESCheques>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESCheques>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCheques;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCheques = this.OrdenarGrillaDatos<TESCheques>(this.MisCheques, e);
            this.gvDatos.DataSource = this.MisCheques;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosCheques));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";

            this.ddlBancos.DataSource = BancosF.BancosObtenerListaFiltroConCuentas(new TESBancos());//TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.Bancos);
            this.ddlBancos.DataValueField = "IdListaValorDetalle";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        private void CargarLista(TESCheques pParametro)
        {
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.FechaDiferidoDesde = this.txtFechaDiferidoDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDiferidoDesde.Text);
            pParametro.FechaDiferidoHasta = this.txtFechaDiferidoHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDiferidoHasta.Text);
            pParametro.NumeroCheque = this.txtNumeroCheque.Text;
            pParametro.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
            pParametro.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.TitularCheque = this.txtTitularCheque.Text;
            pParametro.ChequePropio = true;
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TESCheques>(pParametro);
            this.MisCheques = BancosF.ChequesObtenerListaFiltroPropios(pParametro);
            this.gvDatos.DataSource = this.MisCheques;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
