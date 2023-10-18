using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Facturas;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using System.Data;

namespace IU.Modulos.Facturas.Controles
{
    public partial class FacturasBuscarPopUp : ControlesSeguros
    {
        private List<VTAFacturas> MisFacturas //solo para cargar combo
        {
            get { return (List<VTAFacturas>)Session[this.MiSessionPagina + "FacturasListarMisFacturas"]; }
            set { Session[this.MiSessionPagina + "FacturasListarMisFacturas"] = value; }
        }

        private VTAFacturas MiFactura
        {
            get { return (VTAFacturas)Session[this.MiSessionPagina + "FacturasListarMiFactura"]; }
            set { Session[this.MiSessionPagina + "FacturasListarMiFactura"] = value; }
        }

        private DataTable MisDetalles //dato a grillar
        {
            get { return (DataTable)Session[this.MiSessionPagina + "FacturasListarMisDetalles"]; }
            set { Session[this.MiSessionPagina + "FacturasListarMisDetalles"] = value; }
        }

        private DataTable MisAcopios
        {
            get { return (DataTable)Session[this.MiSessionPagina + "FacturasListarMisAcopios"]; }
            set { Session[this.MiSessionPagina + "FacturasListarMisAcopios"] = value; }
        }

        public delegate void FacturasBuscarEventHandler(List<VTAFacturasDetalles> e);
        public event FacturasBuscarEventHandler FacturasBuscarSeleccionar;

        public delegate void FacturasBuscarAcopioEventHandler(VTAFacturas e);
        public event FacturasBuscarAcopioEventHandler FacturasBuscarAcopio;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroFactura, this.btnBuscar);

            }
        }

        public void IniciarControl(VTAFacturas pFactura)
        {
            MiFactura = pFactura;
            this.MisFacturas = FacturasF.RemitosObtenerFacturasPendientesARemitarPorIdAfiliado(MiFactura);
            //valida que no haya cambiado el Cliente, y si cambio limpia indice y grilla
            this.CargarCombos();
            CargarLista();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarFactura();", true);
        }

        private void CargarCombos()
        {
            this.ddlFacturas.DataSource = this.MisFacturas;
            //this.ddlTipoFactura.DataSource = TGEGeneralesF.TGETiposFacturasObtenerLista();
            this.ddlFacturas.DataValueField = "IdFactura";
            this.ddlFacturas.DataTextField = "NumeroFacturaCompleto";
            this.ddlFacturas.DataBind();
            if(this.MisFacturas.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFacturas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            
        }

        protected void ddlFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            this.CargarLista();
        }

        //private void PersistirDatosGrilla()
        //{
        //    foreach (GridViewRow fila in this.gvDatos.Rows)
        //    {
        //        if (fila.RowType == DataControlRowType.DataRow)
        //        {
        //            CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
        //            this.MisDetalles[fila.DataItemIndex].Incluir = chkIncluir.Checked;
        //        }
        //    }
        //}

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDetalles.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        //protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvDatos.PageIndex = e.NewPageIndex;
        //    gvDatos.DataSource = this.MisDetalles;
        //    gvDatos.DataBind();
        //}

        //protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    this.MisDetalles = this.OrdenarGrillaDatos<VTAFacturasDetalles>(this.MisDetalles, e);
        //    this.gvDatos.DataSource = this.MisDetalles;
        //    this.gvDatos.DataBind();
        //}

        protected void btnAceptarAcopio_Click(object sender, EventArgs e)
        {
            VTAFacturas item = new VTAFacturas();
            foreach (GridViewRow fila in this.gvAcopios.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    DataRow row = this.MisAcopios.AsEnumerable().Where(dr => dr.Field<int>("IdFactura") == Convert.ToInt32(this.gvAcopios.DataKeys[fila.RowIndex].Value)).FirstOrDefault();
                    CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                    if (chkIncluir.Checked)
                    {
                        item.IdFactura = (int)row["IdFactura"];
                        item.FechaFactura = Convert.ToDateTime(row["FechaFactura"]);
                        item.Filtro = row["TipoNumeroFactura"].ToString();
                        item.ImporteSinIVA = (decimal)row["ImporteSinIVA"];
                        item.ImporteParcial = (decimal)row["ImporteRecibido"];
                        break;
                    }
                }
            }

            if (this.FacturasBuscarAcopio != null)
            {
                this.FacturasBuscarAcopio(item);
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            List<VTAFacturasDetalles> lista = new List<VTAFacturasDetalles>();
            //si no hay ninguna factura, no existen items a mapear por lo que solo escondo el PopUp
            if (this.MisDetalles.Rows.Count > 0)
            {
                VTAFacturasDetalles item;
                foreach (GridViewRow fila in this.gvDatos.Rows)
                {
                    if (fila.RowType == DataControlRowType.DataRow)
                    {
                        DataRow row = this.MisDetalles.AsEnumerable().Where(dr => dr.Field<int>("IdFacturaDetalle") == Convert.ToInt32(this.gvDatos.DataKeys[fila.RowIndex].Value)).FirstOrDefault();
                        CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                        if (chkIncluir.Checked)
                        {
                            item = new VTAFacturasDetalles();
                            item.IdFacturaDetalle = (int)row["IdFacturaDetalle"];
                            item.ListaPrecioDetalle.Producto.IdProducto = (int)row["IdProducto"];
                            item.ListaPrecioDetalle.Producto.Descripcion = row["ProductoDescripcion"].ToString();
                            item.DescripcionProducto = row["ProductoDescripcion"].ToString();
                            item.Descripcion = row["Descripcion"].ToString();
                            item.TipoNumeroFactura = row["TipoNumeroFactura"].ToString();
                            item.ListaPrecioDetalle.StockActual = (decimal)row["StockActual"];
                            item.CantidadEntregada = (decimal)row["CantidadEntregada"];
                            item.PrecioUnitarioSinIva = (decimal)row["PrecioUnitarioSinIva"];
                            item.DescuentoPorcentual = (decimal)row["DescuentoPorcentual"];
                            item.IVA.IdIVA = (int)row["IdIva"];
                            /* Modificacion: Punto Obra --> no traía la cantidad restante correctamente. 
                             * Fecha: 02/06/2021 | Programador: Ornella Leiva  |
                             * Se cambio: item.CantidadRestante = (decimal)row["Cantidad"]; */
                            item.CantidadRestante = (decimal)row["CantidadPendiente"];
                            lista.Add(item);
                        }
                    }
                }
            }
            if (this.FacturasBuscarSeleccionar != null)
            {
                this.FacturasBuscarSeleccionar(lista);
            }
        }

        private void CargarLista()
        {
            MiFactura.IdFactura = this.ddlFacturas.SelectedValue==string.Empty ? 0: Convert.ToInt32(this.ddlFacturas.SelectedValue);

            this.MisAcopios = FacturasF.FacturasObtenerDetallesAcopiosPendienteEntrega(MiFactura);
            this.phAcopios.Visible = this.MisAcopios.Rows.Count > 0;
            this.gvAcopios.DataSource = this.MisAcopios;
            this.gvAcopios.DataBind();

            this.MisDetalles = FacturasF.FacturasObtenerDetallesPendienteEntrega(MiFactura);
            this.phDatos.Visible = this.MisDetalles.Rows.Count > 0;
            this.gvDatos.DataSource = this.MisDetalles;
            this.gvDatos.DataBind();
        }
    }
}