using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;
using Contabilidad.Entidades;
using Contabilidad;

namespace IU.Modulos.TGE
{
    public partial class ValoresSistemaCuentasContablesListar : PaginaSegura
    {

        private List<TGEListasValoresSistemasDetallesCuentasContables> MiListaDetalleSysCta
        {
            get { return (List<TGEListasValoresSistemasDetallesCuentasContables>)Session[this.MiSessionPagina + "ValoresSistemaCuentasListarMiListaDetalleSysCta"]; }
            set { Session[this.MiSessionPagina + "ValoresSistemaCuentasListarMiListaDetalleSysCta"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrCuentasContables.CuentasContablesBuscarSeleccionar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(ctrCuentasContables_CuentasContablesBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("ValoresSistemaCuentasContablesAgregar.aspx");
                this.CargarCombos();
                TGEListasValoresSistemasDetallesCuentasContables parametros = this.BusquedaParametrosObtenerValor<TGEListasValoresSistemasDetallesCuentasContables>();
                if (parametros.BusquedaParametros)
                {
                    //this.txtParametro.Text = parametros.IdListaValorSistemaDetalleCuentaContable.ToString();
                    //this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        private void CargarCombos()
        {
            TGETiposFuncionalidades tipoFuncionalidad = new TGETiposFuncionalidades();
            tipoFuncionalidad.IdTipoFuncionalidad = this.paginaActual.IdTipoFuncionalidad;
            this.ddlListaValor.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerPorTipoFuncionalidad(tipoFuncionalidad);
            this.ddlListaValor.DataValueField = "IdListaValorSistema";
            this.ddlListaValor.DataTextField = "ListaValor";
            this.ddlListaValor.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlListaValor, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        void ctrCuentasContables_CuentasContablesBuscarSeleccionar(global::Contabilidad.Entidades.CtbCuentasContables e, int indiceColeccion)
        {
            TGEListasValoresSistemasDetallesCuentasContables parametros = this.BusquedaParametrosObtenerValor<TGEListasValoresSistemasDetallesCuentasContables>();
            parametros.CuentaContable = e;
            this.upCuentasContables.Update();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGEListasValoresSistemasDetallesCuentasContables parametros = this.BusquedaParametrosObtenerValor<TGEListasValoresSistemasDetallesCuentasContables>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ValoresSistemaCuentasContablesAgregar.aspx"), true);
        }

        #region GRILLA RELACION VALORES CUENTAS

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()))
                //|| e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEListasValoresSistemasDetallesCuentasContables param = this.MiListaDetalleSysCta[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdListaValorSistemaDetalleCuentaContable", param.IdListaValorSistemaDetalleCuentaContable);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ValoresSistemaCuentasContablesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ValoresSistemaCuentasContablesConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = false;//this.ValidarPermiso("ValoresSistemaCuentasContablesConsultar.aspx");

                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnModificar.Visible = this.ValidarPermiso("ValoresSistemaCuentasContablesModificar.aspx");
            }
                
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    int cellCount = e.Row.Cells.Count;
                    e.Row.Cells.Clear();
                    TableCell tableCell = new TableCell();
                    tableCell.ColumnSpan = cellCount;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiListaDetalleSysCta.Count);
                    e.Row.Cells.Add(tableCell);
                }

            
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEListasValoresSistemasDetallesCuentasContables parametros = this.BusquedaParametrosObtenerValor<TGEListasValoresSistemasDetallesCuentasContables>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEListasValoresSistemasDetallesCuentasContables>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiListaDetalleSysCta;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiListaDetalleSysCta = this.OrdenarGrillaDatos<TGEListasValoresSistemasDetallesCuentasContables>(this.MiListaDetalleSysCta, e);
            this.gvDatos.DataSource = this.MiListaDetalleSysCta;
            this.gvDatos.DataBind();
        }


        #endregion

        private void CargarLista(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            pParametro.BusquedaParametros = true;
            //pParametro.IdListaValorSistemaDetalleCuentaContable = this.txtParametro.Text == string.Empty ? 0 : Convert.ToInt32(this.txtParametro.Text);
            pParametro.ListaValorSistemaDetalle.ListaValorSistema.IdListaValorSistema = this.ddlListaValor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlListaValor.SelectedValue);
            //pParametro.ListaValorSistemaDetalle.IdListaValorSistemaDetalle = this.txtCodigoSistema.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigoSistema.Text);
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.BusquedaParametrosGuardarValor<TGEListasValoresSistemasDetallesCuentasContables>(pParametro);
            this.MiListaDetalleSysCta = ContabilidadF.ValoresSistemasCuentasContablesObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MiListaDetalleSysCta;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }

    }
}