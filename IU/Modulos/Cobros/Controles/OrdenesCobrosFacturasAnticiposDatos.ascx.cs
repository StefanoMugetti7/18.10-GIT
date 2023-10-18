using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cobros.Entidades;
using Comunes.Entidades;
using SKP.ASP.Controls;
using Cobros;
using Afiliados.Entidades;
using Afiliados;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Globalization;
using CuentasPagar.Entidades;
using Tesorerias.Entidades;
using Tesorerias;

namespace IU.Modulos.Cobros.Controles
{
    public partial class OrdenesCobrosFacturasAnticiposDatos : ControlesSeguros
    {
        private CobOrdenesCobros MiOrdenCobro
        {
            get { return (CobOrdenesCobros)Session["OrdenesDeCobroDatosMiOrdenCobro"]; }
            set { Session["OrdenesDeCobroDatosMiOrdenCobro"] = value; }
        }

        public delegate void OrdenesDeCobroDatosAceptarEventHandler(CobOrdenesCobros e);
        public event OrdenesDeCobroDatosAceptarEventHandler OrdenesDeCobroDatosAceptar;
        public delegate void OrdenesDeCobroDatosCancelarEventHandler();
        public event OrdenesDeCobroDatosCancelarEventHandler OrdenesDeCobroDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrBuscarClientePopUp.AfiliadosBuscarSeleccionar += new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar);
        }

        protected void ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        {
            this.MiOrdenCobro.Afiliado = e;
            this.txtCodigoSocio.Text = e.IdAfiliado.ToString();
            this.txtRazonSocial.Text = e.ApellidoNombre;
            this.txtCuil.Text = e.CUIL.ToString();
            this.upEntidades.Update();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            TESCajasMovimientos movimiento = new TESCajasMovimientos();
            PaginaCajas nueva = new PaginaCajas();

            TESFiltroMovimientosPendientes movFiltro = new TESFiltroMovimientosPendientes();
            movFiltro.IdFilial = this.MiOrdenCobro.Filial.IdFilial;
            movFiltro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            movFiltro.IdTipoOperacion = this.MiOrdenCobro.TipoOperacion.IdTipoOperacion;
            movFiltro.IdRefTipoOperacion = this.MiOrdenCobro.IdOrdenCobro;
            movimiento = TesoreriasF.CajasObtenerMovimientosPendientesFiltro(movFiltro);

            nueva.MiCajaMovimientoPendiente = movimiento;

            string url = "~/Modulos/Tesoreria/CajasMovimientosConfirmar.aspx";
            this.Response.Redirect(url, true);
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            this.ctrBuscarClientePopUp.IniciarControl(true);
        }

        public void IniciarControl(CobOrdenesCobros pOrdenCobro, Gestion pGestion)
        {
            this.MiOrdenCobro = pOrdenCobro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtFecha.Text = DateTime.Now.ToShortDateString();
                    this.btnBuscarSocio.Visible = true;
                    this.txtCodigoSocio.Enabled = true;
                    this.ddlFilialCobro.Enabled = true;
                    break;
                case Gestion.Anular:
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerFacturasDatosCompletos(this.MiOrdenCobro);
                    this.MapearObjetoControles(this.MiOrdenCobro);
                    break;
                case Gestion.Consultar:
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerFacturasDatosCompletos(this.MiOrdenCobro);
                    this.MapearObjetoControles(this.MiOrdenCobro);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        protected void txtCodigoSocio_TextChanged(object sender, EventArgs e)
        {
            AfiAfiliados afiliado = new AfiAfiliados();
            afiliado.IdAfiliado = this.txtCodigoSocio.Text ==string.Empty ? 0 : Convert.ToInt32( this.txtCodigoSocio.Text);
            afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(afiliado);
            if (afiliado.IdAfiliado > 0)
                this.ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(afiliado);
            else
                this.ctrBuscarClientePopUp.IniciarControl(true);
        }

        protected void btncancelar_Click(object sender, EventArgs e)
        {
            if (this.OrdenesDeCobroDatosCancelar != null)
                this.OrdenesDeCobroDatosCancelar();
        }

        protected void btngrabar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("OrdenesDeCobrosDatos");
            if (!this.Page.IsValid)
                return;
            bool guardo = true;
            this.MapearControlesAObjeto();
            
            this.MiOrdenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiOrdenCobro.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenCobro.Moneda.IdMoneda=(int)EnumTGEMonedas.PesosArgentinos;
                    this.MiOrdenCobro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasAdelantos;
                    this.MiOrdenCobro.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    guardo = CobrosF.OrdenesCobrosAgregarAnticipos(this.MiOrdenCobro);
                    break;
                case Gestion.Anular:
                    //this.MapearControlesObjeto(this.MiOrdenCobro);
                    this.MiOrdenCobro.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenCobro.Estado.IdEstado = (int)EstadosOrdenesCobro.Baja;
                    guardo = CobrosF.OrdenesCobrosAnular(this.MiOrdenCobro);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        this.btnAceptar.Visible = false;
                        this.MiOrdenCobro.CodigoMensaje = "OrdenCobroGenerada";
                        string mensaje = string.Concat(this.ObtenerMensajeSistema(this.MiOrdenCobro.CodigoMensaje), this.MiOrdenCobro.IdOrdenCobro.ToString());
                        this.MiOrdenCobro.CodigoMensaje = "OrdenCobroConfirmarMov";
                        this.popUpMensajes.MostrarMensaje(string.Concat(mensaje, this.ObtenerMensajeSistema(this.MiOrdenCobro.CodigoMensaje)), true);
                        break;
                    case Gestion.Anular:
                        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiOrdenCobro.CodigoMensaje));
                        break;
                }
            }
            else
            {
                this.MostrarMensaje(this.MiOrdenCobro.CodigoMensaje, true, this.MiOrdenCobro.CodigoMensajeArgs);
            }
        }


        private void CargarCombos()
        {
            this.ddlFilialCobro.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilialCobro.DataValueField = "IdFilial";
            this.ddlFilialCobro.DataTextField = "Filial";
            this.ddlFilialCobro.DataBind();

        }

        private void MapearObjetoControles(CobOrdenesCobros pParametro)
        {
            this.txtCodigoSocio.Text = pParametro.Afiliado.NumeroSocio;
            this.txtRazonSocial.Text = pParametro.Afiliado.ApellidoNombre;
            this.txtCuil.Text = pParametro.Afiliado.CUIL.ToString();

            this.txtOrdenCobro.Text = pParametro.IdOrdenCobro.ToString();
            this.txtFecha.Text = pParametro.FechaEmision.ToShortDateString();

            this.ddlFilialCobro.SelectedValue = pParametro.FilialCobro.IdFilialCobro.ToString();
            this.ctrComentarios.IniciarControl(this.MiOrdenCobro, GestionControl);          
            this.txtTotalCobrar.Text = this.MiOrdenCobro.ImporteTotal.ToString("C2");
        }

        private void MapearControlesAObjeto()
        {
            //this.MiOrdenCobro.Detalle = this.txtDetalle.Text;
            this.MiOrdenCobro.FechaEmision = Convert.ToDateTime(this.txtFecha.Text);
            this.MiOrdenCobro.FilialCobro.IdFilialCobro = Convert.ToInt32(this.ddlFilialCobro.SelectedValue);
            this.MiOrdenCobro.FilialCobro.Filial = this.ddlFilialCobro.SelectedItem.Text;
            this.MiOrdenCobro.ImporteSubTotal = this.txtTotalCobrar.Decimal;
            this.MiOrdenCobro.ImporteTotal = this.txtTotalCobrar.Decimal;
        }
    }
}