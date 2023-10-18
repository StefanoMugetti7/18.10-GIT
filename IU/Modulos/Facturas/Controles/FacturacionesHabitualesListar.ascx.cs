using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using System.Collections;
using Comunes.Entidades;
using Afiliados.Entidades;
using Facturas;

namespace IU.Modulos.Facturas.Controles
{
    public partial class FacturacionesHabitualesListar : ControlesSeguros
    {

        private int MiIdAfiliado
        {
            get { return (int)Session[this.MiSessionPagina + "FacturacionesHabitualesListarIdAfiliado"]; }
            set { Session[this.MiSessionPagina + "FacturacionesHabitualesListarIdAfiliado"] = value; }
        }

        private List<VTAFacturacionesHabituales> MisFacturacionesHabituales
        {
            get { return (List<VTAFacturacionesHabituales>)Session[this.MiSessionPagina + "FacturacionesHabitualesListarMisFacturacionesHabituales"]; }
            set { Session[this.MiSessionPagina + "FacturacionesHabitualesListarMisFacturacionesHabituales"] = value; }
        }

        public delegate void FacturacionesHabitualesListarAgregarEventHandler();
        public event FacturacionesHabitualesListarAgregarEventHandler FacturacionesHabitualesListarAgregar;

        public delegate void FacturacionesHabitualesListarConsultarEventHandler(VTAFacturacionesHabituales e);
        public event FacturacionesHabitualesListarConsultarEventHandler FacturacionesHabitualesListarConsultar;

        public delegate void FacturacionesHabitualesListarModificarEventHandler(VTAFacturacionesHabituales e);
        public event FacturacionesHabitualesListarModificarEventHandler FacturacionesHabitualesListarModificar;

        public void IniciarControl(AfiAfiliados pParametro, Gestion pGestion)
        {
            this.MiIdAfiliado = pParametro.IdAfiliado;
            this.GestionControl = pGestion;

            VTAFacturacionesHabituales filtro = new VTAFacturacionesHabituales();
            filtro.IdAfiliado = this.MiIdAfiliado;
            this.MisFacturacionesHabituales = FacturasF.FacturacionesHabitualesObtenerListaFiltro(filtro);
            AyudaProgramacion.CargarGrillaListas<VTAFacturacionesHabituales>(this.MisFacturacionesHabituales, false, this.gvDatos, true);

            if (this.GestionControl == Gestion.Modificar)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("FacturacionesHabitualesAgregar.aspx");
            }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (this.FacturacionesHabitualesListarAgregar != null)
                this.FacturacionesHabitualesListarAgregar();

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiIdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturacionesHabitualesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            VTAFacturacionesHabituales facturacionHabitual = this.MisFacturacionesHabituales[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiIdAfiliado);
            this.MisParametrosUrl.Add("IdFacturacionHabitual", facturacionHabitual.IdFacturacionHabitual);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                if (this.FacturacionesHabitualesListarModificar != null)
                    this.FacturacionesHabitualesListarModificar(facturacionHabitual);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturacionesHabitualesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.FacturacionesHabitualesListarConsultar != null)
                    this.FacturacionesHabitualesListarConsultar(facturacionHabitual);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturacionesHabitualesConsultar.aspx"), true);
            }
            
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                switch (this.GestionControl)
                {                 
                    case Gestion.Consultar:
                        ibtnConsultar.Visible = this.ValidarPermiso("FacturacionesHabitualesConsultar.aspx");
                        break;
                    case Gestion.Modificar:
                        modificar.Visible = this.ValidarPermiso("FacturacionesHabitualesModificar.aspx");
                        break;
                    default:
                        break;
                }
                //Permisos btnEliminar
                //ibtnConsultar.Visible = this.ValidarPermiso("FacturasConsultar.aspx");
                //ibtnConsultar.Visible = true;
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTAFacturacionesHabituales parametros = this.BusquedaParametrosObtenerValor<VTAFacturacionesHabituales>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTAFacturacionesHabituales>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisFacturacionesHabituales;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisFacturacionesHabituales = this.OrdenarGrillaDatos<VTAFacturacionesHabituales>(this.MisFacturacionesHabituales, e);
            this.gvDatos.DataSource = this.MisFacturacionesHabituales;
            this.gvDatos.DataBind();
        }
    }
}