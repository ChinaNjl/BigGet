Namespace Program.Form.ParentForm.DataGridView

    Public Class DataGridView2Class

        Public Shared Sub Initialize()

            '读取策略信息
            PublicGetContracts.Run()
            MainForm.DataGridView2.DataSource = BigGetStrategy.PublicConf.PublicData.Tables("contracttable")
            'MainForm.DataGridView2.DataSource = BigGetStrategy.PublicConf.PublicData.Tables("spot_tickers")
        End Sub

    End Class

End Namespace