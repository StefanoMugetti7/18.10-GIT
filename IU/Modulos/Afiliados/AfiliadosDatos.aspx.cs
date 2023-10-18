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
using Afiliados.Entidades;
using Afiliados;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosDatos : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                AfiAfiliados afiliado = new AfiAfiliados();
                afiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
                afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(afiliado);

                AfiDomicilios domicilio = afiliado.Domicilios.Find(x => x.Predeterminado);
                if (domicilio != null)
                {
                    this.txtCalle.Text = domicilio.DomicilioCalleCompleto;
                    this.txtCodigoPostal.Text = domicilio.CodigoPostal;
                    this.txtLocalidad.Text = domicilio.Localidad.Descripcion;
                    this.txtProvincia.Text = domicilio.Localidad.Provincia.Descripcion;
                }
                this.txtApellidoNombre.Text = afiliado.Apellido;
                this.txtNombre.Text = afiliado.Nombre;
                this.txtIdAfiliado.Text = afiliado.IdAfiliado.ToString();
                this.txtCantidadParticipantes.Text = afiliado.CantidadParticipantes.ToString();
                this.txtCategoria.Text = afiliado.Categoria.Categoria;
                this.txtEstado.Text = afiliado.Estado.Descripcion;
                AyudaProgramacion.FormatoEstadoSocio(this.txtEstado, afiliado);


                this.ddlTipoPersona.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TipoPersona);
                this.ddlTipoPersona.DataValueField = "IdListaValorDetalle";
                this.ddlTipoPersona.DataTextField = "Descripcion";
                this.ddlTipoPersona.DataBind();
                if (afiliado.IdTipoPersona > 0)
                    this.ddlTipoPersona.SelectedValue = afiliado.IdTipoPersona.ToString();
                this.ddlTipoPersona.SelectedValue = afiliado.IdTipoPersona.ToString();
                this.txtEstadoCivil.Text = afiliado.EstadoCivil.EstadoCivil;
                this.txtFechaIngreso.Text = AyudaProgramacion.MostrarFechaPantalla(afiliado.FechaIngreso);
                this.txtFechaNacimiento.Text = AyudaProgramacion.MostrarFechaPantalla(afiliado.FechaNacimiento);
                this.txtEdad.Text = AyudaProgramacion.CalcularEdad(afiliado.FechaNacimiento).ToString();
                this.txtFechaRetiro.Text = AyudaProgramacion.MostrarFechaPantalla(afiliado.FechaRetiro);
                this.txtZG.Text = afiliado.CodigoZonaGrupo;
                this.txtZonaGrupo.Text = afiliado.ZonaGrupo.Descripcion;
                TGEFormasPagosAfiliados formaPagoAfi = new TGEFormasPagosAfiliados();
                formaPagoAfi.IdAfiliado = afiliado.IdAfiliado;
                formaPagoAfi.Estado.IdEstado = (int)Estados.Activo;
                formaPagoAfi = TGEGeneralesF.ObtenerDatosPorAfiliado(formaPagoAfi);
                if (formaPagoAfi.IdFormaPagoAfiliado > 0)
                {
                    this.txtFormaPago.Text = formaPagoAfi.FormaPago.FormaPago;
                    this.txtFilialPago.Text = formaPagoAfi.Filial.Filial;
                    AyudaProgramacion.CargarGrillaListas<TGECampos>(formaPagoAfi.Campos, false, this.gvCamposValores, false);
                    this.tpFormasPagos.Visible = true;
                }
                this.txtGrado.Text = afiliado.Grado.Grado;
                this.txtMatriculaIAF.Text = afiliado.MatriculaIAF.ToString();
                this.txtNumeroDocumento.Text = afiliado.NumeroDocumento.ToString();
                this.txtNumeroSocio.Text = afiliado.NumeroSocio;

                this.txtTelefono.Text = afiliado.TelefonoPredeterminado;
                this.txtTipoDocumento.Text = afiliado.TipoDocumento.TipoDocumento;
                this.txtFilial.Text = afiliado.Filial.Filial;
                this.txtFechaBaja.Text = AyudaProgramacion.MostrarFechaPantalla(afiliado.FechaBaja);
                this.txtFechaSupervivencia.Text = AyudaProgramacion.MostrarFechaPantalla(afiliado.FechaSupervivencia);
                this.chkAlertasTipos.DataSource = AfiliadosF.AlertasTiposObtenerListaFiltro(new AfiAlertasTipos());
                this.chkAlertasTipos.DataTextField = "AlertaTipo";
                this.chkAlertasTipos.DataValueField = "IdAlertaTipo";
                this.chkAlertasTipos.DataBind();
                this.CargarAlertasTiposAfiliado(afiliado);

                if (!string.IsNullOrEmpty(afiliado.InformacionFamiliar))
                {
                    this.lblInformacionFamiliar.Visible = true;
                    this.lblInformacionFamiliar.Text = "El socio pertenece a la cuenta del socio: " + afiliado.InformacionFamiliar;
                }


                //this.chkFormasCobros.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
                //this.chkFormasCobros.DataValueField = "IdFormaCobro";
                //this.chkFormasCobros.DataTextField = "Descripcion";
                //this.chkFormasCobros.DataBind();
                this.CargarFormasCobroAfiliado(afiliado);

                this.txtFechaEvento.Text = this.MiAfiliado.FechaEvento.ToShortDateString();

                if (!this.MiAfiliado.MostrarMensajesAlertas)
                {
                    //afiliado.MostrarMensajesAlertas = true;
                    this.ctrMensajesAlertas.IniciarControl(this.MiAfiliado);
                }

                this.ctrComentarios.IniciarControl(afiliado, Gestion.Consultar);
                this.ctrArchivos.IniciarControl(afiliado, Gestion.Consultar);
                this.ctrCamposValores.IniciarControl(afiliado, new Objeto(), Gestion.Consultar);
            }
        }

        /// <summary>
        /// Marca en Pantalla las Alertas Tipos que tiene el Afiliado
        /// </summary>
        /// <param name="pAfiliado"></param>
        private void CargarAlertasTiposAfiliado(AfiAfiliados pAfiliado)
        {
            foreach (AfiAlertasTipos alerta in pAfiliado.AlertasTipos)
            {
                foreach (ListItem item in chkAlertasTipos.Items)
                {
                    if (Convert.ToInt32(item.Value) == alerta.IdAlertaTipo)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Marca en Pantalla las Formas de Cobro que tiene el Afiliado
        /// </summary>
        /// <param name="pAfiliado"></param>
        private void CargarFormasCobroAfiliado(AfiAfiliados pAfiliado)
        {
            TGEFormasCobrosAfiliados formasCobroAfi = new TGEFormasCobrosAfiliados();
            formasCobroAfi.IdAfiliado = pAfiliado.IdAfiliado;
            formasCobroAfi.Estado.IdEstado = (int)Estados.Activo;
            List<TGEFormasCobrosAfiliados> lista = TGEGeneralesF.FormasCobrosAfiliadosObtenerListaFiltro(formasCobroAfi);
            this.chkFormasCobros.DataSource = lista;
            this.chkFormasCobros.DataValueField = "FormaCobroIdFormaCobro";
            this.chkFormasCobros.DataTextField = "FormaCobroDescripcion";
            this.chkFormasCobros.DataBind();

            foreach (TGEFormasCobrosAfiliados formaCobro in lista)
            {
                foreach (ListItem item in this.chkFormasCobros.Items)
                {
                    if (Convert.ToInt32(item.Value) == formaCobro.FormaCobro.IdFormaCobro)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        //protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        TGECampos campo = (TGECampos)e.Row.DataItem;
                
        //        if(campo.CampoTipo.IdCampoTipo==(int)EnumCamposTipos.DropDownList)
        //            e.Row.Cells
        //    }
        //}
    }
}
