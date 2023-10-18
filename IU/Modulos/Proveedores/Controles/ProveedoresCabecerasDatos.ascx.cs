using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using IU.Modulos.Compras.Controles;
using Proveedores;
using Proveedores.Entidades;

namespace IU.Modulos.Proveedores.Controles
{
    public partial class ProveedoresCabecerasDatos : ControlesSeguros
    {
        
        public CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "ProveedorCabeceraDatosMiProveedor"]; }
            set { Session[this.MiSessionPagina + "ProveedorCabeceraDatosMiProveedor"] = value; }
        }

        public delegate void ProveedoresDatosCabeceraAjaxEventHandler(CapProveedores e);
        public event ProveedoresDatosCabeceraAjaxEventHandler BuscarProveedor;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
            


            }
        }
        
        public void IniciarControl(CapProveedores pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;

            switch (pGestion)
            {
                case Gestion.Consultar:
                    if (pParametro.IdProveedor > 0)
                    {
                        ddlNumeroProveedor.Enabled = false;
                        hdfIdProveedor.Value = pParametro.IdProveedor.ToString();
                        button_Click(button, EventArgs.Empty);
                    }
                    break;
                case Gestion.Agregar:
                    if (paginaSegura.MenuPadre == EnumMenues.Afiliados)
                    {
                        this.ddlNumeroProveedor.Enabled = false;
                    }
                    if (pParametro.IdProveedor > 0)
                    {
                        hdfIdProveedor.Value = pParametro.IdProveedor.ToString();
                        hdfNumeroProveedor.Value = pParametro.RazonSocial.ToString();
                        button_Click(button, EventArgs.Empty);
                        //this.MisParametrosUrl["IdAfiliado"] = pParametro.IdAfiliado;
                        //MapearObjetoAControlesProveedores(pParametro);
                    }
                    break;
                case Gestion.Modificar:
                    if (pParametro.IdProveedor > 0)
                    {
                        ddlNumeroProveedor.Enabled = false;
                        hdfIdProveedor.Value = pParametro.IdProveedor.ToString();
                        hdfNumeroProveedor.Value = pParametro.RazonSocial.ToString();
                        button_Click(button, EventArgs.Empty);
                    }
                    break;
                case Gestion.Anular:
                    if (pParametro.IdProveedor > 0)
                    {
                        ddlNumeroProveedor.Enabled = false;
                        hdfIdProveedor.Value = pParametro.IdProveedor.ToString();
                        hdfNumeroProveedor.Value = pParametro.RazonSocial.ToString();
                        button_Click(button, EventArgs.Empty);
                    }
                    break;
                case Gestion.Autorizar:
                    if (pParametro.IdProveedor > 0)
                    {
                        ddlNumeroProveedor.Enabled = false;
                        hdfIdProveedor.Value = pParametro.IdProveedor.ToString();
                        hdfNumeroProveedor.Value = pParametro.RazonSocial.ToString();
                        button_Click(button, EventArgs.Empty);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void button_Click(object sender, EventArgs e)
        {


            string txtNumeroProveedor = this.hdfIdProveedor.Value;
            MiProveedor = new CapProveedores();
            MiProveedor.IdProveedor = txtNumeroProveedor == string.Empty ? 0 : Convert.ToInt32(txtNumeroProveedor);
            MiProveedor.RazonSocial = this.hdfNumeroProveedor.Value;
            MiProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(MiProveedor);
         
            if (MiProveedor.IdProveedor != 0)
            {
                this.MapearObjetoAControlesProveedores(MiProveedor);
               
            }
            else
            {
                this.txtBeneficiario.Text = string.Empty;
                this.txtCUIT.Text = string.Empty;
                this.txtEstado.Text = string.Empty;
                this.txtCondicionFiscal.Text = string.Empty;
                MiProveedor.CodigoMensaje = "El Proveedor no existe";
                //this.UpdatePanel2.Update();
                this.MostrarMensaje(MiProveedor.CodigoMensaje, true);
            }
            BuscarProveedor?.Invoke(MiProveedor);
        }

        private void MapearObjetoAControlesProveedores(CapProveedores pProveedor)
        {
           
            this.ddlNumeroProveedor.Items.Add(new ListItem(pProveedor.RazonSocial, pProveedor.IdProveedor.ToString()));
            this.ddlNumeroProveedor.SelectedValue = pProveedor.IdProveedor.ToString();

            string proovedor = hdfNumeroProveedor.Value.ToString() ;
            this.txtCUIT.Text = pProveedor.CUIT.ToString();
            this.txtEstado.Text = pProveedor.Estado.Descripcion;
            this.txtCondicionFiscal.Text = pProveedor.CondicionFiscal.Descripcion;
            txtBeneficiario.Text = pProveedor.BeneficiarioDelCheque.ToString();
            //if(!string.IsNullOrEmpty(proovedor))
            //ddlNumeroProveedor.SelectedValue = proovedor;

            if (this.GestionControl == Gestion.Agregar)
            {
                //BuscarProveedor?.Invoke(MiAfiliado);
                //this.MiFactura.ComprobanteExento = this.MiFactura.Afiliado.ComprobanteExento;
                //if (this.FacturasBuscarProveedor != null)
                //{
                //    this.FacturasBuscarProveedor(MiFactura.Afiliado);
                //    this.UpdatePanel2.Visible = false;
                //}

                //if (!this.MiFactura.IdRefTabla.HasValue || this.MiFactura.IdRefTabla == 0)
                //{
                //    if (this.MiFactura.FacturasDetalles.Exists(x => x.ListaPrecioDetalle.Producto.IdProducto != 0))
                //    {
                //        this.MiFactura.FacturasDetalles.Clear();
                //        this.IniciarGrilla();
                //        this.CalcularTotal();
                //        this.upItems.Update();
                //    }
                //}
            }
        }
    }
}
