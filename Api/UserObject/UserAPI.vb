
Public Class UserAPI

    Sub New(_UserInfo As UserInfo)
        UserInfo = _UserInfo
    End Sub

    Private UserInfo As UserInfo


    Public ReadOnly Property AccountsAccounts As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_AccountsAccounts) = True Then
                _AccountsAccounts = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Accounts.Accounts)
            End If
            Return _AccountsAccounts
        End Get
    End Property
    Dim _AccountsAccounts As UserObject.ApiObject.ApiObject

    Public ReadOnly Property MarketDepth As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_MarketDepth) = True Then
                _MarketDepth = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Market.Depth)
            End If
            Return _MarketDepth
        End Get
    End Property
    Dim _MarketDepth As UserObject.ApiObject.ApiObject

    ''' <summary>
    ''' 全部Ticker行情获取
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property MarketTickers As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_MarketTickers) = True Then
                _MarketTickers = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Market.Tickers)
            End If
            Return _MarketTickers
        End Get
    End Property
    Dim _MarketTickers As UserObject.ApiObject.ApiObject

    ''' <summary>
    ''' 单个Ticker行情获取
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property MarketTicker As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_MarketTicker) = True Then
                _MarketTicker = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Market.Ticker)
            End If
            Return _MarketTicker
        End Get
    End Property
    Dim _MarketTicker As UserObject.ApiObject.ApiObject


    Public ReadOnly Property MarketContracts As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_MarketContracts) = True Then
                _MarketContracts = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Market.Contracts)
            End If
            Return _MarketContracts
        End Get
    End Property
    Dim _MarketContracts As UserObject.ApiObject.ApiObject

    ''' <summary>
    ''' 获取k线数据
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property MarketCandles As UserObject.ApiObject.ApiObject

        Get

            If IsNothing(_MarketCandles) = True Then
                _MarketCandles = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Market.Candles)
            End If
            Return _MarketCandles
        End Get

    End Property
    Dim _MarketCandles As UserObject.ApiObject.ApiObject

    Public ReadOnly Property OrderPlaceOrder As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_OrderPlaceOrder) = True Then
                _OrderPlaceOrder = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Order.PlaceOrder)
            End If
            Return _OrderPlaceOrder
        End Get
    End Property
    Dim _OrderPlaceOrder As UserObject.ApiObject.ApiObject

    Public ReadOnly Property OrderCancelAllOrders As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_OrderCancelAllOrders) = True Then
                _OrderCancelAllOrders = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Order.CancelAllOrders)
            End If
            Return _OrderCancelAllOrders
        End Get
    End Property
    Dim _OrderCancelAllOrders As UserObject.ApiObject.ApiObject

    Public ReadOnly Property OrderBatchOrders As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_OrderBatchOrders) = True Then
                _OrderBatchOrders = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Order.OrderBatchOrders)
            End If
            Return _OrderBatchOrders
        End Get
    End Property
    Dim _OrderBatchOrders As UserObject.ApiObject.ApiObject

    Public ReadOnly Property OrderCurrent As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_OrderCurrent) = True Then
                _OrderCurrent = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Order.OrderCurrent)
            End If
            Return _OrderCurrent
        End Get
    End Property
    Dim _OrderCurrent As UserObject.ApiObject.ApiObject

    Public ReadOnly Property TraceCurrentTrack As UserObject.ApiObject.ApiObject
        Get
            If IsNothing(_TraceCurrentTrack) = True Then
                _TraceCurrentTrack = New UserObject.ApiObject.ApiObject(UserInfo, UserObject.ApiObject.RestApi.Trace.CurrentTrack)
            End If
            Return _TraceCurrentTrack
        End Get
    End Property
    Dim _TraceCurrentTrack As UserObject.ApiObject.ApiObject


End Class
