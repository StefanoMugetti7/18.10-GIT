using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Tesorerias.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Tesorerias;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Bancos.Entidades;
using Bancos;
using CuentasPagar.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class BancosLotesListar : PaginaSegura
    {
        private List<TESBancosLotesEnviados> MisBancosLotes
        {
            get { return (List<TESBancosLotesEnviados>)Session[this.MiSessionPagina + "TESBancosLotesEnviadosMisBancosLotesEnviados"]; }
            set { Session[this.MiSessionPagina + "TESBancosLotesEnviadosMisBancosLotesEnviados"] = value; }
        }

        private TESBancosLotesEnviados MiBancoLoteAExportar
        {
            get { return (TESBancosLotesEnviados)Session[this.MiSessionPagina + "TESBancosLotesEnviadosMiBancoLoteExportar"]; }
            set { Session[this.MiSessionPagina + "TESBancosLotesEnviadosMiBancoLoteExportar"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDetalle, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("BancosLotesAgregar.aspx");
                this.CargarCombos();
                TESBancosLotesEnviados parametros = this.BusquedaParametrosObtenerValor<TESBancosLotesEnviados>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlBancoCuenta.SelectedValue = parametros.IdBancoCuenta.ToString();
                    this.txtDetalle.Text = parametros.Filtro;
                    this.CargarLista(parametros);
                }
            }
            else
            {
               this.DescargarArchivosRegisterPostBack();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesAgregar.aspx"), true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TESBancosLotesEnviados parametros = this.BusquedaParametrosObtenerValor<TESBancosLotesEnviados>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()
                || e.CommandName == Gestion.ConfirmarAgregar.ToString()
                || e.CommandName == Gestion.Cancelar.ToString()
                || e.CommandName == Gestion.Exportar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESBancosLotesEnviados bancoCuenta = this.MisBancosLotes[indiceColeccion];

            //if (this.Session[this.MiSessionPagina + "Modulos/Bancos/BancosLotesListar.aspx"] != null)
            //    this.Session[this.MiSessionPagina + "Modulos/Bancos/BancosLotesListar.aspx"] = null;

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdBancoLoteEnvio", bancoCuenta.IdBancoLoteEnvio);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesFinalizar.aspx"), true);
            } 
            else if (e.CommandName == Gestion.Autorizar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesAutorizar.aspx"), true);
            }
            else if (e.CommandName == Gestion.ConfirmarAgregar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesConfirmar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Cancelar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesCancelarConfirmado.aspx"), true);
            }
            else if (e.CommandName == Gestion.Exportar.ToString())
            {
                this.ExportTextFile(bancoCuenta);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TESBancosLotesEnviados bancoCuenta = (TESBancosLotesEnviados)e.Row.DataItem;

                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton conciliar = (ImageButton)e.Row.FindControl("btnConciliar");
                ImageButton cancelarConfirmado = (ImageButton)e.Row.FindControl("btnAnularConfirmar");
                ImageButton exportarTxt = (ImageButton)e.Row.FindControl("btnExportarTxt");


                switch (bancoCuenta.Estado.IdEstado)
                {
                    case (int)EstadosOrdenesPago.Autorizado:
                        anular.Visible = true;
                        modificar.Visible = this.ValidarPermiso("BancosLotesModificar.aspx");
                        conciliar.Visible = this.ValidarPermiso("BancosLotesConfirmar.aspx");
                        exportarTxt.Visible = true;
                        break;
                    case (int)EstadosOrdenesPago.Activo:
                        consultar.Visible = this.ValidarPermiso("BancosLotesConsultar.aspx");
                        modificar.Visible = this.ValidarPermiso("BancosLotesModificar.aspx");
                        autorizar.Visible = this.ValidarPermiso("BancosLotesAutorizar.aspx");
                        break;
                    case (int)EstadosOrdenesPago.Baja:
                        modificar.Visible = false;
                        break;
                    case (int)EstadosOrdenesPago.Conciliado:
                        cancelarConfirmado.Visible = this.ValidarPermiso("BancosLotesCancelarConfirmado.aspx");
                        modificar.Visible = false;
                        exportarTxt.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisBancosLotes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESBancosLotesEnviados parametros = this.BusquedaParametrosObtenerValor<TESBancosLotesEnviados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESBancosLotesEnviados>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisBancosLotes;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisBancosLotes = this.OrdenarGrillaDatos<TESBancosLotesEnviados>(this.MisBancosLotes, e);
            this.gvDatos.DataSource = this.MisBancosLotes;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            List<TESBancosCuentas> lista = BancosF.BancosCuentasObtenerListaFiltroTransferencia(bancoCuenta);
            this.ddlBancoCuenta.DataSource = lista;
            this.ddlBancoCuenta.DataValueField = "IdBancoCuenta";
            this.ddlBancoCuenta.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancoCuenta.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancoCuenta, ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarLista(TESBancosLotesEnviados pParametro)
        {
            pParametro.IdBancoCuenta = this.ddlBancoCuenta.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancoCuenta.SelectedValue);
            pParametro.Filtro = this.txtDetalle.Text;
            pParametro.BusquedaParametros = true;

            this.BusquedaParametrosGuardarValor<TESBancosLotesEnviados>(pParametro);
            this.MisBancosLotes = BancosF.BancosLotesEnviadosObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisBancosLotes;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
            this.DescargarArchivosRegisterPostBack();
        }
        private void ExportarTxtReporte(DataTable ds)
        {
            string separador = ";";
            ExportData exportar = new ExportData();
            exportar.ExportFile(this, ds, separador, false, "NombreReporte");
            
        }

        private void DescargarArchivosRegisterPostBack()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    ImageButton ibtnDescargar = (ImageButton)fila.FindControl("btnExportarTxt");
                    this.toolkitScriptManager.RegisterPostBackControl(ibtnDescargar);
                }
            }
        }
        protected void ExportTextFile(TESBancosLotesEnviados data)
        {
            DataTable datos = BancosF.BancosLotesEnviadosObtenerDatosTxt(data);
            if (datos.Columns.Count > 0)
                this.ExportarTxtReporte(datos);
            UpdatePanel1.Update();
        }
    }
}
