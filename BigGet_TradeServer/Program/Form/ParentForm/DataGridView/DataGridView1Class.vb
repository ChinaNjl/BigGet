Imports BigGetStrategy
Imports BigGetStrategy.PublicData

Namespace Program.Form.ParentForm.DataGridView

    Public Class DataGridView1Class

        ''' <summary>
        ''' 功能初始化
        ''' </summary>
        Public Shared Sub Initialize()

            '打开后台读取bigget信息
            PublicGetTickers.Run()
            MainForm.DataGridView1.DataSource = BigGetStrategy.PublicConf.Tickers.Tables("tickertable")
            PublicData.Run()

        End Sub

    End Class

End Namespace