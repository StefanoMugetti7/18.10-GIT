using Compras;
using Compras.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras.Controles
{
    public partial class ImportarOrdenesComprasDetallesPopUp : ControlesSeguros
    {
        private DataTable MisRecepcionesDetalles
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarOrdenesComprasDetallesPopUppMisRecepcionesDetalles"]; }
            set { Session[this.MiSessionPagina + "ImportarOrdenesComprasDetallesPopUppMisRecepcionesDetalles"] = value; }
        }

        private CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "ImportarOrdenesComprasDetallesPopUppMiProveedor"]; }
            set { Session[this.MiSessionPagina + "ImportarOrdenesComprasDetallesPopUppMiProveedor"] = value; }
        }

        public delegate void ImportarOrdenesComprasDetallesBuscarEventHandler(List<CmpInformesRecepcionesDetalles> e);
        public event ImportarOrdenesComprasDetallesBuscarEventHandler ControlAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoOrdenCompra, this.btnBuscar);
            }
        }
        public void IniciarControl(CapProveedores pFiltro)
        {
            this.MiProveedor = pFiltro;
            this.CargarCombos();
            this.txtCodigoOrdenCompra.Text = string.Empty;
            this.CargarLista();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarOrdenesComprasDetalles();", true);
        }
        private void CargarCombos()
        {
            this.ddlCondicionPago.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CondicionesPagos);
            this.ddlCondicionPago.DataValueField = "IdListaValorDetalle";
            this.ddlCondicionPago.DataTextField = "Descripcion";
            this.ddlCondicionPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarLista();
        }
        private void CargarLista()
        {
            CmpOrdenesCompras pOrdenCompra = new CmpOrdenesCompras();
            pOrdenCompra.IdOrdenCompra = this.txtCodigoOrdenCompra.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigoOrdenCompra.Text);

            pOrdenCompra.CondicionPago.IdCondicionPago = this.ddlCondicionPago.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlCondicionPago.SelectedValue);
            //pOrdenCompra.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //VER QUE BUSQUE CON EL POP UP
            pOrdenCompra.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pOrdenCompra.Proveedor = MiProveedor;
            pOrdenCompra.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pOrdenCompra.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pOrdenCompra.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CmpOrdenesCompras>(pOrdenCompra);
            this.MisRecepcionesDetalles = ComprasF.OrdenCompraDetalleObtenerListaPopUp(pOrdenCompra);
            this.phDatos.Visible = this.MisRecepcionesDetalles.Rows.Count > 0;
            this.gvDatos.DataSource = this.MisRecepcionesDetalles;
            this.gvDatos.PageIndex = pOrdenCompra.IndiceColeccion;
            this.gvDatos.DataBind();
            this.btnAceptar.Visible = this.MisRecepcionesDetalles.Rows.Count > 0;
            this.btnCancelar.Visible = this.MisRecepcionesDetalles.Rows.Count > 0;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            List<CmpInformesRecepcionesDetalles> lista = new List<CmpInformesRecepcionesDetalles>();
            //si no hay ninguna factura, no existen items a mapear por lo que solo escondo el PopUp
            if (this.MisRecepcionesDetalles.Rows.Count > 0)
            {
                CmpInformesRecepcionesDetalles item;
                foreach (GridViewRow fila in this.gvDatos.Rows)
                {
                    if (fila.RowType == DataControlRowType.DataRow)
                    {
                        DataRow row = this.MisRecepcionesDetalles.AsEnumerable().Where(dr => dr.Field<int>("IdOrdenCompraDetalle") == Convert.ToInt32(this.gvDatos.DataKeys[fila.RowIndex].Value)).FirstOrDefault();
                        CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                        if (chkIncluir.Checked)
                        {
                            item = new CmpInformesRecepcionesDetalles();
                            item.IdOrdenCompraDetalle = (int)row["IdOrdenCompraDetalle"];
                            item.Producto.IdProducto = (int)row["IdProducto"];
                            item.Producto.Descripcion = row["Descripcion"].ToString();
                            item.Producto.Familia.Stockeable = (bool)row["Stockeable"];
                            item.TipoNumeroFactura = row["TipoNumeroFactura"].ToString();//REVISAR
                            item.CantidadRecibida = (decimal)row["CantidadRecibida"];
                            lista.Add(item);
                        }
                    }
                }
            }
            if (this.ControlAceptar != null)
            {
                this.ControlAceptar(lista);
            }
        }
    }
}