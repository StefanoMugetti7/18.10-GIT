<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EleccionesVotacionDatos.ascx.cs" Inherits="IU.Modulos.Elecciones.Controles.EleccionesVotacionDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        SetTabIndexInput();
        intiGridDetalle();
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
        var idAfiliado = $("input:hidden[id$='hdfIdAfiliado']");
        var afiliado = $("input:hidden[id$='hdfAfiliado']");
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

    var gridViewIdNacional = '#<%= gvListasNacionales.ClientID %>';
    var gridViewIdRegional = '#<%= gvListasRegionales.ClientID %>';
    //SOLO REPRESENTANTES
    var gridViewIdRegionalYRepresentantes = '#<%= gvListasRepresentantes.ClientID %>';
    function CheckRow(selectCheckbox) {
        if (!selectCheckbox.checked) {
            return;
        }
        $('td :checkbox', gridViewIdNacional).prop("checked", false);
        selectCheckbox.checked = true;
    }
    function CheckRowRegional(selectCheckbox) {
        if (!selectCheckbox.checked) {
            return;
        }
        $('td :checkbox', gridViewIdRegional).prop("checked", false);
        selectCheckbox.checked = true;
    }
    function CheckRowRegionalYRep(selectCheckbox) {
        if (!selectCheckbox.checked) {
            return;
        }
        $('td :checkbox', gridViewIdRegionalYRepresentantes).prop("checked", false);
        selectCheckbox.checked = true;
    }
    function AfiliadoSeleccionar() {
        __doPostBack("<%=button.UniqueID %>", "");
    }

    function MostrarTituloVotacion() {
        let control = document.getElementById("h2ListasVotadas");
        control.removeAttribute("style");
    }
    function MostrarTituloEleccionVigente(title) {
        let control = document.getElementById("h2EleccionVigente");
        control.removeAttribute("style");
        control.innerHTML = title;
    }
</script>



<div class="EleccionesDatos">
    <div class="card-header" id="MiVotante">
        DATOS DEL AFILIADO
    </div>
    <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
    <asp:HiddenField ID="hdfAfiliado" runat="server" />
    <asp:UpdatePanel ID="upAfiliado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-sm-12">
                    <hr widht="10%" />
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblAfiliado" runat="server" Text="Afiliado"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlAfiliado" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvAfiliado" runat="server" ErrorMessage="*"
                        ControlToValidate="ddlAfiliado" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDocumento" runat="server" Text="Documento"></asp:Label>
                <div class="col-lg-1 col-md-1 col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtTipoDocumento" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-6">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblIdAfiliado" runat="server" Text="Id Afiliado"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtIdAfiliado" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroSocio" runat="server" Text="Nro. Socio"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroSocio" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCategoria" runat="server" Text="Categoria"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtCategoria" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtEstado" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtFilial" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFechaNacimiento" runat="server" Text="Fecha Nac."></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtFechaNacimiento" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">
                <hr widht="10%" />
            </div>
                <center>
        <h2 id="h2EleccionVigente" style="display: none;">HOLA</h2>
    </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
        <h2 id="h2ListasVotadas" style="display: none;">EL AFILIADO SELECCIONADO VOTARA LAS SIGUIENTES LISTAS</h2>
    </center>
  
    <br />
    <div class="card-header" id="ListasNacionales">
        CONSEJO DIRECTIVO Y JUNTA FISCALIZADORA
    </div>
    <asp:UpdatePanel ID="upListasNacionales" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <Evol:EvolGridView ID="gvListasNacionales" OnRowCommand="gvListasNacionales_RowCommand"
                OnRowDataBound="gvListasNacionales_RowDataBound" DataKeyNames="IdListaEleccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="ID" SortExpression="IdEleccion">
                        <ItemTemplate>
                            <%# Eval("IdListaEleccion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Detalle" SortExpression="Lista">
                        <ItemTemplate>
                            <%# Eval("Lista")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Representantes">
                        <ItemTemplate>
                            <asp:Literal ID="ltlDetalleCampos" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Region" SortExpression="TipoRegion">
                        <ItemTemplate>
                            <%# Eval("TipoRegion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Votar" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" onclick="CheckRow(this);" Visible="false" runat="server" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/plus.png" runat="server" CommandName="Consultar" Visible="false" ID="btnExpandirLista"
                                AlternateText="Ver Lista Completa" ToolTip="Detalle" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="card-header" id="ListasRegionales">
        COMISION REGIONAL
    </div>
    <asp:UpdatePanel ID="upListasRegionales" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <Evol:EvolGridView ID="gvListasRegionales" OnRowCommand="gvListasRegionales_RowCommand"
                OnRowDataBound="gvListasRegionales_RowDataBound" DataKeyNames="IdListaEleccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="ID" SortExpression="IdEleccion">
                        <ItemTemplate>
                            <%# Eval("IdListaEleccion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Detalle" SortExpression="Lista">
                        <ItemTemplate>
                            <%# Eval("Lista")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Representantes" HeaderStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Literal ID="ltlDetalleCampos" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Region" SortExpression="TipoRegion">
                        <ItemTemplate>
                            <%# Eval("TipoRegion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Votar" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" onclick="CheckRowRegional(this);" Visible="false" runat="server" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/plus.png" runat="server" CommandName="Consultar" Visible="false" ID="btnExpandirLista"
                                AlternateText="Ver Lista Completa" ToolTip="Detalle" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="card-header" id="ListasRepresentantes">
        REPRESENTANTES
    </div>
    <asp:UpdatePanel ID="upListasRepresentantes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <Evol:EvolGridView ID="gvListasRepresentantes" OnRowCommand="gvListasRepresentantes_RowCommand"
                OnRowDataBound="gvListasRepresentantes_RowDataBound" DataKeyNames="IdListaEleccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="ID" SortExpression="IdEleccion">
                        <ItemTemplate>
                            <%# Eval("IdListaEleccion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Detalle" SortExpression="Lista">
                        <ItemTemplate>
                            <%# Eval("Lista")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Representantes" HeaderStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Literal ID="ltlDetalleCampos" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Region" SortExpression="TipoRegion">
                        <ItemTemplate>
                            <%# Eval("TipoRegion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Votar" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" onclick="CheckRowRegionalYRep(this);" Visible="false" runat="server" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/plus.png" runat="server" CommandName="Consultar" Visible="false" ID="btnExpandirLista"
                                AlternateText="Ver Lista Completa" ToolTip="Detalle" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upComprobantes" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upListaGrilla" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnConfirmar" runat="server" Text="Siguiente" OnClick="btnConfirmar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptarConfirmar" runat="server" Text="Confirmar" Visible="false" OnClick="btnAceptarConfirmar_Click" ValidationGroup="Confirmar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false" OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
