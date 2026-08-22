Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZFaxTransaction
    Inherits IDatabaseCommonItems
    Implements IeZFaxTransaction
    Protected _FaxTransactionId As Integer
    Protected _Itemid As Integer
    Protected _ArchivedItemid As Integer
    Protected _ArchivedTemplateId As Integer
    Protected _IsExpired As Boolean
    Protected _FaxReceiverRuleId As Integer
    Protected _FromAdd As Integer
    Protected _ToAdd As Integer
    Protected _DisplayFrom As Integer
    Protected _FromName As String = ""
    Protected _FAXNUMBER As String
    Protected _Subject As String = ""
    Protected _FilePath As String
    Protected _DocType As String = ""
    Protected _IsRead As Boolean
    Protected _IsArchived As Boolean
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy As Integer
    Protected _ArchivedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpFaxTransactionId As Integer)
        Me._FaxTransactionId = tmpFaxTransactionId
    End Sub
  
    Public Sub New()
    End Sub
    Public Property FaxTransactionId() As Integer Implements IeZFaxTransaction.FaxTransactionId
        Get
            If _FaxTransactionId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FaxTransactionId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FaxTransactionId <> 0 AndAlso _FaxTransactionId <> value Then
                Throw New MemberAccessException()
            End If
            _FaxTransactionId = value
        End Set
    End Property
    Public Property ArchivedItemid() As Integer Implements IeZFaxTransaction.ArchivedItemid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ArchivedItemid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ArchivedItemid = value Then
                Return
            End If

            _ArchivedItemid = value
            IsModified = True
        End Set
    End Property
    Public Property ArchivedTemplateId() As Integer Implements IeZFaxTransaction.ArchivedTemplateId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ArchivedTemplateId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ArchivedTemplateId = value Then
                Return
            End If

            _ArchivedTemplateId = value
            IsModified = True
        End Set
    End Property
    Public Property IsExpired() As Boolean Implements IeZFaxTransaction.IsExpired
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsExpired
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsExpired = value Then
                Return
            End If
            _IsExpired = value
            IsModified = True
        End Set
    End Property

    Public Property IsArchived() As Boolean Implements IeZFaxTransaction.IsArchived
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsArchived
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsArchived = value Then
                Return
            End If

            _IsArchived = value
            IsModified = True
        End Set
    End Property
    Public Property Itemid() As Integer Implements IeZFaxTransaction.Itemid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Itemid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Itemid = value Then
                Return
            End If
            _Itemid = value
            IsModified = True
        End Set
    End Property


    Public Property FaxReceiverRuleId() As Integer Implements IeZFaxTransaction.FaxReceiverRuleId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FaxReceiverRuleId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FaxReceiverRuleId = value Then
                Return
            End If

            _FaxReceiverRuleId = value
            IsModified = True
        End Set
    End Property
    Public Property FromAdd() As Integer Implements IeZFaxTransaction.FromAdd
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FromAdd
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FromAdd = value Then
                Return
            End If

            _FromAdd = value
            IsModified = True
        End Set
    End Property

    Public Property ToAdd() As Integer Implements IeZFaxTransaction.ToAdd
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToAdd
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ToAdd = value Then
                Return
            End If

            _ToAdd = value
            IsModified = True
        End Set
    End Property
    Public Property DisplayFrom() As Integer Implements IeZFaxTransaction.DisplayFrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DisplayFrom
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _DisplayFrom = value Then
                Return
            End If

            _DisplayFrom = value
            IsModified = True
        End Set
    End Property
    Public Property FromName() As String Implements IeZFaxTransaction.FromName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FromName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FromName = value Then
                Return
            End If
            _FromName = value
            IsModified = True
        End Set
    End Property
    Public Property DocType() As String Implements IeZFaxTransaction.DocType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DocType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _DocType = value Then
                Return
            End If
            _DocType = value
            IsModified = True
        End Set
    End Property

  
    Public Property IsRead() As Boolean Implements IeZFaxTransaction.IsRead
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsRead
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsRead = value Then
                Return
            End If

            _IsRead = value
            IsModified = True
        End Set
    End Property
    Public Property Subject() As String Implements IeZFaxTransaction.Subject
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
    Public Property FAXNUMBER() As String Implements IeZFaxTransaction.FAXNUMBER
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FAXNUMBER
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FAXNUMBER = value Then
                Return
            End If
            _FAXNUMBER = value
            IsModified = True
        End Set
    End Property
    Public Property FilePath() As String Implements IeZFaxTransaction.FilePath
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

    Public Property UpdatedBy1() As String Implements IeZFaxTransaction.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFaxTransaction.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZFaxTransaction.CreatedBy
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
    Public Property ArchivedBy() As Integer Implements IeZFaxTransaction.ArchivedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ArchivedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ArchivedBy = value Then
                Return
            End If

            _CreatedBy = value
            IsModified = True
        End Set
    End Property


    Public Property CreatedOn() As String Implements IeZFaxTransaction.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZFaxTransaction.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZFaxTransaction.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZFaxTransaction.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsFaxTransactionExist() As Boolean Implements IeZFaxTransaction.IsFaxTransactionExist
        Get
            Return (FaxTransactionId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
