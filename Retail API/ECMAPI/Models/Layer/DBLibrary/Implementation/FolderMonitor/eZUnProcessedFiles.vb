Imports ECMAPI

Public Class eZUnProcessedFiles
    Inherits IDatabaseCommonItems
    Implements IeZUnProcessedFiles

    Protected _UnprocessId As Integer
    Protected _FilePath As String
    Protected _FileName As String
    Protected _FileExtension As String
    Protected _Status As Integer
    Protected _Issue As String
    Protected _ProcessedFrom As String
    Protected _ReprocessPath As String
    Protected _TemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New()

    End Sub
    Public Sub New(UnprocessId As Integer)
        Me._UnprocessId = UnprocessId
    End Sub

    Public Property CreatedBy As Integer Implements IeZUnProcessedFiles.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZUnProcessedFiles.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZUnProcessedFiles.CreatedOn
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

    Public Property FileExtension As String Implements IeZUnProcessedFiles.FileExtension
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FileExtension
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FileExtension = value Then
                Return
            End If
            _FileExtension = value
            IsModified = True
        End Set
    End Property

    Public Property FileName As String Implements IeZUnProcessedFiles.FileName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FileName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FileName = value Then
                Return
            End If
            _FileName = value
            IsModified = True
        End Set
    End Property

    Public Property FilePath As String Implements IeZUnProcessedFiles.FilePath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FilePath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FilePath = value Then
                Return
            End If
            _FilePath = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZUnProcessedFiles.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property Issue As String Implements IeZUnProcessedFiles.Issue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Issue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Issue = value Then
                Return
            End If
            _Issue = value
            IsModified = True
        End Set
    End Property

    Public Property ProcessedFrom As String Implements IeZUnProcessedFiles.ProcessedFrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ProcessedFrom
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ProcessedFrom = value Then
                Return
            End If
            _ProcessedFrom = value
            IsModified = True
        End Set
    End Property

    Public Property ReprocessPath As String Implements IeZUnProcessedFiles.ReprocessPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ReprocessPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ReprocessPath = value Then
                Return
            End If
            _ReprocessPath = value
            IsModified = True
        End Set
    End Property

    Public Property Status As Integer Implements IeZUnProcessedFiles.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId As Integer Implements IeZUnProcessedFiles.TemplateId
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

    Public Property UnprocessId As Integer Implements IeZUnProcessedFiles.UnprocessId
        Get
            If _UnprocessId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _UnprocessId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _UnprocessId <> 0 AndAlso _UnprocessId <> value Then
                Throw New MemberAccessException()
            End If
            _UnprocessId = value
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZUnProcessedFiles.UpdatedBy
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
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZUnProcessedFiles.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZUnProcessedFiles.UpdatedOn
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
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
