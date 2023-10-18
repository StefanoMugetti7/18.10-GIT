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
using Generales.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using System.Collections.Generic;

namespace IU.Modulos.TGE
{
    public partial class FormasCobrosAfiliadosListar : PaginaAfiliados
    {
        private List<TGEFormasCobrosAfiliados> MisFormasCobrosAfiliados
        {
            get { return (List<TGEFormasCobrosAfiliados>)Session[this.MiSessionPagina + "FormasCobrosAfiliadosListarMisFormasCobrosAfiliados"]; }
            set { Session[this.MiSessionPagina + "FormasCobrosAfiliadosListarMisFormasCobrosAfiliados"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("FormasCobrosAfiliadosAgregar.aspx");
                this.CargarLista();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosAfiliadosAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Consultar.ToString() ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEFormasCobrosAfiliados cobroAfiliado = this.MisFormasCobrosAfiliados[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdFormaCobroAfiliado", cobroAfiliado.IdFormaCobroAfiliado);
            
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosAfiliadosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosAfiliadosConsultar.aspx"), true);
            }

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGEFormasCobrosAfiliados item = (TGEFormasCobrosAfiliados)e.Row.DataItem;

                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                consultar.Visible = this.ValidarPermiso("FormasCobrosAfiliadosConsultar.aspx");
                modificar.Visible = this.ValidarPermiso("FormasCobrosAfiliadosModificar.aspx");
                
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisFormasCobrosAfiliados.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        private void CargarLista()
        {
            TGEFormasCobrosAfiliados parametro = new TGEFormasCobrosAfiliados();
            parametro.IdAfiliado = this.MiAfiliado.IdAfiliado;
            parametro.Estado.IdEstado = (int)EstadosTodos.Todos;
            this.MisFormasCobrosAfiliados = TGEGeneralesF.FormasCobrosAfiliadosObtenerListaFiltro(parametro);
            this.gvDatos.DataSource = this.MisFormasCobrosAfiliados;
            this.gvDatos.DataBind();
        }
    }
}
