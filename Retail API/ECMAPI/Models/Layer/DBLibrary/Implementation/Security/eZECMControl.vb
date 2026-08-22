Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZECMControl
    Inherits IDatabaseCommonItems
    Implements IeZECMControl

    Protected _ECMControlId As Integer
    Protected _ECMControlType As Integer
    Protected _ECMControl As String
    Protected _Description As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._ECMControlId = DeptId
    End Sub
    Public Sub New(ECMControlName As String)
        Me._ECMControl = ECMControlName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property ECMControlId() As Integer Implements IeZECMControl.ECMControlId
        Get
            If _ECMControlId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMControlId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMControlId <> 0 AndAlso _ECMControlId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMControlId = value
        End Set
    End Property
    Public Property ECMControl() As String Implements IeZECMControl.ECMControl
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMControl
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMControl = value Then
                Return
            End If
            _ECMControl = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZECMControl.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZECMControl.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZECMControl.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZECMControl.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZECMControl.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZECMControl.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZECMControl.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZECMControltExist() As Boolean Implements IeZECMControl.IseZECMControlExist
        Get
            Return (_ECMControlId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

    Public Property ECMControlType As Integer Implements IeZECMControl.ECMControlType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMControlType
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _ECMControlType = value Then
                Return
            End If

            _ECMControlType = value
            IsModified = True
        End Set
    End Property
End Class
