using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IU
{
    /// <summary>
    /// Descripción breve de Upload
    /// </summary>
    public class Upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile uploads = context.Request.Files["upload"];
            string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
            string file = System.IO.Path.GetFileName(uploads.FileName);
            uploads.SaveAs(context.Server.MapPath(".") + "\\ImagenesCliente\\" + file);
            //provide direct URL here
            string url = string.Concat(this.ObtenerAppPath(context), "ImagenesCliente/" + file);

            context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        protected string ObtenerAppPath(HttpContext context)
        {
            return context.Request.ApplicationPath.EndsWith("/") ? context.Request.ApplicationPath : string.Concat(context.Request.ApplicationPath, "/");
        }
    }
}