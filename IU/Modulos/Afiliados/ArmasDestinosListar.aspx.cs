using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Afiliados;
using Comunes.Entidades;
using System.Collections;

namespace IU.Modulos.Afiliados
{
    public partial class ArmasDestinosListar : PaginaSegura
    {
        
         private List<AfiArmasDestinos> MisArmasDestinos
        {
            get { return (List<AfiArmasDestinos>)Session[this.MiSessionPagina + "ArmasDestinosListarMisArmasDestinos"]; }
            set { Session[this.MiSessionPagina + "ArmasDestinosListarMisArmasDestinos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
               
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoArma, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDestino, this.btnBuscar);
                

                this.btnAgregar.Visible = this.ValidarPermiso("ArmasDestinosAgregar.aspx");
                //this.CargarCombos();

                AfiArmasDestinos parametros = this.BusquedaParametrosObtenerValor<AfiArmasDestinos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCodigoArma.Text = parametros.Arma.IdArma.ToString() == "0" ? String.Empty : parametros.Arma.IdArma.ToString();
                    this.txtDestino.Text = parametros.Destino;
                    //this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiArmasDestinos parametros = this.BusquedaParametrosObtenerValor<AfiArmasDestinos>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ArmasDestinosAgregar.aspx"), true);
        }
     
         
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                ))
                return;
         
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AfiArmasDestinos armas = this.MisArmasDestinos[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdArmaDestino", armas.IdArmaDestino);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ArmasDestinosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ArmasDestinosConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("ArmasDestinosConsultar.aspx");
                //ibtnConsultar.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisArmasDestinos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiArmasDestinos parametros = this.BusquedaParametrosObtenerValor<AfiArmasDestinos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiArmasDestinos>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisArmasDestinos;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisArmasDestinos = this.OrdenarGrillaDatos<AfiArmasDestinos>(this.MisArmasDestinos, e);
            this.gvDatos.DataSource = this.MisArmasDestinos;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisArmasDestinos;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

    
        private void CargarLista(AfiArmasDestinos pArmasDestinos)
        {
            pArmasDestinos.Arma.IdArma = this.txtCodigoArma.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoArma.Text);
            pArmasDestinos.Destino= this.txtDestino.Text;

            pArmasDestinos.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<AfiArmasDestinos>(pArmasDestinos);
            this.MisArmasDestinos = AfiliadosF.ArmasDestinosObtenerListaFiltro(pArmasDestinos);
            this.gvDatos.DataSource = this.MisArmasDestinos;
            this.gvDatos.PageIndex = pArmasDestinos.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisArmasDestinos.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
         
         
    }
}