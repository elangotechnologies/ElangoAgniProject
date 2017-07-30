Imports System.Data.SqlClient
Imports System.IO
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class Agnireport
    Dim Con As SqlConnection
    Dim cmd1, cmd2 As SqlCommand
    Dim Sda1, Sda2 As SqlDataAdapter
    Dim Ds1, Ds2, ds3, ds4, ds5, ds6 As DataSet
    Dim Dt1, Dt2, dt3 As DataTable
    Dim Dr2, dr3, dr4, dr5, dr6 As DataRow
    Dim Dc2, dc3(3), dc4(10), dc5(3), dc6(6) As DataColumn
    Dim Scb2, scb3 As SqlCommandBuilder
    Dim desdate As Date
    Dim desdatestr As String
    Public valnum, valsent As String
    Private n, intpart, realpart, numchar, intword, realword, spltval, spltword As String
    Private flag As Boolean
    Dim objRpt As New BillReport

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

    Private Sub Agnireport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        objRpt.Dispose()
    End Sub
    Private Sub AgniForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Con = New SqlConnection("Data Source=ELAPC;Initial Catalog=agnidatabase;Integrated Security=True")
            Con.Open()

            cmd2 = New SqlCommand("select * from design", Con)
            Sda2 = New SqlDataAdapter()
            Sda2.SelectCommand = cmd2
            Ds2 = New DataSet
            Sda2.Fill(Ds2, "design")
            Dt2 = Ds2.Tables(0)

            Dim i, a, inc As Int32
            Dim key As String
            ds4 = New DataSet
            Dim dt4 As New DataTable
            ds4.Tables.Add(dt4)
            dc4(0) = New DataColumn("Desnum", Type.GetType("System.Int32"))
            dc4(1) = New DataColumn("Desname", Type.GetType("System.String"))
            dc4(2) = New DataColumn("Desdate", Type.GetType("System.String"))
            dc4(3) = New DataColumn("Height", Type.GetType("System.Decimal"))
            dc4(4) = New DataColumn("Width", Type.GetType("System.Decimal"))
            dc4(5) = New DataColumn("Colors", Type.GetType("System.Decimal"))
            dc4(6) = New DataColumn("UnitCost", Type.GetType("System.Decimal"))
            dc4(7) = New DataColumn("Type", Type.GetType("System.String"))
            dc4(8) = New DataColumn("Price", Type.GetType("System.Decimal"))
            dc4(9) = New DataColumn("Image", Type.GetType("System.Byte[]"))
            For i = 0 To 9
                dt4.Columns.Add(dc4(i))
            Next
            key = AgnimainForm.billkey
            key = key.Substring(key.IndexOf("/") + 1, key.Length - key.IndexOf("/") - 1)
            a = Dt2.Rows.Count - 1
            inc = 0
            While (a >= 0)
                Dr2 = Dt2.Rows(inc)
                If key.ToString.Equals(Dr2.Item(12).ToString) Then
                    dr4 = dt4.NewRow
                    dr4.Item(0) = Dr2.Item(1)
                    dr4.Item(1) = Dr2.Item(2)
                    desdate = DateTime.Parse(Dr2.Item(10))
                    desdatestr = desdate.ToString("dd/MM/yyyy")
                    dr4.Item(2) = desdatestr
                    dr4.Item(3) = Dr2.Item(3)
                    dr4.Item(4) = Dr2.Item(4)
                    dr4.Item(5) = Dr2.Item(5)
                    dr4.Item(6) = Dr2.Item(6)
                    dr4.Item(7) = Dr2.Item(7)
                    dr4.Item(8) = Dr2.Item(9)
                    dr4.Item(9) = Dr2.Item(8)
                    dt4.Rows.Add(dr4)
                End If
                a -= 1
                inc += 1
            End While

            ds5 = New DataSet
            Dim dt5 As New DataTable
            ds5.Tables.Add(dt5)
            dc5(0) = New DataColumn("CompName", Type.GetType("System.String"))
            dc5(1) = New DataColumn("BillNo", Type.GetType("System.String"))
            dc5(2) = New DataColumn("BillDate", Type.GetType("System.String"))
            dc5(3) = New DataColumn("PrevBillNo", Type.GetType("System.String"))
            For i = 0 To 3
                dt5.Columns.Add(dc5(i))
            Next
            dr5 = dt5.NewRow
            cmd1 = New SqlCommand("select * from customer where compname='" + AgnimainForm.billcust + "'", Con)
            Sda1 = New SqlDataAdapter()
            Sda1.SelectCommand = cmd1
            Ds1 = New DataSet
            Sda1.Fill(Ds1, "customer")
            Dt1 = Ds1.Tables(0)
            If Dt1.Rows.Count <> 0 Then
                dr5.Item(0) = Dt1.Rows(0).Item(1) + vbNewLine + Dt1.Rows(0).Item(3) + vbNewLine + "Ph: " + Dt1.Rows(0).Item(4) + vbNewLine + Dt1.Rows(0).Item(7)
                dr5.Item(1) = AgnimainForm.billkey
                dr5.Item(2) = AgnimainForm.billdatestring
                dr5.Item(3) = AgnimainForm.PrevBillNo
                dt5.Rows.Add(dr5)
            End If

            ds6 = New DataSet
            Dim dt6 As New DataTable
            ds6.Tables.Add(dt6)
            dc6(0) = New DataColumn("PreBalance", Type.GetType("System.Decimal"))
            dc6(1) = New DataColumn("DesCost", Type.GetType("System.Decimal"))
            dc6(2) = New DataColumn("TotAmount", Type.GetType("System.Decimal"))
            dc6(3) = New DataColumn("wPreBalance", Type.GetType("System.String"))
            dc6(4) = New DataColumn("wDesCost", Type.GetType("System.String"))
            dc6(5) = New DataColumn("wTotAmount", Type.GetType("System.String"))
            For i = 0 To 5
                dt6.Columns.Add(dc6(i))
            Next
            dr6 = dt6.NewRow
            dr6.Item(0) = AgnimainForm.T20
            dr6.Item(1) = AgnimainForm.T21
            dr6.Item(2) = AgnimainForm.T17
            Dim c As Double
            c = Double.Parse(dr6.Item(0))
            valnum = Format(c, ".00")
            Call WordConvert()
            dr6.Item(3) = valsent
            c = Double.Parse(dr6.Item(1))
            valnum = Format(c, ".00")
            Call WordConvert()
            dr6.Item(4) = valsent
            c = Double.Parse(dr6.Item(2))
            valnum = Format(c, ".00")
            Call WordConvert()
            dr6.Item(5) = valsent
            dt6.Rows.Add(dr6)


            objRpt.SetDataSource(ds4.Tables(0))
            objRpt.Subreports.Item("BillHeader").SetDataSource(ds5.Tables(0))
            objRpt.Subreports.Item("BillFooter").SetDataSource(ds6.Tables(0))
            objRpt.Subreports.Item("Amt2words").SetDataSource(ds6.Tables(0))
            CrystalReportViewer1.ReportSource = objRpt
        Catch ex As Exception
            MessageBox.Show("message to agni user:   " & ex.Message)
        End Try
    End Sub

    Private Sub CrystalReportViewer1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub CrystalReportViewer1_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CrystalReportViewer1.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim CrExportOptions As ExportOptions
            Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions
            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions
            Dim fname As String = ""
            Dim billnumber = AgnimainForm.billkey
            Dim billnumber1 As String = billnumber.Substring(billnumber.IndexOf("/") + 1, billnumber.Length - billnumber.IndexOf("/") - 1)
            billnumber = billnumber.Substring(0, billnumber.IndexOf("/"))
            fname = AgnimainForm.pdfdesfolder + "\Bill @" + billnumber + "#" + billnumber1 + " " + AgnimainForm.billcust + ".pdf"
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

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Dim folder As New FolderBrowserDialog
            If folder.ShowDialog() = Windows.Forms.DialogResult.OK Then
                MsgBox("The PDF file will be saved in '" + folder.SelectedPath.ToString + "\'")
                AgnimainForm.pdfdesfolder = folder.SelectedPath.ToString
            End If
        Catch ex As Exception
            MessageBox.Show("message to agni user:   " & ex.Message)
        End Try
    End Sub
End Class