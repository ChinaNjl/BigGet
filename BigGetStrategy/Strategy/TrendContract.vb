
Imports System.Threading.Thread
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports Api
Imports Org.BouncyCastle.Math.EC
Imports Org.BouncyCastle.Utilities
Imports System.Security.Principal
Imports System.Net.Http.Headers
Imports System.Security.Policy
Imports BigGetStrategy.PublicData

Namespace Strategy

    ''' <summary>
    ''' 趋势合约交易
    ''' </summary>
    Public Class TrendContract

#Region "临时设置"

#End Region



#Region "*********************对象和变量*********************"


        ''' <summary>
        ''' true:策略信息获取成功，false：策略信息获取失败
        ''' </summary>
        ''' <returns></returns>
        Public Property flgInitializen As Boolean = False
        Public Property State As Boolean = False                '策略运行状态

#Region "--------------------对象设置------------------------"

        Private Property sql As UserType.SqlInfo = PublicConf.Sql   'sql服务器信息

        Public bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}     '策略线程

        Private Property UserInfo As UserInfo

        Private UserCall As UserCall

        Private Property dsStrategyInfo As New DataSet

        Private Property myadp As MySqlDataAdapter

#End Region


#Region "----------------------产品基本信息-------------------------"

        ''' <summary>
        ''' 价格围栏
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property max As Single
            Get
                If _max = 0 Then
                    _max = PublicConf.DtContracts.Tables("contracttable").Rows.Find(symbol).Item("buyLimitPriceRatio")
                End If

                Return _max
            End Get
        End Property
        Dim _max As Single = 0

        ''' <summary>
        ''' 策略编号（在数据库中的标识）
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property id As String
            Get
                If _id.Length > 0 Then
                    Return _id
                Else
                    _id = dsStrategyInfo.Tables("strategytable").Rows(0).Item("id")
                    Return _id
                End If
            End Get
        End Property
        Dim _id As String = ""

        ''' <summary>
        ''' 合约的单位
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property size As String
            Get
                If _size.Length > 0 Then
                    Return _size
                Else
                    _size = dsStrategyInfo.Tables("strategytable").Rows(0).Item("size")
                    Return _size
                End If
            End Get
        End Property
        Dim _size As String = ""

        ''' <summary>
        ''' 价格变化单位
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property priceChange As Single
            Get
                If _priceChange >= 0 Then
                    Return _priceChange
                Else
                    _priceChange = CType(dsStrategyInfo.Tables("strategytable").Rows(0).Item("priceChange"), Single)
                    Return _priceChange
                End If
            End Get
        End Property
        Dim _priceChange As Single = -1

        ''' <summary>
        ''' 区间上沿
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property upLine As Single
            Get

                If _upLine >= 0 Then
                    Return _upLine
                Else
                    _upLine = CType(dsStrategyInfo.Tables("strategytable").Rows(0).Item("upLine"), Single)
                    Return _upLine
                End If


            End Get
        End Property
        Dim _upLine As Single = -1

        ''' <summary>
        ''' 区间下沿
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property downLine As Single
            Get
                If _downLine >= 0 Then
                    Return _downLine
                Else
                    _downLine = CType(dsStrategyInfo.Tables("strategytable").Rows(0).Item("downLine"), Single)
                    Return _downLine
                End If

            End Get
        End Property
        Dim _downLine As Single = -1

        ''' <summary>
        ''' 初始挂单价
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property startPrice As Single
            Get
                If _startPrice >= 0 Then
                    Return _startPrice
                Else
                    _startPrice = CType(dsStrategyInfo.Tables("strategytable").Rows(0).Item("basePrice"), Single)
                    Return _startPrice
                End If
            End Get
        End Property
        Dim _startPrice As Single = -1

        ''' <summary>
        ''' 产品id
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property symbol As String
            Get
                If _symbol.Length > 0 Then
                    Return _symbol
                Else
                    _symbol = dsStrategyInfo.Tables("strategytable").Rows(0).Item("symbol")
                    Return _symbol
                End If
            End Get
        End Property
        Dim _symbol As String = ""

        ''' <summary>
        ''' 保证金币种
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property marginCoin As String
            Get
                If _marginCoin.Length > 0 Then
                    Return _marginCoin
                Else
                    _marginCoin = dsStrategyInfo.Tables("strategytable").Rows(0).Item("marginCoin")
                    Return _marginCoin
                End If
            End Get
        End Property
        Dim _marginCoin As String = ""

        ''' <summary>
        ''' 产品类型
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property productType As String
            Get
                If _productType.Length > 0 Then
                    Return _productType
                Else
                    _productType = dsStrategyInfo.Tables("strategytable").Rows(0).Item("productType")
                    Return _productType
                End If
            End Get
        End Property
        Dim _productType As String = ""

#End Region


#End Region



#Region "*********************过程和方法********************"

#Region "-----------------------2 初始化----------------------------"

        Sub New(ByVal p_dr As DataRow)
            '从数据库读取策略信息，保存到 dsStrategyInfo 对象中
            GetStrategy(p_dr.Item("id"))

            '记录对象初始化状态
            If IsNothing(dsStrategyInfo) = False Then
                flgInitializen = True
            End If
        End Sub

        ''' <summary>
        ''' 从数据库中读取策略信息
        ''' </summary>
        ''' <param name="p_id"></param>
        ''' <returns></returns>
        Private Function GetStrategy(p_id As String) As Boolean

            Dim conn As New MySqlConnection(sql.ConnectStr)

            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print("Error:打开数据库失败")
            End Try

            Dim commandStr As String = "select * from strategytable where id=" & p_id
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)
            myadp.MissingSchemaAction = MissingSchemaAction.AddWithKey      '加上默认主键


            Try
                myadp.Fill(dsStrategyInfo, "strategytable")   '将读取到的内容存入ds中
            Catch ex As Exception
                Debug.Print(“Error：TrendContract/GetStrategy”)
                Return False
            End Try

            Return True

        End Function


        ''' <summary>
        ''' 更新变动的数据
        ''' </summary>
        ''' <returns></returns>
        Public Function Update() As Integer

            Try
                Return myadp.Update(dsStrategyInfo, "strategytable")
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return 0
            End Try

            Return True

        End Function



