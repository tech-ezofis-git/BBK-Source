Imports System.Collections.Generic
Imports System.Text
Public Interface IeZBookMarks
    Inherits IDatabaseItems
    Property BookMarksId() As Integer
    Property BookMarksName() As String
    Property SearchKeyWord() As String
    Property TemplateId() As Integer
    Property IsSavedSearch() As Boolean
    Property IsContenSearch() As Boolean
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    'udaya
    Property folderid() As String
    'Property foldername() As String
    
    ReadOnly Property Isdeleted() As Integer
    ReadOnly Property isfolderdelete() As Integer
    ReadOnly Property IseZBookMarksExist() As Boolean
End Interface
