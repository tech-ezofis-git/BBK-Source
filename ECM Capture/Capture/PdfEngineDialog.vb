Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Diagnostics

Partial Public Class PdfEngineDialog : Inherits Form
    Public Sub New()
        DialogUtilities.RunFPU()

        InitializeComponent()
    End Sub

    Private Sub _btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnOk.Click
        If _rbCancel.Checked Then
            DialogResult = System.Windows.Forms.DialogResult.Cancel
        Else
            DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub _lbEngine_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles _lbEngine.LinkClicked
        Process.Start(_lbEngine.Text)
    End Sub
End Class
