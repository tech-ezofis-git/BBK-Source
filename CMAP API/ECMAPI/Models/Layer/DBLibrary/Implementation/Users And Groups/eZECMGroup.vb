Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZECMGroup
    Inherits IDatabaseCommonItems
    Implements IeZECMGroup
    Protected _ECMGroupId As Integer
    Protected _ECMGroup As String
    Protected _Description As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._ECMGroupId = DeptId
    End Sub
    Public Sub New(ECMGroupName As String)
        Me._ECMGroup = ECMGroupName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property ECMGroupId() As Integer Implements IeZECMGroup.ECMGroupId
        Get
            If _ECMGroupId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ECMGroupId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ECMGroupId <> 0 AndAlso _ECMGroupId <> value Then
                Throw New MemberAccessException()
            End If
            _ECMGroupId = value
        End Set
    End Property
    Public Property ECMGroup() As String Implements IeZECMGroup.ECMGroup
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMGroup
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMGroup = value Then
                Return
            End If
            _ECMGroup = value
            IsModified = True
        End Set
    End Property
    Public Property Description() As String Implements IeZECMGroup.Description
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Description
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Description = value Then
                Return
            End If
            _Description = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZECMGroup.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZECMGroup.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZECMGroup.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZECMGroup.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZECMGroup.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZECMGroup.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZECMGroup.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZECMGrouptExist() As Boolean Implements IeZECMGroup.IseZECMGroupExist
        Get
            Return (_ECMGroupId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
