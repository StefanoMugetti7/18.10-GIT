<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuentasContablesDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.CuentasContablesDatos" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>



<asp:UpdatePanel ID="upDDL" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblEjercicioContable" runat="server" Text="Seleccione el ejercicio contable:" />
            <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEjerciciosContables_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-sm-7"></div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblBuscar" runat="server" Text="Filtrar Cuentas" />
            <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtBuscar" runat="server" />
            </div>
            <div class="col-sm-7">
            <asp:Button CssClass="botonesEvol" ID="btnFiltrar" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click" />
            </div>
        </div>

        <div class="form-group row">
            <div class="col-sm-5">
                <asp:UpdatePanel ID="upCuentasRamas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TreeView ID="tvCuentasContables" runat="server" ImageSet="BulletedList4"
                            OnSelectedNodeChanged="tvMenu_SelectedNodeChanged" ShowCheckBoxes="None" EnableClientScript="true">
                            <ParentNodeStyle Font-Bold="false" />
                            <HoverNodeStyle Font-Underline="true" ForeColor="#5555DD" />
                            <SelectedNodeStyle Font-Underline="true" ForeColor="#5555DD"
                                HorizontalPadding="0px" VerticalPadding="0px" />
                            <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black"
                                HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                        </asp:TreeView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-sm-1"></div>
            <div class="col-sm-6">
                <asp:UpdatePanel ID="upGrillaCuentas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>                        
                        <asp:Panel ID="pnlGrillaCuentas" runat="server">
                            <div class="card">
                        <div class="card-header">
                            Cuentas Contables
                        </div>
                        <div class="card-body">
                            <div class="form-group row">
                                <div class="col-sm-12">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12">
                            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                                <Columns>
                                    <asp:BoundField HeaderText="Número Cuenta" DataField="NumeroCuenta" SortExpression="NumeroCuenta" />
                                    <asp:BoundField HeaderText="Descripción" DataField="Descripcion" SortExpression="Descripcion" />
                                    <asp:TemplateField HeaderText="Imputable">
                                        <ItemTemplate>
                                            <%# Eval("Imputable").ToString().ToLower() == "true" ? "Si" : "No"%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" ToolTip="Modificar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                                    </div>
                            </div>
                            </div>
                        </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="upCuentaContable" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlCuentaContable" runat="server">
                            <div class="card">
                        <div class="card-header">
                            <asp:Label ID="lblDatosCuenta" runat="server" Text="Datos de la Cuenta"></asp:Label>
                        </div>
                        <div class="card-body">
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblNumeroCuenta" runat="server" Text="Número Cuenta" />
                            <div class="col-sm-8">
                                <asp:TextBox CssClass="form-control" ID="txtNumeroCuenta" runat="server" />
                            </div>
                            </div>
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblDescripcion" runat="server" Text="Descripción" />
                            <div class="col-sm-8">
                                <asp:TextBox CssClass="form-control" ID="txtDescripción" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtDescripción" ValidationGroup="Aceptar" />
                            </div>
                            </div>
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblRama" runat="server" Text="Rama" />
                            <div class="col-sm-8">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlRama" runat="server" />
                            </div>
                                </div>
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblImputable" runat="server" Text="Imputable" />
                            <div class="col-sm-8">
                                <asp:CheckBox ID="chkImputable" CssClass="form-control" Enabled="true" Checked="false" runat="server" />
                            </div>
                                </div>
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                            <div class="col-sm-8">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                            </div>
                                </div>
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblCentroCosto" runat="server" Text="Centro de Costos" />
                            <div class="col-sm-8">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlCentroCostos" runat="server" />
                            </div>
                                </div>
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblCentroCostosObligatorio" runat="server" Text="Centro Costos obligatorio" />
                            <div class="col-sm-8">
                                <asp:CheckBox ID="chkCentroCostosObligatorio" CssClass="form-control" Enabled="true" Checked="false" runat="server" />
                            </div>
                                </div>
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblMonetaria" runat="server" Text="Es Monetaria" />
                            <div class="col-sm-8">
                                <asp:CheckBox ID="chkMonetaria" CssClass="form-control" Enabled="true" Checked="false" runat="server" />
                            </div>                
                                </div>          
<%--                            <div class="row">--%>
                            <AUGE:CamposValores ID="ctrCamposValores" cssLabel="col-sm-4 col-form-label" cssCol="col-sm-8 col-12" ccsContainer="col-12 col-md-12 col-lg-12" runat="server" />
<%--                            </div>--%>
                            <asp:Panel ID="pnlInaes" Visible="true" runat="server">
                                <div class="card">
                        <div class="card-header">
                            Datos para INAES
                        </div>
                        <div class="card-body">
                                <div class="form-group row">
                                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblCapitulo" runat="server" Text="Capítulo" />
                                <div class="col-sm-8">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlCapitulo" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCapitulo" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlCapitulo" ValidationGroup="Aceptar" />
                                </div>
                                </div>
                                <div class="form-group row">
                                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                                <div class="col-sm-8">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="frvMoneda" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlMoneda" ValidationGroup="Aceptar" />
                                </div>
                                </div>
                                <div class="form-group row">
                                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblDepartamento" runat="server" Text="Departamento" />
                                <div class="col-sm-8">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlDepartamento" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDepartamento" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlDepartamento" ValidationGroup="Aceptar" />
                                </div>
                                </div>
                                <div class="form-group row">
                                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblRubro" runat="server" Text="Rubro" />
                                <div class="col-sm-8">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlRubro" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvRubro" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlRubro" ValidationGroup="Aceptar" />
                                </div>
                                </div>
                                <div class="form-group row">
                                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblSubRubro" runat="server" Text="SubRubro" />
                                <div class="col-sm-8">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlSubRubro" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="frvSubRubro" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlSubRubro" ValidationGroup="Aceptar" />
                            </div>
                                </div>
                            <div class="form-group row">
                                 <asp:Label CssClass="col-sm-4 col-form-label" ID="lblImputacion" runat="server" Text="Imputacion" />
                                <div class="col-sm-8">
                                    <asp:TextBox CssClass="form-control" ID="txtImputacion" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImputacion" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtImputacion" ValidationGroup="Aceptar" />
                                </div>
                            </div>
                                    </div>
                                    </asp:Panel>
                                <center>
                <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
                            </div>
                                </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
