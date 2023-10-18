using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Afiliados.Entidades;
using Afiliados;
using System.Web.Services;
using System.Web.Script.Services;
using Generales.Entidades;
using System.Data;
using Servicio.AccesoDatos;
using Hoteles.Entidades;
using Generales.FachadaNegocio;
using Hoteles;
using Evol.Controls;
using System.Globalization;
using System.Collections;
using Seguridad.FachadaNegocio;
using Facturas;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Hotel.Controles
{
    public partial class ReservasDatos : ControlesSeguros
    {
        public HTLReservas MiReserva
        { get { return this.PropiedadObtenerValor<HTLReservas>("ReservasDatosMiReserva");}
            set { this.PropiedadGuardarValor("ReservasDatosMiReserva", value); }
        }

        public List<HTLHoteles> MiHotel
        {
            get { return this.PropiedadObtenerValor<List<HTLHoteles>>("ReservasDatosMiHotel"); }
            set { this.PropiedadGuardarValor("ReservasDatosMiHotel", value); }
        }

        public List<TGEListasValoresDetalles> MisTiposProductosHoteles
        {
            get { return this.PropiedadObtenerValor<List<TGEListasValoresDetalles>>("ReservasDatosMisTiposProductosHoteles"); }
            set { this.PropiedadGuardarValor("ReservasDatosMisTiposProductosHoteles", value); }
        }

        public List<HTLDescuentos> MisDescuentos
        {
            get { return this.PropiedadObtenerValor<List<HTLDescuentos>>("ReservasDatosMisTipoDescuentos"); }
            set { this.PropiedadGuardarValor("ReservasDatosMisTipoDescuentos", value); }
        }

        private int IdReservadetalleIndice
        {
            get { return this.PropiedadObtenerValor<int>("ReservasDatosIdReservadetalle"); }
            set { this.PropiedadGuardarValor("ReservasDatosIdReservadetalle", value); }
        }


        bool AceptarContinuar = false;

        public List<HTLReservasDetallesDescuentos> MisReservasDetallesDescuentos
        {
            get { return this.PropiedadObtenerValor<List<HTLReservasDetallesDescuentos>>("ReservasDatosMisReservasDetallesDescuentos"); }
            set { this.PropiedadGuardarValor("ReservasDatosMisReservasDetallesDescuentos", value); }
        }

        public List<AfiTiposDocumentos> MisTiposDocumentos
        {
            get { return this.PropiedadObtenerValor<List<AfiTiposDocumentos>>("ReservasDatosMisTiposDocumentos"); }
            set { this.PropiedadGuardarValor("ReservasDatosMisTiposDocumentos", value); }
        }

        private DataTable MiCuentaCorriente
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ReservasDatosMiCuentaCorriente"]; }
            set { Session[this.MiSessionPagina + "ReservasDatosMiCuentaCorriente"] = value; }
        }

        //public delegate void ControlDatosAceptarEventHandler(object sender, HTLReservas e);
        //public event ControlDatosAceptarEventHandler ControlModificarDatosAceptar;

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ctrReservasDetallesOpciones.ReservasDetallesOpcioneAceptar += CtrReservasDetallesOpciones_ReservasDetallesOpcioneAceptar;
            if (!this.IsPostBack)
            {
                ddlHoteles.Attributes.Add("OnChange", "HotelesHorarios();");
                this.btnAgregarComprobante.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                this.btnAgregarOC.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAgregar.aspx");
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCargaMasivaCantidad, this.btnCargaMasivaAgregar);

                HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
                if (parametros.BusquedaParametros)
                {
                    this.tcDatos.ActiveTab = this.tcDatos.Tabs[parametros.HashTransaccion];
                }
            }
        }        

        public void IniciarControl(HTLReservas pParametro, Gestion pGestion)
        {
            this.MisReservasDetallesDescuentos = new List<HTLReservasDetallesDescuentos>();
            this.MiReserva = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "InitControlsScript", "InitControls();", true);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlEstado.SelectedValue = ((int)EstadosReservas.PendienteConfirmacion).ToString();
                    //this.ddlHoraIngreso.SelectedValue = DateTime.ParseExact("15:00", "HH:mm", null).ToString("HH:mm");
                    //this.ddlHoraEgreso.SelectedValue = DateTime.ParseExact("10:00", "HH:mm", null).ToString("HH:mm");
                    this.AgregarItem(4);
                    this.AgregarOcupantes(4 );
                    this.ctrArchivos.IniciarControl(this.MiReserva, this.GestionControl);
                    this.ctrComentarios.IniciarControl(this.MiReserva, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiReserva, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Copiar:
                    HotelWS hws = new HotelWS();
                    List<Select2DTO> lista = hws.ListasPreciosHotelesCombo(this.MiReserva.IdHotel.ToString(), this.MiReserva.FechaIngreso.Value.ToShortDateString());
                    this.ddlListasPrecios.ClearSelection();
                    this.ddlListasPrecios.SelectedIndex = -1;
                    this.ddlListasPrecios.Items.Clear();
                    lista.ForEach(x => ddlListasPrecios.Items.Add(new ListItem(x.text, x.id.ToString())));
                    if (MiReserva.ReservasDetalles.Count > 0)
                    {
                        HTLReservasDetalles filtro = new HTLReservasDetalles();
                        filtro.IdTipoProductoHotel = MiReserva.ReservasDetalles[0].IdTipoProductoHotel;
                        filtro.FechaIngreso = MiReserva.ReservasDetalles[0].FechaIngreso;
                        filtro.FechaEgreso = MiReserva.ReservasDetalles[0].FechaEgreso;
                        filtro.IdHotel = MiReserva.IdHotel;
                        filtro.IdListaPrecio = ddlListasPrecios.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlListasPrecios.SelectedValue);
                        filtro.Compartida = MiReserva.ReservasDetalles[0].Compartida;
                        filtro.IdHabitacion = MiReserva.ReservasDetalles[0].IdHabitacion;
                        HTLReservasDetallesDTO detalle = HotelesF.ReservasDetallesObtenerDetalleGastos<HTLReservasDetallesDTO>(filtro);
                        MiReserva.ReservasDetalles[0].Precio = detalle.Precio;
                        MiReserva.ReservasDetalles[0].PrecioEditable = detalle.PrecioEditable;
                        MiReserva.ReservasDetalles[0].PrecioHabitacionCompartida = detalle.PrecioHabitacionCompartida;
                        MiReserva.ReservasDetalles[0].CantidadPersonas = detalle.CantidadPersonas;
                    }

                    this.ddlEstado.SelectedValue = ((int)EstadosReservas.PendienteConfirmacion).ToString();
                    this.GestionControl = Gestion.Agregar;
                    this.AgregarItem(3);
                    this.AgregarOcupantes(4);
                    this.MapearObjetoAControles(this.MiReserva);                    
                    //this.ddlHoraIngreso.SelectedValue = DateTime.ParseExact("15:00", "HH:mm", null).ToString("HH:mm");
                    //this.ddlHoraEgreso.SelectedValue = DateTime.ParseExact("10:00", "HH:mm", null).ToString("HH:mm");
                    
                    this.MisParametrosUrl = new Hashtable();
                    this.ddlHoteles.Enabled = false;
                                        
                    break;
                case Gestion.Modificar:
                    this.btnImprimir.Visible = true;
                    this.MiReserva = HotelesF.ReservasObtenerDatosCompletos(this.MiReserva);
                    this.AgregarItem(2);
                    this.AgregarOcupantes(1);
                    this.MapearObjetoAControles(this.MiReserva);
                    this.ddlReservaApellido.Enabled = false;
                    this.txtReservaNombre.Enabled = false;
                    this.ddlReservaTipoDocumento.Enabled = false;
                    this.txtReservaNumeroDocumento.Enabled = false;
                    this.ddlHoteles.Enabled = false;
                    this.ddlListasPrecios.Enabled = false;
                    break;
                case Gestion.Consultar:
                    this.MiReserva = HotelesF.ReservasObtenerDatosCompletos(this.MiReserva);
                    this.btnImprimir.Visible = true;
                    this.MapearObjetoAControles(this.MiReserva);
                    this.btnAceptar.Visible = false;
                    this.btnAceptarContinuar.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DeshabilitarControlesScript", "$(document).ready(function () { deshabilitarControles('deshabilitarControles') }); ;", true);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.MisTiposProductosHoteles = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposProductosHoteles);
            if (this.MisTiposProductosHoteles.Exists(x => x.CodigoValor.ToUpper() == "TPH01"))
                this.hdfCodigoTPH01.Value = this.MisTiposProductosHoteles.First(x => x.CodigoValor.ToUpper() == "TPH01").IdListaValorDetalle.ToString();
            if (this.MisTiposProductosHoteles.Exists(x => x.CodigoValor.ToUpper() == "TPH02"))
                this.hdfCodigoTPH02.Value = this.MisTiposProductosHoteles.First(x => x.CodigoValor.ToUpper() == "TPH02").IdListaValorDetalle.ToString();
            if (this.MisTiposProductosHoteles.Exists(x => x.CodigoValor.ToUpper() == "TPH03"))
                this.hdfCodigoTPH03.Value = this.MisTiposProductosHoteles.First(x => x.CodigoValor.ToUpper() == "TPH03").IdListaValorDetalle.ToString();
            if (this.MisTiposProductosHoteles.Exists(x => x.CodigoValor.ToUpper() == "TPH05"))
                this.hdfCodigoTPH05.Value = this.MisTiposProductosHoteles.First(x => x.CodigoValor.ToUpper() == "TPH05").IdListaValorDetalle.ToString();


            this.ddlCargaMasivaProductos.DataSource = HotelesF.ReservasObtenerProductosPorTipo( new HTLReservasDetalles() { IdTipoProductoHotel=this.hdfCodigoTPH01.Value==string.Empty ? default(int?) : Convert.ToInt32(this.hdfCodigoTPH01.Value) });
            this.ddlCargaMasivaProductos.DataValueField = "IdProducto";
            this.ddlCargaMasivaProductos.DataTextField = "Descripcion";
            this.ddlCargaMasivaProductos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCargaMasivaProductos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MisTiposDocumentos = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlReservaTipoDocumento.DataSource = this.MisTiposDocumentos;
            this.ddlReservaTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlReservaTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlReservaTipoDocumento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlReservaTipoDocumento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosReservas));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscal.DataTextField = "Descripcion";
            this.ddlCondicionFiscal.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));

          



            //this.ddlListasPrecios.DataSource = Servicio.AccesoDatos.BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPListasPreciosSeleccionarHoteles", new Objeto());
            //this.ddlListasPrecios.DataValueField = "IdListaPrecio";
            //this.ddlListasPrecios.DataTextField = "Descripcion";
            //this.ddlListasPrecios.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlListasPrecios, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            /* Time DropDownList*/
            DateTime StartTime = DateTime.ParseExact("00:00", "HH:mm", null);
            DateTime EndTime = DateTime.ParseExact("23:55", "HH:mm", null);
            //Set 5 minutes interval
            TimeSpan Interval = new TimeSpan(0, 30, 0);
            //To set 1 hour interval
            //TimeSpan Interval = new TimeSpan(1, 0, 0);           
            ddlHoraIngreso.Items.Clear();
            ddlHoraEgreso.Items.Clear();
            while (StartTime <= EndTime)
            {
                ddlHoraIngreso.Items.Add(new ListItem(StartTime.ToString("HH:mm"), StartTime.ToString("HH:mm")));
                ddlHoraEgreso.Items.Add(new ListItem(StartTime.ToString("HH:mm"), StartTime.ToString("HH:mm")));
                StartTime = StartTime.Add(Interval);
            }
            AyudaProgramacion.InsertarItemSeleccione(this.ddlHoraIngreso, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.InsertarItemSeleccione(this.ddlHoraEgreso, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            HTLHoteles hotel = new HTLHoteles();
            hotel.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MiHotel = HotelesF.HotelesObtenerListaActiva(hotel);
            this.ddlHoteles.DataSource = MiHotel;
            this.ddlHoteles.DataValueField = "IdHotel";
            this.ddlHoteles.DataTextField = "Descripcion";
            this.ddlHoteles.DataBind();
            if (ddlHoteles.Items.Count != 1)
            {
                this.ddlHoraIngreso.SelectedValue = DateTime.ParseExact("15:00", "HH:mm", null).ToString("HH:mm");
                this.ddlHoraEgreso.SelectedValue = DateTime.ParseExact("10:00", "HH:mm", null).ToString("HH:mm");
                AyudaProgramacion.AgregarItemSeleccione(this.ddlHoteles, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
                ddlHoteles_OnSelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlHoteles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlHoteles.SelectedValue))
            {
                if (Convert.ToInt32(ddlHoteles.SelectedValue) > 0)
                {

                    //HTLHoteles hotel = MiHotel.FirstOrDefault(x => x.IdHotel == Convert.ToInt32(this.ddlHoteles.SelectedValue));

                    ////ddlHoraIngreso.SelectedValue = hotel.HorarioIngreso.ToString();
                    ////ddlHoraEgreso.SelectedValue = hotel.HorarioEgreso.ToString();
                    //TimeSpan HorarioIngreso = TimeSpan.ParseExact(hotel.HorarioIngreso.ToString(), "HH:mm", null);
                    //ListItem item = ddlHoraIngreso.Items.FindByValue(DateTime.Today.Add(hotel.HorarioIngreso).ToString("HH:mm"));
                    //if (item==null)

                    //this.ddlHoraIngreso.Items.Add(new ListItem(hotel.HorarioIngreso.ToString("HH:mm"), HorarioIngreso.ToString("HH:mm")));
                    //this.ddlHoraIngreso.SelectedValue = item.ToString();

                    //this.ddlHoraEgreso.Items.Add(new ListItem(hotel.HorarioEgreso.ToString()));
                    //this.ddlHoraEgreso.SelectedValue = hotel.HorarioEgreso.ToString();
                    ////upReservaDatos.Update();
                    ////ScriptManager.RegisterStartupScript(this, this.GetType(), "CalcularFechaEgreso", "CalcularFechaEgreso();", true);
                }
            }
            
        }


        private void MapearObjetoAControles(HTLReservas pReserva)
        {
            this.txtNumeroReserva.Text = pReserva.IdReserva.ToString();
            this.ddlHoteles.SelectedValue = pReserva.IdHotel.ToString();
            ListItem item = this.ddlListasPrecios.Items.FindByValue(pReserva.IdListaPrecio.ToString());
            if (item == null && pReserva.IdListaPrecio > 0)
                this.ddlListasPrecios.Items.Add(new ListItem(pReserva.ListaPrecio, pReserva.IdListaPrecio.ToString()));
            this.ddlListasPrecios.SelectedValue = pReserva.IdListaPrecio.ToString();
            item = this.ddlCondicionFiscal.Items.FindByValue(pReserva.IdCondicionFiscal.ToString());
            if (item != null & pReserva.IdCondicionFiscal > 0)
                this.ddlCondicionFiscal.SelectedValue = pReserva.IdCondicionFiscal.ToString();
            this.ddlEstado.SelectedValue = pReserva.Estado.IdEstado.ToString();
            this.ddlReservaTipoDocumento.SelectedValue = pReserva.IdTipoDocumento.ToString();
            this.txtReservaNumeroDocumento.Text = pReserva.NumeroDocumento;
            this.ddlReservaApellido.Items.Add(new ListItem(pReserva.Apellido, pReserva.IdAfiliado.HasValue ? pReserva.IdAfiliado.ToString() : pReserva.Apellido));
            this.hdfReservaIdAfiliado.Value = pReserva.IdAfiliado.HasValue ? pReserva.IdAfiliado.ToString() : string.Empty;
            this.hdfReservaIdAfiliadoTipo.Value = pReserva.IdAfiliadoTipo.HasValue? pReserva.IdAfiliadoTipo.ToString() : string.Empty;
            this.hdfReservaApellido.Value = pReserva.Apellido;
            this.txtReservaNombre.Text = pReserva.Nombre;
            this.txtReservaCorreoElectronico.Text = pReserva.CorreoElectronico;
            txtEstadoSocio.Text = pReserva.EstadoSocio;
            txtCategoria.Text = pReserva.CategoriaSocio;
            this.txtReservaFechaIngreso.Text = pReserva.FechaIngreso.Value.ToShortDateString();
            this.txtReservaFechaEgreso.Text = pReserva.FechaEgreso.Value.ToShortDateString();
            TimeSpan t = this.MiReserva.FechaEgreso.Value - this.MiReserva.FechaIngreso.Value;
            this.txtCantidadDias.Text = (Convert.ToInt32(t.TotalDays)).ToString();
            this.ddlHoraIngreso.SelectedValue = pReserva.FechaIngreso.Value.ToString("HH:mm");
            this.ddlHoraEgreso.SelectedValue = pReserva.FechaEgreso.Value.ToString("HH:mm");
            this.txtCantidadPersonasNumero.Text = pReserva.CantidadPersonas.ToString();
            AyudaProgramacion.CargarGrillaListas<HTLReservasDetalles>(pReserva.ReservasDetalles, false, this.gvDatos, true);
            this.MisReservasDetallesDescuentos = new List<HTLReservasDetallesDescuentos>();
            pReserva.ReservasDetalles.ForEach(x => this.MisReservasDetallesDescuentos.AddRange(x.ReservasDetallesDescuentos));
            if(this.MisReservasDetallesDescuentos.Count>0)
                AyudaProgramacion.CargarGrillaListas<HTLReservasDetallesDescuentos>(this.MisReservasDetallesDescuentos, false, this.gvDescuentos, true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "InitGrillaDescuentoScript", "InitGrillaDescuento();", true);
            AyudaProgramacion.CargarGrillaListas<HTLReservasOcupantes>(pReserva.ReservasOcupantes, false, this.gvOcupante, true);
            if (pReserva.IdAfiliado.HasValue)
            {
                AfiAfiliados afi = new AfiAfiliados();
                afi.IdAfiliado = Convert.ToInt32(pReserva.IdAfiliado.Value);
                afi.IdRefTabla = pReserva.IdReserva;
                afi.Tabla = typeof(HTLReservas).Name;
                this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorClienteIdRefTablaTable(afi);
                this.gvCuentaCorriente.DataSource = this.MiCuentaCorriente;
                this.gvCuentaCorriente.DataBind();
            }
            this.ctrArchivos.IniciarControl(pReserva, this.GestionControl);
            this.ctrComentarios.IniciarControl(pReserva, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pReserva);
            this.ctrCamposValores.IniciarControl(pReserva, new Objeto(), this.GestionControl);
        }

        private void MapearControlesAObjeto(HTLReservas pReserva)
        {
            pReserva.IdHotel = Convert.ToInt32( this.ddlHoteles.SelectedValue);
            string key = this.Request.Form.AllKeys.First(x => x.Contains("$ddlListasPrecios"));
            if (!string.IsNullOrEmpty(key))
                pReserva.IdListaPrecio = this.Request.Form[key] == string.Empty ? 0 : Convert.ToInt32(this.Request.Form[key]);
            key = this.Request.Form.AllKeys.First(x => x.Contains("ddlCondicionFiscal"));
            if (!string.IsNullOrEmpty(key))
                pReserva.IdCondicionFiscal = this.Request.Form[key] == string.Empty ? 0 : Convert.ToInt32(this.Request.Form[key]);
            pReserva.FechaIngreso = Convert.ToDateTime(this.txtReservaFechaIngreso.Text);
            pReserva.FechaEgreso = Convert.ToDateTime(this.txtReservaFechaEgreso.Text);
            if (!string.IsNullOrEmpty(this.ddlHoraIngreso.SelectedValue))
            {
                pReserva.FechaIngreso = pReserva.FechaIngreso.Value.AddHours(Convert.ToInt32(this.ddlHoraIngreso.SelectedValue.Split(':')[0].Trim()));
                pReserva.FechaIngreso = pReserva.FechaIngreso.Value.AddMinutes(Convert.ToInt32(this.ddlHoraIngreso.SelectedValue.Split(':')[1].Trim()));
            }
            if (!string.IsNullOrEmpty(this.ddlHoraEgreso.SelectedValue))
            {
                pReserva.FechaEgreso = pReserva.FechaEgreso.Value.AddHours(Convert.ToInt32(this.ddlHoraEgreso.SelectedValue.Split(':')[0].Trim()));
                pReserva.FechaEgreso = pReserva.FechaEgreso.Value.AddMinutes(Convert.ToInt32(this.ddlHoraEgreso.SelectedValue.Split(':')[1].Trim()));
            }
            pReserva.CantidadPersonas = this.txtCantidadPersonasNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadPersonasNumero.Text);
            if (!string.IsNullOrEmpty(this.hdfReservaIdAfiliado.Value))
            {
                Int64 id = 0;
                if (Int64.TryParse(this.hdfReservaIdAfiliado.Value, out id))
                    pReserva.IdAfiliado = id;
                else
                    pReserva.IdAfiliado = default(Int64?);
            }
            pReserva.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pReserva.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pReserva.Apellido = string.IsNullOrEmpty(this.hdfReservaApellido.Value) ? default(string) : this.hdfReservaApellido.Value;
            pReserva.Nombre = this.txtReservaNombre.Text;
            pReserva.IdTipoDocumento = Convert.ToInt32(this.ddlReservaTipoDocumento.SelectedValue);
            pReserva.NumeroDocumento = this.txtReservaNumeroDocumento.Text;
            pReserva.CorreoElectronico= this.txtReservaCorreoElectronico.Text;
            pReserva.IdAfiliadoTipo = string.IsNullOrEmpty(this.hdfReservaIdAfiliadoTipo.Value) ? default(int?) : Convert.ToInt32(this.hdfReservaIdAfiliadoTipo.Value);
            pReserva.Archivos = this.ctrArchivos.ObtenerLista();
            pReserva.Comentarios = this.ctrComentarios.ObtenerLista();
            pReserva.Campos = this.ctrCamposValores.ObtenerLista();
        }

        protected void btnAceptarContinuar_Click(object sender, EventArgs e)
        {
            this.AceptarContinuar = true;
            HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
            parametros.HashTransaccion = this.tcDatos.ActiveTabIndex;
            this.BusquedaParametrosGuardarValor<HTLReservas>(parametros);
            this.btnAceptar_Click(sender, e);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.btnAceptarContinuar.Visible = false;
            this.MapearControlesAObjeto(this.MiReserva);
            this.PersistirDatosGrilla();
            this.PersistirDatosGrillaOcupantes();
            this.PersistirDescuentosGrilla();

            

            foreach (HTLReservasDetalles detalle in this.MiReserva.ReservasDetalles)
            {
                detalle.ReservasDetallesDescuentos = new List<HTLReservasDetallesDescuentos>();
                detalle.ReservasDetallesDescuentos.AddRange(this.MisReservasDetallesDescuentos.FindAll(y => y.IdHabitacion == detalle.IdHabitacion));
            }
           
            DateTime FechaIngreso = Convert.ToDateTime(MiReserva.FechaIngreso);
            FechaIngreso.ToShortDateString();
           

            this.MiReserva.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiReserva.CargarLoteCamposValores();
            this.MiReserva.CargarLoteCamposValoresOcupantes();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = HotelesF.ReservasAgregar(this.MiReserva);
                    if (guardo)
                    {
                        this.btnImprimir.Visible = true;
                        this.MostrarMensaje(this.MiReserva.CodigoMensaje, false, this.MiReserva.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Anular:
                    this.MiReserva.Estado.IdEstado = (int)Estados.Baja;
                    guardo = HotelesF.ReservasModificar(this.MiReserva);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiReserva.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = HotelesF.ReservasModificar(this.MiReserva);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiReserva.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.btnAceptarContinuar.Visible = true;
                this.MostrarMensaje(this.MiReserva.CodigoMensaje, true, this.MiReserva.CodigoMensajeArgs);
                if (this.MiReserva.dsResultado != null)
                {
                    //this.ctrPopUpGrilla.IniciarControl(this.MiReserva);
                    this.MiReserva.dsResultado = null;
                }
            }
            else
            {
                if (this.AceptarContinuar)
                {
                    this.btnAceptar.Visible = true;
                    this.btnAceptarContinuar.Visible = true;
                    this.ddlReservaApellido.Items.Clear();
                    this.ddlReservaApellido.SelectedIndex = -1;
                    this.IniciarControl(this.MiReserva, Gestion.Modificar);
                    this.upReservaDatos.Update();
                    this.upDetalleGastos.Update();
                    this.upOcupantes.Update();
                    this.upDescuentos.Update();
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DeshabilitarControlesScript", "$(document).ready(function () { deshabilitarControles('deshabilitarControles') }); ;", true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl.Remove("IdReserva");
            HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
            parametros = new HTLReservas();
            this.BusquedaParametrosGuardarValor<HTLReservas>(parametros);

            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "HotelesFormularioAlojamiento";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            if (plantilla.HtmlPlantilla.Trim().Length > 0)
            {
                //ExportPDF.ConvertirHtmlEnPdf(this.upBotones, plantilla, this.MiReserva, this.UsuarioActivo);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, plantilla.Codigo, MiReserva, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.Page, "HotelesFormularioAlojamiento", this.UsuarioActivo);
            }
            else
            {
                this.MostrarMensaje("ValidarPlantillaDatos", true, new List<string>() { plantilla.Codigo });
            }


       
        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            //MailMessage mail = new MailMessage();
            //if (FacturasF.FacturaArmarMail(this.MiFactura, mail))
            //{
            //    this.popUpMail.IniciarControl(mail, this.MiFactura);
            //}
        }

        #region CARGA MASIVA
        protected void btnCargaMasivaMostrar_Click(object sender, EventArgs e)
        {
            this.upCargaMasiva.Visible = !this.upCargaMasiva.Visible;
            this.txtCargaMasivaCantidad.Text = string.Empty;
            this.upCargaMasiva.Update();
        }

        protected void btnCargaMasivaAgregar_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            HTLReservasDetalles filtro = new HTLReservasDetalles();
            filtro.IdHotel = this.ddlHoteles.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlHoteles.SelectedValue);
            filtro.IdTipoProductoHotel = string.IsNullOrEmpty( this.hdfCodigoTPH01.Value) ? default(int?) : Convert.ToInt32(this.hdfCodigoTPH01.Value);
            filtro.IdProducto = this.ddlCargaMasivaProductos.SelectedValue == string.Empty ? default(Int64?) : Convert.ToInt64(this.ddlCargaMasivaProductos.SelectedValue);
            filtro.FechaIngreso = this.txtReservaFechaIngreso.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtReservaFechaIngreso.Text);
            filtro.FechaEgreso = this.txtReservaFechaEgreso.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtReservaFechaEgreso.Text);
            
            if (!filtro.FechaIngreso.HasValue)
            {
                this.MostrarMensaje("ValidarReservaFechaIngreso", true);
                return;
            }
            if (!filtro.FechaEgreso.HasValue)
            {
                this.MostrarMensaje("ValidarReservaFechaEgreso", true);
                return;
            }

            string key = this.Request.Form.AllKeys.First(x => x.Contains("$ddlListasPrecios"));
            if (!string.IsNullOrEmpty(key))
                filtro.IdListaPrecio = this.Request.Form[key] == string.Empty ? 0 : Convert.ToInt32(this.Request.Form[key]);
            this.MiReserva.ReservasDetalles = this.MiReserva.ReservasDetalles.Where(x => x.IdTipoProductoHotel.HasValue).ToList();
            List<HTLReservasDetalles> lista = HotelesF.ReservasDetallesObtenerAjaxComboDetalleGastos<HTLReservasDetalles>(filtro, this.MiReserva.ReservasDetalles);
            lista = lista.Take(Convert.ToInt32( this.txtCargaMasivaCantidad.Decimal)).ToList();
            foreach (HTLReservasDetalles item in lista)
            {
                item.IdTipoProductoHotel = filtro.IdTipoProductoHotel;
                item.FechaIngreso = filtro.FechaIngreso;
                item.FechaEgreso = filtro.FechaEgreso;
                item.Cantidad = (item.FechaEgreso.Value - item.FechaIngreso.Value).Days;
                item.Estado.IdEstado = (int)Estados.Activo;
                item.EstadoColeccion = EstadoColecciones.Agregado;
                MiReserva.ReservasDetalles.Add(item);
                item.IndiceColeccion = MiReserva.ReservasDetalles.IndexOf(item);
                item.IdReservaDetalle = item.IndiceColeccion * -1;
                //item.IdReservaDetalle = -999;
            }
            //this.MiReserva.ReservasDetalles = AyudaProgramacion.AcomodarIndices<HTLReservasDetalles>(this.MiReserva.ReservasDetalles);
            //this.MiReserva.ReservasDetalles.AddRange(lista);
            //this.MiReserva.ReservasDetalles.Where(x=>x.IdReservaDetalle== ).ToList().ForEach(x => x.IdReservaDetalle = x.IndiceColeccion * -1 );
            this.AgregarItem(1);
            AyudaProgramacion.CargarGrillaListas<HTLReservasDetalles>(this.MiReserva.ReservasDetalles, true, this.gvDatos, true);
            ScriptManager.RegisterStartupScript(this.upDetalleGastos, this.upDetalleGastos.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
            this.upDetalleGastos.Update();
        }

        #endregion

        #region Reserva Detalle
        private void AgregarItem(int cantidad)
        {
            HTLReservasDetalles item;
            for (int i = 0; i < cantidad; i++)
            {
                item = new HTLReservasDetalles();
                item.Estado.IdEstado = (int)Estados.Activo;
                item.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiReserva.ReservasDetalles.Add(item);
                item.IndiceColeccion = this.MiReserva.ReservasDetalles.IndexOf(item);
                item.IdReservaDetalle = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<HTLReservasDetalles>(this.MiReserva.ReservasDetalles, true, this.gvDatos, true);
            ScriptManager.RegisterStartupScript(this.upDetalleGastos, this.upDetalleGastos.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
        }

        private void PersistirDatosGrilla()
        {
            if (this.MiReserva.ReservasDetalles.Count == 0)
                return;

            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("gvDatos")).ToList();
            string k;
            int numeroFila = 2;
            HTLReservasDetalles det;
            bool modifica;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    modifica = false;
                    DropDownList ddlTiposProductosHoteles = ((DropDownList)fila.FindControl("ddlTiposProductosHoteles"));
                    det = this.MiReserva.ReservasDetalles.Find(x => x.IdReservaDetalle == Convert.ToInt64( this.gvDatos.DataKeys[fila.RowIndex]["IdReservaDetalle"].ToString()));
                    det.IdTipoProductoHotel = ddlTiposProductosHoteles.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(ddlTiposProductosHoteles.SelectedValue);

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtFechaIngreso"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.FechaIngreso.HasValue && det.FechaIngreso.Value.Date != Convert.ToDateTime(this.Request.Form[k]).Date)
                            modifica = true;
                        det.FechaIngreso = Convert.ToDateTime(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtFechaEgreso"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.FechaEgreso.HasValue && det.FechaEgreso.Value.Date != Convert.ToDateTime(this.Request.Form[k]).Date)
                            modifica = true;
                        det.FechaEgreso = Convert.ToDateTime(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$ddlDetalleGastos"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.IdProducto.HasValue && det.IdProducto.Value != Convert.ToInt64(this.Request.Form[k]))
                            modifica = true;
                        det.IdProducto = Convert.ToInt64(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfDetalleGastos"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Detalle != this.Request.Form[k])
                            modifica = true;
                        det.Detalle = this.Request.Form[k];
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfIdHabitacion"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        //if (det.IdHabitacion.HasValue && det.IdHabitacion.Value != Convert.ToInt32(this.Request.Form[k]))
                        //    modifica = true;
                        det.IdHabitacion = this.Request.Form[k]=="0"? default(long?) : Convert.ToInt32(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtCantidad"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Cantidad != Convert.ToDecimal(this.Request.Form[k]))
                            modifica = true;
                        det.Cantidad = Convert.ToDecimal(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$chkCompartida"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        bool compartida = this.Request.Form[k] == string.Empty ? false : true;
                        if (det.Compartida != compartida)
                            modifica = true;
                        det.Compartida = compartida;
                    }
                    else if (det.Compartida)
                    {
                        modifica = true;
                        det.Compartida = false;
                     
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$ddlMoviliario"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.HabitacionDetalle.IdHabitacionDetalle != Convert.ToInt64(this.Request.Form[k]))
                            modifica = true;
                        det.HabitacionDetalle.IdHabitacionDetalle = Convert.ToInt64(this.Request.Form[k]);
                    }
                    else if (det.HabitacionDetalle.IdHabitacionDetalle.HasValue)
                    {
                        modifica = true;
                        det.HabitacionDetalle.IdHabitacionDetalle = default(int?);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfIdListaPrecioDetalle"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.IdListaPrecioDetalle.HasValue && det.IdListaPrecioDetalle.Value != Convert.ToInt64(this.Request.Form[k]))
                            modifica = true;
                        det.IdListaPrecioDetalle = Convert.ToInt64(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfPrecioEditable"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.PrecioEditable != Convert.ToBoolean(this.Request.Form[k]))
                            modifica = true;
                        det.PrecioEditable = Convert.ToBoolean(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtPrecio"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Precio != decimal.Parse(this.Request.Form[k], NumberStyles.Currency ))
                            modifica = true;
                        det.Precio = decimal.Parse(this.Request.Form[k], NumberStyles.Currency);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtDescuentoPorcentual"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.DescuentoPorcentaje != decimal.Parse(this.Request.Form[k], NumberStyles.Number))
                            modifica = true;
                        det.DescuentoPorcentaje = decimal.Parse(this.Request.Form[k], NumberStyles.Number);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfDescuentoImporte"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        modifica = true;
                        det.DescuentoImporte = Convert.ToDecimal(this.Request.Form[k].Replace(".", ","));
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfSubTotal"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        det.SubTotal = Convert.ToDecimal(this.Request.Form[k].Replace(".", ","));
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfCantidadPersonas"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.CantidadPersonas != Convert.ToInt32(this.Request.Form[k].Replace(".", ",")))
                            modifica = true;
                        det.CantidadPersonas = Convert.ToInt32(this.Request.Form[k].Replace(".", ","));
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfCantidadPersonasOpciones"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.CantidadPersonasOpciones.HasValue
                            && det.CantidadPersonasOpciones.Value != Convert.ToInt32(this.Request.Form[k].Replace(".", ","))
                            || !det.CantidadPersonasOpciones.HasValue && Convert.ToInt32(this.Request.Form[k].Replace(".", ",")) > 0)
                        {
                            modifica = true;
                            det.CantidadPersonasOpciones = Convert.ToInt32(this.Request.Form[k].Replace(".", ","));
                        }
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfMoviliario"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.HabitacionDetalle.IdHabitacionDetalle.HasValue
                            && det.HabitacionDetalle.IdHabitacionDetalle.Value != Convert.ToInt32(this.Request.Form[k].Replace(".", ","))
                            || !det.HabitacionDetalle.IdHabitacionDetalle.HasValue && Convert.ToInt32(this.Request.Form[k].Replace(".", ",")) > 0)
                        {
                            modifica = true;
                            det.HabitacionDetalle.IdHabitacionDetalle = Convert.ToInt32(this.Request.Form[k].Replace(".", ","));
                        }
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfLateCheckOut"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.LateCheckOut.HasValue && det.LateCheckOut.Value != Convert.ToBoolean(this.Request.Form[k])
                            || !det.LateCheckOut.HasValue && Convert.ToBoolean(this.Request.Form[k]))
                        {
                            modifica = true;
                            det.LateCheckOut = Convert.ToBoolean(this.Request.Form[k]);
                        }
                    }

                    if (modifica)
                        det.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(det, this.GestionControl);
                    numeroFila++;
                }
            }
            this.MiReserva.PrecioTotal = this.MiReserva.ReservasDetalles.Sum(x => x.SubTotal);
        }

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            this.AgregarItem(1);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HTLReservasDetalles item = (HTLReservasDetalles)e.Row.DataItem;
                ImageButton Agregar = ((ImageButton)e.Row.FindControl("btnModificar"));
                DropDownList ddlTiposProductosHoteles = ((DropDownList)e.Row.FindControl("ddlTiposProductosHoteles"));
                ddlTiposProductosHoteles.DataSource = this.MisTiposProductosHoteles;
                ddlTiposProductosHoteles.DataValueField = "IdListaValorDetalle";
                ddlTiposProductosHoteles.DataTextField = "Descripcion";
                ddlTiposProductosHoteles.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(ddlTiposProductosHoteles, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                if (item.IdTipoProductoHotel.HasValue && item.IdTipoProductoHotel.Value > 0)
                    ddlTiposProductosHoteles.SelectedValue = item.IdTipoProductoHotel.Value.ToString();

                DropDownList ddlDetalleGastos = ((DropDownList)e.Row.FindControl("ddlDetalleGastos"));
                if (item.IdProducto.HasValue && item.IdProducto.Value > 0)
                    ddlDetalleGastos.Items.Add(new ListItem(item.Detalle, item.IdProducto.ToString()));

          
                //Agregar.Attributes.Add("onclick", "Opciones(this);");

                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                string mensaje = this.ObtenerMensajeSistema("ValidarReservasItemEliminar");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnEliminar.Attributes.Add("OnClick", funcion);

                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Modificar:
                        btnEliminar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            IdReservadetalleIndice = this.MiReserva.ReservasDetalles.Find(x => x.IdReservaDetalle == Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString())).IndiceColeccion ;
           
            this.PersistirDatosGrilla();
            HTLReservasDetalles item = this.MiReserva.ReservasDetalles[IdReservadetalleIndice];

            if (e.CommandName == "Borrar")
            {                
                item.Estado.IdEstado = (int)Estados.Baja;
                item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(item, Gestion.Anular);
                AyudaProgramacion.CargarGrillaListas<HTLReservasDetalles>(this.MiReserva.ReservasDetalles, true, this.gvDatos, true);
                ScriptManager.RegisterStartupScript(this.upDetalleGastos, this.upDetalleGastos.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
                this.upDetalleGastos.Update();
            }
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                AyudaProgramacion.CargarGrillaListas<HTLReservasDetalles>(this.MiReserva.ReservasDetalles, true, this.gvDatos, true);
                ctrReservasDetallesOpciones.IniciarControl(item, GestionControl);
            }
        }

        private void CtrReservasDetallesOpciones_ReservasDetallesOpcioneAceptar(HTLReservasDetalles e)
        {
            this.MiReserva.ReservasDetalles[IdReservadetalleIndice] = e;
            string key = this.Request.Form.AllKeys.First(x => x.Contains("$ddlListasPrecios"));
            if (!string.IsNullOrEmpty(key))
                this.MiReserva.ReservasDetalles[IdReservadetalleIndice].IdListaPrecio = this.Request.Form[key] == string.Empty ? 0 : Convert.ToInt32(this.Request.Form[key]);
            this.MiReserva.ReservasDetalles[IdReservadetalleIndice].CantidadPersonasOpciones = e.CantidadPersonasOpciones.HasValue ? e.CantidadPersonasOpciones.Value : default(int?);
            this.MiReserva.ReservasDetalles[IdReservadetalleIndice].Precio = HotelesF.ReservasDetallesObtenerPrecio(this.MiReserva.ReservasDetalles[IdReservadetalleIndice]);
            AyudaProgramacion.CargarGrillaListas<HTLReservasDetalles>(this.MiReserva.ReservasDetalles, true, this.gvDatos, true);
            ScriptManager.RegisterStartupScript(this.upDetalleGastos, this.upDetalleGastos.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
            this.upDetalleGastos.Update();
        }

        #endregion

        #region Descuentos
        protected void btnAgregarDescuento_Click(object sender, EventArgs e)
        {
            HTLDescuentosFiltros filtro = new HTLDescuentosFiltros();
            filtro.IdHotel = this.ddlHoteles.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlHoteles.SelectedValue);
            filtro.IdAfiliado = !string.IsNullOrEmpty(this.hdfReservaIdAfiliado.Value) ? Convert.ToInt32(this.hdfReservaIdAfiliado.Value) : 0;
            filtro.IdAfiliadoTipo = !string.IsNullOrEmpty(this.hdfReservaIdAfiliadoTipo.Value) ? Convert.ToInt32(this.hdfReservaIdAfiliadoTipo.Value) : 0;
            filtro.FechaIngreso = this.txtReservaFechaIngreso.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(this.txtReservaFechaIngreso.Text);
            filtro.IdReserva = this.MiReserva.IdReserva;
            this.MisDescuentos = HotelesF.DescuentosObtenerPorReserva(filtro);

            this.PersistirDatosGrilla();
            this.PersistirDescuentosGrilla();
            this.AgregarDescuentos(1);
        }

        private void AgregarDescuentos(int cantidad)
        {
            HTLReservasDetallesDescuentos item;
            for (int i = 0; i < cantidad; i++)
            {
                item = new HTLReservasDetallesDescuentos();
                item.Estado.IdEstado = (int)Estados.Activo;
                item.EstadoColeccion = EstadoColecciones.Agregado;
                this.MisReservasDetallesDescuentos.Add(item);
                item.IndiceColeccion = this.MisReservasDetallesDescuentos.IndexOf(item);
                item.IdReservaDetalleDescuento = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<HTLReservasDetallesDescuentos>(this.MisReservasDetallesDescuentos, true, this.gvDescuentos, true);
            ScriptManager.RegisterStartupScript(this.upDescuentos, this.upDescuentos.GetType(), "InitGrillaDescuentoScript", "InitGrillaDescuento();", true);
        }

        private void PersistirDescuentosGrilla()
        {
            if (this.MisReservasDetallesDescuentos.Count == 0)
                return;

            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("gvDescuentos")).ToList();
            string k;
            int numeroFila = 2;
            HTLReservasDetallesDescuentos det;
            bool modifica;
            foreach (GridViewRow fila in this.gvDescuentos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    modifica = false;
                    det = this.MisReservasDetallesDescuentos.Find(x => x.IdReservaDetalleDescuento == Convert.ToInt64(this.gvDescuentos.DataKeys[fila.RowIndex]["IdReservaDetalleDescuento"].ToString()));
                    DropDownList ddlHabitaciones = ((DropDownList)fila.FindControl("ddlHabitaciones"));
                    det.IdHabitacion = ddlHabitaciones.SelectedValue == string.Empty ? default(int) : Convert.ToInt64(ddlHabitaciones.SelectedValue);
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_IdDescuento"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.IdDescuento != Convert.ToInt32(this.Request.Form[k]))
                            modifica = true;
                        det.IdDescuento = Convert.ToInt32(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_IdTipoDescuento"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.TipoDescuento.IdTipoDescuento != Convert.ToInt32(this.Request.Form[k]))
                            modifica = true;
                        det.TipoDescuento.IdTipoDescuento = Convert.ToInt32(this.Request.Form[k]);
                        k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_TipoDescuentoDescripcion"));
                        det.TipoDescuento.Descripcion = this.Request.Form[k] == null ? string.Empty : this.Request.Form[k] ;
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_PorcentajeImporte"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.DescuentoPorcentaje != Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.',',')))
                            modifica = true;
                        det.DescuentoPorcentaje = Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ','));
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_DescuentoImporte"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.DescuentoImporte != Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ',')))
                            modifica = true;
                        det.DescuentoImporte = Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ','));
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_PrecioBase"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.PrecioBase != Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ',')))
                            modifica = true;
                        det.PrecioBase = Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ','));
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_BaseCalculoImporte"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.BaseCalculoImporte != Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ',')))
                            modifica = true;
                        det.BaseCalculoImporte = Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ','));
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_Cantidad"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Cantidad != Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ',')))
                            modifica = true;
                        det.Cantidad = Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ','));
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdf_gvd_lblSubTotal"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.SubTotal != Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ',')))
                            modifica = true;
                        det.SubTotal = Convert.ToDecimal(this.Request.Form[k].ToString().Replace('.', ','));
                    }
                    if (modifica)
                        det.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(det, this.GestionControl);
                    numeroFila++;
                }
            }
            this.MiReserva.PrecioTotal = this.MiReserva.ReservasDetalles.Sum(x => x.SubTotal);
        }

        protected void gvDescuentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HTLReservasDetallesDescuentos item = (HTLReservasDetallesDescuentos)e.Row.DataItem;
                DropDownList ddlHabitaciones = ((DropDownList)e.Row.FindControl("ddlHabitaciones"));
                ddlHabitaciones.DataSource = this.MiReserva.ReservasDetalles.Where(x=>x.IdHabitacion.HasValue);
                ddlHabitaciones.DataValueField = "IdHabitacion";
                ddlHabitaciones.DataTextField = "Detalle";
                ddlHabitaciones.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(ddlHabitaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                ddlHabitaciones.Enabled = item.IdReservaDetalleDescuento <= 0;
                ListItem lstItem;
                lstItem = ddlHabitaciones.Items.FindByValue(item.IdHabitacion.ToString());
                if (lstItem == null && item.IdHabitacion > 0)
                {
                    ddlHabitaciones.Items.Add(new ListItem(item.Detalle, item.IdHabitacion.ToString()));
                }
                if (lstItem != null )
                    ddlHabitaciones.SelectedValue = item.IdHabitacion.ToString();

                DropDownList ddlTiposDescuentos = ((DropDownList)e.Row.FindControl("ddlTiposDescuentos"));
                ddlTiposDescuentos.DataSource = this.MisDescuentos;
                ddlTiposDescuentos.DataValueField = "IdDescuento";
                ddlTiposDescuentos.DataTextField = "Descripcion";
                ddlTiposDescuentos.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(ddlTiposDescuentos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                ddlTiposDescuentos.Enabled = item.IdReservaDetalleDescuento <= 0;
                lstItem = ddlTiposDescuentos.Items.FindByValue(item.IdDescuento.ToString());
                if (lstItem == null && item.IdDescuento > 0)
                {
                    ddlTiposDescuentos.Items.Add(new ListItem(item.TipoDescuento.Descripcion, item.IdDescuento.ToString()));
                    ddlTiposDescuentos.SelectedValue = item.IdDescuento.ToString();
                }
                else if (lstItem != null)
                    ddlTiposDescuentos.SelectedValue = item.IdDescuento.ToString();

                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                string mensaje = this.ObtenerMensajeSistema("ValidarReservasDecuentosEliminar");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnEliminar.Attributes.Add("OnClick", funcion);

                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Modificar:
                        btnEliminar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void gvDescuentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int idReservaDetalleDescuento = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                this.PersistirDescuentosGrilla();
                HTLReservasDetallesDescuentos item = this.MisReservasDetallesDescuentos.Find(x => x.IdReservaDetalleDescuento == idReservaDetalleDescuento);
                item.Estado.IdEstado = (int)Estados.Baja;
                item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(item, Gestion.Anular);
                AyudaProgramacion.CargarGrillaListas<HTLReservasDetallesDescuentos>(this.MisReservasDetallesDescuentos, true, this.gvDescuentos, true);
                ScriptManager.RegisterStartupScript(this.upDescuentos, this.upDescuentos.GetType(), "InitGrillaDescuentoScript", "InitGrillaDescuento();", true);
                ScriptManager.RegisterStartupScript(this.upDescuentos, this.upDescuentos.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
                this.upDetalleGastos.Update();
            }
        }
        #endregion

        #region Acompaniantes
        private void PersistirDatosGrillaOcupantes()
        {
            if (this.MiReserva.ReservasOcupantes.Count == 0)
                return;

            List<string> keys = this.Request.Form.AllKeys.Where(x => x.Contains("gvOcupante")).ToList();
            string k;
            int numeroFila = 2;
            HTLReservasOcupantes det;
            bool modifica;
            foreach (GridViewRow fila in this.gvOcupante.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    modifica = false;
                    det = this.MiReserva.ReservasOcupantes[fila.RowIndex];

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfIdAfiliado"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.IdAfiliado.HasValue && det.IdAfiliado.Value != Convert.ToInt64(this.Request.Form[k]))
                            modifica = true;
                        det.IdAfiliado = Convert.ToInt64(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfApellido"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Apellido != this.Request.Form[k])
                            modifica = true;
                        det.Apellido = this.Request.Form[k];
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtNombre"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Nombre != this.Request.Form[k])
                            modifica = true;
                        det.Nombre = this.Request.Form[k];
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$ddlTipoDocumento"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.IdTipoDocumento.HasValue && det.IdTipoDocumento.Value != Convert.ToInt64(this.Request.Form[k]))
                            modifica = true;
                        det.IdTipoDocumento = Convert.ToInt32(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtNumeroDocumento"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.NumeroDocumento != this.Request.Form[k])
                            modifica = true;
                        det.NumeroDocumento = this.Request.Form[k];
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtEdad"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.EdadFechaSalida.HasValue && det.EdadFechaSalida.Value != int.Parse( this.Request.Form[k], NumberStyles.Currency))
                            modifica = true;
                        det.EdadFechaSalida = int.Parse(this.Request.Form[k], NumberStyles.Currency);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfIdHabitacionSeleccionada"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.IdHabitacion.HasValue && det.IdHabitacion.Value != Convert.ToInt32(this.Request.Form[k]))
                            modifica = true;
                        det.IdHabitacion = Convert.ToInt32(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfDetalleHabitacion"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.DetalleHabitacion != this.Request.Form[k])
                            modifica = true;
                        det.DetalleHabitacion = this.Request.Form[k];
                    }
                    if (modifica)
                        det.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(det, this.GestionControl);
                    numeroFila++;
                }
            }
        }

        private void AgregarOcupantes(int cantidad)
        {
            HTLReservasOcupantes item;
            for (int i = 0; i < 2; i++)
            {
                item = new HTLReservasOcupantes();
                item.Estado.IdEstado = (int)Estados.Activo;
                item.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiReserva.ReservasOcupantes.Add(item);
                item.IndiceColeccion = this.MiReserva.ReservasOcupantes.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<HTLReservasOcupantes>(this.MiReserva.ReservasOcupantes, true, this.gvOcupante, true);
        }

        protected void btnAgregarOcupante_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrillaOcupantes();
            this.AgregarOcupantes(1);
        }

        protected void gvOcupante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HTLReservasOcupantes item = (HTLReservasOcupantes)e.Row.DataItem;

                DropDownList ddlApellido = ((DropDownList)e.Row.FindControl("ddlApellido"));
                if (!string.IsNullOrEmpty(item.Apellido))
                {
                    if (item.IdAfiliado.HasValue)
                        ddlApellido.Items.Add(new ListItem(item.Apellido, item.IdAfiliado.ToString()));
                    else
                        ddlApellido.Items.Add(new ListItem(item.Apellido, item.Apellido));
                }
                DropDownList ddlTipoDocumento = ((DropDownList)e.Row.FindControl("ddlTipoDocumento"));
                ddlTipoDocumento.DataSource = this.MisTiposDocumentos;
                ddlTipoDocumento.DataValueField = "IdTipoDocumento";
                ddlTipoDocumento.DataTextField = "TipoDocumento";
                ddlTipoDocumento.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(ddlTipoDocumento, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                if (item.IdTipoDocumento.HasValue && item.IdTipoDocumento.Value > 0)
                    ddlTipoDocumento.SelectedValue = item.IdTipoDocumento.Value.ToString();

                DropDownList ddlHabitacionesSeleccionadas = ((DropDownList)e.Row.FindControl("ddlHabitacionesSeleccionadas"));
                if (item.IdHabitacion.HasValue && item.IdHabitacion.Value > 0)
                    ddlHabitacionesSeleccionadas.Items.Add(new ListItem(item.DetalleHabitacion, item.IdHabitacion.ToString()));

                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Modificar:
                        btnEliminar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void gvOcupante_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                this.PersistirDatosGrillaOcupantes();
                this.MiReserva.ReservasOcupantes[indiceColeccion].Estado.IdEstado = (int)Estados.Baja;
                this.MiReserva.ReservasOcupantes[indiceColeccion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiReserva.ReservasOcupantes[indiceColeccion], Gestion.Anular);
                AyudaProgramacion.CargarGrillaListas<HTLReservasOcupantes>(this.MiReserva.ReservasOcupantes, true, this.gvOcupante, true);
                this.upOcupantes.Update();
            }
        }

        #endregion

        #region Facturacion Cobro

        protected void btnAgregarComprobante_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiReserva.IdAfiliado);
            this.MisParametrosUrl.Add("IdReserva", this.MiReserva.IdReserva);
            this.MisParametrosUrl.Add("FechaIngreso", this.MiReserva.FechaIngreso);
            this.MisParametrosUrl.Add("FechaEgreso", this.MiReserva.FechaEgreso);
            HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
            parametros.BusquedaParametros = true;
            parametros.HashTransaccion = this.tcDatos.ActiveTabIndex;
            this.BusquedaParametrosGuardarValor<HTLReservas>(parametros);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAgregar.aspx"), true);
        }

        protected void btnAgregarOC_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiReserva.IdAfiliado);
            this.MisParametrosUrl.Add("IdReserva", this.MiReserva.IdReserva);
            HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
            parametros.BusquedaParametros = true;
            parametros.HashTransaccion = this.tcDatos.ActiveTabIndex;
            this.BusquedaParametrosGuardarValor<HTLReservas>(parametros);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAgregar.aspx"), true);
        }

        protected void gvCuentaCorriente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
            parametros.BusquedaParametros = true;
            parametros.HashTransaccion = this.tcDatos.ActiveTabIndex;
            this.BusquedaParametrosGuardarValor<HTLReservas>(parametros);

            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvCuentaCorriente.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvCuentaCorriente.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)
            if (e.CommandName == Gestion.Consultar.ToString())
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

            //Guardo Menu devuelto de la DB
            filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiReserva.IdAfiliado);
            this.MisParametrosUrl.Add("IdReserva", this.MiReserva.IdReserva);
            //Si devuelve una URL Redirecciona si no muestra mensaje error
            if (filtroMenu.URL.Length != 0)
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
            }
            else
            {
                this.MiReserva.CodigoMensaje = "ErrorURLNoValida";
                this.MostrarMensaje(this.MiReserva.CodigoMensaje, true);
            }

        }

        protected void gvCuentaCorriente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //consultar.Visible = this.ValidarPermiso("LibroMayorListar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        #endregion
    }
}