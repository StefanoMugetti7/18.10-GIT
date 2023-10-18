using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using Contabilidad.Entidades;
using Contabilidad;
using Generales.Entidades;

namespace IU.Modulos.Compras.Controles
{
    public partial class ModificarComprobantesDatos : ControlesSeguros
    {
        private List<CapSolicitudPago> MisComprobantes
        {
            get { return (List<CapSolicitudPago>)Session[this.MiSessionPagina + "ComprobantesDatosMisComprobantes"]; }
            set { Session[this.MiSessionPagina + "ComprobantesDatosMisComprobantes"] = value; }
        }

        //public delegate void ModificarDatosAceptarEventHandler(object sender, CapSolicitudPago e);
        //public event ModificarDatosAceptarEventHandler ModificarDatosAceptar;

        public delegate void ModificarDatosCancelarEventHandler();
        public event ModificarDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
            }
        }

        public void IniciarControl(List<CapSolicitudPago> pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MisComprobantes = pParametro;
            this.btnAceptar.Visible = false;
            //obtengo el ultimo período cerrado
            int ultimoPICerrado = ContabilidadF.PeriodosIvasObtenerUltimoCerrado().Periodo;
            int ultimoPCCerrado = ContabilidadF.PeriodosContablesObtenerUltimoCerrado().Periodo;
            //seteo el periodo Contable (incrementado)
            this.txtPeriodoContable.Text = this.IncrementarPeriodo(ultimoPCCerrado).ToString();
            //busco primer dia luego del último periodo IVA cerrado
            DateTime fechaDesde = this.PeriodoADateTime(this.IncrementarPeriodo(ultimoPICerrado));
            this.txtFechaDesde.Text = fechaDesde.ToShortDateString();
            #region switch Gestion
            //switch (this.GestionControl)
            //{
            //    case Gestion.Agregar:

            //        break;
            //    case Gestion.Modificar:

            //        //this.MisComprobantes = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pParametro);
            //        //this.MapearObjetoAControles(this.MiComprobante);
            //        break;
            //    case Gestion.Consultar:

            //        //this.MiComprobante = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pParametro);
            //        //this.MapearObjetoAControles(this.MiComprobante);

            //        break;
            //    default:
            //        break;
            //}
            #endregion
        }

        private int IncrementarPeriodo(int pPeriodo)
        {
            int mes = Convert.ToInt32(pPeriodo.ToString().Remove(0, 4));
            int anio = Convert.ToInt32(pPeriodo.ToString().Remove(4, 2));

            if (mes < 12)
                mes++;
            else
            {
                mes = 1; //Enero por el cambio de año
                anio++;
            }
            return Convert.ToInt32(String.Concat(anio.ToString(), mes.ToString().PadLeft(2, '0')));
        }

        //Formato de entrada AAAAMM , y setea el dia 1
        private DateTime PeriodoADateTime(int periodo)
        {
            int mes = Convert.ToInt32(periodo.ToString().Remove(0, 4));
            int anio = Convert.ToInt32(periodo.ToString().Remove(4, 2));
            DateTime result = new DateTime(anio, mes, 1);

            return result;
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Consultar" || e.CommandName == "Modificar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CapSolicitudPago cheque = this.MisComprobantes[indiceColeccion];

            //this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("IdCheque", cheque.IdCheque);

            //if (e.CommandName == Gestion.Modificar.ToString())
            //{
            //    this.Response.Redirect("~/Modulos/Bancos/ChequesModificar.aspx", true);
            //}
            //else if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    this.Response.Redirect("~/Modulos/Bancos/ChequesConsultar.aspx", true);
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TESCheques bancoCuenta = (TESCheques)e.Row.DataItem;

                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        CheckBox incluir = (CheckBox)e.Row.FindControl("chkIncluir");
                        //TextBox fechaContable = (TextBox)e.Row.FindControl("txtFechaContable");
                        incluir.Visible = true;

                        break;
                    default:
                        break;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label grillaTotal = (Label)e.Row.FindControl("lblGrillaTotalRegistros");
                grillaTotal.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisComprobantes.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //TESCheques parametros = this.BusquedaParametrosObtenerValor<TESCheques>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<TESCheques>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisComprobantes;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisComprobantes = this.OrdenarGrillaDatos<CapSolicitudPago>(this.MisComprobantes, e);
            this.gvDatos.DataSource = this.MisComprobantes;
            this.gvDatos.DataBind();
        }
        #endregion

        #region MAPEO DE DATOS
        private void MapearControlesAObjeto(CapSolicitudPago parametro)
        {

        }

        private void MapearObjetoAControles(CapSolicitudPago parametro)
        {
            throw new NotImplementedException();
        }

        private void MapearComprobanteAFactura(List<CapSolicitudPago> parametro)
        {

            CapSolicitudPago solPago;
            CheckBox incluir;
            TextBox fechaContable;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    solPago = this.MisComprobantes[fila.DataItemIndex];
                    //cuentaCte.Incluir = true;
                    incluir = (CheckBox)fila.FindControl("chkIncluir");
                    fechaContable = (TextBox)fila.FindControl("txtFechaContable");
                    if (incluir.Checked)
                    {
                        solPago.EstadoColeccion = EstadoColecciones.Modificado;
                        solPago.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                        if (solPago.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.CbteInterno)
                        {
                            solPago.TiposFacturas.IdTipoFactura = (int)EnumTiposFacturas.FacturasA;
                            
                        }
                        //else if (solPago.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.cbteInternoC)
                        //{
                        //    solPago.TiposFacturas.IdTipoFactura = (int)EnumTiposFacturas.FacturasC;
                        //}

                        solPago.FechaContable = Convert.ToDateTime(fechaContable.Text);
                    }
                    else
                    {
                        solPago.EstadoColeccion = EstadoColecciones.SinCambio;
                    }
                    
                }
            }

        }
        #endregion

        #region BUTTONS
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CapSolicitudPago filtro = new CapSolicitudPago();
            filtro.FechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MisComprobantes = CuentasPagarF.ComprobantesInternosSeleccionarPorFechaDesde(filtro);
            foreach (CapSolicitudPago comprobante in this.MisComprobantes)
            {
                if (comprobante.FechaContable < this.PeriodoADateTime(Convert.ToInt32(this.txtPeriodoContable.Text)))
                    comprobante.FechaContable = this.PeriodoADateTime(Convert.ToInt32(this.txtPeriodoContable.Text));
                else
                    comprobante.FechaContable = comprobante.FechaFactura;
            }
            if (this.MisComprobantes.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MisComprobantes, false, this.gvDatos, true);
                this.btnAceptar.Visible = true;
            }
            else
            {
                this.btnAceptar.Visible = false;
            }
                this.upGlobal.Update();
                this.UpdatePanel1.Update();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearComprobanteAFactura(this.MisComprobantes);

            //validacion de comprobantes
            if (!this.MisComprobantes.Exists(x => x.EstadoColeccion == EstadoColecciones.Modificado))
            {
                Objeto mensaje = new Objeto();
                mensaje.CodigoMensaje = "SeleccioneComprobante";
                this.MostrarMensaje(mensaje.CodigoMensaje, true);
                return;
            }

            Objeto resultado = new Objeto();
            resultado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            
            //AGREGO (LISTA, resultado) << parametros de entrada, salida >> bool (Guardo mensaje en resultado)
            guardo = CuentasPagarF.ComprobantesInternosModificar(this.MisComprobantes, resultado);

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(resultado.CodigoMensaje));
                this.btnBuscar_Click(null, EventArgs.Empty);
            }
            else
            {
                this.MostrarMensaje(resultado.CodigoMensaje, true, resultado.CodigoMensajeArgs);
            }
            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar();
        }

        #endregion
    }
}