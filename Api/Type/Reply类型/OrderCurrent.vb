Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.Unicode


Namespace UserType.ReplyType

    Public Class OrderCurrent

        Public Property code As String
        Public Property data As DataType()
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

            Public Property symbol As String
            Public Property size
                Get
                    Return _size
                End Get
                Set(value)
                    _size = value.ToString
                End Set
            End Property
            Dim _size As String

            Public Property orderId As String
            Public Property clientOid As String
            Public Property filledQty
                Get
                    Return _filledQty
                End Get
                Set(value)
                    _filledQty = value.ToString
                End Set
            End Property
            Dim _filledQty As String

            Public Property fee
                Get
                    Return _fee
                End Get
                Set(value)
                    _fee = value.ToString
                End Set
            End Property
            Dim _fee As String

            Public Property price
                Get
                    Return _price
                End Get
                Set(value)
                    If IsDBNull(value) = False And IsNothing(value) = False Then
                        _price = value.ToString
                    Else
                        _price = 0
                    End If

                End Set
            End Property
            Dim _price As String

            Public Property state As String

            Public Property side As String

            Public Property timeInForce As String

            Public Property totalProfits
                Get
                    Return _totalProfits
                End Get
                Set(value)
                    _totalProfits = value.ToString
                End Set
            End Property
            Dim _totalProfits As String

            Public Property posSide As String
            Public Property marginCoin As String

            Public Property orderType As String

            Public Property cTime
                Get
                    Return _cTime
                End Get
                Set(value)
                    _cTime = value.ToString
                End Set
            End Property
            Dim _cTime As String




        End Class


        Public Function Count() As Integer
            Return data.Count
        End Function


        Public Function FindOrderType(side As String) As Integer

            Dim c As Integer

            For Each d In data

                If d.side = side Then
                    c = c + 1
                End If


            Next

            Return c

        End Function

        Public Function FindOrderType(side As String, price As String) As Integer

            Dim c As Integer

            For Each d In data

                If d.side = side And CType(d.price, Single).ToString() = CType(price, Single).ToString Then
                    c = c + 1
                End If

            Next

            Return c

        End Function


        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me, New JsonSerializerOptions With {
                                                           .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                           .Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)})
            Return s

        End Function



    End Class

End Namespace




