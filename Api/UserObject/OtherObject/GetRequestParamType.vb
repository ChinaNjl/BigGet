Imports System.Text

Namespace UserObject.OtherObject

	Public Class GetRequestParamType

		Private _params As Dictionary(Of String, String)

		''' <summary>
		''' Constructor
		''' </summary>
		''' <param name="request">The initial object(初始化对象)</param>
		Public Sub New(Optional request As GetRequestParamType = Nothing)
			If request IsNot Nothing Then
				_params = New Dictionary(Of String, String)(request._params)
			Else
				_params = New Dictionary(Of String, String)()
			End If
		End Sub

		''' <summary>
		''' Add URL escape property and value pair（向url中添加属性和对应的值）
		''' </summary>
		''' <param name="property">property</param>
		''' <param name="value">value</param>
		''' <returns>Current object</returns>
		Public Function AddParam([property] As String, value As String) As GetRequestParamType
			If ([property] IsNot Nothing) AndAlso (value IsNot Nothing) Then
				_params.Add(Uri.EscapeDataString([property]), Uri.EscapeDataString(value))
			End If

			Return Me
		End Function

		''' <summary>
		''' Add and merge another object
		''' </summary>
		''' <param name="request">The object that want to add</param>
		''' <returns>Current object</returns>
		Public Function AddParam(request As GetRequestParamType) As GetRequestParamType
			_params.Concat(request._params)

			Return Me
		End Function

		''' <summary>
		''' Concat the property and value pair
		''' </summary>
		''' <returns>string</returns>
		Public Function BuildParams() As String
			If _params.Count = 0 Then
				Return String.Empty
			End If

			Dim sb As New StringBuilder()

			For Each para In _params.OrderBy(Function(i) i.Key, StringComparer.Ordinal)
				sb.Append("&"c)
				sb.Append(para.Key).Append("="c).Append(para.Value)
			Next

			Return sb.ToString().Substring(1)
		End Function
	End Class


End Namespace
