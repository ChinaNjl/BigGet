Imports System.Security.Cryptography.Xml
Imports Mysqlx.XDevAPI.Relational

Public Class PublicConf

    ''' <summary>
    ''' 对外输出Tickers，时效性低
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property Tickers As New DataSet

    ''' <summary>
    ''' 公共行情信息dataset
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property PublicData As New DataSet

    ''' <summary>
    ''' UserSQL服务器信息
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property Sql As New UserType.SqlInfo

    ''' <summary>
    ''' 公共api使用的Userkey
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property PublicUserKey As New Api.UserKeyInfo With {
        .ApiKey = "",
        .Secretkey = "",
        .Passphrase = "",
        .Host = "api.bitget.com"}

End Class