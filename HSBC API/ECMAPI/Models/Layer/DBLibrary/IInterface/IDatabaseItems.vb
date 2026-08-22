Imports System.Collections.Generic
Imports System.Text


Public Interface IDatabaseItems
    Property IsReadFromDB() As Boolean
    Property IsModified() As Boolean
    Property SNo() As Integer
    Sub SaveChanges()
End Interface

Public MustInherit Class IDatabaseCommonItems

    Implements IDatabaseItems


    Protected _IsReadFromDB As Boolean
    Protected _IsModified As Boolean
    Protected _SNo As Integer






    Public Property IsReadFromDB() As Boolean Implements IDatabaseItems.IsReadFromDB
        Get
            Return _IsReadFromDB
        End Get
        Set(value As Boolean)
            _IsReadFromDB = value
        End Set
    End Property

    Public Property IsModified() As Boolean Implements IDatabaseItems.IsModified
        Get
            Return _IsModified
        End Get
        Set(value As Boolean)
            _IsModified = value
        End Set
    End Property

    Public Property SNo() As Integer Implements IDatabaseItems.SNo
        Get
            Return _SNo
        End Get
        Set(value As Integer)
            _SNo = value
        End Set
    End Property
    Public Overridable Sub SaveChanges() Implements IDatabaseItems.SaveChanges
        Throw New InvalidOperationException()
    End Sub
End Class

