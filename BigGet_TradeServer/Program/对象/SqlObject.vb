''' <summary>
''' 数据库链接设置
''' </summary>
Public Class SqlObject

#Region "属性"

    Public Property SqlServer As String
    Public Property SqlPort As String
    Public Property SqlUser As String
    Public Property SqlPassword As String
    Public Property Database As String

    Public Property _AllowUserVariables As Boolean = True

    Public Property _SslMode As String = "None"

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

#End Region

End Class