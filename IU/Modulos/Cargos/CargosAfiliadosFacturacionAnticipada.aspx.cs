using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cargos;
using Cargos.Entidades;
using System.Data;
using System.Xml;

namespace IU.Modulos.Cargos
{
    public partial class CargosAfiliadosFacturacionAnticipada : PaginaAfiliados
    {
        private DataTable MisDatos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CargosAfiliadosFacturacionAnticipadaMisDatos"]; }
            set { Session[this.MiSessionPagina + "CargosAfiliadosFacturacionAnticipadaMisDatos"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPeriodoHasta, this.btnGenerarCargos);
                this.MisDatos = new DataTable();
                this.CargarCombos();
            }
        }

        private void CargarCombos()
        {
            this.ddlTiposCargos.DataSource = CargosF.TiposCargosObtenerFacturacionAnticipadaCombo(this.MiAfiliado);
            this.ddlTiposCargos.DataValueField = "IdTipoCargo";
            this.ddlTiposCargos.DataTextField = "TipoCargo";
            this.ddlTiposCargos.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlTiposCargos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnGenerarCargos_Click(object sender, EventArgs e)
        {
            CarCuentasCorrientes ctaCte = new CarCuentasCorrientes();
            ctaCte.IdAfiliado = this.MiAfiliado.IdAfiliado;
            ctaCte.Periodo = Convert.ToInt32( this.txtPeriodoHasta.Decimal);
            ctaCte.IdRefTipoOperacion = this.ddlTiposCargos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTiposCargos.SelectedValue);
            this.MisDatos = CargosF.CuentasCorrientesObtenerFacturacionAnticipada(ctaCte);
            this.gvCuentaCorriente.DataSource = this.MisDatos;
            this.gvCuentaCorriente.DataBind();
            this.btnAceptar.Visible = this.MisDatos.Rows.Count > 0;
            //this.upFacturacion.Update();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;

            CarCuentasCorrientes ctaCte = new CarCuentasCorrientes();
            ctaCte.IdAfiliado = this.MiAfiliado.IdAfiliado;
            ctaCte.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            ctaCte.FechaMovimiento = Convert.ToDateTime(this.txtFechaMovimiento.Text);
            ctaCte.LoteCuentasCorrientes = new XmlDocument();
            XmlNode ctaCteNode = ctaCte.LoteCuentasCorrientes.CreateElement("CuentasCorrientes");
            ctaCte.LoteCuentasCorrientes.AppendChild(ctaCteNode);

            XmlNode ccNode;
            XmlNode ValorNode;
            int cantidad = 0;
            int idTable;
            DataRow dr;
            foreach (GridViewRow pre in this.gvCuentaCorriente.Rows)
            {
                if (pre.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkIncluir = (CheckBox)pre.FindControl("chkIncluir");
                    if (chkIncluir.Checked)
                    {
                        cantidad++;
                        idTable = Convert.ToInt32(this.gvCuentaCorriente.DataKeys[pre.DataItemIndex]["Id"].ToString());
                        dr = this.MisDatos.AsEnumerable().First(x => x.Field<Int64>("Id") == idTable);
                        ccNode = ctaCte.LoteCuentasCorrientes.CreateElement("CuentaCorriente");

                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("IdRefTipoOperacion");
                        ValorNode.InnerText = dr["IdRefTipoOperacion"].ToString();
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("Concepto");
                        ValorNode.InnerText = dr["Concepto"].ToString();
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("IdFormaCobro");
                        ValorNode.InnerText = dr["IdFormaCobro"].ToString();
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("Periodo");
                        ValorNode.InnerText = dr["Periodo"].ToString();
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("Importe");
                        ValorNode.InnerText = dr["Importe"].ToString().Replace(",", ".");
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("IdReferenciaRegistro");
                        ValorNode.InnerText = dr["IdReferenciaRegistro"].ToString();
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("CuotaNumero");
                        ValorNode.InnerText = dr["CuotaNumero"].ToString().Replace(",",".");
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("CuotaTotal");
                        ValorNode.InnerText = dr["CuotaTotal"].ToString();
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("IdAfiliadoRef");
                        ValorNode.InnerText = dr["IdAfiliadoRef"].ToString();
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("IdRefAfiliadoFormaCobro");
                        ValorNode.InnerText = dr["IdRefAfiliadoFormaCobro"].ToString();
                        ccNode.AppendChild(ValorNode);
                        ValorNode = ctaCte.LoteCuentasCorrientes.CreateElement("TablaReferenciaRegistro");
                        ValorNode.InnerText = dr["TablaReferenciaRegistro"].ToString();
                        ccNode.AppendChild(ValorNode);

                        ctaCteNode.AppendChild(ccNode);
                    }
                }
            }

            if (cantidad == 0)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje("ValidarCantidadItems", true);
                return;
            }

            guardo = CargosF.CuentasCorrientesAgregarFacturacionAnticipada(ctaCte);
            if (guardo)
            {
                //this.ctrAsientoMostrar.IniciarControl(ctaCte);
                this.btnAceptar.Visible = true;
                this.btnCancelar.Visible = true;
                this.btnGenerarCargos_Click(null, EventArgs.Empty);
                this.MostrarMensaje(ctaCte.CodigoMensaje, false, ctaCte.CodigoMensajeArgs);
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(ctaCte.CodigoMensaje, true, ctaCte.CodigoMensajeArgs);
                if (ctaCte.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(ctaCte);
                    ctaCte.dsResultado = null;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else

                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
        }

    }
}