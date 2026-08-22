Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZMail
    Inherits IDatabaseCommonItems
    Implements IeZMail
    Protected _MailId As Integer
    Protected _MailSettingId As Integer
    'Protected _FromAdd As String = ""
    Protected _ToAdd As String
    Protected _Subject As String = ""
    Protected _Body As String = ""
    Protected _BodyHtmlTypeId As Integer
    Protected _MailStatus As Integer
    Protected _AttachmentsPaths As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._MailId = DeptId
    End Sub
    'Public Sub New(MailArchiveName As String)
    '    Me._FromAdd = MailArchiveName.Trim()
    'End Sub
    Public Sub New()
    End Sub

    Public Property MailId() As Integer Implements IeZMail.MailId
        Get
            If _MailId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MailId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MailId <> 0 AndAlso _MailId <> value Then
                Throw New MemberAccessException()
            End If
            _MailId = value
        End Set
    End Property
    Public Property MailSettingId() As Integer Implements IeZMail.MailSettingId
        Get
            DBLayer.DBLInstance.Read(Me)

            Return _MailSettingId
        End Get
        Set(value As Integer)

            DBLayer.DBLInstance.Read(Me)
            If _MailSettingId = value Then
                Return
            End If
            _MailSettingId = value
        End Set
    End Property
    Public Property MailStatus() As Integer Implements IeZMail.MailStatus
        Get
            DBLayer.DBLInstance.Read(Me)

            Return _MailStatus
        End Get
        Set(value As Integer)

            DBLayer.DBLInstance.Read(Me)
            If _MailStatus = value Then
                Return
            End If
            _MailStatus = value
        End Set
    End Property
    Public Property AttachmentsPaths() As String Implements IeZMail.AttachmentsPaths
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _AttachmentsPaths
        End Get
        Set(value As String)

            DBLayer.DBLInstance.Read(Me)
            If _AttachmentsPaths = value Then
                Return
            End If
            _AttachmentsPaths = value
        End Set
    End Property
    Public Property BodyHtmlTypeId() As Integer Implements IeZMail.BodyHtmlTypeId
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _BodyHtmlTypeId
        End Get
        Set(value As Integer)

            DBLayer.DBLInstance.Read(Me)
            If _BodyHtmlTypeId = value Then
                Return
            End If
            _BodyHtmlTypeId = value
        End Set
    End Property
    Public Property ToAdd() As String Implements IeZMail.ToAdd
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToAdd
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ToAdd = value Then
                Return
            End If
            _ToAdd = value
            IsModified = True
        End Set
    End Property
    'Public Property FromAdd() As String Implements IeZMail.FromAdd
    '    Get
    '        DBLayer.DBLInstance.Read(Me)
    '        Return _FromAdd
    '    End Get
    '    Set(value As String)
    '        DBLayer.DBLInstance.Read(Me)
    '        If _FromAdd = value Then
    '            Return
    '        End If
    '        _FromAdd = value
    '        IsModified = True
    '    End Set
    'End Property
    Public Property Body() As String Implements IeZMail.Body
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Body
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Body = value Then
                Return
            End If
            _Body = value
            IsModified = True
        End Set
    End Property
    Public Property Subject() As String Implements IeZMail.Subject
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Subject
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Subject = value Then
                Return
            End If
            _Subject = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZMail.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZMail.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZMail.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZMail.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZMail.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZMail.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZMail.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZMailtExist() As Boolean Implements IeZMail.IseZMailExist
        Get
            Return (_MailId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
