Imports ECMAPI

Public Class eZFieldAlertDoc
    Inherits IDatabaseCommonItems
    Implements IeZFieldAlertDoc

    Protected _FieldAlertDocId As Integer
    Protected _FieldAlertDetailId As Integer
    Protected _ToMail As String = ""
    Protected _Filename As String = ""
    Protected _TemplateId As Integer
    Protected _itemid As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(FieldAlertDocId As Integer)
        Me._FieldAlertDocId = FieldAlertDocId
    End Sub
    Public Property CreatedBy As Integer Implements IeZFieldAlertDoc.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZFieldAlertDoc.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZFieldAlertDoc.CreatedOn
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

    Public Property FieldAlertDetailId As Integer Implements IeZFieldAlertDoc.FieldAlertDetailId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FieldAlertDetailId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FieldAlertDetailId = value Then
                Return
            End If
            _FieldAlertDetailId = value
            IsModified = True
        End Set
    End Property

    Public Property FieldAlertDocId As Integer Implements IeZFieldAlertDoc.FieldAlertDocId
        Get
            If _FieldAlertDocId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FieldAlertDocId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FieldAlertDocId <> 0 AndAlso _FieldAlertDocId <> value Then
                Throw New MemberAccessException()
            End If
            _FieldAlertDocId = value
        End Set
    End Property

    Public Property Filename As String Implements IeZFieldAlertDoc.Filename
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Filename
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Filename = value Then
                Return
            End If
            _Filename = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZFieldAlertDoc.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property itemid As Integer Implements IeZFieldAlertDoc.itemid
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

    Public Property TemplateId As Integer Implements IeZFieldAlertDoc.TemplateId
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

    Public Property ToMail As String Implements IeZFieldAlertDoc.ToMail
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ToMail
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ToMail = value Then
                Return
            End If
            _ToMail = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZFieldAlertDoc.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZFieldAlertDoc.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZFieldAlertDoc.UpdatedOn
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
