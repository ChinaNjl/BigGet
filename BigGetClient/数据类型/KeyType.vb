Imports System.Text

Public Class KeyType

    'key信息对象


    Public Property Remark As String
    Public Property APIKey As String
    Public Property Secretkey As String
    Public Property Passphrase As String

    ''' <summary>
    ''' 检查key对象数据合法性
    ''' </summary>
    ''' <returns></returns>
    Public Function CheckData() As Boolean
        If Remark.Length = 0 Then Return False
        If APIKey.Length = 0 Then Return False
        If Secretkey.Length = 0 Then Return False
        If Passphrase.Length = 0 Then Return False
        Return True
    End Function


    ''' <summary>
    ''' 把对象信息转化为列表信息
    ''' </summary>
    ''' <returns></returns>
    Public Function ToList() As List(Of String)

        Dim str As New List(Of String) From {
            Remark,
            APIKey,
            Secretkey,
            Passphrase
        }

        Return str

    End Function



End Class
