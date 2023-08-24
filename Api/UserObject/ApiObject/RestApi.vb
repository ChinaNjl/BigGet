

Namespace UserObject.ApiObject
    Public Class RestApi

        Public Class Accounts

            Public Shared Accounts As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/account/accounts",
                .Count = 20,
                .Time = 1
            }

        End Class

        Public Class Market

            Public Shared Depth As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/market/depth",
                .Count = 20,
                .Time = 1
            }


            Public Shared Tickers As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/market/tickers",
                .Count = 20,
                .Time = 1
            }

            ''' <summary>
            ''' 单个Ticker行情获取
            ''' </summary>
            Public Shared Ticker As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/market/ticker",
                .Count = 20,
                .Time = 1
            }

            Public Shared Contracts As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/market/contracts",
                .Count = 20,
                .Time = 1
            }

            Public Shared Candles As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/market/candles",
                .Count = 20,
                .Time = 1
            }

        End Class

        Public Class Order

            Public Shared PlaceOrder As New ApiType With {
                .Method = "POST",
                .Address = "/api/mix/v1/order/placeOrder",
                .Count = 10,
                .Time = 1
            }

            Public Shared CancelAllOrders As New ApiType With {
                .Method = "POST",
                .Address = "/api/mix/v1/order/cancel-all-orders",
                .Count = 10,
                .Time = 1
            }

            Public Shared OrderBatchOrders As New ApiType With {
                .Method = "POST",
                .Address = "/api/mix/v1/order/batch-orders",
                .Count = 10,
                .Time = 1
            }

            Public Shared OrderCurrent As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/order/current",
                .Count = 20,
                .Time = 1
            }



        End Class


        Public Class Trace
            Public Shared CurrentTrack As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/trace/currentTrack",
                .Count = 10,
                .Time = 1
            }




        End Class



        Public Class ApiType

            ''' <summary>
            ''' api地址
            ''' </summary>
            ''' <returns></returns>
            Public Property Address As String

            ''' <summary>
            ''' get or post
            ''' </summary>
            ''' <returns></returns>
            Public Property Method As String
                Get
                    Return _Method
                End Get
                Set(value As String)
                    _Method = value.ToUpper
                End Set
            End Property
            Dim _Method As String

            ''' <summary>
            ''' 访问频率
            ''' </summary>
            ''' <returns></returns>
            Public Property Count As Integer

            ''' <summary>
            ''' 单位时间
            ''' </summary>
            ''' <returns></returns>
            Public Property Time As Integer

            ''' <summary>
            ''' api访问间隔
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property Delay As Integer
                Get
                    Return Time * 1000 / Count
                End Get
            End Property


        End Class

    End Class
End Namespace



