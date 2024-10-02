<%@ Page Title="Delete existing book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeleteBook.aspx.cs" Inherits="WebForms._DeleteBook" Async="true" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <% if (deletedBook != null)
            {
        %>
        <form runat="server" style="display: flex; flex-direction: column">

            <span>Are you sure you want to delete this book?</span>

            <hr />

            <span>Title:</span>
            <asp:Label ID="BookTitle" runat="server" />

            <span>Author:</span>
            <asp:Label ID="BookAuthor" runat="server" />

            <span>ISBN:</span>
            <asp:Label ID="BookISBN" runat="server" />

            <span>Year:</span>
            <asp:Label ID="BookYear" runat="server" />

            <span>Quantity:</span>
            <asp:Label ID="BookQuantity" runat="server" />

            <hr />

            <asp:Button OnClick="Submit_Click" Text="Confirm" runat="server" />
            <asp:Label runat="server" ID="SuccessLabel" Visible="false" Text="Successfuly deleted" />
        </form>
        <%
            }
            else
            {
        %>
        <asp:Label runat="server" ID="ErrorLabel" />
        <%
            }%>
    </main>

</asp:Content>
