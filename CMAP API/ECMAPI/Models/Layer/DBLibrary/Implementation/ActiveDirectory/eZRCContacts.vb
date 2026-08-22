Imports ECMAPI

Public Class eZRCContacts
    Inherits IDatabaseCommonItems
    Implements IeZRCContacts

    Protected _ezContactId As Integer

    Protected _CompanyName As String = ""
    Protected _ContactName As String = ""
    Protected _LastName As String = ""
    Protected _Title As String = ""

    Protected _Phone As String = ""
    Protected _Mobile As String = ""
    Protected _AltNumber As String = ""
    Protected _Fax As String = ""
    Protected _Email As String = ""
    Protected _WebPage As String = ""

    Protected _Address As String = ""
    Protected _City As String = ""
    Protected _Country As String = ""

    Protected _SecondPhone As String = ""
    Protected _SecondMobile As String = ""
    Protected _SecondAltNumber As String = ""
    Protected _SecondFax As String = ""
    Protected _SecondCity As String = ""

    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Protected _Categories As String = ""
    Protected _POBox As String = ""

    Public Sub New()
    End Sub

    Public Sub New(ezContactId As Integer)
        Me._ezContactId = ezContactId
    End Sub

    Public Property Address As String Implements IeZRCContacts.Address
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Address
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Address = value Then
                Return
            End If
            _Address = value
            IsModified = True
        End Set
    End Property

    Public Property AltNumber As String Implements IeZRCContacts.AltNumber
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _AltNumber
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _AltNumber = value Then
                Return
            End If
            _AltNumber = value
            IsModified = True
        End Set
    End Property

    Public Property City As String Implements IeZRCContacts.City
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _City
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _City = value Then
                Return
            End If
            _City = value
            IsModified = True
        End Set
    End Property

    Public Property CompanyName As String Implements IeZRCContacts.CompanyName
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

    Public Property ContactName As String Implements IeZRCContacts.ContactName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ContactName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ContactName = value Then
                Return
            End If
            _ContactName = value
            IsModified = True
        End Set
    End Property

    Public Property Country As String Implements IeZRCContacts.Country
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Country
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Country = value Then
                Return
            End If
            _Country = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZRCContacts.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZRCContacts.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZRCContacts.CreatedOn
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

    Public Property Email As String Implements IeZRCContacts.Email
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

    Public Property ezContactId As Integer Implements IeZRCContacts.ezContactId
        Get
            If _ezContactId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ezContactId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ezContactId <> 0 AndAlso _ezContactId <> value Then
                Throw New MemberAccessException()
            End If
            _ezContactId = value
        End Set
    End Property

    Public Property Fax As String Implements IeZRCContacts.Fax
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Fax
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Fax = value Then
                Return
            End If
            _Fax = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZRCContacts.Isdeleted
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property LastName As String Implements IeZRCContacts.LastName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LastName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LastName = value Then
                Return
            End If
            _LastName = value
            IsModified = True
        End Set
    End Property

    Public Property Mobile As String Implements IeZRCContacts.Mobile
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Mobile
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Mobile = value Then
                Return
            End If
            _Mobile = value
            IsModified = True
        End Set
    End Property

    Public Property Phone As String Implements IeZRCContacts.Phone
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Phone
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Phone = value Then
                Return
            End If
            _Phone = value
            IsModified = True
        End Set
    End Property

    Public Property SecondAltNumber As String Implements IeZRCContacts.SecondAltNumber
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SecondAltNumber
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SecondAltNumber = value Then
                Return
            End If
            _SecondAltNumber = value
            IsModified = True
        End Set
    End Property

    Public Property SecondCity As String Implements IeZRCContacts.SecondCity
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SecondCity
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SecondCity = value Then
                Return
            End If
            _SecondCity = value
            IsModified = True
        End Set
    End Property

    Public Property SecondFax As String Implements IeZRCContacts.SecondFax
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SecondFax
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SecondFax = value Then
                Return
            End If
            _SecondFax = value
            IsModified = True
        End Set
    End Property

    Public Property SecondMobile As String Implements IeZRCContacts.SecondMobile
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SecondMobile
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SecondMobile = value Then
                Return
            End If
            _SecondMobile = value
            IsModified = True
        End Set
    End Property

    Public Property SecondPhone As String Implements IeZRCContacts.SecondPhone
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _SecondPhone
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _SecondPhone = value Then
                Return
            End If
            _SecondPhone = value
            IsModified = True
        End Set
    End Property

    Public Property Title As String Implements IeZRCContacts.Title
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

    Public Property UpdatedBy As Integer Implements IeZRCContacts.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZRCContacts.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZRCContacts.UpdatedOn
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

    Public Property WebPage As String Implements IeZRCContacts.WebPage
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _WebPage
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _WebPage = value Then
                Return
            End If
            _WebPage = value
            IsModified = True
        End Set
    End Property

    Public Property Categories As String Implements IeZRCContacts.Categories
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Categories
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Categories = value Then
                Return
            End If
            _Categories = value
            IsModified = True
        End Set
    End Property

    Public Property POBox As String Implements IeZRCContacts.POBox
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _POBox
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _POBox = value Then
                Return
            End If
            _POBox = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
