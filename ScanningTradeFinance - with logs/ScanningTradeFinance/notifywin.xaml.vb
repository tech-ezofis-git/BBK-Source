Imports ScanningTradeFinance.publicvariables

Public Class notifywin

    Public FinalSubmissionPath As String = ""
    Public Sub New(FinalSubmissionPath As String, feProduct As String, fePhase As String, feAccNo As String, cabinetid As String)
        Try


            ' This call is required by the designer.
            InitializeComponent()
            lblworkitem.Content = WorkItmref
            lblpags.Content = Pagescou.ToString()
            lblFinalSubmission.Text = feAccNo & " / " & feProduct & " / " & fePhase

            Dim qry = "select [DOCUMENT TYPE] + Case When [MANDATORY]='Mandatory' then ' *' else '' end as [DOCUMENT TYPE], [MANDATORY],[itemid],
case when itemid Is NULL then '-' 
Else 'RECEIVED' 
End As RECEIVED 
from
(SELECT R1.[DOCUMENT TYPE], R1.[MANDATORY], itemid 
from (Select  distinct [Document Type] [DOCUMENT TYPE],
Case WHEN Mandatory = 'true' THEN 'Mandatory' 
Else '-' End As  [MANDATORY] 
FROM [ezfb_Product CheckList Master]  where [Product]='" & feProduct & "' and [Phase]='" & fePhase & "') as r1 Left Join ezca_" & cabinetid.ToString & "_" & MainWindow.invitaAPIobj.TemplateId & "_items as itemtbl on r1.[Document Type]=itemtbl.[Document Type] And [Work Item Reference]='" & WorkItmref & "') as r2 order by MANDATORY desc"

            Dim ResDs1 = MainWindow.invitaAPIobj.GetDatasetByQuery(qry)

            If ResDs1 IsNot Nothing AndAlso ResDs1.Tables.Count > 0 AndAlso ResDs1.Tables(0).Rows.Count > 0 Then
                Dim allreceivedflag = True
                gridMandateDocuments.ItemsSource = ResDs1.Tables(0).DefaultView()

                For i = 0 To ResDs1.Tables(0).Rows.Count - 1
                    ' If (ResDs1.Tables(0).Rows(i).Item("MANDATORY").ToString.ToUpper = "MANDATORY") Then
                    If (ResDs1.Tables(0).Rows(i).Item("RECEIVED").ToString.ToUpper = "-") Then
                        allreceivedflag = False
                    End If
                    ' End If
                Next

                If (allreceivedflag = False) Then
                    btnContinue.IsEnabled = True
                Else
                    btnContinue.IsEnabled = False
                End If
            End If

        Catch ex As Exception
            MsgBox("Exception in notifywin" & ex.Message)
        End Try
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            CanContinue = 1
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mainhead_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Try
            Application.Current.MainWindow.DragMove()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnNewWorkItem_Click(sender As Object, e As RoutedEventArgs)
        Try
            CanContinue = 2
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnContinue_Click(sender As Object, e As RoutedEventArgs)
        Try
            CanContinue = 1
            Me.DialogResult = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnclipboard_Click(sender As Object, e As RoutedEventArgs)
        Try
            Clipboard.SetData(DataFormats.Text, WorkItmref)

        Catch ex As Exception

        End Try
    End Sub
End Class
