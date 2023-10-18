using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;
using System.Globalization;
using Compras;
using Generales.FachadaNegocio;
using Evol.Controls;
using System.Data;
using Afiliados.Entidades;
using Afiliados;
using OfficeOpenXml;
using System.IO;
using Generales.Entidades;
using Comunes.LogicaNegocio;
using System.Xml;

namespace IU.Modulos.Compras.Controles
{
    public partial class ListasPreciosDatos : ControlesSeguros
    {

        private CMPListasPrecios MiPrecio
        {
            get { return (CMPListasPrecios)Session[this.MiSessionPagina + "ListasPreciosDatosMiPrecio"]; }
            set { Session[this.MiSessionPagina + "ListasPreciosDatosMiPrecio"] = value; }
        }

        //private List<CMPListasPreciosDetalles> MiPrecioDetalle
        //{
        //    get { return (List<CMPListasPreciosDetalles>)Session[this.MiSessionPagina + "ListasPreciosDatosMiPrecioDetalle"]; }
        //    set { Session[this.MiSessionPagina + "ListasPreciosDatosMiPrecioDetalle"] = value; }
        //}

        private List<TGEEstados> MisEstados
        {
            get { return (List<TGEEstados>)Session[this.MiSessionPagina + "ListasPreciosDatosMisEstados"]; }
            set { Session[this.MiSessionPagina + "ListasPreciosDatosMisEstados"] = value; }
        }

