using Afiliados;
using Afiliados.Entidades;
using Afiliados.Entidades.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfiliadoDatosUIF : ControlesSeguros
    {
        private AfiAfiliados MiAfiliado
        {
            get { return (AfiAfiliados)Session[this.MiSessionPagina + "AfiliadoModificarDatosUIFMiAfiliado"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosUIFMiAfiliado"] = value; }
        }
        private AfiAfiliadosDatosUIF MiAfiliadoDatosUIF
        {
            get { return (AfiAfiliadosDatosUIF)Session[this.MiSessionPagina + "AfiliadoModificarDatosUIFMiAfiliadoDatosUIF"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosUIFMiAfiliadoDatosUIF"] = value; }
        }
        private List<TGEListasValoresDetalles> ListaSiNo
        {
            get { return (List<TGEListasValoresDetalles>)Session[this.MiSessionPagina + "AfiliadoModificarDatosUIFMiListaValor"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosUIFMiListaValor"] = value; }
        }
        public delegate void AfiliadoDatosUIFAceptarEventHandler(object sender, AfiAfiliados e);
        public event AfiliadoDatosUIFAceptarEventHandler AfiliadosModificarDatosUIFAceptar;
        public delegate void AfiliadoDatosUIFCancelarEventHandler();
        public event AfiliadoDatosUIFCancelarEventHandler AfiliadosModificarDatosUIFCancelar;
        public DataTable MisMatrices
        {
            get { return this.PropiedadObtenerValor<DataTable>("AfiliadoModificarDatosUIFMiMatriz"); }
            set { this.PropiedadGuardarValor("AfiliadoModificarDatosUIFMiMatriz", value); }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    this.PersistirDatosGrilla();
                }
            }
        }
        public void IniciarControl(AfiAfiliadosDatosUIF pAfiliado, AfiAfiliados pAfi, Gestion pGestion)
        {
            this.MiAfiliadoDatosUIF = pAfiliado;
            this.MiAfiliado = pAfi;
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MisMatrices = AfiliadosF.AfiAfiliadosDatosUIFObtenerMatricesRiesgo(pAfiliado);
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.IniciarGrilla();
                    this.ctrArchivos.IniciarControl(this.MiAfiliadoDatosUIF, this.GestionControl);
                    this.ctrComentarios.IniciarControl(this.MiAfiliadoDatosUIF, this.GestionControl);
                    break;
                case Gestion.Consultar:
                    this.MiAfiliadoDatosUIF = AfiliadosF.AfiliadosObtenerDatosUIFPorIdAfiliado(this.MiAfiliadoDatosUIF);
                    this.MapearObjetoAControles(this.MiAfiliadoDatosUIF);
                    this.ddlEsCliente.Enabled = false;
                    this.ddlPersonaRelacionadaActTerrorista.Enabled = false;
                    this.ddlPersonaRelacionadaTripleFrontera.Enabled = false;
                    this.ddlEsPEP.Enabled = false;
                    this.txtLimitePerfilesAnual.Enabled = false;
                    this.txtLimitePerfilesFechaVenc.Enabled = false;
                    this.txtCargoPersonaJuridicaVinc.Enabled = false;
                    this.ddlAfiliado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.btnImprimir.Visible = true;
                    break;
                case Gestion.Modificar:
                    this.MiAfiliadoDatosUIF = AfiliadosF.AfiliadosObtenerDatosUIFPorIdAfiliado(this.MiAfiliadoDatosUIF);
                    this.MapearObjetoAControles(this.MiAfiliadoDatosUIF);
                    this.btnImprimir.Visible = true;
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles(AfiAfiliadosDatosUIF pParametro)
        {
            this.txtLimitePerfilesFechaVenc.Text = pParametro.LimitePerfilesFechaVencimiento.ToShortDateString();
            this.txtCargoPersonaJuridicaVinc.Text = pParametro.CargoPersonaJuridicaVinculada;
            this.txtLimitePerfilesAnual.Text = pParametro.LimitePerfilesAnual.ToString("N2");
            this.ddlEsPEP.SelectedValue = pParametro.EsPEP.ToString();
            //this.ddlEsCliente.SelectedValue = pParametro.EsCliente.ToString();
            this.ddlPersonaRelacionadaActTerrorista.SelectedValue = pParametro.IdPersonaFisicaRelacionadaActTerrorista.ToString();
            this.ddlPersonaRelacionadaTripleFrontera.SelectedValue = pParametro.IdPersonaFisicaRelacionadaTripleFrontera.ToString();
            this.ddlEsPEP_OnSelectedIndexChanged(null, new EventArgs());
            this.txtCargo.Text = pParametro.Cargo;
            this.txtPais.Text = pParametro.Pais;
            this.ddlEnActividad.SelectedValue = pParametro.DesempenioActual.ToString();
            this.txtDependencia.Text = pParametro.Dependencia;
            this.txtFechaDesde.Text = pParametro.FechaInicio.HasValue ? Convert.ToDateTime(pParametro.FechaInicio).ToShortDateString() : string.Empty;
            this.txtFechaHasta.Text = pParametro.FechaFin.HasValue ? Convert.ToDateTime(pParametro.FechaFin).ToShortDateString() : string.Empty;
            this.hdfAfiliadoJuridVinc.Value = pParametro.PersonaJuridicaVinculada;
            this.hdfIdAfiliadoJuridVinc.Value = pParametro.IdPersonaJuridicaVinculada.ToString();
            this.button_Click(null, new EventArgs());
            if (pParametro.MatricesDeRiesgo.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<AfiAfiliadosMatrizRiesgo>(pParametro.MatricesDeRiesgo, false, this.gvMatrizRiesgo, true);
                this.upMatrizRiesgo.Update();
            }
            else
            {
                IniciarGrilla();
            }
            if (pParametro.Esposa.IdAfiliado > 0)
            {
                List<AfiAfiliados> aux = new List<AfiAfiliados>
                {
                    pParametro.Esposa
                };
                this.DivEsposa.Visible = true;
                AyudaProgramacion.CargarGrillaListas<AfiAfiliados>(aux, false, this.gvDatos, true);
                this.upEsposa.Update();
            }
            this.ctrArchivos.IniciarControl(pParametro, this.GestionControl);
            this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pParametro);
        }
        private void MapearControlesAObjeto(AfiAfiliadosDatosUIF pParametro)
        {
            pParametro.IdAfiliado = this.MiAfiliado.IdAfiliado;
            pParametro.Cargo = this.txtCargo.Visible == true ? this.txtCargo.Text : null;
            pParametro.Pais = this.txtPais.Visible == true ? this.txtPais.Text : null;
            pParametro.Dependencia = this.txtDependencia.Visible == true ? this.txtDependencia.Text : null;
            pParametro.DesempenioActual = this.ddlEnActividad.Visible == true ? Convert.ToInt32(this.ddlEnActividad.SelectedValue) : (Nullable<int>)null;
            pParametro.LimitePerfilesFechaVencimiento = Convert.ToDateTime(this.txtLimitePerfilesFechaVenc.Text);
            pParametro.LimitePerfilesAnual = this.txtLimitePerfilesAnual.Decimal;
            pParametro.IdPersonaFisicaRelacionadaActTerrorista = Convert.ToInt32(this.ddlPersonaRelacionadaActTerrorista.Text);
            pParametro.IdPersonaFisicaRelacionadaTripleFrontera = Convert.ToInt32(this.ddlPersonaRelacionadaTripleFrontera.Text);
            pParametro.CargoPersonaJuridicaVinculada = this.txtCargoPersonaJuridicaVinc.Text;
            pParametro.IdPersonaJuridicaVinculada = this.hdfIdAfiliadoJuridVinc.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliadoJuridVinc.Value);
            //pParametro.EsCliente = this.ddlEsCliente.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEsCliente.SelectedValue);
            pParametro.EsPEP = this.ddlEsPEP.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEsPEP.SelectedValue);
            pParametro.FechaInicio = this.txtFechaDesde.Visible == true && this.txtFechaDesde.Text != string.Empty ? Convert.ToDateTime(this.txtFechaDesde.Text) : (Nullable<DateTime>)null;
            pParametro.FechaFin = this.txtFechaHasta.Visible == true && this.txtFechaHasta.Text != string.Empty ? Convert.ToDateTime(this.txtFechaHasta.Text) : (Nullable<DateTime>)null;

            pParametro.Comentarios = this.ctrComentarios.ObtenerLista();
            pParametro.Archivos = this.ctrArchivos.ObtenerLista();
        }
        private void CargarCombos()
        {
            this.ddlPersonaRelacionadaActTerrorista.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.ListaConSinResultado);
            this.ddlPersonaRelacionadaActTerrorista.DataValueField = "IdListaValorDetalle";
            this.ddlPersonaRelacionadaActTerrorista.DataTextField = "Descripcion";
            this.ddlPersonaRelacionadaActTerrorista.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlPersonaRelacionadaActTerrorista, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlPersonaRelacionadaTripleFrontera.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.ListaConSinResultado);
            this.ddlPersonaRelacionadaTripleFrontera.DataValueField = "IdListaValorDetalle";
            this.ddlPersonaRelacionadaTripleFrontera.DataTextField = "Descripcion";
            this.ddlPersonaRelacionadaTripleFrontera.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlPersonaRelacionadaTripleFrontera, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ListaSiNo = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.SiNo);
            this.ddlEsPEP.DataSource = this.ListaSiNo;
            this.ddlEsPEP.DataValueField = "IdListaValorDetalle";
            this.ddlEsPEP.DataTextField = "Descripcion";
            this.ddlEsPEP.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlEsPEP, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //this.ddlEsCliente.DataSource = this.ListaSiNo;
            //this.ddlEsCliente.DataValueField = "IdListaValorDetalle";
            //this.ddlEsCliente.DataTextField = "Descripcion";
            //this.ddlEsCliente.DataBind();
            //AyudaProgramacion.InsertarItemSeleccione(this.ddlEsCliente, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEnActividad.DataSource = this.ListaSiNo;
            this.ddlEnActividad.DataValueField = "IdListaValorDetalle";
            this.ddlEnActividad.DataTextField = "Descripcion";
            this.ddlEnActividad.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlEnActividad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlEsPEP_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlEsPEP.SelectedValue))
            {
                string codigoValor = this.ListaSiNo.Find(x => x.IdListaValorDetalle == Convert.ToInt32(this.ddlEsPEP.SelectedValue)).CodigoValor;
                if (!string.IsNullOrEmpty(codigoValor) && codigoValor != "0")
                {
                    this.txtCargo.Visible = true;
                    this.txtDependencia.Visible = true;
                    this.txtPais.Visible = true;
                    this.ddlEnActividad.Visible = true;
                    this.lblCargo.Visible = true;
                    this.lblPais.Visible = true;
                    this.lblDesempenia.Visible = true;
                    this.lblDependencia.Visible = true;
                    this.lblFechaDesde.Visible = true;
                    this.txtFechaDesde.Visible = true;
                    this.txtFechaHasta.Visible = true;
                    if (GestionControl == Gestion.Consultar)
                    {
                        this.txtCargo.Enabled = false;
                        this.txtDependencia.Enabled = false;
                        this.ddlEnActividad.Enabled = false;
                        this.txtPais.Enabled = false;
                        this.lblFechaDesde.Enabled = false;
                        this.txtFechaDesde.Enabled = false;
                        this.txtFechaHasta.Enabled = false;
                    }
                    return;
                }
            }
            this.txtCargo.Visible = false;
            this.txtDependencia.Visible = false;
            this.txtPais.Visible = false;
            this.ddlEnActividad.Visible = false;
            this.lblCargo.Visible = false;
            this.lblPais.Visible = false;
            this.lblDesempenia.Visible = false;
            this.lblDependencia.Visible = false;
            this.lblFechaDesde.Visible = false;
            this.txtFechaDesde.Visible = false;
            this.txtFechaHasta.Visible = false;
            this.upPEP.Update();
        }
        protected void button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.hdfIdAfiliadoJuridVinc.Value))
            {
                this.ddlAfiliado.Items.Add(new ListItem(this.hdfAfiliadoJuridVinc.Value, this.hdfIdAfiliadoJuridVinc.Value));
                this.ddlAfiliado.SelectedValue = this.hdfIdAfiliadoJuridVinc.Value;
                this.upAfiliado.Update();
            }
            else
            {
                this.ddlAfiliado.Items.Add(new ListItem("Ingrese el apellido, DNI o nro. de socio", ""));
                this.ddlAfiliado.SelectedValue = "";
            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {

            try
            {
                RepReportes reporte = new RepReportes();
                string parametro = this.MiAfiliado.IdAfiliado.ToString();

                RepParametros param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                param.Parametro = "IdAfiliado";
                param.ValorParametro = parametro;


                TGEPlantillas plantilla = new TGEPlantillas();
                plantilla.Codigo = "AfiAfiliadosDatosUIF";
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                reporte.StoredProcedure = plantilla.NombreSP;
                reporte.Parametros.Add(param);

                DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdAfiliado", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.Page, "AfiliadoUIF_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("No se pudo imprimir el comprobante.", true);
            }

        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MapearControlesAObjeto(this.MiAfiliadoDatosUIF);
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MiAfiliadoDatosUIF.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.PersistirDatosGrilla();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAfiliadoDatosUIF.IdAfiliadoDatosUIF = 0;
                    guardo = AfiliadosF.AfiAfiliadosDatosUIFAgregar(this.MiAfiliadoDatosUIF);
                    if (guardo)
                    {
                        this.btnImprimir.Visible = true;
                        this.MostrarMensaje(this.MiAfiliadoDatosUIF.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = AfiliadosF.AfiAfiliadosDatosUIFModificar(this.MiAfiliadoDatosUIF);
                    if (guardo)
                    {
                        this.btnImprimir.Visible = true;
                        this.MostrarMensaje(this.MiAfiliadoDatosUIF.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiAfiliadoDatosUIF.CodigoMensaje, true, this.MiAfiliadoDatosUIF.CodigoMensajeArgs);
                if (this.MiAfiliadoDatosUIF.dsResultado != null)
                {
                    this.MiAfiliadoDatosUIF.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.AfiliadosModificarDatosUIFCancelar != null)
                this.AfiliadosModificarDatosUIFCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.AfiliadosModificarDatosUIFAceptar != null)
                this.AfiliadosModificarDatosUIFAceptar(null, this.MiAfiliado);
        }

        #region GRILLA MATRICES
        protected void gvMatrizRiesgo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (!(e.CommandName == Gestion.Consultar.ToString()
            //    || e.CommandName == Gestion.Anular.ToString()))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            //this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("Gestion", e.CommandName);
            //this.MisParametrosUrl.Add("IdListaEleccionPostulante", this.MiListaEleccion.Postulantes[indiceColeccion].IdListaEleccionPostulante);

            //if (e.CommandName == Gestion.Anular.ToString())
            //{
            //    this.MiListaEleccion.Postulantes[indiceColeccion].EstadoColeccion = EstadoColecciones.Borrado;
            //    AyudaProgramacion.CargarGrillaListas<EleListasEleccionesPostulantes>(this.MiListaEleccion.Postulantes, true, this.gvPostulantes, true);
            //}
        }

        protected void gvMatrizRiesgo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                TextBox txtValor = ((TextBox)e.Row.FindControl("txtValor"));
                if (GestionControl == Gestion.Modificar || GestionControl == Gestion.Consultar)
                {
                    AfiAfiliadosMatrizRiesgo item = (AfiAfiliadosMatrizRiesgo)e.Row.DataItem;
                    if (item.IdAfiliadoMatriz > 0)
                    {
                        txtValor.Text = item.Valor.ToString();
                    }
                    switch (GestionControl)
                    {
                        case Gestion.Modificar:
                            txtValor.Enabled = true;
                            break;
                        case Gestion.Consultar:
                            txtValor.Enabled = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void IniciarGrilla()
        {
            this.gvMatrizRiesgo.DataSource = this.MisMatrices;
            this.gvMatrizRiesgo.DataBind();
            foreach (var item in this.MisMatrices.Rows)
            {
                this.MiAfiliadoDatosUIF.MatricesDeRiesgo.Add(new AfiAfiliadosMatrizRiesgo());
            }
        }
        private void PersistirDatosGrilla()
        {
            if (this.MiAfiliadoDatosUIF.MatricesDeRiesgo.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvMatrizRiesgo.Rows)
            {
                HiddenField hdfIdMatrizRiesgo = (HiddenField)fila.FindControl("hdfIdMatrizRiesgo");
                HiddenField hdfMatrizRiesgo = (HiddenField)fila.FindControl("hdfMatrizRiesgo");
                TextBox txtValor = (TextBox)fila.FindControl("txtValor");

                if (string.IsNullOrEmpty(hdfIdMatrizRiesgo.Value))
                    hdfIdMatrizRiesgo.Value = "-1";

                this.MiAfiliadoDatosUIF.MatricesDeRiesgo[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAfiliadoDatosUIF.MatricesDeRiesgo[fila.RowIndex], GestionControl);
                this.MiAfiliadoDatosUIF.MatricesDeRiesgo[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                this.MiAfiliadoDatosUIF.MatricesDeRiesgo[fila.RowIndex].Valor = txtValor.Text == string.Empty ? (Nullable<int>)null : Convert.ToInt32(txtValor.Text.Replace(".", ""));
                this.MiAfiliadoDatosUIF.MatricesDeRiesgo[fila.RowIndex].EstadoColeccion = EstadoColecciones.Modificado;
                this.MiAfiliadoDatosUIF.MatricesDeRiesgo[fila.RowIndex].Matriz = hdfMatrizRiesgo.Value;
                this.MiAfiliadoDatosUIF.MatricesDeRiesgo[fila.RowIndex].IdMatriz = Convert.ToInt32(hdfIdMatrizRiesgo.Value);
            }
        }
        #endregion
        #region GRILLA ESPOSA
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        #endregion
    }
}
