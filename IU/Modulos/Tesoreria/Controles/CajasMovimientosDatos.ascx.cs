using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;
using Comunes.Entidades;
using Contabilidad.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Contabilidad;
using Tesorerias;
using System.Xml;
using System.Collections;
using Seguridad.FachadaNegocio;
using Reportes.FachadaNegocio;
using System.Reflection;
using CuentasPagar.Entidades;
using Cobros.Entidades;
using Prestamos.Entidades;

namespace IU.Modulos.Tesoreria.Controles
{
    public partial class CajasMovimientosDatos : ControlesSeguros
    {
        private TESCajasMovimientos MiMovimiento
        {
            get { return this.PropiedadObtenerValor<TESCajasMovimientos>("CajasMovimientosDatosMiMovimiento"); }
            set { this.PropiedadGuardarValor("CajasMovimientosDatosMiMovimiento", value); }
        }
        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "TesoreriasMovimientosAgregarMisMonedas"]; }
            set { Session[this.MiSessionPagina + "TesoreriasMovimientosAgregarMisMonedas"] = value; }
        }
        //protected Objeto MiRefTipoOperacion
        //{
        //    get
        //    {
        //        if (Session[this.MiSessionPagina + "PaginaCajasMiRefTipoOperacion"] != null)
        //            return (Objeto)Session[this.MiSessionPagina + "PaginaCajasMiRefTipoOperacion"];
        //        else
        //        {
        //            return (Objeto)(Session[this.MiSessionPagina + "PaginaCajasMiRefTipoOperacion"] = new Objeto());
        //        }
        //    }
        //    set { Session[this.MiSessionPagina + "PaginaCajasMiRefTipoOperacion"] = value; }
        //}
        private TESCajas MiCaja
        {
            get { return this.PropiedadObtenerValor<TESCajas>("CajasMovimientosDatosMiCaja"); }
            set { this.PropiedadGuardarValor("CajasMovimientosDatosMiCaja", value); }
        }

      

        private List<TGETiposOperaciones> MisTiposOperaciones
        {
            get { return this.PropiedadObtenerValor<List<TGETiposOperaciones>>("CajasMovimientosDatosMisTiposOperaciones"); }
            set { this.PropiedadGuardarValor("CajasMovimientosDatosMisTiposOperaciones", value); }
        }

        private List<CtbConceptosContables> MisConceptosContables
        {
            get { return (List<CtbConceptosContables>)Session[this.MiSessionPagina + "CajasMovimientosDatosMisConceptosContables"]; }
            set { Session[this.MiSessionPagina + "CajasMovimientosDatosMisConceptosContables"] = value; }
        }
        public bool ComprobantePlantillaCajas
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] != null)
                    return (bool)Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"];
                else
                {
                    return (bool)(Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] = false);
                }
            }
            set { Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] = value; }
        }

        private List<CtbCentrosCostosProrrateos> MisCentrosCostosProrrateos
        {
            get { return (List<CtbCentrosCostosProrrateos>)Session[this.MiSessionPagina + "CajasMovimientosDatosMisCentrosCostosProrrateos"]; }
            set { Session[this.MiSessionPagina + "CajasMovimientosDatosMisCentrosCostosProrrateos"] = value; }
        }

        //public delegate void ControlDatosAceptarEventHandler(TESCajasMovimientos e);
        //public event ControlDatosAceptarEventHandler ModificarDatosAceptar;
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(TESCajasMovimientos pCajaMovimiento, TESCajas pCaja, Gestion pGestion)

        {
            this.GestionControl = pGestion;
            this.MiMovimiento = pCajaMovimiento;
            this.MiCaja = pCaja;
            this.CargarCombos();
            switch (this.GestionControl)
            {    
                case Gestion.Agregar:
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                    this.btnAceptar.Attributes.Add("OnClick", funcion);
                    this.MiMovimiento.Fecha = pCaja.FechaAbrir;
                    this.ctrFechaCajaContable.IniciarControl(this.GestionControl, pCaja.FechaAbrir, pCaja.FechaAbrir);
                    this.txtDescripcion.Enabled = true;
                    this.ddlTipoOperacion.Enabled = true;
                    this.ddlMoneda.Enabled = true;
                    //this.pnlAltaDatos.Visible = true;
                    this.btnAceptar.Visible = true;

                    this.ctrCamposValores.IniciarControl(this.MiMovimiento, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Anular:
                    this.MiMovimiento = TesoreriasF.CajasMovimientosObtenerDatosCompletos(this.MiMovimiento);
                    this.MapearObjetoAControles();
                    this.btnImprimir.Visible = true;
                    this.txtDescripcion.Enabled = false;
                    this.ddlTipoOperacion.Enabled = false;
                    this.ddlMoneda.Enabled = false;
                    this.pnlAltaDatos.Visible = false;
                    this.btnAceptar.Visible = true;
                    pnlDetalleConceptos.Visible = this.MiMovimiento.CajasMovimientosConceptosContables.Count > 0;

                    if (!(pCajaMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.IngresosCajas ||
                        pCajaMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.EgresosCajas
                        || pCajaMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.EgresosCajasInternos
                        || pCajaMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.IngresosCajasInternos
                        ))
                    {
                        hplVerDetalle.Visible = true;
                        HiperLink_Click();
                        dvAnular.Visible = true;
                    }
                    break;
                case Gestion.Consultar:
                    this.MiMovimiento = TesoreriasF.CajasMovimientosObtenerDatosCompletos(this.MiMovimiento);
                    this.MapearObjetoAControles();
                    this.btnImprimir.Visible = true;
                    this.txtDescripcion.Enabled = false;
                    this.ddlTipoOperacion.Enabled = false;
                    this.ddlMoneda.Enabled = false;
                    this.pnlAltaDatos.Visible = false;
                    this.btnAceptar.Visible = false;
                    pnlDetalleConceptos.Visible = this.MiMovimiento.CajasMovimientosConceptosContables.Count>0;

                    if (!(MiMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.IngresosCajas ||
                        MiMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.EgresosCajas
                        || pCajaMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.EgresosCajasInternos
                        || pCajaMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.IngresosCajasInternos
                        ))
                    {
                        hplVerDetalle.Visible = true;
                        HiperLink_Click();
                    }
                    break;
                default:
                    break;
            }
        }

        protected void HiperLink_Click()
        {

            int IdTipoOperacion = Convert.ToInt32(MiMovimiento.TipoOperacion.IdTipoOperacion);
            int IdRefTipoOperacion = Convert.ToInt32(MiMovimiento.IdRefTipoOperacion);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            switch (this.GestionControl)
            {
                case Gestion.Consultar:
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

                    break;
                case Gestion.Anular:
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;

                    break;
            }
            filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
            //Si devuelve una URL Redirecciona si no muestra mensaje error
            if (filtroMenu.URL.Length != 0)
            {
                hplVerDetalle.NavigateUrl = AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL));
            }
        }

        private void CargarCombos()
        {
            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();
            this.ddlMoneda.DataSource = MisMonedas; // TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(EnumTGETiposFuncionalidades.CajasMovimientos);
            this.ddlTipoOperacion.DataSource = this.MisTiposOperaciones;
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (ddlTipoOperacion.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (ddlConceptosContables.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptosContables, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            CtbCentrosCostosProrrateos ccp = new CtbCentrosCostosProrrateos();
            ccp.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
            this.MisCentrosCostosProrrateos = ContabilidadF.CentrosCostosProrrateosObtenerCombo(ccp);
            ddlCentrosCostos.DataSource = this.MisCentrosCostosProrrateos;
            ddlCentrosCostos.DataValueField = "IdCentroCostoProrrateo";
            ddlCentrosCostos.DataTextField = "CentroCostoProrrateo";
            ddlCentrosCostos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlCentrosCostos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles()
        {
            this.txtNumero.Text = this.MiMovimiento.IdCajaMovimiento.ToString();
            this.ctrFechaCajaContable.IniciarControl(this.GestionControl, this.MiMovimiento.Fecha);
            this.txtDescripcion.Text = this.MiMovimiento.Descripcion;
            ListItem item = this.ddlTipoOperacion.Items.FindByValue(this.MiMovimiento.TipoOperacion.IdTipoOperacion.ToString());
            if (item == null)
                this.ddlTipoOperacion.Items.Add(new ListItem(this.MiMovimiento.TipoOperacion.TipoOperacion, this.MiMovimiento.TipoOperacion.IdTipoOperacion.ToString()));
            this.ddlTipoOperacion.SelectedValue = this.MiMovimiento.TipoOperacion.IdTipoOperacion.ToString();

            item = this.ddlMoneda.Items.FindByValue(this.MiMovimiento.CajaMoneda.Moneda.IdMoneda.ToString());
            if (item == null)
                this.ddlMoneda.Items.Add(new ListItem(this.MiMovimiento.CajaMoneda.Moneda.Descripcion, this.MiMovimiento.CajaMoneda.Moneda.IdMoneda.ToString()));
            this.ddlMoneda.SelectedValue = this.MiMovimiento.CajaMoneda.Moneda.IdMoneda.ToString();

            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosConceptosContables>(this.MiMovimiento.CajasMovimientosConceptosContables, false, this.gvDatos, true);
            this.ctrIngresosValores.IniciarControl(this.MiMovimiento, this.GestionControl, this.MiMovimiento.TipoOperacion,  this.UsuarioActivo.SectorPredeterminado, false);
            this.ctrAsientoMostrar.IniciarControl(this.MiMovimiento);

            this.ctrCamposValores.IniciarControl(this.MiMovimiento, new Objeto(), this.GestionControl);
        }

        private void MapearControlesAObjeto()
        {
            this.MiMovimiento.Fecha = this.ctrFechaCajaContable.dFechaCajaContable.Value;
            this.MiMovimiento.Descripcion = this.txtDescripcion.Text;
        }

        protected void ddlTipoOperacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlConceptosContables.Items.Clear();
            this.ddlConceptosContables.SelectedIndex = -1;
            this.ddlConceptosContables.SelectedValue = null;
            this.ddlConceptosContables.ClearSelection();

            this.MiMovimiento.CajasMovimientosConceptosContables = new List<TESCajasMovimientosConceptosContables>();
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosConceptosContables>(this.MiMovimiento.CajasMovimientosConceptosContables, false, this.gvDatos, true);

            if (string.IsNullOrEmpty(this.ddlTipoOperacion.SelectedValue))
            {
                this.ctrIngresosValores.IniciarControl(new TESCajasMovimientos(), Gestion.Agregar, new TGETiposOperaciones(), this.UsuarioActivo.SectorPredeterminado, true);
                this.ctrIngresosValores.ActualizarUpdatePanel();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptosContables, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                pnlDetalleConceptos.Visible =false;
                this.pnlAltaDatos.Visible = false;
                return;
            }
            pnlDetalleConceptos.Visible = true;
            this.pnlAltaDatos.Visible = true;
            TGETiposOperaciones opeFiltro = new TGETiposOperaciones();
            opeFiltro.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);// this.paginaSegura.paginaActual.IdTipoFuncionalidad;//(int)EnumTGETiposFuncionalidades.Cobros;
            opeFiltro.Estado.IdEstado = (int)Estados.Activo;

            this.MisConceptosContables = ContabilidadF.ConceptosContablesObtenerListaFiltro(opeFiltro);
            this.ddlConceptosContables.DataSource = this.MisConceptosContables;
            this.ddlConceptosContables.DataValueField = "IdConceptoContable";
            this.ddlConceptosContables.DataTextField = "ConceptoContable";
            this.ddlConceptosContables.DataBind();
            if (ddlConceptosContables.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptosContables, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            else
                this.Concepto_OnSelectedIndexChanged(this.ddlConceptosContables.SelectedValue, e);
            this.upTipoOperacionConceptos.Update();

            ddlMoneda_OnSelectedIndexChanged(ddlMoneda, EventArgs.Empty);

        }
        protected void Concepto_OnSelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(this.ddlConceptosContables.SelectedValue))
            {
                TESCajasMovimientosConceptosContables movcon = new TESCajasMovimientosConceptosContables();
                movcon.ConceptoContable.IdConceptoContable = Convert.ToInt32(ddlConceptosContables.SelectedValue);
                this.ctrCamposValoresConceptosContables.IniciarControl(movcon, movcon.ConceptoContable, this.GestionControl);
                
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCajasMovimientosConceptosContables detalle = this.MiMovimiento.CajasMovimientosConceptosContables[indiceColeccion];
            this.MiMovimiento.CajasMovimientosConceptosContables.Remove(detalle);

            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosConceptosContables>(this.MiMovimiento.CajasMovimientosConceptosContables, false, this.gvDatos, true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                        ibtnEliminar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = this.MiMovimiento.CajasMovimientosConceptosContables.Sum(x => x.Importe).ToString("C2");
            }
        }

        protected void btnIngresarConcepto_Click(object sender, EventArgs e)
        {
            this.Page.Validate("IngresarConcepto");
            if (!this.Page.IsValid)
                return;

            if (this.txtImporte.Decimal <= 0)
            {
                this.MostrarMensaje("ValidarImporteMayorCero", true);
                return;
            }

            TESCajasMovimientosConceptosContables detalle = new TESCajasMovimientosConceptosContables();
            detalle.EstadoColeccion = EstadoColecciones.Agregado;
            detalle.ConceptoContable = this.MisConceptosContables[this.ddlConceptosContables.SelectedIndex];
            if(ddlCentrosCostos.SelectedValue != string.Empty)
                detalle.CentroCostoProrrateo = this.MisCentrosCostosProrrateos[this.ddlCentrosCostos.SelectedIndex];

            detalle.Detalle = this.txtDetalle.Text;
            detalle.Importe = this.txtImporte.Decimal;
            this.MiMovimiento.CajasMovimientosConceptosContables.Add(detalle);
            detalle.IndiceColeccion = this.MiMovimiento.CajasMovimientosConceptosContables.IndexOf(detalle);
            detalle.Campos = ctrCamposValoresConceptosContables.ObtenerLista();
            detalle.LoteCajasMovimientosValores = ctrCamposValoresConceptosContables.ObtenerListaCamposValores();
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosConceptosContables>(this.MiMovimiento.CajasMovimientosConceptosContables, false, this.gvDatos, true);

            this.txtImporte.Text = string.Empty;
            ddlCentrosCostos.SelectedValue = string.Empty;
            this.txtDetalle.Text = string.Empty;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("Aceptar");
            if (!this.Page.IsValid)
            {
                this.upTipoOperacionConceptos.Update();
                return;
            }
            this.btnAceptar.Visible = false;
    
            this.MiCaja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MiMovimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    #region Agregar
                    var cajaMoneda = this.MiCaja.CajasMonedas.Find(x => x.Moneda.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
                    TGEMonedas mon = MisMonedas.Find(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
                    MiMovimiento.TipoOperacion = this.MisTiposOperaciones[this.ddlTipoOperacion.SelectedIndex];
                    MiMovimiento.Fecha = Convert.ToDateTime(ctrFechaCajaContable.dFechaCajaContable);  //this.MiCaja.FechaAbrir;// DateTime.Now;
                    MiMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                    MiMovimiento.Descripcion = this.txtDescripcion.Text.Trim();
                    MiMovimiento.CajasMovimientosValores = new List<TESCajasMovimientosValores>(ctrIngresosValores.ObtenerCajaMovimiento().CajasMovimientosValores);
                    MiMovimiento.Importe = MiMovimiento.CajasMovimientosValores.Sum(x => x.Importe);
                    MiMovimiento.CajaMoneda = cajaMoneda;
                    MiMovimiento.Campos = this.ctrCamposValores.ObtenerLista();
                    MiMovimiento.MonedaCotizacion = mon.MonedeaCotizacion.MonedaCotizacion;

                    if (MiMovimiento.CajasMovimientosConceptosContables.Count == 0)
                    {
                        this.MostrarMensaje("ValidarCantidadItems", true);
                        this.btnAceptar.Visible = true;
                        return;
                    }

                    decimal totalConcepto = MiMovimiento.CajasMovimientosConceptosContables.Sum(x => x.Importe);
                    if (totalConcepto != MiMovimiento.Importe)
                    {
                        this.MostrarMensaje("ValidarMovimientosValoresImporte", true);
                        this.btnAceptar.Visible = true;
                        return;
                    }

                    TESCajasMovimientos valor = new TESCajasMovimientos();
                    valor.TipoOperacion = this.MisTiposOperaciones[this.ddlTipoOperacion.SelectedIndex];
                    valor.IdRefTipoOperacion = Convert.ToInt32(MiMovimiento.IdRefTipoOperacion);
                    valor.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
                    bool bvalor = TesoreriasF.CajasMovimientosCircuitoDiarioCajasAutomatico(valor);
                 

                    if (!bvalor)
                    {
                        if (!TesoreriasF.CajasConfirmarMovimiento(this.MiCaja, MiMovimiento))
                        {
                            this.btnAceptar.Visible = true;
                            this.MostrarMensaje(this.MiCaja.CodigoMensaje, true, this.MiCaja.CodigoMensajeArgs);
                            if (this.MiMovimiento.dsResultado != null)
                            {
                                this.ctrPopUpGrilla.IniciarControl(this.MiMovimiento);
                                this.MiMovimiento.dsResultado = null;
                            }
                        }
                        else
                        {
                            this.MostrarMensaje(this.MiCaja.CodigoMensaje, false);
                            this.IniciarControl(this.MiMovimiento, this.MiCaja, Gestion.Consultar);
                            this.upTipoOperacionConceptos.Update();
                            this.ctrIngresosValores.ActualizarUpdatePanel();
                        }
                    }
                    else
                    {
                        this.MiMovimiento = TesoreriasF.CajasMovimientosObtenerMovimientoValoresXML(this.MiMovimiento);
                        this.MiMovimiento.IdFilial = this.MiCaja.Tesoreria.Filial.IdFilial;
                        this.MiMovimiento.Fecha = this.ctrFechaCajaContable.dFechaCajaContable.Value;

                        XmlNode nodoraiz = this.MiMovimiento.LoteCajasMovimientosValores.SelectSingleNode("CajasMovimientosValores");
                        
                        XmlElement nodo;
                        XmlAttribute attribute;
                        XmlNode nodoCV;
                        XmlNode itemNodo;
                        XmlAttribute itemAttribute;
                        int idAuxiliar = 0;
                        foreach (TESCajasMovimientosConceptosContables concepto in this.MiMovimiento.CajasMovimientosConceptosContables)
                        {
                            idAuxiliar++;
                            nodo = this.MiMovimiento.LoteCajasMovimientosValores.CreateElement("CajasMovimientoConceptosContable");

                            attribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdConceptoContable");
                            attribute.Value = concepto.ConceptoContable.IdConceptoContable.ToString();
                            nodo.Attributes.Append(attribute);

                            attribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("Detalle");
                            attribute.Value = concepto.Detalle.ToString();
                            nodo.Attributes.Append(attribute);

                            attribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = concepto.Importe.ToString().Replace(',', '.'); ;
                            nodo.Attributes.Append(attribute);

                            attribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdCentroCostoProrrateo");
                            attribute.Value = concepto.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                            nodo.Attributes.Append(attribute);

                            attribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdCuentaContable");
                            attribute.Value = concepto.ConceptoContable.CuentaContable.IdCuentaContable.ToString();
                            nodo.Attributes.Append(attribute);

                            attribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdAuxiliar");
                            attribute.Value = idAuxiliar.ToString();
                            nodo.Attributes.Append(attribute);

                            nodoCV = this.MiMovimiento.LoteCajasMovimientosValores.CreateElement("CamposValores");

                            foreach (TGECampos item in concepto.Campos)
                            {
                                itemNodo = this.MiMovimiento.LoteCajasMovimientosValores.CreateElement("CampoValor");
                                itemAttribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdCampo");
                                itemAttribute.Value = item.IdCampo.ToString();
                                itemNodo.Attributes.Append(itemAttribute);
                                itemAttribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("Nombre");
                                itemAttribute.Value = item.Nombre;
                                itemNodo.Attributes.Append(itemAttribute);
                                itemAttribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("Valor");
                                itemAttribute.Value = item.CampoValor.Valor;
                                itemNodo.Attributes.Append(itemAttribute);
                                itemAttribute = this.MiMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdAuxiliar");
                                itemAttribute.Value = idAuxiliar.ToString();
                                itemNodo.Attributes.Append(itemAttribute);
                                nodoCV.AppendChild(itemNodo);

                            }
                            nodo.AppendChild(nodoCV);
                            nodoraiz.AppendChild(nodo);
                        }
                      
                            if (!TesoreriasF.CajasConfirmarMovimientoXml(this.MiMovimiento))
                        {
                            this.btnAceptar.Visible = true;
                            this.MostrarMensaje(this.MiMovimiento.CodigoMensaje, true, this.MiMovimiento.CodigoMensajeArgs);
                            if (this.MiMovimiento.dsResultado != null)
                            {
                                this.ctrPopUpGrilla.IniciarControl(this.MiMovimiento);
                                this.MiMovimiento.dsResultado = null;
                            }
                        }
                        else
                        {
                            this.MostrarMensaje(this.MiMovimiento.CodigoMensaje, false);
                            this.IniciarControl(this.MiMovimiento, this.MiCaja, Gestion.Consultar);
                            this.upTipoOperacionConceptos.Update();
                            this.ctrIngresosValores.ActualizarUpdatePanel();
                        }
                    }
                    #endregion
                    break;
                case Gestion.Anular:
                    if (!TesoreriasF.CajasAnularMovimientoIngresosEgresos(this.MiMovimiento))
                    {
                        this.btnAceptar.Visible = true;
                        this.MostrarMensaje(this.MiMovimiento.CodigoMensaje, true, this.MiMovimiento.CodigoMensajeArgs);
                        if (this.MiMovimiento.dsResultado != null)
                        {
                            this.ctrPopUpGrilla.IniciarControl(this.MiMovimiento);
                            this.MiMovimiento.dsResultado = null;
                        }
                    }
                    else
                    {
                        this.MostrarMensaje(this.MiMovimiento.CodigoMensaje, false);
                        this.IniciarControl(this.MiMovimiento, this.MiCaja, Gestion.Consultar);
                        this.upTipoOperacionConceptos.Update();
                        this.ctrIngresosValores.ActualizarUpdatePanel();
                    }
                    break;
            }
        }

        protected void ddlMoneda_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
            {
                List<TESCajasMonedas> monedas = this.MiCaja.CajasMonedas;
                var idMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
                MiMovimiento.CajaMoneda = monedas.Find(delegate (TESCajasMonedas m) { return m.Moneda.IdMoneda == idMoneda; });
                SetInitializeCulture(MiMovimiento.CajaMoneda.Moneda.Moneda);
                if (!string.IsNullOrEmpty(ddlTipoOperacion.SelectedValue))
                {

                    this.ctrIngresosValores.IniciarControl(MiMovimiento, Gestion.Agregar, this.MisTiposOperaciones[this.ddlTipoOperacion.SelectedIndex], this.UsuarioActivo.SectorPredeterminado, true);
                }
            }
            this.ctrIngresosValores.ActualizarUpdatePanel();
            //upMovimientos.Update();


        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            ModificarDatosCancelar();
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComprobantePlantillaCajas);
            bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
            int idComprobante;

            

            TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(MiMovimiento);

            miPlantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(miPlantilla);

            if (this.MiMovimiento.IdCajaMovimiento > 0 && bvalor)
            {
                idComprobante = this.MiMovimiento.IdCajaMovimiento;
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.TESCajasMovimientos, "TESCajasMovimientos", this.MiMovimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.upMovimientos, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
            }
            else if (miPlantilla.HtmlPlantilla.Trim().Length > 0) {
                Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), this.MiMovimiento.GetType().Name);

                if (comprobante != null)
                {
                    TGEPlantillas plantilla = new TGEPlantillas();
                    plantilla = TGEGeneralesF.PlantillasObtenerDatosPorComprobante((EnumTGEComprobantes)comprobante);

                    Objeto MiTipoOperacion = AyudaProgramacion.ObtenerIdTipoOperacion(MiMovimiento);


                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante, miPlantilla.Codigo, MiTipoOperacion, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.upMovimientos, string.Concat("Comprobante_", MiMovimiento.IdCajaMovimiento.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                }
            }
            else
            {
                Type t2 = MiMovimiento.GetType();
                PropertyInfo prop = t2.GetProperty("TipoOperacion");
                TGETiposOperaciones operacion = (TGETiposOperaciones)prop.GetValue(this.MiMovimiento, null);
                prop = this.MiMovimiento.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
                idComprobante = Convert.ToInt32(prop.GetValue(this.MiMovimiento, null));

                Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), this.MiMovimiento.GetType().Name);

                if (comprobante != null)
                {
                    TGEPlantillas plantilla = new TGEPlantillas();
                    plantilla = TGEGeneralesF.PlantillasObtenerDatosPorComprobante((EnumTGEComprobantes)comprobante);

                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante, plantilla.Codigo, this.MiMovimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.upMovimientos, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                }
            }

                    //this.ctrPopUpComprobantes.CargarReporte(this.MiMovimiento, EnumTGEComprobantes.TESCajasMovimientos);
        }

       

    }
}