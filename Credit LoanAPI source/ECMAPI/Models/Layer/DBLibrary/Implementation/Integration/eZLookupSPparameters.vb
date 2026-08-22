Imports System.Data
Imports System.Configuration
Imports System.Web


Public Class eZLookupSPparameters
    Inherits IDatabaseCommonItems
    Implements IeZLookupSPparameters
    Protected _LookupSPparamId As Integer
    Protected _ECMField As String
    Protected _LookupId As Integer
    Protected _ParameterName As String
    Protected _VariableDataType As String
    Protected _IsOutputParameterDirection As Boolean
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(LookupSPparamId As Integer)
        Me._LookupSPparamId = LookupSPparamId
    End Sub
    Public Sub New()
    End Sub
    Public Property ParameterName() As String Implements IeZLookupSPparameters.ParameterName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ParameterName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ParameterName = value Then
                Return
            End If
            _ParameterName = value
            IsModified = True
        End Set
    End Property
    Public Property VariableDataType() As String Implements IeZLookupSPparameters.VariableDataType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _VariableDataType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _VariableDataType = value Then
                Return
            End If
            _VariableDataType = value
            IsModified = True
        End Set
    End Property
    Public Property IsOutputParameterDirection() As Boolean Implements IeZLookupSPparameters.IsOutputParameterDirection
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsOutputParameterDirection
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsOutputParameterDirection = value Then
                Return
            End If
            _IsOutputParameterDirection = value
            IsModified = True
        End Set
    End Property

    Public Property ECMField() As String Implements IeZLookupSPparameters.ECMField
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _ECMField
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _ECMField = value Then
                Return
            End If
            _ECMField = value
            IsModified = True
        End Set
    End Property


    Public Property LookupId() As Integer Implements IeZLookupSPparameters.LookupId
        Get
            If _LookupId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _LookupId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _LookupId <> 0 AndAlso _LookupId <> value Then
                Throw New MemberAccessException()
            End If
            _LookupId = value
        End Set
    End Property
    Public Property LookupSPparamId() As Integer Implements IeZLookupSPparameters.LookupSPparamId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _LookupSPparamId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _LookupSPparamId = value Then
                Return
            End If
            _LookupSPparamId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZLookupSPparameters.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZLookupSPparameters.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZLookupSPparameters.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZLookupSPparameters.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZLookupSPparameters.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZLookupSPparameters.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZLookupSPparameters.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZLookupSPparameters() As Boolean Implements IeZLookupSPparameters.IseZLookupSPparameters
        Get
            Return (_LookupSPparamId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub




End Class
