Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZCabOwners
    Inherits IDatabaseCommonItems
    Implements IeZCabOwners
    Protected _CabinetID As Integer
    Protected _CabinetName As String
    Protected _CabOwnerID As Integer
    Protected _EmpId As String
    Protected _EmployeeName As String
    Protected _UserId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpCabOwnerID As Integer)
        Me._CabOwnerID = tmpCabOwnerID
    End Sub

    Public Sub New()
    End Sub
    Public Property CabOwnerID() As Integer Implements IeZCabOwners.CabOwnerID
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabOwnerID
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CabOwnerID = value Then
                Return
            End If
            _CabOwnerID = value
            IsModified = True
        End Set
    End Property
    Public Property EmployeeName() As String Implements IeZCabOwners.EmployeeName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EmployeeName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EmployeeName = value Then
                Return
            End If
            _EmployeeName = value
            IsModified = True
        End Set
    End Property
    Public Property UserId() As Integer Implements IeZCabOwners.UserId
        Get
            If _UserId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _UserId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _UserId <> 0 AndAlso _UserId <> value Then
                Throw New MemberAccessException()
            End If
            _UserId = value
        End Set
    End Property
    Public Property EmpId() As String Implements IeZCabOwners.EmpId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EmpId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EmpId = value Then
                Return
            End If
            _EmpId = value
            IsModified = True
        End Set
    End Property
    Public Property CabinetID() As Integer Implements IeZCabOwners.CabinetID
        Get
            If _CabinetID = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CabinetID
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CabinetID <> 0 AndAlso _CabinetID <> value Then
                Throw New MemberAccessException()
            End If
            _CabinetID = value
        End Set
    End Property
    Public Property CabinetName() As String Implements IeZCabOwners.CabinetName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabinetName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CabinetName = value Then
                Return
            End If
            _CabinetName = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZCabOwners.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZCabOwners.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZCabOwners.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZCabOwners.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZCabOwners.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZCabOwners.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZCabOwners.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsCabOwnersExist() As Boolean Implements IeZCabOwners.IsCabOwnersExist
        Get
            Return (_CabinetID > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
