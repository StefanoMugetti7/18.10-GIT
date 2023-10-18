using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Tesorerias.Entidades;
using Tesorerias;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasAbrir : PaginaSegura
    {
        private bool MiReabrirCaja
        {
            get { return (bool)Session[this.MiSessionPagina + "TesoreriaAbrirMiReabrirCaja"]; }
            set { Session[this.MiSessionPagina + "TesoreriaAbrirMiReabrirCaja"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.MiReabrirCaja = false;
                //string mensaje = this.ObtenerMensajeSistema("DomiciliosConfirmarBaja");
                //mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                //string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                //this.btnAceptar.Attributes.Add("OnClick", funcion);

                TESCajas caja = new TESCajas();
                caja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                caja.Tesoreria.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                caja.Usuario.FilialPredeterminada.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;

                caja.Tesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                caja.Tesoreria = TesoreriasF.TesoreriasObtenerAbierta(caja.Tesoreria);
                if (caja.Tesoreria.IdTesoreria == 0)
                {
                    this.MostrarMensaje("TesoreriaValidarAbierta", true);
                    this.btnAceptar.Visible = false;
                    //Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesTesoreriasAbrir.aspx"), true);
                }

                caja.FechaAbrir = caja.Tesoreria.FechaAbrir; ;
                if (TesoreriasF.CajasValidarAbierta(caja))
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
                }

                //necesito Usuario y Tesoreria para levantar el Numero de caja y la FechaAbrir, respectivamente
                caja.Usuario = this.UsuarioActivo;
                //tambien vuelvo a cargar el usuario logueado ya que TesoreriasObtenerAbierta lo pisa
                caja.Tesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                caja.Tesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(caja.Tesoreria);
                this.txtUsuario.Text = this.UsuarioActivo.ApellidoNombre;
                this.txtFecha.Text = caja.Tesoreria.FechaAbrir.ToShortDateString();
                
                //Valido si ya tuvo una caja para la tesoreria la reabrimos
                if (TesoreriasF.CajasValidarCerradaMismaTesoreria(caja))
                {
                    this.MiReabrirCaja = true;
                    this.lblMsgReabrirCaja.Visible = true;
                }
                else
                {
                    //ASIGNO LA ULTIMA CAJA USADA POR EL USUARIO, SI YA ESTA ABIERTA LE ASIGNO UN NUEVO NUMERO
                    this.txtNumeroCaja.Text = TesoreriasF.CajasObtenerNumeroCaja(caja).ToString();
                }
                //Bloqueo numero y fecha
                this.txtNumeroCaja.Enabled = false;
                this.txtFecha.Enabled = false;

            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Validate("Aceptar");
            if (!this.IsValid)
                return;

            TESCajas caja = new TESCajas();
            
            if (this.MiReabrirCaja)
            {
                caja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                caja.Tesoreria.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                caja.Usuario.FilialPredeterminada.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                caja.Tesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                caja.Tesoreria = TesoreriasF.TesoreriasObtenerAbierta(caja.Tesoreria);
                if (TesoreriasF.CajasModificarReabrirCaja(caja))
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
                }
                else
                {
                    this.MostrarMensaje(caja.CodigoMensaje, true, caja.CodigoMensajeArgs);
                }
            }
            else
            {
                caja.Usuario = this.UsuarioActivo;
                caja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                caja.FechaAbrir = Convert.ToDateTime(this.txtFecha.Text);
                caja.FechaAbrirEvento = DateTime.Now;
                caja.NumeroCaja = Convert.ToInt32(this.txtNumeroCaja.Text);
                //caja.Tesoreria.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                //caja.Usuario.FilialPredeterminada.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                //caja.Tesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                //ASIGNACION DE EFECTIVO A CAJA
                this.MapearEfectivoACaja(caja);

                if (TesoreriasF.CajasAbrirObtenerDatos(caja))
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
                }
                else
                {
                    this.MostrarMensaje(caja.CodigoMensaje, true, caja.CodigoMensajeArgs);
                }
            }
        }

        private void MapearEfectivoACaja(TESCajas caja)
        {
            //Cargo las cajas monedas usada por ultima vez por el usuario
            caja.CajasMonedas = TesoreriasF.CajasObtenerSaldoAnteriorPorUsuario(caja);

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }
    }
}
