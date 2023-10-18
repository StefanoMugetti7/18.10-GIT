<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EjerciciosContablesDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.EjerciciosContablesDatos" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="EjerciciosContablesDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripción" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" />
           <asp:RequiredFieldValidator ID="rfvDescripcion" ValidationGroup="Aceptar" ControlToValidate="txtDescripcion" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>

        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaInicio" runat="server" Text="Fecha Inicio" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaInicio" runat="server" />
       <asp:RequiredFieldValidator ID="rfvFechaInicio" ValidationGroup="Aceptar" ControlToValidate="txtFechaInicio" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>

        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaFin" runat="server" Text="Fecha Fin" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFin" runat="server" />
               <asp:RequiredFieldValidator ID="rfvFechaFin" ValidationGroup="Aceptar" ControlToValidate="txtFechaFin" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>

        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaCierre" runat="server" Text="Fecha Cierre" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaCierre" runat="server" />

        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaCopiativo" runat="server" Text="Fecha Copiativo" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaCopiativo" runat="server" />

        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Seleccione un ejercicio contable para copiar el Plan de Cuentas:" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server"></asp:DropDownList>
               <asp:RequiredFieldValidator ID="rfvEjercicioContable" Enabled="false" ValidationGroup="Aceptar" ControlToValidate="ddlEjercicioContable" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        
        </div>
        <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnActualizarPlan" runat="server" Text="Actualizar Plan de Cuentas" onclick="btnActualizarPlan_Click"  /></div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
