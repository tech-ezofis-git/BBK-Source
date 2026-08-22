Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZTempBarcode
    Inherits IDatabaseCommonItems
    Implements IeZTempBarcode
    Protected _BarcodeId As Integer
    Protected _StartsWith As String = ""
    Protected _TemplateName As String
    Protected _prefix As String = ""
    Protected _EndWith As String = ""
    Protected _Length As String = ""
    Protected _suffix As String = ""
    Protected _TemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _BarcodeField As String
    Protected _BarcodeType As String
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Protected _BarcodeTypeId As Integer
    Public Sub New(DeptId As Integer)
        Me._BarcodeId = DeptId
    End Sub

    Public Sub New()
    End Sub

    Public Property suffix() As String Implements IeZTempBarcode.suffix
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _suffix
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _suffix = value Then
                Return
            End If
            _suffix = value
            IsModified = True
        End Set
    End Property
    Public Property BarcodeType() As String Implements IeZTempBarcode.BarcodeType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _BarcodeType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _BarcodeType = value Then
                Return
            End If
            _BarcodeType = value
            IsModified = True
        End Set
    End Property
    Public Property BarcodeField() As String Implements IeZTempBarcode.BarcodeField
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _BarcodeField
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _BarcodeField = value Then
                Return
            End If
            _BarcodeField = value
            IsModified = True
        End Set
    End Property
    Public Property prefix() As String Implements IeZTempBarcode.prefix
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _prefix
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _prefix = value Then
                Return
            End If
            _prefix = value
            IsModified = True
        End Set
    End Property
    Public Property Length() As String Implements IeZTempBarcode.Length
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Length
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _Length = value Then
                Return
            End If
            _Length = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateName() As String Implements IeZTempBarcode.TemplateName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateName = value Then
                Return
            End If
            _TemplateName = value
            IsModified = True
        End Set
    End Property
    Public Property BarcodeId() As Integer Implements IeZTempBarcode.BarcodeId
        Get
            If _BarcodeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _BarcodeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _BarcodeId <> 0 AndAlso _BarcodeId <> value Then
                Throw New MemberAccessException()
            End If
            _BarcodeId = value
        End Set
    End Property
    
    Public Property StartsWith() As String Implements IeZTempBarcode.StartsWith
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _StartsWith
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _StartsWith = value Then
                Return
            End If
            _StartsWith = value
            IsModified = True
        End Set
    End Property
    Public Property EndWith() As String Implements IeZTempBarcode.EndWith
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _EndWith
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _EndWith = value Then
                Return
            End If
            _EndWith = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZTempBarcode.TemplateID
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TemplateId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _TemplateId = value Then
                Return
            End If
            _TemplateId = value
            IsModified = True
        End Set
    End Property
    Public Property BarcodeTypeId() As Integer Implements IeZTempBarcode.BarcodeTypeId
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _BarcodeTypeId
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _BarcodeTypeId = value Then
                Return
            End If
            _BarcodeTypeId = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZTempBarcode.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZTempBarcode.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZTempBarcode.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZTempBarcode.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZTempBarcode.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZTempBarcode.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZTempBarcode.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IseZTempBarcodeExist() As Boolean Implements IeZTempBarcode.IseZTempBarcodeExist
        Get
            Return (_BarcodeId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
End Class
