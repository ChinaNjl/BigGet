Imports System.Text.Json

Namespace UserType.Contract.ParamType

    Public Class OrderBatchOrders

        ''' <summary>
        ''' 产品ID 必须大写
        ''' </summary>
        ''' <returns></returns>
        Public Property symbol As String

        ''' <summary>
        ''' 保证金币种 必须大写
        ''' </summary>
        ''' <returns></returns>
        Public Property marginCoin As String

        ''' <summary>
        ''' 订单信息列表
        ''' </summary>
        ''' <returns></returns>
        Public Property orderDataList As New List(Of orderData)

        Public Class orderData

            ''' <summary>
            ''' 下单数量
            ''' </summary>
            ''' <returns></returns>
            Public Property size As String

            ''' <summary>
            ''' 合约价格
            ''' </summary>
            ''' <returns></returns>
            Public Property price As String

            ''' <summary>
            ''' 开单方向
            ''' </summary>
            ''' <returns></returns>
            Public Property side As String

            ''' <summary>
            ''' 订单类型：limit/market
            ''' </summary>
            ''' <returns></returns>
            Public Property orderType As String

            ''' <summary>
            ''' 订单有效期
            ''' </summary>
            ''' <returns></returns>
            Public Property timeInForceValue As String

            ''' <summary>
            ''' 客户端唯一标识
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
        End Class


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




