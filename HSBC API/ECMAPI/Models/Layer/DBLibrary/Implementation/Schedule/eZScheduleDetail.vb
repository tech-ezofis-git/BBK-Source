Imports ECMAPI

Public Class eZScheduleDetail
    Inherits IDatabaseCommonItems
    Implements IeZScheduleDetail

    Protected _Detailid As Integer
    Protected _Id As Integer
    Protected _ForSchedule As Integer
    Protected _ScheduleId As Integer
    Protected _ScheduleDate As String = ""
    Protected _Status As Boolean
    Protected _Result As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(Detailid As Integer)
        Me._Detailid = Detailid
    End Sub

    Public Property CreatedBy As Integer Implements IeZScheduleDetail.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZScheduleDetail.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZScheduleDetail.CreatedOn
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

    Public Property Detailid As Integer Implements IeZScheduleDetail.Detailid
        Get
            If _Detailid = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Detailid
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Detailid <> 0 AndAlso _Detailid <> value Then
                Throw New MemberAccessException()
            End If
            _Detailid = value
        End Set
    End Property

    Public Property ForSchedule As Integer Implements IeZScheduleDetail.ForSchedule
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ForSchedule
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ForSchedule = value Then
                Return
            End If
            _ForSchedule = value
            IsModified = True
        End Set
    End Property

    Public Property Id As Integer Implements IeZScheduleDetail.Id
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Id
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _Id = value Then
                Return
            End If
            _Id = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZScheduleDetail.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property Result As String Implements IeZScheduleDetail.Result
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Result
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Result = value Then
                Return
            End If
            _Result = value
            IsModified = True
        End Set
    End Property

    Public Property ScheduleDate As String Implements IeZScheduleDetail.ScheduleDate
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ScheduleDate
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ScheduleDate = value Then
                Return
            End If
            _ScheduleDate = value
            IsModified = True
        End Set
    End Property

    Public Property ScheduleId As Integer Implements IeZScheduleDetail.ScheduleId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ScheduleId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ScheduleId = value Then
                Return
            End If
            _ScheduleId = value
            IsModified = True
        End Set
    End Property

    Public Property Status As Boolean Implements IeZScheduleDetail.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZScheduleDetail.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZScheduleDetail.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZScheduleDetail.UpdatedOn
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
