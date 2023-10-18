using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Facturas;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Drawing;

namespace IU.Modulos.Facturas.FacturaElectronica
{
    public partial class FacturaElectronicaValidar : PaginaSegura
    {

        private List<VTATiposPuntosVentas> MisTiposPuntosVentas
        {
            get { return (List<VTATiposPuntosVentas>)Session[this.MiSessionPagina + "FacturaElectronicaValidarMisTiposPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "FacturaElectronicaValidarMisTiposPuntosVentas"] = value; }
        }

        private List<VTAFilialesPuntosVentas> MisFilialesPuntosVentas
        {
            get { return (List<VTAFilialesPuntosVentas>)Session[this.MiSessionPagina + "FacturaElectronicaValidarMisFilialesPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "FacturaElectronicaValidarMisFilialesPuntosVentas"] = value; }
        }

        private List<TGETiposFacturas> MisTiposFacturas
        {
            get { return (List<TGETiposFacturas>)Session[this.MiSessionPagina + "FacturaElectronicaValidarMisTiposFacturas"]; }
            set { Session[this.MiSessionPagina + "FacturaElectronicaValidarMisTiposFacturas"] = value; }
        }

        private VTAFacturas MiFactura
        {
            get { return (VTAFacturas)Session[this.MiSessionPagina + "FacturaElectronicaValidarMiFactura"]; }
            set { Session[this.MiSessionPagina + "FacturaElectronicaValidarMiFactura"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                this.MiFactura = new VTAFacturas();
                this.MiFactura.Filial = this.UsuarioActivo.FilialPredeterminada;
                
                //if (this.MisTiposPuntosVentas.Exists(x => x.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea))
                //{
                //    this.MisTiposPuntosVentas.Remove(this.MisTiposPuntosVentas.Find(x => x.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea));
                //    this.MisTiposPuntosVentas = AyudaProgramacion.AcomodarIndices<VTATiposPuntosVentas>(this.MisTiposPuntosVentas);
                //}
                this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
                this.ddlFiliales.DataValueField = "IdFilial";
                this.ddlFiliales.DataTextField = "Filial";
                this.ddlFiliales.DataBind();
                this.ddlFiliales_SelectedIndexChanged(null, EventArgs.Empty);

                AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPuntoVenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.btnValidarServicio_Click(null, EventArgs.Empty);
            }
        }

        protected void ddlFiliales_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MiFactura.Filial = this.UsuarioActivo.Filiales[this.ddlFiliales.SelectedIndex];

            VTAFilialesPuntosVentas filtro = new VTAFilialesPuntosVentas();
            filtro.IdFilial = this.MiFactura.Filial.IdFilial;
            this.MisTiposPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerPorFilial(filtro);
            this.MisTiposPuntosVentas = this.MisTiposPuntosVentas.Where(x => x.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica).ToList();
            this.MisTiposPuntosVentas = AyudaProgramacion.AcomodarIndices<VTATiposPuntosVentas>(this.MisTiposPuntosVentas);

            this.ddlFilialPuntoVenta.ClearSelection();
            this.ddlFilialPuntoVenta.SelectedIndex = -1;
            this.ddlFilialPuntoVenta.DataSource = this.MisTiposPuntosVentas;
            this.ddlFilialPuntoVenta.DataValueField = "IdTipoPuntoVenta";
            this.ddlFilialPuntoVenta.DataTextField = "Descripcion";
            this.ddlFilialPuntoVenta.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPuntoVenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlFilialPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblProximaFactura2.Text = string.Empty;
            if (!string.IsNullOrEmpty(this.ddlFilialPuntoVenta.SelectedValue))
            {
                this.MiFactura.FilialPuntoVenta.TipoPuntoVenta = this.MisTiposPuntosVentas[this.ddlFilialPuntoVenta.SelectedIndex];

                //Cargo los comprobantes habilitados para el Cliente
                this.MisTiposFacturas = FacturasF.TiposFacturasActivosPorIdTipoFactura(new TGETiposFacturas()); //FacturasF.TiposFacturasSeleccionarPorCondicionFiscal(this.MiFactura);
                this.ddlTipoFactura.Items.Clear();
                this.ddlTipoFactura.SelectedValue = null;
                this.ddlTipoFactura.DataSource = this.MisTiposFacturas;
                this.ddlTipoFactura.DataValueField = "IdTipoFactura";
                this.ddlTipoFactura.DataTextField = "Descripcion";
                this.ddlTipoFactura.DataBind();
                if (this.ddlTipoFactura.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.ddlTipoFactura_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                this.ddlTipoFactura.Items.Clear();
                this.ddlTipoFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MiFactura.FilialPuntoVenta = new VTAFilialesPuntosVentas();
                this.MiFactura.PrefijoNumeroFactura = string.Empty;
                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
            }
        }

        protected void ddlTipoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblProximaFactura2.Text = string.Empty;
            if (!string.IsNullOrEmpty(this.ddlTipoFactura.SelectedValue))
            {
                this.MiFactura.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
                this.MiFactura.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
                this.MiFactura.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;

                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                this.MiFactura.FilialPuntoVenta.IdFilial = this.MiFactura.Filial.IdFilial;
                this.MiFactura.FilialPuntoVenta.IdTipoFactura = this.MiFactura.TipoFactura.IdTipoFactura;
                this.MisFilialesPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerListaFiltro(this.MiFactura.FilialPuntoVenta);
                this.ddlPrefijoNumeroFactura.DataSource = this.MisFilialesPuntosVentas;
                this.ddlPrefijoNumeroFactura.DataValueField = "AfipPuntoVenta";
                this.ddlPrefijoNumeroFactura.DataTextField = "AfipPuntoVentaNumero";
                this.ddlPrefijoNumeroFactura.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
            }
            else
            {
                this.MiFactura.TipoFactura = new TGETiposFacturas();
                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
            }
        }

        protected void btnValidarServicio_Click(object sender, EventArgs e)
        {
            VTAFacturas factura = new VTAFacturas();
            factura.AppPath = this.Request.PhysicalApplicationPath;
            //factura.PrefijoNumeroFactura = this.UsuarioActivo.FilialPredeterminada.AfipPuntoVenta.ToString().PadLeft(4, '0');

            if (FacturasF.FacturaElectronicaValidarServicio(factura))
            {
                this.lblResultado2.Text = string.Concat("Servicio Activo. Cantidad Maxima de Comprobantes a Validar ", factura.IdFactura.ToString());
                this.lblResultado2.ForeColor = Color.Black;
                //this.lblErrores.Text = string.Empty;
                this.Mensaje.Text = string.Empty;
                this.btnValidarProximoNuero.Enabled = true;
            }
            else
            {
                this.lblResultado2.Text = string.Concat("Error al contactar con el Servicio de AFIP. ", string.Format(this.ObtenerMensajeSistema(factura.CodigoMensaje), factura.CodigoMensajeArgs.ToArray()));
                this.lblResultado2.ForeColor = Color.Red;
                this.lblProximaFactura2.Text = string.Empty;
                //this.Mensaje.Text = string.Format(this.ObtenerMensajeSistema(factura.CodigoMensaje), factura.CodigoMensajeArgs.ToArray());
            }
        }

        protected void btnValidarProximoNuero_Click(object sender, EventArgs e)
        {
            this.MiFactura.FilialPuntoVenta.AfipPuntoVenta = this.ddlPrefijoNumeroFactura.SelectedValue==string.Empty ? 0 : Convert.ToInt32(this.ddlPrefijoNumeroFactura.SelectedValue);
            this.MiFactura.PrefijoNumeroFactura = this.MiFactura.FilialPuntoVenta.AfipPuntoVentaNumero;

            if (FacturasF.FacturaElectronicaValidarProximoNumeroComprobante(this.MiFactura))
            {
                this.lblProximaFactura2.Text = this.MiFactura.NumeroFacturaCompleto;
            }
            else 
            {
                this.lblProximaFactura2.Text = "Error al Validar el Proximo Numero de Factura";
                this.Mensaje.Text = string.Format(this.ObtenerMensajeSistema(this.MiFactura.CodigoMensaje), this.MiFactura.CodigoMensajeArgs.ToArray());
            }
            
        }

        protected void btnConsultarComprobante_Click(object sender, EventArgs e)
        {
            this.MiFactura.FilialPuntoVenta.AfipPuntoVenta = this.ddlPrefijoNumeroFactura.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPrefijoNumeroFactura.SelectedValue);
            this.MiFactura.PrefijoNumeroFactura = this.MiFactura.FilialPuntoVenta.AfipPuntoVentaNumero;
            this.MiFactura.NumeroFactura = this.txtNumeroComprobante.Text.Trim();
            if (FacturasF.FacturaElectronicaConsultarDatosAutorizacion2(this.MiFactura))
            {
                List<VTAFacturas> lista = new List<VTAFacturas>();
                lista.Add(this.MiFactura);
                this.gvDatos.DataSource = lista;
                gvDatos.DataBind();
            }
            else
            {
                this.lblProximaFactura2.Text = "Error al Validar los datos de la Factura";
                this.Mensaje.Text = string.Format(this.ObtenerMensajeSistema(this.MiFactura.CodigoMensaje), this.MiFactura.CodigoMensajeArgs.ToArray());
            }
        }
    }
}