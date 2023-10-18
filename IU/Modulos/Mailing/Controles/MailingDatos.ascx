<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailingDatos.ascx.cs" Inherits="IU.Modulos.Mailing.Controles.MailingDatos" %>

<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProceso" runat="server" Text="Proceso" ></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlProceso" Enabled="false" OnSelectedIndexChanged="ddlProceso_OnSelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
               <asp:RequiredFieldValidator CssClass="Validador" ID="rfvProceso" ControlToValidate="ddlProceso" ValidationGroup="MailingPrepararEnvio" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>
    </div>
    <div class="col-sm-3"></div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaInicio" runat="server" Text="Fecha Inicio"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaInicio" Enabled="false" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaInicio" ControlToValidate="txtFechaInicio" ValidationGroup="MailingAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaFin" runat="server" Text="Fecha Fin"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFin" Enabled="false" runat="server"></asp:TextBox>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriocidad" runat="server" Text="Periocidad"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlPeriocidad" Enabled="false" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPerdiocidad" ControlToValidate="ddlPeriocidad" ValidationGroup="MailingAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDiaEjecucion" runat="server" Text="Dia Ejecucion"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtDiaEjecucion" Enabled="false" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDiaEjecucion" ControlToValidate="txtDiaEjecucion" ValidationGroup="MailingAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoPlantilla" runat="server" Text="Plantilla"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlPlantilla" Enabled="false" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPlantilla" ControlToValidate="ddlPlantilla" ValidationGroup="MailingAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
       
    <asp:UpdatePanel ID="UpdatePanel1" RenderMode="Inline" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Button CssClass="botonesEvol" runat="server" ID="btnEditarPlantilla" Visible="true" Text="Editar"
                ToolTip="Editar Plantilla" OnClick="btnEditarPlantilla_Click" />

<%--            <asp:Button CssClass="botonesEvol" runat="server" ID="btnPruebaEnvio" Visible="false" Text="Prueba de Envio"
                OnClick="btnPruebaEnvio_Click" />--%>

            <asp:Button CssClass="botonesEvol" runat="server" ID="btnEjecutarAhora" Visible="false" Text="Agregar"
                OnClick="btnAgregar_Click" />

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAdjuntos" runat="server" Text="Adjuntos"></asp:Label>
    <div class="col-sm-2">
        <asp:DropDownList CssClass="form-control select2" ID="ddlAdjuntos" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvAdjuntos" ControlToValidate="ddlAdjuntos" ValidationGroup="MailingAdjuntos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <div class="col-sm-1">
        <asp:Button CssClass="botonesEvol" runat="server" ID="btnAdjuntosAgregar" Visible="false" Text="Agregar"
            OnClick="btnAdjuntosAgregar_Click" ValidationGroup="MailingAdjuntos" />
    </div>
      
      <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAsunto" runat="server" Text="Asunto"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtAsunto"  runat="server"></asp:TextBox>
       </div>
</div>
  <asp:Panel ID="tablaParametros" CssClass="form-group" runat="server">
        </asp:Panel>

      <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
            SkinID="MyTab">
           <asp:TabPanel runat="server" ID="tpPlantillasAdjuntos"
                HeaderText="Adjuntos Plantillas" TabIndex="0">         <ContentTemplate>
 
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" DataKeyNames="IdMailingAdjunto" OnPageIndexChanging="gvDatos_PageIndexChanging"
        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="False" Visible="false">
        <Columns>
            <asp:TemplateField HeaderText="Plantilla">
                <ItemTemplate>
                    <%# Eval("Plantilla.NombrePlantilla")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="Modificar"  ToolTip="Modificar" />
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Eliminar" ID="btnEliminar"
                        AlternateText="Eliminar" ToolTip="Eliminar" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView></ContentTemplate></asp:TabPanel>
            









        
          </asp:TabContainer>


<asp:UpdatePanel ID="UpdatePanel2" RenderMode="Inline" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    OnClick="btnAceptar_Click" ValidationGroup="MailingAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                    OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
