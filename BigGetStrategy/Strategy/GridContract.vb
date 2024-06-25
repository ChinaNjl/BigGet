Imports System.Threading.Thread
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.Text

Namespace Strategy
    ''' <summary>
    ''' 网格合约交易
    ''' </summary>


    Public Class GridContract


        ''' <summary>
        ''' 策略编号（在数据库中的标识）
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Id As String
            Get
                Return ds.Tables(TableName).Rows(0).Item("id")
            End Get
        End Property
        Public Property State As Boolean = False


        Private dr As DataRow
        Private UserInfo As Api.UserInfo
        Private UserCall As Api.UserObject.Contract.UserCall
        Private ds As New DataSet
        Private Property myadp As MySqlDataAdapter

        Private ReadOnly Property symbol As String
            Get
                Return ds.Tables(TableName).Rows(0).Item("symbol")
            End Get
        End Property

        Private ReadOnly Property basePrice As String
            Get
                Return ds.Tables(TableName).Rows(0).Item("basePrice")
            End Get
        End Property


        ''' <summary>
        ''' 0=按价浮动。1=按比例浮动
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property priceMethod As Integer
            Get
                'Return CType(ds.Tables(TableName).Rows(0).Item("priceMethod"), Integer)
                '默认0
                Return 0
            End Get
        End Property

        Private ReadOnly Property priceChange As String
            Get
                Return ds.Tables(TableName).Rows(0).Item("priceChange")
            End Get
        End Property

        Private ReadOnly Property size As String
            Get
                Return ds.Tables(TableName).Rows(0).Item("size")
            End Get
        End Property

        Private ReadOnly Property marginCoin As String
            Get
                Return ds.Tables(TableName).Rows(0).Item("marginCoin")
            End Get
        End Property

        Private ReadOnly Property productType As String
            Get
                Return ds.Tables(TableName).Rows(0).Item("productType")
            End Get
        End Property

        Private ReadOnly Property upPrice As String
            Get
                Dim _upprice As Single

                If priceMethod = 0 Then
                    _upprice = CType(basePrice, Single) + CType(priceChange, Single)
                    Return _upprice.ToString
                End If

                If priceMethod = 1 Then
                    _upprice = CType(basePrice, Single) + CType(basePrice, Single) * CType(priceChange, Single)
                    Return _upprice.ToString
                End If

                Return _upprice.ToString
            End Get
        End Property

        Private ReadOnly Property downPrice As String
            Get
                Dim _downprice As Single

                If priceMethod = 0 Then
                    _downprice = CType(basePrice, Single) - CType(priceChange, Single)
                    Return _downprice.ToString
                End If

                If priceMethod = 1 Then
                    _downprice = CType(basePrice, Single) - CType(basePrice, Single) * CType(priceChange, Single)
                    Return _downprice.ToString
                End If

                Return _downprice.ToString
            End Get
        End Property

        Private ReadOnly Property TableName As String
            Get
                Return "strategytable"
            End Get
        End Property

        Private Function sClientId() As String

            Dim str As New StringBuilder

            If Id.Length < 4 Then
                For i = 1 To 4 - Id.Length
                    str.Append("0")
                Next
            End If
            str.Append(Id)
            Dim ts As TimeSpan = DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0)
            str.Append(Convert.ToInt64(ts.TotalMilliseconds).ToString())

            Return str.ToString
        End Function


        Public bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}



        '方法----------------------------------------------------------


        Sub New(ByVal _dr As DataRow)

            Call OpenTableFromDatabase(_dr.Item("id"))

        End Sub

        ''' <summary>
        ''' 通过dataset控件读取数据库Strategytable 表
        ''' </summary>
        Private Sub OpenTableFromDatabase(id)

            Dim conn As New MySqlConnection(PublicConf.sql.ConnectStr)

            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try


            Dim commandStr As String = "select * from strategytable where id=" & id
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)


            Try
                SyncLock ds
                    ds = New DataSet
                    myadp.Fill(ds, "strategytable")   '将读取到的内容存入ds中
                End SyncLock
            Catch ex As Exception
                Debug.Print("fijajfdjf")
            End Try
        End Sub




        Private Sub Update()

            Try
                myadp.Update(ds, TableName)
            Catch ex As Exception
                Debug.Print("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
            End Try

        End Sub

        Public Sub Run()

            UserInfo = New Api.UserInfo With {
                .ApiKey = ds.Tables(TableName).Rows(0).Item("apikey"),
                .Secretkey = ds.Tables(TableName).Rows(0).Item("secretkey"),
                .Passphrase = ds.Tables(TableName).Rows(0).Item("passphrase"),
                .Host = ds.Tables(TableName).Rows(0).Item("host")
            }

            UserCall = New Api.UserObject.Contract.UserCall(UserInfo)

            If bgw.IsBusy = False Then
                AddHandler bgw.DoWork, AddressOf DoWorkRunStrategy
                AddHandler bgw.RunWorkerCompleted, AddressOf RunComplete
                bgw.RunWorkerAsync()
            End If

        End Sub

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

            State = True

            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

            If True Then

                '取消全部订单
                CancelAllOrders()

                '批量下单，交易员间隔1秒

                Do While BatchOrders() = False
                    CancelAllOrders()
                    Sleep(1000)
                    If worker.CancellationPending Then Exit Sub
                Loop

                Do

                    Dim ret As Api.UserType.Contract.ReplyType.OrderCurrent
                    ret = UserCall.GetOrderCurrent(symbol)
                    If ret.code <> "99999" Then

                        If ret.code = 0 Then
                            If ret.FindOrderType("open_short", upPrice) = 0 And ret.FindOrderType("open_long", downPrice) = 0 Then

                                OrderOpenShort()
                                OrderOpenLong()

                            Else

                                If ret.FindOrderType("open_short", upPrice) = 0 And ret.FindOrderType("open_long", downPrice) = 1 Then
                                    '空单委托成交，多单委托未成交

                                    '设置基准价
                                    ds.Tables(TableName).Rows(0).Item("basePrice") = upPrice
                                    OrderOpenLong()

                                    If ret.FindOrderType("open_short", upPrice) = 0 Then
                                        OrderOpenShort()
                                    End If

                                    Update()
                                Else
                                    If ret.FindOrderType("open_short", upPrice) = 1 And ret.FindOrderType("open_long", downPrice) = 0 Then
                                        '多单委托成交，空单委托未成交

                                        '设置基准价
                                        ds.Tables(TableName).Rows(0).Item("basePrice") = downPrice
                                        OrderOpenShort()
                                        If ret.FindOrderType("open_long", downPrice) = 0 Then
                                            OrderOpenLong()
                                        End If

                                        Update()

                                    End If
                                End If

                            End If
                        Else
                            Debug.Print(ret.ToJson)
                        End If

                    Else

                    End If



                    Sleep(50)
                    If worker.CancellationPending Then Exit Sub
                Loop

            End If


        End Sub


        Public Sub RunComplete(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs)
            CancelAllOrders()
            State = False
        End Sub


        Private Function CancelAllOrders() As Boolean

            Try
                Dim ret As Api.UserType.Contract.ReplyType.OrderCancelAllOrders = UserCall.OrderCancelAllOrders(productType, marginCoin)
                If ret.code <> 0 Then
                    Debug.Print(ret.msg)
                End If

            Catch ex As Exception
                Return False
            End Try


            Return True
        End Function


        Private Function BatchOrders() As Boolean

            '创建订单参数

            Dim OpenShort As New Api.UserType.Contract.ParamType.OrderBatchOrders.orderData With {
                .price = upPrice,
                .size = size,
                .side = "open_short",
                .orderType = "limit",
                .presetTakeProfitPrice = basePrice,
                .clientOid = sClientId()
            }

            Dim BatchPrarm As Api.UserType.Contract.ParamType.OrderBatchOrders
            Dim ret1 As Api.UserType.Contract.ReplyType.OrderBatchOrders
            Dim ret2 As Api.UserType.Contract.ReplyType.OrderBatchOrders


            BatchPrarm = New Api.UserType.Contract.ParamType.OrderBatchOrders With {
                .symbol = symbol,
                .marginCoin = marginCoin
            }
            BatchPrarm.orderDataList.Add(OpenShort)
            ret1 = UserCall.OrderBatchOrders(BatchPrarm)

            Sleep(1000)
            Dim Openlong As New Api.UserType.Contract.ParamType.OrderBatchOrders.orderData With {
                .price = downPrice,
                .size = size,
                .side = "open_long",
                .orderType = "limit",
                .presetTakeProfitPrice = basePrice,
                .clientOid = sClientId()
            }
            BatchPrarm = New Api.UserType.Contract.ParamType.OrderBatchOrders With {
                .symbol = symbol,
                .marginCoin = marginCoin
            }
            BatchPrarm.orderDataList.Add(Openlong)
            ret2 = UserCall.OrderBatchOrders(BatchPrarm)



            If ret1.code = 0 And ret2.code = 0 Then

                If ret1.data.orderInfo.Count = ret2.data.orderInfo.Count Then
                    Return True
                Else
                    Return False
                End If

            Else
                Return False
            End If


        End Function

        Private Function OrderOpenLong() As Boolean

            '创建订单参数
            Dim Openlong As New Api.UserType.Contract.ParamType.OrderBatchOrders.orderData With {
                .price = downPrice,
                .size = size,
                .side = "open_long",
                .orderType = "limit",
                .presetTakeProfitPrice = basePrice,
                .clientOid = sClientId()
            }
            Dim BatchPrarm As Api.UserType.Contract.ParamType.OrderBatchOrders
            Dim ret As Api.UserType.Contract.ReplyType.OrderBatchOrders


            BatchPrarm = New Api.UserType.Contract.ParamType.OrderBatchOrders With {
                .symbol = symbol,
                .marginCoin = marginCoin
            }
            BatchPrarm.orderDataList.Add(Openlong)

            Try
                ret = UserCall.OrderBatchOrders(BatchPrarm)
                If ret.data.orderInfo.Count > 0 Then
                    Sleep(1000)
                    Return True
                Else
                    Sleep(1000)
                    Return False
                End If


            Catch ex As Exception
                Return False
            End Try

            Return True

        End Function

        Private Function OrderOpenShort() As Boolean
            '创建订单参数
            Dim OpenShort As New Api.UserType.Contract.ParamType.OrderBatchOrders.orderData With {
                .price = upPrice,
                .size = size,
                .side = "open_short",
                .orderType = "limit",
                .presetTakeProfitPrice = basePrice,
                .clientOid = sClientId()
            }
            Dim BatchPrarm As Api.UserType.Contract.ParamType.OrderBatchOrders
            Dim ret As Api.UserType.Contract.ReplyType.OrderBatchOrders

            BatchPrarm = New Api.UserType.Contract.ParamType.OrderBatchOrders With {
                .symbol = symbol,
                .marginCoin = marginCoin
            }
            BatchPrarm.orderDataList.Add(OpenShort)

            Try
                ret = UserCall.OrderBatchOrders(BatchPrarm)
                If ret.data.orderInfo.Count > 0 Then
                    Sleep(1000)
                    Return True
                Else
                    Sleep(1000)
                    Return False
                End If


            Catch ex As Exception
                Return False
            End Try

            Return True


        End Function



        ''' <summary>
        ''' 0=全部成交，2=等于全部未成交，1=成交1单，-1=报错,10=剩多单，20=剩空单
        ''' </summary>
        ''' <returns></returns>
        Private Function CheckOrderCurrentState() As Integer

            Dim ret As Api.UserType.Contract.ReplyType.OrderCurrent
            Try
                ret = UserCall.GetOrderCurrent(symbol)
                If ret.code = 0 Then

                    Dim copen_long As Integer = ret.FindOrderType("open_long")
                    Dim copen_short As Integer = ret.FindOrderType("open_short")

                    If copen_long = 0 And copen_short = 1 Then
                        '剩余空单，多单委托成交
                        Return 20
                    End If
                    If copen_long = 1 And copen_short = 0 Then
                        '剩多单,空单委托成交

                        Return 10
                    End If
                    If copen_long = 0 And copen_short = 0 Then
                        '开多空单都不存在
                        Return 0
                    End If

                End If

            Catch ex As Exception
                Return -1
            End Try

            Return 2
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
