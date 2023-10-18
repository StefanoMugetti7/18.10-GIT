using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;
using Compras;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Web.Services;
using Contabilidad.Entidades;
using Contabilidad;


namespace IU.Modulos.Compras.Controles
{
    public partial class FamiliasDatos : ControlesSeguros
    {
        private CMPFamilias MiFamilia
        {
            get { return (CMPFamilias)Session[this.MiSessionPagina + "FamiliasDatosMiFamilia"]; }
            set { Session[this.MiSessionPagina + "FamiliasDatosMiFamilia"] = value; }
        }

        enum TipoCuetna {
            Gastos,
            Ganancia,
            Activo,
            CostoMV,
        }

        private TipoCuetna MiTipoCuenta
        {
            get { return (TipoCuetna)Session[this.MiSessionPagina + "FamiliasDatosMiTipoCuenta"]; }
            set { Session[this.MiSessionPagina + "FamiliasDatosMiTipoCuenta"] = value; }
        }

        public delegate void ModificarDatosAceptarEventHandler(object sender, CMPFamilias e);
        public event ModificarDatosAceptarEventHandler ModificarDatosAceptar;

        public delegate void ModificarDatosCancelarEventHandler();
        public event ModificarDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.puCuentasContables.CuentasContablesBuscarSeleccionarPopUp += PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp;
            //this.ctrCuentasContables.CuentasContablesBuscarSeleccionar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(ctrCuentasContables_CuentasContablesBuscarSeleccionar);
            //this.ctrCuentasContables.CuentasContablesBuscarIniciar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(ctrCuentasContables_CuentasContablesBuscarIniciar);
            //this.ctrCuentasContablesActivo.CuentasContablesBuscarIniciar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(ctrCuentasContablesActivo_CuentasContablesBuscarIniciar);
            //this.ctrCuentasContablesActivo.CuentasContablesBuscarSeleccionar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(ctrCuentasContablesActivo_CuentasContablesBuscarSeleccionar);
            //this.ctrCuentasContablesGanancia.CuentasContablesBuscarIniciar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(ctrCuentasContablesGanancia_CuentasContablesBuscarIniciar);
            //this.ctrCuentasContablesGanancia.CuentasContablesBuscarSeleccionar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(ctrCuentasContablesGanancia_CuentasContablesBuscarSeleccionar);
            if (this.IsPostBack)
            {
            }
        }

        private void PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp(CtbCuentasContables e)
        {
            switch (this.MiTipoCuenta)
            {
                case TipoCuetna.Gastos:
                    this.MiFamilia.CuentaContable = e;
                    this.txtNumeroCuentaContableGastos.Text = e.NumeroCuenta;
                    this.txtCuentaContableGastos.Text = e.Descripcion;                    
                    break;
                case TipoCuetna.Ganancia:
                    AyudaProgramacion.MatchObjectProperties(e, this.MiFamilia.CuentaContableGanancia);
                    this.MiFamilia.CuentaContableGanancia.IdCuentaContableGanancia = e.IdCuentaContable;
                    this.txtNumeroCuentaContableGanancia.Text = e.NumeroCuenta;
                    this.txtCuentaContableGanancia.Text = e.Descripcion;
                    break;
                case TipoCuetna.Activo:
                    AyudaProgramacion.MatchObjectProperties(e, this.MiFamilia.CuentaContableActivo);
                    this.MiFamilia.CuentaContableActivo.IdCuentaContableActivo = e.IdCuentaContable;
                    this.txtNumeroCuentaContableActivo.Text = e.NumeroCuenta;
                    this.txtCuentaContableActivo.Text = e.Descripcion;
                    break;
                case TipoCuetna.CostoMV:
                    AyudaProgramacion.MatchObjectProperties(e, this.MiFamilia.CuentaContableCostoMercaderiaVendida);
                    this.MiFamilia.CuentaContableCostoMercaderiaVendida.IdCuentaContableCostoMercaderiaVendida = e.IdCuentaContable;
                    this.txtNumeroCuentaContableCMV.Text = e.NumeroCuenta;
                    this.txtCuentaContableCMV.Text = e.Descripcion;
                    break;
                default:
                    break;
            }
            this.upCuentasContables.Update();
        }

