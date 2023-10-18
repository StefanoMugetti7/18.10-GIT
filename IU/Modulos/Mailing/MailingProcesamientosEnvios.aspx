<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MailingProcesamientosEnvios.aspx.cs" Inherits="IU.Modulos.Mailing.MailingProcesamientosEnvios" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Mailing/Controles/MailingVistaPrevia.ascx" TagPrefix="AUGE" TagName="VistaPrevia" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
    <script language="javascript" type="text/javascript">


        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(AgregarVariableGobal);
            SetTabIndexInput();
            AgregarVariableGobal();

        });

        var gridViewId = '#<%= gvDatos.ClientID %>';
        var NecesarioPrepararEnvio;
        function checkAllRow(selectAllCheckbox) {
            //get all checkboxes within item rows and select/deselect based on select all checked status
            //:checkbox is jquery selector to get all checkboxes
            $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
            updateSelectionLabel();
        }

        function CheckRow(selectCheckbox) {
            //if any item is unchecked, uncheck header checkbox as well
            if (!selectCheckbox.checked)
                $('th :checkbox', gridViewId).prop("checked", false);
            updateSelectionLabel();
        }
        function SetTabIndexInput() {
            $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }
        function updateSelectionLabel() {
            //update the caption element with the count of selected items. 
            //:checked is jquery selector to get list of checked checkboxes
            $('caption', gridViewId).html($('td :checkbox:checked', gridViewId).length + " options selected");
        }

        function AgregarVariableGobal() {
            $(":input").each(function (i) {
                $(this).change(function () { NecesarioPrepararEnvio = 0; });
                $("[id$='btnPrepararEnvio']").click(function () { NecesarioPrepararEnvio = 0; });
                //$("[id$='btnAceptar']").click(function() {
                //    if (NecesarioPrepararEnvio = 1) {
                //        MostrarMensaje("Se han realizado cambios en la pantalla, debe volver a hacer la preparación de envío.");
                //        return false;
                //    }
                //});

            });
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>     
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProceso" runat="server" Text="Proceso"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlProceso" runat="server" OnSelectedIndexChanged="ddlProceso_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvProceso" ControlToValidate="ddlProceso" ValidationGroup="MailingPrepararEnvio" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPlantilla" runat="server" Text="Plantilla"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlPlantilla" OnSelectedIndexChanged="ddlPlantilla_OnSelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                               <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPlantilla" ControlToValidate="ddlPlantilla" ValidationGroup="MailingPrepararEnvio" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                    </div>
                <div class="col-sm-1">
                    <asp:Button CssClass="botonesEvol" runat="server" ID="btnModificarCopia" Text="Editar Plantilla"
                        OnClick="btnModificarCopiaPlantilla_Click" />
                </div>
                <div class="col-sm-1">
                    <asp:Button CssClass="botonesEvol" runat="server" ID="btnPrepararEnvio" ValidationGroup="MailingPrepararEnvio" Text="Preparar Envio"
                        OnClick="btnPrepararEnvio_Click" />
                </div>
                <div class="col-sm-2">
                    <asp:Button CssClass="botonesEvol" runat="server" ID="btnPruebaEnvio" Visible="false" Text="Prueba Envio"
                        OnClick="btnPruebaEnvio_Click" />
                </div>

            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAsunto" runat="server" Text="Asunto"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtAsunto" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvAsunto" ControlToValidate="txtAsunto" ValidationGroup="MailingPrepararEnvio" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAdjuntos" runat="server" Text="Adjuntos"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlAdjuntos" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvAdjuntos" ControlToValidate="ddlAdjuntos" ValidationGroup="MailingAdjuntos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
            </div>
            <asp:Panel ID="tablaParametros" CssClass="form-group" runat="server">
            </asp:Panel>
            <div class="table-responsive">
                <asp:GridView ID="gvDatos" AllowPaging="false" OnRowCreated="gvDatos_RowCreated"
                    OnRowCommand="gvDatos_RowCommand"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdMailEnvio"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="true" ShowFooter="true">
                    <Columns>

                        <asp:TemplateField HeaderText="Incluir">
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                            </HeaderTemplate>
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
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Procesar Envios"
                        OnClick="btnProcesarEnvio_Click" ValidationGroup="MailingAceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
            </div>
            <AUGE:VistaPrevia ID="ctrVistaPrevia" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
