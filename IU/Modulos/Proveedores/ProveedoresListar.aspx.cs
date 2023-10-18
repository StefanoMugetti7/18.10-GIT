using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Proveedores;
using Proveedores.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Data;

namespace IU.Modulos.Proveedores
{
    public partial class ProveedoresListar : PaginaSegura
    {
        private DataTable MisProveedores
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProveedoresListarMisProveedores"]; }
            set { Session[this.MiSessionPagina + "ProveedoresListarMisProveedores"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
               
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroProveedor, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCuit, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtRazonSocial, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("ProveedoresAgregar.aspx");
                this.CargarCombos();
                
                CapProveedores parametros = this.BusquedaParametrosObtenerValor<CapProveedores>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroProveedor.Text = parametros.IdProveedor.ToString() == "-1"? String.Empty : parametros.IdProveedor.ToString();
                    this.txtRazonSocial.Text = parametros.RazonSocial;
                    this.txtCuit.Text = parametros.CUIT;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtDetalle.Text = parametros.Detalle;
                    this.chkTieneSaldo.Checked = parametros.TieneSaldo;
                    this.ctrCamposValores.IniciarControl(parametros.Campos, Gestion.Listar);
                    this.CargarLista(parametros);
                }
                else
                    this.ctrCamposValores.IniciarControl(parametros, new Objeto(), Gestion.Listar);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CapProveedores parametros = this.BusquedaParametrosObtenerValor<CapProveedores>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //CapProveedores proveedor = this.MisProveedores[indiceColeccion];

            //this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("IdProveedor", proveedor.IdProveedor);
            ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            parametros.Agregar("IdProveedor", indiceColeccion);//proveedor.IdProveedor);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("ProveedoresConsultar.aspx");
                //ibtnConsultar.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = this.MisProveedores.AsEnumerable().Sum(x => x.Field<decimal>("SaldoActual")).ToString("C2");

                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisProveedores.Rows.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CapProveedores parametros = this.BusquedaParametrosObtenerValor<CapProveedores>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CapProveedores>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisProveedores;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisProveedores = this.OrdenarGrillaDatos<CapProveedores>(this.MisProveedores, e);
            this.gvDatos.DataSource = this.MisProveedores;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisProveedores;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        private void CargarCombos()
        {
            //VER
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstados.SelectedValue = "-1";

        }

        private void CargarLista(CapProveedores pProveedor)
        {
            pProveedor.IdProveedor = this.txtNumeroProveedor.Text == string.Empty ? -1 : Convert.ToInt32(this.txtNumeroProveedor.Text);
            pProveedor.CUIT = this.txtCuit.Text;
            pProveedor.RazonSocial = this.txtRazonSocial.Text;
            pProveedor.Detalle = this.txtDetalle.Text.Trim();
            pProveedor.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //ARREGLAR HARDCODEO
            pProveedor.CondicionFiscal.IdCondicionFiscal = -1;
            pProveedor.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            pProveedor.TieneSaldo = this.chkTieneSaldo.Checked;
            pProveedor.Campos = this.ctrCamposValores.ObtenerLista();
            pProveedor.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
            pProveedor.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CapProveedores>(pProveedor);
            this.MisProveedores = ProveedoresF.ProveedoresObtenerListaFiltroDT(pProveedor);
            this.gvDatos.DataSource = this.MisProveedores;
            this.gvDatos.PageIndex = pProveedor.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisProveedores.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}
    