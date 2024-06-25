



Namespace Program.Form.ParentForm


    Public Class MainFormClass

        ''' <summary>
        ''' 程序窗体加载成功标志
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property UploadState As Boolean = False

        ''' <summary>
        ''' sql服务器信息
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property Sql As New SqlObject With {
            .SqlServer = "45.207.49.244",
            .SqlUser = "BigGetTrade",
            .SqlPassword = "YKdDCapyzBdJFfe2",
            .SqlPort = "3306",
            .Database = "biggettrade"}


        ''' <summary>
        ''' 设置BigGetStrategy的sql服务器信息
        ''' </summary>
        Public Shared Sub SetBigGetStrategySql()

            With BigGetStrategy.PublicConf.Sql
                .SqlPassword = Sql.SqlPassword
                .SqlPort = Sql.SqlPort
                .SqlServer = Sql.SqlServer
                .SqlUser = Sql.SqlUser
                .Database = Sql.Database
            End With

        End Sub

        ''' <summary>
        ''' 窗体及控件绘制
        ''' </summary>
        Public Shared Sub Initialize()

            With MainForm.TabControl1
                .Left = 1
                .Width = MainForm.MenuStrip1.Width
                .Height = MainForm.StatusStrip1.Top - .Top

                With MainForm.TabControl1

                    '行情数据页
                    With MainForm.TabPage1

                        With MainForm.GroupBox1

                            .Left = 1
                            .Width = MainForm.TabPage1.Width - .Left * 2
                            .Height = MainForm.TabPage1.Height - .Top * 2

                            With MainForm.DataGridView1
                                .Left = .Top / 2
                                .Width = MainForm.GroupBox1.Width - .Left * 2
                                .Height = MainForm.GroupBox1.Height - .Top * 1.5
                            End With

                        End With

                    End With

                    '合约信息页
                    With MainForm.TabPage2

                        With MainForm.GroupBox2
                            .Left = 1
                            .Width = MainForm.TabPage2.Width - .Left * 2
                            .Height = MainForm.TabPage2.Height - .Top * 2


                            With MainForm.DataGridView2
                                .Left = .Top / 2
                                .Width = MainForm.GroupBox2.Width - .Left * 2
                                .Height = MainForm.GroupBox2.Height - .Top * 1.5
                            End With

                        End With

                    End With

                    '托管策略页
                    With MainForm.TabPage3

                        With MainForm.GroupBox3
                            .Left = 1
                            .Width = MainForm.TabPage3.Width - .Left * 2
                            .Height = MainForm.TabPage3.Height - .Top * 2

                            With MainForm.DataGridView3
                                .Left = .Top / 2
                                .Width = MainForm.GroupBox3.Width - .Left * 2
                                .Height = MainForm.GroupBox3.Height - .Top * 1.5 - (MainForm.BtnAutoRefresh_TabPage.Height + .Top * 0.8)

                            End With

                        End With



                        With MainForm.BtnAutoRefresh_TabPage
                            .Anchor = AnchorStyles.Bottom
                            .Left = MainForm.DataGridView3.Left
                        End With

                    End With

                End With

            End With

        End Sub


        ''' <summary>
        ''' 窗体及控件属性初始化
        ''' </summary>
        Public Shared Sub Initialize_ProgramSettings()

            With MainForm
                .Text = "策略托管交易端"
            End With

            With MainForm.TabControl1

                With MainForm.TabPage1
                    .Text = "行情数据"

                    With MainForm.GroupBox1
                        .Text = "行情数据"
                    End With
                End With


                With MainForm.TabPage2
                    .Text = "合约信息"
                    With MainForm.GroupBox2
                        .Text = "合约信息"
                    End With
                End With

                With MainForm.TabPage3
                    .Text = "托管策略"
                    With MainForm.GroupBox3
                        .Text = "托管策略"
                    End With

                    MainForm.BtnAutoRefresh_TabPage.Text = "获取用户策略"
                End With





            End With

        End Sub



    End Class


End Namespace




