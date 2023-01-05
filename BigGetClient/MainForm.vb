Public Class MainForm

    ''' <summary>
    ''' 窗体加载
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        clsMainForm.MainFormState = True

        '初始化窗口界面
        Call clsMainForm.SetMainFormInterface()
        '初始化控件
        Call clsTabPage1.clsDataGridView1.Initialization()
        Call clsTabPage2.clsDataGridView2.Initialization()

    End Sub

    ''' <summary>
    ''' 窗口尺寸变化结束
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MainForm_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        Call clsMainForm.SetMainFormInterface()
    End Sub

    ''' <summary>
    ''' 录入key信息
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call clsTabPage1.clsButton.BtnOpenEnterKeyForm()
    End Sub

    ''' <summary>
    ''' 保存信息至txt
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Call clsTabPage1.clsButton.BtnSaveApiInfomation()
    End Sub

    ''' <summary>
    ''' 导入api信息至DataGridView1
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call clsTabPage1.clsButton.BtnReadApiInfomation()
    End Sub

    ''' <summary>
    ''' 删除选中行数据
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Call clsTabPage1.clsButton.BtnDelRow()
    End Sub

    Private Sub TabControl1_Click(sender As Object, e As EventArgs) Handles TabControl1.Click
        Call clsMainForm.SetMainFormInterface()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Call clsTabPage2.clsButton.BtnOpenCreatePortfolio()
    End Sub

    Private Sub ComboBox1_DropDown(sender As Object, e As EventArgs) Handles ComboBox1.DropDown
        Call clsTabPage2.clsComboBox.ComboBox1.Initialization()
    End Sub

    Private Sub ComboBox2_DropDown(sender As Object, e As EventArgs) Handles ComboBox2.DropDown
        Call clsTabPage2.clsComboBox.Combobox2.Initialization()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Call clsTabPage2.clsButton.BtnOpenOrderPlaceOrdersFrom()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Call clsTabPage2.clsButton.BtnOpenSingleOrderPlaceOrderForm()
    End Sub
End Class
