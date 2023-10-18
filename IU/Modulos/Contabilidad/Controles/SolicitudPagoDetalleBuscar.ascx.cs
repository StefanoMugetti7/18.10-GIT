using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class SolicitudPagoDetalle : ControlesSeguros
    {
        private CapSolicitudPagoDetalles MiSolicitudPagoDetalle
        {
            get { return (CapSolicitudPagoDetalles)Session[this.MiSessionPagina + "SolicitudPagoDetalleBuscarMiSolicitudPagoDetalle"]; }
            set { Session[this.MiSessionPagina + "SolicitudPagoDetalleBuscarMiSolicitudPagoDetalle"] = value; }
        }

        public delegate void SolicitudPagoDetalleBuscarEventHandler(CapSolicitudPagoDetalles e);
        public event SolicitudPagoDetalleBuscarEventHandler SolicitudPagoDetalleBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.puSolicitudPagoDetalle.SolicitudPagoDetalleBuscarSeleccionarPopUp += new SolicitudPagoDetalleBuscarPopUp.SolicitudPagoDetalleBuscarPopUpEventHandler(puSolicitudPagoDetalle_SolicitudPagoDetalleBuscarSeleccionarPopUp);
        }

        protected void puSolicitudPagoDetalle_SolicitudPagoDetalleBuscarSeleccionarPopUp(CapSolicitudPagoDetalles e)
        {
            this.MapearObjetoControles(e);
            if (this.SolicitudPagoDetalleBuscarSeleccionar != null)
                this.SolicitudPagoDetalleBuscarSeleccionar(e);
        }

        protected void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            List<CapSolicitudPagoDetalles> solicitudPagoDetalleBuscar = new List<CapSolicitudPagoDetalles>();
            CapSolicitudPagoDetalles solicitudPagoDetalle = new CapSolicitudPagoDetalles();
            solicitudPagoDetalle.Descripcion = this.txtDescripcion.Text;
            solicitudPagoDetalleBuscar = null;// CuentasPagarF.SolicitudPagoDetalleObtenerListaFiltro(solicitudPagoDetalle);
            if (solicitudPagoDetalleBuscar.Count == 1)
            {
                this.txtDescripcion.Text = solicitudPagoDetalleBuscar[0].Descripcion;
                this.hfIdSolicitudPagoDetalle.Value = solicitudPagoDetalleBuscar[0].IdSolicitudPagoDetalle.ToString();
                //Devuele la CuentaContable
                if (this.SolicitudPagoDetalleBuscarSeleccionar != null)
                    this.SolicitudPagoDetalleBuscarSeleccionar(solicitudPagoDetalleBuscar[0]);
            }
            else if (solicitudPagoDetalleBuscar.Count > 1)
            {
                //this.puSolicitudPagoDetalle.IniciarControl(true, solicitudPagoDetalleBuscar);
            }
            else
            {
                this.puSolicitudPagoDetalle.IniciarControl(true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.puSolicitudPagoDetalle.IniciarControl(true);
        }

        public void Enable(bool habilitado)
        {
            this.txtDescripcion.Enabled = habilitado;
            this.btnBuscar.Enabled = habilitado;
        }

        public void MapearObjetoControles(CapSolicitudPagoDetalles pSolicitudPagoDetalle)
        {
            this.txtDescripcion.Text = pSolicitudPagoDetalle.Descripcion;
            this.hfIdSolicitudPagoDetalle.Value = pSolicitudPagoDetalle.IdSolicitudPagoDetalle.ToString();
        }
    }
}