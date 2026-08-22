Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZReminder
    Inherits IDatabaseCommonItems
    Implements IeZReminder
    Protected _ReminderId As Integer
    Protected _StartTime As String
    Protected _EndTime As String
    Protected _Subject As String
    Protected _Reminder As String
    Protected _DefaultId As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Public Sub New(tempReminderTypeId As Integer)
        Me._ReminderId = tempReminderTypeId
    End Sub

    Public Sub New()
    End Sub
    Public Property StartTime() As String Implements IeZReminder.StartTime
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _StartTime
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)

            If _StartTime = value Then
                Return
            End If
            _StartTime = value
            IsModified = True
        End Set
    End Property

    Public Property EndTime() As String Implements IeZReminder.EndTime
        Get

            DBLayer.DBLInstance.Read(Me)
            Return _EndTime
        End Get
        Set(value As String)

            DBLayer.DBLInstance.Read(Me)
            If _EndTime = value Then
                Return
            End If
            _EndTime = value
            IsModified = True
        End Set
    End Property

    Public Property Subject() As String Implements IeZReminder.Subject
        Get

            DBLayer.DBLInstance.Read(Me)

            Return _Subject
        End Get
        Set(value As String)

            DBLayer.DBLInstance.Read(Me)

            If _Subject = value Then
                Return
            End If
            _Subject = value
            IsModified = True
        End Set
    End Property




    Public Property Reminder() As String Implements IeZReminder.Reminder
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Reminder
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Reminder = value Then
                Return
            End If
            _Reminder = value
            IsModified = True
        End Set
    End Property

    Public Property ReminderId() As Integer Implements IeZReminder.ReminderId
        Get
            If _ReminderId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ReminderId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ReminderId <> 0 AndAlso _ReminderId <> value Then
                Throw New MemberAccessException()
            End If
            _ReminderId = value
        End Set
    End Property

  
    Public Property DefaultId() As String Implements IeZReminder.DefaultId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DefaultId
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _DefaultId = value Then
                Return
            End If
            _DefaultId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZReminder.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZReminder.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZReminder.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZReminder.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZReminder.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZReminder.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZReminder.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZReminderExist() As Boolean Implements IeZReminder.IseZReminderExist
        Get
            Return (_ReminderId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class

