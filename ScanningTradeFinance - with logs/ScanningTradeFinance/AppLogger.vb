Imports System.Collections.Specialized
Imports System.Configuration
Imports System.IO
Imports System.Text

Public Module AppLogger

    Private _processLogPath As String = Nothing
    Private _processFunction As String = Nothing
    Private _processAccountNo As String = Nothing
    Private _processIdLabel As String = "Account No"
    Private _processEnabled As Boolean = False
    Private _userCloseLogged As Boolean = False
    Private _lastProcessLogPath As String = Nothing

    ' Active account run (persists across function EndProcess until EndAccountSession)
    Private _accountRunKey As String = Nothing
    Private _accountRunFolder As String = Nothing
    Private _accountRunIdLabel As String = "Account No"

    Private Function IsProcessLogEnabled() As Boolean
        Try
            Dim appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
            If appcon Is Nothing OrElse appcon("enableProcessLog") Is Nothing Then
                Return False
            End If
            Return String.Equals(appcon("enableProcessLog").ToString().Trim(), "true", StringComparison.OrdinalIgnoreCase)
        Catch
            Return False
        End Try
    End Function

    Private Function GetLogDirectory() As String
        Dim logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log")
        If Not Directory.Exists(logDir) Then
            Directory.CreateDirectory(logDir)
        End If
        Return logDir
    End Function

    Private Function GetDailyLogPath(prefix As String) As String
        Dim fileName = prefix & "_" & DateTime.Now.ToString("yyyy-MM-dd") & ".log"
        Return Path.Combine(GetLogDirectory(), fileName)
    End Function

    Private Function SanitizeFolderPart(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return "NA"
        End If
        Dim cleaned = value.Trim()
        For Each c In Path.GetInvalidFileNameChars()
            cleaned = cleaned.Replace(c, "_"c)
        Next
        Return cleaned
    End Function

    Private Function IsWirLabel(idLabel As String) As Boolean
        Return String.Equals(If(idLabel, "").Trim(), "Work Item Reference", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Function StepIdSuffix() As String
        If String.IsNullOrEmpty(_processAccountNo) Then
            Return ""
        End If
        If IsWirLabel(_processIdLabel) Then
            Return " | WIR=" & _processAccountNo
        End If
        Return " | Account=" & _processAccountNo
    End Function

    Public Sub Write(source As String, message As String, Optional ex As Exception = Nothing)
        Try
            Dim logPath = GetDailyLogPath("app")
            Dim sb As New StringBuilder()
            sb.AppendLine("==== " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " ====")
            sb.AppendLine("Source: " & source)
            sb.AppendLine(message)
            If ex IsNot Nothing Then
                sb.AppendLine(ex.ToString())
            Else
                sb.AppendLine(Environment.StackTrace)
            End If
            sb.AppendLine()
            File.AppendAllText(logPath, sb.ToString())
        Catch
        End Try
    End Sub

    Public Sub WriteCrash(source As String, ex As Exception)
        Try
            Dim logPath = GetDailyLogPath("crash")
            Dim sb As New StringBuilder()
            sb.AppendLine("==== " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " ====")
            sb.AppendLine("Source: " & source)
            sb.AppendLine(ex.ToString())
            sb.AppendLine()
            File.AppendAllText(logPath, sb.ToString())
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Starts a function log under
    ''' log\yyyy-MM-dd\{SessionKey}_{accountStamp}\{Function}_{ddMMyyyyHHmmss}_{SessionKey}\process.log
    ''' When forceNewAccountFolder is False and the same account run is active, reuses the account folder.
    ''' idLabel is "Account No" (default) or "Work Item Reference" for WIR-based sessions.
    ''' </summary>
    Public Sub StartProcess(functionName As String, accountNo As String, Optional forceNewAccountFolder As Boolean = False, Optional idLabel As String = "Account No")
        Try
            _processEnabled = IsProcessLogEnabled()
            _processLogPath = Nothing
            _processFunction = Nothing
            _processAccountNo = Nothing
            _processIdLabel = If(String.IsNullOrWhiteSpace(idLabel), "Account No", idLabel.Trim())

            If Not _processEnabled Then
                Return
            End If

            Dim funcPart = SanitizeFolderPart(functionName)
            Dim accPart = SanitizeFolderPart(If(String.IsNullOrWhiteSpace(accountNo), "NA", accountNo))
            Dim stamp = DateTime.Now.ToString("ddMMyyyyHHmmss")
            Dim dateFolder = Path.Combine(GetLogDirectory(), DateTime.Now.ToString("yyyy-MM-dd"))

            Dim needNewAccountFolder As Boolean =
                forceNewAccountFolder OrElse
                String.IsNullOrEmpty(_accountRunFolder) OrElse
                Not String.Equals(_accountRunKey, accPart, StringComparison.OrdinalIgnoreCase)

            If needNewAccountFolder Then
                Dim accountStamp = DateTime.Now.ToString("ddMMyyyyHHmmss")
                _accountRunKey = accPart
                _accountRunFolder = Path.Combine(dateFolder, accPart & "_" & accountStamp)
                _accountRunIdLabel = _processIdLabel
            Else
                ' Keep session label from the active account run
                _processIdLabel = If(String.IsNullOrWhiteSpace(_accountRunIdLabel), _processIdLabel, _accountRunIdLabel)
            End If

            Dim runFolder = Path.Combine(_accountRunFolder, funcPart & "_" & stamp & "_" & accPart)

            If Not Directory.Exists(runFolder) Then
                Directory.CreateDirectory(runFolder)
            End If

            _processLogPath = Path.Combine(runFolder, "process.log")
            _lastProcessLogPath = _processLogPath
            _processFunction = funcPart
            _processAccountNo = accPart

            Dim headerLabel = _processIdLabel & " :"
            ' Pad for alignment similar to existing "Account No :" layout
            If headerLabel.Length < 12 Then
                headerLabel = headerLabel.PadRight(12)
            End If

            Dim sb As New StringBuilder()
            sb.AppendLine("==== PROCESS START ====")
            sb.AppendLine("Time       : " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
            sb.AppendLine("Function   : " & funcPart)
            sb.AppendLine(headerLabel & " " & accPart)
            sb.AppendLine("Session Folder : " & _accountRunFolder)
            sb.AppendLine("Folder     : " & runFolder)
            sb.AppendLine("========================")
            File.AppendAllText(_processLogPath, sb.ToString())
        Catch
            _processEnabled = False
            _processLogPath = Nothing
        End Try
    End Sub

    Public Sub LogStep(message As String)
        WriteProcessLine(message, Nothing)
    End Sub

    Public Sub LogStep(message As String, ex As Exception)
        WriteProcessLine(message, ex)
    End Sub

    ''' <summary>
    ''' Logs an exception to the active process.log when a process session is open;
    ''' otherwise to the last process.log if available. Always also writes app_*.log.
    ''' </summary>
    Public Sub LogException(source As String, ex As Exception, Optional message As String = Nothing)
        Try
            Dim ctx = If(String.IsNullOrWhiteSpace(source), "Exception", source.Trim())
            Dim msg = If(String.IsNullOrWhiteSpace(message), ctx, message.Trim())
            If ex IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(ex.Message) Then
                msg = msg & " | " & ex.Message
            End If

            ' Prefer active process log
            If _processEnabled AndAlso Not String.IsNullOrEmpty(_processLogPath) Then
                WriteProcessLine(msg, ex)
            ElseIf Not String.IsNullOrEmpty(_lastProcessLogPath) AndAlso File.Exists(_lastProcessLogPath) Then
                ' Append into last process session after EndProcess
                Try
                    Dim line = "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "] Exception | " & msg
                    Dim sb As New StringBuilder()
                    sb.AppendLine(line)
                    If ex IsNot Nothing Then
                        sb.AppendLine("  Exception: " & ex.Message)
                        sb.AppendLine(ex.ToString())
                    End If
                    File.AppendAllText(_lastProcessLogPath, sb.ToString())
                Catch
                End Try
            End If

            Write(ctx, msg, ex)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Writes the UI status-box text into the active process log (or last session log) as Status.
    ''' </summary>
    Public Sub LogStatus(statusText As String)
        Try
            If Not IsProcessLogEnabled() Then
                Return
            End If

            Dim display = If(statusText, "")
            display = display.Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ").Trim()
            If String.IsNullOrEmpty(display) Then
                display = "(cleared)"
            End If

            Dim logPath As String = Nothing
            Dim funcName As String = Nothing
            Dim idSuffix As String = ""

            If _processEnabled AndAlso Not String.IsNullOrEmpty(_processLogPath) Then
                logPath = _processLogPath
                funcName = If(String.IsNullOrEmpty(_processFunction), "Process", _processFunction)
                idSuffix = StepIdSuffix()
            ElseIf Not String.IsNullOrEmpty(_lastProcessLogPath) AndAlso File.Exists(_lastProcessLogPath) Then
                logPath = _lastProcessLogPath
                funcName = "Status"
                If Not String.IsNullOrEmpty(_accountRunKey) Then
                    If IsWirLabel(_accountRunIdLabel) Then
                        idSuffix = " | WIR=" & _accountRunKey
                    Else
                        idSuffix = " | Account=" & _accountRunKey
                    End If
                End If
            ElseIf Not String.IsNullOrEmpty(_accountRunFolder) Then
                If Not Directory.Exists(_accountRunFolder) Then
                    Directory.CreateDirectory(_accountRunFolder)
                End If
                logPath = Path.Combine(_accountRunFolder, "status.log")
                funcName = "Status"
                If Not String.IsNullOrEmpty(_accountRunKey) Then
                    If IsWirLabel(_accountRunIdLabel) Then
                        idSuffix = " | WIR=" & _accountRunKey
                    Else
                        idSuffix = " | Account=" & _accountRunKey
                    End If
                End If
            Else
                Return
            End If

            Dim line = "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "] " &
                       funcName & " | Status : " & display & idSuffix

            File.AppendAllText(logPath, line & Environment.NewLine)
        Catch
        End Try
    End Sub

    Private Sub WriteProcessLine(message As String, ex As Exception)
        Try
            If Not _processEnabled OrElse String.IsNullOrEmpty(_processLogPath) Then
                Return
            End If

            Dim line = "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & "] " &
                       If(String.IsNullOrEmpty(_processFunction), "Process", _processFunction) & " | " & message
            line &= StepIdSuffix()

            Dim sb As New StringBuilder()
            sb.AppendLine(line)
            If ex IsNot Nothing Then
                sb.AppendLine("  Exception: " & ex.Message)
                sb.AppendLine(ex.ToString())
            End If
            File.AppendAllText(_processLogPath, sb.ToString())
        Catch
        End Try
    End Sub

    Public Sub EndProcess(resultSummary As String)
        Try
            If Not _processEnabled OrElse String.IsNullOrEmpty(_processLogPath) Then
                _processLogPath = Nothing
                _processFunction = Nothing
                Return
            End If

            Dim sb As New StringBuilder()
            sb.AppendLine("==== PROCESS END ====")
            sb.AppendLine("Time   : " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
            sb.AppendLine("Result : " & resultSummary)
            sb.AppendLine("======================")
            sb.AppendLine()
            File.AppendAllText(_processLogPath, sb.ToString())
        Catch
        Finally
            ' Keep last path so late exceptions can still append to this process.log
            If Not String.IsNullOrEmpty(_processLogPath) Then
                _lastProcessLogPath = _processLogPath
            End If
            ' Keep _accountRunFolder / _accountRunKey so later functions reuse the same account run folder.
            _processEnabled = False
            _processLogPath = Nothing
            _processFunction = Nothing
            _processAccountNo = Nothing
            _processIdLabel = "Account No"
        End Try
    End Sub

    ''' <summary>
    ''' Clears the active account run folder so the next StartProcess creates a new AccountNo_timestamp folder.
    ''' </summary>
    Public Sub EndAccountSession()
        _accountRunKey = Nothing
        _accountRunFolder = Nothing
        _accountRunIdLabel = "Account No"
        _lastProcessLogPath = Nothing
    End Sub

    ''' <summary>
    ''' Logs that the user closed the application mid-process. Safe to call multiple times.
    ''' </summary>
    Public Sub LogUserClosedApplication()
        Try
            If _userCloseLogged Then
                Return
            End If
            _userCloseLogged = True

            Dim hadActiveProcess = _processEnabled AndAlso Not String.IsNullOrEmpty(_processLogPath)
            Dim hadAccountRun = Not String.IsNullOrEmpty(_accountRunFolder)

            If Not hadActiveProcess AndAlso Not hadAccountRun Then
                ' Nothing in progress; still allow a Close entry if process logging is enabled
                If Not IsProcessLogEnabled() Then
                    Return
                End If
                StartProcess("Close", "NA", True)
                LogStep("Application closed by user")
                EndProcess("Application closed by user")
                EndAccountSession()
                Return
            End If

            If hadActiveProcess Then
                LogStep("Application closed by user mid-process")
                EndProcess("Aborted - application closed by user")
            ElseIf hadAccountRun Then
                Dim key = If(String.IsNullOrEmpty(_accountRunKey), "NA", _accountRunKey)
                Dim label = If(String.IsNullOrWhiteSpace(_accountRunIdLabel), "Account No", _accountRunIdLabel)
                StartProcess("Close", key, False, label)
                LogStep("Application closed by user mid-process (no active function log)")
                EndProcess("Aborted - application closed by user")
            End If

            EndAccountSession()
        Catch
        End Try
    End Sub

End Module
