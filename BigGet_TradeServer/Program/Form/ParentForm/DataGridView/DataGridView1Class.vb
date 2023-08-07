
Namespace Program.Form.ParentForm.DataGridView

    Public Class DataGridView1Class

        Public Shared Sub Initialize()


            Call PublicGetTickets.OpenTableFromDatabase()

            '将biggetstrategy.publicconf.dttickets绑定到表的数据源
            MainForm.DataGridView1.DataSource = BigGetStrategy.PublicConf.DtTickets.Tables(PublicGetTickets.TableName)

            PublicGetTickets.Run()

        End Sub


    End Class

End Namespace