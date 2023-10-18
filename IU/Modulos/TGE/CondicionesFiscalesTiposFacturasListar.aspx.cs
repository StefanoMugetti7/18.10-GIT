using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace IU.Modulos.TGE
{
    public partial class CondicionesFiscalesTiposFacturasListar : PaginaSegura
    {
        private DataTable MisCondicionesFiscalesTiposFacturas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisCondicionesFiscalesTiposFacturasListar"]; }
            set { Session[this.MiSessionPagina + "MisCondicionesFiscalesTiposFacturasListar"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                CargarCombos();
                this.btnAgregar.Visible = this.ValidarPermiso("CondicionesFiscalesTiposFacturasAgregar.aspx");
                TGECondicionesFiscalesTiposFacturas parametros = this.BusquedaParametrosObtenerValor<TGECondicionesFiscalesTiposFacturas>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlCondicionFiscal.SelectedValue = parametros.IdCondicionFiscal.ToString();
                    this.ddlTipoFactura.SelectedValue = parametros.IdTipoFactura.ToString();
                    this.CargarLista(parametros);
                }
                else
                {
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CondicionesFiscalesTiposFacturasAgregar.aspx"), true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGECondicionesFiscalesTiposFacturas parametros = this.BusquedaParametrosObtenerValor<TGECondicionesFiscalesTiposFacturas>();
            this.CargarLista(parametros);
        }
        private void CargarCombos()
        {
            this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscal.DataTextField = "Descripcion";
            this.ddlCondicionFiscal.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoFactura.DataSource = TGEGeneralesF.TiposFacturasObtenerLista();
            this.ddlTipoFactura.DataValueField = "IdTipoFactura";
            this.ddlTipoFactura.DataTextField = "TipoFactura";
            this.ddlTipoFactura.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(TGECondicionesFiscalesTiposFacturas parametros)
        {
            parametros.BusquedaParametros = true;

            parametros.IdTipoFactura = this.ddlTipoFactura.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFactura.SelectedValue);
            parametros.IdCondicionFiscal = this.ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscal.SelectedValue);
            this.MisCondicionesFiscalesTiposFacturas = TGEGeneralesF.CondicionesFiscalesTiposFacturasDatosSeleccionarFilttro(parametros);
            this.gvDatos.DataSource = this.MisCondicionesFiscalesTiposFacturas;
            this.gvDatos.DataBind();
            this.UpdatePanel1.Update();
            this.BusquedaParametrosGuardarValor<TGECondicionesFiscalesTiposFacturas>(parametros);
        }
        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Anular"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdCondicionFiscal = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdCondicionFiscal"].ToString());
            int IdTipoFactura = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoFactura"].ToString());

            switch (e.CommandName)
            {
                case "Anular":
                    this.MisParametrosUrl = new Hashtable
                    {
                        { "IdCondicionFiscal", IdCondicionFiscal },
                        { "IdTipoFactura", IdTipoFactura }
                    };
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CondicionesFiscalesTiposFacturasEliminar.aspx"), true);
                    break;
                default:
                    break;
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ibtnAnular.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                object suma;
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                suma = this.MisCondicionesFiscalesTiposFacturas.Rows.Count;
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), suma);
            }
        }
        #endregion
    }
}