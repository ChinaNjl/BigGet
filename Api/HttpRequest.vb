Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports System.Text.Json
Imports Api.Api.RestApiAddress

Public Class HttpRequest

    Private client As HttpClient = New HttpClient()
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
    ''' HttpClient GET
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="data">参数</param>
    ''' <returns></returns>
    Public Function GetHttpClient(url As String, Optional data As String = "") As String
        '初始化client头部信息
        With client.DefaultRequestHeaders
            .Clear()
            .Add("ACCESS-KEY", UserKeyInfo.ApiKey)
            .Add("ACCESS-SIGN", Signer.Sign(TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), Api.Method, Api.Address, data))
            .Add("ACCESS-TIMESTAMP", TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            .Add("ACCESS-PASSPHRASE", UserKeyInfo.Passphrase)
            .Add("locale", "zh-CN")
        End With

        Try
            '发送请求
            Using response As HttpResponseMessage = client.GetAsync(url).Result
                '获取结果
                Dim result As String = response.Content.ReadAsStringAsync().Result
                Return result
            End Using
        Catch ex As Exception
            Dim result As String = "{""code"":""99999"",""msg"":""{0}""}"
            result = result.Replace("{0}"， ex.Message)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' 无参post
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    Public Function PostHttpClient(url As String) As String

        '初始化client头部信息
        With client.DefaultRequestHeaders
            .Clear()
            .Add("ACCESS-KEY", UserKeyInfo.ApiKey)
            .Add("ACCESS-SIGN", Signer.Sign(TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), Api.Method, Api.Address, ""))
            .Add("ACCESS-TIMESTAMP", TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            .Add("ACCESS-PASSPHRASE", UserKeyInfo.Passphrase)
            .Add("locale", "zh-CN")
        End With

        Try
            '发送请求
            Using response As HttpResponseMessage = client.PostAsync(url, Nothing).Result
                '获取结果
                Dim result As String = response.Content.ReadAsStringAsync().Result

                Return result

            End Using
        Catch ex As Exception
            Dim result As String = "{""code"":""99999"",""msg"":""{0}""}"
            result = result.Replace("{0}"， ex.Message)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' 有参post
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Public Function PostHttpClient(url As String, data As String) As String

        '初始化client头部信息
        With client.DefaultRequestHeaders
            .Clear()
            .Add("ACCESS-KEY", UserKeyInfo.ApiKey)
            .Add("ACCESS-SIGN", Signer.Sign(TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), Api.Method, Api.Address, data))
            .Add("ACCESS-TIMESTAMP", TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            .Add("ACCESS-PASSPHRASE", UserKeyInfo.Passphrase)
            .Add("locale", "zh-CN")
        End With

        Try
            '添加参数
            Using content As HttpContent = New StringContent(data, Encoding.UTF8, "application/json")
                '发送请求
                Using response As HttpResponseMessage = client.PostAsync(url, content).Result
                    '获取结果
                    Dim result As String = response.Content.ReadAsStringAsync().Result
                    Return result

                End Using

            End Using
        Catch ex As Exception
            Dim result As String = "{""code"":""99999"",""msg"":""{0}""}"
            result = result.Replace("{0}"， ex.Message)
            Return result
        End Try

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region

#Region "私有方法"

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
    ''' 有参数
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    Public Function Post123(url As String, data As String) As String

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

        '添加post参数
        Dim byteData As Byte() = Encoding.UTF8.GetBytes(data)
        request.ContentLength = byteData.Length
        Using reqStream As Stream = request.GetRequestStream()
            reqStream.Write(byteData, 0, byteData.Length)
            reqStream.Close()
        End Using

        Try
            Using response As HttpWebResponse = request.GetResponse()
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim strResult As String = reader.ReadToEnd()
                    Return strResult
                End Using
            End Using
        Catch ex As Exception
            Dim rep As String = ex.Message
            Console.WriteLine(rep)
            Return rep
        End Try

    End Function

    Public Function PostHttpClient1(url As String, Optional body As String = Nothing, Optional mediaTyp As String = "application/json")

        Try
            With client.DefaultRequestHeaders
                .Add("ACCESS-KEY", UserKeyInfo.ApiKey)
                .Add("ACCESS-SIGN", Signer.Sign(TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), Api.Method, Api.Address, body))
                .Add("ACCESS-TIMESTAMP", TimeZoneInfo.ConvertTimeToUtc(Now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                .Add("ACCESS-PASSPHRASE", UserKeyInfo.Passphrase)
                .Add("locale", "zh-CN")
            End With

            Dim httpContent As StringContent = New StringContent(body, Encoding.UTF8, mediaTyp)
            Dim response = client.PostAsync(url, httpContent)
            '````````````````````
            '默认异步调用改为同步调用
            Dim rep = response.Result
            Dim response2 = rep.Content.ReadAsStringAsync()
            Dim str = response2.Result
            Debug.Print(str)

            Return str
        Catch ex As Exception
            Dim err As String = "错误代码：" & Chr(10) & ex.ToString & Chr(10) &
                                        "错误时间:" & Date.Now & Chr(10) &
                                        "URL：" & url & Chr(10)

            Dim ret As String = "{""code"":""99999"",""msg"":""{0}""}"
            ret = ret.Replace("{0}"， ex.Message)

            Return ret

        End Try

    End Function

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

End Class