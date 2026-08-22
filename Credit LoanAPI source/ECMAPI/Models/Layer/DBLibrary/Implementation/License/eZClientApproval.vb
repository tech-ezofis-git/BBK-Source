Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI

Public Class eZClientApproval
    Inherits IDatabaseCommonItems
    Implements IeZClientAppproval

    Protected _Appprime As String = ""
    Protected _Approval As String = ""
    Protected _ApprovalCode As String = ""
    Protected _ClientApprovalId As Integer
    Protected _ConfigPrimeId As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedOn As String = ""
    Private _Isdeleted As Integer = 0
    Protected _PrimeOn As String = ""
    Protected _ISA As Integer
    Protected _Active As Integer
    Protected _PrimeCount As String = ""
    Protected _PrimeDepart As String = ""
    Protected _CreatedBy As String = ""
    Protected _UpdatedBy As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Protected _UserId As Integer = 0
    Public Sub New(DeptId As Integer)
        Me._ClientApprovalId = DeptId
    End Sub
    Public Sub New()
    End Sub
    Public Property Appprime As String Implements IeZClientAppproval.Appprime
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Appprime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Appprime = value Then
                Return
            End If
            _Appprime = value
            IsModified = True
        End Set
    End Property
    Public Property Approval As String Implements IeZClientAppproval.Approval
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Approval
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Approval = value Then
                Return
            End If
            _Approval = value
            IsModified = True
        End Set
    End Property
    Public Property ApprovalCode As String Implements IeZClientAppproval.ApprovalCode
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ApprovalCode
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ApprovalCode = value Then
                Return
            End If
            _ApprovalCode = value
            IsModified = True
        End Set
    End Property
    Public Property ClientApprovalId As Integer Implements IeZClientAppproval.ClientApprovalId
        Get
            If _ClientApprovalId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ClientApprovalId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ClientApprovalId <> 0 AndAlso _ClientApprovalId <> value Then
                Throw New MemberAccessException()
            End If
            _ClientApprovalId = value
        End Set
    End Property
    Public Property ConfigPrimeId As Integer Implements IeZClientAppproval.ConfigPrimeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ConfigPrimeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ConfigPrimeId = value Then
                Return
            End If
            _ConfigPrimeId = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedOn As String Implements IeZClientAppproval.CreatedOn
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
    Public Property ISA As Integer Implements IeZClientAppproval.ISA
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ISA
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ISA = value Then
                Return
            End If

            _ISA = value
            IsModified = True
        End Set
    End Property
    Public Property Active As Integer Implements IeZClientAppproval.Active
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Active
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Active = value Then
                Return
            End If

            _Active = value
            IsModified = True
        End Set
    End Property
    Public ReadOnly Property IsDeleted As Integer Implements IeZClientAppproval.IsDeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public Property PrimeCount As String Implements IeZClientAppproval.PrimeCount
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _PrimeCount
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _PrimeCount = value Then
                Return
            End If

            _PrimeCount = value
            IsModified = True
        End Set
    End Property
    Public Property PrimeDepart As String Implements IeZClientAppproval.PrimeDepart
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _PrimeDepart
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _PrimeDepart = value Then
                Return
            End If

            _PrimeDepart = value
            IsModified = True
        End Set
    End Property
    Public Property PrimeOn As String Implements IeZClientAppproval.PrimeOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _PrimeOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _PrimeOn = value Then
                Return
            End If

            _PrimeOn = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedOn As String Implements IeZClientAppproval.UpdatedOn
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
    Public Property CreatedBy As String Implements IeZClientAppproval.CreatedBy
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
        End Set
    End Property
    Public Property UpdatedBy As String Implements IeZClientAppproval.UpdatedBy
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
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZClientAppproval.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZClientAppproval.CreatedBy1
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

    Public Property UserId() As Integer Implements IeZClientAppproval.UserId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UserId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UserId = value Then
                Return
            End If
            _UserId = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
