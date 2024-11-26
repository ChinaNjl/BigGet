Namespace Program.Form.ParentForm.DataGridView

    Public Class DataGridView3Class

        ''' <summary>
        ''' 初始化 DataGridView3控件
        ''' </summary>
        Public Shared Sub Initialize()

            '读取初始化所有策略信息
            Call PublicGetUserStrategy.OpenTableFromDatabase()

            '将用户策略表设置为dgv3的源信息
            MainForm.DataGridView3.DataSource = PublicGetUserStrategy.ds.Tables(PublicGetUserStrategy.TableName)

            '启动策略
            PublicGetUserStrategy.Run()

        End Sub

    End Class

End Namespace