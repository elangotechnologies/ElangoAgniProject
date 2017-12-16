Imports System.Data.SqlClient

Imports System.IO
''Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports System.Threading

Public Class BillSearchCrystalReportHolder

    Dim dbConnection As SqlConnection
    Dim billSearchCrystalReport As New BillSearchCrystalReport

    Public valueToConvert As String
    Private n, intpart, realpart, numchar, intword, realword, spltval, spltword As String
    Private flag As Boolean



    Private Sub BillSearchCrystalReportHolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setBillSearchDataInReport(AgniMainForm.gBillSearchResutlDataSet, AgniMainForm.gBillSearchFilterText)
    End Sub

    Sub setBillSearchDataInReport(searchBillDataSet As DataSet, billSearchFilterText As String)
        billSearchCrystalReport.SetDataSource(searchBillDataSet)
        reportBillSearchReportViewer.ReportSource = billSearchCrystalReport

        billSearchCrystalReport.SetParameterValue("SearchFilterData", billSearchFilterText)

        Dim totalAmount As Decimal = 0
        If searchBillDataSet.Tables(1) IsNot Nothing AndAlso searchBillDataSet.Tables(1).Rows.Count > 0 Then
            totalAmount = searchBillDataSet.Tables(1).Rows(0).Item("TotalBillAmount").ToString
        End If
        billSearchCrystalReport.SetParameterValue("TotalBillAmountInWords", getAmountInWords(totalAmount.ToString))
    End Sub

    Private Sub BillSearchCrystalReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        billSearchCrystalReport.Dispose()
    End Sub

    Public Function getAmountInWords(valueToConvert As String) As String
        n = ""
        intpart = ""
        realpart = ""
        numchar = ""
        intword = ""
        realword = ""
        spltval = ""
        spltword = ""
        Dim amountInWords = ""
        If valueToConvert = "" Then
            Return "None"
        End If

        If valueToConvert = "." Then
            valueToConvert = "0.00"
        End If

        intpart = Format(Int(valueToConvert), "000000000")
        realpart = VB.Right(valueToConvert, 2)

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
        If intword <> "" And realword <> "" Then amountInWords = intword + "and " + realword + " Paise Only"
        If intword <> "" And realword = "" Then amountInWords = intword + "Only"
        If intword = "" And realword <> "" Then amountInWords = "Paise: " + realword + "Only"

        Return amountInWords
    End Function

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

End Class

