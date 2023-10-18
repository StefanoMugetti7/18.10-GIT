using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using System.Collections;
using Comunes.Entidades;
using Contabilidad;
using Seguridad.FachadaNegocio;
using Generales.Entidades;
using System.Data;
using Afiliados;
using Afiliados.Entidades;
using CRM.Entidades;

namespace IU.Modulos.Contabilidad
{
    public partial class LibroMayorListar : PaginaSegura
    {

        private CtbCuentasContables MiCuentaContable
        {
            get { return (CtbCuentasContables)Session[this.MiSessionPagina + "MiCuentaContable"]; }
            set { Session[this.MiSessionPagina + "MiCuentaContable"] = value; }
        }

        private List<CtbEjerciciosContables> MisEjerciciosContables
        {
            get { return (List<CtbEjerciciosContables>)Session[this.MiSessionPagina + "MisEjerciciosContables"]; }
            set { Session[this.MiSessionPagina + "MisEjerciciosContables"] = value; }
        }

        private DataTable MisDatosGrilla
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisDatosGrillaLibroMayor"]; }
            set { Session[this.MiSessionPagina + "MisDatosGrillaLibroMayor"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.buscarCuenta.CuentasContablesBuscarSeleccionar+=new Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(buscarCuenta_CuentasContablesBuscarSeleccionar);
            //this.buscarCuenta.CuentasContablesBuscarIniciar += new Controles.CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(buscarCuenta_CuentasContablesBuscarIniciar);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                CtbEjerciciosContables filtroAsiento = new CtbEjerciciosContables();
                filtroAsiento.Estado.IdEstado = (int)EstadosEjerciosContables.Activo;
                this.MisEjerciciosContables = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtroAsiento);

                this.ddlFilial.DataSource = UsuarioActivo.Filiales;
                this.ddlFilial.DataValueField = "IdFilial";
                this.ddlFilial.DataTextField = "Filial";
                this.ddlFilial.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(this.ddlFilial, ObtenerMensajeSistema("SeleccioneOpcion"));
                //this.ddlFilial.SelectedValue = UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

                this.ddlEjercicioContable.DataSource = this.MisEjerciciosContables;
                this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
                this.ddlEjercicioContable.DataTextField = "Descripcion";
                this.ddlEjercicioContable.DataBind();
                this.ddlEjercicioContable_SelectedIndexChanged(this.ddlEjercicioContable, EventArgs.Empty);

                CtbCuentasContables parametros = this.BusquedaParametrosObtenerValor<CtbCuentasContables>();
                //this.buscarCuenta.Validation = true;
                //this.buscarCuenta.ValidationGroup = "Buscar";

                if (parametros.BusquedaParametros)
                {
                    this.ddlEjercicioContable.SelectedValue = parametros.IdEjercicioContable.ToString();
                    this.ddlEjercicioContable_SelectedIndexChanged(this.ddlEjercicioContable, EventArgs.Empty);
                    this.txtFechaDesde.Text = parametros.FechaDesde.ToShortDateString();
                    this.txtFechaHasta.Text = parametros.FechaHasta.ToShortDateString();
                    this.MiCuentaContable = parametros;
                    this.CargarLista(parametros);
                    if(!string.IsNullOrEmpty(parametros.CuentaContable))
                    {
                        this.txtCuenta.Text = parametros.CuentaContable;
                        this.ddlCuentaContable.Items.Add(new ListItem(parametros.CuentaContableCompleta, parametros.IdCuentaContable.ToString()));
                        this.ddlCuentaContable.SelectedValue = parametros.CuentaContableCompleta;
                    }
                    //this.buscarCuenta.MapearObjetoControles(parametros, Gestion.Modificar, 0); 
                }
            }
        }

