using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;
using Compras;
using SKP.ASP.Controls;

namespace IU.Modulos.Compras.Controles
{
    public partial class InformesRecepcionesDatos : ControlesSeguros
    {
        public delegate void InformesRecepcionesDatosAceptarEventHandler(object sender, CmpInformesRecepciones e);
        public event InformesRecepcionesDatosAceptarEventHandler InformesRecepcionesModificarDatosAceptar;

        public delegate void InformesRecepcionesDatosCancelarEventHandler();
        public event InformesRecepcionesDatosCancelarEventHandler InformesRecepcionesModificarDatosCancelar;

        private CmpInformesRecepciones MiInforme
        {
            get { return (CmpInformesRecepciones)Session[this.MiSessionPagina + "InformesRecepcionesDatosMiInforme"]; }
            set { Session[this.MiSessionPagina + "InformesRecepcionesDatosMiInforme"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrBuscarOrdenCompraPopUp.OrdenesComprasBuscarSeleccionar += new IU.Modulos.Compras.Controles.OrdenesComprasBuscarPopUp.OrdenesComprasBuscarEventHandler(ctrBuscarOrdenCompraPopUp_OrdenCompraBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                if (this.MiInforme == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);

                }

            }

        }

        public void IniciarControl(CmpInformesRecepciones pInforme, Gestion pGestion)
        {
            this.MiInforme = pInforme;
            this.GestionControl = pGestion;

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtOrdenCompra.Enabled = true;
                    this.btnBuscarOrden.Visible = true;
                    this.txtNombreProveedor.Enabled = true;
                    //this.txtNumeroRemito.Enabled = true;
                    this.txtObservacion.Enabled = true;
                    this.txtMotivoCancelado.Enabled = false;
                    //this.cdFechaEmision.Enabled = true;
                    this.txtFechaEmision.Enabled = true;
                    //AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInforme.InformesRecepcionesDetalles, false, this.gvDatos, true);

                    break;
                case Gestion.Autorizar:

                    break;
                case Gestion.Anular:
                    
                    this.txtFechaEmision.Enabled = false;
                    this.MiInforme = ComprasF.InformesRecepcionesObtenerDatosCompletos(this.MiInforme);
                    this.MapearObjetoControles(this.MiInforme);
                    this.MapearAGrilla(this.MiInforme);
                    this.btnBuscarOrden.Visible = false;
                    this.txtMotivoCancelado.Enabled = true;
                    //AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInforme.InformesRecepcionesDetalles, false, this.gvDatos, true);
                    this.upInformesRecepcionesDetalle.Update();
                    break;

                case Gestion.Consultar:
                    this.btnAceptar.Visible = false;
                    this.txtFechaEmision.Enabled = false;
                    this.MiInforme = ComprasF.InformesRecepcionesObtenerDatosCompletos(this.MiInforme);
                    this.MapearObjetoControles(this.MiInforme);
                    this.MapearAGrilla(this.MiInforme);
                    this.btnBuscarOrden.Visible = false;

                    
                    this.upInformesRecepcionesDetalle.Update();
                    break;
                default:
                    break;
            }
        }

        #region POP UP OC
        protected void btnBuscarOrden_Click(object sender, EventArgs e)
        {
            this.ctrBuscarOrdenCompraPopUp.IniciarControl();
        }

        void ctrBuscarOrdenCompraPopUp_OrdenCompraBuscarSeleccionar(CmpOrdenesCompras pOrden)
        {

            this.txtOrdenCompra.Text = pOrden.IdOrdenCompra.ToString();
            //this.MiInforme.Proveedor.IdProveedor = pOrden.Proveedor.IdProveedor;
            this.txtNombreProveedor.Text = pOrden.Proveedor.RazonSocial;
            this.MiInforme.InformesRecepcionesDetalles = null;
            this.MapearOrdenAInforme( pOrden );
            this.MiInforme.OrdenCompra = pOrden;
            this.MiInforme.Proveedor = pOrden.Proveedor;
            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInforme.InformesRecepcionesDetalles,false, this.gvDatos, true);
            this.upInformesRecepcionesDetalle.Update(); //-> probablemente tenga que actualizar tambien este
            this.UpdatePanel1.Update();
            
        }

