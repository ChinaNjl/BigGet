


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
        ''' 构建sql命令
        ''' </summary>
        ''' <param name="whereStr"></param>
        ''' <returns></returns>
        Public Function BuildSql(ByVal whereStr As String) As String
            Return BuildSql(TableName, whereStr)
        End Function

        Public Function BuildSql(ByVal _tableName As String, ByVal _whereStr As String) As String
            Return BuildSql(ColName, _tableName, _whereStr)
        End Function

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

            Return OpenTableFromDatabase(sql.ConnectStr, BuildSql("state=101 or state=102"), TableName)

        End Function

        Public Function OpenTableFromDatabase(ByVal comSqlStr As String) As Boolean
            Return OpenTableFromDatabase(sql.ConnectStr, comSqlStr, TableName)
        End Function

        Public Function OpenTableFromDatabase(ByVal comSqlStr As String, ByVal tableName As String) As Boolean
            Return OpenTableFromDatabase(sql.ConnectStr, comSqlStr, tableName)
        End Function

        Public Function OpenTableFromDatabase(ByVal ConnectStr As String, ByVal comSqlStr As String, ByVal tableName As String) As Boolean

            Dim conn As New MySqlConnection(ConnectStr)

            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print("10001:{0}", ex.Message)
                Return False
            End Try

            Dim commandStr As String = comSqlStr.Replace("{0}", tableName)
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)

            ds = New DataSet
            Try
                SyncLock ds
                    myadp.Fill(ds, tableName)   '将读取到的内容存入ds中
                End SyncLock
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return False
            End Try

            Return True

        End Function



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


        Public Sub DoWorkUserStrategy(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            Do

                Dim dt As DataTable = ds.Tables(TableName)

                '把需要运行的策略保存到dataset对象中，然后再读取列表来分别启动
                If OpenTableFromDatabase(BuildSql("state=101 or state=102")) Then
                    dt = ds.Tables(TableName)
                    For Each dr As DataRow In dt.Rows

                        '启动策略
                        If StartStrategy(dr, bgwList) = False Then
                            '启动策略失败，则停止策略
                            StopStrategy(dr, bgwList)
                        End If

                    Next
                End If

                Sleep(3000)
            Loop

        End Sub


        ''' <summary>
        ''' 启动策略
        ''' </summary>
        ''' <param name="_dr"></param>
        ''' <param name="_bgwList"></param>
        ''' <returns></returns>
        Private Function StartStrategy(ByVal _dr As DataRow, ByRef _bgwList As List(Of Object)) As Boolean

            If _dr.Item("state") = 101 Then

                '....新增策略
                If _dr.Item("strategytypeid") = 1002 Then
                    Dim FindRunStrategy As Boolean = False
                    For Each s As Object In _bgwList

                        If s.Id = _dr.Item("id") Then
                            FindRunStrategy = True  '存在于列表中
                            Exit For
                        End If
                    Next

                    '不存在于列表中的策略，启动策略
                    If FindRunStrategy = False Then
                        '当策略不存在运行列表中，则启动新策略
                        Dim sobject As New Strategy.TrendContract(_dr)
                        sobject.Run()
                        _bgwList.Add(sobject)
                    End If



                End If





                '策略未运行
                If _dr.Item("strategytypeid") = 1001 Then

                    Dim FindRunStrategy As Boolean = False
                    For Each s As Strategy.GridContract In _bgwList
                        If s.Id = _dr.Item("id") Then
                            FindRunStrategy = True  '存在于列表中
                            Exit For
                        End If
                    Next

                    '不存在于列表中的策略，启动策略
                    If FindRunStrategy = False Then
                        '当策略不存在运行列表中，则启动新策略
                        Dim sobject As New Strategy.GridContract(_dr)
                        sobject.Run()
                        _bgwList.Add(sobject)
                    End If

                End If




                Return True
            Else
                '策略已经运行
                Return False
            End If


        End Function

        ''' <summary>
        ''' 停止策略
        ''' </summary>
        ''' <param name="_dr"></param>
        ''' <param name="_bgwList"></param>
        ''' <returns></returns>
        Private Function StopStrategy(ByVal _dr As DataRow, ByRef _bgwList As List(Of Object)) As Boolean

            If _dr.Item("state") = 102 Then

                Dim FindRunStrategy As Boolean = False
                Dim tmpRunStrategy As Strategy.GridContract = Nothing

                For Each s As Object In _bgwList

                    If s.Id = _dr.Item("id") Then
                        '暂停指定策略正在运行中
                        FindRunStrategy = True
                        s.StopRun()
                        tmpRunStrategy = s
                        Exit For
                    End If
                Next

                If FindRunStrategy = True Then
                    _bgwList.Remove(tmpRunStrategy)

                End If

                Return True
            Else
                Return False
            End If
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


    End Class

End Namespace