Public Interface ISyncTable
    Inherits IDatabaseItems

    Property Syncid() As Integer
    Property Syncname() As String
    Property FromERS() As Integer
    Property ToERS() As Integer
    Property Sync() As String
    Property Syncdate() As String
    Property Synctime() As String
    Property Syncschedule() As Integer
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property CreatedBy1() As String
    Property UpdatedBy1() As String
    ReadOnly Property isdeleted() As Integer

End Interface
