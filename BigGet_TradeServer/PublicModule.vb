Imports BigGetStrategy

Module PublicModule


    Public PublicGetContracts As New PublicData.GetContracts
    Public PublicGetTickets As New PublicData.GetTickets
    Public PublicGetUserStrategy As New PublicData.GetUserStrategy


    Public Sub StrategyInitialize()

        With PublicGetTickets.userKey
            .ApiKey = ConfObject.KeyPublic.ApiKey
            .Secretkey = ConfObject.KeyPublic.Secretkey
            .Passphrase = ConfObject.KeyPublic.Passphrase
            .Host = ConfObject.KeyPublic.Host
        End With


        With PublicGetTickets.sql
            .SqlPassword = ConfObject.Sql.SqlPassword
            .SqlPort = ConfObject.Sql.SqlPort
            .SqlServer = ConfObject.Sql.SqlServer
            .SqlUser = ConfObject.Sql.SqlUser
            .Database = ConfObject.Sql.Database
        End With


        With PublicGetContracts.userKey
            .ApiKey = ConfObject.KeyPublic.ApiKey
            .Secretkey = ConfObject.KeyPublic.Secretkey
            .Passphrase = ConfObject.KeyPublic.Passphrase
            .Host = ConfObject.KeyPublic.Host
        End With


        With PublicGetContracts.sql
            .SqlPassword = ConfObject.Sql.SqlPassword
            .SqlPort = ConfObject.Sql.SqlPort
            .SqlServer = ConfObject.Sql.SqlServer
            .SqlUser = ConfObject.Sql.SqlUser
            .Database = ConfObject.Sql.Database
        End With

        With PublicGetUserStrategy.userKey
            .ApiKey = ConfObject.KeyPublic.ApiKey
            .Secretkey = ConfObject.KeyPublic.Secretkey
            .Passphrase = ConfObject.KeyPublic.Passphrase
            .Host = ConfObject.KeyPublic.Host
        End With

        With PublicGetUserStrategy.sql
            .SqlPassword = ConfObject.Sql.SqlPassword
            .SqlPort = ConfObject.Sql.SqlPort
            .SqlServer = ConfObject.Sql.SqlServer
            .SqlUser = ConfObject.Sql.SqlUser
            .Database = ConfObject.Sql.Database
        End With

        With PublicConf.sql
            .SqlPassword = ConfObject.Sql.SqlPassword
            .SqlPort = ConfObject.Sql.SqlPort
            .SqlServer = ConfObject.Sql.SqlServer
            .SqlUser = ConfObject.Sql.SqlUser
            .Database = ConfObject.Sql.Database
        End With


    End Sub
End Module
