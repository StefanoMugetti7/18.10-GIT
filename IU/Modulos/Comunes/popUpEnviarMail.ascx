<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="popUpEnviarMail.ascx.cs" Inherits="IU.Modulos.Comunes.popUpEnviarMail" %>
<%--<%@ Register Assembly="FreeTextBox" Namespace="FreeTextBoxControls" TagPrefix="FTB" %>--%>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<%--<script language="javascript" type="text/javascript">tinymce.init({ selector:'textarea' });</script>--%>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>


<script type="text/javascript" lang="javascript">
    function ShowModalEnviarMail(){
        $("[id$='modalEnviarMail']").modal('show');
    }

    function RemoveModalBackground() {
        $('.modal-backdrop').remove();
    }
    function HideModalEnviarMail() {
    
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalEnviarMail']").modal('hide');
    }
</script>
<div class="modal" id="modalEnviarMail" tabindex="-1" role="dialog" >
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
    <div class="modal-content modal-minHeight85">
        <div class="modal-header">
            <h5 class="modal-title">Enviar Mail</h5>
            <button type="button" class="close" data-dismiss="modal" onclick="HideModalEnviarMail();" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
    <div class="modal-body">
        <div class="form-group row">
    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblEnviarA" runat="server" Text="Enviar a"/>
            <div class="col-sm-10">
    <asp:TextBox CssClass="form-control" ID="txtEnviarA" runat="server" />
                </div>
        </div>
        <div class="form-group row">
    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblAsunto" runat="server" Text="Asunto"/>
            <div class="col-sm-10">
    <asp:TextBox CssClass="form-control" ID="txtAsunto" runat="server" />
                </div>
        </div>
        <div class="form-group row">
    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblAdjuntos" runat="server" Text="Adjuntos"/>
        </div>
        <div class="col-sm-10"></div>
        <div class="form-group row">
    <asp:UpdatePanel ID="upComprobante" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
    <asp:GridView ID="gvDatos" DataKeyNames="Name" Width="50%" ShowHeader="false"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="False"
            OnRowCommand="gvDatos_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Archivo" >
                            <ItemTemplate>
                                <%# Eval("Name")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" >
                            <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
            </div>
    <%--<asp:TextBox CssClass="form-control" ID="__txtMensaje" TextMode="MultiLine" Width="100%" Height="100%" Rows="10" runat="server" />--%>
    <div class="form-group row">
        <div class="col-sm-12">
        <CKEditor:CKEditorControl ID="txtMensaje"
        ImageRemoveLinkByEmptyURL="true" Toolbar="Full"
        runat="server"></CKEditor:CKEditorControl>
            </div>
    </div>
    </div>
        <div class="modal-footer">
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" CausesValidation="false" runat="server" Text="Aceptar" />
        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
        </div>
    </div>
    </div>
</div>
