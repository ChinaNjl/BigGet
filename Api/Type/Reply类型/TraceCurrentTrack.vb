
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.Unicode

Namespace UserType.ReplyType

    Public Class TraceCurrentTrack

        Public Property code As String
        Public Property msg As String
        Public Property requestTime As Long
        Public Property data As List(Of Datum)
        Public Class Datum
            Public Property trackingNo As String
            Public Property openOrderId As String
            Public Property closeOrderId As Object
            Public Property symbol As String
            Public Property holdSide As String
            Public Property openLeverage As Integer
            Public Property openAvgPrice As Single
            Public Property openTime As Long
            Public Property openDealCount As String
            Public Property stopProfitPrice As Single
            Public Property stopLossPrice As Object
        End Class

        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me, New JsonSerializerOptions With {
                                                           .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                           .Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)})
            Return s

        End Function

    End Class





End Namespace