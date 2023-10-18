using Afiliados;
using Afiliados.Entidades;
using Ahorros;
using Ahorros.Entidades;
using Cargos;
using Cargos.Entidades;
using Comunes.Entidades;
using Evol.Controls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;

namespace IU.Modulos.Cargos.Controles
{
    public partial class CargoAfiliadoModificarDatosFormaCobro : ControlesSeguros
    {
        private CarCuentasCorrientes CuentaCorriente
        {
            get { return (CarCuentasCorrientes)Session[this.MiSessionPagina + "CargoAfiliadoModificarDatosPopUpCuentaCorriente"]; }
            set { Session[this.MiSessionPagina + "CargoAfiliadoModificarDatosPopUpCuentaCorriente"] = value; }
        }

        public delegate void CargoAfiliadoModificarDatosFormaCobroAceptarEventHandler(CarCuentasCorrientes e);
        public event CargoAfiliadoModificarDatosFormaCobroAceptarEventHandler ModificarDatosAceptar;
        public delegate void CargoAfiliadoModificarDatosFormaCobroCancelarEventHandler();
        public event CargoAfiliadoModificarDatosFormaCobroCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
        }

        public void IniciarControl(CarCuentasCorrientes pCuentaCorriente, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CuentaCorriente = CargosF.CuentasCorrientesObtenerDatosCompletos(pCuentaCorriente);
            SetInitializeCulture(CuentaCorriente.Moneda.Moneda);
            this.CargarCombos();
            this.MapearObjetoAControles(this.CuentaCorriente);

            string funcion;
            switch (pGestion)
            {
                case Gestion.Consultar:
                    this.ddlFormaCobro.Enabled = false;
                    this.ddlNumeroSocio.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                case Gestion.Modificar:
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.AnularConfirmar:
                    this.ddlFormaCobro.Enabled = false;
                    this.pnlCuentasAhorros.Visible = true;
                    AhoCuentas cuenta = new AhoCuentas();
                    cuenta.CuentaTipo.IdCuentaTipo = (int)EnumAhorrosCuentasTipos.CajaAhorro;
                    cuenta.Estado.IdEstado = (int)EstadosAhorrosCuentas.CuentaAbierta;
                    cuenta.Moneda.IdMoneda = pCuentaCorriente.Moneda.IdMoneda;
                    cuenta.Afiliado.IdAfiliado = pCuentaCorriente.IdAfiliado;
                    this.ddlCuentasAhorros.DataSource = AhorroF.CuentasObtenerListaFiltro(cuenta);
                    this.ddlCuentasAhorros.DataValueField = "IdCuenta";
                    this.ddlCuentasAhorros.DataTextField = "CuentaDatos";
                    this.ddlCuentasAhorros.DataBind();
                    if (this.ddlCuentasAhorros.Items.Count != 1)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlCuentasAhorros, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    //this.txtImporteDesimputar.Decimal = this.CuentaCorriente.ImporteCobrado;
                    this.gvDatos.Columns[6].Visible = true;
                    this.gvDatos.Columns[6].HeaderText = "A desimputar";
                    this.gvDatos.ShowFooter = true;
                    this.gvDatos.DataBind();
                    this.btnAceptar.Visible = true;
                    funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarDesimputarCobro"));
                    this.btnAceptar.Attributes.Add("OnClick", funcion);
                    break;
                case Gestion.Renovar:
                    this.ddlFormaCobro.Enabled = false;
                    this.btnAceptar.Visible = true;
                    //this.gvDatos.Columns[5].Visible = true;
                    this.gvDatos.Columns[6].Visible = true;
                    this.gvDatos.ShowFooter = true;
                    this.gvDatos.DataBind();
                    funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarRevertirCobro"));
                    this.btnAceptar.Attributes.Add("OnClick", funcion);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            TGEFormasCobrosAfiliados formasCobroAfi = new TGEFormasCobrosAfiliados();
            formasCobroAfi.IdAfiliado = this.CuentaCorriente.IdAfiliado;
            formasCobroAfi.IdTipoCargo = this.CuentaCorriente.TipoCargo.IdTipoCargo;
            List<TGEFormasCobrosAfiliados> lista = TGEGeneralesF.FormasCobrosAfiliadosObtenerPorAfiliadoTipoCargo(formasCobroAfi);

            this.ddlFormaCobro.DataSource = lista;
            this.ddlFormaCobro.DataValueField = "FormaCobroIdFormaCobro";
            this.ddlFormaCobro.DataTextField = "FormaCobroDescripcion";
            this.ddlFormaCobro.DataBind();
        }

        private void MapearControlesAObjeto(CarCuentasCorrientes pCuentaCorriente)
        {
            pCuentaCorriente.FormaCobro.IdFormaCobro = Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            pCuentaCorriente.FormaCobro.FormaCobro = this.ddlFormaCobro.SelectedItem.Text;
            pCuentaCorriente.IdRefAfiliadoFormaCobro = this.hdfIdAfiliado.Value == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.hdfIdAfiliado.Value);
            pCuentaCorriente.Campos = this.ctrCamposValores.ObtenerLista();
        }

