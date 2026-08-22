
Imports System.Collections.Generic
Imports System.Text

Public Interface IeZECMGroupusers
    Inherits IDatabaseItems

    Property ECMGroupUserId() As Integer
    Property ECMGroupId() As Integer
    Property ECMLoginId() As Integer
    Property CreatedOn() As String
    Property UpdatedOn() As String
    Property CreatedBy() As Integer
    Property UpdatedBy() As Integer
    Property Createdby1() As String
    Property updatedby1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
