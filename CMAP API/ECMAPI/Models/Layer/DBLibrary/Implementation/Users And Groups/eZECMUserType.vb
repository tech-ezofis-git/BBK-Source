Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZECMUserType
    Inherits IDatabaseCommonItems
    Implements IeZECMUserType
    Protected _ECMUserTypeId As Integer
    Protected _ECMUserType As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CUserName As String
    Protected _CUserCode As String
    Protected _UUserName As String
    Protected _UUserCode As String
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpECMUserTypeId As Integer)
        Me._ECMUserTypeId = tmpECMUserTypeId
    End Sub
    Public Sub New(tmpECMUserType As String)
        Me._ECMUserType = tmpECMUserType
    End Sub

    Public Sub New()
    End Sub
    Public Property ECMUserTypeId() As Integer Implements IeZECMUserType.ECMUserTypeId
        Get
            If _ECMUserTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMUserTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMUserTypeId <> 0 AndAlso _ECMUserTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMUserTypeId = value
        End Set
    End Property

    Public Property ECMUserType() As String Implements IeZECMUserType.ECMUserType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMUserType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMUserType = value Then
                Return
            End If
            _ECMUserType = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZECMUserType.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZECMUserType.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZECMUserType.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZECMUserType.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZECMUserType.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZECMUserType.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZECMUserType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsECMUserTypeExist() As Boolean Implements IeZECMUserType.IsECMUserTypeExist
        Get
            Return (ECMUserTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
