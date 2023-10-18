using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;
using Compras;
using Evol.Controls;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Data;
using System.IO;
using System.Globalization;
using OfficeOpenXml;

namespace IU.Modulos.Compras.Controles
{
    public partial class StockMovimientosDatos : ControlesSeguros
    {
        private CmpStockMovimientos MiStockMovimiento
        {
            get { return (CmpStockMovimientos)Session[this.MiSessionPagina + "StockMovimientosDatosMiStockMovimiento"]; }
            set { Session[this.MiSessionPagina + "StockMovimientosDatosMiStockMovimiento"] = value; }
        }
        private bool noactualizarGrilla
        {
            get { return (bool)Session[this.MiSessionPagina + "noactualizarGrilla"]; }
            set { Session[this.MiSessionPagina + "noactualizarGrilla"] = value; }
        }

        private bool mostrarValidarArchivo
        {
            get { return (bool)Session[this.MiSessionPagina + "StockMovimientosDatosmostrarValidarArchivo"]; }
            set { Session[this.MiSessionPagina + "StockMovimientosDatosmostrarValidarArchivo"] = value; }
        }
        private string mensajeValidarArchivo
        {
            get { return (string)Session[this.MiSessionPagina + "StockMovimientosDatosmensajeValidarArchivo"]; }
            set { Session[this.MiSessionPagina + "StockMovimientosDatosmensajeValidarArchivo"] = value; }
        }
        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "StockMovimientosModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "StockMovimientosModificarDatosMiIndiceDetalleModificar"] = value; }
        }
        //private TGEFiliales MiFilial
        //{
        //    get { return (TGEFiliales)Session[this.MiSessionPagina + "StockMovimientosModificarDatosMiFilial"]; }
        //    set { Session[this.MiSessionPagina + "StockMovimientosModificarDatosMiFilial"] = value; }
        //}
        public delegate void StockMovimientosAceptarEventHandler(object sender, CmpStockMovimientos e);
        public event StockMovimientosAceptarEventHandler StockMovimientosModificarDatosAceptar;

        public delegate void StockMovimientosCancelarEventHandler();
        public event StockMovimientosCancelarEventHandler StockMovimientosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
                if (this.MiStockMovimiento == null && this.GestionControl != Gestion.Agregar)
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        public void IniciarControl(CmpStockMovimientos pStockMov, Gestion pGestion)
        {
            this.GestionControl = pGestion;

            if (this.GestionControl != Gestion.Agregar)
                pStockMov = ComprasF.StockMovimientosObtenerDatosCompletos(pStockMov);
            this.MiStockMovimiento = pStockMov;

            this.CargarCombos();

            this.MiIndiceDetalleModificar = 0;

            if (pStockMov.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia)
                this.MostrarFilialDestino();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiStockMovimiento.Filial = this.UsuarioActivo.FilialPredeterminada;
                    this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
                    this.IniciarGrilla(2);
                    this.ddlTipo.Enabled = true;
                    this.ddlFilial.Enabled = true;
                    this.txtDescripcion.Enabled = true;
                    this.txtFecha.Text = DateTime.Now.ToShortDateString();
                    this.txtFecha.Enabled = true;
                    //this.ceFecha.Enabled = true;           
                    this.ctrComentarios.IniciarControl(this.MiStockMovimiento, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiStockMovimiento, this.GestionControl);
                    break;
                case Gestion.Consultar:
                    this.MapearObjetoAControles(pStockMov);
                    this.btnAceptar.Visible = false;
                    this.btnAgregarItem.Visible = false;
                    break;

                case Gestion.Anular:
                    this.MapearObjetoAControles(pStockMov);
                    this.btnAgregarItem.Visible = false;
                    break;
                case Gestion.ConfirmarAgregar: //se usa para aceptar la transferencia
                    this.MapearObjetoAControles(pStockMov);
                    this.btnAgregarItem.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void MostrarFilialDestino() //CORREGIR CUANDO MOSTRAR
        {
            //int filialOrigen = this.MiFilial.IdFilial;
            List<TGEFiliales> filialesDestino = TGEGeneralesF.FilialesObenerLista();
            //filialesDestino.RemoveAt(filialesDestino.FindIndex(x => x.IdFilial == filialOrigen));
            //AyudaProgramacion.AcomodarIndices(filialesDestino);
            this.ddlFilialDestino.DataSource = filialesDestino;
            this.ddlFilialDestino.DataValueField = "IdFilial";
            this.ddlFilialDestino.DataTextField = "Filial";
            this.ddlFilialDestino.DataBind();
            if (this.ddlFilialDestino.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialDestino, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.lblFilialDestino.Visible = true;
                    this.ddlFilialDestino.Enabled = true;
                    this.ddlFilialDestino.Visible = true;
                    this.rfvFilialDestino.Enabled = true;
                    break;
                case Gestion.Consultar:
                    this.lblFilialDestino.Visible = true;
                    this.ddlFilialDestino.Enabled = false;
                    this.ddlFilialDestino.Visible = true;
                    this.rfvFilialDestino.Enabled = false;
                    break;
                case Gestion.Anular:
                    this.lblFilialDestino.Visible = true;
                    this.ddlFilialDestino.Enabled = false;
                    this.ddlFilialDestino.Visible = true;
                    this.rfvFilialDestino.Enabled = false;
                    break;
                case Gestion.ConfirmarAgregar:
                    this.lblFilialDestino.Visible = true;
                    this.ddlFilialDestino.Enabled = false;
                    this.ddlFilialDestino.Visible = true;
                    this.rfvFilialDestino.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();

            if (this.MiStockMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockAlta)
                this.ddlTipo.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TStockMovAlta);
            else if (this.MiStockMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockBaja)
                this.ddlTipo.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TStockMovBaja);
            else if (this.MiStockMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia)
                this.ddlTipo.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TStockMovTransferencia);

