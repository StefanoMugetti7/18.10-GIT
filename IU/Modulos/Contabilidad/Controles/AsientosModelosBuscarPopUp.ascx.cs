using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Contabilidad;
using Generales.Entidades;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class AsientosModelosBuscarPopUp : ControlesSeguros
    {
        private List<CtbAsientosModelos> MisAsientosModelos
        {
            get { return (List<CtbAsientosModelos>)Session[this.MiSessionPagina + "AsientosModelosBuscarPopUpMisAsientosModelos"]; }
            set { Session[this.MiSessionPagina + "AsientosModelosBuscarPopUpMisAsientosModelos"] = value; }
        }

        public delegate void AsientosModelosBuscarPopUpEventHandler(CtbAsientosModelos e);
        public event AsientosModelosBuscarPopUpEventHandler AsientosModelosBuscarSeleccionarPopUp;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(bool pLimpiarDatos)
        {
            if (pLimpiarDatos)
            {
                this.MisAsientosModelos = new List<CtbAsientosModelos>();
                this.gvDatos.DataSource = this.MisAsientosModelos;
                this.gvDatos.DataBind();
            }
            this.CargarCombos();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarAsientoModelo();", true);
        }

        public void IniciarControl(bool pLimpiarDatos, CtbEjerciciosContables pEjercicio)
        {
            if (pLimpiarDatos)
            {
                this.MisAsientosModelos = new List<CtbAsientosModelos>();
                this.gvDatos.DataSource = this.MisAsientosModelos;
                this.gvDatos.DataBind();
            }
            this.CargarCombos();
            this.ddlEjercicioContable.SelectedValue = pEjercicio.IdEjercicioContable.ToString();
            this.ddlEjercicioContable.Enabled = false;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarAsientoModelo();", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarLista();
            //this.mpePopUp.Show();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CtbAsientosModelos asientoModelo = this.MisAsientosModelos[indiceColeccion];

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.AsientosModelosBuscarSeleccionarPopUp != null)
                {
                    asientoModelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asientoModelo);
                    this.AsientosModelosBuscarSeleccionarPopUp(asientoModelo);
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisAsientosModelos.Count);
                e.Row.Cells.Add(tableCell);
            }
            //this.mpePopUp.Show();
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisAsientosModelos;
            gvDatos.DataBind();
            //this.mpePopUp.Show();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisAsientosModelos = this.OrdenarGrillaDatos<CtbAsientosModelos>(this.MisAsientosModelos, e);
            this.gvDatos.DataSource = this.MisAsientosModelos;
            this.gvDatos.DataBind();
            //this.mpePopUp.Show();
        }

        private void CargarCombos()
        {
            CtbEjerciciosContables filtro = new CtbEjerciciosContables();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            List<CtbEjerciciosContables> lista = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtro);
            this.ddlEjercicioContable.DataSource = lista;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();
            if (this.ddlEjercicioContable.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEjercicioContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)EstadosTodos.Todos).ToString();

            this.ddlTipoAsiento.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposAsientos);
            this.ddlTipoAsiento.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoAsiento.DataTextField = "Descripcion";
            this.ddlTipoAsiento.DataBind();
            if (this.ddlTipoAsiento.Items.Count > 1)
            {
                this.ddlTipoAsiento.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
                this.ddlTipoAsiento.SelectedValue = ((int)EstadosTodos.Todos).ToString();
            }
        }

        private void CargarLista()
        {
            CtbAsientosModelos asientoModelo = new CtbAsientosModelos();
            asientoModelo.EjercicioContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            asientoModelo.Detalle = this.txtDetalle.Text.Trim();
            asientoModelo.TipoAsiento.IdTipoAsiento = Convert.ToInt32(this.ddlTipoAsiento.SelectedValue);
            asientoModelo.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            this.MisAsientosModelos = ContabilidadF.AsientosModelosObtenerListaFiltro(asientoModelo);
            this.gvDatos.DataSource = this.MisAsientosModelos;
            this.gvDatos.DataBind();
        }
    }
}