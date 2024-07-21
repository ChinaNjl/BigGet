Imports System.Text.Json
Imports System.Threading

Namespace Api.Request.Contract.ParamType

    Public Class OrderPlaceOrder

        ''' <summary>
        ''' 产品ID 必须大写
        ''' </summary>
        ''' <returns></returns>
        Public Property symbol As String
            Get
                Return _symbol
            End Get
            Set(value As String)
                _symbol = value.ToUpper
            End Set
        End Property

        Dim _symbol As String

        ''' <summary>
        ''' 保证金币种 必须大写
        ''' </summary>
        ''' <returns></returns>
        Public Property marginCoin As String
            Get
                Return _marginCoin
            End Get
            Set(value As String)
                _marginCoin = value.ToUpper
            End Set
        End Property

        Dim _marginCoin As String

        ''' <summary>
        ''' 下单数量，base coin
        ''' </summary>
        ''' <returns></returns>
        Public Property size As String

        ''' <summary>
        ''' 开单方向
        ''' </summary>
        ''' <returns></returns>
        Public Property side As String

        ''' <summary>
        ''' 订单类型 limit/market
        ''' </summary>
        ''' <returns></returns>
        Public Property orderType As String

        '非必须参数

        ''' <summary>
        ''' 下单价格(市价时不传)
        ''' </summary>
        ''' <returns></returns>
        Public Property price As String

        ''' <summary>
        ''' 订单有效期
        ''' </summary>
        ''' <returns></returns>
        Public Property timeInForceValue As String

        ''' <summary>
        ''' 客户端标识 唯一
        ''' </summary>
        ''' <returns></returns>
        Public Property clientOid As String

        ''' <summary>
        ''' 预设止盈价格
        ''' </summary>
        ''' <returns></returns>
        Public Property presetTakeProfitPrice As String

        ''' <summary>
        ''' 预设止损价格
        ''' </summary>
        ''' <returns></returns>
        Public Property presetStopLossPrice As String

        ''' <summary>
        ''' 默认false; 只减仓，单向持仓平仓时仓位价值小于5USDT则需要设置为true, 双向持仓时不生效
        ''' </summary>
        ''' <returns></returns>
        Public Property reduceOnly As Boolean
            Get
                Return _reduceOnly
            End Get
            Set(value As Boolean)
                _reduceOnly = value
            End Set
        End Property

        Dim _reduceOnly As Boolean = False

        ''' <summary>
        ''' 是否反手订单： 不传时默认为false(正常下单操作)； true:表示这是反手操作
        ''' </summary>
        ''' <returns></returns>
        Public Property reverse As Boolean
            Get
                Return _reverse
            End Get
            Set(value As Boolean)
                _reverse = value
            End Set
        End Property

        Dim _reverse As Boolean = False

        Public Function ToJson() As String

            Dim s As String = JsonSerializer.Serialize(Me)

            s = s.Replace("""price"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""timeInForceValue"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""clientOid"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""presetTakeProfitPrice"":null,", "")
            s = s.Replace("""presetTakeProfitPrice"":"""",", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""presetStopLossPrice"":null", "")
            s = s.Replace("""presetStopLossPrice"":""""", "")
            s = s.Replace(",,", ",")

            s = s.Replace("{,", "{")
            s = s.Replace(",}", "}")

            Return s

        End Function

    End Class

End Namespace