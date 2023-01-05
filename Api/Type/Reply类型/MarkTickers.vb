


Namespace UserType.ReplyType


    Public Class MarkTickers


        Public Property code As String
        Public Property data As DataType()
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

            Public Property symbol As String
            Public Property last As String
            Public Property bestAsk As String
            Public Property bestBid As String
            Public Property bidSz As String
            Public Property askSz As String
            Public Property high24h As String
            Public Property low24h As String
            Public Property timestamp As String
            Public Property priceChangePercent As String
            Public Property baseVolume As String
            Public Property quoteVolume As String
            Public Property usdtVolume As String
            Public Property openUtc As String
            Public Property chgUtc As String

            Public Function toList() As List(Of String)
                Dim lst As New List(Of String) From {
                   symbol, last, bestAsk, bestBid, bidSz, askSz, high24h, low24h, timestamp, priceChangePercent, baseVolume, quoteVolume, usdtVolume, openUtc, chgUtc
                }



                Return lst
            End Function

        End Class


    End Class

End Namespace