        void MapearOrdenAInforme(CmpOrdenesCompras pOrden)
        {
            CmpInformesRecepcionesDetalles infoDetalle;
            foreach (CmpOrdenesComprasDetalles ordenDetalle in pOrden.OrdenesComprasDetalles)
            {
                if (ordenDetalle.Cantidad > (ordenDetalle.CantidadRecibida == null ? 0 : Convert.ToInt32(ordenDetalle.CantidadRecibida)))
                {
                    infoDetalle = new CmpInformesRecepcionesDetalles();
                    infoDetalle.IdOrdenCompraDetalle = ordenDetalle.IdOrdenCompraDetalle;
                    infoDetalle.CantidadPedida = ordenDetalle.Cantidad - Convert.ToInt32(ordenDetalle.CantidadRecibida);
                    //infoDetalle.CantidadRecibida = Convert.ToInt32(ordenDetalle.CantidadRecibida);
                    //infoDetalle.Descripcion = ordenDetalle.Descripcion;
                    infoDetalle.Producto = ordenDetalle.Producto;
                    //infoDetalle.CantidadPagada = (ordenDetalle.CantidadPagada == null ? 0 : Convert.ToInt32(ordenDetalle.CantidadPagada)); // - (ordenDetalle.CantidadRecibida == null ? 0 : Convert.ToInt32(ordenDetalle.CantidadRecibida));
                    this.MiInforme.InformesRecepcionesDetalles.Add(infoDetalle);
                }
            }
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            string txtCodigo = ((TextBox)sender).Text;
            CmpOrdenesCompras parametro = new CmpOrdenesCompras();
            parametro.IdOrdenCompra = txtCodigo == null ? 0 : Convert.ToInt32(txtCodigo);
            parametro = ComprasF.OrdenCompraObtenerDatosCompletos(parametro);
            this.MiInforme.OrdenCompra.IdOrdenCompra = parametro.IdOrdenCompra;

            if (parametro.IdOrdenCompra > 0 &&  ( parametro.Estado.IdEstado == (int)EstadosOrdenesCompras.Activo || parametro.Estado.IdEstado == (int)EstadosOrdenesCompras.ParcialmenteRecibido ))
            {
                
                this.ctrBuscarOrdenCompraPopUp_OrdenCompraBuscarSeleccionar(parametro);
            }
            else
                this.ctrBuscarOrdenCompraPopUp.IniciarControl();

        }
        #endregion

        #region GRILLA

