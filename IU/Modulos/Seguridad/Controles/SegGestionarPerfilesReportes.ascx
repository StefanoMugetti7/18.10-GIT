<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegGestionarPerfilesReportes.ascx.cs" Inherits="IU.Modulos.Seguridad.Controles.SegGestionarPerfilesReportes" %>
<div class="SegGestionarPerfilesReportes">
<asp:TreeView ID="tvReportes" runat="server" ImageSet="BulletedList4" 
    ShowCheckBoxes="All">
    <ParentNodeStyle Font-Bold="false" />
    <HoverNodeStyle Font-Underline="true" ForeColor="#5555DD" />
    <SelectedNodeStyle Font-Underline="true" ForeColor="#5555DD" 
        HorizontalPadding="0px" VerticalPadding="0px" />
    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
</asp:TreeView>
</div>