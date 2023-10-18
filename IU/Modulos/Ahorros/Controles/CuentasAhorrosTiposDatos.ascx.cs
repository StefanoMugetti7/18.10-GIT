using Ahorros;
using Ahorros.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Xml;

namespace IU.Modulos.Ahorros.Controles
{
    public partial class CuentasAhorrosTiposDatos : ControlesSeguros
    {
        private AhoCuentasTipos MiAhoCuentasTipos
        {
            get { return (AhoCuentasTipos)Session[this.MiSessionPagina + "AhorroMiAhoCuentasTipos"]; }
            set { Session[this.MiSessionPagina + "AhorroMiAhoCuentasTipos"] = value; }
        }
        private List<AhoCuentasTiposCuentasContables> MiAhoCuentasTiposCuentasContables
        {
            get { return (List<AhoCuentasTiposCuentasContables>)Session[this.MiSessionPagina + "AhorroMiAhoCuentasTiposCuentasContables"]; }
            set { Session[this.MiSessionPagina + "AhorroMiAhoCuentasTiposCuentasContables"] = value; }
        }
        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "AhorroMiAhoCuentasTipoMisMonedas"]; }
            set { Session[this.MiSessionPagina + "AhorroMiAhoCuentasTipoMisMonedas"] = value; }
        }
        public delegate void AhorroDatosAceptarEventHandler(object sender, AhoCuentasTipos e);
        public event AhorroDatosAceptarEventHandler AhorroModificarDatosAceptar;
        public delegate void AhorroDatosCancelarEventHandler();
        public event AhorroDatosCancelarEventHandler AhorroModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);

            if (this.IsPostBack)
            {
                if (this.MiAhoCuentasTipos == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            else
            {
                if (this.MiAhoCuentasTipos.CuentasContables.Count == 0)
                {
                    MiAhoCuentasTiposCuentasContables = new List<AhoCuentasTiposCuentasContables>();
                    this.IniciarGrilla(1);
                }
            }
        }
        public void IniciarControl(AhoCuentasTipos pCuentas, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAhoCuentasTipos = pCuentas;
                    this.ddlEstado.SelectedValue = 1.ToString();
                    this.ddlEstado.Enabled = false;
                    break;
                case Gestion.Modificar:
                    this.MiAhoCuentasTipos = AhorroF.CuentasTiposObtenerDatosCompletos(pCuentas);
                    this.MiAhoCuentasTiposCuentasContables = this.MiAhoCuentasTipos.CuentasContables;
                    this.MapearObjetoAControles(this.MiAhoCuentasTipos);
                    break;
                case Gestion.Consultar:
                    this.MiAhoCuentasTipos = AhorroF.CuentasTiposObtenerDatosCompletos(pCuentas);
                    this.MapearObjetoAControles(this.MiAhoCuentasTipos);
                    this.ddlEstado.Enabled = false;
                    this.txtCodigo.Enabled = false;
                    this.txtCuentaTipo.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.lblCantidadAgregar.Visible = false;
                    this.txtCantidadAgregar.Visible = false;
                    this.btnAgregarItem.Visible = false;
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
        private void MapearObjetoAControles(AhoCuentasTipos pCuenta)
        {
            this.txtCodigo.Text = pCuenta.CodigoBNRA;
            this.txtDescripcion.Text = pCuenta.Descripcion;
            this.ddlEstado.SelectedValue = pCuenta.Estado.IdEstado.ToString();
            this.txtCuentaTipo.Text = pCuenta.CuentaTipo;

            if (pCuenta.CuentasContables.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<AhoCuentasTiposCuentasContables>(pCuenta.CuentasContables, false, this.gvDatos, true);
                this.UpdatePanel2.Update();
            }
        }
        private void MapearControlesAObjeto(AhoCuentasTipos pCuenta)
        {
            pCuenta.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pCuenta.CodigoBNRA = this.txtCodigo.Text;
            pCuenta.Descripcion = this.txtDescripcion.Text;
            pCuenta.CuentaTipo = this.txtCuentaTipo.Text;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiAhoCuentasTipos);
            this.PersistirDatosGrilla();
            this.MiAhoCuentasTipos.CuentasContables = this.MiAhoCuentasTiposCuentasContables;

            string XML = "<CuentasContables>";
            foreach (var item in this.MiAhoCuentasTipos.CuentasContables)
            {
                XML = string.Concat(XML, "<CuentaContable>" +
                    "<IdMoneda>", item.Moneda.IdMoneda, "</IdMoneda>" +
                    "<IdCuentaContable>", item.IdCuentaContable, "</IdCuentaContable>", "</CuentaContable>");
            }
            XML = string.Concat(XML, "</CuentasContables>");
            this.MiAhoCuentasTipos.LoteDetalles = new XmlDocument();
            this.MiAhoCuentasTipos.LoteDetalles.LoadXml(XML);

            this.MiAhoCuentasTipos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = AhorroF.CuentasTiposAgregar(this.MiAhoCuentasTipos);
                    break;
                case Gestion.Modificar:
                    guardo = AhorroF.CuentasTiposModificar(this.MiAhoCuentasTipos);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAhoCuentasTipos.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiAhoCuentasTipos.CodigoMensaje, true, this.MiAhoCuentasTipos.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.AhorroModificarDatosCancelar != null)
                this.AhorroModificarDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.AhorroModificarDatosAceptar != null)
                this.AhorroModificarDatosAceptar(null, this.MiAhoCuentasTipos);
        }
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            if (txtCantidadAgregar.Text == string.Empty || txtCantidadAgregar.Text == "0")
            {
                txtCantidadAgregar.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregar.Text);
            this.IniciarGrilla(cantidad);
            this.txtCantidadAgregar.Text = string.Empty;
        }
        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                DropDownList moneda = ((DropDownList)fila.FindControl("ddlMoneda"));
                HiddenField idCuentaContable = ((HiddenField)fila.FindControl("hdfIdCuentaContable"));
                HiddenField cuentaContable = ((HiddenField)fila.FindControl("hdfCuentaContable"));

                this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.SinCambio;
                if (this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].Moneda.IdMoneda != Convert.ToInt32(moneda.SelectedValue))
                {
                    this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].Moneda.IdMoneda = Convert.ToInt32(moneda.SelectedValue);
                    this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Modificado;
                }
                if (this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].IdCuentaContable != Convert.ToInt32(idCuentaContable.Value))
                {
                    this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].CuentaContableDescripcion = cuentaContable.Value;
                    this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].IdCuentaContable = Convert.ToInt32(idCuentaContable.Value);
                    this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Modificado;
                }
                if (this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].IdCuentaTipoCuentaContable == 0)
                {
                    this.MiAhoCuentasTiposCuentasContables[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Agregado;
                }
            }
        }
        private void IniciarGrilla(int pCantidad)
        {
            for (int i = 0; i < pCantidad; i++)
            {
                this.MiAhoCuentasTiposCuentasContables.Add(new AhoCuentasTiposCuentasContables());
            }
            this.gvDatos.DataSource = this.MiAhoCuentasTiposCuentasContables;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AhoCuentasTiposCuentasContables dr = (AhoCuentasTiposCuentasContables)e.Row.DataItem;
                if (dr != null)
                {
                    DropDownList ddlCuentaContable = ((DropDownList)e.Row.FindControl("ddlCuentaContable"));
                    DropDownList ddlMoneda = ((DropDownList)e.Row.FindControl("ddlMoneda"));
                    ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                    ddlMoneda.DataSource = MisMonedas;
                    ddlMoneda.DataValueField = "IdMoneda";
                    ddlMoneda.DataTextField = "Descripcion";
                    ddlMoneda.DataBind();

                    if (dr.Moneda.IdMoneda > 0)
                        ddlMoneda.SelectedValue = dr.Moneda.IdMoneda.ToString();

                    if (dr.IdCuentaContable > 0)
                        ddlCuentaContable.Items.Add(new ListItem(dr.CuentaContableDescripcion, dr.IdCuentaContable.ToString()));

                    if (GestionControl == Gestion.Consultar)
                    {
                        ddlCuentaContable.Enabled = false;
                        ddlMoneda.Enabled = false;
                        ibtn.Visible = false;
                    }

                }
            }
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //int MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                this.PersistirDatosGrilla();
                AhoCuentasTiposCuentasContables dr = MiAhoCuentasTiposCuentasContables[index];
                this.MiAhoCuentasTiposCuentasContables.Remove(dr);
                this.gvDatos.DataSource = MiAhoCuentasTiposCuentasContables;
                this.gvDatos.DataBind();
                items.Update();
            }
        }
    }
}