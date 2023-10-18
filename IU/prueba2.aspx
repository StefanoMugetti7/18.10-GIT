<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="prueba2.aspx.cs" Inherits="IU.prueba2" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.3/css/jquery.dataTables.css" />
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.3/js/jquery.dataTables.js"></script>

    <div class="container-fluid">
        <Evol:EvolGridView ID="CustomPaging_GridView" AutoGenerateColumns="true" runat="server" OnSorting="gvDatos_Sorting"
            AllowPaging="true" AllowSorting="true" OnPageIndexChanging="GridView_PageIndexChanging" SkinID="GrillaBasicaFormal">
            <PagerSettings Mode="Numeric" Position="Bottom" PageButtonCount="5" />
        </Evol:EvolGridView>
    </div>
</asp:Content>
