using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace IU
{
    /// <summary>
    /// Methods to set default button
    /// </summary>
    public class BotonesPorDefecto
    {
        /// <summary>
        /// Sets default button for the specified control
        /// </summary>
        /// <param name="control">Control name where user hits enter key</param>
        /// <param name="btButton">Button control to be called</param>
        public static void EstablecerBotonPorDefecto(TextBox control, Button btButton)
        {
            control.Attributes.Add("onkeypress", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btButton.ClientID + "').click();return false;}} else {return true}; ");
            ////string js="if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById(@\'" + btButton.ClientID + "@\').click();return false;}} else {return true}; ";
            //string js = string.Concat(" return BotonesPorDefecto(", btButton.ClientID, ", event);");
            //if (!string.IsNullOrEmpty(control.Attributes["onkeydown"]))
            //    control.Attributes["onkeydown"] = string.Concat(control.Attributes["onkeydown"], ", ", js);
            //control.Attributes.Add("onkeydown", js);
        }

        /// <summary>
        /// Sets default button as the image button
        /// </summary>
        /// <param name="control">Control name where user hits enter key</param>
        /// <param name="btButton">Imagebutton to be called</param>
        public static void EstablecerBotonPorDefecto(TextBox control, ImageButton btButton)
        {
            control.Attributes.Add("onkeypress", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btButton.UniqueID + "').click();return false;}} else {return true}; ");
            ////string js = @"if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById(\'" + btButton.UniqueID + "\').click();return false;}} else {return true}; ";
            //string js = string.Concat(" return BotonesPorDefecto(", btButton.ClientID, ", event);");
            //if (!string.IsNullOrEmpty(control.Attributes["onkeydown"]))
            //    control.Attributes["onkeydown"] = string.Concat(control.Attributes["onkeydown"], ", ", js);
            //control.Attributes.Add("onkeydown", js);
        }
    }
}
