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
using Reportes.FachadaNegocio;
using System.Data;
using System.IO;
using Comunes.LogicaNegocio;

namespace IU.Modulos.Medicina.Controles
{
    public partial class EstudiosDatos : ControlesSeguros
    {
        private MedEstudios MiEstudio
        {
            get { return (MedEstudios)Session[this.MiSessionPagina + "EstudiosDatosMiEstudio"]; }
            set { Session[this.MiSessionPagina + "EstudiosDatosMiEstudio"] = value; }
        }
        private AfiPacientes MiPaciente
        {
            get { return (AfiPacientes)Session[this.MiSessionPagina + "PacientesEstudiosDatos"]; }
            set { Session[this.MiSessionPagina + "PacientesEstudiosDatos"] = value; }
        }

        public delegate void ModificarDatosCancelarEventHandler();
        public event ModificarDatosCancelarEventHandler ModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
            }
        }

        public void IniciarControl(MedEstudios pParametro, Gestion pGestion)
        {
            MiEstudio = pParametro;
            MiPaciente = new AfiPacientes();
            MiPaciente.IdAfiliado = 0;
            GestionControl = pGestion; 
            btnAceptar.Visible = true;
            CargarCombos();

            if (MisParametrosUrl.Contains("IdAfiliado"))
            {
                MiPaciente = new AfiPacientes();

                MiPaciente.IdAfiliado = Convert.ToInt32(MisParametrosUrl["IdAfiliado"].ToString());
                MiPaciente = AfiliadosF.PacientesObtenerDatos(MiPaciente);
                AyudaProgramacion.MatchObjectProperties(MiPaciente, MiEstudio.Afiliado);
                ddlApellido.Items.Add(new ListItem(MiPaciente.Apellido, MiPaciente.IdAfiliado.ToString()));
                ddlApellido.SelectedValue = MiPaciente.IdAfiliado.ToString();
                hdfIdAfiliado.Value = MiPaciente.IdAfiliado.ToString();
                hdfIdAfiliadoReferrer.Value = MisParametrosUrl["IdAfiliado"].ToString();

                ddlTipoDocumento.SelectedValue = MiPaciente.TipoDocumento.IdTipoDocumento.ToString();
                txtNumeroDocumento.Text = MiPaciente.NumeroDocumento.ToString();
                txtEstadoPaciente.Text = MiPaciente.Estado.Descripcion;
                txtNombre.Text = MiPaciente.Nombre;
                txtFechaNacimiento.Text = MiPaciente.FechaNacimiento.ToString();
            }

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //hdfEditor.Value = MedicinaF.EstudiosObtenerPlantilla(MiEstudio);
                    CKEditor1.Text = MedicinaF.EstudiosObtenerPlantilla(MiEstudio);
                    ctrArchivos.IniciarControl(pParametro, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    btnImprimir.Visible = true;
                    this.MiEstudio = MedicinaF.EstudiosObtenerDatosCompletos(MiEstudio);
                    this.MapearObjetoAControles(this.MiEstudio);
                    break;
                case Gestion.Consultar:
                    btnAceptar.Visible = false;
                    btnImprimir.Visible = true;
                    txtFechaEstudio.Enabled = false;
                    ddlApellido.Enabled = false;
                    ddlEstados.Enabled = false;
                    ddlTipoEstudio.Enabled = false;
                    ddlPrestadores.Enabled = false;
                    txtFechaEstudio.Enabled = false;
                    ddlPrestadores.Enabled = false;

                    this.MiEstudio = MedicinaF.EstudiosObtenerDatosCompletos(MiEstudio);
                    this.MapearObjetoAControles(this.MiEstudio);
                    break;
                case Gestion.Anular:
                    btnAceptar.Visible = true;
                    btnImprimir.Visible = false;
                    txtFechaEstudio.Enabled = false;
                    ddlApellido.Enabled = false;
                    ddlEstados.Enabled = false;
                    ddlTipoEstudio.Enabled = false;
                    ddlPrestadores.Enabled = false;
                    txtFechaEstudio.Enabled = false;
                    ddlPrestadores.Enabled = false;
                    

                    this.MiEstudio = MedicinaF.EstudiosObtenerDatosCompletos(MiEstudio);
                    this.MapearObjetoAControles(this.MiEstudio);
                    break;
                default:
                    break;
                 
            }
        }

        private void MapearObjetoAControles(MedEstudios pParametro)
        {
            ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
            ddlTipoEstudio.SelectedValue = pParametro.IdTipoEstudio.ToString();
            ddlPrestadores.SelectedValue = pParametro.Prestador.IdPrestador.ToString();
            txtFechaEstudio.Text = pParametro.FechaEstudio.ToShortDateString();
            //hdfEditor.Value = pParametro.InformeEstudio;
            CKEditor1.Text = pParametro.InformeEstudio;
            ctrArchivos.IniciarControl(pParametro, this.GestionControl);
        }

        private void MapearControlesAObjeto(MedEstudios pParametro)
        {
            pParametro.Afiliado.IdAfiliado = MiPaciente.IdAfiliado;
            pParametro.Estado.IdEstado = Convert.ToInt32(ddlEstados.SelectedValue);
            pParametro.IdTipoEstudio = ddlTipoEstudio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoEstudio.SelectedValue);
            pParametro.Prestador.IdPrestador = ddlPrestadores.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlPrestadores.SelectedValue);
            pParametro.FechaEstudio = Convert.ToDateTime(txtFechaEstudio.Text);
            pParametro.Archivos = ctrArchivos.ObtenerLista();
            //pParametro.InformeEstudio = hdfEditor.Value;
            pParametro.InformeEstudio = CKEditor1.Text;
            pParametro.FechaAlta = DateTime.Now;
            ctrArchivos.IniciarControl(pParametro, this.GestionControl);
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

            MedPrestadores filtro = new MedPrestadores();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            this.ddlPrestadores.DataSource = MedicinaF.PrestadoresObtenerListaFiltro(filtro); ;
            this.ddlPrestadores.DataValueField = "IdPrestador";
            this.ddlPrestadores.DataTextField = "ApellidoNombre";
            this.ddlPrestadores.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrestadores, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestaciones));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            this.ddlTipoEstudio.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposEstudios);
            this.ddlTipoEstudio.DataValueField = "IdListaValorDetalle";
            this.ddlTipoEstudio.DataTextField = "Descripcion";
            this.ddlTipoEstudio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoEstudio, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            
        }

        protected void ddlTipoEstudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlTipoEstudio.SelectedValue))
            {
                string rem = MiEstudio.TipoEstudio.Trim() == string.Empty ? "{TipoEstudio}" : MiEstudio.TipoEstudio;
                MiEstudio.IdTipoEstudio = Convert.ToInt32(ddlTipoEstudio.SelectedValue);
                MiEstudio.TipoEstudio = ddlTipoEstudio.SelectedItem.Text;
                //hdfEditor.Value = hdfEditor.Value.Replace(rem, MiEstudio.TipoEstudio);
                CKEditor1.Text = CKEditor1.Text.Replace(rem, MiEstudio.TipoEstudio);
                //EditorSetData
                //ScriptManager.RegisterStartupScript(this.upEstudioMedico, this.upEstudioMedico.GetType(), "EditorSetDataScript", "EditorSetData();", true);
            }
        }

        protected void ddlPrestadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlPrestadores.SelectedValue))
            {
                System.Drawing.Bitmap img;
                string base64String;
                string imgTag;
                string rem;

                rem = "{Firma}";
                if (CKEditor1.Text.IndexOf("{Firma}") == -1)
                {
                    MiEstudio.Prestador = MedicinaF.PrestadoresObtenerDatosCompletos(MiEstudio.Prestador);

                    if (MiEstudio.Prestador.Firma != null)
                    {
                        img = new System.Drawing.Bitmap(new MemoryStream(MiEstudio.Prestador.Firma));
                        base64String = Convert.ToBase64String((byte[])new System.Drawing.ImageConverter().ConvertTo(img, typeof(byte[])));
 
                        imgTag = "<img alt=" + '"' + '"' + " src=" + '"' + "data:image/png;base64," + base64String + '"' + " />";

                        rem = imgTag;
                        int index = CKEditor1.Text.IndexOf(imgTag);
                    }
                }

                MiEstudio.Prestador.IdPrestador = Convert.ToInt32(ddlPrestadores.SelectedValue);
                MiEstudio.Prestador.ApellidoNombre = ddlPrestadores.SelectedItem.Text;
                MiEstudio.Prestador = MedicinaF.PrestadoresObtenerDatosCompletos(MiEstudio.Prestador);

                if (MiEstudio.Prestador.Firma != null)
                {
                    img = new System.Drawing.Bitmap(new MemoryStream(MiEstudio.Prestador.Firma));
                    base64String = Convert.ToBase64String((byte[])new System.Drawing.ImageConverter().ConvertTo(img, typeof(byte[])));
                    if (base64String != string.Empty)
                    { 
                        imgTag = "<img alt =\"\" src=\"data:image/png;base64," + base64String + "\" />";
                        CKEditor1.Text = CKEditor1.Text.Replace(rem, imgTag);
                    }
                }
                else
                {
                    CKEditor1.Text = CKEditor1.Text.Replace(rem, "{Firma}");
                }
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("PrestacionesModificarDatos");
            if (!this.Page.IsValid)
                return;
            btnAceptar.Visible = false;
            bool guardo = true;

            this.MapearControlesAObjeto(this.MiEstudio);
            this.MiEstudio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiEstudio.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
                    this.MiEstudio.FechaAlta = DateTime.Now;
                    guardo = MedicinaF.EstudiosAgregar(this.MiEstudio);
                    break;
                case Gestion.Modificar:
                    guardo = MedicinaF.EstudiosModificar(this.MiEstudio);
                    btnAceptar.Visible = false;
                    break;
                case Gestion.Anular:
                    this.MiEstudio.Estado.IdEstado = (int)Estados.Baja;
                    this.MiEstudio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = MedicinaF.EstudiosAnular(MiEstudio);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiEstudio.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.MostrarMensaje(this.MiEstudio.CodigoMensaje, false, this.MiEstudio.CodigoMensajeArgs);
                btnImprimir.Visible = true;
            }
            else
            {
                this.MostrarMensaje(this.MiEstudio.CodigoMensaje, true, this.MiEstudio.CodigoMensajeArgs);
                btnAceptar.Visible = true;
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            if (string.IsNullOrEmpty(hdfIdAfiliadoReferrer.Value))
                this.MisParametrosUrl.Add("IdAfiliado", hdfIdAfiliadoReferrer.Value);

            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.ModificarDatosCancelar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.ModificarDatos_ModificarDatosCancelar();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                MiEstudio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                //DataSet ds = MedicinaF.EstudiosObtenerDataSetPdf(MiEstudio);
                //TGEPlantillas plantilla = new TGEPlantillas();
                //plantilla.HtmlPlantilla = ds.Tables[0].Rows[0]["InformeEstudio"].ToString();

                byte[] pdf = MedicinaF.EstudiosObtenerComprobante(MiEstudio);
                ExportPDF.ExportarPDF(pdf, this.upBotones, string.Concat("Estudio_", MiEstudio.IdEstudio.ToString().PadLeft(10, '0')), this.UsuarioActivo);

            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, true);
            }
        }

        
    }
}
