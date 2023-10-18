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
using Ahorros.Entidades;
using Ahorros;
using Comunes.Entidades;
using System.Collections.Generic;
using Generales.Entidades;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Ahorros
{
    public partial class PlazosFijosListar : PaginaAfiliados
    {
        private List<AhoPlazosFijos> MisPlazosFijos2
        {
            get { return (List<AhoPlazosFijos>)Session[this.MiSessionPagina + "PlazosFijosListarMisPlazosFijos"]; }
            set { Session[this.MiSessionPagina + "PlazosFijosListarMisPlazosFijos"] = value; }
        }
        private DataTable MisPlazosFijos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PlazosFijosListarMisPlazosFijos"]; }
            set { Session[this.MiSessionPagina + "PlazosFijosListarMisPlazosFijos"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("PlazosFijosAgregar.aspx");
                AhoPlazosFijos plazosFijos = new AhoPlazosFijos();
                plazosFijos.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.CargarLista(plazosFijos);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            DataRow row = this.MisPlazosFijos.AsEnumerable().FirstOrDefault(x => x.Field <int>("IdPlazoFijo") == indiceColeccion);
            AhoPlazosFijos plazoFijos = new AhoPlazosFijos();
            Servicio.AccesoDatos.Mapeador.SetearEntidadPorfila(row, plazoFijos, EstadoColecciones.SinCambio); //this.MisPlazosFijos[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPlazoFijo", plazoFijos.IdPlazoFijo);
            this.MisParametrosUrl.Add("IdPlazoFijoAnterior", plazoFijos.IdPlazoFijoAnterior);
            //ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            //parametros.Agregar("IdPlazoFijo", indiceColeccion);//proveedor.IdProveedor);

            if (e.CommandName == "Cancelar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosCancelar.aspx"), true);
            else if (e.CommandName == "AnularCancelacion")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosAnularCancelacion.aspx"), true);
            else if (e.CommandName == "Anular")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosAnular.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosConsultar.aspx"), true);
            else if (e.CommandName == "Renovar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosRenovar.aspx"), true);
            else if (e.CommandName == "Pagar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosPagar.aspx"), true);
            else if (e.CommandName == "Modificar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosModificar.aspx"), true);
            else if (e.CommandName == "AnularRenovacion")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosRenovacionAnticipada.aspx"), true);
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.AhoPlazosFijos, "AhorroPlazoFijo", plazoFijos, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this, "AhorroPlazoFijo", this.UsuarioActivo);



            }

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnAnularCancelacion = (ImageButton)e.Row.FindControl("btnAnularCancelacion");
                ImageButton ibtnCancelar = (ImageButton)e.Row.FindControl("btnCancelar");
                ImageButton ibtnRenovar = (ImageButton)e.Row.FindControl("btnRenovar");
                ImageButton ibtnAnularRenovacion = (ImageButton)e.Row.FindControl("btnAnularRenovacion");
                ImageButton ibtnPagar = (ImageButton)e.Row.FindControl("btnPagar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnConsultar.Visible = this.ValidarPermiso("PlazosFijosConsultar.aspx");

                //AhoPlazosFijos plazoFijo = (AhoPlazosFijos)e.Row.DataItem;

                //if (ibtnCancelar.Visible)
                //{
                //    string mensaje = this.ObtenerMensajeSistema("PlazosFijosConfirmarCancelar");
                //    mensaje = string.Format(mensaje, plazoFijo.IdPlazoFijo);
                //    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                //    ibtnCancelar.Attributes.Add("OnClick", funcion);
                //}

                if ((Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosPlazosFijos.Confirmado) && (Convert.ToInt32(dr["IdPlazoFijoAnterior"]) > 0))
                {
                    ibtnAnularRenovacion.Visible = true;
                }
                if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosPlazosFijos.Confirmado)
                {
                    if ((Convert.ToDateTime(dr["FechaVencimiento"])) > DateTime.Now.Date)
                    {
                        ibtnCancelar.Visible = this.ValidarPermiso("PlazosFijosCancelar.aspx");
                        ibtnModificar.Visible = this.ValidarPermiso("PlazosFijosModificar.aspx");
                        ibtnAnularRenovacion.Visible = this.ValidarPermiso("PlazosFijosRenovacionAnticipada.aspx");
                    }
                    else
                    {
                       
                        ibtnRenovar.Visible = this.ValidarPermiso("PlazosFijosRenovar.aspx");
                        ibtnPagar.Visible = this.ValidarPermiso("PlazosFijosPagar.aspx");
                    }
                }
                if ((Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosPlazosFijos.PendienteConfirmacion) || (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosPlazosFijos.PendientePago))
                {
                    ibtnAnular.Visible = this.ValidarPermiso("PlazosFijosAnular.aspx");

                }
                if ((Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosPlazosFijos.PendienteCancelacion))
                {
                    ibtnAnularCancelacion.Visible = this.ValidarPermiso("PlazosFijosAnularCancelacion.aspx");

                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPlazosFijos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AhoPlazosFijos parametros = this.BusquedaParametrosObtenerValor<AhoPlazosFijos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AhoPlazosFijos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisPlazosFijos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPlazosFijos = this.OrdenarGrillaDatos<AhoPlazosFijos>(this.MisPlazosFijos, e);
            this.gvDatos.DataSource = this.MisPlazosFijos;
            this.gvDatos.DataBind();
        }

        private void CargarLista(AhoPlazosFijos pPlazosFijos)
        {
            this.MisPlazosFijos = AhorroF.PlazosFijosObtenerListaFiltroDT(pPlazosFijos);
            this.gvDatos.DataSource = this.MisPlazosFijos;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);
        }

    }
}