Imports System.ComponentModel
Imports System.Configuration
Imports System.IO
Imports System.Collections.Specialized
Imports System.ServiceModel.Description
Imports System.ServiceModel
Public Class ServiceRef
    Private Sub btn_save_Click(sender As System.Object, e As System.EventArgs) Handles btn_save.Click
        Try
            Dim Url As String = txt_ServiceUrl.Text
            ConfigSettings.SaveEndpointAddress(Url)
            DialogResult = Forms.DialogResult.OK
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ServiceRef_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim StrIp As String = String.Empty
        Try
            StrIp = ConfigSettings.loadConfigDocument().SelectSingleNode("//system.serviceModel//client//endpoint").Attributes("address").Value.ToString.ToUpper
            StrIp = Replace(StrIp, "/", "")
            StrIp = Replace(StrIp, "EZOFISSERVICECACSERVICE.SVC", "")
            StrIp = Replace(StrIp, "HTTP:", "")
            txt_ServiceUrl.Text = StrIp.ToLower
        Catch ex As Exception
        End Try
    End Sub
End Class