        protected void ddlEjercicioContable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
            {
                CtbEjerciciosContables ejerCon = this.MisEjerciciosContables[this.ddlEjercicioContable.SelectedIndex];
                //this.ceFechaDesde.StartDate = ejerCon.FechaInicio;
                //this.ceFechaDesde.EndDate = ejerCon.FechaFin;
                //this.ceFechaHasta.StartDate = ejerCon.FechaInicio;
                //this.ceFechaHasta.EndDate = ejerCon.FechaFin;

                this.txtFechaDesde.Text = ejerCon.FechaInicio.ToShortDateString();
                this.txtFechaHasta.Text = ejerCon.FechaFin.ToShortDateString();

                this.MiCuentaContable = new CtbCuentasContables();
              //  this.buscarCuenta.MapearObjetoControles(this.MiCuentaContable, Gestion.Modificar, 0);

                this.gvDatos.DataSource = null;
                this.gvDatos.DataBind();
                this.upGrilla.Update();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCuenta.Text))
            {
                this.MostrarMensaje("Debe ingresar una cuenta contable.", true);
                this.ddlCuentaContable.Items.Clear();
            }
            else
            {
                CtbCuentasContables parametros = this.BusquedaParametrosObtenerValor<CtbCuentasContables>();
                parametros.PageIndex = 0;
                gvDatos.PageIndex = 0;
                this.CargarLista(parametros);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbCuentasContables parametros = BusquedaParametrosObtenerValor<CtbCuentasContables>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            int IdAfiliado = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdAfiliado")).Value);
            int IdAsientoContable = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdAsientoContable")).Value);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
             
            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)
            if (e.CommandName == Gestion.Consultar.ToString())
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

            if (IdTipoOperacion == (int)EnumTGETiposOperaciones.AsientoContableManual)
            {
                IdRefTipoOperacion = IdAsientoContable;
            }

            //Guardo Menu devuelto de la DB
            filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
            this.MisParametrosUrl.Add("IdTipoOperacion", IdTipoOperacion);
            //Si devuelve una URL Redirecciona si no muestra mensaje error
            if (filtroMenu.URL.Length != 0)
            {
                if (IdAfiliado > 0)
                {
                    AfiAfiliados afi = new AfiAfiliados();
                    afi.IdAfiliado = IdAfiliado;
                    PaginaAfiliados paginaAfi = new PaginaAfiliados();
                    paginaAfi.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatos(afi));
                }
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros( string.Concat("~/", filtroMenu.URL)), true);
            }
            else
            {
                this.MiCuentaContable.CodigoMensaje = "ErrorURLNoValida";
                this.MostrarMensaje(this.MiCuentaContable.CodigoMensaje, true);
            }
            
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                consultar.Visible = this.ValidarPermiso("LibroMayorListar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDatosGrilla.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        private void CargarLista(CtbCuentasContables pFiltro)
        {
            //mapeo la cuenta contable que obtengo del Control de Buscar Cuenta Contable;
            int index = pFiltro.PageIndex;
            pFiltro = this.MiCuentaContable;
            pFiltro.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            pFiltro.FechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text);
            pFiltro.FechaHasta = Convert.ToDateTime(this.txtFechaHasta.Text);
            pFiltro.IdFilial = this.ddlFilial.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pFiltro.BusquedaParametros = true;
            pFiltro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            this.gvDatos.PageSize = pFiltro.PageSize;

            if (!string.IsNullOrEmpty(this.hdfCuentaContableCompleta.Value))
            {
                pFiltro.IdCuentaContable = this.hdfIdCuentaContable.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdCuentaContable.Value);
                pFiltro.CuentaContable = this.hdfCuentaContable.Value == string.Empty ? string.Empty : this.hdfCuentaContable.Value;
                pFiltro.CuentaContableCompleta = this.hdfCuentaContableCompleta.Value == string.Empty ? string.Empty : this.hdfCuentaContableCompleta.Value;
                MapearCuentaContable();
            }
            this.BusquedaParametrosGuardarValor<CtbCuentasContables>(pFiltro);
            pFiltro.PageIndex = index;
            this.MisDatosGrilla = ContabilidadF.AsientosContablesObtenerLibroMayorGrilla(pFiltro);
            this.gvDatos.DataSource = this.MisDatosGrilla;
            this.gvDatos.VirtualItemCount = MisDatosGrilla.Rows.Count > 0 ? Convert.ToInt32(MisDatosGrilla.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = index;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);
            //this.gvDatos.PageIndex = pCuentaContable.IndiceColeccion;
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CtbCuentasContables parametros = this.BusquedaParametrosObtenerValor<CtbCuentasContables>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }
        private void MapearCuentaContable()
        {
            if (!string.IsNullOrEmpty(this.hdfCuentaContableCompleta.Value))
            {
                this.txtCuenta.Text = this.hdfCuentaContable.Value;
                this.ddlCuentaContable.Items.Add(new ListItem(this.hdfCuentaContableCompleta.Value, this.hdfIdCuentaContable.Value));
                this.ddlCuentaContable.SelectedValue = this.hdfIdCuentaContable.Value;
            }
            else
            {
                this.txtCuenta.Text = string.Empty;
                this.ddlCuentaContable.Items.Add(new ListItem(string.Empty, string.Empty));
                this.ddlCuentaContable.SelectedValue = string.Empty;
            }
        }
    }
}
