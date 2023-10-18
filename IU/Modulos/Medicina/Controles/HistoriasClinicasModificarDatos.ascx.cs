using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using Medicina;
using System.Collections;
using Afiliados.Entidades;
using Afiliados;

namespace IU.Modulos.Medicina.Controles
{
    public partial class HistoriasClinicasModificarDatos : ControlesSeguros
    {
        private MedHistoriasClinicas MiHistoriaClinica
        {
            get { return (MedHistoriasClinicas)Session[this.MiSessionPagina + "HistoriasClinicasModificarDatosMiHistoriaClinica"]; }
            set { Session[this.MiSessionPagina + "HistoriasClinicasModificarDatosMiHistoriaClinica"] = value; }
        }
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
        private int MiIdPrestacion
        {
            get { return (int)Session[this.MiSessionPagina + "HistoriasClinicasModificarDatosMiIdPrestacion"]; }
            set { Session[this.MiSessionPagina + "HistoriasClinicasModificarDatosMiIdPrestacion"] = value; }
        }

        //private MedPrestadores MiPrestador
        //{
        //    get { return (MedPrestadores)Session[this.MiSessionPagina + "TurnerasModificarDatosMiPrestador"]; }
        //    set { Session[this.MiSessionPagina + "TurnerasModificarDatosMiPrestador"] = value; }
        //}

        public delegate void ModificarDatosAceptarEventHandler(MedHistoriasClinicas e, Gestion pGestion);
        public event ModificarDatosAceptarEventHandler ModificarDatosAceptar;
        public delegate void ModificarDatosCancelarEventHandler();
        public event ModificarDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {

            }
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar(this.MiHistoriaClinica, this.GestionControl);
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de un Afiliado
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(MedHistoriasClinicas pParametro, int pIdPrestacion, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiHistoriaClinica = pParametro;
            this.MiIdPrestacion = pIdPrestacion;

            //AfiAfiliados tabIndex = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            //tcDatos.ActiveTabIndex = tabIndex.HashTransaccion;

            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                    //this.ddlEstados.Enabled = false;
                    //this.txtFecha.Text = DateTime.Now.ToShortDateString();
                    //this.ctrCamposValores.IniciarControl(this.MiHistoriaClinica, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
              //      this.MiHistoriaClinica = MedicinaF.HistoriasClinicasObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiHistoriaClinica);
                    MedPrestaciones prestaciones = this.BusquedaParametrosObtenerValor<MedPrestaciones>();
                    this.CargarLista(prestaciones);
                    break;
                case Gestion.Consultar:
                    this.MiHistoriaClinica = MedicinaF.HistoriasClinicasObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiHistoriaClinica);
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Mapea la Entidad SolicitudesMateriales a los controles de Pantalla
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearObjetoAControles(MedHistoriasClinicas pParametro)
        {
            //this.txtFecha.Text = pParametro.Fecha.ToShortDateString();
            //this.txtAfiliado.Text = pParametro.Afiliado.ApellidoNombre;
            //this.txtTipoDocumento.Text = pParametro.Afiliado.TipoDocumento.TipoDocumento;
            //this.txtNumeroDocumento.Text = pParametro.Afiliado.NumeroDocumento.ToString();
            //this.txtFechaNacimiento.Text = AyudaProgramacion.MostrarFechaPantalla(pParametro.Afiliado.FechaNacimiento);
            //this.txtPrestador.Text = pParametro.Prestador.ApellidoNombre;
            //ListItem item = this.ddlPrestadores.Items.FindByValue(pParametro.Prestador.IdPrestador.ToString());
            //if (item == null && pParametro.Prestador.IdPrestador > 0)
            //    this.ddlPrestadores.Items.Add(new ListItem(pParametro.Prestador.ApellidoNombre, pParametro.Prestador.IdPrestador.ToString()));
            //this.ddlPrestadores.SelectedValue = pParametro.Prestador.IdPrestador == 0 ? string.Empty : pParametro.Prestador.IdPrestador.ToString();

            //item = this.ddlEspecializaciones.Items.FindByValue(pParametro.Especializacion.IdEspecializacion.ToString());
            //if (item == null && pParametro.Especializacion.IdEspecializacion > 0)
            //    this.ddlEspecializaciones.Items.Add(new ListItem(pParametro.Especializacion.Descripcion, pParametro.Especializacion.IdEspecializacion.ToString()));
            //this.ddlEspecializaciones.SelectedValue = pParametro.Especializacion.IdEspecializacion == 0 ? string.Empty : pParametro.Especializacion.IdEspecializacion.ToString();

            ////this.txtEspecializacion.Text = pParametro.Especializacion.Descripcion;
            //item = this.ddlObraSocial.Items.FindByValue(pParametro.ObraSocial.IdObraSocial.ToString());
            //if (item == null && pParametro.ObraSocial.IdObraSocial > 0)
            //    this.ddlObraSocial.Items.Add(new ListItem(pParametro.ObraSocial.Descripcion, pParametro.ObraSocial.IdObraSocial.ToString()));
            //this.ddlObraSocial.SelectedValue = pParametro.ObraSocial.IdObraSocial == 0 ? string.Empty : pParametro.ObraSocial.IdObraSocial.ToString();

            //item = this.ddlNomenclador.Items.FindByValue(pParametro.Nomenclador.IdNomenclador.ToString());
            //if (item == null && pParametro.Nomenclador.IdNomenclador > 0)
            //    this.ddlNomenclador.Items.Add(new ListItem(pParametro.Nomenclador.Prestacion, pParametro.Nomenclador.IdNomenclador.ToString()));
            //this.ddlNomenclador.SelectedValue = pParametro.Nomenclador.IdNomenclador == 0 ? string.Empty : pParametro.Nomenclador.IdNomenclador.ToString();

            //this.txtObservaciones.Text = pParametro.Observaciones;
            //this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
            //this.ctrAuditoria.IniciarControl(pParametro);
            this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            this.ctrArchivos.IniciarControl(pParametro, this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiHistoriaClinica, new Objeto(), this.GestionControl);
        }

