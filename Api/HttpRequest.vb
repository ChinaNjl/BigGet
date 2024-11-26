Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json
Imports Api.Api.RestApiAddress

Public Class HttpRequest

    Private Property UserKeyInfo As UserKeyInfo
    Private Property Api As ApiType

    Private Signer As Signer

#Region "构造函数"

    Sub New(_UserKeyInfo As UserKeyInfo, _api As Api.RestApiAddress.ApiType)
        Api = _api
        UserKeyInfo = _UserKeyInfo
        Signer = New Signer With {.Secret_Key = UserKeyInfo.Secretkey}
    End Sub

#End Region

#Region "公共方法"

    ''' <summary>
    ''' 无参数
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    Public Function Post(url As String) As String
        Dim request As HttpWebRequest = WebRequest.Create(url)
        With request
            .Method = "POST"
            .ContentType = "application/json"
            request.Headers.Add("ACCESS-KEY", UserKeyInfo.ApiKey)
            request.Headers.Add("ACCESS-SIGN", Signer.Sign(TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), Api.Method, Api.Address, ""))
            request.Headers.Add("ACCESS-TIMESTAMP", TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            request.Headers.Add("ACCESS-PASSPHRASE", UserKeyInfo.Passphrase)
            request.Headers.Add("locale", "zh-CN")
            .Timeout = 20000
        End With

        Try
            Using response As HttpWebResponse = request.GetResponse()
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim strResult As String = reader.ReadToEnd()
                    Return strResult
                End Using
            End Using
        Catch ex As Exception
            Dim strResult As String = "{""code"":""99999"",""msg"":""{0}""}"
            strResult = strResult.Replace("{0}"， ex.Message)
            Return strResult
        End Try
    End Function

    ''' <summary>
    ''' 有参数
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    Public Function Post(url As String, data As String) As String

        Dim request As HttpWebRequest = WebRequest.Create(url)
        With request
            .Method = "POST"
            .ContentType = "application/json"
            request.Headers.Add("ACCESS-KEY", UserKeyInfo.ApiKey)
            request.Headers.Add("ACCESS-SIGN", Signer.Sign(TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), Api.Method, Api.Address, data))
            request.Headers.Add("ACCESS-TIMESTAMP", TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            request.Headers.Add("ACCESS-PASSPHRASE", UserKeyInfo.Passphrase)
            request.Headers.Add("locale", "zh-CN")
            .Timeout = 20000
        End With

        Dim byteData As Byte() = Encoding.UTF8.GetBytes(data)
        request.ContentLength = byteData.Length
        Dim requestStream As Stream = request.GetRequestStream()
        requestStream.Write(byteData, 0, byteData.Length)
        requestStream.Close()

        Try
            Using response As HttpWebResponse = request.GetResponse()
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim strResult As String = reader.ReadToEnd()
                    Return strResult
                End Using
            End Using
        Catch ex As Exception
            Dim strResult As String = "{""code"":""99999"",""msg"":""{0}""}"
            strResult = strResult.Replace("{0}"， ex.Message)
            Return strResult
        End Try

    End Function

    ''' <summary>
    ''' 无参数的Get
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    Public Function GetWebApi(ByVal url As String) As String

        Dim request As HttpWebRequest = WebRequest.Create(url)
        With request
            .Method = "GET"
            .ContentType = "application/json"
            .Headers.Add("ACCESS-KEY", UserKeyInfo.ApiKey)
            .Headers.Add("ACCESS-SIGN", Signer.Sign(TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), Api.Method, Api.Address, ""))
            .Headers.Add("ACCESS-TIMESTAMP", TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            .Headers.Add("ACCESS-PASSPHRASE", UserKeyInfo.Passphrase)
            .Headers.Add("locale", "zh-CN")
            .Timeout = 20000
        End With

        Try
            Using response As HttpWebResponse = request.GetResponse()
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim strResult As String = reader.ReadToEnd()
                    'Debug.Print(strResult)
                    Return strResult
                End Using
            End Using
        Catch ex As Exception
            Dim strResult As String = "{""code"":""99999"",""msg"":""{0}""}"
            strResult = strResult.Replace("{0}"， ex.Message)
            Return strResult
        End Try

    End Function

    ''' <summary>
    ''' 有参数的Get
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    Public Function GetWebApi(ByVal url As String， ByVal strParam As String) As String

        Dim request As HttpWebRequest = WebRequest.Create(url)
        With request
            .Method = "GET"
            .ContentType = "application/json"
            .Headers.Add("ACCESS-KEY", UserKeyInfo.ApiKey)
            .Headers.Add("ACCESS-SIGN", Signer.Sign(TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), Api.Method, Api.Address, strParam))
            .Headers.Add("ACCESS-TIMESTAMP", TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            .Headers.Add("ACCESS-PASSPHRASE", UserKeyInfo.Passphrase)
            .Headers.Add("locale", "zh-CN")
            .Timeout = 20000
        End With

        Try
            Using response As HttpWebResponse = request.GetResponse()
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim strResult As String = reader.ReadToEnd()
                    Return strResult
                End Using
            End Using
        Catch ex As Exception
            Dim strResult As String = "{""code"":""99999"",""msg"":""{0}""}"
            strResult = strResult.Replace("{0}"， ex.Message)
            Return strResult
        End Try

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region

#Region "私有方法"

    ''' <summary>
    ''' json反序列化
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="r"></param>
    ''' <returns></returns>
    Friend Function JsonStrToObject(Of T)(ByVal r As String)
        Dim str As String = r
        Try

            Dim weatherForecast = JsonSerializer.Deserialize(Of T)(Encoding.UTF8.GetBytes(str))

            Return weatherForecast
        Catch ex As Exception
            Dim ret As String = "{""code"":""9998"",""msg"":""{0}""}"
            ret = ret.Replace("{0}"， "序列化错误")
            Return ret
        End Try

    End Function

#End Region

#Region "弃用方法"

    ' HttpClient 每个只需要实例一次，不需要对每次应用实例化，容易产生错误
    Public Property httpclient As HttpClient = New HttpClient() With {
                .Timeout = New TimeSpan(0, 0, 10)}

    Public Function PostAsync(Of T)(url As String, Optional body As String = Nothing, Optional mediaTyp As String = "application/json")
        Dim t1 As Date
        Try
            t1 = Date.UtcNow
            Dim httpContent As StringContent

            If String.IsNullOrEmpty(body) Then
                httpContent = Nothing
            Else
                httpContent = New StringContent(body, Encoding.UTF8, mediaTyp)
            End If

            Dim response = httpclient.PostAsync(url, httpContent)
            '````````````````````
            '默认异步调用改为同步调用
            Dim rep = response.Result
            Dim response2 = rep.Content.ReadAsStringAsync()
            Dim str = response2.Result

            Return JsonStrToObject(Of T)(str)
        Catch ex As Exception
            Dim err As String = "错误代码：" & Chr(10) & ex.ToString & Chr(10) &
                                        "错误时间:" & Date.Now & Chr(10) &
                                        "URL：" & url & Chr(10)

            Dim ret As String = "{""code"":""99999"",""msg"":""{0}""}"
            ret = ret.Replace("{0}"， ex.Message & (Date.UtcNow - t1).TotalMilliseconds)
            Dim obj = JsonStrToObject(Of T)(ret)

            Return obj

        End Try

    End Function

    Friend Function GetData(Of T)(URI As String)
        '在 try/catch语句块中调用异步网络方法并进行错误处理
        'Dim response As HttpResponseMessage = Nothing
        Dim TimeStart As Date = Date.UtcNow
        Try

            Dim response As HttpResponseMessage = httpclient.GetAsync(URI).Result

            response.EnsureSuccessStatusCode()
            Dim responseBody As String = response.Content.ReadAsStringAsync().Result

            Return JsonStrToObject(Of T)(responseBody)
        Catch ex As Exception
            Console.WriteLine(Environment.NewLine & "Exception Caught!")
            Console.WriteLine("Message :{0} ", ex.Message)

            httpclient.CancelPendingRequests()

            Dim ret As String = "{""code"":""99999"",""msg"":""API访问超时 time={0}""}"
            ret = ret.Replace("{0}"， ex.Message & (Date.UtcNow - TimeStart).TotalMilliseconds)
            Dim obj = JsonStrToObject(Of T)(ret)

            Return obj
        End Try

    End Function

#End Region

End Class