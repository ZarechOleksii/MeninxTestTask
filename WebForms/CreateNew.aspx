<%@ Page Title="Add new book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateNew.aspx.cs" Inherits="WebForms._CreateNew" Async="true" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <form runat="server" style="display:flex;flex-direction:column">

            <span>Title:</span>
            <asp:TextBox ID="BookTitle" runat="server" />

            <span>Author:</span>
            <asp:TextBox ID="BookAuthor" runat="server" />

            <span>ISBN:</span>
            <asp:TextBox ID="BookISBN" runat="server" />

            <span>Year:</span>
            <asp:TextBox ID="BookYear" runat="server" />

            <span>Quantity:</span>
            <asp:TextBox ID="BookQuantity" runat="server" />

            <span>Category:</span>
            <asp:DropDownList runat="server" ID="BookCategory" />

            <asp:Button OnClick="Submit_Click" Text="Submit" runat="server" />
            <asp:Label runat="server" ID="SuccessLabel" Visible="false" Text="Success"></asp:Label>
            <asp:CustomValidator ID="BookValidator" runat="server" ValidationGroup="BookGroup" OnServerValidate="BookValidator_ServerValidate" Display="Dynamic" />
            <asp:ValidationSummary ValidationGroup="BookGroup" ForeColor="Red" runat="server" ID="BookValidation" DisplayMode="BulletList" HeaderText="Your input is incorrect:" />
        </form>
    </main>

</asp:Content>
