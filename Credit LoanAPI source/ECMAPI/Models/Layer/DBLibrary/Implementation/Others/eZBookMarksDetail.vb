
Public Class eZBookMarksDetail
    Inherits IDatabaseCommonItems
    Implements IeZBookMarksDetail
    Protected _BookMarksDetailid As Integer
    Protected _ItemId As Integer
    Protected _BookMarksId As Integer
    Protected _TemplateId As Integer
    Protected _HitCount As String = ""
    Protected _DisplayName As String = ""
    Protected _DirectLink As String = ""
    Protected _Dates As String = ""
    Protected _Size As String = ""
    Protected _Synopsis As String = ""
    Protected _ifiletype As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Friend Sub New(BookMarksDetailid As Integer)
        Me._BookMarksDetailid = BookMarksDetailid
    End Sub

    Public Sub New()
    End Sub
    Public Property BookMarksDetailid() As Integer Implements IeZBookMarksDetail.BookMarksDetailid
        Get
            If _BookMarksDetailid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _BookMarksDetailid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _BookMarksDetailid <> 0 AndAlso _BookMarksDetailid <> value Then
                Throw New MemberAccessException()
            End If
            _BookMarksDetailid = value
        End Set
    End Property
    Public Property BookMarksId() As Integer Implements IeZBookMarksDetail.BookMarksId
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _BookMarksId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _BookMarksId <> 0 AndAlso _BookMarksId <> value Then
                Throw New MemberAccessException()
            End If
            _BookMarksId = value
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZBookMarksDetail.TemplateId
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
    Public Property ItemId() As Integer Implements IeZBookMarksDetail.ItemId
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
    Public Property DisplayName() As String Implements IeZBookMarksDetail.DisplayName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DisplayName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _DisplayName = value Then
                Return
            End If
            _DisplayName = value
            IsModified = True
        End Set
    End Property
    Public Property ifiletype() As String Implements IeZBookMarksDetail.ifiletype
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ifiletype
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ifiletype = value Then
                Return
            End If
            _ifiletype = value
            IsModified = True
        End Set
    End Property
    Public Property HitCount() As String Implements IeZBookMarksDetail.HitCount
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _HitCount
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _HitCount = value Then
                Return
            End If
            _HitCount = value
            IsModified = True
        End Set
    End Property
    Public Property Dates() As String Implements IeZBookMarksDetail.Dates
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Dates
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Dates = value Then
                Return
            End If
            _Dates = value
            IsModified = True
        End Set
    End Property
    Public Property DirectLink() As String Implements IeZBookMarksDetail.DirectLink
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DirectLink
        End Get
        Set(value As String)

            DBLayer.DBLInstance.Read(Me)
            If _DirectLink = value Then
                Return
            End If
            _DirectLink = value
            IsModified = True
        End Set
    End Property
    Public Property Size() As String Implements IeZBookMarksDetail.Size
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Size
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Size = value Then
                Return
            End If
            _Size = value
            IsModified = True
        End Set
    End Property
    Public Property Synopsis() As String Implements IeZBookMarksDetail.Synopsis
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Synopsis
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Synopsis = value Then
                Return
            End If
            _Synopsis = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZBookMarksDetail.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZBookMarksDetail.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZBookMarksDetail.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZBookMarksDetail.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZBookMarksDetail.UpdatedBy
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
        End Set
    End Property
    Public Property UpdatedOn() As String Implements IeZBookMarksDetail.UpdatedOn
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
        End Set
    End Property
    Public ReadOnly Property Isdeleted() As Integer Implements IeZBookMarksDetail.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZBookMarksDetailExist() As Boolean Implements IeZBookMarksDetail.IseZBookMarksDetailExist
        Get
            Return (_BookMarksDetailid > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class