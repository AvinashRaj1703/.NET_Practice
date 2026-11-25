
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="WebForm1.aspx.cs"
    Inherits="practical8.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Management</title>

    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            background-color: white;
            color: black;
            margin: 0;
            padding: 25px;
        }

        .container {
            max-width: 900px;
            margin: auto;
            padding: 20px;
            border: 1px solid black;
        }

        h2 {
            text-align: center;
        }

        .form-table {
            margin: auto;
            border-spacing: 10px;
        }

        .form-table td:first-child {
            font-weight: bold;
            padding-right: 15px;
        }

        .textbox {
            width: 250px;
            padding: 7px;
            border: 1px solid black;
            box-sizing: border-box;
        }

        .buttons {
            text-align: center;
            padding-top: 10px;
        }

        .btn {
            padding: 8px 18px;
            margin: 4px;
            background-color: white;
            color: black;
            border: 1px solid black;
            cursor: pointer;
        }

        .message {
            display: block;
            text-align: center;
            margin: 15px 0;
        }

        .section-title {
            margin-top: 30px;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }

        .grid th,
        .grid td {
            border: 1px solid black;
            padding: 9px;
            text-align: center;
        }

        .grid th {
            font-weight: bold;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <div class="container">

            <h2>Inventory Management</h2>

            <table class="form-table">

                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server"
                            Text="PROD_ID"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server"
                            CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server"
                            Text="PROD_NAME"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server"
                            CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server"
                            Text="COST_PRICE"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox3" runat="server"
                            CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server"
                            Text="SELLING_PRICE"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server"
                            CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server"
                            Text="QTY"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox5" runat="server"
                            CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" class="buttons">

                        <asp:Button ID="Button1" runat="server"
                            Text="INSERT"
                            CssClass="btn"
                            OnClick="Button1_Click" />

                        <asp:Button ID="Button2" runat="server"
                            Text="UPDATE"
                            CssClass="btn"
                            OnClick="Button2_Click" />

                        <asp:Button ID="Button3" runat="server"
                            Text="DELETE"
                            CssClass="btn"
                            OnClick="Button3_Click"
                            OnClientClick="return confirm('Are you sure you want to delete this product?');" />

                    </td>
                </tr>

            </table>

            <asp:Label ID="lblMessage" runat="server"
                CssClass="message"></asp:Label>

            <h3 class="section-title">Inventory Records</h3>

            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="PROD_ID"
                CssClass="grid"
                GridLines="Both"
                EmptyDataText="No records found."
                ShowHeaderWhenEmpty="True">

                <Columns>
                    <asp:BoundField
                        DataField="PROD_ID"
                        HeaderText="PROD_ID" />

                    <asp:BoundField
                        DataField="PROD_NAME"
                        HeaderText="PROD_NAME" />

                    <asp:BoundField
                        DataField="COST_PRICE"
                        HeaderText="COST_PRICE"
                        DataFormatString="{0:F2}" />

                    <asp:BoundField
                        DataField="SELLING_PRICE"
                        HeaderText="SELLING_PRICE"
                        DataFormatString="{0:F2}" />

                    <asp:BoundField
                        DataField="QTY"
                        HeaderText="QTY" />
                </Columns>

            </asp:GridView>

        </div>

    </form>
</body>
</html>
