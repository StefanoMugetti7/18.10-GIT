using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.ComponentModel;

namespace IU.Modulos.Comunes
{
    public partial class FechaCajaContable : ControlesSeguros
    {
        public bool bCircuitoCajaAutomatico
        {
            get { return (bool)Session[this.MiSessionPagina + "FechaCajaContable"]; }
            set { Session[this.MiSessionPagina + "FechaCajaContable"] = value; }
        }

        public DateTime? dFechaCajaContable
        {
            get { return this.txtFechaCajaContabilizacion.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaCajaContabilizacion.Text); }
            set { this.txtFechaCajaContabilizacion.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public string LabelFechaCajaContabilizacion
        {
            get { return this.lblFechaCajaContabilizacion.Text; }
            set { this.lblFechaCajaContabilizacion.Text = value; }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public bool LabelFechaCajaContabilizacionVisible
        {
            get { return this.lblFechaCajaContabilizacion.Enabled; }
            set { this.lblFechaCajaContabilizacion.Enabled = value; }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public string DivCajaContabilizacion
        {
            get { return this.divCajaContabilizacion.Attributes["class"]; }
            set { this.divCajaContabilizacion.Attributes["class"] = value; }
        }


        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                if (this.LabelFechaCajaContabilizacion != null)
                {
                    this.lblFechaCajaContabilizacion.Text = this.LabelFechaCajaContabilizacion;
                    this.lblFechaCajaContabilizacion.Visible = this.LabelFechaCajaContabilizacionVisible;
                }
                if (this.DivCajaContabilizacion != null)
                    this.divCajaContabilizacion.Attributes["class"] = this.DivCajaContabilizacion;
            }
        }

        /// <summary>
        /// Metodo para la Caja
        /// </summary>
        /// <param name="pGestion"></param>
        /// <param name="pFechaComprobante"></param>
        /// <param name="pFechaCaja"></param>
        public void IniciarControl(Gestion pGestion, DateTime pFechaComprobante, DateTime pFechaCaja)
        {
            if (Gestion.Consultar != pGestion)
            {
                this.rfvFechaCajaContabilizacion.Enabled = true;
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FechaContableImputacionIgualFechaComprobante);
                bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
                if (bvalor)
                {
                    this.txtFechaCajaContabilizacion.Text = pFechaComprobante.ToShortDateString();
                }
                else
                {
                    this.txtFechaCajaContabilizacion.Text = pFechaCaja.ToShortDateString();
                }
                valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ModificaFechaContableImputacion);
                bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
                if (bvalor)
                {
                    this.txtFechaCajaContabilizacion.Enabled = true;
                    this.txtFechaCajaContabilizacion.CssClass = "form-control datepicker";
                    //this.imgFechaCajaContabilizacion.Visible = true;
                    //this.cdFechaCajaContabilizacion.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Metodo para Comprobantes
        /// </summary>
        /// <param name="pGestion"></param>
        /// <param name="pFechaComprobante"></param>
        /// <param name="pFechaCaja"></param>
        public void IniciarControl(Gestion pGestion, DateTime? pFechaComprobante)
        {
            if (Gestion.Consultar != pGestion)
            {
                this.rfvFechaCajaContabilizacion.Enabled = true;
                this.bCircuitoCajaAutomatico = false;
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CircuitoDiarioCajasAutomatico);
                bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
                if (bvalor)
                {
                    this.bCircuitoCajaAutomatico = bvalor;
                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FechaContableImputacionIgualFechaComprobante);
                    bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
                    if (bvalor)
                    {
                        this.txtFechaCajaContabilizacion.Text = pFechaComprobante.HasValue ? pFechaComprobante.Value.ToShortDateString() : string.Empty;
                    }
                    else
                    {
                        this.txtFechaCajaContabilizacion.Text = DateTime.Now.ToShortDateString();
                    }
                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ModificaFechaContableImputacion);
                    bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
                    if (bvalor)
                    {
                        this.txtFechaCajaContabilizacion.Enabled = true;
                        this.txtFechaCajaContabilizacion.CssClass = "form-control datepicker";
                        //this.imgFechaCajaContabilizacion.Visible = true;
                        //this.cdFechaCajaContabilizacion.Enabled = true;
                    }
                }
                else
                    this.txtFechaCajaContabilizacion.Text = pFechaComprobante.HasValue ? pFechaComprobante.Value.ToShortDateString() : DateTime.Now.ToShortDateString();
            }
            else
            {
                this.txtFechaCajaContabilizacion.Text = pFechaComprobante.HasValue ? pFechaComprobante.Value.ToShortDateString() : string.Empty;
            }
        }
    }
}