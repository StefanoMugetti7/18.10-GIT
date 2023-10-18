using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using CuentasPagar.Entidades;
using Comunes.Entidades;
using CuentasPagar.FachadaNegocio;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Proveedores.Entidades;
using Proveedores;
using Afiliados.Entidades;
using Afiliados;
using Bancos;
using Bancos.Entidades;
using System.Web.Services;
using System.Globalization;
using Evol.Controls;
using Reportes.FachadaNegocio;
using System.Data;
using System.Collections;
using System.Reflection;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class OrdenesPagosDatos : ControlesSeguros
    {
        private bool MiCheckTodosPrimeraVez
        {
            get { return (bool)Session[this.MiSessionPagina + "OrdenesDePagoDatosMiCheckTodosPrimeraVez"]; }
            set { Session[this.MiSessionPagina + "OrdenesDePagoDatosMiCheckTodosPrimeraVez"] = value; }
        }

        private bool MiCheckIncluirTodos
        {
            get { return (bool)Session[this.MiSessionPagina + "OrdenesDePagoDatosMiCheckIncluirTodos"]; }
            set { Session[this.MiSessionPagina + "OrdenesDePagoDatosMiCheckIncluirTodos"] = value; }
        }

        private CapOrdenesPagos MiOrdenPago
        {
            get { return (CapOrdenesPagos)Session[this.MiSessionPagina + "OrdenesDePagoDatosMiOrdenPago"]; }
            set { Session[this.MiSessionPagina + "OrdenesDePagoDatosMiOrdenPago"] = value; }
        }

        private List<TGEFiliales> MisFilialPago
        {
            get { return (List<TGEFiliales>)Session[this.MiSessionPagina + "OrdenesDePagoDatosMisFilialPago"]; }
            set { Session[this.MiSessionPagina + "OrdenesDePagoDatosMisFilialPago"] = value; }
        }

        public AfiAfiliados MiAfiliado
        {
            get { return (AfiAfiliados)Session[this.MiSessionPagina + "OrdenesPagosListarCapOrdenesPagos"]; }
            set { Session[this.MiSessionPagina + "OrdenesPagosListarCapOrdenesPagos"] = value; }
        }

        public CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "ObtenerDatosProveedor"]; }
            set { Session[this.MiSessionPagina + "ObtenerDatosProveedor"] = value; }
        }

        public delegate void AfiliadosDatosEventHandler(AfiAfiliados e);
        public event AfiliadosDatosEventHandler BuscarAfiliado;

        public delegate void OrdenesDePagoDatosAceptarEventHandler(CapOrdenesPagos e);
        public event OrdenesDePagoDatosAceptarEventHandler OrdenesDePagoDatosAceptar;

        public delegate void OrdenesDePagoDatosCancelarEventHandler();
        public event OrdenesDePagoDatosCancelarEventHandler OrdenesDePagoDatosCancelar;
        
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            //this.ctrAfiliados.AfiliadosBuscarSeleccionar += new Afiliados.Controles.AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            //this.ctrProveedores.ProveedoresBuscar += new ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrProveedores_ProveedoresBuscar);
            ctrAnticiposTurismo.AnticiposTurismoBuscarSeleccionar += CtrAnticiposTurismo_AnticiposTurismoBuscarSeleccionar;
        }

        //#region Buscar por Entidad
        //void ctrProveedores_ProveedoresBuscar(global::Proveedores.Entidades.CapProveedores e)
        //{
        //    this.MiOrdenPago.Beneficiario = e.RazonSocial;
        //    this.MiOrdenPago.Entidad.IdRefEntidad = e.IdProveedor.Value;
        //    this.MiOrdenPago.Entidad.IdCuentaContable = e.CuentaContable.IdCuentaContable;
        //    this.txtCodigo.Text = e.IdProveedor.ToString();
        //    this.txtBeneficiario.Text = e.RazonSocial;
        //    this.txtCuit.Text = e.CUIT;
        //    this.IniciarControlAgregar(this.MiOrdenPago);
        //}

        //void ctrAfiliados_AfiliadosBuscarSeleccionar(global::Afiliados.Entidades.AfiAfiliados e)
        //{
        //    this.MiOrdenPago.Beneficiario = e.ApellidoNombre;
        //    this.MiOrdenPago.Entidad.IdRefEntidad = e.IdAfiliado;
        //    this.txtCodigo.Text = e.NumeroSocio;
        //    this.txtBeneficiario.Text = e.ApellidoNombre;
        //    this.txtCuit.Text = e.CUILFormateado;
        //    this.IniciarControlAgregar(this.MiOrdenPago);
        //}

        //protected void btnBuscarEntidad_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.ddlEntidades.SelectedValue))
        //    {
        //        this.MiOrdenPago.Entidad.IdEntidad = Convert.ToInt32(this.ddlEntidades.SelectedValue);
        //        switch (this.MiOrdenPago.Entidad.IdEntidad)
        //        {
        //            case (int)EnumTGEEntidades.Proveedores:
        //                this.ctrProveedores.IniciarControl();
        //                break;
        //            case (int)EnumTGEEntidades.Afiliados:
        //                this.ctrAfiliados.IniciarControl(true);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.ddlEntidades.SelectedValue))
        //    {
        //        this.MiOrdenPago.Entidad.IdEntidad = Convert.ToInt32(this.ddlEntidades.SelectedValue);
        //        switch (this.MiOrdenPago.Entidad.IdEntidad)
        //        {
        //            case (int)EnumTGEEntidades.Proveedores:
        //                CapProveedores prov = new CapProveedores();
        //                prov.IdProveedor = this.txtCodigo.Text ==string.Empty? 0 : Convert.ToInt32(this.txtCodigo.Text);
        //                prov = ProveedoresF.ProveedoresObtenerDatosCompletos(prov);
        //                if (prov.IdProveedor > 0)
        //                    this.ctrProveedores_ProveedoresBuscar(prov);
        //                else
        //                    this.ctrProveedores.IniciarControl();
        //                break;
        //            case (int)EnumTGEEntidades.Afiliados:
        //                AfiAfiliados afi = new AfiAfiliados();
        //                afi.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Socios;
        //                afi.NumeroSocio = this.txtCodigo.Text;
        //                afi = AfiliadosF.AfiliadosObtenerPorNumeroSocio(afi);
        //                if (afi.IdAfiliado > 0)
        //                    this.ctrAfiliados_AfiliadosBuscarSeleccionar(afi);
        //                else
        //                    this.ctrAfiliados.IniciarControl(true);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        //#endregion

        public void IniciarControl(CapOrdenesPagos pOrdenPago, Gestion pGestion)
        {
            this.MiCheckTodosPrimeraVez = true;
            this.MiCheckIncluirTodos = true;
            this.MiOrdenPago = pOrdenPago;
            this.MiOrdenPago.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.GestionControl = pGestion;
            this.CargarCombos();
            TGEParametrosValores valor;
            bool bvalor;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
                    this.txtFechaAlta.Enabled = true;
                    //this.cdFechaAlta.Enabled = true;
                    //this.imgFechaAlta.Visible = true;
                    //this.btnBuscarEntidad.Visible = true;
                    //this.txtCodigo.Enabled = true;
                    this.ddlNumeroSocio.Enabled = true;
                    this.ddlFilialPago.Enabled = true;
                    //this.ddlTiposValores.Enabled = true;
                    this.ddlEntidades.Enabled = true;
                    this.pnlImportesAPagar.Visible = true;
                    this.btnCalcularRetenciones.Visible = true;
                    this.pnlRetenciones.Visible = true;
                    AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MiOrdenPago.SolicitudesPagos, false, this.gvDatos, true);
                    this.ctrComentarios.IniciarControl(pOrdenPago, this.GestionControl);
                    this.ctrOrdenesPagosValores.IniciarControl(this.MiOrdenPago, this.GestionControl);
                    if (pOrdenPago.Entidad.IdEntidad > 0)
                    {
                        this.ddlEntidades.SelectedValue = pOrdenPago.Entidad.IdEntidad.ToString();

                        //if (pOrdenPago.Entidad.IdRefEntidad > 0)
                        //{
                        //    if (pOrdenPago.Entidad.IdEntidad == (int)EnumTGEEntidades.Afiliados)
                        //    {
                        //        AfiAfiliados afi = new AfiAfiliados();
                        //        afi.IdAfiliado = pOrdenPago.Entidad.IdRefEntidad;
                        //        afi = AfiliadosF.AfiliadosObtenerDatosCompletos(afi);
                        //        //if (afi.IdAfiliado > 0)
                        //            //this.ctrAfiliados_AfiliadosBuscarSeleccionar(afi);

                        //    }
                        //    else
                        //    {
                        //        //this.txtCodigo.Text = pOrdenPago.Entidad.IdRefEntidad.ToString();
                        //        //this.txtCodigo_TextChanged(null, EventArgs.Empty);
                        //    }
                        //}
                    }
                    //STEFANO -> TODO ESTO DE CONT VA?
                    //valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.OrdenesPagosCircuitoAprobacion);
                    //bvalor = false;
                    //if (!string.IsNullOrEmpty(valor.ParametroValor) && bool.TryParse(valor.ParametroValor, out bvalor))
                    //{
                    //    if (!bvalor)
                    //    {
                    //        this.ctrFechaCajaContable.IniciarControl(Gestion.Agregar, DateTime.Now, DateTime.Now);
                    //        this.txtFechaAlta.Attributes.Add("onchange", "SetearFechaPago();");
                    //    }
                    //}
                    this.ctrFechaCajaContable.IniciarControl(Gestion.Agregar, DateTime.Now, DateTime.Now);
                    this.txtFechaAlta.Attributes.Add("onchange", "SetearFechaPago();");
                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CalcularRetencionesAutomatico);
                    if (valor.ParametroValor.ToLower() == "true")
                    {
                        hdfCalcularRetenciones.Value = "1";
                    }
                    if (this.MisParametrosUrl.Contains("IdRefEntidad"))
                    {
                        string Entidad = this.MisParametrosUrl["IdEntidad"].ToString();
                        if (Convert.ToInt32(this.MisParametrosUrl["IdEntidad"].ToString()) == (int)EnumTGEEntidades.Afiliados)
                        {

                            MiAfiliado = new AfiAfiliados();
                            MiAfiliado.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdRefEntidad"].ToString());
                            MiAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(MiAfiliado);

                            if (MiAfiliado.IdAfiliado != 0)
                            {
                                this.ddlNumeroSocio.Items.Add(new ListItem(MiAfiliado.RazonSocial.ToString(), MiAfiliado.IdAfiliado.ToString()));
                                this.ddlNumeroSocio.SelectedValue = MiAfiliado.IdAfiliado.ToString();
                                
                                txtCuit.Text = MiAfiliado.CUIL.ToString();
                                MiOrdenPago.Entidad.IdEntidad = 188;
                                this.MiOrdenPago.Beneficiario = MiAfiliado.RazonSocial;
                                this.MiOrdenPago.Entidad.IdRefEntidad = MiAfiliado.IdAfiliado;
                                //this.MiOrdenPago.Entidad.IdCuentaContable = MiAfiliado..CuentaContable.IdCuentaContable;
                                this.MiOrdenPago.Entidad.Cuit = MiAfiliado.CUIL.ToString();
                                this.ddlNumeroSocio.SelectedValue = MiAfiliado.IdAfiliado.ToString();
                                this.txtBeneficiario.Text = MiAfiliado.RazonSocial;
                                this.txtCuit.Text = MiAfiliado.CUIL.ToString();
                                this.IniciarControlAgregar(this.MiOrdenPago);
                            }
                        }
                        else
                        {
                            MiProveedor = new CapProveedores();
                            MiProveedor.IdProveedor = Convert.ToInt32(this.MisParametrosUrl["IdRefEntidad"].ToString());
                            MiProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(MiProveedor);
                            //this.hdfIdAfiliado.Value = this.MisParametrosUrl["IdAfiliado"].ToString();
                            //MiProveedor.IdProveedor = 0;
                            if (MiProveedor.IdProveedor != 0)
                            {
                                this.ddlNumeroSocio.Items.Add(new ListItem(MiProveedor.RazonSocial.ToString(), MiProveedor.IdProveedor.ToString()));
                                this.ddlNumeroSocio.SelectedValue = MiProveedor.IdProveedor.ToString();
                                txtBeneficiario.Text = MiProveedor.BeneficiarioDelCheque;
                                txtCuit.Text = MiProveedor.CUIT.ToString();
                                MiOrdenPago.Entidad.IdEntidad = 187;
                                this.MiOrdenPago.Beneficiario = MiProveedor.RazonSocial;
                                this.MiOrdenPago.Entidad.IdRefEntidad = MiProveedor.IdProveedor.Value;
                                this.MiOrdenPago.Entidad.IdCuentaContable = MiProveedor.CuentaContable.IdCuentaContable;
                                this.MiOrdenPago.Entidad.Cuit = MiProveedor.CUIT;
                                this.ddlNumeroSocio.SelectedValue = MiProveedor.IdProveedor.ToString();
                                this.txtBeneficiario.Text = MiProveedor.RazonSocial;
                                this.txtCuit.Text = MiProveedor.CUIT;
                                this.IniciarControlAgregar(this.MiOrdenPago);
                            }

                            /*Integracion con Turismo*/
                            if (this.MisParametrosUrl.Contains("AnticipoTurismo"))
                            {
                                this.MisParametrosUrl.Remove("AnticipoTurismo");
                                btnAnticipoTurismo_Click(btnAnticipoTurismo, EventArgs.Empty);
                            }
                        }
                        //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                    }
                    this.phBotones.Visible = true;
                    this.ctrArchivos.IniciarControl(this.MiOrdenPago, this.GestionControl);
                    break;
                case Gestion.Autorizar:
                    this.ddlFilialPago.Enabled = true;
                    this.MiOrdenPago = CuentasPagarF.OrdenesPagosObtenerDatosCompletos(this.MiOrdenPago);
                    this.MapearObjetoControles(this.MiOrdenPago);
                    this.btnImprimir.Visible = true;
                    this.ctrFechaCajaContable.IniciarControl(Gestion.Agregar, this.MiOrdenPago.FechaAlta);
                    this.txtDiferencia.Visible = false;
                    this.lblDiferencia.Visible = false;

                    this.ctrArchivos.IniciarControl(this.MiOrdenPago, this.GestionControl);
                    break;
                case Gestion.Anular:
                case Gestion.AnularConfirmar:
                    this.ddlFilialPago.Enabled = false;
                    this.MiOrdenPago = CuentasPagarF.OrdenesPagosObtenerDatosCompletos(this.MiOrdenPago);
                    this.MapearObjetoControles(this.MiOrdenPago);
                    this.ctrFechaCajaContable.IniciarControl(this.GestionControl, this.MiOrdenPago.FechaPago.HasValue ? this.MiOrdenPago.FechaPago : this.MiOrdenPago.FechaAlta);
                    this.btnImprimir.Visible = true;
                    this.txtDiferencia.Visible = false;
                    this.lblDiferencia.Visible = false;
                    break;
                case Gestion.Consultar:
                    this.ddlFilialPago.Enabled = false;
                    this.MiOrdenPago = CuentasPagarF.OrdenesPagosObtenerDatosCompletos(this.MiOrdenPago);
                    this.MapearObjetoControles(this.MiOrdenPago);
                    this.ctrFechaCajaContable.IniciarControl(this.GestionControl, this.MiOrdenPago.FechaPago.HasValue ? this.MiOrdenPago.FechaPago : this.MiOrdenPago.FechaAlta);
                    this.btnAceptar.Visible = false;
                    this.btnImprimir.Visible = true;
                    this.txtDiferencia.Visible = false;
                    this.lblDiferencia.Visible = false;
                    FirmarDocumento();
                    break;
                case Gestion.Modificar:
                    this.ddlFilialPago.Enabled = false;
                    this.MiOrdenPago = CuentasPagarF.OrdenesPagosObtenerDatosCompletos(this.MiOrdenPago);
                    this.MapearObjetoControles(this.MiOrdenPago);
                    this.ctrFechaCajaContable.IniciarControl(this.GestionControl, this.MiOrdenPago.FechaPago.HasValue ? this.MiOrdenPago.FechaPago : this.MiOrdenPago.FechaAlta);
                    this.btnImprimir.Visible = true;
                    this.btnAceptar.Visible = true;
                    this.txtDiferencia.Visible = false;
                    this.lblDiferencia.Visible = false;

                    this.ctrArchivos.IniciarControl(this.MiOrdenPago, this.GestionControl);
                    break;
                default:
                    break;
            }

            //this.MisParametrosUrl = new Hashtable();
        }

        private void FirmarDocumento()
        {
            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = this.MiOrdenPago.IdOrdenPago;
            firmarDoc.Tabla = "CapOrdenesPagos";
            PropertyInfo prop = MiOrdenPago.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            firmarDoc.Key = prop.Name;
            firmarDoc.CodigoPlantilla = "OrdenesPago";
            this.btnFirmaDigital.Visible = TGEGeneralesF.FirmarDocumentosValidar(firmarDoc);
            this.btnFirmaDigitalBaja.Visible = !this.btnFirmaDigital.Visible;
            this.copyClipboard.Visible = btnFirmaDigital.Visible;
            this.btnWhatsAppFirmarDocumento.Visible = btnFirmaDigital.Visible;
            hfLinkFirmarDocumento.Value = TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link;

            if (this.btnFirmaDigitalBaja.Visible)
            {
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarFirmaDigitalManuscritaBaja"));
                this.btnFirmaDigitalBaja.Attributes.Add("OnClick", funcion);
            }
        }

        private void CargarCombos()
        {
            this.ddlFilialPago.DataSource = this.UsuarioActivo.Filiales;// TGEGeneralesF.FilialesObenerLista();
            this.ddlFilialPago.DataValueField = "IdFilial";
            this.ddlFilialPago.DataTextField = "Filial";
            this.ddlFilialPago.DataBind();
            this.ddlFilialPago.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            this.ddlEntidades.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Entidades);
            this.ddlEntidades.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlEntidades.DataTextField = "Descripcion";
            this.ddlEntidades.DataBind();

            //TESBancosCuentas filtro=new TESBancosCuentas();
            //filtro.Estado.IdEstado = (int)EstadosTodos.Todos;
            //this.ddlBancosCuentas.DataSource = BancosF.BancosCuentasObtenerListaFiltro(filtro);
            //this.ddlBancosCuentas.DataValueField = "IdBancoCuenta";
            //this.ddlBancosCuentas.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            //this.ddlBancosCuentas.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoControles(CapOrdenesPagos pParametro)
        {
            //this.ddlEntidades.SelectedValue = pParametro.Entidad.IdEntidad.ToString();
            //this.ddlNumeroSocio.SelectedValue = pParametro.Entidad.IdRefEntidad.ToString();
            this.txtBeneficiario.Text = pParametro.Beneficiario; // pParametro.Entidad.Nombre;
            this.txtCuit.Text = pParametro.Entidad.Cuit;

            this.txtOrdenPago.Text = pParametro.IdOrdenPago.ToString();
            this.txtFechaAlta.Text = pParametro.FechaAlta.ToShortDateString();            
            this.txtSubTotal.Text = pParametro.ImporteSubTotal.ToString("C2");
            this.txtImporteRetenciones.Text = pParametro.ImporteRetenciones.ToString("C2");
            this.txtAnticipo.Text = pParametro.ImporteAnticipos.ToString("C2");
            this.txtTotalAPagar.Text = pParametro.ImporteTotal.ToString("C2");

            //ListItem item = this.ddlTiposValores.Items.FindByValue(pParametro.TipoValor.IdTipoValor.ToString());
            //if (item == null)
            //    this.ddlTiposValores.Items.Add(new ListItem(pParametro.TipoValor.TipoValor, pParametro.TipoValor.IdTipoValor.ToString()));
            //this.ddlTiposValores.SelectedValue = pParametro.TipoValor.IdTipoValor.ToString();

            ListItem item = this.ddlFilialPago.Items.FindByValue(pParametro.FilialPago.IdFilialPago.ToString());
            if (item == null)
                this.ddlFilialPago.Items.Add(new ListItem(pParametro.FilialPago.Filial, pParametro.FilialPago.IdFilialPago.ToString()));
            this.ddlFilialPago.SelectedValue = pParametro.FilialPago.IdFilialPago.ToString();

            item = this.ddlEntidades.Items.FindByValue(pParametro.Entidad.IdEntidad.ToString());
            if (item == null)
                this.ddlEntidades.Items.Add(new ListItem(pParametro.Entidad.Nombre, pParametro.Entidad.IdEntidad.ToString()));
            this.ddlEntidades.SelectedValue = pParametro.Entidad.IdEntidad.ToString();

            item = this.ddlNumeroSocio.Items.FindByValue(pParametro.Entidad.IdRefEntidad.ToString());
            if (item == null)
                this.ddlNumeroSocio.Items.Add(new ListItem(pParametro.Entidad.Nombre, pParametro.Entidad.IdRefEntidad.ToString()));
            this.ddlNumeroSocio.SelectedValue = pParametro.Entidad.IdRefEntidad.ToString();


            this.ctrComentarios.IniciarControl(this.MiOrdenPago, GestionControl);
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(pParametro.SolicitudesPagos, false, this.gvDatos, true);
            if (pParametro.OrdenesPagosTiposRetenciones.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<CapOrdenesPagosTiposRetenciones>(pParametro.OrdenesPagosTiposRetenciones, false, this.gvRetenciones, true);
                this.pnlRetenciones.Visible = true;
            }
            //CARGO GRILLA ANTICIPO
            if (this.MiOrdenPago.SolicitudesAnticipos.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(pParametro.SolicitudesAnticipos, false, this.gvAnticipo, true);
                this.pnlAnticipo.Visible = true;
                this.upAnticipos.Update();
            }
            this.ctrFechaCajaContable.IniciarControl(Gestion.Consultar, this.MiOrdenPago.FechaPago);
            this.ctrOrdenesPagosValores.IniciarControl(this.MiOrdenPago, this.GestionControl);
            this.ctrAsientoMostrar.IniciarControl(this.MiOrdenPago);
            this.ctrCamposValores.IniciarControl(this.MiOrdenPago, new Objeto(), this.GestionControl);
            this.ctrArchivos.IniciarControl(this.MiOrdenPago, this.GestionControl);
        }

        private void MapearControlesObjeto(CapOrdenesPagos pParametro)
        {
            this.MiOrdenPago.FechaAlta = Convert.ToDateTime(this.txtFechaAlta.Text);
            //this.MiOrdenPago.TipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);
            this.MiOrdenPago.FilialPago.IdFilialPago = Convert.ToInt32(this.ddlFilialPago.SelectedValue);
            //this.MiOrdenPago.BancoCuenta.IdBancoCuenta = this.ddlBancosCuentas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancosCuentas.SelectedValue);
            //this.MiOrdenPago.NumeroCheque = this.txtNumeroCheque.Text;
            this.MiOrdenPago.OrdenesPagosValore = this.ctrOrdenesPagosValores.ObtenerOrdenesPagosValores();
            this.MiOrdenPago.Comentarios = this.ctrComentarios.ObtenerLista();
            this.MiOrdenPago.FechaPago = this.ctrFechaCajaContable.dFechaCajaContable;
            MiOrdenPago.Campos = this.ctrCamposValores.ObtenerLista();
            this.MiOrdenPago.Archivos = ctrArchivos.ObtenerLista();
        }

        protected void button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.hdfIdAfiliado.Value))
            {
                //MiAfiliado = new AfiAfiliados();
                //MiAfiliado.IdAfiliado = ddlNumeroSocio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlNumeroSocio.SelectedValue);
                //MiAfiliado = AfiliadosF.AfiliadosObtenerDatos(MiAfiliado);
                //string txtRefEntidad = this.hdfIdRefEntidad.Value;
                string txtNumeroProveedor = this.hdfIdAfiliado.Value;
                MiOrdenPago.SolicitudesPagos.Clear();
                gvDatos.DataSource = MiOrdenPago.SolicitudesPagos;
                gvDatos.DataBind();
               

                if (ddlEntidades.SelectedValue == ((int)EnumTGEEntidades.Afiliados).ToString())
                {
                    MiAfiliado = new AfiAfiliados();
                    MiAfiliado.IdAfiliado = txtNumeroProveedor == string.Empty ? 0 : Convert.ToInt32(txtNumeroProveedor);
                    //MiAfiliado.RazonSocial = this.hdfNumeroProveedor.Value;
                    MiAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(MiAfiliado);

                    if (MiAfiliado.IdAfiliado != 0)
                    {
                        this.ddlNumeroSocio.Items.Add(new ListItem(MiAfiliado.DescripcionAfiliado.ToString(), MiAfiliado.IdAfiliado.ToString()));
                        this.ddlNumeroSocio.SelectedValue = MiAfiliado.IdAfiliado.ToString();
                        txtBeneficiario.Text = MiAfiliado.ApellidoNombre;
                        txtCuit.Text = MiAfiliado.CUIL.ToString();
                        this.MiOrdenPago.Beneficiario = MiAfiliado.ApellidoNombre;
                        MiOrdenPago.Entidad.IdEntidad = 188;
                        this.MiOrdenPago.Entidad.IdRefEntidad = MiAfiliado.IdAfiliado;
                        this.MiOrdenPago.Entidad.Cuit = MiAfiliado.CUIL.ToString();
                        this.ddlNumeroSocio.SelectedValue = MiAfiliado.IdAfiliado.ToString();//MiAfiliado.NumeroSocio;
                        this.txtBeneficiario.Text = MiAfiliado.ApellidoNombre;
                        this.txtCuit.Text = MiAfiliado.CUILFormateado;
                        this.ctrOrdenesPagosValores.IniciarControl(this.MiOrdenPago, this.GestionControl);
                    }
                    else
                    {
                        this.ddlNumeroSocio.Items.Clear();
                        this.ddlNumeroSocio.SelectedValue = null;
                        this.ddlNumeroSocio.ClearSelection();
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                        this.txtCuit.Text = string.Empty;
                        txtBeneficiario.Text = string.Empty;
                        MiAfiliado.CodigoMensaje = "El cliente no existe";
                        this.MostrarMensaje(MiAfiliado.CodigoMensaje, true);
                    }
                }
                else if (ddlEntidades.SelectedValue == ((int)EnumTGEEntidades.Proveedores).ToString())
                {
                    MiProveedor = new CapProveedores();
                    MiProveedor.IdProveedor = txtNumeroProveedor == string.Empty ? 0 : Convert.ToInt32(txtNumeroProveedor);
                    MiProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(MiProveedor);

                    if (MiProveedor.IdProveedor != 0)
                    {
                        this.ddlNumeroSocio.Items.Add(new ListItem(MiProveedor.RazonSocial.ToString(), MiProveedor.IdProveedor.ToString()));
                        this.ddlNumeroSocio.SelectedValue = MiProveedor.IdProveedor.ToString();
                        txtBeneficiario.Text = MiProveedor.BeneficiarioDelCheque;
                        txtCuit.Text = MiProveedor.CUIT.ToString();
                        MiOrdenPago.Entidad.IdEntidad = 187;
                        this.MiOrdenPago.Beneficiario = MiProveedor.RazonSocial;
                        this.MiOrdenPago.Entidad.IdRefEntidad = MiProveedor.IdProveedor.Value;
                        this.MiOrdenPago.Entidad.IdCuentaContable = MiProveedor.CuentaContable.IdCuentaContable;
                        this.MiOrdenPago.Entidad.Cuit = MiProveedor.CUIT;
                        this.ddlNumeroSocio.SelectedValue = MiProveedor.IdProveedor.ToString();
                        this.txtBeneficiario.Text = MiProveedor.RazonSocial;
                        this.txtCuit.Text = MiProveedor.CUIT;
                        this.ctrOrdenesPagosValores.IniciarControl(this.MiOrdenPago, this.GestionControl);
                    }
                    else
                    {
                        this.ddlNumeroSocio.Items.Clear();
                        this.ddlNumeroSocio.SelectedValue = null;
                        this.ddlNumeroSocio.ClearSelection();
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                        this.txtCuit.Text = string.Empty;
                        txtBeneficiario.Text = string.Empty;
                        MiAfiliado.CodigoMensaje = "El Proveedor no existe";
                        this.MostrarMensaje(MiAfiliado.CodigoMensaje, true);
                    }
                }
                this.IniciarControlAgregar(this.MiOrdenPago);
                BuscarAfiliado?.Invoke(MiAfiliado);
            }
            else
            {
                MiOrdenPago.SolicitudesPagos.Clear();
                MiOrdenPago.SolicitudesAnticipos.Clear();
                txtBeneficiario.Text = "";
                txtCuit.Text = "";
                this.ctrOrdenesPagosValores.IniciarControl(new CapOrdenesPagos(), this.GestionControl);
                
                MiOrdenPago.Entidad.IdEntidad = 0;
                gvDatos.DataSource = MiOrdenPago.SolicitudesPagos;
                gvDatos.DataBind();
                gvAnticipo.DataSource = MiOrdenPago.SolicitudesAnticipos;
                gvAnticipo.DataBind();
                txtImporteAPagar.Text = "0.00";
                this.ddlNumeroSocio.Items.Clear();
                this.upAnticipos.Update();
                this.upEntidades.Update();
                this.upOrdenPagoDetalle.Update();
                Calcular();
               
            }
        }

        protected void ddlEntidades_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlEntidades.SelectedValue))
            {
                CapOrdenesPagos parametros = BusquedaParametrosObtenerValor<CapOrdenesPagos>();
                parametros.Entidad.IdEntidad = Convert.ToInt32(ddlEntidades.SelectedValue);
                txtBeneficiario.Text = string.Empty;
                txtCuit.Text = string.Empty;
                ddlNumeroSocio.Items.Clear();
                ddlNumeroSocio.SelectedValue = null;
                ddlNumeroSocio.ClearSelection();
                IniciarControlAgregar(parametros);
                //this.ctrOrdenesPagosValores.IniciarControl(parametros, this.GestionControl);
            }
        }

        private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        {
            this.ddlNumeroSocio.Items.Add(new ListItem(pAfiliado.DescripcionAfiliado, pAfiliado.IdAfiliado.ToString()));
            this.ddlNumeroSocio.SelectedValue = pAfiliado.IdAfiliado.ToString();

            this.lblCuit.Text = pAfiliado.TipoDocumento.TipoDocumento;
            this.txtCuit.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtBeneficiario.Text = pAfiliado.ApellidoNombre;


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

        private void IniciarControlAgregar(CapOrdenesPagos pOrdenPago)
        {
            this.MiOrdenPago.SolicitudesPagos = CuentasPagarF.SolicitudPagoObtenerPendientePago(pOrdenPago);
            if (this.MiOrdenPago.Entidad.IdEntidad == (int)EnumTGEEntidades.Proveedores)
                this.MiOrdenPago.SolicitudesAnticipos = CuentasPagarF.SolicitudPagoAnticipoObtenerPendientesPorProveedor(this.MiOrdenPago);

            if (hdfCalcularRetenciones.Value == "1" && MiOrdenPago.Entidad.IdRefEntidad > 0)
            {
                this.Calcular();
                this.btnCalcularRetencionesr_Click(btnCalcularRetenciones, EventArgs.Empty);
            }

            this.IniciarControlAgregarDatos(pOrdenPago);
        }

        private void IniciarControlAgregarDatos(CapOrdenesPagos pOrdenPago)
        {
            
            this.Calcular();
            //decimal importeAPagar = this.MiOrdenPago.SolicitudesPagos.Where(x => x.IncluirEnOP && x.ImporteParcial > 0).Sum(x => x.ImporteParcial); //this.MiOrdenPago.ImporteSubTotal > 0 ? this.MiOrdenPago.ImporteSubTotal : 0;
            decimal importeAPagar = this.MiOrdenPago.SolicitudesPagos.Where(x => x.IncluirEnOP).Where(x => x.ImporteParcial > 0).Sum(x => x.ImporteParcial); //this.MiOrdenPago.ImporteSubTotal > 0 ? this.MiOrdenPago.ImporteSubTotal : 0;
            this.txtImporteAPagar.Text = importeAPagar.ToString("C2");
            this.hdfImporteAPagar.Value = importeAPagar.ToString();
            this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
            this.txtFechaAlta.Enabled = true;
            this.ctrCamposValores.IniciarControl(pOrdenPago, new Objeto(), this.GestionControl);
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(pOrdenPago.SolicitudesPagos, false, this.gvDatos, true);

            //CARGO GRILLA ANTICIPO
            if (this.MiOrdenPago.SolicitudesAnticipos.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(pOrdenPago.SolicitudesAnticipos, false, this.gvAnticipo, true);
                this.pnlAnticipo.Visible = true;
                this.upAnticipos.Update();
            }

            //this.ddlEntidades.Enabled = false;
            this.upEntidades.Update();

            this.upOrdenPagoDetalle.Update();

            this.phBotones.Visible = true;
            this.upBotonesMenu.Update();
        }

        #region Grilla Solicitudes

        private void PersistirDatosGrilla()
        {

            if (this.MiOrdenPago.SolicitudesPagos.Count == 0)
                return;
            
            //decimal importeTotalAPagar = this.txtImporteAPagar.Decimal;
            
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                this.MiOrdenPago.SolicitudesPagos[fila.DataItemIndex].IncluirEnOP = chkIncluir.Checked;
                decimal importeParcial = ((CurrencyTextBox)fila.FindControl("txtImporteParcial")).Decimal;
                //decimal importeAPagar = 0;
                //if (chkIncluir.Checked)
                //{
                //    if (importeTotalAPagar > 0)
                //    {
                //        importeAPagar = importeTotalAPagar > importeParcial ? importeParcial : importeTotalAPagar;
                //        importeTotalAPagar -= importeAPagar;
                //    }
                //}
                if (!chkIncluir.Checked)
                    importeParcial = 0;

                this.MiOrdenPago.SolicitudesPagos[fila.DataItemIndex].ImporteParcial = importeParcial;
            }
            //AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MiOrdenPago.SolicitudesPagos, false, this.gvDatos, true);
            //this.Calcular();
            //this.MiOrdenPago.ImporteSubTotal = this.MiOrdenPago.SolicitudesPagos.Where(x => x.IncluirEnOP).Sum(x => x.ImporteParcial);
            //this.txtImporteAImputar.Text = (this.txtImporteAPagar.Decimal - this.MiOrdenPago.ImporteSubTotal).ToString("C2"); 
        }

        private void Calcular()
        {
            this.MiOrdenPago.ImporteSubTotal = this.MiOrdenPago.SolicitudesPagos.Where(x => x.IncluirEnOP).Sum(x => x.ImporteParcial);
            this.MiOrdenPago.ImporteRetenciones = this.MiOrdenPago.OrdenesPagosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
            decimal totalAnticipo = this.MiOrdenPago.SolicitudesAnticipos.Where(x => x.IncluirEnOP == true).Sum(x => x.ImporteTotal);
            this.MiOrdenPago.ImporteTotal = this.MiOrdenPago.ImporteSubTotal - this.MiOrdenPago.ImporteRetenciones - totalAnticipo;
            this.txtSubTotal.Text = this.MiOrdenPago.ImporteSubTotal.ToString("C2");
            this.txtAnticipo.Text = totalAnticipo.ToString("C2");
            this.txtImporteRetenciones.Text = this.MiOrdenPago.ImporteRetenciones.ToString("C2");
            this.txtTotalAPagar.Text = this.MiOrdenPago.ImporteTotal.ToString("C2") ;
            //Dejo el foco en el tipo de valor a ingresar
            // Lo comento porque si estas seleccionando items se va para abajo
            //this.ctrOrdenesPagosValores.FindControl("ddlTiposValores").Focus();
            this.upTotales.Update();
        }

        //protected void txtImporteAPagar_TextChanged(object sender, EventArgs e)
        //{
        //    this.PersistirDatosGrilla();
        //    //this.CalcularRetenciones();
        //}

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluirTodos");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Checked = this.MiCheckIncluirTodos;
                    this.MiCheckTodosPrimeraVez = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapSolicitudPago solPago = (CapSolicitudPago)e.Row.DataItem;
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    //ibtnConsultar.Attributes.Add("onchange", "CalcularItem();");
                    if (solPago.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosAFIP
                        || solPago.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosSubsidios
                        || solPago.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosCompras
                        || solPago.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosInternos
                        )
                    {
                        TextBox importeParcial = (TextBox)e.Row.FindControl("txtImporteParcial");

                        importeParcial.Enabled = true;
                        importeParcial.Attributes.Add("onchange", "CalcularItem();");
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                Label lblImporteSubTotal = (Label)e.Row.FindControl("lblImporteSubTotal");
                if (this.GestionControl == Gestion.Agregar)
                {
                    //lblImporteTotal.Text = this.MiOrdenPago.SolicitudesPagos.Sum(x => x.ImporteParcial).ToString("C2");
                    lblImporteSubTotal.Text = this.MiOrdenPago.SolicitudesPagos.Where(x => x.IncluirEnOP).Sum(x => x.ImporteParcial).ToString("C2");
                }
                else
                {
                    lblImporteSubTotal.Text = this.MiOrdenPago.ImporteSubTotal.ToString("C2");
                }
            }
        }

        //protected void chkIncluirTodos_CheckedChanged(Object sender, EventArgs args)
        //{
        //    if (this.MiOrdenPago.SolicitudesPagos.Count == 0)
        //        return;
        //    bool check = ((CheckBox)sender).Checked;
        //    this.MiCheckIncluirTodos = check;

        //    foreach (GridViewRow fila in this.gvDatos.Rows)
        //    {
        //        CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
        //        chkIncluir.Checked = check;
        //    }
        //    this.PersistirDatosGrilla();
        //    this.CalcularRetenciones();
        //}

        //protected void chkIncluir_CheckedChanged(Object sender, EventArgs args)
        //{
        //    this.PersistirDatosGrilla();
        //    //this.CalcularRetenciones();
        //}

        //protected void txtImporteParcial_TextChanged(object sender, EventArgs e)
        //{
        //    this.PersistirDatosGrilla();
        //    //this.CalcularRetenciones();
        //}

        protected void btnComprobantesCompras_Click(object sender, EventArgs e)
        {
            TGEParametrosValores valor;
            valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CalcularRetencionesAutomatico);
            if (valor.ParametroValor.ToLower() == "true")
            {
                hdfCalcularRetenciones.Value = "1";
            }
            this.IniciarControlAgregar(MiOrdenPago);
        }
        #endregion

        #region Grilla Retenciones
        protected void gvRetenciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapOrdenesPagosTiposRetenciones ordenesPagosTiposRetenciones = (CapOrdenesPagosTiposRetenciones)e.Row.DataItem;
                GridView gvDatosDetalles = e.Row.FindControl("gvDatosDetalles") as GridView;
                gvDatosDetalles.ShowHeaderWhenEmpty = true;
                gvDatosDetalles.DataSource = ordenesPagosTiposRetenciones.OrdenesPagosTiposRetencionesDetalle;
                gvDatosDetalles.DataBind();
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                        btnEliminar.Visible = true;
                        string mensaje = this.ObtenerMensajeSistema("ConfirmarEliminar");
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                        btnEliminar.Attributes.Add("OnClick", funcion);
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotalRetencion = (Label)e.Row.FindControl("lblImporteTotalRetencion");
                lblImporteTotalRetencion.Text = this.MiOrdenPago.ImporteRetenciones.ToString("C2");
            }
        }

        protected void gvRetenciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiOrdenPago.OrdenesPagosTiposRetenciones.RemoveAt(indiceColeccion);
                this.MiOrdenPago.OrdenesPagosTiposRetenciones = AyudaProgramacion.AcomodarIndices<CapOrdenesPagosTiposRetenciones>(this.MiOrdenPago.OrdenesPagosTiposRetenciones);
                this.MiOrdenPago.ImporteRetenciones = this.MiOrdenPago.OrdenesPagosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
                AyudaProgramacion.CargarGrillaListas<CapOrdenesPagosTiposRetenciones>(this.MiOrdenPago.OrdenesPagosTiposRetenciones, false, this.gvRetenciones, true);
                this.upRetenciones.Update();
                this.CalcularTotalesRetenciones();   
            }
        }

        protected void btnCalcularRetencionesr_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            decimal subtotal = this.txtSubTotal.Decimal;
            this.hdfImporteCalculoRetenciones.Value = subtotal.ToString();
            this.MiOrdenPago.FechaAlta = Convert.ToDateTime(this.txtFechaAlta.Text);
            this.MiOrdenPago.FechaPago = this.ctrFechaCajaContable.dFechaCajaContable;
            this.MiOrdenPago.OrdenesPagosTiposRetenciones = CuentasPagarF.OrdenesPagosRetencionesObtenerCalculosRetenciones(this.MiOrdenPago);
            this.MiOrdenPago.ImporteRetenciones = this.MiOrdenPago.OrdenesPagosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
            AyudaProgramacion.CargarGrillaListas<CapOrdenesPagosTiposRetenciones>(this.MiOrdenPago.OrdenesPagosTiposRetenciones, false, this.gvRetenciones, true);
            this.upRetenciones.Update();
            this.CalcularTotalesRetenciones();
        }

        private void CalcularTotalesRetenciones()
        {
            this.MiOrdenPago.ImporteSubTotal = this.txtSubTotal.Decimal;
            this.txtImporteRetenciones.Text = this.MiOrdenPago.ImporteRetenciones.ToString("C2");
            decimal totalAnticipo = this.txtAnticipo.Decimal;
            this.MiOrdenPago.ImporteTotal = this.MiOrdenPago.ImporteSubTotal - this.MiOrdenPago.ImporteRetenciones - totalAnticipo;
            this.txtTotalAPagar.Text = this.MiOrdenPago.ImporteTotal.ToString("C2");
            //Dejo el foco en el tipo de valor a ingresar
            // Lo comento porque si estas seleccionando items se va para abajo
            //this.ctrOrdenesPagosValores.FindControl("ddlTiposValores").Focus();
            this.upTotales.Update();
        }

        #endregion

        #region Grilla Anticipos

        private void PersistirDatosAnticipo()
        {

            if (this.MiOrdenPago.SolicitudesAnticipos.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvAnticipo.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluirAnticipo");
                this.MiOrdenPago.SolicitudesAnticipos[fila.DataItemIndex].IncluirEnOP = chkIncluir.Checked;
            }
        }

        protected void gvAnticipo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapSolicitudPago solPago = (CapSolicitudPago)e.Row.DataItem;
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluirAnticipo");
                    ibtnConsultar.Visible = true;
                    //ibtnConsultar.Attributes.Add("onchange", "CalcularItem();");

                    //TextBox importeParcial = (TextBox)e.Row.FindControl("txtImporteParcial");
                    //if (!(solPago.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
                    //    || solPago.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
                    //    || solPago.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC))
                    //    importeParcial.Enabled = true;
                    //importeParcial.Attributes.Add("onchange", "CalcularItem();");
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteSubTotalAnticipo");
                if (this.GestionControl == Gestion.Agregar)
                {
                    lblImporteTotal.Text = this.MiOrdenPago.SolicitudesAnticipos.Where(x => x.IncluirEnOP).Sum(x => x.ImporteTotal).ToString("C2");
                }
                else
                {
                    lblImporteTotal.Text = this.MiOrdenPago.SolicitudesAnticipos.Sum(x => x.ImporteTotal).ToString("C2");
                }
            }
        }

        #endregion

        #region Anticipos Proveedores
        protected void btnAnticipoProveedor_Click(object sender, EventArgs e)
        { }

        #endregion

        #region Anticipos Turismo
        protected void btnAnticipoTurismo_Click(object sender, EventArgs e)
        {
            if (MiProveedor != null && MiProveedor.IdProveedor.HasValue)
            {
                this.CtrAnticiposTurismo_AnticiposTurismoBuscarSeleccionar();
                this.ctrAnticiposTurismo.IniciarControl(MiProveedor);
            }
        }

        private void CtrAnticiposTurismo_AnticiposTurismoBuscarSeleccionar()
        {
            this.hdfCalcularRetenciones.Value = "0";
            MiOrdenPago.Filtro = "CarTiposCargosAfiliadosFormasCobros";
            this.MiOrdenPago.SolicitudesPagos = CuentasPagarF.SolicitudPagoObtenerPendientePago(MiOrdenPago);
            MiOrdenPago.Filtro = string.Empty;
            this.MiOrdenPago.SolicitudesAnticipos = new List<CapSolicitudPago>();
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(MiOrdenPago.SolicitudesAnticipos, false, this.gvAnticipo, true);
            this.pnlAnticipo.Visible = false;
            this.upAnticipos.Update();
            this.MiOrdenPago.OrdenesPagosTiposRetenciones = new List<CapOrdenesPagosTiposRetenciones>();
            AyudaProgramacion.CargarGrillaListas<CapOrdenesPagosTiposRetenciones>(this.MiOrdenPago.OrdenesPagosTiposRetenciones, false, this.gvRetenciones, true);
            this.pnlRetenciones.Visible = false;
            this.upRetenciones.Update();
            this.IniciarControlAgregarDatos(MiOrdenPago);
        }

        #endregion

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.OrdenesDePagoDatosCancelar != null)
                this.OrdenesDePagoDatosCancelar();
        }

        protected void btngrabar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("OrdenesDePagoDatos");
            if (!this.Page.IsValid)
                return;
            bool guardo = true;

            this.MiOrdenPago.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.PersistirDatosGrilla();
                    this.PersistirDatosAnticipo();
                    this.MapearControlesObjeto(this.MiOrdenPago);
                    this.MiOrdenPago.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenPago.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiOrdenPago.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;

                    this.MiOrdenPago.ImporteSubTotal = this.MiOrdenPago.SolicitudesPagos.Where(x => x.IncluirEnOP).Sum(x => x.ImporteParcial);
                    this.MiOrdenPago.ImporteRetenciones = this.MiOrdenPago.OrdenesPagosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
                    decimal totalAnticipo = this.MiOrdenPago.SolicitudesAnticipos.Where(x => x.IncluirEnOP == true).Sum(x => x.ImporteTotal);
                    this.MiOrdenPago.ImporteTotal = this.MiOrdenPago.ImporteSubTotal - this.MiOrdenPago.ImporteRetenciones - totalAnticipo;

                    if (this.MiOrdenPago.ImporteRetenciones > 0
                        && this.MiOrdenPago.ImporteSubTotal != Convert.ToDecimal(this.hdfImporteCalculoRetenciones.Value))
                    {
                        this.MostrarMensaje("ValidarRetencionesCalculo", true, new List<string>() { (Convert.ToDecimal(this.hdfImporteCalculoRetenciones.Value)).ToString("C2") });
                        return;
                    }

                    guardo = CuentasPagarF.OrdenesPagosAgregar(this.MiOrdenPago);
                    break;
                case Gestion.Autorizar:
                    this.MapearControlesObjeto(this.MiOrdenPago);
                    this.MiOrdenPago.EstadoColeccion = EstadoColecciones.Modificado;
                    this.MiOrdenPago.IdUsuarioAutorizacion = this.UsuarioActivo.IdUsuario;
                    this.MiOrdenPago.FechaAutorizacion = DateTime.Now;
                    this.MiOrdenPago.Estado.IdEstado = (int)EstadosOrdenesPago.Autorizado;
                    guardo = CuentasPagarF.OrdenesPagosAutorizar(this.MiOrdenPago);
                    break;
                case Gestion.Modificar:
                    this.MapearControlesObjeto(this.MiOrdenPago);
                    this.MiOrdenPago.EstadoColeccion = EstadoColecciones.Modificado;
                    this.MiOrdenPago.IdUsuarioAutorizacion = this.UsuarioActivo.IdUsuario;
                    this.MiOrdenPago.FechaAutorizacion = DateTime.Now;
                    this.MiOrdenPago.Estado.IdEstado = (int)EstadosOrdenesPago.Autorizado;
                    guardo = CuentasPagarF.OrdenesPagosModificar(this.MiOrdenPago);
                    break;
                case Gestion.Anular:
                    this.MapearControlesObjeto(this.MiOrdenPago);
                    this.MiOrdenPago.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenPago.Estado.IdEstado = (int)EstadosOrdenesPago.Baja;
                    guardo = CuentasPagarF.OrdenesPagosAnular(this.MiOrdenPago);
                    break;
                case Gestion.AnularConfirmar:
                    this.MapearControlesObjeto(this.MiOrdenPago);
                    this.MiOrdenPago.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenPago.Estado.IdEstado = (int)EstadosOrdenesPago.Baja;
                    guardo = CuentasPagarF.OrdenesPagosAnularPagada(this.MiOrdenPago);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.btnImprimir.Visible = true;
                this.MostrarMensaje(this.MiOrdenPago.CodigoMensaje, false);
                this.ctrAsientoMostrar.IniciarControl(this.MiOrdenPago);
            }
            else
            {
                this.MostrarMensaje(this.MiOrdenPago.CodigoMensaje, true, this.MiOrdenPago.CodigoMensajeArgs);
                if (this.MiOrdenPago.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiOrdenPago);
                    this.MiOrdenPago.dsResultado = null;
                }
            }

        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            //this.ctrPopUpComprobantes.CargarReporte(this.MiOrdenPago, EnumTGEComprobantes.CapOrdenesPagos);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapOrdenesPagos, "OrdenesPago", this.MiOrdenPago, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.upAcciones, "OrdenesPago", this.UsuarioActivo);
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.OrdenesDePagoDatosAceptar != null)
                this.OrdenesDePagoDatosAceptar(this.MiOrdenPago);
        }

        protected void btnFirmaDigital_Click(object sender, EventArgs e)
        {
            TGEParametrosValores paramVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FirmaDigitalHabilitar);
            bool firmaDigital = paramVal.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramVal.ParametroValor);
            if (!firmaDigital)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFirmaDigitalManuscritaHabilitar"), true);
                return;
            }

            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = MiOrdenPago.IdOrdenPago;
            PropertyInfo prop = MiOrdenPago.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            firmarDoc.Key = prop.Name;
            firmarDoc.CodigoPlantilla = "OrdenesPago";
            firmarDoc.Tabla = "CapOrdenesPagos";
            firmarDoc.OcultarCaptcha = true;
            firmarDoc.UrlReferrer = true;


            var URL = TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link;
            string[] sURL = URL.Split('?');
            if (sURL.Length > 1)
            {
                URL = AyudaProgramacion.ObtenerUrlParametros(sURL[0]) + sURL[1];
            }
            this.Response.Redirect(URL, true);
        }

        protected void btnFirmaDigitalBaja_Click(object sender, EventArgs e)
        {
            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = this.MiOrdenPago.IdOrdenPago;
            firmarDoc.Tabla = "CapOrdenesPagos";
            firmarDoc.IdTipoOperacion = 0;
            firmarDoc.Estado.IdEstado = (int)Estados.Baja;
            bool resultado = TGEGeneralesF.FirmarDocumentosModificar(firmarDoc);
            if (resultado)
            {
                this.btnFirmaDigitalBaja.Visible = false;
                this.btnFirmaDigital.Visible = true;
                this.copyClipboard.Visible = true;
                this.btnWhatsAppFirmarDocumento.Visible = true;
                this.MostrarMensaje(firmarDoc.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(firmarDoc.CodigoMensaje, true, firmarDoc.CodigoMensajeArgs);
            }
        }

        protected void btnWhatsAppFirmarDocumento_Click(object sender, EventArgs e)
        {
            TGEParametrosValores paramVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FirmaDigitalHabilitar);
            bool firmaDigital = paramVal.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramVal.ParametroValor);
            if (!firmaDigital)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFirmaDigitalManuscritaHabilitar"), true);
                return;
            }

            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = MiOrdenPago.IdOrdenPago;
            PropertyInfo prop = MiOrdenPago.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            firmarDoc.Key = prop.Name;
            firmarDoc.CodigoPlantilla = "OrdenesPago";
            firmarDoc.Tabla = "CapOrdenesPagos";
            firmarDoc.OcultarCaptcha = true;
            firmarDoc.UrlReferrer = true;

            string text = string.Concat("Estimado ", MiOrdenPago.Entidad.Nombre, " haga clic en el siguiente link para firmar el documento ",TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link);
            string numero = MiOrdenPago.Entidad.TelefonoCelular.ToString();
            string urlwa = string.Format("https://api.whatsapp.com/send?phone={0}&text={1}", numero, HttpUtility.UrlEncode(text));
            ScriptManager.RegisterStartupScript(this.upAcciones, this.upAcciones.GetType(), "scriptWa", string.Format("EnviarWhatsApp('{0}');", urlwa), true);
        }
    }
}