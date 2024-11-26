Imports System.Security.Cryptography
Imports System.Text

Public Class Signer
    '签名类

    Public Property Secret_Key As String

    Private ReadOnly Property _hmacsha256 As HMACSHA256
        Get
            If IsNothing(t_hmacsha256) = True Then
                Dim keyBuffer As Byte() = Encoding.UTF8.GetBytes(Secret_Key)

                t_hmacsha256 = New HMACSHA256(keyBuffer)
            End If

            Return t_hmacsha256
        End Get
    End Property

    Dim t_hmacsha256 As HMACSHA256

    Public Function Sign(timestamp As String, method As String, path As String, body As String) As String

        If String.IsNullOrEmpty(timestamp) OrElse String.IsNullOrEmpty(method) OrElse String.IsNullOrEmpty(path) Then
            Return String.Empty
        End If

        Dim str As New StringBuilder

        str.Append(timestamp)
        str.Append(method)
        str.Append(path)
        If Not String.IsNullOrEmpty(body) Then
            If method = "GET" Then
                str.Append("?")
                str.Append(body)
            End If
            If method = "POST" Then
                str.Append(body)
            End If

        End If

        Return Sign(str.ToString)
    End Function

    Public Function Sign(ByVal URL_Signer As String) As String

        If String.IsNullOrEmpty(URL_Signer) Then
            Return String.Empty
        End If

        Dim inputBuffer As Byte() = Encoding.UTF8.GetBytes(URL_Signer)

        Dim hashedBuffer As Byte() = _hmacsha256.ComputeHash(inputBuffer)

        Dim str As String = Convert.ToBase64String(hashedBuffer)

        Return str
    End Function

#Region "IDisposable Support"

    Private isDisposed As Boolean = False

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not isDisposed Then
            If disposing Then
                _hmacsha256.Dispose()
            End If

            isDisposed = True
        End If
    End Sub

    Public Sub Dispose()
        Dispose(True)
    End Sub

#End Region

End Class