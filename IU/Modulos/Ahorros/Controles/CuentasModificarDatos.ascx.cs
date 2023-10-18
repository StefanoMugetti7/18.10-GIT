using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Ahorros.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Ahorros;
using Afiliados.Entidades;
using Generales.Entidades;
using System.Collections.Generic;

namespace IU.Modulos.Ahorros.Controles
{
    public partial class CuentasModificarDatos : ControlesSeguros
    {
        private AhoCuentas MiAhoCuentas
        {
            get { return (AhoCuentas)Session[this.MiSessionPagina + "AhorroMiAhoCuentas"]; }
            set { Session[this.MiSessionPagina + "AhorroMiAhoCuentas"] = value; }
        }

        public delegate void AhorroDatosAceptarEventHandler(object sender, AhoCuentas e);
        public event AhorroDatosAceptarEventHandler AhorroModificarDatosAceptar;

        public delegate void AhorroDatosCancelarEventHandler();
        public event AhorroDatosCancelarEventHandler AhorroModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrAfiliados.AfiliadosBuscarSeleccionar += new IU.Modulos.Afiliados.Controles.AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            if (this.IsPostBack)
            {
                if (this.MiAhoCuentas == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(AhoCuentas pCuentas, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.DeshabilitarControles();
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAhoCuentas = pCuentas;
                    this.ddlEstado.SelectedIndex = 1;
                    this.txtSaldo.Text = "0";
                    this.txtNumero.Text = "0";
                    this.txtUsuarioAlta.Text = this.UsuarioActivo.ApellidoNombre;
                    this.txtFechaAlta.Text = DateTime.Today.ToShortDateString();
                    this.ddlEstado.SelectedValue = ((int)EstadosAhorrosCuentas.CuentaAbierta).ToString();
                    this.ddlEstado.Enabled = false;
                    this.btnAgregarCotitular.Visible = this.ValidarPermiso("CotitularesAgregar.aspx");
                    this.ctrComentarios.IniciarControl(this.MiAhoCuentas, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiAhoCuentas, this.GestionControl);

                    this.ctrCamposValores.IniciarControl(pCuentas, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiAhoCuentas = AhorroF.CuentasObtenerDatosCompletos(pCuentas);
                    this.MapearObjetoAControles(this.MiAhoCuentas);
                    this.ddlTipoCuenta.Enabled = false;
                    this.ddlMoneda.Enabled = false;
                    this.btnAgregarCotitular.Visible = this.ValidarPermiso("CotitularesAgregar.aspx");
                    break;
                case Gestion.Consultar:
                    this.MiAhoCuentas = AhorroF.CuentasObtenerDatosCompletos(pCuentas);
                    this.MapearObjetoAControles(this.MiAhoCuentas);
                    this.ddlTipoCuenta.Enabled = false;
                    this.ddlMoneda.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void DeshabilitarControles()
        {
            this.txtFechaAlta.Enabled = false;
            this.txtUsuarioAlta.Enabled = false;
            this.txtSaldo.Enabled = false;
            this.txtNumero.Enabled = false;
        }

        private void CargarCombos()
        {
            this.ddlTipoCuenta.DataSource = AhorroF.CuentasTiposObtenerListaActiva();
            this.ddlTipoCuenta.DataValueField = "IdCuentaTipo";
            this.ddlTipoCuenta.DataTextField = "Descripcion";
            this.ddlTipoCuenta.DataBind();
            if (this.ddlTipoCuenta.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoCuenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlMoneda.DataSource = TGEGeneralesF.MonedasObtenerLista();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "miMonedaDescripcion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosAhorrosCuentas));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }

        private void MapearObjetoAControles(AhoCuentas pCuenta)
        {
            this.ddlTipoCuenta.SelectedValue = pCuenta.CuentaTipo.IdCuentaTipo.ToString();
            this.ddlMoneda.SelectedValue = pCuenta.Moneda.IdMoneda.ToString();
            this.txtDenominacion.Text = pCuenta.Denominacion;
            this.txtNumero.Text = pCuenta.NumeroCuenta.ToString();
            this.txtSaldo.Text = pCuenta.SaldoActual.ToString();
            this.ddlEstado.SelectedValue = pCuenta.Estado.IdEstado.ToString();
            this.txtFechaAlta.Text = pCuenta.FechaAlta.ToShortDateString();
            this.txtUsuarioAlta.Text = pCuenta.UsuarioAlta.ApellidoNombre;
            AyudaProgramacion.CargarGrillaListas<AhoCotitulares>(this.MiAhoCuentas.Cotitulares, true, this.gvDatos, true);
            this.ctrComentarios.IniciarControl(pCuenta, this.GestionControl);
            this.ctrArchivos.IniciarControl(pCuenta, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pCuenta);

            this.ctrCamposValores.IniciarControl(pCuenta, new Objeto(), this.GestionControl);
        }

        private void MapearControlesAObjeto(AhoCuentas pCuenta)
        {
            pCuenta.CuentaTipo.IdCuentaTipo = Convert.ToInt32(this.ddlTipoCuenta.SelectedValue);
            pCuenta.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            pCuenta.Denominacion = this.txtDenominacion.Text;
            pCuenta.NumeroCuenta = Convert.ToInt32(this.txtNumero.Text);
            pCuenta.SaldoActual = Convert.ToDecimal(this.txtSaldo.Text);
            pCuenta.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);

            pCuenta.Comentarios = ctrComentarios.ObtenerLista();
            pCuenta.Archivos = ctrArchivos.ObtenerLista();

            pCuenta.Campos = this.ctrCamposValores.ObtenerLista();
            pCuenta.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiAhoCuentas);
            this.MiAhoCuentas.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAhoCuentas.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    this.MiAhoCuentas.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    guardo = AhorroF.CuentasAgregar(this.MiAhoCuentas);
                    break;
                case Gestion.Modificar:
                    guardo = AhorroF.CuentasModificar(this.MiAhoCuentas);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAhoCuentas.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiAhoCuentas.CodigoMensaje, true, this.MiAhoCuentas.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.AhorroModificarDatosCancelar != null)
                this.AhorroModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.AhorroModificarDatosAceptar != null)
                this.AhorroModificarDatosAceptar(null, this.MiAhoCuentas);
        }

        #region Cotitulares

        void ctrAfiliados_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        {
            if (e.Estado.IdEstado == (int)EstadosAfiliados.Baja)
            {
                this.MostrarMensaje("ValidarAfiliadoEstado", true, new List<string>() { e.Estado.Descripcion });
                return;
            }
            if (!this.MiAhoCuentas.Cotitulares.Exists(x => x.Afiliado.IdAfiliado == e.IdAfiliado))
            {
                AhoCotitulares cotitular = new AhoCotitulares();
                cotitular.EstadoColeccion = EstadoColecciones.Agregado;
                cotitular.Estado.IdEstado = (int)Estados.Activo;
                AyudaProgramacion.MatchObjectProperties(e, cotitular.Afiliado);
                this.MiAhoCuentas.Cotitulares.Add(cotitular);
                AyudaProgramacion.CargarGrillaListas<AhoCotitulares>(this.MiAhoCuentas.Cotitulares, true, this.gvDatos, true);
                this.upCotitulares.Update();
            }
        }

        protected void btnAgregarCotitular_Click(object sender, EventArgs e)
        {
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CotitularesCantidadMaxima);
            if (this.MiAhoCuentas.Cotitulares.Count(x => x.Estado.IdEstado == (int)Estados.Activo) < Convert.ToInt32(valor.ParametroValor))
            {
                this.ctrAfiliados.IniciarControl(this.MiAhoCuentas.Afiliado, true, EnumAfiliadosTipos.Familiares, false);
            }
            else
            {
                this.MostrarMensaje("CotitularesValidarCantidadMaxima", true, new List<string>() { valor.ParametroValor });
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiAhoCuentas.Cotitulares[indiceColeccion].Estado.IdEstado = (int)Estados.Baja;
                this.MiAhoCuentas.Cotitulares[indiceColeccion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAhoCuentas.Cotitulares[indiceColeccion], Gestion.Anular);
                AyudaProgramacion.CargarGrillaListas<AhoCotitulares>(this.MiAhoCuentas.Cotitulares, true, this.gvDatos, true);
                this.upCotitulares.Update();
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AhoCotitulares item = (AhoCotitulares)e.Row.DataItem;

                switch (this.GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                        bool permisoModificar = this.ValidarPermiso("CotitularesBajas.aspx");
                        ibtn.Visible = permisoModificar;

                        string mensaje = this.ObtenerMensajeSistema("CotitularesConfirmarBaja");
                        mensaje = string.Format(mensaje, string.Concat(item.Afiliado.ApellidoNombre));
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                        ibtn.Attributes.Add("OnClick", funcion);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

    }
}