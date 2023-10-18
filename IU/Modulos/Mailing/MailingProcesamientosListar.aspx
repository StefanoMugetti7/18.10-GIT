<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MailingProcesamientosListar.aspx.cs" Inherits="IU.Modulos.Mailing.MailingProcesamientosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group row">
        
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProceso" runat="server" Text="Proceso"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlProceso"  runat="server" OnSelectedIndexChanged="ddlProceso_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvProceso" ControlToValidate="ddlProceso" ValidationGroup="MailingPrepararEnvio" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

    </div>
        <div class="col-sm-1">
            
            <asp:Button CssClass="botonesEvol" runat="server" ID="btnAgregar" Text="Agregar Envio"
                OnClick="btnAgregar_Click" />
        </div>
    </div>
     <div class="table-responsive">
                                    <asp:GridView ID="gvDetalleEnvio" OnRowCommand="gvDetalleEnvio_RowCommand"
                                        DataKeyNames="IdMailingProcesamiento"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                        <Columns>
                                            
                                            <asp:BoundField HeaderText="IdMailingProcesamiento" DataField="IdMailingProcesamiento" SortExpression="IdMailingProcesamiento" />
                                            <asp:BoundField HeaderText="Proceso" DataField="MailingDescripcion" SortExpression="MailingProcesamiento" />
                                            <asp:BoundField HeaderText="Fecha" DataField="Fecha" SortExpression="Fecha" />
                                           <%-- <asp:BoundField HeaderText="Usuario" DataField="Usuario" SortExpression="Usuario" />--%>
                                                  <asp:TemplateField HeaderText="Usuario" SortExpression="Usuario">
                                                <ItemTemplate>
                                                    <%# Eval("Usuario")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" SortExpression="Cantidad" />
                                        <%--    <asp:TemplateField HeaderText="Fecha Solicitado" SortExpression="Localidad.Descripcion">
                                                <ItemTemplate>
                                                    <%# Eval("MailingProcesamiento.Fecha")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            
                                            <asp:BoundField HeaderText="Cantidad Enviada" DataField="CantidadEnviada" SortExpression="CantidadEnviada" />
                                            <asp:BoundField HeaderText="Cantidad Error" DataField="CantidadError" SortExpression="CantidadError" />
                                                       
                                                  <asp:BoundField HeaderText="Cantidad Pendiente" DataField="CantidadPendiente" SortExpression="CantidadPendiente" />
                                            <asp:TemplateField HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="true"
                                                        AlternateText="Consultar" ToolTip="Consultar" />
                                              
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
</asp:Content>
