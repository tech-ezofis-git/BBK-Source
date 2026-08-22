
Public Class eZVault
    Inherits IDatabaseCommonItems
    Implements IeZVault

    Protected _eZVaultId As Integer
    Protected _TemplateId As Integer
    Protected _Condition As String = ""
    Protected _NodeId As Integer
    Protected _Status As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub

    Public Sub New(ezvaultid As Integer)
        Me._eZVaultId = ezvaultid
    End Sub
    Public Property Condition() As String Implements IeZVault.Condition
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Condition
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Condition = value Then
                Return
            End If
            _Condition = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy() As Integer Implements IeZVault.CreatedBy
        Get
            If _CreatedBy = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CreatedBy
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CreatedBy <> 0 AndAlso _CreatedBy <> value Then
                Throw New MemberAccessException()
            End If
            _CreatedBy = value
        End Set
    End Property

    Public Property CreatedBy1() As String Implements IeZVault.CreatedBy1
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

    Public Property CreatedOn() As String Implements IeZVault.CreatedOn
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

    Public Property Status() As Integer Implements IeZVault.Status
        Get
            If _Status = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Status
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Status <> 0 AndAlso _Status <> value Then
                Throw New MemberAccessException()
            End If
            _Status = value
        End Set
    End Property

    Public Property eZVaultId() As Integer Implements IeZVault.eZVaultId
        Get
            If _eZVaultId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _eZVaultId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _eZVaultId <> 0 AndAlso _eZVaultId <> value Then
                Throw New MemberAccessException()
            End If
            _eZVaultId = value
        End Set
    End Property

    Public ReadOnly Property Isdeleted() As Integer Implements IeZVault.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property TemplateId() As Integer Implements IeZVault.TemplateId
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

    Public Property UpdatedBy() As Integer Implements IeZVault.UpdatedBy
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

    Public Property UpdatedBy1() As String Implements IeZVault.UpdatedBy1
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

    Public Property UpdatedOn() As String Implements IeZVault.UpdatedOn
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
    Public Property NodeId() As Integer Implements IeZVault.NodeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _NodeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _NodeId = value Then
                Return
            End If
            _NodeId = value
            IsModified = True
        End Set
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
