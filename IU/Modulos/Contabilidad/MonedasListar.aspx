﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MonedasListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.MonedasListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="CapitulosListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoMoneda" runat="server" Text="Código Moneda" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCodigoMoneda" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtMoneda" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-9"></div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:BoundField HeaderText="Código Moneda" DataField="CodigoMoneda" SortExpression="CodigoMoneda" />
                        <asp:BoundField HeaderText="Moneda" DataField="Moneda" SortExpression="Moneda" />
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <center>
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" Visible="false" onclick="btnCancelar_Click" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
