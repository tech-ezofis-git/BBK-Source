
Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZECMGroupusers
    Inherits IDatabaseCommonItems
    Implements IeZECMGroupusers


    Protected D_ECMGroupUserId As Integer
    Protected D_ECMGroupId As Integer
    Protected D_ECMLoginId As Integer
    Protected D_CreatedOn As String
    Protected D_UpdatedOn As String
    Protected D_CreatedBy As Integer = 0
    Protected D_UpdatedBy As Integer = 0
    Protected D_UpdatedBy1 As String
    Protected D_Createdby1 As String
    Private D_isdeleted As Integer = 0


    Public Sub New(tempGroupid As Integer)
        Me.D_ECMGroupUserId = tempGroupid
    End Sub

    Public Sub New()
    End Sub
    Public Property CreatedBy As Integer Implements IeZECMGroupusers.CreatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_CreatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_CreatedBy = value Then
                Return
            End If
            D_CreatedBy = value
            IsModified = True
        End Set
    End Property
    Public Property CreatedBy1 As String Implements IeZECMGroupusers.Createdby1
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

    Public Property CreatedOn As String Implements IeZECMGroupusers.CreatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_CreatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_CreatedOn = value Then
                Return
            End If
            D_CreatedOn = value
            IsModified = True
        End Set
    End Property

    Public Property ECMGroupId As Integer Implements IeZECMGroupusers.ECMGroupId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ECMGroupId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_ECMGroupId = value Then
                Return
            End If
            D_ECMGroupId = value
            IsModified = True
        End Set
    End Property

    Public Property ECMGroupUserId As Integer Implements IeZECMGroupusers.ECMGroupUserId
        Get
            If D_ECMGroupUserId = 0 Then
                DBLayer.DBLInstance.read(Me)
            End If
            Return D_ECMGroupUserId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_ECMGroupUserId <> 0 AndAlso D_ECMGroupUserId <> value Then
                Throw New MemberAccessException()
            End If
            D_ECMGroupUserId = value
        End Set
    End Property

    Public Property ECMLoginId As Integer Implements IeZECMGroupusers.ECMLoginId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_ECMLoginId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_ECMLoginId = value Then
                Return
            End If
            D_ECMLoginId = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements IeZECMGroupusers.isdeleted
        Get
            Return D_isdeleted
        End Get
    End Property

    Public Property UpdatedBy As Integer Implements IeZECMGroupusers.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_UpdatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_UpdatedBy = value Then
                Return
            End If
            D_UpdatedBy = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1 As String Implements IeZECMGroupusers.updatedby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_UpdatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_UpdatedBy1 = value Then
                Return
            End If
            D_UpdatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedOn As String Implements IeZECMGroupusers.UpdatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_UpdatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_UpdatedOn = value Then
                Return
            End If
            D_UpdatedOn = value
            IsModified = True
        End Set
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
