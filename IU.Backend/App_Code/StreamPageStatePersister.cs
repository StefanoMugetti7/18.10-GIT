using System;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

namespace IU
{
    
    //
    // The StreamPageStatePersister is an example view state
    // persistence mechanism that persists view and control
    // state on the Web server.
    //
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class StreamPageStatePersister : PageStatePersister
    {
        public StreamPageStatePersister(Page page)
            : base(page)
        {
        }
        //
        // Load ViewState and ControlState.
        //
        public override void Load()
        {
            string key = HttpContext.Current.Request.QueryString["tabName"] == null ? string.Empty : HttpContext.Current.Request.QueryString["tabName"].ToString();
            key = key + this.Page.GetType().FullName;
            IStateFormatter formatter = this.StateFormatter;
            if (HttpContext.Current.Session[key] == null)
                this.Page.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);

            string VSContent = HttpContext.Current.Session[key].ToString();
            Pair statePair = (Pair)formatter.Deserialize(VSContent);
            ViewState = statePair.First;
            ControlState = statePair.Second;
        }
        //
        // Persist any ViewState and ControlState.
        //
        public override void Save()
        {
            string key = HttpContext.Current.Request.QueryString["tabName"] == null ? string.Empty : HttpContext.Current.Request.QueryString["tabName"].ToString();
            key = key + this.Page.GetType().FullName;
            Pair statePair = new Pair(ViewState, ControlState);
            IStateFormatter formatter = this.StateFormatter;
            string VSContent = formatter.Serialize(statePair);
            HttpContext.Current.Session[key] = VSContent;
        }

    }
}