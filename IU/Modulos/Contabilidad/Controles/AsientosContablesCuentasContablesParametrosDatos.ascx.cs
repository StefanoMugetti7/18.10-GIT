using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;
using Bancos;
using Bancos.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class AsientosContablesCuentasContablesParametrosDatos : ControlesSeguros
    {
        private CtbAsientosContablesCuentasContablesParametros MiCuentaContableParametro
        {
            get { return (CtbAsientosContablesCuentasContablesParametros)Session[this.MiSessionPagina + "MiCuentaContableParametro"]; }
            set { Session[this.MiSessionPagina + "MiCuentaContableParametro"] = value; }
        }

        private List<TGETiposValores> MisTiposValores
        {
            get { return (List<TGETiposValores>)Session[this.MiSessionPagina + "MisTiposValores"]; }
            set { Session[this.MiSessionPagina + "MisTiposValores"] = value; }
        }

        public delegate void AsientosContablesCuentasContablesParametrosDatosAceptarEventHandler(object sender, CtbAsientosContablesCuentasContablesParametros e);
        public event AsientosContablesCuentasContablesParametrosDatosAceptarEventHandler ModificarDatosAceptar;

        public delegate void AsientosContablesCuentasContablesParametrosDatosCancelarEventHandler();
        public event AsientosContablesCuentasContablesParametrosDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.buscarCuenta.CuentasContablesBuscarSeleccionar += new CuentasContablesBuscar.CuentasContablesBuscarEventHandler(buscarCuenta_CuentasContablesBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                if (this.MiCuentaContableParametro == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        void buscarCuenta_CuentasContablesBuscarSeleccionar(CtbCuentasContables e, int indiceColeccion)
        {
            this.MiCuentaContableParametro.CuentaContable = e;
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Cuenta Contable
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbAsientosContablesCuentasContablesParametros pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCuentaContableParametro = pParametro;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.MostrarOpciones();
                    break;
                case Gestion.Modificar:
                    this.MiCuentaContableParametro = ContabilidadF.AsientosContablesCuentasContablesParametrosObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiCuentaContableParametro);
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            this.ddlTipoValor.SelectedValue =  pParametro.TipoValor.IdTipoValor.ToString();
            this.ddlFilial.SelectedValue = pParametro.Filial.IdFilial.ToString();
            //this.ddlBanco.SelectedValue =  pParametro.BancoCuentaBancoIdBanco.ToString();
            //ddlBanco_SelectedIndexChanged(null, EventArgs.Empty);
            //this.ddlBancoCuenta.SelectedValue =  pParametro.BancoCuentaIdBancoCuenta.ToString();
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.ddlMoneda.SelectedValue = pParametro.Moneda.IdMoneda.ToString();
            this.buscarCuenta.MapearObjetoControles(pParametro.CuentaContable, this.GestionControl, 0);
            this.MostrarOpciones();
        }

        private void MapearControlesAObjeto(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            pParametro.TipoValor.IdTipoValor = Convert.ToInt32(this.ddlTipoValor.SelectedValue);
            pParametro.Filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
            pParametro.Filial.Filial = this.ddlFilial.SelectedItem.Text;

            pParametro.BancoCuentaBancoIdBanco = this.ddlBanco.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlBanco.SelectedValue);
            pParametro.BancoCuentaBancoDescripcion = this.ddlBanco.SelectedValue == string.Empty ? string.Empty : this.ddlBanco.SelectedItem.Text;
            pParametro.BancoCuentaIdBancoCuenta = this.ddlBancoCuenta.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlBancoCuenta.SelectedValue);
            pParametro.BancoCuentaNumeroCuenta = this.ddlBancoCuenta.SelectedValue == string.Empty ? string.Empty : this.ddlBancoCuenta.SelectedItem.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

           // this.MisTiposValores = TGEGeneralesF.ListasValoresObtenerListaDetalle(Generales.Entidades.EnumTGEListasValores.EntidadesContables);
            EnumTGETiposFuncionalidades tipoFunc = (EnumTGETiposFuncionalidades)this.paginaSegura.paginaActual.IdTipoFuncionalidad;
                    this.MisTiposValores = TGEGeneralesF.TiposValoresObtenerLista(tipoFunc);
            //this.MisTiposValores = TGEGeneralesF.TiposValoresObtenerLista();
            this.ddlTipoValor.DataSource = this.MisTiposValores;
            this.ddlTipoValor.DataValueField = "IdTipoValor";
            this.ddlTipoValor.DataTextField = "TipoValor";
            this.ddlTipoValor.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlEntidadContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            if (this.ddlFilial.Items.Count != 1)
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlMoneda.DataSource = TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.AgregarItemSeleccione(this.ddlBanco, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancoCuenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));
 
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiCuentaContableParametro);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.AsientosContablesCuentasContablesParametrosAgregar(this.MiCuentaContableParametro);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.AsientosContablesCuentasContablesParametrosModificar(this.MiCuentaContableParametro);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCuentaContableParametro.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiCuentaContableParametro.CodigoMensaje, true, this.MiCuentaContableParametro.CodigoMensajeArgs);
            }
        }

        protected void ddlEntidadContable_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MostrarOpciones();    
        }

        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MostrarOpciones(); 
        }

        protected void ddlBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlBanco.SelectedValue) && !string.IsNullOrEmpty(this.ddlFilial.SelectedValue))
            {
                TESBancosCuentas filtro = new TESBancosCuentas();
                filtro.Banco.IdBanco = Convert.ToInt32(this.ddlBanco.SelectedValue);
                filtro.Filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
                this.ddlBancoCuenta.Items.Clear();
                this.ddlBancoCuenta.SelectedIndex = -1;
                this.ddlBancoCuenta.SelectedValue = null;
                this.ddlBancoCuenta.ClearSelection();
                this.ddlBancoCuenta.DataSource = BancosF.BancosCuentasObtenerListaFiltro(filtro);
                this.ddlBancoCuenta.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
                this.ddlBancoCuenta.DataValueField = "IdBancoCuenta";
                this.ddlBancoCuenta.DataBind();
                if (this.ddlBancoCuenta.Items.Count !=1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlBancoCuenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));


                ListItem item = this.ddlBancoCuenta.Items.FindByValue(this.MiCuentaContableParametro.BancoCuentaIdBancoCuenta.ToString());
                if (item == null & this.MiCuentaContableParametro.BancoCuentaIdBancoCuenta > 0)
                    this.ddlBancoCuenta.Items.Add(new ListItem(this.MiCuentaContableParametro.BancoCuentaDescripcion, this.MiCuentaContableParametro.BancoCuentaIdBancoCuenta.ToString()));
                else if(this.MiCuentaContableParametro.BancoCuentaIdBancoCuenta > 0)
                    this.ddlBancoCuenta.SelectedValue = this.MiCuentaContableParametro.BancoCuentaIdBancoCuenta.ToString();

            }
            else
            {
                ddlBancoCuenta.Items.Clear();

                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancoCuenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                ddlBancoCuenta.SelectedValue = string.Empty;
            }
            
        }

        private void MostrarOpciones()
        {
            this.pnlBancoCuenta.Visible = false;
            this.rfvBanco.Enabled = false;
            this.rfvBancoCuenta.Enabled = false;

            if (string.IsNullOrEmpty(this.ddlTipoValor.SelectedValue) || string.IsNullOrEmpty(this.ddlFilial.SelectedValue))
                return;
 

            switch (this.MisTiposValores[this.ddlTipoValor.SelectedIndex].IdTipoValor)
            {
                case (int)EnumTiposValores.Cheque:
                case (int)EnumTiposValores.Transferencia:
                case (int)EnumTiposValores.TarjetaCredito:
                case (int)EnumTiposValores.PlazosFijos:
                    this.pnlBancoCuenta.Visible = true;
                    this.rfvBanco.Enabled = true;
                    this.rfvBancoCuenta.Enabled = true;
                    TESBancos bco = new TESBancos();
                    bco.Estado.IdEstado = (int)EstadosTodos.Todos;
                    this.ddlBanco.Items.Clear();
                    this.ddlBanco.SelectedIndex = -1;
                    this.ddlBanco.SelectedValue = null;
                    this.ddlBanco.ClearSelection();
                    this.ddlBanco.DataSource = BancosF.BancosObtenerListaFiltro(bco);
                    this.ddlBanco.DataValueField = "IdBanco";
                    this.ddlBanco.DataTextField = "Descripcion";
                    this.ddlBanco.DataBind();
                    if (this.ddlBanco.Items.Count != 1)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlBanco, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    ListItem item = this.ddlBanco.Items.FindByValue(this.MiCuentaContableParametro.BancoCuentaBancoIdBanco.ToString());
                    if (item == null & this.MiCuentaContableParametro.BancoCuentaBancoIdBanco > 0)
                        this.ddlBanco.Items.Add(new ListItem(this.MiCuentaContableParametro.BancoCuentaBancoDescripcion, this.MiCuentaContableParametro.BancoCuentaBancoIdBanco.ToString()));
                    if (this.MiCuentaContableParametro.BancoCuentaBancoIdBanco.HasValue && this.MiCuentaContableParametro.BancoCuentaBancoIdBanco == 0)
                        this.ddlBanco.SelectedValue = string.Empty;
                    else
                        this.ddlBanco.SelectedValue = this.MiCuentaContableParametro.BancoCuentaBancoIdBanco.ToString();
                    this.ddlBanco_SelectedIndexChanged(null, EventArgs.Empty);
                    break;
                case (int)EnumTiposValores.Efectivo:
                    break;
                case (int)EnumTiposValores.ChequeTercero:
                    break;

                default:
                    break;
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
                this.ModificarDatosAceptar(null, this.MiCuentaContableParametro);
        }
    }
}