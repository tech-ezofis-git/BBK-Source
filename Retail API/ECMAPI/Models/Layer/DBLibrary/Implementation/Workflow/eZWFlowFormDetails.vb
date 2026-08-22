Imports ECMAPI

Public Class eZWFlowFormDetails
    Inherits IDatabaseCommonItems
    Implements IeZWFlowFormDetails

    Protected _FormDetailsId As Integer
    Protected _formid As Integer
    Protected _parentformid As Integer
    Protected _workflowid As Integer
    Protected _tablename As String = ""
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String = ""
    Protected _UpdatedBy1 As String = ""
    Private _Isdeleted As Integer

    Public Sub New()
    End Sub
    Public Sub New(FormDetailsId As Integer)
        Me._FormDetailsId = FormDetailsId
    End Sub

    Public Property CreatedBy As Integer Implements IeZWFlowFormDetails.CreatedBy
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

    Public Property CreatedBy1 As String Implements IeZWFlowFormDetails.CreatedBy1
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

    Public Property CreatedOn As String Implements IeZWFlowFormDetails.CreatedOn
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

    Public Property FormDetailsId As Integer Implements IeZWFlowFormDetails.FormDetailsId
        Get
            If _FormDetailsId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FormDetailsId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FormDetailsId <> 0 AndAlso _FormDetailsId <> value Then
                Throw New MemberAccessException()
            End If
            _FormDetailsId = value
        End Set
    End Property

    Public Property formid As Integer Implements IeZWFlowFormDetails.formid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _formid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _formid = value Then
                Return
            End If
            _formid = value
            IsModified = True
        End Set
    End Property

    Public ReadOnly Property Isdeleted As Integer Implements IeZWFlowFormDetails.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property

    Public Property parentformid As Integer Implements IeZWFlowFormDetails.parentformid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _parentformid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _parentformid = value Then
                Return
            End If
            _parentformid = value
            IsModified = True
        End Set
    End Property

    Public Property tablename As String Implements IeZWFlowFormDetails.tablename
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _tablename
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _tablename = value Then
                Return
            End If
            _tablename = value
            IsModified = True
        End Set
    End Property

    Public Property UpdatedBy As Integer Implements IeZWFlowFormDetails.UpdatedBy
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

    Public Property UpdatedBy1 As String Implements IeZWFlowFormDetails.UpdatedBy1
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

    Public Property UpdatedOn As String Implements IeZWFlowFormDetails.UpdatedOn
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

    Public Property workflowid As Integer Implements IeZWFlowFormDetails.workflowid
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _workflowid
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _workflowid = value Then
                Return
            End If
            _workflowid = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