        protected void btnBuscarCuentaContableGastos_Click(object sender, EventArgs e)
        {
            this.MiTipoCuenta = TipoCuetna.Gastos;
            CtbCuentasContables cta = new CtbCuentasContables();
            cta.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerUltimoActivo().IdEjercicioContable.Value;
            this.puCuentasContables.IniciarControl(false, cta, new List<CtbCuentasContables>());
        }
        protected void btnBuscarCuentaContableGanancia_Click(object sender, EventArgs e)
        {
            this.MiTipoCuenta = TipoCuetna.Ganancia;
            CtbCuentasContables cta = new CtbCuentasContables();
            cta.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerUltimoActivo().IdEjercicioContable.Value;
            this.puCuentasContables.IniciarControl(false, cta, new List<CtbCuentasContables>());
        }
        protected void btnBuscarCuentaContableActivo_Click(object sender, EventArgs e)
        {
            this.MiTipoCuenta = TipoCuetna.Activo;
            CtbCuentasContables cta = new CtbCuentasContables();
            cta.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerUltimoActivo().IdEjercicioContable.Value;
            this.puCuentasContables.IniciarControl(false, cta, new List<CtbCuentasContables>());
        }
        protected void btnBuscarCuentaContableCostoMercaderiaVendida_Click(object sender, EventArgs e)
        {
            this.MiTipoCuenta = TipoCuetna.CostoMV;
            CtbCuentasContables cta = new CtbCuentasContables();
            cta.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerUltimoActivo().IdEjercicioContable.Value;
            this.puCuentasContables.IniciarControl(false, cta, new List<CtbCuentasContables>());
        }

        //void ctrCuentasContablesGanancia_CuentasContablesBuscarSeleccionar(CtbCuentasContables e, int indiceColeccion)
        //{
        //    AyudaProgramacion.MatchObjectProperties(e, this.MiFamilia.CuentaContableGanancia);
        //    this.MiFamilia.CuentaContableGanancia.IdCuentaContableGanancia = e.IdCuentaContable;
        //    this.upCuentasContables.Update();
        //}

        //void ctrCuentasContablesGanancia_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        //{
        //    AyudaProgramacion.MatchObjectProperties( ContabilidadF.EjerciciosContablesObtenerActivo(), ejercicio );
        //}

        //void ctrCuentasContablesActivo_CuentasContablesBuscarSeleccionar(CtbCuentasContables e, int indiceColeccion)
        //{
        //    AyudaProgramacion.MatchObjectProperties(e, this.MiFamilia.CuentaContableActivo);
        //    this.MiFamilia.CuentaContableActivo.IdCuentaContableActivo = e.IdCuentaContable;
        //    this.upCuentasContables.Update();
        //}

        //void ctrCuentasContables_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        //{
        //    AyudaProgramacion.MatchObjectProperties(ContabilidadF.EjerciciosContablesObtenerActivo(), ejercicio);
        //}

        //void ctrCuentasContablesActivo_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        //{
        //    AyudaProgramacion.MatchObjectProperties(ContabilidadF.EjerciciosContablesObtenerActivo(), ejercicio);
        //}

