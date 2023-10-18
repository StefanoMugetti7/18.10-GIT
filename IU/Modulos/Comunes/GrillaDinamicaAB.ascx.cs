using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Comunes
{
    public partial class GrillaDinamicaAB : ControlesSeguros
    {
        //[Category("EvolProperties")]
        ////[DefaultValue(2)]
        //[Description("StoredProcedure")]
        //public int NumberOfDecimals
        //{
        //    get
        //    {
        //        return m_NumberOfDecimals;
        //    }
        //    set
        //    {
        //        m_NumberOfDecimals = value;
        //    }
        //}

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(TGECampos pCampo, bool pHabilitar)
        {
            
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {

        }

        private void CargarGrilla()
        {
            
        }
    }
}