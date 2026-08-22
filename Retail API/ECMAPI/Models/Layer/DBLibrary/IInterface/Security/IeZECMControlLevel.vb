Imports System.Collections.Generic
Imports System.Text
Public Interface IeZECMControlLevel
    Inherits IDatabaseItems
    Property ECMControlLevelId() As Integer
    Property ECMControlId() As Integer
    Property templatename() As String
    Property templateid() As Integer
    Property ECMProfileId As Integer
    Property ECMControl() As String
    Property ECMControlType() As Integer
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZECMControlLevelExist() As Boolean
End Interface
