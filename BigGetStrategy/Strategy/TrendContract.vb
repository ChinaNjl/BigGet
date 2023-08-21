
Imports System.Threading.Thread
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports Api
Imports Org.BouncyCastle.Math.EC

Namespace Strategy

    ''' <summary>
    ''' 趋势合约交易
    ''' </summary>
    Public Class TrendContract


        Structure UserSetInfoObject
            Public symbol As String      '产品id
            Private granularity As String 'k线类别
        End Structure

        Private UsrSetInfo As New UserSetInfoObject



        ''' <summary>
        ''' 策略编号（在数据库中的标识）
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Id As String
            Get
                Return dsUserInfo.Tables(TableName).Rows(0).Item("id")
            End Get
        End Property
        Private Property sql As UserType.SqlInfo = PublicConf.Sql   'sql服务器信息
        Public bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}     '策略线程
        Public Property State As Boolean = False                '策略运行状态
        Private TableName As String = "strategytable"
        Private Property UserInfo As UserInfo
        Private UserCall As UserCall
        Private dsUserInfo As New DataSet
        Private Property myadp As MySqlDataAdapter


        Sub New(ByVal _dr As DataRow)
            Call OpenTableFromDatabase(_dr.Item("id"))
        End Sub

        ''' <summary>
        ''' 读取策略配置信息,保存到dataset
        ''' </summary>
        ''' <param name="id"></param>
        Private Sub OpenTableFromDatabase(id)

            Dim conn As New MySqlConnection(sql.ConnectStr)

            Try
                conn.Open()

            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try

            Dim commandStr As String = "select * from strategytable where id=" & id
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)


            Try
                SyncLock dsUserInfo
                    dsUserInfo = New DataSet
                    myadp.Fill(dsUserInfo, TableName)   '将读取到的内容存入ds中
                End SyncLock
            Catch ex As Exception
                Debug.Print("12315645646")
            End Try

        End Sub

        ''' <summary>
        ''' 启动策略
        ''' </summary>
        Public Sub Run()

            '创建api对象
            UserInfo = New UserInfo With {
                .ApiKey = dsUserInfo.Tables(TableName).Rows(0).Item("apikey"),
                .Secretkey = dsUserInfo.Tables(TableName).Rows(0).Item("secretkey"),
                .Passphrase = dsUserInfo.Tables(TableName).Rows(0).Item("passphrase"),
                .Host = dsUserInfo.Tables(TableName).Rows(0).Item("host")
            }
            UserCall = New UserCall(UserInfo)

            '启动策略
            If bgw.IsBusy = False Then
                AddHandler bgw.DoWork, AddressOf DoWorkRunStrategy
                'AddHandler bgw.RunWorkerCompleted, AddressOf RunComplete
                bgw.RunWorkerAsync()
            End If
        End Sub

        ''' <summary>
        ''' 停止策略
        ''' </summary>
        Public Sub StopRun()
            bgw.CancelAsync()
            Do
                If State = False Then
                    Exit Do
                Else
                    Sleep(1000)
                End If
            Loop
        End Sub


        Public Sub DoWorkRunStrategy(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            State = True    '策略启动之后将状态设置成true

            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)


        End Sub


    End Class


End Namespace








