Imports ECMAPI

Public Class eZMapFields
    Inherits IDatabaseCommonItems
    Implements IeZMapFields

    Protected _Mapfieldsid As Integer
    Protected _Cabinetid As Integer
    Protected _Templateid As Integer
    Protected _LocationField As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New()
    End Sub
    Public Sub New(Mapfieldsid As Integer)
        Me._Mapfieldsid = Mapfieldsid
    End Sub
    Public Property Cabinetid As Integer Implements IeZMapFields.Cabinetid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Cabinetid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Cabinetid = value Then
                Return
            End If
            _Cabinetid = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZMapFields.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZMapFields.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZMapFields.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZMapFields.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property LocationField As String Implements IeZMapFields.LocationField
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LocationField
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LocationField = value Then
                Return
            End If
            _LocationField = value
            IsModified = True
        End Set
    End Property

    Public Property Mapfieldsid As Integer Implements IeZMapFields.Mapfieldsid
        Get
            If _Mapfieldsid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Mapfieldsid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Mapfieldsid <> 0 AndAlso _Mapfieldsid <> value Then
                Throw New MemberAccessException()
            End If
            _Mapfieldsid = value
        End Set
    End Property

    Public Property Templateid As Integer Implements IeZMapFields.Templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Templateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Templateid = value Then
                Return
            End If
            _Templateid = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMapFields.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZMapFields.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZMapFields.UpdatedOn
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

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
