using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Haberes.Entidades;
using Haberes;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Afiliados.Entidades;
using Comunes.Entidades;
using System.Data;

namespace IU.Modulos.Haberes
{
    public partial class Imprimir : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                CargarLista(new HabRecibosCom());
            }
        }

        private void CargarLista(HabRecibosCom pAfiliado)
        {
            pAfiliado.IdAfiliado = MiAfiliado.IdAfiliado;
            pAfiliado.BusquedaParametros = true;
            pAfiliado.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pAfiliado.PageSize;
            this.BusquedaParametrosGuardarValor<HabRecibosCom>(pAfiliado);

            DataTable dt = HaberesF.RecibosComSeleccionarGrilla(pAfiliado);
            this.gvDatos.DataSource = dt;
            this.gvDatos.VirtualItemCount = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            //if (dt.Rows.Count)
            //    btnExportarExcel.Visible = true;
            //else
            //    btnExportarExcel.Visible = false;
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            HabRecibosCom parametros = this.BusquedaParametrosObtenerValor<HabRecibosCom>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Impresion.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            
            if (e.CommandName == Gestion.Impresion.ToString())
            {
                RepReportes reporte = new RepReportes();
                reporte.IdReporte = (int)EnumReportes.RecibosHaberes;
                reporte = ReportesF.ReportesObtenerUno(reporte);
                reporte.Parametros.Find(x => x.Parametro == "Periodo").ValorParametro = Convert.ToInt32(((GridView)sender).DataKeys[index]["Periodo"].ToString());
                reporte.Parametros.Find(x => x.Parametro == "IdTipoRecibo").ValorParametro = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoRecibo"].ToString());
                if (reporte.Parametros.Exists(x => x.Parametro == "IdAfiliado"))
                    reporte.Parametros.Find(x => x.Parametro == "IdAfiliado").ValorParametro = MiAfiliado.IdAfiliado;
                else
                {
                    RepParametros paramAfi = new RepParametros();
                    paramAfi.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                    paramAfi.Parametro = "IdAfiliado";
                    paramAfi.ValorParametro = MiAfiliado.IdAfiliado;
                    reporte.Parametros.Add(paramAfi);
                }

                reporte.AppPath = System.AppDomain.CurrentDomain.BaseDirectory;
                reporte.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                TGEArchivos archivo = HaberesF.HabRecibosReporteRecibosCOMIAF(reporte);
                 ExportPDF.ExportarPDF(archivo.NombreArchivo, this.UpdatePanel1);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                //ibtnImprimir.Visible = true;
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            HabRecibosCom parametros = BusquedaParametrosObtenerValor<HabRecibosCom>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;

            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //MisAfiliados = OrdenarGrillaDatos<AfiAfiliados>(MisAfiliados, e);
            //gvDatos.DataSource = MisAfiliados;
            //gvDatos.DataBind();
        }

    }
}
