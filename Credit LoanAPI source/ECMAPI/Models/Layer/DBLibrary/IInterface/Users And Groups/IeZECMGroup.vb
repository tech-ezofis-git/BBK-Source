Imports System.Collections.Generic
Imports System.Text
Public Interface IeZECMGroup
    Inherits IDatabaseItems
    Property ECMGroupId() As Integer
    Property ECMGroup() As String
    Property Description() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZECMGroupExist() As Boolean
End Interface
