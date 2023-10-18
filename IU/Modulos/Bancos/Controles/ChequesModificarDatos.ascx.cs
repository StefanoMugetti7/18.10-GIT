using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Seguridad.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.Bancos.Controles
{
    public partial class ChequesModificarDatos : ControlesSeguros
    {
        private TESCheques MiCheque
        {
            get { return (TESCheques)Session[this.MiSessionPagina + "ChequesModificarDatosMiCheque"]; }
            set { Session[this.MiSessionPagina + "ChequesModificarDatosMiCheque"] = value; }
        }

        public delegate void ChequesModificarDatosAceptarEventHandler(TESCheques e);
        public event ChequesModificarDatosAceptarEventHandler ChequesModificarDatosAceptar;

        public delegate void ChequesModificarDatosCancelarEventHandler();
        public event ChequesModificarDatosCancelarEventHandler ChequesModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
        }

        public void IniciarControl(TESCheques pCheque, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiCheque = pCheque;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Modificar:
                    this.MiCheque = BancosF.ChequesObtenerDatosCompletos(this.MiCheque);
                    this.MapearObjetoAControles(this.MiCheque);
                    break;
                case Gestion.Consultar:
                    this.MiCheque = BancosF.ChequesObtenerDatosCompletos(this.MiCheque);
                    this.MapearObjetoAControles(this.MiCheque);
                    this.btnAceptar.Visible = false;
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnCancelar.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlBancos.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Bancos);
            this.ddlBancos.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles(TESCheques pParametro)
        {
            this.txtFecha.Text = pParametro.Fecha.ToShortDateString();
            this.txtFechaDiferido.Text = pParametro.FechaDiferido.HasValue ? pParametro.FechaDiferido.Value.ToShortDateString() : string.Empty;
            this.txtNumeroCheque.Text = pParametro.NumeroCheque;
            this.txtConcepto.Text = pParametro.Concepto;
            ListItem item = this.ddlBancos.Items.FindByValue(pParametro.Banco.IdBanco.ToString());
            if (item == null)
                this.ddlBancos.Items.Add(new ListItem(pParametro.Banco.Descripcion, pParametro.Banco.IdBanco.ToString()));
            this.ddlBancos.SelectedValue = pParametro.Banco.IdBanco.ToString();
            this.txtCUIT.Text = pParametro.CUIT;
            this.txtTitular.Text = pParametro.TitularCheque;
            this.txtImporte.Text = pParametro.Importe.ToString("N2");

            AyudaProgramacion.CargarGrillaListas<TESChequesMovimientos>(this.MiCheque.ChequesMovimientos, false, this.gvDatos, true);

            this.ddlEstados.Items.Add(new ListItem(pParametro.Estado.Descripcion, pParametro.Estado.IdEstado.ToString()));
            if (pParametro.Estado.IdEstado != (int)Estados.Baja)
            {
                TGEEstados estado = new TGEEstados();
                estado.IdEstado = (int)Estados.Baja;
                estado = TGEGeneralesF.TGEEstadosObtener(estado);
                this.ddlEstados.Items.Add(new ListItem(estado.Descripcion, estado.IdEstado.ToString()));
                if (!this.ddlEstados.Items.Contains(new ListItem(pParametro.Estado.Descripcion, pParametro.Estado.IdEstado.ToString())))
                {
                    this.ddlEstados.Items.Add(new ListItem(pParametro.Estado.Descripcion, pParametro.Estado.IdEstado.ToString()));
                }
            }
        }

        private void MapearControlesAObjeto(TESCheques pParametro)
        {
            pParametro.Fecha = Convert.ToDateTime(this.txtFecha.Text);
            pParametro.FechaDiferido = Convert.ToDateTime(this.txtFechaDiferido.Text);
            pParametro.NumeroCheque = this.txtNumeroCheque.Text;
            pParametro.Concepto = this.txtConcepto.Text;
            pParametro.Banco.IdBanco = Convert.ToInt32(this.ddlBancos.SelectedValue);
            pParametro.Banco.Descripcion = this.ddlBancos.SelectedItem.Text;
            pParametro.CUIT = this.txtCUIT.Text;
            pParametro.TitularCheque = this.txtTitular.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiCheque);
            this.MiCheque.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Modificar:
                    guardo = BancosF.ChequesModificar(this.MiCheque);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCheque.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiCheque.CodigoMensaje, true, this.MiCheque.CodigoMensajeArgs);
            }
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ChequesModificarDatosAceptar != null)
                this.ChequesModificarDatosAceptar(this.MiCheque);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ChequesModificarDatosCancelar != null)
                this.ChequesModificarDatosCancelar();
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                TESChequesMovimientos item = (TESChequesMovimientos)e.Row.DataItem;

                if (item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias)
                {
                    imprimir.Visible = true;
                }
                if (item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.RetiroCheques || item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CambioCheques )
                {
                    consultar.Visible = false;
                }
            }
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Consultar" || e.CommandName == "Modificar" || e.CommandName == "Impresion"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName != Gestion.Impresion.ToString())
            {
                //Filtro para Obtener URL y NombreParametro
                Menues filtroMenu = new Menues();
                filtroMenu.IdTipoOperacion = IdTipoOperacion;
                //Control de Tipo de Menues (SOLO CONSULTA)
                if (e.CommandName == Gestion.Consultar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
                this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
                //this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
                //Si devuelve una URL Redirecciona si no muestra mensaje error
                if (filtroMenu.URL.Length != 0)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
                }
                else
                {
                    this.MostrarMensaje("ErrorURLNoValida", true);
                }
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                try
                {
                    RepReportes reporte = new RepReportes();
                    string parametro = this.MiCheque.IdCheque.ToString();

                    RepParametros param = new RepParametros();
                    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                    param.Parametro = "IdCheque";
                    param.ValorParametro = parametro;


                    TGEPlantillas plantilla = new TGEPlantillas();
                    plantilla.Codigo = "TESChequesADepositar";
                    plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                    reporte.StoredProcedure = plantilla.NombreSP;
                    reporte.Parametros.Add(param);

                    DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdCheque", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    //byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdAfiliado", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "ComprobanteCheque_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);
                    UpdatePanel1.Update();
                }
                catch (Exception ex)
                {
                    this.MostrarMensaje("No se pudo imprimir el comprobante.", true);
                }
            }

        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiCheque.ChequesMovimientos;
            this.gvDatos.DataBind();
        }

    }
}