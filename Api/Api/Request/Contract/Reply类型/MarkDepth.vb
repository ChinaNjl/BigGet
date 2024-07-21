Imports System.Text.Json

Namespace Api.Request.Contract.ReplyType

    Public Class MarkDepth

        Public Property code As String
        Public Property data As DataType
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

            Public Property asks
                Get
                    Return _asks.ToArray
                End Get
                Set(value)

                    Dim jsonArray As JsonElement = value

                    For Each a In jsonArray.EnumerateArray
                        _asks.Add({a(0).GetString, a(1).GetString})
                    Next
                End Set
            End Property

            Dim _asks As New List(Of Array)

            Public Property bids
                Get
                    Return _bids.ToArray
                End Get
                Set(value)
                    Dim jsonArray As JsonElement = value

                    For Each a In jsonArray.EnumerateArray
                        _bids.Add({a(0).GetString, a(1).GetString})
                    Next
                End Set
            End Property

            Dim _bids As New List(Of Array)

        End Class

    End Class

End Namespace