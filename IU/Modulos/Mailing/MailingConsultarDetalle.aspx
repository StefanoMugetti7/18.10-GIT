<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MailingConsultarDetalle.aspx.cs" Inherits="IU.Modulos.Mailing.MailingConsultarDetalle" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" RenderMode="Inline" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
     <div class="form-group row">

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFiltro" runat="server" Text="Filtro" />
                <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtFiltro" runat="server" />
                    </div>
                
 <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
   
     </div></div>
  
                            <div class="table-responsive">
                                    <asp:GridView ID="gvDetalleEnvio" OnRowCommand="gvDetalleEnvio_RowCommand"
                                
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                        <Columns>
                                            
                                            <asp:BoundField HeaderText="IdMailEnvio" DataField="IdMailEnvio" SortExpression="IdMailEnvio" />
                                            <asp:BoundField HeaderText="Para" DataField="Para" SortExpression="Para" />
                                           <%-- <asp:BoundField HeaderText="Usuario" DataField="Usuario" SortExpression="Usuario" />--%>
                                                  <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                                                <ItemTemplate>
                                                    <%# Eval("Nombre")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Asunto" DataField="Asunto" SortExpression="Asunto" />
                                        <%--    <asp:TemplateField HeaderText="Fecha Solicitado" SortExpression="Localidad.Descripcion">
                                                <ItemTemplate>
                                                    <%# Eval("MailingProcesamiento.Fecha")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            
                                            <asp:BoundField HeaderText="Fecha Solicitado" DataField="FechaSolicitado" SortExpression="FechaSolicitado" />
                                            <asp:BoundField HeaderText="Fecha Envio" DataField="FechaEnvio" SortExpression="FechaEnvio" />
                                                  <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                                                <ItemTemplate>
                                                    <%# Eval("Estado")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>      
                                                  <asp:BoundField HeaderText="Observacion" DataField="Observacion" SortExpression="Observacion" />
                                           
                                        </Columns>
                                    </asp:GridView>
                                </div>
      <div class="row justify-content-md-center">
            <div class="col-md-auto">

        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div></div>
            </ContentTemplate>
            </asp:UpdatePanel>
                      </asp:Content>