using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;
using Tesorerias;
using Bancos.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using IU.Modulos.Comunes;
using System.Data;
using Generales.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class TesoreriasMovimientosTransferencias : PaginaTesoreria
    {
        private List<TESTesoreriasMovimientos> MisTesoreriaMovimientos
        {
            get { return (List<TESTesoreriasMovimientos>)Session[this.MiSessionPagina + "TesoreriasMovimientosTransferenciasMisTesoreriaMovimientos"]; }
            set { Session[this.MiSessionPagina + "TesoreriasMovimientosTransferenciasMisTesoreriaMovimientos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //TESTesorerias tesoreria = new TESTesorerias();
                //tesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                //tesoreria.FechaAbrir = DateTime.Now;
                //tesoreria.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;

                //this.btnAgregar.Visible = this.ValidarPermiso("RequisicionesAgregar.aspx");
                //this.btnAgregar.Visible = this.ValidarPermiso("TesoreriasMovimientosAgregar.aspx");
                //Se utiliza para persistir los parametros de busqueda ingresados por el usuario
                //TESTesorerias parametros = this.BusquedaParametrosObtenerValor<TESTesorerias>();
                //if (parametros.BusquedaParametros)
                //{
                //}

                this.CargarLista(this.MiTesoreria);
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Modificar" || e.CommandName == "Anular"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indice = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESTesoreriasMovimientos movimiento = this.MisTesoreriaMovimientos[indice];

            if (e.CommandName == "Modificar")
            {
                movimiento.Estado = TGEGeneralesF.TGEEstadosObtener(new TGEEstados() { IdEstado = (int)EstadosTesoreriasMovimientos.Activo });
                movimiento.EstadoColeccion = EstadoColecciones.Agregado;
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MiTesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                FechaCajaContable ctrFecha = (FechaCajaContable)this.gvDatos.Rows[index].FindControl("ctrFechaCajaContable");
                movimiento.Fecha = ctrFecha.dFechaCajaContable.Value;
                
                //Falta agregar el control de fecha y ponerlo en el movimiento!
                if (TesoreriasF.TesoreriaMovimientoAgregarTransferenciaBancos(movimiento))
                {
                    this.MostrarMensaje(movimiento.CodigoMensaje, false);
                    this.CargarLista(this.MiTesoreria);
                    this.MiTesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(this.MiTesoreria);
                    ((nmpTesorerias)this.Master).ActualizarGrilla(this.MiTesoreria);
                }
                else
                {
                    this.MostrarMensaje(movimiento.CodigoMensaje, true, movimiento.CodigoMensajeArgs);
                    if (movimiento.dsResultado != null)
                    {
                        this.ctrPopUpGrilla.IniciarControl(movimiento);
                        movimiento.dsResultado = null;
                    }
                }
            }
            if (e.CommandName == "Anular")
            {
                movimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Baja;
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                MiTesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);

                //Anular --> Anulo el traspaso del cheque y me queda en mi tesoreria nuevamente. 
                if (TesoreriasF.TesoreriaMovimientoAnularTraspasoChequess(movimiento))
                {
                    MostrarMensaje(movimiento.CodigoMensaje, false);
                    CargarLista(MiTesoreria);
                    MiTesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(MiTesoreria);
                    ((nmpTesorerias)Master).ActualizarGrilla(MiTesoreria);
                }
                else
                {
                    MostrarMensaje(movimiento.CodigoMensaje, true, movimiento.CodigoMensajeArgs);
                    if (movimiento.dsResultado != null)
                    {
                        ctrPopUpGrilla.IniciarControl(movimiento);
                        movimiento.dsResultado = null;
                    }
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TESTesoreriasMovimientos movimiento = (TESTesoreriasMovimientos)e.Row.DataItem;

                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnModificar.Visible = this.ValidarPermiso("TesoreriasMovimientosAgregar.aspx");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                string funcionAnular = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAnular"));
                ibtnModificar.Attributes.Add("OnClick", funcion);
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ibtnAnular.Attributes.Add("OnClick", funcionAnular);

                FechaCajaContable ctrFecha = (FechaCajaContable)e.Row.FindControl("ctrFechaCajaContable");
                ctrFecha.IniciarControl(Gestion.Agregar, movimiento.Fecha, this.MiTesoreria.FechaAbrir);

                if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TraspasoChequesFilial
                    && movimiento.Estado.IdEstado == (int)EstadosTesoreriasMovimientos.Traspaso
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.ExtraccionEfectivoParaFilial
                    && movimiento.Estado.IdEstado == (int)EstadosTesoreriasMovimientos.Activo
                    )
                {
                    ibtnModificar.Visible = false;
                    ibtnAnular.Visible = true;
                } 
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblOperativo = (Label)e.Row.FindControl("lblGrillaTotalRegistros");
                lblOperativo.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisTesoreriaMovimientos.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESTesorerias parametros = this.BusquedaParametrosObtenerValor<TESTesorerias>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESTesorerias>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<TESTesoreriasMovimientos>(this.MisTesoreriaMovimientos, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MiTesoreria.TesoreriasMovimientos = this.OrdenarGrillaDatos<TESTesoreriasMovimientos>(this.MiTesoreria.ObtenerTesoreriasMovimientos(), e);
            //this.gvDatos.DataSource = this.MiTesoreria.TesoreriasMovimientos;
            //this.gvDatos.DataBind();
        }

        private void CargarLista(TESTesorerias pTesoreria)
        {
            //pTesoreria.BusquedaParametros = true;
            //this.BusquedaParametrosGuardarValor<TESTesorerias>(pTesoreria);
            //this.MisTesoreria = TesTesoreriasF.TesoreriasObtenerDatosCompletos(pTesoreria);
            //this.gvDatos.DataSource = this.MisTesoreria.TesoreriasMonedas[0].TesoreriasMovimientos;
            this.MisTesoreriaMovimientos = TesoreriasF.TesoreriasMovimientosObtenerPendientesTransferencia(this.MiTesoreria);
            AyudaProgramacion.CargarGrillaListas<TESTesoreriasMovimientos>(this.MisTesoreriaMovimientos, false, this.gvDatos, true);

        }
    }
}
