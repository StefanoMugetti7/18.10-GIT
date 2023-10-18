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
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Cargos;
using ProcesosDatos;
using ProcesosDatos.Entidades;

namespace IU.Modulos.TGE.Control
{
    public partial class FormasCobrosDatos : ControlesSeguros
    {
        private TGEFormasCobros MiFormaCobro
        {
            get { return (TGEFormasCobros)Session[this.MiSessionPagina + "FormasCobrosDatosMiFormaCobro"]; }
            set { Session[this.MiSessionPagina + "FormasCobrosDatosMiFormaCobro"] = value; }
        }

        public delegate void FormasCobrosAceptarEventHandler();
        public event FormasCobrosAceptarEventHandler FormasCobrosModificarDatosAceptar;
        public delegate void FormasCobrosCancelarEventHandler();
        public event FormasCobrosCancelarEventHandler FormasCobrosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
        }

        public void IniciarControl(TGEFormasCobros pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiFormaCobro = pParametro;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    if (this.UsuarioActivo.EsAdministradorGeneral)
                    {
                        this.ddlProceso.Enabled = true;
                        txtCodigoFormaCobro.Enabled = true;
                    }
                    this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ctrCamposValores.IniciarControl(this.MiFormaCobro, new Objeto(), this.MiFormaCobro.IdFormaCobro, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiFormaCobro, this.MiFormaCobro, this.MiFormaCobro.IdFormaCobro, this.GestionControl);

                    break;
                case Gestion.Modificar:
                    if (this.UsuarioActivo.EsAdministradorGeneral)
                    {
                        this.ddlProceso.Enabled = true;
                        txtCodigoFormaCobro.Enabled = true;
                    }
                    this.MiFormaCobro = TGEGeneralesF.FormasCobrosObtenerDatosCompletos(this.MiFormaCobro);
                    this.MapearObjetoAControles(this.MiFormaCobro);
                    break;
                case Gestion.Consultar:
                    this.MiFormaCobro = TGEGeneralesF.FormasCobrosObtenerDatosCompletos(this.MiFormaCobro);
                    this.MapearObjetoAControles(this.MiFormaCobro);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosFormasCobrosAfiliados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();


            SisProcesos proceso = new SisProcesos();
            proceso.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlProceso.DataSource = ProcesosDatosF.ProcesosObtenerListaFiltro(proceso);
            this.ddlProceso.DataValueField = "IdProceso";
            this.ddlProceso.DataTextField = "Descripcion";
            this.ddlProceso.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlProceso, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearControlesAObjeto(TGEFormasCobros pFormaCobro)
        {
            pFormaCobro.FormaCobro = this.txtFormaCobro.Text;
            pFormaCobro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pFormaCobro.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pFormaCobro.IdProceso = ddlProceso.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlProceso.SelectedValue);
            pFormaCobro.Campos = this.ctrCamposValores.ObtenerLista();
            pFormaCobro.CodigoFormaCobro = txtCodigoFormaCobro.Text;
        }

        private void MapearObjetoAControles(TGEFormasCobros pFormaCobro)
        {
            this.txtFormaCobro.Text = pFormaCobro.FormaCobro;
            this.ddlEstados.SelectedValue = pFormaCobro.Estado.IdEstado.ToString();
            this.ddlProceso.SelectedValue = pFormaCobro.IdProceso.ToString();
            this.ctrCamposValores.IniciarControl(this.MiFormaCobro, new Objeto(), this.MiFormaCobro.IdFormaCobro, this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiFormaCobro, this.MiFormaCobro, this.MiFormaCobro.IdFormaCobro, this.GestionControl);
            this.pnlCamposValores.Visible = this.ctrCamposValores.MostrarControl;
            txtCodigoFormaCobro.Text = pFormaCobro.CodigoFormaCobro.ToString();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiFormaCobro);
            this.MiFormaCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, this.MiFormaCobro.UsuarioAlta);
                    this.MiFormaCobro.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.FormasCobrosAgregar(this.MiFormaCobro);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.FormasCobrosModificar(this.MiFormaCobro);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFormaCobro.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiFormaCobro.CodigoMensaje, true, this.MiFormaCobro.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.FormasCobrosModificarDatosCancelar != null)
                this.FormasCobrosModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.FormasCobrosModificarDatosAceptar != null)
                this.FormasCobrosModificarDatosAceptar();
        }

    }
}