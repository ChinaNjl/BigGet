Namespace Api.Request.Contract.ReplyType

    Public Class MarkTicker
        Public Property code As String
        Public Property msg As String
        Public Property data As DataType

        Public Class DataType

            ''' <summary>
            ''' 	币对名称
            ''' </summary>
            ''' <returns></returns>
            Public Property symbol As String

            ''' <summary>
            ''' 最新价
            ''' </summary>
            ''' <returns></returns>
            Public Property last As String

            ''' <summary>
            ''' 卖一价
            ''' </summary>
            ''' <returns></returns>
            Public Property bestAsk As String

            ''' <summary>
            ''' 买一价
            ''' </summary>
            ''' <returns></returns>
            Public Property bestBid As String

            ''' <summary>
            ''' 买一量
            ''' </summary>
            ''' <returns></returns>
            Public Property bidSz As String

            ''' <summary>
            ''' 卖一量
            ''' </summary>
            ''' <returns></returns>
            Public Property askSz As String

            ''' <summary>
            ''' 24小时最高价
            ''' </summary>
            ''' <returns></returns>
            Public Property high24h As String

            ''' <summary>
            ''' 	24小时最低价
            ''' </summary>
            ''' <returns></returns>
            Public Property low24h As String

            ''' <summary>
            ''' 	时间戳(毫秒)
            ''' </summary>
            ''' <returns></returns>
            Public Property timestamp As String

            ''' <summary>
            ''' 价格涨跌幅(24小时)
            ''' </summary>
            ''' <returns></returns>
            Public Property priceChangePercent As String

            ''' <summary>
            ''' 交易币交易量
            ''' </summary>
            ''' <returns></returns>
            Public Property baseVolume As String

            ''' <summary>
            ''' 	计价币交易量
            ''' </summary>
            ''' <returns></returns>
            Public Property quoteVolume As String

            ''' <summary>
            ''' usdt成交量
            ''' </summary>
            ''' <returns></returns>
            Public Property usdtVolume As String

            ''' <summary>
            ''' UTC0 开盘价
            ''' </summary>
            ''' <returns></returns>
            Public Property openUtc As String

            ''' <summary>
            ''' 	UTC0 24小时涨跌幅
            ''' </summary>
            ''' <returns></returns>
            Public Property chgUtc As String

            ''' <summary>
            ''' 指数价格
            ''' </summary>
            ''' <returns></returns>
            Public Property indexPrice As String

            ''' <summary>
            ''' 	资金费率
            ''' </summary>
            ''' <returns></returns>
            Public Property fundingRate As String

            ''' <summary>
            ''' 	当前持仓, 单位是base coin
            ''' </summary>
            ''' <returns></returns>
            Public Property holdingAmount As String

        End Class

    End Class

End Namespace