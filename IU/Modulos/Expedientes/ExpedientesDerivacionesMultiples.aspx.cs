using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Expedientes.Entidades;
using Expedientes;
using Comunes.Entidades;

namespace IU.Modulos.Expedientes
{
    public partial class ExpedientesDerivacionesMultiples : PaginaSegura
    {
        private List<ExpExpedientes> MisExpedientes
        {
            get { return (List<ExpExpedientes>)Session[this.MiSessionPagina + "ExpedientesDerivacionesMultiplesMisExpedientes"]; }
            set { Session[this.MiSessionPagina + "ExpedientesDerivacionesMultiplesMisExpedientes"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
                this.CargarLista();
        }

        private void CargarLista()
        {
            ExpExpedientes pExpediente = new ExpExpedientes();
            pExpediente.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pExpediente.IdEstadoExpedienteTracking = (int)EstadosExpedientesTracking.Derivado;
            this.MisExpedientes = ExpedientesF.ExpedientesObtenerListaFiltro(pExpediente);
            foreach (ExpExpedientes expte in this.MisExpedientes)
                expte.Incluir = true;
            
            AyudaProgramacion.CargarGrillaListas<ExpExpedientes>(this.MisExpedientes, false, this.gvDatos, true);
            this.btnAceptar.Visible = this.MisExpedientes.Count > 0;
        }

        private List<ExpExpedientes> ObtenerExpedientesAceptados()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    this.MisExpedientes[fila.DataItemIndex].Incluir = ((CheckBox)fila.FindControl("chkIncluir")).Checked;
                }
            }
            return this.MisExpedientes.Where(x => x.Incluir == true).ToList();
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisExpedientes;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisExpedientes = this.OrdenarGrillaDatos<ExpExpedientes>(this.MisExpedientes, e);
            this.gvDatos.DataSource = this.MisExpedientes;
            this.gvDatos.DataBind();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            List<ExpExpedientes> lista = this.ObtenerExpedientesAceptados();
            Objeto resultado = new Objeto();
            guardo = ExpedientesF.ExpedientesAceptarDerivaciones(resultado, lista, this.UsuarioActivo.SectorPredeterminado);
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.popUpMensajes.MostrarMensaje("CodigoMensaje");
            }
            else
            {
                this.MostrarMensaje(resultado.CodigoMensaje, true, resultado.CodigoMensajeArgs);
            }
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.CargarLista();
        }
    }
}
