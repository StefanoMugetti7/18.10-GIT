<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PruebaBootstrap.aspx.cs" Inherits="IU.PruebaBootstrap" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFehcaNacimiento" >    
            <div class="row">
        <asp:Label CssClass="col-sm-4 col-form-label"  AssociatedControlID="txtFechaNacimiento" ID="lblFechaNacimiento" runat="server" Text="Fecha Nacimiento"></asp:Label>
    <div class="col-sm-8">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaNacimiento" TabIndex="11" runat="server"></asp:TextBox>
    </div>
            </div>
            </div>
         <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div1" visible="true">    
            <div class="row">
        <asp:Label CssClass="col-sm-4 col-form-label"  AssociatedControlID="txtFechaNacimiento" ID="Label1" runat="server" Text="Fecha div1"></asp:Label>
    <div class="col-sm-8">
                <asp:TextBox CssClass="form-control datepicker" ID="TextBox1" TabIndex="11" runat="server"></asp:TextBox>
    </div>
            </div>
            </div>
         <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div2" visible="false">    
            <div class="row">
        <asp:Label CssClass="col-sm-4 col-form-label"  AssociatedControlID="txtFechaNacimiento" ID="Label2" runat="server" Text="Fecha div2"></asp:Label>
    <div class="col-sm-8">
                <asp:TextBox CssClass="form-control datepicker" ID="TextBox2" TabIndex="11" runat="server"></asp:TextBox>
    </div>
            </div>
            </div>
         <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div3" visible="true">    
            <div class="row">
        <asp:Label CssClass="col-sm-4 col-form-label"  AssociatedControlID="txtFechaNacimiento" ID="Label3" runat="server" Text="Fecha div3"></asp:Label>
    <div class="col-sm-8">
                <asp:TextBox CssClass="form-control datepicker" ID="TextBox3" TabIndex="11" runat="server"></asp:TextBox>
    </div>
            </div>
            </div>
         <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div4" visible="true">    
            <div class="row">
        <asp:Label CssClass="col-sm-4 col-form-label"  AssociatedControlID="txtFechaNacimiento" ID="Label4" runat="server" Text="Fecha div4"></asp:Label>
    <div class="col-sm-8">
                <asp:TextBox CssClass="form-control datepicker" ID="TextBox4" TabIndex="11" runat="server"></asp:TextBox>
    </div>
            </div>
            </div>
         <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div5" visible="true">    
            <div class="row">
        <asp:Label CssClass="col-sm-4 col-form-label"  AssociatedControlID="txtFechaNacimiento" ID="Label5" runat="server" Text="Fecha div5"></asp:Label>
    <div class="col-sm-8">
                <asp:TextBox CssClass="form-control datepicker" ID="TextBox5" TabIndex="11" runat="server"></asp:TextBox>
    </div>
            </div>
            </div>
    <div class="col-sm-4">Segunda columna con texto largo</div>
        <div class="col-sm-4">Tercera columna</div>
    <div class="col-sm-4">cuarta columna</div>
        <div class="col-sm-4">5 columna</div>
    <div class="col-sm-4">.col-6</div>
    <div class="col-sm-4">.col-6</div>
  </div>
</asp:Content>
