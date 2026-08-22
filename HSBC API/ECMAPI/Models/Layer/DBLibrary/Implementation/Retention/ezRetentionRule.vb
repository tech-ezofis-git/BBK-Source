Imports ECMAPI

Public Class ezRetentionRule
    Inherits IDatabaseCommonItems
    Implements IezRetentionRule

    Protected _RetentionId As Integer
    Protected _RuleName As String = ""
    Protected _RetentionType As Integer
    Protected _TemplateId As Integer
    Protected _RetentionRule As String = ""
    Protected _RetentionRuleJSON As String = ""
    Protected _RetentionField As Integer
    Protected _Period As Integer
    Protected _PeriodType As String = ""
    Protected _NotifyMail As String = ""
    Protected _RemainderDays As Integer
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(RetentionId As Integer)
        Me._RetentionId = RetentionId
    End Sub

    Public Property RetentionId As Integer Implements IezRetentionRule.RetentionId
        Get
            If _RetentionId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _RetentionId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _RetentionId <> 0 AndAlso _RetentionId <> value Then
                Throw New MemberAccessException()
            End If
            _RetentionId = value
        End Set
    End Property

    Public Property RuleName As String Implements IezRetentionRule.RuleName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _RuleName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _RuleName = value Then
                Return
            End If
            _RuleName = value
            IsModified = True
        End Set
    End Property

    Public Property RetentionType As Integer Implements IezRetentionRule.RetentionType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _RetentionType
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _RetentionType = value Then
                Return
            End If
            _RetentionType = value
            IsModified = True
        End Set
    End Property

    Public Property TemplateId As Integer Implements IezRetentionRule.TemplateId
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

    Public Property RetentionRule As String Implements IezRetentionRule.RetentionRule
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _RetentionRule
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _RetentionRule = value Then
                Return
            End If
            _RetentionRule = value
            IsModified = True
        End Set
    End Property

    Public Property RetentionField As Integer Implements IezRetentionRule.RetentionField
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _RetentionField
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _RetentionField = value Then
                Return
            End If
            _RetentionField = value
            IsModified = True
        End Set
    End Property

    Public Property Period As Integer Implements IezRetentionRule.Period
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Period
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Period = value Then
                Return
            End If
            _Period = value
            IsModified = True
        End Set
    End Property

    Public Property PeriodType As String Implements IezRetentionRule.PeriodType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _PeriodType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _PeriodType = value Then
                Return
            End If
            _PeriodType = value
            IsModified = True
        End Set
    End Property

    Public Property NotifyMail As String Implements IezRetentionRule.NotifyMail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _NotifyMail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _NotifyMail = value Then
                Return
            End If
            _NotifyMail = value
            IsModified = True
        End Set
    End Property

    Public Property RemainderDays As Integer Implements IezRetentionRule.RemainderDays
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _RemainderDays
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _RemainderDays = value Then
                Return
            End If
            _RemainderDays = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements IezRetentionRule.Createdon
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

    Public Property Updatedon As String Implements IezRetentionRule.Updatedon
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

    Public Property Createdby As Integer Implements IezRetentionRule.Createdby
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

    Public Property Updatedby As Integer Implements IezRetentionRule.Updatedby
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

    Public Property CreatedBy1 As String Implements IezRetentionRule.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements IezRetentionRule.UpdatedBy1
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

    Public ReadOnly Property isdeleted As Integer Implements IezRetentionRule.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property RetentionRuleJSON As String Implements IezRetentionRule.RetentionRuleJSON
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _RetentionRuleJSON
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _RetentionRuleJSON = value Then
                Return
            End If
            _RetentionRuleJSON = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
