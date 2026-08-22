Imports System.Data
Imports System.Configuration
Imports System.Web
''' <summary>
''' Summary description for TempDatatypeGroup
''' </summary>
Public Class eZFormDetails
    Inherits IDatabaseCommonItems
    Implements IeZFormDetails
    Protected _FormId As Integer
    Protected _FormName As String
    Protected _Status As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _FormTableName As String = ""
    Protected _CUserName As String
    Protected _CUserCode As String
    Protected _UUserName As String
    Protected _UUserCode As String
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpFormId As Integer)
        Me._FormId = tmpFormId
    End Sub
    Public Sub New(tmpTempDatatype As String)
        Me._FormName = tmpTempDatatype
    End Sub

    Public Sub New()
    End Sub
    Public Property FormId() As Integer Implements IeZFormDetails.FormId
        Get
            If _FormId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FormId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FormId <> 0 AndAlso _FormId <> value Then
                Throw New MemberAccessException()
            End If
            _FormId = value
        End Set
    End Property

    Public Property FormName() As String Implements IeZFormDetails.FormName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FormName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FormName = value Then
                Return
            End If
            _FormName = value
            IsModified = True
        End Set
    End Property
    Public Property Status() As String Implements IeZFormDetails.Status
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Status
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Status = value Then
                Return
            End If
            _Status = value
            IsModified = True
        End Set
    End Property
    Public Property FormTableName() As String Implements IeZFormDetails.FormTableName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FormTableName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FormTableName = value Then
                Return
            End If
            _FormTableName = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZFormDetails.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZFormDetails.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZFormDetails.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZFormDetails.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZFormDetails.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZFormDetails.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZFormDetails.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IseZFormDetailsExist() As Boolean Implements IeZFormDetails.IseZFormDetailsExist
        Get
            Return (FormId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
