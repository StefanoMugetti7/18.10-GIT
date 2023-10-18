using Comunes.Entidades;
using Generales.FachadaNegocio;
using Google.Authenticator;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Seguridad
{
    public partial class SegGoogleAuthenticator : PaginaSegura
    {
        String AuthenticationCode
        {
            get
            {
                if (ViewState["AuthenticationCode"] != null)
                    return ViewState["AuthenticationCode"].ToString().Trim();
                return String.Empty;
            }
            set
            {
                ViewState["AuthenticationCode"] = value.Trim();
            }
        }

        String AuthenticationTitle
        {
            get
            {
                return "Evol Sistemas Informáticos";
            }
        }


        String AuthenticationBarCodeImage
        {
            get;
            set;
        }

        String AuthenticationManualCode
        {
            get;
            set;
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!Page.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(txtCodigoVerificacion, btnVerificar);
                //lblResult.Text = String.Empty;
                //lblResult.Visible = false;
                GenerateTwoFactorAuthentication();
                imgQrCode.ImageUrl = AuthenticationBarCodeImage;
                lblManualSetupCode.Text = AuthenticationManualCode;
                //lblAccountName.Text = AuthenticationTitle;
            }
        }

        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            String pin = txtCodigoVerificacion.Text.Trim();
            Boolean status = ValidateTwoFactorPIN(pin);
            dvIncorrecto.Visible= false;
            dvResultado.Visible= false;
            if (status)
            {
                dvResultado.InnerHtml = "Código de verificación correcto.";
                dvResultado.Visible = true;
            }
            else
            {
                dvIncorrecto.InnerHtml= "Código inválido.";
                dvIncorrecto.Visible = true;
            }
        }

        public Boolean ValidateTwoFactorPIN(String pin)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            TimeSpan ts = new TimeSpan(0,1,0);
            return tfa.ValidateTwoFactorPIN(AuthenticationCode, pin, ts);
        }

        public Boolean GenerateTwoFactorAuthentication()
        {
            TGEListasValores lv = new TGEListasValores();
            lv.CodigoValor = "TipoAutenticacion";
            List<TGEListasValoresDetalles> lista = TGEGeneralesF.ListasValoresObtenerListaDetalle(lv);
            TGEListasValoresDetalles lvd = lista.FirstOrDefault(x => x.CodigoValor == "GoogleAuthenticator");
            if (lvd.ConsultaDinamicaCombo.Trim() == String.Empty)
            {
                this.MostrarMensaje("Falta definir la clave para Google Autenticator!", true);
                return false;
            }
            AuthenticationCode = lvd.ConsultaDinamicaCombo.Trim();

            Dictionary<String, String> result = new Dictionary<String, String>();
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("EvolApp", UsuarioEmpresa.Empresa, AuthenticationCode, false);
            if (setupInfo != null)
            {
                AuthenticationBarCodeImage = setupInfo.QrCodeSetupImageUrl;
                AuthenticationManualCode = setupInfo.ManualEntryKey;
                return true;
            }
            return false;
        }
    }
}