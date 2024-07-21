Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.Unicode

Namespace Api.Request.Contract.ReplyType

    Public Class OrderDetail
        Public Property code As String
        Public Property msg As String
        Public Property requestTime As Long
        Public Property data As DataType

        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me, New JsonSerializerOptions With {
                                                           .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                           .Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)})
            Return s

        End Function

        Public Class DataType
            Public Property symbol As String
            Public Property size As Single
            Public Property orderId As String
            Public Property clientOid As String
            Public Property filledQty As Single
            Public Property fee As Single
            Public Property price As Object
            Public Property priceAvg As Single
            Public Property state As String
            Public Property side As String
            Public Property timeInForce As String
            Public Property totalProfits As Single
            Public Property posSide As String
            Public Property marginCoin As String
            Public Property presetTakeProfitPrice As Single
            Public Property filledAmount As Single
            Public Property orderType As String
            Public Property leverage As String
            Public Property marginMode As String
            Public Property reduceOnly As Boolean
            Public Property enterPointSource As String
            Public Property tradeSide As String
            Public Property holdMode As String
            Public Property orderSource As String
            Public Property cTime As String
            Public Property uTime As String
        End Class

    End Class

End Namespace