using Comunes.Entidades;
using Evol.Controls;
using Generales.FachadaNegocio;
using Producciones;
using Producciones.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace IU.Modulos.Produccion.Controles
{
    public partial class ProduccionesDatos : ControlesSeguros
    {
        public PrdProducciones MiProduccion
        {
            get { return this.PropiedadObtenerValor<PrdProducciones>("ProduccionesDatosMiProduccion"); }
            set { this.PropiedadGuardarValor("ProduccionesDatosMiProduccion", value); }
        }
        //public delegate void ControlDatosAceptarEventHandler(object sender, PrdProducciones e);
        //public event ControlDatosAceptarEventHandler ControlModificarDatosAceptar;
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        public void IniciarControl(PrdProducciones pParametro, Gestion pGestion)
        {
            this.MiProduccion = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            string funcion = string.Format("ValidarShowConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ProduccionValidarFinalizar"));
            this.btnAceptar.Attributes.Add("OnClick", funcion);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.AgregarItem(4);
                    this.ctrArchivos.IniciarControl(this.MiProduccion, this.GestionControl);
                    this.ctrComentarios.IniciarControl(this.MiProduccion, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiProduccion, new Objeto(), this.GestionControl);
                    this.ddlFiliales.Enabled = true;
                    break;
                case Gestion.Modificar:
                    this.MiProduccion = ProduccionesF.ProduccionesObtenerDatosCompletos(this.MiProduccion);
                    this.AgregarItem(2);
                    this.MapearObjetoAControles(this.MiProduccion);
                    break;
                case Gestion.Consultar:
                    this.MiProduccion = ProduccionesF.ProduccionesObtenerDatosCompletos(this.MiProduccion);
                    this.MapearObjetoAControles(this.MiProduccion);
                    this.txtDescripcion.Enabled = false;
                    this.txtFechaInicio.Enabled = false;
                    this.txtFechaFin.Enabled = false;
                    this.txtCantidadProducida.Enabled = false;
                    this.btnAceptar.Visible = false;
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosProducciones));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }
        private void MapearObjetoAControles(PrdProducciones pProduccion)
        {
            this.txtNumeroProduccion.Text = pProduccion.IdProduccion.ToString();
            this.ddlEstado.SelectedValue = pProduccion.Estado.IdEstado.ToString();
            ListItem item = this.ddlFiliales.Items.FindByValue(pProduccion.Filial.IdFilial.ToString());
            if (item == null)
                this.ddlFiliales.Items.Add(new ListItem(pProduccion.Filial.Filial, pProduccion.Filial.IdFilial.ToString()));
            this.ddlFiliales.SelectedValue = pProduccion.Filial.IdFilial.ToString();
            this.txtDescripcion.Text = pProduccion.Descripcion;
            this.txtFechaInicio.Text = pProduccion.FechaInicio.HasValue ? pProduccion.FechaInicio.Value.ToShortDateString() : string.Empty;
            this.txtFechaFin.Text = pProduccion.FechaFin.HasValue ? pProduccion.FechaFin.Value.ToShortDateString() : string.Empty;
            if (pProduccion.Producto.IdProducto > 0)
                this.ddlProductoProducido.Items.Add(new ListItem(pProduccion.Producto.Descripcion, pProduccion.Producto.IdProducto.ToString()));
            this.txtCantidadProducida.Decimal = pProduccion.CantidadProducida;
            DataTable productosTotales = ProduccionesF.ProduccionesObtenerTotalesProductoPorProduccion(pProduccion);
            this.gvTotalesProductos.DataSource = productosTotales;
            this.gvTotalesProductos.DataBind();
            AyudaProgramacion.CargarGrillaListas<PrdProduccionesDetalles>(pProduccion.ProduccionesDetalles, false, this.gvDatos, true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CalcularPrecioScript", "CalcularValorizacion();", true);
            this.ctrArchivos.IniciarControl(pProduccion, this.GestionControl);
            this.ctrComentarios.IniciarControl(pProduccion, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pProduccion);
            this.ctrCamposValores.IniciarControl(pProduccion, new Objeto(), this.GestionControl);
        }
        private void MapearControlesAObjeto(PrdProducciones pProduccion)
        {
            pProduccion.Descripcion = this.txtDescripcion.Text;
            pProduccion.FechaInicio = this.txtFechaInicio.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaInicio.Text);
            pProduccion.FechaFin = this.txtFechaFin.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaFin.Text);
            string key = this.Request.Form.AllKeys.FirstOrDefault(x => !string.IsNullOrEmpty(x) && x.Contains("$ddlProductoProducido"));
            pProduccion.Producto.IdProducto = !string.IsNullOrEmpty(this.Request.Form[key]) ? Convert.ToInt32(this.Request.Form[key]) : 0;
            //pProduccion.Producto.IdProducto = this.ddlProductoProducido.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlProductoProducido.SelectedValue);
            pProduccion.CantidadProducida = this.txtCantidadProducida.Decimal;
            pProduccion.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pProduccion.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pProduccion.Filial.IdFilial = Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pProduccion.Filial.Filial = this.ddlFiliales.SelectedItem.Text;
            pProduccion.Campos = this.ctrCamposValores.ObtenerLista();
            pProduccion.Archivos = this.ctrArchivos.ObtenerLista();
            pProduccion.Comentarios = this.ctrComentarios.ObtenerLista();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiProduccion);
            this.PersistirDatosGrilla();
            this.MiProduccion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ProduccionesF.ProduccionesAgregar(this.MiProduccion);
                    this.MiProduccion.IdProduccion = this.MiProduccion.IdProduccion;
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiProduccion.CodigoMensaje, false, this.MiProduccion.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Anular:
                    this.MiProduccion.Estado.IdEstado = (int)Estados.Baja;
                    guardo = ProduccionesF.ProduccionesModificar(this.MiProduccion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiProduccion.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = ProduccionesF.ProduccionesModificar(this.MiProduccion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiProduccion.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiProduccion.CodigoMensaje, true, this.MiProduccion.CodigoMensajeArgs);
                if (this.MiProduccion.dsResultado != null)
                {
                    //this.ctrPopUpGrilla.IniciarControl(this.MiProduccion);
                    this.MiProduccion.dsResultado = null;
                }
            }
            else
            {
                DataTable productosTotales = ProduccionesF.ProduccionesObtenerTotalesProductoPorProduccion(this.MiProduccion);
                this.gvTotalesProductos.DataSource = productosTotales;
                this.gvTotalesProductos.DataBind();
                this.upTotalesProducto.Update();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DeshabilitarControlesScript", "$(document).ready(function () { deshabilitarControles('deshabilitarControles') }); ;", true);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }
        protected void gvTotalesProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                lblTotal.Text = ((DataTable)this.gvTotalesProductos.DataSource).AsEnumerable().Sum(r => r.Field<decimal?>("SubTotal") ?? 0).ToString("C2");
            }
        }
        #region Detalle
        private void AgregarItem(int cantidad)
        {
            PrdProduccionesDetalles item;
            for (int i = 0; i < cantidad; i++)
            {
                item = new PrdProduccionesDetalles();
                item.Estado.IdEstado = (int)Estados.Activo;
                item.EstadoColeccion = EstadoColecciones.Agregado;
                item.Fecha = DateTime.Now;
                item.UsuarioAlta.Apellido = this.UsuarioActivo.Apellido;
                item.UsuarioAlta.Nombre = this.UsuarioActivo.Nombre;
                this.MiProduccion.ProduccionesDetalles.Add(item);
                item.IndiceColeccion = this.MiProduccion.ProduccionesDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<PrdProduccionesDetalles>(this.MiProduccion.ProduccionesDetalles, true, this.gvDatos, true);
            ScriptManager.RegisterStartupScript(this.upDetalleItems, this.upDetalleItems.GetType(), "CalcularPrecioScript", "CalcularValorizacion();", true);
        }
        private void PersistirDatosGrilla()
        {
            if (this.MiProduccion.ProduccionesDetalles.Count == 0)
                return;

            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("gvDatos")).ToList();
            string k;
            int numeroFila = 2;
            PrdProduccionesDetalles det;
            bool modifica;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    modifica = false;
                    det = this.MiProduccion.ProduccionesDetalles[fila.RowIndex];

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtFecha"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Fecha.Date != Convert.ToDateTime(this.Request.Form[k]).Date)
                            modifica = true;
                        det.Fecha = Convert.ToDateTime(this.Request.Form[k]);
                    }

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$ddTipoMovimiento"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Sentido != Convert.ToInt32(this.Request.Form[k]))
                            modifica = true;
                        det.Sentido = Convert.ToInt32(this.Request.Form[k]);
                    }

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$ddlProductos"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Producto.IdProducto != Convert.ToInt32(this.Request.Form[k]))
                            modifica = true;
                        det.Producto.IdProducto = Convert.ToInt32(this.Request.Form[k]);
                        //det.Producto.Descripcion = this.Request.Form[k];
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfDetalleProducto"));
                    det.Producto.Descripcion = this.Request.Form[k];

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtCantidad"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Cantidad != Convert.ToDecimal(this.Request.Form[k]))
                            modifica = true;
                        det.Cantidad = Convert.ToDecimal(this.Request.Form[k]);
                    }

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfValorizacion"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        det.Valorizacion = Convert.ToDecimal(this.Request.Form[k]);
                    }

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfStockActual"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        det.Producto.StockActual = Convert.ToDecimal(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfUnidadMedidaDescripcion"));
                    det.Producto.UnidadMedida.Descripcion = this.Request.Form[k];

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfIdUnidadMedida"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Producto.UnidadMedida.IdUnidadMedida != Convert.ToInt32(this.Request.Form[k]))
                            modifica = true;
                        det.Producto.UnidadMedida.IdUnidadMedida = Convert.ToInt32(this.Request.Form[k]);
                    }

                    if (modifica)
                        det.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(det, this.GestionControl);
                    numeroFila++;
                }
            }
        }
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            this.AgregarItem(1);
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PrdProduccionesDetalles item = (PrdProduccionesDetalles)e.Row.DataItem;
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                TextBox txtFecha = (TextBox)e.Row.FindControl("txtFecha");
                //Image imgFecha = (Image)e.Row.FindControl("imgFecha");
                DropDownList ddTipoMovimiento = ((DropDownList)e.Row.FindControl("ddTipoMovimiento"));
                DropDownList ddlProductos = ((DropDownList)e.Row.FindControl("ddlProductos"));
                CurrencyTextBox txtCantidad = ((CurrencyTextBox)e.Row.FindControl("txtCantidad"));

                if (item.EstadoColeccion == EstadoColecciones.Agregado)
                {
                    txtFecha.Enabled = true;
                    //imgFecha.Visible = true;
                    ddTipoMovimiento.Enabled = true;
                    ddlProductos.Enabled = true;
                    txtCantidad.Enabled = true;
                    btnEliminar.Visible = true;
                }

                txtFecha.Text = AyudaProgramacion.MostrarFechaPantalla(item.Fecha);
                if (item.Sentido != 0)
                    ddTipoMovimiento.SelectedValue = item.Sentido.ToString();
                if (item.Producto.IdProducto > 0)
                    ddlProductos.Items.Add(new ListItem(item.Producto.Descripcion, item.Producto.IdProducto.ToString()));
            }
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                this.PersistirDatosGrilla();
                this.MiProduccion.ProduccionesDetalles.RemoveAt(indiceColeccion);
                this.MiProduccion.ProduccionesDetalles = AyudaProgramacion.AcomodarIndices<PrdProduccionesDetalles>(this.MiProduccion.ProduccionesDetalles);
                //this.MiProduccion.ProduccionesDetalles[indiceColeccion].Estado.IdEstado = (int)Estados.Baja;
                //this.MiProduccion.ProduccionesDetalles[indiceColeccion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiProduccion.ProduccionesDetalles[indiceColeccion], Gestion.Anular);
                AyudaProgramacion.CargarGrillaListas<PrdProduccionesDetalles>(this.MiProduccion.ProduccionesDetalles, true, this.gvDatos, true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CalcularPrecioScript", "CalcularValorizacion();", true);
                //this.upDetalleItems.Update();
            }
        }
        #endregion
    }
}