Imports ECMAPI

Public Class eZMailTriggering
    Inherits IDatabaseCommonItems
    Implements IeZMailTriggering


    Protected _MailTriggerid As Integer
    Protected _Status As Boolean
    Protected _TriggerTypeId As Integer
    Protected _MailSettingId As Integer
    Protected _Condition As String = ""
    Protected _TempWFId As Integer
    Protected _UnallocatedMailUser As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New(MailTriggerid As Integer)
        Me._MailTriggerid = MailTriggerid
    End Sub
    Public Sub New()
    End Sub

    Public Property Condition As String Implements IeZMailTriggering.Condition
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Condition
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Condition = value Then
                Return
            End If
            _Condition = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZMailTriggering.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZMailTriggering.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZMailTriggering.CreatedOn
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

    Public ReadOnly Property Isdeleted As Integer Implements IeZMailTriggering.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property MailSettingId As Integer Implements IeZMailTriggering.MailSettingId
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
            IsModified = True
        End Set
    End Property

    Public Property MailTriggerid As Integer Implements IeZMailTriggering.MailTriggerid
        Get
            If _MailTriggerid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MailTriggerid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MailTriggerid <> 0 AndAlso _MailTriggerid <> value Then
                Throw New MemberAccessException()
            End If
            _MailTriggerid = value
        End Set
    End Property

    Public Property Status As Boolean Implements IeZMailTriggering.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property TempWFId As Integer Implements IeZMailTriggering.TempWFId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TempWFId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TempWFId = value Then
                Return
            End If
            _TempWFId = value
            IsModified = True
        End Set
    End Property

    Public Property TriggerTypeId As Integer Implements IeZMailTriggering.TriggerTypeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TriggerTypeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TriggerTypeId = value Then
                Return
            End If
            _TriggerTypeId = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZMailTriggering.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZMailTriggering.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZMailTriggering.UpdatedOn
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

    Public Property UnallocatedMailUser As Integer Implements IeZMailTriggering.UnallocatedMailUser
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UnallocatedMailUser
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UnallocatedMailUser = value Then
                Return
            End If
            _UnallocatedMailUser = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
