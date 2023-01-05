Imports Api
Imports BigGetClient.clsTabPage2
Imports System.Threading.Thread

Public Class OrderPlaceOrderForm

    Private Sub OrderPlaceOrderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ComboBox1.Items.Add("ETHUSDT_UMCBL")
        ComboBox1.Items.Add("BTCUSDT_UMCBL")
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.Items.Add("USDT")
        ComboBox2.SelectedIndex = 0
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.Items.Add("0.1")
        ComboBox3.Items.Add("0.2")
        ComboBox3.Items.Add("0.43")
        ComboBox3.Items.Add("0.7")
        ComboBox3.Items.Add("0.71")
        ComboBox3.Items.Add("0.01")
        ComboBox3.Items.Add("0.02")
        'ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList



    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call clsOrderPlaceOrderForm.BtnOrder()


        Me.Close()
        MainForm.Show()

    End Sub





End Class


Public Class clsOrderPlaceOrderForm

    Public Shared Sub BtnOrder()

        Dim p As New List(Of (Integer, String)) From {
                (4, ""),
                (9, "")
            }
        clsTabPage2.clsDataGridView2.ModAllRowItemData(p)


        Dim Count As Integer = Trim(OrderPlaceOrderForm.TextBox3.Text)

        If Count < 1 Then Count = 1
        For i = 1 To Count

            Dim n As Integer = 0
            Dim Nowtime1 = Now

            For Each a In clsTabPage2.clsDataGridView2.DataList

                Dim UserinfoA As New Api.UserInfo With {
                        .ApiKey = a(1),
                        .Secretkey = a(2),
                        .Passphrase = a(3),
                        .Host = "capi.bitget.com"
                    }
                Dim uAapi As New Api.UserCall(UserinfoA)
                Dim pA As New Api.UserType.ParamType.OrderPlaceOrder With {
                        .symbol = OrderPlaceOrderForm.ComboBox1.Text,
                        .marginCoin = OrderPlaceOrderForm.ComboBox2.Text,
                        .size = OrderPlaceOrderForm.ComboBox3.Text,
                        .side = "open_long",
                        .orderType = "market",
                        .presetTakeProfitPrice = Trim(OrderPlaceOrderForm.TextBox1.Text),
                        .presetStopLossPrice = Trim(OrderPlaceOrderForm.TextBox2.Text)
                    }
                Dim retA As UserType.ReplyType.OrderPlaceOrder = uAapi.OrderPlaceOrder(pA)




                Dim UserinfoB As New Api.UserInfo With {
                        .ApiKey = a(6),
                        .Secretkey = a(7),
                        .Passphrase = a(8),
                        .Host = "capi.bitget.com"
                    }
                Dim uBapi As New Api.UserCall(UserinfoB)
                Dim pB As New Api.UserType.ParamType.OrderPlaceOrder With {
                        .symbol = OrderPlaceOrderForm.ComboBox1.Text,
                        .marginCoin = OrderPlaceOrderForm.ComboBox2.Text,
                        .size = OrderPlaceOrderForm.ComboBox3.Text,
                        .side = "open_short",
                        .orderType = "market",
                        .presetTakeProfitPrice = Trim(OrderPlaceOrderForm.TextBox2.Text),
                        .presetStopLossPrice = Trim(OrderPlaceOrderForm.TextBox1.Text)
                    }
                Dim retB As UserType.ReplyType.OrderPlaceOrder = uBapi.OrderPlaceOrder(pB)

                clsDataGridView2.DataSource.Rows(n).Item(4) = retA.msg
                clsDataGridView2.DataSource.Rows(n).Item(9) = retB.msg
                n = n + 1



            Next

            Dim Nowtime2 = Now
            Dim interval As TimeSpan = Nowtime2 - Nowtime1

            If interval.TotalMilliseconds < 2000 Then
                Sleep(2000 - interval.TotalMilliseconds)
            End If

            OrderPlaceOrderForm.Text = "批量下单" & "    进度：" & n & "/" & Count

        Next





    End Sub




End Class