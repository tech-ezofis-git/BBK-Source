Imports ECMAPI

Public Class eZFormUsers
    Inherits IDatabaseCommonItems
    Implements IeZFormUsers

    Protected _FormUsersId As Integer
    Protected _FormId As Integer
    Protected _ECMLoginId As Integer
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String
    Protected _Updatedby1 As String
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(formusersid As Integer)
        Me._FormUsersId = formusersid
    End Sub

    Public Property Createdby() As Integer Implements IeZFormUsers.Createdby
        Get
            If _Createdby = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Createdby
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Createdby <> 0 AndAlso _Createdby <> value Then
                Throw New MemberAccessException()
            End If
            _Createdby = value
        End Set
    End Property

    Public Property Createdby1() As String Implements IeZFormUsers.Createdby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Createdby1 = value Then
                Return
            End If
            _Createdby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon() As String Implements IeZFormUsers.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Createdon = value Then
                Return
            End If
            _Createdon = value
            IsModified = True
        End Set
    End Property

    Public Property ECMLoginId() As Integer Implements IeZFormUsers.ECMLoginId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMLoginId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMLoginId = value Then
                Return
            End If
            _ECMLoginId = value
            IsModified = True
        End Set
    End Property

    Public Property FormId() As Integer Implements IeZFormUsers.FormId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FormId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FormId = value Then
                Return
            End If
            _FormId = value
            IsModified = True
        End Set
    End Property

    Public Property FormUsersId() As Integer Implements IeZFormUsers.FormUsersId
        Get
            If _FormUsersId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FormUsersId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FormUsersId <> 0 AndAlso _FormUsersId <> value Then
                Throw New MemberAccessException()
            End If
            _FormUsersId = value
        End Set
    End Property

    Public ReadOnly Property isdeleted() As Integer Implements IeZFormUsers.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property Updatedby() As Integer Implements IeZFormUsers.Updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedby = value Then
                Return
            End If
            _Updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby1() As String Implements IeZFormUsers.Updatedby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedby1 = value Then
                Return
            End If
            _Updatedby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedon() As String Implements IeZFormUsers.Updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedon = value Then
                Return
            End If
            _Updatedon = value
            IsModified = True
        End Set
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
