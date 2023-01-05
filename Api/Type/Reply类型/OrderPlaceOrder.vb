
Namespace UserType.ReplyType

    Public Class OrderPlaceOrder
        Public Property code As String
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

        Public Property data As DataType


        Public Class DataType

            Public Property orderId As String
            Public Property clientOid As String

        End Class

    End Class



End Namespace