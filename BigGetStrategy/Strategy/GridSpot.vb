Imports MyCOM.ConnectMysql
Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Threading.Thread
Imports ZstdSharp.Unsafe

Namespace Strategy

    ''' <summary>
    ''' 现货网格交易策略
    ''' </summary>

    Public Class GridSpot

#Region "私有变量"

        Private conn As MySqlConnection
        Private MyAdp As MySqlDataAdapter
        Private ReadOnly DbConfig As dbConfig   '数据库配置信息

        Private dsUser As DataSet
        Private drUser As DataRow
        Private Const Tablename As String = "strategytable"
        Private TestMode As Boolean     '是否测试模式运行策略
        Private Bgw As New BackgroundWorker With {.WorkerSupportsCancellation = True, .WorkerReportsProgress = True}
        Private UserInfo As Api.UserKeyInfo    '调用api的密钥
        Private UserCall As Api.User.UserCall

        Private SymbolInformation As Api.Api.Request.Spot.Reply.AssetsLite.Datum '交易标的’
        Private MarginCoinInformation As Api.Api.Request.Spot.Reply.AssetsLite.Datum '保证金信息’

#End Region

#Region "公有属性"

        ''' <summary>
        '''托管策略运行状态
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property State As Boolean
            Get
                Return Bgw.IsBusy
            End Get
        End Property

        ''' <summary>
        ''' 策略编号（在数据库中的标识）
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Id As String
            Get
                Return drUser.Item("id")
            End Get
        End Property

        ''' <summary>
        ''' 返回交易标的
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property symbol As String
            Get
                Return drUser.Item("symbol")
            End Get
        End Property

        ''' <summary>
        ''' 返回计价货币
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property marginCoin As String
            Get
                Return drUser.Item("marginCoin")
            End Get
        End Property

        ''' <summary>
        ''' 返回交易标的价格
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SymbolPrice As String
            Get
                Dim lst As New List(Of String)
                lst.Add(symbol & marginCoin)
                Dim dtPrice As DataRow = PublicConf.PublicData.Tables("spot_tickers").Rows.Find(lst.ToArray)    '通过比对主键来查找特定行
                Return dtPrice.Item(3)
            End Get
        End Property

#End Region

#Region "构造函数"

        ''' <summary>
        ''' 初始化对象
        ''' </summary>
        ''' <param name="p_dr"></param>
        Sub New(ByVal p_dr As DataRow, ByVal p_TestMode As Boolean)

            '从数据库读取策略信息
            DbConfig = New dbConfig(PublicConf.Sql.SqlServer, PublicConf.Sql.SqlPort, PublicConf.Sql.SqlUser, PublicConf.Sql.SqlPassword, PublicConf.Sql.Database)
            conn = New MySqlConnection(DbConfig.ConnectStr)
            conn.Open()
            dsUser = New DataSet
            drUser = ReadStrategy(p_dr.Item("id"))   '数据库中读取策略信息

            UserInfo = New Api.UserKeyInfo With {
                .ApiKey = drUser.Item("apikey"),
                .Secretkey = drUser.Item("secretkey"),
                .Passphrase = drUser.Item("passphrase"),
                .Host = drUser.Item("host")}
            UserCall = New Api.User.UserCall(UserInfo)
            TestMode = p_TestMode   '选择测试模式
        End Sub

#End Region

#Region "公有方法"

        ''' <summary>
        ''' 停止线程
        ''' </summary>
        Public Sub StopRun()
            Bgw.CancelAsync()
            Do
                If State = False Then
                    Exit Do
                Else
                    Sleep(1000)
                End If
            Loop
        End Sub

        ''' <summary>
        ''' 启动策略线程
        ''' </summary>
        Public Sub Run()

            If Bgw.IsBusy = False Then
                If Not TestMode Then
                    AddHandler Bgw.DoWork, AddressOf TestDoWorkRunStrategy
                Else
                    AddHandler Bgw.DoWork, AddressOf DoWorkRunStrategy
                End If

                AddHandler Bgw.RunWorkerCompleted, AddressOf RunComplete
                Bgw.RunWorkerAsync()
            End If

        End Sub

        ''' <summary>
        ''' 策略停止后执行
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub RunComplete(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs)
            Console.WriteLine("RunComplete")
        End Sub

#End Region

#Region "私有方法"

        ''' <summary>
        ''' 策略主线程(测试)
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub DoWorkRunStrategy(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)

            Dim Worker As BackgroundWorker = CType(sender, BackgroundWorker)

            Do
                Do
                    If Worker.CancellationPending Then Exit Sub

                    '获取标的和保证金币种数据
                    Dim repAssetsLite As Api.Api.Request.Spot.Reply.AssetsLite = UserCall.SpotAccountAssetsLite
                    If repAssetsLite.code = "00000" And repAssetsLite.data.Count > 0 Then
                        MarginCoinInformation = GetCoinInformation(marginCoin, repAssetsLite)
                        SymbolInformation = GetCoinInformation(symbol, repAssetsLite)
                    Else
                        Exit Do
                    End If

                    '计算持仓市值和可用资金（usdt）
                    Dim price As Single = SymbolPrice   '最新币价
                    Dim symbolMarketValue As Single = CType(SymbolInformation.available, Single) * price    '标的市值
                    Dim marginMarketValue As Single = MarginCoinInformation.available   '保证金市值’
                    Dim profit As Single = symbolMarketValue - marginMarketValue    '盈亏
                    Dim TotalMarketValueHalf As Single = (symbolMarketValue + marginMarketValue) / 2
                    Dim diffValue As Single = TotalMarketValueHalf * 0.02   '计算交易阈值，默认为2%
                    If profit > diffValue Then
                        '执行卖出symbol
                    ElseIf profit < -diffValue Then
                        '执行买入symbol

                    End If

                Loop
            Loop

        End Sub

        ''' <summary>
        ''' 获取币种信息
        ''' </summary>
        ''' <param name="p_strCoin">币种</param>
        ''' <param name="p_AssetsLite">UserCall.SpotAccountAssetsLite返回值</param>
        ''' <returns></returns>
        Private Function GetCoinInformation(ByVal p_strCoin As String,
                                     ByVal p_AssetsLite As Api.Api.Request.Spot.Reply.AssetsLite) As Api.Api.Request.Spot.Reply.AssetsLite.Datum

            Dim tmp As Api.Api.Request.Spot.Reply.AssetsLite.Datum = Nothing

            '区分交易标的和保证金币种
            If p_strCoin = marginCoin Then tmp = p_AssetsLite.data.Find(AddressOf FindDataMariginCoinInformation)
            If p_strCoin = symbol Then tmp = p_AssetsLite.data.Find(AddressOf FindDataSymbolInformation)
            If IsNothing(tmp) Then
                tmp.coinName = p_strCoin
                tmp.available = "0"
            End If

            Return tmp

        End Function

        ''' <summary>
        ''' 策略主线程(测试)
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub TestDoWorkRunStrategy(ByVal sender As System.Object, ByVal e As DoWorkEventArgs)
            Dim ret
            If True Then
                ret = UserCall.SpotAccountAssetsLite
                Debug.Print(ret.tojson)
            End If
            If False Then
                ret = UserCall.SpotGetMarketTickers
                Debug.Print("API:{0}    Request:{1}", "SpotGetMarketTickers", ret.ToJson)
            End If

            If False Then
                ret = UserCall.SpotGetAccountGetInfo
                Debug.Print("API:{0}    Request:{1}", "SpotGetAccountGetInfo", ret.ToJson)
            End If

            If True Then
                ret = UserCall.SpotAccountAssets("ETH")
                Debug.Print("API:{0}    Request:{1}", "SpotAccountAssets", ret.ToJson)
            End If

            If False Then
                ret = UserCall.SpotAccountSubAccountSpotAssets
                Debug.Print("API:{0}    Request:{1}", "SpotAccountSubAccountSpotAssets", ret.ToJson)
            End If

            If False Then
                Dim p As New Api.Api.Request.Spot.Param.AccountBills With {
                    .coinId = 2}
                ret = UserCall.SpotAccountBills(p)
                Debug.Print("API:{0}    Request:{1}", "SpotAccountBills", ret.ToJson)
            End If

        End Sub

        ''' <summary>
        ''' 保存策略最新状态到数据库
        ''' </summary>
        Private Sub Update()
            Try
                MyAdp.Update(dsUser, Tablename)
            Catch ex As Exception
                Debug.Print("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
            End Try

        End Sub

        ''' <summary>
        ''' 从数据库中读取策略信息
        ''' </summary>
        ''' <param name="p_id"></param>
        ''' <returns></returns>
        Private Function ReadStrategy(ByVal p_id As String) As DataRow

            Dim commandStr As String = "select * from strategytable where id=" & p_id

            MyAdp = New MySqlDataAdapter(commandStr, conn)

            MyAdp.Fill(dsUser, Tablename)
            Dim tdr As DataRow = dsUser.Tables(Tablename).Rows(0)

            Return tdr

        End Function

#End Region

#Region "托管函数"

        Friend Function FindDataMariginCoinInformation(d As Api.Api.Request.Spot.Reply.AssetsLite.Datum) As Boolean
            Return d.coinName = marginCoin
        End Function

        Friend Function FindDataSymbolInformation(d As Api.Api.Request.Spot.Reply.AssetsLite.Datum) As Boolean
            Return d.coinName = symbol
        End Function

        Friend Function FindSymbol(d As DataRow) As Boolean
            Return d(12) = symbol
        End Function

#End Region

    End Class

End Namespace