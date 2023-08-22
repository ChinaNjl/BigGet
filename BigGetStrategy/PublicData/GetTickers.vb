
Imports System.Threading.Thread
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Runtime.InteropServices.WindowsRuntime
Imports System.Runtime.InteropServices

Namespace PublicData
    Public Class GetTickers




#Region "*****************对象和变量设置********************"
        Private Property sql As UserType.SqlInfo = PublicConf.Sql
        Private Property userKey As Api.UserInfo = PublicConf.PublicUserKey
        Public Property RefDelay As Integer
            Get
                Return _RefDelay
            End Get
            Set(value As Integer)
                _RefDelay = value
            End Set
        End Property
        Dim _RefDelay As Integer = 2000

        Public ReadOnly Property TableName As String
            Get
                Return "tickertable"
            End Get
        End Property

        Private Property myadp As MySqlDataAdapter
        Private Property bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}
        Public Shared Property Tickers As New DataSet

#End Region



        ''' <summary>
        ''' 读取所有币种的行情数据
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub DoWorkGetTickers(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            Dim Worker As BackgroundWorker = CType(sender, BackgroundWorker)
            Dim UserCall As New Api.UserCall(userKey)

            Dim count As Integer = 1

            Do
                '线程控制线程退出循环
                If Worker.CancellationPending Then Exit Do

                Dim ret As Api.UserType.ReplyType.MarkTickers = UserCall.GetMarkTickers("umcbl")

                '刷新Tickers
                If ret.code = "00000" Then

                    If ret.data.Count > 0 Then

                        For Each d In ret.data

                            Dim dr1 As DataRow = Tickers.Tables(TableName).Rows.Find(d.symbol)
                            If IsNothing(dr1) = False Then
                                dr1.ItemArray = d.toList.ToArray
                            Else
                                Dim newdr As DataRow = Tickers.Tables(TableName).NewRow
                                newdr.ItemArray = d.toList.ToArray
                                Tickers.Tables(TableName).Rows.Add(newdr)
                            End If

                        Next

                    Else
                        Debug.Print("Error:{0}.{1}        {2}", MyBase.ToString, "DoWorkGetTickers", "标的数据为空")
                    End If

                Else
                    Debug.Print("Error:{0}.{1}        {2}", MyBase.ToString, "DoWorkGetTickers", ret.msg)
                End If

                '刷新publicconf.tickers
                If count Mod 20 = 0 Then
                    Worker.ReportProgress(1， 2)
                    count = 1
                End If

                count = count + 1
            Loop

        End Sub



        ''' <summary>
        ''' Tickers.Tables(TableName).Rows的中数据更新到PublicConf.Tickers.Tables(TableName).Rows
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub ProgressChanged_GetTickers(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs)

            Dim tmptickets As Integer
            tmptickets = e.UserState

            'Tickers.Tables(TableName).Rows的中数据更新到PublicConf.Tickers.Tables(TableName).Rows
            For Each d1 As DataRow In Tickers.Tables(TableName).Rows

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



#Region "**************************功能函数**********************************"


        ''' <summary>
        ''' 读取表
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadTable() As Boolean

            Dim conn As New MySqlConnection(sql.ConnectStr)

            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print("Error:{0}.{1}        {2}", MyBase.ToString, "ReadTable", "打开数据库失败")
                Return False
            End Try

            Dim commandStr As String = "select * from " & TableName
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)
            myadp.MissingSchemaAction = MissingSchemaAction.AddWithKey      '加上默认主键

            Try
                SyncLock Tickers
                    myadp.Fill(Tickers, TableName)   '将读取到的内容存入ds中
                    myadp.Fill(PublicConf.Tickers, TableName)   '将读取到的内容存入ds中
                End SyncLock
            Catch ex As Exception
                Debug.Print("Error:{0}.{1}        {2}", MyBase.ToString, "ReadTable", "保存到dataset失败")
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
                myadp.Update(Tickers, TableName)
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
                'AddHandler bgw.DoWork, AddressOf DoWorkGetTickets
                AddHandler bgw.DoWork, AddressOf DoWorkGetTickers
                AddHandler bgw.ProgressChanged, AddressOf ProgressChanged_GetTickers
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


    End Class


End Namespace