            this.ddlTipo.DataValueField = "IdListaValorDetalle";
            this.ddlTipo.DataTextField = "Descripcion";
            this.ddlTipo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipo, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFilial.SelectedValue))
                if (this.MiStockMovimiento.Filial.IdFilial != Convert.ToInt32(this.ddlFilial.SelectedValue))
                {
                    this.MiStockMovimiento.Filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
                    //Verificar el Stock Actual de los Productos Ingresados!
                    CMPStock stock;
                    foreach (CmpStockMovimientosDetalles detalle in this.MiStockMovimiento.StockMovimientosDetalles)
                    {
                        stock = new CMPStock();
                        stock.IdFilial = this.MiStockMovimiento.Filial.IdFilial;
                        stock.Producto.IdProducto = detalle.Producto.IdProducto;
                        if (stock.Producto.IdProducto > 0)
                            stock = ComprasF.StockObtenerPorProductoFilial(stock);
                        detalle.StockActual = stock.StockActual;
                    }
                    AyudaProgramacion.CargarGrillaListas<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles, false, this.gvItems, true);
                    this.upGrilla.Update();
                }
        }

        #region Productos
        private void MapearObjetoAGrilla()
        {
            if (this.MiStockMovimiento.StockMovimientosDetalles.Count == 0)
                return;
            CmpStockMovimientosDetalles det;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                det = this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex];
                HiddenField descripcion = ((HiddenField)fila.FindControl("hdfProductoDetalle"));
                HiddenField codigo = ((HiddenField)fila.FindControl("hdfIdProducto"));
                string cantidad = ((TextBox)fila.FindControl("txtCantidad")).Text;
                DropDownList producto = ((DropDownList)fila.FindControl("ddlProducto"));
                string stockActual = Convert.ToString(((Label)fila.FindControl("lblStockActual")).Text);
                string stockFinal = ((Label)fila.FindControl("lblStockFinal")).Text;
                //NumberStyles.Currency para sacar el signo $     

                if (codigo.Value != string.Empty && Convert.ToInt32(codigo.Value) > 0)
                    this.MiStockMovimiento.StockMovimientosDetalles[fila.RowIndex].Producto.IdProducto = Convert.ToInt32(codigo.Value);
                if (cantidad != "0")
                {
                    this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].Cantidad = Convert.ToDecimal(cantidad);
                    if (this.MiStockMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockAlta)
                        this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].StockFinal = Convert.ToDecimal(stockActual) + Convert.ToDecimal(cantidad);
                    else
                        this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].StockFinal = Convert.ToDecimal(stockActual) - Convert.ToDecimal(cantidad);
                }
                if (descripcion.Value != string.Empty)
                    this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].Producto.Descripcion = descripcion.Value;
                if (stockActual != "0")
                    this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].StockActual = Convert.ToDecimal(stockActual);
                if (this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].Producto.IdProducto > 0)
                {
                    Evol.Controls.CurrencyTextBox txtPrecioUnitario = (Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                    if (det.PrecioUnitario != txtPrecioUnitario.Decimal)
                        det.PrecioUnitario = txtPrecioUnitario.Decimal;
                        //this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].PrecioUnitario = Convert.ToDecimal(precioUnitario);
                }
            }
        }
        private void PersistirDatosGrilla()
        {
            CmpStockMovimientosDetalles det;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                det = this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex];
                HiddenField descripcion = ((HiddenField)fila.FindControl("hdfProductoDetalle"));
                HiddenField codigo = ((HiddenField)fila.FindControl("hdfIdProducto"));
                string cantidad = ((TextBox)fila.FindControl("txtCantidad")).Text;
                DropDownList producto = ((DropDownList)fila.FindControl("ddlProducto"));
                HiddenField stockActual = ((HiddenField)fila.FindControl("hdfStockActual"));
                //NumberStyles.Currency para sacar el signo $     

                if (codigo.Value != string.Empty && Convert.ToInt32(codigo.Value) > 0)
                    this.MiStockMovimiento.StockMovimientosDetalles[fila.RowIndex].Producto.IdProducto = Convert.ToInt32(codigo.Value);

                if (cantidad != "0")
                {
                    if (string.IsNullOrEmpty(stockActual.Value))
                        stockActual.Value = "0";
                    this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].Cantidad = Convert.ToDecimal(cantidad);
                    if (this.MiStockMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockAlta)
                        this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].StockFinal = Convert.ToDecimal(stockActual.Value) + Convert.ToDecimal(cantidad);
                    else
                        this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].StockFinal = Convert.ToDecimal(stockActual.Value) - Convert.ToDecimal(cantidad);
                }
                if (descripcion.Value != string.Empty)
                    this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].Producto.Descripcion = string.Concat(codigo.Value + " - " + descripcion.Value);
                if (!string.IsNullOrEmpty(stockActual.Value))
                    this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].StockActual = Convert.ToDecimal(stockActual.Value);
                if (this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].Producto.IdProducto > 0)
                {
                    Evol.Controls.CurrencyTextBox txtPrecioUnitario = (Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                    if (det.PrecioUnitario != txtPrecioUnitario.Decimal)
                        det.PrecioUnitario = txtPrecioUnitario.Decimal;
                        //this.MiStockMovimiento.StockMovimientosDetalles[fila.DataItemIndex].PrecioUnitario = Convert.ToDecimal(precioUnitario);
                }

            }
        }
        protected void btnDescargarPlantilla_Click(object sender, EventArgs e)
        {

            CmpStockMovimientos stock = new CmpStockMovimientos();
            //stock.IdStockMovimiento = Convert.ToInt32(MiStockMovimiento.IdStockMovimiento);
            stock.Filial.IdFilial = Convert.ToInt32(ddlFilial.SelectedValue);
            DataTable dt = ComprasF.cmpStockMovimientosObenterPlantilla(stock);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ExportData exportar = new ExportData();
            exportar.ExportExcel(this.Page, ds, true, "PlantillaStockMovimientos", "PlantillaStockMovimientos");
        }

        #region Importar Archivo

        private DataTable Datos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarArchivoDatos"]; }
            set { Session[this.MiSessionPagina + "ImportarArchivoDatos"] = value; }
        }

        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            mensajeValidarArchivo = string.Empty;
            mostrarValidarArchivo = false;
            noactualizarGrilla = true;
            var excel = new ExcelPackage(new MemoryStream(this.StreamToByteArray(this.afuArchivo.FileContent)));
            this.Datos = ExcelPackageExtensions.ToDataTable(excel);
            if (this.Datos.Rows.Count == 0)
            {
                this.MostrarMensaje("ValidarImportarArchivoColumna", true);
                return;
            }
            string mensaje = string.Empty;
            if (!this.Datos.Columns.Contains("IdProducto"))
            {
                mensaje = "No se encontró la columna IdProducto";
                mostrarValidarArchivo = true;
                //this.MostrarMensaje("No se encontraró la columna 'IdProducto'", true);
                //return;
            }
            if (!mostrarValidarArchivo && !this.Datos.Columns.Contains("Descripcion"))
            {
                mensaje = "No se encontró la columna Descripcion";
                mostrarValidarArchivo = true;
            }
            if (!mostrarValidarArchivo && !this.Datos.Columns.Contains("StockActual"))
            {
                mensaje = "No se encontró la columna StockActual";
                mostrarValidarArchivo = true;
            }
            if (!mostrarValidarArchivo && !this.Datos.Columns.Contains("Cantidad"))
            {
                mensaje = "No se encontró la columna Cantidad";
                mostrarValidarArchivo = true;
            }
            if (!mostrarValidarArchivo && !this.Datos.Columns.Contains("IdFilial"))
            {
                mensaje = "No se encontró la columna IdFilial";
                mostrarValidarArchivo = true;
            }
            if (!mostrarValidarArchivo && !this.Datos.Columns.Contains("Valorizacion"))
            {
                mensaje = "No se encontró la columna Valorizacion";
                mostrarValidarArchivo = true;
            }
            if (mostrarValidarArchivo)
            {
                mensajeValidarArchivo = mensaje;
                return;
            }
            //if (item.Value != this.ddlTipo.SelectedValue)
            //{
            //    this.ddlEjercicioContable.SelectedValue = item.Value;
            //    this.ddlEjercicioContable_SelectedIndexChanged(this.ddlEjercicioContable, EventArgs.Empty);
            //}
            DataTable DatosNuevos = ComprasF.cmpStockMovimientosActualizarStockAlImportarExcel(this.Datos);
            this.MiStockMovimiento.StockMovimientosDetalles = new List<CmpStockMovimientosDetalles>();

            CmpStockMovimientosDetalles detalle;
            foreach (DataRow row in DatosNuevos.Rows)
            {
                detalle = new CmpStockMovimientosDetalles();
                if (row["IdProducto"] == DBNull.Value || row["IdProducto"].ToString() == string.Empty)
                    continue;
                if (row["IdFilial"].ToString() != ddlFilial.SelectedValue)
                {
                    MiStockMovimiento.StockMovimientosDetalles = new List<CmpStockMovimientosDetalles>();
                    IniciarGrilla(2);
                    mensaje = "La Filial del Archivo no se corresponde con la Filial Seleccionada";
                    mensajeValidarArchivo = mensaje;
                    return;
                }
                detalle.Producto.IdProducto = Convert.ToInt32(row["IdProducto"]);
                detalle.StockActual = Convert.ToDecimal(row["StockActual"]);
                detalle.Cantidad = Convert.ToDecimal(row["Cantidad"]);
                detalle.PrecioUnitario = Convert.ToDecimal(row["Valorizacion"]);
                detalle.Producto.Descripcion = string.Concat(Convert.ToInt32(row["IdProducto"]) + " - " + (string)row["Descripcion"]);
                if (detalle.Cantidad > 0)
                {
                    detalle.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiStockMovimiento.StockMovimientosDetalles.Add(detalle);
                    detalle.IndiceColeccion = this.MiStockMovimiento.StockMovimientosDetalles.IndexOf(detalle);
                }
            }
            //this.MiStockMovimiento.StockMovimientosDetalles.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);
            this.MiStockMovimiento.StockMovimientosDetalles = AyudaProgramacion.AcomodarIndices<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles);
            //AyudaProgramacion.CargarGrillaListas(this.MiAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
            //upCuentasContables.Update();
            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();

        }

        protected void button_Click(object sender, EventArgs e)
        {
            if (mostrarValidarArchivo)
            {
                this.MostrarMensaje(mensajeValidarArchivo, true);
                mostrarValidarArchivo = false;
                return;
            }
            ddlFilial.Enabled = false;
            AyudaProgramacion.CargarGrillaListas(this.MiStockMovimiento.StockMovimientosDetalles, false, this.gvItems, true);
            ScriptManager.RegisterStartupScript(this.upGrilla, this.upGrilla.GetType(), "CalcularItemsAgregar", "CalcularItemAgregar(); CalcularValorizacion();", true);
            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();
            this.tcDatos.ActiveTab = this.tpGrilla;
            this.upGrilla.Update();
            this.upTabControl.Update();
            noactualizarGrilla = false;
        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
                throw new ArgumentException();
            // This is optional
            if (inputStream.CanSeek)
                inputStream.Seek(0, SeekOrigin.Begin);
            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }
        #endregion
        private void IniciarGrilla(int pCantidad)
        {
            CmpStockMovimientosDetalles item;
            for (int i = 0; i < pCantidad; i++)
            {
                item = new CmpStockMovimientosDetalles();
                this.MiStockMovimiento.StockMovimientosDetalles.Add(item);
                item.IndiceColeccion = this.MiStockMovimiento.StockMovimientosDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles, false, this.gvItems, true);
            this.upGrilla.Update();
        }
        protected void txtCodigoProducto_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
            int IndiceColeccion = row.RowIndex;
            string contenido = ((TextBox)sender).Text;
            if (contenido == string.Empty)
                return;
            //Cargo Stock
            CMPStock stock = new CMPStock();
            stock.Producto.IdProducto = Convert.ToInt32(contenido);
            stock.IdFilial = this.MiStockMovimiento.Filial.IdFilial;
            stock = ComprasF.StockObtenerPorProductoFilial(stock);
            //if (stock.StockActual > 0)
            //{
            this.MiStockMovimiento.StockMovimientosDetalles[IndiceColeccion].Producto.IdProducto = Convert.ToInt32(contenido);
            this.MiStockMovimiento.StockMovimientosDetalles[IndiceColeccion].Producto.Compra = true;
            this.MiStockMovimiento.StockMovimientosDetalles[IndiceColeccion].Producto.Venta = true;
            this.MiStockMovimiento.StockMovimientosDetalles[IndiceColeccion].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiStockMovimiento.StockMovimientosDetalles[IndiceColeccion].Producto);
            this.MiStockMovimiento.StockMovimientosDetalles[IndiceColeccion].StockActual = stock.StockActual;
            AyudaProgramacion.CargarGrillaListas<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles, false, this.gvItems, true);
            this.gvItems.Rows[IndiceColeccion].FindControl("txtCantidad").Focus();
            //}
            //else
            //{
            //    stock.CodigoMensaje = "ProductoSinStock";
            //    this.MostrarMensaje(stock.CodigoMensaje, true);
            //}
        }
        //void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPProductos e)
        //{
        //    CMPStock stock = new CMPStock();
        //    stock.Producto.IdProducto = e.IdProducto;
        //    stock.IdFilial = this.MiStockMovimiento.Filial.IdFilial;
        //    stock = ComprasF.StockObtenerPorProductoFilial(stock);
        //    //AyudaProgramacion.MatchObjectProperties(e, this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].Producto);
        //    this.MiStockMovimiento.StockMovimientosDetalles[this.MiIndiceDetalleModificar].Producto = e;
        //    this.MiStockMovimiento.StockMovimientosDetalles[this.MiIndiceDetalleModificar].StockActual = stock.StockActual;
        //    AyudaProgramacion.CargarGrillaListas<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles, false, this.gvItems, true);
        //    this.gvItems.Rows[this.MiIndiceDetalleModificar].FindControl("txtCantidad").Focus();
        //    this.upGrilla.Update();
        //}
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.PersistirDatosGrilla();
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            ///this.MiIndiceDetalleModificar = Convert.ToInt32(e.CommandArgument);
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
                this.MiStockMovimiento.StockMovimientosDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiStockMovimiento.StockMovimientosDetalles = AyudaProgramacion.AcomodarIndices<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles);
                AyudaProgramacion.CargarGrillaListas<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles, false, this.gvItems, true);

            }
            if (e.CommandName == "BuscarProducto")
            {
                if (string.IsNullOrEmpty(this.ddlFilial.SelectedValue))
                {
                    this.MostrarMensaje("ValidarSeleccionarFilial", true);
                    return;
                }
                CMPProductos filtro = new CMPProductos();
                filtro.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
            }
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CmpStockMovimientosDetalles item = (CmpStockMovimientosDetalles)e.Row.DataItem;
                CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
                DropDownList producto = (DropDownList)e.Row.FindControl("ddlProducto");
                Label StockActual = (Label)e.Row.FindControl("lblStockActual");
                Label lblStockFinal = (Label)e.Row.FindControl("lblStockFinal");
                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                CurrencyTextBox Valorizacion = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                Label lblproducto = (Label)e.Row.FindControl("lblProducto");
                if (item.Producto.IdProducto > 0)
                {
                    ddlProducto.Items.Add(new ListItem(item.Producto.Descripcion, item.Producto.IdProducto.ToString()));
                    StockActual.Text = item.StockActual.ToString("N2");
                    lblproducto.Text = string.Concat(item.Producto.IdProducto.ToString() + " - " + item.Producto.Descripcion);
                }
                //Seteo todo visible, solo anulo en consultar 

                producto.Enabled = true;
                cantidad.Enabled = true;

                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        if (this.MiStockMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockAlta)
                            cantidad.Attributes.Add("onchange", "CalcularItemAgregar(); CalcularValorizacion();");
                        else
                            cantidad.Attributes.Add("onchange", "CalcularItem();");
                        Evol.Controls.CurrencyTextBox txtPrecioUnitario = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                        txtPrecioUnitario.Attributes.Add("onchange", "CalcularValorizacion();");
                        break;
                    case Gestion.ConfirmarAgregar: //se usa para aceptar la transferencia
                        producto.Enabled = true;
                        cantidad.Enabled = false;
                        break;
                    case Gestion.Anular:
                        producto.Enabled = false;
                        cantidad.Enabled = false;
                        break;
                    case Gestion.Consultar:
                        Valorizacion.Enabled = false;
                        lblStockFinal.Enabled = false;
                        producto.Visible = false;
                        lblproducto.Visible = true;
                        lblproducto.Enabled = false;
                        producto.Enabled = false;
                        cantidad.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {

            PersistirDatosGrilla();
            CmpStockMovimientosDetalles item;
            item = new CmpStockMovimientosDetalles();
            this.MiStockMovimiento.StockMovimientosDetalles.Add(item);
            item.IndiceColeccion = this.MiStockMovimiento.StockMovimientosDetalles.IndexOf(item);
            AyudaProgramacion.CargarGrillaListas<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles, false, this.gvItems, true);
            this.gvItems.Rows[item.IndiceColeccion].FindControl("ddlProducto").Focus();
        }

        #endregion

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;

            this.Page.Validate("Aceptar");

            if (!this.Page.IsValid)
                return;
            this.PersistirDatosGrilla();
            this.MapearControlesAObjeto(this.MiStockMovimiento);
            this.MiStockMovimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    if (this.MiStockMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia)
                        this.MiStockMovimiento.Estado.IdEstado = (int)EstadosStock.PendienteConfirmacion;
                    else
                        this.MiStockMovimiento.Estado.IdEstado = (int)EstadosStock.Activo;

                    foreach (CmpStockMovimientosDetalles item in this.MiStockMovimiento.StockMovimientosDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                            item.EstadoColeccion = EstadoColecciones.Agregado;
                    }
                    guardo = ComprasF.StockMovimientosAgregar(this.MiStockMovimiento);
                    break;
                case Gestion.Modificar:

                    break;

                case Gestion.ConfirmarAgregar:
                    this.MiStockMovimiento.Estado.IdEstado = (int)EstadosStock.Confirmado;
                    foreach (CmpStockMovimientosDetalles item in this.MiStockMovimiento.StockMovimientosDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                            item.EstadoColeccion = EstadoColecciones.Modificado; //para confirmar
                    }
                    guardo = ComprasF.StockMovimientosConfirmar(this.MiStockMovimiento);
                    break;

                case Gestion.Anular:
                    this.MiStockMovimiento.Estado.IdEstado = (int)EstadosStock.Baja;
                    foreach (CmpStockMovimientosDetalles item in this.MiStockMovimiento.StockMovimientosDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                            item.EstadoColeccion = EstadoColecciones.Borrado;
                    }
                    guardo = ComprasF.StockMovimientosAnular(this.MiStockMovimiento);
                    break;
                default:
                    break;
            }
            if (guardo)
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiStockMovimiento.CodigoMensaje));
            else
                this.MostrarMensaje(this.MiStockMovimiento.CodigoMensaje, true, this.MiStockMovimiento.CodigoMensajeArgs);
        }

        private void MapearControlesAObjeto(CmpStockMovimientos pStockMov)
        {
            pStockMov.FechaAlta = Convert.ToDateTime(this.txtFecha.Text);
            pStockMov.Descripcion = this.txtDescripcion.Text;
            pStockMov.TipoStockMovimiento.IdTipoStockMovimiento = this.ddlTipo.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlTipo.SelectedValue);
            pStockMov.Filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
            //Filial Destino con If???
            pStockMov.IdFilialDestino = this.ddlFilialDestino.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlFilialDestino.SelectedValue);

            pStockMov.Comentarios = ctrComentarios.ObtenerLista();
            pStockMov.Archivos = ctrArchivos.ObtenerLista();

        }
        private void MapearObjetoAControles(CmpStockMovimientos pStockMov)
        {
            this.txtFecha.Text = pStockMov.FechaAlta.ToShortDateString();
            this.txtDescripcion.Text = pStockMov.Descripcion;
            ListItem item = this.ddlTipo.Items.FindByValue(pStockMov.TipoStockMovimiento.IdTipoStockMovimiento.ToString());
            if (item == null)
                this.ddlTipo.Items.Add(new ListItem(pStockMov.TipoStockMovimiento.Descripcion, pStockMov.TipoStockMovimiento.IdTipoStockMovimiento.ToString()));
            this.ddlTipo.SelectedValue = pStockMov.TipoStockMovimiento.IdTipoStockMovimiento.ToString();
            this.ddlFilial.SelectedValue = pStockMov.Filial.IdFilial.ToString();
            //vamos a ver filial destino
            this.ddlFilialDestino.SelectedValue = pStockMov.IdFilialDestino.ToString();

            this.MiStockMovimiento.StockMovimientosDetalles = pStockMov.StockMovimientosDetalles;
            AyudaProgramacion.CargarGrillaListas<CmpStockMovimientosDetalles>(this.MiStockMovimiento.StockMovimientosDetalles, false, this.gvItems, true);
            this.upGrilla.Update();

            this.ctrComentarios.IniciarControl(pStockMov, this.GestionControl);
            this.ctrArchivos.IniciarControl(pStockMov, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pStockMov);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.StockMovimientosModificarDatosCancelar != null)
                this.StockMovimientosModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.StockMovimientosModificarDatosAceptar != null)
                this.StockMovimientosModificarDatosAceptar(null, this.MiStockMovimiento);
        }
    }
}