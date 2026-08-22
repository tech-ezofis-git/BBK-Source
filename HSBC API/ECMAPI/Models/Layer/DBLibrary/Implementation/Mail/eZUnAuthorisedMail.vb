Imports ECMAPI

Public Class eZUnAllocatedMail
    Inherits IDatabaseCommonItems
    Implements IeZUnAllocatedMail


    Protected _MailRequestId As Integer
    Protected _MailSubject As String = ""
    Protected _MailBody As String = ""
    Protected _MailFrom As String = ""
    Protected _MailSettingsId As Integer
    Protected _WorkflowId As Integer
    Protected _JunkMail As Boolean = False
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Private _isdeleted As Integer
    Protected _Workflow As String = ""

    Public Sub New()
    End Sub

    Public Sub New(MailRequestId As Integer)
        Me._MailRequestId = MailRequestId
    End Sub

    Public Property MailRequestId As Integer Implements IeZUnAllocatedMail.MailRequestId
        Get
            If _MailRequestId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _MailRequestId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _MailRequestId <> 0 AndAlso _MailRequestId <> value Then
                Throw New MemberAccessException()
            End If
            _MailRequestId = value
        End Set
    End Property

    Public Property MailSubject As String Implements IeZUnAllocatedMail.MailSubject
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailSubject
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MailSubject = value Then
                Return
            End If
            _MailSubject = value
            IsModified = True
        End Set
    End Property

    Public Property MailBody As String Implements IeZUnAllocatedMail.MailBody
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailBody
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MailBody = value Then
                Return
            End If
            _MailBody = value
            IsModified = True
        End Set
    End Property

    Public Property MailFrom As String Implements IeZUnAllocatedMail.MailFrom
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailFrom
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _MailFrom = value Then
                Return
            End If
            _MailFrom = value
            IsModified = True
        End Set
    End Property

    Public Property MailSettingsId As Integer Implements IeZUnAllocatedMail.MailSettingsId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _MailSettingsId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _MailSettingsId = value Then
                Return
            End If
            _MailSettingsId = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IeZUnAllocatedMail.Createdon
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

    Public Property Updatedon As String Implements IeZUnAllocatedMail.Updatedon
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

    Public Property Createdby As Integer Implements IeZUnAllocatedMail.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Createdby = value Then
                Return
            End If
            _Createdby = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby As Integer Implements IeZUnAllocatedMail.Updatedby
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

    Public Property CreatedBy1 As String Implements IeZUnAllocatedMail.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IeZUnAllocatedMail.UpdatedBy1
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

    Public ReadOnly Property isdeleted As Integer Implements IeZUnAllocatedMail.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property WorkflowId As Integer Implements IeZUnAllocatedMail.WorkflowId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _WorkflowId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _WorkflowId = value Then
                Return
            End If
            _WorkflowId = value
            IsModified = True
        End Set
    End Property

    Public Property JunkMail As Boolean Implements IeZUnAllocatedMail.JunkMail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _JunkMail
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _JunkMail = value Then
                Return
            End If
            _JunkMail = value
            IsModified = True
        End Set
    End Property

    Public Property Workflow As String Implements IeZUnAllocatedMail.Workflow
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Workflow
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Workflow = value Then
                Return
            End If
            _Workflow = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
