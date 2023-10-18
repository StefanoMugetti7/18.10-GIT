using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using Medicina;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Afiliados.Entidades;
using Afiliados;
using System.Collections;

namespace IU.Modulos.Medicina.Controles
{
    public partial class PrestacionesModificarDatos : ControlesSeguros
    {
        private MedPrestaciones MiPrestacion
        {
            get { return (MedPrestaciones)Session[this.MiSessionPagina + "PrestacionesModificarDatosMiPrestacion"]; }
            set { Session[this.MiSessionPagina + "PrestacionesModificarDatosMiPrestacion"] = value; }
        }
        private List<MedTurnos> MisTurnos
        {
            get { return (List<MedTurnos>)Session[this.MiSessionPagina + "PrestacionesModificarDatosMisTurnos"]; }
            set { Session[this.MiSessionPagina + "PrestacionesModificarDatosMisTurnos"] = value; }
        }
        private AfiPacientes MiPaciente
        {
            get { return (AfiPacientes)Session[this.MiSessionPagina + "PacientesPrestacionesModificarDatos"]; }
            set { Session[this.MiSessionPagina + "PacientesPrestacionesModificarDatos"] = value; }
        }
        public delegate void PrestacionesModificarDatosAceptarEventHandler(MedPrestaciones e);
        public event PrestacionesModificarDatosAceptarEventHandler PrestacionesModificarDatosAceptar;
        public delegate void PrestacionesModificarDatosCancelarEventHandler();
        public event PrestacionesModificarDatosCancelarEventHandler PrestacionesModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack) { }
        }
        /// <summary>
        /// Inicializa el control de Alta y Modificación de un Afiliado
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(MedPrestaciones pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiPrestacion = pParametro;
            this.btnAceptar.Visible = true;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.btnBuscar.Visible = true;
                    if (MisParametrosUrl.Contains("IdAfiliado"))
                    {
                        this.MiPaciente = new AfiPacientes();
                        this.MiPaciente.IdAfiliado = Convert.ToInt32(MisParametrosUrl["IdAfiliado"].ToString());
                        this.MiPaciente = AfiliadosF.PacientesObtenerDatosCompletos(MiPaciente);
                        this.ddlApellido.Items.Add(new ListItem(MiPaciente.Apellido, MiPaciente.IdAfiliado.ToString()));
                        this.ddlApellido.SelectedValue = MiPaciente.IdAfiliado.ToString();
                        this.hdfIdAfiliado.Value = MiPaciente.IdAfiliado.ToString();
                        this.hdfIdAfiliadoReferrer.Value = MisParametrosUrl["IdAfiliado"].ToString();
                        this.ddlApellido.Enabled = false;
                        this.ddlTipoDocumento.SelectedValue = MiPaciente.TipoDocumento.IdTipoDocumento.ToString();
                        this.txtNumeroDocumento.Text = MiPaciente.NumeroSocio;
                        this.txtEstadoPaciente.Text = MiPaciente.Estado.Descripcion;
                        this.txtNombre.Text = MiPaciente.Nombre;
                        this.txtFechaNacimiento.Text = MiPaciente.FechaNacimiento.ToString().Substring(0, 10);
                        if (MiPrestacion.Turno.IdTurno > 0)
                        {
                            AyudaProgramacion.MatchObjectProperties(MiPaciente, MiPrestacion.Afiliado);
                            this.MapearObjetoAControles(this.MiPrestacion);
                        }
                    }
                    this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ddlEstados.Enabled = false;
                    this.txtFecha.Text = DateTime.Now.ToShortDateString();
                    this.txtFecha.Enabled = true;
                    this.MiPrestacion.Estado.IdEstado = (int)EstadosPrestaciones.Activo;
                    this.ctrCamposValores.IniciarControl(this.MiPrestacion, new Objeto(), this.GestionControl);
                    this.ctrComentarios.IniciarControl(this.MiPrestacion, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiPrestacion, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.ddlEstados.Enabled = true;
                    this.ddlApellido.Enabled = false;
                    this.ddlPrestadores.Enabled = false;
                    this.ddlEspecializaciones.Enabled = false;
                    this.ddlObraSocial.Enabled = false;
                    this.ddlNomenclador.Enabled = false;
                    this.MiPrestacion = MedicinaF.PrestacionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiPrestacion);
                    break;
                case Gestion.Consultar:
                    this.MiPrestacion = MedicinaF.PrestacionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiPrestacion);
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
        private void MapearObjetoAControles(MedPrestaciones pParametro)
        {
            this.txtFecha.Text = pParametro.Fecha.ToShortDateString();
            this.hdfIdAfiliado.Value = pParametro.Afiliado.IdAfiliado.ToString();
            //this.txtFechaNacimiento.Text = AyudaProgramacion.MostrarFechaPantalla(pParametro.Afiliado.FechaNacimiento);
            //this.txtPrestador.Text = pParametro.Prestador.ApellidoNombre;
            ListItem item = this.ddlPrestadores.Items.FindByValue(pParametro.Prestador.IdPrestador.ToString());
            if (item == null && pParametro.Prestador.IdPrestador > 0)
                this.ddlPrestadores.Items.Add(new ListItem(pParametro.Prestador.ApellidoNombre, pParametro.Prestador.IdPrestador.ToString()));
            this.ddlPrestadores.SelectedValue = pParametro.Prestador.IdPrestador == 0 ? string.Empty : pParametro.Prestador.IdPrestador.ToString();
            
            item = this.ddlApellido.Items.FindByValue(pParametro.Afiliado.ApellidoNombre.ToString());
            if (item == null && pParametro.Afiliado.IdAfiliado > 0)
                this.ddlApellido.Items.Add(new ListItem(pParametro.Afiliado.ApellidoNombre.ToString()));
            this.ddlApellido.SelectedValue = pParametro.Afiliado.ApellidoNombre.ToString();
            item = this.ddlEspecializaciones.Items.FindByValue(pParametro.Especializacion.IdEspecializacion.ToString());
            if (item == null && pParametro.Especializacion.IdEspecializacion > 0)
                this.ddlEspecializaciones.Items.Add(new ListItem(pParametro.Especializacion.Descripcion, pParametro.Especializacion.IdEspecializacion.ToString()));
            this.ddlEspecializaciones.SelectedValue = pParametro.Especializacion.IdEspecializacion.ToString();

            if (pParametro.Especializacion.IdEspecializacion > 0) {
                ddlEspecializaciones_SelectedIndexChanged(null, EventArgs.Empty);
            }

            item = this.ddlTipoDocumento.Items.FindByValue(pParametro.Afiliado.TipoDocumento.IdTipoDocumento.ToString());
            if (item == null && pParametro.Afiliado.TipoDocumento.IdTipoDocumento > 0)
                this.ddlTipoDocumento.Items.Add(new ListItem(pParametro.Afiliado.TipoDocumento.TipoDocumento, pParametro.Afiliado.TipoDocumento.IdTipoDocumento.ToString()));
            this.ddlTipoDocumento.SelectedValue = pParametro.Afiliado.TipoDocumento.IdTipoDocumento.ToString();
            item = this.ddlTurnos.Items.FindByValue(pParametro.Turno.IdTurno.ToString());
            if (item == null && pParametro.Turno.IdTurno > 0)
                this.ddlTurnos.Items.Add(new ListItem(pParametro.Turno.DescripcionCombo, pParametro.Turno.IdTurno.ToString()));
            this.ddlTurnos.SelectedValue = pParametro.Turno.IdTurno == 0 ? string.Empty : pParametro.Turno.IdTurno.ToString();
            if (string.IsNullOrEmpty(ddlTurnos.SelectedValue))
            {
                ddlTurnos.Enabled = false;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTurnos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            //this.txtEspecializacion.Text = pParametro.Especializacion.Descripcion;
            item = this.ddlObraSocial.Items.FindByValue(pParametro.ObraSocial.IdObraSocial.ToString());
            if (item == null && pParametro.ObraSocial.IdObraSocial > 0)
                this.ddlObraSocial.Items.Add(new ListItem(pParametro.ObraSocial.Descripcion, pParametro.ObraSocial.IdObraSocial.ToString()));
            this.ddlObraSocial.SelectedValue = pParametro.ObraSocial.IdObraSocial == 0 ? string.Empty : pParametro.ObraSocial.IdObraSocial.ToString();
            item = this.ddlNomenclador.Items.FindByValue(pParametro.Nomenclador.IdNomenclador.ToString());
            if (item == null && pParametro.Nomenclador.IdNomenclador > 0)
                this.ddlNomenclador.Items.Add(new ListItem(pParametro.Nomenclador.Prestacion, pParametro.Nomenclador.IdNomenclador.ToString()));
            this.ddlNomenclador.SelectedValue = pParametro.Nomenclador.IdNomenclador == 0 ? string.Empty : pParametro.Nomenclador.IdNomenclador.ToString();
            this.txtObservaciones.Text = pParametro.Observaciones;
            if (pParametro.Estado.IdEstado == 38)
                this.ddlEstados.SelectedValue = 1.ToString();
            else
                this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            this.ctrArchivos.IniciarControl(pParametro, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pParametro);
            this.ctrCamposValores.IniciarControl(this.MiPrestacion, new Objeto(), this.GestionControl);
            this.ddlTipoDocumento.SelectedValue = pParametro.Afiliado.TipoDocumento.TipoDocumento.ToString();
            this.txtNumeroDocumento.Text = pParametro.Afiliado.NumeroDocumento.ToString();
            this.ctrCamposValores.IniciarControl(this.MiPrestacion, new Objeto(), this.GestionControl);
        }
        /// <summary>
        /// Mapea los controles de Pantalla con la Entidad SolicitudesMateriales.
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearControlesAObjeto(MedPrestaciones pParametro)
        {
            pParametro.Prestador.IdPrestador = this.ddlPrestadores.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPrestadores.SelectedValue);
            pParametro.Especializacion.IdEspecializacion = this.ddlEspecializaciones.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEspecializaciones.SelectedValue);
            pParametro.Turno.IdTurno = this.ddlTurnos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTurnos.SelectedValue);
            pParametro.Afiliado.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            pParametro.Fecha = this.txtFecha.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFecha.Text);
            pParametro.ObraSocial.IdObraSocial = this.ddlObraSocial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlObraSocial.SelectedValue);
            pParametro.ObraSocial.Descripcion = this.ddlObraSocial.SelectedItem.Text;
            pParametro.Nomenclador.IdNomenclador = this.ddlNomenclador.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlNomenclador.SelectedValue);
            pParametro.Nomenclador.Prestacion = this.ddlNomenclador.SelectedItem.Text;
            pParametro.Observaciones = this.txtObservaciones.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pParametro.Comentarios = this.ctrComentarios.ObtenerLista();
            pParametro.Archivos = this.ctrArchivos.ObtenerLista();
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
            this.ctrCamposValores.IniciarControl(this.MiPrestacion, new Objeto(), this.GestionControl);
        }
        /// <summary>
        /// Llena los DropDownList de la Pantalla con los datos
        /// </summary>
        private void CargarCombos()
        {
            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoDocumento, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestaciones));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            //this.ddlNomenclador.DataSource = MedicinaF.NomencladoresObtenerListaFiltro(new MedNomencladores());
            //this.ddlNomenclador.DataValueField = "IdNomenclador";
            //this.ddlNomenclador.DataTextField = "Prestacion";
            //this.ddlNomenclador.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlNomenclador, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlObraSocial.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.ObrasSociales);
            this.ddlObraSocial.DataValueField = "IdListaValorDetalle";
            this.ddlObraSocial.DataTextField = "Descripcion";
            this.ddlObraSocial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlObraSocial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            MedPrestadores filtro = new MedPrestadores();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            this.ddlPrestadores.DataSource = MedicinaF.PrestadoresObtenerListaFiltro(filtro); ;
            this.ddlPrestadores.DataValueField = "IdPrestador";
            this.ddlPrestadores.DataTextField = "ApellidoNombre";
            this.ddlPrestadores.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrestadores, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTurnos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecializaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.PersistirDatos();
            this.Page.Validate("PrestacionesModificarDatos");
            if (!this.Page.IsValid)
                return;
            this.btnAceptar.Visible = false;
            bool guardo = false;
            this.MapearControlesAObjeto(this.MiPrestacion);
            this.MiPrestacion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPrestacion.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
                    this.MiPrestacion.FechaAlta = DateTime.Now;
                    guardo = MedicinaF.PrestacionesAgregar(this.MiPrestacion);
                    break;
                case Gestion.Modificar:
                    guardo = MedicinaF.PrestacionesModificar(this.MiPrestacion);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.MisParametrosUrl.Remove("IdAfiliado");
                this.MostrarMensaje(this.MiPrestacion.CodigoMensaje, false, this.MiPrestacion.CodigoMensajeArgs);
            }
            else
            {
                this.MostrarMensaje(this.MiPrestacion.CodigoMensaje, true, this.MiPrestacion.CodigoMensajeArgs);
                btnAceptar.Visible = true;
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl.Remove("IdAfiliado");
            if (this.PrestacionesModificarDatosCancelar != null)
                this.PrestacionesModificarDatosCancelar();
        }
        protected void button_Click(object sender, EventArgs e)
        {
            MedTurnos filtro = new MedTurnos();
            if (!string.IsNullOrWhiteSpace(this.hdfIdAfiliado.Value))
            {
                filtro.Afiliado.IdAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
                this.MiPaciente = new AfiPacientes();
                this.MiPaciente.IdAfiliado = Convert.ToInt32(hdfIdAfiliado.Value);
                this.MiPaciente = AfiliadosF.PacientesObtenerDatosCompletos(MiPaciente);
                this.ddlApellido.Items.Add(new ListItem(MiPaciente.RazonSocial, MiPaciente.IdAfiliado.ToString()));
                this.ddlApellido.SelectedValue = MiPaciente.IdAfiliado.ToString();
                //filtro.ObraSocial.IdObraSocial = Convert.ToInt32(this.ddlObraSocial.SelectedValue);
                //filtro.IdTurno  filtro.Estado.IdEstado = (int)EstadosTurnos.Reservado; = Convert.ToInt32(this.ddlTurnos.SelectedValue);
                filtro.Estado.IdEstado = (int)EstadosTurnos.Reservado;
                this.MisTurnos = MedicinaF.TurnosObtenerListaFiltro(filtro);
                //llenar el combo de Turno. Turno txt cambio por ddl
                this.ddlTurnos.DataSource = this.MisTurnos;
                this.ddlTurnos.DataValueField = "IdTurno";
                this.ddlTurnos.DataTextField = "DescripcionCombo";
                this.ddlTurnos.DataBind();
                if (ddlTurnos.Items.Count != 1)
                {
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTurnos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    if (MiPaciente.ObraSocial.IdListaValorDetalle > 0)
                        this.ddlObraSocial.SelectedValue = this.MiPaciente.ObraSocial.IdListaValorDetalle.ToString();
                    else
                        this.ddlObraSocial.SelectedValue = "";
                }
                else
                    ddlTurnos_SelectedIndexChanged(ddlTurnos, EventArgs.Empty);
            }
            else
            {
                this.ddlTurnos.Items.Clear();
                //this.ddlTurnos.SelectedIndex = 62;
                this.ddlTurnos.SelectedValue = null;
                this.ddlTurnos.ClearSelection();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTurnos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        protected void ddlTurnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTurnos.SelectedValue))
            {
                MedTurnos t = MisTurnos.First(x => x.IdTurno == Convert.ToInt32(this.ddlTurnos.SelectedValue));
                //this.ddlObraSocial.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.ObrasSociales);
                //this.ddlObraSocial.DataValueField = "IdListaValorSistemaDetalle";
                //this.ddlObraSocial.DataTextField = "Descripcion";
                //this.ddlObraSocial.DataBind();
                if (t.ObraSocial.IdObraSocial > 0)
                {
                    this.ddlObraSocial.SelectedValue = t.ObraSocial.IdObraSocial.ToString();
                    this.ddlObraSocial.Enabled = false;
                }
                else
                {
                    if (MiPaciente.ObraSocial.IdListaValorDetalle > 0)
                        this.ddlObraSocial.SelectedValue = this.MiPaciente.ObraSocial.IdListaValorDetalle.ToString();
                }
                this.ddlPrestadores.SelectedValue = t.Prestador.IdPrestador.ToString();
                this.ddlPrestadores.Enabled = false;
                this.ddlEspecializaciones.Items.Clear();
                this.ddlEspecializaciones.SelectedIndex = -1;
                this.ddlEspecializaciones.SelectedValue = null;
                this.ddlEspecializaciones.ClearSelection();
                this.ddlEspecializaciones.Items.Add(new ListItem(t.Especializacion.Descripcion, t.Especializacion.IdEspecializacion.ToString()));
                this.ddlEspecializaciones.SelectedValue = t.Especializacion.IdEspecializacion.ToString();
                this.ddlEspecializaciones.Enabled = false;
                this.MiPrestacion.Turno = t;
            }
            else
            {
                this.ddlObraSocial.Enabled = true;
                this.ddlObraSocial.SelectedValue = string.Empty;
                this.ddlTurnos.Enabled = true;
                this.ddlTurnos.SelectedValue = string.Empty;
                this.ddlPrestadores.Enabled = true;
                this.ddlPrestadores.SelectedValue = string.Empty;
                this.ddlEspecializaciones.SelectedIndex = -1;
                this.ddlEspecializaciones.SelectedValue = null;
                this.ddlEspecializaciones.Items.Clear();
                this.ddlEspecializaciones.ClearSelection();
                this.ddlEspecializaciones.Enabled = true;
            }
        }
        protected void ddlPrestadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlPrestadores.SelectedValue))
            {
                this.MiPrestacion.Prestador.IdPrestador = Convert.ToInt32(this.ddlPrestadores.SelectedValue);
                this.MiPrestacion.Prestador = MedicinaF.PrestadoresObtenerDatosCompletos(this.MiPrestacion.Prestador);
                this.ddlEspecializaciones.DataSource = this.MiPrestacion.Prestador.ObtenerEspecializaciones();
                this.ddlEspecializaciones.DataValueField = "IdEspecializacion";
                this.ddlEspecializaciones.DataTextField = "Descripcion";
                this.ddlEspecializaciones.DataBind();
            } else {
                this.ddlEspecializaciones.SelectedIndex = -1;
                this.ddlEspecializaciones.SelectedValue = null;
                this.ddlEspecializaciones.Items.Clear();
                this.ddlEspecializaciones.ClearSelection();
            }
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecializaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlEspecializaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlNomenclador.Items.Clear();
            this.ddlNomenclador.SelectedIndex = -1;
            this.ddlNomenclador.SelectedValue = null;
            this.ddlNomenclador.ClearSelection();

            if (!string.IsNullOrEmpty(this.ddlEspecializaciones.SelectedValue))
            {
                MedNomencladores nom = new MedNomencladores();
                nom.IdEspecializacion = Convert.ToInt32(this.ddlEspecializaciones.SelectedValue);
                List<MedNomencladores> lista = new List<MedNomencladores>();
                lista = MedicinaF.NomencladoresObtenerListaCombo(nom);

                this.ddlNomenclador.DataSource = lista;
                this.ddlNomenclador.DataValueField = "IdNomenclador";
                this.ddlNomenclador.DataTextField = "Descripcion";
                this.ddlNomenclador.DataBind();
            }
            AyudaProgramacion.AgregarItemSeleccione(this.ddlNomenclador, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
    }
}