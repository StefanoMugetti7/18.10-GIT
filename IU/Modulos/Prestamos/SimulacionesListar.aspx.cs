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
using Prestamos.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Prestamos;

namespace IU.Modulos.Prestamos
{
    public partial class SimulacionesListar : PaginaAfiliados
    {
        private List<PrePrestamos> MisSimulacionesAfiliados
        {
            get { return (List<PrePrestamos>)Session[this.MiSessionPagina + "SimulacionListarMisSimulacionesAfiliados"]; }
            set { Session[this.MiSessionPagina + "SimulacionListarMisSimulacionesAfiliados"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("SimulacionesAgregar.aspx");
                this.CargarLista();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/SimulacionesAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;
            
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            PrePrestamos simulacion = this.MisSimulacionesAfiliados[indiceColeccion];
            //string parametros = string.Format("?IdPrestamo={0}", prestamoAfiliado.IdPrestamo);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdSimulacion", simulacion.IdSimulacion);

            if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/SimulacionesConsultar.aspx"), true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = this.ValidarPermiso("SimulacionesConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSimulacionesAfiliados.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrePrestamos parametros = this.BusquedaParametrosObtenerValor<PrePrestamos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<PrePrestamos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisSimulacionesAfiliados;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSimulacionesAfiliados = this.OrdenarGrillaDatos<PrePrestamos>(this.MisSimulacionesAfiliados, e);
            this.gvDatos.DataSource = this.MisSimulacionesAfiliados;
            this.gvDatos.DataBind();
        }

        private void CargarLista()
        {
            this.MisSimulacionesAfiliados = PrePrestamosF.SimulacionObtenerPorAfiliado(this.MiAfiliado);
            this.gvDatos.DataSource = this.MisSimulacionesAfiliados;
            this.gvDatos.DataBind();
        }
    }
}
