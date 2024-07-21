Imports System.Text.Json

Namespace Api.Request.Contract.ReplyType

    Public Class MarketContracts

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

            Public Property baseCoin As String
            Public Property buyLimitPriceRatio As String
            Public Property feeRateUpRatio As String
            Public Property makerFeeRate As String
            Public Property minTradeNum As String
            Public Property openCostUpRatio As String
            Public Property priceEndStep As String
            Public Property pricePlace As String
            Public Property quoteCoin As String
            Public Property sellLimitPriceRatio As String
            Public Property sizeMultiplier As String

            Public Property supportMarginCoins
                Get
                    Return _supportMarginCoins.ToArray
                End Get
                Set(value)
                    Dim jsonArray As JsonElement = value
                    For Each a In jsonArray.EnumerateArray
                        _supportMarginCoins.Add(a.GetString)
                    Next
                End Set
            End Property

            Dim _supportMarginCoins As New List(Of String)

            Public Property symbol As String
            Public Property takerFeeRate As String
            Public Property volumePlace As String
            Public Property symbolType As String

        End Class

    End Class

End Namespace