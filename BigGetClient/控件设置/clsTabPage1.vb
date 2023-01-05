Imports System.Text

Public Class clsTabPage1

    Public Class clsDataGridView1

        ''' <summary>
        ''' 创建列名列表
        ''' </summary>
        ''' <returns></returns>
        Private Shared ReadOnly Property ColumnHeaderName As String()
            Get
                Dim ColNameList As New List(Of String) From {
                "备注名",
                "APIKey",
                "Secretkey",
                "Passphrase"
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
        ''' 检查重复项，str=待检查内容，colId=列号
        ''' </summary>
        ''' <param name="str"></param>
        ''' <param name="colId"></param>
        ''' <returns></returns>
        Public Shared Function FindRepeat(str As String, colId As Integer) As Boolean

            For Each r As DataRow In DataSource.Rows
                For Each o In r.Item(colId)
                    If o.ToString = str Then
                        Return True
                    End If
                Next
            Next

            Return False

        End Function


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
            MainForm.DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            '列内容居中
            For Each i As DataGridViewColumn In MainForm.DataGridView1.Columns
                i.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next

            '禁用排序
            For i = 0 To MainForm.DataGridView1.Columns.Count - 1
                MainForm.DataGridView1.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            MainForm.DataGridView1.AllowUserToAddRows = False

            MainForm.DataGridView1.DataSource = DataSource

        End Sub





    End Class


    Public Class clsButton

        Public Shared Sub BtnOpenEnterKeyForm()
            EnterKeyForm.Show()
            MainForm.Hide()
        End Sub

        Public Shared Sub BtnSaveApiInfomation()

            If clsDataGridView1.DgvRowCount = 0 Then
                Exit Sub
            End If

            If clsDataGridView1.DgvRowCount > 0 Then

                Dim str As New StringBuilder

                For Each r As DataRow In clsDataGridView1.DataSource.Rows
                    str.AppendLine(Join(r.ItemArray.ToArray, ","))
                Next

                Dim Write As New OperatePrivateProfile.OperateText With {
                    .Path = GeneralSettings.ApiFilePath,
                    .Append = False,
                    .StrData = str.ToString
                }
                Write.Write()

            End If


        End Sub

        Public Shared Sub BtnReadApiInfomation()
            '清空表内容
            clsDataGridView1.Clear()

            '读入信息
            Dim Read As New OperatePrivateProfile.OperateText With {
                .Path = GeneralSettings.ApiFilePath
            }
            Dim ReadList As New List(Of String)
            ReadList = Read.ReadLineToList()

            For Each rl In ReadList
                clsDataGridView1.AddRow(Split(rl, ","))
            Next

        End Sub

        Public Shared Sub BtnDelRow()

            Dim rows As DataGridViewSelectedRowCollection = MainForm.DataGridView1.SelectedRows

            Try
                MainForm.DataGridView1.Rows.Remove(rows(0))
            Catch ex As Exception

            End Try

        End Sub

    End Class

End Class
