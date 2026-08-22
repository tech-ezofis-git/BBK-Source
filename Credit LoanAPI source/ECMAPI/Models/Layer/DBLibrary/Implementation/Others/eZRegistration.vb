Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZRegistration
    Inherits IDatabaseCommonItems
    Implements IeZRegistration
    Protected _CompanyId As Integer
    Protected _CompanyName As String = ""
    Protected _StateName As String = ""
    Protected _TypeOfIndustry As String = ""
    Protected _Country As String = ""
    Protected _NoOfEmployees As Integer
    Protected _EmpName As String = ""
    Protected _Phone As String = ""
    Protected _Designation As String = ""
    Protected _Email As String = ""
    Protected _Subscribe As Integer
    Protected _AllowTeamToContact As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer


    Public Sub New(tmpCompanyId As Integer)
        Me._CompanyId = tmpCompanyId
    End Sub

    Public Sub New()
    End Sub
    Public Property CompanyId() As Integer Implements IeZRegistration.CompanyId
        Get
            If _CompanyId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CompanyId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CompanyId <> 0 AndAlso _CompanyId <> value Then
                Throw New MemberAccessException()
            End If
            _CompanyId = value
        End Set
    End Property

    Public Property CompanyName() As String Implements IeZRegistration.CompanyName
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
    Public Property StateName() As String Implements IeZRegistration.StateName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _StateName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _StateName = value Then
                Return
            End If
            _StateName = value
            IsModified = True
        End Set
    End Property
    Public Property Country() As String Implements IeZRegistration.Country
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
    Public Property TypeOfIndustry() As String Implements IeZRegistration.TypeOfIndustry
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TypeOfIndustry
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TypeOfIndustry = value Then
                Return
            End If
            _TypeOfIndustry = value
            IsModified = True
        End Set
    End Property
    Public Property NoOfEmployees() As Integer Implements IeZRegistration.NoOfEmployees
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _NoOfEmployees
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _NoOfEmployees = value Then
                Return
            End If
            _NoOfEmployees = value
            IsModified = True
        End Set
    End Property
    Public Property EmpName() As String Implements IeZRegistration.EmpName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EmpName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EmpName = value Then
                Return
            End If
            _EmpName = value
            IsModified = True
        End Set
    End Property
    Public Property Phone() As String Implements IeZRegistration.Phone
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
    Public Property Designation() As String Implements IeZRegistration.Designation
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Designation
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Designation = value Then
                Return
            End If
            _Designation = value
            IsModified = True
        End Set
    End Property
    Public Property Email() As String Implements IeZRegistration.Email
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
    Public Property Subscribe() As Integer Implements IeZRegistration.Subscribe
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Subscribe
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Subscribe = value Then
                Return
            End If
            _Subscribe = value
            IsModified = True
        End Set
    End Property

    Public Property AllowTeamToContact() As Integer Implements IeZRegistration.AllowTeamToContact
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _AllowTeamToContact
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _AllowTeamToContact = value Then
                Return
            End If
            _AllowTeamToContact = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZRegistration.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZRegistration.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZRegistration.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZRegistration.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZRegistration.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZRegistration.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZRegistration.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
