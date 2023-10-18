<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AfiliadosDatos.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosDatos" Title="" %>

<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/MensajesAlertasListarPopUp.ascx" TagPrefix="AUGE" TagName="MensajesAlertas" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceDerechaArriba" runat="server">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEvento" Visible="false" runat="server" Text="Ultima Actualización"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtFechaEvento" Enabled="false" Visible="false" runat="server"></asp:TextBox></div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:MensajesAlertas ID="ctrMensajesAlertas" runat="server" />
    <div class="AfiliadosDatos">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-12 col-form-label" ID="lblInformacionFamiliar" Visible="false" runat="server" Text=""></asp:Label>
              <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoPersona">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoPersona" runat="server" Text="Tipo Persona"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoPersona" Enabled="false" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroSocio">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroSocio" runat="server" Text="Numero socio"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtNumeroSocio" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCategoria">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCategoria" runat="server" Text="Categoria"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtCategoria" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvIdSocio">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblIdAfiliado" runat="server" Text="Id Socio"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtIdAfiliado" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvApellidoNombre">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblApellidoNombre" runat="server" Text="Apellido"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtApellidoNombre" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNombre">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtNombre" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvEstado">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtEstado" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaIngreso">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaIngreso" runat="server" Text="Fecha Ingreso"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtFechaIngreso" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaNacimiento">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaNacimiento" runat="server" Text="Fecha Nacimiento"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox CssClass="form-control" ID="txtFechaNacimiento" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblEdad" runat="server" Text="Edad"></asp:Label>
                    <div class="col-sm-2">
                        <asp:TextBox CssClass="form-control" ID="txtEdad" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvLegajo">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMatriculaIAF" runat="server" Text="Legajo"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtMatriculaIAF" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoDocumento">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtTipoDocumento" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroDocumento">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número documento"></asp:Label>
                    <div class="col-sm-9">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" Enabled="false" runat="server"></AUGE:NumericTextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvEstadoCivil">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblEstadoCivil" runat="server" Text="Estado civil"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtEstadoCivil" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCalle">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCalle" runat="server" Text="Calle"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtCalle" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvGrado">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblGrado" runat="server" Text="Grado"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtGrado" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvLocalidad">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblLocalidad" runat="server" Text="Localidad"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtLocalidad" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvProvincia">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblProvincia" runat="server" Text="Provincia"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtProvincia" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvRegional">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Regional"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtFilial" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCodigoPostal">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCodigoPostal" runat="server" Text="Codigo Postal"></asp:Label>
                    <div class="col-sm-9">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoPostal" Enabled="false" runat="server"></AUGE:NumericTextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTelefono">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTelefono" runat="server" Text="Telefono"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtTelefono" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaBaja">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaBaja" runat="server" Text="Fecha Baja"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtFechaBaja" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCantidadParticipantes">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCantidadParticipantes" runat="server" Text="Cantidad Participantes"></asp:Label>
                    <div class="col-sm-9">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadParticipantes" Enabled="false" runat="server"></AUGE:NumericTextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaRetiro">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaRetiro" runat="server" Text="Fecha Retiro"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtFechaRetiro" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaSupervivencia">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaSupervivencia" runat="server" Text="Fecha Supervivencia"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtFechaSupervivencia" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvDependencia">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblZonaGrupo" runat="server" Text="Dependencia"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtZonaGrupo" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <AUGE:CamposValores ID="ctrCamposValores" runat="server" />

        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
            SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpFormasPagos" Visible="false"
                HeaderText="Formas de Pago">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblZG" runat="server" Text="Zona Grupo"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtZG" Enabled="false" runat="server"></asp:TextBox></div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaPago" runat="server" Text="Pago por"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtFormaPago" Enabled="false" runat="server"></asp:TextBox></div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialPago" runat="server" Text="Filial"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtFilialPago" Enabled="false" runat="server"></asp:TextBox></div>
                        <%--</div>
                    <div class="EnLinea" >--%>
                    </div>
                    <asp:GridView ID="gvCamposValores" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Dato Adicional" DataField="Titulo" />
                            <asp:TemplateField HeaderText="Valor">
                                <ItemTemplate>
                                    <%# Eval("CampoValor.ListaValor").ToString() == string.Empty ? Eval("CampoValor.Valor") : Eval("CampoValor.ListaValor")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpFormasCobros"
                HeaderText="Formas de Cobro">
                <ContentTemplate>
                    <asp:CheckBoxList CssClass="checkboxlist" ID="chkFormasCobros" Enabled="false" RepeatColumns="3" RepeatDirection="Horizontal" runat="server">
                    </asp:CheckBoxList>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpAlertas"
                HeaderText="Alertas">
                <ContentTemplate>
                    <asp:CheckBoxList CssClass="checkboxlist" ID="chkAlertasTipos" Enabled="false" RepeatColumns="3" RepeatDirection="Horizontal" runat="server">
                    </asp:CheckBoxList>
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
    </div>
</asp:Content>
