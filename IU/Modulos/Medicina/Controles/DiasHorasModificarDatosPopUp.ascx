<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiasHorasModificarDatosPopUp.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.DiasHorasModificarDatosPopUp" %>
<script type="text/javascript" lang="javascript">
    function ShowModalDiasHoras(){
        $("[id$='modalDiasHoras']").modal('show');
    }

    function HideModalDiasHoras() {
         $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalDiasHoras']").modal('hide');
    }
</script><%--EspecializacionesModificarDatos--%>

   <div class="modal" id="modalDiasHoras" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Dias Horas</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                             <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
            <div class="col-sm-3">    <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFiliales" ControlToValidate="ddlFiliales" ValidationGroup="DiasHorasModificarDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
              </div>

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDia" runat="server" Text="Dia"></asp:Label>
                            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlDia" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDia" ControlToValidate="ddlDia" ValidationGroup="DiasHorasModificarDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
       </div>
   <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
             <div class="col-sm-3">   <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" >
                </asp:DropDownList></div>
       </div>
                        <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoraDesde" runat="server" Text="Hora Desde"></asp:Label>
                 <div class="col-sm-3"> <asp:TextBox CssClass="form-control hourpicker" ID="txtHoraDesde" runat="server"></asp:TextBox>
                <asp:MaskedEditExtender ID="meeStartTime" runat="server" AcceptAMPM="true" MaskType="Time"
                                        Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                        ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtHoraDesde"
                                        InputDirection="LeftToRight" AcceptNegative="Left">
                </asp:MaskedEditExtender>
                <asp:MaskedEditValidator ID="mevStartTime" runat="server" ControlExtender="meeStartTime"
                    ControlToValidate="txtHoraDesde" IsValidEmpty="false" EmptyValueMessage="*"
                    InvalidValueMessage="Time is invalid" Display="Dynamic" EmptyValueBlurredText="*"
                    InvalidValueBlurredMessage="*" ValidationGroup="DiasHorasModificarDatos" CssClass="Validador" />
       </div>
                            
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoraHasta" runat="server" Text="Hora hasta"></asp:Label>
            <div class="col-sm-3">    <asp:TextBox CssClass="form-control " ID="txtHoraHasta" runat="server"></asp:TextBox>
                <asp:MaskedEditExtender ID="meeEndTime" runat="server" AcceptAMPM="true" MaskType="Time"
                                        Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                        ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtHoraHasta"
                                        InputDirection="LeftToRight" AcceptNegative="Left">
                </asp:MaskedEditExtender>
                <asp:MaskedEditValidator ID="mevEndTime" runat="server" ControlExtender="meeEndTime"
                    ControlToValidate="txtHoraHasta" IsValidEmpty="false" EmptyValueMessage="*"
                    InvalidValueMessage="*" Display="Dynamic" EmptyValueBlurredText="*"
                    InvalidValueBlurredMessage="*" ValidationGroup="DiasHorasModificarDatos" CssClass="Validador" />
        </div>
    
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTiempo" runat="server" Text="Tiempo"></asp:Label>
              <div class="col-sm-3">  <AUGE:NumericTextBox CssClass="form-control " ID="txtTiempo" runat="server" ></AUGE:NumericTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTiempo" ControlToValidate="txtTiempo" ValidationGroup="DiasHorasModificarDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
               </div>
                             <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEspecialidad" runat="server" Text="Especialidad"></asp:Label>
               <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlEspecialidad" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEspecialidad" ControlToValidate="ddlEspecialidad" ValidationGroup="Especialidad" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
               </div>
                                 <div class="col-sm-1">
                   <asp:Button CssClass="botonesEvol" ID="btnAgregarEspecialidad" runat="server" Text="Agregar Especialidad" 
                    onclick="btnAgregarEspecialidad_Click" ValidationGroup="Especialidad" />         
              </div>
          </div>
                        <div class="data-table">
 <asp:GridView ID="gvEspecializaciones" OnRowCommand="gvEspecializaciones_RowCommand" 
            OnRowDataBound="gvEspecializaciones_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="IdEspecializacion" SortExpression="">
                            <ItemTemplate><%# Eval("IdEspecializacion")%>

                            </ItemTemplate>
                                   
                 
                        </asp:TemplateField>       <asp:TemplateField HeaderText="Especializacion" SortExpression="">  
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfIdEspecializacion" Value='<%# Eval("IdEspecializacion") %>' runat="server" />

                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Especializacion" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Eliminar" ID="btnEliminar"
                                        AlternateText="Eliminar Registro" ToolTip="Eliminar Registro" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>   
</div>

  </div>
                        <div class="modal-footer">        
       <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="DiasHorasModificarDatos" />
                 <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" 
                    onclick="btnCancelar_Click" />
        </div>
        
                  
                </div> </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>

