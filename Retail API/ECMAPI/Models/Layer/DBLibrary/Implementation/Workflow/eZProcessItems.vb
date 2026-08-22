Imports ECMAPI

Public Class eZProcessItems
    Inherits IDatabaseCommonItems
    Implements IeZProcessItems

    Protected _ProcessItemsId As Integer
    Protected _ProcessId As Integer
    Protected _ItemId As Integer
    Protected _Workflowid As Integer
    Protected _TemplateId As Integer
    Protected _FormEntryId As Integer
    Protected _FormId As Integer
    Protected _Createdon As String = ""
    Protected _Updatedon As String = ""
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(processitemsid As Integer)
        Me._ProcessItemsId = processitemsid
    End Sub

    Public Property Createdby() As Integer Implements IeZProcessItems.Createdby
        Get
            If _Createdby = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Createdby
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Createdby <> 0 AndAlso _Createdby <> value Then
                Throw New MemberAccessException()
            End If
            _Createdby = value
        End Set
    End Property

    Public Property Createdby1() As String Implements IeZProcessItems.Createdby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Createdby1 = value Then
                Return
            End If
            _Createdby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon() As String Implements IeZProcessItems.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Createdon = value Then
                Return
            End If

            _Createdon = value
            IsModified = True
        End Set
    End Property

    Public Property FormEntryId() As Integer Implements IeZProcessItems.FormEntryId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FormEntryId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FormEntryId = value Then
                Return
            End If
            _FormEntryId = value
            IsModified = True
        End Set
    End Property

    Public Property FormId() As Integer Implements IeZProcessItems.FormId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FormId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FormId = value Then
                Return
            End If
            _FormId = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted() As Integer Implements IeZProcessItems.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property ItemId() As Integer Implements IeZProcessItems.ItemId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ItemId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ItemId = value Then
                Return
            End If
            _ItemId = value
            IsModified = True
        End Set
    End Property

    Public Property ProcessId() As Integer Implements IeZProcessItems.ProcessId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ProcessId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ProcessId = value Then
                Return
            End If
            _ProcessId = value
            IsModified = True
        End Set
    End Property

    Public Property ProcessItemsId() As Integer Implements IeZProcessItems.ProcessItemsId
        Get
            If _ProcessItemsId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ProcessItemsId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ProcessItemsId <> 0 AndAlso _ProcessItemsId <> value Then
                Throw New MemberAccessException()
            End If
            _ProcessItemsId = value
        End Set
    End Property

    Public Property TemplateId() As Integer Implements IeZProcessItems.TemplateId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateId = value Then
                Return
            End If
            _TemplateId = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby() As Integer Implements IeZProcessItems.Updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedby = value Then
                Return
            End If
            _Updatedby = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby1() As String Implements IeZProcessItems.Updatedby1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedby1 = value Then
                Return
            End If
            _Updatedby1 = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedon() As String Implements IeZProcessItems.Updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Updatedon = value Then
                Return
            End If
            _Updatedon = value
            IsModified = True
        End Set
    End Property

    Public Property Workflowid() As Integer Implements IeZProcessItems.Workflowid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Workflowid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Workflowid = value Then
                Return
            End If
            _Workflowid = value
            IsModified = True
        End Set
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
