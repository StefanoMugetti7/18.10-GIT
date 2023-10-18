using Comunes.Entidades;
using ProcesosDatos.Entidades;
using ProcesosDatos;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Evol.Controls;

namespace IU.Modulos.ProcesosDatos.Controles
{
    public partial class GridViewCheckImporte : ControlesSeguros
    {
        private string MiParametro
        {
            get { return (string)Session[this.MiSessionPagina + "GridViewCheckImporteMiParametro"]; }
            set { Session[this.MiSessionPagina + "GridViewCheckImporteMiParametro"] = value; }
        }

        private string MiNombreParametro
        {
            get { return (string)Session[this.MiSessionPagina + "GridViewCheckImporteMiNombreParametro"]; }
            set { Session[this.MiSessionPagina + "GridViewCheckImporteMiNombreParametro"] = value; }
        }
        protected DataSet MiDatosLote
        {
            get { return (DataSet)Session[this.MiSessionPagina + "GridViewCheckImporteMiDatosLote"]; }
            set { Session[this.MiSessionPagina + "GridViewCheckImporteMiDatosLote"] = value; }
        }

        
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public void IniciarControl(DataSet ds, string Parametro, string NombreParametro)
        {
            if (!string.IsNullOrEmpty(Parametro)) this.MiParametro = Parametro;
            if (!string.IsNullOrEmpty(NombreParametro)) this.MiNombreParametro = NombreParametro;

            MiDatosLote = ds;

            if (ds.Tables.Count > 0)
            {
                this.gvDatosLote.DataSource = MiDatosLote.Tables[0];
                this.gvDatosLote.DataBind();
                AyudaProgramacion.FixGridView(gvDatosLote);
                this.gvDatosLote.Visible = ds.Tables[0].Rows.Count > 0;
            }
        }


        public bool ObtenerValores(SisParametros pParmetro)
        {
            string XML = "<Valores>";
            CheckBox chkIncluir;
            HiddenField hdfIdValor;
            foreach (GridViewRow item in this.gvDatosLote.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    chkIncluir = (CheckBox)item.FindControl("chkIncluir");
                    if (chkIncluir.Checked)
                    {
                        //hdfIdValor = (HiddenField)item.FindControl("hdfIdValor");
                        string idValor = gvDatosLote.DataKeys[item.RowIndex]["IdValor"].ToString();
                        string datoAdicional = gvDatosLote.DataKeys[item.RowIndex]["DatoAdicional"].ToString();
                        XML = string.Concat(XML, "<Valor><IdValor>", idValor, "</IdValor>");
                        XML = string.Concat(XML, "<DatoAdicional>", datoAdicional, "</DatoAdicional>");
                        GridView gv = (GridView)item.FindControl("gvDatosDetalles");
                        if (gv != null)
                        {
                            foreach (GridViewRow itemDetalle in gv.Rows)
                            {
                                XML = string.Concat(XML, "<Detalles>");
                                HiddenField cc = (HiddenField)itemDetalle.FindControl("hdfIdValorDetalle");
                                int idValorDetalle = Convert.ToInt32(cc.Value);
                                CurrencyTextBox txtImporte = (CurrencyTextBox)itemDetalle.FindControl("txtImporte");
                                XML = string.Concat(XML, "<IdValorDetalle>", idValorDetalle, "</IdValorDetalle>");
                                XML = string.Concat(XML, "<Importe>", txtImporte.Decimal.ToString().Replace(",","."), "</Importe>");
                                XML = string.Concat(XML, "</Detalles>");
                            }
                        }
                        XML = string.Concat(XML, "</Valor>");
                    }
                }
            }
            XML = string.Concat(XML, "</Valores>");
            pParmetro.ValorParametro = XML;
            return true;
        }

