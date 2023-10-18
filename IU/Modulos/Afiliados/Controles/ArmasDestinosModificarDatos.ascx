<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArmasDestinosModificarDatos.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.ArmasDestinosModificarDatos" %>

<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<div class="ArmasDestinosModificarDatos">

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblArma" runat="server" Text="Arma"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9"> <asp:DropDownList CssClass="form-control select2" ID="ddlArma" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvArma" ControlToValidate="ddlArma" ValidationGroup="ArmasDestinosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
         </div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDestino" runat="server" Text="Destino"></asp:Label>
              <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control" ID="txtDestino" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDestino" ControlToValidate="txtDestino" ValidationGroup="ArmasDestinosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
          </div></div> <div class="form-group row">
                <%--TextMode="MultiLine" Rows="3"  para hacer un textbox multilinea--%>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCalle" runat="server" Text="Calle"></asp:Label>
               <div class="col-lg-3 col-md-3 col-sm-9">  <asp:TextBox CssClass="form-control" ID="txtCalle" runat="server"></asp:TextBox>
       </div> </div> <div class="form-group row">    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumero" runat="server" Text="Numero"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9"> <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero" runat="server"></AUGE:NumericTextBox>
                
            </div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPiso" runat="server" Text="Piso"></asp:Label>
              <div class="col-lg-3 col-md-3 col-sm-9">   <AUGE:NumericTextBox CssClass="form-control" ID="txtPiso" runat="server"></AUGE:NumericTextBox>
              </div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDepartamento" runat="server" Text="Departamento"></asp:Label>
             <div class="col-lg-3 col-md-3 col-sm-9">    <asp:TextBox CssClass="form-control" ID="txtDepartamento" runat="server"></asp:TextBox>
          </div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblProvincia" runat="server" Text="Provincia"></asp:Label>
             <div class="col-lg-3 col-md-3 col-sm-9">    <asp:DropDownList CssClass="form-control select2" ID="ddlProvincia" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlProvincia_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvProvincia" ControlToValidate="ddlProvincia" ValidationGroup="ArmasDestinosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPredeterminado" runat="server" Text="Predeterminado"></asp:Label>
         <div class="col-lg-3 col-md-3 col-sm-9">        <asp:CheckBox ID="chkPredeterminado" runat="server" CssClass="form-control" />
         </div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblLocalidad" runat="server" Text="Localidad"></asp:Label>
               <div class="col-lg-3 col-md-3 col-sm-9">  <asp:DropDownList CssClass="form-control select2" ID="ddlLocalidad" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvLocalidad" ControlToValidate="ddlLocalidad" ValidationGroup="ArmasDestinosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
             </div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigoPostal" runat="server" Text="Codigo Postal"></asp:Label>
               <div class="col-lg-3 col-md-3 col-sm-9">  <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoPostal" 
                    ValidationGroup="AfiliadosDatosDomicilios"  runat="server"></AUGE:NumericTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigoPostal" ControlToValidate="txtCodigoPostal" ValidationGroup="ArmasDestinosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
              </div></div>
            </div>
            <center>
            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="ArmasDestinosModificarDatosAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" 
                    onclick="btnCancelar_Click" />
                </center>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>