Namespace Api.Request.Contract.ReplyType

    Public Class AccountAccounts
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
            Public Property marginCoin As String
            Public Property locked As String
            Public Property available As String
            Public Property crossMaxAvailable As String

            Public Property fixedMaxAvailable As String

            Public Property maxTransferOut As String
            Public Property equity As String

            Public Property usdtEquity As String
            Public Property btcEquity As String

        End Class

    End Class

End Namespace