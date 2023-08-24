Imports System.CodeDom
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System.Threading.Thread



Namespace PublicData
    Public Class GetContracts

        'Public Property ds As DataSet
        Public Property sql As UserType.SqlInfo = PublicConf.Sql
        Public Property userKey As Api.UserInfo = PublicConf.PublicUserKey
        Private Property myadp As MySqlDataAdapter

        Private Shared bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}

        ''' <summary>
        ''' 通过dataset控件读取数据库contracttable 表
        ''' </summary>
        Public Sub OpenTableFromDatabase()

            Dim conn As New MySqlConnection(sql.ConnectStr)

            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try

            PublicConf.DtContracts = New DataSet
            Dim commandStr As String = "select * from contracttable"
            myadp = New MySqlDataAdapter(commandStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(myadp)

            myadp.MissingSchemaAction = MissingSchemaAction.AddWithKey      '加上默认主键
            myadp.Fill(PublicConf.DtContracts, "contracttable")

        End Sub

        ''' <summary>
        ''' 更新变动的数据
        ''' </summary>
        ''' <returns></returns>
        Public Function Update() As Boolean


            Try
                myadp.Update(PublicConf.DtContracts, "contracttable")
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return False
            End Try


            Return True
        End Function

        Public Sub Run()

            If bgw.IsBusy = False Then
                AddHandler bgw.DoWork, AddressOf DoWorkGetContracts
                ' AddHandler bgw.ProgressChanged, AddressOf ProgressChanged_GetTickeets
                bgw.RunWorkerAsync()
            End If

        End Sub

        Private Sub DoWorkGetContracts(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            Dim Worker As BackgroundWorker = CType(sender, BackgroundWorker)

            Dim userCall As New Api.UserCall(userKey)


            '从bigget上读取合约信息
            Dim ret As Api.UserType.ReplyType.MarketContracts = userCall.GetMarkContracts("umcbl")

            If ret.code = "00000" Then


                For Each d In ret.data
                    Dim dList As New List(Of String) From {
                            d.symbol,
                            d.baseCoin,
                            d.quoteCoin,
                            d.buyLimitPriceRatio,
                            d.sellLimitPriceRatio,
                            d.feeRateUpRatio,
                            d.makerFeeRate,
                            d.takerFeeRate,
                            d.openCostUpRatio,
                            Strings.Join(d.supportMarginCoins, "|"),
                            d.minTradeNum,
                            d.priceEndStep,
                            d.volumePlace,
                            d.pricePlace,
                            d.sizeMultiplier,
                            d.symbolType
                        }
                    Dim dr As DataRow = PublicConf.DtContracts.Tables("contracttable").Rows.Find(d.symbol)
                    If IsNothing(dr) = False Then
                        dr.ItemArray = dList.ToArray
                    Else
                        Dim ndr As DataRow = PublicConf.DtContracts.Tables("contracttable").NewRow
                        ndr.ItemArray = dList.ToArray
                        PublicConf.DtContracts.Tables("contracttable").Rows.Add(ndr)
                    End If

                Next

                Update()
            Else
                Debug.Print("Error:{0}.{1}        {2}", MyBase.ToString, "DoWorkGetContracts", ret.msg)
            End If




        End Sub








    End Class
End Namespace