        private void MapearObjetoAControles(CarCuentasCorrientes pCuentaCorriente)
        {
            this.txtFechaMoviminto.Text = pCuentaCorriente.FechaMovimiento.ToShortDateString();
            this.txtImporte.Text = pCuentaCorriente.Importe.ToString("N2");
            this.txtPeriodo.Text = pCuentaCorriente.Periodo.ToString();
            this.txtTipoCargo.Text = pCuentaCorriente.TipoCargo.TipoCargo;
            this.txtTipoValor.Text = pCuentaCorriente.TipoValor.TipoValor;
            this.txtConcepto.Text = pCuentaCorriente.Concepto;
            ListItem item = this.ddlFormaCobro.Items.FindByValue(pCuentaCorriente.FormaCobro.IdFormaCobro.ToString());
            if (item == null)
                this.ddlFormaCobro.Items.Add(new ListItem(pCuentaCorriente.FormaCobro.FormaCobro, pCuentaCorriente.FormaCobro.IdFormaCobro.ToString()));
            this.ddlFormaCobro.SelectedValue = pCuentaCorriente.FormaCobro.IdFormaCobro.ToString();

            this.gvDatos.DataSource = CargosF.CuentasCorrientesObtenerCobrosPorCargo(pCuentaCorriente);
            this.gvDatos.DataBind();

            if (pCuentaCorriente.IdRefAfiliadoFormaCobro.HasValue && pCuentaCorriente.IdRefAfiliadoFormaCobro.Value > 0)
            { 
                //AfiAfiliados afi = new AfiAfiliados();
                //afi.IdAfiliado = pCuentaCorriente.IdRefAfiliadoFormaCobro.Value;
                //afi = AfiliadosF.AfiliadosObtenerDatos(afi);
                this.ddlNumeroSocio.Items.Add(new ListItem(pCuentaCorriente.Filtro, pCuentaCorriente.IdRefAfiliadoFormaCobro.Value.ToString()));
                this.hdfIdAfiliado.Value = pCuentaCorriente.IdRefAfiliadoFormaCobro.Value.ToString();
                this.hdfRazonSocial.Value = pCuentaCorriente.Filtro;
                this.ddlNumeroSocio.SelectedValue = pCuentaCorriente.IdRefAfiliadoFormaCobro.Value.ToString();
            }

            this.MostrarDetalleFormacobro();
            this.ctrAuditoria.IniciarControl(pCuentaCorriente);
            this.ctrCamposValores.IniciarControl(pCuentaCorriente, new Objeto(), this.GestionControl);
        }

