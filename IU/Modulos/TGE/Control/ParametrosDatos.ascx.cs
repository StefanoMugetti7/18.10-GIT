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
using System.Collections.Generic;
using Generales.Entidades;
using Comunes.Entidades;
using System.Reflection;
using Generales.FachadaNegocio;

namespace IU.Modulos.TGE.Control
{
    public partial class ParametrosDatos : ControlesSeguros
    {
        private TGEParametros MiParametro
        {
            get
            {
                return (Session[this.MiSessionPagina + "ParametrosValoresMiParametro"] == null ?
                    (TGEParametros)(Session[this.MiSessionPagina + "ParametrosValoresMiParametro"] = new TGEParametros()) : (TGEParametros)Session[this.MiSessionPagina + "ParametrosValoresMiParametro"]);
            }
            set { Session[this.MiSessionPagina + "ParametrosValoresMiParametro"] = value; }
        }

        public delegate void ParametrosDatosAceptarEventHandler(object sender, TGEParametros e);
        public event ParametrosDatosAceptarEventHandler ParametrosDatosAceptar;

        public delegate void ParametrosDatosCancelarEventHandler();
        public event ParametrosDatosCancelarEventHandler ParametrosDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrParametrosPopUp.ParametrosValoresPopUpAceptar += new ParametrosDatosPopUp.ParametrosDatosPopUpAceptarEventHandler(ctrParametrosPopUp_ParametrosValoresPopUpAceptar);
            this.popUpMensajes.popUpMensajesPostBackAceptar +=new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
            }
        }

        void ctrParametrosPopUp_ParametrosValoresPopUpAceptar(TGEParametrosValores e)
        {
            this.MiParametro.ParametrosValores.Add(e);
            e.IndiceColeccion = this.MiParametro.ParametrosValores.IndexOf(e);
            this.CargarGrilla();
        }

        public void IniciarControl(TGEParametros pParametro, Gestion pGestion)
        {
            this.MiParametro = pParametro;
            this.GestionControl = pGestion;

            if (pGestion == Gestion.Consultar)
            {
                this.btnAceptar.Visible = false;
                this.btnAgregar.Visible = false;
            }

            this.MiParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiParametro = TGEGeneralesF.ParametrosObtenerDatosCompletos(pParametro);
            this.txtParametro.Text = this.MiParametro.NombreParametro;
            this.CargarGrilla();
            
            this.ctrComentarios.IniciarControl(pParametro, pGestion);
            this.ctrArchivos.IniciarControl(pParametro, pGestion);
            this.ctrAuditoria.IniciarControl(pParametro);
        }
                
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            TGEParametrosValores param = new TGEParametrosValores();
            param.Parametro = this.MiParametro;
            this.ctrParametrosPopUp.IniciarControl(param);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = false;
            this.MiParametro.Comentarios = this.ctrComentarios.ObtenerLista();
            this.MiParametro.Archivos = this.ctrArchivos.ObtenerLista();
            this.MiParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.ParametrosModificar(this.MiParametro);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiParametro.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiParametro.CodigoMensaje, true, this.MiParametro.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ParametrosDatosCancelar != null)
                this.ParametrosDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ParametrosDatosAceptar != null)
                this.ParametrosDatosAceptar(null, this.MiParametro);
        }

        protected void gvParametrosValores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Eliminar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiParametro.ParametrosValores[indiceColeccion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiParametro.ParametrosValores[indiceColeccion], GestionControl);
            this.MiParametro.ParametrosValores[indiceColeccion].Estado.IdEstado = (int)Estados.Baja;
            this.MiParametro.ParametrosValores[indiceColeccion].Estado = TGEGeneralesF.TGEEstadosObtener(this.MiParametro.ParametrosValores[indiceColeccion].Estado);

            this.CargarGrilla();
        }

        protected void gvParametrosValores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                string mensaje = this.ObtenerMensajeSistema("ParametroValorConfirmarBaja");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                ibtn.Attributes.Add("OnClick", funcion);
                ibtn.Visible = true;
            }
        }

        private void CargarGrilla()
        {
            this.MiParametro.ParametrosValores = this.MiParametro.ParametrosValores.OrderByDescending(x => x.FechaDesde).ToList();
            this.MiParametro.ParametrosValores = AyudaProgramacion.AcomodarIndices<TGEParametrosValores>(this.MiParametro.ParametrosValores);
            AyudaProgramacion.CargarGrillaListas(this.MiParametro.ParametrosValores, true, this.gvParametrosValores, true);
        }
    }
}