Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.Unicode

Namespace Api.Request.Contract.ReplyType

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

        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me, New JsonSerializerOptions With {
                                                           .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                           .Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)})
            Return s

        End Function

    End Class

End Namespace