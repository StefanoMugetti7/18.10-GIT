using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Contabilidad;
using Evol.Controls;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class CentrosCostosProrrateosDatos : ControlesSeguros
    {
        private CtbCentrosCostosProrrateos MiCentroCosto
        {
            get { return (CtbCentrosCostosProrrateos)Session[this.MiSessionPagina + "CentrosCostosProrrateosDatosMiCentroCosto"]; }
            set { Session[this.MiSessionPagina + "CentrosCostosProrrateosDatosMiCentroCosto"] = value; }
        }

        private List<TGEListasValoresDetalles> MisCentrosCostos
        {
            get { return (List<TGEListasValoresDetalles>)Session[this.MiSessionPagina + "CentrosCostosProrrateosDatosMisCentrosCostos"]; }
            set { Session[this.MiSessionPagina + "CentrosCostosProrrateosDatosMisCentrosCostos"] = value; }
        }

        public delegate void AsientoContableDatosAceptarEventHandler(object sender, CtbCentrosCostosProrrateos e);
        public event AsientoContableDatosAceptarEventHandler ControlDatosAceptar;

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //if (this.MiCentroCosto == null && this.GestionControl != Gestion.Agregar)
                //{
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                //}
            }
            else
            {
                if (this.MiCentroCosto != null)
                    this.PersistirDatosGrilla();
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Cuenta Modelo
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbCentrosCostosProrrateos pCentroCostoProrrateo, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MisCentrosCostos = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CentrosCostos);
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCentroCosto = pCentroCostoProrrateo;
                    this.txtCentrosCostosProrrateo.Text = this.MiCentroCosto.CentroCostoProrrateo;
                    this.AgregarCuentas(2);
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    AyudaProgramacion.CargarGrillaListas(this.MiCentroCosto.CentrosCostosProrrateosDetalles, false, this.gvDatos, true);
                    break;                
                case Gestion.Modificar:
                    this.MiCentroCosto = ContabilidadF.CentrosCostosProrrateosObtenerDatosCompletos(pCentroCostoProrrateo);
                    this.MapearObjetoAControles(this.MiCentroCosto);
                    break;
                case Gestion.Consultar:
                    this.MiCentroCosto = ContabilidadF.CentrosCostosProrrateosObtenerDatosCompletos(pCentroCostoProrrateo);
                    this.MapearObjetoAControles(this.MiCentroCosto);
                    this.btnAgregarDetalle.Visible = false;
                    this.txtCentrosCostosProrrateo.Enabled = false;
                    this.ddlFiliales.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiCentroCosto);

            this.MiCentroCosto.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.CentrosCostosProrrateosAgregar(this.MiCentroCosto);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.CentrosCostosProrrateosModificar(this.MiCentroCosto);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                if (this.ControlDatosAceptar != null)
                    this.ControlDatosAceptar(null, this.MiCentroCosto);
                else
                    this.MostrarMensaje(this.MiCentroCosto.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(this.MiCentroCosto.CodigoMensaje, true, this.MiCentroCosto.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlDatosCancelar != null)
                this.ControlDatosCancelar();
        }

        protected void btnAgregarCuenta_Click(object sender, EventArgs e)
        {
            this.AgregarCuentas(1);
            AyudaProgramacion.CargarGrillaListas(this.MiCentroCosto.CentrosCostosProrrateosDetalles, false, this.gvDatos, true);
            this.upDetalleCentrosCostos.Update();
        }

        private void AgregarCuentas(int pCantidad)
        {
            CtbCentrosCostosProrrateosDetalles detalle;
            for (int i = 0; i < pCantidad; i++)
            {
                detalle = new CtbCentrosCostosProrrateosDetalles();
                detalle.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiCentroCosto.CentrosCostosProrrateosDetalles.Add(detalle);
                detalle.IndiceColeccion = this.MiCentroCosto.CentrosCostosProrrateosDetalles.IndexOf(detalle);
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;
            if (e.CommandArgument.ToString() != string.Empty)
            {
                int index = Convert.ToInt32(e.CommandArgument);
                //this.MiPosicion = index;
                int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
                if (e.CommandName == "Borrar")
                {
                    this.MiCentroCosto.CentrosCostosProrrateosDetalles.RemoveAt(indiceColeccion);
                    AyudaProgramacion.AcomodarIndices<CtbCentrosCostosProrrateosDetalles>(this.MiCentroCosto.CentrosCostosProrrateosDetalles);
                    AyudaProgramacion.CargarGrillaListas(this.MiCentroCosto.CentrosCostosProrrateosDetalles, false, this.gvDatos, true);
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CtbCentrosCostosProrrateosDetalles centroCostoProrrateoDetalle = (CtbCentrosCostosProrrateosDetalles)e.Row.DataItem;
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                CurrencyTextBox txtporC = (CurrencyTextBox)e.Row.FindControl("txtPorcentaje");
                txtporC.Attributes.Add("onchange", "CentrosCostosCalcularItem();");
                string mensaje = this.ObtenerMensajeSistema("ConfirmarBaja");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnEliminar.Attributes.Add("OnClick", funcion);
                DropDownList ddlCentrosCostos = (DropDownList)e.Row.FindControl("ddlCentrosCostos");
                ddlCentrosCostos.DataSource = this.MisCentrosCostos;
                ddlCentrosCostos.DataValueField = "IdListaValorDetalle";
                ddlCentrosCostos.DataTextField = "Descripcion";
                ddlCentrosCostos.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlCentrosCostos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                if (centroCostoProrrateoDetalle.IdCentroCosto > 0)
                {
                    ListItem item = ddlCentrosCostos.Items.FindByValue(centroCostoProrrateoDetalle.IdCentroCosto.ToString());
                    if (item != null)
                        ddlCentrosCostos.Items.Add(new ListItem(centroCostoProrrateoDetalle.CentroCosto, centroCostoProrrateoDetalle.IdCentroCosto.ToString()));
                    ddlCentrosCostos.SelectedValue = centroCostoProrrateoDetalle.IdCentroCosto.ToString();
                }
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Consultar:
                        break;
                    case Gestion.Modificar:
                        btnEliminar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                Label lblPorcentajeTotal = (Label)e.Row.FindControl("lblPorcentajeTotal");
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiCentroCosto.CentrosCostosProrrateosDetalles.Count);
                lblPorcentajeTotal.Text = this.MiCentroCosto.CentrosCostosProrrateosDetalles.Sum(x=>x.Porcentaje).ToString("N0");
            }
        }

        private void MapearObjetoAControles(CtbCentrosCostosProrrateos pCentroCostoProrrateo)
        {
            this.txtCentrosCostosProrrateo.Text = pCentroCostoProrrateo.CentroCostoProrrateo;
            ListItem item = this.ddlFiliales.Items.FindByValue(pCentroCostoProrrateo.Filial.IdFilial.ToString());
            if (item != null)
                this.ddlFiliales.Items.Add(new ListItem(pCentroCostoProrrateo.Filial.Filial, pCentroCostoProrrateo.Filial.IdFilial.ToString()));
            this.ddlFiliales.SelectedValue = pCentroCostoProrrateo.Filial.IdFilial.ToString();
            this.ddlEstado.SelectedValue = pCentroCostoProrrateo.Estado.IdEstado.ToString();

            AyudaProgramacion.CargarGrillaListas(this.MiCentroCosto.CentrosCostosProrrateosDetalles, false, this.gvDatos, true);
            //this.ctrComentarios.IniciarControl(pAsientoContable, this.GestionControl);
            //this.ctrArchivos.IniciarControl(pAsientoContable, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pCentroCostoProrrateo);
        }

        private void MapearControlesAObjeto(CtbCentrosCostosProrrateos pCentroCostoProrrateo)
        {
            pCentroCostoProrrateo.CentroCostoProrrateo = this.txtCentrosCostosProrrateo.Text;
            pCentroCostoProrrateo.Filial.IdFilial = Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pCentroCostoProrrateo.Filial.Filial = this.ddlFiliales.SelectedItem.Text;
            pCentroCostoProrrateo.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        private void PersistirDatosGrilla()
        {
            if (this.MiCentroCosto.CentrosCostosProrrateosDetalles.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                DropDownList ddlCentrosCostos = ((DropDownList)fila.FindControl("ddlCentrosCostos"));
                decimal porcDesc = ((CurrencyTextBox)fila.FindControl("txtPorcentaje")).Decimal;
                if (ddlCentrosCostos.SelectedValue != string.Empty)
                {
                    this.MiCentroCosto.CentrosCostosProrrateosDetalles[fila.RowIndex].IdCentroCosto = Convert.ToInt32(ddlCentrosCostos.SelectedValue);
                    this.MiCentroCosto.CentrosCostosProrrateosDetalles[fila.RowIndex].Porcentaje = porcDesc;
                    this.MiCentroCosto.CentrosCostosProrrateosDetalles[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiCentroCosto.CentrosCostosProrrateosDetalles[fila.RowIndex], GestionControl);
                    this.MiCentroCosto.CentrosCostosProrrateosDetalles[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                }
                else
                {
                    this.MiCentroCosto.CentrosCostosProrrateosDetalles[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiCentroCosto.CentrosCostosProrrateosDetalles[fila.RowIndex], Gestion.Anular);
                    this.MiCentroCosto.CentrosCostosProrrateosDetalles[fila.RowIndex].Estado.IdEstado = (int)Estados.Baja;
                }
            }
        }
    }
}