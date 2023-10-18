using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using Comunes.Entidades;
using Bancos;
using Tesorerias.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Cobros.Entidades;
using Tesorerias;
using System.Xml;
using Comunes.LogicaNegocio;

namespace IU.Modulos.Bancos.Controles
{
    public partial class ChequesCambiarValoresDatos : ControlesSeguros
    {
        private List<TESCheques> MisChequesCambiar
        {
            get { return (List<TESCheques>)Session[this.MiSessionPagina + "ChequesCambiarValoresDatosChequesCambiar"]; }
            set { Session[this.MiSessionPagina + "ChequesCambiarValoresDatosChequesCambiar"] = value; }
        }

        private TESCajasMovimientos MiCajaMovimiento
        {
            get { return (TESCajasMovimientos)Session[this.MiSessionPagina + "ChequesCambiarValoresDatosMiCajaMovimiento"]; }
            set { Session[this.MiSessionPagina + "ChequesCambiarValoresDatosMiCajaMovimiento"] = value; }
        }

        //public delegate void ChequesModificarDatosAceptarEventHandler(TESCheques e);
        //public event ChequesModificarDatosAceptarEventHandler ChequesCambiarDatosAceptar;

        //public delegate void ChequesModificarDatosCancelarEventHandler();
        //public event ChequesModificarDatosCancelarEventHandler ChequesCambiarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            //this.ctrBuscarCheque.ChequesBuscarSeleccionar += new Tesoreria.Controles.ChequesTercerosPopUp.ChequesBuscarEventHandler(ctrBuscarCheque_ChequesBuscarSeleccionar);
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                this.btnAceptar.Attributes.Add("OnClick", funcion);
            }
        }

        public void IniciarControl(TESCajasMovimientos pParametro, Gestion pGestion)
        {
            this.MisChequesCambiar = new List<TESCheques>();
            this.GestionControl = pGestion;
            this.MiCajaMovimiento = pParametro;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCajaMovimiento.Fecha = DateTime.Now;
                    this.MiCajaMovimiento.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.CambioCheques);
                    break;
                //case Gestion.Consultar:
                //    this.MiCajaMovimiento = TesoreriasF.CajasMovimientosObtenerDatosCompletos(pParametro);
                //    this.MapearObjetoAControles(this.MiCajaMovimiento);
                //    this.ddlChequesTerceros.Enabled = false;
                //    this.btnAceptar.Visible = false;
                //    this.btnAgregarChequesTerceros.Visible = false;
                //    break;
                default:
                    break;
            }
            this.ctrFechaCajaContable.IniciarControl(this.GestionControl, this.MiCajaMovimiento.Fecha);
            this.ctrIngresosValores.IniciarControl(this.MiCajaMovimiento, this.GestionControl, pParametro.TipoOperacion, null, true);
        }

        //void ctrBuscarCheque_ChequesBuscarSeleccionar(TESCheques e)
        //{
        //    this.MisChequesCambiar = new List<TESCheques>();
        //    this.MisChequesCambiar.Add(e);
        //    AyudaProgramacion.CargarGrillaListas<TESCheques>(this.MisChequesCambiar, false, this.gvChequesTerceros, true);
        //}

        //protected void btnBuscarCheque_Click(object sender, EventArgs e)
        //{
        //    List<TESCheques> filtro = new List<TESCheques>();
        //    this.ctrBuscarCheque.IniciarControl(filtro);
        //}
        //private void MapearObjetoAControles(TESCajasMovimientos pParametro)
        //{
        //    ListItem item3 = this.ddlChequesTerceros.Items.FindByValue(pParametro.IdCajaMovimiento.ToString());
        //    if (item3 == null)
        //        this.ddlChequesTerceros.Items.Add(new ListItem(pParametro.Descripcion, pParametro.IdCajaMovimiento.ToString()));

        //    this.ddlChequesTerceros.SelectedValue = pParametro.IdCajaMovimiento.ToString();

        //}
        #region "Grilla Cheques Terceros"
        protected void btnAgregarCheque_Click(object sender, EventArgs e)
        {
            TESCheques filtro = new TESCheques();
            string[] ar = ddlChequesTerceros.SelectedItem.ToString().Split('-');
            string numeroCheque = ar[0].ToString().Trim();
            filtro.IdCheque  = ddlChequesTerceros.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlChequesTerceros.SelectedValue);
            filtro.NumeroCheque = numeroCheque;
            filtro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            List<TESCheques> lista = BancosF.ChequesObtenerChequesTercerosFiltro(filtro);
            if (lista.Count == 1)
            {
                MisChequesCambiar = new List<TESCheques>();
                MisChequesCambiar.Add(lista[0]);
                AyudaProgramacion.CargarGrillaListas<TESCheques>(MisChequesCambiar, false, gvChequesTerceros, true);
                btnAgregarChequesTerceros.Visible = false;
            }
        }

        protected void gvChequesTerceros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCheques cheque = this.MisChequesCambiar[indiceColeccion];
            this.MisChequesCambiar.Remove(cheque);
            this.MisChequesCambiar = AyudaProgramacion.AcomodarIndices<TESCheques>(this.MisChequesCambiar);
            AyudaProgramacion.CargarGrillaListas<TESCheques>(this.MisChequesCambiar, false, this.gvChequesTerceros, false);
            btnAgregarChequesTerceros.Visible = true;
        }

        protected void gvChequesTerceros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                TESCheques detalle = (TESCheques)e.Row.DataItem;

                if (this.GestionControl == Gestion.Consultar)
                    ibtnEliminar.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                
            }
        }

        #endregion      

        private void CargarCombos()
        {
            TESCheques cheques = new TESCheques();
            cheques.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;
            ddlChequesTerceros.DataSource = BancosF.ChequesObtenerChequesTerceros(cheques);
            ddlChequesTerceros.DataValueField = "IdCheque";
            ddlChequesTerceros.DataTextField = "Concepto";
            ddlChequesTerceros.DataBind();
            if (ddlChequesTerceros.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(ddlChequesTerceros, ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.btnAceptar.Visible = false;
            this.MiCajaMovimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiCajaMovimiento.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            this.MiCajaMovimiento.CajasMovimientosValores = new List<TESCajasMovimientosValores>(ctrIngresosValores.ObtenerCajaMovimiento().CajasMovimientosValores);
            this.MiCajaMovimiento.Importe = this.MiCajaMovimiento.CajasMovimientosValores.Sum(x => x.Importe);
            this.MiCajaMovimiento.Fecha = this.ctrFechaCajaContable.dFechaCajaContable.Value;
            this.MiCajaMovimiento.Descripcion = string.Join(" ", this.MisChequesCambiar.Select(x => string.Concat("Nro. ", x.NumeroCheque, " - Banco ", x.Banco.Descripcion)).ToArray());

            if (this.MisChequesCambiar.Count == 0)
            {
                this.MostrarMensaje("ValidarCambioChequesItems", true);
                return;
            }

            if (this.MiCajaMovimiento.Importe != this.MisChequesCambiar.Sum(x => x.Importe))
            {
                List<string> listaArgs = new List<string>();
                listaArgs.Add(this.MisChequesCambiar.Sum(x => x.Importe).ToString("C2"));
                listaArgs.Add(this.MiCajaMovimiento.Importe.ToString("C2"));
                this.MostrarMensaje("ValidarCambioChequesImportes", true, listaArgs);
                return;
            }

            //SOLO PESOS ARGENTINOS
            this.MiCajaMovimiento.CajaMoneda.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            this.MiCajaMovimiento = TesoreriasF.CajasMovimientosObtenerMovimientoValoresXML(this.MiCajaMovimiento);

            XmlNode nodoraiz = this.MiCajaMovimiento.LoteCajasMovimientosValores.SelectSingleNode("CajasMovimientosValores");

            XmlNode nodo;
            XmlAttribute attribute;
            foreach (TESCheques cheque in this.MisChequesCambiar)
            {
                nodo = this.MiCajaMovimiento.LoteCajasMovimientosValores.CreateElement("DevolucionCambioCheques");

                attribute = this.MiCajaMovimiento.LoteCajasMovimientosValores.CreateAttribute("Descripcion");
                attribute.Value = string.Concat("Nro. ", cheque.NumeroCheque, " - Banco ", cheque.Banco.Descripcion);
                nodo.Attributes.Append(attribute);

                attribute = this.MiCajaMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdTipoOperacion");
                attribute.Value = ((int)EnumTGETiposOperaciones.DevolucionCambioCheques).ToString();
                nodo.Attributes.Append(attribute);

                attribute = this.MiCajaMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdRefTipoOperacion");
                attribute.Value = cheque.IdCheque.ToString();
                nodo.Attributes.Append(attribute);

                attribute = this.MiCajaMovimiento.LoteCajasMovimientosValores.CreateAttribute("Importe");
                attribute.Value = cheque.Importe.ToString().Replace(',', '.');
                nodo.Attributes.Append(attribute);

                attribute = this.MiCajaMovimiento.LoteCajasMovimientosValores.CreateAttribute("IdEstado");
                attribute.Value = cheque.Estado.IdEstado.ToString();
                nodo.Attributes.Append(attribute);        

                nodoraiz.AppendChild(nodo);
            }

            if (!TesoreriasF.CajasConfirmarMovimientoXml(this.MiCajaMovimiento))
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiCajaMovimiento.CodigoMensaje, true, this.MiCajaMovimiento.CodigoMensajeArgs);
                if (this.MiCajaMovimiento.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiCajaMovimiento);
                    this.MiCajaMovimiento.dsResultado = null;
                }
            }
            else
            {
                this.btnAceptar.Visible = false;
                this.btnImprimir.Visible = true;
                this.MostrarMensaje(this.MiCajaMovimiento.CodigoMensaje, false);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.MiCajaMovimiento = new TESCajasMovimientos();
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            
        }
    }
}