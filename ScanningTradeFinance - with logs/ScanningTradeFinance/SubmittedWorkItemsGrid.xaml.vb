Imports System.Data
Imports ScanningTradeFinance.publicvariables

Public Class SubmittedWorkItemsGrid
    Public GRIDPHASE As String = ""
    Public GRIDPRODUCT As String = ""
    Public GRIDWIR As String = ""
    Public GRIDTYPE As String = ""
    Public TotalNoOfRec As Integer = 0

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            SubmittedworkItemFlag = 0
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

    Public Sub New(ByRef ResTicketQueue As DataSet)

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        gridSubmittedDocuments.ItemsSource = ResTicketQueue.Tables(0).DefaultView


    End Sub


    Public Sub OnCellHyperlinkChoose_Click(sender As Object, e As RoutedEventArgs)
        'Dim textblock As TextBlock = CType(sender, TextBlock)
        'Dim id As String = CStr(textblock.Tag)

        'MsgBox("WORK ITEM REF" & id)
        Dim selectedFile As DataRowView = gridSubmittedDocuments.SelectedItem
        If selectedFile Is Nothing Then
            SubmittedworkItemFlag = 0
            MessageBox.Show("Please select a work item.", "Submitted Documents",
                            MessageBoxButton.OK, MessageBoxImage.Information)
            Return
        End If

        SubmittedworkItemFlag = 1
        GRIDWIR = Convert.ToString(selectedFile.Row.ItemArray(0))
        GRIDPRODUCT = Convert.ToString(selectedFile.Row.ItemArray(2))
        GRIDPHASE = Convert.ToString(selectedFile.Row.ItemArray(3))
        GRIDTYPE = Convert.ToString(selectedFile.Row.ItemArray(4))
        TotalNoOfRec = gridSubmittedDocuments.Items.Count

        'MsgBox("PRODUCT : " & PRODUCT & "  ,   PHASE : " & PHASE)

        Me.Close()

        'handle the event as before
    End Sub

    Private Sub ContinueAsNew_Click(sender As Object, e As RoutedEventArgs)
        Try
            SubmittedworkItemFlag = 0
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class
