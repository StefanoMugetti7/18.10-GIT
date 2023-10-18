using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;
using Generales.Entidades;
using Afiliados.Entidades;

namespace IU.Modulos.Plantillas
{
    public partial class PlantillasListar : PaginaSegura
    {
        private List<TGEPlantillas> MiPlantilla
        {
            get { return (List<TGEPlantillas>)Session[this.MiSessionPagina + "PlantillasListarMiPlantilla"]; }
            set { Session[this.MiSessionPagina + "PlantillasListarMiPlantilla"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPlantilla, this.btnBuscar);
                this.CargarCombos();
                TGEPlantillas parametros = this.BusquedaParametrosObtenerValor<TGEPlantillas>();
                if (parametros.BusquedaParametros)
                {
                    
                    this.txtPlantilla.Text = parametros.NombrePlantilla;
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGEPlantillas parametros = this.BusquedaParametrosObtenerValor<TGEPlantillas>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasAgregar.aspx"), true);
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEPlantillas sector = this.MiPlantilla[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPlantilla", sector.IdPlantilla);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasConsultar.aspx"), true);
            }

        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            //TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.esta);
           
        }

            protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGEPlantillas plantilla = (TGEPlantillas)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("PlantillasModificar.aspx");
                //consultar.Visible = this.ValidarPermiso("PlantillasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEPlantillas parametros = this.BusquedaParametrosObtenerValor<TGEPlantillas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEPlantillas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<TGEPlantillas>(this.MiPlantilla, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiPlantilla = this.OrdenarGrillaDatos<TGEPlantillas>(this.MiPlantilla, e);
            AyudaProgramacion.CargarGrillaListas<TGEPlantillas>(this.MiPlantilla, false, this.gvDatos, true);
        }
        #endregion

        private void CargarLista(TGEPlantillas pParametro)
        {
            pParametro.Estado.IdEstado = Convert.ToInt32( this.ddlEstados.SelectedValue);
            pParametro.NombrePlantilla = this.txtPlantilla.Text;
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TGEPlantillas>(pParametro);
            this.MiPlantilla = TGEGeneralesF.PlantillasObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<TGEPlantillas>(this.MiPlantilla, false, this.gvDatos, true);
        }
    }
}
