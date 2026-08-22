Imports ECMAPI

Public Class SyncTable
    Inherits IDatabaseCommonItems
    Implements ISyncTable

    Protected _Syncid As Integer
    Protected _Syncname As String = ""
    Protected _FromERS As Integer
    Protected _ToERS As Integer
    Protected _Sync As String = ""
    Protected _Syncdate As String = ""
    Protected _Synctime As String = ""
    Protected _Syncschedule As Integer
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Private _isdeleted As Integer


    Public Sub New()
    End Sub
    Public Sub New(Syncid As Integer)
        Me._Syncid = Syncid
    End Sub

    Public Property Createdby As Integer Implements ISyncTable.Createdby
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Createdby
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Createdby = value Then
                Return
            End If
            _Createdby = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy1 As String Implements ISyncTable.CreatedBy1
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

    Public Property Createdon As String Implements ISyncTable.Createdon
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

    Public Property FromERS As Integer Implements ISyncTable.FromERS
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FromERS
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FromERS = value Then
                Return
            End If
            _FromERS = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property isdeleted As Integer Implements ISyncTable.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property Sync As String Implements ISyncTable.Sync
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Sync
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Sync = value Then
                Return
            End If
            _Sync = value
            IsModified = True
        End Set
    End Property

    Public Property Syncdate As String Implements ISyncTable.Syncdate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Syncdate
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Syncdate = value Then
                Return
            End If
            _Syncdate = value
            IsModified = True
        End Set
    End Property

    Public Property Syncid As Integer Implements ISyncTable.Syncid
        Get
            If _Syncid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Syncid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Syncid <> 0 AndAlso _Syncid <> value Then
                Throw New MemberAccessException()
            End If
            _Syncid = value
        End Set
    End Property

    Public Property Syncname As String Implements ISyncTable.Syncname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Syncname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Syncname = value Then
                Return
            End If
            _Syncname = value
            IsModified = True
        End Set
    End Property

    Public Property Syncschedule As Integer Implements ISyncTable.Syncschedule
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Syncschedule
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Syncschedule = value Then
                Return
            End If
            _Syncschedule = value
            IsModified = True
        End Set
    End Property

    Public Property Synctime As String Implements ISyncTable.Synctime
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Synctime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Synctime = value Then
                Return
            End If
            _Synctime = value
            IsModified = True
        End Set
    End Property

    Public Property ToERS As Integer Implements ISyncTable.ToERS
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToERS
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ToERS = value Then
                Return
            End If
            _ToERS = value
            IsModified = True
        End Set
    End Property

    Public Property Updatedby As Integer Implements ISyncTable.Updatedby
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

    Public Property UpdatedBy1 As String Implements ISyncTable.UpdatedBy1
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

    Public Property Updatedon As String Implements ISyncTable.Updatedon
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

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
