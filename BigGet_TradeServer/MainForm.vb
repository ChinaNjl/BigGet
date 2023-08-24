



Imports Api
Imports BigGet_TradeServer.Program.Form.ParentForm.DataGridView
Imports Google.Protobuf.WellKnownTypes
Imports System.Threading.Thread

Public Class MainForm



    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Call Program.Form.ParentForm.MainFormClass.SetBigGetStrategySql()
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

    End Sub


    Private Sub 测试ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 测试ToolStripMenuItem.Click



    End Sub

    Private Function a(b As Integer) As Boolean
        If b > 5 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub 更新数据库ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 更新数据库ToolStripMenuItem.Click

        Dim UtcTime = TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        Dim ts As TimeSpan = Date.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
        UtcTime = CType(ts.TotalMilliseconds, Int64)

        Sleep(1000)

        Dim UtcTime1 = TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        ts = Date.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
        UtcTime1 = CType(ts.TotalMilliseconds, Int64)

        Debug.Print(UtcTime1 - UtcTime)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick



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



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If PublicGetUserStrategy.WorkerIsBusy Then
            PublicGetUserStrategy.StopRun()
        Else
            PublicGetUserStrategy.Run()
        End If
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
End Class









