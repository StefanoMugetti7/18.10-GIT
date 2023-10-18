<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="Integracion.aspx.cs" Inherits="IU.Modulos.Wordpress.Integracion" Async="true"%>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" Width="100%" ID="btn1" runat="server" Text="Actualiza Precio de Productos en el Portal" CausesValidation="false" onclick="btnAgregarProductos" />
                    </div>
                    <asp:Label CssClass="col-sm-9 col-form-label" ID="lblResultadoProducto" runat="server" />
                </div>
                <div class="form-group row"></div>
                <div class="form-group row">
                    <div class="col-sm-3">
                       <asp:Button CssClass="botonesEvol" Width="100%" ID="Button1" runat="server" Text="Importar Pedidos desde el Portal" CausesValidation="false" onclick="btnImportarCompras" />
                    </div>
                    <asp:Label CssClass="col-sm-9 col-form-label" ID="lblResultadoCompras" runat="server" />
                </div>
                </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
