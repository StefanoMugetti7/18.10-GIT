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
    public partial class GridViewCheckFecha : ControlesSeguros
    {
        private string MiParametro
        {
            get { return (string)Session[this.MiSessionPagina + "GridViewCheckFechaMiParametro"]; }
            set { Session[this.MiSessionPagina + "GridViewCheckFechaMiParametro"] = value; }
        }

        private string MiNombreParametro
        {
            get { return (string)Session[this.MiSessionPagina + "GridViewCheckFechaMiNombreParametro"]; }
            set { Session[this.MiSessionPagina + "GridViewCheckFechaMiNombreParametro"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(DataSet ds, string Parametro, string NombreParametro)
        {
            if (!string.IsNullOrEmpty(Parametro)) this.MiParametro = Parametro;
            if (!string.IsNullOrEmpty(NombreParametro)) this.MiNombreParametro = NombreParametro;

            if (ds.Tables.Count > 0)
            {
                this.gvDatos.DataSource = ds.Tables[0];
                this.gvDatos.DataBind();
                this.gvDatos.Columns[0].HeaderText = this.MiNombreParametro;
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TextBox txtRepetirFecha = (TextBox)e.Row.FindControl("txtRepetirFecha");
                if (txtRepetirFecha.Attributes["onchange"] == null)
                    txtRepetirFecha.Attributes.Add("onchange", "return RepetirFecha();");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView fila = (DataRowView)e.Row.DataItem;
                HiddenField hdfIdValor = (HiddenField)e.Row.FindControl("hdfIdValor");
                hdfIdValor.Value = fila[this.MiParametro].ToString();
            }
        }

        public bool ObtenerValores(SisParametros pParmetro)
        {
            string XML = "<Valores>";
            string XMLDescripcion = "";
            CheckBox chkIncluir;
            TextBox txtFecha;
            HiddenField hdfIdValor;
            HiddenField hdfDescripcion;
            DateTime fecha;
            foreach (GridViewRow item in this.gvDatos.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    chkIncluir = (CheckBox)item.FindControl("chkIncluir");
                    if (chkIncluir.Checked)
                    {
                        hdfIdValor = (HiddenField)item.FindControl("hdfIdValor");
                        hdfDescripcion = (HiddenField)item.FindControl("hdfDescripcion");
                        txtFecha = (TextBox)item.FindControl("txtFecha");
                        if (!DateTime.TryParse(txtFecha.Text, out fecha))
                        {
                            this.MostrarMensaje("gvCheckFechaValidarFecha", true, new List<string>() { item.Cells[0].Text});
                            return false;
                        }
                        fecha = Convert.ToDateTime(txtFecha.Text);
                        XML = string.Concat(XML, "<Valor><IdValor>", hdfIdValor.Value, "</IdValor>",
                        "<Fecha>", fecha.Year.ToString(), fecha.Month.ToString().PadLeft(2,'0'), fecha.Day.ToString().PadLeft(2,'0'), "</Fecha>",
                        "</Valor>");

                        XMLDescripcion = string.Concat(XMLDescripcion, hdfDescripcion.Value, " - ", fecha.ToShortDateString());
                    }
                }
            }
            XML = string.Concat(XML, "</Valores>");
            pParmetro.ValorParametro = XML;
            pParmetro.ValorParametroDescripcion = XMLDescripcion;
            return true;
        }
    }
}