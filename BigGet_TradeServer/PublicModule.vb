Imports BigGetStrategy

Module PublicModule


    Public PublicGetContracts As New BigGetStrategy.PublicData.GetContracts
    Public PublicGetTickets As New BigGetStrategy.PublicData.GetTickets
    Public Property PublicGetUserStrategy As New BigGetStrategy.PublicData.GetUserStrategy


    Public Sub StrategyInitialize()

        With PublicGetTickets.userKey
            .ApiKey = ConfObject.KeyPublic.ApiKey
            .Secretkey = ConfObject.KeyPublic.Secretkey
            .Passphrase = ConfObject.KeyPublic.Passphrase
            .Host = ConfObject.KeyPublic.Host
        End With


        'With PublicGetTickets.sql
        '    .SqlPassword = ConfObject.Sql.SqlPassword
        '    .SqlPort = ConfObject.Sql.SqlPort
        '    .SqlServer = ConfObject.Sql.SqlServer
        '    .SqlUser = ConfObject.Sql.SqlUser
        '    .Database = ConfObject.Sql.Database
        'End With


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

        With BigGetStrategy.PublicConf.Sql
            .SqlPassword = ConfObject.Sql.SqlPassword
            .SqlPort = ConfObject.Sql.SqlPort
            .SqlServer = ConfObject.Sql.SqlServer
            .SqlUser = ConfObject.Sql.SqlUser
            .Database = ConfObject.Sql.Database
        End With


    End Sub
End Module
