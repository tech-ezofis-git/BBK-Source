Imports System.Collections.Generic
Imports System.Text

Public Interface IOldeZECMLogin
    Inherits IDatabaseItems

    Property ECMLoginId() As Integer

    Property IsFaxUser() As Boolean
    Property IsADUser() As Boolean
    Property LoginName() As String
    'Property ECMGroup() As String
    Property Pasword() As String
    Property Signatureid() As String
    Property ECMProfileId() As Integer

    Property ECMGroupList() As String
    Property Chart1() As Integer
    Property Chart2() As Integer
    Property Chart3() As Integer
    Property LanguageId() As Integer
    Property ECMUserTypeId() As Integer
    Property ECMProfile() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsECMLoginExist() As Boolean

End Interface
