using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Comunes.Entidades;

namespace IU.Modulos.Reportes
{
    public partial class GestionarReportes : PaginaSegura
    {
        private List<RepReportes> MisReportes
        {
            get { return this.PropiedadObtenerValor<List<RepReportes>>("GestionarReportesMisReportes"); }
            set { this.PropiedadGuardarValor("GestionarReportesMisReportes", value); }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.RepDatos.GestionarReportesDatosAceptar += new GestionarReportesDatos.GestionarReportesDatosAceptarEventHandler(RepDatos_GestionarReportesDatosAceptar);
            this.RepDatos.GestionarReportesDatosCancelar += new GestionarReportesDatos.GestionarReportesDatosCancelarEventHandler(RepDatos_GestionarReportesDatosCancelar);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
                //Limpio las variables de sesion
                this.MisReportes = new List<RepReportes>();
                this.mvGestion.SetActiveView(vBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("GestionarReportesAgregar.aspx");
            }
        }

        void RepDatos_GestionarReportesDatosCancelar()
        {
            this.mvGestion.SetActiveView(vBuscar);
            this.UpdatePanel1.Update();
        }

        void RepDatos_GestionarReportesDatosAceptar(object sender, Objeto e, bool resultado)
        {
            this.CargarGrilla(this.txtDescripcion.Text);
            this.mvGestion.SetActiveView(vBuscar);
            this.UpdatePanel1.Update();
        }

        private void CargarGrilla(string pDescripcion)
        {
            this.MisReportes = ReportesF.ReportesObtenerFiltro(pDescripcion);
            this.gvDatos.DataSource = this.MisReportes;
            this.gvDatos.DataBind();
            this.UpdatePanel1.Update();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString() || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            RepReportes reporte = this.MisReportes[indiceColeccion];
            reporte = ReportesF.ReportesObtenerUno(reporte);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.RepDatos.IniciarControl(reporte, Gestion.Consultar);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.RepDatos.IniciarControl(reporte, Gestion.Modificar);
            }
            this.mvGestion.SetActiveView(vDatos);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarGrilla(this.txtDescripcion.Text);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.RepDatos.IniciarControl(new RepReportes(), Gestion.Agregar);
            this.mvGestion.SetActiveView(this.vDatos);
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisReportes;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisReportes = this.OrdenarGrillaDatos<RepReportes>(this.MisReportes, e);
            this.gvDatos.DataSource = this.MisReportes;
            this.gvDatos.DataBind();
        }

    }
}
