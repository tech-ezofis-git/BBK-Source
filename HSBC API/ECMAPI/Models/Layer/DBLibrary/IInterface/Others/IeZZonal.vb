
Public Interface IeZZonal
    Inherits IDatabaseItems
    Property ZonalId() As Integer
    Property CabinetId() As Integer
    Property TemplateId() As Integer
    Property ZonalName() As String
    Property CabinetName() As String
    Property TemplateName() As String
    Property ProcessName() As String
    Property CreatedFrom() As String
    Property CreatedBy() As Integer
    Property CreatedOn() As String
    Property UpdatedBy() As Integer
    Property UpdatedOn() As String
    ReadOnly Property Isdeleted() As Integer
End Interface
