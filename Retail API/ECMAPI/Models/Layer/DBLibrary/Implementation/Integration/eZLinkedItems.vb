

Imports ECMAPI

Public Class eZLinkedItems
    Inherits IDatabaseCommonItems
    Implements IeZLinkedItems

    Protected _linkedid1 As Integer
    Protected _Templateid1 As Integer
    Protected _Linkedfieldid1 As Integer
    Protected _SourceFieldid1 As Integer
    Protected _CreatedBy1 As Integer
    Protected _CreatedOn1 As String = ""
    Protected _UpdatedBy1 As Integer
    Protected _UpdatedOn1 As String = ""
    Protected _CreatedBy As String = ""
    Protected _UpdatedBy As String = ""
    Private _Isdeleted1 As Integer

    Public Sub New(Linkedid As Integer)
        Me._linkedid1 = Linkedid
    End Sub

    Public Sub New()
    End Sub


    Public Property CreatedBy As Integer Implements IeZLinkedItems.CreatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy1
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy1 = value Then
                Return
            End If
            _CreatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedOn As String Implements IeZLinkedItems.CreatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedOn1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedOn1 = value Then
                Return
            End If
            _CreatedOn1 = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Boolean Implements IeZLinkedItems.Isdeleted
        Get
            Return _Isdeleted1
        End Get
    End Property

    Public Property Linkedfieldid As Integer Implements IeZLinkedItems.Linkedfieldid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Linkedfieldid1
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Linkedfieldid1 = value Then
                Return
            End If
            _Linkedfieldid1 = value
            IsModified = True
        End Set
    End Property

    Public Property SourceFieldid As Integer Implements IeZLinkedItems.SourceFieldid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SourceFieldid1
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _SourceFieldid1 = value Then
                Return
            End If
            _SourceFieldid1 = value
            IsModified = True
        End Set
    End Property

    Public Property templateid As Integer Implements IeZLinkedItems.templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Templateid1
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Templateid1 = value Then
                Return
            End If
            _Templateid1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZLinkedItems.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy1
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy1 = value Then
                Return
            End If
            _UpdatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedOn As String Implements IeZLinkedItems.UpdatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedOn1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedOn1 = value Then
                Return
            End If
            _UpdatedOn1 = value
            IsModified = True
        End Set
    End Property

    Public Property Linkedid As Integer Implements IeZLinkedItems.Linkedid
        Get
            If _linkedid1 = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _linkedid1
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _linkedid1 <> 0 AndAlso _linkedid1 <> value Then
                Throw New MemberAccessException()
            End If
            _linkedid1 = value
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZLinkedItems.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy = value Then
                Return
            End If
            _CreatedBy = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZLinkedItems.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy = value Then
                Return
            End If
            _UpdatedBy = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
