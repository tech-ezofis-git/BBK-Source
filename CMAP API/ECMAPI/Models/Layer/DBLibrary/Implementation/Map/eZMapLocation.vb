Imports ECMAPI

Public Class eZMapLocation
    Inherits IDatabaseCommonItems
    Implements IeZMapLocation

    Protected _LocationId As Integer
    Protected _Longitude As String = ""
    Protected _Latitude As String = ""
    Protected _LocationName As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New()
    End Sub
    Public Sub New(LocationId As Integer)
        Me._LocationId = LocationId
    End Sub
    Public Property CreatedBy As Integer Implements IeZMapLocation.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZMapLocation.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZMapLocation.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZMapLocation.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property Latitude As String Implements IeZMapLocation.Latitude
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Latitude
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Latitude = value Then
                Return
            End If
            _Latitude = value
            IsModified = True
        End Set
    End Property

    Public Property LocationId As Integer Implements IeZMapLocation.LocationId
        Get
            If _LocationId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LocationId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LocationId <> 0 AndAlso _LocationId <> value Then
                Throw New MemberAccessException()
            End If
            _LocationId = value
        End Set
    End Property

    Public Property LocationName As String Implements IeZMapLocation.LocationName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LocationName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _LocationName = value Then
                Return
            End If
            _LocationName = value
            IsModified = True
        End Set
    End Property

    Public Property Longitude As String Implements IeZMapLocation.Longitude
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Longitude
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Longitude = value Then
                Return
            End If
            _Longitude = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMapLocation.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZMapLocation.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZMapLocation.UpdatedOn
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
