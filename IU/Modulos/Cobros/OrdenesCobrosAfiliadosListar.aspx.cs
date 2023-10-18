using Cobros;
using Cobros.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosAfiliadosListar : PaginaAfiliados
    {
        private DataTable MisOrdenesCobros
        {
            get { return (DataTable)Session[this.MiSessionPagina + "OrdenesCobrosAfiliadosListarMisOrdenesCobros"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosAfiliadosListarMisOrdenesCobros"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvDatos.PageSizeEvent += this.GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
                if (parametros.BusquedaParametros)
                {
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
                this.btnAgregar.Visible = this.ValidarPermiso("OrdenesCobrosAfiliadosAgregar.aspx");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosAgregar.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idOrdenCobro = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
            ordenCobro.IdOrdenCobro = idOrdenCobro;
            ordenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(ordenCobro);

            this.MisParametrosUrl = new Hashtable
            {
                { "IdOrdenCobro", ordenCobro.IdOrdenCobro },
                { "IdTipoOperacion", ordenCobro.TipoOperacion.IdTipoOperacion }
            };
            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.AnularConfirmar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosAnularConfirmada.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.EnviarMail.ToString())
            {
                MailMessage mail = new MailMessage();
                ordenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                ordenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(ordenCobro);
                if (CobrosF.OrdenesCobroArmarMail(ordenCobro, mail))
                {

                    this.popUpMail.IniciarControl(mail, ordenCobro);
                }
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                CobOrdenesCobros obj = new CobOrdenesCobros();
                obj.Filtro = obj.IdOrdenCobro.ToString();
                obj.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                obj.TipoOperacion.IdTipoOperacion = ordenCobro.TipoOperacion.IdTipoOperacion;
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(obj);
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, miPlantilla.Codigo, ordenCobro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this, "OrdenesCobrosAfiliados", this.UsuarioActivo);
                this.UpdatePanel1.Update();
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                consultar.Visible = this.ValidarPermiso("OrdenesCobrosAfiliadosConsultar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;
                switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                {
                    case (int)EstadosOrdenesCobro.Activo:
                        ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                        anular.Visible = this.ValidarPermiso("OrdenesCobrosAfiliadosAnular.aspx");
                        break;
                    case (int)EstadosOrdenesCobro.Cobrado:
                        ImageButton anularC = (ImageButton)e.Row.FindControl("btnAnularConfirmar");
                        anularC.Visible = this.ValidarPermiso("OrdenesCobrosAfiliadosAnularConfirmada.aspx");
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisOrdenesCobros.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisOrdenesCobros = this.OrdenarGrillaDatos<CobOrdenesCobros>(this.MisOrdenesCobros, e);
            this.gvDatos.DataSource = this.MisOrdenesCobros;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";
        }

        private void CargarLista(CobOrdenesCobros pParametro)
        {
            pParametro.Afiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados;
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CobOrdenesCobros>(pParametro);
            this.MisOrdenesCobros = CobrosF.OrdenesCobrosListaFiltroDT(pParametro);
            this.gvDatos.DataSource = this.MisOrdenesCobros;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}