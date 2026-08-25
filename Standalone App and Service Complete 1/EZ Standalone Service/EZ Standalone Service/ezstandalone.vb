Imports System.IO

Public Class EzofisStandaloneService

    Inherits System.ServiceProcess.ServiceBase
    Public thisTimer As System.Timers.Timer
    Public LogFileName As String
    Dim ProcessEngine As New Standalone_Service.EncrptyDecrypt
    Dim filelocation As String


    Public Sub writetxtfle(ByVal msg As String)
        Try
            'filelocation = dir() & "\" & LogFileName & ".txt"
            Dim lines As New ArrayList()
            Dim line As String
            Dim lastline As String
            Using r As New StreamReader(ProcessEngine.filelocation)
                line = r.ReadLine()
                While line IsNot Nothing
                    lines.Add(line)
                    line = r.ReadLine()
                End While
                lastline = lines(lines.Count - 1).ToString()
            End Using
            Using sw As StreamWriter = New StreamWriter(ProcessEngine.filelocation, True)
                If Not lastline.EndsWith(msg) Then
                    sw.WriteLine(Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + " : " + msg)
                End If
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Public Function dir() As String
        Dim source As String = ""
        Try
            Dim apppath As String = ""
            apppath = System.Reflection.Assembly.GetEntryAssembly().Location
            apppath = Path.GetDirectoryName(apppath)
            source = apppath + "\log"
            If Not Directory.Exists(source) Then   'Checking Directory Exist or Not
                Directory.CreateDirectory(source)
            End If
        Catch ex As Exception

        End Try

        Return source
    End Function


    Protected Overrides Sub OnStart(ByVal args() As String)
        Try
            LogFileName = Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss")
            LogFileName = LogFileName.Replace("/", "")
            LogFileName = LogFileName.Replace(":", "")
            LogFileName = LogFileName.Replace(" ", "")
            filelocation = dir() & "\" & LogFileName & ".txt"
            ProcessEngine.filelocation = filelocation
            Using sw As StreamWriter = New StreamWriter(filelocation)
                sw.WriteLine(Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + " : Service Is Start.")
            End Using
            thisTimer = New System.Timers.Timer()
            thisTimer.Enabled = True
            thisTimer.Interval = 1000
            thisTimer.AutoReset = True
            AddHandler thisTimer.Elapsed, AddressOf thisTimer_Tick
            thisTimer.Start()
        Catch ex As Exception
            'writetxtfle(ex.Message.ToString())
        End Try
    End Sub

    Private Sub thisTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            RunProcess()
            'LogFileName = Format(DateTime.Now, "MM/dd/yyyy")
            'LogFileName = LogFileName.Replace("/", "")
            'LogFileName = LogFileName.Replace(":", "")
            'LogFileName = LogFileName.Replace(" ", "")
            'ProcessEngine.filelocation = dir() & "\" & LogFileName & ".txt"
            'If Not File.Exists(ProcessEngine.filelocation) Then
            '    Using sw As StreamWriter = New StreamWriter(ProcessEngine.filelocation)
            '        sw.WriteLine(Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss") + " : Service Is Start.")
            '    End Using
            'End If
            thisTimer.Enabled = True
        Catch ex As Exception
            writetxtfle(ex.Message)
        Finally
            thisTimer.Enabled = True
        End Try
    End Sub

    Public Sub RunProcess()
        Try

            thisTimer.Enabled = False
            ProcessEngine.encryptdecrypt()
        Catch ex As Exception
            writetxtfle(ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
    End Sub
End Class
