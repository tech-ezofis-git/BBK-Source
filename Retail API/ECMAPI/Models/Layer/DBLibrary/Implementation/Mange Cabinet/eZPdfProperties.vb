Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZPdfProperties
    Inherits IDatabaseCommonItems
    Implements IeZPdfProperties
    Protected _PdfId As Integer
    Protected _Subject As String = ""
    Protected _TemplateName As String = ""
    Protected _Sync As Integer
    Protected _Author As String = ""
    Protected _Title As String = ""
    Protected _Keyword As String = ""
    Protected _Signature As String = ""
    Protected _TemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._PdfId = DeptId
    End Sub
   
    Public Sub New()
    End Sub

    Public Property Keyword() As String Implements IeZPdfProperties.Keyword
        Get
             DBLayer.DBLInstance.Read(Me)
            Return _Keyword
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Keyword = value Then
                Return
            End If
            _Keyword = value
            IsModified = True
        End Set
    End Property
    Public Property Signature() As String Implements IeZPdfProperties.Signature
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Signature
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Signature = value Then
                Return
            End If
            _Signature = value
            IsModified = True
        End Set
    End Property
    Public Property Sync() As Integer Implements IeZPdfProperties.Sync
        Get
            If _Sync = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Sync
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Sync <> 0 AndAlso _Sync <> value Then
                Throw New MemberAccessException()
            End If
            _Sync = value
        End Set
    End Property
   
    Public Property Author() As String Implements IeZPdfProperties.Author
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Author
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Author = value Then
                Return
            End If
            _Author = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateName() As String Implements IeZPdfProperties.TemplateName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateName = value Then
                Return
            End If
            _TemplateName = value
            IsModified = True
        End Set
    End Property
    Public Property PdfId() As Integer Implements IeZPdfProperties.PdfId
        Get
            If _PdfId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _PdfId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _PdfId <> 0 AndAlso _PdfId <> value Then
                Throw New MemberAccessException()
            End If
            _PdfId = value
        End Set
    End Property
    Public Property Subject() As String Implements IeZPdfProperties.Subject
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Subject
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Subject = value Then
                Return
            End If
            _Subject = value
            IsModified = True
        End Set
    End Property
    Public Property Title() As String Implements IeZPdfProperties.Title
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Title
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Title = value Then
                Return
            End If
            _Title = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZPdfProperties.TemplateID
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateId = value Then
                Return
            End If
            _TemplateId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZPdfProperties.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy1 = value Then
                Return
            End If
            _UpdatedBy1 = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy1() As String Implements IeZPdfProperties.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy1 = value Then
                Return
            End If
            _CreatedBy1 = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy() As Integer Implements IeZPdfProperties.CreatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy = value Then
                Return
            End If

            _CreatedBy = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedOn() As String Implements IeZPdfProperties.CreatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedOn = value Then
                Return
            End If

            _CreatedOn = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy() As Integer Implements IeZPdfProperties.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy = value Then
                Return
            End If

            _UpdatedBy = value
        End Set
    End Property
    Public Property UpdatedOn() As String Implements IeZPdfProperties.UpdatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedOn = value Then
                Return
            End If

            _UpdatedOn = value
        End Set
    End Property
    Public ReadOnly Property Isdeleted() As Integer Implements IeZPdfProperties.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZPdfPropertiesExist() As Boolean Implements IeZPdfProperties.IseZPdfPropertiesExist
        Get
            Return (_PdfId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
