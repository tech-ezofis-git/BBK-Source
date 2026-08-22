Imports ECMAPI

Public Class eZCollaboration
    Inherits IDatabaseCommonItems
    Implements IeZCollaboration

    Protected _CollId As Integer
    Protected _CollName As String = ""
    Protected _itemid As Integer
    Protected _Templateid As Integer
    Protected _XMLPath As String = ""
    Protected _XMLHistorypath As String = ""
    Protected _Status As String = ""
    Protected _StartDateTime As String = ""
    Protected _EndDateTime As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(CollId As Integer)
        Me._CollId = CollId
    End Sub
    Public Property CollId As Integer Implements IeZCollaboration.CollId
        Get
            If _CollId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _CollId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _CollId <> 0 AndAlso _CollId <> value Then
                Throw New MemberAccessException()
            End If
            _CollId = value
        End Set
    End Property

    Public Property CollName As String Implements IeZCollaboration.CollName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CollName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CollName = value Then
                Return
            End If
            _CollName = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZCollaboration.CreatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy = value Then
                Return
            End If
            _CreatedBy = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy1 As String Implements IeZCollaboration.CreatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedBy1 = value Then
                Return
            End If
            _CreatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedOn As String Implements IeZCollaboration.CreatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CreatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CreatedOn = value Then
                Return
            End If

            _CreatedOn = value
            IsModified = True
        End Set
    End Property

    Public Property EndDateTime As String Implements IeZCollaboration.EndDateTime
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EndDateTime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EndDateTime = value Then
                Return
            End If
            _EndDateTime = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZCollaboration.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property itemid As Integer Implements IeZCollaboration.itemid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _itemid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _itemid = value Then
                Return
            End If
            _itemid = value
            IsModified = True
        End Set
    End Property

    Public Property StartDateTime As String Implements IeZCollaboration.StartDateTime
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _StartDateTime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _StartDateTime = value Then
                Return
            End If
            _StartDateTime = value
            IsModified = True
        End Set
    End Property

    Public Property Status As String Implements IeZCollaboration.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property Templateid As Integer Implements IeZCollaboration.Templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Templateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Templateid = value Then
                Return
            End If
            _Templateid = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZCollaboration.UpdatedBy
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy = value Then
                Return
            End If
            _UpdatedBy = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1 As String Implements IeZCollaboration.UpdatedBy1
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedBy1
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedBy1 = value Then
                Return
            End If
            _UpdatedBy1 = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedOn As String Implements IeZCollaboration.UpdatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _UpdatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _UpdatedOn = value Then
                Return
            End If
            _UpdatedOn = value
            IsModified = True
        End Set
    End Property

    Public Property XMLHistorypath As String Implements IeZCollaboration.XMLHistorypath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _XMLHistorypath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _XMLHistorypath = value Then
                Return
            End If
            _XMLHistorypath = value
            IsModified = True
        End Set
    End Property

    Public Property XMLPath As String Implements IeZCollaboration.XMLPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _XMLPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _XMLPath = value Then
                Return
            End If
            _XMLPath = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
