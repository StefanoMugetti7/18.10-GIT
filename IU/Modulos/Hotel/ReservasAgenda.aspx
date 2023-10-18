<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ReservasAgenda.aspx.cs" Inherits="IU.Modulos.Hotel.ReservasAgenda" %>
 <%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitSelect2);
            InitSelect2();
        });

        function InitSelect2() {
            var ddlHoteles = $("select[name$='ddlHoteles']");
            ddlHoteles.select2({ width: '100%' });
        }

        
    </script>

    <asp:UpdatePanel ID="upAgenda" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

               <div class="form-group row">
      <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblHotel" runat="server" Text="Hotel"></asp:Label>
  <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlHoteles_SelectedIndexChanged" ID="ddlHoteles" runat="server">
        </asp:DropDownList>
   </div>
    <div class="col-lg-3 col-md-3 col-sm-9"> 
        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
</div></div>
     <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFecha" runat="server" Text="Agenda"></asp:Label>
         <div class="col-lg-3 col-md-3 col-sm-9">    
             <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" runat="server"></asp:TextBox>
        </div>
         <div class="col-lg-3 col-md-3 col-sm-9"> 
         <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
             </div>
     </div>
            <DayPilot:DayPilotScheduler ID="DayPilotScheduler1" runat="server"
                HeaderFontSize="8pt" HeaderHeight="20"
                DataStartField="FechaIngreso"
                DataEndField="FechaEgreso"
                DataTextField="ReservaDetalle"
                DataValueField="IdReserva"
                DataResourceField="IdHabitacion"
                EventFontSize="11px"
                CellDuration="1440"
                OnBeforeEventRender="DayPilotScheduler1_BeforeEventRender"
                EventHeight="40"
                CellWidth="35"
                OnEventClick="DayPilotScheduler1_OnEventClick"
                EventClickHandling="PostBack"
                OnTimeRangeSelected="DayPilotScheduler1_TimeRangeSelected"
                TimeRangeSelectedHandling="PostBack"
                >
                <HeaderColumns>
                    <DayPilot:RowHeaderColumn Title="Habitacion" Width="200" />
                </HeaderColumns>
            </DayPilot:DayPilotScheduler>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>