Class Application

    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        AddHandler DispatcherUnhandledException, AddressOf Application_DispatcherUnhandledException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf CurrentDomain_UnhandledException
        MyBase.OnStartup(e)
    End Sub

    Protected Overrides Sub OnExit(e As ExitEventArgs)
        Try
            AppLogger.LogUserClosedApplication()
        Catch
        End Try
        MyBase.OnExit(e)
    End Sub

    Private Sub Application_DispatcherUnhandledException(sender As Object, e As System.Windows.Threading.DispatcherUnhandledExceptionEventArgs)
        Try
            LogUnhandledException("DispatcherUnhandledException", e.Exception)
            MessageBox.Show("An unexpected error occurred. Details were written to log\crash_" & DateTime.Now.ToString("yyyy-MM-dd") & ".log." & vbCrLf & e.Exception.Message,
                            "ScanningTradeFinance", MessageBoxButton.OK, MessageBoxImage.Error)
            e.Handled = True
        Catch
        End Try
    End Sub

    Private Sub CurrentDomain_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        Try
            Dim ex = TryCast(e.ExceptionObject, Exception)
            If ex IsNot Nothing Then
                LogUnhandledException("UnhandledException", ex)
            Else
                LogUnhandledException("UnhandledException", New Exception(e.ExceptionObject.ToString()))
            End If
        Catch
        End Try
    End Sub

    Private Shared Sub LogUnhandledException(source As String, ex As Exception)
        Try
            AppLogger.LogException(source, ex, "Unhandled exception")
        Catch
        End Try
        Try
            AppLogger.WriteCrash(source, ex)
        Catch
        End Try
    End Sub

End Class
