<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EleccionesDatos.ascx.cs" Inherits="IU.Modulos.Elecciones.Controles.EleccionesDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initGridApoderado);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initGridAval);
        SetTabIndexInput();
        intiGridDetalle();
        initGridApoderado();
        initGridAval();
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
        var idAfiliado = $("input[id*='hdfIdAfiliado']").val();

        $('#<%=gvPostulantes.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlAfiliado = $(this).find('[id*="ddlAfiliado"]');
            var hdfIdAfiliado = $(this).find("input[id*='hdfIdAfiliado']");
            var hdfAfiliado = $(this).find("input[id*='hdfAfiliado']");
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
                                }
                            })
                        };
                        cache: true
                    }
                },
            });

            ddlAfiliado.on('select2:select', function (e) {
                //var newOption = new Option(e.params.data.Apellido + ", " + e.params.data.Nombre, e.params.data.id, false, true);
                //$("select[id$='ddlAfiliado']").append(newOption).trigger('change');
                hdfIdAfiliado.val(e.params.data.id);
                hdfAfiliado.val(e.params.data.text);
            });
            ddlAfiliado.on('select2:unselect', function (e) {
                hdfIdAfiliado.val('');
                hdfAfiliado.val('');
                //ddlAfiliado.val(null).trigger('change');
            });
            rowindex++;
        });
    }

    function initGridApoderado() {
        var rowindex = 0;
        var idAfiliado = $("input[id*='hdfIdAfiliadoApoderado']").val();

        $('#<%=gvApoderados.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlAfiliadoApoderado = $(this).find('[id*="ddlAfiliadoApoderado"]');
            var hdfIdAfiliadoApoderado = $(this).find("input[id*='hdfIdAfiliadoApoderado']");
            var hdfAfiliadoApoderado = $(this).find("input[id*='hdfAfiliadoApoderado']");
            ddlAfiliadoApoderado.select2({
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
                            value: ddlAfiliadoApoderado.val(), // search term");
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
                                }
                            })
                        };
                        cache: true
                    }
                },
            });

            ddlAfiliadoApoderado.on('select2:select', function (e) {
                //var newOption = new Option(e.params.data.Apellido + ", " + e.params.data.Nombre, e.params.data.id, false, true);
                //$("select[id$='ddlAfiliado']").append(newOption).trigger('change');
                hdfIdAfiliadoApoderado.val(e.params.data.id);
                hdfAfiliadoApoderado.val(e.params.data.text)
            });
            ddlAfiliadoApoderado.on('select2:unselect', function (e) {
                hdfIdAfiliadoApoderado.val('');
                hdfAfiliadoApoderado.val('');
                //ddlAfiliado.val(null).trigger('change');
            });
            rowindex++;
        });
    }

    function initGridAval() {
        var rowindex = 0;
        var idAfiliado = $("input[id*='hdfIdAfiliadoAval']").val();

        $('#<%=gvAvales.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlAfiliadoAval = $(this).find('[id*="ddlAfiliadoAval"]');
            var hdfIdAfiliadoAval = $(this).find("input[id*='hdfIdAfiliadoAval']");
            var hdfAfiliadoAval = $(this).find("input[id*='hdfAfiliadoAval']");
            ddlAfiliadoAval.select2({
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
                            value: ddlAfiliadoAval.val(), // search term");
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
                                }
                            })
                        };
                        cache: true
                    }
                },
            });
            ddlAfiliadoAval.on('select2:select', function (e) {
                hdfIdAfiliadoAval.val(e.params.data.id);
                hdfAfiliadoAval.val(e.params.data.text);
            });
            ddlAfiliadoAval.on('select2:unselect', function (e) {
                hdfIdAfiliadoAval.val('');
                hdfAfiliadoAval.val('');
            });
            rowindex++;
        });
    }

</script>

