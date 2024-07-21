Imports System.Reflection
Imports System.Text.Json
Imports System.Threading

Namespace User

    ''' <summary>
    ''' 用户调用api对象
    ''' </summary>
    Public Class UserCall

#Region "内部变量和对象"

        Private UserAPI As Api.RestApi.UserAPI

#End Region

#Region "通用接口"

        Sub New(_UserInfo As UserKeyInfo)
            UserAPI = New Api.RestApi.UserAPI(_UserInfo)
        End Sub

#End Region

#Region "行情接口"

        ''' <summary>
        ''' 深度行情接口
        ''' </summary>
        ''' <param name="symbol"></param>
        ''' <param name="limit"></param>
        ''' <returns></returns>
        Public Function ContractGetMarkDepth(symbol As String, limit As Integer) As Api.Request.Contract.ReplyType.MarkDepth

            '创建参数对象
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("symbol", symbol)
            param.AddParam("limit", CType(limit, Integer))

            UserAPI.ContractMarketDepth.Param = param

            Dim ret As Api.Request.Contract.ReplyType.MarkDepth = UserAPI.ContractMarketDepth.Value(Of Api.Request.Contract.ReplyType.MarkDepth)
            Return ret

        End Function

        Public Function ContractGetMarkTickers(productType As String) As Api.Request.Contract.ReplyType.MarkTickers
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("productType", productType)
            UserAPI.ContractMarketTickers.Param = param

            Dim ret As Api.Request.Contract.ReplyType.MarkTickers = UserAPI.ContractMarketTickers.Value(Of Api.Request.Contract.ReplyType.MarkTickers)

            Return ret
        End Function

        ''' <summary>
        ''' 单个Ticker行情获取
        ''' </summary>
        ''' <param name="symbol"></param>
        ''' <returns></returns>
        Public Function ContractGetMarkTicker(symbol As String) As Api.Request.Contract.ReplyType.MarkTicker
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("symbol", symbol)
            UserAPI.ContractMarketTicker.Param = param

            Dim ret As Api.Request.Contract.ReplyType.MarkTicker = UserAPI.ContractMarketTicker.Value(Of Api.Request.Contract.ReplyType.MarkTicker)

            Return ret
        End Function

        Public Function ContractGetMarkContracts(productType As String) As Api.Request.Contract.ReplyType.MarketContracts

            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("productType", productType)
            UserAPI.ContractMarketContracts.Param = param

            Dim ret As Api.Request.Contract.ReplyType.MarketContracts = UserAPI.ContractMarketContracts.Value(Of Api.Request.Contract.ReplyType.MarketContracts)

            Return ret

        End Function

        Public Function ContractGetMarketCandles(symbol As String，
                                        granularity As String,
                                        startTime As String,
                                        endTime As String,
                                        Optional kLineType As String = "market",
                                        Optional limit As String = "100") As Api.Request.Contract.ReplyType.MarketCandles

            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("symbol", symbol)
            param.AddParam("granularity", granularity)
            param.AddParam("startTime", startTime)
            param.AddParam("endTime", endTime)
            param.AddParam("kLineType", kLineType)
            param.AddParam("limit", limit)
            UserAPI.ContractMarketCandles.Param = param

            Dim ret As New Api.Request.Contract.ReplyType.MarketCandles With {
                .code = "0",
                .data = New Api.Request.Contract.ReplyType.MarketCandles.DataType With {
                        .data = UserAPI.ContractMarketCandles.Value(Of List(Of List(Of String)))},
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
        Public Function ContractGetAccountAccounts(productType As String) As Api.Request.Contract.ReplyType.AccountAccounts

            '创建参数对象
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("productType", productType)

            UserAPI.ContractAccountsAccounts.Param = param

            Dim ret As Api.Request.Contract.ReplyType.AccountAccounts = UserAPI.ContractAccountsAccounts.Value(Of Api.Request.Contract.ReplyType.AccountAccounts)

            Return ret

        End Function

        ''' <summary>
        ''' 获取ApiKey信息
        ''' </summary>
        ''' <returns></returns>
        Public Function SpotGetAccountGetInfo() As Api.Request.Spot.ReplyType.AccountGetInfo
            Return Nothing
        End Function

#End Region

#Region "交易接口"

        ''' <summary>
        ''' 下单
        ''' </summary>
        ''' <param name="p"></param>
        ''' <returns></returns>
        Public Function ContractOrderPlaceOrder(p As Api.Request.Contract.ParamType.OrderPlaceOrder) As Api.Request.Contract.ReplyType.OrderPlaceOrder

            UserAPI.ContractOrderPlaceOrder.Param = p

            Dim ret = UserAPI.ContractOrderPlaceOrder.Value(Of Api.Request.Contract.ReplyType.OrderPlaceOrder)

            Return ret

        End Function

        ''' <summary>
        ''' 批量下单
        ''' </summary>
        ''' <param name="p"></param>
        ''' <returns></returns>
        Public Function ContractOrderBatchOrders(p As Api.Request.Contract.ParamType.OrderBatchOrders) As Api.Request.Contract.ReplyType.OrderBatchOrders

            UserAPI.ContractOrderBatchOrders.Param = p

            Dim ret = UserAPI.ContractOrderBatchOrders.Value(Of Api.Request.Contract.ReplyType.OrderBatchOrders)

            Return ret

        End Function

        ''' <summary>
        ''' 获取当前委托
        ''' </summary>
        ''' <param name="symbol"></param>
        ''' <returns></returns>
        Public Function ContractGetOrderCurrent(symbol As String) As Api.Request.Contract.ReplyType.OrderCurrent

            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '参数写入参数字典
            param.AddParam("symbol", symbol)

            UserAPI.ContractOrderCurrent.Param = param

            Dim ret As Api.Request.Contract.ReplyType.OrderCurrent = UserAPI.ContractOrderCurrent.Value(Of Api.Request.Contract.ReplyType.OrderCurrent)

            Return ret

        End Function

        Public Function ContractGetOrderDetail(symbol As String, orderId As String) As Api.Request.Contract.ReplyType.OrderDetail

            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '参数写入参数字典
            param.AddParam("symbol", symbol)
            param.AddParam("orderId", orderId)

            UserAPI.ContractOrderDetail.Param = param

            Dim ret As Api.Request.Contract.ReplyType.OrderDetail = UserAPI.ContractOrderDetail.Value(Of Api.Request.Contract.ReplyType.OrderDetail)

            Return ret

        End Function

        ''' <summary>
        ''' 撤销所有委托
        ''' </summary>
        ''' <param name="productType"></param>
        ''' <param name="marginCoin"></param>
        ''' <returns></returns>
        Public Function ContractOrderCancelAllOrders(ByVal productType As String, ByVal marginCoin As String) As Api.Request.Contract.ReplyType.OrderCancelAllOrders

            Dim p As New Api.Request.Contract.ParamType.CancelAllOrders With {
                .productType = productType,
                .marginCoin = marginCoin
            }

            UserAPI.ContractOrderCancelAllOrders.Param = p

            Dim ret As Api.Request.Contract.ReplyType.OrderCancelAllOrders = UserAPI.ContractOrderCancelAllOrders.Value(Of Api.Request.Contract.ReplyType.OrderCancelAllOrders)

            If ret.code <> 0 Then Debug.Print(ret.msg)

            Return ret
        End Function

        Public Function ContractTraceCurrentTrack(symbol As String, productType As String, pageSize As Integer, pageNo As Integer) As Api.Request.Contract.ReplyType.TraceCurrentTrack

            '创建参数对象
            Dim param As New UserObject.OtherObject.GetRequestParamType(Nothing)

            '写入参数字典
            param.AddParam("symbol", symbol)
            param.AddParam("productType", productType)
            param.AddParam("pageSize", pageSize)
            param.AddParam("pageNo", pageNo)

            UserAPI.ContractTraceCurrentTrack.Param = param

            Dim ret As Api.Request.Contract.ReplyType.TraceCurrentTrack = UserAPI.ContractTraceCurrentTrack.Value(Of Api.Request.Contract.ReplyType.TraceCurrentTrack)

            Return ret

        End Function

#End Region

    End Class

End Namespace