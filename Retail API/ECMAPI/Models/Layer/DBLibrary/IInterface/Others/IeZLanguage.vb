Imports System.Collections.Generic
Imports System.Text

Public Interface IeZLanguage
    Inherits IDatabaseItems
    Property LanguageId() As Integer
    Property Language() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property IsLanguageExist() As Boolean
End Interface
