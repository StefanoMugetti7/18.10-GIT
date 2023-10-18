using Comunes.Entidades;
using Generales.FachadaNegocio;
using Medicina.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Medicina.Controles
{
    public partial class DiasHorasModificarDatosPopUp : ControlesSeguros
    {
        private MedPrestadoresDiasHoras MiDiasHoras
        {
            get { return (MedPrestadoresDiasHoras)Session[this.MiSessionPagina + "DiasHorasModificarDatosPopUpMiDiasHoras"]; }
            set { Session[this.MiSessionPagina + "DiasHorasModificarDatosPopUpMiDiasHoras"] = value; }
        }
        private List<MedEspecializaciones> MisEspecialidadesCombo
        {
            get
            {
                return (Session[this.MiSessionPagina + "EspecializacionesDiasHorasPopUpCombo"] == null ?
                    (List<MedEspecializaciones>)(Session[this.MiSessionPagina + "EspecializacionesDiasHorasPopUpCombo"] = new List<MedEspecializaciones>()) : (List<MedEspecializaciones>)Session[this.MiSessionPagina + "EspecializacionesDiasHorasPopUpCombo"]);
            }
            set { Session[this.MiSessionPagina + "EspecializacionesDiasHorasPopUpCombo"] = value; }
        }
        public delegate void DiasHorasModificarDatosEventHandler(MedPrestadoresDiasHoras e, Gestion pGestion);
        public event DiasHorasModificarDatosEventHandler DiasHorasModificarDatosAceptar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //this.paginaSegura.scriptManager.Scripts.Add(new ScriptReference("~/Recursos/jquery.ptTimeSelect.js"));
            }
        }
        public void IniciarControl(MedPrestadoresDiasHoras pParametro, List<MedEspecializaciones> pEspecializaciones, Gestion pGestion)
        {
            this.MisEspecialidadesCombo = pEspecializaciones;
            this.MiDiasHoras = pParametro;
            this.GestionControl = pGestion;
            this.ddlEstados.Enabled = true;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.txtFe.Text = DateTime.Now.ToShortDateString();
                    pParametro.Estado.IdEstado = (int)Estados.Activo;
                    this.ddlEstados.Enabled = false;
                    //pAfiliado.Estado = TGEGeneralesF.TGEEstadosObtener(pAfiliado.Estado);
                    this.CargarCombos();
                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiDiasHoras);
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MapearObjetoAControles(this.MiDiasHoras);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalDiasHoras();", true);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("DiasHorasModificarDatos");
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiDiasHoras);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiDiasHoras.Estado.IdEstado = (int)Estados.Activo;
                    this.MiDiasHoras.EstadoColeccion = EstadoColecciones.Agregado;
                    break;
                case Gestion.Modificar:
                    this.MiDiasHoras.Estado.IdEstado = Convert.ToInt32(ddlEstados.SelectedValue);
                    this.MiDiasHoras.Estado.Descripcion = ddlEstados.SelectedItem.ToString();
                    this.MiDiasHoras.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiDiasHoras, this.GestionControl);
                    break;
                default:
                    break;
            }
            if (this.MiDiasHoras.Tiempo < 5)
            {
                this.MostrarMensaje("ValidarTiempoDelPrestador", true);
            } else {
            if (this.DiasHorasModificarDatosAceptar != null)
                this.DiasHorasModificarDatosAceptar(this.MiDiasHoras, this.GestionControl);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalDiasHoras();", true);
            }
        }
        private void MapearControlesAObjeto(MedPrestadoresDiasHoras pParametro)
        {
            pParametro.Dia.IdDia = this.ddlDia.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlDia.SelectedValue);
            pParametro.Dia.Descripcion = this.ddlDia.SelectedItem.Text;
            DateTime horaDesde = DateTime.Parse(this.txtHoraDesde.Text);
            pParametro.HoraDesde = new TimeSpan(horaDesde.Hour, horaDesde.Minute, horaDesde.Second);
            DateTime horaHasta = DateTime.Parse(this.txtHoraHasta.Text);
            pParametro.HoraHasta = new TimeSpan(horaHasta.Hour, horaHasta.Minute, horaHasta.Second);
            pParametro.Tiempo = this.txtTiempo.Text == string.Empty ? 0 : Convert.ToInt32(this.txtTiempo.Text);
            pParametro.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.Filial.Filial = this.ddlFiliales.SelectedItem.Text;
            pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEstados.SelectedValue);
        }
        private void MapearObjetoAControles(MedPrestadoresDiasHoras pParametro)
        {
            AyudaProgramacion.CargarGrillaListas(this.MiDiasHoras.Especializaciones, true, this.gvEspecializaciones, true);
            this.CargarCombos();
            DateTime horaDesde = DateTime.Parse(pParametro.HoraDesde.ToString());
            this.txtHoraDesde.Text = horaDesde.ToString("hh:mm tt");
            DateTime horaHasta = DateTime.Parse(pParametro.HoraHasta.ToString());
            this.txtHoraHasta.Text = horaHasta.ToString("hh:mm tt");
            this.txtTiempo.Text = pParametro.Tiempo.ToString();

            ListItem item = this.ddlDia.Items.FindByValue(pParametro.Dia.IdDia.ToString());
            if (item == null)
                this.ddlDia.Items.Add(new ListItem(pParametro.Dia.IdDia.ToString(), pParametro.Dia.Descripcion));
            this.ddlDia.SelectedValue = pParametro.Dia.IdDia.ToString();
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalDiasHoras();", true);
        }
        private void CargarCombos()
        {
            this.CargarComboEspecializacion();
            List<TGEListasValoresSistemasDetalles> lista = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.Dias);
            lista = lista.OrderBy(X => X.CodigoValor).ToList();
            this.ddlDia.DataSource = lista;
            this.ddlDia.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlDia.DataTextField = "Descripcion";
            this.ddlDia.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlDia, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFiliales.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataBind();
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }
        private void CargarComboEspecializacion()
        {
            List<MedEspecializaciones> especializacion = MisEspecialidadesCombo.Where(p => !(MiDiasHoras.Especializaciones.Any(p2 => p2.IdEspecializacion == p.IdEspecializacion))).ToList();
            this.ddlEspecialidad.DataSource = especializacion;
            this.ddlEspecialidad.DataValueField = "IdEspecializacion";
            this.ddlEspecialidad.DataTextField = "Descripcion";
            this.ddlEspecialidad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecialidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void gvEspecializaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                HiddenField IdEspecializacionBorrar = gvEspecializaciones.Rows[index].FindControl("hdfIdEspecializacion") as HiddenField;
                var especialidad = MiDiasHoras.Especializaciones.FirstOrDefault(x => x.IdEspecializacion == Convert.ToInt32(IdEspecializacionBorrar.Value));
                especialidad.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(especialidad, this.GestionControl);
                if (especialidad.EstadoColeccion == EstadoColecciones.Agregado)
                    this.MiDiasHoras.Especializaciones.Remove(especialidad);
                else
                    especialidad.EstadoColeccion = EstadoColecciones.Borrado;
                AyudaProgramacion.CargarGrillaListas(this.MiDiasHoras.Especializaciones, true, this.gvEspecializaciones, true);
                //this.MisEspecialidadesCombo.Add(especialidad);
                // this.MiDiasHoras.Especializaciones.Sort((x, y) => x.IdEspecializacion.CompareTo(y.Descripcion));
                this.CargarComboEspecializacion();
            }
        }

        protected void gvEspecializaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MedEspecializaciones Comentarios = (MedEspecializaciones)e.Row.DataItem;
                if (((MedEspecializaciones)e.Row.DataItem).EstadoColeccion == EstadoColecciones.Agregado)
                {
                    ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                    string mensaje = this.ObtenerMensajeSistema("POComentarioConfirmarBaja");
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    ibtn.Attributes.Add("OnClick", funcion);
                    ibtn.Visible = true;
                }
            }
        }
        protected void btnAgregarEspecialidad_Click(object sender, EventArgs e)
        {
            var Especialidad = this.MisEspecialidadesCombo.FirstOrDefault(x => x.IdEspecializacion == Convert.ToInt32(this.ddlEspecialidad.SelectedValue));
            if (Especialidad != null)
            {
                Especialidad.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiDiasHoras.Especializaciones.Add(Especialidad);
                //this.MisEspecialidadesCombo.Remove(Especialidad);
                AyudaProgramacion.CargarGrillaListas(this.MiDiasHoras.Especializaciones, true, this.gvEspecializaciones, true);
                this.CargarComboEspecializacion();
            }

        }

    }
}