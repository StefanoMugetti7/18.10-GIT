using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Haberes;
using Haberes.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Haberes
{
    public partial class HabSueldosAportesRangosListar : PaginaSegura
    {
        private DataTable MisSueldosAportesRangos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisSueldosAportesRangosListar"]; }
            set { Session[this.MiSessionPagina + "MisSueldosAportesRangosListar"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                CargarCombos();



                this.btnAgregar.Visible = this.ValidarPermiso("HabSueldosAportesRangosAgregar.aspx");

                HabSueldosAportesRangos parametros = this.BusquedaParametrosObtenerValor<HabSueldosAportesRangos>();

                if (parametros.BusquedaParametros)
                {
                    //this.ddlCondicionFiscal.SelectedValue = parametros.IdCondicionFiscal.ToString();
                    //this.ddlTipoFactura.SelectedValue = parametros.IdTipoFactura.ToString();

                    this.CargarLista(parametros);
                }
                else
                    CargarLista(parametros);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/HabSueldosAportesRangosAgregar.aspx"), true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            HabSueldosAportesRangos parametros = this.BusquedaParametrosObtenerValor<HabSueldosAportesRangos>();
            this.CargarLista(parametros);
        }


        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("Todos"));
        }

        private void CargarLista(HabSueldosAportesRangos parametros)
        {
            parametros.BusquedaParametros = true;
            parametros.AniosMaximos = txtAnioMaximo.Text == string.Empty ? 0 : Convert.ToInt32(txtAnioMaximo.Text);
            parametros.AniosMinimos = txtAnioMinimo.Text == string.Empty ? 0 : Convert.ToInt32(txtAnioMinimo.Text);
            parametros.FechaIngresoDesde =  this.txtIngresoDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtIngresoDesde.Text);
            parametros.FechaIngresoHasta =  this.txtIngresoHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtIngresoHasta.Text);
            parametros.PorcentajeAporteMinimo = txtPorcentajeMinimo.Text == string.Empty ? 0 : Convert.ToDecimal(txtPorcentajeMinimo.Text);
            parametros.PorcentajeAporteMaximo = txtPorcentajeMaximo.Text == string.Empty ? 0 : Convert.ToDecimal(txtPorcentajeMaximo.Text);
            parametros.Estado.IdEstado = ddlEstados.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlEstados.SelectedValue);
            this.MisSueldosAportesRangos = HaberesF.AportesRangosObtenerDatosDataTable(parametros);
            this.gvDatos.DataSource = this.MisSueldosAportesRangos;
            this.gvDatos.DataBind();
            this.UpdatePanel1.Update();

            this.BusquedaParametrosGuardarValor<HabSueldosAportesRangos>(parametros);
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Modificar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdSueldoAporteRango = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdSueldoAporteRango"].ToString());

            switch (e.CommandName)
            {
                case "Modificar":
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("IdSueldoAporteRango", IdSueldoAporteRango);
       
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/HabSueldosAportesRangosModificar.aspx"), true);
                    break;
                default:
                    break;
            }
        }


        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = Convert.ToInt32(e.Row.RowIndex);
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnAnular.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                object suma;
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                suma = this.MisSueldosAportesRangos.Rows.Count;
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), suma);

            }
        }
        #endregion
    }
}