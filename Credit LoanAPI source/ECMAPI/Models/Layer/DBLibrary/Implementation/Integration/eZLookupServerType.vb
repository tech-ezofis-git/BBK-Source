Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZLookupServerType
    Inherits IDatabaseCommonItems
    Implements IeZLookupServerType
    Protected _LookupServerTypeId As Integer
    Protected _LookupServerType As String
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

    Public Sub New(tmpLookupServerTypeId As Integer)
        Me._LookupServerTypeId = tmpLookupServerTypeId
    End Sub
    Public Sub New(tmpLookupServerType As String)
        Me._LookupServerType = tmpLookupServerType
    End Sub

    Public Sub New()
    End Sub
    Public Property LookupServerTypeId() As Integer Implements IeZLookupServerType.LookupServerTypeId
        Get
            If _LookupServerTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LookupServerTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LookupServerTypeId <> 0 AndAlso _LookupServerTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _LookupServerTypeId = value
        End Set
    End Property

    Public Property LookupServerType() As String Implements IeZLookupServerType.LookupServerType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupServerType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LookupServerType = value Then
                Return
            End If
            _LookupServerType = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZLookupServerType.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLookupServerType.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZLookupServerType.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZLookupServerType.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZLookupServerType.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZLookupServerType.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZLookupServerType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsLookupServerTypeExist() As Boolean Implements IeZLookupServerType.IsLookupServerTypeExist
        Get
            Return (LookupServerTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
