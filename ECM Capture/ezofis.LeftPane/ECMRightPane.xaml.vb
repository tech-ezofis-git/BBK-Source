Imports ezofis.UserControl.CAC
Imports Telerik.Windows.Controls
Imports Telerik.Windows.Controls.Docking
Imports Telerik.Windows.Controls.Input
Imports Telerik.Windows.Controls.Navigation
Imports System.Data
Imports ezofis.Export
Imports Microsoft.Win32
Imports System.Net.NetworkInformation
Imports Leadtools.ImageProcessing
Imports Leadtools.Forms.Ocr
Imports System.Globalization
Imports System.Resources
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Collections.Specialized
Imports System.Configuration
Imports iTextSharp.text.pdf
Imports ezofis.Viewer
Imports System.Xml
Imports System.Data.SqlClient
Imports ezofis
Imports System.Collections.ObjectModel
Public Class ECMRightPane
#Region "Variables"
    Public fieldlst As New List(Of eZTemplateField)
    Public Imaging As String
    Public Shared cabinetid As Integer
    Public Shared templateid As Integer = 0
    Public Shared cabinetName As String
    Public Shared CreateOnId As String
    Public Shared TemplateName As String
    Dim CAC As New CACserviceClient
    Dim stageitmid As String
    Public CurrentFnInRightPane As String = ""
    Public Index As Integer = Nothing
    Public SelectZoneIsClicked As Boolean
    Public ECMImage As Leadtools.RasterImage
    Public ErsId As Integer = 0
    Public sAuthor As String
    Public sTitle As String
    Public sSubject As String

    Public sRemarks As String
    Private ERSPath As String
    Dim ezpdf As eZPdfProperties
    Dim ezTempField As List(Of eZTemplateField)
    Public ifilenamelist As String = ""
    Dim Appcon As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("api"), NameValueCollection)
    Dim AppconDB As NameValueCollection = DirectCast(ConfigurationSettings.GetConfig("Database"), NameValueCollection)
    Dim apiUrl As String = Appcon("api").ToString
    Dim Monitorpath As String = Appcon("Monitor").ToString
    Public ecmloginu As eZECMLogin
    Dim cadid As String = ""
    Dim TemplateNo As String = Appcon("TemplateidCAD").ToString
    Dim TemplateNocredit As String = Appcon("TemplateidCredit").ToString

