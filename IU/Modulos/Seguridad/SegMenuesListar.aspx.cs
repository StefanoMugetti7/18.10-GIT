using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Comunes.Entidades;
using System.Collections.Generic;
using Seguridad.FachadaNegocio;

namespace IU.Modulos.Seguridad
{
    public partial class SegMenuesListar : PaginaSegura
    {
        private List<Menues> MisMenues
        {
            get { return (List<Menues>)Session[this.MiSessionPagina + "SegMenuesListarMisMenues"]; }
            set { Session[this.MiSessionPagina + "SegMenuesListarMisMenues"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            ctrPopUpMenu.MenuesModificarDatosAceptar += new IU.Modulos.Seguridad.Controles.SegMenuesModificarDatosPopUp.MenuesModificarDatosPopUpEventHandler(ctrPopUpMenu_MenuesModificarDatosAceptar);
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //this.btnAgregar.Visible = this.ValidarPermiso("RequisicionesAgregar.aspx");
                Menues parametro = new Menues();
                parametro.IdMenuPadre = (int)EnumMenues.AdministrarTodos;
                this.CargarLista(parametro);
            }
        }

        void ctrPopUpMenu_MenuesModificarDatosAceptar(object sender, Menues e, Gestion pGestion)
        {
            Menues parametro = new Menues();
            parametro.IdMenuPadre = (int)EnumMenues.AdministrarTodos;
            this.CargarLista(parametro);
            this.UpdatePanel1.Update();
        }

        //protected void btnBuscar_Click(object sender, EventArgs e)
        //{
        //    Menues parametros = this.BusquedaParametrosObtenerValor<Menues>();
        //    this.CargarLista(parametros);
        //}

        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Seguridad/SegMenuesAgregar.aspx"), true);
        //}

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            Menues tipoCargo = this.MisMenues[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdMenu", tipoCargo.IdMenu);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                string url = "~/Modulos/Seguridad/SegMenuesModificar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                string url = "~/Modulos/Seguridad/SegMenuesConsultar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisMenues.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Menues parametros = this.BusquedaParametrosObtenerValor<Menues>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<Menues>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisMenues;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisMenues = this.OrdenarGrillaDatos<Menues>(this.MisMenues, e);
            this.gvDatos.DataSource = this.MisMenues;
            this.gvDatos.DataBind();
        }

        private void CargarLista(Menues men)
        {            
            //men.BusquedaParametros = true;
            //this.BusquedaParametrosGuardarValor<Menues>(men);
            this.MisMenues = SeguridadF.MenuesObtenerPorPadre(men);
            this.gvDatos.DataSource = this.MisMenues;
            this.gvDatos.PageIndex = men.IndiceColeccion;
            this.gvDatos.DataBind();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.ctrPopUpMenu.IniciarControl(new Menues(), Gestion.Agregar, true, -1);
        }
    }
}
