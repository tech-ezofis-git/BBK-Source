Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZPrivateFolders
    Inherits IDatabaseCommonItems
    Implements IeZPrivateFolders

    Protected D_Privatefolderid As Integer
    Protected D_Nodeid As Integer
    Protected D_userid As Integer
    Protected D_Createdon As String
    Protected D_Updatedon As String
    Protected D_Createdby As Integer = 0
    Protected D_Updatedby As Integer = 0
    Protected D_Createdby1 As String
    Protected D_Updatedby1 As String
    Private D_isdeleted As Integer = 0

    Public Sub New(tmpprivateid As String)
        Me.D_Privatefolderid = tmpprivateid
    End Sub
    Public Sub New()
    End Sub


    Public Property Createdby As Integer Implements IeZPrivateFolders.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdby = value Then
                Return
            End If
            D_Createdby = value
            IsModified = True
        End Set
    End Property

    Public Property Createdby1 As String Implements IeZPrivateFolders.Createdby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdby1 = value Then
                Return
            End If
            D_Createdby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IeZPrivateFolders.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Createdon = value Then
                Return
            End If
            D_Createdon = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZPrivateFolders.isdeleted
        Get
            Return D_isdeleted
        End Get
    End Property

    Public Property Nodeid As Integer Implements IeZPrivateFolders.Nodeid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Nodeid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Nodeid = value Then
                Return
            End If
            D_Nodeid = value
            IsModified = True
        End Set
    End Property

    Public Property Privatefolderid As Integer Implements IeZPrivateFolders.Privatefolderid
        Get
            If D_Privatefolderid = 0 Then
                DBLayer.DBLInstance.read(Me)
            End If
            Return D_Privatefolderid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_Privatefolderid <> 0 AndAlso D_Privatefolderid <> value Then
                Throw New MemberAccessException()
            End If
            D_Privatefolderid = value
        End Set
    End Property

    Public Property Updatedby As Integer Implements IeZPrivateFolders.Updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Updatedby = value Then
                Return
            End If
            D_Updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby1 As String Implements IeZPrivateFolders.Updatedby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Updatedby1 = value Then
                Return
            End If
            D_Updatedby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedon As String Implements IeZPrivateFolders.Updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Updatedon = value Then
                Return
            End If
            D_Updatedon = value
            IsModified = True
        End Set
    End Property

    Public Property userid As Integer Implements IeZPrivateFolders.userid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_userid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_userid = value Then
                Return
            End If
            D_userid = value
            IsModified = True
        End Set
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
