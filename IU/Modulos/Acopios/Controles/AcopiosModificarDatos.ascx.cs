using Acopios;
using Acopios.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Acopios.Controles
{
    public partial class AcopiosModificarDatos : ControlesSeguros
    {
        public AcpAcopios MiAcopio
        {
            get { return this.PropiedadObtenerValor<AcpAcopios>("AcopiosModificarDatosMiAcopio"); }
            set { this.PropiedadGuardarValor("AcopiosModificarDatosMiAcopio", value); }
        }
        bool AceptarContinuar = false;

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(AcpAcopios pParametro, Gestion pGestion)
        {
            this.MiAcopio = pParametro;
            this.hdfTabla.Value = this.MiAcopio.Tabla;
            this.GestionControl = pGestion;
            this.txtCodigo.Text = this.MiAcopio.IdRefTabla.ToString();
            this.txtRazonSocial.Text = this.MiAcopio.RazonSocial;
            this.hdfTabla.Value = this.MiAcopio.Tabla;
            //this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAcopio.AcopiosImportes.Add(new AcpAcopiosImportes());
                    this.MiAcopio.AcopiosImportes.Add(new AcpAcopiosImportes());
                    this.gvDatos.DataSource = this.MiAcopio.AcopiosImportes;
                    this.gvDatos.DataBind();
                    this.tpDetalleConsumos.Visible = false;
                    //this.ctrCamposValores.IniciarControl(this.MiHabitacion, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    this.MiAcopio = AcopiosF.AcopiosObtenerDatosCompletos(this.MiAcopio);
                    this.MapearObjetoAControles(this.MiAcopio);
                    break;
                case Gestion.Consultar:
                    break;
                default:
                    break;
            }
        }

        private void MapearControlesAObjeto(AcpAcopios pAcopio)
        {
            pAcopio.Descripcion = this.txtDescripcion.Text;
        }

        private void MapearObjetoAControles(AcpAcopios pAcopio)
        {
            this.txtIdAcopio.Text = pAcopio.IdAcopio.ToString();
            this.txtDescripcion.Text = pAcopio.Descripcion;
            this.txtImporteTotal.Text = pAcopio.ImporteTotal.ToString("C2");
        }

        protected void btnAceptarContinuar_Click(object sender, EventArgs e)
        {
            this.AceptarContinuar = true;
            AcpAcopios parametros = this.BusquedaParametrosObtenerValor<AcpAcopios>();
            parametros.HashTransaccion = this.tcDatos.ActiveTabIndex;
            this.BusquedaParametrosGuardarValor<AcpAcopios>(parametros);
            this.btnAceptar_Click(sender, e);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.btnAceptarContinuar.Visible = false;
            this.MapearControlesAObjeto(this.MiAcopio);
            this.PersistirAcopiosImportes();
            this.MiAcopio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //this.MiReserva.CargarLoteCamposValores();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = AcopiosF.AcopiosAgregar(this.MiAcopio);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiAcopio.CodigoMensaje, false, this.MiAcopio.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Anular:
                    //this.MiReserva.Estado.IdEstado = (int)Estados.Baja;
                    //guardo = HotelesF.ReservasModificar(this.MiReserva);
                    //if (guardo)
                    //{
                    //    this.MostrarMensaje(this.MiReserva.CodigoMensaje, false);
                    //}
                    break;
                case Gestion.Modificar:
                    guardo = AcopiosF.AcopiosModificar(this.MiAcopio);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiAcopio.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.btnAceptarContinuar.Visible = true;
                this.MostrarMensaje(this.MiAcopio.CodigoMensaje, true, this.MiAcopio.CodigoMensajeArgs);
                if (this.MiAcopio.dsResultado != null)
                {
                    //this.ctrPopUpGrilla.IniciarControl(this.MiReserva);
                    this.MiAcopio.dsResultado = null;
                }
            }
            else
            {
                if (this.AceptarContinuar)
                {
                    this.btnAceptar.Visible = true;
                    this.btnAceptarContinuar.Visible = true;
                    this.IniciarControl(this.MiAcopio, Gestion.Modificar);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DeshabilitarControlesScript", "deshabilitarControles('deshabilitarControles');", true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }

        #region Acopios Importes
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirAcopiosImportes();
            AcpAcopiosImportes item = new AcpAcopiosImportes();
            this.MiAcopio.AcopiosImportes.Add(item);
            this.gvDatos.DataSource = this.MiAcopio.AcopiosImportes;
            this.gvDatos.DataBind();
        }

        private void PersistirAcopiosImportes()
        {
            if (this.MiAcopio.AcopiosImportes.Count == 0)
                return;

            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("gvDatos")).ToList();
            string k;
            int numeroFila = 2;
            AcpAcopiosImportes det;
            bool modifica;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    modifica = false;
                    det = this.MiAcopio.AcopiosImportes.Find(x => x.IdAcopioImporte == Convert.ToInt64(this.gvDatos.DataKeys[fila.RowIndex]["IdAcopioImporte"].ToString()));
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$ddlDetalleImportes"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.IdRefTabla != Convert.ToInt64(this.Request.Form[k]))
                            modifica = true;
                        det.IdRefTabla = Convert.ToInt64(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtFecha"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Fecha.HasValue && det.Fecha.Value.Date != Convert.ToDateTime(this.Request.Form[k]).Date)
                            modifica = true;
                        det.Fecha = Convert.ToDateTime(this.Request.Form[k]);
                    }
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$hdfImporte"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        modifica = true;
                        det.Importe = Convert.ToDecimal(this.Request.Form[k].Replace(".", ","));
                    }                    
                    if (modifica)
                        det.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(det, this.GestionControl);
                    numeroFila++;
                }
            }
            this.MiAcopio.ImporteTotal = this.MiAcopio.AcopiosImportes.Sum(x => x.Importe);
        }
        #endregion
    }
}