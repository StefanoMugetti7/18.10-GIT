using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Tesorerias;
using Tesorerias.Entidades;

namespace IU.Modulos.Bancos.Controles
{
    public partial class ChequesModificarListar : ControlesSeguros
    {
        private List<TESCheques> MisCheques
        {
            get { return (List<TESCheques>)Session[this.MiSessionPagina + "ChequesModificarListarMisCheques"]; }
            set { Session[this.MiSessionPagina + "ChequesModificarListarMisCheques"] = value; }
        }
        private List<TESCheques> MisChequesSeleccionados
        {
            get { return (List<TESCheques>)Session[this.MiSessionPagina + "ChequesModificarListarMisChequesSeleccionados"]; }
            set { Session[this.MiSessionPagina + "ChequesModificarListarMisChequesSeleccionados"] = value; }
        }
        private TESTesorerias MiTesoreria
        {
            get { return (TESTesorerias)Session[this.MiSessionPagina + "ChequesModificarListarMiTesoreria"]; }
            set { Session[this.MiSessionPagina + "ChequesModificarListarMiTesoreria"] = value; }
        }

        //public delegate void ChequesModificarListarAceptarEventHandler(object sender, TESBancosCuentas e);
        //public event ChequesModificarListarAceptarEventHandler ChequesModificarListarAceptar;

