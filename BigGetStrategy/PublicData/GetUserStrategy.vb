


Imports System.Threading.Thread
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports Org.BouncyCastle.Math

Namespace PublicData


    ''' <summary>
    ''' 获取用户的策略，并控制启动/关闭
    ''' </summary>
    Public Class GetUserStrategy




        Public Property ds As DataSet
        Public Property sql As UserType.SqlInfo = PublicConf.Sql
        Public ReadOnly Property TableName As String = "strategytable"
        Public ReadOnly Property ColName As String = "id,strategytypeid,state"
        Public Property bgwList As New List(Of Object)
        Public ReadOnly Property BgwCount As Integer
            Get
                Return bgwList.Count
            End Get
        End Property

        ''' <summary>
        ''' 检查后台任务运行状态
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property WorkerIsBusy() As Boolean
            Get
                Return bgw.IsBusy
            End Get
        End Property


        Private myadp As MySqlDataAdapter
        Private bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}




        ''' <summary>
        ''' 停止后台线程
        ''' </summary>
        Public Sub StopRun()
            bgw.CancelAsync()
        End Sub

        ''' <summary>
        ''' 启动线程：读取用户策略信息表
        ''' </summary>
        Public Sub Run()
            If bgw.IsBusy = False Then
                AddHandler bgw.DoWork, AddressOf DoWorkUserStrategy
                'AddHandler bgw.ProgressChanged, AddressOf ProgressChanged_GetTickeets
                bgw.RunWorkerAsync()
            End If
        End Sub



        ''' <summary>
        ''' 后台过程，用于启动停止策略
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub DoWorkUserStrategy(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            Dim dt As DataTable
            Do

                '把需要运行的策略保存到dataset对象中，然后再读取列表来分别启动
                If OpenTableFromDatabase(BuildSql(ColName, TableName， "state=101 or state=102")) Then
                    dt = ds.Tables(TableName)
                    For Each dr As DataRow In dt.Rows

                        If dr.Item("state") = 101 Then
                            StartStrategy(dr, bgwList)
                        End If

                        If dr.Item("state") = 102 Then
                            StopStrategy(dr, bgwList)
                        End If
                    Next

                End If
                Sleep(3000)
                ClearStrategyList()
            Loop

        End Sub


#Region "*****************************自定义方法*******************************"

        ''' <summary>
        ''' 启动策略
        ''' </summary>
        ''' <param name="p_dr"></param>
        ''' <param name="p_bgwList"></param>
        ''' <returns></returns>
        Private Function StartStrategy(ByVal p_dr As DataRow, ByRef p_bgwList As List(Of Object)) As Boolean

            '当策略不存在运行列表中，启动策略
            _idFindStrategy1 = p_dr.Item("id")
            If IsNothing(p_bgwList.Find(AddressOf FindStrategy1)) = True Then


                Dim sobject As Object = Nothing
                Select Case p_dr.Item("strategytypeid")
                    Case 1002
                        sobject = New Strategy.GridContractLong(p_dr)
                    Case 1001
                        sobject = New Strategy.GridContract(p_dr)
                    Case Else
                End Select

                sobject.Run()
                p_bgwList.Add(sobject)

            End If

            Return True

        End Function



        ''' <summary>
        ''' 停止策略
        ''' </summary>
        ''' <param name="p_dr"></param>
        ''' <param name="p_bgwList"></param>
        ''' <returns></returns>
        Private Function StopStrategy(ByVal p_dr As DataRow, ByRef p_bgwList As List(Of Object)) As Boolean

            _idFindStrategy1 = p_dr.Item("id")
            If IsNothing(p_bgwList.Find(AddressOf FindStrategy1)) = False Then

                p_bgwList.Find(AddressOf FindStrategy1).StopRun()

            End If

            Return True

        End Function


        ''' <summary>
        ''' 从列表中清除暂停的策略
        ''' </summary>
        ''' <returns></returns>
        Private Function ClearStrategyList() As Integer

            Dim ret As Integer = bgwList.RemoveAll(AddressOf FindStrategy)
            Return ret
        End Function


        ''' <summary>
        ''' 更新变动的数据
        ''' </summary>
        ''' <returns></returns>
        Public Function Update() As Boolean


            Try
                myadp.Update(ds, TableName)
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return False
            End Try


            Return True
        End Function



        ''' <summary>
        ''' 构造sql
        ''' </summary>
        ''' <param name="_colName"></param>
        ''' <param name="_tableName"></param>
        ''' <param name="_whereStr"></param>
        ''' <returns></returns>
        Public Function BuildSql(ByVal _colName As String, ByVal _tableName As String, ByVal _whereStr As String) As String

            Dim sql As String = "select {colName} from {tableName} where {where}"

            sql = sql.Replace("{colName}", _colName)
            sql = sql.Replace("{tableName}", _tableName)
            sql = sql.Replace("{where}", _whereStr)

            Return sql
        End Function

        ''' <summary>
        ''' 通过dataset控件读取数据库strategytable 表
        ''' </summary>
        Public Function OpenTableFromDatabase() As Boolean

            Return OpenTableFromDatabase(sql.ConnectStr, BuildSql(ColName, TableName， "state=101 or state=102"), TableName)

        End Function

        Public Function OpenTableFromDatabase(ByVal _comSqlStr As String) As Boolean
            Return OpenTableFromDatabase(sql.ConnectStr, _comSqlStr, TableName)
        End Function

        Public Function OpenTableFromDatabase(ByVal _ConnectStr As String, ByVal _comSqlStr As String, ByVal _tableName As String) As Boolean

            Dim conn As New MySqlConnection(_ConnectStr)

            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print("10001:{0}", ex.Message)
                Return False
            End Try

            Dim commandStr As String = _comSqlStr.Replace("{0}", _tableName)
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)

            ds = New DataSet
            Try
                SyncLock ds
                    myadp.Fill(ds, _tableName)   '将读取到的内容存入ds中
                End SyncLock
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return False
            End Try

            Return True

        End Function




#End Region

#Region "委托函数"

        ''' <summary>
        ''' 委托函数，搜索的条件
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        Private Function FindStrategy(obj As Object) As Boolean
            If obj.state = False Then
                Return True
            Else
                Return False
            End If
        End Function


        ''' <summary>
        ''' 委托函数，搜索的条件
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        Private Function FindStrategy1(obj As Object) As Boolean
            If obj.id = _idFindStrategy1 Then
                Return True
            Else
                Return False
            End If
        End Function
        Dim _idFindStrategy1 As String

#End Region





    End Class

End Namespace