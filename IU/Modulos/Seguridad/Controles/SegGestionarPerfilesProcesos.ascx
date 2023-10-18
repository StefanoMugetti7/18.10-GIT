<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegGestionarPerfilesProcesos.ascx.cs" Inherits="IU.Modulos.Seguridad.Controles.SegGestionarPerfilesProcesos" %>
<div class="SegGestionarPerfilesProcesos">
<asp:TreeView ID="tvProcesos" runat="server" ImageSet="BulletedList4" 
    ShowCheckBoxes="All">
    <ParentNodeStyle Font-Bold="false" />
    <HoverNodeStyle Font-Underline="true" ForeColor="#5555DD" />
    <SelectedNodeStyle Font-Underline="true" ForeColor="#5555DD" 
        HorizontalPadding="0px" VerticalPadding="0px" />
    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
</asp:TreeView>
</div>