Imports System.Text
Imports System.Text.Json

Namespace Api.Request

    Namespace Contract

    End Namespace

    Namespace Spot

        Namespace Reply

            'webapi的返回类型
            Public Class AccountBills
                Public Property code As String
                Public Property msg As String
                Public Property requestTime As Int64
                Public Property data As List(Of Datum)

                ''' <summary>
                ''' 序列化
                ''' </summary>
                ''' <returns></returns>
                Public Function ToJson() As String
                    Return JsonSerializer.Serialize(Me)
                End Function

                Public Class Datum
                    Public Property cTime As String
                    Public Property coinId As String
                    Public Property coinName As String
                    Public Property groupType As String
                    Public Property bizType As String
                    Public Property quantity As String
                    Public Property balance As String
                    Public Property fees As String
                    Public Property billId As String
                End Class

            End Class

            Public Class AssetsLite
                Public Property code As String
                Public Property msg As String
                Public Property requestTime As Int64
                Public Property data As List(Of Datum)

                Public Class Datum
                    Public Property coinId As Integer
                    Public Property coinName As String
                    Public Property available As String
                    Public Property frozen As String
                    Public Property lock As String
                    Public Property uTime As String
                End Class

                ''' <summary>
                ''' 序列化
                ''' </summary>
                ''' <returns></returns>
                Public Function ToJson() As String
                    Return JsonSerializer.Serialize(Me)
                End Function

            End Class

            Public Class Assets
                Public Property code As String
                Public Property msg As String
                Public Property data As List(Of Datum)
                Public Property requestTime As Int64

                Public Class Datum

                    Public Property coinId As Integer
                    Public Property coinName As String

                    Public Property coinDisplayName As String
                    Public Property available As String
                    Public Property frozen As String
                    Public Property lock As String

                    Public Property uTime As Object
                        Get
                            Return _uTime
                        End Get
                        Set(value As Object)
                            _uTime = CType(value, String)
                        End Set
                    End Property

                    Dim _uTime As String
                End Class

                ''' <summary>
                ''' 序列化
                ''' </summary>
                ''' <returns></returns>
                Public Function ToJson() As String
                    Return JsonSerializer.Serialize(Me)
                End Function

            End Class

            Public Class AccountGetInfo
                Public Property code As String
                Public Property msg As String
                Public Property requestTime As Int64
                Public Property data As DataType

                ''' <summary>
                ''' 序列化
                ''' </summary>
                ''' <returns></returns>
                Public Function ToJson() As String
                    Return JsonSerializer.Serialize(Me)
                End Function

                Public Class DataType
                    Public Property user_id As String

                    Public Property inviter_id As String

                    Public Property agent_inviter_code As String

                    Public Property channel_ As String

                    Dim _channel As String

                    Public Property ips As String

                    Public Property authorities As List(Of String)

                    Public Property parentId As Int64

                    Public Property trader As Boolean
                    Public Property isSpotTrader As Boolean
                End Class

            End Class

            Public Class SubAccountSpotAssets
                Public Property code As String
                Public Property msg As String
                Public Property requestTime As Int64
                Public Property data As List(Of Datum)

                ''' <summary>
                ''' 序列化
                ''' </summary>
                ''' <returns></returns>
                Public Function ToJson() As String
                    Return JsonSerializer.Serialize(Me)
                End Function

                Public Class Datum
                    Public Property userId As Long
                    Public Property spotAssetsList As List(Of SpotassetslistType)

                    Public Class SpotassetslistType
                        Public Property coinId As Integer
                        Public Property coinName As String
                        Public Property available As String
                        Public Property frozen As String
                        Public Property lock As String
                    End Class

                End Class

            End Class

            Public Class MarketTickers
                Public Property code As String
                Public Property data As List(Of Datum)
                Public Property msg As String
                Public Property requestTime As String

                ''' <summary>
                ''' 序列化
                ''' </summary>
                ''' <returns></returns>
                Public Function ToJson() As String
                    Return JsonSerializer.Serialize(Me)
                End Function

                Public Class Datum

                    Public Property change As String
                    Public Property sellOne As String
                    Public Property baseVol As String
                    Public Property close As String
                    Public Property bidSz As String
                    Public Property quoteVol As String
                    Public Property high24h As String
                    Public Property askSz As String
                    Public Property low24h As String
                    Public Property ts As String
                    Public Property buyOne As String
                    Public Property openUtc0 As String
                    Public Property symbol As String
                    Public Property changeUtc As String
                    Public Property usdtVol As String

                    Public Function ToList() As List(Of String)
                        Return New List(Of String) From {
                            change, sellOne, baseVol, close, bidSz, quoteVol, high24h, askSz, low24h, ts, buyOne, openUtc0, symbol, changeUtc, usdtVol}
                    End Function

                End Class

            End Class

        End Namespace

        Namespace Param

            'webapi的参数类型
            Public Class AccountBills
                Public Property coinId As String
                Public Property groupType As String
                Public Property bizType As String
                Public Property before As String
                Public Property after As String
                Public Property limit As String

                ''' <summary>
                ''' 序列化
                ''' </summary>
                ''' <returns></returns>
                Public Function ToJson() As String
                    Return JsonSerializer.Serialize(Me)
                End Function

            End Class

        End Namespace

    End Namespace

End Namespace