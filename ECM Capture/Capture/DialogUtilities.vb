Imports System
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public NotInheritable Class DialogUtilities
    Private Sub New()
    End Sub

    Public Shared Function ParseInteger(ByVal textBox As TextBox, ByVal name As String, ByVal min As Integer, ByVal useMin As Boolean, ByVal max As Integer, ByVal useMax As Boolean, ByVal cancelDialog As Boolean, ByRef value As Integer) As Boolean
        Try
            value = Integer.Parse(textBox.Text)

            If (useMin AndAlso value < min) Then
                Return Fail(textBox.FindForm(), cancelDialog, String.Format("'{0}' should not be less than {1}", name, min))
            End If

            If (useMax AndAlso value > max) Then
                Return Fail(textBox.FindForm(), cancelDialog, String.Format("'{0}' should not be greater than {1}", name, max))
            End If

            Return True
        Catch ex As Exception
            value = 0
            Return Fail(textBox.FindForm(), cancelDialog, ex.Message)
        End Try
    End Function

    Private Shared Function Fail(ByVal form As Form, ByVal cancelDialog As Boolean, ByVal message As String) As Boolean
        Messager.ShowWarning(form, message)

        If (cancelDialog) Then
            form.DialogResult = DialogResult.None
        End If

        Return False
    End Function

    Public Shared Sub NumericOnLeave(ByVal sender As Object)
        Dim num As NumericUpDown = CType(sender, NumericUpDown)
        If (num.Value < num.Minimum) Then
            num.Value = num.Minimum
        ElseIf (num.Value > num.Maximum) Then
            num.Value = num.Maximum
        End If
    End Sub

    Public Shared Sub SetNumericValue(ByVal num As NumericUpDown, ByVal value As Integer)
        num.Value = Math.Max(num.Minimum, Math.Min(num.Maximum, value))
    End Sub

    ' Fix for the font issue in Windows 98 (Q326219)
    <DllImport("msvcrt.dll")> Private Shared Function _controlfp(ByVal IN_New As Integer, ByVal IN_Mask As Integer) As Integer
    End Function

    Private Const _MCW_EW As Integer = &H8001F
    Private Const _EM_INVALID As Integer = &H10

    Public Shared Sub RunFPU()
        Try
            _controlfp(_MCW_EW, _EM_INVALID)
        Catch
        End Try
    End Sub

    ' System.Windows.Forms.PrintPreviewDialog has a bug on Windows 98 that causes a crash.  Search groups.google.com for an explination and a potentional fix
    Public Shared ReadOnly Property CanRunPrintPreview() As Boolean
        Get
            Dim os As OperatingSystem = Environment.OSVersion
            Return (os.Platform <> PlatformID.Win32Windows)
        End Get
    End Property
End Class
