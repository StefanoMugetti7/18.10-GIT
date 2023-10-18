using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using Proveedores;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PrePrestamosLotesDatos : ControlesSeguros
    {
        private PrePrestamosLotes MiPrestamoLote
        {
            get { return (PrePrestamosLotes)Session[MiSessionPagina + "PrePrestamosLotesDatosMiPrestamoLote"]; }
            set { Session[MiSessionPagina + "PrePrestamosLotesDatosMiPrestamoLote"] = value; }
        }

        private DataTable MisPrestamosLotesSeleccionados
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PrePrestamosLotesDatosMisPrestamosLotesSeleccionados"]; }
            set { Session[this.MiSessionPagina + "PrePrestamosLotesDatosMisPrestamosLotesSeleccionados"] = value; }
        }

        private DataTable MisPrestamosLotesBusqueda
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PrePrestamosLotesDatosMisPrestamosLotesBusqueda"]; }
            set { Session[this.MiSessionPagina + "PrePrestamosLotesDatosMisPrestamosLotesBusqueda"] = value; }
        }

        public delegate void PrePrestamosLotesDatossCancelarEventHandler();
        public event PrePrestamosLotesDatossCancelarEventHandler PrestamosLotesModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!IsPostBack)
            {

            }
        }

        public void IniciarControl(PrePrestamosLotes pPrestamoLote, Gestion pGestion)
        {
            GestionControl = pGestion;
            MiPrestamoLote = pPrestamoLote;
            CargarCombos();
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    txtNumeroLote.Enabled = false;
                    ddlEstado.SelectedValue = ((int)EstadosPrestamosLotes.PendienteConfirmacion).ToString();
                    break;
                case Gestion.Modificar:
                    MiPrestamoLote = PrePrestamosF.PrestamosLotesObtenerDatosCompletos(pPrestamoLote);
                    MapearObjetoControles(MiPrestamoLote);
                    break;
                case Gestion.Consultar:
                    MiPrestamoLote = PrePrestamosF.PrestamosLotesObtenerDatosCompletos(pPrestamoLote);
                    MapearObjetoControles(MiPrestamoLote);
                    txtNumeroLote.Enabled = false;
                    ddlInversor.Enabled = false;
                    txtTasaInversor.Enabled = false;
                    txtDetalle.Enabled = false;
                    ddlEstado.Enabled = false;
                    tpBusquedaPrestamos.Visible = false;
                    break;
                default:
                    break;
            }
        }

        #region Mapeo
        protected void MapearControlesAObjeto(PrePrestamosLotes pPrestamoLote)
        {
            pPrestamoLote.Proveedor.IdProveedor = Convert.ToInt32(ddlInversor.SelectedValue);
            pPrestamoLote.Proveedor.RazonSocial = ddlInversor.SelectedItem.Text;
            pPrestamoLote.Estado.IdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            pPrestamoLote.Estado.Descripcion = ddlEstado.SelectedItem.Text;
            pPrestamoLote.TasaInversor = Convert.ToDecimal(txtTasaInversor.Text);
            pPrestamoLote.Detalle = txtDetalle.Text;
            //cantidad?
            //pPrestamoLote.ImporteTotal = Convert.ToDecimal(txtImporteTotal.Text); ?
        }

        private void MapearObjetoControles(PrePrestamosLotes pPrestamoLote)
        {
            txtNumeroLote.Text = pPrestamoLote.IdPrestamoLote.ToString();
            ListItem item = ddlInversor.Items.FindByValue(pPrestamoLote.Proveedor.IdProveedor.ToString());
            if (item == null)
                ddlInversor.Items.Add(new ListItem(pPrestamoLote.Proveedor.RazonSocial, pPrestamoLote.Proveedor.IdProveedor.Value.ToString()));
            ddlInversor.SelectedValue = pPrestamoLote.Proveedor.IdProveedor.Value.ToString();
            txtTasaInversor.Text = pPrestamoLote.TasaInversor.ToString();
            txtDetalle.Text = pPrestamoLote.Detalle;
            ddlEstado.SelectedValue = pPrestamoLote.Estado.IdEstado.ToString();
            MisPrestamosLotesSeleccionados = pPrestamoLote.DetalleSeleccionado;
            MostrarGrilla();
        }
        #endregion

        #region Botones
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            btnAceptar.Visible = false;
            if (MisPrestamosLotesSeleccionados.Rows.Count <= 0)
            {
                MostrarMensaje("Debe ingresar al menos un prestamo", true);
                return;
            }
            MapearControlesAObjeto(MiPrestamoLote);
            MiPrestamoLote.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            MiPrestamoLote.IdUsuarioAlta = UsuarioActivo.IdUsuarioEvento;
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    MisPrestamosLotesSeleccionados.TableName = "PrestamosLotesSeleccionados";
                    MiPrestamoLote.LotePrestamosLotes = MisPrestamosLotesSeleccionados.ToXmlDocument();
                    guardo = PrePrestamosF.PrestamosLotesAgregar(MiPrestamoLote);
                    break;
                case Gestion.Modificar:
                    MiPrestamoLote.DetalleSeleccionado = MisPrestamosLotesSeleccionados;
                    guardo = PrePrestamosF.PrestamosLotesModificar(MiPrestamoLote);
                    break;
                default:
                    break;
            }
            if (ddlEstado.SelectedValue == ((int)EstadosPrestamosLotes.Confirmado).ToString())
            {
                MiPrestamoLote.IdUsuarioConfirmacion = UsuarioActivo.IdUsuarioEvento;
                MiPrestamoLote.FechaConfirmacion = DateTime.Now;
            }
            if (guardo)
            {
                MostrarMensaje(MiPrestamoLote.CodigoMensaje, false);
            }
            else
            {
                btnAceptar.Visible = true;
                MostrarMensaje(MiPrestamoLote.CodigoMensaje, true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            PrestamosLotesModificarDatosCancelar?.Invoke();
        }
        #endregion

        #region Combos
        private void CargarCombos()
        {
            CapProveedoresPorcentajesComisiones inversor = new CapProveedoresPorcentajesComisiones();
            inversor.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.LiquidacionInversores;
            List<CapProveedores> capProveedores = ProveedoresF.CapProveedoresPorcentajesComisionesObtenerProveedores(inversor);
            ddlInversor.DataSource = capProveedores;
            ddlInversor.DataValueField = "IdProveedor";
            ddlInversor.DataTextField = "RazonSocial";
            ddlInversor.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlInversor, ObtenerMensajeSistema("SeleccioneOpcion"));

            ddlInversorGrilla.DataSource = capProveedores;
            ddlInversorGrilla.DataValueField = "IdProveedor";
            ddlInversorGrilla.DataTextField = "RazonSocial";
            ddlInversorGrilla.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlInversorGrilla, ObtenerMensajeSistema("SeleccioneOpcion"));

            ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestamosLotes));
            ddlEstado.DataValueField = "IdEstado";
            ddlEstado.DataTextField = "Descripcion";
            ddlEstado.DataBind();
        }

        #endregion

        #region Inversores
        protected void ddlInversor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlInversor.SelectedValue))
            {
                MiPrestamoLote.Proveedor.IdProveedor = Convert.ToInt32(ddlInversor.SelectedValue);
                List<TGECampos> campos = TGEGeneralesF.CamposObtenerListaFiltro(MiPrestamoLote.Proveedor, new Objeto());
                TGECampos campoInversor = campos.FirstOrDefault(x => x.Nombre == "TasaInversor");
                if (campoInversor != null)
                {
                    MiPrestamoLote.TasaInversor = string.IsNullOrWhiteSpace(campoInversor.CampoValor.Valor) ? 0 : Convert.ToDecimal(campoInversor.CampoValor.Valor);
                    txtTasaInversor.Decimal = MiPrestamoLote.TasaInversor;
                }
                else {
                    MiPrestamoLote.TasaInversor = 0;
                    txtTasaInversor.Decimal = MiPrestamoLote.TasaInversor;
                }
                MisPrestamosLotesSeleccionados = PrePrestamosF.PrestamosLotesObtenerPorProveedor(MiPrestamoLote);
                gvDatos.DataSource = MisPrestamosLotesSeleccionados;
                gvDatos.DataBind();
                upPrestamosSeleccionados.Update();
            }
        }
        #endregion

        #region Grillas

        #region Grilla Prestamos Seleccionados
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                DataRow dr = MisPrestamosLotesSeleccionados.AsEnumerable().FirstOrDefault(x => x.Field<int>("IdPrestamo") == id);
                dr["IdEstado"] = 0;
                //MisPrestamosLotesSeleccionados.AcceptChanges(); 
                //NO uso porque AcceptChanges() cambia el RowState de todos los agregados y modificados a Unchanged!!!!!
                MostrarGrilla();
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                string mensaje = "Se eliminará un item";
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                ibtn.Attributes.Add("OnClick", funcion);

                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ibtn.Visible = true;
                        break;
                    case Gestion.Modificar:
                        ibtn.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                //lblImporte.Text = Convert.ToDecimal(MisPrestamosLotesSeleccionados.Compute("Sum(PrestamoImporteAutorizado)", string.Empty)).ToString("C2");
                lblImporte.Text = Convert.ToDecimal(MisPrestamosLotesSeleccionados.AsEnumerable().Where(dr => dr.Field<int>("IdEstado") != 0).Sum(dr => dr.Field<decimal>("PrestamoImporteAutorizado"))).ToString("C2");
                
                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                //lblCantidadRegistros.Text = string.Format(ObtenerMensajeSistema("GrillaTotalRegistros"), MisPrestamosLotesSeleccionados.Rows.Count);
                lblCantidadRegistros.Text = string.Format(ObtenerMensajeSistema("GrillaTotalRegistros"), MisPrestamosLotesSeleccionados.AsEnumerable().Where(dr => dr.Field<int>("IdEstado") != 0).Count());
            }
        }

        private void MostrarGrilla()
        {
            gvDatos.DataSource = MisPrestamosLotesSeleccionados.AsEnumerable().Where(row => row.Field<int>("IdEstado") != 0).CopyToDataTable();
            gvDatos.DataBind();
        }
        #endregion

        #region Grilla Busqueda de Prestamos
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvItems.PageIndex = e.NewPageIndex;
            gvItems.DataSource = MisPrestamosLotesBusqueda;
            gvItems.DataBind();
        }

        protected void btnAceptarIncluir_Click(object sender, EventArgs e)
        {
            PersistirDatosCheck();
            MapearChecked();
            MostrarGrilla();
            upPrestamosSeleccionados.Update();
            btnBuscar_Click(sender, e);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            MiPrestamoLote.Proveedor.IdProveedor = ddlInversorGrilla.SelectedValue == string.Empty ? (Nullable<Int32>)null : Convert.ToInt32(ddlInversorGrilla.SelectedValue);
            MiPrestamoLote.FechaDesde = txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            MiPrestamoLote.FechaHasta = txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            MiPrestamoLote.ImporteDesde = txtImporteDesde.Text == string.Empty ? (Nullable<Decimal>)null : Convert.ToDecimal(txtImporteDesde.Text);
            MiPrestamoLote.ImporteHasta = txtImporteHasta.Text == string.Empty ? (Nullable<Decimal>)null : Convert.ToDecimal(txtImporteHasta.Text);
            MiPrestamoLote.CantidadCuotas = txtCantidadCuotas.Text == string.Empty ? (Nullable<Int32>)null : Convert.ToInt32(txtCantidadCuotas.Text);
            //falta campo dinamico. scoring
            MisPrestamosLotesSeleccionados.TableName = "PrestamosLotesSeleccionados";
            MiPrestamoLote.LotePrestamosLotes = MisPrestamosLotesSeleccionados.ToXmlDocument();

            MisPrestamosLotesBusqueda = PrePrestamosF.PrestamosLotesObtenerPrestamosDisponiblesPorFiltro(MiPrestamoLote);
            gvItems.DataSource = MisPrestamosLotesBusqueda;
            gvItems.DataBind();
            upBusquedaPrestamos.Update();
        }

        private void PersistirDatosCheck()
        {
            int idTable;
            DataRow dr;
            foreach (GridViewRow fila in gvItems.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    idTable = Convert.ToInt32(gvItems.DataKeys[fila.DataItemIndex]["IdPrestamo"].ToString());
                    dr = MisPrestamosLotesBusqueda.AsEnumerable().First(x => x.Field<Int32>("IdPrestamo") == idTable);

                    CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                    if (!chkIncluir.Checked)
                    {
                        MisPrestamosLotesBusqueda.Rows.Remove(dr);
                        MisPrestamosLotesBusqueda.AcceptChanges();
                    }
                }
            }
        }

        private void MapearChecked()
        {
            int filas = 0;
            if (MisPrestamosLotesSeleccionados.Rows.Count <= 0)
            {
                filas = 0;
            }
            else
            {
                filas = MisPrestamosLotesSeleccionados.Rows.Count;
            }
            foreach (DataRow dr in MisPrestamosLotesBusqueda.Rows)
            {
                dr["IdPrestamoLoteDetalle"] = (filas + 1) * -1;
                dr["IdPrestamoLote"] = Convert.ToInt32(txtNumeroLote.Text);
                dr["IdEstado"] = 1;
                MisPrestamosLotesSeleccionados.ImportRow(dr);
                filas++;
                //MisPrestamosLotesSeleccionados.AcceptChanges();
            }
        }
        #endregion

        #endregion
    }
}