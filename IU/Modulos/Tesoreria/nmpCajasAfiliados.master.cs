using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class nmpCajasAfiliados : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                PaginaAfiliados pagina = new PaginaAfiliados();
                Maestra master = (Maestra)this.Master;
                AfiAfiliados afiliado = pagina.Obtener(master.MiSessionPagina);
                this.MapearObjetoAControles(afiliado);
            }
        }

        private void MapearObjetoAControles(AfiAfiliados pAfiliado)
        {
            this.txtAfiliado.Text = pAfiliado.ApellidoNombre;
            this.txtCategoria.Text = pAfiliado.Categoria.Categoria;
            //this.txtGrado.Text = pAfiliado.Grado.Grado;
            this.txtNumeroDocumento.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtNumeroSocio.Text = pAfiliado.NumeroSocio;
            this.txtTipoDocumento.Text = pAfiliado.TipoDocumento.TipoDocumento;
            this.txtEstado.Text = pAfiliado.Estado.Descripcion;
            //AfiDomicilios domicilio = pAfiliado.Domicilios.Find(delegate(AfiDomicilios dom) { return dom.Predeterminado; });
            //if (domicilio != null)
            //{
            //    this.txtDireccion.Text = string.Concat(domicilio.Calle, " ", domicilio.Numero, ", ", domicilio.Localidad.Descripcion);
            //}
            //this.txtCorreoElectronico.Text = pAfiliado.CorreoElectronico;
            //this.txtFechaNacimiento.Text = AyudaProgramacion.MostrarFechaPantalla(pAfiliado.FechaNacimiento);

            AyudaProgramacion.FormatoEstadoSocio(this.txtEstado, pAfiliado);
        }
    }
}
