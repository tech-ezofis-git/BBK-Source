Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZMailArchive
    Inherits IDatabaseCommonItems
    Implements IeZMailArchive
    Protected _MailArchiveId As Integer
    Protected _ScheduleId As Integer
    Protected _MailArchiveTypeId As Integer
    Protected _MailArchiveType As String
    Protected _MailArchiveValue As String
    Protected _MailArchiveValueId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._MailArchiveId = DeptId
    End Sub
    Public Sub New(MailArchiveName As String)
        Me._ScheduleId = MailArchiveName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property MailArchiveId() As Integer Implements IeZMailArchive.MailArchiveId
        Get
            If _MailArchiveId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MailArchiveId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MailArchiveId <> 0 AndAlso _MailArchiveId <> value Then
                Throw New MemberAccessException()
            End If
            _MailArchiveId = value
        End Set
    End Property
    Public Property MailArchiveValueId() As Integer Implements IeZMailArchive.MailArchiveValueId
        Get
            If _MailArchiveValueId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MailArchiveValueId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MailArchiveValueId <> 0 AndAlso _MailArchiveValueId <> value Then
                Throw New MemberAccessException()
            End If
            _MailArchiveValueId = value
        End Set
    End Property
    Public Property MailArchiveTypeId() As Integer Implements IeZMailArchive.MailArchiveTypeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailArchiveTypeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _MailArchiveTypeId = value Then
                Return
            End If
            _MailArchiveTypeId = value
            IsModified = True
        End Set
    End Property
    Public Property ScheduleId() As Integer Implements IeZMailArchive.ScheduleId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ScheduleId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ScheduleId = value Then
                Return
            End If
            _ScheduleId = value
            IsModified = True
        End Set
    End Property
    Public Property MailArchiveValue() As String Implements IeZMailArchive.MailArchiveValue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailArchiveValue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MailArchiveValue = value Then
                Return
            End If
            _MailArchiveValue = value
            IsModified = True
        End Set
    End Property
    Public Property MailArchiveType() As String Implements IeZMailArchive.MailArchiveType
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
    Public Property UpdatedBy1() As String Implements IeZMailArchive.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZMailArchive.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZMailArchive.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZMailArchive.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZMailArchive.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZMailArchive.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZMailArchive.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZMailArchivetExist() As Boolean Implements IeZMailArchive.IseZMailArchiveExist
        Get
            Return (_MailArchiveId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
