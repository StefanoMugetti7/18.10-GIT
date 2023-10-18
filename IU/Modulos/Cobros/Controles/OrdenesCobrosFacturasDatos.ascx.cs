using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cobros.Entidades;
using Comunes.Entidades;
using Cobros;
using Afiliados.Entidades;
using Afiliados;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Globalization;
using CuentasPagar.Entidades;
using Evol.Controls;
using System.Collections;
using Tesorerias.Entidades;
using Tesorerias;
using Prestamos.Entidades;
using Prestamos;
using ProcesosDatos;
using Cargos.Entidades;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System.Data;
using CrystalDecisions.Web;
using System.IO;
using Comunes.LogicaNegocio;
using CrystalDecisions.Shared;
using Hoteles.Entidades;
using System.Net.Mail;

namespace IU.Modulos.Cobros.Controles
{
    public partial class OrdenesCobrosFacturasDatos : ControlesSeguros
    {
        private CobOrdenesCobros MiOrdenCobro
        {
            get { return (CobOrdenesCobros)Session[this.MiSessionPagina + "OrdenesDeCobroDatosMiOrdenCobro"]; }
            set { Session[this.MiSessionPagina + "OrdenesDeCobroDatosMiOrdenCobro"] = value; }
        }

        private List<TGEListasValoresSistemasDetalles> MisRetenciones
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "OrdenesCobrosFacturasDatosMisRetenciones"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosFacturasDatosMisRetenciones"] = value; }
        }

