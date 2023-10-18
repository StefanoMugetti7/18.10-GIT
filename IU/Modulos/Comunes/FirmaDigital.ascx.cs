using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Generales.Entidades;
using Comunes.LogicaNegocio;
using System.IO;
using System.Text;
using jSignature.Tools;
using Svg;

namespace IU.Modulos.Comunes
{
    public partial class FirmaDigital : ControlesSeguros //System.Web.UI.UserControl
    {
        public delegate void popUpAceptarEventHandler(TGEArchivos archivo);
        public event popUpAceptarEventHandler popUpAceptar;
        //public delegate void popUpCancelarEventHandler();
        //public event popUpCancelarEventHandler popUpCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
               
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            TGEArchivos archivo = new TGEArchivos();
            string[] valores = this.hiddenSigData.Value.Split(',');
            if (valores.Length >= 1)
            {
                archivo.Archivo = AyudaProgramacionLN.ImageToByteArray( this.GetSignaturePNG(valores[1]));
                //archivo.Archivo = AyudaProgramacionLN.GetBytes(valores[1]);
            }
            this.mpePopUp.Hide();
            if (this.popUpAceptar != null)
                this.popUpAceptar(archivo);
        }

        //protected void btnVolver_Click(object sender, EventArgs e)
        //{
        //    this.mpePopUp.Hide();
        //    if (this.popUpCancelar != null)
        //        this.popUpCancelar();
        //}

        public void IniciarControl()
        {
            this.mpePopUp.Show();
        }

        private System.Drawing.Image GetSignaturePNG(string pStream_to_convert)
        {
            Base30Converter conv = new Base30Converter();
            int[][][] arrBase30Data = conv.GetData(pStream_to_convert);

            string strSVG = jSignature.Tools.SVGConverter.ToSVG(arrBase30Data);

            var s = new MemoryStream(Encoding.ASCII.GetBytes(strSVG));
            SvgDocument mySVG = SvgDocument.Open(s, null);

            var tempStream = new System.IO.MemoryStream();
            mySVG.Draw().Save(tempStream, System.Drawing.Imaging.ImageFormat.Png);
            System.Drawing.Image img = System.Drawing.Image.FromStream(tempStream);
            return img;
        }
    }
}