Imports System.Net.Http
Imports System.Text
Imports System.Text.Json


Public Class HttpRequest

    ' HttpClient 每个只需要实例一次，不需要对每次应用实例化，容易产生错误
    Public httpclient As HttpClient = New HttpClient() With {
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

            ' Debug.Print(str)
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

        Dim response As HttpResponseMessage = Nothing
        Dim t1 As Date
        Try
            t1 = Date.UtcNow
            response = httpclient.GetAsync(URI).Result
            response.EnsureSuccessStatusCode()
            Dim responseBody As String = response.Content.ReadAsStringAsync().Result
            'Dim responseBody As String = httpclient.GetStringAsync(URI).Result
            'Debug.Print(URI)
            'Debug.Print(responseBody)
            Return JsonStrToObject(Of T)(responseBody)

        Catch ex As Exception
            Console.WriteLine(Environment.NewLine & "Exception Caught!")
            Console.WriteLine("Message :{0} ", ex.Message)


            httpclient.CancelPendingRequests()

            Dim ret As String = "{""code"":""99999"",""msg"":""API访问超时 time={0}""}"
            ret = ret.Replace("{0}"， ex.Message & (Date.UtcNow - t1).TotalMilliseconds)
            Dim obj = JsonStrToObject(Of T)(ret)



            Return obj
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
            ' Dim weatherForecast = JsonSerializer.Deserialize(Of List(Of List(Of String)))(Encoding.UTF8.GetBytes(str))
            Debug.Print("HttpRequest.JsonStrToObject错误:{1}"， ex.Message)
            Return Nothing
        End Try

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class


