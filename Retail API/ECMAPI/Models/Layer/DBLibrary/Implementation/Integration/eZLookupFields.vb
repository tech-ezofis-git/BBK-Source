Imports System.Data
Imports System.Configuration
Imports System.Web


Public Class eZLookupFields
    Inherits IDatabaseCommonItems
    Implements IeZLookupFields
    Protected _LookupFieldId As Integer
    Protected _ECMField As String
    Protected _LookupId As Integer
    Protected _ClientField As String = ""
    Protected _ParameterOrder As Integer
    Protected _IsSyncField As Boolean
    Protected _Templateid As String
    Protected _Cabinetid As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(LookupFieldId As Integer)
        Me._LookupFieldId = LookupFieldId
    End Sub
    Public Sub New()
    End Sub
    Public Property ClientField() As String Implements IeZLookupFields.ClientField
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
    Public Property Templateid() As String Implements IeZLookupFields.Templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Templateid
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Templateid = value Then
                Return
            End If
            _Templateid = value
            IsModified = True
        End Set
    End Property
    Public Property Cabinetid() As String Implements IeZLookupFields.Cabinetid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Cabinetid
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Cabinetid = value Then
                Return
            End If
            _Cabinetid = value
            IsModified = True
        End Set
    End Property

    Public Property IsSyncField() As Boolean Implements IeZLookupFields.IsSyncField
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsSyncField
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsSyncField = value Then
                Return
            End If
            _IsSyncField = value
            IsModified = True
        End Set
    End Property

    Public Property ECMField() As String Implements IeZLookupFields.ECMField
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMField
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMField = value Then
                Return
            End If
            _ECMField = value
            IsModified = True
        End Set
    End Property


    Public Property LookupId() As Integer Implements IeZLookupFields.LookupId
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
    Public Property LookupFieldId() As Integer Implements IeZLookupFields.LookupFieldId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupFieldId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LookupFieldId = value Then
                Return
            End If
            _LookupFieldId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZLookupFields.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLookupFields.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZLookupFields.CreatedBy
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
    Public Property ParameterOrder() As Integer Implements IeZLookupFields.ParameterOrder
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ParameterOrder
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ParameterOrder = value Then
                Return
            End If

            _ParameterOrder = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedOn() As String Implements IeZLookupFields.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZLookupFields.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZLookupFields.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZLookupFields.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZLookupFields() As Boolean Implements IeZLookupFields.IseZLookupFields
        Get
            Return (_LookupFieldId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub




End Class
