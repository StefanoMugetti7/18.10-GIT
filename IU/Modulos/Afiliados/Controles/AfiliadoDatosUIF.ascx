<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfiliadoDatosUIF.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfiliadoDatosUIF" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>

<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ValidarValor);
        SetTabIndexInput();
        intiGridDetalle();
        ValidarValor();
        IniciarAfiliadosWS();
    });

    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function IniciarAfiliadosWS() {
        $.ajax({
            url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/IniciarWS',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            success: function (response) {
                //do whatever your thingy..
            }
        });
    }
    function intiGridDetalle() {
        var rowindex = 0;
        var idAfiliado = $("input:hidden[id$='hdfIdAfiliadoJuridVinc']");
        var afiliado = $("input:hidden[id$='hdfAfiliadoJuridVinc']");
        var ddlAfiliado = $("select[name$='ddlAfiliado']");

        ddlAfiliado.select2({
            placeholder: 'Ingrese el apellido, DNI o nro. de socio',
            selectOnClose: true,
            theme: 'bootstrap4',
            width: '100%',
            //theme: 'bootstrap',
            minimumInputLength: 3,
            language: 'es',
            //tags: true,
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: ddlAfiliado.val(), // search term");
                        filtro: params.term // search term");
                    });
                },
                processResults: function (data, params) {
                    //return { results: data.items };
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.DescripcionCombo,
                                id: item.IdAfiliado,
                                Apellido: item.Apellido,
                                Nombre: item.Nombre,
                                NumeroDocumento: item.NumeroDocumento,
                            }
                        })
                    };
                    cache: true
                }
            },
        });
        ddlAfiliado.on('select2:select', function (e) {
            var newOption = new Option(e.params.data.Apellido + ", " + e.params.data.Nombre, e.params.data.id, false, true);
            $("select[id$='ddlAfiliado']").append(newOption).trigger('change');
            idAfiliado.val(e.params.data.id);
            afiliado.val(e.params.data.id + ' - ' + e.params.data.Apellido + ", " + e.params.data.Nombre + ' - ' + e.params.data.NumeroDocumento);
            AfiliadoSeleccionar();
        });
        ddlAfiliado.on('select2:unselect', function (e) {
            idAfiliado.val('');
            afiliado.val('');
            ddlAfiliado.val(null).trigger('change');
            AfiliadoSeleccionar();
        });
    }
    function AfiliadoSeleccionar() {
        __doPostBack("<%=button.UniqueID %>", "");
    }

    function ValidarValor() {
        $('#<%=gvMatrizRiesgo.ClientID%> tr').not(':first').not(':last').each(function () {
            var txtValor = $(this).find('[id*="txtValor"]');
            txtValor.blur(function () {
                if (txtValor.val() > "5") {
                    txtValor.val("5");
                }
            });
        });
    }



</script>

