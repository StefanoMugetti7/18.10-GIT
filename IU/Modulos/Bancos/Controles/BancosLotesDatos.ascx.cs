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
using Tesorerias.Entidades;
using Comunes.Entidades;
using Tesorerias;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Bancos.Entidades;
using Bancos;
using System.Collections.Generic;
using CuentasPagar.Entidades;

namespace IU.Modulos.Bancos.Controles
{
    public partial class BancosLotesDatos : ControlesSeguros
    {
        private bool MiCheckTodosPrimeraVez
        {
            get { return (bool)Session[this.MiSessionPagina + "BancosLotesDatosMiCheckTodosPrimeraVez"]; }
            set { Session[this.MiSessionPagina + "BancosLotesDatosMiCheckTodosPrimeraVez"] = value; }
        }

        private bool MiCheckIncluirTodos
        {
            get { return (bool)Session[this.MiSessionPagina + "BancosLotesDatosMiCheckIncluirTodos"]; }
            set { Session[this.MiSessionPagina + "BancosLotesDatosMiCheckIncluirTodos"] = value; }
        }

        private TESBancosLotesEnviados MiBancoLotesEnviados
        {
            get { return (TESBancosLotesEnviados)Session[this.MiSessionPagina + "BancosLotesEnviadosDatosMiBancoLotesEnviados"]; }
            set { Session[this.MiSessionPagina + "BancosLotesEnviadosDatosMiBancoLotesEnviados"] = value; }
        }

        public delegate void BancoLotesEnviadosDatosAceptarEventHandler(object sender, TESBancosLotesEnviados e);
        public event BancoLotesEnviadosDatosAceptarEventHandler BancoLotesEnviadosModificarDatosAceptar;

