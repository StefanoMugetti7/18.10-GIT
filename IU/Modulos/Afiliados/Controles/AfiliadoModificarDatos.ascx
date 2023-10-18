<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfiliadoModificarDatos.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfiliadoModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosDomicilioPopUp.ascx" TagName="popUpDomicilio" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosTelefonoPopUp.ascx" TagName="popUpTelefono" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagName="popUpAfiliadosBuscar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfipPadronTXTBuscar.ascx" TagName="popUpBuscarPadronTXT" TagPrefix="auge" %>

<auge:popupbotonconfirmar id="popUpConfirmar" runat="server" />

<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(NumeroDocumentoChange);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFechaNacimiento);
        SetTabIndexInput();
        SetFechaNacimiento();
        calcularEdad();
    });

    function EnviarWhatsApp(url) {
        var win = window.open(url, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            MostrarMensaje('Por favor habilite los popups para este sitio', 'red');
        }
    }


    function ImportarClienteComoSocio() {
        $("#<%= btnImportarCliente.ClientID %>").click();
    }

    function NumeroDocumentoChange() {
        $("input[type=text][id$='txtNumeroDocumento']").change(function () {
            var ddlTipoDocumento = $("select[name$='ddlTipoDocumento']");
            if (ddlTipoDocumento.val() == "9" || ddlTipoDocumento.val() == "10") {
                $("input[type=text][id$='txtCUIL']").val($(this).val());
            }
        });
    }

    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function SetFechaNacimiento() {
        $("input:text[id$='txtFechaNacimiento']").change(calcularEdad);
    }

    function calcularEdad() {

        var txtNacimiento = $("input:text[id$='txtFechaNacimiento']")
        if (txtNacimiento.val() != '') {
            var hoy = new Date();
            var cumpleanos = new Date(toDate(txtNacimiento.val()));
            var edad = hoy.getFullYear() - cumpleanos.getFullYear();
            var m = hoy.getMonth() - cumpleanos.getMonth();

            if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
                edad--;
            }
            $("input[type=text][id$='txtEdad']").val(edad);
        }
    }

</script>


