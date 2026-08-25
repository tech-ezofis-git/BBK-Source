Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Web.Script.Serialization
Imports System.Windows
Imports System.Windows.Controls
Imports Newtonsoft.Json



Public Class Batching
    Private Const GWL_STYLE As Integer = -16
    Private Const WS_SYSMENU As Integer = &H80000
    Dim jsonpath As String
    Dim passwordjson
    Dim status As String
    Dim noffiles As Integer
    Dim fsize As String
    Dim namefile As String
    Dim exts As String
    Dim Batchid As String
    Dim dttime As String
    Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
    Dim Appcon As NameValueCollection = DirectCast(System.Configuration.ConfigurationManager.GetSection("Database"), NameValueCollection)
    Dim jspath = Appcon("Jsonpath")
    Dim dwnpath = Appcon("Downloadjpath")
    Dim dt As DataTable
    Public notifiy As New Notification
    Dim file1 As IO.StreamWriter

    Dim batchno
    Public checkprocess As Boolean
    Public clickdecrypy As String
    Public clickdownload As String

    Dim custommsgbox As New CustomMessageBoxControl
    Public Sub New()
        Try
            InitializeComponent()
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Class folderinfo
        Property foldername As String
        Property pass
        Property foldersize As String
        Property Nooffiles As Integer
        Property batchid As String
        Property datime As String
        Property status As String
        Property NooffilesProcessed As Integer
        Property NooffilesUnProcessed As Integer
    End Class


    Public Class downloadfile
        Property foldername As String
        Property passwordd As String
        Property dfoldersize As String
        Property Nooffiles As Integer
        Property batchid As String
        Property datime As String
        Property status As String
        Property NooffilesProcessed As Integer
        Property NooffilesUnProcessed As Integer
    End Class

    Public Class downloadin
        Public Property infos As List(Of downloadfile)
    End Class

    Public Class folderin
        Public Property info As List(Of folderinfo)
    End Class



    Private Sub Batching_Loaded(sender As Object, e As Windows.RoutedEventArgs)
        Try
            Dim dataset As New DataSet
            Resultgrid.ItemsSource = Nothing
            For del As Integer = Resultgrid.Columns.Count - 1 To 1 Step -1
                Resultgrid.Columns.RemoveAt(del)
            Next
            'If clickdecrypy = "Decrypt" Then
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            Dim uristring = File.ReadAllText(jspath)
            If String.IsNullOrEmpty(uristring) Then
                Try
                    Dim savepaths = System.Reflection.Assembly.GetEntryAssembly().Location
                    savepaths = Path.GetDirectoryName(savepaths)
                    Dim sources = savepaths + "\downloadjson"
                    Dim filelocation = sources & "\" & "Downloaddata.txt"
                    If File.Exists(filelocation) Then
                        Dim content = File.ReadAllText(filelocation).Trim()
                        uristring = content
                    End If
                Catch ex As Exception

                End Try
            End If
            Dim fileinfos As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring)
            DeserializeDataTable(uristring)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Rows.Count <> 0 Then
                Dim dv As DataView = New DataView(dt)
                If dv IsNot Nothing AndAlso dv.Count > 0 Then
                    For i As Integer = 0 To dt.Columns.Count - 1
                        Dim DGC As New System.Windows.Controls.DataGridTextColumn()
                        DGC.CanUserSort = True
                        DGC.IsReadOnly = True
                        DGC.SortMemberPath = dv.Table.Columns(i).ColumnName.ToString()
                        DGC.Header = dv.Table.Columns(i).ColumnName.ToString()
                        Dim binding As System.Windows.Data.Binding = New System.Windows.Data.Binding(String.Format("[{0}]", dv.Table.Columns(i).ColumnName.ToString()))
                        binding.Mode = System.Windows.Data.BindingMode.TwoWay
                        DGC.Binding = binding
                        Resultgrid.Columns.Add(DGC)
                    Next
                    'Resultgrid.ItemsSource = dv

                    'For Each col In Resultgrid.Columns
                    '    If col.Header = "batchid" Or col.Header = "Filename" Or col.Header = "filesize" Or col.Header = "Nooffiles" Or col.Header = "extension" Or col.Header = "pass" Or col.Header = "ezofile" Or col.Header = "foldersize" Then
                    '        col.Visibility = System.Windows.Visibility.Hidden
                    '    End If
                    'Next
                End If
            End If
            'ElseIf clickdownload = "download" Then
            'Dim client = New WebClient()
            'client.Headers("Content-Type") = "application/json"
            'client.Encoding = System.Text.Encoding.UTF8
            Dim uristrings = File.ReadAllText(dwnpath)
            Dim fileinfoss As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring)
            DeserializeDataTable(uristrings)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Rows.Count <> 0 Then
                Dim dv As DataView = New DataView(dt)
                If dv IsNot Nothing AndAlso dv.Count > 0 Then
                    For i As Integer = 0 To dt.Columns.Count - 1
                        Dim DGC As New System.Windows.Controls.DataGridTextColumn()
                        DGC.CanUserSort = True
                        DGC.IsReadOnly = True
                        DGC.SortMemberPath = dv.Table.Columns(i).ColumnName.ToString()
                        DGC.Header = dv.Table.Columns(i).ColumnName.ToString()
                        Dim binding As System.Windows.Data.Binding = New System.Windows.Data.Binding(String.Format("[{0}]", dv.Table.Columns(i).ColumnName.ToString()))
                        binding.Mode = System.Windows.Data.BindingMode.TwoWay
                        DGC.Binding = binding
                        Resultgrid.Columns.Add(DGC)
                    Next

                    'Dim dtview As New DataView(dtresult)
                    Dim sortstring As String = "Date and Time DESC" ' sorting in descending manner 
                    dv.Sort = sortstring

                    ' dtresult.AsEnumerable().OrderByDescending(Function(r) CDate(Format(r("Date and Time").ToString.Trim, "MM/dd/yyyyhhmmss"))).CopyToDataTable
                    'dtresult.DefaultView.Sort = "Date and Time desc"
                    Dim dtsoredtable As New DataTable
                    dtsoredtable = dv.ToTable
                    Resultgrid.ItemsSource = dtsoredtable.DefaultView

                    'Resultgrid.ItemsSource = dv
                    For Each col In Resultgrid.Columns
                        If col.Header = "batchid" Or col.Header = "Filename" Or col.Header = "filesize" Or col.Header = "Nooffiles" Or col.Header = "extension" Or col.Header = "ezofile" Or col.Header = "foldersize" Then
                            col.Visibility = System.Windows.Visibility.Hidden
                        End If
                    Next
                End If
            End If
            'End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Function DeserializeDataTable(json As String) As DataTable
        dt = TryCast(JsonConvert.DeserializeObject(json, (GetType(DataTable))), DataTable)
        Return dt
    End Function

    Private Sub btnbatchid_Click(sender As Object, e As Windows.RoutedEventArgs)

        Try
            batchno = TryCast(e.OriginalSource, Button).Content.ToString
            Dim notifications As Notification = New Notification()
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            If (File.Exists(jspath)) Then
                Dim uristring = File.ReadAllText(jspath)
                Dim fileinfos As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring)
                Dim info As New List(Of folderinfo)
                If fileinfos.Count > 0 Then
                    For k = 0 To fileinfos.Count - 1
                        Dim a As New folderinfo
                        a.foldername = fileinfos(k).foldername
                        a.pass = fileinfos(k).pass
                        a.foldersize = fileinfos(k).foldersize
                        a.status = fileinfos(k).status
                        a.Nooffiles = fileinfos(k).Nooffiles
                        a.datime = fileinfos(k).datime
                        a.batchid = fileinfos(k).batchid
                        a.NooffilesProcessed = fileinfos(k).NooffilesProcessed
                        a.NooffilesUnProcessed = fileinfos(k).NooffilesUnProcessed
                        info.Add(a)
                    Next
                End If
                For Each item As folderinfo In info
                    If item.batchid = batchno Then
                        notifications.BatchidValue.Content = item.batchid
                        notifications.processedtype.Content = "Decrypt"
                        notifications.Filescount.Content = item.Nooffiles
                        notifications.countfiles.Content = item.NooffilesProcessed
                        notifications.countunprocessedfiles.Content = item.NooffilesUnProcessed
                        If notifications.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                        End If
                        Exit For
                    End If
                Next
            End If
            If (File.Exists(dwnpath)) Then
                Dim uristring = File.ReadAllText(dwnpath)
                Dim fileinfoss As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring)
                Dim infos As New List(Of downloadfile)
                If fileinfoss.Count > 0 Then
                    For k = 0 To fileinfoss.Count - 1
                        Dim a As New downloadfile
                        a.foldername = fileinfoss(k).foldername
                        a.status = fileinfoss(k).status
                        a.dfoldersize = fileinfoss(k).dfoldersize
                        a.Nooffiles = fileinfoss(k).Nooffiles
                        a.datime = fileinfoss(k).datime
                        a.batchid = fileinfoss(k).batchid
                        a.NooffilesProcessed = fileinfoss(k).NooffilesProcessed
                        a.NooffilesUnProcessed = fileinfoss(k).NooffilesUnProcessed
                        infos.Add(a)
                    Next
                End If
                For Each item As downloadfile In infos
                    If item.batchid = batchno Then
                        notifications.BatchidValue.Content = item.batchid
                        notifications.processedtype.Content = "Download"
                        notifications.Filescount.Content = item.Nooffiles
                        notifications.countfiles.Content = item.NooffilesProcessed
                        notifications.countunprocessedfiles.Content = item.NooffilesUnProcessed
                        If notifications.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                        End If
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub Window_Loaded(sender As Object, e As Windows.RoutedEventArgs)
        Try
            Dim dataset As New DataSet
            Resultgrid.ItemsSource = Nothing
            For del As Integer = Resultgrid.Columns.Count - 1 To 1 Step -1
                Resultgrid.Columns.RemoveAt(del)
            Next
            LabelBatch.Content = "Batching Result"
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            Dim dtresult As New DataTable
            dtresult.Columns.Add("Operation")
            dtresult.Columns.Add("batchid")
            dtresult.Columns.Add("Folderpath")
            dtresult.Columns.Add("Date and time", System.Type.GetType("System.DateTime"))
            dtresult.Columns.Add("status")
            For i As Integer = 0 To dtresult.Columns.Count - 1
                Dim DGC As New System.Windows.Controls.DataGridTextColumn()
                DGC.CanUserSort = True
                DGC.IsReadOnly = True
                DGC.SortMemberPath = dtresult.Columns(i).ToString()
                DGC.Header = dtresult.Columns(i).ToString()
                Dim binding As System.Windows.Data.Binding = New System.Windows.Data.Binding(String.Format("[{0}]", dtresult.Columns(i).ToString()))
                binding.Mode = System.Windows.Data.BindingMode.TwoWay
                DGC.Binding = binding
                Resultgrid.Columns.Add(DGC)
            Next
            If (File.Exists(jspath)) Then
                Dim uristring = File.ReadAllText(jspath)
                If String.IsNullOrEmpty(uristring) Then
                    Try
                        Dim savepaths = System.Reflection.Assembly.GetEntryAssembly().Location
                        savepaths = Path.GetDirectoryName(savepaths)
                        Dim sources = savepaths + "\downloadjson"
                        Dim filelocation = sources & "\" & "Downloaddata.txt"
                        If File.Exists(filelocation) Then
                            Dim content = File.ReadAllText(filelocation).Trim()
                            uristring = content
                        End If
                    Catch ex As Exception

                    End Try
                End If
                Dim fileinfos As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring)
                DeserializeDataTable(uristring)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Rows.Count <> 0 Then
                    For Each row As DataRow In dt.Rows
                        Dim dtrow = dtresult.NewRow
                        dtrow("Operation") = "Decrypt"
                        dtrow("batchid") = row("batchid")
                        dtrow("FolderPath") = row("foldername")
                        dtrow("Date and Time") = Convert.ToDateTime(row("datime"))
                        dtrow("Status") = row("status")
                        dtresult.Rows.Add(dtrow)
                    Next
                End If
            End If

            If (File.Exists(dwnpath)) Then
                Dim uristring = File.ReadAllText(dwnpath)
                Dim downloadinfo As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring)
                DeserializeDataTable(uristring)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Rows.Count <> 0 Then
                    For Each row As DataRow In dt.Rows
                        Dim dtrow = dtresult.NewRow
                        dtrow("Operation") = "Download"
                        dtrow("batchid") = row("batchid")
                        dtrow("FolderPath") = row("foldername")
                        dtrow("Date and Time") = Convert.ToDateTime(row("datime"))
                        dtrow("Status") = row("status")
                        dtresult.Rows.Add(dtrow)
                    Next
                End If
            End If

            Dim dtview As New DataView(dtresult)
            Dim sortstring As String = "Date and Time DESC" ' sorting in descending manner 
            dtview.Sort = sortstring

            Dim dtsoredtable As New DataTable
            dtsoredtable = dtview.ToTable
            Resultgrid.ItemsSource = dtsoredtable.DefaultView

            For Each col In Resultgrid.Columns
                If col.Header = "batchid" Then
                    col.Visibility = System.Windows.Visibility.Hidden
                End If
            Next
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try

    End Sub


    Private Sub Closebtn2_Click(sender As Object, e As Windows.RoutedEventArgs) Handles closebtn2.Click
        Try
            Me.Hide()
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btnRefresh.Click
        Try
            Dim dataset As New DataSet
            Resultgrid.ItemsSource = Nothing
            For del As Integer = Resultgrid.Columns.Count - 1 To 1 Step -1
                Resultgrid.Columns.RemoveAt(del)
            Next
            LabelBatch.Content = "Batching Result"
            Dim client = New WebClient()
            client.Headers("Content-Type") = "application/json"
            client.Encoding = System.Text.Encoding.UTF8
            Dim dtresult As New DataTable
            dtresult.Columns.Add("Operation")
            dtresult.Columns.Add("batchid")
            dtresult.Columns.Add("Folderpath")
            dtresult.Columns.Add("Date and time", System.Type.GetType("System.DateTime"))
            dtresult.Columns.Add("status")
            For i As Integer = 0 To dtresult.Columns.Count - 1
                Dim DGC As New System.Windows.Controls.DataGridTextColumn()
                DGC.CanUserSort = True
                DGC.IsReadOnly = True
                DGC.SortMemberPath = dtresult.Columns(i).ToString()
                DGC.Header = dtresult.Columns(i).ToString()
                Dim binding As System.Windows.Data.Binding = New System.Windows.Data.Binding(String.Format("[{0}]", dtresult.Columns(i).ToString()))
                binding.Mode = System.Windows.Data.BindingMode.TwoWay
                DGC.Binding = binding
                Resultgrid.Columns.Add(DGC)
            Next
            If (File.Exists(jspath)) Then
                Dim uristring = File.ReadAllText(jspath)
                Dim fileinfos As List(Of folderinfo) = ser.Deserialize(Of List(Of folderinfo))(uristring)
                DeserializeDataTable(uristring)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Rows.Count <> 0 Then
                    For Each row As DataRow In dt.Rows
                        Dim dtrow = dtresult.NewRow
                        dtrow("Operation") = "Decrypt"
                        dtrow("batchid") = row("batchid")
                        dtrow("FolderPath") = row("foldername")
                        dtrow("Date and Time") = Convert.ToDateTime(row("datime"))
                        'dtrow("Date and Time") = Convert.ToDateTime("25-Mar-2021 11:39:00 AM")
                        dtrow("Status") = row("status")
                        dtresult.Rows.Add(dtrow)
                    Next
                End If
            End If

            If (File.Exists(dwnpath)) Then
                Dim uristring = File.ReadAllText(dwnpath)
                Dim downloadinfo As List(Of downloadfile) = ser.Deserialize(Of List(Of downloadfile))(uristring)
                DeserializeDataTable(uristring)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 AndAlso dt.Rows.Count <> 0 Then
                    For Each row As DataRow In dt.Rows
                        Dim dtrow = dtresult.NewRow
                        dtrow("Operation") = "Download"
                        dtrow("batchid") = row("batchid")
                        dtrow("FolderPath") = row("foldername")
                        dtrow("Date and Time") = Convert.ToDateTime(row("datime"))
                        'dtrow("Date and Time") = Convert.ToDateTime("25-Mar-2021 11:39:00 AM") ', "MM/dd/yyyy hh:mm:ss tt"))
                        'table.Columns.Add("Date", TypeOf (DateTime));
                        dtrow("Status") = row("status")
                        dtresult.Rows.Add(dtrow)
                    Next
                End If
            End If

            Dim dtview As New DataView(dtresult)
            Dim sortstring As String = "Date and Time DESC" ' sorting in descending manner 
            dtview.Sort = sortstring

            Dim dtsoredtable As New DataTable
            dtsoredtable = dtview.ToTable
            Resultgrid.ItemsSource = dtsoredtable.DefaultView

            For Each col In Resultgrid.Columns
                If col.Header = "batchid" Then
                    col.Visibility = System.Windows.Visibility.Hidden
                End If
            Next
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub excelexport_Click(sender As Object, e As Windows.RoutedEventArgs) Handles excelexport.Click
        '' creating Excel Application  
        'Dim app As Microsoft.Office.Interop.Excel._Application = New Microsoft.Office.Interop.Excel.Application()
        'Dim workbook As Microsoft.Office.Interop.Excel._Workbook = app.Workbooks.Add(Type.Missing)
        'Dim worksheet As Microsoft.Office.Interop.Excel._Worksheet = Nothing
        'app.Visible = True
        'worksheet = workbook.Sheets("Sheet1")
        'worksheet = workbook.ActiveSheet
        'worksheet.Name = "Exported from gridview"

        'Dim dt As New DataTable
        'dt = CType(Resultgrid.ItemsSource, DataView).ToTable

        'For i As Integer = 1 To dt.Columns.Count
        '    worksheet.Cells(1, i) = dt.Columns(i - 1).ToString.ToUpper
        'Next

        'For i As Integer = 0 To dt.Rows.Count - 1
        '    For j As Integer = 0 To dt.Columns.Count - 1
        '        'worksheet.Cells(i + 2, j + 1) = dt.Row(i).Cells(j).Value.ToString()
        '        worksheet.Cells(i + 2, j + 1) = dt.Rows(i)(j).ToString
        '    Next
        'Next

        'workbook.SaveAs("c:\output.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing)
        'app.Quit()

        'Resultgrid.SelectionMode = DataGridSelectionMode.Extended
        'Resultgrid.SelectAllCells()
        'Resultgrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader
        'ApplicationCommands.Copy.Execute(Nothing, Resultgrid)
        'Dim resultat As String = CType(Clipboard.GetData(DataFormats.CommaSeparatedValue), String)
        'Dim result As String = CType(Clipboard.GetData(DataFormats.Text), String)
        'Resultgrid.UnselectAllCells()
        'Dim file1 As New System.IO.StreamWriter("C:\Users\test.xls")
        'file1.WriteLine(result.Replace(","c, " "c))
        'file1.Close()
        'MessageBox.Show(" Exporting DataGrid data to Excel file created.xls")
        Dim sfd As New Windows.Forms.SaveFileDialog
        sfd.AddExtension = True
        sfd.DefaultExt = ".xlsx"
        ' sfd.FileName = "D:\5.xlsx"
        If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Export(sfd.FileName)
        End If
    End Sub

    Private Sub Export(excelfilename As String)
        Dim sheetIndex As Integer
        Dim Ex As Object
        Dim Wb As Object
        Dim Ws As Object
        Ex = CreateObject("Excel.Application")

        Wb = Ex.workbooks.add


        Dim dt As New DataTable
        dt = CType(Resultgrid.ItemsSource, DataView).ToTable

        ' Copy each DataTable as a new Sheet

        'On Error Resume Next
        Dim col, row As Integer
        ' Copy the DataTable to an object array
        Dim rawData(dt.Rows.Count, dt.Columns.Count - 1) As Object

        ' Copy the column names to the first row of the object array

        For col = 0 To dt.Columns.Count - 1
            rawData(0, col) = dt.Columns(col).ToString.ToUpper

        Next

        For col = 0 To dt.Columns.Count - 1
            For row = 0 To dt.Rows.Count - 1
                rawData(row + 1, col) = dt.Rows(row)(col).ToString
            Next
        Next
        ' Calculate the final column letter
        Dim finalColLetter As String = String.Empty
        finalColLetter = ExcelColName(dt.Columns.Count) 'Generate Excel Column Name (Column ID)


        sheetIndex += 1
        Ws = Wb.Worksheets(sheetIndex)
        ' Ws.name = excelfilename
        Dim excelRange As String = String.Format("A1:{0}{1}", finalColLetter, dt.Rows.Count + 1)

        Ws.Range(excelRange, Type.Missing).Value2 = rawData
        Ws = Nothing


        '   Wb.SaveAs(FileName, Type.Missing, Type.Missing,
        '    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        '    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing)
        Wb.close(True, excelfilename, Type.Missing)
        Wb = Nothing
        ' Release the Application object
        Ex.Quit()
        Ex = Nothing
        ' Collect the unreferenced objects
        GC.Collect()
        custommsgbox.showCustomMessageBox("Info", "Exported Successfully")

        ' MsgBox("Exported Successfully.", MsgBoxStyle.Information)
    End Sub

    Public Function ExcelColName(ByVal Col As Integer) As String
        If Col < 0 And Col > 256 Then
            MsgBox("Invalid Argument", MsgBoxStyle.Critical)
            Return Nothing
            Exit Function
        End If
        Dim i As Int16
        Dim r As Int16
        Dim S As String
        If Col <= 26 Then
            S = Chr(Col + 64)
        Else
            r = Col Mod 26
            i = System.Math.Floor(Col / 26)
            If r = 0 Then
                r = 26
                i = i - 1
            End If
            S = Chr(i + 64) & Chr(r + 64)
        End If
        ExcelColName = S
    End Function

    Private Sub Btn_cancel_Click(sender As Object, e As RoutedEventArgs) Handles Btn_cancel.Click
        Try
            Me.Hide()
        Catch ex As Exception
            custommsgbox.showCustomMessageBox("error", "Exception : " & vbCrLf & ex.Message)
        End Try
    End Sub
End Class

