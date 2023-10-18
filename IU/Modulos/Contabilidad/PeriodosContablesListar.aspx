<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PeriodosContablesListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.PeriodosContablesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PeriodosContablesListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--<asp:Label CssClass="labelEvol" ID="lblPeriodo" runat="server" Text="Período" />
            <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtPeriodo" runat="server"></AUGE:NumericTextBox>--%>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio:" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                    </div>


                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <%-- <asp:Label CssClass="labelEvol" ID="lblFechaCierreDesde" runat="server" Text="Fecha Desde" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaCierreDesde"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaCierreDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaCierreDesde" runat="server" Enabled="true" 
                    TargetControlID="txtFechaCierreDesde" PopupButtonID="imgFechaCierreDesde" Format="dd/MM/yyyy" />
            </div>
            <asp:Label CssClass="labelEvol" ID="lblFechaCierreHasta" runat="server" Text="Fecha Hasta" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaCierreHasta"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaCierreHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaCierreHasta" runat="server" Enabled="true" 
                    TargetControlID="txtFechaCierreHasta" PopupButtonID="imgFechaCierreHasta" Format="dd/MM/yyyy" />
            </div>--%>
<div class="table-responsive">
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:BoundField HeaderText="Período" DataField="Periodo" SortExpression="Periodo" />
                        <asp:BoundField HeaderText="Fecha Cierre" DataField="FechaCierre" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaCierre" />
                        <asp:TemplateField HeaderText="Ejercicio Contable" SortExpression="EjercicioContable.Descripcion">
                            <ItemTemplate>
                                <%# Eval("EjercicioContable.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                </asp:GridView></div>
                <br />
                <center>
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" Visible="false"  onclick="btnCancelar_Click" />
            </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
