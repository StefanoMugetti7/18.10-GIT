using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Tesorerias.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Tesorerias;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Bancos.Entidades;
using Bancos;

namespace IU.Modulos.Tesoreria
{
    public partial class BancosCuentasListar : PaginaSegura
    {
        private List<TESBancosCuentas> MisBancosCuentas
        {
            get { return (List<TESBancosCuentas>)Session[this.MiSessionPagina + "TESBancosCuentasMisBancosCuentas"]; }
            set { Session[this.MiSessionPagina + "TESBancosCuentasMisBancosCuentas"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDenominacion, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroCuenta, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("BancosCuentasAgregar.aspx");
                this.CargarCombos();
                TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroCuenta.Text = parametros.NumeroCuenta;
                    this.txtDenominacion.Text = parametros.Denominacion;
                    this.ddlBancos.SelectedValue = parametros.Banco.IdBanco.ToString();
                    this.ddlFiliales.SelectedValue = parametros.Filial.IdFilial.ToString();
                    this.ddlMonedas.SelectedValue = parametros.Moneda.IdMoneda.ToString();
                    this.ddlBancosCuentasTipos.SelectedValue = parametros.BancoCuentaTipo.IdBancoCuentaTipo.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasAgregar.aspx"), true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            this.CargarLista(parametros);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == "Movimientos"
                || e.CommandName == "Conciliar"
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESBancosCuentas bancoCuenta = this.MisBancosCuentas[indiceColeccion];

            if (this.Session[this.MiSessionPagina + "Modulos/Bancos/BancosCuentasMovimientosListar.aspx"] != null)
                this.Session[this.MiSessionPagina + "Modulos/Bancos/BancosCuentasMovimientosListar.aspx"] = null;

            this.MisParametrosUrl = new Hashtable
            {
                { "IdBancoCuenta", bancoCuenta.IdBancoCuenta }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasConsultar.aspx"), true);
            }
            else if (e.CommandName == "Movimientos")
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosListar.aspx"), true);
            }
            else if (e.CommandName == "Conciliar")
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosConciliar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TESBancosCuentas bancoCuenta = (TESBancosCuentas)e.Row.DataItem;

                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("BancosCuentasModificar.aspx");
                consultar.Visible = this.ValidarPermiso("BancosCuentasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisBancosCuentas.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESBancosCuentas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisBancosCuentas;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisBancosCuentas = this.OrdenarGrillaDatos<TESBancosCuentas>(this.MisBancosCuentas, e);
            this.gvDatos.DataSource = this.MisBancosCuentas;
            this.gvDatos.DataBind();
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";

            this.ddlMonedas.DataSource = TGEGeneralesF.MonedasObtenerLista();
            this.ddlMonedas.DataValueField = "IdMoneda";
            this.ddlMonedas.DataTextField = "miMonedaDescripcion";
            this.ddlMonedas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlMonedas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //this.ddlBancos.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.Bancos);
            //this.ddlBancos.DataValueField = "IdListaValorDetalle";
            this.ddlBancos.DataSource = BancosF.BancosObtenerListaFiltroConCuentas(new TESBancos());
            this.ddlBancos.DataValueField = "IdBanco";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            this.ddlBancosCuentasTipos.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.BancosCuentasTipos);
            this.ddlBancosCuentasTipos.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlBancosCuentasTipos.DataTextField = "Descripcion";
            this.ddlBancosCuentasTipos.DataBind();
            if (this.ddlBancosCuentasTipos.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentasTipos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(TESBancosCuentas pParametro)
        {
            pParametro.NumeroCuenta = this.txtNumeroCuenta.Text;
            pParametro.Denominacion = this.txtDenominacion.Text;
            pParametro.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
            pParametro.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.Moneda.IdMoneda = this.ddlMonedas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMonedas.SelectedValue);
            pParametro.BancoCuentaTipo.IdBancoCuentaTipo = this.ddlBancosCuentasTipos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlBancosCuentasTipos.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TESBancosCuentas>(pParametro);
            this.MisBancosCuentas = BancosF.BancosCuentasObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisBancosCuentas;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
