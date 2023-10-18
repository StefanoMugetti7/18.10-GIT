using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProcesosDatos.Entidades;

namespace IU.Modulos.ProcesosDatos.Controles
{
    public partial class GridViewCheckDinamico : ControlesSeguros
    {
        private string MiParametro
        {
            get { return (string)Session[this.MiSessionPagina + "GridViewCheckDinamicoMiParametro"]; }
            set { Session[this.MiSessionPagina + "GridViewCheckDinamicoMiParametro"] = value; }
        }

        private string MiNombreParametro
        {
            get { return (string)Session[this.MiSessionPagina + "GridViewCheckDinamicoMiParametroMiNombreParametro"]; }
            set { Session[this.MiSessionPagina + "GridViewCheckDinamicoMiParametroMiNombreParametro"] = value; }
        }

        private DataTable MisDatosGrilla
        {
            get { return (DataTable)Session[this.MiSessionPagina + "GridViewCheckDinamicoMiParametroMisDatosGrilla"]; }
            set { Session[this.MiSessionPagina + "GridViewCheckDinamicoMiParametroMisDatosGrilla"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(DataSet ds, string Parametro, string NombreParametro)
        {
            if (!string.IsNullOrEmpty(Parametro)) this.MiParametro = Parametro;
            if (!string.IsNullOrEmpty(NombreParametro)) this.MiNombreParametro = NombreParametro;
            //btnExportarExcel.Visible = false;
            
            if (ds.Tables.Count > 0)
            {
                MisDatosGrilla = ds.Tables[0];
                this.gvDatos.DataSource = ds.Tables[0];
                this.gvDatos.DataBind();
                AyudaProgramacion.FixGridView(gvDatos);
                this.gvDatos.Visible = ds.Tables[0].Rows.Count > 0;
                btnExportarExcel.Visible = ds.Tables[0].Rows.Count > 0;
            }
        }

        protected void gvDatos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            // Intitialize TableCell list
            List<TableCell> columns = new List<TableCell>();
            foreach (DataControlField column in gvDatos.Columns)
            {
                //Get the first Cell /Column
                TableCell cell = row.Cells[0];
                // Then Remove it after
                row.Cells.Remove(cell);
                //And Add it to the List Collections
                columns.Add(cell);
            }
            // Add cells
            row.Cells.AddRange(columns.ToArray());
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView fila = (DataRowView)e.Row.DataItem;
                HiddenField hdfIdValor = (HiddenField)e.Row.FindControl("hdfIdValor");
                hdfIdValor.Value = fila[this.MiParametro].ToString();
            }
        }

        public bool ObtenerValores(SisParametros pParmetro)
        {
            string XML = "<Valores>";
            //string XMLDescripcion = "";
            CheckBox chkIncluir;
            HiddenField hdfIdValor;
            //HiddenField hdfDescripcion;
            foreach (GridViewRow item in this.gvDatos.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    chkIncluir = (CheckBox)item.FindControl("chkIncluir");
                    if (chkIncluir.Checked)
                    {
                        hdfIdValor = (HiddenField)item.FindControl("hdfIdValor");
                        //hdfDescripcion = (HiddenField)item.FindControl("hdfDescripcion");
                        XML = string.Concat(XML, "<Valor><",this.MiParametro,">", hdfIdValor.Value, "</",this.MiParametro,"></Valor>");
                        //XMLDescripcion = string.Concat(XMLDescripcion, hdfDescripcion.Value);
                    }
                }
            }
            XML = string.Concat(XML, "</Valores>");
            pParmetro.ValorParametro = XML;
            //pParmetro.ValorParametroDescripcion = XMLDescripcion;
            return true;
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisDatosGrilla;
            this.gvDatos.DataBind();
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MisDatosGrilla);
        }
    }
}