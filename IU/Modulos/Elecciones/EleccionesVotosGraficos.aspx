<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="EleccionesVotosGraficos.aspx.cs" Inherits="IU.Modulos.Elecciones.EleccionesVotosGraficos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
 
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    <div class="EleccionesGraficos">
        <div class="form-group row">
        <div class="col-lg-3 col-md-3 col-sm-9">
               <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblInfoEleccion" runat="server" Text="."></asp:Label>

            </div>

               <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoLista" runat="server" Text="Tipo Lista"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoLista" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoLista_SelectedIndexChanged" runat="server"></asp:DropDownList>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblRegion" runat="server" visible="false" Text="Region"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlRegion" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" Enabled="false" visible="false" runat="server"></asp:DropDownList>
        </div>
            </div>
        <center>
         <div id="piechart_3d" style="width: 800px; height: 400px;"></div>
        </center>
        <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