<div class="EleccionesDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblIdListaEleccion" runat="server" Text="Identificador"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtIdListaEleccion" Enabled="false" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEleccion" runat="server" Text="Eleccion"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEleccion" runat="server" Enabled="false"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEleccion" ControlToValidate="ddlEleccion" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoLista" runat="server" Text="Tipo Lista"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoLista" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoLista_SelectedIndexChanged" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvTipoLista" ControlToValidate="ddlTipoLista" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblRegion" runat="server" Text="Region"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlRegion" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" runat="server"></asp:DropDownList>
            <%--<asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvRegion" ControlToValidate="ddlRegion" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>--%>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPorcentajeAval" runat="server" Text="% Aval"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtPorcentajeAval" Enabled="false" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblListaElegir" runat="server" Text="Comisiones Regionales"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlListaElegir" Enabled="false" runat="server"></asp:DropDownList>
            <%--<asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvListaElegir" ControlToValidate="ddListaElegir" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>--%>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblLista" runat="server" Text="Lista"></asp:Label>
        <div class="col-sm-7">
            <asp:TextBox CssClass="form-control" ID="txtLista" TextMode="MultiLine" runat="server"></asp:TextBox>
        </div>
    </div>
    <br />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpPostulantes" HeaderText="Postulantes">
            <ContentTemplate>
                <asp:UpdatePanel ID="upPostulantes" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <asp:GridView ID="gvPostulantes" OnRowCommand="gvPostulantes_RowCommand"
                                OnRowDataBound="gvPostulantes_RowDataBound" ShowFooter="true" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Puesto" ItemStyle-Width="30%" SortExpression="Puesto">
                                        <ItemTemplate>
                                            <asp:Label CssClass="col-form-label" ID="lblPuesto" runat="server" Text='<%#Eval("Puesto")%>'></asp:Label>
                                            <asp:HiddenField ID="hdfIdPuesto" Value='<%#Bind("IdPuesto") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Postulante" SortExpression="Afiliado">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlAfiliado" runat="server" Enabled="false"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfIdAfiliado" Value='<%#Bind("IdAfiliado") %>' runat="server" />
                                            <asp:HiddenField ID="hdfAfiliado" Value='<%#Bind("Afiliado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpApoderados" HeaderText="Apoderados">
            <ContentTemplate>
                <asp:UpdatePanel ID="upApoderados" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregarApoderados" runat="server" Text="Cantidad: "></asp:Label>
                            <asp:TextBox CssClass="form-control col-sm-3" ID="txtCantidadAgregarApoderados" Enabled="false" runat="server"></asp:TextBox>
                            <div class="col-sm-1">
                                <asp:Button CssClass="botonesEvol" ID="btnAgregarItemApoderados" runat="server" Text="Agregar item" OnClick="btnAgregarItemApoderados_Click" />
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvApoderados" OnRowCommand="gvApoderados_RowCommand"
                                OnRowDataBound="gvApoderados_RowDataBound" ShowFooter="true" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Apoderados" SortExpression="Afiliado">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlAfiliadoApoderado" runat="server" Enabled="false"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfIdAfiliadoApoderado" Value='<%#Bind("IdAfiliado") %>' runat="server" />
                                            <asp:HiddenField ID="hdfAfiliadoApoderado" Value='<%#Bind("Afiliado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnEliminar" Visible="true"
                                                AlternateText="Eliminar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpAvales" HeaderText="Avales">
            <ContentTemplate>
                <asp:UpdatePanel ID="upAvales" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregarAvales" runat="server" Text="Cantidad: "></asp:Label>
                            <asp:TextBox CssClass="form-control col-sm-3" ID="txtCantidadAgregarAvales" Enabled="false" runat="server"></asp:TextBox>
                            <div class="col-sm-1">
                                <asp:Button CssClass="botonesEvol" ID="btnAgregarItemAvales" runat="server" Text="Agregar item" OnClick="btnAgregarItemAvales_Click" />
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvAvales" OnRowCommand="gvAvales_RowCommand"
                                OnRowDataBound="gvAvales_RowDataBound" ShowFooter="true" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Avales" SortExpression="Afiliado">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlAfiliadoAval" runat="server" Enabled="false"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfIdAfiliadoAval" Value='<%#Bind("IdAfiliado") %>' runat="server" />
                                            <asp:HiddenField ID="hdfAfiliadoAval" Value='<%#Bind("Afiliado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnEliminar" Visible="true"
                                                AlternateText="Eliminar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpComentarios"
            HeaderText="Comentarios">
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos"
            HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
