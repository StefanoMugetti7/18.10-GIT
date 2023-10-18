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

namespace IU.Modulos.Ahorros
{
    public partial class CuentasListar : PaginaAfiliados
    {
        private List<AhoCuentas> MisCuentasAfiliados
        {
            get { return (List<AhoCuentas>)Session[this.MiSessionPagina + "CuentasListarMisCuentasAfiliados"]; }
            set { Session[this.MiSessionPagina + "CuentasListarMisCuentasAfiliados"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("CuentasAgregar.aspx");
                this.CargarLista();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAgregar.aspx"), true);
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
            AhoCuentas cuentafiliado = this.MisCuentasAfiliados[indiceColeccion];
            //string parametros = string.Format("?IdCuenta={0}", cuentafiliado.IdCuenta);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCuenta", cuentafiliado.IdCuenta);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                //string url = string.Concat(, parametros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasConsultar.aspx"), true);
            }
            else if (e.CommandName == "Movimientos")
            {
                //switch (cuentafiliado.CuentaTipo.IdCuentaTipo)
                //{
                //case (int)EnumAhorrosCuentasTipos.CuentaCorriente:
                //    break;
                //case (int)EnumAhorrosCuentasTipos.CajaAhorro:
                //case (int)EnumAhorrosCuentasTipos.CajaAhorroHaberes:
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasMovimientosListar.aspx"), true);
                //break;
                //case (int)EnumAhorrosCuentasTipos.PlazosFijos:
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
                //    break;
                //default:
                //    break;
                //}
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AhoCuentas cuenta = (AhoCuentas)e.Row.DataItem;
                if (cuenta.SaldoActual < 0)
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;

                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton movimientos = (ImageButton)e.Row.FindControl("btnMovimientos");

                switch (cuenta.CuentaTipo.IdCuentaTipo)
                {
                    case (int)EnumAhorrosCuentasTipos.CuentaCorriente:
                        break;
                    case (int)EnumAhorrosCuentasTipos.CajaAhorro:
                    case (int)EnumAhorrosCuentasTipos.CajaAhorroHaberes:
                        modificar.Visible = this.ValidarPermiso("CuentasModificar.aspx");
                        movimientos.Visible = this.ValidarPermiso("CuentasMovimientosListar.aspx");
                        break;
                    case (int)EnumAhorrosCuentasTipos.PlazosFijos:
                        modificar.Visible = this.ValidarPermiso("PlazosFijosModificar.aspx");
                        movimientos.Visible = this.ValidarPermiso("PlazosFijosListar.aspx");
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCuentasAfiliados.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AhoCuentas parametros = this.BusquedaParametrosObtenerValor<AhoCuentas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AhoCuentas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCuentasAfiliados;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCuentasAfiliados = this.OrdenarGrillaDatos<AhoCuentas>(this.MisCuentasAfiliados, e);
            this.gvDatos.DataSource = this.MisCuentasAfiliados;
            this.gvDatos.DataBind();
        }

        private void CargarLista()
        {
            AhoCuentas cuenta = new AhoCuentas();
            cuenta.Afiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.MisCuentasAfiliados = AhorroF.CuentasObtenerListaFiltro(cuenta);
            this.gvDatos.DataSource = this.MisCuentasAfiliados;
            this.gvDatos.DataBind();
        }
    }
}
