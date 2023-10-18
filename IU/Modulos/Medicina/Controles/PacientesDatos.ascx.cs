using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using IU.Modulos.Afiliados.Controles;
using Medicina;
using Medicina.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IU.Modulos.Medicina.Controles
{
    public partial class PacientesDatos : ControlesSeguros
    {
        private AfiPacientes MiAfiliado
        {
            get { return (AfiPacientes)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiAfiliado"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiAfiliado"] = value; }
        }
        private List<MedPrestaciones> MisPrestaciones
        {
            get { return (List<MedPrestaciones>)Session[this.MiSessionPagina + "PrestacionesListarMisPrestaciones"]; }
            set { Session[this.MiSessionPagina + "PrestacionesListarMisPrestaciones"] = value; }
        }
        private List<MedTurnos> MisTurnos
        {
            get { return (List<MedTurnos>)Session[this.MiSessionPagina + "TurnosMedicosListarMisPrestaciones"]; }
            set { Session[this.MiSessionPagina + "TurnosMedicosListarMisPrestaciones"] = value; }
        }

        private List<MedEstudios> MisEstudios
        {
            get { return (List<MedEstudios>)Session[this.MiSessionPagina + "EstudiosListarMisPrestaciones"]; }
            set { Session[this.MiSessionPagina + "EstudiosListarMisPrestaciones"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        public delegate void AfiliadoDatosAceptarEventHandler(object sender, AfiPacientes e);
        public event AfiliadoDatosAceptarEventHandler AfiliadosModificarDatosAceptar;
        public delegate void AfiliadoDatosCancelarEventHandler();
        public event AfiliadoDatosCancelarEventHandler AfiliadosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrComentarios.ComentariosPersistirDatosGrilla += new IU.Modulos.Comunes.Comentarios.ComentariosPersistirDatosGrillaEventHandler(comentarios_PersistirDatosGrilla);
            this.ctrDomicilios.AfiliadosModificarDatosAceptar += new AfiliadoModificarDatosDomicilioPopUp.AfiliadoModificarDatosDomicilioEventHandler(ctrDomicilios_AfiliadosModificarDatosAceptar);
            this.ctrTelefonos.AfiliadosModificarDatosAceptar += new AfiliadoModificarDatosTelefonoPopUp.AfiliadoModificarDatosTelefonoEventHandler(ctrTelefonos_AfiliadosModificarDatosAceptar);
            base.PageLoadEvent(sender, e);
            if (this.IsPostBack)
            {
                if (this.MiAfiliado == null)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlSesion.aspx"), true);
                }
            }
        }
        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.AfiliadosModificarDatosAceptar != null)
                this.AfiliadosModificarDatosAceptar(null, this.MiAfiliado);
        }
        void comentarios_PersistirDatosGrilla()
        {
            ctrComentarios.PersistirDatosGrilla();
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de un Afiliado
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(AfiPacientes pAfiliado, Gestion pGestion)
        {
            AfiAfiliados tabIndex = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            tcDatos.ActiveTabIndex = tabIndex.HashTransaccion;

            this.GestionControl = pGestion;
            this.MiAfiliado = pAfiliado;
            this.btnAceptar.Visible = true;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlEstados.SelectedValue = ((int)EstadosAfiliados.Normal).ToString();
                    //this.txtTipoAfiliado.Text = EnumAfiliadosTipos.Clientes.ToString();
                    this.ddlEstados.Enabled = false;
                    this.txtApellido.Enabled = true;

                    //this.ctrArchivos.IniciarControl(this.MiAfiliado, this.GestionControl);
                    this.ctrComentarios.IniciarControl(this.MiAfiliado, this.GestionControl);
                    AfiPacientes cliente = new AfiPacientes();
                    cliente.IdAfiliado = this.MiAfiliado.IdAfiliado;
                    this.ctrCamposValores.IniciarControl(cliente, new Objeto(), this.GestionControl);

                    //ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.UpdatePanel2.GetType(), "InitControlsScript", "InitControls();", true);
                    break;
                case Gestion.Modificar:
                    this.MiAfiliado = AfiliadosF.PacientesObtenerDatosCompletos(pAfiliado);
                    this.MapearObjetoAControles(this.MiAfiliado);

                    MedTurnos turnos = this.BusquedaParametrosObtenerValor<MedTurnos>();

                    this.CargarListaTurnos(turnos);

                    MedPrestaciones prestaciones = this.BusquedaParametrosObtenerValor<MedPrestaciones>();
                    this.CargarLista(prestaciones);

                    MedEstudios estudios = this.BusquedaParametrosObtenerValor<MedEstudios>();
                    this.CargarListaEstudios(estudios);

                    if (this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo != (int)EnumAfiliadosTipos.PacientesExternos)
                    {
                        this.txtApellido.Enabled = false;
                        this.txtNombre.Enabled = false;
                        this.ddlEstados.Enabled = false;
                        this.ddlTipoDocumento.Enabled = false;
                        this.txtNumeroDocumento.Enabled = false;
                        this.txtCorreoElectronico.Enabled = false;
                        this.ddlGrupoSanguineo.Enabled = false;
                        this.ddlSexo.Enabled = false;
                        this.txtFechaNacimiento.Enabled = false;
                    }
                    else
                    {
                        this.txtApellido.Enabled = true;
                    }
                    break;
                case Gestion.Consultar:
                    this.MiAfiliado = AfiliadosF.PacientesObtenerDatosCompletos(pAfiliado);
                    this.MapearObjetoAControles(this.MiAfiliado);

                    MedTurnos turnosc = this.BusquedaParametrosObtenerValor<MedTurnos>();

                    this.CargarListaTurnos(turnosc);

                    MedPrestaciones prestacionesc = this.BusquedaParametrosObtenerValor<MedPrestaciones>();
                    this.CargarLista(prestacionesc);

                    MedEstudios estudiosC = this.BusquedaParametrosObtenerValor<MedEstudios>();
                    this.CargarListaEstudios(estudiosC);
                    //AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnAceptar.Visible = false;
                    this.txtFechaNacimiento.Enabled = false;
                    this.ddlGrupoSanguineo.Enabled = false;
                    this.ddlSexo.Enabled = false;
                    this.txtAntecedentesFamiliares.Enabled = false;
                    this.txtAntecedentesPersonales.Enabled = false;
                    //this.txtIdAfiliado.Enabled = false;
                    this.txtApellido.Enabled = false;
                    this.ddlEstados.Enabled = false;
                    this.txtNombre.Enabled = false;
                    this.ddlTipoDocumento.Enabled = false;
                    this.txtNumeroDocumento.Enabled = false;
                    this.ddlCondicionFiscal.Enabled = false;
                    this.txtCorreoElectronico.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarLista(MedPrestaciones pPrestacion)
        {
            //pPrestacion.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            //pPrestacion.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            //pPrestacion.Apellido = this.txtApellido.Text.Trim();
            //pPrestacion.Nombre = this.txtNombre.Text.Trim();
            //pPrestacion.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pPrestacion.Afiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
            pPrestacion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<MedPrestaciones>(pPrestacion);
            this.MisPrestaciones = MedicinaF.PrestacionesObtenerListaFiltroPacientes(pPrestacion);
            this.gvDatos.PageIndex = pPrestacion.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<MedPrestaciones>(this.MisPrestaciones, false, this.gvDatos, true);
        }
        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            MedTurnos prestacion = this.MisTurnos[indiceColeccion];

            PaginaAfiliados pagina = new PaginaAfiliados();
            pagina.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatosCompletos(prestacion.Afiliado));

            MedHistoriasClinicas historiaClinica = new MedHistoriasClinicas();
            historiaClinica.IdAfiliado = prestacion.Afiliado.IdAfiliado;
            if (!MedicinaF.HistoriasClinicasValidarExiste(historiaClinica))
            {
                historiaClinica.FechaAlta = DateTime.Now;
                historiaClinica.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
                historiaClinica.Estado.IdEstado = (int)Estados.Activo;
                if (!MedicinaF.HistoriasClinicasAgregar(historiaClinica))
                {
                    this.MostrarMensaje(historiaClinica.CodigoMensaje, true);
                    return;
                }
            }

            this.MisParametrosUrl = new Hashtable
            {
                { "IdTurno", prestacion.IdTurno }
            };
        }
        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultarTurnos");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificarTurnos");

                //Permisos btnEliminar
                ibtnConsultar.Visible = false;// this.ValidarPermiso("HistoriasClinicasConsultar.aspx");
                ibtnModificar.Visible = false;//this.ValidarPermiso("HistoriasClinicasModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisTurnos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvTurnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            MedPrestadores parametros = this.BusquedaParametrosObtenerValor<MedPrestadores>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<MedPrestadores>(parametros);

            this.gvTurnos.PageIndex = e.NewPageIndex;
            this.gvTurnos.DataSource = this.MisTurnos;
            this.gvTurnos.DataBind();
        }
        protected void gvTurnos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisTurnos = this.OrdenarGrillaDatos<MedTurnos>(this.MisTurnos, e);
            this.gvTurnos.DataSource = this.MisTurnos;
            this.gvTurnos.DataBind();
        }
        private void CargarListaTurnos(MedTurnos pPrestacion)
        {
            //pPrestacion.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            //pPrestacion.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            //pPrestacion.Apellido = this.txtApellido.Text.Trim();
            //pPrestacion.Nombre = this.txtNombre.Text.Trim();
            //pPrestacion.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pPrestacion.Afiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
            pPrestacion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<MedTurnos>(pPrestacion);
            this.MisTurnos = MedicinaF.TurnosObtenerListaFiltroPacientes(pPrestacion);
            this.gvTurnos.DataSource = this.MisTurnos;
            this.gvTurnos.PageIndex = pPrestacion.IndiceColeccion;
            this.gvTurnos.DataBind();
            AyudaProgramacion.CargarGrillaListas<MedTurnos>(this.MisTurnos, false, this.gvTurnos, true);
        }
        protected void gvEstudios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            MedEstudios estudio = this.MisEstudios[indiceColeccion];

            PaginaAfiliados pagina = new PaginaAfiliados();
            pagina.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatosCompletos(estudio.Afiliado));

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEstudio", estudio.IdEstudio);
            this.MisParametrosUrl.Add("IdAfiliado", MiAfiliado.IdAfiliado);

            AfiAfiliados tabIndex = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            tabIndex.HashTransaccion = 5;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(tabIndex);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/EstudiosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/EstudiosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/EstudiosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                try
                {
                    estudio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    byte[] pdf = MedicinaF.EstudiosObtenerComprobante(estudio);
                    ExportPDF.ExportarPDF(pdf, this.upPacientes, string.Concat("Estudio_", estudio.IdEstudio.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                }
                catch (Exception ex)
                {
                    MostrarMensaje(ex.Message, true);
                }
            }
        }
        protected void gvEstudios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultarEstudios");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificarEstudios");
                ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnModificarEstudios");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");

                //Permisos btnEliminar
                ibtnConsultar.Visible = ValidarPermiso("EstudiosConsultar.aspx");
                ibtnModificar.Visible = ValidarPermiso("EstudiosModificar.aspx");
                ibtnImprimir.Visible = true;
                ibtnAnular.Visible = ValidarPermiso("EstudiosAnular.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisTurnos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvEstudios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            MedEstudios parametros = this.BusquedaParametrosObtenerValor<MedEstudios>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<MedEstudios>(parametros);

            gvTurnos.PageIndex = e.NewPageIndex;
            gvTurnos.DataSource = this.MisEstudios;
            this.gvTurnos.DataBind();
        }

        protected void gvEstudios_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisEstudios = this.OrdenarGrillaDatos<MedEstudios>(this.MisEstudios, e);
            this.gvTurnos.DataSource = this.MisEstudios;
            this.gvTurnos.DataBind();
        }
        private void CargarListaEstudios(MedEstudios pEstudios)
        {
            pEstudios.Afiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
            pEstudios.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<MedEstudios>(pEstudios);
            this.MisEstudios = MedicinaF.EstudiosObtenerListaFiltro(pEstudios);
            this.gvEstudios.DataSource = this.MisEstudios;
            this.gvEstudios.PageIndex = pEstudios.IndiceColeccion;
            this.gvEstudios.DataBind();
            AyudaProgramacion.CargarGrillaListas<MedTurnos>(this.MisTurnos, false, this.gvTurnos, true);
        }
        /// <summary>
        /// Mapea la Entidad SolicitudesMateriales a los controles de Pantalla
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearObjetoAControles(AfiPacientes pAfiliado)
        {
            //this.txtIdAfiliado.Text = pAfiliado.IdAfiliado.ToString();
            this.ddlEstados.SelectedValue = pAfiliado.Estado.IdEstado.ToString();
            this.txtApellido.Text = pAfiliado.Apellido;
            this.txtNombre.Text = pAfiliado.Nombre;
            this.ddlTipoDocumento.SelectedValue = pAfiliado.TipoDocumento.IdTipoDocumento.HasValue ? pAfiliado.TipoDocumento.IdTipoDocumento.Value.ToString() : string.Empty;
            this.txtNumeroDocumento.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtCorreoElectronico.Text = pAfiliado.CorreoElectronico;
            this.txtAntecedentesPersonales.Text = pAfiliado.AntecedentesPersonales;
            this.txtAntecedentesFamiliares.Text = pAfiliado.AntecedentesFamiliares;
            if (!string.IsNullOrEmpty(pAfiliado.FechaNacimiento.ToString()))
            {
                DateTime FechaNacimiento = Convert.ToDateTime(pAfiliado.FechaNacimiento.ToString());
                this.txtFechaNacimiento.Text = FechaNacimiento.ToShortDateString();
                int edad = DateTime.Today.AddTicks(-FechaNacimiento.Ticks).Year - 1;
                int year = DateTime.Now.Year - FechaNacimiento.Year;
                int month = 0;
                if (DateTime.Now.Month < FechaNacimiento.Month)
                    month = 12 - FechaNacimiento.Month + DateTime.Now.Month;
                else
                    month = DateTime.Now.Month - FechaNacimiento.Month;
                txtEdad.Text = edad.ToString() + " años y " + month.ToString() + " meses";
            }
            //this.txtTipoAfiliado.Text = pAfiliado.AfiliadoTipo.AfiliadoTipo;
            ListItem condicionFiscal = this.ddlCondicionFiscal.Items.FindByValue(pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString());
            if (condicionFiscal == null && pAfiliado.CondicionFiscal.IdCondicionFiscal > 0)
                this.ddlCondicionFiscal.Items.Add(new ListItem(pAfiliado.CondicionFiscal.Descripcion, pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString()));
            this.ddlCondicionFiscal.SelectedValue = pAfiliado.CondicionFiscal.IdCondicionFiscal == 0 ? string.Empty : pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString();
            ListItem GrupoSanguieno = this.ddlGrupoSanguineo.Items.FindByValue(pAfiliado.GrupoSanguieno.IdGrupoSanguieno.ToString());
            if (GrupoSanguieno == null && pAfiliado.GrupoSanguieno.IdGrupoSanguieno > 0)
                this.ddlGrupoSanguineo.Items.Add(new ListItem(pAfiliado.GrupoSanguieno.GrupoSanguineo, pAfiliado.GrupoSanguieno.IdGrupoSanguieno.ToString()));
            this.ddlGrupoSanguineo.SelectedValue = pAfiliado.GrupoSanguieno.IdGrupoSanguieno == 0 ? string.Empty : pAfiliado.GrupoSanguieno.IdGrupoSanguieno.ToString();
            ListItem Sexo = this.ddlSexo.Items.FindByValue(pAfiliado.Sexo.IdSexo.ToString());
            if (Sexo == null && pAfiliado.Sexo.IdSexo > 0)
                this.ddlSexo.Items.Add(new ListItem(pAfiliado.Sexo.Sexo, pAfiliado.Sexo.IdSexo.ToString()));
            this.ddlSexo.SelectedValue = pAfiliado.Sexo.IdSexo == 0 ? string.Empty : pAfiliado.Sexo.IdSexo.ToString();
            //VTACuentasCorrientes filtro = new VTACuentasCorrientes();
            //filtro.IdAfiliado = pAfiliado.IdAfiliado;
            //this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorCliente(pAfiliado);
            /* ACA VAMOS A CARGAR LA CUENTA CORRIENTE POR MEDIO DEL DATASET*/
            //this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(this.MiAfiliado);
            //VTACuentasCorrientes parametros = this.BusquedaParametrosObtenerValor<VTACuentasCorrientes>();
            //this.gvDatos.PageIndex = parametros.IndiceColeccion;
            //this.gvDatos.DataSource = this.MiCuentaCorriente;
            //this.gvDatos.DataBind();

            //AyudaProgramacion.CargarGrillaListas<VTACuentasCorrientes>(this.MiCuentaCorriente, false, this.gvDatos, true);
            //if (this.MiCuentaCorriente.Rows.Count > 0)
            //    btnExportarExcel.Visible = true;
            //else
            //    btnExportarExcel.Visible = false;


            this.ctrComentarios.IniciarControl(pAfiliado, this.GestionControl);
            //this.ctrArchivos.IniciarControl(pAfiliado, this.GestionControl);
            this.gvDomicilios.DataSource = pAfiliado.Domicilios;
            this.gvDomicilios.DataBind();

            this.gvTelefonos.DataSource = this.MiAfiliado.Telefonos;
            this.gvTelefonos.DataBind();


            AfiPacientes cliente = new AfiPacientes();
            cliente.IdAfiliado = this.MiAfiliado.IdAfiliado;
            cliente.Campos.AddRange(pAfiliado.Campos);
            this.ctrCamposValores.IniciarControl(cliente, new Objeto(), this.GestionControl);







            // ? this.gvDatos.PageIndex = presupuestos.IndiceColeccion;

        }

        /// <summary>
        /// Mapea los controles de Pantalla con la Entidad SolicitudesMateriales.
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearControlesAObjeto(AfiPacientes pAfiliado)
        {
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pAfiliado.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pAfiliado.Apellido = this.txtApellido.Text;
            pAfiliado.Nombre = this.txtNombre.Text;
            pAfiliado.TipoDocumento.IdTipoDocumento = this.ddlTipoDocumento.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.CorreoElectronico = this.txtCorreoElectronico.Text;
            pAfiliado.CondicionFiscal.IdCondicionFiscal = this.ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscal.SelectedValue);
            pAfiliado.GrupoSanguieno.IdGrupoSanguieno = this.ddlGrupoSanguineo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlGrupoSanguineo.SelectedValue);
            pAfiliado.GrupoSanguieno.GrupoSanguineo = this.ddlGrupoSanguineo.SelectedItem.Text;
            pAfiliado.Comentarios = ctrComentarios.ObtenerLista();
            pAfiliado.Campos = this.ctrCamposValores.ObtenerLista();
            pAfiliado.FechaNacimiento = this.txtFechaNacimiento.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(this.txtFechaNacimiento.Text);
            pAfiliado.Sexo.IdSexo = this.ddlSexo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlSexo.SelectedValue);
            pAfiliado.AntecedentesFamiliares = txtAntecedentesFamiliares.Text;
            pAfiliado.AntecedentesPersonales = txtAntecedentesPersonales.Text;
            //pAfiliado.Archivos = ctrArchivos.ObtenerLista();
        }

        /// <summary>
        /// Llena los DropDownList de la Pantalla con los datos
        /// </summary>
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosAfiliados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            this.ddlSexo.DataSource = AfiliadosF.SexoObtenerLista();
            this.ddlSexo.DataValueField = "IdSexo";
            this.ddlSexo.DataTextField = "Sexo";
            this.ddlSexo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlSexo, this.ObtenerMensajeSistema("SeleccioneOpcion"));



            this.ddlGrupoSanguineo.DataSource = AfiliadosF.GruposSanguineosObtenerListar();
            this.ddlGrupoSanguineo.DataValueField = "IdGrupoSanguieno";
            this.ddlGrupoSanguineo.DataTextField = "GrupoSanguineo";
            this.ddlGrupoSanguineo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlGrupoSanguineo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoDocumento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscal.DataTextField = "Descripcion";
            this.ddlCondicionFiscal.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        protected void btntxtNumeroDocumentoBlur_Click(object sender, EventArgs e)
        {
            MapearControlesAObjeto(MiAfiliado);
            if (txtNumeroDocumento.Text != string.Empty
                && !string.IsNullOrEmpty(ddlTipoDocumento.SelectedValue)
                && (ddlTipoDocumento.SelectedValue == ((int)EnumTiposDocumentos.CUIL).ToString()
                    || ddlTipoDocumento.SelectedValue == ((int)EnumTiposDocumentos.CUIT).ToString()))
            {
                if (!AfiliadosF.AfiliadosObtenerDatosAFIP(MiAfiliado))
                {
                    this.txtApellido.Enabled = true;
                    MostrarMensaje(MiAfiliado.CodigoMensaje, true);
                }
                else
                {
                    this.txtApellido.Enabled = false;
                }
            }
            else
                this.txtApellido.Enabled = true;

            MapearObjetoAControles(MiAfiliado);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            MedPrestaciones prestador = this.MisPrestaciones[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", MiAfiliado.IdAfiliado);
            this.MisParametrosUrl.Add("IdPrestacion", prestador.IdPrestacion);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesModificar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("PrestacionesConsultar.aspx");
                ibtnModificar.Visible = this.ValidarPermiso("PrestacionesModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestaciones.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            MedPrestadores parametros = this.BusquedaParametrosObtenerValor<MedPrestadores>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<MedPrestadores>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisPrestaciones;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPrestaciones = this.OrdenarGrillaDatos<MedPrestaciones>(this.MisPrestaciones, e);
            this.gvDatos.DataSource = this.MisPrestaciones;
            this.gvDatos.DataBind();
        }

        #region Domicilios

        protected void btnAgregarDomicilio_Click(object sender, EventArgs e)
        {
            this.ctrDomicilios.IniciarControl(new AfiDomicilios(), Gestion.Agregar);
        }

        void ctrDomicilios_AfiliadosModificarDatosAceptar(object sender, AfiDomicilios e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiAfiliado.Domicilios.Add(e);
                    e.IndiceColeccion = this.MiAfiliado.Domicilios.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiAfiliado.Domicilios[this.MiIndiceDetalleModificar] = e;

                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiAfiliado.Domicilios, true, this.gvDomicilios, true);
            this.upDomicilios.Update();
        }

        protected void gvDomicilios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrDomicilios.IniciarControl(this.MiAfiliado.Domicilios[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrDomicilios.IniciarControl(this.MiAfiliado.Domicilios[indiceColeccion], Gestion.Consultar);
            }
        }

        protected void gvDomicilios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiDomicilios item = (AfiDomicilios)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("DomiciliosConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Telefonos

        protected void gvTelefonos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrTelefonos.IniciarControl(this.MiAfiliado.Telefonos[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrTelefonos.IniciarControl(this.MiAfiliado.Telefonos[indiceColeccion], Gestion.Consultar);
            }
        }

        protected void gvTelefonos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiTelefonos item = (AfiTelefonos)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.Cells[4].FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("TelefonosConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        void ctrTelefonos_AfiliadosModificarDatosAceptar(object sender, AfiTelefonos e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiAfiliado.Telefonos.Add(e);
                    e.IndiceColeccion = this.MiAfiliado.Telefonos.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiAfiliado.Telefonos[this.MiIndiceDetalleModificar] = e;
                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiAfiliado.Telefonos, true, this.gvTelefonos, true);
            this.upTelefonos.Update();
        }

        protected void btnAgregarTelefono_Click(object sender, EventArgs e)
        {
            AfiTelefonos telefono = new AfiTelefonos();
            telefono.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.ctrTelefonos.IniciarControl(telefono, Gestion.Agregar);
        }

        #endregion

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.PersistirDatos();
            this.Page.Validate("AfiliadosModificarDatosAceptar");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiAfiliado);
            //this.ActualizarGrilla();
            this.MiAfiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, this.MiAfiliado.UsuarioAlta);
                    this.MiAfiliado.UsuarioAlta.IdUsuarioAlta = this.MiAfiliado.UsuarioAlta.IdUsuario;
                    this.MiAfiliado.FechaAlta = DateTime.Now;
                    if (AfiliadosF.AfiliadosAgregar(this.MiAfiliado))
                    {
                        //this.MostrarMensaje(this.MiSolMat.CodigoMensaje, false, this.MiSolMat.CodigoMensajeArgs);
                        //this.RepNotaPedido.CargarReporte(this.MiAfiliado);
                        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje));
                    }
                    else
                    {
                        this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true, this.MiAfiliado.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Modificar:
                    comentarios_PersistirDatosGrilla();
                    //COMENTADO 12/06 PARA EL TICKET NUMERO 900
                    //if (MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios || MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Familiares)
                    //{
                    //    this.MostrarMensaje("PacientesValidarSociosTitulares", true);
                    //    return;
                    //}
                    //if (MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Clientes)
                    //{
                    //    this.MostrarMensaje("PacientesValidarSociosClientes", true);
                    //    return;
                    //}
                    if (AfiliadosF.AfiliadosModificar(this.MiAfiliado))
                    {
                        PaginaAfiliados pagina = new PaginaAfiliados();
                        AfiAfiliados afi = pagina.Obtener(this.MiSessionPagina);
                        if (afi.IdAfiliado == this.MiAfiliado.IdAfiliado)
                            pagina.Guardar(this.MiSessionPagina, this.MiAfiliado);
                        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje));
                    }
                    else
                    {
                        if (!this.MiAfiliado.ErrorAccesoDatos && this.MiAfiliado.ConfirmarAccion)
                        {
                            this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje, this.MiAfiliado.CodigoMensajeArgs), true);
                        }
                        else
                            this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true, this.MiAfiliado.CodigoMensajeArgs);
                    }
                    break;
                default:
                    break;
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.AfiliadosModificarDatosCancelar != null)
                this.AfiliadosModificarDatosCancelar();
        }

        protected void btnAgregarPrestacion_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", MiAfiliado.IdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesAgregar.aspx"), true);
        }
        protected void btnAgregarTurno_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", MiAfiliado.IdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/TurnerasModificar.aspx"), true);
        }
        protected void btnAgregarEstudio_Click(object sender, EventArgs e)
        {
            AfiAfiliados tabIndex = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            tabIndex.HashTransaccion = 5;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(tabIndex);

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", MiAfiliado.IdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/EstudiosAgregar.aspx"), true);
        }
    }
}
