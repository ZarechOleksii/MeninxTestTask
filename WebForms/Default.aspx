﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForms._Default" Async="true" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <table class="table">
            <tr>
                <th>Title</th>
                <th>Author</th>
                <th>ISBN</th>
                <th>PublicationYear</th>
                <th>Quantity</th>
            </tr>

            <asp:Repeater ID="tableRows" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Title") %></td>
                        <td><%# Eval("Author") %></td>
                        <td><%# Eval("ISBN") %></td>
                        <td><%# Eval("PublicationYear") %></td>
                        <td><%# Eval("Quantity") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </main>

</asp:Content>
