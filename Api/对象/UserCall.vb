


Imports System.Reflection

Public Class UserCall
    Sub New(_UserInfo As UserInfo)
        UserAPI = New UserAPI(_UserInfo)
    End Sub

    Private UserAPI As UserAPI


    ''' <summary>
    ''' 深度行情接口
    ''' </summary>
    ''' <param name="symbol"></param>
    ''' <param name="limit"></param>
    ''' <returns></returns>
    Public Function GetMarkDepth(symbol As String, limit As Integer) As UserType.ReplyType.MarkDepth

        '创建参数对象
        Dim param As New GetRequestParamType(Nothing)

        '写入参数字典
        param.AddParam("symbol", symbol)
        param.AddParam("limit", CType(limit, Integer))

        UserAPI.MarketDepth.Param = param

        Dim ret As UserType.ReplyType.MarkDepth = UserAPI.MarketDepth.Value(Of UserType.ReplyType.MarkDepth)
        Return ret

    End Function



    Public Function GetMarkTickers(productType As String) As UserType.ReplyType.MarkTickers
        Dim param As New GetRequestParamType(Nothing)

        '写入参数字典
        param.AddParam("productType", productType)
        UserAPI.MarketTickers.Param = param

        Dim ret As UserType.ReplyType.MarkTickers = UserAPI.MarketTickers.Value(Of UserType.ReplyType.MarkTickers)

        Return ret
    End Function

    Public Function GetMarkContracts(productType As String) As UserType.ReplyType.MarketContracts

        Dim param As New GetRequestParamType(Nothing)

        '写入参数字典
        param.AddParam("productType", productType)
        UserAPI.MarketContracts.Param = param

        Dim ret As UserType.ReplyType.MarketContracts = UserAPI.MarketContracts.Value(Of UserType.ReplyType.MarketContracts)

        Return ret

    End Function

    Public Function GetOrderCurrent(symbol As String) As UserType.ReplyType.OrderCurrent
        Dim param As New GetRequestParamType(Nothing)

        '写入参数字典
        param.AddParam("symbol", symbol)
        UserAPI.OrderCurrent.Param = param


        Dim ret As UserType.ReplyType.OrderCurrent = UserAPI.OrderCurrent.Value(Of UserType.ReplyType.OrderCurrent)

        Return ret

    End Function






    ''' <summary>
    ''' 用户信息
    ''' </summary>
    ''' <param name="productType"></param>
    ''' <returns></returns>
    Public Function GetAccountAccounts(productType As String) As UserType.ReplyType.AccountAccounts

        '创建参数对象
        Dim param As New GetRequestParamType(Nothing)

        '写入参数字典
        param.AddParam("productType", productType)

        UserAPI.AccountsAccounts.Param = param

        Dim ret As UserType.ReplyType.AccountAccounts = UserAPI.AccountsAccounts.Value(Of UserType.ReplyType.AccountAccounts)

        Return ret

    End Function


    ''' <summary>
    ''' 下单
    ''' </summary>
    ''' <param name="p"></param>
    ''' <returns></returns>
    Public Function OrderPlaceOrder(p As UserType.ParamType.OrderPlaceOrder) As UserType.ReplyType.OrderPlaceOrder

        UserAPI.OrderPlaceOrder.Param = p

        Dim ret = UserAPI.OrderPlaceOrder.Value(Of UserType.ReplyType.OrderPlaceOrder)

        Return ret

    End Function


    Public Function OrderBatchOrders(p As UserType.ParamType.OrderBatchOrders) As UserType.ReplyType.OrderBatchOrders

        UserAPI.OrderBatchOrders.Param = p

        Dim ret = UserAPI.OrderBatchOrders.Value(Of UserType.ReplyType.OrderBatchOrders)

        Return ret

    End Function






    Public Function OrderCancelAllOrders(ByVal productType As String, ByVal marginCoin As String) As UserType.ReplyType.OrderCancelAllOrders

        Dim p As New UserType.ParamType.CancelAllOrders With {
            .productType = productType,
            .marginCoin = marginCoin
        }

        UserAPI.OrderCancelAllOrders.Param = p

        Dim ret As UserType.ReplyType.OrderCancelAllOrders = UserAPI.OrderCancelAllOrders.Value(Of UserType.ReplyType.OrderCancelAllOrders)


        If ret.code <> 0 Then Debug.Print(ret.msg)

        Return ret
    End Function




End Class
