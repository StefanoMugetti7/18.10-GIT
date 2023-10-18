using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Prestamos.Entidades;
using Comunes.Entidades;
using Prestamos;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Collections.Generic;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PlanesDatos : ControlesSeguros
    {
        private PrePrestamosPlanes MiPlan
        {
            get
            {
                return (Session[MiSessionPagina + "PlanesDatosMiPlan"] == null ?
                    (PrePrestamosPlanes)(Session[MiSessionPagina + "PlanesDatosMiPlan"] = new PrePrestamosPlanes()) : (PrePrestamosPlanes)Session[MiSessionPagina + "PlanesDatosMiPlan"]);
            }
            set { Session[MiSessionPagina + "PlanesDatosMiPlan"] = value; }
        }

        public delegate void PlanesDatosAceptarEventHandler(object sender, PrePrestamosPlanes e);
        public event PlanesDatosAceptarEventHandler PlanesDatosAceptar;

        public delegate void PlanesDatosCancelarEventHandler();
        public event PlanesDatosCancelarEventHandler PlanesDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ctrParametrosPopUp.PrePrestamosPlanesTasasAceptar += new PlanesTasasDatosPopUp.PlanesTasasDatosPopUpAceptarEventHandler(ctrParametrosPopUp_PrePrestamosPlanesTasasAceptar);
            popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!IsPostBack)
            {
            }
        }

        void ctrParametrosPopUp_PrePrestamosPlanesTasasAceptar(PrePrestamosPlanesTasas e)
        {
            List<PrePrestamosPlanesTasas> lista = new List<PrePrestamosPlanesTasas>();
            lista.AddRange(MiPlan.PrestamosPlanesTasas);
            MiPlan.PrestamosPlanesTasas = new List<PrePrestamosPlanesTasas>();
            MiPlan.PrestamosPlanesTasas.Add(e);
            //e.IndiceColeccion = MiPlan.PrestamosPlanesTasas.IndexOf(e);
            MiPlan.PrestamosPlanesTasas.AddRange(lista);
            AyudaProgramacion.AcomodarIndices<PrePrestamosPlanesTasas>(MiPlan.PrestamosPlanesTasas);
            CargarGrilla();
            upParametrosValores.Update();
        }

        public void IniciarControl(PrePrestamosPlanes pParametro, Gestion pGestion)
        {
            MiPlan = pParametro;
            GestionControl = pGestion;

            ddlMoneda.DataSource = TGEGeneralesF.MonedasObtenerListaActiva();
            ddlMoneda.DataValueField = "IdMoneda";
            ddlMoneda.DataTextField = "Descripcion";
            ddlMoneda.DataBind();
            if (ddlMoneda.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(ddlMoneda, ObtenerMensajeSistema("SeleccioneOpcion"));

            ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            ddlEstado.DataValueField = "IdEstado";
            ddlEstado.DataTextField = "Descripcion";
            ddlEstado.DataBind();
            ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();

            chkFormasCobros.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
            chkFormasCobros.DataValueField = "IdFormaCobro";
            chkFormasCobros.DataTextField = "FormaCobro";
            chkFormasCobros.DataBind();

            //TIPOS OPERACIONES 
            chkTiposOperaciones.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(EnumTGETiposFuncionalidades.Prestamos); //levantar tipos operaciones
            chkTiposOperaciones.DataValueField = "IdTipoOperacion";
            chkTiposOperaciones.DataTextField = "TipoOperacion";
            chkTiposOperaciones.DataBind();


            ddlTipoUnidad.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposUnidades);
            ddlTipoUnidad.DataValueField = "IdListaValorDetalle";
            ddlTipoUnidad.DataTextField = "Descripcion";
            ddlTipoUnidad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlTipoUnidad, ObtenerMensajeSistema("SeleccioneOpcion"));

            MiPlan = PrePrestamosF.PrestamosPlanesObtenerDatosCompletos(pParametro);
            if (pGestion == Gestion.Consultar)
            {
                txtPlan.Enabled = false;

                txtImporteGastos.Enabled = false;
                txtPorcentajeGasto.Enabled = false;
                txtPorcentajeCapitalSocial.Enabled = false;
                txtPorcentajeSeguro.Enabled = false;
                btnAceptar.Visible = false;
                btnAgregar.Visible = false;
                ddlTipoUnidad.SelectedValue = MiPlan.TipoUnidad.IdTiposUnidades.ToString();
            }
            else if (pGestion == Gestion.Modificar)
            {
                ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
                ddlTipoUnidad.SelectedValue = MiPlan.TipoUnidad.IdTiposUnidades.ToString();
            }                       
                       
            txtPlan.Text = MiPlan.Descripcion;
            txtImporteGastos.Text = MiPlan.ImporteGasto.ToString("C2");
            txtPorcentajeGasto.Text = MiPlan.PorcentajeGasto.ToString("N4");
            txtPorcentajeCapitalSocial.Text = MiPlan.PorcentajeCapitalSocial.HasValue ? MiPlan.PorcentajeCapitalSocial.Value.ToString("N4") : string.Empty;
            txtPorcentajeSeguro.Text = MiPlan.PorcentajeSeguroCuota.HasValue ? MiPlan.PorcentajeSeguroCuota.Value.ToString("N4") : string.Empty;

            CargarGrilla();
            ListItem item;
            item = ddlMoneda.Items.FindByValue(MiPlan.Moneda.IdMoneda.ToString());
            if (item == null && MiPlan.Moneda.IdMoneda > 0)
                ddlMoneda.Items.Add(new ListItem(MiPlan.Moneda.Descripcion, MiPlan.Moneda.IdMoneda.ToString()));
            ddlMoneda.SelectedValue = MiPlan.Moneda.IdMoneda > 0 ? MiPlan.Moneda.IdMoneda.ToString() : string.Empty;

            foreach (TGEFormasCobros fp in MiPlan.FormasCobros)
            {
                item = chkFormasCobros.Items.FindByValue(fp.IdFormaCobro.ToString());
                if (item != null)
                    item.Selected = true;
            }
            foreach (TGETiposOperaciones to in MiPlan.TiposOperaciones)
            {
                item = chkTiposOperaciones.Items.FindByValue(to.IdTipoOperacion.ToString());
                if (item != null)
                    item.Selected = true;
            }
            ctrPlanesIps.IniciarControl(MiPlan, pGestion);
            ctrPlanesGrillas.IniciarControl(MiPlan, pGestion);
            ctrComentarios.IniciarControl(MiPlan, pGestion);
            ctrArchivos.IniciarControl(MiPlan, pGestion);
            ctrAuditoria.IniciarControl(MiPlan);
            ctrCamposValores.IniciarControl(MiPlan, new Objeto(), GestionControl);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            PrePrestamosPlanesTasas param = new PrePrestamosPlanesTasas();
            param.IdPrestamoPlan= MiPlan.IdPrestamoPlan;
            ctrParametrosPopUp.IniciarControl(param);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = false;
            MiPlan.Descripcion = txtPlan.Text;
            MiPlan.Moneda.IdMoneda = Convert.ToInt32(ddlMoneda.SelectedValue);
            MiPlan.Moneda.Descripcion = ddlMoneda.SelectedItem.Text;
            MiPlan.ImporteGasto = txtImporteGastos.Decimal;
            MiPlan.PorcentajeGasto = txtPorcentajeGasto.Decimal;
            MiPlan.PorcentajeSeguroCuota = txtPorcentajeSeguro.Decimal;
            MiPlan.Estado.IdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            MiPlan.Estado.Descripcion = ddlEstado.SelectedItem.Text;
            MiPlan.PorcentajeCapitalSocial = txtPorcentajeCapitalSocial.Text == string.Empty ? default(decimal?) : txtPorcentajeCapitalSocial.Decimal;
            MiPlan.TipoUnidad.IdTiposUnidades = ddlTipoUnidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoUnidad.SelectedValue);

            TGEFormasCobros formaCobro;
            foreach (ListItem lst in chkFormasCobros.Items)
            {
                formaCobro = MiPlan.FormasCobros.Find(x => x.IdFormaCobro == Convert.ToInt32(lst.Value));
                if (formaCobro == null && lst.Selected)
                {
                    formaCobro = new TGEFormasCobros();
                    formaCobro.IdFormaCobro = Convert.ToInt32(lst.Value);
                    formaCobro.FormaCobro = lst.Text;
                    MiPlan.FormasCobros.Add(formaCobro);
                    formaCobro.EstadoColeccion = EstadoColecciones.Agregado;
                    formaCobro.IndiceColeccion = MiPlan.FormasCobros.IndexOf(formaCobro);
                }
                else if (formaCobro != null && !lst.Selected)
                    formaCobro.EstadoColeccion = EstadoColecciones.Borrado;
            }

            //Tipos Operaciones
            TGETiposOperaciones tipoOperacion;
            foreach (ListItem lst in chkTiposOperaciones.Items)
            {
                tipoOperacion = MiPlan.TiposOperaciones.Find(x => x.IdTipoOperacion == Convert.ToInt32(lst.Value));
                if (tipoOperacion == null && lst.Selected)
                {
                    tipoOperacion = new TGETiposOperaciones();
                    tipoOperacion.IdTipoOperacion = Convert.ToInt32(lst.Value);
                    tipoOperacion.TipoOperacion = lst.Text;
                    MiPlan.TiposOperaciones.Add(tipoOperacion);
                    tipoOperacion.EstadoColeccion = EstadoColecciones.Agregado;
                    tipoOperacion.IndiceColeccion = MiPlan.TiposOperaciones.IndexOf(tipoOperacion);
                }
                else if (tipoOperacion != null && !lst.Selected)
                    tipoOperacion.EstadoColeccion = EstadoColecciones.Borrado;
            }
            MiPlan.PrestamosIpsPlanes = ctrPlanesIps.ObtenerLista();
            MiPlan.PrestamosBancoSolParametros = ctrPlanesGrillas.ObtenerLista();
            MiPlan.Comentarios = ctrComentarios.ObtenerLista();
            MiPlan.Archivos = ctrArchivos.ObtenerLista();
            MiPlan.Campos = ctrCamposValores.ObtenerLista();
            MiPlan.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    guardo = PrePrestamosF.PrestamosPlanesAgregar(MiPlan);
                    break;
                case Gestion.Modificar:
                    guardo = PrePrestamosF.PrestamosPlanesModificar(MiPlan);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                popUpMensajes.MostrarMensaje(ObtenerMensajeSistema(MiPlan.CodigoMensaje));
            }
            else
            {
                MostrarMensaje(MiPlan.CodigoMensaje, true, MiPlan.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (PlanesDatosCancelar != null)
                PlanesDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (PlanesDatosAceptar != null)
                PlanesDatosAceptar(null, MiPlan);
        }

        protected void gvParametrosValores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Eliminar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            MiPlan.PrestamosPlanesTasas[indiceColeccion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(MiPlan.PrestamosPlanesTasas[indiceColeccion], GestionControl);
            MiPlan.PrestamosPlanesTasas[indiceColeccion].Estado.IdEstado = (int)Estados.Baja;
            MiPlan.PrestamosPlanesTasas[indiceColeccion].Estado = TGEGeneralesF.TGEEstadosObtener(MiPlan.PrestamosPlanesTasas[indiceColeccion].Estado);

            CargarGrilla();
        }

        protected void gvParametrosValores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
            //    string mensaje = ObtenerMensajeSistema("ParametroValorConfirmarBaja");
            //    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
            //    ibtn.Attributes.Add("OnClick", funcion);
            //    ibtn.Visible = true;
            //}
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvParametrosValores.PageIndex = e.NewPageIndex;
            gvParametrosValores.DataSource = MiPlan.PrestamosPlanesTasas;
            gvParametrosValores.DataBind();
        }

        private void CargarGrilla()
        {
            //MiPlan.PrestamosPlanesTasas = MiPlan.PrestamosPlanesTasas.OrderByDescending(x => x.FechaDesde).ToList();
            //MiPlan.PrestamosPlanesTasas = AyudaProgramacion.AcomodarIndices<PrePrestamosPlanesTasas>(MiPlan.PrestamosPlanesTasas);
            AyudaProgramacion.CargarGrillaListas(MiPlan.PrestamosPlanesTasas, true, gvParametrosValores, true);
        }
    }
}