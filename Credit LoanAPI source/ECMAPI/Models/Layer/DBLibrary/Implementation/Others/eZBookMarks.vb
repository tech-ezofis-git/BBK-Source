Public Class eZBookMarks
    Inherits IDatabaseCommonItems
    Implements IeZBookMarks
    Protected _BookMarksId As Integer
    Protected _BookMarksName As String
    Protected _SearchKeyWord As String
    Protected _TemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Protected _IsContenSearch As Boolean
    Protected _IsSavedSearch As Boolean
    'udaya
    Protected _folderid As String
    'Protected _FolderName As String
   
    Private _Isdeleted As Integer
    Private _isfolderdelete As Integer
    Friend Sub New(BookMarksId As Integer)
        Me._BookMarksId = BookMarksId
    End Sub
    Friend Sub New(tmpBookMarksName As String)
        Me._BookMarksName = tmpBookMarksName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property BookMarksId() As Integer Implements IeZBookMarks.BookMarksId
        Get
            If _BookMarksId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _BookMarksId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _BookMarksId <> 0 AndAlso _BookMarksId <> value Then
                Throw New MemberAccessException()
            End If
            _BookMarksId = value
        End Set
    End Property
    Public Property SearchKeyWord() As String Implements IeZBookMarks.SearchKeyWord
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SearchKeyWord
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SearchKeyWord = value Then
                Return
            End If
            _SearchKeyWord = value
            IsModified = True
        End Set
    End Property
    Public Property BookMarksName() As String Implements IeZBookMarks.BookMarksName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _BookMarksName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _BookMarksName = value Then
                Return
            End If
            _BookMarksName = value
            IsModified = True
        End Set
    End Property
  
    'udaya
    Public Property folderid() As String Implements IeZBookMarks.folderid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _folderid
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _folderid = value Then
                Return
            End If
            _folderid = value
            IsModified = True
        End Set
    End Property
    'Public Property foldername() As String Implements IeZBookMarks.foldername
    '    Get
    '        DBLayer.DBLInstance.Read(Me)
    '        Return _FolderName
    '    End Get
    '    Set(value As String)
    '        DBLayer.DBLInstance.Read(Me)
    '        If _FolderName = value Then
    '            Return
    '        End If
    '        _FolderName = value
    '        IsModified = True
    '    End Set
    'End Property
    Public Property TemplateId() As Integer Implements IeZBookMarks.TemplateId
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
    Public Property IsSavedSearch() As Boolean Implements IeZBookMarks.IsSavedSearch
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsSavedSearch
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsSavedSearch = value Then
                Return
            End If
            _IsSavedSearch = value
            IsModified = True
        End Set
    End Property
    Public Property IsContenSearch() As Boolean Implements IeZBookMarks.IsContenSearch
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsContenSearch
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsContenSearch = value Then
                Return
            End If
            _IsContenSearch = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZBookMarks.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZBookMarks.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZBookMarks.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZBookMarks.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZBookMarks.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZBookMarks.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZBookMarks.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property isfolderdelete() As Integer Implements IeZBookMarks.isfolderdelete
        Get
            Return _isfolderdelete
        End Get
    End Property
    Public ReadOnly Property IseZBookMarksExist() As Boolean Implements IeZBookMarks.IseZBookMarksExist
        Get
            Return (_BookMarksId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
