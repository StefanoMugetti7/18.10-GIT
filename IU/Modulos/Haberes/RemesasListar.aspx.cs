using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Haberes.Entidades;
using System.Collections;
using Comunes.Entidades;
using Haberes;

namespace IU.Modulos.Haberes
{
    public partial class RemesasListar : PaginaSegura
    {
        private List<HabRemesas> MisRemesas
        {
            get { return (List<HabRemesas>)Session[this.MiSessionPagina + "RemesasListarMisRemesas"]; }
            set { Session[this.MiSessionPagina + "RemesasListarMisRemesas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //this.btnAgregar.Visible = this.ValidarPermiso("CuentasAgregar.aspx");
                this.CargarLista();
            }
        }

        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/CuentasAgregar.aspx"), true);
        //}

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            HabRemesas remesa = this.MisRemesas[indiceColeccion];
            //string parametros = string.Format("?IdCuenta={0}", cuentafiliado.IdCuenta);
            this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("IdRemesa", remesa.IdRemesa);
            this.MisParametrosUrl.Add("Periodo", remesa.Periodo);
            this.MisParametrosUrl.Add("IdRemesaTipo", remesa.RemesaTipo.IdRemesaTipo);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/RemesasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                //string url = string.Concat(, parametros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/RemesasConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HabRemesas remesa = (HabRemesas)e.Row.DataItem;
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                consultar.Visible = this.ValidarPermiso("RemesasConsultar.aspx");

                switch (remesa.Estado.IdEstado)
                {
                    case (int)Estados.Activo:
                        modificar.Visible = this.ValidarPermiso("RemesasModificar.aspx");
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisRemesas.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            HabRemesas parametros = this.BusquedaParametrosObtenerValor<HabRemesas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<HabRemesas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisRemesas;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisRemesas = this.OrdenarGrillaDatos<HabRemesas>(this.MisRemesas, e);
            this.gvDatos.DataSource = this.MisRemesas;
            this.gvDatos.DataBind();
        }

        private void CargarLista()
        {
            HabRemesas cuenta = new HabRemesas();
            this.MisRemesas = HaberesF.RemesasObtenerLista(cuenta);
            AyudaProgramacion.CargarGrillaListas<HabRemesas>(this.MisRemesas, false, this.gvDatos, true);
        }
    }
}