<asp:UpdatePanel ID="upNumeroSocio" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoPersona">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoPersona" runat="server" Text="Tipo Persona"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoPersona" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTipoPersona_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoSocio">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoSocio" runat="server" Text="Tipo Socio"></asp:Label>
                    <div class="col-sm-6">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoSocio" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnDesvincularSocio" Visible="false" Text="Desvincular" ToolTip="Desvincular la cuenta del socio Titular" OnClick="btnDesvincularSocio_Click" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvEstado">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEstados_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCategoria">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCategoria" runat="server" Text="Categoria"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCategoria" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" AutoPostBack="true" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroSocio">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroSocio" runat="server" Text="Numero socio"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox CssClass="form-control" ID="txtNumeroSocio" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnObtenerNumeroSocio" Visible="true" Text="Recalcular" OnClick="btnObtenerNumeroSocio_Click" runat="server" />


                    </div>
                </div>
            </div>

            <%--    </div>--%>
            <%-- </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upImportarDatos" UpdateMode="Conditional" runat="server">
    <ContentTemplate>--%>
            <%-- <div class="form-group row">--%>
            <div class="w-100"></div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoDocumento">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoDocumento_SelectedIndexChanged" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroDocumento">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número documento"></asp:Label>
                    <div class="col-sm-6">
                        <auge:numerictextbox cssclass="form-control" id="txtNumeroDocumento" autopostback="true" ontextchanged="txtNumeroDocumento_TextChanged" runat="server"></auge:numerictextbox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvtNumeroDocumento" ControlToValidate="txtNumeroDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <div style="visibility: hidden; display: inline" id="dvImportarCliente">
                            <asp:Button CssClass="botonesEvol" ID="btnImportarCliente" Width="0px" runat="server" Text="Importar Cliente" OnClick="btnImportarCliente_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnTxtCuitBlur" Visible="false" Text="Validar" OnClick="btnTxtCuitBlur_Click" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvSexo" visible="false">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblSexo" runat="server" Text="Sexo"></asp:Label>
                    <div class="col-sm-6">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlSexo" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvSexo" ControlToValidate="ddlSexo" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-2">
                        <asp:Button CssClass="botonesEvol" ID="btnRenaper" runat="server" Text="Renaper" ToolTip="Validar e importar datos desde Renaper" OnClick="btnRenaper_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvApellido">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvApellido" ControlToValidate="txtApellido" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNombre">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" ControlToValidate="txtNombre" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvMatriculaIAF">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMatriculaIAF" runat="server" Text="Legajo"></asp:Label>
                    <div class="col-sm-9">
                        <evol:currencytextbox cssclass="form-control" prefix="" thousandsseparator="" numberofdecimals="0" id="txtMatriculaIAF" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFilial">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" ControlToValidate="ddlFilial" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <auge:popupbuscarpadrontxt id="ctrBuscarPadronTXT" runat="server" />
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCuil">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCUIL" runat="server" Text="CUIT/CUIL"></asp:Label>
                    <div class="col-sm-9">
                        <auge:numerictextbox cssclass="form-control" id="txtCUIL" runat="server"></auge:numerictextbox>

                    </div>

                </div>
            </div>

            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvParentesco">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblParentesco" runat="server" Text="Parentesco"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlParentesco" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaNacimiento">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaNacimiento" runat="server" Text="Fecha Nacimiento"></asp:Label>
                    <div class="col-sm-5">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaNacimiento" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblEdad" runat="server" Text="Edad"></asp:Label>
                    <div class="col-sm-2">
                        <asp:TextBox CssClass="form-control " ID="txtEdad" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaIngreso">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaIngreso" runat="server" Text="Fecha Ingreso"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaIngreso" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaIngreso" ControlToValidate="txtFechaIngreso" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                        <%--  <asp:CompareValidator ID="cvFechaIngreso" runat="server" ErrorMessage="*" ValidationGroup="AfiliadosModificarDatosFechaIngreso"
                            Operator="Equal" ControlToValidate="txtFechaIngreso" Enabled="false"></asp:CompareValidator>--%>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" data-ocultar="dvEstadoCivil" runat="server" id="dvEstadoCivil">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblEstadoCivil" runat="server" Text="Estado civil"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstadoCivil" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCorreoElectronico">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCorreoElectronico" runat="server" Text="Correo electronico"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtCorreoElectronico" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvGrupoSanguineo">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblGrupoSanguineo" runat="server" Text="Grupo Sanguineo"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlGrupoSanguineo" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvGrado">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblGrado" runat="server" Text="Grado"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlGrado" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaFallecimiento">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaFallecimiento" runat="server" Text="Fecha Fallecimiento"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFallecimiento" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaRetiro">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaRetiro" runat="server" Text="Fecha Retiro"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaRetiro" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaBaja">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaBaja" runat="server" Text="Fecha Baja"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaBaja" Enabled="false" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaBaja" Enabled="false" ControlToValidate="txtFechaBaja" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaSupervivencia">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaSupervivencia" runat="server" Text="Fecha Supervivencia"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaSupervivencia" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoApoderado" visible="false">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoApoderado" runat="server" Text="Tipo Apoderado"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoApoderado" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoApoderado" Enabled="false" ControlToValidate="ddlTipoApoderado" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvZonasGrupos">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblZonaGrupo" runat="server" Text="Dependencia"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlZonasGrupos" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCondicionesFiscales">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCondicionesFiscales" runat="server" Text="Condicion Fiscal"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

        </div>
        <auge:camposvalores id="ctrCamposValores" runat="server" />
        <asp:Panel ID="pnlAlertasTipos" class="card" runat="server">
            <div class="card-header">
                Alertas
            </div>
            <div class="card-body">
                <asp:CheckBoxList CssClass="checkboxlist" ID="chkAlertasTipos" RepeatColumns="3" RepeatDirection="Horizontal" runat="server">
                </asp:CheckBoxList>
            </div>
        </asp:Panel>

        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
            SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpFamiliares"
                HeaderText="Familiares">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upFamiliares" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarFamiliar" runat="server" Text="Agregar familiar"
                                        OnClick="btnAgregarFamiliar_Click" Visible="false" CausesValidation="false" />
                                </div>
                            </div>
                            <auge:popupcomprobantes id="PopUpComprobantes1" runat="server" />
                            <div class="table-responsive">
                                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand"
                                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="Apellido" DataField="Apellido" SortExpression="Apellido" />
                                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" SortExpression="Nombre" />
                                        <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumento.TipoDocumento">
                                            <ItemTemplate>
                                                <%# Eval("TipoDocumento.TipoDocumento")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Número" DataField="NumeroDocumento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
                                        <asp:TemplateField HeaderText="Categoria" SortExpression="Categoria.Categoria">
                                            <ItemTemplate>
                                                <%# Eval("Categoria.Categoria")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Domicilio" ItemStyle-Wrap="true" SortExpression="DomicilioPredeterminado">
                                            <ItemTemplate>
                                                <%# Eval("DomicilioPredeterminado")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Telefono" ItemStyle-Wrap="true" SortExpression="TelefonoPredeterminado">
                                            <ItemTemplate>
                                                <%# Eval("TelefonoPredeterminado")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Parentesco" ItemStyle-Wrap="true" SortExpression="Parentesco">
                                            <ItemTemplate>
                                                <%# Eval("Parentesco.Parentesco")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Estado.Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                    AlternateText="Consultar" ToolTip="Consultar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                    AlternateText="Modificar" ToolTip="Modificar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpDomicilios"
                HeaderText="Domicilios">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upDomicilios" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarDomicilio" runat="server" Text="Agregar domicilio"
                                        OnClick="btnAgregarDomicilio_Click" CausesValidation="false" />
                                </div>
                            </div>
                            <auge:popupdomicilio id="ctrDomicilios" runat="server" />
                            <div class="table-responsive">
                                <asp:GridView ID="gvDomicilios" OnRowCommand="gvDomicilios_RowCommand"
                                    OnRowDataBound="gvDomicilios_RowDataBound" DataKeyNames="IndiceColeccion"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tipo domicilio" SortExpression="DomicilioTipo.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("DomicilioTipo.Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Calle" DataField="Calle" SortExpression="Calle" />
                                        <asp:BoundField HeaderText="Numero" DataField="Numero" SortExpression="Numero" />
                                        <asp:BoundField HeaderText="Piso" DataField="Piso" SortExpression="Piso" />
                                        <asp:BoundField HeaderText="Dpto" DataField="Departamento" SortExpression="Departamento" />
                                        <asp:TemplateField HeaderText="Localidad" SortExpression="Localidad.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Localidad.Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Provincia" SortExpression="Localidad.Provincia.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Localidad.Provincia.Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Codigo Postal" DataField="CodigoPostal" SortExpression="CodigoPostal" />
                                        <asp:BoundField HeaderText="Predeterminado" DataField="Predeterminado" SortExpression="Predeterminado" />
                                        <asp:TemplateField HeaderText="Acciones">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                    AlternateText="Consultar" ToolTip="Consultar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                    AlternateText="Modificar" ToolTip="Modificar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpTelefonos"
                HeaderText="Telefonos">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upTelefonos" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarTelefono" runat="server" Text="Agregar telefono"
                                        OnClick="btnAgregarTelefono_Click" CausesValidation="false" />
                                </div>
                            </div>
                            <auge:popuptelefono id="ctrTelefonos" runat="server" />
                            <div class="table-responsive">
                                <asp:GridView ID="gvTelefonos" OnRowCommand="gvTelefonos_RowCommand"
                                    OnRowDataBound="gvTelefonos_RowDataBound" DataKeyNames="IndiceColeccion"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tipo telefono" SortExpression="TelefonoTipo.Descripcion">
                                            <ItemTemplate><%# Eval("TelefonoTipo.Descripcion")%></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Empresa Telefonica" SortExpression="EmpresaTelefonica.Descripcion">
                                            <ItemTemplate><%# Eval("EmpresaTelefonica.Descripcion")%></ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField HeaderText="Prefijo" DataField="Prefijo" SortExpression="Prefijo" />--%>
                                        <asp:BoundField HeaderText="Numero" DataField="Numero" SortExpression="Numero" />
                                        <asp:BoundField HeaderText="Interno" DataField="Interno" SortExpression="Interno" />
                                        <asp:TemplateField HeaderText="Acciones">
                                            <ItemTemplate>

                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                    AlternateText="Consultar" ToolTip="Consultar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/whatsup26x26.png" runat="server" CommandName="WSP" ID="btnWSP" Visible="false"
                                                    AlternateText="Enviar WhatsApp" ToolTip="WhatsApp" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                    AlternateText="Modificar" ToolTip="Modificar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpComentarios"
                HeaderText="Comentarios">
                <ContentTemplate>
                    <auge:comentarios id="ctrComentarios" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpArchivos"
                HeaderText="Archivos">
                <ContentTemplate>
                    <auge:archivos id="ctrArchivos" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpEstados"
                HeaderText="Cambios Estados">
                <ContentTemplate>
                    <auge:auditoriadatos id="ctrAuditoriaEstados" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpApoderados"
                HeaderText="Apoderados">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upApoderados" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarApoderado" runat="server" Text="Agregar Apoderado"
                                        OnClick="btnAgregarApoderado_Click" Visible="false" CausesValidation="false" />
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvApoderado" OnRowCommand="gvApoderados_RowCommand"
                                    OnRowDataBound="gvApoderado_RowDataBound" DataKeyNames="IndiceColeccion"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="Apellido" DataField="Apellido" SortExpression="Apellido" />
                                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" SortExpression="Nombre" />
                                        <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumento.TipoDocumento">
                                            <ItemTemplate>
                                                <%# Eval("TipoDocumento.TipoDocumento")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Número" DataField="NumeroDocumento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
                                        <asp:BoundField HeaderText="Telefono" DataField="TelefonoPredeterminado" SortExpression="TelefonoPredeterminado" />
                                        <asp:BoundField HeaderText="Correo" DataField="CorreoElectronico" SortExpression="CorreoElectronico" />
                                        <asp:TemplateField HeaderText="Domicilio" SortExpression="DomicilioPredeterminado">
                                            <ItemTemplate>
                                                <%# Eval("DomicilioPredeterminado")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Estado.Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                    AlternateText="Consultar" ToolTip="Consultar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                    AlternateText="Modificar" ToolTip="Modificar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpHistorial"
                HeaderText="Auditoria">
                <ContentTemplate>
                    <auge:auditoriadatos id="ctrAuditoria" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <auge:popupcomprobantes id="ctrPopUpComprobantes" runat="server" />
        <auge:popupmensajespostback id="popUpMensajes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptarContinuar" runat="server" Text="Aplicar" OnClick="btnAceptarContinuar_Click" ValidationGroup="AfiliadosModificarDatosAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    OnClick="btnAceptar_Click" ValidationGroup="AfiliadosModificarDatosAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                    OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
