Imports MyCOM.ConnectMysql
Imports Org.BouncyCastle.Crypto.Engines
Imports System.ComponentModel
Imports System.Threading.Thread
Imports System.Reflection
Imports Google.Protobuf.WellKnownTypes
Imports MySql.Data.MySqlClient
Imports System.Text

Namespace PublicData

    ''' <summary>
    ''' 获取公共行情数据的类
    ''' 1、现货行情
    ''' </summary>
    Public Class GetSpotTickers

#Region "内部变量"

        Private ReadOnly DbConfig As dbConfig   '数据库配置信息
        Private ReadOnly MysqlConnect As ConnectMysql   '数据库对象
        Private MyAdp As MySqlDataAdapter
        Private ReadOnly UserCall As Api.User.UserCall
        Private ReadOnly UserKey As Api.UserKeyInfo = PublicConf.PublicUserKey
        Private ReadOnly bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}
        Private ReadOnly TableName As String = "spot_tickers"    '表名

#End Region

#Region "公有属性"

        ''' <summary>
        ''' 全部现货tickers
        ''' </summary>
        ''' <returns></returns>
        Public Property SpotTickers As New DataSet

#End Region

#Region "构造函数"

        Sub New()
            DbConfig = New dbConfig(PublicConf.Sql.SqlServer, PublicConf.Sql.SqlPort, PublicConf.Sql.SqlUser, PublicConf.Sql.SqlPassword, PublicConf.Sql.Database)
            MysqlConnect = New ConnectMysql(DbConfig)
            UserCall = New Api.User.UserCall(UserKey)  'biggetapi对象
        End Sub

#End Region

#Region "公有方法"

        ''' <summary>
        ''' 启动线程
        ''' </summary>
        Public Sub Run()
            If bgw.IsBusy = False Then
                '第一次读取数据库
                'ReadTable()
                GetDataForMySql(DbConfig.ConnectStr, TableName)    '初始化公共数据dataset
                AddHandler bgw.DoWork, AddressOf DoWorkGetPublicData
                AddHandler bgw.ProgressChanged, AddressOf ProgressChangedDoWorkGetPublicData
                AddHandler bgw.RunWorkerCompleted, AddressOf WorkerCompleted
                bgw.RunWorkerAsync()    '启动策略
            End If
        End Sub

        ''' <summary>
        ''' 结束线程
        ''' </summary>
        Public Sub StopRun()

        End Sub

        ''' <summary>
        ''' 线程状态
        ''' </summary>
        ''' <returns></returns>
        Public Function WorkerIsBusy() As Boolean
            Return bgw.IsBusy
        End Function

#End Region

#Region "私有方法"

        Private Sub DoWorkGetPublicData(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)
            Dim Worker As BackgroundWorker = CType(sender, BackgroundWorker)
            Dim Method As MethodInfo = MethodBase.GetCurrentMethod  '方法自身的信息

            Do
                '控制线程退出循环
                If Worker.CancellationPending Then Exit Do

                If GetSpotTickers() Then

                End If
                Sleep(200)
            Loop

        End Sub

        Private Sub ProgressChangedDoWorkGetPublicData(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs)

        End Sub

        Private Sub WorkerCompleted(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs)

        End Sub

        ''' <summary>
        ''' 获取bigget，tickers数据
        ''' </summary>
        ''' <returns></returns>
        Friend Function GetSpotTickers() As Boolean
            '获取现货行情
            Dim ret As Api.Api.Request.Spot.Reply.MarketTickers = UserCall.SpotGetMarketTickers
            If ret.code = "00000" Then
                If ret.data.Count > 0 Then
                    For Each d As Api.Api.Request.Spot.Reply.MarketTickers.Datum In ret.data

                        'ret.data保存到dataset
                        Dim dr As DataRow = PublicConf.PublicData.Tables(TableName).Rows.Find(d.symbol)
                        If IsNothing(dr) = False Then
                            '存在symbol
                            dr.ItemArray = d.ToList.ToArray
                        Else
                            Dim newdr As DataRow = PublicConf.PublicData.Tables(TableName).NewRow
                            newdr.ItemArray = d.ToList.ToArray
                            PublicConf.PublicData.Tables(TableName).Rows.Add(newdr)
                        End If
                    Next
                    Return True
                End If
            End If

            Return False
        End Function

        ''' <summary>
        ''' 获取spot数据
        ''' </summary>
        ''' <param name="p_cmdSql"></param>
        ''' <param name="p_tableName"></param>
        Private Sub GetDataForMySql(ByVal p_cmdSql As String, ByVal p_tableName As String)

            Using connection As New MySqlConnection(p_cmdSql)
                Using Command As MySqlCommand = connection.CreateCommand
                    Command.CommandText = "select * from " & p_tableName
                    Using adapter As New MySqlDataAdapter
                        adapter.SelectCommand = Command
                        adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey    '加上默认主键
                        SyncLock PublicConf.PublicData
                            adapter.Fill(PublicConf.PublicData, p_tableName)
                        End SyncLock
                    End Using
                End Using
            End Using

        End Sub

#End Region

#Region "过时方法"

        ''' <summary>
        ''' 更新数据库
        ''' </summary>
        ''' <returns></returns>
        Friend Function Update() As Boolean
            Try
                MyAdp.Update(PublicConf.PublicData, TableName)
                Return True
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' 读取数据库
        ''' </summary>
        ''' <returns></returns>
        Friend Function ReadTable() As Boolean
            Dim conn As New MySqlConnection(DbConfig.ConnectStr)
            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print("Error:{0}.{1}        {2}", MyBase.ToString, "ReadTable", "打开数据库失败")
                Return False
            End Try

            Dim cmdStr As String = "select * from " & TableName
            MyAdp = New MySqlDataAdapter(cmdStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(MyAdp)
            MyAdp.MissingSchemaAction = MissingSchemaAction.AddWithKey      '加上默认主键

            SyncLock PublicConf.PublicData
                '把数据库中的数据读取到publicconf.publicdata中
                MyAdp.Fill(PublicConf.PublicData, TableName)
            End SyncLock

            Return True

        End Function

#End Region

    End Class

End Namespace