


Imports System.Text.Json

Namespace UserType.ReplyType
    Public Class MarketCandles
        Public Property code As String
        Public Property data As DataType
        Public Property msg As String






        Public Class DataType

            ''' <summary>
            ''' k线数据，按时间先后顺序排序。最新的k线在最后面
            ''' </summary>
            ''' <returns></returns>
            Public Property data As List(Of List(Of String))

            ''' <summary>
            ''' 返回最新币价
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property NewPrice As String
                Get
                    Return data.Last.Item(4)
                End Get
            End Property

            ''' <summary>
            ''' 返回k线数量
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property Count As Integer
                Get
                    Return data.Count
                End Get
            End Property


            ''' <summary>
            ''' MA（包含最新价）
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property AveragePrice As Single
                Get
                    Dim results As Single = 0

                    For Each tmp As List(Of String) In data
                        results = CType(tmp.Item(1), Single) + results
                    Next

                    Return results / Count
                End Get
            End Property


            ''' <summary>
            ''' MA(不包含最新价)
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property AveragePrice1 As Single
                Get
                    Dim results As Single = 0

                    For Each tmp As List(Of String) In data
                        results = CType(tmp.Item(1), Single) + results
                    Next

                    results = results - CType(NewPrice, Single)

                    Return results / (Count - 1)

                End Get
            End Property




        End Class



        Public Function ToJson() As String
            Dim s As String = JsonSerializer.Serialize(Me)

            Return s

        End Function

    End Class





End Namespace





