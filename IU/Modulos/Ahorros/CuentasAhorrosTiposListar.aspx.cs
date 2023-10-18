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
using Ahorros.Entidades;
using System.Collections.Generic;
using Ahorros;
using Comunes.Entidades;
using LavaYa.Entidades;

namespace IU.Modulos.Ahorros
{
    public partial class CuentasAhorrosTiposListar : PaginaSegura
    {
        private List<AhoCuentasTipos> MisCuentasTipos
        {
            get { return (List<AhoCuentasTipos>)Session[this.MiSessionPagina + "CuentasListarMisCuentasTipos"]; }
            set { Session[this.MiSessionPagina + "CuentasListarMisCuentasTipos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("CuentasAhorrosTiposAgregar.aspx");
                this.CargarLista();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;
             
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AhoCuentasTipos cuentafiliado = this.MisCuentasTipos[index];
            //string parametros = string.Format("?IdCuenta={0}", cuentafiliado.IdCuenta);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCuentaTipo", cuentafiliado.IdCuentaTipo);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AhoCuentasTipos cuenta = (AhoCuentasTipos)e.Row.DataItem;

                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");

                
                modificar.Visible = this.ValidarPermiso("CuentasAhorrosTiposModificar.aspx");
                consultar.Visible = this.ValidarPermiso("CuentasAhorrosTiposConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCuentasTipos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AhoCuentasTipos parametros = this.BusquedaParametrosObtenerValor<AhoCuentasTipos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AhoCuentasTipos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCuentasTipos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCuentasTipos = this.OrdenarGrillaDatos<AhoCuentasTipos>(this.MisCuentasTipos, e);
            this.gvDatos.DataSource = this.MisCuentasTipos;
            this.gvDatos.DataBind();
        }

        private void CargarLista(AhoCuentasTipos pParametro)
        {
            AhoCuentasTipos cuenta = new AhoCuentasTipos();
            cuenta.Descripcion = this.txtDescripcion.Text;
            this.MisCuentasTipos = AhorroF.CuentasTiposObtenerListaFiltro(cuenta);
            this.gvDatos.DataSource = this.MisCuentasTipos;
            this.gvDatos.DataBind();
        }
        private void CargarLista()
        {
            AhoCuentasTipos cuenta = new AhoCuentasTipos();
            cuenta.Descripcion = this.txtDescripcion.Text;
            this.MisCuentasTipos = AhorroF.CuentasTiposObtenerListaFiltro(cuenta);
            this.gvDatos.DataSource = this.MisCuentasTipos;
            this.gvDatos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AhoCuentasTipos parametros = this.BusquedaParametrosObtenerValor<AhoCuentasTipos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }
    }
}
