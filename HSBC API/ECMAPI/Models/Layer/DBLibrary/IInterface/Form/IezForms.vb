Imports System.Collections.Generic
Imports System.Text

Public Interface IezForms
    Inherits IDatabaseItems
    Property DynamicProperty() As Dictionary(Of String, String)
End Interface
