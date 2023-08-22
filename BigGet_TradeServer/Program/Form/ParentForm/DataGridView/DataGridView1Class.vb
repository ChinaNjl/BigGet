
Imports BigGetStrategy.PublicData

Namespace Program.Form.ParentForm.DataGridView

    Public Class DataGridView1Class

        ''' <summary>
        ''' 功能初始化
        ''' </summary>
        Public Shared Sub Initialize()

            '初始化dataset结构，然后设置datagridview数据源
            Call PublicGetTickers.ReadTable()
            MainForm.DataGridView1.DataSource = BigGetStrategy.PublicConf.Tickers.Tables(PublicGetTickers.TableName)

            '打开后台读取bigget信息
            PublicGetTickers.Run()

        End Sub


    End Class

End Namespace