using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;

namespace IU.Modulos.Contabilidad
{
    public partial class CuentasContablesListar : PaginaSegura
    {
        private List<CtbCuentasContables> MisCuentasContables
        {
            get { return (List<CtbCuentasContables>)Session[this.MiSessionPagina + "CuentasContablesListar"]; }
            set { Session[this.MiSessionPagina + "CuentasContablesListar"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CuentasContablesAgregar.aspx"), true);

                this.btnAgregar.Visible = this.ValidarPermiso("CuentasContablesAgregar.aspx");
                this.CargarCombos();
                CtbCuentasContables parametros = this.BusquedaParametrosObtenerValor<CtbCuentasContables>();
                if (parametros.BusquedaParametros)
                {
                    this.txtDescripción.Text = parametros.Descripcion;
                    this.txtNumeroCuenta.Text = parametros.NumeroCuenta;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlCapitulo.SelectedValue = parametros.Capitulo.IdCapitulo.ToString();
                    this.ddlDepartamento.SelectedValue = parametros.Departamento.IdDepartamento.ToString();
                    this.ddlMoneda.SelectedValue = parametros.Moneda.IdMoneda.ToString();
                    this.ddlRubro.SelectedValue = parametros.Rubro.IdRubro.ToString();
                    this.ddlSubRubro.SelectedValue = parametros.SubRubro.IdSubRubro.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbCuentasContables parametros = this.BusquedaParametrosObtenerValor<CtbCuentasContables>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CuentasContablesAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CtbCuentasContables cuentaContable = this.MisCuentasContables[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCuentaContable", cuentaContable.IdCuentaContable);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CuentasContablesModificar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CtbCuentasContables cuentaContable = (CtbCuentasContables)e.Row.DataItem;

                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton movimientos = (ImageButton)e.Row.FindControl("btnMovimientos");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCuentasContables.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbCuentasContables parametros = this.BusquedaParametrosObtenerValor<CtbCuentasContables>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbCuentasContables>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCuentasContables;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCuentasContables = this.OrdenarGrillaDatos<CtbCuentasContables>(this.MisCuentasContables, e);
            this.gvDatos.DataSource = this.MisCuentasContables;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";
            this.ddlCapitulo.DataSource = ContabilidadF.CapitulosObtenerListar(new CtbCapitulos());
            this.ddlCapitulo.DataValueField = "IdCapitulo";
            this.ddlCapitulo.DataTextField = "Capitulo";
            this.ddlCapitulo.DataBind();
            this.ddlCapitulo.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlCapitulo.SelectedValue = "-1";
            this.ddlDepartamento.DataSource = ContabilidadF.DepartamentosObtenerListar(new CtbDepartamentos());
            this.ddlDepartamento.DataValueField = "IdDepartamento";
            this.ddlDepartamento.DataTextField = "Departamento";
            this.ddlDepartamento.DataBind();
            this.ddlDepartamento.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlDepartamento.SelectedValue = "-1";
            this.ddlMoneda.DataSource = ContabilidadF.MonedasObtenerListar(new CtbMonedas());
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Moneda";
            this.ddlMoneda.DataBind();
            this.ddlMoneda.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlMoneda.SelectedValue = "-1";
            this.ddlRubro.DataSource = ContabilidadF.RubrosObtenerListar(new CtbRubros());
            this.ddlRubro.DataValueField = "IdRubro";
            this.ddlRubro.DataTextField = "Rubro";
            this.ddlRubro.DataBind();
            this.ddlRubro.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlRubro.SelectedValue = "-1";
            this.ddlSubRubro.DataSource = ContabilidadF.SubRubrosObtenerListar(new CtbSubRubros());
            this.ddlSubRubro.DataValueField = "IdSubRubro";
            this.ddlSubRubro.DataTextField = "SubRubro";
            this.ddlSubRubro.DataBind();
            this.ddlSubRubro.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlSubRubro.SelectedValue = "-1";
        }

        private void CargarLista(CtbCuentasContables pCuentasContalbes)
        {
            pCuentasContalbes.Descripcion = this.txtDescripción.Text.Trim();
            pCuentasContalbes.NumeroCuenta = this.txtNumeroCuenta.Text.Trim();
            pCuentasContalbes.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pCuentasContalbes.Capitulo.IdCapitulo = Convert.ToInt32(this.ddlCapitulo.SelectedValue);
            pCuentasContalbes.Departamento.IdDepartamento = Convert.ToInt32(this.ddlDepartamento.SelectedValue);
            pCuentasContalbes.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            pCuentasContalbes.Rubro.IdRubro = Convert.ToInt32(this.ddlRubro.SelectedValue);
            pCuentasContalbes.SubRubro.IdSubRubro = Convert.ToInt32(this.ddlSubRubro.SelectedValue);
            pCuentasContalbes.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbCuentasContables>(pCuentasContalbes);
            this.MisCuentasContables = ContabilidadF.CuentasContablesObtenerListaFiltro(pCuentasContalbes);
            this.gvDatos.DataSource = this.MisCuentasContables;
            this.gvDatos.PageIndex = pCuentasContalbes.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}