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
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Afiliados.Entidades;
using System.Collections.Generic;

namespace IU.Modulos.TGE.Control
{
    public partial class TiposFuncionalidadesPorEstadosModificarDatos : ControlesSeguros
    {
        private TGEEstados MiEstado
        {
            get
            {
                return (Session[this.MiSessionPagina + "TiposFuncionalidadesPorEstadosModificarDatosMiEstado"] == null ?
                    (TGEEstados)(Session[this.MiSessionPagina + "TiposFuncionalidadesPorEstadosModificarDatosMiEstado"] = new TGEEstados()) : (TGEEstados)Session[this.MiSessionPagina + "TiposFuncionalidadesPorEstadosModificarDatosMiEstado"]);
            }
            set { Session[this.MiSessionPagina + "TiposFuncionalidadesPorEstadosModificarDatosMiEstado"] = value; }
        }

        //public delegate void TiposFuncionalidadesPorEstadosDatosAceptarEventHandler(object sender, TGEParametros e);
        //public event TiposFuncionalidadesPorEstadosDatosAceptarEventHandler TiposFuncionalidadesPorEstadosDatosAceptar;

        public delegate void TiposFuncionalidadesPorEstadosDatosCancelarEventHandler();
        public event TiposFuncionalidadesPorEstadosDatosCancelarEventHandler TiposFuncionalidadesPorEstadosDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar +=new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
            }
        }

        public void IniciarControl(Gestion pGestion)
        {
            this.MiEstado = new TGEEstados();
            this.GestionControl = pGestion;

            if (pGestion == Gestion.Consultar)
            {
                this.btnAceptar.Visible = false;
                this.chkEstados.Enabled = false;
            }
            this.CargarCombos();
        }

        private void CargarCombos()
        {
            string parametro = "AfiAfiliados";
            List<TGEEstados> estados = TGEGeneralesF.TGEEstadosObtenerLista(parametro);
            if (estados.Exists(x => x.IdEstado == (int)EstadosAfiliados.Vigente))
                estados.Remove(estados.Find(x => x.IdEstado == (int)EstadosAfiliados.Vigente));
            this.ddlEstados.DataSource = AyudaProgramacion.AcomodarIndices<TGEEstados>(estados);
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
        }
                
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = false;
            this.MiEstado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ObtenerTiposFuncionalidades(this.MiEstado);
            switch (this.GestionControl)
            {
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.TGETiposFuncionalidadesEstadosModificar(this.MiEstado);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.MostrarMensaje(this.MiEstado.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(this.MiEstado.CodigoMensaje, true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.TiposFuncionalidadesPorEstadosDatosCancelar != null)
                this.TiposFuncionalidadesPorEstadosDatosCancelar();
        }

        //protected void popUpMensajes_popUpMensajesPostBackAceptar()
        //{
        //    if (this.TiposFuncionalidadesPorEstadosDatosAceptar != null)
        //        this.TiposFuncionalidadesPorEstadosDatosAceptar(null, this.MiEstado);
        //}

        protected void ddlEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlEstados.SelectedValue))
            {
                this.chkEstados.DataSource = TGEGeneralesF.TGETiposFuncionalidadesObtenerLista();
                this.chkEstados.DataValueField = "IdTipoFuncionalidad";
                this.chkEstados.DataTextField = "TipoFuncionalidad";
                this.chkEstados.DataBind();
                this.MiEstado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
                this.MiEstado.TiposFuncionalidades = TGEGeneralesF.TGETiposFuncionalidadesObtenerListaFiltro(this.MiEstado);
                this.CargarTiposFuncionalidadesEstados(this.MiEstado);
                this.btnAceptar.Visible = true;
            }
            else
            {
                this.chkEstados.Items.Clear();
                this.btnAceptar.Visible = false;
            }
        }

        private void CargarTiposFuncionalidadesEstados(TGEEstados pEstado)
        {
            foreach (TGETiposFuncionalidades func in pEstado.TiposFuncionalidades)
            {
                foreach (ListItem item in this.chkEstados.Items)
                {
                    if (Convert.ToInt32(item.Value) == func.IdTipoFuncionalidad)
                        item.Selected = true;
                }
            }
        }

        private void ObtenerTiposFuncionalidades(TGEEstados pEstado)
        {
            TGETiposFuncionalidades tipoFunc;
            //pUsuario.Perfiles = new List<Perfiles>();

            foreach (ListItem lst in this.chkEstados.Items)
            {
                tipoFunc = pEstado.TiposFuncionalidades.Find(delegate(TGETiposFuncionalidades per)
                { return per.IdTipoFuncionalidad == Convert.ToInt32(lst.Value); });

                if (tipoFunc == null && lst.Selected)
                {
                    tipoFunc = new TGETiposFuncionalidades();
                    tipoFunc.IdTipoFuncionalidad = Convert.ToInt32(lst.Value);
                    tipoFunc.TipoFuncionalidad = lst.Text;
                    pEstado.TiposFuncionalidades.Add(tipoFunc);
                    tipoFunc.EstadoColeccion = EstadoColecciones.Agregado;
                }
                else if (tipoFunc != null && !lst.Selected)
                    tipoFunc.EstadoColeccion = EstadoColecciones.Borrado;

            }
        }
    }
}