Imports System.Data
Imports System.Configuration
Imports System.Web

Public Class eZWorkFlowProcess
    Inherits IDatabaseCommonItems
    Implements IeZWorkFlowProcess
    Protected _ProcessId As Integer
    Protected _WorkFlowId As Integer
    Protected _Stage As String = ""
    Protected _InitiatedOn As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpProcessId As Integer)
        Me._ProcessId = tmpProcessId
    End Sub
    Public Sub New()
    End Sub

    Public Property ProcessId() As Integer Implements IeZWorkFlowProcess.ProcessId
        Get
            If _ProcessId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _ProcessId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _ProcessId <> 0 AndAlso _ProcessId <> value Then
                Throw New MemberAccessException()
            End If
            _ProcessId = value
        End Set
    End Property
    Public Property Stage() As String Implements IeZWorkFlowProcess.Stage
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Stage
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Stage = value Then
                Return
            End If

            _Stage = value
            IsModified = True
        End Set
    End Property
    Public Property InitiatedOn() As String Implements IeZWorkFlowProcess.InitiatedOn
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _InitiatedOn
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _InitiatedOn = value Then
                Return
            End If

            _InitiatedOn = value
            IsModified = True
        End Set
    End Property
    Public Property WorkFlowId() As Integer Implements IeZWorkFlowProcess.WorkFlowId
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

    Public Property UpdatedBy1() As String Implements IeZWorkFlowProcess.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZWorkFlowProcess.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZWorkFlowProcess.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZWorkFlowProcess.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZWorkFlowProcess.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZWorkFlowProcess.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZWorkFlowProcess.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
