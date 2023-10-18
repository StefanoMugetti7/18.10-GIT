<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegUsuariosSectoresDatos.ascx.cs" Inherits="IU.Modulos.Seguridad.Controles.SegUsuariosSectoresDatos" %>


<div class="form-group row">
<asp:Label CssClass="col-form-label col-sm-1" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
    <div class="col-sm-3">
<asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" AutoPostBack="true" 
    onselectedindexchanged="ddlFiliales_SelectedIndexChanged">
</asp:DropDownList></div>
</div>
<asp:CheckBoxList ID="chkSectores" RepeatColumns="4" RepeatDirection="Vertical" runat="server"
    AutoPostBack="true" onselectedindexchanged="chkSectores_SelectedIndexChanged">
</asp:CheckBoxList>
