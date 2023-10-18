using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Contabilidad;
using System.Collections;

namespace IU.Modulos.Contabilidad
{
    public partial class DepartamentosListar : PaginaSegura
    {
        private List<CtbDepartamentos> MisDepartamentos
        {
            get { return (List<CtbDepartamentos>)Session[this.MiSessionPagina + "DepartamentosListar"]; }
            set { Session[this.MiSessionPagina + "DepartamentosListar"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("DepartamentosAgregar.aspx");
                this.CargarCombos();
                CtbDepartamentos parametros = this.BusquedaParametrosObtenerValor<CtbDepartamentos>();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDepartamento, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoDepartamento, this.btnBuscar);
                if (parametros.BusquedaParametros)
                {
                    this.txtDepartamento.Text = parametros.Departamento;
                    this.txtCodigoDepartamento.Text = parametros.CodigoDepartamento;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbDepartamentos parametros = this.BusquedaParametrosObtenerValor<CtbDepartamentos>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosAgregar.aspx"), true);
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
            CtbDepartamentos departamento = this.MisDepartamentos[indiceColeccion];
            this.MisParametrosUrl = new Hashtable
            {
                { "IdDepartamento", departamento.IdDepartamento }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CtbDepartamentos departamento = (CtbDepartamentos)e.Row.DataItem;

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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDepartamentos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbDepartamentos parametros = this.BusquedaParametrosObtenerValor<CtbDepartamentos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbDepartamentos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisDepartamentos;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisDepartamentos = this.OrdenarGrillaDatos<CtbDepartamentos>(this.MisDepartamentos, e);
            this.gvDatos.DataSource = this.MisDepartamentos;
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
        }
        private void CargarLista(CtbDepartamentos pDepartamentos)
        {
            pDepartamentos.Departamento = this.txtDepartamento.Text.Trim();
            pDepartamentos.CodigoDepartamento = this.txtCodigoDepartamento.Text.Trim();
            pDepartamentos.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pDepartamentos.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbDepartamentos>(pDepartamentos);
            this.MisDepartamentos = ContabilidadF.DepartamentosObtenerListar(pDepartamentos);
            this.gvDatos.DataSource = this.MisDepartamentos;
            this.gvDatos.PageIndex = pDepartamentos.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}