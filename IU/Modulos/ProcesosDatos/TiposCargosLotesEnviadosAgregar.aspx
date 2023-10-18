<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="TiposCargosLotesEnviadosAgregar.aspx.cs" Inherits="IU.Modulos.ProcesosDatos.TiposCargosLotesEnviadosAgregar" Title="" %>
<%@ Register Src="~/Modulos/ProcesosDatos/Controles/TiposCargosLotesEnviadosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-maskmoney/3.0.1/jquery.maskMoney.min.js" integrity="sha512-0P/3WcVoLIAGw88XIgLfEe2oWNKFmW0iffc89QxqITfoPJVHUBBYnBj+ikkad+K76Z6NkLNIJttdi1LmiubfeA==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceEncabezado" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceIzquierdoArriba" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceCentroArriba" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceDerechaArriba" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceIzquierdoCentro" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModifDatos" runat="server" />
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentPlaceDerechaCentro" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ContentPlaceIzquierdoAbajo" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="ContentPlaceDerechaAbajo" runat="server">
</asp:Content>
