Imports System.Data
Imports System.Configuration
Imports System.Web
Imports ECMAPI

Public Class eZTemplateField
    Inherits IDatabaseCommonItems
    Implements IeZTemplateField

    Protected _FieldId As Integer
    Protected _FieldName As String
    Protected _TemplateName As String
    Protected _DataTypeId As Integer
    'Protected _BarcodeTypeId As Integer
    Protected _TableName As String
    'Protected _BarcodeType As String
    Protected _DataType As String
    Protected _DT As String
    Protected _Mandatory As Boolean
    Protected _FieldLevel As Integer
    Protected _TemplateId As Integer
    Protected _CreatedBy As Integer
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Private _Isdeleted As Integer
    Protected _IsEditable As Boolean

    Public Sub New(DeptId As Integer)
        Me._FieldId = DeptId
    End Sub
    Public Sub New(tmpFieldName As String)
        Me._FieldName = tmpFieldName.Trim()
    End Sub
    Public Sub New()
    End Sub

    Public Property TableName() As String Implements IeZTemplateField.TableName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TableName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TableName = value Then
                Return
            End If
            _TableName = value
            IsModified = True
        End Set
    End Property
    Public Property DataTypeId() As Integer Implements IeZTemplateField.DataTypeId
        Get
            If _DataTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _DataTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _DataTypeId <> 0 AndAlso _DataTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _DataTypeId = value
        End Set
    End Property
    'Public Property BarcodeTypeId() As Integer Implements IeZTemplateField.BarcodeTypeId
    '    Get
    '        If _BarcodeTypeId = 0 Then
    '            DBLayer.DBLInstance.Read(Me)
    '        End If
    '        Return _BarcodeTypeId
    '    End Get
    '    Set(value As Integer)
    '        If Not _IsReadFromDB Then
    '            DBLayer.DBLInstance.Read(Me)
    '        End If
    '        If _BarcodeTypeId <> 0 AndAlso _BarcodeTypeId <> value Then
    '            Throw New MemberAccessException()
    '        End If
    '        _BarcodeTypeId = value
    '    End Set
    'End Property
    Public Property FieldLevel() As Integer Implements IeZTemplateField.FieldLevel
        Get
            If _FieldLevel = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FieldLevel
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FieldLevel <> 0 AndAlso _FieldLevel <> value Then
                Throw New MemberAccessException()
            End If
            _FieldLevel = value
        End Set
    End Property
    Public Property DataType() As String Implements IeZTemplateField.DataType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DataType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _DataType = value Then
                Return
            End If
            _DataType = value
            IsModified = True
        End Set
    End Property
    Public Property DT() As String Implements IeZTemplateField.DT
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DT
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _DT = value Then
                Return
            End If
            _DT = value
            IsModified = True
        End Set
    End Property
    'Public Property BarcodeType() As String Implements IeZTemplateField.BarcodeType
    '    Get
    '        DBLayer.DBLInstance.Read(Me)
    '        Return _BarcodeType
    '    End Get
    '    Set(value As String)
    '        DBLayer.DBLInstance.Read(Me)
    '        If _BarcodeType = value Then
    '            Return
    '        End If
    '        _BarcodeType = value
    '        IsModified = True
    '    End Set
    'End Property
    Public Property TemplateName() As String Implements IeZTemplateField.TemplateName
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
    Public Property FieldId() As Integer Implements IeZTemplateField.FieldId
        Get
            If _FieldId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _FieldId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _FieldId <> 0 AndAlso _FieldId <> value Then
                Throw New MemberAccessException()
            End If
            _FieldId = value
        End Set
    End Property
    Public Property FieldName() As String Implements IeZTemplateField.FieldName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _FieldName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _FieldName = value Then
                Return
            End If
            _FieldName = value
            IsModified = True
        End Set
    End Property
    Public Property Mandatory() As Boolean Implements IeZTemplateField.Mandatory
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _Mandatory
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _Mandatory = value Then
                Return
            End If
            _Mandatory = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZTemplateField.TemplateID
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
    Public Property UpdatedBy1() As String Implements IeZTemplateField.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZTemplateField.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZTemplateField.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZTemplateField.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZTemplateField.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZTemplateField.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZTemplateField.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsTemplateFieldExist() As Boolean Implements IeZTemplateField.IsTemplateFieldExist
        Get
            Return (_FieldId > 0)
        End Get
    End Property

    Public Property IsEditable As Boolean Implements IeZTemplateField.IsEditable
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _IsEditable
        End Get
        Set(value As Boolean)
            DBLayer.DBLInstance.Read(Me)
            If _IsEditable = value Then
                Return
            End If

            _IsEditable = value
            IsModified = True
        End Set
    End Property

    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub

End Class
