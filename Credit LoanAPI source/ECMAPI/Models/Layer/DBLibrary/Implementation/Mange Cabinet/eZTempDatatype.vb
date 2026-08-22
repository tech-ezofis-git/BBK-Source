Imports System.Data
Imports System.Configuration
Imports System.Web
''' <summary>
''' Summary description for TempDatatypeGroup
''' </summary>
Public Class eZTempDatatype
    Inherits IDatabaseCommonItems
    Implements IeZTempDatatype
    Protected _TempDatatypeId As Integer
    Protected _TempDatatype As String
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CUserName As String
    Protected _CUserCode As String
    Protected _UUserName As String
    Protected _UUserCode As String
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer

    Public Sub New(tmpTempDatatypeId As Integer)
        Me._TempDatatypeId = tmpTempDatatypeId
    End Sub
    Public Sub New(tmpTempDatatype As String)
        Me._TempDatatype = tmpTempDatatype
    End Sub

    Public Sub New()
    End Sub
    Public Property TempDatatypeId() As Integer Implements IeZTempDatatype.TempDatatypeId
        Get
            If _TempDatatypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TempDatatypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TempDatatypeId <> 0 AndAlso _TempDatatypeId <> value Then
                Throw New MemberAccessException()
            End If
            _TempDatatypeId = value
        End Set
    End Property

    Public Property TempDatatype() As String Implements IeZTempDatatype.TempDatatype
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TempDatatype
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TempDatatype = value Then
                Return
            End If
            _TempDatatype = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZTempDatatype.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZTempDatatype.CreatedBy1
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


    Public Property CreatedBy() As Integer Implements IeZTempDatatype.CreatedBy
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

    Public Property CreatedOn() As String Implements IeZTempDatatype.CreatedOn
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


    Public Property UpdatedBy() As Integer Implements IeZTempDatatype.UpdatedBy
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

    Public Property UpdatedOn() As String Implements IeZTempDatatype.UpdatedOn
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

    Public ReadOnly Property Isdeleted() As Integer Implements IeZTempDatatype.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    '---------------------------------------------------------------------------

    Public ReadOnly Property IsTempDatatypeExist() As Boolean Implements IeZTempDatatype.IsTempDatatypeExist
        Get
            Return (TempDatatypeId > 0)
        End Get
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
