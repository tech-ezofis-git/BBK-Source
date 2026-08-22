Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZWorkFlowRelation
    Inherits IDatabaseCommonItems
    Implements IeZWorkFlowRelation
    Protected _RelationId As Integer
    Protected _WorkFlowId As Integer
    Protected _FormId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpRelationId As Integer)
        Me._RelationId = tmpRelationId
    End Sub
    Public Sub New()
    End Sub

    Public Property RelationId() As Integer Implements IeZWorkFlowRelation.RelationId
        Get
            If _RelationId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _RelationId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _RelationId <> 0 AndAlso _RelationId <> value Then
                Throw New MemberAccessException()
            End If
            _RelationId = value
        End Set
    End Property

    Public Property FormId() As Integer Implements IeZWorkFlowRelation.FormId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FormId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _FormId = value Then
                Return
            End If

            _FormId = value
            IsModified = True
        End Set
    End Property

    Public Property WorkFlowId() As Integer Implements IeZWorkFlowRelation.WorkFlowId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _WorkFlowId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _WorkFlowId = value Then
                Return
            End If

            _WorkFlowId = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy1() As String Implements IeZWorkFlowRelation.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZWorkFlowRelation.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZWorkFlowRelation.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZWorkFlowRelation.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZWorkFlowRelation.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZWorkFlowRelation.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZWorkFlowRelation.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
