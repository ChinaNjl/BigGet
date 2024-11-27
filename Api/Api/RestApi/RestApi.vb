Imports System.Threading.Thread

Namespace Api.RestApiAddress

    ''' <summary>
    ''' 合约api信息
    ''' </summary>
    Public Class AddressContractAPI

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

            Public Shared OrderDetail As New ApiType With {
                .Method = "GET",
                .Address = "/api/mix/v1/order/detail",
                .Count = 10,
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

    End Class

    ''' <summary>
    ''' 现货api信息
    ''' </summary>
    Public Class AddressSpotAPI

        Public Class Account

            Public Shared getInfo As New ApiType With {
                .Method = "GET",
                .Address = "/api/spot/v1/account/getInfo",
                .Count = 1,
                .Time = 1
            }

            Public Shared assets As New ApiType With {
                .Method = "GET",
                .Address = "/api/spot/v1/account/assets",
                .Count = 10,
                .Time = 1
            }

            Public Shared assets_lite As New ApiType With {
                .Method = "GET",
                .Address = "/api/spot/v1/account/assets-lite",
                .Count = 10,
                .Time = 1
            }

            Public Shared sub_account_spot_assets As New ApiType With {
                .Method = "POST",
                .Address = "/api/spot/v1/account/sub-account-spot-assets",
                .Count = 1,
                .Time = 10
            }

            Public Shared bills As New ApiType With {
                .Method = "POST",
                .Address = "/api/spot/v1/account/bills",
                .Count = 10,
                .Time = 1
            }

            Public Shared transferRecords As New ApiType With {
                .Method = "GET",
                .Address = "/api/spot/v1/account/transferRecords",
                .Count = 20,
                .Time = 1
            }

        End Class

        ''' <summary>
        ''' 市场类型api
        ''' </summary>
        Public Class Market

            ''' <summary>
            ''' 全部tickets的信息
            ''' </summary>
            Public Shared Tickers As New ApiType With {
                .Method = "GET",
                .Address = "/api/spot/v1/market/tickers",
                .Count = 20,
                .Time = 1
            }

        End Class

        ''' <summary>
        ''' 订单类型api
        ''' </summary>
        Public Class Trade

            ''' <summary>
            ''' 现货下单
            ''' </summary>
            Public Shared orders As New ApiType With {
                .Method = "POST",
                .Address = "/api/spot/v1/trade/orders",
                .Count = 10,
                .Time = 1
            }

        End Class

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
        ''' 单位时间秒
        ''' </summary>
        ''' <returns></returns>
        Public Property Time As Integer

        Private Property que As New Queue(Of Int64)

        Public Sub Add()
            '将本次调用的时间记录到队列中
            Dim ts As TimeSpan = Date.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
            que.Enqueue(CType(ts.TotalMilliseconds, Int64))

            '当队列元素超过count的时候删除队列的第一个元素
            If que.Count > Count Then
                que.Dequeue()
            End If
        End Sub

        Public Sub Delay()

            If que.Count = Count Then
                Dim ts As TimeSpan = Date.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                '当前时间-队列第一个元素的时间=c
                Dim c As Int64 = ts.TotalMilliseconds - que.Peek
                If c <= Time * 1000 Then
                    Debug.Print("延迟：{0}     队列:{1}", Math.Abs(c - Time * 1000), que.Count)
                    que.Dequeue()
                    Sleep(Math.Abs(c - Time * 1000) + 100)
                End If

            End If
        End Sub

    End Class

End Namespace