


Imports System.Text.Json

Namespace ParamType

    Public Class OrderPlaceOrder

        Public Property symbol As String
        Public Property marginCoin As String
        Public Property size As String
        Public Property side As String
        Public Property orderType As String



        '非必须参数
        Public Property price As String
        Public Property timeInForceValue As String
        Public Property clientOid As String
        Public Property presetTakeProfitPrice As String
        Public Property presetStopLossPrice As String



        Public Function ToJson() As String

            Dim s As String = JsonSerializer.Serialize(Me)

            s = s.Replace("""price"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""timeInForceValue"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""clientOid"":null", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""presetTakeProfitPrice"":null,", "")
            s = s.Replace(",,", ",")
            s = s.Replace("""presetStopLossPrice"":null", "")
            s = s.Replace(",,", ",")

            s = s.Replace("{,", "{")
            s = s.Replace(",}", "}")

            Return s

        End Function



    End Class

End Namespace