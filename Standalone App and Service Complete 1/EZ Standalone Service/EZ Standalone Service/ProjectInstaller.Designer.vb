<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.STServiceProcessinstaller = New System.ServiceProcess.ServiceProcessInstaller()
        Me.STServiceprocess = New System.ServiceProcess.ServiceInstaller()
        Me.ServiceController1 = New System.ServiceProcess.ServiceController()
        '
        'STServiceProcessinstaller
        '
        Me.STServiceProcessinstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.STServiceProcessinstaller.Password = Nothing
        Me.STServiceProcessinstaller.Username = Nothing
        '
        'STServiceprocess
        '
        Me.STServiceprocess.Description = "This Service is used for decrypt ezo files"
        Me.STServiceprocess.DisplayName = "EZOFIS Standalone Service"
        Me.STServiceprocess.ServiceName = "Ezofis Standalone Service"
        Me.STServiceprocess.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'ServiceController1
        '
        Me.ServiceController1.ServiceName = "Ezofis Standalone Service"
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.STServiceProcessinstaller, Me.STServiceprocess})

    End Sub
    Private WithEvents STServiceProcessinstaller As ServiceProcess.ServiceProcessInstaller
    Private WithEvents STServiceprocess As ServiceProcess.ServiceInstaller
    Friend WithEvents ServiceController1 As ServiceProcess.ServiceController
End Class
