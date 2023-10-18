using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using System.Collections;
using Comunes.Entidades;
using Afiliados;

namespace IU.Modulos.Afiliados
{
    public partial class MensajesAlertasListar : PaginaAfiliados
    {
        private List<AfiMensajesAlertas> MisMensajes
        {
            get { return (List<AfiMensajesAlertas>)Session[this.MiSessionPagina + "MensajesAlertasListarMisMensajes"]; }
            set { Session[this.MiSessionPagina + "MensajesAlertasListarMisMensajes"] = value; }
        }
        
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("MensajesAlertasAgregar.aspx");
                this.CargarLista();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/MensajesAlertasAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AfiMensajesAlertas mensaje = this.MisMensajes[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdMensajeAlerta", mensaje.IdMensajeAlerta);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/MensajesAlertasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                //string url = string.Concat(, parametros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/MensajesAlertasConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiMensajesAlertas mensaje = (AfiMensajesAlertas)e.Row.DataItem;

                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("MensajesAlertasModificar.aspx");
                consultar.Visible = this.ValidarPermiso("MensajesAlertasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisMensajes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiMensajesAlertas parametros = this.BusquedaParametrosObtenerValor<AfiMensajesAlertas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiMensajesAlertas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisMensajes;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisMensajes = this.OrdenarGrillaDatos<AfiMensajesAlertas>(this.MisMensajes, e);
            this.gvDatos.DataSource = this.MisMensajes;
            this.gvDatos.DataBind();
        }

        private void CargarLista()
        {
            AfiMensajesAlertas mensaje = new AfiMensajesAlertas();
            mensaje.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.MisMensajes = AfiliadosF.MensajesAlertasObtenerListaFiltro(mensaje);
            this.gvDatos.DataSource = this.MisMensajes;
            this.gvDatos.DataBind();
        }
    }
}