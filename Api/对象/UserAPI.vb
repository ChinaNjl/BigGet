
Public Class UserAPI

    Sub New(_UserInfo As UserInfo)
        UserInfo = _UserInfo
    End Sub

    Private UserInfo As UserInfo


    Public ReadOnly Property AccountsAccounts As RestApi.ApiObject
        Get
            If IsNothing(_AccountsAccounts) = True Then
                _AccountsAccounts = New RestApi.ApiObject(UserInfo, RestApi.Accounts.Accounts)
            End If
            Return _AccountsAccounts
        End Get
    End Property
    Dim _AccountsAccounts As RestApi.ApiObject

    Public ReadOnly Property MarketDepth As RestApi.ApiObject
        Get
            If IsNothing(_MarketDepth) = True Then
                _MarketDepth = New RestApi.ApiObject(UserInfo, RestApi.Market.Depth)
            End If
            Return _MarketDepth
        End Get
    End Property
    Dim _MarketDepth As RestApi.ApiObject


    Public ReadOnly Property MarketTickers As RestApi.ApiObject
        Get
            If IsNothing(_MarketTickers) = True Then
                _MarketTickers = New RestApi.ApiObject(UserInfo, RestApi.Market.Tickers)
            End If
            Return _MarketTickers
        End Get
    End Property
    Dim _MarketTickers As RestApi.ApiObject


    Public ReadOnly Property MarketContracts As RestApi.ApiObject
        Get
            If IsNothing(_MarketContracts) = True Then
                _MarketContracts = New RestApi.ApiObject(UserInfo, RestApi.Market.Contracts)
            End If
            Return _MarketContracts
        End Get
    End Property
    Dim _MarketContracts As RestApi.ApiObject



    Public ReadOnly Property OrderPlaceOrder As RestApi.ApiObject
        Get
            If IsNothing(_OrderPlaceOrder) = True Then
                _OrderPlaceOrder = New RestApi.ApiObject(UserInfo, RestApi.Order.PlaceOrder)
            End If
            Return _OrderPlaceOrder
        End Get
    End Property
    Dim _OrderPlaceOrder As RestApi.ApiObject

    Public ReadOnly Property OrderCancelAllOrders As RestApi.ApiObject
        Get
            If IsNothing(_OrderCancelAllOrders) = True Then
                _OrderCancelAllOrders = New RestApi.ApiObject(UserInfo, RestApi.Order.CancelAllOrders)
            End If
            Return _OrderCancelAllOrders
        End Get
    End Property
    Dim _OrderCancelAllOrders As RestApi.ApiObject

    Public ReadOnly Property OrderBatchOrders As RestApi.ApiObject
        Get
            If IsNothing(_OrderBatchOrders) = True Then
                _OrderBatchOrders = New RestApi.ApiObject(UserInfo, RestApi.Order.OrderBatchOrders)
            End If
            Return _OrderBatchOrders
        End Get
    End Property
    Dim _OrderBatchOrders As RestApi.ApiObject

    Public ReadOnly Property OrderCurrent As RestApi.ApiObject
        Get
            If IsNothing(_OrderCurrent) = True Then
                _OrderCurrent = New RestApi.ApiObject(UserInfo, RestApi.Order.OrderCurrent)
            End If
            Return _OrderCurrent
        End Get
    End Property
    Dim _OrderCurrent As RestApi.ApiObject


End Class
