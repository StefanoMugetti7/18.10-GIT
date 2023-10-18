using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class ClientesDatosCabecera : ControlesSeguros
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(AfiAfiliados pParametro)
        {
            this.MapearObjetoAControles(pParametro);
        }

        private void MapearObjetoAControles(AfiAfiliados pAfiliado)
        {
            this.txtIdAfiliado.Text = pAfiliado.IdAfiliado.ToString();
            this.txtEstado.Text = pAfiliado.Estado.Descripcion;
            this.txtApellido.Text = pAfiliado.Apellido;
            this.txtTipoDocumento.Text = pAfiliado.TipoDocumento.TipoDocumento;
            this.txtNumeroDocumento.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtCorreoElectronico.Text = pAfiliado.CorreoElectronico;
            this.txtCondicionesFiscales.Text = pAfiliado.CondicionFiscal.Descripcion;
        }

        ///// <summary>
        ///// Mapea los controles de Pantalla con la Entidad SolicitudesMateriales.
        ///// </summary>
        ///// <param name="pRequisicion"></param>
        //private void MapearControlesAObjeto(AfiAfiliados pAfiliado)
        //{
        //    pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
        //    pAfiliado.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
        //    pAfiliado.Apellido = this.txtApellido.Text;
        //    pAfiliado.TipoDocumento.IdTipoDocumento = this.ddlTipoDocumento.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
        //    pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
        //    pAfiliado.CorreoElectronico = this.txtCorreoElectronico.Text;
        //    pAfiliado.CondicionFiscal.IdCondicionFiscal = this.ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscal.SelectedValue);
        //}
    }
}