Public Interface Iezlookupsynchistory
    Inherits IDatabaseItems

    Property synchistoryid() As Integer
    Property lookupid() As Integer
    Property query() As String
    Property application() As String
    Property ecmloginid() As Integer
    Property result() As String
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    Property Loginname() As String
    ReadOnly Property isdeleted() As Integer

End Interface
