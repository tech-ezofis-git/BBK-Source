Imports ECMAPI

Public Class eZScanBatch
    Inherits IDatabaseCommonItems
    Implements IeZScanBatch

    Protected _BatchId As Integer
    Protected _Batch As String = ""
    Protected _CreatedAt As String = ""
    Protected _Status As Integer
    Protected _NoOfDocument As Integer
    Protected _TemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New()
    End Sub
    Public Sub New(BatchId As Integer)
        Me._BatchId = BatchId
    End Sub
    Public Property Batch As String Implements IeZScanBatch.Batch
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Batch
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Batch = value Then
                Return
            End If
            _Batch = value
            IsModified = True
        End Set
    End Property

    Public Property BatchId As Integer Implements IeZScanBatch.BatchId
        Get
            If _BatchId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _BatchId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _BatchId <> 0 AndAlso _BatchId <> value Then
                Throw New MemberAccessException()
            End If
            _BatchId = value
        End Set
    End Property

    Public Property CreatedAt As String Implements IeZScanBatch.CreatedAt
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedAt
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedAt = value Then
                Return
            End If
            _CreatedAt = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZScanBatch.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZScanBatch.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZScanBatch.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZScanBatch.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property NoOfDocument As Integer Implements IeZScanBatch.NoOfDocument
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _NoOfDocument
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _NoOfDocument = value Then
                Return
            End If
            _NoOfDocument = value
            IsModified = True
        End Set
    End Property

    Public Property Status As Integer Implements IeZScanBatch.Status
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

    Public Property TemplateId As Integer Implements IeZScanBatch.TemplateId
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

    Public Property UpdatedBy As Integer Implements IeZScanBatch.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZScanBatch.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZScanBatch.UpdatedOn
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
