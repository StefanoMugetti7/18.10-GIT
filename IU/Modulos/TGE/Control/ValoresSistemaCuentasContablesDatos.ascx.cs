using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Contabilidad.Entidades;
using Contabilidad;

namespace IU.Modulos.TGE.Control
{
    public partial class ValoresSistemaCuentasContablesDatos : ControlesSeguros
    {

        private TGEListasValoresSistemasDetallesCuentasContables MiDetalleSysCta
        {
            get { return (TGEListasValoresSistemasDetallesCuentasContables)Session[this.MiSessionPagina + "ValoresSistemaCuentasDatosMiDetalleSysCta"]; }
            set { Session[this.MiSessionPagina + "ValoresSistemaCuentasDatosMiDetalleSysCta"] = value; }
        }

        private List<TGEListasValoresSistemasDetallesCuentasContables> MiListaDetalleSysCta
        {
            get { return (List<TGEListasValoresSistemasDetallesCuentasContables>)Session[this.MiSessionPagina + "ValoresSistemaCuentasDatosMiListaDetalleSysCta"]; }
            set { Session[this.MiSessionPagina + "ValoresSistemaCuentasDatosMiListaDetalleSysCta"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "ValoresSistemaCuentasDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "ValoresSistemaCuentasDatosMiIndiceDetalleModificar"] = value; }
        }
        public delegate void ValoresSistemasCuentasContablesDatosAceptarEventHandler(object sender, TGEListasValoresSistemasDetallesCuentasContables e);
        public event ValoresSistemasCuentasContablesDatosAceptarEventHandler ValoresSistemasCuentasContablesDatosAceptar;
        public delegate void ValoresSistemasCuentasContablesDatosCancelarEventHandler();
        public event ValoresSistemasCuentasContablesDatosCancelarEventHandler ValoresSistemasCuentasContablesDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //ctrDatosPopUp.ListasValoresDetallesPopUpAceptar += new ListasValoresDetallesDatosPopUp.ListasValoresDetallesDatosPopUpAceptarEventHandler(ctrDatosPopUp_ListasValoresDetallesPopUpAceptar);
            popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrCuentasContables.CuentasContablesBuscarSeleccionar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(ctrCuentasContables_CuentasContablesBuscarSeleccionar);
            this.ctrCuentasContables.CuentasContablesBuscarIniciar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(ctrCuentasContables_CuentasContablesBuscarIniciar);
        }

        void ctrCuentasContables_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        {
            AyudaProgramacion.MatchObjectProperties(ContabilidadF.EjerciciosContablesObtenerUltimoActivo(), ejercicio);
        }

        internal void IniciarControl(TGEListasValoresSistemasDetallesCuentasContables pValorCuenta, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiDetalleSysCta = pValorCuenta;
            this.CargarCombo();
            switch (pGestion)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlListasValores, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    this.ddlEstados.Enabled = true;
                    break;
                case Gestion.Consultar:
                    this.MiDetalleSysCta = ContabilidadF.ValoresSistemasCuentasContablesObtenerDatosCompletos(this.MiDetalleSysCta);
                    this.MapearObjetoAControles(this.MiDetalleSysCta);
                    this.ddlListasValores.Enabled = false;
                    this.ddlListaValorDetalles.Enabled = false;
                    this.btnAceptar.Visible = false;

                    break;
                case Gestion.Modificar:
                    this.MiDetalleSysCta = ContabilidadF.ValoresSistemasCuentasContablesObtenerDatosCompletos(this.MiDetalleSysCta);
                    this.ddlEstados.Enabled = true;
                    this.MapearObjetoAControles(this.MiDetalleSysCta);
                    break;
                default:
                    break;
            }
        }

        void CargarCombo()
        {
            TGETiposFuncionalidades tipoFuncionalidad = new TGETiposFuncionalidades();
            tipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            this.ddlListasValores.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerPorTipoFuncionalidad(tipoFuncionalidad);
            this.ddlListasValores.DataValueField = "IdListaValorSistema";
            this.ddlListasValores.DataTextField = "ListaValor";
            this.ddlListasValores.DataBind();

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
        }

        void ctrCuentasContables_CuentasContablesBuscarSeleccionar(global::Contabilidad.Entidades.CtbCuentasContables e, int indiceColeccion)
        {
            this.MiDetalleSysCta.CuentaContable = e;
            this.upCuentasContables.Update();
        }

