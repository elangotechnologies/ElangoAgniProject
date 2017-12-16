Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class AllBillReport
    Dim ds8, ds9 As DataSet
    Dim dc8(8), dc9 As DataColumn
    Dim dt8, dt9 As DataTable
    Dim dr8, dr9, billrow As DataRow
    Dim billdate As Date
    Dim billdatestr As String
    Dim totbill As Decimal
    Public valnum, valsent As String
    Private n, intpart, realpart, numchar, intword, realword, spltval, spltword As String
    Private flag As Boolean
    Dim objRpt As New AllBill

    Public Sub WordConvert()
        n = ""
        intpart = ""
        realpart = ""
        numchar = ""
        intword = ""
        realword = ""
        spltval = ""
        spltword = ""
        valsent = ""
        If valnum = "." Then valnum = "0.00"
        If valnum = "" Then Exit Sub

        intpart = Format(Int(valnum), "000000000")
        realpart = VB.Right(valnum, 2)

        spltval = realpart
        Call ValFind()
        If spltword <> "" Then realword = spltword
        spltval = Mid(intpart, 1, 2)
        Call ValFind()
        If spltword <> "" Then intword = spltword + "Crore "
        spltval = Mid(intpart, 3, 2)
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword + "Lakh "
        spltval = Mid(intpart, 5, 2)
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword + "Thousand "
        n = Mid(intpart, 7, 1)
        Call ONES()
        If numchar <> "" Then intword = intword + numchar + "Hundred "
        spltval = Mid(intpart, 8, 2)
        If intword <> "" And Val(spltval) > 0 And realword = "" Then intword = intword + "and "
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword
        If intword <> "" And realword <> "" Then valsent = intword + "and " + realword + " Paise Only"
        If intword <> "" And realword = "" Then valsent = intword + "Only"
        If intword = "" And realword <> "" Then valsent = "Paise: " + realword + "Only"
    End Sub

    Public Sub ValFind()
        n = ""
        spltword = ""
        If Val(spltval) = 0 Then Exit Sub
        n = VB.Left(spltval, 1)
        Call TENS()
        spltword = numchar
        If flag = False Then n = VB.Right(spltval, 1) : Call ONES() : spltword = spltword + numchar
    End Sub

    Public Sub ONES()
        numchar = ""
        If n = 0 Then numchar = ""
        If n = 1 Then numchar = "One "
        If n = 2 Then numchar = "Two "
        If n = 3 Then numchar = "Three "
        If n = 4 Then numchar = "Four "
        If n = 5 Then numchar = "Five "
        If n = 6 Then numchar = "Six "
        If n = 7 Then numchar = "Seven "
        If n = 8 Then numchar = "Eight "
        If n = 9 Then numchar = "Nine "
    End Sub

    Public Sub TENS()
        numchar = ""
        If n = 1 Then n = VB.Right(spltval, 1) : Call TEENS() : flag = True : Exit Sub Else flag = False
        If n = 0 Then numchar = ""
        If n = 2 Then numchar = "Twenty "
        If n = 3 Then numchar = "Thirty "
        If n = 4 Then numchar = "Fourty "
        If n = 5 Then numchar = "Fifty "
        If n = 6 Then numchar = "Sixty "
        If n = 7 Then numchar = "Seventy "
        If n = 8 Then numchar = "Eighty "
        If n = 9 Then numchar = "Ninety "
    End Sub

    Public Sub TEENS()
        numchar = ""
        If n = 0 Then numchar = "Ten "
        If n = 1 Then numchar = "Eleven "
        If n = 2 Then numchar = "Twelve "
        If n = 3 Then numchar = "Thirteen "
        If n = 4 Then numchar = "Fourteen "
        If n = 5 Then numchar = "Fifteen "
        If n = 6 Then numchar = "Sixteen "
        If n = 7 Then numchar = "Seventeen "
        If n = 8 Then numchar = "Eighteen "
        If n = 9 Then numchar = "Nineten "
    End Sub

    Private Sub AllBillReport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        objRpt.Dispose()
    End Sub
    Private Sub AllBillReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim i, a, inc As Int32
            ds8 = New DataSet
            dt8 = New DataTable
            ds8.Tables.Add(dt8)
            dc8(0) = New DataColumn("CompName", Type.GetType("System.String"))
            dc8(1) = New DataColumn("BillNo", Type.GetType("System.String"))
            dc8(2) = New DataColumn("BillDate", Type.GetType("System.String"))
            dc8(3) = New DataColumn("prebalance", Type.GetType("System.Decimal"))
            dc8(4) = New DataColumn("descost", Type.GetType("System.Decimal"))
            dc8(5) = New DataColumn("TotAmount", Type.GetType("System.Decimal"))
            'dc8(6) = New DataColumn("Paid", Type.GetType("System.String"))
            dc8(6) = New DataColumn("curBalance", Type.GetType("System.Decimal"))
            For i = 0 To 6
                dt8.Columns.Add(dc8(i))
            Next
            a = AgnimainForm.billtable.Rows.Count
            inc = 0
            totbill = 0
            While (a)
                billrow = AgnimainForm.billtable.Rows(inc)
                dr8 = dt8.NewRow()
                dr8.Item(0) = billrow.Item(0)
                dr8.Item(1) = billrow.Item(8).ToString + "/" + billrow.Item(1).ToString
                billdate = DateTime.Parse(billrow.Item(2))
                billdatestr = billdate.ToString("dd/MM/yyyy")
                dr8.Item(2) = billdatestr
                dr8.Item(3) = billrow.Item(3)
                dr8.Item(4) = billrow.Item(4)
                totbill += billrow.Item(4)
                dr8.Item(5) = billrow.Item(5)
                'dr8.Item(6) = billrow.Item(6)
                dr8.Item(6) = billrow.Item(7)
                dt8.Rows.Add(dr8)
                inc += 1
                a -= 1
            End While

            ds9 = New DataSet
            dt9 = New DataTable
            Dim dc9(2) As DataColumn
            ds9.Tables.Add(dt9)
            dc9(0) = New DataColumn("Total", Type.GetType("System.Decimal"))
            dt9.Columns.Add(dc9(0))
            dc9(1) = New DataColumn("Totalinword", Type.GetType("System.String"))
            dt9.Columns.Add(dc9(1))
            dr9 = dt9.NewRow()
            dr9.Item(0) = totbill
            Dim c As Double
            c = Double.Parse(dr9.Item(0))
            valnum = Format(c, ".00")
            Call WordConvert()
            dr9.Item(1) = valsent
            dt9.Rows.Add(dr9)



            Dim ds13 As New DataSet
            Dim dt13 As New DataTable
            Dim dc13(3) As DataColumn
            Dim dr13 As DataRow
            ds13.Tables.Add(dt13)
            dc13(0) = New DataColumn("FromDate", Type.GetType("System.String"))
            dc13(1) = New DataColumn("ToDate", Type.GetType("System.String"))
            dc13(2) = New DataColumn("Company", Type.GetType("System.String"))
            dt13.Columns.Add(dc13(0))
            dt13.Columns.Add(dc13(1))
            dt13.Columns.Add(dc13(2))
            dr13 = dt13.NewRow()
            dr13.Item(0) = AgnimainForm.fromdatestr1
            dr13.Item(1) = AgnimainForm.todatestr1
            dr13.Item(2) = AgnimainForm.compname
            dt13.Rows.Add(dr13)

            objRpt.SetDataSource(ds8.Tables(0))
            objRpt.Subreports.Item("TotBill").SetDataSource(ds9.Tables(0))
            objRpt.Subreports.Item("BillDates").SetDataSource(ds13.Tables(0))
            objRpt.Subreports.Item("AllBillInWord").SetDataSource(ds9.Tables(0))
            CrystalReportViewer1.ReportSource = objRpt

            'objRpt.Subreports.Item("BillDates").
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Dim folder As New FolderBrowserDialog
            If folder.ShowDialog() = Windows.Forms.DialogResult.OK Then
                MsgBox("The PDF file will be saved in '" + folder.SelectedPath.ToString + "\'")
                AgnimainForm.pdfdesfolder1 = folder.SelectedPath.ToString
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
            Dim custname As String = ""
            Dim frdate As String = ""
            Dim todate As String = ""
            If AgnimainForm.fromdatestr1.Trim.Equals("Not Specified") And AgnimainForm.todatestr1.Trim.Equals("Not Specified") Then
                frdate = ""
                todate = ""
                custname = AgnimainForm.compname
            ElseIf AgnimainForm.compname.Trim.Equals("Not Specified") Then
                frdate = AgnimainForm.fromdatestr1 + " to "
                todate = AgnimainForm.todatestr1
                custname = ""
            Else
                frdate = AgnimainForm.fromdatestr1 + " to "
                todate = AgnimainForm.todatestr1 + " "
                custname = AgnimainForm.compname
            End If
            fname = AgnimainForm.pdfdesfolder1 + "\Report @" + frdate + "" + todate + "" + custname + ".pdf"
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