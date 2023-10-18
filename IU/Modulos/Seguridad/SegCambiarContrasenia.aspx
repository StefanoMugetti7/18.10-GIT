<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SegCambiarContrasenia.aspx.cs" Inherits="IU.Modulos.Seguridad.SegCambiarContrasenia" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .VeryPoor {
            background: Red;
            color: White;
            font-weight: bold;
        }

        .Average {
            background: orange;
            color: black;
            font-weight: bold;
        }

        .Good {
            background: Yellow;
            color: White;
            font-weight: bold;
        }

        .Excellent {
            background: LimeGreen;
            color: White;
            font-weight: bold;
        }

        .border {
            border-style: solid;
            border-width: 0.5px;
            width: 210px;
            padding: 0px 0px 0px 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlCargarDatos" runat="server">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" runat="server" ID="lblContraseniaAnterior" Text="Contraseña anterior"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtContraseniaAnterior" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvContraseniaAnterior" runat="server"
                            ControlToValidate="txtContraseniaAnterior" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" runat="server" ID="lblContrasenia" Text="Contraseña"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtContrasenia" runat="server" Style="text-align: left" TextMode="Password" EnableViewState="false"></asp:TextBox>
                        <%-- BAR INDICATOR --%>
                        <Ajax:PasswordStrength ID="PasswordStrength1" runat="server" DisplayPosition="RightSide" StrengthIndicatorType="BarIndicator"
                            TargetControlID="txtContrasenia" PrefixText="Stength:" Enabled="true"
                            RequiresUpperAndLowerCaseCharacters="true" MinimumLowerCaseCharacters="0"
                            MinimumUpperCaseCharacters="0" MinimumSymbolCharacters="0"
                            MinimumNumericCharacters="0" PreferredPasswordLength="10"
                            TextStrengthDescriptions="VeryPoor; Average; Good; Excellent"
                            StrengthStyles="VeryPoor; Average; Good ;Excellent "
                            CalculationWeightings="35;25;15;25" BarBorderCssClass="border"
                            HelpStatusLabelID="Label1" EnableViewState="true"></Ajax:PasswordStrength>
                        <%--<HASTA AQUI BAR INDICATOR>--%>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txtContrasenia" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <%--<div class="Espacio" />--%>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" runat="server" ID="lblRepetirContrasenia" Text="Repetir contraseña"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtRepetirContrasenia" TextMode="Password" runat="server"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvRepetirContrasenia" runat="server" 
                                    ControlToValidate="txtRepetirContrasenia" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                        <asp:CompareValidator ID="cvRepetirContrasenia" runat="server"
                            ControlToCompare="txtContrasenia" ControlToValidate="txtRepetirContrasenia"
                            ErrorMessage="*"></asp:CompareValidator>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <center>
                            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server"
                                Text="Aceptar" onclick="btnAceptar_Click" />
                            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server"
                                Text="Volver" CausesValidation="false" onclick="btnCancelar_Click" />
                        </center>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
