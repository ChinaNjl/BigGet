Imports System.CodeDom
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System.Threading.Thread
Imports MyCOM.ConnectMysql

Namespace PublicData

    Public Class GetContracts

#Region "私有变量"

        Private ReadOnly DbConfig As dbConfig   '数据库配置信息
        Private ReadOnly MysqlConnect As ConnectMysql   '数据库对象
        Private ReadOnly TableName As String = "contracttable"    '表名
        Private MyAdp As MySqlDataAdapter
        Private ReadOnly UserCall As Api.User.UserCall
        Private ReadOnly UserKey As Api.UserKeyInfo = PublicConf.PublicUserKey
        Private ReadOnly bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}

#End Region

        Public Property sql As UserType.SqlInfo = PublicConf.Sql

#Region "构造函数"

        Sub New()
            DbConfig = New dbConfig(PublicConf.Sql.SqlServer, PublicConf.Sql.SqlPort, PublicConf.Sql.SqlUser, PublicConf.Sql.SqlPassword, PublicConf.Sql.Database)
            MysqlConnect = New ConnectMysql(DbConfig)
            UserCall = New Api.User.UserCall(UserKey)  'biggetapi对象
        End Sub

#End Region

#Region "公有方法"

        ''' <summary>
        ''' 更新变动的数据
        ''' </summary>
        ''' <returns></returns>
        Public Function Update() As Boolean

            Try
                MyAdp.Update(PublicConf.PublicData, TableName)
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return False
            End Try

            Return True
        End Function

        Public Sub Run()

            If bgw.IsBusy = False Then
                ReadTable()

                AddHandler bgw.DoWork, AddressOf DoWorkGetContracts
                'AddHandler bgw.ProgressChanged, AddressOf WorkerChanged
                'AddHandler bgw.RunWorkerCompleted, AddressOf WorkerCompleted
                bgw.RunWorkerAsync()
            End If

        End Sub

#End Region

        ''' <summary>
        ''' 读取数据库
        ''' </summary>
        ''' <returns></returns>
        Friend Function ReadTable() As Boolean
            Dim conn As New MySqlConnection(DbConfig.ConnectStr)
            Try
                conn.Open()
            Catch ex As Exception
                Debug.Print("Error:{0}.{1}        {2}", MyBase.ToString, "ReadTable", "打开数据库失败")
                Return False
            End Try

            Dim cmdStr As String = "select * from " & TableName
            MyAdp = New MySqlDataAdapter(cmdStr, conn)
            Dim commandBuilder As New MySqlCommandBuilder(MyAdp)
            MyAdp.MissingSchemaAction = MissingSchemaAction.AddWithKey      '加上默认主键

            SyncLock PublicConf.PublicData
                MyAdp.Fill(PublicConf.PublicData, TableName)
            End SyncLock

            Return True

        End Function

        Private Sub DoWorkGetContracts(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            Dim Worker As BackgroundWorker = CType(sender, BackgroundWorker)

            If GetContracts() Then

            End If

        End Sub

        Friend Function GetContracts() As Boolean
            '从bigget上读取合约信息
            Dim ret As Api.Api.Request.Contract.ReplyType.MarketContracts = UserCall.ContractGetMarkContracts("umcbl")
            If ret.code = "00000" Then
                If ret.data.Count > 0 Then
                    For Each d As Api.Api.Request.Contract.ReplyType.MarketContracts.DataType In ret.data
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
                        Dim dr As DataRow = PublicConf.PublicData.Tables(TableName).Rows.Find(d.symbol)
                        If IsNothing(dr) = False Then
                            dr.ItemArray = dList.ToArray
                        Else
                            Dim ndr As DataRow = PublicConf.PublicData.Tables(TableName).NewRow
                            ndr.ItemArray = dList.ToArray
                            PublicConf.PublicData.Tables(TableName).Rows.Add(ndr)
                        End If
                    Next
                    Return True
                End If
            End If
            Return False
        End Function

    End Class

End Namespace