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
using Afiliados.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class nmpAfiliados : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                PaginaAfiliados pagina = new PaginaAfiliados();
                Maestra master = (Maestra)this.Master;
                this.MapearObjetoAControles(pagina.Obtener(master.MiSessionPagina));
                
                //AfiAfiliados afiliado = pagina.MiAfiliado;
                //this.MapearObjetoAControles(afiliado);
            }
        }

        private void MapearObjetoAControles(AfiAfiliados pAfiliado)
        {
            this.txtIdAfiliado.Text = pAfiliado.IdAfiliado.ToString();
            this.txtAfiliado.Text = pAfiliado.ApellidoNombre;
            this.txtCategoria.Text = pAfiliado.Categoria.Categoria;
            //this.txtGrado.Text = pAfiliado.Grado.Grado;
            this.txtNumeroDocumento.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtNumeroSocio.Text = pAfiliado.NumeroSocio;
            this.txtTipoDocumento.Text = pAfiliado.TipoDocumento.TipoDocumento;
            this.txtEstado.Text = pAfiliado.Estado.Descripcion;
            this.txtZonaGrupo.Text = pAfiliado.ZonaGrupo.Descripcion;
            //AfiDomicilios domicilio = pAfiliado.Domicilios.Find(delegate(AfiDomicilios dom) { return dom.Predeterminado; });
            //if (domicilio != null)
            //{
            //    this.txtDireccion.Text = string.Concat(domicilio.Calle, " ", domicilio.Numero, ", ", domicilio.Localidad.Descripcion);
            //}
            this.txtMatriculaIAF.Text = pAfiliado.MatriculaIAF.ToString();
            this.txtCorreoElectronico.Text = pAfiliado.CorreoElectronico;
            this.txtFechaNacimientoTitular.Text = AyudaProgramacion.MostrarFechaPantalla(pAfiliado.FechaNacimiento);
            this.txtCbu.Text = pAfiliado.CBU;
            this.txtCelular.Text = pAfiliado.PrefijoNumero;

            AyudaProgramacion.FormatoEstadoSocio(this.txtEstado, pAfiliado);
        }
    }
}