        public delegate void ChequesModificarListarCancelarEventHandler();
        public event ChequesModificarListarCancelarEventHandler ChequesModificarListarCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //this.txtFechaDesde = DateTime.Now.Add(-30).ToShortDateString();
                //this.txtFechaHasta = DateTime.Now.ToShortDateString();
            }
        }

        public void IniciarControl()
        {
            this.CargarCombos();
            this.CargarLista(new TESCheques());

            if (Convert.ToInt32(this.ddlTiposOperaciones.SelectedValue) == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias)
            {
                this.pnlDatosDeposito.Visible = true;
                this.rfvBancosCuentas.Enabled = true;
                this.ddlBancosCuentas.DataSource = BancosF.BancosCuentasObtenerDepositar(this.UsuarioActivo.FilialPredeterminada);
                this.ddlBancosCuentas.DataValueField = "IdBancoCuenta";
                this.ddlBancosCuentas.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
                this.ddlBancosCuentas.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            if (Convert.ToInt32(ddlTiposOperaciones.SelectedValue) == (int)EnumTGETiposOperaciones.TraspasoChequesFilial)
            {
                lblBancosCuentas.Visible = false;
                ddlBancosCuentas.Visible = false;
                lblBoletaDeposito.Visible = false;
                txtBoletaDeposito.Visible = false;
                pnlDatosDeposito.Visible = true;
                ddlFilialesTraspaso.Visible = true;
                lblFilialesTraspaso.Visible = true;
                rfvFilialesTraspaso.Enabled = true;
                List<TGEFiliales> filiales = TGEGeneralesF.FilialesObenerLista();
                filiales = AyudaProgramacion.AcomodarIndices<TGEFiliales>(filiales.Where(x => x.IdFilial != UsuarioActivo.FilialPredeterminada.IdFilial).ToList());
                ddlFilialesTraspaso.DataSource = filiales;
                ddlFilialesTraspaso.DataValueField = "IdFilial";
                ddlFilialesTraspaso.DataTextField = "Filial";
                ddlFilialesTraspaso.DataBind();
                if (ddlFilialesTraspaso.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(ddlFilialesTraspaso, ObtenerMensajeSistema("SeleccioneOpcion"));
                //ddlFilialesTraspaso.DataSource = TGEGeneralesF.ChequesObtenerFilialesTraspaso(UsuarioActivo.FilialPredeterminada);
                //ddlFilialesTraspaso.DataValueField = "IdFilial";
                //ddlFilialesTraspaso.DataTextField = "Descripcion";
                //ddlFilialesTraspaso.DataBind();
                //AyudaProgramacion.AgregarItemSeleccione(ddlFilialesTraspaso, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }

        public void IniciarControl(TESTesorerias pTesoreria)
        {
            this.MiTesoreria = pTesoreria;
            this.IniciarControl();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TESCheques parametros = this.BusquedaParametrosObtenerValor<TESCheques>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Consultar" || e.CommandName == "Modificar"))// || e.CommandName == "Impresion"
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCheques cheque = this.MisCheques[indiceColeccion];

            //if (e.CommandName == Gestion.Impresion.ToString())
            //{
            //    if (cheque.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosVarios
            //        || cheque.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
            //        || cheque.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados)
            //    {
            //        CobOrdenesCobros pReporte = new CobOrdenesCobros();
            //        pReporte.IdOrdenCobro = cheque.IdRefTipoOperacion;
            //        this.ctrPopUpComprobantes.CargarReporte(pReporte, EnumTGEComprobantes.CobOrdenesCobros);
            //        this.UpdatePanel1.Update();
            //    }
            //    else if (cheque.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagos)
            //    {
            //        CapOrdenesPagos pReporte = new CapOrdenesPagos();
            //        pReporte.IdOrdenPago = cheque.IdRefTipoOperacion;
            //        this.ctrPopUpComprobantes.CargarReporte(pReporte, EnumTGEComprobantes.CapOrdenesPagos);
            //        this.UpdatePanel1.Update();
            //    }
            //    else if (cheque.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.AhorroDepositos
            //            || cheque.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias)
            //    {
            //        AhoCuentasMovimientos pReporte = new AhoCuentasMovimientos();
            //        pReporte.IdCuentaMovimiento = cheque.IdRefTipoOperacion;
            //        this.ctrPopUpComprobantes.CargarReporte(pReporte, EnumTGEComprobantes.AhoCuentasMovimientos);
            //        this.UpdatePanel1.Update();
            //    }
            //}
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
                // ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                // imprimir.Visible = true;

                switch (this.GestionControl)
                {
                    case Gestion.Modificar:
                        CheckBox incluir = (CheckBox)e.Row.FindControl("chkIncluir");
                        incluir.Visible = true;
                        //imprimir.Visible = true;
                        break;
                    default:
                        break;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label grillaTotal = (Label)e.Row.FindControl("lblGrillaTotalRegistros");
                grillaTotal.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCheques.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESCheques parametros = this.BusquedaParametrosObtenerValor<TESCheques>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESCheques>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCheques;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCheques = this.OrdenarGrillaDatos<TESCheques>(this.MisCheques, e);
            this.gvDatos.DataSource = this.MisCheques;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            TGETiposOperaciones tipoOperacion = new TGETiposOperaciones();
            tipoOperacion.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            //tipoOperacion.Estado.IdEstado = (int)EstadosTodos.Todos;
            this.ddlTiposOperaciones.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(tipoOperacion);
            this.ddlTiposOperaciones.DataValueField = "IdTipoOperacion";
            this.ddlTiposOperaciones.DataTextField = "TipoOperacion";
            this.ddlTiposOperaciones.DataBind();

            this.ddlBancos.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Bancos);
            this.ddlBancos.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        private void CargarLista(TESCheques pParametro)
        {
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.FechaDiferidoDesde = this.txtFechaDiferidoDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDiferidoDesde.Text);
            pParametro.FechaDiferidoHasta = this.txtFechaDiferidoHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDiferidoHasta.Text);
            pParametro.NumeroCheque = this.txtNumeroCheque.Text;
            pParametro.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
            pParametro.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.TitularCheque = this.txtTitularCheque.Text;
            pParametro.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTiposOperaciones.SelectedValue);
            switch (Convert.ToInt32(this.ddlTiposOperaciones.SelectedValue))
            {
                case (int)EnumTGETiposOperaciones.RecibirChequesCajas:
                    pParametro.Estado.IdEstado = (int)EstadosCheques.EnCaja;
                    break;
                //case (int)EnumTGETiposOperaciones.TransferirChequesBancos:
                //    pParametro.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;
                //    break;
                //case (int)EnumTGETiposOperaciones.RecibirChequesTesoreria:
                //    pParametro.Estado.IdEstado = (int)EstadosCheques.EnviadoSectorBancos;
                //    break;
                case (int)EnumTGETiposOperaciones.DepositosCuentasBancarias:
                    //pParametro.Estado.IdEstado = (int)EstadosCheques.EnSectorBancos;
                    pParametro.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;
                    //if (!pParametro.FechaDiferidoHasta.HasValue || pParametro.FechaDiferidoHasta.Value.Date > DateTime.Now.Date)
                    //{
                    //    pParametro.FechaHasta = DateTime.Now.Date;
                    //    this.txtFechaHasta.Text = DateTime.Now.ToShortDateString();
                    //    pParametro.FechaDiferidoHasta = DateTime.Now.Date;
                    //    this.txtFechaDiferidoHasta.Text = DateTime.Now.ToShortDateString();
                    //}
                    break;
                case (int)EnumTGETiposOperaciones.TraspasoChequesFilial:
                    pParametro.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;
                    break;
                default:
                    break;
            }
            //pParametro.BusquedaParametros = true;
            //this.BusquedaParametrosGuardarValor<TESCheques>(pParametro);
            this.MisCheques = BancosF.ChequesObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisCheques;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }

        private void MapearControlesAObjeto()
        {
            TESCheques cheque;
            CheckBox incluir;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    cheque = this.MisCheques[fila.DataItemIndex];
                    //cuentaCte.Incluir = true;
                    incluir = (CheckBox)fila.FindControl("chkIncluir");
                    if (incluir.Checked)
                    {
                        cheque.EstadoColeccion = EstadoColecciones.Modificado;
                        cheque.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    }
                    else
                    {
                        cheque.EstadoColeccion = EstadoColecciones.SinCambio;
                    }
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto();

            Objeto resultado = new Objeto();
            resultado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            TESBancosCuentasMovimientos bancoCuentaMovimiento = new TESBancosCuentasMovimientos();
            if (!string.IsNullOrEmpty(this.ddlBancosCuentas.SelectedValue))
                bancoCuentaMovimiento.BancoCuenta.IdBancoCuenta = Convert.ToInt32(this.ddlBancosCuentas.SelectedValue);
            bancoCuentaMovimiento.NumeroTipoOperacion = this.txtBoletaDeposito.Text;

            TESChequesMovimientos chequeCuentaMovimiento = new TESChequesMovimientos();
            if (!string.IsNullOrEmpty(ddlFilialesTraspaso.SelectedValue))
            {
                chequeCuentaMovimiento.FilialDestino.IdFilialDestino = Convert.ToInt32(ddlFilialesTraspaso.SelectedValue);
                chequeCuentaMovimiento.FilialDestino.Descripcion = ddlFilialesTraspaso.SelectedItem.ToString();
                chequeCuentaMovimiento.Descripcion = txtDetalle.Text;
                chequeCuentaMovimiento.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;
            }

            MisChequesSeleccionados = this.MisCheques.Where(x => x.EstadoColeccion == EstadoColecciones.Modificado).ToList();
            EnumTGETiposOperaciones operacion = (EnumTGETiposOperaciones)Enum.Parse(typeof(EnumTGETiposOperaciones), this.ddlTiposOperaciones.SelectedValue);
            if (operacion == EnumTGETiposOperaciones.TransferirChequesBancos
                || operacion == EnumTGETiposOperaciones.RecibirChequesCajas)
            {
                guardo = TesoreriasF.TesoreriasTransferirCheques(MisChequesSeleccionados, this.MiTesoreria, operacion);
                resultado.CodigoMensaje = this.MiTesoreria.CodigoMensaje;
                resultado.CodigoMensajeArgs.AddRange(this.MiTesoreria.CodigoMensajeArgs);
            }
            else
                guardo = BancosF.ChequesTransferir(MisChequesSeleccionados, resultado, operacion, bancoCuentaMovimiento, chequeCuentaMovimiento);

            if (guardo)
            {
                this.btnImprimir.Visible = true;
                //this.btnImprimir_Click(null, new EventArgs());
                this.CargarLista(new TESCheques());
                this.MostrarMensaje(this.ObtenerMensajeSistema(resultado.CodigoMensaje), false);
            }
            else
            {
                this.MostrarMensaje(resultado.CodigoMensaje, true, resultado.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ChequesModificarListarCancelar != null)
                this.ChequesModificarListarCancelar();
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                RepReportes reporte = new RepReportes();
                string parametro = "";

                foreach (TESCheques uni in MisChequesSeleccionados)
                {
                    if (uni.IdCheque > 0)
                    {
                        parametro += uni.IdCheque.ToString() + ',';
                    }
                }


                RepParametros param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                param.Parametro = "IdCheque";
                param.ValorParametro = parametro;
                reporte.Parametros.Add(param);


                TGEPlantillas plantilla = new TGEPlantillas();
                plantilla.Codigo = "TESChequesADepositar";
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                reporte.StoredProcedure = plantilla.NombreSP;

                DataSet ds = ReportesF.ReportesObtenerDatos(reporte);
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdCheque", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.Page, "ChequesADepositar", this.UsuarioActivo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("No se pudo imprimir el comprobante.", true);
            }
        }
    }
}
