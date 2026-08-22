Public Interface IeZFormUsers
    Inherits IDatabaseItems
    Property FormUsersId() As Integer
    Property FormId() As Integer
    Property ECMLoginId() As Integer
    Property Createdon() As String
    Property Updatedon() As String
    Property Createdby() As Integer
    Property Updatedby() As Integer
    Property Createdby1() As String
    Property Updatedby1() As String
    ReadOnly Property isdeleted() As Integer
End Interface
