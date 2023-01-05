Imports Api

Public Class clsTabPage2

    Public Class clsDataGridView2

        ''' <summary>
        ''' 创建列名列表
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property ColumnHeaderName As String()
            Get
                Dim ColNameList As New List(Of String) From {
                "备注名1",
                "APIKey1",
                "Secretkey1",
                "Passphrase1",
                "状态1",
                "备注名2",
                "APIKey2",
                "Secretkey2",
                "Passphrase2",
                "状态2"
            }

                Return ColNameList.ToArray
            End Get
        End Property

        ''' <summary>
        ''' 创建dgv数据源
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property DataSource As DataTable
            Get
                If IsNothing(_DataSource) = True Then

                    Dim dt As New DataTable
                    Dim col As DataColumn

                    For Each s In ColumnHeaderName
                        col = New DataColumn(s)
                        dt.Columns.Add(col)
                    Next
                    _DataSource = dt
                End If

                Return _DataSource
            End Get
        End Property
        Shared _DataSource As DataTable

        ''' <summary>
        ''' 获取控件数据条数
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property DgvRowCount As Integer
            Get
                Return DataSource.Rows.Count
            End Get
        End Property

        ''' <summary>
        ''' 添加一行数据
        ''' </summary>
        ''' <param name="key"></param>
        Public Shared Sub AddRow(ByVal data As Array)

            Dim dr As DataRow = DataSource.NewRow
            dr.ItemArray = data
            DataSource.Rows.Add(dr)

        End Sub

        ''' <summary>
        ''' 清空表内容
        ''' </summary>
        Public Shared Sub Clear()
            DataSource.Clear()
        End Sub

        ''' <summary>
        ''' 修改指定行的指定项数据
        ''' </summary>
        ''' <param name="NumRow"></param>
        ''' <param name="NumItem"></param>
        Public Shared Sub ModRowItemData(NumRow As Integer, NumItem As Integer， data As String)

            If NumRow < DgvRowCount Then
                If NumItem < DataSource.Rows(NumRow).ItemArray.Count Then
                    DataSource.Rows(NumRow).Item(NumItem) = data
                End If
            End If
        End Sub

        ''' <summary>
        ''' 根据列表，批量修改row数据
        ''' </summary>
        ''' <param name="arrList"></param>
        Public Shared Sub ModAllRowItemData(arrList As List(Of (Integer, String)))

            For Each r As DataRow In DataSource.Rows
                Dim a As Array
                a = r.ItemArray

                For Each arr In arrList

                    a(arr.Item1) = arr.Item2

                Next

                r.ItemArray = a
            Next

        End Sub





        ''' <summary>
        ''' 以list方式返回datagridview列表中的数据
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property DataList As List(Of Array)
            Get

                If DgvRowCount = 0 Then
                    Return New List(Of Array)
                End If

                If DgvRowCount > 0 Then

                    Dim l As New List(Of Array)

                    For Each r As DataRow In DataSource.Rows

                        l.Add(r.ItemArray)

                    Next

                    Return l
                End If

                Return New List(Of Array)
            End Get
        End Property



        ''' <summary>
        ''' 初始化控件
        ''' </summary>
        Public Shared Sub Initialization()

            '列标题居中
            MainForm.DataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            '列内容居中
            For Each i As DataGridViewColumn In MainForm.DataGridView2.Columns
                i.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next

            '禁用排序
            For i = 0 To MainForm.DataGridView2.Columns.Count - 1
                MainForm.DataGridView2.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            MainForm.DataGridView2.AllowUserToAddRows = False

            MainForm.DataGridView2.DataSource = DataSource

        End Sub

    End Class

    Public Class clsButton

        Public Shared Sub BtnOpenCreatePortfolio()


            If MainForm.ComboBox1.Text.Length > 0 And MainForm.ComboBox2.Text.Length > 0 Then
                Dim str As String = MainForm.ComboBox1.Text & ",," & MainForm.ComboBox2.Text & ","
                clsDataGridView2.AddRow(Split(str, ","))
                With MainForm
                    .ComboBox1.SelectedIndex = -1   '清空选定值
                    .ComboBox2.SelectedIndex = -1   '清空选定值
                End With
            Else
                MessageBox.Show("请正确选择apikey 1 & 2")
            End If

        End Sub

        ''' <summary>
        ''' 打开批量下单窗口
        ''' </summary>
        Public Shared Sub BtnOpenOrderPlaceOrdersFrom()
            OrderPlaceOrderForm.Show()
            MainForm.Hide()
        End Sub

        Public Shared Sub BtnOpenSingleOrderPlaceOrderForm()

            SingleOrderPlaceOrderForm.Show()
            MainForm.Hide()

        End Sub



    End Class

    Public Class clsComboBox

        Public Class ComboBox1

            Public Shared Sub Initialization()

                MainForm.ComboBox1.Items.Clear()

                If clsTabPage1.clsDataGridView1.DgvRowCount > 0 Then

                    'datagridview1中的数据
                    Dim s2 As String = ""
                    For Each l In clsDataGridView2.DataList
                        Dim str As String = Join(CType(l, Object), ",")
                        s2 = s2 & "|" & str
                    Next

                    For Each l In clsTabPage1.clsDataGridView1.DataList

                        Dim str As String = Join(CType(l, Object), ",")

                        If s2.Contains(str) = False And MainForm.ComboBox2.Text.Equals(str) = False Then
                            MainForm.ComboBox1.Items.Add(Join(CType(l, Object), ","))
                        End If

                    Next

                End If

            End Sub

        End Class

        Public Class Combobox2

            Public Shared Sub Initialization()

                MainForm.ComboBox2.Items.Clear()

                If clsTabPage1.clsDataGridView1.DgvRowCount > 0 Then

                    'datagridview1中的数据
                    Dim s2 As String = ""
                    For Each l In clsDataGridView2.DataList
                        Dim str As String = Join(CType(l, Object), ",")
                        s2 = s2 & "|" & str
                    Next

                    For Each l In clsTabPage1.clsDataGridView1.DataList

                        Dim str As String = Join(CType(l, Object), ",")

                        If s2.Contains(str) = False And MainForm.ComboBox1.Text.Equals(str) = False Then
                            MainForm.ComboBox2.Items.Add(Join(CType(l, Object), ","))
                        End If


                    Next

                End If

            End Sub

        End Class


    End Class

End Class
