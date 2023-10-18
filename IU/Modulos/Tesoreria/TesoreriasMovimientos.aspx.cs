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
using Tesorerias.Entidades;
using Tesorerias;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class TesoreriasMovimientos : PaginaTesoreria
    {
        //private TESTesorerias MisTesoreria
        //{
        //    get { return (TESTesorerias)Session[this.MiSessionPagina + "PlazosListarMisPlazos"]; }
        //    set { Session[this.MiSessionPagina + "PlazosListarMisPlazos"] = value; }
        //}

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //TESTesorerias tesoreria = new TESTesorerias();
                //tesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                //tesoreria.FechaAbrir = DateTime.Now;
                //tesoreria.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;

                //this.btnAgregar.Visible = this.ValidarPermiso("RequisicionesAgregar.aspx");
                this.btnAgregar.Visible = this.ValidarPermiso("TesoreriasMovimientosAgregar.aspx");
                //Se utiliza para persistir los parametros de busqueda ingresados por el usuario
                //TESTesorerias parametros = this.BusquedaParametrosObtenerValor<TESTesorerias>();
                //if (parametros.BusquedaParametros)
                //{
                //}

                this.CargarLista(this.MiTesoreria);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientosAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");

                TESTesoreriasMovimientos item = (TESTesoreriasMovimientos)e.Row.DataItem;

                ibtnConsultar.Visible = false;
                if (item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaBancosDebito ||
                    item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.ExtraccionEfectivoParaFilial)
                {
                    ibtnConsultar.Visible = this.ValidarPermiso("TesoreriasMovimientosAgregar.aspx");
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiTesoreria.ObtenerTesoreriasMovimientos().Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTesoreriaMovimiento"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdTesoreriaMovimiento", id);

            if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientosAgregar.aspx"), true);
            //ACA TENGO QUE PREGUNTAR SI TIENE PARAMETROSURL Y BLOQUEAR TODO EN TesoreriasMovimientosAgregar
        }


        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESTesorerias parametros = this.BusquedaParametrosObtenerValor<TESTesorerias>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESTesorerias>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiTesoreria.ObtenerTesoreriasMovimientos();
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MiTesoreria.TesoreriasMovimientos = this.OrdenarGrillaDatos<TESTesoreriasMovimientos>(this.MiTesoreria.ObtenerTesoreriasMovimientos(), e);
            //this.gvDatos.DataSource = this.MiTesoreria.TesoreriasMovimientos;
            //this.gvDatos.DataBind();
        }

        private void CargarLista(TESTesorerias pTesoreria)
        {
            //pTesoreria.BusquedaParametros = true;
            //this.BusquedaParametrosGuardarValor<TESTesorerias>(pTesoreria);
            //this.MisTesoreria = TesTesoreriasF.TesoreriasObtenerDatosCompletos(pTesoreria);
            //this.gvDatos.DataSource = this.MisTesoreria.TesoreriasMonedas[0].TesoreriasMovimientos;
            this.gvDatos.DataSource = this.MiTesoreria.ObtenerTesoreriasMovimientos();
            //this.gvDatos.PageIndex = pTesoreria.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
