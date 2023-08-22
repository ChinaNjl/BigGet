Imports Microsoft.SqlServer.Server

Public Class config

    Public Shared Sql As New BigGetStrategy.UserType.SqlInfo With {
            .SqlServer = "43.154.0.215",
            .SqlUser = "BigGetTrade",
            .SqlPassword = "YKdDCapyzBdJFfe2",
            .SqlPort = "3306",
            .Database = "biggettrade"}


    Public Shared Sub Initialize()

        Form1.Text = "公共市场行情获取（BigGet） V1.0"

        '初始化sql信息
        BigGetStrategy.PublicConf.Sql = Sql



    End Sub



End Class
