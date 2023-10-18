using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Generales.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Facturas;
using Afiliados.Entidades;
using Afiliados;
using System.Globalization;
using Compras.Entidades;
using Evol.Controls;
using Contabilidad.Entidades;
using Contabilidad;

namespace IU.Modulos.Facturas.Controles
{
    public partial class FacturacionesHabitualesDatos : ControlesSeguros
    {
        private VTAFacturacionesHabituales MiFacturacionHabitual
        {
            get { return (VTAFacturacionesHabituales)Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMiFacturacionHabitual"]; }
            set { Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMiFacturacionHabitual"] = value; }
        }

        private List<TGEListasValoresSistemasDetalles> MisTiposFacturas
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMisTiposFacturas"]; }
            set { Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMisTiposFacturas"] = value; }
        }

        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMisIvas"]; }
            set { Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMisIvas"] = value; }
        }

        private List<TGEEstados> MisEstados
        {
            get { return (List<TGEEstados>)Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMisEstados"]; }
            set { Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMisEstados"] = value; }
        }
        private List<CtbCentrosCostosProrrateos> MisCentrosCostos
        {
            get { return (List<CtbCentrosCostosProrrateos>)Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMisCentrosCostos"]; }
            set { Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMisCentrosCostos"] = value; }
        }
        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "FacturacionesHabitualesDatosMiIndiceDetalleModificar"] = value; }
        }

        public delegate void FacturacionesHabitualesDatosAceptarEventHandler(VTAFacturacionesHabituales e, Gestion f);
        public event FacturacionesHabitualesDatosAceptarEventHandler ModificarDatosAceptar;

        public delegate void FacturacionesHabitualesDatosCancelarEventHandler(VTAFacturacionesHabituales e, Gestion f);
        public event FacturacionesHabitualesDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new CuentasPagar.Controles.BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            this.popUpMensajes.popUpMensajesPostBackAceptar+=new Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiFacturacionHabitual == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
                this.ctrCamposValores.IniciarControl(MiFacturacionHabitual, new Objeto(), this.GestionControl);
            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }

        public void IniciarControl(VTAFacturacionesHabituales pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiFacturacionHabitual = pParametro;

            AfiAfiliados afi = new AfiAfiliados();
            afi.IdAfiliado = this.MiFacturacionHabitual.IdAfiliado;
            afi = AfiliadosF.AfiliadosObtenerDatos(afi);
            this.ctrClienteDatosCabecera.IniciarControl(afi);

            this.MisEstados = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            TGEIVA pParamaetro = new TGEIVA();

            pParamaetro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(pParamaetro);

            CtbCentrosCostosProrrateos ccp = new CtbCentrosCostosProrrateos();
            ccp.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
            this.MisCentrosCostos = ContabilidadF.CentrosCostosProrrateosObtenerCombo(ccp);

            this.CargarCombos();            
            
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
                    this.ctrCamposValores.IniciarControl(this.MiFacturacionHabitual, new Objeto(), this.GestionControl);
                    this.IniciarGrilla();
                    break;
                case Gestion.Modificar:
                    // ANULAR
                    this.MiFacturacionHabitual = FacturasF.FacturacionesHabitualesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoControles(this.MiFacturacionHabitual);
                    break;
                case Gestion.Consultar:
                    this.MiFacturacionHabitual = FacturasF.FacturacionesHabitualesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoControles(this.MiFacturacionHabitual);
                    this.txtDescripcion.Enabled = false;
                    this.txtFacturaDiaVencimiento.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.ddlTipoFacturaHabitual.Enabled = false;
                    this.txtIncrementoPeriodoMeses.Enabled = false;
                    this.txtIncrementoPorcentaje.Enabled = false;
                    this.txtPeriodoFin.Enabled = false;
                    this.txtPeriodoIncio.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlTipoFacturaHabitual.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposFacturacionesHabituales);
            this.ddlTipoFacturaHabitual.DataValueField = "IdListaValorDetalle";
            this.ddlTipoFacturaHabitual.DataTextField = "Descripcion";
            this.ddlTipoFacturaHabitual.DataBind();
            if (this.ddlTipoFacturaHabitual.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFacturaHabitual, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MisTiposFacturas = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposFacturas);
            this.ddlTipoFactura.DataSource = this.MisTiposFacturas;
            this.ddlTipoFactura.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoFactura.DataTextField = "Descripcion";
            this.ddlTipoFactura.DataBind();
            if (this.ddlTipoFactura.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = this.MisEstados;
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }
                
        private void IniciarGrilla()
        {
            VTAFacturacionesHabitualesDetalles item;
            for (int i = 0; i < 2; i++)
            {
                item = new VTAFacturacionesHabitualesDetalles();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiFacturacionHabitual.FacturacionesHabitualesDetalles.Add(item);
                item.IndiceColeccion = this.MiFacturacionHabitual.FacturacionesHabitualesDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<VTAFacturacionesHabitualesDetalles>(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles, false, this.gvItems, true);
        }

        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                string codigo = ((TextBox)fila.FindControl("txtCodigo")).Text;
                DropDownList ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                DropDownList ddlEstados = ((DropDownList)fila.FindControl("ddlEstados"));
                DropDownList ddlCentrosCostos = ((DropDownList)fila.FindControl("ddlCentrosCostos"));
                decimal importe = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtImporte")).Decimal;
                //string importeDesc = ((Label)fila.FindControl("lblDescuentoImporte")).Text == string.Empty ? 0 : Convert.ToString(((Label)fila.FindControl("lblDescuentoImporte")).Text);
                decimal Cantidad = ((CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                bool modifica = false;
                if (codigo != string.Empty && Convert.ToInt32(codigo) > 0
                    && Convert.ToInt32(codigo) != this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Producto.IdProducto)
                {
                    modifica = true;
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Producto.IdProducto = Convert.ToInt32(codigo);
                }
                if (Cantidad != this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Cantidad)
                {
                    modifica = true;
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Cantidad = Cantidad;
                }
                if (importe != this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Importe)
                {
                    modifica = true;
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Importe = importe;
                }
                if (ddlAlicuotaIVA.SelectedValue != string.Empty
                    && ddlAlicuotaIVA.SelectedValue != this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].IVA.Alicuota.ToString())
                {
                    modifica = true;
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].IVA = this.MisIvas[ddlAlicuotaIVA.SelectedIndex];
                }

                if (ddlEstados.SelectedValue != string.Empty
                    && ddlEstados.SelectedValue != this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Estado.IdEstado.ToString())
                {
                    modifica = true;
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].Estado = this.MisEstados[ddlEstados.SelectedIndex];
                }
                if (ddlCentrosCostos.SelectedValue != this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.DataItemIndex].CentroCostoProrrateo.IdCentroCostoProrrateo.ToString())
                {
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.DataItemIndex].CentroCostoProrrateo = this.MisCentrosCostos.Find(x => x.IdCentroCostoProrrateo == (ddlCentrosCostos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlCentrosCostos.SelectedValue)));
                    modifica = true;
                }
                if (modifica)
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[fila.RowIndex], GestionControl);
            }
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            //if (e.CommandName == "Borrar")
            //{
            //    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles.RemoveAt(this.MiIndiceDetalleModificar);
            //    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles = AyudaProgramacion.AcomodarIndices<VTAFacturacionesHabitualesDetalles>(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles);
            //    AyudaProgramacion.CargarGrillaListas<VTAFacturacionesHabitualesDetalles>(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles, false, this.gvItems, true);
            //}
            if (e.CommandName == "BuscarProducto")
            {
                this.ctrBuscarProductoPopUp.IniciarControl(EnumTiposProductos.Ventas, new CMPProductos());
            }
        }

        void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(global::Compras.Entidades.CMPProductos e)
        {
            this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[this.MiIndiceDetalleModificar].Producto = e;
            AyudaProgramacion.CargarGrillaListas<VTAFacturacionesHabitualesDetalles>(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles, false, this.gvItems, true);
            this.upItems.Update();
        }

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            VTAFacturacionesHabitualesDetalles item;
            item = new VTAFacturacionesHabitualesDetalles();
            item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
            this.MiFacturacionHabitual.FacturacionesHabitualesDetalles.Add(item);
            item.IndiceColeccion = this.MiFacturacionHabitual.FacturacionesHabitualesDetalles.IndexOf(item);
            AyudaProgramacion.CargarGrillaListas<VTAFacturacionesHabitualesDetalles>(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles, false, this.gvItems, true);
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
            int IndiceColeccion = row.RowIndex;

            string contenido = ((TextBox)sender).Text;
            this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[IndiceColeccion].Producto.IdProducto = Convert.ToInt32(contenido);
            this.MiFacturacionHabitual.FacturacionesHabitualesDetalles[IndiceColeccion].Producto.Venta = true;

            AyudaProgramacion.CargarGrillaListas<VTAFacturacionesHabitualesDetalles>(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles, false, this.gvItems, true);
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VTAFacturacionesHabitualesDetalles item = (VTAFacturacionesHabitualesDetalles)e.Row.DataItem;

                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));
                ddlAlicuotaIVA.DataSource = this.MisIvas;
                ddlAlicuotaIVA.DataValueField = "Alicuota";
                ddlAlicuotaIVA.DataTextField = "Descripcion";
                ddlAlicuotaIVA.DataBind();

                DropDownList ddlEstados = ((DropDownList)e.Row.FindControl("ddlEstados"));
                ddlEstados.DataSource = this.MisEstados;
                ddlEstados.DataValueField = "IdEstado";
                ddlEstados.DataTextField = "Descripcion";
                ddlEstados.DataBind();

                DropDownList ddlCentrosCostos = ((DropDownList)e.Row.FindControl("ddlCentrosCostos"));
                ddlCentrosCostos.DataSource = this.MisCentrosCostos;
                ddlCentrosCostos.DataValueField = "IdCentroCostoProrrateo";
                ddlCentrosCostos.DataTextField = "CentroCostoProrrateo";
                ddlCentrosCostos.DataBind();
                if (ddlCentrosCostos.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(ddlCentrosCostos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                //if (ddlAlicuotaIVA.Items.Count!=1);
                //    AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                //ddlAlicuotaIVA.SelectedValue = item.IVA.Alicuota.ToString();
                //ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                ddlAlicuotaIVA.Enabled = true;
                ddlEstados.Enabled = true;
                ddlCentrosCostos.Enabled = true;
                if(item.IVA.IdIVA > 0)
                    ddlAlicuotaIVA.SelectedValue = item.IVA.Alicuota.ToString();

                ddlEstados.SelectedValue = item.Estado.IdEstado.ToString();
                
                if (this.GestionControl == Gestion.Agregar
                    || this.GestionControl == Gestion.Modificar)
                {
                    ImageButton ibtnProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                    ibtnProducto.Visible = true;
                    //ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                    //btnEliminar.Visible = true;
                    CurrencyTextBox codigo = (CurrencyTextBox)e.Row.FindControl("txtCodigo");
                    codigo.Enabled = true;
                    CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
                    cantidad.Enabled = true;
                    //CurrencyTextBox precioUnitario = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                    //precioUnitario.Attributes.Add("onchange", "CalcularItem();");
                    //precioUnitario.Enabled = true;

                    Evol.Controls.CurrencyTextBox importe = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtImporte");
                    importe.Enabled = true;

                    if (item.CentroCostoProrrateo.IdCentroCostoProrrateo > 0)
                        ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                    else if (ddlCentrosCostos.Items.Count != 1)
                        ddlCentrosCostos.SelectedValue = string.Empty;
                }
                else if (this.GestionControl == Gestion.Consultar)
                {
                    ddlAlicuotaIVA.Enabled = false;
                    ddlEstados.Enabled = false;
                    ddlCentrosCostos.Enabled = false;

                }
            }
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar(this.MiFacturacionHabitual, this.GestionControl);
        }

        protected void MapearControlesAObjeto(VTAFacturacionesHabituales pParametro)
        {
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdListaValorSistemaDetalle;
            pParametro.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
            pParametro.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;
            pParametro.FechaAlta = Convert.ToDateTime(this.txtFechaAlta.Text);
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.TipoFacturacionHabitual.IdTipoFacturacionHabitual = Convert.ToInt32(this.ddlTipoFacturaHabitual.SelectedValue);
            pParametro.IncrementoPorcentaje = this.txtIncrementoPorcentaje.Decimal ;// == string.Empty ? 0 : Convert.ToDecimal(this.txtIncrementoPorcentaje.Text);
            pParametro.IncrementoPeriodoMeses = this.txtIncrementoPeriodoMeses.Text == string.Empty ? 0 : Convert.ToInt32(this.txtIncrementoPeriodoMeses.Text);
            pParametro.PeriodoInicio = Convert.ToInt32(this.txtPeriodoIncio.Text);
            pParametro.PeriodoFin = this.txtPeriodoFin.Text == string.Empty ? default(int?) : Convert.ToInt32(this.txtPeriodoFin.Text);
            pParametro.FacturaDiaVencimiento = this.txtFacturaDiaVencimiento.Text == string.Empty ? default(int?) : Convert.ToInt32(this.txtFacturaDiaVencimiento.Text);
            pParametro.CorreoElectronico = this.txtCorreoElectronico.Text;
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
            //this.PersistirDatosGrilla();
        }

        private void MapearObjetoControles(VTAFacturacionesHabituales pFactura)
        {
            //this.MapearObjetoAControlesAfiliado(pFactura.Afiliado);
            this.ddlEstados.SelectedValue = pFactura.Estado.IdEstado.ToString();
            this.txtFechaAlta.Text = pFactura.FechaAlta.ToShortDateString();
            this.ddlTipoFacturaHabitual.SelectedValue = pFactura.TipoFacturacionHabitual.IdTipoFacturacionHabitual.ToString();
            this.ddlTipoFactura.SelectedValue = pFactura.TipoFactura.IdTipoFactura.ToString();
            this.txtDescripcion.Text = pFactura.Descripcion;
            this.txtIncrementoPorcentaje.Text = pFactura.IncrementoPorcentaje.ToString();
            this.txtIncrementoPeriodoMeses.Text = pFactura.IncrementoPeriodoMeses.ToString();
            this.txtPeriodoIncio.Text = pFactura.PeriodoInicio.ToString();
            this.txtPeriodoFin.Text = pFactura.PeriodoFin.HasValue ? pFactura.PeriodoFin.Value.ToString() : string.Empty;
            this.txtFacturaDiaVencimiento.Text = pFactura.FacturaDiaVencimiento.HasValue ? pFactura.FacturaDiaVencimiento.Value.ToString() : string.Empty;
            this.txtCorreoElectronico.Text = pFactura.CorreoElectronico;
            AyudaProgramacion.CargarGrillaListas<VTAFacturacionesHabitualesDetalles>(pFactura.FacturacionesHabitualesDetalles, false, this.gvItems, true);
            this.ctrCamposValores.IniciarControl(pFactura, new Objeto(), this.GestionControl);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiFacturacionHabitual);

            this.MiFacturacionHabitual.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiFacturacionHabitual.FacturacionesHabitualesDetalles = this.MiFacturacionHabitual.FacturacionesHabitualesDetalles.Where(x => x.EstadoColeccion == EstadoColecciones.Agregado).ToList();
                    AyudaProgramacion.AcomodarIndices<VTAFacturacionesHabitualesDetalles>(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles);
                    AyudaProgramacion.CargarGrillaListas(this.MiFacturacionHabitual.FacturacionesHabitualesDetalles, false, this.gvItems, true);
                    this.upItems.Update();
                    //this.MiFacturacionHabitual.IdUsuarioAlta = this.MiFacturacionHabitual.UsuarioLogueado.IdUsuario;
                    //this.MiFacturacionHabitual.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    guardo = FacturasF.FacturacionesHabitualesAgregar(this.MiFacturacionHabitual);
                    break;
                case Gestion.Modificar:
                    guardo = FacturasF.FacturacionesHabitualesModificar(this.MiFacturacionHabitual);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFacturacionHabitual.CodigoMensaje));
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiFacturacionHabitual.CodigoMensaje, true, this.MiFacturacionHabitual.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar(this.MiFacturacionHabitual, this.GestionControl);
        }
    }
}