        protected void ddlFormaCobro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFormaCobro.SelectedValue))
            {
                this.MostrarDetalleFormacobro();
            }
        }

        private void MostrarDetalleFormacobro()
        {
            TGEFormasCobrosAfiliados fca = new TGEFormasCobrosAfiliados();
            fca.IdAfiliado = this.CuentaCorriente.IdAfiliado;
            List<TGEFormasCobrosAfiliados> listafca = TGEGeneralesF.FormasCobrosAfiliadosObtenerListaFiltro(fca);
            if (!(string.IsNullOrEmpty(this.ddlFormaCobro.SelectedValue)))
            {
                fca = listafca.Find(x => x.FormaCobro.IdFormaCobro == Convert.ToInt32(this.ddlFormaCobro.SelectedValue));
                List<TGECampos> lista = new List<TGECampos>();
                if (fca != null)
                {
                    fca.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    lista = TGEGeneralesF.CamposObtenerListaFiltro(fca, fca.FormaCobro);
                }
                lista = lista.Where(x => x.CampoValor.IdCampoValor > 0).ToList();
                this.gvDetalleFormaCobro.DataSource = AyudaProgramacion.PivotList(lista);
                this.gvDetalleFormaCobro.DataBind();

                DataTable dtFormaCobro = CargosF.CuentasCorrientesObetenerCodigosConceptosCargos(this.CuentaCorriente);
                this.gvFormasCobrosCodigosConceptos.DataSource = dtFormaCobro;
                this.gvFormasCobrosCodigosConceptos.DataBind();
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (this.GestionControl == Gestion.Renovar
                    || this.GestionControl == Gestion.AnularConfirmar)
                {
                    TextBox aCobrar = (TextBox)e.Row.FindControl("txtImporteRevertir");
                    aCobrar.Attributes.Add("onchange", "CalcularItem();");
                }
            }
        }

        public bool CargarLoteRevertirCobrosCargos()
        {
            bool resultado = false;
            this.CuentaCorriente.LoteCuentasCorrientes = new XmlDocument();
            decimal importeRevertir;

            XmlNode items = this.CuentaCorriente.LoteCuentasCorrientes.CreateElement("CuentasCorrientes");
            this.CuentaCorriente.LoteCuentasCorrientes.AppendChild(items);

            XmlNode item;
            XmlAttribute attribute;
            foreach (GridViewRow row in this.gvDatos.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    importeRevertir = ((CurrencyTextBox)row.FindControl("txtImporteRevertir")).Decimal;

                    if (importeRevertir > 0)
                    {
                        item = this.CuentaCorriente.LoteCuentasCorrientes.CreateElement("CuentaCorriente");

                        attribute = this.CuentaCorriente.LoteCuentasCorrientes.CreateAttribute("IdCuentaCorrienteCobro");
                        attribute.Value = this.gvDatos.DataKeys[row.RowIndex]["IdCuentaCorrienteCobro"].ToString();
                        item.Attributes.Append(attribute);
                        items.AppendChild(item);

                        attribute = this.CuentaCorriente.LoteCuentasCorrientes.CreateAttribute("Importe");
                        attribute.Value = importeRevertir.ToString().Replace(',', '.'); ;
                        item.Attributes.Append(attribute);
                        items.AppendChild(item);
                        resultado = true;
                    }
                }
            }
            return resultado;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MapearControlesAObjeto(this.CuentaCorriente);
            this.CuentaCorriente.EstadoColeccion = EstadoColecciones.Modificado;
            this.CuentaCorriente.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            bool resultado = false;
            switch (this.GestionControl)
            {
                case Gestion.Modificar:
                    resultado = CargosF.CuentasCorrientesActualizar(this.CuentaCorriente);
                    break;
                case Gestion.AnularConfirmar:
                    if (!this.CargarLoteRevertirCobrosCargos())
                    {
                        this.MostrarMensaje("ValidarImporteDesimputarItems", true);
                        return;
                    }
                    this.CuentaCorriente.IdCuenta = this.ddlCuentasAhorros.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCuentasAhorros.SelectedValue);
                    resultado = CargosF.CuentasCorrientesDesimputarCobro(this.CuentaCorriente);
                    break;
                case Gestion.Renovar:
                    if (!this.CargarLoteRevertirCobrosCargos())
                    {
                        this.MostrarMensaje("ValidarImporteRevertirItems", true);
                        return;
                    }
                    resultado = CargosF.CuentasCorrientesRevertirCobroCargos(this.CuentaCorriente);
                    break;
                default:
                    break;
            }
            if (resultado)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.CuentaCorriente.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.CuentaCorriente.CodigoMensaje, true, this.CuentaCorriente.CodigoMensajeArgs);
                if (this.CuentaCorriente.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.CuentaCorriente);
                    this.CuentaCorriente.dsResultado = null;
                }
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
                this.ModificarDatosAceptar(this.CuentaCorriente);
        }
    }
}