Namespace Api.RestApi

    ''' <summary>
    ''' BigGetApi封装成对象
    ''' </summary>
    Public Class UserAPI

#Region "内部变量和对象"

        Private UserInfo As UserKeyInfo
        Dim _ContractAccountsAccounts As Api.RestApi.ApiObject
        Dim _ContractMarketDepth As Api.RestApi.ApiObject
        Dim _ContractMarketTickers As Api.RestApi.ApiObject
        Dim _ContractMarketTicker As Api.RestApi.ApiObject
        Dim _ContractMarketContracts As Api.RestApi.ApiObject
        Dim _ContractMarketCandles As Api.RestApi.ApiObject
        Dim _ContractOrderPlaceOrder As Api.RestApi.ApiObject
        Dim _ContractOrderCancelAllOrders As Api.RestApi.ApiObject
        Dim _ContractOrderBatchOrders As Api.RestApi.ApiObject
        Dim _ContractOrderDetail As Api.RestApi.ApiObject
        Dim _ContractOrderCurrent As Api.RestApi.ApiObject
        Dim _ContractTraceCurrentTrack As Api.RestApi.ApiObject
        Dim _SpotAccountGetInfo As Api.RestApi.ApiObject
        Dim _SpotAccountAssets As Api.RestApi.ApiObject
        Dim _SpotAccountAssetsLite As Api.RestApi.ApiObject
        Dim _SpotAccountSubAccountSpotAssets As Api.RestApi.ApiObject
        Dim _SpotAccountBills As Api.RestApi.ApiObject
        Dim _SpotAccountTransferRecords As Api.RestApi.ApiObject
        Dim _SpotMarketTickers As Api.RestApi.ApiObject
        Dim _SpotTradeOrders As Api.RestApi.ApiObject

#End Region

#Region "通用方法"

        ''' <summary>
        ''' 对象初始化
        ''' </summary>
        ''' <param name="_UserInfo"></param>
        Sub New(_UserInfo As UserKeyInfo)
            UserInfo = _UserInfo
        End Sub

#End Region

#Region "属性（现货）"

        ''' <summary>
        ''' 现货下单接口
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SpotTradeOrders As Api.RestApi.ApiObject
            Get
                If IsNothing(_SpotTradeOrders) = True Then
                    _SpotTradeOrders = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressSpotAPI.Trade.orders)
                End If
                Return _SpotTradeOrders
            End Get
        End Property

        ''' <summary>
        ''' 获取划转记录
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SpotAccountTransferRecords As Api.RestApi.ApiObject
            Get
                If IsNothing(_SpotAccountTransferRecords) = True Then
                    _SpotAccountTransferRecords = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressSpotAPI.Account.transferRecords)
                End If
                Return _SpotAccountTransferRecords
            End Get
        End Property

        ''' <summary>
        ''' 获取全部标的的信息
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SpotMarketTickers As Api.RestApi.ApiObject
            Get
                If IsNothing(_SpotMarketTickers) = True Then
                    _SpotMarketTickers = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressSpotAPI.Market.Tickers)
                End If
                Return _SpotMarketTickers
            End Get
        End Property

        ''' <summary>
        ''' 获取账单流水
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SpotAccountBills As Api.RestApi.ApiObject
            Get
                If IsNothing(_SpotAccountBills) = True Then
                    _SpotAccountBills = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressSpotAPI.Account.bills)
                End If
                Return _SpotAccountBills
            End Get
        End Property

        ''' <summary>
        ''' 获取所有子账户现货资产
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SpotAccountSubAccountSpotAssets As Api.RestApi.ApiObject
            Get
                If IsNothing(_SpotAccountSubAccountSpotAssets) = True Then
                    _SpotAccountSubAccountSpotAssets = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressSpotAPI.Account.sub_account_spot_assets)
                End If
                Return _SpotAccountSubAccountSpotAssets
            End Get
        End Property

        ''' <summary>
        ''' 获取有价账户资产
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SpotAccountAssetsLite As Api.RestApi.ApiObject
            Get
                If IsNothing(_SpotAccountAssetsLite) = True Then
                    _SpotAccountAssetsLite = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressSpotAPI.Account.assets_lite)
                End If
                Return _SpotAccountAssetsLite
            End Get
        End Property

        ''' <summary>
        ''' 获取ApiKey信息
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SpotAccountGetInfo As Api.RestApi.ApiObject
            Get
                If IsNothing(_SpotAccountGetInfo) = True Then
                    _SpotAccountGetInfo = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressSpotAPI.Account.getInfo)
                End If
                Return _SpotAccountGetInfo
            End Get
        End Property

        ''' <summary>
        ''' 获取账户资产
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SpotAccountAssets As Api.RestApi.ApiObject
            Get
                If IsNothing(_SpotAccountAssets) = True Then
                    _SpotAccountAssets = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressSpotAPI.Account.assets)
                End If
                Return _SpotAccountAssets
            End Get
        End Property

#End Region

#Region "属性（合约）"

        Public ReadOnly Property ContractAccountsAccounts As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractAccountsAccounts) = True Then
                    _ContractAccountsAccounts = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Accounts.Accounts)
                End If
                Return _ContractAccountsAccounts
            End Get
        End Property

        Public ReadOnly Property ContractMarketDepth As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractMarketDepth) = True Then
                    _ContractMarketDepth = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Market.Depth)
                End If
                Return _ContractMarketDepth
            End Get
        End Property

        ''' <summary>
        ''' 全部Ticker行情获取
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ContractMarketTickers As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractMarketTickers) = True Then
                    _ContractMarketTickers = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Market.Tickers)
                End If
                Return _ContractMarketTickers
            End Get
        End Property

        ''' <summary>
        ''' 单个Ticker行情获取
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ContractMarketTicker As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractMarketTicker) = True Then
                    _ContractMarketTicker = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Market.Ticker)
                End If
                Return _ContractMarketTicker
            End Get
        End Property

        Public ReadOnly Property ContractMarketContracts As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractMarketContracts) = True Then
                    _ContractMarketContracts = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Market.Contracts)
                End If
                Return _ContractMarketContracts
            End Get
        End Property

        ''' <summary>
        ''' 获取k线数据
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ContractMarketCandles As Api.RestApi.ApiObject

            Get

                If IsNothing(_ContractMarketCandles) = True Then
                    _ContractMarketCandles = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Market.Candles)
                End If
                Return _ContractMarketCandles
            End Get

        End Property

        Public ReadOnly Property ContractOrderPlaceOrder As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractOrderPlaceOrder) = True Then
                    _ContractOrderPlaceOrder = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Order.PlaceOrder)
                End If
                Return _ContractOrderPlaceOrder
            End Get
        End Property

        ''' <summary>
        ''' 取消所有订单
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ContractOrderCancelAllOrders As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractOrderCancelAllOrders) = True Then
                    _ContractOrderCancelAllOrders = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Order.CancelAllOrders)
                End If
                Return _ContractOrderCancelAllOrders
            End Get
        End Property

        ''' <summary>
        ''' 批量下单
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ContractOrderBatchOrders As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractOrderBatchOrders) = True Then
                    _ContractOrderBatchOrders = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Order.OrderBatchOrders)
                End If
                Return _ContractOrderBatchOrders
            End Get
        End Property

        ''' <summary>
        ''' 订单详情
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ContractOrderDetail As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractOrderDetail) = True Then
                    _ContractOrderDetail = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Order.OrderDetail)
                End If
                Return _ContractOrderDetail
            End Get
        End Property

        Public ReadOnly Property ContractOrderCurrent As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractOrderCurrent) = True Then
                    _ContractOrderCurrent = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Order.OrderCurrent)
                End If
                Return _ContractOrderCurrent
            End Get
        End Property

        Public ReadOnly Property ContractTraceCurrentTrack As Api.RestApi.ApiObject
            Get
                If IsNothing(_ContractTraceCurrentTrack) = True Then
                    _ContractTraceCurrentTrack = New Api.RestApi.ApiObject(UserInfo, Api.RestApiAddress.AddressContractAPI.Trace.CurrentTrack)
                End If
                Return _ContractTraceCurrentTrack
            End Get
        End Property

#End Region

    End Class

End Namespace