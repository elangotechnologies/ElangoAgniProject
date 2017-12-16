Imports System.Data.SqlClient

Imports System.IO
''Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports System.Threading

Public Class OutstandingCrystalReportHolder

    Dim dbConnection As SqlConnection
    Dim outstandingBalanceCrystalReport As New OutstandingBalanceCrystalReport


    Private Sub OutstandingBalanceReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbConnection = New SqlConnection("server=DESKTOP-EHEMD7K\ELASQLEXPRESS;Database=agnidatabase;Integrated Security=true; MultipleActiveResultSets=True;")
        dbConnection.Open()

        loadOutstandingBalance()
    End Sub

    Sub loadOutstandingBalance()
        Dim thread As Thread = New Thread(AddressOf fetchAndSetOutstandingBalance)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub fetchAndSetOutstandingBalance()
        fetchOutstandingBalance()
    End Sub

    Sub fetchOutstandingBalance()
        Dim outstandingBalanceQuery As SqlCommand
        outstandingBalanceQuery = New SqlCommand("select cd.custno, cd.compname as CompanyName, round(cd.unbilledDesAmtNoGST,0) as UnbilledAmount,
                                        round(cb.billedDesAmtWithGST - cb.FinalPaidAmount, 0) as UnPaidBilledAmount, round(cd.unbilledDesAmtNoGST + (cb.billedDesAmtWithGST - cb.FinalPaidAmount), 0) as OutstandingBalance
                                        from 
                                        (select c.custno, c.compname, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesAmtNoGST from customer c left join design d on c.custno = d.custno group by c.custno, c.compname) as cd,
                                        (select c.custno, isnull(sum(b.DesignCost+((isnull(b.CGST,0)+isnull(b.SGST,0)+isnull(b.IGST,0))*b.DesignCost/100)),0) AS billedDesAmtWithGST, isnull(sum(b.PaidAmount),0) as FinalPaidAmount from customer c left join bill b on c.custno = b.custno group by c.custno) as cb
                                        where cd.custno = cb.custno order by cd.compname asc", dbConnection)
        Dim outstandingBalanceAdapter = New SqlDataAdapter()
        outstandingBalanceAdapter.SelectCommand = outstandingBalanceQuery
        Dim outstandingBalanceDataSet As DataSet = New DataSet
        outstandingBalanceAdapter.Fill(outstandingBalanceDataSet, "OutstandingBalance")

        Dim totalOutstandingBalanceQuery As SqlCommand
        totalOutstandingBalanceQuery = New SqlCommand("select round(isnull(sum(UnbilledAmount),0),0) as TotalUnBilledAmount,
                                        round(isnull(sum(UnPaidBilledAmount),0),0) as TotalUnPaidBilledAmount, round(isnull(sum(OutstandingBalance),0),0) as TotalOutstandingBalance 
                                        from(
                                        select cd.custno, cd.compname as CompanyName, round(cd.unbilledDesAmtNoGST,0) as UnbilledAmount,
                                        round(cb.billedDesAmtWithGST - cb.FinalPaidAmount, 0) as UnPaidBilledAmount, round(cd.unbilledDesAmtNoGST + (cb.billedDesAmtWithGST - cb.FinalPaidAmount), 0) as OutstandingBalance
                                        from 
                                        (select c.custno, c.compname, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesAmtNoGST from customer c left join design d on c.custno = d.custno group by c.custno, c.compname) as cd,
                                        (select c.custno, isnull(sum(b.DesignCost+((isnull(b.CGST,0)+isnull(b.SGST,0)+isnull(b.IGST,0))*b.DesignCost/100)),0) AS billedDesAmtWithGST, isnull(sum(b.PaidAmount),0) as FinalPaidAmount from customer c left join bill b on c.custno = b.custno group by c.custno) as cb
                                        where cd.custno = cb.custno) as billSumary", dbConnection)
        Dim totalOutstandingBalanceAdapter = New SqlDataAdapter()
        totalOutstandingBalanceAdapter.SelectCommand = totalOutstandingBalanceQuery
        totalOutstandingBalanceAdapter.Fill(outstandingBalanceDataSet, "TotalOutstandingBalance")

        'MsgBox(outstandingBalanceDataSet.Tables("OutstandingBalanceTotal").Rows.Count.ToString)

        Dim setOutstandingBalanceInReportInvoker As New setOutstandingBalanceInReportDelegate(AddressOf Me.setOutstandingBalanceInReport)
        Me.Invoke(setOutstandingBalanceInReportInvoker, outstandingBalanceDataSet)
    End Sub

    Delegate Sub setOutstandingBalanceInReportDelegate(outstandingBalanceDataSet As DataSet)

    Sub setOutstandingBalanceInReport(outstandingBalanceDataSet As DataSet)
        outstandingBalanceCrystalReport.SetDataSource(outstandingBalanceDataSet)
        reportOutstandingBalanceReportViewer.ReportSource = outstandingBalanceCrystalReport
    End Sub

    Private Sub OutstandingCrystalReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        outstandingBalanceCrystalReport.Dispose()
    End Sub
End Class