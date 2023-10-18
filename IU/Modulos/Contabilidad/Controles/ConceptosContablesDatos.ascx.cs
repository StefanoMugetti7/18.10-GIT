using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Contabilidad;
using Generales.Entidades;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class ConceptosContablesDatos : ControlesSeguros
    {
        private CtbConceptosContables MiConceptosContables
        {
            get { return (CtbConceptosContables)Session[this.MiSessionPagina + "MiConceptosContables"]; }
            set { Session[this.MiSessionPagina + "MiConceptosContables"] = value; }
        }

        private CtbCuentasContables MiCuentaContable
        {
            get { return (CtbCuentasContables)Session[this.MiSessionPagina + "MiCuentaContable"]; }
            set { Session[this.MiSessionPagina + "MiCuentaContable"] = value; }
        }

        private List<TGETiposOperaciones> MisTiposOperaciones
        {
            get { return (List<TGETiposOperaciones>)Session[this.MiSessionPagina + "MisTiposOperaciones"]; }
            set { Session[this.MiSessionPagina + "MisTiposOperaciones"] = value; }
        }

        public delegate void ConceptosContablesDatosAceptarEventHandler(object sender, CtbConceptosContables e);
        public event ConceptosContablesDatosAceptarEventHandler ConceptosContablesDatosAceptar;

        public delegate void ConceptosContablesDatosCancelarEventHandler();
        public event ConceptosContablesDatosCancelarEventHandler ConceptosContablesDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.buscarCuenta.CuentasContablesBuscarSeleccionar+=new CuentasContablesBuscar.CuentasContablesBuscarEventHandler(buscarCuenta_CuentasContablesBuscarSeleccionar);
            this.buscarCuenta.CuentasContablesBuscarIniciar += new CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(buscarCuenta_CuentasContablesBuscarIniciar);
            if (!this.IsPostBack)
            {
                if (this.MiConceptosContables == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        void buscarCuenta_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        {
            AyudaProgramacion.MatchObjectProperties(ContabilidadF.EjerciciosContablesObtenerUltimoActivo(), ejercicio);
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Moneda
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbConceptosContables pConceptoContable, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            TGETiposOperaciones filtro = new TGETiposOperaciones();
            filtro.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            this.MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(filtro);            
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiConceptosContables = pConceptoContable;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    break;
                case Gestion.Modificar:
                    this.MiConceptosContables = ContabilidadF.ConceptosContablesObtenerDatosCompletos(pConceptoContable);
                    this.MapearObjetoAControles(this.MiConceptosContables);
                    break;
                case Gestion.Consultar:
                    this.MiConceptosContables = ContabilidadF.ConceptosContablesObtenerDatosCompletos((pConceptoContable));
                    this.MapearObjetoAControles(this.MiConceptosContables);
                    this.txtConceptoContable.Enabled = false;
                    this.buscarCuenta.Enable(false);
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
            this.CargarCombos();
            AyudaProgramacion.CargarGrillaListas(this.MiConceptosContables.TiposOperaciones, true, this.gvDatos, true);
        }

        private void MapearObjetoAControles(CtbConceptosContables pConceptoContable)
        {
            this.txtConceptoContable.Text = pConceptoContable.ConceptoContable;
            this.buscarCuenta.MapearObjetoControles(pConceptoContable.CuentaContable, this.GestionControl, 0);
            this.ddlEstado.SelectedValue = pConceptoContable.Estado.IdEstado.ToString();
        }

        private void MapearControlesAObjeto(CtbConceptosContables pConceptoContable)
        {
            pConceptoContable.ConceptoContable = this.txtConceptoContable.Text;
            if (this.MiCuentaContable != null)
                pConceptoContable.CuentaContable = this.MiCuentaContable;
            pConceptoContable.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        }

        private void CargarCombos()
        {
            this.CargarComboTipoOperacion();

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }

        private void CargarComboTipoOperacion()
        {
            var aux = new List<TGETiposOperaciones>();
            foreach (var tipoOperacion in this.MisTiposOperaciones)
            {
                if (!this.MiConceptosContables.TiposOperaciones.Exists(x => x.IdTipoOperacion == tipoOperacion.IdTipoOperacion))
                    aux.Add(tipoOperacion);
            }
            this.ddlTipoOperacion.DataSource = aux;
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiConceptosContables);
            this.MiConceptosContables.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.ConceptosContablesAgregar(this.MiConceptosContables);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.ConceptosContablesModificar(this.MiConceptosContables);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiConceptosContables.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiConceptosContables.CodigoMensaje, true, this.MiConceptosContables.CodigoMensajeArgs);
            }
        }

        protected void buscarCuenta_CuentasContablesBuscarSeleccionar(CtbCuentasContables pCuentaContable, int indiceColeccion)
        {
            this.MiCuentaContable = pCuentaContable;
            this.upCuentaContable.Update();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ConceptosContablesDatosCancelar != null)
                this.ConceptosContablesDatosCancelar();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            var tipoOperacion = this.MisTiposOperaciones.FirstOrDefault(x => x.IdTipoOperacion == Convert.ToInt32(this.ddlTipoOperacion.SelectedValue));
            if (tipoOperacion != null)
            {
                tipoOperacion.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiConceptosContables.TiposOperaciones.Add(tipoOperacion);
                this.MisTiposOperaciones.Remove(tipoOperacion);
                AyudaProgramacion.CargarGrillaListas(this.MiConceptosContables.TiposOperaciones, true, this.gvDatos, true);
                this.CargarComboTipoOperacion();
            }
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ConceptosContablesDatosAceptar != null)
                this.ConceptosContablesDatosAceptar(null, this.MiConceptosContables);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;
            if (e.CommandArgument.ToString() != string.Empty)
            {
                if (e.CommandName == "Borrar")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    HiddenField tipoOperacionEliminar = gvDatos.Rows[index].FindControl("hIdTipoOperacion") as HiddenField;
                    var tipoOperacion = this.MiConceptosContables.TiposOperaciones.FirstOrDefault(x => x.IdTipoOperacion == Convert.ToInt32(tipoOperacionEliminar.Value));
                    if (tipoOperacion.EstadoColeccion == EstadoColecciones.Agregado)
                    {
                        this.MiConceptosContables.TiposOperaciones.Remove(tipoOperacion);
                    }
                    else
                    {
                        tipoOperacion.EstadoColeccion = EstadoColecciones.Borrado;
                    }
                    AyudaProgramacion.CargarGrillaListas(this.MiConceptosContables.TiposOperaciones, true, this.gvDatos, true);
                    this.MisTiposOperaciones.Add(tipoOperacion);
                    this.MisTiposOperaciones.Sort((x,y)=> x.TipoOperacion.CompareTo(y.TipoOperacion));
                    this.CargarComboTipoOperacion();
                }
            }            
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                string mensaje = this.ObtenerMensajeSistema("TiposOperacionConfirmarBaja");
                mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnEliminar.Attributes.Add("OnClick", funcion);
                if (this.GestionControl == Gestion.Consultar)
                    btnEliminar.Visible = false;
                else
                    btnEliminar.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                var aux = new List<TGETiposOperaciones>();
                foreach (var tipoOperacionAux in this.MiConceptosContables.TiposOperaciones)
                {
                    if (tipoOperacionAux.EstadoColeccion != EstadoColecciones.Borrado)
                        aux.Add(tipoOperacionAux);
                }
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), aux.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiConceptosContables.TiposOperaciones;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiConceptosContables.TiposOperaciones = this.OrdenarGrillaDatos<TGETiposOperaciones>(this.MiConceptosContables.TiposOperaciones, e);
            this.gvDatos.DataSource = this.MiConceptosContables.TiposOperaciones;
            this.gvDatos.DataBind();
        }
    }
}