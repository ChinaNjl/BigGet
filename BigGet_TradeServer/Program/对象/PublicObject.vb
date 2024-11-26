Imports ZstdSharp.Unsafe

''' <summary>
''' 公共对象
''' </summary>
Module PublicObject

    ''' <summary>
    ''' 公共信息：合约信息对象
    ''' </summary>
    ''' <returns></returns>
    Public Property PublicGetContracts As New BigGetStrategy.PublicData.GetContracts

    ''' <summary>
    ''' 公共信息：全部交易对信息
    ''' </summary>
    ''' <returns></returns>
    Public Property PublicGetTickers As New BigGetStrategy.PublicData.GetTickers

    Public PublicData As New BigGetStrategy.PublicData.GetSpotTickers

    ''' <summary>
    '''  用户策略信息
    ''' </summary>
    ''' <returns></returns>
    Public Property PublicGetUserStrategy As New BigGetStrategy.PublicData.GetUserStrategy

End Module