        public delegate void BancoLotesEnviadosDatosCancelarEventHandler();
        public event BancoLotesEnviadosDatosCancelarEventHandler BancoLotesEnviadosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                
                //AyudaProgramacion.CargarGrillaListas<TESBancosLotesEnviadosDetalle>(this.MiBancoLotesEnviados.Detalles, false, this.gvDatos, true);
                //if (this.MiBancoCuenta == null && this.GestionControl != Gestion.Agregar)
                //{
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                //}
            }
        }
        private void CargarEstadosGrilla()
        {
            var data1 = TGEGeneralesF.TGEEstadosObtenerLista("TESBancosLotesEnviadosDetalles");
            if(data1.Count > 0)
            {
                //this.hdfMensaje1.Value = data[0].Descripcion == null ? "" : data[0].Descripcion;
                //this.hdfMensaje2.Value = data[1].Descripcion == null ? "" : data[1].Descripcion;
                this.hdfMensaje1.Value = data1.FirstOrDefault(x=>x.IdEstado == (int)EstadosBancosLotesEnviadosDetalles.Conciliado).Descripcion;
                this.hdfMensaje2.Value = data1.FirstOrDefault(x => x.IdEstado == (int)EstadosBancosLotesEnviadosDetalles.Rechazado).Descripcion;
            }
        }
        public void IniciarControl(TESBancosLotesEnviados pBancoLotesEnviados, Gestion pGestion)
        {
            this.hdfGestionConciliar.Value = "0";
            this.MiCheckTodosPrimeraVez = true;
            this.MiCheckIncluirTodos = true;
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiBancoLotesEnviados = pBancoLotesEnviados;
            this.CargarEstadosGrilla();
            //AyudaProgramacion.CargarGrillaListas<TESBancosLotesEnviadosDetalle>(this.MiBancoLotesEnviados.Detalles, false, this.gvDatos, true);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, pBancoLotesEnviados.UsuarioAlta);
                    this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ddlEstado.Enabled = false;
                    this.CargarTiposOperaciones();
                    this.CargarDetalles();
                    CargarImportesJS();
                    break;
                case Gestion.Modificar:
                    this.MiBancoLotesEnviados = BancosF.BancosLotesEnviadosObtenerDatosCompletos(pBancoLotesEnviados);
                    this.MapearObjetoAControles(this.MiBancoLotesEnviados);
                    CargarDetallesCargados(MiBancoLotesEnviados.Detalles);
                    CargarImportesJS();
                    this.txtFechaAlta.Enabled = false;
                    this.btnBuscarFiltro.Visible = false;
                    this.ddlTipoOperacion.Visible = false;
                    this.txtFechaDesde.Visible = false;
                    this.txtFechaHasta.Visible = false;
                    this.lblFechaDesde.Visible = false;
                    this.lblFechaHasta.Visible = false;
                    this.lblTipoOperacion.Visible = false;
                    if (this.MiBancoLotesEnviados.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado)
                    {
                        this.ddlEstado.Enabled = false;
                        this.ddlTipoArchivo.Enabled = false;
                    }

                    break;
                case Gestion.Consultar:
                    this.MiBancoLotesEnviados = BancosF.BancosLotesEnviadosObtenerDatosCompletos(pBancoLotesEnviados);
                    CargarDetallesCargados(MiBancoLotesEnviados.Detalles);
                    this.MapearObjetoAControles(this.MiBancoLotesEnviados);
                    this.ddlEstado.Enabled = false;
                    this.ddlBancosCuentas.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.txtFechaAutorizado.Enabled = false;
                    this.gvDatos.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.btnBuscarFiltro.Visible = false;
                    this.ddlTipoOperacion.Visible = false;
                    this.txtFechaDesde.Visible = false;
                    this.txtFechaHasta.Visible = false;
                    this.lblFechaDesde.Visible = false;
                    this.lblFechaHasta.Visible = false;
                    this.lblTipoOperacion.Visible = false;
                    this.txtObservacion.Enabled = false;
                    this.ddlTipoArchivo.Enabled = false;
                    this.txtNroTramite.Enabled = false;
                    this.btnExportarTxt.Visible = false;
                    this.btnExportarExcel.Visible = false;

                    if (MiBancoLotesEnviados.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado)
                    {
                        this.btnExportarTxt.Visible = true;
                        this.btnExportarExcel.Visible = true;
                    }

                    if(MiBancoLotesEnviados.CantidadRegistrosConciliado > 0 )
                    {
                        this.lblNroTramite.Visible = true;
                        this.txtNroTramite.Visible = true;
                        this.lblCantidadRegistrosConciliados.Visible = true;
                        this.txtCantidadRegistrosConciliado.Visible = true;
                        this.lblImporteTotalConciliado.Visible = true;
                        this.txtImporteTotalConciliado.Visible = true;
                        this.btnExportarTxt.Visible = true;
                        this.btnExportarExcel.Visible = true;
                    }
                    break;
                case Gestion.Autorizar:
                    this.MiBancoLotesEnviados = BancosF.BancosLotesEnviadosObtenerDatosCompletos(pBancoLotesEnviados);
                    this.MapearObjetoAControles(this.MiBancoLotesEnviados);
                    CargarDetallesCargados(MiBancoLotesEnviados.Detalles);
                    this.txtNumeroLote.Text = this.MiBancoLotesEnviados.IdBancoLoteEnvio.ToString();
                    this.ddlEstado.Enabled = false;
                    this.ddlBancosCuentas.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.txtFechaAutorizado.Enabled = false;
                    this.gvDatos.Enabled = false;
                    this.btnBuscarFiltro.Visible = false;
                    this.ddlTipoOperacion.Visible = false;
                    this.txtFechaDesde.Visible = false;
                    this.txtFechaHasta.Visible = false;
                    this.lblFechaDesde.Visible = false;
                    this.lblFechaHasta.Visible = false;
                    this.lblTipoOperacion.Visible = false;
                    this.rfvTipoArchivo.Enabled = true;
                    break;
                case Gestion.Anular:
                    this.MiBancoLotesEnviados = BancosF.BancosLotesEnviadosObtenerDatosCompletos(pBancoLotesEnviados);
                    this.MapearObjetoAControles(this.MiBancoLotesEnviados);
                    CargarDetallesCargados(MiBancoLotesEnviados.Detalles);
                    this.ddlEstado.Enabled = false;
                    this.ddlBancosCuentas.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.txtFechaAutorizado.Enabled = false;
                    this.gvDatos.Enabled = false;
                    this.btnBuscarFiltro.Visible = false;
                    this.ddlTipoOperacion.Visible = false;
                    this.txtFechaDesde.Visible = false;
                    this.txtFechaHasta.Visible = false;
                    this.lblFechaDesde.Visible = false;
                    this.lblFechaHasta.Visible = false;
                    this.lblTipoOperacion.Visible = false;
                    this.txtObservacion.Enabled = false;
                    this.ddlTipoArchivo.Enabled = false;
                    if (MiBancoLotesEnviados.CantidadRegistrosConciliado > 0)
                    {
                        this.lblNroTramite.Visible = true;
                        this.txtNroTramite.Visible = true;
                        this.lblCantidadRegistrosConciliados.Visible = true;
                        this.txtCantidadRegistrosConciliado.Visible = true;
                        this.lblImporteTotalConciliado.Visible = true;
                        this.txtImporteTotalConciliado.Visible = true;
                        this.btnExportarTxt.Visible = true;
                        this.btnExportarExcel.Visible = true;
                    }
                    break;
                case Gestion.ConfirmarAgregar:
                    this.MiBancoLotesEnviados = BancosF.BancosLotesEnviadosObtenerDatosCompletos(pBancoLotesEnviados);
                    this.MapearObjetoAControles(this.MiBancoLotesEnviados);
                    CargarDetallesCargados(MiBancoLotesEnviados.Detalles);
                    CargarImportesJS();
                    this.hdfGestionConciliar.Value = "1";
                    this.txtNumeroLote.Text = this.MiBancoLotesEnviados.IdBancoLoteEnvio.ToString();
                    this.ddlEstado.Enabled = false;
                    this.ddlBancosCuentas.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.txtFechaAutorizado.Enabled = false;
                    this.btnBuscarFiltro.Visible = false;
                    this.ddlTipoOperacion.Visible = false;
                    this.txtFechaDesde.Visible = false;
                    this.txtFechaHasta.Visible = false;
                    this.lblFechaDesde.Visible = false;
                    this.lblFechaHasta.Visible = false;
                    this.lblTipoOperacion.Visible = false;
                    this.lblCantidadRegistrosConciliados.Visible = true;
                    this.txtCantidadRegistrosConciliado.Visible = true;
                    this.lblImporteTotalConciliado.Visible = true;
                    this.txtImporteTotalConciliado.Visible = true;  
                    this.txtObservacion.Enabled = false;
                    this.ddlTipoArchivo.Enabled = false;
                    this.txtNroTramite.Enabled = true;
                    this.txtNroTramite.Visible = true;
                    this.lblNroTramite.Visible = true;
                    this.rfvNroTramite.Enabled = true;
                    this.txtFechaPago.Enabled = true;
                    this.rfvFechaPago.Enabled = true;
                    this.txtFechaPago.Text = DateTime.Now.ToShortDateString();
                    this.btnAceptar.Text = "Conciliar";
                    string funcion = string.Format("ValidarShowConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ValidarConciliarLoteBancos"));
                    this.btnAceptar.Attributes.Add("OnClick", funcion);
                    this.CargarImportesJS();
                    break;
                case Gestion.Cancelar:
                    this.MiBancoLotesEnviados = BancosF.BancosLotesEnviadosObtenerDatosCompletos(pBancoLotesEnviados);
                    this.MapearObjetoAControles(this.MiBancoLotesEnviados);
                    CargarDetallesCargados(MiBancoLotesEnviados.Detalles);
                    this.txtNumeroLote.Text = this.MiBancoLotesEnviados.IdBancoLoteEnvio.ToString();
                    this.ddlEstado.Enabled = false;
                    this.ddlBancosCuentas.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.txtFechaAutorizado.Enabled = false;
                    this.gvDatos.Enabled = false;
                    this.btnBuscarFiltro.Visible = false;
                    this.ddlTipoOperacion.Visible = false;
                    this.txtFechaDesde.Visible = false;
                    this.txtFechaHasta.Visible = false;
                    this.lblFechaDesde.Visible = false;
                    this.lblFechaHasta.Visible = false;
                    this.lblTipoOperacion.Visible = false;
                    this.txtObservacion.Enabled = false;
                    this.ddlTipoArchivo.Enabled = false;
                    if (MiBancoLotesEnviados.CantidadRegistrosConciliado > 0)
                    {
                        this.lblNroTramite.Visible = true;
                        this.txtNroTramite.Visible = true;
                        this.lblCantidadRegistrosConciliados.Visible = true;
                        this.txtCantidadRegistrosConciliado.Visible = true;
                        this.lblImporteTotalConciliado.Visible = true;
                        this.txtImporteTotalConciliado.Visible = true;
                        this.btnExportarTxt.Visible = true;
                        this.btnExportarExcel.Visible = true;
                    }
                    break;
                default:
                    break;
            }
            
            
        }
        private void CargarImportesJS()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CalcularItem", "CalcularItem();", true);
        }
        private void MapearObjetoAControles(TESBancosLotesEnviados pBancoLotesEnviados)
        {
            this.txtNumeroLote.Text = pBancoLotesEnviados.IdBancoLoteEnvio.ToString();
            this.ddlEstado.SelectedValue = pBancoLotesEnviados.Estado.IdEstado.ToString();
            this.ddlBancosCuentas.SelectedValue = pBancoLotesEnviados.IdBancoCuenta.ToString();
            this.ddlTipoArchivo.SelectedValue = pBancoLotesEnviados.IdTipoArchivo.ToString();
            this.txtCantidadRegistros.Text = pBancoLotesEnviados.CantidadRegistros.ToString();
            //this.txtImporteTotal.Text = "$" + String.Format("{0:#,##0.00}", pBancoLotesEnviados.ImporteTotal);
            this.txtImporteTotal.Text = pBancoLotesEnviados.ImporteTotal.ToString("C2");
            this.txtFechaAlta.Text = pBancoLotesEnviados.FechaAlta.ToShortDateString();
            this.txtFechaAutorizado.Text = pBancoLotesEnviados.FechaAutorizacion.HasValue ? pBancoLotesEnviados.FechaAutorizacion.Value.ToShortDateString() : string.Empty;
            this.txtFechaPago.Text = pBancoLotesEnviados.FechaPago.HasValue ? pBancoLotesEnviados.FechaPago.Value.ToShortDateString() : string.Empty;
            this.txtObservacion.Text = pBancoLotesEnviados.Observacion;
            if(pBancoLotesEnviados.CantidadRegistrosConciliado > 0)
            {
                this.txtCantidadRegistrosConciliado.Text = pBancoLotesEnviados.CantidadRegistrosConciliado.ToString();
                //this.txtImporteTotalConciliado.Text = "$" + String.Format("{0:#,##0.00}", pBancoLotesEnviados.ImporteTotalConciliado);
                this.txtImporteTotalConciliado.Text =pBancoLotesEnviados.ImporteTotalConciliado.ToString("C2");
                this.txtNroTramite.Text = pBancoLotesEnviados.NumeroTramite;
            }

            ListItem estado = this.ddlEstado.Items.FindByValue(pBancoLotesEnviados.Estado.IdEstado.ToString());
            if (estado == null && pBancoLotesEnviados.Estado.IdEstado > 0)
                this.ddlEstado.Items.Add(new ListItem(pBancoLotesEnviados.Estado.Descripcion, pBancoLotesEnviados.Estado.IdEstado.ToString()));
            this.ddlEstado.SelectedValue = pBancoLotesEnviados.Estado.IdEstado == 0 ? string.Empty : pBancoLotesEnviados.Estado.IdEstado.ToString();


        }
        private void MapearControlesAObjeto(TESBancosLotesEnviados pBancoLote)
        {
            pBancoLote.FechaAlta = this.txtFechaAlta.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaAlta.Text);
            pBancoLote.FechaAutorizacion = this.txtFechaAutorizado.Text == string.Empty ? (DateTime?)null : Convert.ToDateTime(this.txtFechaAutorizado.Text);
            pBancoLote.FechaPago = this.txtFechaPago.Text == string.Empty ? (DateTime?)null : Convert.ToDateTime(this.txtFechaPago.Text);
            pBancoLote.IdBancoCuenta = this.ddlBancosCuentas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancosCuentas.SelectedValue);
            pBancoLote.IdTipoArchivo = this.ddlTipoArchivo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoArchivo.SelectedValue);
            pBancoLote.CantidadRegistros = this.txtCantidadRegistros.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadRegistros.Text);
            pBancoLote.Observacion = this.txtObservacion.Text;
            
            var importeAux = this.txtImporteTotal.Text.Replace("$", "");
            importeAux = importeAux.Replace(".", "");
            pBancoLote.ImporteTotal = this.txtImporteTotal.Text == string.Empty ? 0 : Convert.ToDecimal(importeAux);
            if(pBancoLote.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado)//Autorizado
            {
                pBancoLote.CantidadRegistrosConciliado = this.txtCantidadRegistrosConciliado.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadRegistrosConciliado.Text);
                var importeAux2 = this.txtImporteTotalConciliado.Text.Replace("$", "");
                importeAux2 = importeAux2.Replace(".", "");
                pBancoLote.ImporteTotalConciliado = this.txtImporteTotalConciliado.Text == string.Empty ? 0 : Convert.ToDecimal(importeAux2);
                pBancoLote.NumeroTramite = this.txtNroTramite.Text;
            }
            else
            {
                pBancoLote.Estado.IdEstado = this.ddlEstado.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEstado.SelectedValue);
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            List<TESBancosCuentas> lista = BancosF.BancosCuentasObtenerListaFiltroTransferencia(bancoCuenta);
            this.ddlBancosCuentas.DataSource = lista;
            this.ddlBancosCuentas.DataValueField = "IdBancoCuenta";
            this.ddlBancosCuentas.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancosCuentas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentas, ObtenerMensajeSistema("SeleccioneOpcion"));


            this.ddlTipoArchivo.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposArchivosInterbanking);
            this.ddlTipoArchivo.DataValueField = "IdListaValorDetalle"; 
            this.ddlTipoArchivo.DataTextField = "Descripcion";
            this.ddlTipoArchivo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoArchivo, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarTiposOperaciones()
        {
            this.ddlTipoOperacion.DataSource = BancosF.BancosLotesEnviadosObtenerTiposOperaciones();
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnBuscarFiltro_Click(object sender, EventArgs e)
        {
            //this.txtCantidadRegistros.Text = "0";
            //this.txtImporteTotal.Text = "0";
            this.MiBancoLotesEnviados.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (DateTime?)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiBancoLotesEnviados.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (DateTime?)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            this.MiBancoLotesEnviados.IdTipoOperacionFiltro = this.ddlTipoOperacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            this.gvDatos.DataSource = null;
            this.CargarDetalles();
            ScriptManager.RegisterStartupScript(this.upPanel2, this.upPanel2.GetType(), "CalcularItem", "CalcularItem();", true);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = false;
            this.Page.Validate("Aceptar");
           
            if (this.Page.IsValid)
            {
                this.MapearControlesAObjeto(this.MiBancoLotesEnviados);
                this.btnAceptar.Visible = false;
                this.MiBancoLotesEnviados.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MiBancoLotesEnviados.Detalles.ForEach(x => x.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                this.MiBancoLotesEnviados.IdFilial = this.MiBancoLotesEnviados.UsuarioAlta.FilialPredeterminada.IdFilial;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    if (this.MiBancoLotesEnviados.CantidadRegistros > 0)
                    {
                        this.PersistirDatosGrilla();
                        guardo = BancosF.BancosLotesEnviadosAgregar(this.MiBancoLotesEnviados);
                    }
                    else
                    {
                         this.MiBancoLotesEnviados.CodigoMensaje = "Debe incluir uno o mas registros";
                    }
                    break;
                case Gestion.Modificar:
                    if (this.MiBancoLotesEnviados.CantidadRegistros > 0)
                    {
                        this.PersistirDatosGrillaModificar();
                        guardo = BancosF.BancosLotesEnviadosModificar(this.MiBancoLotesEnviados);
                    }
                    else
                    {
                        this.MiBancoLotesEnviados.CodigoMensaje = "Debe incluir uno o mas registros";
                    }
                        break;
                case Gestion.Autorizar:
                    MiBancoLotesEnviados.IdUsuarioAutorizar = MiBancoLotesEnviados.UsuarioLogueado.IdUsuarioEvento;
                    guardo = BancosF.BancosLotesEnviadosAutorizar(this.MiBancoLotesEnviados);
                    if (guardo)
                    {
                        this.btnExportarTxt.Visible = true;
                        this.btnExportarExcel.Visible = true;
                    }
                    break;
                case Gestion.Anular:
                    guardo = BancosF.BancosLotesEnviadosAnular(this.MiBancoLotesEnviados);
                    break;
                case Gestion.ConfirmarAgregar:
                    if (this.MiBancoLotesEnviados.CantidadRegistrosConciliado > 0)
                    {
                        this.PersistirDatosGrillaConciliacion();
                        guardo = BancosF.BancosLotesEnviadosConfirmar(this.MiBancoLotesEnviados);
                        if(guardo)
                        {
                            this.btnExportarTxt.Visible = true;
                            this.btnExportarExcel.Visible = true;
                        }
                    }
                    else
                    {
                        this.MiBancoLotesEnviados.CodigoMensaje = "Debe incluir uno o mas registros";
                    }
                        break;
                case Gestion.Cancelar:
                    guardo = BancosF.BancosLotesEnviadosCancelarConfirmado(this.MiBancoLotesEnviados);
                    break;
                default:
                    break;
            }
            }
            if (guardo)
            {
                this.MostrarMensaje(this.MiBancoLotesEnviados.CodigoMensaje, false);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DeshabilitarControlesScript", "$(document).ready(function () { deshabilitarControles('deshabilitarControles') }); ;", true);
            }
            else
            {
                if (string.IsNullOrEmpty(this.txtNroTramite.Text) && this.GestionControl == Gestion.ConfirmarAgregar)
                    this.MiBancoLotesEnviados.CodigoMensaje = "Debe ingresar el numero de tramite.";

                if (string.IsNullOrEmpty(this.txtFechaPago.Text) && this.GestionControl == Gestion.ConfirmarAgregar)
                    this.MiBancoLotesEnviados.CodigoMensaje = "Debe ingresar la fecha de pago.";

                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiBancoLotesEnviados.CodigoMensaje, true, this.MiBancoLotesEnviados.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.BancoLotesEnviadosModificarDatosCancelar?.Invoke();
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (this.GestionControl == Gestion.Agregar || this.GestionControl == Gestion.ConfirmarAgregar || this.GestionControl == Gestion.Modificar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluirTodos");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Checked = this.MiCheckIncluirTodos;
                    this.MiCheckTodosPrimeraVez = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TESBancosLotesEnviadosDetalle detalle = (TESBancosLotesEnviadosDetalle)e.Row.DataItem;
                if (this.GestionControl == Gestion.Agregar || this.GestionControl == Gestion.ConfirmarAgregar || this.GestionControl == Gestion.Modificar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Checked = true;
                 
                    //if(this.GestionControl == Gestion.Agregar)
                    //{
                        //TextBox importeTotal = (TextBox)this.FindControl("txtImporteTotal");
                        //TextBox registros = (TextBox)this.FindControl("txtCantidadRegistros");
                        //importeTotal.Attributes.Add("onchange", "CalcularItem();");
                        //if (importeTotal.Text != string.Empty)
                        //{
                        //    //importeTotal.Text = (Convert.ToDecimal(importeTotal.Text) + detalle.ImporteTotal).ToString();
                        //    //registros.Text = (Convert.ToInt32(registros.Text) + 1).ToString();
                            
                        //}
                    //}
                    //if(this.GestionControl == Gestion.ConfirmarAgregar)
                    //{
                    //    TextBox importeTotalConciliado = (TextBox)this.FindControl("txtImporteTotalConciliado");
                    //    TextBox registros = (TextBox)this.FindControl("txtCantidadRegistrosConciliado");
                    //    importeTotalConciliado.Attributes.Add("onchange", "CalcularItem();");
                    //    if (importeTotalConciliado.Text != string.Empty)
                    //    {
                    //        importeTotalConciliado.Text =(Convert.ToDecimal(importeTotalConciliado.Text) + detalle.ImporteTotal).ToString();
                    //        registros.Text =  (Convert.ToInt32(registros.Text) + 1).ToString(); 
                    //    }
                    //}
                    //if(this.GestionControl == Gestion.Modificar)
                    //{
                    //    if(this.MiBancoLotesEnviados.Estado.IdEstado == (int)EstadosOrdenesPago.Activo)
                    //    {
                    //        TextBox importeTotalConciliado = (TextBox)this.FindControl("txtImporteTotalConciliado");
                    //        TextBox registros = (TextBox)this.FindControl("txtCantidadRegistrosConciliado");
                    //        importeTotalConciliado.Attributes.Add("onchange", "CalcularItem();");
                    //    }
                    //}
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.GestionControl != Gestion.Agregar && this.GestionControl != Gestion.ConfirmarAgregar && this.GestionControl != Gestion.Modificar)
                {
                    Label footer = (Label)e.Row.FindControl("lblImporteSubTotal");
                    footer.Text = "$" + String.Format("{0:#,##0.00}", MiBancoLotesEnviados.ImporteTotal);
                }
            }
        }
        private void CargarDetalles()
        {
            TESBancosLotesEnviados lote = BancosF.BancosObtenerDetalles(MiBancoLotesEnviados);
            this.MiBancoLotesEnviados.Detalles = lote.Detalles;
            this.hdfImporteAPagar.Value = MiBancoLotesEnviados.ImporteTotal.ToString() == string.Empty ? 0.ToString() : MiBancoLotesEnviados.ImporteTotal.ToString();
            this.gvDatos.DataSource = MiBancoLotesEnviados.Detalles;
            this.gvDatos.DataBind();
            this.upPanel2.Update();
        }
        private void CargarDetallesCargados(List<TESBancosLotesEnviadosDetalle> lista)
        {
            if (lista.Count  > 0)
            {
                this.gvDatos.DataSource = lista;
                this.gvDatos.DataBind();
            }
            else
            {
                this.gvDatos.DataSource = null;
            }
        }
        private void PersistirDatosGrilla()
        {
            int flag = 0;
            if (this.MiBancoLotesEnviados.Detalles.Count == 0)
                return;

            //decimal importeTotalAPagar = this.txtImporteAPagar.Decimal;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                if (!chkIncluir.Checked)
                {
                    MiBancoLotesEnviados.Detalles.RemoveAt(flag);
                    flag--;
                }
                flag++;
               // this.MiBancoLotesEnviados.Detalles[fila.DataItemIndex].IncluirEnLote = chkIncluir.Checked;
            }
        }
        private void PersistirDatosGrillaConciliacion()
        {
            int flag = 0;
            if (this.MiBancoLotesEnviados.Detalles.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                this.MiBancoLotesEnviados.Detalles[flag].Estado.IdEstado = (int)EstadosOrdenesPago.Conciliado;
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                if (!chkIncluir.Checked)
                {
                    this.MiBancoLotesEnviados.Detalles[flag].Estado.IdEstado = (int)EstadosOrdenesPago.Rechazado;
                    //flag--;
                }
                flag++;
                // this.MiBancoLotesEnviados.Detalles[fila.DataItemIndex].IncluirEnLote = chkIncluir.Checked;

            }
        }
        private void PersistirDatosGrillaModificar()
        {
            int flag = 0;
            if (this.MiBancoLotesEnviados.Detalles.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                this.MiBancoLotesEnviados.Detalles[flag].Estado.IdEstado = (int)EstadosOrdenesPago.Activo;
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                if (!chkIncluir.Checked)
                {
                    this.MiBancoLotesEnviados.Detalles[flag].Estado.IdEstado = (int)EstadosOrdenesPago.Rechazado;
                    //flag--;
                }
                flag++;
                // this.MiBancoLotesEnviados.Detalles[fila.DataItemIndex].IncluirEnLote = chkIncluir.Checked;

            }
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            //DataTable detalles = BancosF.BancosLotesEnviadosObtenerDatosExcel(this.MiBancoLotesEnviados);
            //ExportData exportData = new ExportData();
            //exportData.ExportExcel(this.Page, detalles);
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
            this.UpdatePanel2.Update();
        }
        protected void btnExportarTxt_Click(object sender, ImageClickEventArgs e)
        {
            DataTable datos = BancosF.BancosLotesEnviadosObtenerDatosTxt(this.MiBancoLotesEnviados);
            //if (this.CargarDatosReporte(ref datos))
            if(datos.Columns.Count > 0)
             this.ExportarTxtReporte(datos);
            this.UpdatePanel2.Update();

        }
        private void ExportarTxtReporte(DataTable ds)
        {
            string separador = ";";
            ExportData exportar = new ExportData();
            exportar.ExportFile(this.Page, ds, separador, false,"NombreReporte");
        }
    }
}
