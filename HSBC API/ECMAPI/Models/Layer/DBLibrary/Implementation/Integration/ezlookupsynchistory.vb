Imports ECMAPI

Public Class ezlookupsynchistory
    Inherits IDatabaseCommonItems
    Implements Iezlookupsynchistory

    Protected _synchistoryid As Integer
    Protected _lookupid As Integer
    Protected _query As String = ""
    Protected _application As String = ""
    Protected _ecmloginid As Integer
    Protected _result As String = ""
    Protected _Createdon As String
    Protected _Updatedon As String
    Protected _Createdby As Integer
    Protected _Updatedby As Integer
    Protected _Createdby1 As String = ""
    Protected _Updatedby1 As String = ""
    Protected _Loginname As String = ""
    Private _isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(synchistoryid As Integer)
        Me._synchistoryid = synchistoryid
    End Sub

    Public Property synchistoryid As Integer Implements Iezlookupsynchistory.synchistoryid
        Get
            If _synchistoryid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _synchistoryid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _synchistoryid <> 0 AndAlso _synchistoryid <> value Then
                Throw New MemberAccessException()
            End If
            _synchistoryid = value
        End Set
    End Property

    Public Property lookupid As Integer Implements Iezlookupsynchistory.lookupid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _lookupid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _lookupid = value Then
                Return
            End If
            _lookupid = value
            IsModified = True
        End Set
    End Property

    Public Property query As String Implements Iezlookupsynchistory.query
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _query
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _query = value Then
                Return
            End If
            _query = value
            IsModified = True
        End Set
    End Property

    Public Property application As String Implements Iezlookupsynchistory.application
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _application
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _application = value Then
                Return
            End If
            _application = value
            IsModified = True
        End Set
    End Property

    Public Property ecmloginid As Integer Implements Iezlookupsynchistory.ecmloginid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ecmloginid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ecmloginid = value Then
                Return
            End If
            _ecmloginid = value
            IsModified = True
        End Set
    End Property

    Public Property result As String Implements Iezlookupsynchistory.result
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _result
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _result = value Then
                Return
            End If
            _result = value
            IsModified = True
        End Set
    End Property

    Public Property Createdon As String Implements Iezlookupsynchistory.Createdon
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

    Public Property Updatedon As String Implements Iezlookupsynchistory.Updatedon
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

    Public Property Createdby As Integer Implements Iezlookupsynchistory.Createdby
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

    Public Property Updatedby As Integer Implements Iezlookupsynchistory.Updatedby
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

    Public Property CreatedBy1 As String Implements Iezlookupsynchistory.CreatedBy1
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

    Public Property UpdatedBy1 As String Implements Iezlookupsynchistory.UpdatedBy1
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

    Public ReadOnly Property isdeleted As Integer Implements Iezlookupsynchistory.isdeleted
        Get
            Return _isdeleted
        End Get
    End Property

    Public Property Loginname As String Implements Iezlookupsynchistory.Loginname
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Loginname
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Loginname = value Then
                Return
            End If
            _Loginname = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
