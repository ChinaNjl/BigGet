Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.Unicode


Namespace UserType.Contract.ReplyType

    ''' <summary>
    ''' 当前委托
    ''' </summary>
    Public Class OrderCurrent

        Public Property code As String
        Public Property data As List(Of DataType)
        Public Property msg As String
        Public Property requestTime
            Get
                Return _requestTime
            End Get
            Set(value)
                _requestTime = CType(_requestTime, String)
            End Set
        End Property
        Dim _requestTime

        Public Class DataType

            ''' <summary>
            ''' 交易对名称
            ''' </summary>
            ''' <returns></returns>
            Public Property symbol As String

            ''' <summary>
            ''' 委托数量
            ''' </summary>
            ''' <returns></returns>
            Public Property size
                Get
                    Return _size
                End Get
                Set(value)
                    _size = value.ToString
                End Set
            End Property
            Dim _size As String

            ''' <summary>
            ''' 订单号
            ''' </summary>
            ''' <returns></returns>
            Public Property orderId As String

            ''' <summary>
            ''' 客户端自定义id
            ''' </summary>
            ''' <returns></returns>
            Public Property clientOid As String

            ''' <summary>
            ''' 成交数量，左币
            ''' </summary>
            ''' <returns></returns>
            Public Property filledQty
                Get
                    Return _filledQty
                End Get
                Set(value)
                    _filledQty = value.ToString
                End Set
            End Property
            Dim _filledQty As String

            ''' <summary>
            ''' 手续费
            ''' </summary>
            ''' <returns></returns>
            Public Property fee
                Get
                    Return _fee
                End Get
                Set(value)
                    _fee = value.ToString
                End Set
            End Property
            Dim _fee As String

            ''' <summary>
            ''' 委托价格
            ''' </summary>
            ''' <returns></returns>
            Public Property price
                Get
                    Return _price
                End Get
                Set(value)
                    If IsDBNull(value) = False And IsNothing(value) = False Then
                        _price = value.ToString
                    Else
                        _price = 0
                    End If

                End Set
            End Property
            Dim _price As String

            ''' <summary>
            ''' 平均成交价格
            ''' </summary>
            ''' <returns></returns>
            Public Property priceAvg As String

            ''' <summary>
            ''' 订单状态
            ''' </summary>
            ''' <returns></returns>
            Public Property state As String

            ''' <summary>
            ''' 开单方向
            ''' </summary>
            ''' <returns></returns>
            Public Property side As String

            ''' <summary>
            ''' 订单有效期
            ''' </summary>
            ''' <returns></returns>
            Public Property timeInForce As String

            ''' <summary>
            ''' 总盈亏
            ''' </summary>
            ''' <returns></returns>
            Public Property totalProfits
                Get
                    Return _totalProfits
                End Get
                Set(value)
                    _totalProfits = value.ToString
                End Set
            End Property
            Dim _totalProfits As String

            ''' <summary>
            ''' 持仓方向
            ''' </summary>
            ''' <returns></returns>
            Public Property posSide As String

            ''' <summary>
            ''' 保证金币种
            ''' </summary>
            ''' <returns></returns>
            Public Property marginCoin As String

            ''' <summary>
            ''' 成交金额, 右币
            ''' </summary>
            ''' <returns></returns>
            Public Property filledAmount As Single

            ''' <summary>
            ''' 杠杆倍数
            ''' </summary>
            ''' <returns></returns>
            Public Property leverage As String

            ''' <summary>
            ''' 	Margin mode
            ''' </summary>
            ''' <returns></returns>
            Public Property marginMode As String

            ''' <summary>
            ''' 是否只减仓
            ''' </summary>
            ''' <returns></returns>
            Public Property reduceOnly As Boolean

            ''' <summary>
            ''' enterPointSource
            ''' </summary>
            ''' <returns></returns>
            Public Property enterPointSource As String

            ''' <summary>
            ''' 	Trade Side
            ''' </summary>
            ''' <returns></returns>
            Public Property tradeSide As String

            ''' <summary>
            ''' 订单类型
            ''' </summary>
            ''' <returns></returns>
            Public Property orderType As String

            ''' <summary>
            ''' 创建时间
            ''' </summary>
            ''' <returns></returns>
            Public Property cTime
                Get
                    Return _cTime
                End Get
                Set(value)
                    _cTime = value.ToString
                End Set
            End Property
            Dim _cTime As String




        End Class


        Public Function Count() As Integer
            Return data.Count
        End Function

        ''' <summary>
        ''' 返回最大价格委托
        ''' </summary>
        ''' <returns></returns>
        Public Function FindMaxPrice() As Single

            Dim price As Single = 0

            If Count() = 0 Then Return 0

            For Each d In data
                If price = 0 Then
                    price = d.price
                Else
                    If CType(d.price, Single) > price Then
                        price = CType(d.price, Single)
                    End If
                End If
            Next

            Return price
        End Function

        Public Function FindOrderType(side As String) As Integer

            Dim c As Integer

            For Each d In data

                If d.side = side Then
                    c = c + 1
                End If


            Next

            Return c

        End Function

        Public Function FindOrderType(side As String, price As String) As Integer

            Dim c As Integer

            For Each d In data

                If d.side = side And CType(d.price, Single).ToString() = CType(price, Single).ToString Then
                    c = c + 1
                End If

            Next

            Return c

        End Function

        ''' <summary>
        ''' 搜索委托价
        ''' </summary>
        ''' <param name="p_Price"></param>
        ''' <returns></returns>
        Public Function FindPrice(p_Price As Single) As Boolean
            _FindPrice = p_Price
            Dim ret = data.Find(AddressOf BoolPrice)

            Return Not IsNothing(ret)
        End Function
        Dim _FindPrice As Single

        Private Function BoolPrice(obj As DataType) As Boolean
            Return obj.price = _FindPrice
        End Function













        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me, New JsonSerializerOptions With {
                                                           .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                           .Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)})
            Return s

        End Function



    End Class

End Namespace




