Public Class Pubvar
    Public Shared Filepath As String = ""
    Public Shared filename As String = ""
    Public fileinfo As New efileinfo
    Public Shared CustomMessageBoxResult As Integer = 0
    ' Dim custommsgbox As New CustomMessageBoxControl


    Public Class efileinfo
        Property Filepath As String
        Property filesize As String
        Property Nooffiles As Integer
    End Class


    Public Class folderinfo
        Property foldername As String
        Property Nooffiles As String
        'Property 
    End Class

End Class
