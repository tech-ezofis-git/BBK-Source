Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZOutlookContact
    Inherits IDatabaseCommonItems
    Implements IeZOutlookContact
    Protected _OutlookContactId As Integer
    Protected _Name As String = ""
    Protected _CompanyName As String = ""
    Protected _EntryId As String = ""
    Protected _Email As String = ""
    Protected _MobileNumber As String = ""
    Protected _Description As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._OutlookContactId = DeptId
    End Sub
    
    Public Sub New()
    End Sub

    Public Property OutlookContactId() As Integer Implements IeZOutlookContact.OutlookContactId
        Get
            If _OutlookContactId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _OutlookContactId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _OutlookContactId <> 0 AndAlso _OutlookContactId <> value Then
                Throw New MemberAccessException()
            End If
            _OutlookContactId = value
        End Set
    End Property
    Public Property Name() As String Implements IeZOutlookContact.Name
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Name
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Name = value Then
                Return
            End If
            _Name = value
            IsModified = True
        End Set
    End Property
    Public Property EntryId() As String Implements IeZOutlookContact.EntryId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EntryId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EntryId = value Then
                Return
            End If
            _EntryId = value
            IsModified = True
        End Set
    End Property
    Public Property CompanyName() As String Implements IeZOutlookContact.CompanyName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CompanyName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CompanyName = value Then
                Return
            End If
            _CompanyName = value
            IsModified = True
        End Set
    End Property
    Public Property Email() As String Implements IeZOutlookContact.Email
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Email
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Email = value Then
                Return
            End If
            _Email = value
            IsModified = True
        End Set
    End Property
    Public Property MobileNumber() As String Implements IeZOutlookContact.MobileNumber
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MobileNumber
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MobileNumber = value Then
                Return
            End If
            _MobileNumber = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZOutlookContact.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZOutlookContact.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZOutlookContact.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZOutlookContact.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZOutlookContact.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZOutlookContact.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZOutlookContact.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZOutlookContacttExist() As Boolean Implements IeZOutlookContact.IseZOutlookContactExist
        Get
            Return (_OutlookContactId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
