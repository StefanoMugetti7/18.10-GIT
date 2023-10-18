<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PacientesDatos.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.PacientesDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>

<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosDomicilioPopUp.ascx" TagName="popUpDomicilio" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosTelefonoPopUp.ascx" TagName="popUpTelefono" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagName="popUpAfiliadosBuscar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>


<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        SetTabIndexInput();

    });

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }


</script>


<asp:UpdatePanel ID="upPacientes" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo doc."></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoDocumento" Enabled="true" ControlToValidate="ddlTipoDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroDocumento" Enabled="true" ControlToValidate="txTNumeroDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3  col-form-label" ID="lblEstado" runat="server" Text="Estado del Paciente"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvApellido" Enabled="true" ControlToValidate="txtApellido" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" Enabled="true" ControlToValidate="txtNombre" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFechaNacimiento" runat="server" Text="Fecha de Nacimiento"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaNacimiento" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEdad" runat="server" Text="Edad"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtEdad" Enabled="false" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblSexo" runat="server" Text="Sexo"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlSexo" runat="server"></asp:DropDownList>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblGrupoSanguineo" runat="server" Text="Grupo Sanguineo"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlGrupoSanguineo" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCorreoElectronico" runat="server" Text="Correo Electrónico"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtCorreoElectronico" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Condición Fiscal"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" runat="server"></asp:DropDownList>
            </div>
        </div>

        <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
            SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpHistorial"
                HeaderText="Historia Clínica">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarPrestacion" runat="server" Text="Agregar Prestación"
                                OnClick="btnAgregarPrestacion_Click" />
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Fecha" SortExpression="Fecha">
                                    <ItemTemplate>
                                        <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Prestador" HeaderStyle-Width="20%" SortExpression="Prestador.ApellidoNombre">
                                    <ItemTemplate>
                                        <%# Eval("Prestador.ApellidoNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Evoluciones" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="Evoluciones">
                                    <ItemTemplate>
                                        <%# Eval("Observaciones")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("Estado.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                            AlternateText="Mostrar" ToolTip="Mostrar" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                            AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="TabPanel1"
                HeaderText="Turnos Futuros">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarTurno" runat="server" Text="Agregar Turno"
                                OnClick="btnAgregarTurno_Click" />
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvTurnos" OnRowCommand="gvTurnos_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvTurnos_RowDataBound" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvTurnos_PageIndexChanging" OnSorting="gvTurnos_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Prestador" SortExpression="Prestador.ApellidoNombre">
                                    <ItemTemplate>
                                        <%# Eval("Prestador.ApellidoNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paciente" SortExpression="Afiliado.ApellidoNombre">
                                    <ItemTemplate>
                                        <%# Eval("Afiliado.ApellidoNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Turno" SortExpression="FechaHoraDesde">
                                    <ItemTemplate>
                                        <%# Eval("FechaHoraDesde", "{0:dd/MM/yyyy HH:mm}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("Estado.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultarTurnos"
                                            AlternateText="Mostrar" ToolTip="Mostrar" Visible="false" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificarTurnos"
                                            AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpAntecedentesFamiliares"
                HeaderText="Antecedentes Familiares">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAntecedentesFamiliares" runat="server" Text="Observaciones"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtAntecedentesFamiliares" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpAntecedentesPersonales"
                HeaderText="Antecedentes Personales">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAntecedentesPersonales" runat="server" Text="Observaciones"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtAntecedentesPersonales" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpComentarios"
                HeaderText="Comentarios - Pendientes">
                <ContentTemplate>
                    <AUGE:Comentarios ID="ctrComentarios" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpEstudios"
                HeaderText="Resultados de Estudios">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="Button1" runat="server" Text="Agregar Estudio" OnClick="btnAgregarEstudio_Click" />
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvEstudios" OnRowCommand="gvEstudios_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvEstudios_RowDataBound" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvEstudios_PageIndexChanging" OnSorting="gvEstudios_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Fecha" SortExpression="FechaEstudio">
                                    <ItemTemplate>
                                        <%# Eval("FechaEstudio", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo de Estudio" SortExpression="TipoEstudio">
                                    <ItemTemplate>
                                        <%# Eval("TipoEstudio")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Turno" SortExpression="FechaHoraDesde">
                                    <ItemTemplate>
                                        <%# Eval("Prestador.ApellidoNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                            AlternateText="Imprimir" ToolTip="Imprimir" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultarEstudios"
                                            AlternateText="Mostrar" ToolTip="Mostrar" Visible="false" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificarEstudios"
                                            AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                            AlternateText="Anular" ToolTip="Anular" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
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
                            <AUGE:popUpDomicilio ID="ctrDomicilios" runat="server" />
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
                            <AUGE:popUpTelefono ID="ctrTelefonos" runat="server" />
                            <div class="table-responsive">
                                <asp:GridView ID="gvTelefonos" OnRowCommand="gvTelefonos_RowCommand"
                                    OnRowDataBound="gvTelefonos_RowDataBound" DataKeyNames="IndiceColeccion"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tipo telefono" SortExpression="TelefonoTipo.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("TelefonoTipo.Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Numero" DataField="Numero" SortExpression="Numero" />
                                        <asp:BoundField HeaderText="Interno" DataField="Interno" SortExpression="Interno" />
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
        </asp:TabContainer>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
        <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    OnClick="btnAceptar_Click" ValidationGroup="AfiliadosModificarDatosAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                    OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
