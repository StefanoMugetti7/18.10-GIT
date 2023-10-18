<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnviarMails.ascx.cs" Inherits="IU.Modulos.Comunes.EnviarMails" %>

<script type="text/javascript">
   
</script>

<div class="EnviarMails">
<asp:Panel ID="pnlAsientoMostrar" Visible="true" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
<asp:Accordion
    ID="MyAccordion"
    runat="Server"
    SelectedIndex="-1"
    HeaderCssClass="accordionHeader"
    HeaderSelectedCssClass="accordionHeaderSelected"
    ContentCssClass="accordionContent"
    AutoSize="None"
    FadeTransitions="true"
    TransitionDuration="250"
    FramesPerSecond="40"
    RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">
    <Panes>
        <asp:AccordionPane
            HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>Enviar Correos</Header>
            <Content>
                   <center>
                    <br />
                    <div id="divInicio" ></div>
                    <div id="divStatus" ></div>
                    </center>
                    <br />
                       <div id="myProgress" class="myProgress">
                              <div id="myBar" class="myBar"></div>
                        </div>
                    <br /> 
                    <center>
                    <asp:UpdatePanel ID="upBtnEnviarMails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                       <input type="button" class="botonesEvol" value="Enviar mails" onclick="fnEmpezarProcesoEnviarMails();"  ID="btnEnviarMailAceptar" />
                       <asp:Button CssClass="botonesEvol" ID="btnEnviarMailProceso" OnClick="btnEnviarMailProceso_Click" runat="server" CausesValidation="false" 
                     Text="Procesar Hidden" style="display:none"/>
                       <input type="button" class="botonesEvol" value="Cancelar proceso"  ID="btnEnviarMailCancelar" />
                        <%--<asp:Button CssClass="botonesEvol" ID="btnEnviarMailAceptar" runat="server" Text="Enviar mails"  />--%>
                        <%--<asp:Button CssClass="botonesEvol" ID="btnEnviarMailCancelar" runat="server" Text="Cancelar proceso"  />--%>
                        <asp:Button CssClass="botonesEvol" runat="server" ID="btnFinalizar" Text="" style="display:none;" OnClick="btnFinalizar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>  
            </Content>
        </asp:AccordionPane>        
    </Panes>            
</asp:Accordion>
            </div>
        </div>
</asp:Panel>
</div>    