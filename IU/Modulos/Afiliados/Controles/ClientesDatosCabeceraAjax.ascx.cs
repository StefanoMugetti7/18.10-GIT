using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Afiliados;
using Comunes.Entidades;
using Facturas.Entidades;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class ClientesDatosCabeceraAjax : ControlesSeguros
    {
        public AfiAfiliados MiAfiliado
        {
            get { return (AfiAfiliados)Session[this.MiSessionPagina + "ClientesDatosCabeceraAjaxMiAfiliado"]; }
            set { Session[this.MiSessionPagina + "ClientesDatosCabeceraAjaxMiAfiliado"] = value; }
        }

        public delegate void ClientesDatosCabeceraAjaxEventHandler(AfiAfiliados e);
        public event ClientesDatosCabeceraAjaxEventHandler BuscarCliente;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                
            }
        }

        public void IniciarControl(AfiAfiliados pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;

            switch (pGestion)
            {
                case Gestion.Consultar:
                    if (pParametro.IdAfiliado > 0)
                    {
                        ddlNumeroSocio.Enabled = false;
                        hdfIdAfiliado.Value = pParametro.IdAfiliado.ToString();
                        button_Click(button, EventArgs.Empty);
                    }
                    break;
                case Gestion.Agregar:
                    if (paginaSegura.MenuPadre == EnumMenues.Afiliados)
                    {
                        this.ddlNumeroSocio.Enabled = false;
                    }
                    if (pParametro.IdAfiliado>0)
                    {
                        hdfIdAfiliado.Value = pParametro.IdAfiliado.ToString();
                        button_Click(button, EventArgs.Empty);
                        //this.MisParametrosUrl["IdAfiliado"] = pParametro.IdAfiliado;
                        //MapearObjetoAControlesAfiliado(pParametro);
                    }
                    break;
                case Gestion.Modificar:
                    if (pParametro.IdAfiliado > 0)
                    {
                        ddlNumeroSocio.Enabled = false;
                        hdfIdAfiliado.Value = pParametro.IdAfiliado.ToString();
                        button_Click(button, EventArgs.Empty);
                    }
                    break;
                case Gestion.Anular:
                    if (pParametro.IdAfiliado > 0)
                    {
                        ddlNumeroSocio.Enabled = false;
                        hdfIdAfiliado.Value = pParametro.IdAfiliado.ToString();
                        button_Click(button, EventArgs.Empty);
                    }
                    break;
                case Gestion.AnularConfirmar:
                    if (pParametro.IdAfiliado > 0)
                    {
                        ddlNumeroSocio.Enabled = false;
                        hdfIdAfiliado.Value = pParametro.IdAfiliado.ToString();
                        button_Click(button, EventArgs.Empty);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void button_Click(object sender, EventArgs e)
        {
            string txtNumeroSocio = this.hdfIdAfiliado.Value; 
            MiAfiliado = new AfiAfiliados();
            MiAfiliado.IdAfiliado = txtNumeroSocio == string.Empty ? 0 : Convert.ToInt32(txtNumeroSocio);
            MiAfiliado = AfiliadosF.AfiliadosObtenerDatos(MiAfiliado);
            if (MiAfiliado.IdAfiliado != 0)
            {
                this.MapearObjetoAControlesAfiliado(MiAfiliado);
            }
            else
            {
                this.txtCUIT.Text = string.Empty;
                this.txtEstado.Text = string.Empty;
                this.txtCondicionFiscal.Text = string.Empty;
                MiAfiliado.CodigoMensaje = "El cliente no existe";
                //this.UpdatePanel2.Update();
                this.MostrarMensaje(MiAfiliado.CodigoMensaje, true);
            }
            BuscarCliente?.Invoke(MiAfiliado);
        }

        private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        {
            this.ddlNumeroSocio.Items.Add(new ListItem(pAfiliado.DescripcionAfiliado, pAfiliado.IdAfiliado.ToString()));
            this.ddlNumeroSocio.SelectedValue = pAfiliado.IdAfiliado.ToString();

            this.lblCUIT.Text = pAfiliado.TipoDocumento.TipoDocumento;
            this.txtCUIT.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtDetalle.Text = pAfiliado.Detalle;
            this.txtEstado.Text = pAfiliado.Estado.Descripcion;
            this.txtCondicionFiscal.Text = pAfiliado.CondicionFiscal.Descripcion;

            if (this.GestionControl == Gestion.Agregar)
            {
                //BuscarCliente?.Invoke(MiAfiliado);
                //this.MiFactura.ComprobanteExento = this.MiFactura.Afiliado.ComprobanteExento;
                //if (this.FacturasBuscarCliente != null)
                //{
                //    this.FacturasBuscarCliente(MiFactura.Afiliado);
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