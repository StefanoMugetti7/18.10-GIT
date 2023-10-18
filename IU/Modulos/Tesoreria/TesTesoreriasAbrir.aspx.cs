using System;
using System.Collections;
using Tesorerias;
using Tesorerias.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class TesTesoreriasAbrir : PaginaSegura
    {
        private TESTesorerias MiTesoreria
        {
            get { return (TESTesorerias)Session[this.MiSessionPagina + "TesoreriaAbrirMiTesoreria"]; }
            set { Session[this.MiSessionPagina + "TesoreriaAbrirMiTesoreria"] = value; }
        }
        private TESTesorerias MiUltimoCierre
        {
            get { return (TESTesorerias)Session[this.MiSessionPagina + "TesoreriaAbrirMiUltimoCierre"]; }
            set { Session[this.MiSessionPagina + "TesoreriaAbrirMiUltimoCierre"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                //string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                //this.btnAceptar.Attributes.Add("OnClick", funcion);

                //ME FIJO SI LA TESORERIA YA ESTA ABIERTA REDIRECCIONO A TESORERIA MOVIMIENTOS
                this.MiTesoreria = new TESTesorerias();
                this.MiUltimoCierre = new TESTesorerias();

                this.MiTesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MiTesoreria.FechaAbrirEvento = DateTime.Now;
                this.MiTesoreria.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;
                this.MiTesoreria = TesoreriasF.TesoreriasObtenerAbierta(this.MiTesoreria);

                if (this.MiTesoreria.IdTesoreria != 0)
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientos.aspx"), true);
                }
                //si pasa la validacion busco la última tesoreria cerrada
                this.MiUltimoCierre.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MiUltimoCierre.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;
                this.MiUltimoCierre = TesoreriasF.TesoreriasObtenerUltimoCierreFilialUsuarioEvento(this.MiUltimoCierre);
                this.txtFechaUltimaTesoreria.Text = this.MiUltimoCierre.FechaAbrir.ToShortDateString();
                this.txtFechaAbrir.Text = DateTime.Now.ToShortDateString();
            }
        }
        void MapearTesoreria()
        {
            this.MiTesoreria.FechaAbrir = Convert.ToDateTime(this.txtFechaAbrir.Text);
            this.MiTesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiTesoreria.FechaAbrirEvento = DateTime.Now;
            this.MiTesoreria.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;

            this.MiUltimoCierre.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            //Valida que no haya Tesorerias (viejas) Abiertas al día de la Fecha (FechaAbrirEvento), si esta abierta para otra fecha redirecciono.
            if (!TesoreriasF.TesoreriaValidarAbiertaFechaEventoAnterior(this.MiTesoreria))
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientos.aspx"), true);

            /* Si la Fecha es = a la ultima reabro la Tesoreria */
            if (Convert.ToDateTime(this.txtFechaAbrir.Text).Date == Convert.ToDateTime(this.txtFechaUltimaTesoreria.Text).Date)
            {
                if (TesoreriasF.TesoreriasModificarReabrir(this.MiUltimoCierre))
                {
                    this.MiTesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(this.MiUltimoCierre);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientos.aspx"), true);
                }
                else
                {
                    this.MostrarMensaje(this.MiTesoreria.CodigoMensaje, true);
                    return;
                }
            }
            //Valida que la Tesoreria NO este Cerrada para la fecha (FechaAbrir)
            if (!TesoreriasF.TesoreriaValidarCerradaEvento(this.MiTesoreria))
            {
                this.MisParametrosUrl = new Hashtable
                {
                    { "CodigoMensaje", "TesoreriaValidarCerrada" }
                };
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlMensajes.aspx"), true);
            }
            //Genera la apertura automatica de la Tesoreria
            if (TesoreriasF.TesoreriaAbrirObtenerDatos(this.MiTesoreria)) //ESTE METODO NO LO MODIFICO, ya que solo controla que FechaAbrir <= DateTime.Now
            {
                this.MiTesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(this.MiTesoreria);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientos.aspx"), true);
            }
            else
            {
                this.MostrarMensaje(this.MiTesoreria.CodigoMensaje, true);
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Validate("Aceptar");
            if (!this.IsValid)
                return;

            if (!this.MiTesoreria.ConfirmarAccion)
            {
                if (Convert.ToDateTime(this.txtFechaAbrir.Text).Date == Convert.ToDateTime(this.txtFechaUltimaTesoreria.Text).Date)
                {
                    this.MiTesoreria.CodigoMensaje = "TesoreriaReabrir";
                    this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiTesoreria.CodigoMensaje), true);
                }
                else if (Convert.ToDateTime(this.txtFechaAbrir.Text).Date < DateTime.Now.Date)
                {
                    this.MiTesoreria.CodigoMensaje = "TesoreriaAbiertaFueraDeFecha";
                    //ACA TENGO QUE PONER UN CARTEL DE SISTEMA QUE AVISE QUE LA FECHA NO ES LA DEL DIA DE HOY
                    this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiTesoreria.CodigoMensaje), true);
                }
                else
                {
                    this.MapearTesoreria();
                }
            }

        }
        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.MapearTesoreria();
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientos.aspx"), true);
        }
    }
}