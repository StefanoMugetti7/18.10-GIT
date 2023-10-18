using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Prestamos.Entidades;
using Comunes.Entidades;
using System.Collections;
using Generales.FachadaNegocio;
using Prestamos;

namespace IU.Modulos.Prestamos
{
    public partial class PrePrestamosIpsCadAutorizacionesListar : PaginaAfiliados
    {
        private DataTable MisDatosGrilla
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisDatosGrillaPrePrestamosIpsCadAutorizaciones"]; }
            set { Session[this.MiSessionPagina + "MisDatosGrillaPrePrestamosIpsCadAutorizaciones"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                PrePrestamosIpsCadAutorizaciones parametros = this.BusquedaParametrosObtenerValor<PrePrestamosIpsCadAutorizaciones>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumero.Text = parametros.Numero.ToString();
                    this.ddlPeriodo.SelectedValue = parametros.Periodo == 0 ? string.Empty : parametros.Periodo.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PrePrestamosIpsCadAutorizaciones parametros = this.BusquedaParametrosObtenerValor<PrePrestamosIpsCadAutorizaciones>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idPrestamoIpsCadAutorizacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdPrestamoIpsCadAutorizacion")).Value);
            long numeroCAD = Convert.ToInt64(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfNumeroCAD")).Value);
            string SelloTiempo = ((HiddenField)this.gvDatos.Rows[index].FindControl("hdfSelloTiempo")).Value;
            //string parametros = string.Format("?IdPlazo={0}", plan.IdPlazos);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPrestamoIpsCadAutorizacion", idPrestamoIpsCadAutorizacion);
            this.MisParametrosUrl.Add("IpsCADNumero", numeroCAD);
            this.MisParametrosUrl.Add("IpsCADSelloTiempo", SelloTiempo);
            
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAgregar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                consultar.Visible = this.ValidarPermiso("PrestamosAfiliadosAgregar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (Convert.ToInt32(dr["IdEstado"]) != (int)EstadosIPSCAD.Activo)
                    consultar.Visible = false;

                HiddenField hdfSelloTiempo = (HiddenField)e.Row.FindControl("hdfSelloTiempo");
                hdfSelloTiempo.Value = Convert.ToBase64String((byte[])dr["SelloTiempo"]);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrePrestamosPlanes parametros = this.BusquedaParametrosObtenerValor<PrePrestamosPlanes>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<PrePrestamosPlanes>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisDatosGrilla;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisDatosGrilla = this.OrdenarGrillaDatos<PrePrestamosPlanes>(this.MisDatosGrilla, e);
            this.gvDatos.DataSource = this.MisDatosGrilla;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlPeriodo.DataSource = PrePrestamosF.PrePrestamosIpsCadAutorizacionesObtenerPeriodos();
            this.ddlPeriodo.DataValueField = "Periodo";
            this.ddlPeriodo.DataTextField = "Periodo";
            this.ddlPeriodo.DataBind();
            this.ddlPeriodo.Items.Add(new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), ""));
            this.ddlPeriodo.SelectedValue = "";

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), ""));
            this.ddlEstado.SelectedValue = "";
        }

        private void CargarLista(PrePrestamosIpsCadAutorizaciones pPrestamosIPS)
        {
            pPrestamosIPS.Estado.IdEstado = this.ddlEstado.SelectedValue==string.Empty ? (int)EstadosTodos.Todos : Convert.ToInt32(this.ddlEstado.SelectedValue);
            pPrestamosIPS.Periodo = this.ddlPeriodo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
            pPrestamosIPS.Numero = Convert.ToInt64(this.txtNumero.Decimal);
            pPrestamosIPS.IdAfiliado = this.MiAfiliado.IdAfiliado;
            pPrestamosIPS.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<PrePrestamosIpsCadAutorizaciones>(pPrestamosIPS);
            this.MisDatosGrilla = PrePrestamosF.PrePrestamosIpsCadAutorizacionesSeleccionarGrilla(pPrestamosIPS);
            this.gvDatos.DataSource = this.MisDatosGrilla;
            this.gvDatos.PageIndex = pPrestamosIPS.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}