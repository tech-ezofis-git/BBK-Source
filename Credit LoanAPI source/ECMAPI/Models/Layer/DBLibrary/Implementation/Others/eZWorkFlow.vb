Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZWorkFlow
    Inherits IDatabaseCommonItems
    Implements IeZWorkFlow
    Protected _WorkFlowId As Integer
    Protected _WorkFlowName As String
    Protected _WorkFlowPath As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpWorkFlowId As Integer)
        Me._WorkFlowId = tmpWorkFlowId
    End Sub
    Public Sub New()
    End Sub

    Public Property WorkFlowId() As Integer Implements IeZWorkFlow.WorkFlowId
        Get
            If _WorkFlowId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _WorkFlowId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _WorkFlowId <> 0 AndAlso _WorkFlowId <> value Then
                Throw New MemberAccessException()
            End If
            _WorkFlowId = value
        End Set
    End Property

    Public Property WorkFlowName() As String Implements IeZWorkFlow.WorkFlowName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _WorkFlowName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _WorkFlowName = value Then
                Return
            End If
            _WorkFlowName = value
            IsModified = True
        End Set
    End Property

    Public Property WorkFlowPath() As String Implements IeZWorkFlow.WorkFlowPath
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _WorkFlowPath
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _WorkFlowPath = value Then
                Return
            End If
            _WorkFlowPath = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZWorkFlow.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZWorkFlow.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZWorkFlow.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZWorkFlow.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZWorkFlow.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZWorkFlow.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZWorkFlow.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