        //private List<TGEMonedasCotizaciones> MiMoneda
        //{
        //    get { return (List<TGEMonedasCotizaciones>)Session[this.MiSessionPagina + "ListasPreciosDatosMiMoneda"]; }
        //    set { Session[this.MiSessionPagina + "ListasPreciosDatosMiMoneda"] = value; }
        //}

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "ListasPreciosDatosMiMonedas"]; }
            set { Session[this.MiSessionPagina + "ListasPreciosDatosMiMonedas"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "ListasPreciosModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "ListasPreciosModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        public delegate void ModificarDatosAceptarEventHandler(object sender, CMPListasPrecios e);
        public event ModificarDatosAceptarEventHandler ModificarDatosAceptar;

        public delegate void ModificarDatosCancelarEventHandler();
        public event ModificarDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new IU.Modulos.CuentasPagar.Controles.BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            this.ctrBuscarListaPrecio.ListasPreciosBuscarSeleccionar += new IU.Modulos.Compras.Controles.ListasPreciosBuscarPopUp.ListasPreciosBuscarEventHandler(ctrBuscarListaPrecio_SeleccionarLista);
            //this.ctrImportarArchivo.ImportarArchivoDatosAceptar += new Comunes.ImportarArchivo.ImportarArchivoAceptarEventHandler(ctrImportarArchivo_ImportarArchivoDatosAceptar);
            this.ctrBuscarClientePopUp.AfiliadosBuscarSeleccionar += new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                if (this.MiPrecio == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcionFiltro, this.btnFiltrar);
            }
            else
            {
                //if ((this.MiPrecioDetalle == null ? 0 : this.MiPrecioDetalle.Count) == 0)
                    this.PersistirLista();
                //else
                //    this.PersistirListaFiltro();
            }
        }

        public void IniciarControl(CMPListasPrecios pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiPrecio = pParametro;
            this.MisEstados = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosListasPrecios));
            this.MisMonedas = TGEGeneralesF.MonedasObtenerLista();
            //this.MisCotizaciones = TGEGeneralesF.MonedasCotizacionesObtenerLista();

            this.chkFiliales.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.chkFiliales.DataValueField = "IdFilial";
            this.chkFiliales.DataTextField = "Filial";
            this.chkFiliales.DataBind();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtDescripcion.Enabled = true;
                    this.txtFinanciacion.Enabled = true;
                    this.txtMargenImporte.Enabled = true;
                    this.txtMargenPorcentual.Enabled = true;
                    this.txtInicioVigencia.Enabled = true;
                    this.txtFinVigencia.Enabled = true;
                    //this.cdFinVigencia.Enabled = true;
                    //this.cdInicioVigencia.Enabled = true;
                    this.btnAceptar.Visible = true;
                    this.btnAgregarItem.Visible = true;
                    this.btnImportarLista.Visible = true;
                    //this.btnImportarExcel.Visible = true;
                    this.btnFiltrar.Visible = true;
                    this.btnLimpiar.Visible = false;
                    this.pnlDatosDelSocio.Visible = true;
                    //
                    this.MiPrecio.DataTableListasPreciosDetalle = ComprasF.ListasPreciosDetallesSeleccionar(new CMPListasPrecios());
                    //
                    this.IniciarGrilla(1);
                    //this.tpImportarArchivo.Visible = true;
                    this.ddlListasPrecios.DataSource = ComprasF.ListasPreciosObtenerListaFiltro(new CMPListasPrecios());
                    this.ddlListasPrecios.DataValueField = "IdListaPrecio";
                    this.ddlListasPrecios.DataTextField = "Descripcion";
                    this.ddlListasPrecios.DataBind();
                    AyudaProgramacion.InsertarItemSeleccione(this.ddlListasPrecios, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    this.ddlListasPrecios.Enabled = true;
                    this.phSubirArchivo.Visible = true;
                    this.ctrCamposValores.IniciarControl(this.MiPrecio, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    //if (this.MiPrecio.UltimaAgregada)
                    //{
                    //    this.MiPrecio = ComprasF.ListasPreciosObtenerDatosCompletos(pParametro);
                    //    this.MiPrecio.UltimaAgregada = true;
                    //}
                    //else
                    this.MiPrecio = ComprasF.ListasPreciosObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiPrecio);
                    this.txtDescripcion.Enabled = true;
                    this.txtFinanciacion.Enabled = true;
                    this.txtMargenImporte.Enabled = true;
                    this.txtMargenPorcentual.Enabled = true;
                    this.txtInicioVigencia.Enabled = true;
                    this.txtFinVigencia.Enabled = true; //se puede modificar la fecha de fin
                    //this.cdFinVigencia.Enabled = true;
                    //this.cdInicioVigencia.Enabled = true;
                    this.btnFiltrar.Visible = true;
                    this.btnLimpiar.Visible = false;
                    this.btnAceptar.Visible = true;
                    this.btnAgregarItem.Visible = true;
                    this.pnlDatosDelSocio.Visible = true;
                    //this.btnImportarExcel.Visible = true;
                    break;

                case Gestion.Anular:
                    this.btnAceptar.Visible = true;
                    this.MiPrecio = ComprasF.ListasPreciosObtenerDatosCompletos(pParametro);
                    this.btnFiltrar.Visible = false;
                    this.btnLimpiar.Visible = false;
                    this.txtDescripcionFiltro.Visible = false;
                    this.MapearObjetoAControles(this.MiPrecio);
                    break;
                case Gestion.Consultar:
                    this.MiPrecio = ComprasF.ListasPreciosObtenerDatosCompletos(pParametro);
                    this.btnFiltrar.Visible = true;
                    this.btnLimpiar.Visible = false;
                    this.txtDescripcionFiltro.Visible = true;
                    this.MapearObjetoAControles(this.MiPrecio);
                    break;
                default:
                    break;
            }
        }

        #region Productos CTRL
        void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPProductos e)
        {
            //if (this.MiPrecio.ListaPrecioDetalle.Exists(x => x.Producto.IdProducto == e.IdProducto))
            if (this.MiPrecio.DataTableListasPreciosDetalle.AsEnumerable().ToList().Exists(x => x.Field<int>("IdProducto") == e.IdProducto))
            {
                this.MostrarMensaje("ProductoYaIngresado",false);
            }
            else
            {
                DataRow dr = this.MiPrecio.DataTableListasPreciosDetalle.AsEnumerable().FirstOrDefault(x => x.Field<int>("IdListaPrecioDetalle") == this.MiIndiceDetalleModificar);
                //this.MiPrecio.ListaPrecioDetalle[this.MiIndiceDetalleModificar].Producto = e;
                dr["IdProducto"] = e.IdProducto;
                dr["Descripcion"] = e.Descripcion;
                this.MostrarGrillaDetalles();
            }
        }
        protected void txtCodigoProducto_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
            int IndiceColeccion = row.DataItemIndex;
            DataRowView dr = (DataRowView)row.DataItem;

            string contenido = ((TextBox)sender).Text;
            if (contenido == string.Empty)
                return;

            if(this.MiPrecio.DataTableListasPreciosDetalle.AsEnumerable().ToList().Exists(x => x.Field<int>("IdProducto") == Convert.ToInt32(contenido)))
            //if (this.MiPrecio.ListaPrecioDetalle.Exists(x => x.Producto.IdProducto == Convert.ToInt32(contenido) && x.IndiceColeccion != IndiceColeccion))
            {
                this.MostrarMensaje("ProductoYaIngresado", false);
            }
            else
            {
                dr["IdProducto"] = Convert.ToInt32(contenido);
                CMPProductos producto = new CMPProductos();
                producto.IdProducto = Convert.ToInt32(contenido);
                producto.Venta = true;
                producto = ComprasF.ProductosObtenerPorIdProducto(producto);
                dr["Producto"] = producto.Descripcion;
                //this.MiPrecio.ListaPrecioDetalle[IndiceColeccion].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiPrecio.ListaPrecioDetalle[IndiceColeccion].Producto);
                this.MostrarGrillaDetalles();
            }
        }

        #endregion

        #region DETALLES
        private void IniciarGrilla(int pCantidad)
        {
            DataRow workRow;
            int filas = MiPrecio.DataTableListasPreciosDetalle.Rows.Count;
            for (int i = 0; i < pCantidad; i++)
            {
                workRow = this.MiPrecio.DataTableListasPreciosDetalle.NewRow();
                //miraraca en el store!
                workRow["IdEstado"] = (int)Estados.Activo;
                workRow["Precio"] = 0;
                workRow["IdProducto"] = 0;
                workRow["MargenPorcentaje"] = 0;
                workRow["PrecioEditable"] = false;
                workRow["IdListaPrecioDetalle"] = (filas + i + 1) * -1;
                this.MiPrecio.DataTableListasPreciosDetalle.Rows.Add(workRow);
            }
            //CMPListasPreciosDetalles item;
            //for (int i = 0; i < pCantidad; i++)
            //{
            //    item = new CMPListasPreciosDetalles();
            //    this.MiPrecio.ListaPrecioDetalle.Add(item);
            //    item.IndiceColeccion = this.MiPrecio.ListaPrecioDetalle.IndexOf(item);
            //}
            ////AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle, false, gvItems, true);
            this.MostrarGrillaDetalles();
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "BuscarProducto"
                || e.CommandName == "Borrar"))
                return;
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);

                DataRow dr = this.MiPrecio.DataTableListasPreciosDetalle.AsEnumerable().FirstOrDefault(x => x.Field<int>("IdListaPrecioDetalle") == this.MiIndiceDetalleModificar);
                this.MiPrecio.DataTableListasPreciosDetalle.Rows.Remove(dr);
                this.MiPrecio.DataTableListasPreciosDetalle.AcceptChanges();
                MostrarGrillaDetalles();
                
                //this.MiPrecio.ListaPrecioDetalle.RemoveAt(this.MiIndiceDetalleModificar);
                //this.MiPrecio.ListaPrecioDetalle = AyudaProgramacion.AcomodarIndices<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle);
                //AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle, false, this.gvItems, true);
            }
            if (e.CommandName == "BuscarProducto")
            {
                CMPProductos producto;
                List<CMPProductos> productos = new List<CMPProductos>();
                foreach (DataRow row in MiPrecio.DataTableListasPreciosDetalle.Rows)
                {
                    producto = new CMPProductos();
                    if (!string.IsNullOrEmpty(row["IdProducto"].ToString()) && row["IdProducto"].ToString() != "0")
                    {
                        producto.IdProducto = Convert.ToInt32(row["IdProducto"]);
                        productos.Add(producto);
                    }
                }
                
                //MiPrecio.DataTableListasPreciosDetalle.AsEnumerable().ToList();//
                this.ctrBuscarProductoPopUp.IniciarControl(EnumTiposProductos.Ventas, new CMPProductos(), productos);
            }
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {              
                CheckBox btnCheckAll = (CheckBox)e.Row.FindControl("checkAll");
                btnCheckAll.Enabled = false;
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        btnCheckAll.Enabled = true;
                        break;
                    case Gestion.Modificar:
                        btnCheckAll.Enabled = true;
                        break;
                    case Gestion.Autorizar:
                        break;
                    case Gestion.Anular:
                    case Gestion.Consultar:
                        break;
                    default:
                        break;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //CMPListasPreciosDetalles item = (CMPListasPreciosDetalles)e.Row.DataItem;
                //SKP.ASP.Controls.NumericTextBox codigo = (SKP.ASP.Controls.NumericTextBox)e.Row.FindControl("txtCodigoProducto");
                TextBox codigo = (TextBox)e.Row.FindControl("txtCodigoProducto");
                //TextBox producto = (TextBox)e.Row.FindControl("txtProducto");
                CurrencyTextBox precio = (CurrencyTextBox)e.Row.FindControl("txtPrecio");
                CurrencyTextBox margenPorcentaje = (CurrencyTextBox)e.Row.FindControl("txtMargenPorcentaje");
                ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                DropDownList ddlEstados = (DropDownList)e.Row.FindControl("ddlEstadosDetalles");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                CheckBox btnCheck = (CheckBox)e.Row.FindControl("chkIncluir");
                DropDownList ddlMoneda = (DropDownList)e.Row.FindControl("ddlMoneda");

                ddlMoneda.DataSource = this.MisMonedas;
                ddlMoneda.DataValueField = "IdMoneda";
                ddlMoneda.DataTextField = "Descripcion";
                ddlMoneda.DataBind();
                ddlMoneda.Enabled = true;
                ddlMoneda.SelectedValue = dr["IdMoneda"].ToString();

                ddlEstados.DataSource = this.MisEstados;
                ddlEstados.DataValueField = "IdEstado";
                ddlEstados.DataTextField = "Descripcion";
                ddlEstados.DataBind();
                ddlEstados.Enabled = true;
                ddlEstados.SelectedValue = dr["IdEstado"].ToString();


                btnCheck.Enabled = false;
                btnEliminar.Visible = false;
                //btnBuscarProducto.Visible = false;
                ddlEstados.Enabled = false;
                ddlMoneda.Enabled = false;
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        btnBuscarProducto.Visible = true;
                        codigo.Enabled = true;
                        precio.Enabled = true;
                        margenPorcentaje.Enabled = true;
                        btnCheck.Enabled = true;
                        ddlMoneda.Enabled = true;
                        ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                        btnEliminar.Visible = true;
                        //if (this.MiPrecio.ImportarLista == true)
                        //{
                        //    ddlEstados.Enabled = true;
                        //    ddlEstados.SelectedValue = item.Estado.IdEstado.ToString();
                        //    btnEliminar.Visible = false;
                        //    //ddlMoneda.Enabled = false;
                        //    //ddlEstados.SelectedValue = item.Moneda.IdMoneda.ToString();
                        //}
                        //else
                        //{
                        //    ddlEstados.SelectedValue = ((int)EstadosListasPrecios.Activo).ToString();
                        //    ddlEstados.Enabled = false;
                        //    btnEliminar.Visible = true;
                        //ddlMoneda.Enabled = false;
                        //ddlMoneda.SelectedValue = item.Moneda.IdMoneda.ToString();
                        //}
                        //ddlMoneda.SelectedValue = item.Moneda.Descripcion.ToString();
                        //ddlMoneda.SelectedValue = item.Moneda.IdMoneda.ToString();
                        break;
                    case Gestion.Modificar:
                        //precio.Attributes.Add("onchange", "CalcularItem();");
                        //if (item.EstadoColeccion == EstadoColecciones.Agregado)
                        if (dr.Row.RowState == DataRowState.Added)
                        {
                            codigo.Enabled = true;
                            precio.Enabled = true;
                            ddlMoneda.Enabled = true;
                            ddlEstados.Enabled = true;
                            margenPorcentaje.Enabled = true;
                            btnBuscarProducto.Visible = true;
                            btnCheck.Enabled = true;
                            btnEliminar.Visible = true;
                            ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                        }
                        else
                        {
                            precio.Enabled = true;
                            margenPorcentaje.Enabled = true;
                            codigo.Enabled = false;
                            btnBuscarProducto.Visible = false;
                            btnCheck.Enabled = true;
                            ddlEstados.Enabled = true;
                            //ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                        }

                        //if (item.IdListaPrecioDetalle == 0)
                        //    btnEliminar.Visible = true;
                        //btnCheck.Enabled = true;                        
                        //ddlEstados.Enabled = true;
                        //if (item.Estado.IdEstado != (int)EstadosTodos.Todos)
                        //    ddlEstados.SelectedValue = item.Estado.IdEstado.ToString();
                        //else
                        //    ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                        break;
                    case Gestion.Autorizar:
                       break;
                    case Gestion.Anular:
                    case Gestion.Consultar:
                        //precio.Enabled = false;
                        //codigo.Enabled = false;
                        //margenPorcentaje.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            IniciarGrilla(1);
            //this.MapearParametrosPrecio();
            //CMPListasPreciosDetalles item;
            //item = new CMPListasPreciosDetalles();
            //item.EstadoColeccion = EstadoColecciones.Agregado;
            //this.MiPrecio.ListaPrecioDetalle.Add(item);
            //item.IndiceColeccion = this.MiPrecio.ListaPrecioDetalle.IndexOf(item);
            ////AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle, false, this.gvItems, true);
            ////this.chkNuevos.Checked = true;
            this.MostrarGrillaDetalles();
        }

        private void MapearParametrosPrecio()
        {
            this.MiPrecio.FinanciacionPorcentaje = this.txtFinanciacion.Decimal;
            this.MiPrecio.MargenPorcentaje = this.txtMargenPorcentual.Decimal;
            this.MiPrecio.MargenImporte = this.txtMargenImporte.Decimal;
        }

        private void PersistirLista()
        {
            bool modificaItem;
            DataRow dr;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    modificaItem = false;
                    string codigo = ((TextBox)fila.FindControl("txtCodigoProducto")).Text; //== string.Empty ? "0" : ((TextBox)fila.FindControl("txtCodigoProducto")).Text;
                    //string descripcion = ((TextBox)fila.FindControl("txtProducto")).Text;
                    decimal precio = ((TextBox)fila.FindControl("txtPrecio")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtPrecio")).Text, NumberStyles.Currency);
                    decimal margenPorcentaje = ((CurrencyTextBox)fila.FindControl("txtMargenPorcentaje")).Decimal;
                    DropDownList ddlEstados = ((DropDownList)fila.FindControl("ddlEstadosDetalles"));
                    CheckBox check = ((CheckBox)fila.FindControl("chkIncluir"));
                    DropDownList ddlMoneda = (DropDownList)fila.FindControl("ddlMoneda");
                    int id = Convert.ToInt32(this.gvItems.DataKeys[fila.RowIndex]["IdListaPrecioDetalle"].ToString());
                    dr = MiPrecio.DataTableListasPreciosDetalle.AsEnumerable().FirstOrDefault(x => x.Field<int>("IdListaPrecioDetalle") == id);
                    //DESCOMENTAR la linea de abajo cuando se habilite la parte de CUOTAS ¬
                    //this.MiPrecio.ListaPrecioDetalle[fila.DataItemIndex].ListaPrecio = this.MiPrecio;

                    if (dr["IdProducto"] != System.DBNull.Value && (int)dr["IdProducto"] != Convert.ToInt32(codigo))
                    {
                        modificaItem = true;
                        dr["IdProducto"] = Convert.ToInt32(codigo);
                    }
                    //if (dr["Descripcion"] != System.DBNull.Value && (string)dr["Descripcion"] != string.Empty)
                    //{
                    //    dr["Descripcion"] = descripcion;
                    //}
                    if (dr["Precio"] != System.DBNull.Value && (decimal)dr["Precio"] != precio)
                    {
                        dr["Precio"] = precio;
                        modificaItem = true;
                    }
                    if (dr["IdEstado"] != System.DBNull.Value && (int)dr["IdEstado"] != Convert.ToInt32(ddlEstados.SelectedValue))
                    {
                        modificaItem = true;
                        dr["IdEstado"] = Convert.ToInt32(ddlEstados.SelectedValue);
                        //dr.RowState¿?
                    }
                    if ((dr["IdMoneda"] == System.DBNull.Value &&  !string.IsNullOrWhiteSpace(ddlMoneda.SelectedValue)) || (dr["IdMoneda"] != System.DBNull.Value && (int)dr["IdMoneda"] != Convert.ToInt32(ddlMoneda.SelectedValue)))
                    {
                        //modificaItem = true;
                        dr["IdMoneda"] = Convert.ToInt32(ddlMoneda.SelectedValue);
                        //dr["IdMoneda"] = Convert.ToString(ddlMoneda.SelectedValue);
                    }
                    if (dr["MargenPorcentaje"] != System.DBNull.Value && (decimal)dr["MargenPorcentaje"] != margenPorcentaje)
                    {
                        dr["MargenPorcentaje"] = margenPorcentaje;
                        modificaItem = true;
                    }
                    if (dr["PrecioEditable"] != System.DBNull.Value && (bool)dr["PrecioEditable"] != check.Checked)
                    {
                        modificaItem = true;
                        dr["PrecioEditable"] = check.Checked;
                    }
                }
            }
            //AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle, false, this.gvItems, true);
        }

        protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CMPListasPreciosDetalles parametros = this.BusquedaParametrosObtenerValor<CMPListasPreciosDetalles>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CMPListasPreciosDetalles>(parametros);

            this.gvItems.PageIndex = e.NewPageIndex;
            MostrarGrillaDetalles();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            this.MostrarGrillaDetalles();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = ComprasF.ListasPreciosObtenerPlantilla(this.MiPrecio);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ExportData exportar = new ExportData();
            exportar.ExportExcel(this.Page, ds, true, "PlantillaListaDePrecios", "PlantillaListaDePrecios");
        }

        private void MostrarGrillaDetalles()
        {
            //List<CMPListasPreciosDetalles> listaFiltro =
            //    this.MiPrecio.ListaPrecioDetalle.Where(x => ((this.txtDescripcionFiltro.Text.Trim().Length == 0 || x.Producto.Descripcion.ToLower().Contains(this.txtDescripcionFiltro.Text.Trim().ToLower()))
            //                                                        && !this.chkNuevos.Checked)
            //                                            || (this.chkNuevos.Checked && x.EstadoColeccion == EstadoColecciones.Agregado)).ToList();
            //AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(listaFiltro, false, this.gvItems, true);
            DataView MisDvPreciosDetalles = MiPrecio.DataTableListasPreciosDetalle.DefaultView;
            if (txtDescripcionFiltro.Text.Trim().Length > 0)
                MisDvPreciosDetalles.RowFilter = string.Format("Descripcion LIKE '%{0}%'", txtDescripcionFiltro.Text.Trim());
            //DataTable dt = MisDvPreciosDetalles.ToTable().Copy();
            gvItems.DataSource = MisDvPreciosDetalles;
            gvItems.DataBind();

            this.items.Update();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtDescripcionFiltro.Text = string.Empty;
            //this.MapearFiltroAGrilla();
            this.btnLimpiar.Visible = false;
            this.MostrarGrillaDetalles();
            this.UpdatePanel1.Update();
        }

        //private void MapearFiltroAGrilla()
        //{
        //    foreach (CMPListasPreciosDetalles detalle in this.MiPrecioDetalle)
        //    {
        //        int indice = this.MiPrecio.ListaPrecioDetalle.FindIndex(x => x.Producto.Descripcion == detalle.Producto.Descripcion);
        //        AyudaProgramacion.MatchObjectProperties(detalle, this.MiPrecio.ListaPrecioDetalle[indice]);
        //        AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle, false, gvItems, true);
        //    }
        //    this.MiPrecioDetalle.Clear();
        //}
        //private void PersistirListaFiltro()
        //{
        //    foreach (GridViewRow fila in this.gvItems.Rows)
        //    {
        //        string codigo = ((TextBox)fila.FindControl("txtCodigoProducto")).Text;
        //        string descripcion = ((TextBox)fila.FindControl("txtProducto")).Text;
        //        decimal precio = ((TextBox)fila.FindControl("txtPrecio")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtPrecio")).Text, NumberStyles.Currency);
        //        DropDownList ddlEstados = ((DropDownList)fila.FindControl("ddlEstadosDetalles"));
        //        CheckBox check = ((CheckBox)fila.FindControl("chkIncluir"));
        //        //DESCOMENTAR la linea de abajo cuando se habilite la parte de CUOTAS ¬
        //        //this.MiPrecio.ListaPrecioDetalle[fila.DataItemIndex].ListaPrecio = this.MiPrecio;

        //        if (Convert.ToInt32(codigo) != 0)
        //        {
        //            this.MiPrecioDetalle[fila.DataItemIndex].Producto.IdProducto = Convert.ToInt32(codigo);
        //        }
        //        if (descripcion != string.Empty)
        //        {
        //            this.MiPrecioDetalle[fila.DataItemIndex].Producto.Descripcion = descripcion;
        //        }
        //        if (precio != 0)
        //        {
        //            this.MiPrecioDetalle[fila.DataItemIndex].Precio = precio;
        //        }
        //        if (ddlEstados.SelectedValue != string.Empty)
        //        {
        //            this.MiPrecioDetalle[fila.DataItemIndex].Estado.IdEstado = Convert.ToInt32(ddlEstados.SelectedValue);
        //        }

        //        this.MiPrecioDetalle[fila.DataItemIndex].PrecioEditable = check.Checked;

        //    }
        //    //AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle, false, this.gvItems, true);
        //}
        #endregion 

        private void MapearObjetoAControles(CMPListasPrecios pParametro)
        {
            this.txtMargenPorcentual.Text = pParametro.MargenPorcentaje.ToString("N4");
            this.txtMargenImporte.Text = pParametro.MargenImporte.ToString("C4");
            this.txtFinanciacion.Text = pParametro.FinanciacionPorcentaje.ToString("N4");
            this.txtInicioVigencia.Text = pParametro.FechaInicioVigencia.ToShortDateString();
            this.txtFinVigencia.Text = pParametro.FechaFinVigencia.ToShortDateString(); 
            this.txtDescripcion.Text = pParametro.Descripcion;
            this.MostrarGrillaDetalles();
            AyudaProgramacion.CargarGrillaListas(pParametro.Afiliados, false, this.gvDatosAfiliados, true);

            MostrarGrillaDetalles();
            this.ctrAuditoria.IniciarControl(pParametro);

            List<CMPListasPrecios> lista = new List<CMPListasPrecios>();
            lista.Add(pParametro);
            this.ddlListasPrecios.DataSource = lista;
            this.ddlListasPrecios.DataValueField = "IdListaPrecio";
            this.ddlListasPrecios.DataTextField = "Descripcion";
            this.ddlListasPrecios.DataBind();

            ListItem item;
            foreach (TGEFiliales fp in pParametro.Filiales)
            {
                item = this.chkFiliales.Items.FindByValue(fp.IdFilial.ToString());
                if (item != null)
                    item.Selected = true;
            }
            //gvItems.DataSource = pParametro.DataTableListasPreciosDetalle;
            //gvItems.DataBind();
            //this.ctrAuditoria.IniciarControl(pParametro);
            this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);
        }

        private void MapearControlesAObjeto(CMPListasPrecios pLista)
        {
            pLista.MargenPorcentaje = this.txtMargenPorcentual.Decimal;
            pLista.MargenImporte = this.txtMargenImporte.Decimal;
            pLista.FinanciacionPorcentaje = this.txtFinanciacion.Decimal;
            pLista.FechaInicioVigencia= Convert.ToDateTime(this.txtInicioVigencia.Text);
            pLista.FechaFinVigencia = Convert.ToDateTime(this.txtFinVigencia.Text);
            pLista.Descripcion = this.txtDescripcion.Text;

            TGEFiliales filial;
            foreach (ListItem lst in this.chkFiliales.Items)
            {
                filial = pLista.Filiales.Find(x => x.IdFilial == Convert.ToInt32(lst.Value));
                if (filial == null && lst.Selected)
                {
                    filial = new TGEFiliales();
                    filial.IdFilial = Convert.ToInt32(lst.Value);
                    filial.Filial = lst.Text;
                    pLista.Filiales.Add(filial);
                    filial.EstadoColeccion = EstadoColecciones.Agregado;
                    filial.IndiceColeccion = pLista.Filiales.IndexOf(filial);
                }
                else if (filial != null && !lst.Selected)
                    filial.EstadoColeccion = EstadoColecciones.Borrado;
            }
            pLista.Campos = this.ctrCamposValores.ObtenerLista();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar(null, this.MiPrecio);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiPrecio);
            ////MAPEO SI LA GRILLA ESTA FILTRADA
            //if ((this.MiPrecioDetalle == null ? 0 : this.MiPrecioDetalle.Count) != 0)
            //    this.MapearFiltroAGrilla();
            this.MiPrecio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.PersistirLista();
                    this.MiPrecio.Estado.IdEstado = (int)EstadosListasPrecios.Activo;
                    foreach (CMPListasPreciosDetalles item in this.MiPrecio.ListaPrecioDetalle)
                    
                    {
                        if (item.Producto.IdProducto != 0)
                        {
                            item.EstadoColeccion = EstadoColecciones.Agregado;
                            item.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                        }
                    }
                    //this.MiPrecio.ListaPrecioDetalle = this.MiPrecio.ListaPrecioDetalle.FindAll(x => x.Producto.IdProducto != 0);
                    this.MostrarGrillaDetalles();
                    guardo = ComprasF.ListasPreciosAgregar(this.MiPrecio);
                    break;
                case Gestion.Modificar:
                    //this.PersistirLista();
                    //this.MiPrecio.Estado.IdEstado = (int)EstadosListasPrecios.Activo;
                    //foreach (CMPListasPreciosDetalles item in this.MiPrecio.ListaPrecioDetalle)
                    //{
                    //    if (item.Producto.IdProducto != 0 && item.EstadoColeccion != EstadoColecciones.Agregado)
                    //    {
                    //        item.EstadoColeccion = EstadoColecciones.Modificado;
                            
                    //    }
                    //}
                    //this.MiPrecio.ListaPrecioDetalle = this.MiPrecio.ListaPrecioDetalle.FindAll(x => x.Producto.IdProducto != 0);
                    this.MostrarGrillaDetalles();
                    guardo = ComprasF.ListasPreciosModificar(this.MiPrecio);
                    break;
                case Gestion.Autorizar:
                   
                    break;

                case Gestion.Anular:
                    this.MiPrecio.Estado.IdEstado = (int)EstadosListasPrecios.Baja;
                    //foreach (CMPListasPreciosDetalles item in this.MiPrecio.ListaPrecioDetalle)
                    //{
                    //    if (item.Producto.IdProducto != 0)
                    //    {
                    //        item.EstadoColeccion = EstadoColecciones.Borrado;
                            
                    //    }
                    //}
                    guardo = ComprasF.ListasPreciosAnular(this.MiPrecio);
                    break;
                default:
                    break;
            }
            items.Update();
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiPrecio.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiPrecio.CodigoMensaje, true, this.MiPrecio.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar();
        }

        #region Importar Lista

        void ctrBuscarListaPrecio_SeleccionarLista(CMPListasPrecios e)
        {
            this.MiPrecio.ImportarLista = true;

            this.MiPrecio.DataTableListasPreciosDetalle = e.DataTableListasPreciosDetalle;

            this.btnLimpiar_Click(null, EventArgs.Empty);
            //AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle, false, gvItems, true);
            this.items.Update();
        }

        protected void btnImportarLista_Click(object sender, EventArgs e)
        {
            this.ctrBuscarListaPrecio.IniciarControl();
        }
        #endregion

        #region Importar Archivo

        protected void btnDescargarPlantilla_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlListasPrecios.SelectedValue))
            {
                CMPListasPrecios lista = new CMPListasPrecios();
                lista.IdListaPrecio = Convert.ToInt32(this.ddlListasPrecios.SelectedValue);
                DataTable dt = ComprasF.ListasPreciosObtenerPlantilla(lista);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ExportData exportar = new ExportData();
                exportar.ExportExcel(this.Page, ds, true, "PlantillaListaDePrecios", "PlantillaListaDePrecios");
            }
            else
            {
                this.MostrarMensaje("ValidarSeleccionarListaPrecio", true);
            }
        }

        //private DataTable Datos
        //{
        //    get { return (DataTable)Session[this.MiSessionPagina + "ListasPreciosDatosImportarArchivoDatos"]; }
        //    set { Session[this.MiSessionPagina + "ListasPreciosDatosImportarArchivoDatos"] = value; }
        //}

        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            var excel = new ExcelPackage(new MemoryStream(this.StreamToByteArray(this.afuArchivo.FileContent)));
            DataTable dt = ExcelPackageExtensions.ToDataTable(excel);
            dt.TableName = "ListasPreciosDetalles";

            this.MiPrecio.LoteListasPreciosDetalles = dt.ToXmlDocument();

            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();
        }

        protected void afuArchivo_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();
        }

        protected void button_Click(object sender, EventArgs e)
        {

            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();

            if (!ComprasF.ListasPreciosImportarFamiliasProductosValidaciones(this.MiPrecio))
            {
                this.MostrarMensaje(this.MiPrecio.CodigoMensaje, true, this.MiPrecio.CodigoMensajeArgs);
                return;
            }

            this.MiPrecio.DataTableListasPreciosDetalle = ComprasF.ListasPreciosImportarFamiliasProductos(this.MiPrecio);
            this.MiPrecio.ImportarLista = true;
            if (this.MiPrecio.DataTableListasPreciosDetalle.Rows.Count == 0)
            {
                this.MostrarMensaje("No se encontraron registros", true);
                return;
            }

            MostrarGrillaDetalles();

            this.tcDatos.ActiveTab = this.tpPrecioDetalles;
            this.items.Update();
            this.upTabControl.Update();

            
            //this.MiPrecio.ListaPrecioDetalle = new List<CMPListasPreciosDetalles>();
            //this.gvItems.DataSource = null;
            //this.gvItems.DataBind();
            //CMPListasPreciosDetalles detalle;
            ////bool precioEditable;
            //foreach (DataRow row in this.Datos.Rows)
            //{
            //    detalle = new CMPListasPreciosDetalles();
            //    detalle.Producto = new CMPProductos();
            //    //precioEditable = false;
            //    if (row["IdProducto"] == DBNull.Value || row["IdProducto"].ToString() == string.Empty)
            //        continue;
            //    detalle.Producto.IdProducto = Convert.ToInt32(row["IdProducto"]);
            //    detalle.Producto.Descripcion = (string)row["Descripcion"];
            //    detalle.Precio = row["Precio"].ToString() == string.Empty ? 0 : decimal.Parse(row["Precio"].ToString(), NumberStyles.Currency);
            //    detalle.MargenPorcentaje = row["MargenPorcentaje"].ToString() == string.Empty ? 0 : Convert.ToDecimal(row["Precio"]);
            //    if (row["PrecioEditable"].ToString() == "1" || row["PrecioEditable"].ToString().ToUpper() == "SI")
            //        detalle.PrecioEditable = true;
            //    else
            //        detalle.PrecioEditable = false;
            //    detalle.EstadoColeccion = EstadoColecciones.Agregado;
            //    detalle.Estado.IdEstado = (int)Estados.Activo;
            //    this.MiPrecio.ListaPrecioDetalle.Add(detalle);
            //    detalle.IndiceColeccion = this.MiPrecio.ListaPrecioDetalle.IndexOf(detalle);
            //}
            //this.MiPrecio.ListaPrecioDetalle.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.AgregadoPrevio);
            //this.MiPrecio.ListaPrecioDetalle.RemoveAll(x => x.Producto.IdProducto == 0);
            //this.MiPrecio.ListaPrecioDetalle = AyudaProgramacion.AcomodarIndices<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle);

            //this.MostrarGrillaDetalles();
            
            //this.afuArchivo.FailedValidation = false;
            //this.afuArchivo.ClearAllFilesFromPersistedStore();
            //this.tcDatos.ActiveTab = this.tpPrecioDetalles;
            //this.items.Update();
            //this.upTabControl.Update();
        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            // This is optional
            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }
        #endregion

        //#region Importar Archivo
        //protected void btnImportarExcel_Click(object sender, EventArgs e)
        //{
        //    string columnas = "CODIGO | ARTICULO | PRECIO";
        //    this.ctrImportarArchivo.IniciarControl(columnas);
        //}

        //void ctrImportarArchivo_ImportarArchivoDatosAceptar(object sender, System.Data.DataTable e)
        //{
        //    CMPListasPreciosDetalles item;
        //    List<CMPListasPreciosDetalles> lista = new List<CMPListasPreciosDetalles>();
        //    string sep=string.Empty;
        //    string codigosNoEncontrados = string.Empty;
        //    bool faltanCodigos = false;
        //    decimal precio;

        //    DataColumnCollection columns = e.Columns;
        //    string columnas = "CODIGO | ARTICULO | PRECIO";
        //    string[] cols = columnas.Split('|');

        //    foreach (string i in cols.ToList())
        //    {
        //        if (!columns.Contains(i.Trim()))
        //        {
        //            this.MostrarMensaje("ValidarImportarArchivoColumna", true, new List<string>() { i.Trim() });
        //        }
        //    }



        //    foreach (DataRow r in e.Rows)
        //    {
        //        precio = 0;
        //        item = new CMPListasPreciosDetalles();
        //        item.EstadoColeccion = EstadoColecciones.Agregado;
        //        item.Producto.ProveedorCodigoProducto = r["CODIGO"].ToString();
        //        item.Producto = ComprasF.ProductosObtenerPorCodigo(item.Producto);
        //        if (item.Producto.IdProducto == 0)
        //        {
        //            faltanCodigos = true;
        //            codigosNoEncontrados = string.Concat(sep, codigosNoEncontrados);
        //            sep = ", ";
        //            continue;
        //        }
        //        else
        //        {
        //            if (!decimal.TryParse(r["Precio"].ToString(), out precio))
        //            {
        //                this.MostrarMensaje("ValidarImportarArchivoPrecio", true, new List<string>() { item.Producto.ProveedorCodigoProducto });
        //                return;
        //            }
        //            item.Precio = Convert.ToDecimal(r["Precio"]);
        //            if (!lista.Exists(x => x.Producto.IdProducto == item.Producto.IdProducto))
        //                lista.Add(item);
        //        }
        //    }

        //    if (faltanCodigos)
        //    {
        //        this.MostrarMensaje("ValidarImportarArchivoCodigo", true, new List<string>() { codigosNoEncontrados });
        //    }
        //    else
        //    {
        //        this.MiPrecio.ListaPrecioDetalle.AddRange(lista);
        //        this.MiPrecio.ListaPrecioDetalle = AyudaProgramacion.AcomodarIndices<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle);
        //        this.btnLimpiar_Click(null, EventArgs.Empty);
        //    }
        //}
        //#endregion

        #region Control Filiales

        #endregion

        #region Control Clientes
        void ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(AfiAfiliados pAfiliado)
        {
            this.txtNumeroSocio.Text = string.Empty;
            //Si existia en la coleccion y los pusieron de baja lo activo de nuevo
            if (this.MiPrecio.Afiliados.Exists(x => x.IdAfiliado == pAfiliado.IdAfiliado))
                this.MiPrecio.Afiliados.Find(x => x.IdAfiliado == pAfiliado.IdAfiliado).EstadoColeccion = EstadoColecciones.SinCambio;
            else
                pAfiliado.EstadoColeccion = EstadoColecciones.Agregado;
            
            this.MiPrecio.Afiliados.Add(pAfiliado);
            AyudaProgramacion.CargarGrillaListas<AfiAfiliados>(this.MiPrecio.Afiliados, true, this.gvDatosAfiliados, true);
            this.upAfiliados.Update();
        }

        protected void txtNumeroSocio_TextChanged(object sender, EventArgs e)
        {
            string txtNumeroSocio = ((TextBox)sender).Text;
            AfiAfiliados parametro = new AfiAfiliados();
            parametro.IdAfiliado = Convert.ToInt32(txtNumeroSocio);
            parametro = AfiliadosF.AfiliadosObtenerDatosCompletos(parametro);
            if (parametro.IdAfiliado != 0)
            {
                if (!this.MiPrecio.Afiliados.Exists(x => x.IdAfiliado == parametro.IdAfiliado))
                    this.ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(parametro);
                else
                {
                    this.txtNumeroSocio.Text = string.Empty;
                    parametro.CodigoMensaje = "ValidarNumeroCliente";
                    this.upAfiliados.Update();
                    this.MostrarMensaje(parametro.CodigoMensaje, true);
                }
            }
            else
            {
                this.txtNumeroSocio.Text = string.Empty;
                parametro.CodigoMensaje = "NumeroSocioNoExiste";
                this.upAfiliados.Update();
                this.MostrarMensaje(parametro.CodigoMensaje, true);
            }
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            this.ctrBuscarClientePopUp.IniciarControl(new AfiAfiliados(), true, EnumAfiliadosTipos.Clientes, true, this.MiPrecio.Afiliados);
        }

        protected void gvDatosAfiliados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;
            int index = Convert.ToInt32(e.CommandArgument);
            int idAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AfiAfiliados afi = this.MiPrecio.Afiliados.Find(x => x.IdAfiliado == idAfiliado);
            if (e.CommandName == "Borrar")
            {
                if (afi.EstadoColeccion == EstadoColecciones.Agregado)
                    this.MiPrecio.Afiliados.Remove(afi);
                else
                    afi.EstadoColeccion = EstadoColecciones.Borrado;
                
                this.MiPrecio.Afiliados = AyudaProgramacion.AcomodarIndices<AfiAfiliados>(this.MiPrecio.Afiliados);
                AyudaProgramacion.CargarGrillaListas<AfiAfiliados>(this.MiPrecio.Afiliados, true, this.gvDatosAfiliados, true);

            }
        }

        protected void gvDatosAfiliados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiAfiliados item = (AfiAfiliados)e.Row.DataItem;
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                btnEliminar.Visible = false;
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                    case Gestion.Modificar:
                        btnEliminar.Visible = true;
                        string mensaje = this.ObtenerMensajeSistema("ConfirmarEliminarArgs");
                        mensaje = string.Format(mensaje, string.Concat(item.RazonSocial));
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                        btnEliminar.Attributes.Add("OnClick", funcion);
                        break;
                    default:
                        break;
                }
            }
        }

        protected void gvDatosAfiliados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDatosAfiliados.PageIndex = e.NewPageIndex;
            this.gvDatosAfiliados.DataSource = this.MiPrecio.Afiliados;
            this.gvDatosAfiliados.DataBind();
        }

        protected void gvDatosAfiliados_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiPrecio.Afiliados = this.OrdenarGrillaDatos<AfiAfiliados>(this.MiPrecio.Afiliados, e);
            this.gvDatosAfiliados.DataSource = this.MiPrecio.Afiliados;
            this.gvDatosAfiliados.DataBind();
        }

        #endregion
    }
}