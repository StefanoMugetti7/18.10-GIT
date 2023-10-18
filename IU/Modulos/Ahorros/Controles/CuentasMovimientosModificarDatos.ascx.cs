using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Ahorros.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Ahorros;
using Generales.Entidades;
using Contabilidad.Entidades;
using Contabilidad;
using System.Collections.Generic;
using Reportes.FachadaNegocio;
using Afiliados.Entidades;
using System.Xml;

namespace IU.Modulos.Ahorros.Controles
{
    public partial class CuentasMovimientosModificarDatos : ControlesSeguros
    {
        private AhoCuentasMovimientos MiAhoCuentasMovimientos
        {
            get { return (AhoCuentasMovimientos)Session[this.MiSessionPagina + "AhorroMiAhoCuentasMovimientos"]; }
            set { Session[this.MiSessionPagina + "AhorroMiAhoCuentasMovimientos"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "CuentasMovimientosModificarDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "CuentasMovimientosModificarDatosMisMonedas"] = value; }
        }
        private List<AfiAfiliados> MisAfiliadosCombo
        {
            get { return (List<AfiAfiliados>)Session[this.MiSessionPagina + "CuentasMovimientosModificarDatosMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "CuentasMovimientosModificarDatosMisAfiliados"] = value; }
        }

        public delegate void CuentaMovimientosDatosAceptarEventHandler(object sender, AhoCuentasMovimientos e);
        public event CuentaMovimientosDatosAceptarEventHandler CuentaMovimientosModificarDatosAceptar;

        public delegate void CuentaMovimientosDatosCancelarEventHandler();
        public event CuentaMovimientosDatosCancelarEventHandler CuentaMovimientosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiAhoCuentasMovimientos == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }

                if (this.paginaSegura.paginaActual.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroDepositosEspeciales
                    || this.paginaSegura.paginaActual.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroExtraccionesEspeciales)
                {
                    string mensaje = this.ObtenerMensajeSistema("ConfirmarOperacionEspecial");
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    this.btnAceptar.Attributes.Add("OnClick", funcion);
                }
            }
        }
        public void IniciarControl(AhoCuentasMovimientos pCuentasMovimientos, EnumTGETiposMovimientos pMovimiento)
        {
            IniciarControl(pCuentasMovimientos, pMovimiento, Gestion.Agregar);
        }
            /// <summary>
            /// Inicializa el control de Movimientos de Cuentas de Ahorros
            /// </summary>
            /// <param name="pRequisicion"></param>
            public void IniciarControl(AhoCuentasMovimientos pCuentasMovimientos, EnumTGETiposMovimientos pMovimiento, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiAhoCuentasMovimientos = pCuentasMovimientos;
            this.MiAhoCuentasMovimientos.TipoOperacion.TipoMovimiento.IdTipoMovimiento = Convert.ToInt32(pMovimiento);
            //this.MiAhoCuentasMovimientos.TipoOperacion.IdTipoOperacion = Convert.ToInt32(pMovimiento);
            this.ctrComentarios.IniciarControl(new AhoCuentasMovimientos(), this.GestionControl);
            this.DeshabilitarControles();
            this.CargarCombos();
            this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
            this.txtFechaMovimiento.Text = DateTime.Now.ToShortDateString();
            this.ddlEstado.SelectedValue = ((int)EstadosAhorrosCuentasMovimientos.PendienteConfirmacion).ToString();
            this.ddlEstado.Enabled = false;

            if (this.paginaSegura.paginaActual.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroDepositosEspeciales)
            {
                this.txtFechaMovimiento.Enabled = true;
                //this.dvFechaMovimiento.Visible = true;
            }
            else if(this.paginaSegura.paginaActual.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroExtraccionesEspeciales)
            {
                this.txtFechaMovimiento.Enabled = true;
        
            }
            else
            {
                lblConceptoContable.Visible = false;
                ddlConceptoContable.Visible = false;
            }
        }

        private void DeshabilitarControles()
        {
            this.txtSaldoActual.Enabled = false;
            this.txtFechaAlta.Enabled = false;
        }

        private void CargarCombos()
        {
            this.MiAhoCuentasMovimientos.Cuenta.CuentaTipo.IdCuentaTipo = (int)EnumAhorrosCuentasTipos.CajaAhorro;
            this.MiAhoCuentasMovimientos.Cuenta.Estado.IdEstado = (int)EstadosAhorrosCuentas.CuentaAbierta;
            this.ddlCuenta.DataSource = AhorroF.CuentasObtenerListaFiltro(this.MiAhoCuentasMovimientos.Cuenta);
            this.ddlCuenta.DataValueField = "IdCuenta";
            this.ddlCuenta.DataTextField = "CuentaDatos";
            this.ddlCuenta.DataBind();
            //if (ddlCuenta.Items.Count>1)
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCuenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();
            //if(MiAhoCuentasMovimientos.TipoOperacion.TipoMovimiento.IdTipoMovimiento==(int)EnumTGETiposMovimientos.Credito)
            //    MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();
            //else
            //    MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();


            //EnumTGETiposFuncionalidades tipoFunc = (EnumTGETiposFuncionalidades)this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            //this.ddlTipoValor.DataSource = TGEGeneralesF.TiposValoresObtenerLista(tipoFunc);
            ////this.ddlTipoValor.DataSource = TGEGeneralesF.TiposValoresObtenerLista(EnumTGETiposFuncionalidades.AhorroDepositos);
            //this.ddlTipoValor.DataValueField = "IdTipoValor";
            //this.ddlTipoValor.DataTextField = "TipoValor";
            //this.ddlTipoValor.DataBind();
            //if (ddlTipoValor.Items.Count>1)
            //    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoValor, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            this.ddlTipoOperacion.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (ddlTipoOperacion.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (this.paginaSegura.paginaActual.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroDepositosEspeciales
                || this.paginaSegura.paginaActual.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroExtraccionesEspeciales)
            {
                operaciones.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);// this.paginaSegura.paginaActual.IdTipoFuncionalidad;//(int)EnumTGETiposFuncionalidades.Cobros;
                List<CtbConceptosContables> ctbConceptos = new List<CtbConceptosContables>();
                ctbConceptos = ContabilidadF.ConceptosContablesObtenerListaFiltro(operaciones);
                this.ddlConceptoContable.DataSource = ctbConceptos;
                this.ddlConceptoContable.DataValueField = "IdConceptoContable";
                this.ddlConceptoContable.DataTextField = "ConceptoContable";
                this.ddlConceptoContable.DataBind();
                if (ddlConceptoContable.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptoContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosAhorrosCuentasMovimientos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }

        private void MapearControlesAObjeto(AhoCuentasMovimientos pCuentasMovimientos)
        {
            pCuentasMovimientos.Cuenta.IdCuenta = Convert.ToInt32(this.ddlCuenta.SelectedValue);
            pCuentasMovimientos.FechaAlta = DateTime.Now;
            pCuentasMovimientos.FechaMovimiento = Convert.ToDateTime(this.txtFechaMovimiento.Text);
            //pCuentasMovimientos.TipoValor.IdTipoValor = Convert.ToInt32(this.ddlTipoValor.SelectedValue);
            pCuentasMovimientos.Importe = this.txtImporte.Decimal;
            pCuentasMovimientos.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pCuentasMovimientos.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            pCuentasMovimientos.TipoOperacion.TipoOperacion = this.ddlTipoOperacion.SelectedItem.Text;
            pCuentasMovimientos.IdConceptoContable = ddlConceptoContable.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(ddlConceptoContable.SelectedValue);
            pCuentasMovimientos.Concepto = ddlConceptoContable.SelectedValue == string.Empty ? string.Empty : this.ddlConceptoContable.SelectedItem.Text;
            pCuentasMovimientos.Campos = this.ctrCamposValores.ObtenerLista();
            //pCuentasMovimientos.Cotitulares = ddlCotitulares.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCotitulares.SelectedValue);

            TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(pCuentasMovimientos.Cuenta.Moneda.IdMoneda));
            if (moneda.IdMoneda > 0)
            {
                pCuentasMovimientos.Cuenta.Moneda = moneda;
                pCuentasMovimientos.MonedaCotizacion = moneda.MonedeaCotizacion.MonedaCotizacion;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
           
            try
            {
                this.btnAceptar.Visible = false;
                if (!this.Page.IsValid)
                    return;

                this.MapearControlesAObjeto(this.MiAhoCuentasMovimientos);
                this.MiAhoCuentasMovimientos.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                this.MiAhoCuentasMovimientos.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                this.MiAhoCuentasMovimientos.Cuenta = AhorroF.CuentasObtenerDatosCompletos(this.MiAhoCuentasMovimientos.Cuenta);
                this.MiAhoCuentasMovimientos.Comentarios = this.ctrComentarios.ObtenerLista();
                this.MiAhoCuentasMovimientos.Archivos = this.ctrArchivos.ObtenerLista();
                TGETiposFuncionalidades tipoFuncionalidad = new TGETiposFuncionalidades();
                tipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
                this.MiAhoCuentasMovimientos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);


                MiAhoCuentasMovimientos.LoteCuentasMovimientosCotitulares = new XmlDocument();

                XmlNode items = MiAhoCuentasMovimientos.LoteCuentasMovimientosCotitulares.CreateElement("Cotitulares");
                MiAhoCuentasMovimientos.LoteCuentasMovimientosCotitulares.AppendChild(items);

                XmlNode itemNodo;
                XmlNode ValorNode;

                foreach (AfiAfiliados item in MiAhoCuentasMovimientos.Cotitulares.Where(x => x.IdAfiliado > 0).ToList())
                {
                    itemNodo = MiAhoCuentasMovimientos.LoteCuentasMovimientosCotitulares.CreateElement("Cotitular");

                    ValorNode = MiAhoCuentasMovimientos.LoteCuentasMovimientosCotitulares.CreateElement("IdCuentaMovimiento");
                    ValorNode.InnerText = MiAhoCuentasMovimientos.IdCuentaMovimiento.ToString();
                    itemNodo.AppendChild(ValorNode);

                    ValorNode = MiAhoCuentasMovimientos.LoteCuentasMovimientosCotitulares.CreateElement("IdAfiliado");
                    ValorNode.InnerText = item.IdAfiliado.ToString();
                    itemNodo.AppendChild(ValorNode);

                    items.AppendChild(itemNodo);
                }


                if (AhorroF.MovimientosAgregar(this.MiAhoCuentasMovimientos, tipoFuncionalidad))
                {
                    this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAhoCuentasMovimientos.CodigoMensaje));
                    btnImprimir.Visible = true;
                }
                else
                {
                    this.btnAceptar.Visible = true;
                    this.MostrarMensaje(this.MiAhoCuentasMovimientos.CodigoMensaje, true, this.MiAhoCuentasMovimientos.CodigoMensajeArgs);
                }
            }
            catch
            {

            }
          
        }
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.CuentaMovimientosModificarDatosCancelar?.Invoke();
        }

        protected void btnAgregarCotitular_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlCotitulares.SelectedValue))
            {
                AfiAfiliados cotitular = new AfiAfiliados();
                cotitular = MisAfiliadosCombo.Find(x => x.IdAfiliado == Convert.ToInt32(ddlCotitulares.SelectedValue));
                cotitular.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                cotitular.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiAhoCuentasMovimientos.Cotitulares.Add(cotitular);
                CargarComboCotitulares();



                AyudaProgramacion.CargarGrillaListas(this.MiAhoCuentasMovimientos.Cotitulares, true, this.gvCotitulares, true);
            }
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.CuentaMovimientosModificarDatosAceptar?.Invoke(null, this.MiAhoCuentasMovimientos);
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.AhoCuentasMovimientos, "AhorroCuentasMovimientos", MiAhoCuentasMovimientos, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.Page, "AhorroCuentasMovimientos", this.UsuarioActivo);
        }

        protected void gvCotitulares_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Eliminar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            HiddenField IdAfiliadoBorrar = gvCotitulares.Rows[index].FindControl("hdfIdAfiliado") as HiddenField;
            var cotitular = MiAhoCuentasMovimientos.Cotitulares.FirstOrDefault(x => x.IdAfiliado == Convert.ToInt32(IdAfiliadoBorrar.Value));

            this.MiAhoCuentasMovimientos.Cotitulares.RemoveAt(indiceColeccion);
            this.MiAhoCuentasMovimientos.Cotitulares = AyudaProgramacion.AcomodarIndices<AfiAfiliados>(this.MiAhoCuentasMovimientos.Cotitulares);
            this.gvCotitulares.DataSource = this.MiAhoCuentasMovimientos.Cotitulares;
            this.gvCotitulares.DataBind();

            CargarComboCotitulares();

        }

        protected void gvCotitulares_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                if (((AfiAfiliados)e.Row.DataItem).EstadoColeccion == EstadoColecciones.Agregado)
                {
                    ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                    //string mensaje = this.ObtenerMensajeSistema("POComentarioConfirmarBaja");
                    //string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    //ibtn.Attributes.Add("OnClick", funcion);
                    ibtn.Visible = true;
                }
            }
        }

        public void PersistirDatosGrilla()
        {
            if (this.MiAhoCuentasMovimientos.Cotitulares.Count > 0)
            {
                foreach (GridViewRow fila in this.gvCotitulares.Rows)
                {


                    
                }
            }
        }


        protected void ddlCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlCuenta.SelectedValue))
            {
                this.MiAhoCuentasMovimientos.Cuenta.IdCuenta = Convert.ToInt32(this.ddlCuenta.SelectedValue);
                this.MiAhoCuentasMovimientos.Cuenta = AhorroF.CuentasObtenerDatosCompletos(this.MiAhoCuentasMovimientos.Cuenta);
                this.txtSaldoActual.Text = this.MiAhoCuentasMovimientos.Cuenta.SaldoActual.ToString();
                this.ctrCamposValores.BorrarControlesParametros();
                this.ctrCamposValores.IniciarControl(MiAhoCuentasMovimientos, MiAhoCuentasMovimientos.Cuenta.CuentaTipo, this.GestionControl, MiAhoCuentasMovimientos.Cuenta.IdCuenta);
                SetInitializeCulture(MiAhoCuentasMovimientos.Cuenta.Moneda.Moneda);
                if (this.paginaSegura.paginaActual.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroDepositos
                    || this.paginaSegura.paginaActual.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroExtracciones)
                {
                    AhoCuentas cuenta = new AhoCuentas();
                    cuenta.IdCuenta = MiAhoCuentasMovimientos.Cuenta.IdCuenta;
                    List<AhoCotitulares> cotitulares = new List<AhoCotitulares>();

                    cotitulares = AhorroF.CuentasObtenerDatosCompletos(cuenta).Cotitulares;
                    MisAfiliadosCombo = new List<AfiAfiliados>();
                    cotitulares.ForEach(x => MisAfiliadosCombo.Add( x.Afiliado));
                    CargarComboCotitulares();
      


                    tpCotitulares.Visible = true;
                    tcDatos.ActiveTabIndex = 0;
                }
                else
                {
                    tpCotitulares.Visible = false;
                }

            }
            else 
            {
                this.txtSaldoActual.Text = "0";
                tpCotitulares.Visible = false;
            }

        }

        private void CargarComboCotitulares()
        {


            ddlCotitulares.DataSource = MisAfiliadosCombo.Where(p => !(MiAhoCuentasMovimientos.Cotitulares.Any(p2 => p2.IdAfiliado == p.IdAfiliado))).ToList();
            ddlCotitulares.DataTextField = "ApellidoNombre";
            ddlCotitulares.DataValueField = "IdAfiliado";
            ddlCotitulares.DataBind();

            AyudaProgramacion.AgregarItemSeleccione(this.ddlCotitulares, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

    }
}