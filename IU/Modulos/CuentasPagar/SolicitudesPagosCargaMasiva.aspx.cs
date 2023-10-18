using Comunes.Entidades;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using Evol.Controls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using ProcesosDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace IU.Modulos.CuentasPagar
{
    public partial class SolicitudesPagosCargaMasiva : PaginaSegura
    {
        protected SisProcesosProcesamiento MiProcesoProcesamiento
        {
            get { return (SisProcesosProcesamiento)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiProcesoProcesamientoCargaMasiva"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiProcesoProcesamientoCargaMasiva"] = value; }
        }
        private DataTable MisSolicitudesPagosCargaMasiva
        {
            get { return (DataTable)Session[this.MiSessionPagina + "SolicitudesPagosCargaMasivaMisSolicitudesPagosCargaMasiva"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosCargaMasivaMisSolicitudesPagosCargaMasiva"] = value; }
        }
        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "SolicitudesPagosCargaMasivaMisIvas"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosCargaMasivaMisIvas"] = value; }
        }
        private List<TGETiposFacturas> MisTiposComprobantes
        {
            get { return (List<TGETiposFacturas>)Session[this.MiSessionPagina + "SolicitudesPagosCargaMasivaMisTiposComprobantes"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosCargaMasivaMisTiposComprobantes"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                TGEIVA iva = new TGEIVA
                {
                    UsuarioLogueado = this.UsuarioActivo.UsuarioLogueado
                };
                this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(iva);
                this.MisTiposComprobantes = TGEGeneralesF.TiposOperacionesTiposFacturasObtenerLista();
                string funcion = string.Format("Validar(this,'{0}'); return false;", "Se daran de alta ### comprobantes");
                this.btnAceptar.Attributes.Add("OnClick", funcion);
                CapSolicitudPago pParametro = new CapSolicitudPago();
                this.MisSolicitudesPagosCargaMasiva = CuentasPagarF.SolicitudPagoCargaMasivaSeleccionarTabla(pParametro);
                this.IniciarGrilla(10);
                string parametro = "CapSolicitudPagoCargaMasiva";
                List<TGEEstados> estados = TGEGeneralesF.TGEEstadosObtenerLista(parametro);
                this.ddlEstados.DataSource = AyudaProgramacion.AcomodarIndices<TGEEstados>(estados);
                this.ddlEstados.DataValueField = "IdEstado";
                this.ddlEstados.DataTextField = "Descripcion";
                this.ddlEstados.DataBind();
                this.MiProcesoProcesamiento = new SisProcesosProcesamiento();
                string fun = string.Format("ValidarCargados(this,'{0}'); return false;", "Al importar archivo se perderan los datos agregados hasta el momento. ¿Desea Continuar?");
                this.btnCargarArchivo.Attributes.Add("OnClick", fun);
                //   this.btnCargarArchivo.Attributes.Remove("OnClick");
            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;

            CapSolicitudPago parametroCap = new CapSolicitudPago();
            this.PersistirDatosGrilla();
            this.MisSolicitudesPagosCargaMasiva = this.MisSolicitudesPagosCargaMasiva.AsEnumerable().Where(row => row.Field<int?>("ProveedorIdProveedor") > 0).CopyToDataTable();
            this.gvDatos.DataSource = this.MisSolicitudesPagosCargaMasiva;
            this.gvDatos.DataBind();
            parametroCap.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            parametroCap.IdFilial = parametroCap.UsuarioLogueado.IdFilialEvento;
            parametroCap.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            guardo = CuentasPagarF.SolicitudPagoCargaMasivaAgregar(this.MisSolicitudesPagosCargaMasiva, parametroCap);

            if (guardo)
            {
                this.MostrarMensaje(parametroCap.CodigoMensaje, false, parametroCap.CodigoMensajeArgs);
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(parametroCap.CodigoMensaje, true, parametroCap.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("InicioSistema.aspx"), true);
        }
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            if (this.txtCantidadAgregar.Text == string.Empty || this.txtCantidadAgregar.Text == "0")
            {
                this.txtCantidadAgregar.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregar.Text);
            this.IniciarGrilla(cantidad);
            this.txtCantidadAgregar.Text = string.Empty;
        }
        #region Grilla
        private void IniciarGrilla(int pCantidad)
        {
            DataRow workRow;
            int filas = this.MisSolicitudesPagosCargaMasiva.Rows.Count;
            for (int i = 0; i < pCantidad; i++)
            {
                //DateTime dt = DateTime.Now;
                //string date = dt.ToShortDateString();
                workRow = MisSolicitudesPagosCargaMasiva.NewRow();
                workRow["Proveedor"] = 0;
                workRow["FechaFactura"] = DateTime.Now;//.ToShortDateString();
                workRow["NumeroPuntoVenta"] = 0;
                workRow["NumeroFactura"] = 0;
                workRow["TipoComprobante"] = 0;
                workRow["FechaContable"] = DateTime.Now;//.ToShortDateString();
                workRow["Producto"] = 0;
                workRow["Cantidad"] = 0;
                workRow["Precio"] = 0;
                workRow["IVA"] = 0;
                workRow["ImporteTotal"] = 0;
                workRow["IdSolicitudPagoDetalle"] = (filas + i + 1) * -1;
                workRow["Importado"] = false;
                this.MisSolicitudesPagosCargaMasiva.Rows.Add(workRow);
            }
            this.gvDatos.DataSource = this.MisSolicitudesPagosCargaMasiva;
            this.gvDatos.DataBind();

            this.items.Update();
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //int MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                this.PersistirDatosGrilla();
                DataRow dr = MisSolicitudesPagosCargaMasiva.AsEnumerable().FirstOrDefault(x => x.Field<int>("IdSolicitudPagoDetalle") == MiIndiceDetalleModificar);
                this.MisSolicitudesPagosCargaMasiva.Rows.Remove(dr);
                this.MisSolicitudesPagosCargaMasiva.AcceptChanges();
                this.gvDatos.DataSource = this.MisSolicitudesPagosCargaMasiva;
                this.gvDatos.DataBind();
                this.items.Update();
                //MostrarGrillaDetalles();
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //CapSolicitudPagoDetalles item = (CapSolicitudPagoDetalles)e.Row.DataItem;
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //TGEIVA pParamaetro = new TGEIVA();
                
                TextBox txtNumeroFactura = ((TextBox)e.Row.FindControl("txtNumeroFactura"));
                if (!string.IsNullOrEmpty(txtNumeroFactura.Text) && txtNumeroFactura.Text.Contains("."))
                {
                    txtNumeroFactura.Text = txtNumeroFactura.Text.Replace(".", "");
                }

                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlIVA"));
                ddlAlicuotaIVA.DataSource = this.MisIvas;
                ddlAlicuotaIVA.DataValueField = "IdIVAAlicuota";
                ddlAlicuotaIVA.DataTextField = "Descripcion";
                ddlAlicuotaIVA.DataBind();
                //if (ddlAlicuotaIVA.Items.Count != 1)
                //    AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, ObtenerMensajeSistema("SeleccioneOpcion"));
                ddlAlicuotaIVA.SelectedValue = string.Concat(dr["IdIVA"].ToString(), "|", dr["IVADescripcion"].ToString());
                //ddlAlicuotaIVA.SelectedItem.Text = dr["IVADescripcion"].ToString();
                //if (dr["IdIVA"].ToString() != string.Empty)
                //    ddlAlicuotaIVA.Items.Add(new ListItem(dr["IVADescripcion"].ToString(), dr["IdIVA"].ToString()));

                DropDownList ddlTipoComprobante = ((DropDownList)e.Row.FindControl("ddlTipoComprobante"));
                ddlTipoComprobante.DataSource = this.MisTiposComprobantes;////TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposFacturas); ;
                //ddlTipoComprobante.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposFacturas); ;
                ddlTipoComprobante.DataValueField = "TipoFactura";
                ddlTipoComprobante.DataTextField = "Descripcion";
                ddlTipoComprobante.DataBind();
                if (ddlTipoComprobante.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(ddlTipoComprobante, ObtenerMensajeSistema("SeleccioneOpcion"));
                ddlTipoComprobante.SelectedValue = string.Concat(dr["FacturaIdTipoFactura"].ToString(), "|", dr["FacturaDescripcion"].ToString(), "|", dr["FacturaMostrarIVA"].ToString());
                //ddlTipoComprobante.SelectedValue = string.Concat(dr["FacturaIdTipoFactura"].ToString(), "|", dr["IVADescripcion"].ToString());

                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                if (dr["ProductoIdProducto"].ToString() != string.Empty && Convert.ToInt32(dr["ProductoIdProducto"].ToString()) > 0)
                    ddlProducto.Items.Add(new ListItem(dr["ProductoDescripcion"].ToString(), dr["ProductoIdProducto"].ToString()));

                DropDownList ddlProveedor = ((DropDownList)e.Row.FindControl("ddlProveedor"));
                if (dr["ProveedorIdProveedor"].ToString() != string.Empty)
                    ddlProveedor.Items.Add(new ListItem(dr["ProveedorDescripcion"].ToString(), dr["ProveedorIdProveedor"].ToString()));

                CurrencyTextBox txtPrecio = (CurrencyTextBox)e.Row.FindControl("txtPrecio");
                CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");

                txtPrecio.Attributes.Add("onchange", "CalcularItem();");
                string numberSymbol = txtPrecio.Prefix == string.Empty ? "N" : "C";
                decimal precio = Convert.ToDecimal(dr["Precio"]);
                txtPrecio.Text = precio.ToString(string.Concat(numberSymbol, "2"));
                ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                ddlTipoComprobante.Attributes.Add("onchange", "CalcularItem();");
                cantidad.Attributes.Add("onchange", "CalcularItem();");
            }
        }
        private void PersistirDatosGrilla()
        {
            //bool modificaItem;
            DataRow dr;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    //modificaItem = false;
                    DropDownList ddlProveedor = (DropDownList)fila.FindControl("ddlProveedor");
                    HiddenField hdfIdProveedor = (HiddenField)fila.FindControl("hdfIdProveedor");
                    HiddenField hdfProveedorDescripcion = (HiddenField)fila.FindControl("hdfProveedorDescripcion");
                    string fechaFactura = ((TextBox)fila.FindControl("txtFechaFactura")).Text;
                    string numeroPuntoVenta = ((TextBox)fila.FindControl("txtNumeroPuntoVenta")).Text;
                    string numeroFactura = ((TextBox)fila.FindControl("txtNumeroFactura")).Text;
                    DropDownList ddlTipoComprobante = (DropDownList)fila.FindControl("ddlTipoComprobante");
                    string fechaContable = ((TextBox)fila.FindControl("txtFechaContable")).Text;
                    DropDownList ddlProducto = (DropDownList)fila.FindControl("ddlProducto");
                    HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                    HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");
                    decimal cantidad = ((CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                    decimal precio = ((CurrencyTextBox)fila.FindControl("txtPrecio")).Decimal;
                    //decimal precio = ((TextBox)fila.FindControl("txtPrecio")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtPrecio")).Text, NumberStyles.Currency);

                    //decimal precio = ((CurrencyTextBox)fila.FindControl("txtPrecio")).Decimal;
                    DropDownList ddlIVA = (DropDownList)fila.FindControl("ddlIVA");
                    //HiddenField hdfPrecio = (HiddenField)fila.FindControl("hdfPrecio");
                    //string lblTotal = ((Label)fila.FindControl("lblTotal")).ToString();
                    string lblTotal = ((Label)fila.FindControl("lblTotal")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblTotal")).Text);
                    decimal hdfTotal = ((HiddenField)fila.FindControl("hdfTotal")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfTotal")).Value.ToString().Replace('.', ','));
                    decimal hdfImporteSinIVA = ((HiddenField)fila.FindControl("hdfImporteSinIVA")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfImporteSinIVA")).Value.ToString().Replace('.', ','));
                    decimal hdfIvaTotal = ((HiddenField)fila.FindControl("hdfIvaTotal")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfIvaTotal")).Value.ToString().Replace('.', ','));

                    int id = Convert.ToInt32(gvDatos.DataKeys[fila.RowIndex]["IdSolicitudPagoDetalle"].ToString());
                    dr = MisSolicitudesPagosCargaMasiva.AsEnumerable().FirstOrDefault(x => x.Field<int>("IdSolicitudPagoDetalle") == id);

                    if (hdfIdProveedor.Value != string.Empty)
                    {
                        if (dr["Proveedor"] != System.DBNull.Value && Convert.ToInt32(dr["Proveedor"].ToString()) != Convert.ToInt32(hdfIdProveedor.Value))
                        {
                            dr["ProveedorIdProveedor"] = Convert.ToInt32(hdfIdProveedor.Value);
                            dr["ProveedorDescripcion"] = hdfProveedorDescripcion.Value;
                        }
                    }
                    if (dr["FechaFactura"] != System.DBNull.Value && Convert.ToDateTime(dr["FechaFactura"].ToString()) != Convert.ToDateTime(fechaFactura))
                    {
                        dr["FechaFactura"] = Convert.ToDateTime(fechaFactura);
                    }
                    if (dr["NumeroPuntoVenta"] != System.DBNull.Value && dr["NumeroPuntoVenta"].ToString() != Convert.ToString(numeroPuntoVenta))
                    {
                        dr["NumeroPuntoVenta"] = Convert.ToString(numeroPuntoVenta);
                    }
                    if (dr["NumeroFactura"] != System.DBNull.Value && dr["NumeroFactura"].ToString() != Convert.ToString(numeroFactura))
                    {
                        dr["NumeroFactura"] = Convert.ToString(numeroFactura);
                    }
                    //if (dr["FacturaIdTipoFactura"] != System.DBNull.Value && (int)dr["FacturaIdTipoFactura"] != (ddlTipoComprobante.SelectedValue==string.Empty ? 0 : Convert.ToInt32(ddlTipoComprobante.SelectedValue)))
                    if (ddlTipoComprobante.SelectedValue == string.Empty && dr["FacturaIdTipoFactura"] != System.DBNull.Value && Convert.ToInt32(dr["FacturaIdTipoFactura"]) != (ddlTipoComprobante.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoComprobante.SelectedValue)))
                    {
                        dr["FacturaIdTipoFactura"] = (ddlTipoComprobante.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlTipoComprobante.SelectedValue));
                        dr["FacturaDescripcion"] = ddlTipoComprobante.SelectedValue;//ddlIVA.SelectedItem.Text;
                        dr["FacturaMostrarIVA"] = (ddlTipoComprobante.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlTipoComprobante.SelectedValue));

                        //dr["FacturaIdTipoFactura"] = (ddlTipoComprobante.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoComprobante.SelectedValue));
                    }
                    if (ddlTipoComprobante.SelectedValue != string.Empty)
                    {
                        string[] comprobante = ddlTipoComprobante.SelectedValue.Split('|');
                        int idTipoFactura = Convert.ToInt32(comprobante[0]);
                        string descripcionFactura = comprobante[1];
                        int mostrarIVAFactura = Convert.ToInt32(comprobante[2]);
                        if (dr["TipoComprobante"] != System.DBNull.Value && Convert.ToInt32(dr["TipoComprobante"]) != idTipoFactura)//Convert.ToDecimal(ddlIVA.SelectedIndex))
                        {
                            dr["FacturaIdTipoFactura"] = idTipoFactura; //Convert.ToDecimal(ddlIVA.SelectedIndex);
                            dr["FacturaDescripcion"] = descripcionFactura;
                            dr["FacturaMostrarIVA"] = mostrarIVAFactura;
                        }
                        //if (dr["TipoComprobante"] != System.DBNull.Value && (int)dr["TipoComprobante"] != Convert.ToInt32(ddlTipoComprobante.SelectedValue))
                        //{
                        //    dr["FacturaIdTipoFactura"] = Convert.ToInt32(ddlTipoComprobante.SelectedValue);
                        //}
                    }
                    if (dr["FechaContable"] != System.DBNull.Value && Convert.ToDateTime(dr["FechaContable"]) != Convert.ToDateTime(fechaContable))
                    {
                        dr["FechaContable"] = Convert.ToDateTime(fechaContable);
                    }
                    if (hdfIdProducto.Value != string.Empty)
                    {
                        if (dr["Producto"] != System.DBNull.Value && Convert.ToInt32(dr["Producto"]) != Convert.ToInt32(hdfIdProducto.Value))
                        {
                            dr["ProductoIdProducto"] = Convert.ToInt32(hdfIdProducto.Value);
                            dr["ProductoDescripcion"] = hdfProductoDetalle.Value;
                        }
                    }
                    if (dr["Cantidad"] != System.DBNull.Value && Convert.ToDecimal(dr["Cantidad"]) != cantidad)
                    {
                        dr["Cantidad"] = cantidad;
                    }
                    if (dr["Precio"] != System.DBNull.Value && Convert.ToDecimal(dr["Precio"]) != precio)
                    {
                        dr["Precio"] = precio;
                    }
                    if (dr["ImporteTotal"] != System.DBNull.Value && Convert.ToDecimal(dr["ImporteTotal"]) != hdfTotal) //Convert.ToDecimal(hdfTotal.Value))
                    {
                        dr["ImporteTotal"] = hdfTotal;
                        dr["ImporteSinIVA"] = hdfImporteSinIVA;
                        dr["IvaTotal"] = hdfIvaTotal;
                    }
                    if (ddlIVA.SelectedValue != string.Empty)
                    {
                        string[] ar = ddlIVA.SelectedValue.Split('|');
                        int idIva = Convert.ToInt32(ar[0]);
                        string descripcionIva = ar[1].ToString();
                        if (dr["IVA"] != System.DBNull.Value && Convert.ToInt32(dr["IVA"]) != idIva)//Convert.ToDecimal(ddlIVA.SelectedIndex))
                        {
                            dr["IdIVA"] = idIva; //Convert.ToDecimal(ddlIVA.SelectedIndex);
                            dr["IVADescripcion"] = descripcionIva;//ddlIVA.SelectedItem.Text;
                            dr["AlicuotaIVA"] = decimal.Parse(descripcionIva.Replace(".", ","), NumberStyles.AllowDecimalPoint);
                        }
                    }
                    if (ddlIVA.SelectedValue == string.Empty && dr["IdIVA"] != System.DBNull.Value && Convert.ToInt32(dr["IdIVA"]) != (ddlIVA.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlIVA.SelectedValue)))
                    {
                        dr["IdIVA"] = (ddlIVA.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlIVA.SelectedValue));
                        dr["IVADescripcion"] = ddlIVA.SelectedValue;//ddlIVA.SelectedItem.Text;
                        dr["AlicuotaIVA"] = Convert.ToDecimal((ddlIVA.SelectedValue == string.Empty ? -1 : Convert.ToDecimal(ddlIVA.SelectedValue)));
                    }

                    ((Label)fila.FindControl("lblTotal")).Text = lblTotal;
                }
            }
            //AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MiPrecio.ListaPrecioDetalle, false, this.gvItems, true);
        }
        //protected void button_Click(object sender, EventArgs e)
        //{

        //    DataRow dr;
        //    foreach (GridViewRow fila in this.gvDatos.Rows)
        //    {
        //        if (fila.RowType == DataControlRowType.DataRow)
        //        {
        //            HiddenField hdfIdCondicionFiscal = (HiddenField)fila.FindControl("hdfIdCondicionFiscal");

        //            if (!string.IsNullOrWhiteSpace(hdfIdCondicionFiscal.Value))
        //            {
        //                VTAFacturas factura = new VTAFacturas();
        //                factura.Afiliado.CondicionFiscal.IdCondicionFiscal = Convert.ToInt32(hdfIdCondicionFiscal.Value);
        //                factura.FilialPuntoVenta.IdFilialPuntoVenta = 0;
        //                MisTiposComprobantes = FacturasF.TiposFacturasSeleccionarPorCondicionFiscal(factura);
        //                DropDownList ddlTipoComprobante = ((DropDownList)fila.FindControl("ddlTipoComprobante"));
        //                ddlTipoComprobante.DataSource = MisTiposComprobantes;
        //                ddlTipoComprobante.DataValueField = "IdTipoFactura";
        //                ddlTipoComprobante.DataTextField = "Descripcion";
        //                ddlTipoComprobante.DataBind();
        //                if (ddlTipoComprobante.Items.Count != 1)
        //                    AyudaProgramacion.AgregarItemSeleccione(ddlTipoComprobante, ObtenerMensajeSistema("SeleccioneOpcion"));
        //                ddlTipoComprobante.Enabled = true;
        //            }
        //        }
        //    }
        //}
        #endregion
        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            this.AsyncFileUpload1.FailedValidation = false;
            this.AsyncFileUpload1.ClearAllFilesFromPersistedStore();
        }
        protected void AsyncFileUpload1_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            this.AsyncFileUpload1.FailedValidation = false;
            this.AsyncFileUpload1.ClearAllFilesFromPersistedStore();
        }
        protected void FileUploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            try
            {
                string filename = System.IO.Path.GetFileName(this.AsyncFileUpload1.FileName);
                this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo = this.AsyncFileUpload1.FileName;

                string HoyTexto = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");

                TGEParametrosValores RutaDelArchivo = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ProcesosDatosDirectorioArchivo);

                string RutaDelArchivoTexto = string.Concat(RutaDelArchivo.ParametroValor.ToString(), this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo);

                if (File.Exists(RutaDelArchivoTexto))// + strFileName))
                {
                    File.Copy(RutaDelArchivoTexto, RutaDelArchivoTexto + "-" + HoyTexto);
                    File.Delete(RutaDelArchivoTexto);
                }
                this.AsyncFileUpload1.SaveAs(RutaDelArchivoTexto);
                this.MiProcesoProcesamiento.Proceso.TieneArchivo = true;

            }
            catch (Exception ex)
            {
                this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo = "";
                this.MostrarMensaje("Error al cargar el archivo.", true);
            }
        }
        protected void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            if (this.MiProcesoProcesamiento.Proceso.TieneArchivo)
            {

                this.MiProcesoProcesamiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MisSolicitudesPagosCargaMasiva = CuentasPagarF.SolicitudPagoCargaMasivaImportarArchivo(this.MiProcesoProcesamiento);
                if (this.MisSolicitudesPagosCargaMasiva.Rows.Count > 0)
                {
                    this.MiProcesoProcesamiento.Proceso.TieneArchivo = true;
                    this.gvDatos.DataSource = this.MisSolicitudesPagosCargaMasiva;
                    this.gvDatos.DataBind();
                }
                else
                {
                    throw new Exception("Error al cargar grilla.");
                }
            }
            else
            {
                this.MostrarMensaje("El archivo a importar no fue cargado.", true);
            }
        }
    }
}