Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZMailArchiveType
    Inherits IDatabaseCommonItems
    Implements IeZMailArchiveType
    Protected _MailArchiveTypeId As Integer
    Protected _MailArchiveType As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CUserName As String = ""
    Protected _CUserCode As String = ""
    Protected _UUserName As String = ""
    Protected _UUserCode As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New(tmpMailArchiveTypeId As Integer)
        Me._MailArchiveTypeId = tmpMailArchiveTypeId
    End Sub
    Public Sub New(tmpMailArchiveType As String)
        Me._MailArchiveType = tmpMailArchiveType
    End Sub

    Public Sub New()
    End Sub
    Public Property MailArchiveTypeId() As Integer Implements IeZMailArchiveType.MailArchiveTypeId
        Get
            If _MailArchiveTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MailArchiveTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MailArchiveTypeId <> 0 AndAlso _MailArchiveTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _MailArchiveTypeId = value
        End Set
    End Property

    Public Property MailArchiveType() As String Implements IeZMailArchiveType.MailArchiveType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailArchiveType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MailArchiveType = value Then
                Return
            End If
            _MailArchiveType = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZMailArchiveType.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZMailArchiveType.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZMailArchiveType.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZMailArchiveType.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZMailArchiveType.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZMailArchiveType.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZMailArchiveType.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsMailArchiveTypeExist() As Boolean Implements IeZMailArchiveType.IsMailArchiveTypeExist
        Get
            Return (MailArchiveTypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
