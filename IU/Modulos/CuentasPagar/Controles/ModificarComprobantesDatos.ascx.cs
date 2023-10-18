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

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class ModificarComprobantesDatos : ControlesSeguros
    {
        private List<CapSolicitudPago> MisComprobantes
        {
            get { return (List<CapSolicitudPago>)Session[this.MiSessionPagina + "ComprobantesDatosMisComprobantes"]; }
            set { Session[this.MiSessionPagina + "ComprobantesDatosMisComprobantes"] = value; }
        }

        private int UltimoPICerrado
        {
            get { return (int)Session[this.MiSessionPagina + "ComprobantesDatosUltimoPICerrado"]; }
            set { Session[this.MiSessionPagina + "ComprobantesDatosUltimoPICerrado"] = value; }
        }

        private int UltimoPCCerrado
        {
            get { return (int)Session[this.MiSessionPagina + "ComprobantesDatosUltimoPCCerrado"]; }
            set { Session[this.MiSessionPagina + "ComprobantesDatosUltimoPCCerrado"] = value; }
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
            this.UltimoPICerrado = ContabilidadF.PeriodosIvasObtenerUltimoCerrado().Periodo;
            this.UltimoPCCerrado = ContabilidadF.PeriodosContablesObtenerUltimoCerrado().Periodo;

            if (this.UltimoPCCerrado == 0 || this.UltimoPICerrado == 0)
            {
                CtbEjerciciosContables EjercicioContable = new CtbEjerciciosContables();
                EjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo();
                EjercicioContable = ContabilidadF.EjerciciosContablesObtenerDatosCompletos(EjercicioContable);
                DateTime fecha = EjercicioContable.FechaInicio.AddMonths(-1);
                string primerPeriodo = fecha.Year.ToString();
                primerPeriodo = string.Concat(primerPeriodo, fecha.Month.ToString().PadLeft(2, '0'));
                if (this.UltimoPICerrado == 0) this.UltimoPICerrado = Convert.ToInt32(primerPeriodo);
                if (this.UltimoPCCerrado == 0) this.UltimoPCCerrado = Convert.ToInt32(primerPeriodo);
            }

            this.CargarCombo();
            //seteo el periodo Contable (incrementado)
            //this.txtPeriodoContable.Text = this.IncrementarPeriodo(this.UltimoPCCerrado).ToString();
            //busco primer dia luego del último periodo IVA cerrado
            //DateTime fechaDesde = this.PeriodoADateTime(this.IncrementarPeriodo(UltimoPICerrado));
            //this.txtFechaDesde.Text = fechaDesde.ToShortDateString();
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

        private void CargarCombo()
        {
            int i = 0;
            int periodosMostrarr = this.UltimoPICerrado;
            List<int> listaP = new List<int>();
            
            while (i < 4)
            {
                periodosMostrarr = this.IncrementarPeriodo(periodosMostrarr); //lo incremento desde el principio porque el periodo inicial es el Cerrado, por lo que no sirve
                listaP.Add(periodosMostrarr);
                i++;
            }
            foreach (int per in listaP)
                this.ddlPeriodoContable.Items.Add(new ListItem(per.ToString(), per.ToString()));

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
            pPeriodo = Convert.ToInt32(String.Concat(anio.ToString(), mes.ToString().PadLeft(2, '0')));
            return pPeriodo;
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
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesModificar.aspx"), true);
            //}
            //else if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesConsultar.aspx"), true);
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
            CtbPeriodosIvas periodoFiltro = new CtbPeriodosIvas();
            periodoFiltro.Periodo = Convert.ToInt32(this.ddlPeriodoContable.SelectedValue); //ACA VA EL DLL.SelectedValue
            this.MisComprobantes = CuentasPagarF.ComprobantesInternosSeleccionarPorFechaDesde(periodoFiltro);
            //foreach (CapSolicitudPago comprobante in this.MisComprobantes)
            //{
            //    if (comprobante.FechaContable < this.PeriodoADateTime(Convert.ToInt32(this.txtPeriodoContable.Text)))
            //        comprobante.FechaContable = this.PeriodoADateTime(Convert.ToInt32(this.txtPeriodoContable.Text));
            //    else
            //        comprobante.FechaContable = comprobante.FechaFactura;
            //}
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MisComprobantes, false, this.gvDatos, true);
            if (this.MisComprobantes.Count > 0)
            {
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
            Objeto resultado = new Objeto();
            //validacion de comprobantes
            if (!this.MisComprobantes.Exists(x => x.EstadoColeccion == EstadoColecciones.Modificado))
            {
                
                resultado.CodigoMensaje = "SeleccioneComprobante";
                this.MostrarMensaje(resultado.CodigoMensaje, true);
                return;
            }
            //else
            //{
            //    if (this.MisComprobantes.Exists(x => x.EstadoColeccion == EstadoColecciones.Modificado && x.FechaContable < DateTime.Now))
            //    {
            //        resultado.CodigoMensaje = "SeleccioneComprobante";
            //        this.MostrarMensaje(resultado.CodigoMensaje, true);
            //        return;
            //    }
            //}

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