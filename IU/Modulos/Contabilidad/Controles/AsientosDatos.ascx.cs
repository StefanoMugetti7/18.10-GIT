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
using Generales.Entidades;
using Evol.Controls;
using System.Globalization;
using System.Collections;
using Seguridad.FachadaNegocio;
using Afiliados;
using System.Data;
using OfficeOpenXml;
using System.IO;
using Afiliados.Entidades;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class AsientosDatos : ControlesSeguros
    {
        //bool noactualizarGrilla = true;
        private bool noactualizarGrilla
        {
            get { return (bool)Session[this.MiSessionPagina + "noactualizarGrilla"]; }
            set { Session[this.MiSessionPagina + "noactualizarGrilla"] = value; }
        }
        private CtbAsientosContables MiAsientoContable
        {
            get { return (CtbAsientosContables)Session[this.MiSessionPagina + "MiAsientoContable"]; }
            set { Session[this.MiSessionPagina + "MiAsientoContable"] = value; }
        }

        private List<CtbEjerciciosContables> MisEjerciciosContables
        {
            get { return (List<CtbEjerciciosContables>)Session[this.MiSessionPagina + "AsientosDatosMiEjercicioContable"]; }
            set { Session[this.MiSessionPagina + "AsientosDatosMiEjercicioContable"] = value; }
        }

        private int MiPosicion
        {
            get { return (int)Session[this.MiSessionPagina + "MiPosicion"]; }
            set { Session[this.MiSessionPagina + "MiPosicion"] = value; }
        }

        private List<CtbCentrosCostosProrrateos> MisCentrosCostos
        {
            get { return (List<CtbCentrosCostosProrrateos>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisCentrosCostos"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisCentrosCostos"] = value; }
        }

        private int AjusteInflacionPeriodo
        {
            get { return (int)Session[this.MiSessionPagina + "AjusteInflacionPeriodo"]; }
            set { Session[this.MiSessionPagina + "AjusteInflacionPeriodo"] = value; }
        }
        public delegate void AsientoContableDatosAceptarEventHandler(object sender, CtbAsientosContables e);
        public event AsientoContableDatosAceptarEventHandler AsientoContableDatosAceptar;

        public delegate void AsientoContableDatosCancelarEventHandler();
        public event AsientoContableDatosCancelarEventHandler AsientoContableDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.puAsientosModelos.AsientosModelosBuscarSeleccionarPopUp += new AsientosModelosBuscarPopUp.AsientosModelosBuscarPopUpEventHandler(puAsientosModelos_AsientosModelosBuscarSeleccionarPopUp);
          //  this.puCuentasContables.CuentasContablesBuscarSeleccionarPopUp += PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp;
            if (!this.IsPostBack)
            {
                if (this.MiAsientoContable == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Cuenta Modelo
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbAsientosContables pAsientoContable, Gestion pGestion)
        {
            noactualizarGrilla = false;
            this.GestionControl = pGestion;
            CtbCentrosCostosProrrateos ccp = new CtbCentrosCostosProrrateos();
            ccp.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
            this.MisCentrosCostos = ContabilidadF.CentrosCostosProrrateosObtenerCombo(ccp);
            this.CargarCombos();
            CtbEjerciciosContables ejercicio = this.MisEjerciciosContables[this.ddlEjercicioContable.SelectedIndex];
            double desde = ejercicio.FechaInicio.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            double hasta = ejercicio.FechaFin.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string script = string.Format("InitControlFecha('{0}', '{1}');", desde, hasta);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "InitControlFechaScript", script, true);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAsientoContable = pAsientoContable;
                    this.MiAsientoContable.IdTipoOperacion = (int)EnumTGETiposOperaciones.AsientoContableManual;

                    this.MiAsientoContable.FechaAsiento = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    if (this.MiAsientoContable.FechaAsiento.Date > ejercicio.FechaFin.Date)
                        this.MiAsientoContable.FechaAsiento = ejercicio.FechaFin.Date;
                    else if (this.MiAsientoContable.FechaAsiento.Date < ejercicio.FechaInicio.Date)
                        this.MiAsientoContable.FechaAsiento = ejercicio.FechaInicio.Date;
                    
                    this.AgregarCuentas(2);
                    this.MiAsientoContable.FechaRealizado = this.MiAsientoContable.FechaAsiento;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.txtFechaAsiento.Text = this.MiAsientoContable.FechaAsiento.ToShortDateString();                    
                    this.txtNumeroAsiento.Text = this.MiAsientoContable.NumeroAsiento;
                    AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
                    this.MapearObjetoAGrilla();
                    this.lblTipoOperacion.Visible = false;
                    this.txtTipoOperacion.Visible = false;
                    this.lblRefTipoOperacion.Visible = false;
                    this.txtRefTipoOperacion.Visible = false;

                    this.txtFechaAsiento.Enabled = true;
                    this.ddlTiposAsientos.Enabled = true;
                    this.ddlFiliales.Enabled = true;
                    //this.ctrComentarios.IniciarControl(pAsientoContable, pGestion);
                    //this.ctrArchivos.IniciarControl(pAsientoContable, pGestion);
                    this.ctrAuditoria.IniciarControl(pAsientoContable);
                    break;
                case Gestion.Copiar:
                    this.MiAsientoContable = ContabilidadF.AsientosContablesObtenerDatosCompletos(pAsientoContable);
                    TGETiposOperaciones tipoOP = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.AsientoContableManual);
                    this.MiAsientoContable.IdTipoOperacion = tipoOP.IdTipoOperacion;
                    this.MiAsientoContable.TipoOperacion = tipoOP.TipoOperacion;
                    this.MiAsientoContable.Filial = this.UsuarioActivo.FilialPredeterminada;
                    this.MiAsientoContable.NumeroAsiento = string.Empty;
                    
                    foreach (CtbAsientosContablesDetalles det in this.MiAsientoContable.AsientosContablesDetalles)
                        det.EstadoColeccion = EstadoColecciones.Agregado;

                    this.MiAsientoContable.FechaAsiento = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    this.MiAsientoContable.FechaRealizado = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();

                    AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
                    this.MapearObjetoAControles(this.MiAsientoContable);                   

                    this.lblTipoOperacion.Visible = false;
                    this.txtTipoOperacion.Visible = false;
                    this.lblRefTipoOperacion.Visible = false;
                    this.txtRefTipoOperacion.Visible = false;

                    this.txtFechaAsiento.Enabled = true;
                    this.ddlTiposAsientos.Enabled = true;
                    this.ddlFiliales.Enabled = true;
                    this.ctrAuditoria.IniciarControl(pAsientoContable);
                    this.btnInvertirValores.Visible = true;
                    this.GestionControl = Gestion.Agregar;
                    this.MisParametrosUrl = new Hashtable();

                    break;
                case Gestion.Modificar:
                    this.MiAsientoContable = ContabilidadF.AsientosContablesObtenerDatosCompletos(pAsientoContable);

                    AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
                    this.MapearObjetoAControles(this.MiAsientoContable);
                    if ((Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.AjustesPorInflacion)
                        || Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.AjustesPorInflacionPorSucursal)
                        this.AjusteInflacionPeriodo = AyudaProgramacion.ObtenerPeriodo(this.MiAsientoContable.FechaAsiento);

                    CtbPeriodosContables ultimoPeriodo = new CtbPeriodosContables();
                    ultimoPeriodo.EjercicioContable.IdEjercicioContable = Convert.ToInt32(this.MiAsientoContable.IdEjercicioContable);
                    ultimoPeriodo.Periodo = Convert.ToInt32(string.Concat(this.MiAsientoContable.FechaAsiento.Year.ToString(), this.MiAsientoContable.FechaAsiento.Month.ToString().PadLeft(2, '0')));
                    bool nomodifica = ContabilidadF.PeriodosContablesValidarCierre(ultimoPeriodo);
                    //ultimoPeriodo.Periodo = ContabilidadF.PeriodosContablesObtenerUltimoCerrado(ultimoPeriodo).Periodo;
                    //int fechaAsiento = Convert.ToInt32( string.Concat( this.MiAsientoContable.FechaAsiento.Year.ToString() , this.MiAsientoContable.FechaAsiento.Month.ToString().PadLeft(2, '0') ));
                    //if (fechaAsiento <= ultimoPeriodo.Periodo && ultimoPeriodo.Periodo != 0) //dado que la base si no hay ningun periodo para ese asiento devuelve 0 (cero)
                    if (nomodifica)
                    {
                        this.btnAceptar.Visible = false;
                        this.MiAsientoContable.CodigoMensaje = "AsientoNoModificable";
                        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAsientoContable.CodigoMensaje));
                    }
                    ddlEstado.Enabled = true;
                    if (!UsuarioActivo.EsAdministradorGeneral && MiAsientoContable.AsientoContableTipo.IdAsientoContableTipo == (int)EnumTiposAsientos.Automaticos)
                    {
                        ddlEstado.Enabled = false;
                    }
                    this.txtFechaAsiento.Enabled = true;
                    this.ddlTiposAsientos.Enabled = this.MiAsientoContable.AsientoContableTipo.IdAsientoContableTipo != (int)EnumTiposAsientos.Automaticos;
                    this.btnCopiarAsiento.Visible = this.ValidarPermiso("AsientosAgregar.aspx");
                    this.btnBuscarComprobante.Visible = true;
                    //this.ddlEstado.Enabled = true;
                    this.ddlFiliales.Enabled = true;
                    break;
                case Gestion.Consultar:
                    this.MiAsientoContable = ContabilidadF.AsientosContablesObtenerDatosCompletos(pAsientoContable);
                    AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
                    this.MapearObjetoAControles(this.MiAsientoContable);
                    this.btnAgregarCuenta.Visible = false;
                    this.btnBuscarAsientoModelo.Visible = false;
                    this.txtDetalle.Enabled = false;
                    this.txtFechaAsiento.Enabled = false;
                    this.txtNumeroAsiento.Enabled = false;
                    this.txtDiferencia.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.btnCopiarAsiento.Visible = this.ValidarPermiso("AsientosAgregar.aspx");
                    this.btnBuscarComprobante.Visible = true;
                    break;
                default:
                    break;
            }
        }

        protected void btnBuscarComprobante_Click(object sender, EventArgs e)
        {
            int IdAfiliado = 0;
            if (this.MiAsientoContable.IdRefTipoOperacion.HasValue)
            {
                TGETiposOperacionesFiltros filtro = new TGETiposOperacionesFiltros();
                filtro.IdTipoOperacion = this.MiAsientoContable.IdTipoOperacion;
                filtro.IdRefTipoOperacion = this.MiAsientoContable.IdRefTipoOperacion.Value;
                IdAfiliado = AfiliadosF.AfiliadosObtenerPorTipoOperacionRefTipoOperacion(filtro).IdAfiliado;
            }    
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAsientoContable", this.MiAsientoContable.IdAsientoContable);
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = this.MiAsientoContable.IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)
            filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

            //Guardo Menu devuelto de la DB
            filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, this.MiAsientoContable.IdRefTipoOperacion);
            this.MisParametrosUrl.Add("IdTipoOperacion", this.MiAsientoContable.IdTipoOperacion);
            //Si devuelve una URL Redirecciona si no muestra mensaje error
            if (filtroMenu.URL.Length != 0)
            {
                if (IdAfiliado > 0)
                {
                    AfiAfiliados afi = new AfiAfiliados();
                    afi.IdAfiliado = IdAfiliado;
                    PaginaAfiliados paginaAfi = new PaginaAfiliados();
                    paginaAfi.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatos(afi));
                }
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros( string.Concat("~/", filtroMenu.URL)), true);
            }
            else
            {
                this.MiAsientoContable.CodigoMensaje = "ErrorURLNoValida";
                this.MostrarMensaje(this.MiAsientoContable.CodigoMensaje, true);
            }
        }

        protected void btnCopiarAsiento_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", Gestion.Copiar);
            this.MisParametrosUrl.Add("IdAsientoContable", this.MiAsientoContable.IdAsientoContable);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosAgregar.aspx"), true);
        }

        protected void btnDescargarPlantilla_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
            {
                CtbCuentasContables cuenta = new CtbCuentasContables();
                cuenta.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
                DataTable dt = ContabilidadF.CtbCuentasContablesObenterImputablesPlantilla(cuenta);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ExportData exportar = new ExportData();
                exportar.ExportExcel(this.Page, ds, true, "PlantillaAsientoContable", "PlantillaAsientoContable");
            }
            else
            {
                this.MostrarMensaje("ValidarSeleccionarEjercicioContable", true);
            }
        }

        #region Importar Archivo
        
        private DataTable Datos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarArchivoDatos"]; }
            set { Session[this.MiSessionPagina + "ImportarArchivoDatos"] = value; }
        }

        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            noactualizarGrilla = true; 
            var excel = new ExcelPackage(new MemoryStream(this.StreamToByteArray(this.afuArchivo.FileContent)));
            this.Datos = ExcelPackageExtensions.ToDataTable(excel);

            if (this.Datos.Rows.Count == 0)
            {
                this.MostrarMensaje("ValidarArchivo", true);
                return;
            }

            if (!this.Datos.Columns.Contains("IdEjercicioContable"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "IdEjercicioContable" });
                return;
            }
            if (!this.Datos.Columns.Contains("IdCuentaContable"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "IdCuentaContable" });
                return;
            }
            if (!this.Datos.Columns.Contains("NumeroCuenta"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "NumeroCuenta" });
                return;
            }
            if (!this.Datos.Columns.Contains("Descripcion"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "Descripcion" });
                return;
            }
            if (!this.Datos.Columns.Contains("Detalle"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "Detalle" });
                return;
            }
            if (!this.Datos.Columns.Contains("Debe"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "Debe" });
                return;
            }
            if (!this.Datos.Columns.Contains("Haber"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "Haber" });
                return;
            }

            ListItem item = this.ddlEjercicioContable.Items.FindByValue(this.Datos.Rows[0]["IdEjercicioContable"].ToString());
            if (item == null)
            {
                this.MostrarMensaje("ValidarSeleccionarEjercicioContableImportarPlantilla", true);
                return;
            }

            if (item.Value != this.ddlEjercicioContable.SelectedValue)
            {
                this.ddlEjercicioContable.SelectedValue = item.Value;
                this.ddlEjercicioContable_SelectedIndexChanged(this.ddlEjercicioContable, EventArgs.Empty);
            }

            //this.MiAsientoContable.AsientosContablesDetalles = new List<CtbAsientosContablesDetalles>();

            CtbAsientosContablesDetalles detalle;
            foreach (DataRow row in this.Datos.Rows)
            {
                detalle = new CtbAsientosContablesDetalles();
                if (row["IdCuentaContable"] == DBNull.Value || row["IdCuentaContable"].ToString() == string.Empty)
                    continue;
                detalle.CuentaContable.IdCuentaContable = Convert.ToInt32(row["IdCuentaContable"]);
                detalle.CuentaContable.NumeroCuenta = row["NumeroCuenta"] != DBNull.Value ? (string)row["NumeroCuenta"] : string.Empty;
                detalle.CuentaContable.Descripcion = row["Descripcion"] != DBNull.Value ? (string)row["Descripcion"] : string.Empty;
                detalle.Detalle = row["Detalle"] != DBNull.Value ? (string)row["Detalle"] : string.Empty;
                detalle.Debe = row["Debe"] != DBNull.Value ? row["Debe"].ToString() == string.Empty ? 0 : decimal.Parse(row["Debe"].ToString(), NumberStyles.Currency) : 0;
                detalle.Haber = row["Haber"] != DBNull.Value ? row["Haber"].ToString() == string.Empty ? 0 : decimal.Parse(row["Haber"].ToString(), NumberStyles.Currency) : 0;
                if ((detalle.Debe.HasValue && detalle.Debe.Value > 0)
                    || detalle.Haber.HasValue && detalle.Haber.Value > 0)
                {
                    detalle.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiAsientoContable.AsientosContablesDetalles.Add(detalle);
                    detalle.IndiceColeccion = this.MiAsientoContable.AsientosContablesDetalles.IndexOf(detalle);
                }
            }
            this.MiAsientoContable.AsientosContablesDetalles.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.AgregadoPrevio);
            this.MiAsientoContable.AsientosContablesDetalles = AyudaProgramacion.AcomodarIndices<CtbAsientosContablesDetalles>(this.MiAsientoContable.AsientosContablesDetalles);
            //AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
            //upCuentasContables.Update();
            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();

        }

        protected void button_Click(object sender, EventArgs e)
        {
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
            this.MapearObjetoAGrilla();

            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();
            this.tcDatos.ActiveTab = this.TabPanel1;
            this.upCuentasContables.Update();
            this.upTabControl.Update();
            noactualizarGrilla = false;
        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            // This is optional
            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }
        #endregion

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiAsientoContable);

            if (Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.AjustesPorInflacion
                || Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.AjustesPorInflacionPorSucursal)
                if (this.AjusteInflacionPeriodo != AyudaProgramacion.ObtenerPeriodo(this.MiAsientoContable.FechaAsiento))
                {
                    this.MostrarMensaje("ValidarAjustePorInflacionModificarPeriodo", true);
                    return;
                }
            this.MiAsientoContable.CargarLoteAsientosContablesDetalles();
            this.MiAsientoContable.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.AsientosContablesAgregar(this.MiAsientoContable);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.AsientosContablesModificar(this.MiAsientoContable);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAsientoContable.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiAsientoContable.CodigoMensaje, true, this.MiAsientoContable.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.AsientoContableDatosCancelar != null)
                this.AsientoContableDatosCancelar();
        }

        protected void btnAgregarCuenta_Click(object sender, EventArgs e)
        {
            this.MapearGrillaAObjeto();
            this.AgregarCuentas(1);
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
            this.MapearObjetoAGrilla();
            ScriptManager.RegisterStartupScript(this.upCuentasContables, this.upCuentasContables.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.upCuentasContables.Update();
        }

        private void AgregarCuentas(int pCantidad)
        {
            CtbAsientosContablesDetalles detalle;
            for (int i = 0; i < pCantidad; i++)
            {
                detalle = new CtbAsientosContablesDetalles();
                detalle.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiAsientoContable.AsientosContablesDetalles.Add(detalle);
                detalle.IndiceColeccion = this.MiAsientoContable.AsientosContablesDetalles.IndexOf(detalle);
            }
        }

        protected void ddlEjercicioContable_SelectedIndexChanged(object sender, EventArgs e)
        {
            CtbEjerciciosContables ejercicio = this.MisEjerciciosContables[this.ddlEjercicioContable.SelectedIndex];
            double desde = ejercicio.FechaInicio.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            double hasta = ejercicio.FechaFin.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string script = string.Format("InitControlFecha('{0}', '{1}');", desde, hasta);
            ScriptManager.RegisterStartupScript(this.upEjercicioContable, this.upEjercicioContable.GetType(), "InitControlFechaScript", script, true);

            DateTime fechaAsiento = this.txtFechaAsiento.Text == string.Empty ? ejercicio.FechaInicio : Convert.ToDateTime(this.txtFechaAsiento.Text);
            if (fechaAsiento.Date > ejercicio.FechaFin.Date)
                fechaAsiento = ejercicio.FechaFin.Date;
            else if (fechaAsiento.Date < ejercicio.FechaInicio.Date)
                fechaAsiento = ejercicio.FechaInicio.Date;
            this.txtFechaAsiento.Text = fechaAsiento.ToShortDateString();

            //Cambiar Cuentas Contables Por Ejercicio
            this.MiAsientoContable.IdEjercicioContable = ejercicio.IdEjercicioContable;

            if (!noactualizarGrilla)
            {
                this.MapearGrillaAObjeto();
                this.MiAsientoContable.CargarLoteAsientosContablesDetalles();
                List<CtbCuentasContables> cuentasNuevas = ContabilidadF.CuentasContablesObtenerPorEjercicioNumeroCuenta(this.MiAsientoContable);
                foreach (CtbAsientosContablesDetalles det in this.MiAsientoContable.AsientosContablesDetalles)
                {
                    if (cuentasNuevas.Exists(x => x.NumeroCuenta == det.CuentaContable.NumeroCuenta))
                    {
                        AyudaProgramacion.MatchObjectProperties(cuentasNuevas.Find(x => x.NumeroCuenta == det.CuentaContable.NumeroCuenta), det.CuentaContable);
                        det.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(det, this.GestionControl);
                    }
                    else
                    {
                        det.CuentaContable = new CtbCuentasContables();
                    }
                }

                //AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
                this.MapearObjetoAGrilla();
                this.upCuentasContables.Update();
            }
        }

        protected void ddlTiposAsientos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.Apertura
                || Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.Cierre
                || Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.CierreActivosYPasivos
                || Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.AjustesPorInflacion
                || Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.AjustesPorInflacionPorSucursal)
            {
                this.btnGenerarAsientoCierre.Text = string.Concat("Armar ", this.ddlTiposAsientos.SelectedItem.Text);
                this.btnGenerarAsientoCierre.Visible = true;
            }
            else
                this.btnGenerarAsientoCierre.Visible = false;

            this.MiAsientoContable.AsientosContablesDetalles = new List<CtbAsientosContablesDetalles>();
            this.AgregarCuentas(2);
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
            this.MapearObjetoAGrilla();
            ScriptManager.RegisterStartupScript(this.upCuentasContables, this.upCuentasContables.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.upCuentasContables.Update();
        }

        protected void btnGenerarAsientoCierre_Click(object sender, EventArgs e)
        {
            this.MiAsientoContable.FechaAsiento = Convert.ToDateTime(this.txtFechaAsiento.Text);
            
            if ((Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.AjustesPorInflacion)
                || (Convert.ToInt32(this.ddlTiposAsientos.SelectedValue) == (int)EnumTiposAsientos.AjustesPorInflacionPorSucursal)
                )
                this.AjusteInflacionPeriodo = AyudaProgramacion.ObtenerPeriodo(this.MiAsientoContable.FechaAsiento);

            this.MiAsientoContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            this.MiAsientoContable.AsientoContableTipo.IdAsientoContableTipo = Convert.ToInt32( this.ddlTiposAsientos.SelectedValue);
            this.MiAsientoContable.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            this.MiAsientoContable.AsientosContablesDetalles = ContabilidadF.AsientosContablesDetallesArmarAsientoCierre(this.MiAsientoContable);
            this.MiAsientoContable.AsientosContablesDetalles.ForEach(x => x.EstadoColeccion = EstadoColecciones.Agregado);
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
            this.MapearObjetoAGrilla();
            ScriptManager.RegisterStartupScript(this.upCuentasContables, this.upCuentasContables.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.upCuentasContables.Update();
        }

        #region Asiento Modelo
        protected void btnBuscarAsientoModelo_Click(object sender, EventArgs e)
        {
            this.MapearGrillaAObjeto();
            if (string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
            {
                this.MostrarMensaje("ValidarSeleccionarEjercicioContable", true);
                return;
            }
            CtbEjerciciosContables ejercicio = new CtbEjerciciosContables();
            ejercicio.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            this.puAsientosModelos.IniciarControl(true, ejercicio);
        }

        protected void puAsientosModelos_AsientosModelosBuscarSeleccionarPopUp(CtbAsientosModelos pAsientoModelo)
        {
            this.MiAsientoContable.AsientosContablesDetalles = new List<CtbAsientosContablesDetalles>();
            CtbAsientosContablesDetalles asientoContableDetalle;
            foreach (var asientoModeloDetalle in pAsientoModelo.AsientosModelosDetalles)
            {
                asientoContableDetalle = new CtbAsientosContablesDetalles();
                asientoContableDetalle.CuentaContable = asientoModeloDetalle.CuentaContable;
                this.MiAsientoContable.AsientosContablesDetalles.Add(asientoContableDetalle);
                asientoContableDetalle.IndiceColeccion = this.MiAsientoContable.AsientosContablesDetalles.IndexOf(asientoContableDetalle);
            }
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
            this.MapearObjetoAGrilla();
            this.upCuentasContables.Update();
        }

        #endregion
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.AsientoContableDatosAceptar != null)
                this.AsientoContableDatosAceptar(null, this.MiAsientoContable);
        }
        
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            if (e.CommandArgument.ToString() != string.Empty)
            {
                int index = Convert.ToInt32(e.CommandArgument);
                //this.MiPosicion = index;
                this.MiPosicion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
                if (e.CommandName == "BuscarCuentaContable") {
                    this.MapearGrillaAObjeto();
                    if (string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
                    {
                        this.MostrarMensaje("ValidarSeleccionarEjercicioContable", true);
                        return;
                    }
                    CtbCuentasContables cta = new CtbCuentasContables();
                    cta.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
                 //   this.puCuentasContables.IniciarControl(true, cta, new List<CtbCuentasContables>());
                }
                else if (e.CommandName == "Borrar")
                {
                    this.MapearGrillaAObjeto();
                    this.MiAsientoContable.AsientosContablesDetalles.RemoveAt(this.MiPosicion);
                    AyudaProgramacion.AcomodarIndices<CtbAsientosContablesDetalles>(this.MiAsientoContable.AsientosContablesDetalles);
                    AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
                    if (this.MiAsientoContable.AsientosContablesDetalles.Count > 0)
                    {
                        this.MapearObjetoAGrilla();
                        this.UpdatePanel2.Update();
                    }
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCuentaContable = ((DropDownList)e.Row.FindControl("ddlCuentaContable"));
                HiddenField hdfIdCuentaContable = (HiddenField)e.Row.FindControl("hdfIdCuentaContable");
                HiddenField hdfCuentaContableDescripcion = (HiddenField)e.Row.FindControl("hdfCuentaContableDescripcion");
                HiddenField hdfCuentaContableNumeroCuenta = (HiddenField)e.Row.FindControl("hdfCuentaContableNumeroCuenta");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                TextBox txtDebe = (TextBox)e.Row.FindControl("txtDebe");
                TextBox txtHaber = (TextBox)e.Row.FindControl("txtHaber");
                txtDebe.Attributes.Add("onchange", "CalcularItem();");
                txtHaber.Attributes.Add("onchange", "CalcularItem();");
                string mensaje = this.ObtenerMensajeSistema("CuentasContablesConfirmarBaja");
                mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnEliminar.Attributes.Add("OnClick", funcion);
                this.MiPosicion = e.Row.RowIndex;
                DropDownList ddlCentrosCostos = (DropDownList)e.Row.FindControl("ddlCentrosCostos");
                ddlCentrosCostos.DataSource = this.MisCentrosCostos;
                ddlCentrosCostos.DataValueField = "IdCentroCostoProrrateo";
                ddlCentrosCostos.DataTextField = "CentroCostoProrrateo";
                ddlCentrosCostos.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlCentrosCostos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                ddlCuentaContable.Enabled = true;
                CtbAsientosContablesDetalles item = (CtbAsientosContablesDetalles)e.Row.DataItem;
                if (item.CuentaContable.IdCuentaContable > 0)
                {

                    ListItem itemComboC = ddlCuentaContable.Items.FindByValue(item.CuentaContable.IdCuentaContable.ToString());
                    if (itemComboC == null)
                    {
                        var descripcionCombo = item.CuentaContable.NumeroCuenta + " - " + item.CuentaContable.Descripcion;

                        ddlCuentaContable.Items.Add(new ListItem(descripcionCombo, item.CuentaContable.IdCuentaContable.ToString()));
                        hdfIdCuentaContable.Value = item.CuentaContable.IdCuentaContable.ToString();
                        hdfCuentaContableNumeroCuenta.Value = item.CuentaContable.NumeroCuenta.ToString();
                        hdfCuentaContableDescripcion.Value = item.CuentaContable.Descripcion.ToString();

                    }
                    ddlCuentaContable.SelectedValue = item.CuentaContable.IdCuentaContable.ToString();
                }

                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                    case Gestion.Copiar:
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Consultar:
                        btnEliminar.Visible = false;
                        break;
                    case Gestion.Modificar:
                        btnEliminar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                Label lblDebe = (Label)e.Row.FindControl("lblDebe");
                Label lblHaber = (Label)e.Row.FindControl("lblHaber");
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiAsientoContable.AsientosContablesDetalles.Count);
                lblDebe.Text = Convert.ToDecimal(this.MiAsientoContable.TotalDebe).ToString("C2");
                lblHaber.Text = Convert.ToDecimal(this.MiAsientoContable.TotalHaber).ToString("C2");
                this.txtDiferencia.Text = (Convert.ToDecimal(this.MiAsientoContable.TotalDebe) - Convert.ToDecimal(this.MiAsientoContable.TotalHaber)).ToString("F2");
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.MapearGrillaAObjeto();
            CtbAsientosModelos parametros = this.BusquedaParametrosObtenerValor<CtbAsientosModelos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbAsientosModelos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiAsientoContable.AsientosContablesDetalles;
            this.gvDatos.DataBind();
            this.MapearObjetoAGrilla();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MapearGrillaAObjeto();
            this.MiAsientoContable.AsientosContablesDetalles = this.OrdenarGrillaDatos<CtbAsientosContablesDetalles>(this.MiAsientoContable.AsientosContablesDetalles, e);
            this.gvDatos.DataSource = this.MiAsientoContable.AsientosContablesDetalles;
            this.gvDatos.DataBind();
            this.MapearObjetoAGrilla();
        }

        //protected void buscarCuenta_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        //{
        //    this.MapearGrillaAObjeto();
        //    if (string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
        //    {
        //        this.MostrarMensaje("ValidarSeleccionarEjercicioContable", true);
        //        return;
        //    }
        //    ejercicio.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
        //}

        //protected void buscarCuenta_CuentasContablesBuscarSeleccionar(CtbCuentasContables pCuentaContable, int indiceColeccion)
        //{
        protected void PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp(CtbCuentasContables e)
        {
            //this.MapearGrillaAObjeto();
            this.MiAsientoContable.AsientosContablesDetalles[this.MiPosicion].CuentaContable = e;
            this.MiAsientoContable.AsientosContablesDetalles[this.MiPosicion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAsientoContable.AsientosContablesDetalles[this.MiPosicion], this.GestionControl);
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
            this.MapearObjetoAGrilla();
            this.upCuentasContables.Update();
            this.UpdatePanel2.Update();
        }

        private void MapearObjetoAControles(CtbAsientosContables pAsientoContable)
        {
            this.txtDetalle.Text = pAsientoContable.DetalleGeneral;
            ListItem item = this.ddlFiliales.Items.FindByValue(pAsientoContable.Filial.IdFilial.ToString());
            if(item!=null)
                this.ddlFiliales.Items.Add(new ListItem(pAsientoContable.Filial.Filial, pAsientoContable.Filial.IdFilial.ToString()));
            this.ddlFiliales.SelectedValue=pAsientoContable.Filial.IdFilial.ToString();
            this.txtNumeroAsiento.Text = pAsientoContable.NumeroAsiento;
            this.ddlEstado.SelectedValue = pAsientoContable.Estado.IdEstado.ToString();
            this.txtTipoOperacion.Text = pAsientoContable.TipoOperacion;
            this.txtRefTipoOperacion.Text = pAsientoContable.IdRefTipoOperacion.ToString();
            this.txtFechaAsiento.Text = pAsientoContable.FechaAsiento.ToShortDateString();
            //this.ddlEjercicioContable.SelectedValue = pAsientoContable.IdEjercicioContable.ToString();
            item = this.ddlEjercicioContable.Items.FindByValue(pAsientoContable.IdEjercicioContable.ToString());
            if (item == null)
                this.ddlEjercicioContable.Items.Add(new ListItem(pAsientoContable.EjercicioDescripcion, pAsientoContable.IdEjercicioContable.ToString()));
            this.ddlEjercicioContable.SelectedValue = pAsientoContable.IdEjercicioContable.ToString();

            if (pAsientoContable.AsientoContableTipo.IdAsientoContableTipo.HasValue)
            {
                item = this.ddlTiposAsientos.Items.FindByValue(pAsientoContable.AsientoContableTipo.IdAsientoContableTipo.ToString());
                if(item == null)
                    this.ddlTiposAsientos.Items.Add(new ListItem(pAsientoContable.AsientoContableTipo.Descripcion, pAsientoContable.AsientoContableTipo.IdAsientoContableTipo.ToString()));
                this.ddlTiposAsientos.SelectedValue = pAsientoContable.AsientoContableTipo.IdAsientoContableTipo.ToString();
            }
            this.MapearObjetoAGrilla();
            //this.ctrComentarios.IniciarControl(pAsientoContable, this.GestionControl);
            //this.ctrArchivos.IniciarControl(pAsientoContable, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pAsientoContable);
        }

        private void MapearControlesAObjeto(CtbAsientosContables pAsientoContable)
        {
            pAsientoContable.FechaAsiento = Convert.ToDateTime(this.txtFechaAsiento.Text);
            pAsientoContable.DetalleGeneral = this.txtDetalle.Text;
            pAsientoContable.Filial.IdFilial = Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pAsientoContable.Filial.Filial = this.ddlFiliales.SelectedItem.Text;
            pAsientoContable.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pAsientoContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            pAsientoContable.AsientoContableTipo.IdAsientoContableTipo = Convert.ToInt32(this.ddlTiposAsientos.SelectedValue);
            this.MapearGrillaAObjeto();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            //Cargar Ejercicios contables
            CtbEjerciciosContables filtro = new CtbEjerciciosContables();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            MisEjerciciosContables = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtro);
            this.ddlEjercicioContable.DataSource = MisEjerciciosContables;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();

            List<TGEListasValoresSistemasDetalles> tiposAsientos = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposAsientos);
            if (tiposAsientos.Exists(x => x.IdListaValorSistemaDetalle == (int)EnumTiposAsientos.Automaticos))
            {
                tiposAsientos.Remove(tiposAsientos.Find(x => x.IdListaValorSistemaDetalle == (int)EnumTiposAsientos.Automaticos));
            }
            
            this.ddlTiposAsientos.DataSource = AyudaProgramacion.AcomodarIndices<TGEListasValoresSistemasDetalles>(tiposAsientos);
            this.ddlTiposAsientos.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTiposAsientos.DataTextField = "Descripcion";
            this.ddlTiposAsientos.DataBind();
            this.ddlTiposAsientos.SelectedValue = ((int)EnumTiposAsientos.Manuales).ToString();

            //AyudaProgramacion.AgregarItemSeleccione(this.ddlEjercicioContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            ////TO DO: Completar con la lista correspondiente, aun no esta definida
            //this.ddlTipoOperacion.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.TiposAsientos);
            //this.ddlTipoOperacion.DataValueField = "IdListaValorDetalle";
            //this.ddlTipoOperacion.DataTextField = "Descripcion";
            //this.ddlTipoOperacion.DataBind();
            ////TO DO: Completar con la lista correspondiente, aun no esta definida
            //this.ddlRefTipoOperacion.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.TiposAsientos);
            //this.ddlRefTipoOperacion.DataValueField = "IdListaValorDetalle";
            //this.ddlRefTipoOperacion.DataTextField = "Descripcion";
            //this.ddlRefTipoOperacion.DataBind();

        }

        private void MapearGrillaAObjeto()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                int indiceColeccion = fila.DataItemIndex;
                DropDownList ddlCuentaContable = ((DropDownList)fila.FindControl("ddlCuentaContable"));

                HiddenField hdfIdCuentaContable = (HiddenField)fila.FindControl("hdfIdCuentaContable");
                HiddenField hdfCuentaContableDescripcion = (HiddenField)fila.FindControl("hdfCuentaContableDescripcion");
                HiddenField hdfCuentaContableNumeroCuenta = (HiddenField)fila.FindControl("hdfCuentaContableNumeroCuenta");

                CurrencyTextBox txtDebe = (CurrencyTextBox)fila.FindControl("txtDebe");
                decimal debe = txtDebe.Decimal; // decimal.Parse(txtDebe.Text, NumberStyles.Currency);
                CurrencyTextBox txtHaber = (CurrencyTextBox)fila.FindControl("txtHaber");
                decimal haber = txtHaber.Decimal; // decimal.Parse(txtHaber.Text, NumberStyles.Currency);
                DropDownList ddlCentrosCostos = ((DropDownList)fila.FindControl("ddlCentrosCostos"));
                Label lblDetalle = (Label)fila.FindControl("lblDetalle");
                TextBox txtDetalle = (TextBox)fila.FindControl("txtDetalle");

                CtbAsientosContablesDetalles item = this.MiAsientoContable.AsientosContablesDetalles[indiceColeccion];

                if (item.CentroCostoProrrateo.IdCentroCostoProrrateo.HasValue)
                {
                    //Preguntar si no existe el Centro de Costo si se agrega
                    ListItem itemCombo = ddlCentrosCostos.Items.FindByValue(item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString());
                    if (itemCombo == null)
                        ddlCentrosCostos.Items.Add(new ListItem(item.CentroCostoProrrateo.CentroCostoProrrateo.ToString(), item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString()));
                    //ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                }
                if (hdfIdCuentaContable.Value != string.Empty && Convert.ToInt32(hdfIdCuentaContable.Value) > 0)
                {
                    item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(item, GestionControl);
                    item.CuentaContable.IdCuentaContable = Convert.ToInt32(hdfIdCuentaContable.Value);
                    item.CuentaContable.Descripcion = hdfCuentaContableDescripcion.Value;
                    item.CuentaContable.NumeroCuenta = hdfCuentaContableNumeroCuenta.Value;
       
                }
                if (item.Debe.HasValue && debe != item.Debe
                    || !item.Debe.HasValue && debe > 0)
                {
                    item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(item, this.GestionControl);
                    item.Debe = debe;
                }

                if (item.Haber.HasValue && haber != item.Haber
                    || !item.Haber.HasValue && haber > 0)
                {
                    item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAsientoContable.AsientosContablesDetalles[indiceColeccion], this.GestionControl);
                    item.Haber = haber;
                }

                if (item.CentroCostoProrrateo.IdCentroCostoProrrateo.HasValue && ddlCentrosCostos.SelectedValue != item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString()
                    || !item.CentroCostoProrrateo.IdCentroCostoProrrateo.HasValue && ddlCentrosCostos.SelectedValue != string.Empty)
                {
                    item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAsientoContable.AsientosContablesDetalles[indiceColeccion], this.GestionControl);
                    item.CentroCostoProrrateo = ddlCentrosCostos.SelectedValue==string.Empty? new CtbCentrosCostosProrrateos() : this.MisCentrosCostos[ddlCentrosCostos.SelectedIndex];
                }
                if (item.Detalle.Trim() != txtDetalle.Text.Trim())
                {
                    item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAsientoContable.AsientosContablesDetalles[indiceColeccion], this.GestionControl);
                    item.Detalle = txtDetalle.Text.Trim();
                }
                    
            }
            this.txtDiferencia.Text = (Convert.ToDecimal(this.MiAsientoContable.TotalDebe) - Convert.ToDecimal(this.MiAsientoContable.TotalHaber)).ToString("N2");
            
        }

        private void MapearObjetoAGrilla()
        {
            if (this.MiAsientoContable.AsientosContablesDetalles.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                int indiceColeccion = fila.DataItemIndex;
                  DropDownList ddlCuentaContable = ((DropDownList)fila.FindControl("ddlCuentaContable"));
                
                HiddenField hdfIdCuentaContable = (HiddenField)fila.FindControl("hdfIdCuentaContable");
                TextBox txtDebe = (TextBox)fila.FindControl("txtDebe");
                TextBox txtHaber = (TextBox)fila.FindControl("txtHaber");
                TextBox txtNumeroCuenta = (TextBox)fila.FindControl("txtNumeroCuenta");
                TextBox txtDescripcion = (TextBox)fila.FindControl("txtDescripcion");
                Label lblDetalle = (Label)fila.FindControl("lblDetalle");
                TextBox txtDetalle = (TextBox)fila.FindControl("txtDetalle");
                DropDownList ddlCentrosCostos = ((DropDownList)fila.FindControl("ddlCentrosCostos"));

                CtbAsientosContablesDetalles item = this.MiAsientoContable.AsientosContablesDetalles[indiceColeccion];
                hdfIdCuentaContable.Value = item.CuentaContable.IdCuentaContable.ToString();
       
               // txtDescripcion.Text = item.CuentaContable.Descripcion;

                if (this.GestionControl == Gestion.Consultar)
                {
                    lblDetalle.Visible = true;
                }
                else if (this.GestionControl == Gestion.Agregar ||
                    this.GestionControl == Gestion.Modificar ||
                    this.GestionControl == Gestion.Copiar)
                {
                    txtDetalle.Visible = true;
                }


                if (item.Debe.HasValue)
                {
                    txtDebe.Text = item.Debe.Value.ToString("C2");
                    if (this.GestionControl == Gestion.Consultar)
                    {
                        txtHaber.Enabled = false;
                        txtDebe.Enabled = false;
                    }
                }
                if (item.Haber.HasValue)
                {
                    txtHaber.Text = item.Haber.Value.ToString("C2");
                    if (this.GestionControl == Gestion.Consultar)
                    {
                        txtDebe.Enabled = false;
                        txtHaber.Enabled = false;
                    }
                }
                
                if (item.CentroCostoProrrateo.IdCentroCostoProrrateo > 0)
                {
                    ListItem itemCombo = ddlCentrosCostos.Items.FindByValue(item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString());
                    if (itemCombo == null)
                        ddlCentrosCostos.Items.Add(new ListItem(item.CentroCostoProrrateo.CentroCostoProrrateo.ToString(), item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString()));
                    ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                }

               

            }
        }

    }
}