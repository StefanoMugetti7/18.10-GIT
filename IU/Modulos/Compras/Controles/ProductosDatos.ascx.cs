using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Compras;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Afiliados.Entidades;
using EO.Web.Internal;
using Servicio.AccesoDatos;
using System.Data;

namespace IU.Modulos.Compras.Controles
{
    public partial class ProductosDatos : ControlesSeguros
    {
        private CMPProductos MiProducto
        {
            get { return (CMPProductos)Session[this.MiSessionPagina + "ProductosDatosMiProducto"]; }
            set { Session[this.MiSessionPagina + "ProductosDatosMiProducto"] = value; }
        }

        public delegate void ModificarDatosAceptarEventHandler(object sender, CMPProductos e);
        public event ModificarDatosAceptarEventHandler ModificarDatosAceptar;
        public delegate void ModificarDatosCancelarEventHandler();
        public event ModificarDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnAceptar);
            BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoBarras, this.btnAceptar);
            BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnAceptar);
            BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtStockMaximo, this.btnAceptar);
            BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtStockMinimo, this.btnAceptar);
            BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtStockRecomendado, this.btnAceptar);
            
            if (this.IsPostBack)
            {
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Cuenta Bancaria
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CMPProductos pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiProducto = pParametro;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //pParametro.FechaAlta = DateTime.Now;
                    //AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, pParametro.UsuarioAlta);
                    this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ctrCamposValores.IniciarControl(this.MiProducto, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    pParametro.Compra = true;
                    pParametro.Venta = true;
                    this.MiProducto = ComprasF.ProductosObtenerPorIdProducto(pParametro);
                    this.MapearObjetoAControles(this.MiProducto);
                    break;
                case Gestion.Consultar:
                    pParametro.Compra = true;
                    pParametro.Venta = true;
                    txtStockMaximo.Enabled = false;
                    txtStockMinimo.Enabled = false;
                    txtStockRecomendado.Enabled = false;
                    txtDescripcion.Enabled = false;
                    ddlIva.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.MiProducto = ComprasF.ProductosObtenerPorIdProducto(pParametro);
                    this.MapearObjetoAControles(this.MiProducto);
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnCancelar.Visible = true;
                    break;
                default:
                    break;
            }
        }
        protected void ddlTiposProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTiposProductos.SelectedValue))
            {
                this.MiProducto.TipoProducto.IdTipoProducto = Convert.ToInt32(this.ddlTiposProductos.SelectedValue);
                this.ctrCamposValores.IniciarControl(this.MiProducto, this.MiProducto.TipoProducto, this.GestionControl);
            }
        }
        protected void CrearImagenCodigoBarra(object sender, EventArgs e)
        {
            BarcodeInter25 code25;
            //if(txtCodigoBarras.Text.Length > 0 && txtCodigoBarras.Text.Length < 13)
            //{
            //    MostrarMensaje("El Codigo de Barras Debe Tener 13 Digitos", true);
            //    imgCodigoBarras.Visible = false;
            //    return;
            //}
                 
            //if (txtCodigoBarras.Text.Length == 13)
            //{
                code25 = new BarcodeInter25();
                code25.GenerateChecksum = true;
                code25.ChecksumText = true;
                code25.StartStopText = true;
                code25.BarHeight = 30f;
                code25.GuardBars = true;
                code25.Size = 10f;
                // Codigo de Barra AFIP Factura Electronica
                code25.Code = txtCodigoBarras.Text;
                code25.AltText = code25.Code;
                if (code25.Code.Trim().Length > 0)
                {
                    System.Drawing.Image img = code25.CreateDrawingImage(Color.Black, Color.White);
                    //System.Drawing.Bitmap newImg = AyudaProgramacionLN.ScaleImage(img, Convert.ToInt32(this.txtCodigoBarras.Width.Value), Convert.ToInt32(this.txtCodigoBarras.Height.Value));

                    using (var ms = new MemoryStream())
                    {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                        string base64String = Convert.ToBase64String((byte[])new System.Drawing.ImageConverter().ConvertTo(img, typeof(byte[])));
                        //string base64String = this.UsuarioEmpresa.Logo == null ? string.Empty : Convert.ToBase64String(this.UsuarioEmpresa.Logo, 0, this.UsuarioEmpresa.Logo.Length);
                        if (base64String != string.Empty)
                        {
                            this.imgCodigoBarras.ImageUrl = "data:image/png;base64," + base64String;
                            this.imgCodigoBarras.Width = 150;
                            this.imgCodigoBarras.Height = 50;
                            this.imgCodigoBarras.AlternateText = "Codigo de Barras";
                            this.imgCodigoBarras.Visible = true;
                        }
                    }
                }


            //}
            //else
            //{
            //    imgCodigoBarras.Visible = false;
            //}
                      
          //break;
                  
        }
        private void MapearObjetoAControles(CMPProductos pParametro)
        {
            this.txtCodigo.Text = pParametro.IdProducto.ToString();
            this.txtDescripcion.Text = pParametro.Descripcion;
            this.chkCompra.Checked = pParametro.Compra;
            this.chkVenta.Checked = pParametro.Venta;

            //ListItem item = this.ddlIva.Items.FindByValue(pParametro.IdIva.ToString());
            //if (item == null)
            //    this.ddlIva.Items.Add(new ListItem(pParametro.AliCuotaIva.ToString(), pParametro.IdIva.ToString()));
          //  this.ddlIva.SelectedValue = pParametro.IdIva.ToString();

            ListItem item = this.ddlFamilias.Items.FindByValue(pParametro.Familia.IdFamilia.ToString());
            if (item == null)
                this.ddlFamilias.Items.Add(new ListItem(pParametro.Familia.Descripcion, pParametro.Familia.IdFamilia.ToString()));
            this.ddlFamilias.SelectedValue = pParametro.Familia.IdFamilia.ToString();

            item = this.ddlTiposProductos.Items.FindByValue(pParametro.TipoProducto.IdTipoProducto.ToString());
            if (item == null)
                this.ddlTiposProductos.Items.Add(new ListItem(pParametro.TipoProducto.Descripcion, pParametro.TipoProducto.IdTipoProducto.ToString()));
            this.ddlTiposProductos.SelectedValue = pParametro.TipoProducto.IdTipoProducto.ToString();

            this.ddlUnidadMedida.SelectedValue = pParametro.UnidadMedida.IdUnidadMedida.ToString();
            
            this.txtStockMaximo.Text = pParametro.StockMaximo.ToString();
            this.txtStockMinimo.Text = pParametro.StockMinimo.ToString();
            this.txtStockRecomendado.Text = pParametro.StockRecomendado.ToString();
            this.txtCodigoBarras.Text = pParametro.CodigoBarra.ToString();
            if (txtCodigoBarras.Text.Length > 0)
            {
                CrearImagenCodigoBarra(null, EventArgs.Empty);
            }
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.ctrAuditoria.IniciarControl(pParametro);
            this.ctrCamposValores.IniciarControl(this.MiProducto, new Objeto(), this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiProducto, this.MiProducto.TipoProducto, this.GestionControl);

            DataTable data = ComprasF.ProductosObtenerListaCompradores(pParametro);
            if (data.Rows.Count > 0)
            {
                this.gvProveedores.DataSource = data;
                this.gvProveedores.DataBind();
            }

            item = this.ddlIva.Items.FindByValue(pParametro.IdIva.ToString());
            if (item != null)
            {
                //this.ddlTiposProductos.Items.Add(new ListItem(pParametro.TipoProducto.Descripcion, pParametro.TipoProducto.IdTipoProducto.ToString()));
                this.ddlIva.SelectedValue = pParametro.IdIva.ToString();
            }
        }
        private void MapearControlesAObjeto(CMPProductos pParametro)
        {
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.CodigoBarra = this.txtCodigoBarras.Text;
            pParametro.Familia.IdFamilia = Convert.ToInt32(this.ddlFamilias.SelectedValue);
            pParametro.Familia.Descripcion = this.ddlFamilias.SelectedItem.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.StockMinimo = this.txtStockMinimo.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtStockMinimo.Text);
            pParametro.StockRecomendado = this.txtStockRecomendado.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtStockRecomendado.Text);
            pParametro.StockMaximo = this.txtStockMaximo.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtStockMaximo.Text);
            pParametro.Compra = this.chkCompra.Checked;
            pParametro.Venta = this.chkVenta.Checked;
            pParametro.TipoProducto.IdTipoProducto = this.ddlTiposProductos.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlTiposProductos.SelectedValue);
            pParametro.TipoProducto.Descripcion = this.ddlTiposProductos.SelectedValue == string.Empty ? default(string) : this.ddlTiposProductos.SelectedItem.Text;
            pParametro.UnidadMedida.IdUnidadMedida = this.ddlUnidadMedida.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlUnidadMedida.SelectedValue);
            pParametro.UnidadMedida.Descripcion = this.ddlUnidadMedida.SelectedValue == string.Empty ? default(string) : this.ddlUnidadMedida.SelectedItem.Text;
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
            pParametro.IdIva = ddlIva.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlIva.SelectedValue);
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            TGEIVA pParamaetro = new TGEIVA();
            pParamaetro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario( UsuarioActivo);

          

            ddlIva.DataSource = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(pParamaetro);
            ddlIva.DataValueField = "IdIva";
            ddlIva.DataTextField = "Descripcion";
            ddlIva.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlIva, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            CMPFamilias filtro = new CMPFamilias();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            this.ddlFamilias.DataSource = ComprasF.FamiliasObtenerListaFiltro(filtro);
            this.ddlFamilias.DataValueField = "IdFamilia";
            this.ddlFamilias.DataTextField = "Descripcion";
            this.ddlFamilias.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFamilias, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTiposProductos.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresCodigos.CmpTiposProductos);
            this.ddlTiposProductos.DataValueField = "IdListaValorDetalle";
            this.ddlTiposProductos.DataTextField = "Descripcion";
            this.ddlTiposProductos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposProductos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlUnidadMedida.DataSource = TGEGeneralesF.TGEUnidadesMedidasObtenerLista();
            this.ddlUnidadMedida.DataValueField = "IdUnidadMedida";
            this.ddlUnidadMedida.DataTextField = "Descripcion";
            this.ddlUnidadMedida.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlUnidadMedida, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiProducto);
            this.MiProducto.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.MiProducto.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = ComprasF.ProductosAgregar(this.MiProducto);
                    break;
                case Gestion.Modificar:
                    guardo = ComprasF.ProductosModificar(this.MiProducto);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiProducto.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiProducto.CodigoMensaje, true, this.MiProducto.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar(null, this.MiProducto);
        }
    }
}