#End Region


#Region "-----------------2 Backgroundwork设置----------------------"


        ''' <summary>
        ''' 启动策略
        ''' </summary>
        Public Sub Run()

            '创建api对象
            UserInfo = New UserInfo With {
                .ApiKey = dsStrategyInfo.Tables("strategytable").Rows(0).Item("apikey"),
                .Secretkey = dsStrategyInfo.Tables("strategytable").Rows(0).Item("secretkey"),
                .Passphrase = dsStrategyInfo.Tables("strategytable").Rows(0).Item("passphrase"),
                .Host = dsStrategyInfo.Tables("strategytable").Rows(0).Item("host")
            }
            UserCall = New UserCall(UserInfo)

            '启动策略
            If bgw.IsBusy = False Then
                AddHandler bgw.DoWork, AddressOf DoWorkRunStrategy
                AddHandler bgw.RunWorkerCompleted, AddressOf RunComplete
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

        ''' <summary>
        ''' 策略主进程
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub DoWorkRunStrategy(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            State = True    '策略启动之后将状态设置成true

            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

            '撤销所有委托
            Dim ret As Boolean = CancelOrders(productType, marginCoin)


            Dim t As Api.UserType.ReplyType.MarkTicker = UserCall.GetMarkTicker(symbol)
            '从上往下挂单，如果价格超过标的挂单范围改用市价开单
            FistOpenOrders（symbol, marginCoin, startPrice, upLine, downLine, size, CType(priceChange, Single), t.data.last * (1 + max)）

            Do
                '获取价格数据
                'Dim retTicker As Api.UserType.ReplyType.MarkTicker = UserCall.GetMarkTicker(symbol)

                Dim drTicker As DataRow = GetTickers.Tickers.Tables("tickertable").Rows.Find(symbol)

                '获取委托数据
                Dim retCurrent As Api.UserType.ReplyType.OrderCurrent = UserCall.GetOrderCurrent(symbol)

                Dim maxCurrentPrice As Single = retCurrent.FindMaxPrice     '最大委托价格
                dsStrategyInfo.Tables("strategytable").Rows.Find("2").Item("basePrice") = maxCurrentPrice + priceChange
                Update()
                Dim newPrice As Single = drTicker.Item("last")
                '当没有委托单的时候将最大委托价格设置为区间下沿
                If maxCurrentPrice = 0 Then maxCurrentPrice = downLine

                '从下往上补补单
                MakeUpOrder(newPrice, maxCurrentPrice, CType(priceChange, Single), upLine, symbol, marginCoin, size)

                Sleep（50）
            Loop


        End Sub

        ''' <summary>
        ''' 策略停止，将状态设置为false
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub RunComplete(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs)
            State = False
        End Sub

#End Region


#Region "------------------------2 自定义函数-------------------------"

        ''' <summary>
        ''' 初始化委托,返回第一次下单的价格
        ''' </summary>
        ''' <param name="p_startPrice"></param>
        ''' <param name="p_upLine"></param>
        ''' <param name="p_downLine"></param>
        ''' <param name="p_change"></param>
        ''' <returns></returns>
        Private Function FistOpenOrders(p_symbol As String,
                                        p_marginCoin As String,
                                        p_startPrice As Single,
                                        p_upLine As Single,
                                        p_downLine As Single,
                                        p_size As String,
                                        p_change As Single,
                                        p_ticker As Single) As Boolean

            Dim flgFirst As Boolean = False
            Dim flgFirstPrice As Single

            '检查p_startPrice是否在交易区间内
            If p_startPrice > p_upLine Or p_startPrice < p_downLine Then Return False


            '循环下单
            Do
                '当p_startPrice价格超出区间下线，退出循环
                p_startPrice = p_startPrice - p_change
                If p_startPrice < p_downLine Then Exit Do

                If flgFirst = False Then
                    flgFirstPrice = p_startPrice
                    flgFirst = Not flgFirst
                End If

                '开始下单
                If p_startPrice > p_ticker Then
                    OpenOrder(p_symbol, p_marginCoin, "open_long", p_startPrice.ToString, p_size, "market", (p_startPrice + p_change).ToString)
                Else
                    OpenOrder(p_symbol, p_marginCoin, "open_long", p_startPrice.ToString, p_size, "limit", (p_startPrice + p_change).ToString)
                End If


                Sleep(1000)
            Loop

            Return flgFirstPrice

        End Function

        Private Function MakeUpOrder(p_newPrice As Single,
                                     p_maxCurrentPrice As Single,
                                     p_change As Single,
                                     p_upline As Single,
                                     p_symbol As String,
                                     p_marginCoin As String,
                                     p_size As String) As Boolean

            Do While p_newPrice > p_maxCurrentPrice + p_change * 2 And p_maxCurrentPrice + p_change <= p_upline

                Dim ret1 As Boolean = OpenOrder(p_symbol, p_marginCoin, "open_long", (p_maxCurrentPrice + p_change).ToString, p_size, "limit", p_maxCurrentPrice + p_change * 2)
                p_maxCurrentPrice = p_maxCurrentPrice + p_change
                If ret1 = True Then Sleep(1000)


            Loop

            Return True
        End Function



        ''' <summary>
        ''' 计算时间戳
        ''' </summary>
        ''' <returns></returns>
        Private Function GetUtcNowTimestamp() As Int64

            Dim UtcTime As String = TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            Dim ts As TimeSpan = Date.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
            Return CType(ts.TotalMilliseconds, Int64)

        End Function

        ''' <summary>
        ''' 撤销所有委托
        ''' </summary>
        ''' <param name="p_productType"></param>
        ''' <param name="p_marginCoin"></param>
        ''' <returns></returns>
        Private Function CancelOrders(p_productType As String,
                                      p_marginCoin As String) As Boolean

            Try
                Dim ret As Api.UserType.ReplyType.OrderCancelAllOrders = UserCall.OrderCancelAllOrders(p_productType, p_marginCoin)
                If ret.code = "00000" Then
                    Return True
                Else
                    Debug.Print(ret.msg)
                    Return False
                End If
            Catch ex As Exception
                Debug.Print(“Error：TrendContract/CancelOrders”)
            End Try

            Return False
        End Function



        ''' <summary>
        ''' 开单
        ''' </summary>
        ''' <param name="_symbol"></param>
        ''' <param name="_marginCoin"></param>
        ''' <param name="_side"></param>
        ''' <param name="_price"></param>
        ''' <param name="_size"></param>
        ''' <param name="_orderType"></param>
        ''' <param name="_presetTakeProfitPrice"></param>
        ''' <param name="_presetStopLossPrice"></param>
        ''' <returns></returns>
        Private Function OpenOrder(_symbol As String,
                                   _marginCoin As String,
                                   _side As String，
                                   _price As String,
                                   _size As String,
                                   _orderType As String,
                                   Optional _presetTakeProfitPrice As String = "",
                                   Optional _presetStopLossPrice As String = "") As Boolean

            '创建和设置批量下单参数，
            Dim prarm As New Api.UserType.ParamType.OrderBatchOrders With {
                .symbol = _symbol,
                .marginCoin = _marginCoin}
            Dim order As New Api.UserType.ParamType.OrderBatchOrders.orderData With {
                .price = _price.ToString,
                .size = _size.ToString,
                .side = _side.ToString,
                .orderType = _orderType.ToString,
                .presetTakeProfitPrice = _presetTakeProfitPrice,
                .presetStopLossPrice = _presetStopLossPrice}
            prarm.orderDataList.Add(order)

            '下单和返回结果
            Try

                Dim ret As Api.UserType.ReplyType.OrderBatchOrders = UserCall.OrderBatchOrders(prarm)
                Debug.Print(ret.ToJson)
                If ret.data.orderInfo.Count > 0 Then
                    Return True
                Else
                    Debug.Print(ret.ToJson)
                End If

            Catch ex As Exception

                Debug.Print(“Error：TrendContract/OpenOrder”)

            End Try

            Return False
        End Function



        ''' <summary>
        ''' 反手
        ''' </summary>
        ''' <param name="_symbol"></param>
        ''' <param name="_marginCoin"></param>
        ''' <param name="_size"></param>
        ''' <param name="_side"></param>
        ''' <returns></returns>
        Private Function RevOrder(_symbol As String,
                                  _marginCoin As String,
                                  _size As String,
                                  _side As String) As Boolean

            '创建反手参数对象，并设置参数
            Dim prarm As New Api.UserType.ParamType.OrderPlaceOrder With {
                .symbol = _symbol,
                .marginCoin = _marginCoin,
                .size = _size,
                .side = _side,
                .orderType = "market"}


            Dim ret As Api.UserType.ReplyType.OrderPlaceOrder = UserCall.OrderPlaceOrder(prarm)
            Debug.Print(ret.ToJson)
            If ret.code = "00000" Then
                Return True
            Else
                Return False
            End If

        End Function





#End Region


#End Region












    End Class


End Namespace








