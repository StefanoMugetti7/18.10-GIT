<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegMenuesModificarDatos.ascx.cs" Inherits="IU.Modulos.Seguridad.Controles.SegMenuesModificarDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="AUGE" %>
<%@ Register src="~/Modulos/Seguridad/Controles/SegMenuesModificarDatosPopUp.ascx" tagname="SegMenuesModificarDatosPopUp" tagprefix="AUGE" %>


<asp:UpdatePanel ID="upMenues" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
       <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMenu" runat="server" Text="Menu"></asp:Label>
    <div class="col-sm-3">
           <asp:TextBox CssClass="form-control" ID="txtMenu" runat="server"></asp:TextBox>
        </div>
    <div class="col-sm-3">
           <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar Item" 
        onclick="btnAgregar_Click" />
        <div class="col-sm-4">
        </div>
           </div>
           </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Nro. orden"></asp:Label>
    <div class="col-sm-3">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtOrden" runat="server"></auge:NumericTextBox>
        </div>
        <div class="col-sm-8"></div>
    </div>

<div class="form-group row">
<div class="col-6">
    <asp:TreeView ID="tvMenues" runat="server"  ImageSet="BulletedList4"
        onselectednodechanged="tvMenues_SelectedNodeChanged" ExpandDepth="0">
        <ParentNodeStyle Font-Bold="false" />
        <HoverNodeStyle Font-Underline="true" ForeColor="#5555DD" />
        <SelectedNodeStyle Font-Underline="true" ForeColor="#5555DD" 
            HorizontalPadding="0px" VerticalPadding="0px" />
        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
            HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
    </asp:TreeView>

</div>
<div class="col-6">
 <AUGE:SegMenuesModificarDatosPopUp ID="ctrPopUpMenu" Visible="false" runat="server" />
</div>
    </div>
  <center>

                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
            
        </ContentTemplate>
</asp:UpdatePanel> 