<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewCheckDinamico.ascx.cs" Inherits="IU.Modulos.ProcesosDatos.Controles.GridViewCheckDinamico" %>

<script type="text/javascript">

    var gridViewId = $('[id$="gvDatos"]').attr('id');
    
    function checkAllRow(selectAllCheckbox) {
        //get all checkboxes within item rows and select/deselect based on select all checked status
        //:checkbox is jquery selector to get all checkboxes
        $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
        updateSelectionLabel();
    }
    function CheckRow(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as well
        if (!selectCheckbox.checked)
            $('th :checkbox', gridViewId).prop("checked", false);
        updateSelectionLabel();
    }
    function updateSelectionLabel() {
        //update the caption element with the count of selected items. 
        //:checked is jquery selector to get list of checked checkboxes
        $('caption', gridViewId).html($('td :checkbox:checked', gridViewId).length + " options selected");
    }


</script>
<style type="text/css">
   .GridViewHeaderStyle {
    position: -webkit-sticky; /* this is for all Safari (Desktop & iOS), not for Chrome*/
    position: sticky;
    top: 0;
    z-index: 1; /* any positive value, layer order is global*/
}
</style>
<asp:UpdatePanel ID="upArchivos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-3">
                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                        runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                </div>
                <div class="col-sm-9"></div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
</asp:UpdatePanel>
<div class="overflow-auto" style="max-height:500px">
            <asp:GridView ID="gvDatos" AllowPaging="false" AllowSorting="false" 
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="true" ShowFooter="true"
            OnRowCreated="gvDatos_RowCreated" OnRowDataBound="gvDatos_RowDataBound" >
                <Columns>                    
                    <asp:TemplateField HeaderText="Incluir" >
                            <ItemTemplate>
                            </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" />
                            <asp:HiddenField ID="hdfIdValor" runat="server" />
                        </ItemTemplate>
                     </asp:TemplateField>
                    </Columns>
            </asp:GridView>
</div>