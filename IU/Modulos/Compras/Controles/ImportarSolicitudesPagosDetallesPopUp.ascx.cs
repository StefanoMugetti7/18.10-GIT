using Compras;
using Compras.Entidades;
using CuentasPagar.Entidades;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras.Controles
{
    public partial class ImportarSolicitudesPagosDetallesPopUp : ControlesSeguros
    {
        private DataTable MisRecepcionesAcopios
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarSolicitudesPagosDetallesPopUppMisRecepcionesAcopios"]; }
            set { Session[this.MiSessionPagina + "ImportarSolicitudesPagosDetallesPopUppMisRecepcionesAcopios"] = value; }
        }

        private DataTable MisRecepcionesDetalles
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarSolicitudesPagosDetallesPopUppMisRecepcionesDetalles"]; }
            set { Session[this.MiSessionPagina + "ImportarSolicitudesPagosDetallesPopUppMisRecepcionesDetalles"] = value; }
        }

        private CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "ImportarSolicitudesPagosDetallesPopUppMiProveedor"]; }
            set { Session[this.MiSessionPagina + "ImportarSolicitudesPagosDetallesPopUppMiProveedor"] = value; }
        }

        public delegate void ImportarSolicitudesPagosDetallesBuscarEventHandler(List<CmpInformesRecepcionesDetalles> e);
        public event ImportarSolicitudesPagosDetallesBuscarEventHandler ControlAceptar;

        public delegate void ImportarSolicitudesPagosDetallesBuscarAcopioEventHandler(CapSolicitudPago e);
        public event ImportarSolicitudesPagosDetallesBuscarAcopioEventHandler ControlAceptarAcopio;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroFactura, this.btnBuscar);
            }
        }

        public void IniciarControl(CapProveedores pFiltro)
        {
            this.MiProveedor= pFiltro;
            this.txtNumeroFactura.Text = string.Empty;
            this.CargarLista();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarSolicitudesPagosDetalles();", true);
        }
        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
        }

        private void CargarLista()
        {
            CmpInformesRecepciones filtro = new CmpInformesRecepciones();
            filtro.Proveedor = this.MiProveedor;
            filtro.NumeroFactura = this.txtNumeroFactura.Text.Trim();
            filtro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            this.MisRecepcionesAcopios = ComprasF.InformesRecepcionesObtenerAcopiosPendientesRecibirFiltroPorProveedor(filtro);
            this.phAcopios.Visible = this.MisRecepcionesAcopios.Rows.Count > 0;
            this.gvAcopios.DataSource = this.MisRecepcionesAcopios;
            this.gvAcopios.DataBind();

            this.MisRecepcionesDetalles = ComprasF.InformesRecepcionesObtenerDetallesPendientesRecibirFiltroPorProveedor(filtro);
            this.phDatos.Visible = this.MisRecepcionesDetalles.Rows.Count > 0;
            this.gvDatos.DataSource = this.MisRecepcionesDetalles;
            this.gvDatos.DataBind();

            this.btnAceptar.Visible = this.MisRecepcionesDetalles.Rows.Count > 0 || this.MisRecepcionesAcopios.Rows.Count > 0;
            this.btnCancelar.Visible = this.MisRecepcionesDetalles.Rows.Count > 0 || this.MisRecepcionesAcopios.Rows.Count > 0;
        }

        protected void btnAceptarAcopio_Click(object sender, EventArgs e)
        {
            CapSolicitudPago item = new CapSolicitudPago();
            foreach (GridViewRow fila in this.gvAcopios.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    DataRow row = this.MisRecepcionesAcopios.AsEnumerable().Where(dr => dr.Field<int>("IdSolicitudPago") == Convert.ToInt32(this.gvAcopios.DataKeys[fila.RowIndex].Value)).FirstOrDefault();
                    CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                    if (chkIncluir.Checked)
                    {
                        item.IdSolicitudPago = (int)row["IdSolicitudPago"];
                        item.Filtro = row["TipoNumeroFactura"].ToString();
                        item.ImporteSinIVA = (decimal)row["ImporteSinIVA"];
                        item.ImporteParcial = (decimal)row["ImporteRecibido"];
                        break;
                    }
                }
            }

            if (this.ControlAceptarAcopio != null)
            {
                this.ControlAceptarAcopio(item);
            }
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
                        DataRow row = this.MisRecepcionesDetalles.AsEnumerable().Where(dr=> dr.Field<int>("IdSolicitudPagoDetalle")== Convert.ToInt32(this.gvDatos.DataKeys[fila.RowIndex].Value)).FirstOrDefault();
                        CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                        if (chkIncluir.Checked)
                        {
                            item = new CmpInformesRecepcionesDetalles();
                            item.IdSolicitudPagoDetalle = (int)row["IdSolicitudPagoDetalle"];
                            item.Producto.IdProducto = (int)row["IdProducto"];
                            item.Producto.Descripcion = row["Descripcion"].ToString();
                            item.Producto.Familia.Stockeable = (bool)row["Stockeable"];
                            item.TipoNumeroFactura = row["TipoNumeroFactura"].ToString();
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