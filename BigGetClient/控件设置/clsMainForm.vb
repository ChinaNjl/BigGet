Public Class clsMainForm
    ''' <summary>
    ''' 主窗体加载状态，加载成功返回true，否则false
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property MainFormState As Boolean = False



    ''' <summary>
    ''' 初始化窗口各个控件位置
    ''' </summary>
    Public Shared Sub SetMainFormInterface()

        '检测主窗口是否完成初始化
        If Not MainFormState Then Exit Sub


        Dim UseScreenTop As Integer = MainForm.MenuStrip1.Top + MainForm.MenuStrip1.Height + 1
        Dim UseScreenHeigh As Integer = MainForm.StatusStrip1.Top - UseScreenTop - 1
        Dim UseScreebWidth As Integer = MainForm.Width - 8 - 8 - 1 - 1

        With MainForm

            With MainForm.TabControl1
                .Top = UseScreenTop
                .Left = 1
                .Height = UseScreenHeigh
                .Width = UseScreebWidth

                With MainForm.TabPage1
                    .Text = "导入APIKEY"

                    With MainForm.GroupBox1
                        .Text = "ApiKeyList"
                        .Top = 1
                        .Left = 1
                        .Width = MainForm.TabPage1.Width - 2
                        .Height = MainForm.TabPage1.Height - 2

                        With MainForm.DataGridView1
                            .Top = 20
                            .Left = 8
                            .Width = MainForm.GroupBox1.Width - 16
                            .Height = MainForm.GroupBox1.Height - 200

                        End With

                        With MainForm.Button1
                            .Top = MainForm.DataGridView1.Top + MainForm.DataGridView1.Height + 8
                            .Left = 8
                            .Text = "导入"
                        End With

                        With MainForm.Button2
                            .Top = MainForm.DataGridView1.Top + MainForm.DataGridView1.Height + 8
                            .Left = MainForm.Button1.Left + MainForm.Button1.Width + 8
                            .Text = "保存"
                        End With

                        With MainForm.Button3
                            .Top = MainForm.DataGridView1.Top + MainForm.DataGridView1.Height + 8
                            .Left = MainForm.Button2.Left + MainForm.Button2.Width + 8
                            .Text = "录入"
                        End With

                        With MainForm.Button4
                            .Top = MainForm.DataGridView1.Top + MainForm.DataGridView1.Height + 8
                            .Left = MainForm.Button3.Left + MainForm.Button3.Width + 8
                            .Text = "删除"
                        End With

                    End With

                End With

                With MainForm.TabPage2
                    .Text = "批量对冲交易"

                    With MainForm.GroupBox2
                        .Text = "批量对冲交易"
                        .Top = 1
                        .Left = 1
                        .Width = MainForm.TabPage2.Width - 2
                        .Height = MainForm.TabPage2.Height - 2
                    End With

                    With MainForm.GroupBox3
                        .Text = "创建对冲交易"
                        .Top = MainForm.DataGridView2.Top + MainForm.DataGridView2.Height + 8
                        .Left = 8
                        .Width = 550
                        .Height = MainForm.GroupBox2.Height - MainForm.DataGridView2.Top - MainForm.DataGridView2.Height - 12

                        With MainForm.Label1
                            .Text = "账号1"
                        End With

                        With MainForm.Label2
                            .Text = "账号2"
                        End With

                        With MainForm.Button5
                            .Text = "创建对冲组合"
                        End With

                        With MainForm.ComboBox1
                            .DropDownStyle = ComboBoxStyle.DropDownList '禁止修改text
                        End With

                        With MainForm.ComboBox2
                            .DropDownStyle = ComboBoxStyle.DropDownList '禁止修改text
                        End With


                    End With

                    With MainForm.DataGridView2
                        .Top = 20
                        .Left = 8
                        .Width = MainForm.GroupBox2.Width - 16
                        .Height = MainForm.GroupBox2.Height - 200
                    End With


                    With MainForm.GroupBox4
                        .Text = "交易操作"
                        .Top = MainForm.GroupBox3.Top
                        .Left = MainForm.GroupBox3.Left + MainForm.GroupBox3.Width + 8
                        .Height = MainForm.GroupBox3.Height
                        .Width = MainForm.DataGridView2.Width - MainForm.GroupBox3.Width - 8


                        With MainForm.Button6
                            .Text = "批量下单"
                            .Top = 24
                            .Left = 8
                            .Height = 50
                            .Width = 150
                        End With

                        With MainForm.Button7
                            .Text = "单账户下单"
                            .Top = 24 + MainForm.Button6.Height + 8
                            .Left = 8
                            .Height = 50
                            .Width = 150
                        End With



                    End With






                End With

            End With

        End With



    End Sub
End Class
