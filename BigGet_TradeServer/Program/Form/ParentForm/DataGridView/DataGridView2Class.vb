


Namespace Program.Form.ParentForm.DataGridView

    Public Class DataGridView2Class

        Public Shared Sub Initialize()

            Call PublicGetContracts.OpenTableFromDatabase()

            MainForm.DataGridView2.DataSource = BigGetStrategy.PublicConf.DtContracts.Tables("contracttable")

            '读取策略信息
            PublicGetContracts.Run()

        End Sub

    End Class


End Namespace