Imports System.Collections.Generic
Imports System.Text
Public Interface IeZECMCabinetLevel
    Inherits IDatabaseItems
    Property ECMCabinetLevelId() As Integer
    Property ECMLoginId() As Integer
    Property LoginName() As String
    Property CabinetId() As Integer
    Property Cabinet() As String
    Property TemplateId() As Integer
    Property Template() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    Property Encrypt() As Integer
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IseZECMCabinetLevelExist() As Boolean
End Interface
