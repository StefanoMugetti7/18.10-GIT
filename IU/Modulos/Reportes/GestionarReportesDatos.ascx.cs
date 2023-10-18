using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU.Modulos.Reportes
{
    public partial class GestionarReportesDatos : ControlesSeguros
    {
        private RepReportes MiReporte
        {
            get { return (RepReportes)Session[this.MiSessionPagina + "GestionarReportesDatosMiReporte"]; }
            set { Session[this.MiSessionPagina + "GestionarReportesDatosMiReporte"] = value; }
        }

        public delegate void GestionarReportesDatosAceptarEventHandler(object sender, Objeto e, bool resultado);
        public event GestionarReportesDatosAceptarEventHandler GestionarReportesDatosAceptar;
        public delegate void GestionarReportesDatosCancelarEventHandler();
        public event GestionarReportesDatosCancelarEventHandler GestionarReportesDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.RepDatosParametros.GestionarReportesDatosParametrosAceptar += new GestionarReportesDatosParametros.GestionarReportesDatosParametrosAceptarEventHandler(RepDatosParametros_GestionarReportesDatosParametrosAceptar);
            if (!this.IsPostBack)
            {
                //Limpio las variables de sesion
                this.MiReporte = new RepReportes();
            }
        }

        void RepDatosParametros_GestionarReportesDatosParametrosAceptar(object sender, Objeto e)
        {
            RepParametros parametro = (RepParametros)e;
            if (parametro.EstadoColeccion == EstadoColecciones.Agregado)
            {
                this.MiReporte.Parametros.Add(parametro);
                parametro.IndiceColeccion = this.MiReporte.Parametros.IndexOf(parametro);
            }
            else
            {
                this.MiReporte.Parametros[parametro.IndiceColeccion] = parametro;
            }
            this.gvParametros.DataSource = this.MiReporte.Parametros;
            this.gvParametros.DataBind();
            this.UpdatePanel1.Update();
        }

        public void IniciarControl(RepReportes pReporte, Gestion pGestion)
        {
            AyudaProgramacion.LimpiarControles(this, true);
            this.MiReporte = pReporte;
            this.GestionControl = pGestion;
            this.CargarModulosSistema();
            // AyudaProgramacion.CompletarDropConEnum(this.ddlEstadoIdEstado, typeof(Estados));
            this.ddlEstadoIdEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstadoIdEstado.DataTextField = "Descripcion";
            this.ddlEstadoIdEstado.DataValueField = "IdEstado";
            this.ddlEstadoIdEstado.DataBind();
            this.ddlEstadoIdEstado.SelectedValue = ((int)Estados.Activo).ToString();

            this.ddlPlantilla.DataSource =TGEGeneralesF.PlantillasObtenerLista();
            this.ddlPlantilla.DataTextField = "NombrePlantilla";
            this.ddlPlantilla.DataValueField = "Codigo";
            this.ddlPlantilla.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPlantilla, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.HabilitarControles(this, true, this.paginaSegura);
                    break;
                case Gestion.Modificar:
                    MapearObjetoAControles(this.MiReporte);
                    AyudaProgramacion.HabilitarControles(this, true, this.paginaSegura);
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Consultar:
                    MapearObjetoAControles(this.MiReporte);
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    break;
                default:
                    break;
            }
        }

        protected void gvParametros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.RepDatosParametros.IniciarControl(this.MiReporte.Parametros[indiceColeccion], Gestion.Modificar);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.RepDatosParametros.IniciarControl(new RepParametros(), Gestion.Agregar);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("GestionarReportesDatos");
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiReporte);
            this.MiReporte.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            bool resultado = false;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiReporte.EstadoColeccion = EstadoColecciones.Agregado;
                    resultado = ReportesF.ReportesAgregar(this.MiReporte);
                    break;
                case Gestion.Modificar:
                    this.MiReporte.EstadoColeccion = EstadoColecciones.Modificado;
                    resultado = ReportesF.ReportesModificar(this.MiReporte);
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Consultar:
                    break;
                default:
                    break;
            }

            if (resultado)
            {
                AyudaProgramacion.LimpiarControles(this, true);
            }
            if (GestionarReportesDatosAceptar != null)
                GestionarReportesDatosAceptar(sender, this.MiReporte, resultado);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            AyudaProgramacion.LimpiarControles(this, true);
            if (this.GestionarReportesDatosCancelar != null)
                this.GestionarReportesDatosCancelar();
        }

        private void CargarModulosSistema()
        {
            this.ddlModulosSistemaIdModulosSistema.DataSource = TGEGeneralesF.ModulosSistemaObtenerLista();
            this.ddlModulosSistemaIdModulosSistema.DataValueField = "IdModuloSistema";
            this.ddlModulosSistemaIdModulosSistema.DataTextField = "ModuloSistema";
            this.ddlModulosSistemaIdModulosSistema.DataBind();
        }

        private void MapearControlesAObjeto(RepReportes pParametro)
        {
            pParametro.ModuloSistema.IdModuloSistema = Convert.ToInt32(this.ddlModulosSistemaIdModulosSistema.SelectedValue);
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.Detalle = this.txtDetalle.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstadoIdEstado.SelectedValue);
            pParametro.Excel = this.chkExcel.Checked;
            pParametro.Texto = this.chkTexto.Checked;
            pParametro.NombreCrystal = this.txtNombreCrystal.Text;
            pParametro.Seguridad = this.chkSeguridad.Checked;
            pParametro.StoredProcedure = this.txtStoredProcedure.Text;
            pParametro.NombreArchivo = this.txtNombreArchivo.Text.Trim();
            pParametro.NombreSolapa = this.txtNombreSolapa.Text.Trim();
            pParametro.DBF = this.chkDBF.Checked;
            pParametro.IncluirNombreCampos = this.chkIncluirNombreCampos.Checked;
            pParametro.IncluirSeparador = this.chkSeparador.Checked;
            pParametro.Separador = this.txtSeparador.Text.Trim();
            pParametro.CodigoPlantilla = this.ddlPlantilla.SelectedValue;
            pParametro.KeysPDFCorte = this.txtCortePDF.Text;
        }

        private void MapearObjetoAControles(RepReportes pParametro)
        {
            this.ddlModulosSistemaIdModulosSistema.SelectedValue = pParametro.ModuloSistema.IdModuloSistema.ToString();
            this.txtDescripcion.Text = pParametro.Descripcion;
            this.txtDetalle.Text = pParametro.Detalle;
            this.ddlEstadoIdEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.chkExcel.Checked = pParametro.Excel;
            this.chkTexto.Checked = pParametro.Texto;
            this.txtNombreCrystal.Text = pParametro.NombreCrystal;
            this.chkSeguridad.Checked = pParametro.Seguridad;
            this.txtStoredProcedure.Text = pParametro.StoredProcedure;
            this.txtNombreArchivo.Text = pParametro.NombreArchivo;
            this.txtNombreSolapa.Text = pParametro.NombreSolapa;
            this.chkDBF.Checked = pParametro.DBF;
            this.chkIncluirNombreCampos.Checked = pParametro.IncluirNombreCampos;
            this.chkSeparador.Checked = pParametro.IncluirSeparador;
            this.txtSeparador.Text = pParametro.Separador;
            this.txtCortePDF.Text = pParametro.KeysPDFCorte;
            if (!string.IsNullOrWhiteSpace(pParametro.CodigoPlantilla))
            {
                ListItem item = ddlPlantilla.Items.FindByValue(pParametro.CodigoPlantilla);
                if (item == null)
                {
                    this.ddlPlantilla.Items.Add(new ListItem(pParametro.CodigoPlantilla, pParametro.CodigoPlantilla));
                }
                this.ddlPlantilla.SelectedValue = pParametro.CodigoPlantilla;
            }
            
            
            this.gvParametros.DataSource = this.MiReporte.Parametros;
            this.gvParametros.DataBind();
        }
    }
}