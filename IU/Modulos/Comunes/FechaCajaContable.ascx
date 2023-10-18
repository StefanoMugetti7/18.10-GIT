<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FechaCajaContable.ascx.cs" Inherits="IU.Modulos.Comunes.FechaCajaContable" %>

<asp:Label CssClass="col-3 col-md-2 col-lg-1 col-form-label" ID="lblFechaCajaContabilizacion" runat="server" Text="Fecha Caja y Contabilizacion"></asp:Label>
     <div class="col-9 col-md-6 col-lg-3" id="divCajaContabilizacion" runat="server" >
<asp:TextBox CssClass="form-control" ID="txtFechaCajaContabilizacion" Enabled="false" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator CssClass="Validador" Enabled="false"  ID="rfvFechaCajaContabilizacion" ControlToValidate="txtFechaCajaContabilizacion" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
     </div>
  