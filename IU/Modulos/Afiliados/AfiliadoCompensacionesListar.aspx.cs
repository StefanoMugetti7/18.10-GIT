using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using System.Collections;
using Afiliados;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadoCompensacionesListar : PaginaSegura
    {

        private List<AfiCompensaciones> MisCompensaciones
        {
            get { return (List<AfiCompensaciones>)Session[this.MiSessionPagina + "AfiliadosListarMisCompensaciones"]; }
            set { Session[this.MiSessionPagina + "AfiliadosListarMisCompensaciones"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("AfiliadoCompensacionesAgregar.aspx");
                this.CargarCombos();
                AfiCompensaciones parametros = this.BusquedaParametrosObtenerValor<AfiCompensaciones>();
                if (parametros.BusquedaParametros)
                {
                    //this.txtNumeroSocio.Text = parametros.NumeroSocio.ToString();
                    parametros.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);

                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiCompensaciones parametros = this.BusquedaParametrosObtenerValor<AfiCompensaciones>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesAgregar.aspx"), true);
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AfiCompensaciones compensacion = this.MisCompensaciones[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCompensacion", compensacion.IdCompensacion);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesConsultar.aspx"), true);
            }
            //else if (e.CommandName == Gestion.Anular.ToString())
            //{
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesAnular.aspx"), true);
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ImageButton baja = (ImageButton)e.Row.FindControl("btnBaja");
                

                //Permisos btnEliminar
                modificar.Visible = this.ValidarPermiso("AfiliadoCompensacionesModificar.aspx"); 
                ibtnConsultar.Visible = this.ValidarPermiso("AfiliadoCompensacionesConsultar.aspx");
                //baja.Visible = this.ValidarPermiso("AfiliadoCompensacionesAnular.aspx");
                //ibtnConsultar.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCompensaciones.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiCompensaciones parametros = this.BusquedaParametrosObtenerValor<AfiCompensaciones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiCompensaciones>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisCompensaciones;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCompensaciones = this.OrdenarGrillaDatos<AfiCompensaciones>(this.MisCompensaciones, e);
            this.gvDatos.DataSource = this.MisCompensaciones;
            this.gvDatos.DataBind();
        }
        #endregion

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        private void CargarLista(AfiCompensaciones pCompensacion)
        {
            pCompensacion.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pCompensacion.IdAfiliado != 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = hdfIdAfiliado.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            pCompensacion.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);

            pCompensacion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<AfiCompensaciones>(pCompensacion);
            this.MisCompensaciones = AfiliadosF.CompensacionesObtenerListaFiltro(pCompensacion);
            this.gvDatos.DataSource = this.MisCompensaciones;
            this.gvDatos.PageIndex = pCompensacion.IndiceColeccion;
            this.gvDatos.DataBind();
        }

    
    }
}