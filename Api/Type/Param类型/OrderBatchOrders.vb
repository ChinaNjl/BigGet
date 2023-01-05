Imports System.Text.Json

Namespace UserType.ParamType

    Public Class OrderBatchOrders

        Public Property symbol As String
        Public Property marginCoin As String
        Public Property orderDataList As New List(Of orderData)

        Public Class orderData
            Public Property size As String
            Public Property price As String
            Public Property side As String
            Public Property orderType As String
            Public Property timeInForceValue As String
            Public Property clientOid As String
            Public Property presetTakeProfitPrice As String
            Public Property presetStopLossPrice As String
        End Class


        Public Function ToJson() As String

            Dim s As String = JsonSerializer.Serialize(Me)

            s = s.Replace("""price"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""timeInForceValue"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""clientOid"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""presetTakeProfitPrice"":null,", "")
            s = s.Replace("""presetTakeProfitPrice"":"""",", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""presetStopLossPrice"":null", "")
            s = s.Replace("""presetStopLossPrice"":""""", "")
            s = s.Replace(",,", ",")

            s = s.Replace("{,", "{")
            s = s.Replace(",}", "}")

            Return s

        End Function





    End Class




End Namespace




