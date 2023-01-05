


Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.Unicode

Namespace UserType.ReplyType


    Public Class OrderCancelAllOrders
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

            Public Property order_ids
                Get
                    Return _order_ids.ToArray
                End Get
                Set(value)
                    Dim jsonArray As JsonElement = value
                    For Each a In jsonArray.EnumerateArray
                        _order_ids.Add(a.GetString)
                    Next

                End Set
            End Property
            Dim _order_ids As New List(Of String)

            Public Property fail_infos As fail_infosType()



            Public Class fail_infosType

                Public Property order_id As String
                Public Property err_code As String
                Public Property err_msg As String

            End Class

        End Class


        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me, New JsonSerializerOptions With {
                                                           .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                           .Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)})
            Return s

        End Function


    End Class


End Namespace