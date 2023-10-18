<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpCajas.master" AutoEventWireup="true" CodeBehind="CajasTraspasoEfectivo.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasTraspasoEfectivo" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
<div class="TesoreriasMovimientosAgregar">
    <asp:UpdatePanel ID="upMovimientos" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
                <auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" OnSelectedIndexChanged="ddlMoneda_OnSelectedIndexChanged"
                    AutoPostBack="true"/>
            </div>

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldoActual" runat="server" Text="Saldo Caja" />
                    <div class="col-sm-3"><AUGE:CurrencyTextBox CssClass="form-control" ID="txtSaldoActual" runat="server" />
                </div>
                </div>
             <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCajero" runat="server" Text="Cajero" />
                <div class="col-sm-3">    <asp:DropDownList CssClass="form-control select2" ID="ddlCajero" runat="server" OnSelectedIndexChanged="ddlCajero_OnSelectedIndexChanged"
                    AutoPostBack="true"/>
                <asp:RequiredFieldValidator ID="rfvCajero" CssClass="Validador" ValidationGroup="Aceptar" ControlToValidate="ddlCajero" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
           </div>
                
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCaja" runat="server" Text="Número de Caja" />
                <div class="col-sm-3">    <asp:TextBox CssClass="form-control" ID="txtNumeroCaja" runat="server" />
                    </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
                   <div class="col-sm-3"> <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte" 
                    ErrorMessage="*" ValidationGroup="Aceptar"/>
              </div></div>
             <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion" />
                <div class="col-sm-3">    <asp:TextBox CssClass="form-control" ID="txtDescripcion" TextMode="MultiLine" runat="server" />
             </div>
                    </div>
              <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
             </div>
                  </div>


        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>