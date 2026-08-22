Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZLookupType
    Inherits IDatabaseCommonItems
    Implements IeZLookupType
    Protected _LookupTypeId As Integer
    Protected _LookupType As String
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

    Public Sub New(tmpLookupTypeId As Integer)
        Me._LookupTypeId = tmpLookupTypeId
    End Sub
    Public Sub New(tmpLookupType As String)
        Me._LookupType = tmpLookupType
    End Sub

    Public Sub New()
    End Sub
    Public Property LookupTypeId() As Integer Implements IeZLookupType.LookupTypeId
        Get
            If _LookupTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LookupTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LookupTypeId <> 0 AndAlso _LookupTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _LookupTypeId = value
        End Set
    End Property

    Public Property LookupType() As String Implements IeZLookupType.LookupType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LookupType = value Then
                Return
            End If
            _LookupType = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZLookupType.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLookupType.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZLookupType.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZLookupType.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZLookupType.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZLookupType.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZLookupType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsLookupTypeExist() As Boolean Implements IeZLookupType.IsLookupTypeExist
        Get
            Return (LookupTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
