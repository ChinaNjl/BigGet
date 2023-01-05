Imports System.Net.Http.Headers

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

        Public Shared Contracts As New ApiType With {
            .Method = "GET",
            .Address = "/api/mix/v1/market/contracts",
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







    Public Class ApiObject

        Sub New(_UserInfo As UserInfo, _Api As ApiType)
            UserInfo = _UserInfo
            Api = _Api
        End Sub


        Private UserInfo As UserInfo
        Private Api As ApiType

        Private ReadOnly Property Signer As Signer
            Get
                If IsNothing(_Signer) = True Then
                    _Signer = New Signer With {.Secret_Key = UserInfo.Secretkey}
                End If

                Return _Signer
            End Get
        End Property
        Dim _Signer As Signer

        Private HttpRequest As New HttpRequest


        ''' <summary>
        ''' 保存最近一次调用api的返回值
        ''' </summary>
        ''' <returns></returns>
        Public Property resualt
            Get
                Return _resualt
            End Get
            Set(value)
                _resualt = value
            End Set
        End Property
        Dim _resualt


        ''' <summary>
        ''' 访问参数
        ''' </summary>
        ''' <returns></returns>
        Public Property Param
            Get
                Return _temp_Param
            End Get
            Set(value)
                _temp_Param = value
            End Set
        End Property
        Dim _temp_Param As Object = Nothing

        Private Sub HttpRequestHeader()
            Dim UtcTime = TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            Dim ts As TimeSpan = Date.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
            UtcTime = CType(ts.TotalMilliseconds, Int64)

            HttpRequest.httpclient.DefaultRequestHeaders.Clear()

            HttpRequest.httpclient.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
            HttpRequest.httpclient.DefaultRequestHeaders.Add("ACCESS-KEY", UserInfo.ApiKey)
            HttpRequest.httpclient.DefaultRequestHeaders.Add("ACCESS-TIMESTAMP", UtcTime)
            HttpRequest.httpclient.DefaultRequestHeaders.Add("ACCESS-PASSPHRASE", UserInfo.Passphrase)
            HttpRequest.httpclient.DefaultRequestHeaders.Add("locale", "zh-CN")

            If Api.Method = "GET" Then

                '对有无参数进行处理
                If IsNothing(Param) = True Then
                    '无参数
                    HttpRequest.httpclient.DefaultRequestHeaders.Add("ACCESS-SIGN", Signer.Sign(UtcTime, Api.Method, Api.Address, ""))
                Else
                    '有参数
                    HttpRequest.httpclient.DefaultRequestHeaders.Add("ACCESS-SIGN", Signer.Sign(UtcTime, Api.Method, Api.Address, Param.BuildParams))
                End If

            End If

            If Api.Method = "POST" Then
                '对有无参数进行处理
                If IsNothing(Param) = True Then
                    '无参数
                    HttpRequest.httpclient.DefaultRequestHeaders.Add("ACCESS-SIGN", Signer.Sign(UtcTime, Api.Method, Api.Address, ""))
                Else
                    '有参数
                    HttpRequest.httpclient.DefaultRequestHeaders.Add("ACCESS-SIGN", Signer.Sign(UtcTime, Api.Method, Api.Address, Param.tojson))
                End If
            End If


        End Sub


        Public Function Value(Of T)()

            Dim _UrlBuilder As New PublicUrlBuilder With {
                .Host = UserInfo.Host,
                ._Api = Api}

            Dim url As String



            If Api.Method = "GET" Then

                If IsNothing(Param) = False Then
                    url = _UrlBuilder.Build(Param)
                Else
                    url = _UrlBuilder.Build()
                End If


                Call HttpRequestHeader()
                _resualt = HttpRequest.GetData(Of T)(url)
                'err.ErrorCodeToDataBase(_resualt)

                'Sleep(_Api.Int_Time * 1000 / _Api.Int_Count)
                'Delay()
                Return _resualt

            End If

            If Api.Method = "POST" Then

                url = _UrlBuilder.Build()

                Call HttpRequestHeader()
                _resualt = HttpRequest.PostAsync(Of T)(url, Param.toJson)
                'err.ErrorCodeToDataBase(_resualt)

                'Sleep(_Api.Int_Time * 1000 / _Api.Int_Count)
                'Delay()
                Return _resualt

            End If


            Return Nothing
        End Function




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

