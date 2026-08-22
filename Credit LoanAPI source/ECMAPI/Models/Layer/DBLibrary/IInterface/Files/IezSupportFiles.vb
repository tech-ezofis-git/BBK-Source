Public Interface IezSupportFiles
    Inherits IDatabaseItems

    Property Attachmentid() As Integer
    Property ersid() As Integer
    Property itemid() As Integer
    Property templateid() As Integer
    Property ifilepath() As String
    Property ifiletype() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property Isdeleted() As Integer

End Interface
