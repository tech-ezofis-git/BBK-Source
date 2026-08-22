Imports System.Data
Imports System.Configuration
Imports System.Web
Public Class eZTemplate
    Inherits IDatabaseCommonItems
    Implements IeZTemplate
    Protected _TemplateId As Integer
    Protected _TemplateName As String
    Protected _CabinetName As String
    Protected _DocumentCount As Integer
    Protected _DuplicateTypeId As Integer
    Protected _DuplicateType As String
    Protected _TableName As String
    Protected _Description As String = ""
    Protected _CabinetID As Integer
    Protected _CreatedBy As Integer
    Protected _TempCurrentSize As String
    Protected _CreatedOn As String = ""
    Protected _UpdatedBy As Integer
    Protected _UpdatedOn As String = ""
    Protected _CreatedBy1 As String
    Protected _UpdatedBy1 As String
    Protected _Encrypt As Integer
    Private _Isdeleted As Integer

    Public Sub New(DeptId As Integer)
        Me._TemplateId = DeptId
    End Sub
    Public Sub New(tmpTemplateName As String)
        Me._TemplateName = tmpTemplateName.Trim()
    End Sub
    Public Sub New()
    End Sub
    Public Property DocumentCount() As Integer Implements IeZTemplate.DocumentCount
        Get
            If _DocumentCount = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _DocumentCount
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _DocumentCount <> 0 AndAlso _DocumentCount <> value Then
                Throw New MemberAccessException()
            End If
            _DocumentCount = value
        End Set
    End Property
    Public Property TableName() As String Implements IeZTemplate.TableName
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
    Public Property TempCurrentSize() As String Implements IeZTemplate.TempCurrentSize
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _TempCurrentSize
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _TempCurrentSize = value Then
                Return
            End If
            _TempCurrentSize = value
            IsModified = True
        End Set
    End Property
    Public Property DuplicateTypeId() As Integer Implements IeZTemplate.DuplicateTypeId
        Get
            If _DuplicateTypeId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _DuplicateTypeId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _DuplicateTypeId <> 0 AndAlso _DuplicateTypeId <> value Then
                Throw New MemberAccessException()
            End If
            _DuplicateTypeId = value
        End Set
    End Property
    Public Property DuplicateType() As String Implements IeZTemplate.DuplicateType
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _DuplicateType
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _DuplicateType = value Then
                Return
            End If
            _DuplicateType = value
            IsModified = True
        End Set
    End Property
    Public Property CabinetName() As String Implements IeZTemplate.CabinetName
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabinetName
        End Get
        Set(value As String)
            DBLayer.DBLInstance.Read(Me)
            If _CabinetName = value Then
                Return
            End If
            _CabinetName = value
            IsModified = True
        End Set
    End Property
    Public Property TemplateId() As Integer Implements IeZTemplate.TemplateId
        Get
            If _TemplateId = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _TemplateId
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _TemplateId <> 0 AndAlso _TemplateId <> value Then
                Throw New MemberAccessException()
            End If
            _TemplateId = value
        End Set
    End Property
    Public Property TemplateName() As String Implements IeZTemplate.TemplateName
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
    Public Property Description() As String Implements IeZTemplate.Description
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
    Public Property CabinetID() As Integer Implements IeZTemplate.CabinetID
        Get
            DBLayer.DBLInstance.Read(Me)
            Return _CabinetID
        End Get
        Set(value As Integer)
            DBLayer.DBLInstance.Read(Me)
            If _CabinetID = value Then
                Return
            End If
            _CabinetID = value
            IsModified = True
        End Set
    End Property
    Public Property UpdatedBy1() As String Implements IeZTemplate.UpdatedBy1
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
    Public Property CreatedBy1() As String Implements IeZTemplate.CreatedBy1
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
    Public Property CreatedBy() As Integer Implements IeZTemplate.CreatedBy
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
    Public Property CreatedOn() As String Implements IeZTemplate.CreatedOn
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
    Public Property UpdatedBy() As Integer Implements IeZTemplate.UpdatedBy
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
    Public Property UpdatedOn() As String Implements IeZTemplate.UpdatedOn
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
    Public ReadOnly Property Isdeleted() As Integer Implements IeZTemplate.Isdeleted
        Get
            Return _Isdeleted
        End Get
    End Property
    Public ReadOnly Property IsTemplateExist() As Boolean Implements IeZTemplate.IsTemplateExist
        Get
            Return (_TemplateId > 0)
        End Get
    End Property
    Public Overrides Sub SaveChanges()
        DBLayer.DBLInstance.Update(Me)
    End Sub
    Public Property Encrypt() As Integer Implements IeZTemplate.Encrypt
        Get
            If _Encrypt = 0 Then
                DBLayer.DBLInstance.Read(Me)
            End If
            Return _Encrypt
        End Get
        Set(value As Integer)
            If Not _IsReadFromDB Then
                DBLayer.DBLInstance.Read(Me)
            End If
            If _Encrypt <> 0 AndAlso _Encrypt <> value Then
                Throw New MemberAccessException()
            End If
            _Encrypt = value
        End Set
    End Property
End Class
