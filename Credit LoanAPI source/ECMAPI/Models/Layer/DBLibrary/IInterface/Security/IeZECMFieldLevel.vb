Imports System.Collections.Generic
Imports System.Text
Public Interface IeZECMFieldLevel
    Inherits IDatabaseItems
    Property ECMFieldLevelId() As Integer
    Property ECMLoginId() As Integer
    Property LoginName() As String
    Property FieldId() As Integer
    Property FieldValue() As String
    Property Visibility() As Integer
    Property TemplateId() As Integer
    Property ConditionId() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    Property ECMGroupId() As Integer
    ReadOnly Property Isdeleted() As Integer
End Interface