        protected void ddlListasValores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlListasValores.SelectedValue))
                return;

            TGEListasValoresSistemas listaValorSys = new TGEListasValoresSistemas();
            listaValorSys.IdListaValorSistema = Convert.ToInt32(this.ddlListasValores.SelectedValue);

            this.ddlListaValorDetalles.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(listaValorSys); ;
            this.ddlListaValorDetalles.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlListaValorDetalles.DataTextField = "Descripcion";
            this.ddlListaValorDetalles.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlListaValorDetalles, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            MiListaDetalleSysCta = ContabilidadF.ValoresSistemasCuentasContablesObtenerPorIdListaValor(listaValorSys);
            //Cargo y muestro la grilla SOLO SI HAY ELEMENTOS PARA MOSTRAR
            if (MiListaDetalleSysCta.Count() != 0)
            {
                AyudaProgramacion.CargarGrillaListas<TGEListasValoresSistemasDetallesCuentasContables>(this.MiListaDetalleSysCta, false, this.gvDatos, true);
                //es para cargar la grilla con las relaciones ya existentes de esa LISTA VALOR SISTEMA
            }
            
            //MUESTRO LOS PANELES OCULTOS 
            this.pnlCuentasContables.Visible = true;
            this.ddlListaValorDetalles.Enabled = true;
            this.upListaDetalle.Update();
            //this.upCuentasContables.Update();
        }

        #region GRILLA RELACION VALORES CUENTAS

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiListaDetalleSysCta.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiListaDetalleSysCta = AyudaProgramacion.AcomodarIndices<TGEListasValoresSistemasDetallesCuentasContables>(this.MiListaDetalleSysCta);
                AyudaProgramacion.CargarGrillaListas<TGEListasValoresSistemasDetallesCuentasContables>(this.MiListaDetalleSysCta, false, this.gvDatos, true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                btnEliminar.Visible = false;
                //switch (GestionControl)
                //{
                //    case Gestion.Agregar:
                //        btnEliminar.Visible = false;
                //        break;
                //    case Gestion.Modificar:
                //        btnEliminar.Visible = true;
                //        break;
                //    case Gestion.Consultar:
                //        btnEliminar.Visible = false;
                //        break;
                //    default:
                //        break;
                //}
            } 
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    int cellCount = e.Row.Cells.Count;
                    e.Row.Cells.Clear();
                    TableCell tableCell = new TableCell();
                    tableCell.ColumnSpan = cellCount;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiListaDetalleSysCta.Count);
                    e.Row.Cells.Add(tableCell);
                }

            
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEListasValoresSistemasDetallesCuentasContables parametros = this.BusquedaParametrosObtenerValor<TGEListasValoresSistemasDetallesCuentasContables>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEListasValoresSistemasDetallesCuentasContables>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiListaDetalleSysCta;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiListaDetalleSysCta = this.OrdenarGrillaDatos<TGEListasValoresSistemasDetallesCuentasContables>(this.MiListaDetalleSysCta, e);
            this.gvDatos.DataSource = this.MiListaDetalleSysCta;
            this.gvDatos.DataBind();
        }


        #endregion

        #region Mapeos
        private void MapearControlesObjeto()
        {
            this.MiDetalleSysCta.ListaValorSistemaDetalle.IdListaValorSistemaDetalle = Convert.ToInt32(this.ddlListaValorDetalles.SelectedValue);
            //this.MiDetalleSysCta.CuentaContable.IdCuentaContable = this.txtIdCuentaContable.Text;
            this.MiDetalleSysCta.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
        }

        private void MapearObjetoAControles(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            this.ddlListasValores.SelectedValue = pParametro.ListaValorSistemaDetalle.ListaValorSistema.IdListaValorSistema.ToString();
            //Cargo el ddl de lista valores
            this.ddlListaValorDetalles.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(pParametro.ListaValorSistemaDetalle.ListaValorSistema); ;
            this.ddlListaValorDetalles.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlListaValorDetalles.DataTextField = "Descripcion";
            this.ddlListaValorDetalles.DataBind();
            this.ddlListaValorDetalles.SelectedValue = pParametro.ListaValorSistemaDetalle.IdListaValorSistemaDetalle.ToString();
            
            //Bloqueo de datos
            this.ddlListasValores.Enabled = false;
            this.ddlListaValorDetalles.Enabled = false;
            this.pnlCuentasContables.Visible = true;

            //Estado
            this.ddlEstados.SelectedValue = MiDetalleSysCta.Estado.IdEstado.ToString();
            this.ctrCuentasContables.MapearObjetoControles(pParametro.CuentaContable, this.GestionControl, 0);
            //Obtengo la lista de relaciones segun la lista valor
            MiListaDetalleSysCta = ContabilidadF.ValoresSistemasCuentasContablesObtenerPorIdListaValor(pParametro.ListaValorSistemaDetalle.ListaValorSistema);
            //Cargo y muestro la grilla SOLO SI HAY ELEMENTOS PARA MOSTRAR
            if (MiListaDetalleSysCta.Count() != 0)
            {
                AyudaProgramacion.CargarGrillaListas<TGEListasValoresSistemasDetallesCuentasContables>(this.MiListaDetalleSysCta, false, this.gvDatos, true);
                //es para cargar la grilla con las relaciones ya existentes de esa LISTA VALOR SISTEMA
            }

            this.ctrAuditoria.IniciarControl(pParametro);
        }
        #endregion

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = false;
            this.MapearControlesObjeto();
            this.MiDetalleSysCta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.ValoresSistemasCuentasContablesAgregar(this.MiDetalleSysCta);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.ValoresSistemasCuentasContablesModificar(this.MiDetalleSysCta);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiDetalleSysCta.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiDetalleSysCta.CodigoMensaje, true, this.MiDetalleSysCta.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ValoresSistemasCuentasContablesDatosCancelar != null)
                this.ValoresSistemasCuentasContablesDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ValoresSistemasCuentasContablesDatosAceptar != null)
                this.ValoresSistemasCuentasContablesDatosAceptar(null, this.MiDetalleSysCta);
        }

        
    }
}