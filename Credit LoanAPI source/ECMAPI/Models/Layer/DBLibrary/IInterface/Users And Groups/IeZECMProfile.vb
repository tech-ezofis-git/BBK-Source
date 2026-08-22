Imports System.Collections.Generic
Imports System.Text
Public Interface IeZECMProfile
    Inherits IDatabaseItems
    Property ECMProfileId() As Integer
    Property ECMProfile() As String
    Property Description() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZECMProfileExist() As Boolean
End Interface
