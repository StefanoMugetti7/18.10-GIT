<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="IU.menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <style type="text/css">
        .level1
        {
            color: White;
            background-color: Black;
            font-variant: small-caps;
            font-size: large;
            font-weight: bold;
        }

        .level2
        {
            color: Blue;
            font-family: Gill Sans MT !important;
            font-size: medium;
            background-color: Gray;
        }

        .level3
        {
            color: black;
            background-color: yellow;
            font-family: Gill Sans MT !important;
            font-size: small;
        }
        
         .level4
        {
            background-color: Gray !important;
            color: Green !important;
            font-size: small;
        } 

        .hoverstyle
        {
            font-weight: bold;
        }

    </style>
    <asp:menu id="NavigationMenu1" CssClass=""
  staticdisplaylevels="1"
  staticsubmenuindent="0" 
  orientation="Horizontal"
  target="_blank"
  Font-names="Arial, Gill Sans"
  Width="100px"
  runat="server">

  <LevelMenuItemStyles>
    <asp:MenuItemStyle CssClass="level1"/>
    <asp:MenuItemStyle CssClass="level2"/>
    <asp:MenuItemStyle CssClass="level3" />
    <asp:MenuItemStyle CssClass="level4" />
  </LevelMenuItemStyles>
  
  <StaticHoverStyle CssClass="hoverstyle"/>
  
  <items>
    <asp:menuitem text="Home" tooltip="Home">
    <asp:menuitem text="Section 1" tooltip="Section 1">
      <asp:menuitem text="Item 1" tooltip="Item 1"/>
      <asp:menuitem text="Item 2" tooltip="Item 2"/>
      <asp:menuitem text="Item 3" tooltip="Item 3"/>
    </asp:menuitem>
    <asp:menuitem text="Section 2" tooltip="Section 2">
      <asp:menuitem text="Item 1" tooltip="Item 1"/>
      <asp:menuitem text="Item 2" tooltip="Item 2">
        <asp:menuitem text="Subitem 1"/>
        <asp:menuitem text="Subitem 2" />
      </asp:menuitem>
      <asp:menuitem text="Item 3" tooltip="Item 3"/>
    </asp:menuitem>
  </asp:menuitem>
  </items>
</asp:menu>
    </div>
    </form>
</body>
</html>
