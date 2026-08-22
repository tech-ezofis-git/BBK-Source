Imports ECMAPI

Public Class eZFolderMonitor
    Inherits IDatabaseCommonItems
    Implements IeZFolderMonitor

    Protected _Monitorid As Integer
    Protected _TemplateId As Integer
    Protected _MonitorPath As String = ""
    Protected _Monitortype As String = ""
    Protected _MonitorTypeId As Integer
    Protected _FileType As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Protected _IsActive As Boolean
    Protected _Schedule As Boolean
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(Monitorid As Integer)
        Me._Monitorid = Monitorid
    End Sub

    Public Property CreatedBy As Integer Implements IeZFolderMonitor.CreatedBy
        Get
            If _CreatedBy = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CreatedBy
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CreatedBy <> 0 AndAlso _CreatedBy <> value Then
                Throw New MemberAccessException()
            End If
            _CreatedBy = value
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZFolderMonitor.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZFolderMonitor.CreatedOn
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

    Public Property FileType As String Implements IeZFolderMonitor.FileType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FileType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FileType = value Then
                Return
            End If
            _FileType = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZFolderMonitor.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property Monitorid As Integer Implements IeZFolderMonitor.Monitorid
        Get
            If _Monitorid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Monitorid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Monitorid <> 0 AndAlso _Monitorid <> value Then
                Throw New MemberAccessException()
            End If
            _Monitorid = value
        End Set
    End Property

    Public Property MonitorPath As String Implements IeZFolderMonitor.MonitorPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MonitorPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MonitorPath = value Then
                Return
            End If
            _MonitorPath = value
            IsModified = True
        End Set
    End Property

    Public Property Monitortype As String Implements IeZFolderMonitor.Monitortype
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Monitortype
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Monitortype = value Then
                Return
            End If
            _Monitortype = value
            IsModified = True
        End Set
    End Property

    Public Property MonitorTypeId As Integer Implements IeZFolderMonitor.MonitorTypeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MonitorTypeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _MonitorTypeId = value Then
                Return
            End If
            _MonitorTypeId = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId As Integer Implements IeZFolderMonitor.TemplateId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateId = value Then
                Return
            End If
            _TemplateId = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZFolderMonitor.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZFolderMonitor.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZFolderMonitor.UpdatedOn
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

    Public Property IsActive As Boolean Implements IeZFolderMonitor.IsActive
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsActive
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsActive = value Then
                Return
            End If
            _IsActive = value
            IsModified = True
        End Set
    End Property

    Public Property Schedule As Boolean Implements IeZFolderMonitor.Schedule
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Schedule
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _Schedule = value Then
                Return
            End If
            _Schedule = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
