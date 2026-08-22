Public Interface IezRetentionRule
    Inherits IDatabaseItems

    Property RetentionId() As Integer
    Property RuleName() As String
    Property RetentionType() As Integer
    Property TemplateId() As Integer
    Property RetentionRule() As String
    Property RetentionRuleJSON() As String
    Property RetentionField() As Integer
    Property Period() As Integer
    Property PeriodType() As String
    Property NotifyMail() As String
    Property RemainderDays() As Integer
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
