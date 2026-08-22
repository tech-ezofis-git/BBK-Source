Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZInboxTransaction
    Inherits IDatabaseCommonItems
    Implements IeZInboxTransaction
    Protected _InboxId As Integer
    Protected _ItemId As Integer
    Protected _TemplateId As Integer
    Protected _ProcessId As Integer
    Protected _FromUserId As Integer
    Protected _ToUserId As Integer
    Protected _FromUser As String = ""
    Protected _ToUser As String = ""
    Protected _URL As String = ""
    Protected _Status As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer


    Public Sub New(tmpInboxId As Integer)
        Me._InboxId = tmpInboxId
    End Sub

    Public Sub New()
    End Sub
    Public Property inboxId() As Integer Implements IeZInboxTransaction.InboxId
        Get
            If _InboxId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _InboxId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _InboxId <> 0 AndAlso _InboxId <> value Then
                Throw New MemberAccessException()
            End If
            _InboxId = value
        End Set
    End Property
    Public Property ProcessId() As Integer Implements IeZInboxTransaction.ProcessId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ProcessId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ProcessId = value Then
                Return
            End If

            _ProcessId = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZInboxTransaction.TemplateId
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

    Public Property Itemid() As Integer Implements IeZInboxTransaction.ItemId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ItemId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ItemId = value Then
                Return
            End If
            _ItemId = value
            IsModified = True
        End Set
    End Property

    Public Property FromUserId() As Integer Implements IeZInboxTransaction.FromUserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FromUserId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FromUserId = value Then
                Return
            End If

            _FromUserId = value
            IsModified = True
        End Set
    End Property

    Public Property ToUserId() As Integer Implements IeZInboxTransaction.ToUserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToUserId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ToUserId = value Then
                Return
            End If

            _ToUserId = value
            IsModified = True
        End Set
    End Property
    Public Property FromUser() As String Implements IeZInboxTransaction.FromUser
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FromUser
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FromUser = value Then
                Return
            End If
            _FromUser = value
            IsModified = True
        End Set
    End Property
    Public Property ToUser() As String Implements IeZInboxTransaction.ToUser
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToUser
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ToUser = value Then
                Return
            End If
            _ToUser = value
            IsModified = True
        End Set
    End Property

    Public Property URL() As String Implements IeZInboxTransaction.URL
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _URL
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _URL = value Then
                Return
            End If
            _URL = value
            IsModified = True
        End Set
    End Property

    Public Property Status() As String Implements IeZInboxTransaction.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZInboxTransaction.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZInboxTransaction.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZInboxTransaction.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZInboxTransaction.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZInboxTransaction.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZInboxTransaction.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZInboxTransaction.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsInboxIdExist() As Boolean Implements IeZInboxTransaction.IsInboxIdExist
        Get
            Return (inboxId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
