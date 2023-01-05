



Imports Api
Imports BigGet_TradeServer.Program.Form.ParentForm.DataGridView

Public Class MainForm



    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call StrategyInitialize()

        Call Program.Form.ParentForm.MainFormClass.Initialize()
        Call Program.Form.ParentForm.MainFormClass.Initialize_ProgramSettings()


        DataGridView1Class.Initialize()
        DataGridView2Class.Initialize()
        DataGridView3Class.Initialize()
        Timer1.Enabled = True
        Program.Form.ParentForm.MainFormClass.UploadState = True
    End Sub

    Private Sub MainForm_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If Program.Form.ParentForm.MainFormClass.UploadState = True Then
            Call Program.Form.ParentForm.MainFormClass.Initialize()
        End If
    End Sub

    Private Sub MainForm_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        If Program.Form.ParentForm.MainFormClass.UploadState = True Then
            Call Program.Form.ParentForm.MainFormClass.Initialize()
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged

        Call Program.Form.ParentForm.MainFormClass.Initialize()


        Select Case TabControl1.SelectedIndex
            Case 0
                'Call Program.Form.ParentForm.DataGridView.DataGridView1Class.StartGetTickets()
            Case 1
                Call PublicGetContracts.Run()
        End Select


    End Sub


    Private Sub 测试ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 测试ToolStripMenuItem.Click
        PublicGetUserStrategy.Update()
    End Sub

    Private Sub 更新数据库ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 更新数据库ToolStripMenuItem.Click

        DataGridView3.DataSource = PublicGetUserStrategy.ds.Tables(PublicGetUserStrategy.TableName)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If PublicGetTickets.WorkerIsBusy Then
            Label1.Text = "后台读取行情.....打开"
        Else
            Label1.Text = "后台读取行情.....关闭"
        End If

        If PublicGetUserStrategy.WorkerIsBusy Then
            Label2.Text = "后台获取策略.....打开"
        Else
            Label2.Text = "后台获取策略.....关闭"
        End If

        If PublicGetUserStrategy.bgwList.Count > 0 Then
            If PublicGetUserStrategy.bgwList(0).bgw.IsBusy Then
                Label3.Text = "后台策略运行.....打开"
            Else
                Label3.Text = "后台策略运行.....关闭"
            End If
        End If






    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If PublicGetTickets.WorkerIsBusy Then
            PublicGetTickets.StopRun()
        Else
            PublicGetTickets.Run()
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If PublicGetUserStrategy.WorkerIsBusy Then
            PublicGetUserStrategy.StopRun()
        Else
            PublicGetUserStrategy.Run()
        End If
    End Sub
End Class









