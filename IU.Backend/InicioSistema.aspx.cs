﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU
{
    public partial class InicioSistema : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
               
            }
        }
    }
}


