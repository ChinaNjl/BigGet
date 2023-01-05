


Imports System.Threading.Thread
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports Org.BouncyCastle.Math

Namespace PublicData

    Public Class GetUserStrategy

        Public Property ds As DataSet
        Public Property sql As New UserType.SqlInfo
        Public Property userKey As New Api.UserInfo
        Public ReadOnly Property TableName As String
            Get
                Return "strategytable"
            End Get
        End Property
        Public Property bgwList As New List(Of Strategy.GridContract)

        ''' <summary>
        ''' sql命令模板
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property expSql As String
            Get
                Dim str As String = "select id,strategytypeid from {0} where state=101"
                Return str

            End Get
        End Property



        Private myadp As MySqlDataAdapter
        Private bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}




        Public Function BuildSql(ByVal whereStr As String) As String
            Return BuildSql(TableName, whereStr)
        End Function

        Public Function BuildSql(ByVal tableName As String, ByVal whereStr As String) As String
            Return BuildSql("id,strategytypeid", tableName, whereStr)
        End Function

        Public Function BuildSql(ByVal colName As String, ByVal tableName As String, ByVal whereStr As String) As String

            Dim sql As String = "select {colName} from {tableName} where {where}"

            sql = sql.Replace("{colName}", colName)
            sql = sql.Replace("{tableName}", tableName)
            sql = sql.Replace("{whereStr}", whereStr)

            Return sql
        End Function

        ''' <summary>
        ''' 通过dataset控件读取数据库contracttable 表
        ''' </summary>
        Public Function OpenTableFromDatabase() As Boolean



            Return OpenTableFromDatabase(sql.ConnectStr, expSql, TableName)

            Dim conn As New MySqlConnection(sql.ConnectStr)

            Try
                conn.Open()
            Catch ex As Exception
                Return False
            End Try


            Dim commandStr As String = expSql.Replace("{0}", TableName)
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)

            ds = New DataSet

            Try
                SyncLock ds
                    myadp.Fill(ds, TableName)   '将读取到的内容存入ds中
                End SyncLock
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return False
            End Try

            Return True



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
        ''' 停止后台线程
        ''' </summary>
        Public Sub StopRun()
            bgw.CancelAsync()
        End Sub

        ''' <summary>
        ''' 检查后台任务运行状态
        ''' </summary>
        ''' <returns></returns>
        Public Function WorkerIsBusy() As Boolean
            Return bgw.IsBusy
        End Function

        Public Sub Run()
            If bgw.IsBusy = False Then
                AddHandler bgw.DoWork, AddressOf DoWorkUserStrategy
                'AddHandler bgw.ProgressChanged, AddressOf ProgressChanged_GetTickeets
                bgw.RunWorkerAsync()
            End If
        End Sub


        Public Sub DoWorkUserStrategy(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            Do
                If OpenTableFromDatabase() = True Then

                    Dim dt As DataTable = ds.Tables(TableName)

                    If bgwList.Count = 0 Then

                        For Each dr As DataRow In dt.Rows
                            Dim sobject As New Strategy.GridContract(dr)
                            sobject.Run()
                            bgwList.Add(sobject)
                        Next

                    Else

                        '先启动，后关闭
                        If OpenTableFromDatabase(BuildSql("state=101 or state=102")) Then
                            dt = ds.Tables(TableName)



                        End If

                    End If



                End If


                Sleep(3000)
            Loop



        End Sub


    End Class

End Namespace