#End Region
    Private Sub ddlstcab_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ddlstcab.SelectionChanged
        Try
            If CurrentFnInRightPane <> "" Then
                SaveRecords(CurrentFnInRightPane)
            End If
            CurrentFnInRightPane = ""
            cabinetid = ddlstcab.SelectedValue
            cabinetName = ddlstcab.Text.ToString()
            ddlsttem.DisplayMemberPath = "TemplateName"
            ddlsttem.SelectedValuePath = "TemplateId"
            ddlsttem.ItemsSource = CAC.eZTemplateListByLoginId(CreateOnId, ddlstcab.SelectedValue)
            If Not ddlsttem.Items.Count = 0 Then
                If cabinetid = 2 Then
                    For Each d As eZTemplate In ddlsttem.Items
                        If d.TemplateId = 5 Then
                            ddlsttem.SelectedItem = d
                            Exit For
                        End If
                    Next
                Else
                    ddlsttem.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
        Finally
            CurrentFnInRightPane = ""
            ECMViewer.Viewer.Image = Nothing
        End Try
    End Sub
    Private Sub ddlsttem_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ddlsttem.SelectionChanged
        Try
            If CurrentFnInRightPane <> "" Then
                SaveRecords(CurrentFnInRightPane)
            End If
            templateid = ddlsttem.SelectedValue
            TemplateName = GetSelectedTemplateName()
            If templateid <> 0 Then
                'Dim obj As New List(Of eZTempBarcode)
                'Dim CAC As New CACserviceClient
                'obj = CAC.SelectedeZTempBarcodeList("TemplateId", templateid)
                loadcontrol()
                'Getrecords(CurrentFnInRightPane)
                GetContextMenu(ECMImage)
            End If
        Catch ex As Exception
        Finally
            CurrentFnInRightPane = ""
            ECMViewer.Viewer.Image = Nothing
            ECMLeftPane.Refresh()
        End Try
    End Sub
    Private Function GetSelectedTemplateName() As String
        Dim selectedTemplate = TryCast(ddlsttem.SelectedItem, eZTemplate)
        If selectedTemplate IsNot Nothing AndAlso Not String.IsNullOrEmpty(selectedTemplate.TemplateName) Then
            Return selectedTemplate.TemplateName
        End If
        If Not String.IsNullOrEmpty(TemplateName) Then
            Return TemplateName
        End If
        If ddlsttem.Text IsNot Nothing Then
            Return ddlsttem.Text.ToString()
        End If
        Return ""
    End Function
    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            '  IndexingFieldPanel.Height = scr.Height - 5
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try
    End Sub
    Public Sub LoadCabinets()
        Try
            ddlstcab.SelectedValuePath = "CabinetID"
            ddlstcab.DisplayMemberPath = "CabinetName"
            ddlstcab.ItemsSource = CAC.eZCabinetListByLoginId(CreateOnId)
            If Not ddlstcab.Items.Count = 0 Then
                ddlstcab.SelectedIndex = 0
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function LoginFormLoad() As String
        Dim CAC As New CACserviceClient
        Dim ipproperties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
        Dim domnme As String = ipproperties.DomainName
        Dim username As String = "asif" + "@" + domnme
        'Dim CreateOnQuery As String = "select [ECMLoginId] from [eZOfisDB].[dbo].[eZECMLogin] where [LoginName]='" & username & "'"
        'Dim ECMLoginIdDs As New List(Of eZECMLogin)
        'ECMLoginIdDs = CAC.SelectedeZECMLoginList("loginname", username)
        Return 1
        'If ECMLoginIdDs.Count > 0 Then
        '    Return ECMLoginIdDs(0).ECMLoginId
        'Else
        '    Dim LoginFrm = New LoginForm
        '    If LoginFrm.ShowDialog() = True Then
        '        LoginFrm.Close()
        '        Return LoginFrm.LoginCreateId
        '    Else
        '        Return 0
        '    End If
        'End If
    End Function
    Private Function IsSyncField(ByVal LookupId As Integer, ByVal FieldName As String) As Boolean
        Try
            Dim CAC As New CACserviceClient
            If LookupId <> 0 Then
                Dim objeZLookupFields As New List(Of eZLookupFields)
                objeZLookupFields = CAC.SelectedeZLookupFieldsListWithLookupId("ECMField", FieldName, LookupId.ToString())
                If objeZLookupFields.Count <> 0 Then
                    If objeZLookupFields(0).IsSyncField Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else

                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub txtbox_KeyPress(ByVal sender As Object, ByVal e As KeyEventArgs)
        Try
            Dim keys = e.Key
            Dim txtbox As TextBox = sender
            If Not Char.IsDigit(CChar(ChrW(KeyInterop.VirtualKeyFromKey(e.Key)))) And (e.Key >= 74 And e.Key <= 83) = False And e.Key <> 88 And e.Key <> 142 And e.Key <> 144 Then
                If e.Key <> Key.Tab Then
                    e.Handled = True
                End If
            Else
                If e.Key = 144 And e.Key = 88 Then
                    txtbox.Text = txtbox.Text.Trim()
                    If txtbox.Text.Length = 0 Then
                        e.Handled = True
                    End If
                    If txtbox.Text.IndexOf(".") <> -1 Then
                        e.Handled = True
                    End If
                End If
                If e.Key = 142 Then
                    txtbox.Text = txtbox.Text.Trim()
                    If txtbox.Text.Length = 0 Then
                        e.Handled = True
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    'Class DocumentTypeForInvita
    '    Public DocumentTypeID As Integer
    '    Public DocumentType As String
    'End Class

    'Class AccountTypeForInvita
    '    Public AccountTypeID As Integer
    '    Public AccountType As String
    'End Class

    'Private Function GetDocumentType() As List(Of DocumentTypeForInvita)
    '    Dim lstAnswerChoices As New List(Of DocumentTypeForInvita)
    '    Dim objAnswerChoice As DocumentTypeForInvita

    '    objAnswerChoice = New DocumentTypeForInvita
    '    objAnswerChoice.DocumentTypeID = 1
    '    objAnswerChoice.DocumentType = "Answer Choice One"
    '    lstAnswerChoices.Add(objAnswerChoice)

    '    objAnswerChoice = New AnswerChoice
    '    objAnswerChoice.AnswerChoiceID = 2
    '    objAnswerChoice.AnswerChoice = "Answer Choice Two"
    '    lstAnswerChoices.Add(objAnswerChoice)

    '    Return lstAnswerChoices
    'End Function


    Public Sub loadcontrol()
        Try
            IndexingFieldPanel.Children.Clear()
            Dim CAC As New CACserviceClient
            fieldlst = New List(Of eZTemplateField)
            fieldlst = CAC.SelectedeZTemplateFieldList(Criteria:="TemplateId", templateid.ToString())
            Dim LookupId As Integer = 0
            Dim objeZLookup As New List(Of eZLookup)
            objeZLookup = CAC.SelectedeZLookupList(Criteria:="TemplateId", templateid)
            If objeZLookup.Count <> 0 Then
                LookupId = objeZLookup(0).LookupId
            End If
            For i As Integer = 0 To fieldlst.Count - 1
                'Dim item1 As New MenuItem()
                'item1.Header = fieldlst(i).FieldName.Trim()
                'ConMenu.Items.Add(item1)
                If IsSyncField(LookupId, fieldlst(i).FieldName.Trim()) Then
                    Dim DynamicGrid As New Grid
                    DynamicGrid.Name = "DG" & fieldlst(i).FieldName.Replace(" ", "")
                    Dim gridCol1 As New ColumnDefinition()
                    Dim gridCol2 As New ColumnDefinition()
                    Dim gridRow1 As New RowDefinition()
                    gridCol2.Width = New GridLength(25)
                    gridRow1.Height = New GridLength(23)
                    DynamicGrid.ColumnDefinitions.Add(gridCol1)
                    DynamicGrid.ColumnDefinitions.Add(gridCol2)
                    DynamicGrid.RowDefinitions.Add(gridRow1)
                    DynamicGrid.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                    DynamicGrid.VerticalAlignment = Windows.VerticalAlignment.Stretch
                    DynamicGrid.Margin = New Thickness(5)
                    '------------------
                    Dim DynamicGrid1 As New Grid
                    DynamicGrid1.Name = "DG1" & fieldlst(i).FieldName.Replace(" ", "")
                    Dim gridCol11 As New ColumnDefinition()
                    Dim gridCol21 As New ColumnDefinition()
                    Dim gridRow11 As New RowDefinition()
                    gridCol21.Width = New GridLength(25)
                    gridRow11.Height = New GridLength(23)
                    DynamicGrid1.ColumnDefinitions.Add(gridCol11)
                    DynamicGrid1.ColumnDefinitions.Add(gridCol21)
                    DynamicGrid1.RowDefinitions.Add(gridRow11)
                    DynamicGrid1.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                    DynamicGrid1.VerticalAlignment = Windows.VerticalAlignment.Stretch
                    DynamicGrid1.Margin = New Thickness(5)
                    '--------------------
                    Dim labels As New Label
                    Dim labelss As New Label
                    If fieldlst(i).DataTypeId = 1 Then
                        labels.Name = "Lbltxt" & fieldlst(i).FieldName.Replace(" ", "")
                        If fieldlst(i).Mandatory Then labelss.Content = "*" Else labelss.Content = ""
                        labels.Content = "  " + fieldlst(i).FieldName + " (Sync)"
                        'IndexingFieldPanel.Children.Add(labels)
                        '---------------
                        labelss.Name = "Lbltxts" & fieldlst(i).FieldName.Replace(" ", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        '-------------------
                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fieldlst(i).FieldName.Replace(" ", "")
                        txtbox.MaxWidth = 270
                        AddHandler txtbox.KeyDown, AddressOf txtbox_KeyPress
                        'AddHandler txtbox.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fieldlst(i).FieldName.Replace(" ", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fieldlst(i).DataTypeId = 2 Then
                        labels.Name = "Lblcbo" & fieldlst(i).FieldName.Replace(" ", "")
                        If fieldlst(i).Mandatory Then labelss.Content = "*" Else labelss.Content = ""
                        labels.Content = "  " + fieldlst(i).FieldName + " (Sync)"
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lblcbos" & fieldlst(i).FieldName.Replace(" ", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim cmb As New ComboBox
                        cmb.Name = "cbo" & fieldlst(i).FieldName.Replace(" ", "")
                        cmb.IsEditable = fieldlst(i).IsEditable
                        ' cmb.IsEditable = "True"
                        cmb.MaxWidth = 270
                        cmb.ItemsSource = CAC.GetDatasetByQuery("select '' as [" & fieldlst(i).FieldName.Trim() & "] union all select distinct [" & fieldlst(i).FieldName.Trim() & "] from " & "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_items WITH (NOLOCK) where [" & fieldlst(i).FieldName.Trim() & "] <> ''  order by [" & fieldlst(i).FieldName.Trim() & "]").Tables(0).DefaultView
                        cmb.SelectedValuePath = fieldlst(i).FieldName.Trim()
                        cmb.DisplayMemberPath = fieldlst(i).FieldName.Trim()


                        'cmb.DropDownStyle = ComboBoxStyle.DropDown
                        'IndexingFieldPanel.Children.Add(cmb)
                        'AddHandler cmb.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fieldlst(i).FieldName.Replace(" ", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(cmb)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(cmb, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fieldlst(i).DataTypeId = 6 Then
                        labels.Name = "Lbltxt" & fieldlst(i).FieldName.Replace(" ", "")
                        If fieldlst(i).Mandatory Then labelss.Content = "*" Else labelss.Content = ""
                        labels.Content = "  " + fieldlst(i).FieldName + " (Sync)"
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbltxts" & fieldlst(i).FieldName.Replace(" ", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fieldlst(i).FieldName.Replace(" ", "")
                        txtbox.MaxWidth = 270
                        AddHandler txtbox.KeyDown, AddressOf txtbox_KeyPress
                        'AddHandler txtbox.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fieldlst(i).FieldName.Replace(" ", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fieldlst(i).DataTypeId = 4 Then
                        labels.Name = "Lbltxt" & fieldlst(i).FieldName.Replace(" ", "")
                        If fieldlst(i).Mandatory Then labelss.Content = "*" Else labelss.Content = ""
                        labels.Content = "  " + fieldlst(i).FieldName + " (Sync)"
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbltxts" & fieldlst(i).FieldName.Replace(" ", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fieldlst(i).FieldName.Replace(" ", "")
                        txtbox.MaxWidth = 270
                        'AddHandler txtbox.Leave, AddressOf txtbox_Leave
                        'LookupFieldPanel.Controls.Add(txtbox)
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fieldlst(i).FieldName.Replace(" ", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fieldlst(i).DataTypeId = 5 Then
                        labels.Name = "Lbldt" & fieldlst(i).FieldName.Replace(" ", "")
                        If fieldlst(i).Mandatory Then labelss.Content = "*" Else labelss.Content = ""
                        labels.Content = "  " + fieldlst(i).FieldName + " (Sync)"
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbldts" & fieldlst(i).FieldName.Replace(" ", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        'Dim dtbox As New RadDatePicker
                        Dim dtbox As New Telerik.Windows.Controls.RadDatePicker
                        dtbox.Name = "dt" & fieldlst(i).FieldName.Replace(" ", "")
                        dtbox.MaxWidth = 270
                        dtbox.InputMode = Telerik.Windows.Controls.InputMode.DatePicker
                        dtbox.DisplayFormat = "dd/MM/yyyy"

                        ' Set the culture to ensure proper formatting in the dropdown
                        dtbox.Culture = New System.Globalization.CultureInfo("en-GB") ' British English uses dd/MM/yyyy
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fieldlst(i).FieldName.Replace(" ", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(dtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(dtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    End If
                End If
            Next
            If LookupId <> 0 Then
                Dim DynamicGrid As New Grid
                DynamicGrid.Name = "DGBtn"
                Dim gridCol1 As New ColumnDefinition()
                Dim gridCol2 As New ColumnDefinition()
                Dim gridRow1 As New RowDefinition()
                gridCol2.Width = New GridLength(40)
                gridRow1.Height = New GridLength(23)
                DynamicGrid.ColumnDefinitions.Add(gridCol1)
                DynamicGrid.ColumnDefinitions.Add(gridCol2)
                DynamicGrid.RowDefinitions.Add(gridRow1)
                DynamicGrid.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                DynamicGrid.VerticalAlignment = Windows.VerticalAlignment.Stretch
                DynamicGrid.Margin = New Thickness(5)
                'DynamicGrid.Background = New SolidColorBrush(Colors.LightSteelBlue)
                Dim Btn As New Button
                Btn.Name = "SyncBtn"
                Btn.Content = "Sync"
                DynamicGrid.Children.Add(Btn)
                AddHandler Btn.Click, AddressOf Btn_Click
                Grid.SetColumn(Btn, 1)
                IndexingFieldPanel.Children.Add(DynamicGrid)
            End If
            Dim fldmandatory = fieldlst.FindAll(Function(kj) kj.Mandatory = True)
            For i As Integer = 0 To fldmandatory.Count - 1
                If Not IsSyncField(LookupId, fldmandatory(i).FieldName.Trim()) Then
                    Dim DynamicGrid As New Grid
                    DynamicGrid.Name = "DG" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                    Dim gridCol1 As New ColumnDefinition()
                    Dim gridCol2 As New ColumnDefinition()
                    Dim gridRow1 As New RowDefinition()
                    gridCol2.Width = New GridLength(25)
                    gridRow1.Height = New GridLength(23)
                    DynamicGrid.ColumnDefinitions.Add(gridCol1)
                    DynamicGrid.ColumnDefinitions.Add(gridCol2)
                    DynamicGrid.RowDefinitions.Add(gridRow1)
                    DynamicGrid.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                    DynamicGrid.VerticalAlignment = Windows.VerticalAlignment.Stretch
                    '  DynamicGrid.Margin = New Thickness(5)
                    '----------------------
                    Dim DynamicGrid1 As New Grid
                    DynamicGrid1.Name = "DG1" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                    Dim gridCol11 As New ColumnDefinition()
                    Dim gridCol21 As New ColumnDefinition()
                    Dim gridRow11 As New RowDefinition()
                    gridCol21.Width = New GridLength(25)
                    gridRow11.Height = New GridLength(23)
                    DynamicGrid1.ColumnDefinitions.Add(gridCol11)
                    DynamicGrid1.ColumnDefinitions.Add(gridCol21)
                    DynamicGrid1.RowDefinitions.Add(gridRow11)
                    DynamicGrid1.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                    DynamicGrid1.VerticalAlignment = Windows.VerticalAlignment.Stretch
                    '  DynamicGrid1.Margin = New Thickness(5)
                    '------------------------------
                    Dim labels As New Label
                    Dim labelss As New Label
                    If fldmandatory(i).DataTypeId = 1 Then
                        labels.Name = "Lbltxt" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = "*"
                        labels.Content = "  " + fldmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbltxts" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        DynamicGrid1.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        txtbox.MaxWidth = 270
                        AddHandler txtbox.KeyDown, AddressOf txtbox_KeyPress
                        txtbox.Margin = New Thickness(3, 0, 0, 0)
                        'AddHandler txtbox.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        DynamicGrid.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fldmandatory(i).DataTypeId = 2 Then
                        labels.Name = "Lblcbo" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = "*"
                        labels.Content = "  " + fldmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lblcbos" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        DynamicGrid1.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim cmb As New ComboBox
                        cmb.IsEditable = fldmandatory(i).IsEditable

                        'cmb.IsReadOnly = True
                        ' cmb.IsEditable = True
                        cmb.MaxWidth = 270
                        cmb.Name = "cbo" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")







                        Dim sql As String = "select '' as [" & fldmandatory(i).FieldName.Trim() & "] union all select distinct [" & fldmandatory(i).FieldName.Trim() & "] from " & "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_items WITH (NOLOCK) where [" & fldmandatory(i).FieldName.Trim() & "] <> '' order by [" & fldmandatory(i).FieldName.Trim() & "]"
                        'cmb.ItemsSource = CAC.GetDatasetByQuery(sql).Tables(0).DefaultView
                        'cmb.SelectedValuePath = fldmandatory(i).FieldName.Trim()
                        'cmb.DisplayMemberPath = fldmandatory(i).FieldName.Trim()

                        If fldmandatory(i).FieldName.Trim().ToLower().Replace(" ", "") = "documenttype" Then
                            If (TemplateNo = templateid) Then


                                Dim qry As String = "SELECT DISTINCT [Document_type] as [" & fldmandatory(i).FieldName.Trim() & "] FROM [eZDocument_category]"

                                cmb.ItemsSource = CAC.GetDatasetByQuery(qry).Tables(0).DefaultView
                                cmb.SelectedValuePath = fldmandatory(i).FieldName.Trim()
                                cmb.DisplayMemberPath = fldmandatory(i).FieldName.Trim()

                                AddHandler cmb.SelectionChanged, AddressOf cmb_SelectionChanged
                            ElseIf (TemplateNocredit = templateid) Then
                                Dim cmbitems36 As New ComboBoxItem
                                cmbitems36.Background = Brushes.DarkGreen
                                cmbitems36.Foreground = Brushes.White
                                cmbitems36.Content = "Customer Identification"
                                cmb.Items.Add(cmbitems36)
                                Dim cmbitems As New ComboBoxItem
                                cmbitems.Background = Brushes.DarkGreen
                                cmbitems.Foreground = Brushes.White
                                cmbitems.Content = "Primary Loan Documents"
                                cmb.Items.Add(cmbitems)
                                Dim cmbitems2 As New ComboBoxItem
                                cmbitems2.Background = Brushes.DarkGreen
                                cmbitems2.Foreground = Brushes.White
                                cmbitems2.Content = "Supporting Documents"
                                cmb.Items.Add(cmbitems2)
                                Dim cmbitems3 As New ComboBoxItem
                                cmbitems3.Background = Brushes.DarkGreen
                                cmbitems3.Foreground = Brushes.White
                                cmbitems3.Content = "Other Documents"
                                cmb.Items.Add(cmbitems3)
                                Dim cmbitems4 As New ComboBoxItem
                                cmbitems4.Background = Brushes.DarkGreen
                                cmbitems4.Foreground = Brushes.White
                                cmbitems4.Content = "Old Closed Loans"
                                cmb.Items.Add(cmbitems4)
                                Dim cmbitems5 As New ComboBoxItem
                                cmbitems5.Background = Brushes.DarkGreen
                                cmbitems5.Foreground = Brushes.White
                                cmbitems5.Content = "Consumer Loan Account"
                                cmb.Items.Add(cmbitems5)

                            Else

                                Dim cmbitems36 As New ComboBoxItem
                                cmbitems36.Background = Brushes.DarkGreen
                                cmbitems36.Foreground = Brushes.White
                                cmbitems36.Content = ""
                                cmb.Items.Add(cmbitems36)
                                Dim cmbitems As New ComboBoxItem
                                cmbitems.Background = Brushes.DarkGreen
                                cmbitems.Foreground = Brushes.White
                                cmbitems.Content = "Deposit"
                                cmb.Items.Add(cmbitems)
                                Dim cmbitems2 As New ComboBoxItem
                                cmbitems2.Background = Brushes.DarkGreen
                                cmbitems2.Foreground = Brushes.White
                                cmbitems2.Content = "Other Documents"
                                cmb.Items.Add(cmbitems2)
                                Dim cmbitems3 As New ComboBoxItem
                                cmbitems3.Background = Brushes.DarkGreen
                                cmbitems3.Foreground = Brushes.White
                                cmbitems3.Content = "Compliance"
                                cmb.Items.Add(cmbitems3)
                                Dim cmbitems4 As New ComboBoxItem
                                cmbitems4.Background = Brushes.DarkGreen
                                cmbitems4.Foreground = Brushes.White
                                cmbitems4.Content = "RIM Details"
                                cmb.Items.Add(cmbitems4)
                                Dim cmbitems9 As New ComboBoxItem
                                cmbitems9.Background = Brushes.DarkGreen
                                cmbitems9.Foreground = Brushes.White
                                cmbitems9.Content = "Consumer Loan"
                                cmb.Items.Add(cmbitems9)
                                'Dim cmbitems18 As New ComboBoxItem
                                'cmbitems18.Background = Brushes.DarkGreen
                                'cmbitems18.Foreground = Brushes.White
                                'cmbitems18.Content = "Consumer Loan Other Documents"
                                'cmb.Items.Add(cmbitems18)
                                Dim cmbitems10 As New ComboBoxItem
                                cmbitems10.Background = Brushes.DarkGreen
                                cmbitems10.Foreground = Brushes.White
                                cmbitems10.Content = "Corporate Loan - ATs_Rollovers_LIS Documents"
                                cmb.Items.Add(cmbitems10)
                                Dim cmbitems11 As New ComboBoxItem
                                cmbitems11.Background = Brushes.DarkGreen
                                cmbitems11.Foreground = Brushes.White
                                cmbitems11.Content = "Corporate Loan - Other Documents"
                                cmb.Items.Add(cmbitems11)
                                'Dim cmbitems15 As New ComboBoxItem
                                'cmbitems15.Background = Brushes.DarkGreen
                                'cmbitems15.Foreground = Brushes.White
                                'cmbitems15.Content = "Corporate Loan ATs/Rollovers/LIS"
                                'cmb.Items.Add(cmbitems15)
                                'Dim cmbitems16 As New ComboBoxItem
                                'cmbitems16.Background = Brushes.DarkGreen
                                'cmbitems16.Foreground = Brushes.White
                                'cmbitems16.Content = "Corporate Loan Other Documents  Available"
                                'cmb.Items.Add(cmbitems16)
                                'Dim cmbitems17 As New ComboBoxItem
                                'cmbitems17.Background = Brushes.DarkGreen
                                'cmbitems17.Foreground = Brushes.White
                                'cmbitems17.Content = "Consumer Loan Avaible"
                                'cmb.Items.Add(cmbitems17)

                                'Dim cmbitems19 As New ComboBoxItem
                                'cmbitems19.Background = Brushes.DarkGreen
                                'cmbitems19.Foreground = Brushes.White
                                'cmbitems19.Content = "Car Loan"
                                'cmb.Items.Add(cmbitems19)
                                'Dim cmbitems20 As New ComboBoxItem
                                'cmbitems20.Background = Brushes.DarkGreen
                                'cmbitems20.Foreground = Brushes.White
                                'cmbitems20.Content = "Car Loan Other Documents"
                                'cmb.Items.Add(cmbitems20)

                                Dim cmbitems5 As New ComboBoxItem
                                cmbitems5.Background = Brushes.Yellow
                                cmbitems5.Content = "Property_Asset details"
                                cmb.Items.Add(cmbitems5)
                                Dim cmbitems6 As New ComboBoxItem
                                cmbitems6.Background = Brushes.Yellow
                                cmbitems6.Content = "Insurance Documents"
                                cmb.Items.Add(cmbitems6)
                                Dim cmbitems7 As New ComboBoxItem
                                cmbitems7.Background = Brushes.Yellow
                                cmbitems7.Content = "Loan Agreement and Customer Details"
                                cmb.Items.Add(cmbitems7)
                                Dim cmbitems8 As New ComboBoxItem
                                cmbitems8.Background = Brushes.Yellow
                                cmbitems8.Content = "Loan Calculation and Other"
                                cmb.Items.Add(cmbitems8)


                                'cmb.DisplayMemberPath = String
                                '   AddHandler cmb.SelectionChanged, AddressOf cmb_SelectionChanged
                            End If

                        ElseIf fldmandatory(i).FieldName.Trim().ToLower().Replace(" ", "") = "documentcategory" Then
                            If (TemplateNo = templateid) Then
                                cmb.SelectedIndex = -1

                                'Dim qry As String = "SELECT DISTINCT [Document_category] as [" & fldmandatory(i).FieldName.Trim() & "] FROM [eZDocument_category]"

                                'cmb.ItemsSource = CAC.GetDatasetByQuery(qry).Tables(0).DefaultView
                                'cmb.SelectedValuePath = fldmandatory(i).FieldName.Trim()
                                'cmb.DisplayMemberPath = fldmandatory(i).FieldName.Trim()
                                'AddHandler cmb.SelectionChanged, AddressOf cmb_SelectionChanged
                            End If
                        ElseIf fldmandatory(i).FieldName.Trim().ToLower().Replace(" ", "") = "loantype" Then

                            If (TemplateNocredit = templateid) Then
                                Dim item1 As New ComboBoxItem()
                                item1.Background = Brushes.White
                                item1.Foreground = Brushes.Black
                                item1.Content = "Retail"
                                cmb.Items.Add(item1)

                                Dim item2 As New ComboBoxItem()
                                item2.Background = Brushes.White
                                item2.Foreground = Brushes.Black
                                item2.Content = "Corporate Loans"
                                cmb.Items.Add(item2)

                                AddHandler cmb.SelectionChanged, AddressOf cmbcredit_SelectionChanged
                            End If
                        Else
                            cmb.ItemsSource = CAC.GetDatasetByQuery(sql).Tables(0).DefaultView
                            cmb.SelectedValuePath = fldmandatory(i).FieldName.Trim()
                            cmb.DisplayMemberPath = fldmandatory(i).FieldName.Trim()
                        End If
                        cmb.Margin = New Thickness(3, 0, 0, 0)
                        If fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "") = "FileStatus" Then
                            cmb.SelectedIndex = 1
                        End If








                        ' cmb. = ComboBoxStyle.DropDown
                        'IndexingFieldPanel.Children.Add(cmb)
                        'AddHandler cmb.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(cmb)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(cmb, 0)
                        Grid.SetColumn(chkbox, 1)
                        DynamicGrid.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fldmandatory(i).DataTypeId = 6 Then
                        labels.Name = "Lbltxt" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = "*"
                        labels.Content = "  " + fldmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbltxts" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        DynamicGrid1.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        txtbox.MaxWidth = 270
                        AddHandler txtbox.KeyDown, AddressOf txtbox_KeyPress
                        txtbox.Margin = New Thickness(3, 0, 0, 0)
                        'AddHandler txtbox.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        DynamicGrid.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fldmandatory(i).DataTypeId = 4 Then
                        labels.Name = "Lbltxt" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = "*"
                        labels.Content = "  " + fldmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbltxts" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        DynamicGrid1.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid1)

                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        txtbox.MaxWidth = 270
                        txtbox.Margin = New Thickness(3, 0, 0, 0)
                        'ecmlogin = New eZECMLogin()
                        If templateid = TemplateNo AndAlso fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "").ToLower() = "documentuploadby" Then
                            'txtbox.Text = ecmlogin.LoginName ' Set the TextBox content
                            Dim sqlquery = "select LoginName from eZECMLogin where ECMLoginId = " + CreateOnId + " and Isdeleted=0"
                            Dim ds As DataSet = CAC.GetDatasetByQuery(sqlquery)
                            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                ' Get the first value from the first row and first column (adjust as needed)
                                Dim value As String = ds.Tables(0).Rows(0)(0).ToString()

                                ' Assign the value to the TextBox
                                txtbox.Text = value
                            End If
                        End If
                        If templateid = 9 Or templateid = 10 Then
                            If fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "").ToLower() = "rimnumber" Then
                                AddHandler txtbox.LostKeyboardFocus, AddressOf txtbox_Leave
                            End If

                        End If
                        'If templateid = TemplateNo & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "").ToLower() = "documentuploadby" Then
                        '    txtbox.Text = "kani" ' Set the TextBox content
                        'End If


                        'LookupFieldPanel.Controls.Add(txtbox)
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        DynamicGrid.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fldmandatory(i).DataTypeId = 5 Then
                        labels.Name = "Lbldt" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = "*"
                        labels.Content = "  " + fldmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbldts" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        DynamicGrid1.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        'Dim dtbox As New RadDatePicker
                        Dim dtbox As New Telerik.Windows.Controls.RadDatePicker
                        dtbox.Name = "dt" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        dtbox.MaxWidth = 270
                        dtbox.InputMode = Telerik.Windows.Controls.InputMode.DatePicker
                        dtbox.Margin = New Thickness(3, 0, 0, 0)
                        'dtbox.DisplayFormat = "dd/MM/yyyy"

                        ' Set the culture to ensure proper formatting in the dropdown
                        dtbox.Culture = New System.Globalization.CultureInfo("en-GB")
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(dtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(dtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        DynamicGrid.Background = Brushes.DarkGray
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    End If
                End If
            Next
            Dim fldnotmandatory = fieldlst.FindAll(Function(kj) kj.Mandatory = False)
            For i As Integer = 0 To fldnotmandatory.Count - 1
                If Not IsSyncField(LookupId, fldnotmandatory(i).FieldName.Trim()) Then
                    Dim DynamicGrid As New Grid
                    DynamicGrid.Name = "DG" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                    Dim gridCol1 As New ColumnDefinition()
                    Dim gridCol2 As New ColumnDefinition()
                    Dim gridRow1 As New RowDefinition()
                    gridCol2.Width = New GridLength(25)
                    gridRow1.Height = New GridLength(23)
                    DynamicGrid.ColumnDefinitions.Add(gridCol1)
                    DynamicGrid.ColumnDefinitions.Add(gridCol2)
                    DynamicGrid.RowDefinitions.Add(gridRow1)
                    DynamicGrid.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                    DynamicGrid.VerticalAlignment = Windows.VerticalAlignment.Stretch
                    '  DynamicGrid.Margin = New Thickness(5)
                    '----------------------
                    Dim DynamicGrid1 As New Grid
                    DynamicGrid1.Name = "DG1" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                    Dim gridCol11 As New ColumnDefinition()
                    Dim gridCol21 As New ColumnDefinition()
                    Dim gridRow11 As New RowDefinition()
                    gridCol21.Width = New GridLength(25)
                    gridRow11.Height = New GridLength(23)
                    DynamicGrid1.ColumnDefinitions.Add(gridCol11)
                    DynamicGrid1.ColumnDefinitions.Add(gridCol21)
                    DynamicGrid1.RowDefinitions.Add(gridRow11)
                    DynamicGrid1.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                    DynamicGrid1.VerticalAlignment = Windows.VerticalAlignment.Stretch
                    '  DynamicGrid1.Margin = New Thickness(5)
                    '------------------------------
                    Dim labels As New Label
                    Dim labelss As New Label
                    If fldnotmandatory(i).DataTypeId = 1 Then
                        labels.Name = "Lbltxt" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = ""
                        labels.Content = "  " + fldnotmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbltxts" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        txtbox.MaxWidth = 270
                        txtbox.Margin = New Thickness(3, 0, 0, 0)
                        AddHandler txtbox.KeyDown, AddressOf txtbox_KeyPress
                        'AddHandler txtbox.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fldnotmandatory(i).DataTypeId = 2 Then
                        labels.Name = "Lblcbo" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = ""
                        labels.Content = "  " + fldnotmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lblcbos" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim cmb As New ComboBox
                        cmb.IsEditable = fldnotmandatory(i).IsEditable
                        'If fldnotmandatory(i).FieldName.Replace(" ", "") <> "ProjectName" And fldnotmandatory(i).FieldName.Replace(" ", "") <> "Beneficiary" And fldnotmandatory(i).FieldName.Replace(" ", "") <> "CorrespondenceType" Then
                        '    cmb.IsEditable = False
                        'Else
                        '    cmb.IsEditable = True
                        'End If
                        'cmb.IsReadOnly = True
                        ' cmb.IsEditable = True
                        cmb.Name = "cbo" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        'cmb.ItemsSource = CAC.GetDatasetByQuery("select '' as [" & fldnotmandatory(i).FieldName.Trim() & "]  union all select distinct [" & fldnotmandatory(i).FieldName.Trim() & "] from " & "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_items where [" & fldnotmandatory(i).FieldName.Trim() & "] <> ''  order by [" & fldnotmandatory(i).FieldName.Trim() & "]").Tables(0).DefaultView
                        'cmb.SelectedValuePath = fldnotmandatory(i).FieldName.Trim()
                        'cmb.DisplayMemberPath = fldnotmandatory(i).FieldName.Trim()
                        cmb.MaxWidth = 270


                        If fldnotmandatory(i).FieldName.Trim().ToLower().Replace(" ", "") = "accounttype" Then
                            Dim cmbitems36 As New ComboBoxItem
                            cmbitems36.Background = Brushes.Yellow
                            cmbitems36.Content = ""
                            cmb.Items.Add(cmbitems36)
                            Dim cmbitems As New ComboBoxItem
                            cmbitems.Background = Brushes.DarkGreen
                            cmbitems.Foreground = Brushes.White
                            cmbitems.Content = "CMZ"
                            cmb.Items.Add(cmbitems)
                            Dim cmbitems2 As New ComboBoxItem
                            cmbitems2.Background = Brushes.DarkGreen
                            cmbitems2.Foreground = Brushes.White
                            cmbitems2.Content = "CNS"
                            cmb.Items.Add(cmbitems2)
                            Dim cmbitems3 As New ComboBoxItem
                            cmbitems3.Background = Brushes.DarkGreen
                            cmbitems3.Foreground = Brushes.White
                            cmbitems3.Content = "CSC"
                            cmb.Items.Add(cmbitems3)
                            Dim cmbitems4 As New ComboBoxItem
                            cmbitems4.Background = Brushes.DarkGreen
                            cmbitems4.Foreground = Brushes.White
                            cmbitems4.Content = "CUR"
                            cmb.Items.Add(cmbitems4)
                            Dim cmbitems46 As New ComboBoxItem
                            cmbitems46.Background = Brushes.DarkGreen
                            cmbitems46.Foreground = Brushes.White
                            cmbitems46.Content = "CUX"
                            cmb.Items.Add(cmbitems46)
                            Dim cmbitems9 As New ComboBoxItem
                            cmbitems9.Background = Brushes.DarkGreen
                            cmbitems9.Foreground = Brushes.White
                            cmbitems9.Content = "FFD"
                            cmb.Items.Add(cmbitems9)
                            Dim cmbitems10 As New ComboBoxItem
                            cmbitems10.Background = Brushes.DarkGreen
                            cmbitems10.Foreground = Brushes.White
                            cmbitems10.Content = "FTD"
                            cmb.Items.Add(cmbitems10)
                            Dim cmbitems11 As New ComboBoxItem
                            cmbitems11.Background = Brushes.DarkGreen
                            cmbitems11.Foreground = Brushes.White
                            cmbitems11.Content = "SAV"
                            cmb.Items.Add(cmbitems11)
                            Dim cmbitems12 As New ComboBoxItem
                            cmbitems12.Background = Brushes.DarkGreen
                            cmbitems12.Foreground = Brushes.White
                            cmbitems12.Content = "SCL"
                            cmb.Items.Add(cmbitems12)
                            Dim cmbitems13 As New ComboBoxItem
                            cmbitems13.Background = Brushes.DarkGreen
                            cmbitems13.Foreground = Brushes.White
                            cmbitems13.Content = "SCM"
                            cmb.Items.Add(cmbitems13)
                            Dim cmbitems14 As New ComboBoxItem
                            cmbitems14.Background = Brushes.DarkGreen
                            cmbitems14.Foreground = Brushes.White
                            cmbitems14.Content = "SHR"
                            cmb.Items.Add(cmbitems14)
                            Dim cmbitems15 As New ComboBoxItem
                            cmbitems15.Background = Brushes.DarkGreen
                            cmbitems15.Foreground = Brushes.White
                            cmbitems15.Content = "SMM"
                            cmb.Items.Add(cmbitems15)
                            Dim cmbitems16 As New ComboBoxItem
                            cmbitems16.Background = Brushes.DarkGreen
                            cmbitems16.Foreground = Brushes.White
                            cmbitems16.Content = "SSM"
                            cmb.Items.Add(cmbitems16)
                            Dim cmbitems17 As New ComboBoxItem
                            cmbitems17.Background = Brushes.DarkGreen
                            cmbitems17.Foreground = Brushes.White
                            cmbitems17.Content = "SSV"
                            cmb.Items.Add(cmbitems17)
                            Dim cmbitems18 As New ComboBoxItem
                            cmbitems18.Background = Brushes.DarkGreen
                            cmbitems18.Foreground = Brushes.White
                            cmbitems18.Content = "TIL"
                            cmb.Items.Add(cmbitems18)
                            Dim cmbitems35 As New ComboBoxItem
                            cmbitems35.Background = Brushes.DarkGreen
                            cmbitems35.Foreground = Brushes.White
                            cmbitems35.Content = "CDV"
                            cmb.Items.Add(cmbitems35)
                            Dim cmbitems37 As New ComboBoxItem
                            cmbitems37.Background = Brushes.DarkGreen
                            cmbitems37.Foreground = Brushes.White
                            cmbitems37.Content = "CCO"
                            cmb.Items.Add(cmbitems37)
                            Dim cmbitems38 As New ComboBoxItem
                            cmbitems38.Background = Brushes.DarkGreen
                            cmbitems38.Foreground = Brushes.White
                            cmbitems38.Content = "CLV"
                            cmb.Items.Add(cmbitems38)
                            Dim cmbitems39 As New ComboBoxItem
                            cmbitems39.Background = Brushes.DarkGreen
                            cmbitems39.Foreground = Brushes.White
                            cmbitems38.Content = "CLB"
                            cmb.Items.Add(cmbitems39)
                            Dim cmbitems40 As New ComboBoxItem
                            cmbitems40.Background = Brushes.DarkGreen
                            cmbitems40.Foreground = Brushes.White
                            cmbitems40.Content = "ZBA"
                            cmb.Items.Add(cmbitems40)
                            Dim cmbitems41 As New ComboBoxItem
                            cmbitems41.Background = Brushes.DarkGreen
                            cmbitems41.Foreground = Brushes.White
                            cmbitems41.Content = "Salary Transfer"
                            cmb.Items.Add(cmbitems41)
                            Dim cmbitems42 As New ComboBoxItem
                            cmbitems42.Background = Brushes.DarkGreen
                            cmbitems42.Foreground = Brushes.White
                            cmbitems42.Content = "PIL"
                            cmb.Items.Add(cmbitems42)
                            Dim cmbitems43 As New ComboBoxItem
                            cmbitems43.Background = Brushes.DarkGreen
                            cmbitems43.Foreground = Brushes.White
                            cmbitems43.Content = "SHB"
                            cmb.Items.Add(cmbitems43)
                            Dim cmbitems44 As New ComboBoxItem
                            cmbitems44.Background = Brushes.DarkGreen
                            cmbitems44.Foreground = Brushes.White
                            cmbitems44.Content = "MFD"
                            cmb.Items.Add(cmbitems44)
                            Dim cmbitems45 As New ComboBoxItem
                            cmbitems45.Background = Brushes.DarkGreen
                            cmbitems45.Foreground = Brushes.White
                            cmbitems45.Content = "STS"
                            cmb.Items.Add(cmbitems45)
                            Dim cmbitems45A As New ComboBoxItem
                            cmbitems45A.Background = Brushes.DarkGreen
                            cmbitems45A.Foreground = Brushes.White
                            cmbitems45A.Content = "IFD"
                            cmb.Items.Add(cmbitems45A)
                            Dim cmbitems47 As New ComboBoxItem
                            cmbitems47.Background = Brushes.DarkGreen
                            cmbitems47.Foreground = Brushes.White
                            cmbitems47.Content = "CMA"
                            cmb.Items.Add(cmbitems47)
                            Dim cmbitems5 As New ComboBoxItem
                            cmbitems5.Background = Brushes.Yellow
                            cmbitems5.Content = "LBD"
                            cmb.Items.Add(cmbitems5)
                            Dim cmbitems6 As New ComboBoxItem
                            cmbitems6.Background = Brushes.Yellow
                            cmbitems6.Content = "LBN"
                            cmb.Items.Add(cmbitems6)
                            Dim cmbitems7 As New ComboBoxItem
                            cmbitems7.Background = Brushes.Yellow
                            cmbitems7.Content = "LBR"
                            cmb.Items.Add(cmbitems7)
                            Dim cmbitems8 As New ComboBoxItem
                            cmbitems8.Background = Brushes.Yellow
                            cmbitems8.Content = "LCD"
                            cmb.Items.Add(cmbitems8)
                            Dim cmbitems31 As New ComboBoxItem
                            cmbitems31.Background = Brushes.Yellow
                            cmbitems31.Content = "LCM"
                            cmb.Items.Add(cmbitems31)
                            Dim cmbitems19 As New ComboBoxItem
                            cmbitems19.Background = Brushes.Yellow
                            cmbitems19.Content = "LCN"
                            cmb.Items.Add(cmbitems19)
                            Dim cmbitems20 As New ComboBoxItem
                            cmbitems20.Background = Brushes.Yellow
                            cmbitems20.Content = "LDO"
                            cmb.Items.Add(cmbitems20)
                            Dim cmbitems21 As New ComboBoxItem
                            cmbitems21.Background = Brushes.Yellow
                            cmbitems21.Content = "LEX"
                            cmb.Items.Add(cmbitems21)
                            Dim cmbitems22 As New ComboBoxItem
                            cmbitems22.Background = Brushes.Yellow
                            cmbitems22.Content = "LLR"
                            cmb.Items.Add(cmbitems22)
                            Dim cmbitems23 As New ComboBoxItem
                            cmbitems23.Background = Brushes.Yellow
                            cmbitems23.Content = "LMG"
                            cmb.Items.Add(cmbitems23)
                            Dim cmbitems24 As New ComboBoxItem
                            cmbitems24.Background = Brushes.Yellow
                            cmbitems24.Content = "LMS"
                            cmb.Items.Add(cmbitems24)

                            Dim currentTemplateName As String = GetSelectedTemplateName()
                            If Not String.IsNullOrEmpty(currentTemplateName) AndAlso currentTemplateName.IndexOf("Retail", StringComparison.OrdinalIgnoreCase) >= 0 Then
                                Dim cmbLmt As New ComboBoxItem
                                cmbLmt.Background = Brushes.Yellow
                                cmbLmt.Content = "LMT"
                                cmb.Items.Add(cmbLmt)
                            End If

                            Dim cmbitems25 As New ComboBoxItem
                            cmbitems25.Background = Brushes.Yellow
                            cmbitems25.Content = "LNB"
                            cmb.Items.Add(cmbitems25)
                            Dim cmbitems26 As New ComboBoxItem
                            cmbitems26.Background = Brushes.Yellow
                            cmbitems26.Content = "LPC"
                            cmb.Items.Add(cmbitems26)
                            Dim cmbitems27 As New ComboBoxItem
                            cmbitems27.Background = Brushes.Yellow
                            cmbitems27.Content = "LPO"
                            cmb.Items.Add(cmbitems27)
                            Dim cmbitems28 As New ComboBoxItem
                            cmbitems28.Background = Brushes.Yellow
                            cmbitems28.Content = "LRV"
                            cmb.Items.Add(cmbitems28)
                            Dim cmbitems29 As New ComboBoxItem
                            cmbitems29.Background = Brushes.Yellow
                            cmbitems29.Content = "LSA"
                            cmb.Items.Add(cmbitems29)
                            Dim cmbitems30 As New ComboBoxItem
                            cmbitems30.Background = Brushes.Yellow
                            cmbitems30.Content = "LSR"
                            cmb.Items.Add(cmbitems30)

                            '   AddHandler cmb.SelectionChanged, AddressOf cmb_SelectionChanged
                        Else
                            cmb.ItemsSource = CAC.GetDatasetByQuery("select '' as [" & fldnotmandatory(i).FieldName.Trim() & "]  union all select distinct [" & fldnotmandatory(i).FieldName.Trim() & "] from " & "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_items WITH (NOLOCK) where [" & fldnotmandatory(i).FieldName.Trim() & "] <> ''  order by [" & fldnotmandatory(i).FieldName.Trim() & "]").Tables(0).DefaultView
                            cmb.SelectedValuePath = fldnotmandatory(i).FieldName.Trim()
                            cmb.DisplayMemberPath = fldnotmandatory(i).FieldName.Trim()
                        End If

                        cmb.Margin = New Thickness(3, 0, 0, 0)
                        If fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "") = "QualityCheck" Then
                            cmb.SelectedIndex = 1
                            cmb.IsEnabled = False
                        ElseIf fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "") = "AccountStatus" Then
                            cmb.SelectedIndex = 1

                        End If

                        'IndexingFieldPanel.Children.Add(cmb)
                        'AddHandler cmb.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(cmb)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(cmb, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fldnotmandatory(i).DataTypeId = 6 Then
                        labels.Name = "Lbltxt" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = ""
                        labels.Content = "  " + fldnotmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbltxts" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        txtbox.MaxWidth = 270
                        AddHandler txtbox.KeyDown, AddressOf txtbox_KeyPress
                        txtbox.Margin = New Thickness(3, 0, 0, 0)
                        'AddHandler txtbox.Leave, AddressOf txtbox_Leave
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fldnotmandatory(i).DataTypeId = 4 Then
                        labels.Name = "Lbltxt" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = ""
                        labels.Content = "  " + fldnotmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbltxts" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        Dim txtbox As New TextBox
                        txtbox.Name = "txt" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        txtbox.MaxWidth = 270
                        txtbox.Margin = New Thickness(3, 0, 0, 0)
                        'AddHandler txtbox.Leave, AddressOf txtbox_Leave
                        'LookupFieldPanel.Controls.Add(txtbox)
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(txtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(txtbox, 0)
                        Grid.SetColumn(chkbox, 1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    ElseIf fldnotmandatory(i).DataTypeId = 5 Then
                        labels.Name = "Lbldt" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Content = ""
                        labels.Content = "  " + fldnotmandatory(i).FieldName
                        'IndexingFieldPanel.Children.Add(labels)
                        labelss.Name = "Lbldts" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        labelss.Foreground = Brushes.Red
                        DynamicGrid1.Children.Add(labels)
                        DynamicGrid1.Children.Add(labelss)
                        IndexingFieldPanel.Children.Add(DynamicGrid1)
                        'Dim dtbox As New RadDatePicker
                        Dim dtbox As New Telerik.Windows.Controls.RadDatePicker
                        dtbox.Name = "dt" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        dtbox.MaxWidth = 270
                        dtbox.InputMode = Telerik.Windows.Controls.InputMode.DatePicker
                        dtbox.Margin = New Thickness(3, 0, 0, 0)
                        'dtbox.DisplayFormat = "dd/MM/yyyy"

                        ' Set the culture to ensure proper formatting in the dropdown
                        dtbox.Culture = New System.Globalization.CultureInfo("en-GB") ' British English uses dd/MM/yyyy
                        Dim chkbox As New CheckBox
                        chkbox.Name = "chk" & fldnotmandatory(i).FieldName.Replace(" ", "").Replace(".", "")
                        chkbox.Margin = New Thickness(3)
                        DynamicGrid.Children.Add(dtbox)
                        DynamicGrid.Children.Add(chkbox)
                        Grid.SetColumn(dtbox, 0)
                        Grid.SetColumn(chkbox, value:=1)
                        IndexingFieldPanel.Children.Add(DynamicGrid)
                    End If
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub
    Public Class Field
        Public Property FieldName As String
    End Class

    'Public Sub cmb_SelectionChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
    '    Dim fldmandatory = fieldlst.FindAll(Function(kj) kj.Mandatory = True)
    '    Dim CAC As New CACserviceClient
    '    Dim documentTypeComboBox As ComboBox = CType(sender, ComboBox)
    '    Dim LookupId As Integer = 0



    '    For i As Integer = 0 To fieldlst.Count - 1

    '        'If fldmandatory(i).FieldName.Trim().ToLower().Replace(" ", "") = "documentcategory" Then

    '        Dim count = VisualTreeHelper.GetChildrenCount(IndexingFieldPanel)

    '        'For r As Integer = 0 To count - 1
    '        Dim child = VisualTreeHelper.GetChild(IndexingFieldPanel, i)
    '        If TypeOf child Is Grid Then
    '            Dim targetItem As Grid = DirectCast(child, Grid)
    '            For Each childcontrol As UIElement In targetItem.Children

    '                If TypeOf childcontrol Is ComboBox Then


    '                    Dim ctl = DirectCast(childcontrol, ComboBox)
    '                    'If fldmandatory(i).FieldName.Trim().ToLower().Replace(" ", "") = "documentcategory" Then
    '                    'MessageBox.Show(ctl.Name)
    '                    'MessageBox.Show(documentTypeComboBox.SelectedItem.ToString())
    '                    'If ctl.Name = "cbo" & fieldlst(i).FieldName.Trim().Replace(".", "") Then
    '                    If (ctl.Name = "cboDocumentCategory") Then



    '                        ' Check if SelectedItem is not Nothing
    '                        If documentTypeComboBox.SelectedItem IsNot Nothing Then
    '                            ' Cast SelectedItem to DataRowView

    '                            Dim selectedRow As DataRowView = CType(documentTypeComboBox.SelectedItem, DataRowView)

    '                            ' Debug: List available columns in the DataRowView
    '                            Dim availableColumns As String = String.Join(", ", selectedRow.Row.Table.Columns.Cast(Of DataColumn)().Select(Function(c) c.ColumnName))

    '                            ' Access the specific column value, replace "Document_type" with the correct column name
    '                            Dim selectedDocumentType As String = selectedRow("Document Type").ToString()

    '                            '  MessageBox.Show(selectedDocumentType)

    '                            ' Example retrieval code





    '                            'If documentCategoryComboBox IsNot Nothing AndAlso Not String.IsNullOrEmpty(selectedDocumentType) Then
    '                            ' Query to fetch categories based on selected document type
    '                            Dim categoryQry As String = "SELECT DISTINCT [Document_category] as [" & fldmandatory(i).FieldName.Trim() & "] FROM [eZDocument_category] WHERE [Document_type] = '" & selectedDocumentType & "'"



    '                            ' Execute the query and bind the results to the document category ComboBox
    '                            ctl.ItemsSource = CAC.GetDatasetByQuery(categoryQry).Tables(0).DefaultView
    '                            ctl.SelectedValuePath = fldmandatory(i).FieldName.Trim()
    '                            ctl.DisplayMemberPath = fldmandatory(i).FieldName.Trim()
    '                            'End If
    '                        End If


    '                        'End If
    '                    End If
    '                    'End If
    '                End If
    '            Next
    '        End If

    '        'Next
    '        'End If
    '        'End If
    '    Next
    'End Sub

    Public Class SubLoans
        Public Property SubLoanType As String
    End Class

    Public Sub cmbcredit_SelectionChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim loanTypeComboBox As ComboBox = CType(sender, ComboBox)
        Dim LookupId As Integer = 0
        'MessageBox.Show("fldmandatory ")
        For i As Integer = 0 To fieldlst.Count - 1
            Try
                Dim count = VisualTreeHelper.GetChildrenCount(IndexingFieldPanel)

                Dim child = VisualTreeHelper.GetChild(IndexingFieldPanel, i)
                If TypeOf child Is Grid Then
                    Dim targetItem As Grid = DirectCast(child, Grid)

                    For Each childcontrol As UIElement In targetItem.Children
                        If TypeOf childcontrol Is ComboBox Then
                            Dim ctl = DirectCast(childcontrol, ComboBox)
                            ' MessageBox.Show(ctl.Name)

                            If ctl.Name = "cboSubLoanType" Then
                                If loanTypeComboBox.SelectedItem IsNot Nothing Then
                                    Dim categoryQry As String = $"SELECT DISTINCT SubLoanType as [Sub Loan Type] FROM [eZSubLoanType] WHERE [LoanType] = '{loanTypeComboBox.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "")}'"
                                    'MessageBox.Show("categoryQry " + categoryQry)
                                    ' Execute the query and bind the results to the document category ComboBox
                                    ctl.ItemsSource = CAC.GetDatasetByQuery(categoryQry).Tables(0).DefaultView
                                    'MessageBox.Show(" ctl.ItemsSource " + ctl.ItemsSource.ToString())
                                    ctl.SelectedValuePath = "Sub Loan Type"
                                    ctl.DisplayMemberPath = "Sub Loan Type"

                                End If
                            End If
                        End If
                    Next
                End If
            Catch ex As InvalidCastException
                MessageBox.Show($"Casting error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Catch ex As NullReferenceException
                MessageBox.Show($"Null reference error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Catch ex As Exception
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        Next
    End Sub
    Public Sub cmb_SelectionChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim fldmandatory = fieldlst.FindAll(Function(kj) kj.Mandatory = True)
        Dim CAC As New CACserviceClient
        Dim documentTypeComboBox As ComboBox = CType(sender, ComboBox)
        Dim LookupId As Integer = 0
        'MessageBox.Show("fldmandatory ")
        For i As Integer = 0 To fieldlst.Count - 1
            Try
                Dim count = VisualTreeHelper.GetChildrenCount(IndexingFieldPanel)

                Dim child = VisualTreeHelper.GetChild(IndexingFieldPanel, i)
                If TypeOf child Is Grid Then
                    Dim targetItem As Grid = DirectCast(child, Grid)

                    For Each childcontrol As UIElement In targetItem.Children
                        If TypeOf childcontrol Is ComboBox Then
                            Dim ctl = DirectCast(childcontrol, ComboBox)
                            'MessageBox.Show("cboDocumentCategory ")
                            If ctl.Name = "cboDocumentCategory" Then
                                If documentTypeComboBox.SelectedItem IsNot Nothing Then
                                    Dim selectedRow As DataRowView = CType(documentTypeComboBox.SelectedItem, DataRowView)
                                    Dim selectedDocumentType As String = selectedRow("Document Type").ToString()
                                    'MessageBox.Show(" selectedDocumentType" + selectedDocumentType.ToString())
                                    Dim categoryQry As String = $"SELECT DISTINCT [Document_category] as [{fldmandatory(i).FieldName.Trim()}] FROM [eZDocument_category] WHERE [Document_type] = '{selectedDocumentType}'"
                                    'MessageBox.Show("categoryQry " + categoryQry)
                                    ' Execute the query and bind the results to the document category ComboBox
                                    ctl.ItemsSource = CAC.GetDatasetByQuery(categoryQry).Tables(0).DefaultView
                                    'MessageBox.Show(" ctl.ItemsSource " + ctl.ItemsSource.ToString())
                                    ctl.SelectedValuePath = fldmandatory(i).FieldName.Trim()
                                    ctl.DisplayMemberPath = fldmandatory(i).FieldName.Trim()


                                End If
                            End If
                        End If
                    Next
                End If
            Catch ex As InvalidCastException
                MessageBox.Show($"Casting error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Catch ex As NullReferenceException
                MessageBox.Show($"Null reference error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Catch ex As Exception
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        Next
    End Sub


    Public Sub txtbox_Leave(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try

            If templateid = 9 Or templateid = 10 Then
                Dim tablename = CAC.GetTableName(templateid)
                If tablename <> "" Then
                    '  Dim lookupstr = objeZLookup(0).LookupValue.ToUpper()
                    Dim qry = "select * from " + tablename + " WITH (NOLOCK)  where [RIM Number]='" + e.Source.ToString().Replace("System.Windows.Controls.TextBox:", "").Trim() + "'"
                    Dim sdataset As DataSet
                    sdataset = CAC.GetDatasetByQuery(qry)
                    If sdataset.Tables(0).Rows.Count > 0 Then
                        For i As Int16 = 0 To fieldlst.Count - 1
                            If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() <> "documenttype" Then
                                If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() <> "rimnumber" Then
                                    If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() <> "qualitycheck" Then
                                        If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() = "lastkycdate" Then
                                            If fieldlst(i).FieldName.Trim().Replace(".", "") <> "Null" Then
                                                SetIndexingValue(fieldlst(i).FieldName.Trim().Replace(".", ""), sdataset.Tables(0).Rows(sdataset.Tables(0).Rows.Count - 1).Item(fieldlst(i).FieldName.Trim().Replace(".", "")).ToString)
                                            End If
                                        Else
                                            SetIndexingValue(fieldlst(i).FieldName.Trim().Replace(".", ""), sdataset.Tables(0).Rows(sdataset.Tables(0).Rows.Count - 1).Item(fieldlst(i).FieldName.Trim().Replace(".", "")).ToString)
                                        End If
                                    End If

                                End If
                            End If

                        Next
                        'QualityCheck
                        ' SaveRecords(CurrentFnInRightPane)
                    Else
                        For i As Int16 = 0 To fieldlst.Count - 1
                            If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() <> "documenttype" Then
                                If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() <> "rimnumber" Then
                                    If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() <> "qualitycheck" Then
                                        SetIndexingControlValues(fieldlst(i).FieldName.Trim(), "")
                                    End If

                                End If
                            End If
                        Next
                    End If
                End If
            End If

        Catch ex As Exception
        End Try
    End Sub
    Private Function SetIndexingValue(ByVal ControlName As String, ByVal ControlValue As String) As Boolean
        Try
            Dim count = VisualTreeHelper.GetChildrenCount(IndexingFieldPanel)
            If count = 0 Then
                Return False
            End If
            For i As Integer = 0 To count - 1
                Dim child = VisualTreeHelper.GetChild(IndexingFieldPanel, i)
                If TypeOf child Is Grid Then
                    Dim targetItem As Grid = DirectCast(child, Grid)
                    For Each childcontrol As UIElement In targetItem.Children
                        If TypeOf childcontrol Is TextBox Then
                            Dim ctl = DirectCast(childcontrol, TextBox)
                            If ctl.Name = "txt" & ControlName.Replace(" ", "") Then
                                For Each childchk As UIElement In targetItem.Children
                                    If TypeOf childchk Is CheckBox Then
                                        Dim chkchild = DirectCast(childchk, CheckBox)
                                        If chkchild.IsChecked = False Then ctl.Text = ControlValue
                                        Return True
                                    End If
                                Next
                            End If
                        ElseIf TypeOf childcontrol Is ComboBox Then
                            Dim ctl = DirectCast(childcontrol, ComboBox)
                            If ctl.Name = "cbo" & ControlName.Replace(" ", "") Then
                                For Each childchk As UIElement In targetItem.Children
                                    If TypeOf childchk Is CheckBox Then
                                        Dim chkchild = DirectCast(childchk, CheckBox)
                                        If chkchild.IsChecked = False Then
                                            If ctl.IsEditable Then
                                                ctl.Text = ControlValue
                                            Else
                                                ctl.SelectedValue = ControlValue
                                            End If
                                            If ControlName.Replace(" ", "") = "QualityCheck" Then
                                                If ControlValue = "" Then
                                                    ctl.SelectedValue = "No"
                                                Else
                                                    ctl.SelectedValue = ControlValue
                                                End If
                                            ElseIf ControlName.Replace(" ", "") = "AccountStatus" Then
                                                If ControlValue = "" Then
                                                    ctl.SelectedValue = "Active"
                                                Else
                                                    ctl.SelectedValue = ControlValue
                                                End If
                                            ElseIf ControlName.Replace(" ", "") = "FileStatus" Then
                                                If ControlValue = "" Then
                                                    ctl.SelectedValue = "Original File Received"
                                                Else
                                                    ctl.SelectedValue = ControlValue
                                                End If
                                            Else
                                                ctl.Text = ControlValue
                                            End If
                                        End If
                                        Return True
                                    End If
                                Next
                            End If
                        ElseIf TypeOf childcontrol Is RadDatePicker Then
                            Dim ctl = DirectCast(childcontrol, RadDatePicker)
                            If ctl.Name = "dt" & ControlName.Replace(" ", "") Then
                                For Each childchk As UIElement In targetItem.Children
                                    If TypeOf childchk Is CheckBox Then
                                        Dim chkchild = DirectCast(childchk, CheckBox)
                                        Dim DTTemp As DateTime
                                        If ControlValue <> "" And Not ControlValue = "null" Then DTTemp = DateTime.Parse(ControlValue)
                                        If chkchild.IsChecked = False Then
                                            If DTTemp = Nothing Then
                                                ctl.DateTimeText = ""
                                            Else
                                                ctl.DateTimeText = DTTemp.ToString("dd/MM/yyyy")
                                            End If
                                        End If
                                        Return True
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetIndexingValue(ByVal ControlName As String) As String
        Dim val = ""
        Try
            Application.Current.Dispatcher.Invoke(DirectCast(Function()
                                                                 Dim count = VisualTreeHelper.GetChildrenCount(IndexingFieldPanel)
                                                                 If count = 0 Then
                                                                     val = ""
                                                                     Return ""
                                                                 End If
                                                                 For i As Integer = 0 To count - 1
                                                                     Dim child = VisualTreeHelper.GetChild(IndexingFieldPanel, i)
                                                                     If TypeOf child Is Grid Then
                                                                         Dim targetItem As Grid = DirectCast(child, Grid)
                                                                         For Each childcontrol As UIElement In targetItem.Children
                                                                             If TypeOf childcontrol Is TextBox Then
                                                                                 Dim ctl = DirectCast(childcontrol, TextBox)
                                                                                 If ctl.Name = "txt" & ControlName.Replace(" ", "") Then


                                                                                     If templateid = TemplateNo AndAlso ControlName.Replace(" ", "").Replace(".", "").ToLower() = "documentuploadby" Then
                                                                                         'txtbox.Text = ecmlogin.LoginName ' Set the TextBox content
                                                                                         Dim sqlquery = "select LoginName from eZECMLogin where ECMLoginId = " + CreateOnId + " and Isdeleted=0"
                                                                                         Dim ds As DataSet = CAC.GetDatasetByQuery(sqlquery)
                                                                                         If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                                                                             ' Get the first value from the first row and first column (adjust as needed)
                                                                                             Dim value As String = ds.Tables(0).Rows(0)(0).ToString()

                                                                                             ' Assign the value to the TextBox
                                                                                             val = value
                                                                                             ctl.Text = value
                                                                                             Return ctl.Text
                                                                                         End If
                                                                                     Else
                                                                                         val = ctl.Text
                                                                                         Return ctl.Text
                                                                                     End If

                                                                                 End If
                                                                             ElseIf TypeOf childcontrol Is ComboBox Then
                                                                                 Dim ctl = DirectCast(childcontrol, ComboBox)
                                                                                 If ctl.Name = "cbo" & ControlName.Replace(" ", "") Then
                                                                                     If ctl.IsEditable Then
                                                                                         If ctl.SelectedValue <> Nothing Then
                                                                                             val = ctl.SelectedValue.ToString
                                                                                             Return ctl.SelectedValue.ToString
                                                                                             'ElseIf ctl.SelectedItem <> Nothing Then
                                                                                             '    val = ctl.SelectedItem.ToString
                                                                                             '    Return ctl.SelectedItem.ToString
                                                                                         Else
                                                                                             val = ctl.Text
                                                                                             Return ctl.Text
                                                                                         End If
                                                                                     Else
                                                                                         If ctl.SelectedValue IsNot Nothing Then
                                                                                             If ctl.SelectedValue.ToString() <> Nothing Then
                                                                                                 val = ctl.SelectedValue.ToString
                                                                                                 Return ctl.SelectedValue.ToString
                                                                                                 'ElseIf ctl.SelectedItem <> Nothing Then
                                                                                                 '    val = ctl.SelectedItem.ToString
                                                                                                 '    Return ctl.SelectedItem.ToString
                                                                                             Else
                                                                                                 val = ctl.Text
                                                                                                 Return ctl.Text
                                                                                             End If

                                                                                         End If

                                                                                     End If
                                                                                     'If ControlName.Replace(" ", "") <> "ProjectName" And ControlName.Replace(" ", "") <> "Beneficiary" And ControlName.Replace(" ", "") <> "CorrespondenceType" Then
                                                                                     '    If ctl.SelectedValue <> Nothing Then
                                                                                     '        val = ctl.SelectedValue.ToString
                                                                                     '        Return ctl.SelectedValue.ToString
                                                                                     '        'ElseIf ctl.SelectedItem <> Nothing Then
                                                                                     '        '    val = ctl.SelectedItem.ToString
                                                                                     '        '    Return ctl.SelectedItem.ToString
                                                                                     '    Else
                                                                                     '        val = ctl.Text
                                                                                     '        Return ctl.Text
                                                                                     '    End If
                                                                                     'Else
                                                                                     '    If ctl.SelectedValue <> Nothing Then
                                                                                     '        val = ctl.SelectedValue.ToString
                                                                                     '        Return ctl.SelectedValue.ToString
                                                                                     '        'ElseIf ctl.SelectedItem <> Nothing Then
                                                                                     '        '    val = ctl.SelectedItem.ToString
                                                                                     '        '    Return ctl.SelectedItem.ToString
                                                                                     '    Else
                                                                                     '        val = ctl.Text
                                                                                     '        Return ctl.Text
                                                                                     '    End If
                                                                                     'End If
                                                                                     'If ctl.SelectedValue <> Nothing Then
                                                                                     '    val = ctl.SelectedValue.ToString
                                                                                     '    Return ctl.SelectedValue.ToString
                                                                                     '    'ElseIf ctl.SelectedItem <> Nothing Then
                                                                                     '    '    val = ctl.SelectedItem.ToString
                                                                                     '    '    Return ctl.SelectedItem.ToString
                                                                                     'Else
                                                                                     '    Return ctl.Text
                                                                                     'End If
                                                                                 End If
                                                                             ElseIf TypeOf childcontrol Is RadDatePicker Then
                                                                                 Dim ctl = DirectCast(childcontrol, RadDatePicker)
                                                                                 If ctl.Name = "dt" & ControlName.Replace(" ", "") Then
                                                                                     'Return Format(ctl.DateTimeText, "MM/dd/yyyy")
                                                                                     If ctl.DateTimeText <> "" Then
                                                                                         '"dd/MM/yyyy"
                                                                                         '  Dim str As String = DateStringToString(ctl.SelectedDate + " 00:0:00 AM", 0, "MM/dd/yyyy")
                                                                                         ' Dim form = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.GetFormat()
                                                                                         Dim str As String = DateStringToString(ctl.SelectedDate, 0, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern)
                                                                                         val = str
                                                                                         Return str
                                                                                     Else
                                                                                         val = "null"
                                                                                         Return "null"
                                                                                     End If
                                                                                 End If
                                                                             End If
                                                                         Next
                                                                     End If
                                                                 Next
                                                                 val = ""
                                                                 Return ""
                                                             End Function, Action))
            Return val
        Catch ex As Exception
            Return val
        End Try
    End Function
    Public Function DateStringToString(ByVal dt As String, ByVal WithTime As Boolean, ByVal sysformat As String) As String
        Try
            If dt <> "" Then
                Dim dateValue As DateTime
                DateTime.TryParseExact(dt, sysformat, CultureInfo.InvariantCulture, DateTimeStyles.None, dateValue)
                If WithTime Then
                    dt = dateValue.ToString()
                    dt = dateValue.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                Else
                    dt = dateValue.ToString()
                    dt = dateValue.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                End If
                Return dt
            Else
                Return dt
            End If
        Catch ex As Exception
            Dim exc As String
            exc = "ERROR CODE:WSR740F220 " + ex.Message.ToString()
        End Try
    End Function
    Public Sub Getrecords(ByVal _currentFileName As String)
        Try
            Dim CAC As New CACserviceClient
            If templateid <> 0 Then
                If _currentFileName <> "" Then
                    Dim sdataset As DataSet
                    sdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", _currentFileName)
                    If IndexingFieldPanel.Children.Count > 2 Then
                        If sdataset.Tables.Count > 0 And sdataset.Tables(0).Rows.Count > 0 Then
                            For i As Int16 = 0 To fieldlst.Count - 1
                                If IsDBNull(sdataset.Tables(0).Rows(0).Item(fieldlst(i).FieldName.Trim())) Then
                                    SetIndexingValue(fieldlst(i).FieldName.Trim().Replace(".", ""), "")
                                Else
                                    SetIndexingValue(fieldlst(i).FieldName.Trim().Replace(".", ""), sdataset.Tables(0).Rows(0).Item(fieldlst(i).FieldName.Trim()))
                                End If
                            Next
                        Else
                            For i As Int16 = 0 To fieldlst.Count - 1
                                SetIndexingValue(fieldlst(i).FieldName.Trim().Replace(".", ""), "")
                            Next
                        End If
                    End If
                End If
            End If
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Public Sub ClearFields()
        Try
            For i As Int16 = 0 To fieldlst.Count - 1
                SetIndexingControlValues(fieldlst(i).FieldName.Trim(), "")
            Next
        Catch ex As Exception
        End Try
    End Sub
    Public Sub IndexingFieldEnabled(ByVal val As Boolean)
        Try
            For i As Int16 = 0 To fieldlst.Count - 1
                SetIndexingEnabled(fieldlst(i).FieldName.Trim(), val)
            Next
        Catch ex As Exception
        End Try
    End Sub
    Public Sub SaveRecords(ByVal _currentFileName As String)
        Try
            Dim CAC As New CACserviceClient
            If templateid <> 0 Then

                Dim flag = 0
                'cabinetName = ddlstcab.Text.ToString()
                'TemplateName = ddlsttem.Text.ToString()
                'GetERSPath()
                'Getdbstruct(_currentFileName)
                'filldb(_currentFileName)

                'If IO.File.Exists(Volume & "\" & pdfilename & ".pdf") Then
                '    Dim v As New Versioning(Path.GetFileName(Volume & "\" & pdfilename & ".pdf"))
                '    v.ShowDialog()
                '    If cancontinue = 0 Then
                '        'ConvertToPDF = 2
                '        'Exit Function
                '        flag = 1
                '    End If
                'End If
                If flag = 0 Then
                    Dim tblname = "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_stage"
                    Dim sdataset As DataSet
                    Dim sqlstring As New System.Text.StringBuilder
                    Dim fl As Integer = 0
                    If _currentFileName <> "" Then

                        sdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", _currentFileName)
                        If sdataset.Tables.Count > 0 And sdataset.Tables(0).Rows.Count > 0 Then
                            Dim fldinsert As New System.Text.StringBuilder("")
                            fldinsert.Append("Update " + tblname + " set ")

                            Dim DocumentType = ""
                            Dim AccNo = ""
                            Dim AccType = ""
                            Dim loginname = ""
                            For i As Int16 = 0 To fieldlst.Count - 1

                                Dim indexval = GetIndexingValue(fieldlst(i).FieldName.Trim().Replace(".", "")).Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim()



                                If indexval = "null" Then
                                    fldinsert.Append("[" + fieldlst(i).FieldName.Trim() & "]=" & "" & indexval & ",")
                                Else

                                    If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() = "documenttype" Then
                                        '  If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() <> "documenttype" Then
                                        DocumentType = indexval.Replace("'", "''").ToLower().Trim()
                                    ElseIf fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() = "accountnumber" Then
                                        AccNo = indexval.Replace("'", "''").Trim()
                                    ElseIf fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() = "accounttype" Then
                                        AccType = indexval.Replace("'", "''").Trim()
                                    End If

                                    If fieldlst(i).DataTypeId = 1 And indexval.Trim = "" Then
                                        fldinsert.Append("[" + fieldlst(i).FieldName.Trim() & "]=null,")

                                    ElseIf fieldlst(i).DataTypeId = 5 Then
                                        fldinsert.Append("[" + fieldlst(i).FieldName.Trim() & "]=Convert(DateTime,'" + indexval.Trim() + "',101),")


                                    Else
                                        If fieldlst(i).FieldName.Trim() = "QualityCheck" AndAlso indexval.Replace("'", "''") = "" Then
                                            fldinsert.Append("[" + fieldlst(i).FieldName.Trim() & "]=" & "N'No',")
                                        ElseIf fieldlst(i).FieldName.Trim() = "FileStatus" AndAlso indexval.Replace("'", "''") = "" Then
                                            fldinsert.Append("[" + fieldlst(i).FieldName.Trim() & "]=" & "N'Original File Received',")
                                        Else

                                            fldinsert.Append("[" + fieldlst(i).FieldName.Trim() & "]=" & "N'" & indexval.Replace("'", "''") & "',")
                                        End If

                                    End If

                                End If
                            Next
                            sqlstring = (fldinsert.Remove(Len(fldinsert.ToString) - 1, 1))
                            sqlstring.Append(",createdby='" + CreateOnId + "' where itemid=" & sdataset.Tables(0).Rows(0).Item(0).ToString)
                            stageitmid = sdataset.Tables(0).Rows(0).Item(0).ToString
                            If CAC.InsertAndUpdateAndDeleteeZUserDefined(sqlstring.ToString()) = 1 Then
                                'MsgBox(sqlstring.ToString())
                            Else
                                MsgBox("Due to some Error while Save Records : " + sqlstring.ToString())
                            End If
                            '   MessageBox.Show("Account Number: " + AccNo + " DocumentType :" + DocumentType, "Important Message")
                            If DocumentType = "deposit" And AccNo = "" And AccType = "" Then
                                MessageBox.Show("Account Number/Account Type is missing for this Deposit Document !", "Important Message")
                            ElseIf DocumentType = "deposit" And AccNo = "" And AccType <> "" Then
                                MessageBox.Show("Account Number is missing for this Deposit Document !", "Important Message")
                            ElseIf DocumentType = "deposit" And AccNo <> "" And AccType = "" Then
                                MessageBox.Show("Account Type is missing for this Deposit Document !", "Important Message")
                            End If
                        Else
                            Dim DocumentType = ""
                            Dim AccNo = ""
                            Dim AccType = ""
                            Dim fldinsert As New System.Text.StringBuilder("")
                            sqlstring.Append("Insert into " + tblname + " (")
                            For i As Int16 = 0 To fieldlst.Count - 1
                                sqlstring.Append("[" + fieldlst(i).FieldName.Trim() & "],")
                                Dim indexval = GetIndexingValue(fieldlst(i).FieldName.Trim().Replace(".", "")).Trim()
                                If indexval = "null" Then
                                    fldinsert.Append("" & indexval & ",")
                                Else
                                    If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() = "documenttype" Then
                                        '  If fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() <> "documenttype" Then
                                        DocumentType = indexval.Replace("'", "''").ToLower().Trim()
                                    ElseIf fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() = "accountnumber" Then
                                        AccNo = indexval.Replace("'", "''").Trim()
                                    ElseIf fieldlst(i).FieldName.Trim().Replace(".", "").Replace(" ", "").ToLower() = "accounttype" Then
                                        AccType = indexval.Replace("'", "''").Trim()
                                    End If
                                    If fieldlst(i).DataTypeId = 1 And indexval = "" Then
                                        fldinsert.Append("null,")
                                    ElseIf fieldlst(i).DataTypeId = 5 Then
                                        fldinsert.Append("Convert(DateTime,'" + indexval.Trim() + "',101),")


                                    Else
                                        If fieldlst(i).FieldName.Trim() = "QualityCheck" AndAlso indexval.Replace("'", "''") = "" Then
                                            fldinsert.Append("N'No',")
                                        ElseIf fieldlst(i).FieldName.Trim() = "FileStatus" AndAlso indexval.Replace("'", "''") = "" Then
                                            fldinsert.Append("N'Original File Received',")
                                        Else
                                            fldinsert.Append("N'" & indexval.Replace("'", "''") & "',")
                                        End If


                                    End If

                                End If
                            Next

                            GetERSPath()
                            sqlstring.Append("templateid,ifilepath,ifilename ,ifiletype,version ,createdby,updatedby,dtitle,dauthor,dsubject," +
    "dkeywords,checkout,checkoutpath,checkoutby,dstatus,dsize,nopages,CreatedOn,UpdatedOn," +
    "Isdeleted,ersid,ezfrom) values(")
                            sqlstring.Append(fldinsert)
                            sqlstring.Append("'" + templateid.ToString() + "', '" + Imaging + "', '" + _currentFileName + "', 'tif', '0' ,'" +
    CreateOnId + "', '" + CreateOnId + "', '', '', '', '', '', '', '0', '', '', '', '" +
    CAC.DateDateTimeToString(DateTime.Now, 1) + "', '" + CAC.DateDateTimeToString(DateTime.Now, 1) + "', " +
    "'0'," & ErsId & ",'ECM-CAPTURE(" + Environment.MachineName + ")')")
                            'vinsi oct 24 ,2019
                            stageitmid = CAC.InsertAndUpdateAndDeleteeZUserDefinedWithScope(sqlstring.ToString())
                            If stageitmid > 0 Then
                                '   MsgBox(sqlstring.ToString())
                            Else
                                MsgBox("Due to some Error while Save Records : " + sqlstring.ToString())
                            End If
                            '   MessageBox.Show("Account Number: " + AccNo + " DocumentType :" + DocumentType, "Important Message")
                            If DocumentType = "deposit" And AccNo = "" And AccType = "" Then
                                MessageBox.Show("Account Number/Account Type is missing for this Deposit Document !", "Important Message")
                            ElseIf DocumentType = "deposit" And AccNo = "" And AccType <> "" Then
                                MessageBox.Show("Account Number is missing for this Deposit Document !", "Important Message")
                            ElseIf DocumentType = "deposit" And AccNo <> "" And AccType = "" Then
                                MessageBox.Show("Account Type is missing for this Deposit Document !", "Important Message")
                            End If
                        End If
                    End If
                Else
                    Kill(_currentFileName)
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
        End Try
    End Sub
    Public Function SetIndexingControlValues(ByVal ControlName As String, ByVal ControlValue As String) As Boolean
        Try
            ControlName = ControlName.Replace(".", "")
            Dim count = VisualTreeHelper.GetChildrenCount(IndexingFieldPanel)
            If count = 0 Then
                Return False
            End If
            For i As Integer = 0 To count - 1
                Dim child = VisualTreeHelper.GetChild(IndexingFieldPanel, i)
                If TypeOf child Is Grid Then
                    Dim targetItem As Grid = DirectCast(child, Grid)
                    For Each childcontrol As UIElement In targetItem.Children
                        If TypeOf childcontrol Is TextBox Then
                            Dim ctl = DirectCast(childcontrol, TextBox)
                            If ctl.Name = "txt" & ControlName.Replace(" ", "") Then
                                ctl.Text = ControlValue
                                If SelectZoneIsClicked = True Then Call LoadZonesToFile(ctl.Name.ToString)
                            End If
                        ElseIf TypeOf childcontrol Is ComboBox Then
                            Dim ctl = DirectCast(childcontrol, ComboBox)
                            If ctl.Name = "cbo" & ControlName.Replace(" ", "") Then
                                'have to do
                                If ctl.IsEditable Then
                                    ctl.Text = ControlValue
                                Else
                                    ctl.SelectedValue = ControlValue
                                End If


                                '  If SelectZoneIsClicked = True Then Call LoadZonesToFile(ctl.Name.ToString)
                            End If
                        ElseIf TypeOf childcontrol Is CheckBox Then
                            Dim ctl = DirectCast(childcontrol, CheckBox)
                            If ctl.Name = "chk" & ControlName.Replace(" ", "") Then
                                'have to do
                                ctl.IsChecked = False
                                'If ControlName.Replace(" ", "") <> "ProjectName" And ControlName.Replace(" ", "") <> "Beneficiary" And ControlName.Replace(" ", "") <> "CorrespondenceType" Then
                                '    ctl.SelectedValue = ControlValue
                                'Else
                                '    ctl.Text = ControlValue
                                'End If
                                'If SelectZoneIsClicked = True Then Call LoadZonesToFile(ctl.Name.ToString)
                            End If
                            '
                        ElseIf TypeOf childcontrol Is RadDatePicker Then
                            Dim ctl = DirectCast(childcontrol, RadDatePicker)
                            If ctl.Name = "dt" & ControlName.Replace(" ", "") Then
                                ctl.DateTimeText = ControlValue
                                If SelectZoneIsClicked = True Then Call LoadZonesToFile(ctl.Name.ToString)
                            End If
                        End If
                    Next
                End If
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function SetIndexingEnabled(ByVal ControlName As String, ByVal ControlValue As Boolean) As Boolean
        Try
            Dim count = VisualTreeHelper.GetChildrenCount(IndexingFieldPanel)
            If count = 0 Then
                Return False
            End If
            For i As Integer = 0 To count - 1
                Dim child = VisualTreeHelper.GetChild(IndexingFieldPanel, i)
                If TypeOf child Is Grid Then
                    Dim targetItem As Grid = DirectCast(child, Grid)
                    For Each childcontrol As UIElement In targetItem.Children
                        If TypeOf childcontrol Is TextBox Then
                            Dim ctl = DirectCast(childcontrol, TextBox)
                            If ctl.Name = "txt" & ControlName Then
                                ctl.IsEnabled = ControlValue
                            End If
                        ElseIf TypeOf childcontrol Is ComboBox Then
                            Dim ctl = DirectCast(childcontrol, ComboBox)
                            If ctl.Name = "cbo" & ControlName Then
                                ctl.IsEnabled = ControlValue
                            End If
                        ElseIf TypeOf childcontrol Is RadDatePicker Then
                            Dim ctl = DirectCast(childcontrol, RadDatePicker)
                            If ctl.Name = "dt" & ControlName Then
                                ctl.IsEnabled = ControlValue
                            End If
                        End If
                    Next
                End If
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function DeleteRecords(ByVal _currentFileName As String) As Boolean
        Try
            Dim CAC As New CACserviceClient
            If templateid <> 0 Then
                Dim tblname = "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_stage"
                Dim sdataset As New DataSet
                sdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", _currentFileName)
                If sdataset.Tables(0).Rows.Count > 0 Then
                    If CAC.InsertAndUpdateAndDeleteeZUserDefined("delete from " + tblname + " where itemid= " & sdataset.Tables(0).Rows(0).Item(0).ToString) = 1 Then
                    Else
                        MsgBox("Due to some Error while Delete Records")
                    End If
                End If
            End If
            Return True
        Catch ex As SqlClient.SqlException
            MsgBox(ex.Message.ToString)
            Return False
        End Try
    End Function

    Public Function GetContextMenu(ByVal Image As Leadtools.RasterImage) As System.Windows.Forms.ContextMenuStrip
        Try
            Dim ConMenu As New System.Windows.Forms.ContextMenuStrip
            For i As Integer = 0 To fieldlst.Count - 1
                Dim item1 As New System.Windows.Forms.ToolStripMenuItem()
                item1.Text = fieldlst(i).FieldName.Replace(".", "").Trim()
                AddHandler item1.Click, AddressOf mnuItem_Click
                ConMenu.Items.Add(item1)
                ECMImage = Image

                'If filePath.EndsWith(".pdf") Then
                '    ' Assign the image for TIFF
                '    ECMImage = Image ' Assuming Viewer.Image gets the image representation
                'Else
                '    ' Assign the image for PDF
                '    Dim pdfDocument As PdfDocument = pdfDocument.Load(filePath)
                '    ECMImage = ExtractImageFromPdf(pdfDocument)
                'End If
            Next
            'ECMViewer.Viewer.ContextMenuStrip = ConMenu
            Return ConMenu
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Sub mnuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim S As String = CType(sender, System.Windows.Forms.ToolStripMenuItem).Text
            SetIndexingControlValues(S, _recognitionResults)
        Catch ex As Exception
        End Try
    End Sub
    Public Sub Getdbstruct(ByVal filename As String)
        Try
            Dim CAC As New CACserviceClient
            ezpdf = New eZPdfProperties
            ezTempField = New List(Of eZTemplateField)
            Dim obj As New List(Of eZPdfProperties)
            obj = CAC.SelectedeZPdfPropertiesList("TemplateId", templateid.ToString())
            For i As Integer = 0 To obj.Count - 1
                ezpdf = obj(0)
            Next
            ezTempField = CAC.SelectedeZTemplateFieldListForPdfCreation("TemplateId", templateid.ToString())
        Catch ex As SqlClient.SqlException
            writetxtfle("Error From Pdf Properties.", filename)
            'MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Public Sub CSVFileAppend(ByVal csvvalues As String)
        Try
            Dim loc = "C:\QCReport\Archived"
            Dim filename As String = loc + "\" + DateTime.Now.ToString("ddMMMyyyy") + ".csv"
            If Not IO.Directory.Exists(loc) Then
                IO.Directory.CreateDirectory(loc)
            End If
            If Not IO.File.Exists(filename) Then
                Dim str(1) As String
                str(0) = "Barcode-Value,UserName,Time"
                IO.File.WriteAllLines(filename, str)
            End If
            Dim exststr() As String = IO.File.ReadAllLines(filename)
            Array.Resize(exststr, exststr.Length + 1)
            exststr(exststr.Length - 1) = csvvalues
            IO.File.WriteAllLines(filename, exststr)
        Catch ex As Exception
        End Try
    End Sub
    Public LogFileName As String = ""
    Public Function dir() As String
        Dim source As String = ""
        Try
            Dim apppath As String = ""
            apppath = System.AppDomain.CurrentDomain.BaseDirectory
            'apppath = System.Reflection.Assembly.GetEntryAssembly().Location
            'apppath = apppath.Replace("OutlookSync Service\OutlookSyncService.exe", "OutlookSync Service")
            source = apppath + "Log"
            If Not Directory.Exists(source) Then   'Checking Directory Exist or Not
                Directory.CreateDirectory(source)
            End If
        Catch ex As Exception
        End Try
        Return source
    End Function
    Public Sub writetxtfle(ByVal msg As String, ByVal filename As String)
        Try
            Dim filelocation As String = dir() & "\" & LogFileName & ".csv"
            If Not File.Exists(filelocation) Then
                Dim fs = File.Create(filelocation)
                fs.Close()
            End If
            Dim lines As New ArrayList()
            Dim line As String = ""
            Dim lastline As String = ""
            Using r As New StreamReader(filelocation)
                line = r.ReadLine()
                While line IsNot Nothing
                    lines.Add(line)
                    line = r.ReadLine()
                End While
                If lines.Count > 0 Then
                    lastline = lines(lines.Count - 1).ToString()
                End If
                r.Close()
            End Using
            Using sw As StreamWriter = New StreamWriter(filelocation, True)
                If lines.Count = 0 Then
                    sw.Write("DateTime")
                    sw.Write(",")
                    sw.Write("Status")
                    sw.Write(",")
                    sw.Write("Filename")
                    sw.Write(sw.NewLine())
                End If
                If Not lastline.EndsWith(msg) Then
                    If msg.Trim().Trim(" ") <> "" Or msg <> Environment.NewLine Then
                        sw.Write(sw.NewLine())
                        sw.Write(Format(DateTime.Now, "MM/dd/yyyy hh:mm:ss"))
                        sw.Write(",")
                        sw.Write(msg)
                        sw.Write(",")
                        sw.Write(filename)
                        sw.Write(sw.NewLine())
                    End If
                End If
                sw.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Function CheckIndexingForExportEngine(ByVal Filename As String, ByRef Itemid As String, ByRef RimNumber As String) As Boolean
        Try
            Dim chkfieldlst = CAC.SelectedeZTemplateFieldList("TemplateId", templateid.ToString())
            Dim chksdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", Filename)
            If chksdataset.Tables(0).Rows.Count > 0 Then
                For i As Int16 = 0 To chkfieldlst.Count - 1
                    If chksdataset.Tables(0).Rows(0).Item(chkfieldlst(i).FieldName.Trim()).ToString() = "" And chkfieldlst(i).Mandatory Then
                        ' MessageBox.Show("Please enter the value for " + chkfieldlst(i).FieldName.Trim())


                        Return False
                    End If
                    If chkfieldlst(i).FieldName.Replace(" ", "").Replace(".", "").ToLower() = "rimnumber" Then
                        RimNumber = chksdataset.Tables(0).Rows(0).Item(chkfieldlst(i).FieldName.Trim()).ToString()
                    End If
                Next

                Itemid = chksdataset.Tables(0).Rows(0).Item("Itemid").ToString()
                Return True
            Else
                ' Itemid = chksdataset.Tables(0).Rows(0).Item("Itemid").ToString()
                ' MessageBox.Show("Datas Not Found for " + Filename)
                Return False
            End If
        Catch ex As Exception
            MsgBox("Error From Check Indexing : " + ex.ToString)
            Return False
        End Try
    End Function

    Public Function CheckIndexing(ByVal Filename As String) As Boolean
        Try
            Dim chkfieldlst = CAC.SelectedeZTemplateFieldList("TemplateId", templateid.ToString())
            Dim chksdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", Filename)
            If chksdataset.Tables(0).Rows.Count > 0 Then
                For i As Int16 = 0 To chkfieldlst.Count - 1
                    If chksdataset.Tables(0).Rows(0).Item(chkfieldlst(i).FieldName.Trim()).ToString() = "" And chkfieldlst(i).Mandatory Then
                        ' MessageBox.Show("Please enter the value for " + chkfieldlst(i).FieldName.Trim())
                        Return False
                    End If
                Next
                '  itemid = chksdataset.Tables(0).Rows(0).Item("Itemid").ToString()
                Return True
            Else
                ' MessageBox.Show("Datas Not Found for " + Filename)
                Return False
            End If
        Catch ex As Exception
            MsgBox("Error From Check Indexing : " + ex.ToString)
            Return False
        End Try
    End Function
    Public Function ExportToEngine(ByVal filenames As String) As Integer

        Try
            Dim ExportCheck As Integer = 0
            If GetERSPath() = True Then
                Getdbstruct(filenames)
                Application.Current.Dispatcher.Invoke(DirectCast(Sub()
                                                                     cabinetName = ddlstcab.Text.ToString()
                                                                     TemplateName = ddlsttem.Text.ToString()
                                                                 End Sub, Action))
                vers = "1.0"
                itemid = "0"
                Dim stagaeitemid = "0"
                Dim RIMNumber = ""
                If CheckIndexingForExportEngine(Imaging + "\" + filenames, stagaeitemid, RIMNumber) Then
                    If stagaeitemid <> "0" Then
                        Dim result As Boolean = filldb(Imaging + "\" + filenames)
                        Dim oldfilename = pdfilename.Trim
                        cancontinue = 1
                        Try
                            Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials

                                If unc.NetUseWithCredentials(AppconDB("UNCpath"), AppconDB("Username"), AppconDB("Domain"), AppconDB("Password")) Then

                                    If IO.File.Exists(Volume & "\" & pdfilename & ".ezo") Or IO.File.Exists(Volume & "\" & pdfilename & ".pdf") Then

                                        Application.Current.Dispatcher.Invoke(DirectCast(Sub()
                                                                                             Dim v As New Versioning(Path.GetFileName(Volume & "\" & pdfilename & ".pdf"), RIMNumber)
                                                                                             v.ShowDialog()
                                                                                         End Sub, Action))
                                    End If
                                Else
                                    MessageBox.Show("The path " + AppconDB("UNCpath") + " Not Connected , Please check with your IT Team")
                                    Exit Function
                                End If
                            End Using

                            If cancontinue = 0 Then
                                ' Kill(Imaging + "\" + filenames)
                                Exit Function
                            End If
                        Catch ex As Exception
                            MessageBox.Show("impersonation issue : " + ex.Message.ToString())
                        End Try

                        Try

                            filenames = Imaging + "\" + filenames
                            'vinsi Oct 25, 2019
                            Dim currdate As String = CAC.DateDateTimeToString(DateTime.Now, 1)
                            Dim ext = System.IO.Path.GetExtension(filenames).Replace(".", "")
                            Dim new_file_name = DateTime.Now.ToString("ddMMyyyyhhmmssfff") + "." + ext
                            Dim destpath = Monitorpath + "\" + templateid.ToString() + "\" + new_file_name
                            If IO.Directory.Exists(System.IO.Path.GetDirectoryName(destpath)) = False Then IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destpath))
                            'If File.Exists(destpath) Then
                            '    new_file_name = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + ext
                            '    destpath = Monitorpath + "\" + templateid.ToString() + "\" + new_file_name
                            'End If

                            File.Copy(filenames, destpath)
                            If File.Exists(destpath) Then
                                Dim ScanedTypestr = "Scanned"
                                Dim ExportAtQuery = "select scanedfile from  ezBatchProcessing WITH (NOLOCK) where batchid in (select batchid from ezbatchfiles WITH (NOLOCK) where Filename ='" + Path.GetFileName(filenames) + "') "
                                Dim ScanedtYpeDS = CAC.GetDatasetByQuery(ExportAtQuery)
                                If Not IsNothing(ScanedtYpeDS) AndAlso ScanedtYpeDS.Tables.Count > 0 AndAlso ScanedtYpeDS.Tables(0).Rows.Count > 0 Then
                                    If ScanedtYpeDS.Tables(0).Rows(0)(0).ToString().Contains("DCMS") Then
                                        ScanedTypestr = "Scanned"
                                    Else
                                        ScanedTypestr = "Digital"
                                    End If
                                End If


                                Dim QueryU = "update eZBatchFiles set [RIMNumber]='" + RIMNumber + "',[ExportedOn]='" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + "',[ExportedAt]='" + ScanedTypestr + "(" + Environment.MachineName + ")' where [Filename]='" + Path.GetFileName(filenames) + "'"
                                '   MessageBox.Show(QueryU)
                                If CAC.InsertAndUpdate(QueryU) > 0 Then

                                End If
                                Dim query = "insert into eZFileUpload(Templateid,Itemid,filepath,filetype,filename,status,UploadFrom,UploadAt,CreatedOn,CreatedBy,Isdeleted,cancontinue)values(" + templateid.ToString() + "," + stagaeitemid + ",'" + System.IO.Path.GetDirectoryName(destpath) + "','" + ext + "','" + new_file_name + "','Export','ECM-Capture','" + Environment.MachineName + "','" + currdate + "'," + CreateOnId + ",'0','" + cancontinue.ToString() + "')"

                                Dim itemid As Integer = CAC.InsertAndUpdateAndDeleteeZUserDefinedWithScope(query)
                                If itemid <> 0 Then
                                    writetxtfle("Record Saved Successfully in eZFileupload table ", filenames)
                                    Dim sql = "update eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_stage set ifilepath='" + System.IO.Path.GetDirectoryName(destpath) + "',ifilename='" + new_file_name + "',updatedon='" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + "',updatedby='" + CreateOnId.ToString() + "',Ezfrom='" + ScanedTypestr + "(" + Environment.MachineName + ")' where itemid='" + stagaeitemid.ToString() + "'"
                                    '  MessageBox.Show(sql)
                                    Dim i = CAC.InsertAndUpdateAndDeleteeZUserDefined(sql)
                                    writetxtfle("item id : " + i.ToString(), filenames)
                                    'Dim ecmViewerControl As ECMViewer = Me.ecmViewerControl

                                    '' Ensure the reference is not Nothing before calling methods

                                    'If ecmViewerControl IsNot Nothing Then
                                    '    ecmViewerControl.DisposePdfViewerResources()
                                    'Else
                                    '    MessageBox.Show("ECMViewer control is not initialized.")
                                    'End If
                                    'DeleteRecords(filenames)
                                    Kill(filenames)
                                    ' DeleteFileUsingCmd(filenames)
                                    Return 1
                                Else
                                    writetxtfle("Record Not Saved in eZFileupload table ", filenames)
                                    Return 0
                                End If
                            Else
                                writetxtfle("File Not copied into Monitor path : " + Monitorpath, filenames)
                                Return 0
                            End If
                        Catch ex As Exception
                            writetxtfle("Error From ExportPdf  " + ex.ToString, filenames)
                            Return 0
                        Finally
                            'fldinsert = Nothing
                            'sqlstring = Nothing
                        End Try
                    End If

                Else
                    writetxtfle("Check Mandatory Fields : ", Imaging + "\" + filenames)
                    Return 0
                End If
                System.Windows.Forms.Application.DoEvents()
            Else
                writetxtfle("ERS Path Not Found ", Imaging + "\" + filenames)
                Return 0
            End If
        Catch ex As Exception
            If errstr <> "" Then
                writetxtfle("Error From ExportTiff Files : " + errstr, filenames)
                Return 0
            Else
                writetxtfle("Error From ExportTiff Files : " + ex.ToString, filenames)
                Return 0
            End If
        Finally
        End Try

    End Function

    Public Sub DeleteFileUsingCmd(filePath As String)
        Dim process As New Process()

        ' Setting up the process configuration to use cmd.exe
        process.StartInfo.FileName = "cmd.exe"
        process.StartInfo.UseShellExecute = False
        process.StartInfo.RedirectStandardInput = True
        process.StartInfo.RedirectStandardOutput = True
        process.StartInfo.RedirectStandardError = True
        process.StartInfo.CreateNoWindow = True  ' Avoid creating a CMD window

        Try
            process.Start()

            ' Using a StreamWriter to input commands into cmd
            Using sw As StreamWriter = process.StandardInput
                If sw.BaseStream.CanWrite Then
                    ' Del command to delete the file
                    sw.WriteLine($"del /f /q ""{filePath}""")
                End If
            End Using

            ' Optional: Reading outputs
            Dim output As String = process.StandardOutput.ReadToEnd()
            Dim errors As String = process.StandardError.ReadToEnd()

            ' Close the process
            process.WaitForExit()

            ' Output results (for debugging purpose)
            Console.WriteLine("Output: " + output)
            Console.WriteLine("Errors: " + errors)

        Catch ex As Exception
            Console.WriteLine("Exception: " + ex.Message)
        Finally
            If Not process.HasExited Then
                process.Kill()
            End If
            process.Dispose()
        End Try
    End Sub
    Public Function ExportTifFiles(ByVal filenames As String) As Integer
        Dim ExtentTiff As String = "pdf"
        Try
            Dim ExportCheck As Integer = 0
            If GetERSPath() = True Then
                Getdbstruct(filenames)
                vers = "1.0"
                itemid = ""
                If CheckIndexing(Imaging + "\" + filenames) Then
                    Dim result As Boolean = filldb(Imaging + "\" + filenames)
                    Dim oldfilename = pdfilename.Trim
                    If result = True And sTitle <> "" And sSubject <> "" And sAuthor <> "" And sRemarks <> "" And sPdfSignature <> "" And pdfilename <> "" Then
                        Dim res As Integer = ExportToArchive.ExportAsPdf(sTitle.Trim, sSubject.Trim, sAuthor.Trim, sRemarks.Trim, sPdfSignature.Trim, pdfilename.Trim, Imaging + "\" + filenames.ToString, Volume.Trim)
                        If res = 1 Or res = 3 Then
                            If res = 1 Then


                                If ExportPdf(pdfilename, Volume + "\", Imaging + "\" + filenames, ExtentTiff, oldfilename) = True Then
                                    Kill(Imaging + "\" + filenames)
                                    Dim folderin As System.IO.DirectoryInfo
                                    Dim filein As System.IO.FileInfo
                                    folderin = New System.IO.DirectoryInfo(Imaging)
                                    Dim firstfile As String = ""
                                    For Each filein In folderin.GetFiles("*.xls")
                                        firstfile = filein.Name
                                    Next
                                    If System.IO.File.Exists(Imaging + "\" + firstfile) Then
                                        Kill(Imaging + "\" & firstfile)
                                    End If
                                    Return 1
                                Else
                                    Kill(Volume.Replace("/", "-").Trim(" ").Replace(":", ":") & "\" & pdfilename)
                                    Return 0
                                End If
                            Else
                                Kill(Imaging + "\" + filenames)
                                Dim folderin As System.IO.DirectoryInfo
                                Dim filein As System.IO.FileInfo
                                folderin = New System.IO.DirectoryInfo(Imaging)
                                Dim firstfile As String = ""
                                For Each filein In folderin.GetFiles("*.xls")
                                    firstfile = filein.Name
                                Next
                                If System.IO.File.Exists(Imaging + "\" + firstfile) Then
                                    Kill(Imaging + "\" & firstfile)
                                End If
                                Return 1
                            End If
                        ElseIf res = 2 Then
                            writetxtfle("Version Creation Skipped ", Imaging + "\" + filenames)
                            Return 0
                        Else
                            writetxtfle("Error in Pdf Creation : " + errstr, Imaging + "\" + filenames)
                            Return 0
                        End If
                        Dim filinf As System.IO.FileInfo
                        Dim dirinf = New System.IO.DirectoryInfo(Imaging)
                        For Each filinf In dirinf.GetFiles("*.txt")
                            If filinf.Name.Contains("_pg_") Or filinf.Name.Contains("_pg_0.") Then
                                Kill(Imaging + "\" + filinf.Name)
                            End If
                        Next
                    Else
                        Return 0
                    End If
                Else
                    writetxtfle("Check Mandatory Fields : ", Imaging + "\" + filenames)
                End If
                System.Windows.Forms.Application.DoEvents()
            Else
                writetxtfle("ERS Path Not Found ", Imaging + "\" + filenames)
                Return 0
            End If
        Catch ex As Exception
            If errstr <> "" Then
                writetxtfle("Error From ExportTiff Files : " + errstr, filenames)
                Return 0
            Else
                writetxtfle("Error From ExportTiff Files : " + ex.ToString, filenames)
                Return 0
            End If
        Finally
        End Try
    End Function
    Public Function ExportOtherFiles(ByVal filenames As String) As Integer
        Dim ExtentTiff As String = "pdf"
        Try
            Using unc As ConnectUNCWithCredentials = New ConnectUNCWithCredentials
                If unc.NetUseWithCredentials(AppconDB("UNCpath"), AppconDB("Username"), AppconDB("Domain"), AppconDB("Password")) Then
                    Dim ExportCheck As Integer = 0
                    If GetERSPath() = True Then
                        Getdbstruct(filenames)
                        If CheckIndexing(Imaging + "\" + filenames) Then
                            Dim result As Boolean = filldb(Imaging + "\" + filenames)
                            Dim Cf As New pdfconvertor
                            Dim oldfilename = pdfilename.Trim
                            Dim Extension As String = System.IO.Path.GetExtension(Imaging & "\" & filenames).Replace(".", "").ToUpper
                            If result = True Then
                                Dim res = Cf.ConvertToOther(Extension.Trim, Volume.Trim, pdfilename.Trim, Imaging + "\" + filenames, Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff"))
                                If res = 1 Or res = 3 Then
                                    'ExportExtention = 0
                                    If res = 1 Then
                                        '   Extension = ".ezo"
                                        If ExportPdf(pdfilename, Volume, Imaging + "\" + filenames.ToString, Extension, oldfilename) = True Then
                                            Kill(Imaging + "\" + filenames)
                                            If IO.File.Exists(Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff")) Then
                                                Kill(Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff"))
                                            End If
                                            Return 1
                                        Else
                                            Kill(Volume & "\" & pdfilename)
                                            Return 0
                                        End If
                                    Else
                                        Kill(Imaging + "\" + filenames)
                                        If IO.File.Exists(Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff")) Then
                                            Kill(Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff"))
                                        End If
                                        Return 1
                                    End If
                                    System.Windows.Forms.Application.DoEvents()
                                ElseIf res = 2 Then
                                    writetxtfle("Version Creation Skipped ", Imaging + "\" + filenames)
                                    Return 0
                                Else
                                    writetxtfle("Error From ConvertToOther Files ", filenames)
                                    writetxtfle(Environment.NewLine, "")
                                    writetxtfle(errstr, filenames)
                                    Return 0
                                End If
                            Else
                                Return 0
                            End If
                        Else
                            writetxtfle("Check Mandatory Fields : ", Imaging + "\" + filenames)
                        End If
                    Else
                        writetxtfle("ERS Path Not Found ", Imaging + "\" + filenames)
                        Return 0
                    End If
                Else
                    '   MessageBox.Show("Failed to connect to " & Appcon("UNCpath") & vbCrLf & "LastError = " + unc.LastError.ToString)
                    If GetERSPath() = True Then
                        Getdbstruct(filenames)
                        If CheckIndexing(Imaging + "\" + filenames) Then
                            Dim result As Boolean = filldb(Imaging + "\" + filenames)
                            Dim Cf As New pdfconvertor
                            Dim oldfilename = pdfilename.Trim
                            Dim Extension As String = System.IO.Path.GetExtension(Imaging & "\" & filenames).Replace(".", "").ToUpper
                            If result = True Then
                                Dim res = Cf.ConvertToOther(Extension.Trim, Volume.Trim, pdfilename.Trim, Imaging + "\" + filenames, Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff"))
                                If res = 1 Or res = 3 Then
                                    'ExportExtention = 0
                                    If res = 1 Then
                                        '  Extension = ".ezo"
                                        If ExportPdf(pdfilename, Volume, Imaging + "\" + filenames.ToString, Extension, oldfilename) = True Then
                                            Kill(Imaging + "\" + filenames)
                                            If IO.File.Exists(Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff")) Then
                                                Kill(Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff"))
                                            End If
                                            Return 1
                                        Else
                                            Kill(Volume & "\" & pdfilename)
                                            Return 0
                                        End If
                                    Else
                                        Kill(Imaging + "\" + filenames)
                                        If IO.File.Exists(Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff")) Then
                                            Kill(Imaging + "\Temp\" + filenames.ToString.Replace(System.IO.Path.GetExtension(filenames.ToString), ".tiff"))
                                        End If
                                        Return 1
                                    End If
                                    System.Windows.Forms.Application.DoEvents()
                                ElseIf res = 2 Then
                                    writetxtfle("Version Creation Skipped ", Imaging + "\" + filenames)
                                    Return 0
                                Else
                                    writetxtfle("Error From ConvertToOther Files ", filenames)
                                    writetxtfle(Environment.NewLine, "")
                                    writetxtfle(errstr, filenames)
                                    Return 0
                                End If
                            Else
                                Return 0
                            End If
                        Else
                            writetxtfle("Check Mandatory Fields : ", Imaging + "\" + filenames)
                        End If
                    Else
                        writetxtfle("ERS Path Not Found ", Imaging + "\" + filenames)
                        Return 0
                    End If
                End If
            End Using
        Catch ex As Exception
            If errstr <> "" Then
                writetxtfle("Error From ExportOther Files : " + errstr, filenames)
                Return 0
            Else
                writetxtfle("Error From ExportOther Files : " + ex.ToString, filenames)
                Return 0
            End If
        Finally
        End Try
    End Function
    Public Function GetERSPath() As Boolean
        Try
            Dim CAC As New CACserviceClient
            Dim obj As New List(Of eZERSInfo)
            Dim cabinfo = CAC.SelectedeZCabinetList("cabinetid", cabinetid.ToString)
            obj = CAC.SelectedeZERSInfoList("ErsId", cabinfo(0).ERSId.ToString)
            If obj.Count <> 0 Then
                ERSPath = obj(0).ERSDirPath
                ErsId = obj(0).ERSId
                'MessageBox.Show(ERSPath)
                Return True
            Else
                Return False
            End If
            'Dim host As String = ""
            'Dim LocalHostaddress As String = ""
            'Try
            '    Dim strHostName As String = System.Net.Dns.GetHostName()
            '    Dim iphe As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)
            '    For Each ipheal As System.Net.IPAddress In iphe.AddressList
            '        If ipheal.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
            '            LocalHostaddress = ipheal.ToString()
            '        End If
            '    Next
            '    'LocalHostaddress = "192.168.001.055"
            'Catch ex As Exception
            '    MessageBox.Show("Ip Error")
            '    Return False
            'End Try
            ''LocalHostaddress = "192.168.001.001"
            'If LocalHostaddress <> "" Then
            '    Dim obj As New List(Of eZERSInfo)
            '    obj = CAC.SelectedeZERSInfoListByIP(LocalHostaddress)
            '    If obj.Count <> 0 Then
            '        ERSPath = obj(0).ERSDirPath
            '        ErsId = obj(0).ERSId
            '        'MessageBox.Show(ERSPath)
            '        Return True
            '    Else
            '        Return False
            '    End If
            'Else
            '    MessageBox.Show("Ip Error")
            '    Return False
            'End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function filldb(ByVal filename As String) As Boolean
        Try
            Dim Check1 As Integer = 0
            Dim Check2 As Integer = 0
            Dim CAC As New CACserviceClient
            Dim sdataset As DataSet
            sdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", filename)
            If Not IsNothing(sdataset) AndAlso sdataset.Tables.Count > 0 AndAlso sdataset.Tables(0).Rows.Count > 0 Then
                If ezpdf.Title.Trim() <> "" Then
                    sTitle = RmvSplChar(sdataset.Tables(0).Rows(0).Item(ezpdf.Title).ToString())
                Else
                    sTitle = ""
                End If
                If ezpdf.Subject.Trim() <> "" Then
                    sSubject = RmvSplChar(sdataset.Tables(0).Rows(0).Item(ezpdf.Subject).ToString())
                Else
                    sSubject = ""
                End If
                If ezpdf.Author.Trim() <> "" Then
                    sAuthor = RmvSplChar(sdataset.Tables(0).Rows(0).Item(ezpdf.Author).ToString())
                Else
                    sAuthor = ""
                End If
                If ezpdf.Keyword.Trim() <> "" Then
                    sRemarks = RmvSplChar(sdataset.Tables(0).Rows(0).Item(ezpdf.Keyword).ToString())
                Else
                    sRemarks = ""
                End If
                sPdfSignature = "ezofis"
                Volume = ERSPath + "\" + cabinetName + "\" + TemplateName


                If ezTempField.Count >= 2 Then
                    For i As Integer = 0 To ezTempField.Count - 2
                        If Not IsDBNull(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim().ToString())) Then
                            If Trim(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim().ToString())) <> "" Then
                                If ezTempField(i).DataTypeId = 5 Then
                                    Dim stdt As String = CAC.DateStringToString(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim()), 0, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern)
                                    Volume = Volume & "\" & stdt
                                    Check1 += 1
                                Else
                                    Volume = Volume & "\" & RmvSplChar(sdataset.Tables(0).Rows(0).Item(ezTempField(i).FieldName.Trim()).ToString)
                                    Check1 += 1
                                End If
                            End If
                        End If
                    Next
                End If
                pdfilename = RmvSplChar(sdataset.Tables(0).Rows(0).Item(ezTempField(ezTempField.Count - 1).FieldName.Trim()))



            Else
                sSubject = Nothing
                sTitle = Nothing
                sRemarks = Nothing
                sAuthor = Nothing
                sPdfSignature = Nothing
                Return False
            End If
            'MessageBox.Show("Check1 :" + Check1.ToString())
            'MessageBox.Show("Check2 :" + Check2.ToString())
            'If Check1 = Check2 Then
            Return True
            'Else
            'Return False
            'End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Function
    Public Function RmvSplChar(ByVal value As String) As String
        Dim res = ""
        Try
            res = value.Trim.Replace("/", "-").Replace(":", "-").Replace("\", "-").Replace("*", "-").
                Replace("<", "-").Replace(">", "-").Replace("?", "-").Replace("|", "-").Replace("""", "-")
        Catch ex As Exception
            res = value
        End Try
        Return res
    End Function
    Public Function ExportPdf(ByVal newfilename As String, ByVal path As String, ByVal SelectedItem As String, ByVal ExtentFile As String, ByVal oldfilename As String) As Boolean
        Dim quer As String = ""
        Dim fldinsert As New System.Text.StringBuilder("")
        Dim sqlstring As New System.Text.StringBuilder("")
        Dim idataset As New DataSet
        Dim sdataset As DataSet
        sdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", SelectedItem)
        If sdataset IsNot Nothing Then
            If sdataset.Tables.Count > 0 Then
                If sdataset.Tables(0).Rows.Count > 0 Then
                    path = path.Replace(ERSPath + "\", "") + "\"
                    Dim tblname = "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_items"
                    sqlstring.Append("Insert into " & tblname & " (")
                    Dim currdt As String = CAC.DateDateTimeToString(DateTime.Now, 1)
                    For i = 0 To fieldlst.Count - 1
                        If fieldlst(i).DataTypeId = 5 Then
                            sqlstring.Append("[" + fieldlst(i).FieldName.Trim.ToString() + "]" + ",")
                            Dim stdt As String = "Null"
                            Try
                                Dim daTime = CDate(sdataset.Tables(0).Rows(0).Item(fieldlst(i).FieldName.Trim()))
                                stdt = CAC.DateStringToString(daTime.Date, 0, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern)
                            Catch ex As Exception
                            End Try
                            If stdt = "Null" Then
                                fldinsert.Append("" & stdt & ",")
                            Else
                                fldinsert.Append("N'" & stdt & "',")
                            End If

                        Else
                            sqlstring.Append("[" + fieldlst(i).FieldName.Trim.ToString() + "]" + ",")
                            If IsDBNull(sdataset.Tables(0).Rows(0).Item(fieldlst(i).FieldName.Trim())) Then
                                fldinsert.Append("null,")
                            Else
                                fldinsert.Append("N'" & sdataset.Tables(0).Rows(0).Item(fieldlst(i).FieldName.Trim()).ToString().Replace("'", "''") & "',")
                            End If
                        End If
                    Next
                    sqlstring.Append("templateid,ifilepath,ifilename ,ifiletype,version ,createdby,updatedby,dtitle,dauthor,dsubject,dkeywords,checkout,checkoutpath,checkoutby,dstatus,dsize,nopages,CreatedOn,UpdatedOn,Isdeleted,ersid,ezfrom) values(")
                    sqlstring.Append(fldinsert)
                    sqlstring.Append("'" + templateid.ToString() + "', N'" + path.Replace("/", "-").Trim(" ").Replace(":", ":").Replace("'", "''") + "', N'" + newfilename.Replace("'", "''") + "." + ExtentFile + "', '" + ExtentFile + "', '" + vers + "' ,'" + CreateOnId + "', '" + CreateOnId + "',N'" + sTitle.Replace("'", "''") + "', N'" + sAuthor.Replace("'", "''") + "', N'" + sSubject.Replace("'", "''") + "', N'" + sRemarks.Replace("/", "-").Trim(" ").Replace(":", ":").Replace("'", "''") + "', '', '', '0','Active','" + docsize.ToString() + "', '" + nopages.ToString() + "', '" + currdt + "', '" + currdt + "','0'," & ErsId & ",'ECM-Capture(" + Environment.MachineName + ")')")
                    idataset = CAC.SelectedeZUserDefinedList(3, templateid, "*", "ifilename=N'" + oldfilename + "." + ExtentFile + "' and ifilepath", path)


                    Dim versioncount = idataset.Tables(0).Rows.Count
                    If versioncount = 0 Then
                        Try
                            quer = sqlstring.ToString()
                            '    MessageBox.Show(quer)
                            Dim itemid As Integer = CAC.InsertAndUpdateAndDeleteeZUserDefinedWithScope(quer)
                            If itemid <> 0 Then
                                quer = "Insert into eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_history select * from " + tblname + " where itemid='" + itemid.ToString() + "'"
                                If CAC.InsertAndUpdateAndDeleteeZUserDefined(quer) = 1 Then
                                    quer = path.Replace("/", "-").Trim(" ").Replace(":", ":")
                                    CAC.InsertFolders(quer, 0)
                                    quer = "delete from " & "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_stage" & " where ifilename='" & SelectedItem.ToString().Replace("'", "''") & "'"
                                    If CAC.InsertAndUpdateAndDeleteeZUserDefined(quer) > 0 Then
                                        writetxtfle("File Exported Successfully  ", SelectedItem)
                                        ifilenamelist += "'" + SelectedItem.Replace("'", "''") + "',"
                                        Return True
                                    Else
                                        writetxtfle("Due to some Error while Delete Records : ", SelectedItem)
                                        writetxtfle(Environment.NewLine, "")
                                        writetxtfle("Query : " + quer, SelectedItem)
                                    End If
                                Else
                                    writetxtfle("Due to some Error while Save Records In History : ", SelectedItem)
                                    writetxtfle(Environment.NewLine, "")
                                    writetxtfle("Query : " + quer, SelectedItem)
                                End If
                            Else
                                writetxtfle("Due to some Error while Save Records In Item  ", SelectedItem)
                                writetxtfle(Environment.NewLine, "")
                                writetxtfle("Query : " + quer, SelectedItem)
                            End If
                        Catch ex As SqlClient.SqlException
                            writetxtfle("Error From Sql  ", SelectedItem)
                            writetxtfle(Environment.NewLine, "")
                            writetxtfle("Error From Query : " + quer.ToString, SelectedItem)
                            Return False
                        End Try
                    Else
                        quer = "Update " + tblname + " set ifilename= N'" + newfilename.Replace("'", "''") + "." + ExtentFile + "',version='" + vers + "',updatedon=N'" + currdt + "',updatedby='" + CreateOnId + "',dsize='" + docsize.ToString + "',nopages='" + nopages.ToString + "'"
                        quer += " where itemid=" & idataset.Tables(0).Rows(0).Item(0).ToString
                        Try
                            Dim itemid As Integer = CAC.InsertAndUpdateAndDeleteeZUserDefined(quer)
                            If itemid <> 0 Then
                                quer = "Insert into eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_history select * from " + tblname + " where itemid='" + idataset.Tables(0).Rows(0).Item(0).ToString + "'"
                                If CAC.InsertAndUpdateAndDeleteeZUserDefined(quer) = 1 Then
                                    quer = path.Replace("/", "-").Trim(" ").Replace(":", ":")
                                    CAC.InsertFolders(quer, 0)
                                    quer = "delete from " & "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_stage" & " where ifilename='" & SelectedItem.ToString() & "'"
                                    If CAC.InsertAndUpdateAndDeleteeZUserDefined(quer) = 1 Then
                                        writetxtfle("File Exported Successfully  ", SelectedItem)
                                        ifilenamelist += "'" + SelectedItem.Replace("'", "''") + "',"
                                        Return True
                                    Else
                                        writetxtfle("Due to some Error while Delete Records : ", SelectedItem)
                                        writetxtfle(Environment.NewLine, "")
                                        writetxtfle("Query : " + quer, SelectedItem)
                                    End If
                                Else
                                    writetxtfle("Due to some Error while Save Records In History : ", SelectedItem)
                                    writetxtfle(Environment.NewLine, "")
                                    writetxtfle("Query : " + quer, SelectedItem)
                                End If
                            Else
                                writetxtfle("Due to some Error while Save Records In Item  ", SelectedItem)
                                writetxtfle(Environment.NewLine, "")
                                writetxtfle("Query : " + quer, SelectedItem)
                            End If
                        Catch ex As SqlClient.SqlException
                            writetxtfle("Error From Sql  ", SelectedItem)
                            writetxtfle(Environment.NewLine, "")
                            writetxtfle("Error From Query : " + quer.ToString, SelectedItem)
                            Return False
                        End Try
                    End If
                Else
                    writetxtfle("File Record Not Found in Database  ", SelectedItem)
                End If
            Else
                writetxtfle("File Record Not Found in Database  ", SelectedItem)
            End If
        End If
        'Catch ex As Exception
        'writetxtfle("Error From ExportPdf  " + ex.ToString, SelectedItem)
        'Finally
        'fldinsert = Nothing
        'sqlstring = Nothing
        'End Try
    End Function
    'Public Sub SyncFromXL(ByVal lftpanlst As List(Of String))
    '    Try
    '        Dim CAC As New CACserviceClient
    '        Dim sdataset As DataSet
    '        Dim tblname = "eZCA_" + cabinetid.ToString() + "_" + templateid.ToString() + "_stage"
    '        Dim sqlstring As New System.Text.StringBuilder
    '        Dim fldinsert As New System.Text.StringBuilder("")
    '        Dim SqlStringInsert As New System.Text.StringBuilder("")
    '        Dim FldString As New System.Text.StringBuilder("")
    '        'fldinsert.Append("Update " + tblname + " set ")
    '        Dim openFileDialog1 As New OpenFileDialog()
    '        openFileDialog1.Title = "Select a XL File"
    '        Dim strm As Boolean
    '        strm = openFileDialog1.ShowDialog()
    '        If strm = True Then
    '            Dim sfname As String = ""
    '            Dim files As String
    '            files = openFileDialog1.FileName
    '            Dim xlApp As Microsoft.Office.Interop.Excel.Application
    '            Dim xlWorkBook As Microsoft.Office.Interop.Excel.Workbook
    '            Dim xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet
    '            xlApp = New Microsoft.Office.Interop.Excel.Application
    '            xlWorkBook = xlApp.Workbooks.Open(files)
    '            xlWorkSheet = xlWorkBook.Worksheets("sheet1")
    '            Dim range As Microsoft.Office.Interop.Excel.Range
    '            Dim flename As String
    '            range = xlWorkSheet.UsedRange
    '            Dim rCnt As Integer
    '            If lftpanlst.Count > 0 Then
    '                If lftpanlst.Count = range.Rows.Count - 1 Then
    '                    For rCnt = 2 To range.Rows.Count
    '                        flename = "c:\imaging\" + lftpanlst.Item(rCnt - 2).ToString
    '                        sdataset = CAC.SelectedeZUserDefinedList(1, templateid, "*", "ifilename", flename)
    '                        If sdataset.Tables(0).Rows.Count > 0 Then
    '                            fldinsert.Append("Update " + tblname + " set ")
    '                            For CCnt = 1 To range.Columns.Count
    '                                If fieldlst.Count >= CCnt Then
    '                                    Dim name As String = xlWorkSheet.Cells(1, CCnt).value
    '                                    For i As Integer = 0 To fieldlst.Count - 1
    '                                        If LCase(fieldlst(i).FieldName.Trim()) = LCase(name) Then
    '                                            fldinsert.Append("[" + fieldlst(i).FieldName.Trim() & "]=" & "'" & xlWorkSheet.Cells(rCnt, CCnt).value & "',")
    '                                        End If
    '                                    Next
    '                                End If
    '                            Next
    '                            sqlstring = (fldinsert.Remove(Len(fldinsert.ToString) - 1, 1))
    '                            sqlstring.Append(" where itemid=" & sdataset.Tables(0).Rows(0).Item(0).ToString)
    '                            If CAC.InsertAndUpdateAndDeleteeZUserDefined(sqlstring.ToString()) = 1 Then
    '                            Else
    '                                MsgBox("Due to some Error while Save Records")
    '                            End If
    '                            sqlstring.Clear()
    '                            fldinsert.Clear()
    '                        Else
    '                            'Dim fldinsert As New System.Text.StringBuilder("")
    '                            SqlStringInsert.Append("Insert into " + tblname + " (")
    '                            For CCnt = 1 To range.Columns.Count
    '                                If fieldlst.Count >= CCnt Then
    '                                    Dim name As String = xlWorkSheet.Cells(1, CCnt).value
    '                                    For i As Integer = 0 To fieldlst.Count - 1
    '                                        If LCase(fieldlst(i).FieldName.Trim()) = LCase(name) Then
    '                                            SqlStringInsert.Append("[" + fieldlst(i).FieldName.Trim() & "],")
    '                                            'fldinsert.Append("[" + fieldlst(i).FieldName.Trim() & "]=" & "'" & xlWorkSheet.Cells(rCnt, CCnt).value & "',")
    '                                            FldString.Append("'" & xlWorkSheet.Cells(rCnt, CCnt).value & "',")
    '                                        End If
    '                                    Next
    '                                End If
    '                            Next
    '                            SqlStringInsert.Append("templateid,ifilepath,ifilename ,ifiletype,version ,createdby,updatedby,dtitle,dauthor,dsubject,dkeywords,checkout,checkoutpath,checkoutby,dstatus,dsize,nopages,CreatedOn,UpdatedOn,Isdeleted) values(")
    '                            SqlStringInsert.Append(FldString)
    '                            SqlStringInsert.Append("'" + templateid.ToString() + "', '" + Imaging + "', '" + flename + "', 'tif', '1' ,'" + CreateOnId + "', '" + CreateOnId + "', '', '', '', '', '', '', '1', '', '', '', '" + CAC.DateDateTimeToString(DateTime.Now, 1) + "', '" + CAC.DateDateTimeToString(DateTime.Now, 1) + "', '0')")
    '                            If CAC.InsertAndUpdateAndDeleteeZUserDefined(SqlStringInsert.ToString()) = 1 Then
    '                            Else
    '                                MsgBox("Due to some Error while Save Records")
    '                            End If
    '                            SqlStringInsert.Clear()
    '                            FldString.Clear()
    '                        End If
    '                    Next
    '                    xlWorkBook.Close()
    '                    xlApp.Quit()
    '                    releaseObject(xlApp)
    '                    releaseObject(xlWorkBook)
    '                    releaseObject(xlWorkSheet)
    '                Else
    '                    MessageBox.Show("Records And files Mismatch")
    '                End If
    '            Else
    '                MsgBox("List is empty")
    '            End If
    '        Else
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message.ToString)
    '    Finally
    '    End Try
    'End Sub
    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
    Public Sub Btn_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim LookupId As Integer = 0
            Dim count = VisualTreeHelper.GetChildrenCount(IndexingFieldPanel)
            Dim objeZLookup As New List(Of eZLookup)
            objeZLookup = CAC.SelectedeZLookupList("TemplateId", templateid)
            If objeZLookup.Count <> 0 Then
                LookupId = objeZLookup(0).LookupId
                If objeZLookup(0).LookupTypeId = 1 Then
                    Dim lookupstr = objeZLookup(0).LookupValue.ToUpper()
                    Dim qry = "select * from ezlookupclientfield WITH (NOLOCK) where lookupid='" + LookupId.ToString + "'"
                    Dim lookupclientfield = CAC.GetDatasetByQuery(qry)
                    Dim lookupfieldslist = CAC.SelectedeZLookupFieldsListWithLookupId("IsSyncField", "1", LookupId.ToString)
                    For Each lookupfield As eZLookupFields In lookupfieldslist
                        If Not lookupclientfield Is Nothing Then
                            If lookupclientfield.Tables(0).Rows.Count > 0 Then
                                Dim rowclientfield = lookupclientfield.Tables(0).Select("ClientField='param" + lookupfield.ParameterOrder.ToString + "'")
                                If rowclientfield.Count > 0 Then
                                    Dim IndexingValue As String = GetIndexingValue(lookupfield.ECMField.Trim().Replace(".", "")).Trim()
                                    lookupstr = lookupstr.Replace(rowclientfield(0)("ClientFieldValues").ToString, IndexingValue)
                                End If
                            End If
                        End If
                    Next
                    Dim sdataset As DataSet
                    sdataset = CAC.SyncFromClient(lookupstr, LookupId, "ECM-Capture", CreateOnId)
                    If sdataset.Tables(0).Rows.Count > 0 Then
                        For i As Int16 = 0 To fieldlst.Count - 1
                            Dim Fieldname As List(Of eZLookupFields) = GetSyncField(LookupId, fieldlst(i).FieldName.Trim())
                            If Not Fieldname Is Nothing Then
                                If Fieldname(0).IsSyncField = False Then
                                    Try
                                        SetIndexingValue(fieldlst(i).FieldName.Trim().Replace(".", ""), sdataset.Tables(0).Rows(sdataset.Tables(0).Rows.Count - 1).Item(Fieldname(0).ClientField).ToString)
                                    Catch ex As Exception
                                    End Try
                                End If
                            End If
                        Next
                        SaveRecords(CurrentFnInRightPane)
                    Else
                        For i As Int16 = 0 To fieldlst.Count - 1
                            Dim Fieldname As List(Of eZLookupFields) = GetSyncField(LookupId, fieldlst(i).FieldName.Trim())
                            If Not Fieldname Is Nothing Then
                                If Fieldname(0).IsSyncField = False Then
                                    SetIndexingControlValues(fieldlst(i).FieldName.Trim(), "")
                                End If
                            End If
                        Next
                    End If
                Else
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function GetSyncField(ByVal LookupId As Integer, ByVal FieldName As String) As List(Of eZLookupFields)
        Try
            If LookupId <> 0 Then
                Dim objeZLookupFields As New List(Of eZLookupFields)
                objeZLookupFields = CAC.SelectedeZLookupFieldsListWithLookupId("ECMField", FieldName, LookupId.ToString())
                If objeZLookupFields.Count <> 0 Then
                    'If objeZLookupFields(0).IsSyncField Then
                    Return objeZLookupFields
                    'Else
                    '    Return ""
                End If
                '    Else
                '        Return ""
                '    End If
                'Else
                '    Return ""
            End If
        Catch ex As Exception
            'Return ""
        End Try
    End Function


    Public Sub LoadZonesToFile(ByVal ZoneName As String)
        Dim _Zone As New OcrZone
        If Not (String.IsNullOrEmpty(_recognitionResults)) Then
            Index = Index + 1
            _Zone.Id = Index
            _Zone.Bounds = _document.Pages(0).Zones(0).Bounds
            _Zone.Name = ZoneName
            _Zone.ZoneType = OcrZoneType.Text
            _Zone.FillMethod = OcrZoneFillMethod.Default
            _Zone.RecognitionModule = OcrZoneRecognitionModule.Auto
            _Zone.CharacterFilters = OcrZoneCharacterFilters.None
            If _OcrZonePage.Pages.Count = 0 Then
                _OcrZonePage.Pages.Clear()
                _OcrZonePage.Pages.AddPage(ECMImage, Nothing)
            End If
            _OcrZonePage.Pages(0).Zones.Add(_Zone)
        End If
    End Sub
    Public Sub SetCulture(ByVal Lang As String)
        Dim culture = CultureInfo.CreateSpecificCulture(Lang)
        Dim rm As New ResourceManager("ezofis.UserControl.Main", GetType(ECMRightPane).Assembly)
        Me.LblCabinet.Content = rm.GetString("Cabinet", culture)
        Me.LblTemplate.Content = rm.GetString("Template", culture)
        Me.LblFields.Content = rm.GetString("Fields", culture)
    End Sub
    Public Function CheckIndexingField() As Boolean
        Dim res As Boolean = False, qry = "select top 1 itemid from ezca_" + cabinetid.ToString + "_" + templateid.ToString + "_items WITH (NOLOCK) where ", ds As New DataSet
        Try
            Dim fildlst = CAC.SelectedeZTemplateFieldList("TemplateId", templateid.ToString())
            For i As Int16 = 0 To fieldlst.FindAll(Function(obj) obj.Mandatory = True).Count - 1
                Dim value = GetIndexingValue(fieldlst(i).FieldName.Trim())
                If value = "" And fieldlst(i).Mandatory Then
                    Return False
                Else
                    qry += " [" + fieldlst(i).FieldName.Trim + "]=N'" + value.Replace("'", "''") + "' and"
                End If
            Next
            qry = qry.Substring(0, qry.Length - 3) + " and isdeleted=0"
            ds = CAC.GetDatasetByQuery(qry)
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        Return True
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("Error From Check Indexing : " + ex.ToString)
        End Try
        Return res
    End Function

    Public Function SplitWithBarcode(ByVal barcodeTiflst As List(Of String), _fileFormat As String, _bitsPerPixel As String) As String
        Dim Result As String = ""
        Try
            Dim obj As New List(Of eZTempBarcode)
            obj = CAC.SelectedeZTempBarcodeList("TemplateId", templateid)
            If obj.Count <> 0 Then
                For i As Integer = 0 To barcodeTiflst.Count - 1
                    BarcodeStartsWith = obj(0).StartsWith.ToString
                    BarcodeEndsWith = obj(0).EndWith.ToString
                    BarcodeType = obj(0).BarcodeType.ToString
                    barcodecount = 0
                    Dim barcode As New barcoderead
                    Dim dt As New DataTable
                    dt = barcode.barcodee(barcodeTiflst(i).ToString, Imaging, _fileFormat, _bitsPerPixel)
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        CurrentFnInRightPane = barcodeTiflst(i).ToString
                        SetIndexingControlValues(obj(0).BarcodeField, dt.Rows(0).Item(0))
                        Btn_Click(Nothing, New System.Windows.RoutedEventArgs)
                        SaveRecords(barcodeTiflst(i).ToString)
                        Result = "Success"
                    End If
                Next
            End If
        Catch ex As Exception
            Result = ex.Message
        End Try
        Return Result
    End Function
End Class
