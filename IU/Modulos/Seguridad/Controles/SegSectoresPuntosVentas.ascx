<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegSectoresPuntosVentas.ascx.cs" Inherits="IU.Modulos.Seguridad.Controles.SegSectoresPuntosVentas" %>
<div class="SectoresPuntosVentas">
    <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" AutoPostBack="true" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvFilial" ControlToValidate="ddlFilial"  ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>      
    </div>
             <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblSector" runat="server" Text="Sector"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlSector"  runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvSector" ControlToValidate="ddlSector" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>      
    </div>
             <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>   
        </div>
    <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPuntoVenta" runat="server" Text="Punto Venta"></asp:Label>

            <div class="col-lg-3 col-md-3 col-sm-9">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtPuntoVenta" runat="server"  NumberOfDecimals="0" Prefix="" ></Evol:CurrencyTextBox>
                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvPuntoVenta" ControlToValidate="txtPuntoVenta"  ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>      
            </div>
        </div>
      <div class="row justify-content-md-center">
        <div class="col-md-auto">
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
        </div>
    </div>
</div>