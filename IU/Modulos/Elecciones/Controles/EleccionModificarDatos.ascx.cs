using Comunes.Entidades;
using Elecciones;
using Elecciones.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Elecciones.Controles
{
    public partial class EleccionModificarDatos : ControlesSeguros
    {
        public DataTable MisEtapas
        {
            get { return this.PropiedadObtenerValor<DataTable>("EleccionModificarDatosMisEtapas"); }
            set { this.PropiedadGuardarValor("EleccionModificarDatosMisEtapas", value); }
        }
        private EleElecciones MiEleccion
        {
            get { return (EleElecciones)Session[this.MiSessionPagina + "EleccionModificarDatosMiEleccion"]; }
            set { Session[this.MiSessionPagina + "EleccionModificarDatosMiEleccion"] = value; }
        }

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (this.IsPostBack)
            {
                if (this.MiEleccion == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            else
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    this.PersistirDatosGrilla();
                }
            }
        }
        public void IniciarControl(EleElecciones pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MisEtapas = EleccionesF.EleccionesObtenerEtapas(pParametro);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiEleccion = pParametro;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ddlEstado.Enabled = false;
                    this.IniciarGrilla();
                    this.tpHistorial.Visible = false;
                    this.ctrArchivos.IniciarControl(new EleElecciones(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiEleccion = new EleElecciones();
                    this.MiEleccion = EleccionesF.EleccionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiEleccion);
                    this.tpHistorial.Visible = false;
                    this.ctrArchivos.IniciarControl(MiEleccion, this.GestionControl);
                    break;
                case Gestion.Consultar:
                    this.MiEleccion = EleccionesF.EleccionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiEleccion);
                    this.txtObservacion.Enabled = false;
                    this.txtAnio.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.tpHistorial.Visible = true;
                    this.ctrArchivos.IniciarControl(MiEleccion, this.GestionControl);
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }

        private void MapearObjetoAControles(EleElecciones pParametro)
        {
            this.txtAnio.Text = pParametro.Anio.ToString();
            this.txtObservacion.Text = pParametro.Eleccion;
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();

            if (pParametro.Etapas.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<EleEleccionesEtapas>(pParametro.Etapas, false, this.gvEtapas, true);
                this.upEtapas.Update();
            }
            else
            {
                IniciarGrilla();
            }
                this.ctrAuditoria.IniciarControl(pParametro);
        }

        private void MapearControlesAObjeto(EleElecciones pParametro)
        {
            pParametro.Eleccion = this.txtObservacion.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Anio = this.txtAnio.Text == string.Empty ? 0 : Convert.ToInt32(this.txtAnio.Text.Replace(".",""));
            pParametro.Archivos = ctrArchivos.ObtenerLista();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiEleccion);
            this.MiEleccion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.PersistirDatosGrilla();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    MiEleccion.IdEleccion = 0;
                    guardo = EleccionesF.EleccionesAgregar(this.MiEleccion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiEleccion.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Anular:
                    this.MiEleccion.Estado.IdEstado = (int)Estados.Baja;
                    guardo = EleccionesF.EleccionesModificar(this.MiEleccion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiEleccion.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = EleccionesF.EleccionesModificar(this.MiEleccion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiEleccion.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiEleccion.CodigoMensaje, true, this.MiEleccion.CodigoMensajeArgs);
                if (this.MiEleccion.dsResultado != null)
                {
                    this.MiEleccion.dsResultado = null;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }



        #region GRILLA ETAPAS
        protected void gvEtapas_RowCommand(object sender, GridViewCommandEventArgs e)
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

        protected void gvEtapas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                TextBox txtFechaDesde = ((TextBox)e.Row.FindControl("txtFechaDesde"));
                TextBox txtFechaHasta = ((TextBox)e.Row.FindControl("txtFechaHasta"));
                if (GestionControl == Gestion.Modificar || GestionControl == Gestion.Consultar)
                {
                    EleEleccionesEtapas item = (EleEleccionesEtapas)e.Row.DataItem;
                    if (item.IdEleccionEtapa > 0)
                    {
                        txtFechaDesde.Text = Convert.ToDateTime(item.FechaDesde).ToShortDateString();
                        txtFechaHasta.Text = Convert.ToDateTime(item.FechaHasta).ToShortDateString();
                    }
                    switch (GestionControl)
                    {
                        case Gestion.Modificar:
                            txtFechaDesde.Enabled = true;
                            txtFechaHasta.Enabled = true;
                            break;
                        case Gestion.Consultar:
                            txtFechaDesde.Enabled = false;
                            txtFechaHasta.Enabled = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void IniciarGrilla()
        {
            this.gvEtapas.DataSource = this.MisEtapas;
            this.gvEtapas.DataBind();
            foreach (var item in MisEtapas.Rows)
            {
                this.MiEleccion.Etapas.Add(new EleEleccionesEtapas());
            }
        }
        private void PersistirDatosGrilla()
        {
            if (this.MiEleccion.Etapas.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvEtapas.Rows)
            {
                HiddenField hdfIdEtapa = (HiddenField)fila.FindControl("hdfIdEtapa");
                HiddenField hdfEtapa= (HiddenField)fila.FindControl("hdfEtapa");
                TextBox txtFechaDesde= (TextBox)fila.FindControl("txtFechaDesde");
                TextBox txtFechaHasta= (TextBox)fila.FindControl("txtFechaHasta");

                if (string.IsNullOrEmpty(hdfIdEtapa.Value))
                    hdfIdEtapa.Value = "-1";

                this.MiEleccion.Etapas[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiEleccion.Etapas[fila.RowIndex], GestionControl);
                this.MiEleccion.Etapas[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                this.MiEleccion.Etapas[fila.RowIndex].FechaDesde = txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(txtFechaDesde.Text);
                this.MiEleccion.Etapas[fila.RowIndex].FechaHasta = txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(txtFechaHasta.Text);
                this.MiEleccion.Etapas[fila.RowIndex].EstadoColeccion = EstadoColecciones.Modificado;
                this.MiEleccion.Etapas[fila.RowIndex].Etapa = hdfEtapa.Value;
                this.MiEleccion.Etapas[fila.RowIndex].IdEtapa = Convert.ToInt32(hdfIdEtapa.Value);
            }
        }
        #endregion
    }
}