
Namespace Program.Form.ParentForm.DataGridView

    Public Class DataGridView1Class

        Public Shared Sub Initialize()


            Call PublicGetTickets.OpenTableFromDatabase()
            MainForm.DataGridView1.DataSource = BigGetStrategy.PublicConf.DtTickets.Tables(PublicGetTickets.TableName)
            PublicGetTickets.Run()

        End Sub


    End Class

End Namespace