Public Class EnterKeyForm
    Private Sub EnterKeyForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "备注名"
        Label2.Text = "APIKey"
        Label3.Text = "Secretkey"
        Label4.Text = "Passphrase"
        Button1.Text = "确认"
    End Sub

    Private Sub EnterKeyForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        MainForm.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim key As New KeyType With {
            .Remark = TextBox1.Text,
            .APIKey = TextBox2.Text,
            .Secretkey = TextBox3.Text,
            .Passphrase = TextBox4.Text
        }

        If key.CheckData = False Then
            MessageBox.Show("key信息录入有误，请检查数据合法性", "错误")
            Exit Sub
        End If


        If clsTabPage1.clsDataGridView1.FindRepeat(key.Remark， 0) Or
           clsTabPage1.clsDataGridView1.FindRepeat(key.APIKey， 1) Or
           clsTabPage1.clsDataGridView1.FindRepeat(key.Secretkey， 2) Then

            MessageBox.Show("备注名，APIKey，Secretkey不能重复输入。请检查输入信息。")
            Me.Close()
        Else
            clsTabPage1.clsDataGridView1.AddRow(key.ToList.ToArray)
            Me.Close()
        End If

    End Sub

End Class