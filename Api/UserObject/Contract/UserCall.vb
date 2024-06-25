


Imports System.Reflection
Imports System.Text.Json
Imports System.Threading





Namespace UserObject.Contract
    ''' <summary>
    ''' 用户调用api对象
    ''' </summary>
    Public Class UserCall

        Sub New(_UserInfo As UserInfo)
            UserAPI = New UserAPI(_UserInfo)
        End Sub

        Private UserAPI As UserAPI




#Region "行情接口"

        ''' <summary>
        ''' 深度行情接口
        ''' </summary>
        ''' <param name="symbol"></param>
        ''' <param name="limit"></param>
        ''' <returns></returns>
        Public Function GetMarkDepth(symbol As String, limit As Integer) As UserType.Contract.ReplyType.MarkDepth

            '创建参数对象
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("symbol", symbol)
            param.AddParam("limit", CType(limit, Integer))

            UserAPI.MarketDepth.Param = param

            Dim ret As UserType.Contract.ReplyType.MarkDepth = UserAPI.MarketDepth.Value(Of UserType.Contract.ReplyType.MarkDepth)
            Return ret

        End Function

        Public Function GetMarkTickers(productType As String) As UserType.Contract.ReplyType.MarkTickers
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("productType", productType)
            UserAPI.MarketTickers.Param = param

            Dim ret As UserType.Contract.ReplyType.MarkTickers = UserAPI.MarketTickers.Value(Of UserType.Contract.ReplyType.MarkTickers)

            Return ret
        End Function

        ''' <summary>
        ''' 单个Ticker行情获取
        ''' </summary>
        ''' <param name="symbol"></param>
        ''' <returns></returns>
        Public Function GetMarkTicker(symbol As String) As UserType.Contract.ReplyType.MarkTicker
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("symbol", symbol)
            UserAPI.MarketTicker.Param = param

            Dim ret As UserType.Contract.ReplyType.MarkTicker = UserAPI.MarketTicker.Value(Of UserType.Contract.ReplyType.MarkTicker)

            Return ret
        End Function



        Public Function GetMarkContracts(productType As String) As UserType.Contract.ReplyType.MarketContracts

            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("productType", productType)
            UserAPI.MarketContracts.Param = param

            Dim ret As UserType.Contract.ReplyType.MarketContracts = UserAPI.MarketContracts.Value(Of UserType.Contract.ReplyType.MarketContracts)

            Return ret

        End Function

        Public Function GetMarketCandles(symbol As String，
                                        granularity As String,
                                        startTime As String,
                                        endTime As String,
                                        Optional kLineType As String = "market",
                                        Optional limit As String = "100") As UserType.Contract.ReplyType.MarketCandles

            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("symbol", symbol)
            param.AddParam("granularity", granularity)
            param.AddParam("startTime", startTime)
            param.AddParam("endTime", endTime)
            param.AddParam("kLineType", kLineType)
            param.AddParam("limit", limit)
            UserAPI.MarketCandles.Param = param

            '48746505


            Dim ret As New UserType.Contract.ReplyType.MarketCandles With {
                .code = "0",
                .data = New Api.UserType.Contract.ReplyType.MarketCandles.DataType With {
                        .data = UserAPI.MarketCandles.Value(Of List(Of List(Of String)))},
                .msg = ""}

            Return ret

        End Function



#End Region


#Region "账户接口"
        ''' <summary>
        ''' 用户信息
        ''' </summary>
        ''' <param name="productType"></param>
        ''' <returns></returns>
        Public Function GetAccountAccounts(productType As String) As UserType.Contract.ReplyType.AccountAccounts

            '创建参数对象
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("productType", productType)

            UserAPI.AccountsAccounts.Param = param

            Dim ret As UserType.Contract.ReplyType.AccountAccounts = UserAPI.AccountsAccounts.Value(Of UserType.Contract.ReplyType.AccountAccounts)

            Return ret

        End Function
#End Region



#Region "交易接口"

        ''' <summary>
        ''' 下单
        ''' </summary>
        ''' <param name="p"></param>
        ''' <returns></returns>
        Public Function OrderPlaceOrder(p As UserType.Contract.ParamType.OrderPlaceOrder) As UserType.Contract.ReplyType.OrderPlaceOrder

            UserAPI.OrderPlaceOrder.Param = p

            Dim ret = UserAPI.OrderPlaceOrder.Value(Of UserType.Contract.ReplyType.OrderPlaceOrder)

            Return ret

        End Function

        ''' <summary>
        ''' 批量下单
        ''' </summary>
        ''' <param name="p"></param>
        ''' <returns></returns>
        Public Function OrderBatchOrders(p As UserType.Contract.ParamType.OrderBatchOrders) As UserType.Contract.ReplyType.OrderBatchOrders

            UserAPI.OrderBatchOrders.Param = p

            Dim ret = UserAPI.OrderBatchOrders.Value(Of UserType.Contract.ReplyType.OrderBatchOrders)

            Return ret

        End Function


        ''' <summary>
        ''' 获取当前委托
        ''' </summary>
        ''' <param name="symbol"></param>
        ''' <returns></returns>
        Public Function GetOrderCurrent(symbol As String) As UserType.Contract.ReplyType.OrderCurrent

            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '参数写入参数字典
            param.AddParam("symbol", symbol)

            UserAPI.OrderCurrent.Param = param

            Dim ret As UserType.Contract.ReplyType.OrderCurrent = UserAPI.OrderCurrent.Value(Of UserType.Contract.ReplyType.OrderCurrent)

            Return ret

        End Function

        Public Function GetOrderDetail(symbol As String, orderId As String) As UserType.Contract.ReplyType.OrderDetail

            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '参数写入参数字典
            param.AddParam("symbol", symbol)
            param.AddParam("orderId", orderId)

            UserAPI.OrderDetail.Param = param

            Dim ret As UserType.Contract.ReplyType.OrderDetail = UserAPI.OrderDetail.Value(Of UserType.Contract.ReplyType.OrderDetail)

            Return ret

        End Function




        ''' <summary>
        ''' 撤销所有委托
        ''' </summary>
        ''' <param name="productType"></param>
        ''' <param name="marginCoin"></param>
        ''' <returns></returns>
        Public Function OrderCancelAllOrders(ByVal productType As String, ByVal marginCoin As String) As UserType.Contract.ReplyType.OrderCancelAllOrders

            Dim p As New UserType.Contract.ParamType.CancelAllOrders With {
                .productType = productType,
                .marginCoin = marginCoin
            }

            UserAPI.OrderCancelAllOrders.Param = p

            Dim ret As UserType.Contract.ReplyType.OrderCancelAllOrders = UserAPI.OrderCancelAllOrders.Value(Of UserType.Contract.ReplyType.OrderCancelAllOrders)


            If ret.code <> 0 Then Debug.Print(ret.msg)

            Return ret
        End Function







#End Region



        Public Function TraceCurrentTrack(symbol As String, productType As String, pageSize As Integer, pageNo As Integer) As UserType.Contract.ReplyType.TraceCurrentTrack

            '创建参数对象
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("symbol", symbol)
            param.AddParam("productType", productType)
            param.AddParam("pageSize", pageSize)
            param.AddParam("pageNo", pageNo)


            UserAPI.TraceCurrentTrack.Param = param

            Dim ret As UserType.Contract.ReplyType.TraceCurrentTrack = UserAPI.TraceCurrentTrack.Value(Of UserType.Contract.ReplyType.TraceCurrentTrack)

            Return ret

        End Function










    End Class
End Namespace



