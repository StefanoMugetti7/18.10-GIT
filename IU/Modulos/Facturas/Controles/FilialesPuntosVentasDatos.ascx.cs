using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Facturas;

namespace IU.Modulos.Facturas.Controles
{
    public partial class FilialesPuntosVentasDatos : ControlesSeguros
    {
        private VTAFilialesPuntosVentas MiFilialPuntoVenta
        {
            get
            {
                return (Session[this.MiSessionPagina + "FilialesPuntosVentasDatosMiFilialPuntoVenta"] == null ?
                    (VTAFilialesPuntosVentas)(Session[this.MiSessionPagina + "FilialesPuntosVentasDatosMiFilialPuntoVenta"] = new VTAFilialesPuntosVentas()) : (VTAFilialesPuntosVentas)Session[this.MiSessionPagina + "FilialesPuntosVentasDatosMiFilialPuntoVenta"]);
            }
            set { Session[this.MiSessionPagina + "FilialesPuntosVentasDatosMiFilialPuntoVenta"] = value; }
        }

        public delegate void FilialesPuntosVentasDatosAceptarEventHandler(object sender, VTAFilialesPuntosVentas e);
        public event FilialesPuntosVentasDatosAceptarEventHandler FilialPuntoVentaDatosAceptar;

        //public delegate void FilialesPuntosVentasDatosCancelarEventHandler();
        //public event FilialesPuntosVentasDatosCancelarEventHandler FilialPuntoVentaDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
            }
        }
        public void IniciarControl(VTAFilialesPuntosVentas pParametro, Gestion pGestion)
        {
            this.MiFilialPuntoVenta = pParametro;
            this.GestionControl = pGestion;

            List<TGEListasValoresSistemasDetalles> tiposPuntosVentas = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPuntosVentas);
            this.ddlTiposPuntosVentas.DataSource = tiposPuntosVentas;
            this.ddlTiposPuntosVentas.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTiposPuntosVentas.DataTextField = "Descripcion";
            this.ddlTiposPuntosVentas.DataBind();
            if (this.ddlTiposPuntosVentas.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposPuntosVentas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFilial.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTiposFacturas.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposFacturas);
            this.ddlTiposFacturas.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTiposFacturas.DataTextField = "Descripcion";
            this.ddlTiposFacturas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposFacturas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();

            this.btnAceptar.Visible = true;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.HabilitarDeshabilitarControles(true);
                    break;
                case Gestion.Consultar:
                    this.MiFilialPuntoVenta = FacturasF.VTAFilialesPuntosVentasObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiFilialPuntoVenta);
                    this.HabilitarDeshabilitarControles(false);
                    this.btnAceptar.Visible = false;
                    break;
                case Gestion.Modificar:
                    this.MiFilialPuntoVenta = FacturasF.VTAFilialesPuntosVentasObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiFilialPuntoVenta);
                    this.HabilitarDeshabilitarControles(false);
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalFilialesPuntosVentas();", true);
        }
        private void HabilitarDeshabilitarControles(bool estado)
        {
            this.txtNumeroFactura.Enabled = estado;
            this.txtUltimoNumeroFactura.Enabled = estado;
            this.ddlFilial.Enabled = estado;
            this.ddlTiposFacturas.Enabled = estado;
            this.ddlTiposPuntosVentas.Enabled = estado;
        }
        private void MapearObjetoAControles(VTAFilialesPuntosVentas pParametro)
        {
            this.ddlTiposPuntosVentas.SelectedValue = pParametro.TipoPuntoVenta.IdTipoPuntoVenta.ToString();
            this.txtNumeroFactura.Text = pParametro.AfipPuntoVenta.ToString();
            this.ddlFilial.SelectedValue = pParametro.IdFilial.ToString();
            this.ddlTiposFacturas.SelectedValue = pParametro.IdTipoFactura.ToString();
            this.txtUltimoNumeroFactura.Text = pParametro.UltimoNumeroFacturaAnterior.ToString();
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
        }
        private void MapearControlesAObjeto(VTAFilialesPuntosVentas pParametro)
        {
            pParametro.TipoPuntoVenta.IdTipoPuntoVenta = Convert.ToInt32(this.ddlTiposPuntosVentas.SelectedValue);
            pParametro.TipoPuntoVenta.Descripcion = this.ddlTiposPuntosVentas.SelectedItem.Text;
            pParametro.AfipPuntoVenta = Convert.ToInt32( this.txtNumeroFactura.Text);
            pParametro.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
            pParametro.IdTipoFactura = Convert.ToInt32(this.ddlTiposFacturas.SelectedValue);
            pParametro.UltimoNumeroFacturaAnterior = this.txtUltimoNumeroFactura.Text == string.Empty ? 0 : Convert.ToInt32(this.txtUltimoNumeroFactura.Text);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("Aceptar");
            if (!this.Page.IsValid)
                return;

            bool guardo = false;
            this.MapearControlesAObjeto(this.MiFilialPuntoVenta);
            this.MiFilialPuntoVenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = FacturasF.VTAFilialesPuntosVentasAgregar(this.MiFilialPuntoVenta);
                    break;
                case Gestion.Modificar:
                    guardo = FacturasF.VTAFilialesPuntosVentasModificar(this.MiFilialPuntoVenta);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                if (this.FilialPuntoVentaDatosAceptar != null)
                    this.FilialPuntoVentaDatosAceptar(null, this.MiFilialPuntoVenta);

                this.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFilialPuntoVenta.CodigoMensaje), false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalFilialesPuntosVentas", "HideModalFilialesPuntosVentas();", true);
            }
            else
            {
                this.MostrarMensaje(this.MiFilialPuntoVenta.CodigoMensaje, true, this.MiFilialPuntoVenta.CodigoMensajeArgs);
            }
        }
    }
}