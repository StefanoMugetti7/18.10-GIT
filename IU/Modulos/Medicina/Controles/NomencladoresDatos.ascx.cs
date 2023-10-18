using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Medicina;
using Medicina.Entidades;
using System;
using System.Web.UI.WebControls;

namespace IU.Modulos.Medicina.Controles
{
    public partial class NomencladoresDatos : ControlesSeguros
    {
        public MedNomencladores MiNomenclador
        {
            get { return PropiedadObtenerValor<MedNomencladores>("NomencladorDatosMiNomenclador"); }
            set { PropiedadGuardarValor("NomencladorDatosMiNomenclador", value); }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!IsPostBack)
            {

            }
        }
        public void IniciarControl(MedNomencladores pParametro, Gestion pGestion)
        {
            this.MiNomenclador = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Modificar:
                    this.MiNomenclador = MedicinaF.NomencladoresObtenerDatosCompletos(this.MiNomenclador);
                    this.MapearObjetoAControles(this.MiNomenclador);
                    break;
                case Gestion.Consultar:
                    this.MiNomenclador = MedicinaF.NomencladoresObtenerDatosCompletos(this.MiNomenclador);
                    this.MapearObjetoAControles(this.MiNomenclador);
                    this.txtPrestacion.Enabled = false;
                    this.txtCodigo.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.ddlEspecializacion.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("MedNomencladores");
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, ObtenerMensajeSistema("SeleccioneOpcion"));

            MedNomencladores nom = new MedNomencladores();
            nom.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlEspecializacion.DataSource = MedicinaF.NomencladoresObtenerEspecializaciones(nom);
            this.ddlEspecializacion.DataValueField = "IdEspecializacion";
            this.ddlEspecializacion.DataTextField = "Especializacion";
            this.ddlEspecializacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecializacion, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(MedNomencladores pParametro)
        {
            this.txtCodigo.Text = pParametro.Codigo;
            this.txtPrestacion.Text = pParametro.Prestacion;

            var item = this.ddlEstado.Items.FindByValue(pParametro.Estado.IdEstado.ToString());
            if (item == null)
                this.ddlEstado.Items.Add(new ListItem(pParametro.Estado.Descripcion, pParametro.Estado.IdEstado.ToString()));
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();

            var item2 = this.ddlEspecializacion.Items.FindByValue(pParametro.IdEspecializacion.ToString());
            if (item2 == null)
                this.ddlEspecializacion.Items.Add(new ListItem(pParametro.Especializacion, pParametro.IdEspecializacion.ToString()));
            this.ddlEspecializacion.SelectedValue = pParametro.IdEspecializacion.ToString();
        }
        private void MapearControlesAObjeto(MedNomencladores pParametro)
        {
            pParametro.Codigo = this.txtCodigo.Text;
            pParametro.Prestacion = this.txtPrestacion.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.IdEspecializacion = Convert.ToInt32(this.ddlEspecializacion.SelectedValue);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiNomenclador);

            this.MiNomenclador.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = MedicinaF.NomencladoresAgregar(this.MiNomenclador);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiNomenclador.CodigoMensaje, false, this.MiNomenclador.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = MedicinaF.NomencladoresModificar(this.MiNomenclador);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiNomenclador.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiNomenclador.CodigoMensaje, true, this.MiNomenclador.CodigoMensajeArgs);
                if (this.MiNomenclador.dsResultado != null)
                {
                    this.MiNomenclador.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }
    }
}