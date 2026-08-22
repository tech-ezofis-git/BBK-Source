Imports ECMAPI

Public Class eZOutlookDetail
    Inherits IDatabaseCommonItems
    Implements IeZOutlookDetail

    Protected _Outlookdetailid As Integer
    Protected _ConversationIndex As String = ""
    Protected _EntryId As String = ""
    Protected _itemid As Integer
    Protected _templateid As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer
    Public Sub New()
    End Sub
    Public Sub New(Outlookdetailid As Integer)
        Me._Outlookdetailid = Outlookdetailid
    End Sub
    Public Property ConversationIndex As String Implements IeZOutlookDetail.ConversationIndex
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ConversationIndex
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ConversationIndex = value Then
                Return
            End If
            _ConversationIndex = value
            IsModified = True
        End Set
    End Property

    Public Property CreatedBy As Integer Implements IeZOutlookDetail.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZOutlookDetail.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZOutlookDetail.CreatedOn
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

    Public Property EntryId As String Implements IeZOutlookDetail.EntryId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EntryId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EntryId = value Then
                Return
            End If
            _EntryId = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZOutlookDetail.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property itemid As Integer Implements IeZOutlookDetail.itemid
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

    Public Property Outlookdetailid As Integer Implements IeZOutlookDetail.Outlookdetailid
        Get
            If _Outlookdetailid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Outlookdetailid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Outlookdetailid <> 0 AndAlso _Outlookdetailid <> value Then
                Throw New MemberAccessException()
            End If
            _Outlookdetailid = value
        End Set
    End Property

    Public Property templateid As Integer Implements IeZOutlookDetail.templateid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _templateid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _templateid = value Then
                Return
            End If
            _templateid = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZOutlookDetail.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZOutlookDetail.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZOutlookDetail.UpdatedOn
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

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
