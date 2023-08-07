Imports System.Threading.Thread
Imports Api

Public Class SingleOrderPlaceOrderForm
    Private Sub SingleOrderPlaceOrderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList

        ComboBox1.Items.Clear()
        If clsTabPage1.clsDataGridView1.DgvRowCount > 0 Then

            For Each l In clsTabPage1.clsDataGridView1.DataList

                Dim str As String = Strings.Join(CType(l, Object), ",")

                ComboBox1.Items.Add(str)

            Next

        End If


        ComboBox2.Items.Add("ETHUSDT_UMCBL")
        ComboBox2.Items.Add("BTCUSDT_UMCBL")
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.Items.Add("USDT")
        ComboBox3.SelectedIndex = 0
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox4.Items.Add("0.1")
        ComboBox4.Items.Add("0.01")
        'ComboBox4.DropDownStyle = ComboBoxStyle.DropDownList

        ComboBox5.Items.Add("open_long")
        ComboBox5.Items.Add("open_short")
        ComboBox5.DropDownStyle = ComboBoxStyle.DropDownList


    End Sub

    Private Sub SingleOrderPlaceOrderForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        MainForm.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call clsSingleOrderPlaceOrderForm.BtnSingleOrder()
    End Sub
End Class


Public Class clsSingleOrderPlaceOrderForm

    Public Shared Sub BtnSingleOrder()

        '清空dgv2状态栏
        Dim p As New List(Of (Integer, String)) From {
            (4, ""),
            (9, "")
        }
        clsTabPage2.clsDataGridView2.ModAllRowItemData(p)

        '获取下单次数，默认1
        Dim Count As Integer = Trim(SingleOrderPlaceOrderForm.TextBox3.Text)
        If Count < 1 Then Count = 1

        '循环下单
        For i = 1 To Count

            Dim UserInfo As String = SingleOrderPlaceOrderForm.ComboBox1.Text
            Dim ArrUserInfo As Array = Split(UserInfo, ",")

            Dim UserinfoA As New Api.UserInfo With {
                .ApiKey = ArrUserInfo(1),
                .Secretkey = ArrUserInfo(2),
                .Passphrase = ArrUserInfo(3),
                .Host = "capi.bitget.com"
            }
            Dim UserTrade As New Api.UserCall(UserinfoA)

            Dim pA As New Api.UserType.ParamType.OrderPlaceOrder With {
                .symbol = SingleOrderPlaceOrderForm.ComboBox2.Text,
                .marginCoin = SingleOrderPlaceOrderForm.ComboBox3.Text,
                .size = SingleOrderPlaceOrderForm.ComboBox4.Text,
                .side = SingleOrderPlaceOrderForm.ComboBox5.Text,
                .orderType = "market",
                .presetTakeProfitPrice = Trim(SingleOrderPlaceOrderForm.TextBox1.Text),
                .presetStopLossPrice = Trim(SingleOrderPlaceOrderForm.TextBox2.Text)
            }


            Debug.Print(pA.ToJson)

            Dim retA As UserType.ReplyType.OrderPlaceOrder = UserTrade.OrderPlaceOrder(pA)


            Debug.Print(retA.tojson)

            SingleOrderPlaceOrderForm.Text = "单账户补单" & "    进度：" & i & "/" & Count

            Sleep(1000)

        Next

    End Sub

End Class




