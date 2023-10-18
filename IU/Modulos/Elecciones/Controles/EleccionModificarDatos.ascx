<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EleccionModificarDatos.ascx.cs" Inherits="IU.Modulos.Elecciones.Controles.EleccionModificarDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>

<div class="EleccionModificarDatos">
    <div class="form-group row">
       <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Descripcion"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtObservacion"  runat="server"></asp:TextBox><%--seria el "nombre"--%>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvObservacion" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblAnio" runat="server" Text="Año"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <Evol:CurrencyTextBox CssClass="form-control" ID="txtAnio" MaxLength="4" NumberOfDecimals="0" ThousandsSeparator="" runat="server" Prefix=""></Evol:CurrencyTextBox>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvAnio" ControlToValidate="txtAnio" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>
    </div>

 

</div>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpEtapas" HeaderText="Etapas">
            <ContentTemplate>
                <asp:UpdatePanel ID="upEtapas" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <asp:GridView ID="gvEtapas" OnRowCommand="gvEtapas_RowCommand"
                                OnRowDataBound="gvEtapas_RowDataBound" ShowFooter="true" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Codigo" SortExpression="Codigo">
                                        <ItemTemplate>
                                            <asp:Label CssClass="col-form-label" ID="lblCodigoEtapa" runat="server" Text='<%#Eval("CodigoEtapa")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Etapa" ItemStyle-Width="30%" SortExpression="Puesto">
                                        <ItemTemplate>
                                            <asp:Label CssClass="col-form-label" ID="lblEtapa" runat="server" Text='<%#Eval("Etapa")%>'></asp:Label>
                                            <asp:HiddenField ID="hdfIdEtapa" Value='<%#Bind("IdEtapa") %>' runat="server" />
                                            <asp:HiddenField ID="hdfEtapa" Value='<%#Bind("Etapa") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Desde">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFechaDesde" Value='<%#Bind("FechaDesde") %>' runat="server" />
                                            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvFechaDesde" ControlToValidate="txtFechaDesde" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Hasta">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFechaHasta" Value='<%#Bind("FechaHasta") %>' runat="server" />
                                            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvFechaHasta" ControlToValidate="txtFechaHasta" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
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
