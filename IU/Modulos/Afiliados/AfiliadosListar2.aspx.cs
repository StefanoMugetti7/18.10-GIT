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
using Afiliados;
using Afiliados.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosListar2 : PaginaSegura
    {
        private DataTable MisAfiliados
        {
            get { return (DataTable)Session[this.MiSessionPagina + "AfiliadosListarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "AfiliadosListarMisAfiliados"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                paginaAfi.Guardar(this.MiSessionPagina, new AfiAfiliados());
                //paginaAfi.MiAfiliado = new AfiAfiliados();

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNombre, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("AfiliadosAgregar.aspx");
                this.CargarCombos();

                AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSocio.Text = parametros.NumeroSocio;
                    this.ddlTipoDocumento.SelectedValue = parametros.TipoDocumento.IdTipoDocumento.ToString();
                    this.txtNumeroDocumento.Text = parametros.NumeroDocumento.ToString();
                    this.txtApellido.Text = parametros.Apellido;
                    this.txtNombre.Text = parametros.Nombre;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlAlertas.SelectedValue = parametros.AlertaTipo.IdAlertaTipo.ToString();
                    this.ddlCategoria.SelectedValue = parametros.Categoria.IdCategoria.ToString();
                    this.chkFamiliares.Checked = parametros.IdAfiliadoRef == 0 ? false : true;
                    this.txtMatricula.Text = parametros.MatriculaIAF.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosAgregar.aspx"), true);
        }

        protected void gvDatos_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            gv.UseAccessibleHeader = true;
            if (gv.ShowHeader == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            AfiAfiliados afiliado = new AfiAfiliados();
            afiliado.IdAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());
            afiliado.IdAfiliadoRef = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliadoRef"].ToString());

            if (afiliado.IdAfiliadoRef > 0)
            {
                int idref=afiliado.IdAfiliadoRef;
                afiliado=new AfiAfiliados();
                afiliado.IdAfiliado=idref;
                afiliado.TipoDocumento.IdTipoDocumento = -1;
                afiliado.Estado.IdEstado = (int)EstadosTodos.Todos;
                afiliado = AfiliadosF.AfiliadosObtenerListaFiltro(afiliado)[0];
            }
            //string parametros = string.Format("?Gestion={0}&IdAfiliado={1}", e.CommandName, afiliado.IdAfiliado);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdAfiliado", afiliado.IdAfiliado);

            afiliado = AfiliadosF.AfiliadosObtenerDatos(afiliado);
            PaginaAfiliados paginaAfi = new PaginaAfiliados();
            paginaAfi.Guardar(this.MiSessionPagina, afiliado);
            //paginaAfi.MiAfiliado = afiliado;
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                //string url = string.Concat("~/Modulos/Afiliados/PosicionGlobal.aspx", parametros);
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                TGEPlantillas plantilla = new TGEPlantillas();
                plantilla.Codigo = "AfiliadoSolicitudIngreso";
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                if (plantilla.HtmlPlantilla.Trim().Length > 0)
                {
                    TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.AltaAfiliado);
                    DataSet ds = ExportPDF.ObtenerDatosReporteComprobante(paginaAfi.Obtener(this.MiSessionPagina), comprobante);
                    ExportPDF.ConvertirHtmlEnPdf(this.UpdatePanel1, plantilla, ds, this.UsuarioActivo);
                }
                else
                {
                    this.ctrPopUpComprobantes.CargarReporte(paginaAfi.Obtener(this.MiSessionPagina), EnumTGEComprobantes.AltaAfiliado);
                    this.UpdatePanel1.Update();
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("AfiliadosConsultar.aspx");
                ibtnImprimir.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisAfiliados.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisAfiliados;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisAfiliados = this.OrdenarGrillaDatos<AfiAfiliados>(this.MisAfiliados, e);
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.DataBind();
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MisAfiliados);
        }

        private void CargarCombos()
        {
            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            this.ddlTipoDocumento.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlTipoDocumento.SelectedValue = "-1";

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosAfiliados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstados.SelectedValue = ((int)EstadosAfiliados.Vigente).ToString();
            //this.ddlEstados.SelectedValue = "-1";

            this.ddlAlertas.DataSource = AfiliadosF.AlertasTiposObtenerListaFiltro(new AfiAlertasTipos());
            this.ddlAlertas.DataValueField = "IdAlertaTipo";
            this.ddlAlertas.DataTextField = "AlertaTipo";
            this.ddlAlertas.DataBind();
            this.ddlAlertas.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "0"));
            this.ddlAlertas.SelectedValue = "0";

            AfiAfiliados afiFiltro = new AfiAfiliados();
            afiFiltro.Estado.IdEstado = (int)Estados.Activo;
            this.ddlCategoria.DataSource = AfiliadosF.CategoriasObtenerListaActiva(afiFiltro);
            this.ddlCategoria.DataValueField = "IdCategoria";
            this.ddlCategoria.DataTextField = "Categoria";
            this.ddlCategoria.DataBind();
            this.ddlCategoria.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "0"));
            this.ddlCategoria.SelectedValue = "0";

        }

        private void CargarLista(AfiAfiliados pAfiliado)
        {
            //pAfiliado.UsuarioActivo = this.UsuarioActivo;
            pAfiliado.NumeroSocio = this.txtNumeroSocio.Text.Trim();
            pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.Apellido = this.txtApellido.Text.Trim();
            pAfiliado.Nombre = this.txtNombre.Text.Trim();
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pAfiliado.IdAfiliadoRef = this.chkFamiliares.Checked ? -1 : 0;
            pAfiliado.AlertaTipo.IdAlertaTipo = Convert.ToInt32(this.ddlAlertas.SelectedValue);
            pAfiliado.Categoria.IdCategoria = Convert.ToInt32(this.ddlCategoria.SelectedValue);
            pAfiliado.MatriculaIAF = this.txtMatricula.Text == string.Empty ? 0 : Convert.ToInt64(this.txtMatricula.Text);
            pAfiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Socios;
            pAfiliado.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(pAfiliado);
            this.MisAfiliados = AfiliadosF.AfiliadosObtenerGrilla(pAfiliado);
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.PageIndex = pAfiliado.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisAfiliados.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}
