Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class Balance
    Dim objRpt As New OutBalanceReport

    Private Sub Balance_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        objRpt.Dispose()
    End Sub
    Private Sub Balance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ds9 As New DataSet
        Dim dt9 As New DataTable
        Dim dc9(3) As DataColumn
        Dim dr9 As DataRow
        ds9.Tables.Add(dt9)
        dc9(0) = New DataColumn("OutBal", Type.GetType("System.Decimal"))
        dt9.Columns.Add(dc9(0))
        dc9(1) = New DataColumn("UnBilled", Type.GetType("System.Decimal"))
        dt9.Columns.Add(dc9(1))
        dc9(2) = New DataColumn("TotOutBal", Type.GetType("System.Decimal"))
        dt9.Columns.Add(dc9(2))
        dr9 = dt9.NewRow()
        dr9.Item(0) = AgnimainForm.outbal
        dr9.Item(1) = AgnimainForm.unbilled
        dr9.Item(2) = AgnimainForm.outbal + AgnimainForm.unbilled
        dt9.Rows.Add(dr9)

        objRpt.SetDataSource(AgnimainForm.ds9.Tables(0))
        objRpt.Subreports.Item("totamounts").SetDataSource(ds9.Tables(0))
        CrystalReportViewer1.ReportSource = objRpt
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Dim folder As New FolderBrowserDialog
            If folder.ShowDialog() = Windows.Forms.DialogResult.OK Then
                MsgBox("The PDF file will be saved in '" + folder.SelectedPath.ToString + "\'")
                AgnimainForm.pdfdesfolder2 = folder.SelectedPath.ToString
            End If
        Catch ex As Exception
            MessageBox.Show("message to agni user:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim CrExportOptions As ExportOptions
            Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions
            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions
            Dim fname As String = ""
            Dim nowtime As DateTime = System.DateTime.Now
            Dim day As String = nowtime.Day.ToString
            Dim month As String = nowtime.Month.ToString
            Dim year As String = nowtime.Year.ToString
            Dim hour As String = nowtime.Hour.ToString
            Dim min As String = nowtime.Minute.ToString
            Dim sec As String = nowtime.Second.ToString
            Dim timeformat As String = "OutStanding @" + day + "-" + month + "-" + year + " " + hour + "`" + min + "`" + sec
            fname = AgnimainForm.pdfdesfolder2 + "\" + timeformat + ".pdf"
            CrDiskFileDestinationOptions.DiskFileName = fname
            CrExportOptions = objRpt.ExportOptions
            With CrExportOptions
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
                .DestinationOptions = CrDiskFileDestinationOptions
                .FormatOptions = CrFormatTypeOptions
            End With
            objRpt.Export()
            MsgBox("'" + fname + "' is successfully created")
        Catch ex As Exception
            MessageBox.Show("message to agni user:   " & ex.Message)
        End Try
       
    End Sub
End Class