<div class="AfiliadoDatosUIF">
    <asp:HiddenField ID="hdfIdAfiliadoJuridVinc" runat="server" />
    <asp:HiddenField ID="hdfAfiliadoJuridVinc" runat="server" />
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPersonaRelacionadaActTerrorista" runat="server" Text="Persona Fisica Relacionada con Act. Terrorista"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlPersonaRelacionadaActTerrorista" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPersonaRelacionadaActTerrorista" ControlToValidate="ddlPersonaRelacionadaActTerrorista" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPersonaRelacionadaTripleFrontera" runat="server" Text="Persona Fisica Relacionada con Triple Frontera"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlPersonaRelacionadaTripleFrontera" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPersonaRelacionadaTripleFrontera" ControlToValidate="ddlPersonaRelacionadaTripleFrontera" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEsCliente" runat="server" Text="El Reportado es Cliente" Visible="false"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEsCliente" runat="server" Visible="false"></asp:DropDownList>
            <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvEsCliente" ControlToValidate="ddlEsCliente" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
        </div>
    </div>
    <asp:UpdatePanel ID="upPEP" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEsPEP" runat="server" Text="Es PEP"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEsPEP" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEsPEP_OnSelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEsPEP" ControlToValidate="ddlEsPEP" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCargo" Visible="false" runat="server" Text="Cargo"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCargo" Visible="false" ValidationGroup="Aceptar" runat="server"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvIdAfiliado" ControlToValidate="txtIdAfiliado" ValidationGroup="Filtrar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDesempenia" Visible="false" runat="server" Text="En Actividad"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEnActividad" runat="server" Visible="false"></asp:DropDownList>
                    <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvIdAfiliado" ControlToValidate="txtIdAfiliado" ValidationGroup="Filtrar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha" Visible="false"></asp:Label>
                <div class="col-sm-3">
                    <div class="form-group row">
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Inicio" ID="txtFechaDesde" runat="server" Visible="false"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Fin" ID="txtFechaHasta" runat="server" Visible="false"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDependencia" Visible="false" runat="server" Text="Dependencia"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtDependencia" Visible="false" ValidationGroup="Aceptar" runat="server"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvIdAfiliado" ControlToValidate="txtIdAfiliado" ValidationGroup="Filtrar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPais" Visible="false" runat="server" Text="Pais"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtPais" Visible="false" ValidationGroup="Aceptar" runat="server"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvIdAfiliado" ControlToValidate="txtIdAfiliado" ValidationGroup="Filtrar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="table-responsive" id="DivEsposa" runat="server" visible="false">
        <div class="card-header" id="TituloEsposa">
            Esposa/o
        </div>
        <asp:UpdatePanel ID="upEsposa" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAfiliado"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Apellido y Nombre">
                            <ItemTemplate>
                                <%# Eval("ApellidoNombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Nac.">
                            <ItemTemplate>
                                <%# Eval("FechaNacimiento", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sexo">
                            <ItemTemplate>
                                <%# Eval("Sexo.Sexo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nacionalidad">
                            <ItemTemplate>
                                <%# Eval("Nacionalidad.Nacionalidad")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo">
                            <ItemTemplate>
                                <%# Eval("TipoDocumento.TipoDocumento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Numero">
                            <ItemTemplate>
                                <%# Eval("NumeroDocumento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="card-header" id="TituloLimitePerfiles">
        Limites de Perfiles
    </div>
    <hr widht="10%" />
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblLimitePerfilesAnual" runat="server" Text="Anual $"></asp:Label>
        <div class="col-sm-3">
            <Evol:CurrencyTextBox CssClass="form-control" ID="txtLimitePerfilesAnual" Prefix="" NumberOfDecimals="2" runat="server" />
            <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" ID="rfvLimitePerfilesAnual" ControlToValidate="txtLimitePerfilesAnual" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblLimitePerfilesFechaVenc" runat="server" Text="Fecha Vencimiento"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtLimitePerfilesFechaVenc" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvLimitePerfilesFechaVenc" ControlToValidate="txtLimitePerfilesFechaVenc" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
    </div>
    <hr widht="10%" />
    <asp:UpdatePanel ID="upAfiliado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPersonaJuridicaVinculada" Visible="false" runat="server" Text="Persona Juridica Vinculada"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlAfiliado" Visible="false" runat="server"></asp:DropDownList>
                    <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvAfiliado" runat="server" ErrorMessage="*" ControlToValidate="ddlAfiliado" ValidationGroup="Aceptar" />--%>
                    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCargoPersonaJuridicaVinc" Visible="false" runat="server" Text="Cargo Persona Juridica Vinculada"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCargoPersonaJuridicaVinc" ValidationGroup="Aceptar" Visible="false" runat="server"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvCargoPersonaJuridicaVinc" ControlToValidate="txtCargoPersonaJuridicaVinc" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpMatrizRiesgo" HeaderText="Matriz de Riesgo">
            <ContentTemplate>
                <asp:UpdatePanel ID="upMatrizRiesgo" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <asp:GridView ID="gvMatrizRiesgo" OnRowCommand="gvMatrizRiesgo_RowCommand"
                                OnRowDataBound="gvMatrizRiesgo_RowDataBound" ShowFooter="true" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Matriz de Riesgo" ItemStyle-Width="80%">
                                        <ItemTemplate>
                                            <asp:Label CssClass="col-form-label" ID="lblMatrizRiesgo" runat="server" Text='<%#Eval("Matriz")%>'></asp:Label>
                                            <asp:HiddenField ID="hdfIdMatrizRiesgo" Value='<%#Bind("IdMatriz") %>' runat="server" />
                                            <asp:HiddenField ID="hdfMatrizRiesgo" Value='<%#Bind("CodigoMatriz") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valores">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtValor" Prefix="" NumberOfDecimals="0" runat="server" MaxLength="1" />
                                            <asp:HiddenField ID="hdfValor" Value='<%#Bind("Valor") %>' runat="server" />
                                            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvValor" ControlToValidate="txtValor" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <asp:UpdatePanel ID="upComprobantes" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false" OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
