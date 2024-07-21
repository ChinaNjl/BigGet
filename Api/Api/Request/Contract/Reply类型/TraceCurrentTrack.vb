Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.Unicode

Namespace Api.Request.Contract.ReplyType

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

        Public Function FindStopProfitPrice(p_stopProfitPrice As Single) As Boolean
            _FindStopProfitPrice = p_stopProfitPrice
            Dim ret = data.Find(AddressOf BoolStopProfitPrice)

            Return Not IsNothing(ret)
        End Function

        Dim _FindStopProfitPrice As Single

        Private Function BoolStopProfitPrice(obj As Datum) As Boolean
            Return obj.stopProfitPrice = _FindStopProfitPrice
        End Function

        Public Function MinStopProfitPrice() As Single

            If data.Count > 0 Then

                Dim min As Single = 0
                Dim i As Integer = 1
                For Each d In data
                    If i = 1 Then
                        min = d.stopProfitPrice
                    Else
                        If min > d.stopProfitPrice Then
                            min = d.stopProfitPrice
                        End If
                    End If
                    i = i + 1
                Next

                Return min
            Else
                Return 0
            End If

        End Function

        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me, New JsonSerializerOptions With {
                                                           .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                           .Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)})
            Return s

        End Function

    End Class

End Namespace