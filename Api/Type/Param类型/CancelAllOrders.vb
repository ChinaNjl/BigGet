Imports System.Text.Json




Namespace UserType.ParamType
    Public Class CancelAllOrders


        Public Property productType As String
        Public Property marginCoin As String
            Get
                Return _marginCoin.ToUpper
            End Get
            Set(value As String)
                _marginCoin = value
            End Set
        End Property
        Dim _marginCoin As String


        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me)

            Return s

        End Function


    End Class

End Namespace



