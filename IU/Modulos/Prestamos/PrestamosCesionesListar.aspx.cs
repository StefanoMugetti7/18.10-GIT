using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestamos.Entidades;
using Comunes.Entidades;
using System.Collections;
using Generales.FachadaNegocio;
using Prestamos;

namespace IU.Modulos.Prestamos
{
    public partial class PrestamosCesionesListar : PaginaSegura
    {
        private List<PrePrestamosCesiones> MisPrestamosCesiones
        {
            get { return (List<PrePrestamosCesiones>)Session[this.MiSessionPagina + "PrestamosCesionesListarMisPrestamosCesiones"]; }
            set { Session[this.MiSessionPagina + "PrestamosCesionesListarMisPrestamosCesiones"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("PrestamosCesionesAgregar.aspx");
                this.CargarCombos();
                PrePrestamosCesiones parametros = this.BusquedaParametrosObtenerValor<PrePrestamosCesiones>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlProveedores.SelectedValue = parametros.Cesionario.Proveedor.IdProveedor == 0 ? string.Empty : parametros.Cesionario.Proveedor.IdProveedor.ToString();
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado==0? string.Empty : parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PrePrestamosCesiones parametros = this.BusquedaParametrosObtenerValor<PrePrestamosCesiones>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosCesionesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            PrePrestamosCesiones cesion = this.MisPrestamosCesiones[indiceColeccion];
            //string parametros = string.Format("?IdPlazo={0}", plan.IdPlazos);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPrestamoCesion", cesion.IdPrestamoCesion);
            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosCesionesAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosCesionesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Autorizar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosCesionesAutorizar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = this.ValidarPermiso("PrestamosCesionesConsultar.aspx");

                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                //ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                
                PrePrestamosCesiones prestamoCesion = (PrePrestamosCesiones)e.Row.DataItem;

                switch (prestamoCesion.Estado.IdEstado)
                {
                    case (int)EstadosCesiones.Activa:
                        ibtnAnular.Visible = this.ValidarPermiso("PrestamosCesionesAnular.aspx");
                        ibtnAutorizar.Visible = this.ValidarPermiso("PrestamosCesionesAutorizar.aspx");
                        break;
                    //case (int)EstadosCesiones.Anulado:
                    //    break;
                    //case (int)EstadosCesiones.Autorizado:
                    //    break;
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestamosCesiones.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrePrestamosCesiones parametros = this.BusquedaParametrosObtenerValor<PrePrestamosCesiones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<PrePrestamosCesiones>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisPrestamosCesiones;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPrestamosCesiones = this.OrdenarGrillaDatos<PrePrestamosCesiones>(this.MisPrestamosCesiones, e);
            this.gvDatos.DataSource = this.MisPrestamosCesiones;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosCesiones));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            PreCesionarios filtro = new PreCesionarios();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            this.ddlProveedores.DataSource = PrePrestamosF.CesionariosObtenerListaFiltro(filtro);
            this.ddlProveedores.DataValueField = "IdCesionario";
            this.ddlProveedores.DataTextField = "MiProveedorRazonSocial";
            this.ddlProveedores.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlProveedores, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            
        }

        private void CargarLista(PrePrestamosCesiones pParametro)
        {
            pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue==string.Empty? (int)EstadosTodos.Todos : Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Cesionario.IdCesionario = this.ddlProveedores.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlProveedores.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<PrePrestamosCesiones>(pParametro);
            this.MisPrestamosCesiones = PrePrestamosF.PrestamosCesionesObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisPrestamosCesiones;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}