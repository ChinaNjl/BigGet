Imports System.Threading.Thread
Imports System.Net.Http.Headers

Namespace Api.RestApi

    Public Class ApiObject

        Private Property UserInfo As UserKeyInfo
        Private Property Api As Api.RestApi.ApiType

        Private ReadOnly Property Signer As UserObject.OtherObject.Signer
            Get
                If IsNothing(_Signer) = True Then
                    _Signer = New UserObject.OtherObject.Signer With {.Secret_Key = UserInfo.Secretkey}
                End If

                Return _Signer
            End Get
        End Property

        Dim _Signer As UserObject.OtherObject.Signer

        Private HttpRequest As New UserObject.OtherObject.HttpRequest

        Sub New(_UserInfo As UserKeyInfo, _Api As Api.RestApi.ApiType)
            UserInfo = _UserInfo
            Api = _Api
        End Sub

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

        ''' <summary>
        ''' 设置头信息
        ''' </summary>
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

        Private Property que As New Queue(Of Int64)

        Private ReadOnly Property delay As Integer
            Get
                If _delay = -1 Then
                    _delay = Api.Time * 1000 / Api.Count
                End If

                Return _delay
            End Get
        End Property

        Dim _delay As Integer = -1

        Public Function Value(Of T)()

            Dim _UrlBuilder As New UserObject.OtherObject.PublicUrlBuilder With {
                .Host = UserInfo.Host,
                ._Api = Api}
            Dim url As String

            '当访问频率过高的时候执行延迟
            If que.Count = Api.Count Then
                Dim c As Int64 = que.Last - que.Peek
                If c <= Api.Time Then
                    Sleep(Api.Time - c + 10)
                End If
            End If

            If Api.Method = "GET" Then

                If IsNothing(Param) = False Then
                    url = _UrlBuilder.Build(Param)
                Else
                    url = _UrlBuilder.Build()
                End If

                Call HttpRequestHeader()
                resualt = HttpRequest.GetData(Of T)(url)

                Return resualt

            End If

            If Api.Method = "POST" Then

                url = _UrlBuilder.Build()

                Call HttpRequestHeader()
                resualt = HttpRequest.PostAsync(Of T)(url, Param.toJson)

                Return resualt

            End If

            '将访问时间保存到que队列的末尾,超过最大数量，则删除开头的一次记录
            Dim ts As TimeSpan = Date.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
            que.Enqueue(CType(ts.TotalMilliseconds, Int64))
            If que.Count > Api.Count Then
                que.Dequeue()
            End If

            Return Nothing
        End Function

    End Class

End Namespace