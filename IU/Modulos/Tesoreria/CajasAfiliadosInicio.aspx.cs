using Cargos;
using Cargos.Entidades;
using Comunes.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasAfiliadosInicio : PaginaCajasAfiliados
    {
        private DataTable MisCargosAfiliadosMensuales
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisCargosAfiliadosMensuales"]; }
            set { Session[this.MiSessionPagina + "MisCargosAfiliadosMensuales"] = value; }
        }
        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            base.PageLoadEventCajas(sender, e);
            if (!this.IsPostBack)
            {
                if (!this.MiAfiliado.MostrarMensajesAlertas)
                {
                    this.ctrMensajesAlertas.IniciarControl(this.MiAfiliado);
                }
                this.CargarCargosMensuales();
            }
        }
        private void CargarCargosMensuales()
        {
            CarCuentasCorrientes afiFiltro = new CarCuentasCorrientes();
            afiFiltro.IdAfiliado = this.MiAfiliado.IdAfiliado;
            afiFiltro.Estado.IdEstado = (int)EstadosCuentasCorrientes.Pendiente;


            afiFiltro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CarCuentasCorrientes>(afiFiltro);
            this.MisCargosAfiliadosMensuales = CargosF.CuentasCorrientesObtenerDT(afiFiltro);
            this.gvCargosMensuales.DataSource = this.MisCargosAfiliadosMensuales;
            this.gvCargosMensuales.DataBind();
           
        }
        //private void CargarLista()
        //{
        //    CarTiposCargosAfiliadosFormasCobros afiFiltro = new CarTiposCargosAfiliadosFormasCobros();

        //    afiFiltro.IdAfiliado = this.MiAfiliado.IdAfiliado;
        //    afiFiltro.Estado.IdEstado = (int)EstadosCuentasCorrientes.Pendiente;


        //    this.MisCargosAfiliadosAutomatico = CargosF.TiposCargosAfiliadosObtenerLista(afiFiltro);

        //    AyudaProgramacion.CargarGrillaListas<CarTiposCargosAfiliadosFormasCobros>(afiFiltro, false, this.gvCargosMensuales, true);

        //    afiFiltro.IdAfiliado = this.MiAfiliado.IdAfiliado;
        //    afiFiltro.Estado.IdEstado = (int)EstadosTodos.Todos;
        //    afiFiltro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Administrable;
        //    List<CarTiposCargosAfiliadosFormasCobros> lista = CargosF.TiposCargosAfiliadosObtenerLista(afiFiltro);
        //    afiFiltro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Bonificacion;
        //    lista.AddRange(CargosF.TiposCargosAfiliadosObtenerLista(afiFiltro));
        //    lista = lista.OrderByDescending(x => x.FechaAlta).ThenByDescending(x => x.IdTipoCargoAfiliadoFormaCobro).ToList();


        //    this.MisCuotasPendientes = CargosF.CarTiposCargosAfiliadosFormasCobrosObtenerCuotasPendientes(afiFiltro);
        //    this.gvCuotasPendientes.DataSource = this.MisCuotasPendientes;
        //    this.gvCuotasPendientes.DataBind();
        //}
        protected void gvCargosMensuales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (!(e.CommandName == "Modificar"
            //    || e.CommandName == "Auditoria"
            //    || e.CommandName == "Consultar"
            //    || e.CommandName == "DesimputarCobro"
            //    || e.CommandName == "RevertirCobro"))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int idCuentaCorriente = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            // DataRow fila = this.MisCargosAfiliadosMensuales.Rows.Find(index);


            //switch (e.CommandName)
            //{
            //    case "Modificar":
            //        this.MisParametrosUrl = new Hashtable();
            //        this.MisParametrosUrl.Add("IdCuentaCorriente", idCuentaCorriente);
            //        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosModificarFomasCobros.aspx"), true);
            //        break;
            //    //case "Auditoria":
            //    //    cargoAfiliado = CargosF.CuentasCorrientesObtenerDatosCompletos(cargoAfiliado);
            //    //    this.ctrAuditoria.IniciarControl(cargoAfiliado);
            //    //    break;
            //    case "Consultar":
            //        this.MisParametrosUrl = new Hashtable();
            //        this.MisParametrosUrl.Add("IdCuentaCorriente", idCuentaCorriente);
            //        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosConsultarFomasCobros.aspx"), true);
            //        break;
            //    case "DesimputarCobro":
            //        this.MisParametrosUrl = new Hashtable();
            //        this.MisParametrosUrl.Add("IdCuentaCorriente", idCuentaCorriente);
            //        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosDesimputarCobro.aspx"), true);
            //        break;
            //    case "RevertirCobro":
            //        this.MisParametrosUrl = new Hashtable();
            //        this.MisParametrosUrl.Add("IdCuentaCorriente", idCuentaCorriente);
            //        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosRevertirCobro.aspx"), true);
            //        break;
            //    default:
            //        break;
            //}
        }

        protected void gvCargosMensuales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = Convert.ToInt32(e.Row.RowIndex);
                //CarCuentasCorrientes item = (CarCuentasCorrientes)e.Row.DataItem;
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ImageButton ibtnAuditoria = (ImageButton)e.Row.FindControl("btnAuditoria");
                //ibtnAuditoria.Visible = this.ValidarPermiso("AuditoriaDatos");
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnDesimputarCobro = (ImageButton)e.Row.FindControl("btnDesimputarCobro");
                ImageButton ibtnRevertirCobro = (ImageButton)e.Row.FindControl("btnRevertirCobro");

                //ibtnConsultar.Visible = this.ValidarPermiso("CargosAfiliadosConsultarFomasCobros.aspx");
                //int idEstado = (int)EstadosCuentasCorrientes.Pendiente;
                //switch (idEstado)
                //{
                //    case (int)EstadosCuentasCorrientes.EnviadoAlCobro:
                //        ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificarFomasCobrosEnviadoAlCobro.aspx");
                //        break;
                //    case (int)EstadosCuentasCorrientes.Activo:
                //    case (int)EstadosCuentasCorrientes.Pendiente:
                //    case (int)EstadosCuentasCorrientes.CobroDevuelto:
                //    case (int)EstadosCuentasCorrientes.Rechazado:
                //        ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificarFomasCobros.aspx");
                //        break;
                //    case (int)EstadosCuentasCorrientes.CobroParcial:
                //        ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificarFomasCobros.aspx");
                //        ibtnDesimputarCobro.Visible = this.ValidarPermiso("CargosAfiliadosDesimputarCobro.aspx");
                //        ibtnRevertirCobro.Visible = this.ValidarPermiso("CargosAfiliadosRevertirCobro.aspx");
                //        break;
                //    case (int)EstadosCuentasCorrientes.Cobrado:
                //        ibtnDesimputarCobro.Visible = this.ValidarPermiso("CargosAfiliadosDesimputarCobro.aspx");
                //        ibtnRevertirCobro.Visible = this.ValidarPermiso("CargosAfiliadosRevertirCobro.aspx");
                //        break;
                //    default:
                //        break;
                //}
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                decimal suma;
                Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.Compute("Sum(Importe)", ""));
                lblImporte.Text = suma.ToString("C2");

                Label lblImporteCobrado = (Label)e.Row.FindControl("lblImporteCobrado");
                suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.Compute("Sum(ImporteCobrado)", ""));
                lblImporteCobrado.Text = suma.ToString("C2");

                Label lblImporteEnviar = (Label)e.Row.FindControl("lblImporteEnviar");
                suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.Compute("Sum(ImporteEnviar)", ""));
                lblImporteEnviar.Text = suma.ToString("C2");

            

            }
        }

        protected void gvCargosMensuales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametros);

            this.gvCargosMensuales.PageIndex = e.NewPageIndex;
            this.gvCargosMensuales.DataSource = this.MisCargosAfiliadosMensuales;
            this.gvCargosMensuales.DataBind();
        }
    }
}