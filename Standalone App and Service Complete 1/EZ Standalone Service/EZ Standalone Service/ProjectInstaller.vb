Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.IO
Imports System.Security.AccessControl

Public Class ProjectInstaller

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Private Sub ProjectInstaller_AfterInstall(ByVal sender As System.Object, ByVal e As System.Configuration.Install.InstallEventArgs) Handles MyBase.AfterInstall
        Try
            ServiceController1.Start()
        Catch ex As Exception

        End Try


    End Sub
    Public Shared Function RunCmd(ByVal ParamArray commands As String()) As String
        Dim returnvalue As String = String.Empty

        Dim info As New ProcessStartInfo("cmd")
        info.UseShellExecute = False
        info.RedirectStandardInput = True
        info.RedirectStandardOutput = True
        info.CreateNoWindow = True

        Using process__1 As Process = Process.Start(info)
            Dim sw As StreamWriter = process__1.StandardInput
            Dim sr As StreamReader = process__1.StandardOutput

            For Each command As String In commands
                sw.WriteLine(command)
            Next

            sw.Close()
            returnvalue = sr.ReadToEnd()
        End Using

        Return returnvalue
    End Function
    Public Sub AddFolderSecurity(ByVal folderName As String, ByVal account As String, ByVal rights As FileSystemRights, ByVal controlType As AccessControlType)

        Dim fSecurity As New DirectorySecurity(folderName, AccessControlSections.All)
        Dim accessRule As New FileSystemAccessRule(account, rights, controlType)
        fSecurity.AddAccessRule(accessRule)
        Directory.SetAccessControl(folderName, fSecurity)

    End Sub

    Private Sub ProjectInstaller_BeforeUninstall(sender As System.Object, e As System.Configuration.Install.InstallEventArgs) Handles MyBase.BeforeUninstall
        Try
            ServiceController1.Stop()
        Catch ex As Exception

        End Try

    End Sub

End Class
