<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MailingEnvioManual.aspx.cs" Inherits="IU.Modulos.Mailing.MailingEnvioManual" %>
<%@ Register Src="~/Modulos/Mailing/Controles/MailingVistaPrevia.ascx" TagPrefix="AUGE" TagName="VistaPrevia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<%--<script language="javascript" type="text/javascript">
     $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(habilitarcheckbox);
        habilitarcheckbox();
});
    function habilitarcheckbox() {
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var chkincluir = $(this).find('input:checkbox');
            chkincluir.prop("disabled", false);
        });
    }
</script>--%>
      <asp:UpdatePanel ID="UpdatePanel2" RenderMode="Inline" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:VistaPrevia ID="ctrVistaPrevia" runat="server" />
    <div class="table-responsive">
        <asp:GridView ID="gvDatos" AllowPaging="false" OnRowCreated="gvDatos_RowCreated"
            OnRowCommand="gvDatos_RowCommand"
                OnRowDataBound="gvDatos_RowDataBound"  DataKeyNames="IdMailEnvio"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="true" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Incluir" >
                      <ItemTemplate>
                                        <asp:CheckBox ID="chkIncluir" runat="server" />
                          <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                            AlternateText="Vista previa" ToolTip="Vista previa" />
                                    </ItemTemplate>
                    </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Proceso">
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
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
    </div>

    <div class="row justify-content-md-center">
    <div class="col-md-auto">
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
            OnClick="btnAceptar_Click" ValidationGroup="MailingAceptar" />
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
            OnClick="btnCancelar_Click" />
    </div>
</div>
                </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