        /// <summary>
        /// Mapea los controles de Pantalla con la Entidad SolicitudesMateriales.
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearControlesAObjeto(MedHistoriasClinicas pParametro)
        {
            //pParametro.Fecha = Convert.ToDateTime(this.txtFecha.Text);
            //pParametro.ObraSocial.IdObraSocial = this.ddlObraSocial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlObraSocial.SelectedValue);
            //pParametro.ObraSocial.Descripcion = this.ddlObraSocial.SelectedItem.Text;
            //pParametro.Nomenclador.IdNomenclador = this.ddlNomenclador.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlNomenclador.SelectedValue);
            //pParametro.Nomenclador.Prestacion = this.ddlNomenclador.SelectedItem.Text;
            //pParametro.Observaciones = this.txtObservaciones.Text;
            //pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //pParametro.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;

            pParametro.Comentarios = ctrComentarios.ObtenerLista();
            pParametro.Archivos = ctrArchivos.ObtenerLista();
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
        }

        /// <summary>
        /// Llena los DropDownList de la Pantalla con los datos
        /// </summary>
        private void CargarCombos()
        {
            //this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestaciones));
            //this.ddlEstados.DataValueField = "IdEstado";
            //this.ddlEstados.DataTextField = "Descripcion";
            //this.ddlEstados.DataBind();

            //this.ddlNomenclador.DataSource = MedicinaF.NomencladoresObtenerListaFiltro(new MedNomencladores());
            //this.ddlNomenclador.DataValueField = "IdNomencladaor";
            //this.ddlNomenclador.DataTextField = "Prestacion";
            //this.ddlNomenclador.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlNomenclador, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //this.ddlObraSocial.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.ObrasSociales);
            //this.ddlObraSocial.DataValueField = "IdListaValorDetalle";
            //this.ddlObraSocial.DataTextField = "Descripcion";
            //this.ddlObraSocial.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlObraSocial, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //MedPrestadores filtro = new MedPrestadores();
            //filtro.Estado.IdEstado = (int)Estados.Activo;
            //this.ddlPrestadores.DataSource = MedicinaF.PrestadoresObtenerListaFiltro(filtro); ;
            //this.ddlPrestadores.DataValueField = "IdPrestador";
            //this.ddlPrestadores.DataTextField = "ApellidoNombre";
            //this.ddlPrestadores.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlPrestadores, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecializaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.PersistirDatos();
            this.Page.Validate("PrestacionesModificarDatos");
            if (!this.Page.IsValid)
                return;

            bool guardo = false;

            this.MapearControlesAObjeto(this.MiHistoriaClinica);
            this.MiHistoriaClinica.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiHistoriaClinica.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
                    this.MiHistoriaClinica.FechaAlta = DateTime.Now;
                    guardo = MedicinaF.HistoriasClinicasAgregar(this.MiHistoriaClinica);
                    break;
                case Gestion.Modificar:
                    guardo = MedicinaF.HistoriasClinicasModificar(this.MiHistoriaClinica);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiHistoriaClinica.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiHistoriaClinica.CodigoMensaje, true, this.MiHistoriaClinica.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar();
        }

        private void CargarLista(MedPrestaciones pPrestacion)
        {
            //pPrestacion.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            //pPrestacion.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            //pPrestacion.Apellido = this.txtApellido.Text.Trim();
            //pPrestacion.Nombre = this.txtNombre.Text.Trim();
            //pPrestacion.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pPrestacion.Afiliado.IdAfiliado = this.MiHistoriaClinica.IdAfiliado;
            pPrestacion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<MedPrestaciones>(pPrestacion);
            this.MisPrestaciones = MedicinaF.PrestacionesObtenerListaFiltroPacientes(pPrestacion);
            this.gvDatos.PageIndex = pPrestacion.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<MedPrestaciones>(this.MisPrestaciones, false, this.gvDatos, true);
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

        //protected void ddlPrestadores_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.ddlPrestadores.SelectedValue))
        //    {
        //        this.MiHistoriaClinica.Prestador.IdPrestador = Convert.ToInt32(this.ddlPrestadores.SelectedValue);
        //        this.MiHistoriaClinica.Prestador = MedicinaF.PrestadoresObtenerDatosCompletos(this.MiHistoriaClinica.Prestador);

        //        this.ddlEspecializaciones.DataSource = this.MiHistoriaClinica.Prestador.ObtenerEspecializaciones();
        //        this.ddlEspecializaciones.DataValueField = "IdEspecializacion";
        //        this.ddlEspecializaciones.DataTextField = "Descripcion";
        //        this.ddlEspecializaciones.DataBind();
        //    }
        //    else
        //    {
        //        this.ddlEspecializaciones.SelectedIndex = -1;
        //        this.ddlEspecializaciones.SelectedValue = null;
        //        this.ddlEspecializaciones.Items.Clear();
        //        this.ddlEspecializaciones.ClearSelection();
        //    }
        //    AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecializaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        //}
    }
}