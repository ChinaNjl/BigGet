Namespace Api.Request.Spot.ReplyType

    Public Class AccountGetInfo
        Public Property code As String
        Public Property msg As String
        Public Property data As Data
    End Class

    Public Class Data
        Public Property user_id As String
        Public Property inviter_id As String
        Public Property agent_inviter_code As Object
        Public Property channel As Object
        Public Property ips As String
        Public Property authorities() As String
        Public Property parentId As Integer
        Public Property trader As Boolean
        Public Property isSpotTrader As Boolean
    End Class

End Namespace