using Comunes.Entidades;
using Generales.FachadaNegocio;
using Medicina;
using Medicina.Entidades;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.Medicina
{
    public partial class NomencladoresListar : PaginaSegura
    {
        private DataTable MisNomencladores
        {
            get { return (DataTable)Session[this.MiSessionPagina + "NomencladoresListarMisNomencladores"]; }
            set { Session[this.MiSessionPagina + "NomencladoresListarMisNomencladores"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNombre, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("NomencladoresAgregar.aspx");
                this.CargarCombos();

                MedNomencladores parametros = this.BusquedaParametrosObtenerValor<MedNomencladores>();

                if (parametros.BusquedaParametros)
                {
                    this.txtNombre.Text = parametros.Filtro;
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            MedNomencladores parametros = this.BusquedaParametrosObtenerValor<MedNomencladores>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/NomencladoresAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdNomenclador"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "Gestion", e.CommandName },
                { "IdNomenclador", id }
            };

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/NomencladoresConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/NomencladoresModificar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("NomencladoresConsultar.aspx");
                ibtnModificar.Visible = this.ValidarPermiso("NomencladoresModificar.aspx");
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            MedNomencladores parametros = this.BusquedaParametrosObtenerValor<MedNomencladores>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<MedNomencladores>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisNomencladores;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisNomencladores = this.OrdenarGrillaDatos<MedNomencladores>(this.MisNomencladores, e);
            this.gvDatos.DataSource = this.MisNomencladores;
            this.gvDatos.DataBind();
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("MedNomencladores");
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(MedNomencladores pParametro)
        {
            pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Filtro = this.txtNombre.Text.Trim();
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<MedNomencladores>(pParametro);
            this.MisNomencladores = MedicinaF.NomencladoresObtenerGrilla(pParametro);
            this.gvDatos.DataSource = this.MisNomencladores;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}