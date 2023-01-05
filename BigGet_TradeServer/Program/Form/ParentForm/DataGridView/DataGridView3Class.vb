


Namespace Program.Form.ParentForm.DataGridView


    Public Class DataGridView3Class
        Public Shared Sub Initialize()

            Call PublicGetUserStrategy.OpenTableFromDatabase()
            MainForm.DataGridView3.DataSource = PublicGetUserStrategy.ds.Tables(PublicGetUserStrategy.TableName)
            'PublicGetUserStrategy.Run()

        End Sub
    End Class


End Namespace