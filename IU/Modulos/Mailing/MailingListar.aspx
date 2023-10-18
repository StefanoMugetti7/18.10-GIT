<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MailingListar.aspx.cs" Inherits="IU.Modulos.Mailing.MailingListar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group row">
      <%--  <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
        </div>--%>
        <div class="col-sm-11"></div>
        <div class="col-sm-1">
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
        </div>
        </div>

    <div class="table-responsive">
        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdMailing"
            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderText="Proceso">
                    <ItemTemplate>
                        <%# Eval("MailingProcesos.IdMailingProceso")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descripcion">
                    <ItemTemplate>
                        <%# Eval("MailingProcesos.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha Inicio">
                    <ItemTemplate>
                        <%# Eval("FechaInicio", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Periocidad">
                    <ItemTemplate>
                        <%# Eval("ListasValoresDetalles.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dia de Ejecucion">
                    <ItemTemplate>
                        <%# Eval("DiaEjecucion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                            AlternateText="Imprimir Comprobante" Visible="false" ToolTip="Imprimir Comprobante" />
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                            AlternateText="Ver" ToolTip="Mostrar" />
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                            AlternateText="Modificar" ToolTip="Modificar" />
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja"
                            AlternateText="Anular" Visible="false" ToolTip="Anular" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
