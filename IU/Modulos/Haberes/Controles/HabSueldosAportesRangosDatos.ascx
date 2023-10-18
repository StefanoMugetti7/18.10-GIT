<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HabSueldosAportesRangosDatos.ascx.cs" Inherits="IU.Modulos.Haberes.Controles.HabSueldosAportesRangosDatos" %>
<div class="PlazosFijosDatos">
    <asp:UpdatePanel ID="upSaldoActual" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIngresoDesde" runat="server" Text="Ingreso Desde"></asp:Label>
                <div class=" col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtIngresoDesde" Prefix="" runat="server"></asp:TextBox>

                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvIngresoDesde" runat="server" ControlToValidate="txtIngresoDesde"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIngresoHasta" runat="server" Text="Ingreso Hasta"></asp:Label>
                <div class=" col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtIngresoHasta" Prefix="" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvIngresoHasta" runat="server" ControlToValidate="txtIngresoHasta"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estado"></asp:Label>
     <div class=" col-sm-3"><asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
            </div></div>
                 <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAnioMinimo" runat="server" Text="Año minimo"></asp:Label>
                <div class=" col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtAnioMinimo" Prefix="" NumberOfDecimals="0" runat="server"></Evol:CurrencyTextBox>

                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvAnioMinimo" runat="server" ControlToValidate="txtAnioMinimo"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAnioMaximo" runat="server" Text="Año maximo"></asp:Label>
                <div class=" col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtAnioMaximo" Prefix="" NumberOfDecimals="0" runat="server"></Evol:CurrencyTextBox>

                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvAnioMaximo" runat="server" ControlToValidate="txtAnioMaximo"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPorcentajeMinimo" runat="server" Text="Porcentaje Aporte Minimo"></asp:Label>
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeMinimo" runat="server" NumberOfDecimals="4" Prefix=""></Evol:CurrencyTextBox>
                     <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPorcentajeMinimo" runat="server" ControlToValidate="txtPorcentajeMinimo"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPorcentajeMaximo" runat="server" Text="Porcentaje Aporte Maximo"></asp:Label>
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeMaximo" runat="server" NumberOfDecimals="4" Prefix=""></Evol:CurrencyTextBox>
                     <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPorcentajeMaximo" runat="server" ControlToValidate="txtPorcentajeMaximo"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
            
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">

                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
