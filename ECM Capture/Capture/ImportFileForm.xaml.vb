Imports ezofis.UserControl.CAC
Imports System.Data
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.IO
Imports ezofis.UserControl.PublicVariable

Public Class ImportFileForm
    Dim CAC As New CACserviceClient
    Dim _filelist As New List(Of IndexingField)
    Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("Database"), NameValueCollection)
    Dim _Selected As Integer = 0
    Dim _imported As Integer = 0
    Dim _selectedBatch As Integer = 0
    Dim DocumentFrom As Integer = 1
    Public Property filelist() As List(Of IndexingField)
        Get
            Return _filelist
        End Get
        Protected Set(value As List(Of IndexingField))
            _filelist = value
        End Set
    End Property

    Public Property Selected() As Integer
        Get
            Return _Selected
        End Get
        Protected Set(value As Integer)
            _Selected = value
        End Set
    End Property
    Public Property imported() As Integer
        Get
            Return _imported
        End Get
        Protected Set(value As Integer)
            _imported = value
        End Set
    End Property
    Public Property selectedBatch() As Integer
        Get
            Return _selectedBatch
        End Get
        Protected Set(value As Integer)
            _selectedBatch = value
        End Set
    End Property



    Private Sub BtnOk_Click(sender As Object, e As RoutedEventArgs) Handles BtnOk.Click
        Try
            imported = 0
            selectedBatch = DG1.SelectedItems.Count
            For Each rowview As DataRowView In DG1.SelectedItems
                If rowview IsNot Nothing Then
                    Dim ds As DataSet = CAC.GetDatasetByQuery("select * from ezBatchProcessing WITH (NOLOCK) where BatchId='" + rowview.Row.ItemArray(1).ToString() + "' and Status=2 and ProcessedPath like '" + Appcon("ProcessedPath") + "%'")
                    If Not IsNothing(ds) Then
                        If ds.Tables.Count <> 0 Then
                            If ds.Tables(0).Rows.Count <> 0 Then
                                If CAC.InsertAndUpdate("update ezBatchProcessing set Status=3,ImportedAt='" + Environment.MachineName.ToString() + "',[ImportedOn]='" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + "',Importedby='" + ecmlogin.ECMLoginId.ToString() + "' where BatchId='" + rowview.Row.ItemArray(1).ToString() + "'") > 0 Then
                                    Dim dirinfo As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(ds.Tables(0).Rows(0)("ProcessedPath"))
                                    'filelist.Add(rowview.Row.ItemArray(9).ToString())
                                    For Each fileinfo_loopVariable As System.IO.FileInfo In dirinfo.GetFiles("*.tif", SearchOption.AllDirectories)

                                        Dim dirname = fileinfo_loopVariable.FullName.Replace("\" + System.IO.Path.GetFileName(fileinfo_loopVariable.FullName), "")
                                        ' MessageBox.Show(dirname)
                                        dirname = System.IO.Path.GetFileName(dirname)
                                        '  MessageBox.Show(dirname)
                                        If rowview.Row.ItemArray(1).ToString() = dirname Then
                                            Dim rec As New IndexingField
                                            rec.FieldName = fileinfo_loopVariable.FullName
                                            rec.BatchId = rowview.Row.ItemArray(1).ToString()
                                            filelist.Add(rec)
                                            ' filelist.Add(fileinfo_loopVariable.FullName)
                                            Dim QueryU = "insert into eZBatchFiles([Filename],[RIMNumber],[BatchId])values('" + fileinfo_loopVariable.Name.ToString() + "','','" + rowview.Row.ItemArray(1).ToString() + "')"
                                            '   MessageBox.Show(QueryU)
                                            If CAC.InsertAndUpdate(QueryU) > 0 Then

                                            End If
                                            imported = imported + 1
                                        ElseIf Path.GetFileNameWithoutExtension(fileinfo_loopVariable.FullName).Contains(rowview.Row.ItemArray(1).ToString()) Then
                                            Dim rec As New IndexingField
                                            rec.FieldName = fileinfo_loopVariable.FullName
                                            rec.BatchId = rowview.Row.ItemArray(1).ToString()
                                            filelist.Add(rec)

                                            ' filelist.Add(fileinfo_loopVariable.FullName)
                                            Dim QueryU = "insert into eZBatchFiles([Filename],[RIMNumber],[BatchId])values('" + fileinfo_loopVariable.Name.ToString() + "','','" + rowview.Row.ItemArray(1).ToString() + "')"
                                            '   MessageBox.Show(QueryU)
                                            If CAC.InsertAndUpdate(QueryU) > 0 Then

                                            End If
                                            imported = imported + 1
                                        End If



                                    Next

                                    'Dim selval = cmbprocesstype.SelectedValue.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "")

                                    'If selval = "Not Detected" Then
                                    '    For Each fileinfo_loopVariable As System.IO.FileInfo In dirinfo.GetFiles("*.tif", SearchOption.AllDirectories)

                                    '        Dim dirname = fileinfo_loopVariable.FullName.Replace("\" + System.IO.Path.GetFileName(fileinfo_loopVariable.FullName), "")
                                    '        '  MessageBox.Show(dirname)
                                    '        dirname = System.IO.Path.GetFileName(dirname)
                                    '        ' MessageBox.Show(dirname)
                                    '        If Path.GetFileNameWithoutExtension(fileinfo_loopVariable.FullName).Contains(rowview.Row.ItemArray(1).ToString()) Then
                                    '            Dim rec As New IndexingField
                                    '            rec.FieldName = fileinfo_loopVariable.FullName
                                    '            rec.BatchId = rowview.Row.ItemArray(1).ToString()
                                    '            filelist.Add(rec)

                                    '            ' filelist.Add(fileinfo_loopVariable.FullName)
                                    '            Dim QueryU = "insert into eZBatchFiles([Filename],[RIMNumber],[BatchId])values('" + fileinfo_loopVariable.Name.ToString() + "','','" + rowview.Row.ItemArray(1).ToString() + "')"
                                    '            '   MessageBox.Show(QueryU)
                                    '            If CAC.InsertAndUpdate(QueryU) > 0 Then

                                    '            End If
                                    '            imported = imported + 1
                                    '        End If

                                    '    Next
                                    'Else

                                    'End If
                                End If

                            End If
                        End If
                    End If

                End If
            Next
            DialogResult = True


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As RoutedEventArgs) Handles BtnCancel.Click
        Try
            DialogResult = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Try
            '  Dim lst = CAC.GetDBCONFIG()

            Dim Cond As String = ""

            If DocumentFrom = 2 Then
                Cond = "and ScanedFile Not like '%DCMS%'"
            Else
                Cond = "and ScanedFile like '%DCMS%'"
            End If
            Dim ds As DataSet = CAC.GetDatasetByQuery("select ROW_NUMBER() OVER (ORDER BY BatchId) as SNO, [BatchId],[TotalDoc] as [No of Barcode],TotalScanedPage as [Total Page],ScanedAt as ScannedPC,[ScanedOn] as ScannedOn,[ProcessedPath] from ezBatchProcessing WITH (NOLOCK) where Status=2 and [Review]='Detected' and ProcessedPath like '" + Appcon("ProcessedPath") + "%' " + Cond)
            If Not IsNothing(ds) Then
                DG1.ItemsSource = ds.Tables(0).DefaultView
            Else
                DG1.ItemsSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub DG1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DG1.SelectionChanged
        Try
            Selected = 0

            lblresultBatch.Content = "No. of Batch Selected : " + DG1.SelectedItems.Count.ToString()
            For Each rowview As DataRowView In DG1.SelectedItems
                If rowview IsNot Nothing Then
                    Try
                        Selected = Selected + Convert.ToInt32(rowview.Row.ItemArray(2).ToString())
                        lblresult.Content = "No. of Files Selected : " + Selected.ToString()
                    Catch ex As Exception

                    End Try

                End If
            Next


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Cmbprocesstype_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbprocesstype.SelectionChanged
        Try

            '  Dim selval = cmbprocesstype.SelectedValue.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "")

            LoadGrid()


            'If selval = "Not Detected" Then
            '    Dim ds As DataSet = CAC.GetDatasetByQuery("select ROW_NUMBER() OVER (ORDER BY BatchId) as SNO, [BatchId],[TotalDoc] as [No of Barcode],TotalScanedPage as [Total Page],ScanedAt as ScannedPC,[ScanedOn] as ScannedOn,[ProcessedPath] from ezBatchProcessing where Status=2 and [Review]='Not Detected' and ProcessedPath like '" + Appcon("ProcessedPath") + "%'")
            '    If Not IsNothing(ds) Then
            '        DG1.ItemsSource = ds.Tables(0).DefaultView
            '    Else
            '        DG1.ItemsSource = Nothing
            '    End If

            'ElseIf selval = "Detected" Then
            '    Dim ds As DataSet = CAC.GetDatasetByQuery("select ROW_NUMBER() OVER (ORDER BY BatchId) as SNO, [BatchId],[TotalDoc] as [No of Barcode],TotalScanedPage as [Total Page],ScanedAt as ScannedPC,[ScanedOn] as ScannedOn,[ProcessedPath] from ezBatchProcessing where Status=2 and [Review]='Detected' and ProcessedPath like '" + Appcon("ProcessedPath") + "%'")
            '    If Not IsNothing(ds) Then
            '        DG1.ItemsSource = ds.Tables(0).DefaultView
            '    Else
            '        DG1.ItemsSource = Nothing
            '    End If
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub LoadGrid()
        Try
            Dim selval = cmbprocesstype.SelectedValue.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "")
            Dim Cond As String = ""

            If DocumentFrom = 2 Then
                Cond = "and ScanedFile Not like '%DCMS%'"
            Else
                Cond = "and ScanedFile like '%DCMS%'"
            End If

            If selval = "Not Detected" Then
                Dim ds As DataSet = CAC.GetDatasetByQuery("select ROW_NUMBER() OVER (ORDER BY BatchId) as SNO, [BatchId],[TotalDoc] as [No of Barcode],TotalScanedPage as [Total Page],ScanedAt as ScannedPC,[ScanedOn] as ScannedOn,[ProcessedPath] from ezBatchProcessing WITH (NOLOCK) where Status=2 And [Review]='Not Detected' and ProcessedPath like '" + Appcon("ProcessedPath") + "%' " + Cond)
                If Not IsNothing(ds) Then
                    DG1.ItemsSource = ds.Tables(0).DefaultView
                Else
                    DG1.ItemsSource = Nothing
                End If

            ElseIf selval = "Detected" Then
                Dim ds As DataSet = CAC.GetDatasetByQuery("select ROW_NUMBER() OVER (ORDER BY BatchId) as SNO, [BatchId],[TotalDoc] as [No of Barcode],TotalScanedPage as [Total Page],ScanedAt as ScannedPC,[ScanedOn] as ScannedOn,[ProcessedPath] from ezBatchProcessing WITH (NOLOCK) where Status=2 and [Review]='Detected' and ProcessedPath like '" + Appcon("ProcessedPath") + "%'" + Cond)
                If Not IsNothing(ds) Then
                    DG1.ItemsSource = ds.Tables(0).DefaultView
                Else
                    DG1.ItemsSource = Nothing
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub rbtscanned_Checked(sender As Object, e As RoutedEventArgs)
        Try
            If rbtDigital.IsChecked Then
                DocumentFrom = 2
            ElseIf rbtscanned.IsChecked Then
                DocumentFrom = 1

            End If
            LoadGrid()
        Catch ex As Exception

        End Try
    End Sub
End Class
