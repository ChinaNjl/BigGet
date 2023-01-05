Public Class GeneralSettings

    ''' <summary>
    ''' 程序路径
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property ProgramPath As String = Application.StartupPath

    ''' <summary>
    ''' 保存api信息的文件的名字
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property ApiFileName As String = "ApiInfomation.txt"
    ''' <summary>
    ''' ApiInfomation.txt   路径
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property ApiFilePath As String
        Get
            Return ProgramPath & "\" & ApiFileName
        End Get
    End Property




End Class
