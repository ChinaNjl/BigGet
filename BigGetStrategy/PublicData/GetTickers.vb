Imports System.Threading.Thread
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Runtime.InteropServices.WindowsRuntime
Imports System.Runtime.InteropServices
Imports Api.User
Imports MyCOM.ConnectMysql

Namespace PublicData

    Public Class GetTickers

#Region "私有变量"

        Private ReadOnly DbConfig As dbConfig   '数据库配置信息
        Private ReadOnly MysqlConnect As ConnectMysql   '数据库对象
        Private MyAdp As MySqlDataAdapter
        Private ReadOnly UserCall As Api.User.UserCall
        Private ReadOnly UserKey As Api.UserKeyInfo = PublicConf.PublicUserKey
        Private ReadOnly bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}
        Private ReadOnly TableName As String = "tickertable"    '表名

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

                ReadTable()
                AddHandler bgw.DoWork, AddressOf DoWorkGetTickers
                AddHandler bgw.ProgressChanged, AddressOf ProgressChanged_GetTickers
                'AddHandler bgw.RunWorkerCompleted, AddressOf WorkerCompleted
                bgw.RunWorkerAsync()
            End If
        End Sub

        ''' <summary>
        ''' 停止后台线程
        ''' </summary>
        Public Sub StopRun()
            bgw.CancelAsync()
        End Sub

        Public Function WorkerIsBusy() As Boolean
            Return bgw.IsBusy
        End Function

#End Region

#Region "私有方法"

        ''' <summary>
        ''' 读取所有币种的行情数据
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub DoWorkGetTickers(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            Dim Worker As BackgroundWorker = CType(sender, BackgroundWorker)

            Dim count As Integer = 1

            Do
                '线程控制线程退出循环
                If Worker.CancellationPending Then Exit Do

                If GetTickers() Then

                End If

                '控制刷新频率
                If count Mod 20 = 0 Then
                    Worker.ReportProgress(1， 2)
                    count = 1
                    'Update()
                End If

                count = count + 1
            Loop

        End Sub

        Friend Function GetTickers() As Boolean
            Dim ret As Api.Api.Request.Contract.ReplyType.MarkTickers = UserCall.ContractGetMarkTickers("umcbl")
            If ret.code = "00000" Then
                If ret.data.Count > 0 Then
                    For Each d As Api.Api.Request.Contract.ReplyType.MarkTickers.DataType In ret.data
                        Dim dr1 As DataRow = PublicConf.PublicData.Tables(TableName).Rows.Find(d.symbol)  '通过主键查找
                        If IsNothing(dr1) = False Then
                            dr1.ItemArray = d.toList.ToArray
                        Else
                            Dim newdr As DataRow = PublicConf.PublicData.Tables(TableName).NewRow
                            newdr.ItemArray = d.toList.ToArray
                            PublicConf.PublicData.Tables(TableName).Rows.Add(newdr)
                        End If
                    Next

                    Return True
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Tickers.Tables(TableName).Rows的中数据更新到PublicConf.Tickers.Tables(TableName).Rows
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ProgressChanged_GetTickers(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs)

            Dim tmptickets As Integer
            tmptickets = e.UserState

            For Each d1 As DataRow In PublicConf.PublicData.Tables(TableName).Rows

                Dim dr As DataRow = PublicConf.Tickers.Tables(TableName).Rows.Find(d1.Item(0))

                If IsNothing(dr) = False Then
                    dr.ItemArray = d1.ItemArray
                Else
                    Dim ndr As DataRow = PublicConf.Tickers.Tables(TableName).NewRow
                    ndr.ItemArray = d1.ItemArray
                    PublicConf.Tickers.Tables(TableName).Rows.Add(ndr)
                End If

            Next

        End Sub

        Friend Function Update() As Boolean
            Using conn As New MySqlConnection(DbConfig.ConnectStr)
                Try
                    conn.Open()

                    ' 创建一个MySqlDataAdapter
                    Using adapter As New MySqlDataAdapter("SELECT * FROM " & TableName, conn)

                        ' 创建一个MySqlCommandBuilder来自动生成SQL语句
                        Dim builder As New MySqlCommandBuilder(adapter)

                        ' 更新数据库
                        adapter.Update(PublicConf.Tickers.Tables(TableName))
                        PublicConf.Tickers.Tables(TableName).AcceptChanges()
                    End Using

                    Return True
                Catch ex As Exception
                    Console.WriteLine("An error occurred: " & ex.Message)
                    Return False
                End Try
            End Using
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
                MyAdp.Fill(PublicConf.PublicData, TableName)
            End SyncLock

            MyAdp.Fill(PublicConf.Tickers, TableName)

            Return True

        End Function

#End Region

    End Class

End Namespace