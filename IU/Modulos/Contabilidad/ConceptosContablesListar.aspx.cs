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
    public partial class ConceptosContablesListar : PaginaSegura
    {
        private List<CtbConceptosContables> MisConceptosContables
        {
            get { return (List<CtbConceptosContables>)Session[this.MiSessionPagina + "ConceptosContablesListarMisConceptosContables"]; }
            set { Session[this.MiSessionPagina + "ConceptosContablesListarMisConceptosContables"] = value; }
        }

        private CtbCuentasContables MiCuentaContable
        {
            get { return (CtbCuentasContables)Session[this.MiSessionPagina + "ConceptosContablesListarMiCuentaContable"]; }
            set { Session[this.MiSessionPagina + "ConceptosContablesListarMiCuentaContable"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.buscarCuenta.CuentasContablesBuscarIniciar += new Controles.CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(buscarCuenta_CuentasContablesBuscarIniciar);
            this.buscarCuenta.CuentasContablesBuscarSeleccionar+=new Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(buscarCuenta_CuentasContablesBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtConceptoContable, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("ConceptosContablesAgregar.aspx");
                this.CargarCombos();    
                CtbConceptosContables parametros = this.BusquedaParametrosObtenerValor<CtbConceptosContables>();
                if (parametros.BusquedaParametros)
                {
                    this.txtConceptoContable.Text = parametros.ConceptoContable;
                    this.buscarCuenta.MapearObjetoControles(parametros.CuentaContable, Gestion.Modificar, 0);
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbConceptosContables parametros = this.BusquedaParametrosObtenerValor<CtbConceptosContables>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesAgregar.aspx"), true);
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
            CtbConceptosContables conceptoContable = this.MisConceptosContables[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdConceptoContable", conceptoContable.IdConceptoContable);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                consultar.Visible = this.ValidarPermiso("ConceptosContablesConsultar.aspx");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                modificar.Visible = this.ValidarPermiso("ConceptosContablesModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisConceptosContables.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbMonedas parametros = this.BusquedaParametrosObtenerValor<CtbMonedas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbMonedas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisConceptosContables;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisConceptosContables = this.OrdenarGrillaDatos<CtbConceptosContables>(this.MisConceptosContables, e);
            this.gvDatos.DataSource = this.MisConceptosContables;
            this.gvDatos.DataBind();
        }

        void buscarCuenta_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        {
            AyudaProgramacion.MatchObjectProperties( ContabilidadF.EjerciciosContablesObtenerUltimoActivo(), ejercicio);
        }

        protected void buscarCuenta_CuentasContablesBuscarSeleccionar(CtbCuentasContables pCuentaContable, int indiceColeccion)
        {
            this.MiCuentaContable = pCuentaContable;
            this.btnBuscar_Click(this.btnBuscar, EventArgs.Empty);
            this.UpdatePanel1.Update();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        private void CargarLista(CtbConceptosContables pConceptosContables)
        {
            pConceptosContables.ConceptoContable = this.txtConceptoContable.Text.Trim();
            pConceptosContables.CuentaContable = this.MiCuentaContable;
            pConceptosContables.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pConceptosContables.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbConceptosContables>(pConceptosContables);
            this.MisConceptosContables = ContabilidadF.ConceptosContablesObtenerListaFiltroCompleta(pConceptosContables);
            this.gvDatos.DataSource = this.MisConceptosContables;
            this.gvDatos.PageIndex = pConceptosContables.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}