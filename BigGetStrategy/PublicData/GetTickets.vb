
Imports System.Threading.Thread
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Runtime.InteropServices.WindowsRuntime

Namespace PublicData
    Public Class GetTickets

        'Public Property ds As DataSet
        Private Property sql As UserType.SqlInfo = PublicConf.Sql
        Public Property userKey As New Api.UserInfo
        Public Property RefDelay As Integer
            Get
                Return _RefDelay
            End Get
            Set(value As Integer)
                _RefDelay = value
            End Set
        End Property
        Dim _RefDelay As Integer = 3000

        Public ReadOnly Property TableName As String
            Get
                Return "tickertable"
            End Get
        End Property




        Private Property myadp As MySqlDataAdapter
        Private Property bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}


        ''' <summary>
        ''' 通过dataset控件读取数据库contracttable 表
        ''' </summary>
        Public Sub OpenTableFromDatabase()

            Dim conn As New MySqlConnection(sql.ConnectStr)

            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try


            Dim commandStr As String = "select * from " & TableName
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)

            Try
                SyncLock PublicConf.DtTickets
                    PublicConf.DtTickets = New DataSet
                    myadp.Fill(PublicConf.DtTickets, TableName)   '将读取到的内容存入ds中
                End SyncLock
            Catch ex As Exception
                Debug.Print("abavj")
            End Try

        End Sub


        ''' <summary>
        ''' 更新变动的数据
        ''' </summary>
        ''' <returns></returns>
        Public Function Update() As Boolean


            Try
                myadp.Update(PublicConf.DtTickets, TableName)
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return False
            End Try


            Return True
        End Function

        ''' <summary>
        ''' 启动线程
        ''' </summary>
        Public Sub Run()

            If bgw.IsBusy = False Then
                AddHandler bgw.DoWork, AddressOf DoWorkGetTickets
                AddHandler bgw.ProgressChanged, AddressOf ProgressChanged_GetTickeets
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


        ''' <summary>
        ''' 从服务器bitget获取行情数据
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub DoWorkGetTickets(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)


            Dim Worker As BackgroundWorker = CType(sender, BackgroundWorker)

            Dim userCall As New Api.UserCall(userKey)

            Do
                If Worker.CancellationPending Then Exit Do


                Dim ret As Api.UserType.ReplyType.MarkTickers = userCall.GetMarkTickers("umcbl")
                Dim tmptickets As New List(Of List(Of String))

                If ret.code = 0 Then

                    For Each d In ret.data

                        tmptickets.Add(d.toList)

                    Next

                    Worker.ReportProgress(1, tmptickets)
                Else
                    Debug.Print(ret.msg)
                End If
                Sleep(RefDelay)
            Loop

        End Sub

        ''' <summary>
        ''' 将数据保存到publicconf中的DtTickets对象中
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub ProgressChanged_GetTickeets(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs)

            Dim tmptickets As New List(Of List(Of String))
            tmptickets = e.UserState

            For Each lst In tmptickets

                Dim isnew As Boolean = False

                For Each r As DataRow In PublicConf.DtTickets.Tables(TableName).Rows
                    If lst(0) = r(1) Then
                        '标的在列表中
                        Dim l As New List(Of String)
                        l.Add(r.Item(0))
                        l.AddRange(lst.ToList)
                        r.ItemArray = l.ToArray
                        isnew = True
                    End If
                Next

                If isnew = False Then
                    Dim dr As DataRow = PublicConf.DtTickets.Tables(TableName).NewRow

                    Dim l As New List(Of String)
                    l.Add(PublicConf.DtTickets.Tables(TableName).Rows.Count + 1)
                    l.AddRange(lst.ToList)
                    dr.ItemArray = l.ToArray
                    PublicConf.DtTickets.Tables(TableName).Rows.Add(dr)

                End If

            Next

            'Update()

        End Sub


    End Class


End Namespace