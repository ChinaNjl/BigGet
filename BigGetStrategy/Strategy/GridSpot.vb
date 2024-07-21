Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Threading.Thread

Namespace Strategy

    Public Class GridSpot

#Region "变量和对象"

        Private MyAdp As MySqlDataAdapter
        Private Ds As New DataSet   '策略信息表
        Private dr As DataRow
        Private Const Tablename As String = "strategytable"

        Private Bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}
        Private UserInfo As Api.UserKeyInfo    '调用api的密钥
        Private UserCall As Api.User.UserCall

#End Region

#Region "属性"

        ''' <summary>
        '''托管策略运行状态
        ''' </summary>
        ''' <returns></returns>
        Public Property State As Boolean = False

        ''' <summary>
        ''' 策略编号（在数据库中的标识）
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Id As String
            Get
                Return Ds.Tables(Tablename).Rows(0).Item("id")
            End Get
        End Property

#End Region

#Region "对外方法"

        ''' <summary>
        ''' 初始化对象
        ''' </summary>
        ''' <param name="p_dr"></param>
        Sub New(ByVal p_dr As DataRow)
            ReadStrategy(p_dr.Item("id"))
        End Sub

        ''' <summary>
        ''' 停止线程
        ''' </summary>
        Public Sub StopRun()
            Bgw.CancelAsync()
            Do
                If State = False Then
                    Exit Do
                Else
                    Sleep(1000)
                End If
            Loop
        End Sub

        Public Sub Run()

            '保存api信息
            UserInfo = New Api.UserKeyInfo With {
                .ApiKey = dr.Item("apikey"),
                .Secretkey = dr.Item("secretkey"),
                .Passphrase = dr.Item("passphrase"),
                .Host = dr.Item("host")}

            UserCall = New Api.User.UserCall(UserInfo)

        End Sub

#End Region

#Region "内部方法"

        ''' <summary>
        ''' 策略主线程
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub DoWorkRunStrategy(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

        End Sub

        ''' <summary>
        ''' 保存策略最新状态到数据库
        ''' </summary>
        Private Sub Update()
            Try
                MyAdp.Update(Ds, Tablename)
            Catch ex As Exception
                Debug.Print("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
            End Try

        End Sub

        ''' <summary>
        ''' 从数据库中读取策略信息
        ''' </summary>
        ''' <param name="p_id"></param>
        ''' <returns></returns>
        Private Function ReadStrategy(ByVal p_id As String) As Boolean

            '读取策略
            Using conn As New MySqlConnection(PublicConf.Sql.ConnectStr)
                conn.Open()
                Dim commandStr As String = "select * from strategytable where id=" & p_id
                MyAdp = New MySqlDataAdapter(commandStr, conn)

                SyncLock Ds
                    Ds = New DataSet
                    MyAdp.Fill(Ds, Tablename)
                End SyncLock
                dr = Ds.Tables(Tablename).Rows(0)
                Return True
            End Using

            Return False
        End Function

#End Region

    End Class

End Namespace