Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI

Public Class eZLookupClientField
    Inherits IDatabaseCommonItems
    Implements IeZLookupClientField

    Protected _LookupClientFieldId As Integer
    Protected _ECMField As String
    Protected _LookupId As Integer
    Protected _ClientField As String
    Protected _IsSyncField As Boolean
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Protected _ClientFieldValues As String = ""

    Public Sub New(LookupClientFieldId As Integer)
        Me._LookupClientFieldId = LookupClientFieldId
    End Sub
    Public Sub New()
    End Sub
    Public Property ClientField() As String Implements IeZLookupClientField.ClientField
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ClientField
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ClientField = value Then
                Return
            End If
            _ClientField = value
            IsModified = True
        End Set
    End Property





    Public Property LookupId() As Integer Implements IeZLookupClientField.LookupId
        Get
            If _LookupId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LookupId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LookupId <> 0 AndAlso _LookupId <> value Then
                Throw New MemberAccessException()
            End If
            _LookupId = value
        End Set
    End Property
    Public Property LookupClientFieldId() As Integer Implements IeZLookupClientField.LookupClientFieldId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupClientFieldId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LookupClientFieldId = value Then
                Return
            End If
            _LookupClientFieldId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZLookupClientField.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLookupClientField.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZLookupClientField.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZLookupClientField.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZLookupClientField.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZLookupClientField.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZLookupClientField.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZLookupClientField() As Boolean Implements IeZLookupClientField.IseZLookupClientField
        Get
            Return (_LookupClientFieldId > 0)
        End Get
    End Property

    Public Property ClientFieldValues() As String Implements IeZLookupClientField.ClientFieldValues
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ClientFieldValues
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ClientFieldValues = value Then
                Return
            End If
            _ClientFieldValues = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub




End Class
