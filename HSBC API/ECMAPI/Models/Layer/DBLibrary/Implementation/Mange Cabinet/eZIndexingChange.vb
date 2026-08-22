Imports ECMAPI

Public Class eZIndexingChange
    Inherits IDatabaseCommonItems
    Implements IeZIndexingChange

    Protected IM_Indexingchangeid As Integer
    Protected IM_Templateid As Integer
    Protected IM_Nodeid As Integer
    Protected IM_Parentid As Integer
    Protected IM_del As Integer
    Protected IM_Levelid As Integer
    Protected IM_oldvalue As String = ""
    Protected IM_Newvalue As String = ""
    Protected IM_Fieldid As Integer
    Protected IM_itemid As Integer
    Protected IM_Createdon As String = ""
    Protected IM_Updatedon As String = ""
    Protected IM_Createdby1 As String = ""
    Protected IM_Updatedby1 As String = ""
    Protected IM_Createdby As Integer
    Protected IM_Updatedby As Integer
    Private IM_isdeleted As Integer

    Public Sub New(Indexingchangeid As Integer)
        Me.IM_Indexingchangeid = Indexingchangeid
    End Sub
    Public Sub New()

    End Sub

    Public Property Fieldid As Integer Implements IeZIndexingChange.Fieldid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Fieldid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Fieldid = value Then
                Return
            End If
            IM_Fieldid = value
            IsModified = True
        End Set
    End Property

    Public Property Indexingchangeid As Integer Implements IeZIndexingChange.Indexingchangeid
        Get
            If IM_Indexingchangeid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return IM_Indexingchangeid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If IM_Indexingchangeid <> 0 AndAlso IM_Indexingchangeid <> value Then
                Throw New MemberAccessException()
            End If
            IM_Indexingchangeid = value
        End Set
    End Property

    Public Property itemid As Integer Implements IeZIndexingChange.itemid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Indexingchangeid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Indexingchangeid = value Then
                Return
            End If
            IM_Indexingchangeid = value
            IsModified = True
        End Set
    End Property

    Public Property Newvalue As String Implements IeZIndexingChange.Newvalue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Newvalue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Newvalue = value Then
                Return
            End If
            IM_Newvalue = value
            IsModified = True
        End Set
    End Property


    Public Property Nodeid As Integer Implements IeZIndexingChange.Nodeid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Nodeid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Nodeid = value Then
                Return
            End If
            IM_Nodeid = value
            IsModified = True
        End Set
    End Property

    Public Property oldvalue As String Implements IeZIndexingChange.oldvalue
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_oldvalue
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_oldvalue = value Then
                Return
            End If
            IM_oldvalue = value
            IsModified = True
        End Set
    End Property

    Public Property Templateid As Integer Implements IeZIndexingChange.Templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Templateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Templateid = value Then
                Return
            End If
            IM_Templateid = value
            IsModified = True
        End Set
    End Property
    Public Property Createdon As String Implements IeZIndexingChange.Createdon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Createdon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Createdon = value Then
                Return
            End If
            IM_Createdon = value
            IsModified = True
        End Set
    End Property
    Public Property Updatedon As String Implements IeZIndexingChange.Updatedon
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Updatedon
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Updatedon = value Then
                Return
            End If
            IM_Updatedon = value
            IsModified = True
        End Set
    End Property
    Public Property Createdby As Integer Implements IeZIndexingChange.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Createdby = value Then
                Return
            End If
            IM_Createdby = value
            IsModified = True
        End Set
    End Property
    Public Property Updatedby As Integer Implements IeZIndexingChange.Updatedby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Updatedby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Updatedby = value Then
                Return
            End If
            IM_Updatedby = value
            IsModified = True
        End Set
    End Property
    Public ReadOnly Property Isdeleted As Integer Implements IeZIndexingChange.isdeleted
        Get
            Return IM_isdeleted
        End Get
    End Property

    Public Property Parentid As Integer Implements IeZIndexingChange.Parentid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Parentid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Parentid = value Then
                Return
            End If
            IM_Parentid = value
            IsModified = True
        End Set
    End Property

    Public Property del As Integer Implements IeZIndexingChange.del
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_del
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_del = value Then
                Return
            End If
            IM_del = value
            IsModified = True
        End Set
    End Property

    Public Property Levelid As Integer Implements IeZIndexingChange.Levelid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Levelid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If IM_Levelid = value Then
                Return
            End If
            IM_Levelid = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZIndexingChange.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Createdby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Createdby1 = value Then
                Return
            End If
            IM_Createdby1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZIndexingChange.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return IM_Updatedby1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If IM_Updatedby1 = value Then
                Return
            End If
            IM_Updatedby1 = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
