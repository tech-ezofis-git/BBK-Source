Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZMailConfig
    Inherits IDatabaseCommonItems
    Implements IeZMailConfig
    Protected D_MailConfigId As Integer
    Protected D_Host As String
    Protected D_Port As Integer
    Protected D_Mailid As String
    Protected D_UserName As String
    Protected D_Password As String
    Private D_EnableSSL As Boolean
    Protected D_CreatedOn As String
    Protected D_UpdatedOn As String
    Protected D_CreatedBy As Integer
    Protected D_UpdatedBy As Integer
    Protected D_CreatedBy1 As String
    Protected D_UpdatedBy1 As String
    Private D_Isdeleted As Boolean

    Public Property CreatedBy As Integer Implements IeZMailConfig.CreatedBy
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


    Public Property CreatedBy1 As String Implements IeZMailConfig.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_CreatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_CreatedBy1 = value Then
                Return
            End If
            D_CreatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedOn As String Implements IeZMailConfig.CreatedOn
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

    Public ReadOnly Property EnableSSL As Boolean Implements IeZMailConfig.EnableSSL
        Get
            Return D_EnableSSL
        End Get
    End Property

    Public Property Host As String Implements IeZMailConfig.Host
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Host
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Host = value Then
                Return
            End If

            D_Host = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Boolean Implements IeZMailConfig.Isdeleted
        Get
            Return D_Isdeleted
        End Get
    End Property

    Public Property MailConfigId As Integer Implements IeZMailConfig.MailConfigId
        Get
            If D_MailConfigId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return D_MailConfigId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If D_MailConfigId <> 0 AndAlso D_MailConfigId <> value Then
                Throw New MemberAccessException()
            End If
            D_MailConfigId = value
        End Set
    End Property

    Public Property Mailid As String Implements IeZMailConfig.Mailid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Mailid
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Mailid = value Then
                Return
            End If

            D_Mailid = value
            IsModified = True
        End Set
    End Property

    Public Property Password As String Implements IeZMailConfig.Password
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Password
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_Password = value Then
                Return
            End If

            D_Password = value
            IsModified = True
        End Set
    End Property

    Public Property Port As Integer Implements IeZMailConfig.Port
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_Port
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If D_Port = value Then
                Return
            End If

            D_Port = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMailConfig.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZMailConfig.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZMailConfig.UpdatedOn
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

    Public Property UserName As String Implements IeZMailConfig.UserName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return D_UserName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If D_UserName = value Then
                Return
            End If

            D_UserName = value
            IsModified = True
        End Set
    End Property
End Class
