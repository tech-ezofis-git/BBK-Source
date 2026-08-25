Imports ezofis
Imports System.Windows.Forms
Public NotInheritable Class ExportToArchive
   
  

    Public Shared Function ExportAsPdf(ByVal sTitle As String, ByVal sSubject As String, ByVal sAuthor As String, ByVal sRemarks As String, ByVal sPdfSignature As String, ByVal filename As String, ByVal SourceFileName As String, ByVal CabinetPath As String) As Integer
        Dim Result As Integer = 0
        Try
            Dim gPdf As New pdfconvertor
            Result = gPdf.ConvertToPDF(CabinetPath, filename, SourceFileName, sTitle, sSubject, sAuthor, sRemarks, sPdfSignature, "")
            Return Result
        Catch ex As Exception
            Result = 0
            MessageBox.Show(ex.Message, "Pdf Creation")
        End Try
        Return Result
    End Function

   
End Class