        private List<TGETiposOperaciones> MisTiposOperaciones
        {
            get { return (List<TGETiposOperaciones>)Session[this.MiSessionPagina + "OrdenesCobrosFacturasDatosMisTiposOperaciones"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosFacturasDatosMisTiposOperaciones"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "TesoreriasMovimientosAgregarMisMonedas"]; }
            set { Session[this.MiSessionPagina + "TesoreriasMovimientosAgregarMisMonedas"] = value; }
        }
        //private List<CobOrdenesCobrosTiposRetencionesDetalles> MisRetencionesDetalles
        //{
        //    get { return (List<CobOrdenesCobrosTiposRetencionesDetalles>)Session[this.MiSessionPagina + "OrdenesCobrosFacturasDatosMisRetencionesDetalles"]; }
        //    set { Session[this.MiSessionPagina + "OrdenesCobrosFacturasDatosMisRetencionesDetalles"] = value; }
        //}

        public delegate void OrdenesDeCobroDatosAceptarEventHandler(CobOrdenesCobros e);
        public event OrdenesDeCobroDatosAceptarEventHandler OrdenesDeCobroDatosAceptar;
        public delegate void OrdenesDeCobroDatosCancelarEventHandler();
        public event OrdenesDeCobroDatosCancelarEventHandler OrdenesDeCobroDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            //this.ctrBuscarClientePopUp.AfiliadosBuscarSeleccionar += new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar);
            ctrBuscarCliente.BuscarCliente += new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(CtrBuscarCliente_BuscarCliente);
            this.ctrOrdenesCobrosValores.OrdenesCobrosIngresarValor += new OrdenesCobrosValores.OrdenesCobrosValoresIngresarEventHandler(ctrOrdenesCobrosValores_OrdenesCobrosIngresarValor);
       
        
               
            if (this.IsPostBack)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    this.PersistirDatosGrilla();
                    this.PersistirRetenciones();
                    this.PersistirAnticipos();
                    //this.PersistirDescuentoCargos();
                    //this.Calcular();
                    //this.PersistirPrestamos();
                }
            }
            
            
        }

        void ctrOrdenesCobrosValores_OrdenesCobrosIngresarValor(List<CobOrdenesCobrosValores> e)
        {
            bool actualizar=false;
            if(this.MiOrdenCobro.OrdenesCobrosDetalles.Exists(x=>x.EsAnticipo))
            {
                this.MiOrdenCobro.OrdenesCobrosDetalles.RemoveAll(x=>x.EsAnticipo);
                actualizar=true;
                this.btnAceptar.Attributes.Remove("OnClick");
            }
            if(this.MiOrdenCobro.OrdenesCobrosDetalles.Where(x=>x.IncluirEnOP).Sum(x=>x.Factura.ImporteParcial) < e.Sum(x=>x.Importe) + this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.Sum(x=>x.ImporteTotalRetencion))
            {
                CobOrdenesCobrosDetalles item = new CobOrdenesCobrosDetalles();
                item.EsAnticipo = true;
                item.Factura.FechaFactura = this.MiOrdenCobro.FechaEmision;
                item.Detalle = "Anticipo en cuenta corriente";
                item.Importe = e.Sum(x => x.Importe) + this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.Sum(x => x.ImporteTotalRetencion) - this.MiOrdenCobro.OrdenesCobrosDetalles.Sum(x => x.Importe);
                item.Factura.ImporteParcial = item.Importe;
                this.MiOrdenCobro.OrdenesCobrosDetalles.Add(item);
                item.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosDetalles.IndexOf(item);
                item.EstadoColeccion = EstadoColecciones.Agregado;
                item.IncluirEnOP = true;
                actualizar = true;
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAnticipoCuentaCorriente"));
                this.btnAceptar.Attributes.Add("OnClick", funcion);
            }
           

            if (actualizar)
            {
                AyudaProgramacion.CargarGrillaListas(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvDatos, true);
                this.upOrdenPagoDetalle.Update();
                this.upAcciones.Update();
            }
        }

        protected void btnEliminarAnticipo_Click(object sender, EventArgs e)
        {
            if (this.MiOrdenCobro.OrdenesCobrosDetalles.Exists(x => x.EsAnticipo))
            {
                this.MiOrdenCobro.OrdenesCobrosDetalles.RemoveAll(x => x.EsAnticipo);
                AyudaProgramacion.CargarGrillaListas(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvDatos, true);
                this.upOrdenPagoDetalle.Update();
            }
        }
        
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            switch(GestionControl)
            {
                case Gestion.Agregar:
                    if (this.MiOrdenCobro.Estado.IdEstado != (int)EstadosOrdenesCobro.Cobrado)
                    {
                        TESCajasMovimientos movimiento = new TESCajasMovimientos();
                        PaginaCajas nueva = new PaginaCajas();

                        TESFiltroMovimientosPendientes movFiltro = new TESFiltroMovimientosPendientes();
                        movFiltro.IdFilial = this.MiOrdenCobro.Filial.IdFilial;
                        movFiltro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                        movFiltro.IdTipoOperacion = this.MiOrdenCobro.TipoOperacion.IdTipoOperacion;
                        movFiltro.IdRefTipoOperacion = this.MiOrdenCobro.IdOrdenCobro;
                        movimiento = TesoreriasF.CajasObtenerMovimientosPendientesFiltro(movFiltro);

                        nueva.GuardarMovimiento(this.MiSessionPagina, movimiento);

                        string url = "~/Modulos/Tesoreria/CajasMovimientosConfirmar.aspx";
                        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
                    }
                    else
                    {
                        if (this.OrdenesDeCobroDatosAceptar != null)
                            this.OrdenesDeCobroDatosAceptar(this.MiOrdenCobro);
                    }

            break;
                case Gestion.Anular:
            if (this.OrdenesDeCobroDatosAceptar != null)
                this.OrdenesDeCobroDatosAceptar( this.MiOrdenCobro);
            break;
                default:
            break;
        }
        }
        
        protected void ddlTipoOperacionOC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoOperacionOC.SelectedValue))
            {
                if (this.GestionControl != Gestion.Agregar || string.IsNullOrEmpty(this.ddlTipoOperacionOC.SelectedValue))
                    return;

                //this.MiOrdenCobro.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacionOC.SelectedValue);
                //this.MiOrdenCobro.TipoOperacion.TipoOperacion = this.ddlTipoOperacionOC.SelectedItem.Text;
                this.MiOrdenCobro.TipoOperacion = this.MisTiposOperaciones[this.ddlTipoOperacionOC.SelectedIndex];

                if (this.MiOrdenCobro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas)
                    this.btnAgregarRetencion.Visible = true;
                else
                    this.btnAgregarRetencion.Visible = false;

                
                //this.IniciarControlAgregar(this.MiOrdenCobro);
            }
            ddlMoneda_OnSelectedIndexChanged(ddlMoneda, EventArgs.Empty);
        }

        protected void ddlMoneda_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
            {             
                var idMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);

                if (MiOrdenCobro.Moneda.IdMoneda != idMoneda)
                {
                    MiOrdenCobro.OrdenesCobrosValores = MiOrdenCobro.OrdenesCobrosValores
                        .Where(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo).ToList();
                    MiOrdenCobro.OrdenesCobrosValores = AyudaProgramacion.AcomodarIndices<CobOrdenesCobrosValores>(MiOrdenCobro.OrdenesCobrosValores);
                }
                MiOrdenCobro.Moneda = MisMonedas.Find(delegate (TGEMonedas m) { return m.IdMoneda == idMoneda; });
                SetInitializeCulture(MiOrdenCobro.Moneda.Moneda);
            }
            this.ctrOrdenesCobrosValores.IniciarControl(this.MiOrdenCobro, this.GestionControl, true);
            this.ctrOrdenesCobrosValores.ActualizarUpdatePanel();
            this.upOrdenPagoDetalle.Update();
            this.IniciarControlAgregar(this.MiOrdenCobro);
            //upMovimientos.Update();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CobOrdenesCobrosDetalles detalle = MiOrdenCobro.OrdenesCobrosDetalles[indiceColeccion];
            this.MiOrdenCobro.OrdenesCobrosDetalles.Remove(detalle);
            this.MiOrdenCobro.OrdenesCobrosDetalles = AyudaProgramacion.AcomodarIndices<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles);
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvDatos, true);
            this.upOrdenPagoDetalle.Update();
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CobOrdenesCobrosDetalles item = (CobOrdenesCobrosDetalles)e.Row.DataItem;
              
                if (this.GestionControl == Gestion.Agregar)
                {
                    this.gvDatos.Columns[4].Visible = true;
                    this.gvDatos.Columns[5].Visible = true;

                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Enabled = !item.EsAnticipo;
                    ibtnConsultar.Attributes.Add("onchange", "CalcularItem();");

                    //CurrencyTextBox importeParcial = (CurrencyTextBox)e.Row.FindControl("txtImporteParcial");
                    //importeParcial.Enabled = true;
                    //importeParcial.Attributes.Add("onchange", "CalcularItem();");
                    //importeParcial.Visible = true;
          
                    CurrencyTextBox importePagar = (CurrencyTextBox)e.Row.FindControl("txtImportePagar");
                    importePagar.Visible = true;

                    //if (item.EsAnticipo)
                    //{
                    //    ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                    //    ibtnEliminar.Visible = true;
                    //}
                    //Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                    //lblImporte.Visible = false;
                }
             
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteSubTotal");
                    lblImporteTotal.Text = this.MiOrdenCobro.OrdenesCobrosDetalles.Sum(x => x.Factura.ImporteParcial).ToString("C2");

                    Label lblImporteSubTotalPagar = (Label)e.Row.FindControl("lblImporteSubTotalPagar");
                    lblImporteSubTotalPagar.Text = this.MiOrdenCobro.ImporteSubTotal.ToString("C2");
                }
                else
                {
                    Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteSubTotal");
                    lblImporteTotal.Text = this.MiOrdenCobro.ImporteSubTotal.ToString("C2");
                }
            }
        }

        public void IniciarControl(CobOrdenesCobros pOrdenCobro, Gestion pGestion)
        {
            this.MiOrdenCobro = pOrdenCobro;
            this.GestionControl = pGestion;
            //this.MiPrestamo = null;
            this.CargarCombos();
            this.MisRetenciones = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposRetenciones);
            this.ctrComentarios.IniciarControl(new CobOrdenesCobros(), GestionControl);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                   
                    this.txtFecha.Text = DateTime.Now.ToShortDateString();
                    this.txtFecha.Enabled = true;
                    this.txtPrefijoNumeroRecibo.Enabled = true;
                    this.txtNumeroRecibo.Enabled = true;
                    //this.btnBuscarSocio.Visible = true;
                    //this.txtCodigoSocio.Enabled = true;
                    this.ddlFilialCobro.Enabled = true;
                    this.ddlTipoOperacionOC.Enabled = true;
                    this.ddlMoneda.Enabled = true;
                    //this.btnBuscarSocio.Enabled = true;
                    AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvDatos, true);
                    
                    if (pOrdenCobro.IdRefFacturaOCAutomatica > 0
                        && pOrdenCobro.TipoOperacion.IdTipoOperacion>0)
                    {
                        this.ddlTipoOperacionOC.SelectedValue = pOrdenCobro.TipoOperacion.IdTipoOperacion.ToString();
                    }
                   
                    /*Reservas de Hoteles*/
                    if (this.MisParametrosUrl.Contains("IdReserva"))
                    {
                        this.MiOrdenCobro.IdRefTabla = Convert.ToInt64(this.MisParametrosUrl["IdReserva"].ToString());
                        this.MiOrdenCobro.Tabla = typeof(HTLReservas).Name;
                        this.MisParametrosUrl.Remove("IdReserva");
                        //this.txtCodigoSocio.Enabled = false;
                        //this.btnBuscarSocio.Enabled = false;
                    }
                    if (pOrdenCobro.Afiliado.IdAfiliado > 0)
                    {
                        ctrBuscarCliente.BuscarCliente += new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(CtrBuscarCliente_BuscarCliente);
                        MiOrdenCobro.Afiliado.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"].ToString());
                        ctrBuscarCliente.IniciarControl(MiOrdenCobro.Afiliado, this.GestionControl);
                        //this.txtCodigoSocio.Text = pOrdenCobro.Afiliado.IdAfiliado.ToString();
                        //this.txtCodigoSocio_TextChanged(null, EventArgs.Empty);
                    }
                    //else if (this.PropiedadObtenerValor<int>("gblIdAfiliado") > 0)
                    //{
                    //    this.txtCodigoSocio.Text = this.PropiedadObtenerValor<int>("gblIdAfiliado").ToString();
                    //    this.txtCodigoSocio_TextChanged(null, EventArgs.Empty);
                    //}
           
                    this.ctrFechaCajaContable.IniciarControl(Gestion.Agregar, DateTime.Now, DateTime.Now);
                    this.txtFecha.Attributes.Add("onchange", "SetearFechaComprobante();");
                    if (paginaSegura.MenuPadre == EnumMenues.Afiliados)
                    {
                        //this.txtCodigoSocio.Enabled = false;
                        //this.btnBuscarSocio.Enabled = false;
                    }
                    ddlTipoOperacionOC_SelectedIndexChanged(this.ddlTipoOperacionOC, EventArgs.Empty);
                    
                    if (this.MisParametrosUrl.Contains("UrlReferrer"))
                        this.paginaSegura.viewStatePaginaSegura = this.MisParametrosUrl["UrlReferrer"].ToString();
                    break;
                case Gestion.Anular:
                case Gestion.AnularConfirmar:
                    this.ddlMoneda.Enabled = false;
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerFacturasDatosCompletos(this.MiOrdenCobro);
                    if (this.MiOrdenCobro.Prestamo.IdPrestamo > 0)
                    {
                        this.MiOrdenCobro.Prestamo = PrePrestamosF.PrestamosObtenerDatosCompletos(this.MiOrdenCobro.Prestamo);
                        string funcion = string.Format("ShowConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("OrdenCobroConPrestamoConfirmacionPopUp"));
                        this.btnAceptar.Attributes.Add("OnClick", funcion);
                    }
                    this.MapearObjetoControles(this.MiOrdenCobro);
                    this.btnImprimir.Visible = true;
                    this.btnEnviarMail.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.ddlMoneda.Enabled = false;
                    //this.btnAgregarPrestamo.Visible = false;
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerFacturasDatosCompletos(this.MiOrdenCobro);
                    if (this.MiOrdenCobro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasAdelantos)
                        this.pnlComprobantes.Visible = false;
                    if (this.MiOrdenCobro.Prestamo.IdPrestamo > 0)
                        this.MiOrdenCobro.Prestamo = PrePrestamosF.PrestamosObtenerDatosCompletos(this.MiOrdenCobro.Prestamo);
                    this.MapearObjetoControles(this.MiOrdenCobro);
                    this.btnAceptar.Visible = false;
                    this.btnImprimir.Visible = true;
                    this.btnEnviarMail.Visible = true;
                    this.txtDetalleOrden.Enabled = false;
                    if (MiOrdenCobro.OrdenesCobrosTiposRetenciones.Count == 0)
                    {
                        gvRetenciones.Visible = false;
                    }
                    //this.gvDatos.Columns[2].Visible = true;
                    //this.gvDatos.Columns[3].Visible = false;
                    break;
                case Gestion.Modificar:
                    this.ddlMoneda.Enabled = true;
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerFacturasDatosCompletos(this.MiOrdenCobro);
                    this.MapearObjetoControles(this.MiOrdenCobro);
                    //ddlMoneda_OnSelectedIndexChanged(ddlMoneda, EventArgs.Empty);
                    this.btnAceptar.Visible = true;
                    this.btnImprimir.Visible = true;
                    this.btnEnviarMail.Visible = true;
                    this.ddlTipoOperacionOC.Enabled = true;
                    this.ddlTipoOperacionOC.AutoPostBack = false;
                    break;
                default:
                    break;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.MiOrdenCobro.IdRefTabla.HasValue)
            {
                switch (this.MiOrdenCobro.Tabla)
                {
                    case "HTLReservas":
                        this.MisParametrosUrl.Add("IdReserva", this.MiOrdenCobro.IdRefTabla);
                        break;
                    default:
                        break;
                }
            }
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
                    
                    this.PersistirDatosGrilla();
                    this.PersistirRetenciones();
                    //this.PersistirDescuentoCargos();
                    this.PersistirAnticipos();

                    this.MiOrdenCobro = this.ctrOrdenesCobrosValores.ObtenerOrdenesCobrosValores(this.MiOrdenCobro);
                    CarTiposCargosAfiliadosFormasCobros cargo = this.ctrOrdenesCobrosValores.MiCargoAfiliado;
                    this.MiOrdenCobro.PrefijoNumeroRecibo = this.MiOrdenCobro.PrefijoNumeroRecibo.PadLeft(4, '0');
                    this.MiOrdenCobro.NumeroRecibo = this.MiOrdenCobro.NumeroRecibo.PadLeft(8, '0');
                    this.MiOrdenCobro.ImporteRetenciones = this.txtImporteRetenciones.Decimal; //this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
                    this.MiOrdenCobro.ImporteSubTotal = this.txtSubTotal.Decimal;// this.MiOrdenCobro.OrdenesCobrosDetalles.Where(x => x.IncluirEnOP == true).Sum(x => x.Importe);
                    this.MiOrdenCobro.ImporteTotal = this.txtTotalCobrar.Decimal; //this.MiOrdenCobro.ImporteSubTotal - this.MiOrdenCobro.ImporteRetenciones - this.MiOrdenCobro.OrdenesCobrosAnticipos.Where(x => x.IncluirEnOC == true).Sum(x => x.ImporteAplicar);

                    this.MiOrdenCobro.EstadoColeccion = EstadoColecciones.Agregado;
            
                    //this.MiOrdenCobro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas;
                    this.MiOrdenCobro.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    
                    guardo = CobrosF.OrdenesCobrosAgregar(this.MiOrdenCobro, cargo);
                    break;
                case Gestion.Anular:
                    if (this.MiOrdenCobro.Prestamo.IdPrestamo > 0)
                    {
                        this.MiOrdenCobro.Prestamo.EstadoColeccion = EstadoColecciones.Borrado;
                    }
                    this.MiOrdenCobro.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenCobro.Estado.IdEstado = (int)EstadosOrdenesCobro.Baja;
                    guardo = CobrosF.OrdenesCobrosAnular(this.MiOrdenCobro);
                    break;
                case Gestion.AnularConfirmar:
                    this.MiOrdenCobro.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenCobro.Estado.IdEstado = (int)EstadosOrdenesCobro.Baja;
                    guardo = CobrosF.OrdenesCobrosAnularCobrada(this.MiOrdenCobro);
                    break;
                case Gestion.Modificar:
                    this.MiOrdenCobro.EstadoColeccion = EstadoColecciones.Modificado;
                    guardo = CobrosF.OrdenesCobrosModificar(this.MiOrdenCobro);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.btnImprimir.Visible = true;
                this.btnEnviarMail.Visible = true;
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        if (this.MiOrdenCobro.Estado.IdEstado != (int)EstadosOrdenesCobro.Cobrado)
                        {
                            if (MiOrdenCobro.ModuloTesoreriaCajas)
                            {
                                if (this.OrdenesDeCobroDatosAceptar != null)
                                    this.OrdenesDeCobroDatosAceptar(this.MiOrdenCobro);
                            }
                            else
                            {
                                this.MiOrdenCobro.CodigoMensaje = "OrdenCobroConfirmarMov";
                                //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiOrdenCobro.CodigoMensaje, new List<string>() { this.MiOrdenCobro.IdOrdenCobro.ToString() }), true);
                            }
                        }
                        else
                        {
                            this.MiOrdenCobro.CodigoMensaje = "OrdenCobroGenerada";
                            this.MostrarMensaje(this.MiOrdenCobro.CodigoMensaje, false, new List<string>() { this.MiOrdenCobro.IdOrdenCobro.ToString() });
                            this.ctrAsientoMostrar.IniciarControl(this.MiOrdenCobro);
                        }
                        break;
                    case Gestion.Anular:
                    case Gestion.AnularConfirmar:
                    case Gestion.Modificar:
                      //  this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiOrdenCobro.CodigoMensaje));
                        break;
                }
            }
            else
            {

                if (this.MiOrdenCobro.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiOrdenCobro);
                    this.MiOrdenCobro.dsResultado = null;
                }
                else
                {
                    this.MostrarMensaje(this.MiOrdenCobro.CodigoMensaje, true, this.MiOrdenCobro.CodigoMensajeArgs);
                }
            }
        }
        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage();
            if (CobrosF.OrdenesCobroArmarMail(this.MiOrdenCobro, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiOrdenCobro);
            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            //if (this.MiOrdenCobro.Prestamo.IdPrestamo != 0)
            //{
            //    #region COMPROBANTE ORDEN/PRESTAMO

            //    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, "OrdenesCobros", MiOrdenCobro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            //    ExportPDF.ExportarPDF(pdf, this, "OrdenesCobros", this.UsuarioActivo);

            //    todo lo de abajo es uniendo prestamos con cobros

            //    List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            //    TGEArchivos archivo = new TGEArchivos();
            //    TGEArchivos archivoPre = new TGEArchivos();
            //    //CobOrdenesCobros ordenCobroPdf = FacturasF.FacturasObtenerArchivo(factura);
            //    byte[] pdfOrden;
            //    byte[] pdfPrestamo;

            //    TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.CobOrdenesCobros);
            //    string archivoReporteLeer = string.Concat(this.ObtenerAppPath(), comprobante.NombreRPT);

            //    RepReportes pReporte = new RepReportes();
            //    pReporte.StoredProcedure = comprobante.NombreSP;
            //    RepParametros param = new RepParametros();
            //    param.NombreParametro = "IdOrdenCobro";
            //    param.Parametro = "IdOrdenCobro";
            //    param.ValorParametro = this.MiOrdenCobro.IdOrdenCobro.ToString();
            //    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            //    pReporte.Parametros.Add(param);
            //    DataSet dataSet = ReportesF.ReportesObtenerDatos(pReporte);

            //    CrystalReportSource CryReportSource = new CrystalReportSource();
            //    CryReportSource.CacheDuration = 1;
            //    CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer);
            //    CryReportSource.ReportDocument.SetDataSource(dataSet);
            //    Stream reciboCom = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            //    CryReportSource.ReportDocument.Close();
            //    CryReportSource.Dispose();
            //    pdfOrden = AyudaProgramacionLN.StreamToByteArray(reciboCom);
            //    //ordenCobroPdf.IdOrdenCobro = ordenCobro.IdOrdenCobro;
            //    archivo.Archivo = pdfOrden;
            //    listaArchivos.Add(archivo);

            //    //// AHORA CARGO EL PRESTAMO
            //    TGEComprobantes comprobantePrestamo = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.PrePrestamos);
            //    string archivoReporteLeer2 = string.Concat(this.ObtenerAppPath(), comprobantePrestamo.NombreRPT);

            //    RepReportes pReportePres = new RepReportes();
            //    pReportePres.StoredProcedure = comprobantePrestamo.NombreSP;
            //    RepParametros paramPre = new RepParametros();
            //    paramPre.NombreParametro = "IdPrestamo";
            //    paramPre.Parametro = "IdPrestamo";
            //    paramPre.ValorParametro = this.MiOrdenCobro.Prestamo.IdPrestamo.ToString();
            //    paramPre.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            //    pReportePres.Parametros.Add(paramPre);
            //    DataSet dataSetPre = ReportesF.ReportesObtenerDatos(pReportePres);

            //    CryReportSource = new CrystalReportSource();
            //    CryReportSource.CacheDuration = 1;
            //    CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer2);
            //    CryReportSource.ReportDocument.SetDataSource(dataSetPre);
            //    Stream reciboPre = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            //    CryReportSource.ReportDocument.Close();
            //    CryReportSource.Dispose();
            //    pdfPrestamo = AyudaProgramacionLN.StreamToByteArray(reciboPre);
            //    //ordenCobroPdf.IdOrdenCobro = ordenCobro.IdOrdenCobro;
            //    archivoPre.Archivo = pdfPrestamo;
            //    listaArchivos.Add(archivoPre);

            //    //Levanto la Factura y el Remito
            //    listaArchivos.AddRange(CobrosF.OrdenesCobrosObtenerArchivos(this.MiOrdenCobro));

            //    string nombre = string.Concat(this.MiOrdenCobro.GetType().Name, "_", this.MiOrdenCobro.IdOrdenCobro.ToString().PadLeft(10, '0'));
            //    nombre = string.Format("{0}.pdf", nombre);

            //    this.ctrPopUpComprobantes.CargarArchivo(listaArchivos, nombre);
            //    #endregion
            //}
            //else
            //{
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, "OrdenesCobros", MiOrdenCobro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.upOrdenPagoDetalle, "OrdenesCobros", this.UsuarioActivo);
                this.upOrdenPagoDetalle.Update();
            //}
        }

        private void PersistirDatosGrilla()
        {
            if (this.MiOrdenCobro.OrdenesCobrosDetalles.Count > 0)
            {
                foreach (GridViewRow fila in this.gvDatos.Rows)
                {
                    CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                    this.MiOrdenCobro.OrdenesCobrosDetalles[fila.DataItemIndex].IncluirEnOP = chkIncluir.Checked;

                    decimal importePagar = ((CurrencyTextBox)fila.FindControl("txtImportePagar")).Decimal;
                    this.MiOrdenCobro.OrdenesCobrosDetalles[fila.DataItemIndex].Importe = importePagar;
                    if (chkIncluir.Checked)
                        this.MiOrdenCobro.OrdenesCobrosDetalles[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Agregado;
                    else
                        this.MiOrdenCobro.OrdenesCobrosDetalles[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.SinCambio;
                }
                
            }
        }

        protected void txtImporteRetencion_TextChanged(object sender, EventArgs e)
        {
            this.MiOrdenCobro = this.ctrOrdenesCobrosValores.ObtenerOrdenesCobrosValores(this.MiOrdenCobro);
            ctrOrdenesCobrosValores_OrdenesCobrosIngresarValor(this.MiOrdenCobro.OrdenesCobrosValores);
        }
        //private void Calcular()
        //{
        //    this.MiOrdenCobro.ImporteSubTotal = this.MiOrdenCobro.OrdenesCobrosDetalles.Where(x => x.IncluirEnOP).Sum(x => x.Importe);
        //    this.MiOrdenCobro.ImporteRetenciones = this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
        //    this.MiOrdenCobro.ImporteTotal = this.MiOrdenCobro.ImporteSubTotal - this.MiOrdenCobro.ImporteRetenciones - this.MiOrdenCobro.OrdenesCobrosAnticipos.Where(x => x.IncluirEnOC).Sum(x => x.ImporteAplicar); ;
        //    this.txtSubTotal.Text = this.MiOrdenCobro.ImporteSubTotal.ToString("C2");
        //    this.txtImporteRetenciones.Text = this.MiOrdenCobro.ImporteRetenciones.ToString("C2");
        //    this.txtTotalCobrar.Text = this.MiOrdenCobro.ImporteTotal.ToString("C2");
        //    this.upTotales.Update();
        //}

        private void CargarCombos()
        {
            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();
            this.ddlMoneda.DataSource = MisMonedas; // TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));


            this.ddlFilialCobro.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilialCobro.DataValueField = "IdFilial";
            this.ddlFilialCobro.DataTextField = "Filial";
            this.ddlFilialCobro.DataBind();
            this.ddlFilialCobro.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacionOC.DataSource = MisTiposOperaciones;
            this.ddlTipoOperacionOC.DataTextField = "TipoOperacion";
            this.ddlTipoOperacionOC.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacionOC.DataBind();
            if (ddlTipoOperacionOC.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacionOC, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        private void IniciarControlAgregar(CobOrdenesCobros pOrdenCobro)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoOperacionOC.SelectedValue))
            {
                if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
                {
                    pOrdenCobro.Moneda.IdMoneda = Convert.ToInt32(ddlMoneda.SelectedValue);
                    this.MiOrdenCobro.OrdenesCobrosDetalles = CobrosF.FacturaObtenerPendientePago(pOrdenCobro);
                    this.MiOrdenCobro.OrdenesCobrosAnticipos = CobrosF.OrdenesCobrosAnticiposPendientesAplicar(pOrdenCobro);
                    //si es factura automatica .... 
                    if (Convert.ToInt32(pOrdenCobro.IdRefFacturaOCAutomatica) != 0)
                    {
                        foreach (CobOrdenesCobrosDetalles detalle in this.MiOrdenCobro.OrdenesCobrosDetalles)
                        {
                            if (detalle.Factura.IdFactura != Convert.ToInt32(pOrdenCobro.IdRefFacturaOCAutomatica))
                            {
                                detalle.IncluirEnOP = false;
                            }
                        }
                    }

                    //this.MiOrdenCobro.ImporteSubTotal = this.MiOrdenCobro.OrdenesCobrosDetalles.Where(x => x.IncluirEnOP).Sum(x => x.Importe);
                    this.txtSubTotal.Text = (0).ToString("C2"); // this.MiOrdenCobro.ImporteSubTotal.ToString("C2");
                    this.txtImporteRetenciones.Text = (0).ToString("C2");
                    this.txtTotalCobrar.Text = (0).ToString("C2");// this.MiOrdenCobro.ImporteSubTotal.ToString("C2");
                                                                  //this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
                    AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvDatos, true);
                    //if (this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.Count == 0)
                    //    this.btnAgregarRetencion_Click(null, EventArgs.Empty);
                    this.upOrdenPagoDetalle.Update();
                    this.upRetenciones.Update();

                    AyudaProgramacion.CargarGrillaListas<CobOrdenesCobros>(this.MiOrdenCobro.OrdenesCobrosAnticipos, false, this.gvAnticipos, true);

                    if (this.MiOrdenCobro.OrdenesCobrosAnticipos.Count > 0)
                    {
                        this.pnlOrdenesCobrosAnticipos.Visible = true;
                    }
                    else
                    {
                        this.pnlOrdenesCobrosAnticipos.Visible = false;
                    }
                    this.upOrdenesCobrosAnticipos.Update();
                    this.upTotales.Update();
                }
            }
            //ScriptManager.RegisterStartupScript(this.upOrdenPagoDetalle, this.upOrdenPagoDetalle.GetType(), "gvDetallesCalcularItem", "CalcularItem();", true);
        }

        private void MapearObjetoControles(CobOrdenesCobros pParametro)
        {
            //this.txtCodigoSocio.Text = pParametro.Afiliado.NumeroSocio;
            //this.txtRazonSocial.Text = pParametro.Afiliado.ApellidoNombre;
            ////this.txtCuil.Text = pParametro.Afiliado.CUIL.ToString();
            //this.lblCuil.Text = pParametro.Afiliado.TipoDocumento.TipoDocumento;
            //this.txtCuil.Text = pParametro.Afiliado.NumeroDocumento.ToString();

            this.txtOrdenCobro.Text = pParametro.IdOrdenCobro.ToString();
            this.txtFecha.Text = pParametro.FechaEmision.ToShortDateString();
            this.txtPrefijoNumeroRecibo.Text = this.MiOrdenCobro.PrefijoNumeroRecibo;
            this.txtNumeroRecibo.Text = this.MiOrdenCobro.NumeroRecibo;

            this.txtDetalleOrden.Text = this.MiOrdenCobro.Detalle;

            ListItem item = this.ddlTipoOperacionOC.Items.FindByValue(pParametro.TipoOperacion.IdTipoOperacion.ToString());
            if (item == null)
                this.ddlTipoOperacionOC.Items.Add(new ListItem(pParametro.TipoOperacion.TipoOperacion, pParametro.TipoOperacion.IdTipoOperacion.ToString()));
            this.ddlTipoOperacionOC.SelectedValue = pParametro.TipoOperacion.IdTipoOperacion.ToString();

            this.txtSubTotal.Text = this.MiOrdenCobro.ImporteBruto.ToString("C2");
            this.txtImporteRetenciones.Text = this.MiOrdenCobro.ImporteRetenciones.ToString("C2");
            this.txtTotalCobrar.Text = this.MiOrdenCobro.ImporteTotal.ToString("C2");

            item = this.ddlMoneda.Items.FindByValue(this.MiOrdenCobro.Moneda.IdMoneda.ToString());
            if (item == null)
                this.ddlMoneda.Items.Add(new ListItem(this.MiOrdenCobro.Moneda.Descripcion, this.MiOrdenCobro.Moneda.IdMoneda.ToString()));
            this.ddlMoneda.SelectedValue = this.MiOrdenCobro.Moneda.IdMoneda.ToString();
            SetInitializeCulture(MiOrdenCobro.Moneda.Moneda);
            //ListItem item = this.ddlFilialPago.Items.FindByValue(pParametro.FilialPago.IdFilialPago.ToString());
            //if (item == null)
            //    this.ddlFilialPago.Items.Add(new ListItem(pParametro.FilialPago.Filial, pParametro.FilialPago.IdFilialPago.ToString()));
            this.ddlFilialCobro.SelectedValue = pParametro.FilialCobro.IdFilialCobro.ToString();

            this.ctrComentarios.IniciarControl(this.MiOrdenCobro, GestionControl);

            pParametro.ImporteSubTotal = pParametro.OrdenesCobrosDetalles.Sum(x => x.Importe);
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(pParametro.OrdenesCobrosDetalles, false, this.gvDatos, true);
            //mostrar TiposRetencionesDetalles
            //List<CobOrdenesCobrosTiposRetencionesDetalles> listaMostar = new List<CobOrdenesCobrosTiposRetencionesDetalles>();
            //pParametro.OrdenesCobrosTiposRetenciones.ForEach(x => listaMostar.AddRange(x.OrdenesCobrosTiposRetencionesDetalle));
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosTiposRetenciones>(pParametro.OrdenesCobrosTiposRetenciones, false, this.gvRetenciones, false);

            //if (pParametro.FormaCobroAfiliado.IdFormaCobroAfiliado> 0)
            //{
            //    List<CobOrdenesCobros> lista = new List<CobOrdenesCobros>();
            //    lista.Add(this.MiOrdenCobro);
            //    AyudaProgramacion.CargarGrillaListas<CobOrdenesCobros>(lista, false, this.gvDescuentoCargos, true);
            //    this.gvDescuentoCargos.Visible = true;
            //}

            this.txtSubTotal.Text = (this.MiOrdenCobro.OrdenesCobrosDetalles.Sum(x => x.Importe) - this.MiOrdenCobro.OrdenesCobrosAnticipos.Sum(x=>x.ImporteAplicado)).ToString("C2");
            this.txtImporteRetenciones.Text = this.MiOrdenCobro.ImporteRetenciones.ToString("C2");
            this.txtTotalCobrar.Text = this.MiOrdenCobro.ImporteTotal.ToString("C2");

            if (this.MiOrdenCobro.OrdenesCobrosAnticipos.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<CobOrdenesCobros>(this.MiOrdenCobro.OrdenesCobrosAnticipos, false, this.gvAnticipos, false);
                this.pnlOrdenesCobrosAnticipos.Visible = true;
            }

            //if (this.MiOrdenCobro.Prestamo.IdPrestamo>0)
            //{
            //    List<PrePrestamos> prestamos = new List<PrePrestamos>();
            //    prestamos.Add(this.MiOrdenCobro.Prestamo);
            //    AyudaProgramacion.CargarGrillaListas<PrePrestamos>(prestamos, false, this.gvPrestamos, false);
            //}
            this.ctrFechaCajaContable.IniciarControl(Gestion.Consultar, this.MiOrdenCobro.FechaConfirmacion);
            this.ctrOrdenesCobrosValores.IniciarControl(this.MiOrdenCobro, this.GestionControl);
            this.ctrAsientoMostrar.IniciarControl(this.MiOrdenCobro);
            ctrBuscarCliente.IniciarControl(MiOrdenCobro.Afiliado, this.GestionControl);
        }

        private void MapearControlesAObjeto()
        {
            this.MiOrdenCobro.Detalle = this.txtDetalleOrden.Text;
            this.MiOrdenCobro.FechaEmision = Convert.ToDateTime(this.txtFecha.Text);
            this.MiOrdenCobro.PrefijoNumeroRecibo = this.txtPrefijoNumeroRecibo.Text;
            this.MiOrdenCobro.NumeroRecibo = this.txtNumeroRecibo.Text;
            this.MiOrdenCobro.FilialCobro.IdFilialCobro = Convert.ToInt32(this.ddlFilialCobro.SelectedValue);
            this.MiOrdenCobro.FilialCobro.Filial = this.ddlFilialCobro.SelectedItem.Text;
            this.MiOrdenCobro.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacionOC.SelectedValue);
            this.MiOrdenCobro.TipoOperacion.TipoOperacion = this.ddlTipoOperacionOC.SelectedItem.Text;
            this.MiOrdenCobro.Comentarios = this.ctrComentarios.ObtenerLista();
            this.MiOrdenCobro.FechaConfirmacion = this.ctrFechaCajaContable.dFechaCajaContable;
            this.MiOrdenCobro.Moneda.IdMoneda = ddlMoneda.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlMoneda.SelectedValue);
            TGEMonedas mon = MisMonedas.Find(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            MiOrdenCobro.MonedaCotizacion = mon.MonedeaCotizacion.MonedaCotizacion;
        }

        #region Grilla Percepciones

        private void PersistirRetenciones()
        {
            if (this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvRetenciones.Rows)
            {
                DropDownList ddlRetenciones = ((DropDownList)fila.FindControl("ddlTipoRetenciones"));
                DropDownList ddlConceptos = ((DropDownList)fila.FindControl("ddlTipoConceptos"));
                decimal importeRetencion = ((CurrencyTextBox)fila.FindControl("txtImporteRetencion")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtImporteRetencion")).Text, NumberStyles.Currency);
                TextBox txtNumeroCertificado = (TextBox)fila.FindControl("txtNumeroCertificado");

                if (ddlRetenciones.SelectedValue != "")
                {
                    this.MiOrdenCobro.OrdenesCobrosTiposRetenciones[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenCobro.OrdenesCobrosTiposRetenciones[fila.DataItemIndex].TipoRetencion.IdTipoRetencion = Convert.ToInt32(ddlRetenciones.SelectedValue);
                }
                if (ddlConceptos.SelectedValue != string.Empty)
                {
                    this.MiOrdenCobro.OrdenesCobrosTiposRetenciones[fila.DataItemIndex].IdRefImpuestoRetencion = Convert.ToInt32(ddlConceptos.SelectedValue);
                    this.MiOrdenCobro.OrdenesCobrosTiposRetenciones[fila.DataItemIndex].Concepto = ddlConceptos.SelectedItem.Text;
                }
                if (importeRetencion != 0)
                {
                    this.MiOrdenCobro.OrdenesCobrosTiposRetenciones[fila.DataItemIndex].ImporteTotalRetencion = importeRetencion;
                }
                this.MiOrdenCobro.OrdenesCobrosTiposRetenciones[fila.DataItemIndex].NumeroCertificado = txtNumeroCertificado.Text;

                //((Label)fila.FindControl("lblImporteIva")).Text = (this.MiSolicitud.SolicitudPagoDetalles[fila.RowIndex].ImporteIvaTotal).ToString();
            }
        }

        protected void gvRetenciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            ///this.MiIndiceDetalleModificar = Convert.ToInt32(e.CommandArgument);
            int index = Convert.ToInt32(e.CommandArgument);
            int indice = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
                this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.RemoveAt(indice);
                this.MiOrdenCobro.OrdenesCobrosTiposRetenciones = AyudaProgramacion.AcomodarIndices<CobOrdenesCobrosTiposRetenciones>(this.MiOrdenCobro.OrdenesCobrosTiposRetenciones);
                AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosTiposRetenciones>(this.MiOrdenCobro.OrdenesCobrosTiposRetenciones, false, this.gvRetenciones, true);
                //this.Calcular();
            }

        }

        protected void gvRetenciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CobOrdenesCobrosTiposRetenciones item = (CobOrdenesCobrosTiposRetenciones)e.Row.DataItem;
                CurrencyTextBox txtImporteTotalRetencion = (CurrencyTextBox)e.Row.FindControl("txtImporteRetencion");
                DropDownList ddlRetenciones = ((DropDownList)e.Row.FindControl("ddlTipoRetenciones"));
                DropDownList ddlConceptos = ((DropDownList)e.Row.FindControl("ddlTipoConceptos"));
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                TextBox txtNumeroCert = (TextBox)e.Row.FindControl("txtNumeroCertificado");

                ddlRetenciones.Enabled = false;
                txtImporteTotalRetencion.Enabled = false;
                btnEliminar.Visible = false;
                txtNumeroCert.Enabled = false;

                ddlRetenciones.DataSource = this.MisRetenciones;//TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPercepciones);
                ddlRetenciones.DataValueField = "IdListaValorSistemaDetalle";
                ddlRetenciones.DataTextField = "Descripcion";
                ddlRetenciones.DataBind();
                if (ddlRetenciones.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(ddlRetenciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                ListItem itemCombo;

                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ddlRetenciones.SelectedValue = item.TipoRetencion.IdTipoRetencion.ToString();
                        ddlRetenciones.Enabled = true;
                        txtImporteTotalRetencion.Attributes.Add("onchange", "CalcularItem();");
                        txtImporteTotalRetencion.Enabled = true;
                        btnEliminar.Visible = true;
                        txtNumeroCert.Enabled = true;
                        ddlConceptos.Enabled = true;
                        if (item.TipoRetencion.IdTipoRetencion > 0)
                        {
                            this.ddlTipoRetenciones_SelectedIndexChanged(ddlRetenciones, EventArgs.Empty);
                            if (item.IdRefImpuestoRetencion > 0)
                                ddlConceptos.SelectedValue = item.IdRefImpuestoRetencion.ToString();
                        }
                        else
                            AyudaProgramacion.AgregarItemSeleccione(ddlConceptos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                        break;
                    case Gestion.Consultar:
                    case Gestion.Autorizar:
                        itemCombo = ddlRetenciones.Items.FindByValue(item.TipoRetencion.IdTipoRetencion.ToString());
                        if (itemCombo == null)
                            ddlRetenciones.Items.Add(new ListItem(item.TipoRetencion.Descripcion, item.TipoRetencion.IdTipoRetencion.ToString()));
                        ddlRetenciones.SelectedValue = item.TipoRetencion.IdTipoRetencion.ToString();
                        ddlRetenciones.Text = item.TipoRetencion.Descripcion;
                        ddlRetenciones.Enabled = false;

                        itemCombo = ddlConceptos.Items.FindByValue(item.IdRefImpuestoRetencion.ToString());
                        if (itemCombo == null)
                            ddlConceptos.Items.Add(new ListItem(item.Concepto, item.IdRefImpuestoRetencion.ToString()));
                        ddlConceptos.SelectedValue = item.IdRefImpuestoRetencion.ToString();
                        ddlConceptos.Enabled = false;

                        break;
                    default:
                        break;
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotalRetencion = (Label)e.Row.FindControl("lblImporteTotalRetencion");
                lblImporteTotalRetencion.Text = this.MiOrdenCobro.ImporteRetenciones.ToString("C2");
            }
        }

        protected void btnAgregarRetencion_Click(object sender, EventArgs e)
        {
            CobOrdenesCobrosTiposRetenciones item;
            item = new CobOrdenesCobrosTiposRetenciones();
            this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.Add(item);
            item.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.IndexOf(item);
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosTiposRetenciones>(this.MiOrdenCobro.OrdenesCobrosTiposRetenciones, false, this.gvRetenciones, true);
            this.upRetenciones.Update();
        }

        protected void ddlTipoRetenciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlRetenciones = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlRetenciones.NamingContainer;
            if (string.IsNullOrEmpty(ddlRetenciones.SelectedValue))
                return;

            TGETiposRetenciones tipoRet = new TGETiposRetenciones();
            tipoRet.IdTipoRetencion = Convert.ToInt32(ddlRetenciones.SelectedValue);
            DropDownList ddlConceptos = (DropDownList)gvRow.FindControl("ddlTipoConceptos");
            List<TGEListasValoresSistemasDetalles> listaConceptos = TGEGeneralesF.ListasValoresSistemasDetallesObtenerRetencionesConceptos(tipoRet);
            ddlConceptos.DataSource = listaConceptos;
            ddlConceptos.DataValueField = "IdListaValorSistemaDetalle";
            ddlConceptos.DataTextField = "Descripcion";
            ddlConceptos.DataBind();
            if (ddlConceptos.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(ddlConceptos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }
        #endregion

        #region Grilla Anticipos

        private void PersistirAnticipos()
        {
            if (this.MiOrdenCobro.OrdenesCobrosAnticipos.Count > 0)
            {
                foreach (GridViewRow fila in this.gvAnticipos.Rows)
                {
                    CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                    this.MiOrdenCobro.OrdenesCobrosAnticipos[fila.DataItemIndex].IncluirEnOC = chkIncluir.Checked;
                    this.MiOrdenCobro.OrdenesCobrosAnticipos[fila.DataItemIndex].ImporteAplicar = ((CurrencyTextBox)fila.FindControl("txtImporteAnticipo")).Decimal;
                    if (this.MiOrdenCobro.OrdenesCobrosAnticipos[fila.DataItemIndex].ImporteAplicar > 0 && chkIncluir.Checked)
                        this.MiOrdenCobro.OrdenesCobrosAnticipos[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Agregado;
                    else
                        this.MiOrdenCobro.OrdenesCobrosAnticipos[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.SinCambio;

                }
            }
        }

        protected void gvAnticipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                        ibtnConsultar.Visible = true;
                        ibtnConsultar.Attributes.Add("onchange", "CalcularItem();");
                        CurrencyTextBox txtImporteAplicar = (CurrencyTextBox)e.Row.FindControl("txtImporteAnticipo");
                        txtImporteAplicar.Visible = true;
                        txtImporteAplicar.Attributes.Add("onchange", "CalcularItem();");
                        Label lblImporteAplicado = (Label)e.Row.FindControl("lblImporteAnticipo");
                        lblImporteAplicado.Visible = false;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotalAnticipo = (Label)e.Row.FindControl("lblImporteTotalAnticipo");
                lblImporteTotalAnticipo.Text = this.MiOrdenCobro.OrdenesCobrosAnticipos.Sum(x=>x.ImporteAplicado).ToString("C2");
            }
        }
        #endregion

        #region Clientes
        void CtrBuscarCliente_BuscarCliente(AfiAfiliados e)
        {
            if (e.IdAfiliado > 0)
            {
                MiOrdenCobro.Afiliado.IdAfiliado = ctrBuscarCliente.MiAfiliado.IdAfiliado;
                if (!string.IsNullOrEmpty(this.ddlTipoOperacionOC.SelectedValue))
                    this.ddlTipoOperacionOC_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            { }
        }

        //protected void ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        //{
        //    if (this.MisParametrosUrl.Contains("IdAfiliado"))
        //        this.MisParametrosUrl["IdAfiliado"] = e.IdAfiliado;
        //    this.MiOrdenCobro.Afiliado = e;
        //    this.txtCodigoSocio.Text = e.IdAfiliado.ToString();
        //    this.txtRazonSocial.Text = e.ApellidoNombre;
        //    this.txtDetalle.Text = e.Detalle;
        //    this.lblCuil.Text = e.TipoDocumento.TipoDocumento;
        //    this.txtCuil.Text = e.NumeroDocumento.ToString();
        //    this.upEntidades.Update();

        //    if (!string.IsNullOrEmpty(this.ddlTipoOperacionOC.SelectedValue))
        //        this.ddlTipoOperacionOC_SelectedIndexChanged(null, EventArgs.Empty);
        //}

        //protected void btnBuscarCliente_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarClientePopUp.IniciarControl(true);
        //}
        //protected void txtCodigoSocio_TextChanged(object sender, EventArgs e)
        //{
        //    AfiAfiliados afiliado = new AfiAfiliados();
        //    afiliado.IdAfiliado = this.txtCodigoSocio.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoSocio.Text);
        //    afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(afiliado);
        //    if (afiliado.IdAfiliado > 0)
        //        this.ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(afiliado);
        //    else
        //        this.ctrBuscarClientePopUp.IniciarControl(true);
        //}
        #endregion
    }
}