        private void MapearAGrilla(CmpInformesRecepciones pInforme)
        {
            foreach (CmpInformesRecepcionesDetalles detalle in pInforme.InformesRecepcionesDetalles)
            {
                CmpOrdenesComprasDetalles ordenDetalle = pInforme.OrdenCompra.OrdenesComprasDetalles.Find(x => x.IdOrdenCompraDetalle == detalle.IdOrdenCompraDetalle);
                //detalle.CantidadPagada = ordenDetalle.CantidadPagada == null ? 0 : Convert.ToInt32(ordenDetalle.CantidadPagada);
                //detalle.Descripcion = ordenDetalle.Descripcion;
                detalle.Producto = ordenDetalle.Producto;
                //esta hecho para que cada informe reste la cantidad pedida que YA FUE RECIBIDA EN OTRO INFORME ANTERIOR
                detalle.CantidadPedida = ordenDetalle.Cantidad - ( Convert.ToInt32(ordenDetalle.CantidadRecibida) - detalle.CantidadRecibida);
            }
            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInforme.InformesRecepcionesDetalles, false, this.gvDatos, true);
        }
             
        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {

                int cantidadRecibida = ((TextBox)fila.FindControl("txtCantidadRecibida")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtCantidadRecibida")).Text);

                int cantidadDevuelta = ((TextBox)fila.FindControl("txtCantidadDevuelta")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtCantidadDevuelta")).Text);

                int cantidadCambio = ((TextBox)fila.FindControl("txtCantidadCambio")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtCantidadCambio")).Text);

                //int cantidadPagada = ((TextBox)fila.FindControl("txtCantidadPagada")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtCantidadPagada")).Text);
                
                if (cantidadRecibida != 0)
                {
                    this.MiInforme.InformesRecepcionesDetalles[fila.RowIndex].CantidadRecibida = Convert.ToInt32(cantidadRecibida);
                }

                if (cantidadDevuelta != 0)
                {
                    this.MiInforme.InformesRecepcionesDetalles[fila.RowIndex].CantidadDevuelta = Convert.ToInt32(cantidadDevuelta);
                }

                if (cantidadCambio != 0)
                {
                    this.MiInforme.InformesRecepcionesDetalles[fila.RowIndex].CantidadCambio = Convert.ToInt32(cantidadCambio);
                }

                //if (cantidadPagada != 0)
                //{
                //    this.MiInforme.InformesRecepcionesDetalles[fila.RowIndex].CantidadPagada = Convert.ToInt32(cantidadPagada);
                //}

            }

            this.ActualizarOrdenCompra(this.MiInforme.InformesRecepcionesDetalles);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    NumericTextBox cantidadRecibida = (NumericTextBox)e.Row.FindControl("txtCantidadRecibida");
                    cantidadRecibida.Enabled = true;
                    cantidadRecibida.Attributes.Add("onchange", "CalcularItem();");


                    NumericTextBox cantidadDevuelta = (NumericTextBox)e.Row.FindControl("txtCantidadDevuelta");
                    cantidadDevuelta.Enabled = true;
                    cantidadDevuelta.Attributes.Add("onchange", "CalcularItem();");

                    NumericTextBox cantidadCambio = (NumericTextBox)e.Row.FindControl("txtCantidadCambio");
                    cantidadCambio.Enabled = true;
                    cantidadCambio.Attributes.Add("onchange", "CalcularItem();");

                    //NumericTextBox cantidadPagada = (NumericTextBox)e.Row.FindControl("txtCantidadPagada");
                    //cantidadPagada.Enabled = true;
                    //cantidadPagada.Attributes.Add("onchange", "CalcularItem();");
                }


            }

        }

        public void ActualizarOrdenCompra(List<CmpInformesRecepcionesDetalles> pInfoDet)
        {
            foreach (CmpInformesRecepcionesDetalles det in pInfoDet)
            {
                //solo le sumo a la orden de compra la cantidad que se recibio en esta recepcion 
                this.MiInforme.OrdenCompra.OrdenesComprasDetalles.Find(x => x.IdOrdenCompraDetalle == det.IdOrdenCompraDetalle).CantidadRecibida += det.CantidadRecibida; 

            }
        }
        #endregion

        #region Mapeo Datos
        private void MapearControlesObjeto(CmpInformesRecepciones pInforme)
        {
            //this.MiInforme.IdRemito = (this.txtNumeroRemito.Text) == string.Empty ? 0 : Convert.ToInt32(this.txtNumeroRemito.Text);
            this.MiInforme.FechaEmision = Convert.ToDateTime(this.txtFechaEmision.Text);
            this.MiInforme.Observacion = this.txtObservacion.Text;
            this.MiInforme.MotivoCancelado = this.txtMotivoCancelado.Text;

            
        }

        private void MapearObjetoControles(CmpInformesRecepciones pInforme)
        {
            //mapeo de Informes Recepciones
            this.txtOrdenCompra.Text = (pInforme.OrdenCompra.IdOrdenCompra).ToString();
            this.txtFechaEmision.Text = pInforme.FechaEmision.ToString();
            this.txtMotivoCancelado.Text = pInforme.MotivoCancelado;
            this.txtObservacion.Text = pInforme.Observacion;
            this.txtNombreProveedor.Text = pInforme.OrdenCompra.Proveedor.RazonSocial;
            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInforme.InformesRecepcionesDetalles, false, this.gvDatos, true);
        }

        #endregion

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.InformesRecepcionesModificarDatosAceptar != null)
                this.InformesRecepcionesModificarDatosAceptar(null, this.MiInforme);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
           
            if (!this.Page.IsValid)
                return;
            bool guardo = true;

            this.MiInforme.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.PersistirDatosGrilla();
                    this.MapearControlesObjeto(this.MiInforme);
                    this.MiInforme.EstadoColeccion = EstadoColecciones.Agregado;

                    foreach (CmpInformesRecepcionesDetalles det in this.MiInforme.InformesRecepcionesDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Agregado;
                    }
                    foreach (CmpOrdenesComprasDetalles det in this.MiInforme.OrdenCompra.OrdenesComprasDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Modificado;
                    }
                    guardo = ComprasF.InformesRecepcionesAgregar(this.MiInforme);
                    break;
                case Gestion.Autorizar:

                    break;
                case Gestion.Anular:
                    this.MapearControlesObjeto(this.MiInforme);
                    this.MiInforme.EstadoColeccion = EstadoColecciones.Borrado;
                    this.MiInforme.Estado.IdEstado = (int)EstadosOrdenesCompras.Baja;
                    foreach (CmpInformesRecepcionesDetalles det in this.MiInforme.InformesRecepcionesDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Borrado;
                    }
                    foreach (CmpOrdenesComprasDetalles det in this.MiInforme.OrdenCompra.OrdenesComprasDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Borrado;
                    }
                    guardo = ComprasF.InformesRecepcionesAnular(this.MiInforme);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiInforme.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiInforme.CodigoMensaje, true, this.MiInforme.CodigoMensajeArgs);
            }

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            if (this.InformesRecepcionesModificarDatosCancelar != null)
                this.InformesRecepcionesModificarDatosCancelar();
        }
    }
}