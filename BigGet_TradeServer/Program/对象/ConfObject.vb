Imports Microsoft.SqlServer
Imports Microsoft.VisualBasic.ApplicationServices

Public Class ConfObject

    Public Class SqlObject

        Public Property SqlServer As String
        Public Property SqlPort As String
        Public Property SqlUser As String
        Public Property SqlPassword As String
        Public Property Database As String

        ''' <summary>
        ''' 默认true
        ''' </summary>
        ''' <returns></returns>
        Public Property AllowUserVariables As Boolean
            Get
                Return _AllowUserVariables
            End Get
            Set(value As Boolean)
                _AllowUserVariables = value
            End Set
        End Property
        Dim _AllowUserVariables As Boolean = True

        ''' <summary>
        ''' 默认None
        ''' </summary>
        ''' <returns></returns>
        Public Property SslMode As String
            Get
                Return _SslMode
            End Get
            Set(value As String)
                _SslMode = value
            End Set
        End Property
        Dim _SslMode As String = "None"


        Public ReadOnly Property ConnectStr As String
            Get

                Dim s As String = "server={0};port={1};user={2};password={3};database={4};Allow User Variables={5};SslMode={6}"

                s = s.Replace("{0}", SqlServer)
                s = s.Replace("{1}", SqlPort)
                s = s.Replace("{2}", SqlUser)
                s = s.Replace("{3}", SqlPassword)
                s = s.Replace("{4}", Database)
                s = s.Replace("{5}", AllowUserVariables)
                s = s.Replace("{6}", SslMode)

                Return s

            End Get
        End Property


    End Class






    Public Shared Sql As New SqlObject With {
        .SqlServer = "156.245.25.149",
        .SqlUser = "BigGetTrade",
        .SqlPassword = "YKdDCapyzBdJFfe2",
        .SqlPort = "3306",
        .Database = "biggettrade"
    }

    Public Shared KeyPublic As New Api.UserInfo With {
        .ApiKey = "bg_ac73bd7ce9b4f500bdc99adba3fcfdee",
        .Secretkey = "9fef2fcce8a9dd0a418ee0bcbae161afe1d114eb27f040e57ab613163a4d9828",
        .Passphrase = "njl12345678",
        .Host = "api.bitget.com"
    }





End Class
