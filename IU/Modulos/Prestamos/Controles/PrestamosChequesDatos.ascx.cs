using Comunes.Entidades;
using Evol.Controls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PrestamosChequesDatos : ControlesSeguros
    {
        public PrePrestamos MiPrePrestamos
        {
            get { return (PrePrestamos)Session[this.MiSessionPagina + "PrestamosChequesMiPrePrestamos"]; }
            set { Session[this.MiSessionPagina + "PrestamosChequesMiPrePrestamos"] = value; }
        }
        
        private List<TGEListasValoresSistemasDetalles> MiListaSistemasDetalles
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "PrestamosChequesDatosMiListaSistemasDetalles"]; }
            set { Session[this.MiSessionPagina + "PrestamosChequesDatosMiListaSistemasDetalles"] = value; }
        }

        //public delegate void PrestamosChequesCancelarEventHandler();
        //public event PrestamosChequesCancelarEventHandler PrestamosChequesCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            //PrePrestamosCheques prePrestamos = this.BusquedaParametrosObtenerValor<PrePrestamosCheques>();
            //if (prePrestamos.BusquedaParametros)
            //{

            //}
        }

        public void IniciarControl(PrePrestamos pParametro, Gestion pGestion)
        {
            this.MiPrePrestamos = pParametro;
            this.GestionControl = pGestion;
            if (pGestion == Gestion.Agregar)
            {
                if (MiListaSistemasDetalles == null || MiListaSistemasDetalles.Count==0)
                    MiListaSistemasDetalles = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Bancos);
                if (this.MiPrePrestamos.PrestamosCheques.Count == 0)
                    this.AgregarItem(1);
            }
            AyudaProgramacion.CargarGrillaListas<PrePrestamosCheques>(this.MiPrePrestamos.PrestamosCheques, true, this.gvDatos, true);
            this.Visible = this.MiPrePrestamos.PrestamosCheques.Count > 0;
        }
                

        public List<PrePrestamosCheques> ObtenerLista()
        {
            this.PersistirDatosGrilla();
            return this.MiPrePrestamos.PrestamosCheques;
        }

        public void LimpiarDatos()
        {
            this.MiPrePrestamos.PrestamosCheques.Clear();
        }

            #region Grilla
            private void AgregarItem(int cantidad)
        {
            PrePrestamosCheques item;
            for (int i = 0; i < cantidad; i++)
            {
                item = new PrePrestamosCheques();
                item.Estado.IdEstado = (int)Estados.Activo;
                item.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiPrePrestamos.PrestamosCheques.Add(item);
                item.IndiceColeccion = this.MiPrePrestamos.PrestamosCheques.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<PrePrestamosCheques>(this.MiPrePrestamos.PrestamosCheques, true, this.gvDatos, true);
        }

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            this.AgregarItem(1);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                this.PersistirDatosGrilla();
                this.MiPrePrestamos.PrestamosCheques.RemoveAt(indiceColeccion);
                this.MiPrePrestamos.PrestamosCheques = AyudaProgramacion.AcomodarIndices<PrePrestamosCheques>(this.MiPrePrestamos.PrestamosCheques);
                AyudaProgramacion.CargarGrillaListas<PrePrestamosCheques>(this.MiPrePrestamos.PrestamosCheques, true, this.gvDatos, true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PrePrestamosCheques item = (PrePrestamosCheques)e.Row.DataItem;
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                TextBox txtNumeroCheque = (TextBox)e.Row.FindControl("txtNumeroCheque");
                TextBox txtFechaDiferido = (TextBox)e.Row.FindControl("txtFechaDiferido");
                CurrencyTextBox txtImporte = ((CurrencyTextBox)e.Row.FindControl("txtImporte"));
                DropDownList ddlBancos = (DropDownList)e.Row.FindControl("ddlBancos");                
                TextBox txtCUIT = (TextBox)e.Row.FindControl("txtCUIT");
                TextBox txtTitularCheque = (TextBox)e.Row.FindControl("txtTitularCheque");
                TextBox txtCodigoPostal = (TextBox)e.Row.FindControl("txtCodigoPostal");
                TextBox txtNumeroSucursal = (TextBox)e.Row.FindControl("txtNumeroSucursal");
                ListItem itemCombo;

                ddlBancos.DataSource = MiListaSistemasDetalles;
                ddlBancos.DataValueField = "IdListaValorSistemaDetalle";
                ddlBancos.DataTextField = "Descripcion";
                ddlBancos.DataBind();
                if (ddlBancos.Items.Count > 1)
                    AyudaProgramacion.AgregarItemSeleccione(ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                itemCombo = ddlBancos.Items.FindByValue(item.IdBanco.ToString());
                if (itemCombo == null)
                    ddlBancos.Items.Add(new ListItem(item.BancoDescripcion, item.IdBanco.ToString()));
                ddlBancos.SelectedValue = item.IdBanco.ToString();

                if (GestionControl == Gestion.Agregar
                    || (GestionControl==Gestion.Modificar 
                        && MiPrePrestamos.Estado.IdEstado==(int)EstadosPrestamos.Activo)
                        )
                {
                    txtNumeroCheque.Enabled = true;
                    txtFechaDiferido.Enabled = true;
                    btnEliminar.Visible = true;
                    txtImporte.Enabled = true;
                    ddlBancos.Enabled = true;                    
                    txtCUIT.Enabled = true;
                    txtTitularCheque.Enabled = true;
                    txtCodigoPostal.Enabled = true;
                    txtNumeroSucursal.Enabled = true;
                    //btnAgregarItem.Visible = true;
                    //btnAgregarItem.Enabled = true;
                }
            }
        }

        private void PersistirDatosGrilla()
        {
            if (this.MiPrePrestamos.PrestamosCheques.Count == 0)
                return;

            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("gvDatos")).ToList();
            string k;
            int numeroFila = 2;
            PrePrestamosCheques det;
            bool modifica;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    modifica = false;
                    det = this.MiPrePrestamos.PrestamosCheques[fila.RowIndex];
                    //CurrencyTextBox txtImporte = (CurrencyTextBox)fila.FindControl("txtImporte");
                    //decimal algo = txtImporte.Decimal;

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtNumeroCheque"));
                    det.NumeroCheque = this.Request.Form[k];
                   
                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtFechaDiferido"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.FechaDiferido != Convert.ToDateTime(this.Request.Form[k]).Date)
                            modifica = true;
                        det.FechaDiferido = Convert.ToDateTime(this.Request.Form[k]);
                    }

                    k = keys.Find(x => x.Contains(numeroFila.ToString() + "$txtImporte"));
                    if (!string.IsNullOrEmpty(this.Request.Form[k]))
                    {
                        if (det.Importe != decimal.Parse(this.Request.Form[k], NumberStyles.Currency))
                            modifica = true;
                        det.Importe = decimal.Parse(this.Request.Form[k], NumberStyles.Currency);
                    }

                    DropDownList ddlBancos= (DropDownList)fila.FindControl("ddlBancos");
                    if (ddlBancos.SelectedValue!= string.Empty)
                    {
                        if (!det.IdBanco.HasValue || (det.IdBanco.HasValue && det.IdBanco.ToString() != ddlBancos.SelectedValue))
                            modifica = true;
                        det.IdBanco = ddlBancos.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(ddlBancos.SelectedValue);
                        det.BancoDescripcion = ddlBancos.SelectedItem.Text == string.Empty ? string.Empty : ddlBancos.SelectedItem.Text;
                    }

                    TextBox txtCUIT = (TextBox)fila.FindControl("txtCUIT");
                    if (txtCUIT.Text != det.CUIT)
                    {
                        modifica = true;
                        det.CUIT = txtCUIT.Text;
                    }
                    TextBox txtTitularCheque = (TextBox)fila.FindControl("txtTitularCheque");
                    if (txtTitularCheque.Text != det.TitularCheque)
                    {
                        modifica = true;
                        det.TitularCheque = txtTitularCheque.Text;
                    }
                    TextBox txtCodigoPostal = (TextBox)fila.FindControl("txtCodigoPostal");
                    if (txtCodigoPostal.Text != det.CodigoPostal)
                    {
                        modifica = true;
                        det.CodigoPostal = txtCodigoPostal.Text;
                    }
                    TextBox txtNumeroSucursal = (TextBox)fila.FindControl("txtNumeroSucursal");
                    if (txtNumeroSucursal.Text != det.NumeroSucursal)
                    {
                        modifica = true;
                        det.NumeroSucursal = txtNumeroSucursal.Text;
                    }
                    if (modifica)
                        det.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(det, this.GestionControl);
                    numeroFila++;
                }
            }
        }

        #endregion

    }
}