        //void ctrCuentasContables_CuentasContablesBuscarSeleccionar(global::Contabilidad.Entidades.CtbCuentasContables e, int indiceColeccion)
        //{
        //    this.MiFamilia.CuentaContable = e;
        //    this.upCuentasContables.Update();
        //}

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Cuenta Bancaria
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CMPFamilias pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiFamilia = pParametro;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    pParametro.FechaAlta = DateTime.Now;
                    //AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, pParametro.UsuarioAlta);
                    this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ctrCamposValores.IniciarControl(this.MiFamilia, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiFamilia = ComprasF.FamiliasObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiFamilia);
                    break;
                case Gestion.Consultar:
                    this.MiFamilia = ComprasF.FamiliasObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiFamilia);
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnCancelar.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(CMPFamilias pParametro)
        {
            this.txtCodigoFamilia.Text = pParametro.IdFamilia.ToString();
            this.txtDescripcion.Text = pParametro.Descripcion;
            //this.ctrCuentasContables.MapearObjetoControles(pParametro.CuentaContable, this.GestionControl, 0);
            this.txtNumeroCuentaContableGastos.Text = pParametro.CuentaContable.NumeroCuenta;
            this.txtCuentaContableGastos.Text = pParametro.CuentaContable.Descripcion;
            this.txtNumeroCuentaContableGanancia.Text = pParametro.CuentaContableGanancia.NumeroCuenta;
            this.txtCuentaContableGanancia.Text = pParametro.CuentaContableGanancia.Descripcion;
            this.txtNumeroCuentaContableActivo.Text = pParametro.CuentaContableActivo.NumeroCuenta;
            this.txtCuentaContableActivo.Text = pParametro.CuentaContableActivo.Descripcion;
            this.txtNumeroCuentaContableCMV.Text = pParametro.CuentaContableCostoMercaderiaVendida.NumeroCuenta;
            this.txtCuentaContableCMV.Text = pParametro.CuentaContableCostoMercaderiaVendida.Descripcion;

            //CtbCuentasContables cuentaMostrar = new CtbCuentasContables();
            //AyudaProgramacion.MatchObjectProperties(this.MiFamilia.CuentaContableActivo, cuentaMostrar);
            //this.ctrCuentasContablesActivo.MapearObjetoControles(cuentaMostrar, this.GestionControl, 0);
            //AyudaProgramacion.MatchObjectProperties(this.MiFamilia.CuentaContableGanancia, cuentaMostrar);
            //this.ctrCuentasContablesGanancia.MapearObjetoControles(cuentaMostrar, this.GestionControl, 0);
            this.ddlRegimenRetencionIIGG.SelectedValue = pParametro.RegimenRetencionIIGG.IdRegimenRetencionIIGG.ToString();
            this.chkRetieneSUSS.Checked = pParametro.RetieneSUSS;
            this.chkStockeable.Checked = pParametro.Stockeable;
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.ctrAuditoria.IniciarControl(pParametro);
            this.ctrCamposValores.IniciarControl(this.MiFamilia, new Objeto(), this.GestionControl);
           
        }

        private void MapearControlesAObjeto(CMPFamilias pParametro)
        {
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.RegimenRetencionIIGG.IdRegimenRetencionIIGG = this.ddlRegimenRetencionIIGG.SelectedValue == string.Empty ? 0 :  Convert.ToInt32(this.ddlRegimenRetencionIIGG.SelectedValue);
            pParametro.RetieneSUSS = this.chkRetieneSUSS.Checked;
            pParametro.Stockeable = this.chkStockeable.Checked;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();

            if (this.txtNumeroCuentaContableGastos.Text.Trim().Length == 0)
                pParametro.CuentaContable.IdCuentaContable = 0;
            if (this.txtNumeroCuentaContableGanancia.Text.Trim().Length == 0)
                pParametro.CuentaContableGanancia.IdCuentaContableGanancia = 0;
            if (this.txtNumeroCuentaContableActivo.Text.Trim().Length == 0)
                pParametro.CuentaContableActivo.IdCuentaContableActivo = 0;
            if (this.txtNumeroCuentaContableCMV.Text.Trim().Length == 0)
                pParametro.CuentaContableCostoMercaderiaVendida.IdCuentaContableCostoMercaderiaVendida = 0;
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            this.ddlRegimenRetencionIIGG.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPRegimenRetencionesIIGG);
            this.ddlRegimenRetencionIIGG.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlRegimenRetencionIIGG.DataTextField = "Descripcion";
            this.ddlRegimenRetencionIIGG.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlRegimenRetencionIIGG, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiFamilia);
            this.MiFamilia.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiFamilia.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = ComprasF.FamiliasAgregar(this.MiFamilia);
                    break;
                case Gestion.Modificar:
                    guardo = ComprasF.FamiliasModificar(this.MiFamilia);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFamilia.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiFamilia.CodigoMensaje, true, this.MiFamilia.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar(null, this.MiFamilia);
        }

    }
}