        protected void gvDatosLote_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (!(e.CommandName == Gestion.Consultar.ToString()
            //    //|| e.CommandName == Gestion.Listar.ToString()//lupa
            //    ))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int idValor = Convert.ToInt32(gvDatosLote.DataKeys[index]["IdValor"].ToString());
            //string datoAdicional = gvDatosLote.DataKeys[index]["DatoAdicional"].ToString();
            //HiddenField mostrar = (HiddenField)this.gvDatosLote.Rows[index].FindControl("hdfMostrarDetalle");
            ////HiddenField hdfIdAfiliado = (HiddenField)this.gvDatosLote.Rows[index].FindControl("hdfIdAfiliado");
            //Panel panel = (Panel)this.gvDatosLote.Rows[index].FindControl("pnlDatosDetalles");
            //ImageButton lupa = (ImageButton)this.gvDatosLote.Rows[index].FindControl("btnBuscarCargos");
            //ImageButton plus = (ImageButton)this.gvDatosLote.Rows[index].FindControl("btnConsultar");

            //this.PersistirGrilla();

            //if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    if (plus.ImageUrl.Contains("plus.png"))
            //    {
            //        plus.ImageUrl = "~/Imagenes/minus.png";
            //    }
            //    else
            //    {
            //        plus.ImageUrl = "~/Imagenes/plus.png";
            //    }
            //    if (GestionControl != Gestion.Consultar)
            //    {
            //        lupa.Visible = !lupa.Visible;
            //    }

            //    if (mostrar.Value == "1")
            //    {
            //        panel.Visible = true;
            //        mostrar.Value = "0";
            //    }
            //    else
            //    {
            //        panel.Visible = false;
            //        mostrar.Value = "1";
            //    }
            //}

            //GridView gv = ((GridView)this.gvDatosLote.Rows[index].FindControl("gvDatosDetalles"));
            //if (gv != null && MiDatosLote.Tables.Count > 0)
            //{
            //    gv.RowDataBound += gvDatosDetalles_RowDataBound;
            //    gv.DataSource = this.MiDatosLote.Tables[1].AsEnumerable().Where(x => x.Field<int>("IdValor") == idValor && x.Field<string>("DatoAdicional") == datoAdicional).AsDataView();
            //    gv.DataBind();
            //}
            //this.upDatos.Update();
        }

        protected void gvDatosLote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gv = ((GridView)e.Row.FindControl("gvDatosDetalles"));
                int idValor = Convert.ToInt32(gvDatosLote.DataKeys[e.Row.RowIndex]["IdValor"].ToString());
                string datoAdicional = gvDatosLote.DataKeys[e.Row.RowIndex]["DatoAdicional"].ToString();

                if (gv != null && MiDatosLote.Tables.Count > 0)
                {
                    gv.RowDataBound += gvDatosDetalles_RowDataBound;
                    gv.DataSource = this.MiDatosLote.Tables[1].AsEnumerable().Where(x => x.Field<int>("IdValor") == idValor && x.Field<string>("DatoAdicional") == datoAdicional).AsDataView();
                    gv.DataBind();
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiDatosLote.Tables[0].Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatosLote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //this.gvDatosLote.PageIndex = e.NewPageIndex;
            //parametros.PageIndex = e.NewPageIndex;
            //this.CargarLista(parametros);
        }
        private void GvDatosLotes_PageSizeEvent(int pageSize)
        {
            //CarTiposCargosLotesEnviadosDetalles parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosLotesEnviadosDetalles>();
            //parametros.PageIndex = 0;
            //this.gvDatosLote.PageIndex = 0;
            //this.UsuarioActivo.PageSize = pageSize;
            //this.CargarLista(parametros);
        }

        protected void gvDatosDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CurrencyTextBox control = ((CurrencyTextBox)e.Row.FindControl("txtImporte"));
                if (control != null)
                {
                    control.Attributes.Add("onchange", "ValidarTotal('" + control.ClientID + "');");
                    control.Enabled= true;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label control = ((Label)e.Row.FindControl("lblImporteTotal"));
                if (control != null)
                {
                    if (GestionControl == Gestion.Consultar)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "InitFooterDetalle", "InitFooterDetalle('" + control.ClientID + "');", true);
                    }
                }
            }
        }
    }
}