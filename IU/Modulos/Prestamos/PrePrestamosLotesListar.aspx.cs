using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using Proveedores;
using Proveedores.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Prestamos
{
    public partial class PrePrestamosLotesListar : PaginaSegura
    {
        private DataTable MisPrestamosLotes
        {
            get { return (DataTable)Session[MiSessionPagina + "PrePrestamosLotesListarMisPrestamosLotes"]; }
            set { Session[MiSessionPagina + "PrePrestamosLotesListarMisPrestamosLotes"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //PrePrestamosLotes pParametro = new PrePrestamosLotes();
                //pParametro.IdPrestamoLote = Convert.ToInt32(MisParametrosUrl["IdPrestamoLote"]);
                this.CargarCombos();

                PrePrestamosLotes parametros = BusquedaParametrosObtenerValor<PrePrestamosLotes>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlInversor.SelectedValue = parametros.Proveedor.IdProveedor.ToString();
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
                //CargarLista(pParametro);
            }
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestamosLotes));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            if (this.ddlEstados.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, ObtenerMensajeSistema("SeleccioneOpcion"));

            CapProveedoresPorcentajesComisiones inversor = new CapProveedoresPorcentajesComisiones();
            inversor.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.LiquidacionInversores;
            List<CapProveedores> capProveedores = ProveedoresF.CapProveedoresPorcentajesComisionesObtenerProveedores(inversor);
            this.ddlInversor.DataSource = capProveedores;
            this.ddlInversor.DataValueField = "IdProveedor";
            this.ddlInversor.DataTextField = "RazonSocial";
            this.ddlInversor.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlInversor, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PrePrestamosLotes parametros = BusquedaParametrosObtenerValor<PrePrestamosLotes>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrePrestamosLotesAgregar.aspx"), true);
        }
        #region Cargar Lista
        private void CargarLista(PrePrestamosLotes pParametro)
        {
            pParametro.Proveedor.IdProveedor = this.ddlInversor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlInversor.SelectedValue);
            pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? (int)EstadosTodos.Todos : Convert.ToInt32(this.ddlEstados.SelectedValue);

            this.MisPrestamosLotes = PrePrestamosF.PrestamosLotesObtenerGrilla(pParametro);
            this.gvDatos.DataSource = this.MisPrestamosLotes;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatos);
        }
        #endregion
        #region Grilla 
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            PrePrestamosLotes prestamoLote = new PrePrestamosLotes();
            prestamoLote.IdPrestamoLote = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdPrestamoLote"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "Gestion", e.CommandName },
                { "IdPrestamoLote", prestamoLote.IdPrestamoLote }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrePrestamosLotesModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrePrestamosLotesConsultar.aspx"), true);
            else if (e.CommandName == "Autorizar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrePrestamosLotesModificar.aspx"), true);
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                DataRowView dr = (DataRowView)e.Row.DataItem;
                switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                {
                    case (int)EstadosPrestamosLotes.Activo:
                        ibtnModificar.Visible = true;
                        ibtnConsultar.Visible = true;
                        break;
                    case (int)EstadosPrestamosLotes.Baja:
                        break;
                    case (int)EstadosPrestamosLotes.PendienteConfirmacion:
                        ibtnModificar.Visible = true;
                        ibtnConsultar.Visible = true;
                        break;
                    case (int)EstadosPrestamos.Confirmado:
                        ibtnModificar.Visible = false;
                        ibtnConsultar.Visible = true;
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestamosLotes.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        #endregion
    }
}