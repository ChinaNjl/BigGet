Imports System.Threading.Thread
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Text.Json

Namespace Api.RestApi

    Public Class ApiObject

#Region "内部属性变量和对象"

        Private UserInfo As UserKeyInfo
        Private Api As Api.RestApiAddress.ApiType

        Private HttpRequest As HttpRequest
        Private que As New Queue(Of Int64)

#End Region

#Region "外部属性变量和对象"

        ''' <summary>
        ''' 保存最近一次调用api返回类型的json值
        ''' </summary>
        ''' <returns></returns>
        Public Property strLastResualt As String

        ''' <summary>
        ''' 访问参数
        ''' </summary>
        ''' <returns></returns>
        Public Property Param As Object

#End Region

#Region "公有方法"

        Sub New(_UserInfo As UserKeyInfo, _Api As Api.RestApiAddress.ApiType)
            UserInfo = _UserInfo
            Api = _Api

            HttpRequest = New HttpRequest(_UserInfo, _Api)
        End Sub

        Public Function Value(Of T)()

            '当访问频率过高的时候执行延迟
            Api.Delay()

            Dim _UrlBuilder As New PublicUrlBuilder With {
                    .Host = UserInfo.Host,
                    ._Api = Api}
            Dim resualt As String = ""   '保存api返回值

            Select Case Api.Method
                Case "GET"

                    '根据有无参数进行处理
                    If IsNothing(Param) Then
                        Dim url As String = _UrlBuilder.Build()
                        strLastResualt = HttpRequest.GetWebApi(url)
                    Else
                        Dim url As String = _UrlBuilder.Build(Param)
                        strLastResualt = HttpRequest.GetWebApi(url, Param.BuildParams)
                    End If

                Case "POST"

                    '根据有无参数进行处理
                    If IsNothing(Param) Then
                        '无参数post
                        Dim url As String = _UrlBuilder.Build()
                        strLastResualt = HttpRequest.Post(url)
                    Else
                        '带参数post
                        Dim url As String = _UrlBuilder.Build()
                        strLastResualt = HttpRequest.Post(url, Param.toJson)
                    End If

                Case Else

            End Select

            '将访问时间保存到que队列的末尾,超过最大数量，则删除开头的一次记录
            Api.Add()

            Param = Nothing '初始化param参数
            Return JsonStrToObject(Of T)(strLastResualt)

        End Function

#End Region

#Region "私有方法"

        ''' <summary>
        ''' json反序列化
        ''' </summary>
        ''' <param name="json"></param>
        ''' <returns></returns>
        Friend Function JsonStrToObject(Of T)(ByVal json As String)

            Try
                'Dim weatherForecast = JsonSerializer.Deserialize(Of T)(json)
                Dim weatherForecast = JsonSerializer.Deserialize(Of T)(Encoding.UTF8.GetBytes(json))
                Return weatherForecast
            Catch ex As Exception
                Dim ret As String = "{""code"":""9998"",""msg"":""{0}""}"
                ret = ret.Replace("{0}"， ex.Message)
                Return JsonSerializer.Deserialize(Of T)(Encoding.UTF8.GetBytes(ret))
            End Try

        End Function

#End Region

    End Class

End Namespace