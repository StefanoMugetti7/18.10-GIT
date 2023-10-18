using Afiliados;
using Afiliados.Entidades;
using Cargos;
using Cargos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using System;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE.Control
{
    public partial class FormasCobrosCodigosConceptosTiposCargosCategorias : ControlesSeguros
    {
        private TGEFormasCobrosCodigosConceptosTiposCargosCategorias MiFormaCobroCodigoConcepto
        {
            get { return (TGEFormasCobrosCodigosConceptosTiposCargosCategorias)Session[this.MiSessionPagina + "MiFormaCobroCodigoConceptoCodigoConcepto"]; }
            set { Session[this.MiSessionPagina + "MiFormaCobroCodigoConceptoCodigoConcepto"] = value; }
        }

        public delegate void FormasCobrosAceptarEventHandler();
        public event FormasCobrosAceptarEventHandler FormasCobrosModificarDatosAceptar;
        public delegate void FormasCobrosCancelarEventHandler();
        public event FormasCobrosCancelarEventHandler FormasCobrosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MiFormaCobroCodigoConcepto == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        public void IniciarControl(TGEFormasCobrosCodigosConceptosTiposCargosCategorias pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiFormaCobroCodigoConcepto = pParametro;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                    break;
                case Gestion.Modificar:
                    this.MiFormaCobroCodigoConcepto = TGEGeneralesF.FormasCobrosCodigosConceptosTiposCargosCategoriasObtenerDatosCompletos(this.MiFormaCobroCodigoConcepto);
                    this.MapearObjetoAControles(this.MiFormaCobroCodigoConcepto);
                    break;
                case Gestion.Consultar:
                    this.MiFormaCobroCodigoConcepto = TGEGeneralesF.FormasCobrosCodigosConceptosTiposCargosCategoriasObtenerDatosCompletos(this.MiFormaCobroCodigoConcepto);
                    this.MapearObjetoAControles(this.MiFormaCobroCodigoConcepto);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            TGEFormasCobros fc = new TGEFormasCobros();
            fc.Estado.IdEstado = (int)Estados.Activo;
            this.ddlFormaCobro.DataSource = TGEGeneralesF.FormasCobrosObtenerListaFiltro(fc);
            this.ddlFormaCobro.DataValueField = "IdFormaCobro";
            this.ddlFormaCobro.DataTextField = "FormaCobro";
            this.ddlFormaCobro.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            CarTiposCargos tc = new CarTiposCargos();
            tc.Estado.IdEstado = (int)Estados.Activo;
            this.ddlTipoCargo.DataSource = CargosF.TiposCargosObtenerListaFiltro(tc);
            this.ddlTipoCargo.DataValueField = "IdTipoCargo";
            this.ddlTipoCargo.DataTextField = "TipoCargo";
            this.ddlTipoCargo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoCargo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AfiCategorias cat = new AfiCategorias();
            cat.Estado.IdEstado = (int)Estados.Activo;
            this.ddlCategoria.DataSource = AfiliadosF.CategoriasObtenerListaFiltro(cat);
            this.ddlCategoria.DataValueField = "IdCategoria";
            this.ddlCategoria.DataTextField = "Categoria";
            this.ddlCategoria.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCategoria, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            PrePrestamosPlanes pp = new PrePrestamosPlanes();
            pp.Estado.IdEstado = (int)Estados.Activo;
            this.ddlPrestamoPlan.DataSource = PrePrestamosF.PrestamosPlanesObtenerLista(pp);
            this.ddlPrestamoPlan.DataValueField = "IdPrestamoPlan";
            this.ddlPrestamoPlan.DataTextField = "Descripcion";
            this.ddlPrestamoPlan.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrestamoPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosFormasCobrosAfiliados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
        }
        private void MapearControlesAObjeto(TGEFormasCobrosCodigosConceptosTiposCargosCategorias pParametro)
        {
            pParametro.FormaCobro.IdFormaCobro = Convert.ToInt32(this.ddlFormaCobro.SelectedValue);

            pParametro.FormaCobro.FormaCobro = this.ddlFormaCobro.SelectedItem.Text;
            pParametro.IdTipoCargo = this.ddlTipoCargo.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlTipoCargo.SelectedValue);
            pParametro.TipoCargo = this.ddlTipoCargo.SelectedItem.Text;
            pParametro.IdCategoria = this.ddlCategoria.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlCategoria.SelectedValue);
            pParametro.Categoria = this.ddlCategoria.SelectedValue == string.Empty ? default(string) : this.ddlCategoria.SelectedItem.Text;
            pParametro.IdPrestamoPlan = this.ddlPrestamoPlan.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlPrestamoPlan.SelectedValue);
            pParametro.PrestamoPlan = this.ddlPrestamoPlan.SelectedValue == string.Empty ? default(string) : this.ddlPrestamoPlan.SelectedItem.Text;
            pParametro.CodigoConcepto = this.txtCodigoConcepto.Text;// == string.Empty ? default(string) : this.txtCodigoConcepto.Text;
            pParametro.CodigoSubConcepto = this.txtCodigoSubConcepto.Text == string.Empty ? default(string) : this.txtCodigoSubConcepto.Text;
            pParametro.CodigoConceptoPrestamoPlan = this.txtCodigoConceptoPrestamoPlan.Text == string.Empty ? default(string) : this.txtCodigoConceptoPrestamoPlan.Text;
            pParametro.SeEnviaComoPrestamo = this.chkSeEnviaComoPrestamo.Checked;
            pParametro.SeEnviaEnTXT = this.chkSeEnviaTXT.Checked;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pParametro.ImporteTopeEnvioCargo = this.txtImporteTopeEnvioCargo.Decimal == 0 ? default(decimal?) : this.txtImporteTopeEnvioCargo.Decimal;
        }
        private void MapearObjetoAControles(TGEFormasCobrosCodigosConceptosTiposCargosCategorias pParametro)
        {
            ListItem item;
            item = this.ddlFormaCobro.Items.FindByValue(pParametro.FormaCobro.IdFormaCobro.ToString());
            if (item == null)
                this.ddlFormaCobro.Items.Add(new ListItem(pParametro.FormaCobro.FormaCobro, pParametro.FormaCobro.IdFormaCobro.ToString()));
            this.ddlFormaCobro.SelectedValue = pParametro.FormaCobro.IdFormaCobro.ToString();

            item = this.ddlTipoCargo.Items.FindByValue(pParametro.IdTipoCargo.ToString());
            if (item == null)
                this.ddlTipoCargo.Items.Add(new ListItem(pParametro.TipoCargo, pParametro.IdTipoCargo.ToString()));
            this.ddlTipoCargo.SelectedValue = pParametro.IdTipoCargo.ToString();

            if (pParametro.IdCategoria.HasValue && pParametro.IdCategoria > 0)
            {
                item = this.ddlCategoria.Items.FindByValue(pParametro.IdCategoria.ToString());
                if (item == null)
                    this.ddlCategoria.Items.Add(new ListItem(pParametro.Categoria, pParametro.IdCategoria.ToString()));
                this.ddlCategoria.SelectedValue = pParametro.IdCategoria.ToString();
            }

            if (pParametro.IdPrestamoPlan.HasValue && pParametro.IdPrestamoPlan > 0)
            {
                item = this.ddlPrestamoPlan.Items.FindByValue(pParametro.IdPrestamoPlan.ToString());
                if (item == null)
                    this.ddlPrestamoPlan.Items.Add(new ListItem(pParametro.PrestamoPlan, pParametro.IdPrestamoPlan.ToString()));
                this.ddlPrestamoPlan.SelectedValue = pParametro.IdPrestamoPlan.ToString();
            }
            this.txtCodigoConcepto.Text = pParametro.CodigoConcepto;
            this.txtCodigoSubConcepto.Text = pParametro.CodigoSubConcepto;
            this.txtCodigoConceptoPrestamoPlan.Text = pParametro.CodigoConceptoPrestamoPlan;
            this.chkSeEnviaComoPrestamo.Checked = pParametro.SeEnviaComoPrestamo;
            this.chkSeEnviaTXT.Checked = pParametro.SeEnviaEnTXT;
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.txtImporteTopeEnvioCargo.Decimal = pParametro.ImporteTopeEnvioCargo.HasValue ? pParametro.ImporteTopeEnvioCargo.Value : 0;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiFormaCobroCodigoConcepto);
            this.MiFormaCobroCodigoConcepto.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, this.MiFormaCobroCodigoConcepto.UsuarioAlta);
                    this.MiFormaCobroCodigoConcepto.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.FormasCobrosCodigosConceptosTiposCargosCategoriasAgregar(this.MiFormaCobroCodigoConcepto);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.FormasCobrosCodigosConceptosTiposCargosCategoriasModificar(this.MiFormaCobroCodigoConcepto);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFormaCobroCodigoConcepto.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiFormaCobroCodigoConcepto.CodigoMensaje, true, this.MiFormaCobroCodigoConcepto.CodigoMensajeArgs);
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