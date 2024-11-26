Imports System.Text

Public Class PublicUrlBuilder

    Public Property _Api As Api.RestApiAddress.ApiType
    ReadOnly _Setting As UserKeyInfo
    Public Property Host As String

    ''' <summary>
    ''' 没有参数的公共url
    ''' </summary>
    ''' <returns></returns>
    Friend Function Build() As String

        Dim str As New StringBuilder

        str.Append("https://")
        str.Append(Host)
        str.Append(_Api.Address)

        Return str.ToString

    End Function

    Friend Function Build(ByVal p_Param As GetRequestParamType) As String

        Dim str As New StringBuilder

        str.Append("https://")
        str.Append(Host)
        str.Append(_Api.Address)
        str.Append("?")
        str.Append(p_Param.BuildParams)

        Return str.ToString

    End Function

End Class