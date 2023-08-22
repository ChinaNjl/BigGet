





Public Class PublicConf

    ''' <summary>
    ''' 对外输出Tickers，时效性低
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property Tickers As New DataSet

    Public Shared Property DtContracts As New DataSet

    ''' <summary>
    ''' sql服务器信息
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property Sql As New UserType.SqlInfo

    ''' <summary>
    ''' 公共api使用的Userkey
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property PublicUserKey As New Api.UserInfo With {
        .ApiKey = "",
        .Secretkey = "",
        .Passphrase = "",
        .Host = "api.bitget.com"}




End Class
