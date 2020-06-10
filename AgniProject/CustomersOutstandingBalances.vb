Imports System.Data.SqlClient

Public Class CustomersOutstandingBalances

    Dim dbConnection As SqlConnection

    Private Sub OutBalance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        dbConnection = getDBConnection()

        bwtCustomerSummaryLoadThread.RunWorkerAsync()
        bwtTotalBillSummaryLoadThread.RunWorkerAsync()

        'Catch ex As Exception
        '    MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End Try
    End Sub



    Function getCustomerBillSummary(Optional custNo As Integer = Nothing) As DataTable

        Dim billSummarySelectQuery As SqlCommand
        If (custNo <> Nothing) Then
            billSummarySelectQuery = New SqlCommand("select cd.custno, cd.compname, cd.designCount, round(cd.billedDesAmtNoGST,0) as billedDesAmtNoGST, round(cd.unbilledDesAmtNoGST,0) as unbilledDesAmtNoGST, round(cd.TotDeisgnAmtNoGST,0) as TotDeisgnAmtNoGST, round(cb.billedDesAmtWithGST,0) as billedDesAmtWithGST,
                                        round(cp.ActPaidAmount,0) as ActPaidAmount, round(cp.Discount,0) as Discount, round(cp.TDS,0) as TDS,
                                        round(cb.FinalPaidAmount,0) as FinalPaidAmount, round(cb.billedDesAmtWithGST - cb.FinalPaidAmount, 0) as UnPaidBilledAmount, round(cd.unbilledDesAmtNoGST + (cb.billedDesAmtWithGST - cb.FinalPaidAmount), 0) as OutstandingBalance
                                        from 
                                        (select c.custno, c.compname, count(d.DesignNo) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesAmtNoGST, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesAmtNoGST, isnull(sum(d.Price),0) as TotDeisgnAmtNoGST from customer c left join design d on c.custno = d.custno where c.custno=" + custNo.ToString + " group by c.custno, c.compname) as cd,


                                        (select c.custno, isnull(sum(Round(b.DesignCost+((isnull(b.CGST,0)+isnull(b.SGST,0)+isnull(b.IGST,0))*b.DesignCost/100),0)),0) AS billedDesAmtWithGST, isnull(sum(b.PaidAmount),0) as FinalPaidAmount from customer c left join bill b on c.custno = b.custno  where c.custno=" + custNo.ToString + " group by c.custno) as cb,
                                        (select c.custno, isnull(sum(p.ActualPaidAmount),0) as ActPaidAmount, isnull(sum(p.Discount),0) as Discount, isnull(sum(p.TDS),0) as TDS from customer c left join payment p on c.custno = p.custno  where c.custno=" + custNo.ToString + " group by c.custno) as cp
                                        where cd.custno = cb.custno and cd.custno = cp.custno", dbConnection)
        Else
            billSummarySelectQuery = New SqlCommand("select cd.custno, cd.compname, cd.designCount, round(cd.billedDesAmtNoGST,0) as billedDesAmtNoGST, round(cd.unbilledDesAmtNoGST,0) as unbilledDesAmtNoGST, round(cd.TotDeisgnAmtNoGST,0) as TotDeisgnAmtNoGST, round(cb.billedDesAmtWithGST,0) as billedDesAmtWithGST,
                                        round(cp.ActPaidAmount,0) as ActPaidAmount, round(cp.Discount,0) as Discount, round(cp.TDS,0) as TDS, 
                                        round(cb.FinalPaidAmount,0) as FinalPaidAmount, round(cb.billedDesAmtWithGST - cb.FinalPaidAmount, 0) as UnPaidBilledAmount, round(cd.unbilledDesAmtNoGST + (cb.billedDesAmtWithGST - cb.FinalPaidAmount), 0) as OutstandingBalance
                                        from 
                                        (select c.custno, c.compname, count(d.DesignNo) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesAmtNoGST, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesAmtNoGST, isnull(sum(d.Price),0) as TotDeisgnAmtNoGST from customer c left join design d on c.custno = d.custno group by c.custno, c.compname) as cd,
                                        (select c.custno, isnull(sum(Round(b.DesignCost+((isnull(b.CGST,0)+isnull(b.SGST,0)+isnull(b.IGST,0))*b.DesignCost/100),0)),0) AS billedDesAmtWithGST, isnull(sum(b.PaidAmount),0) as FinalPaidAmount from customer c left join bill b on c.custno = b.custno group by c.custno) as cb,
                                        (select c.custno, isnull(sum(p.ActualPaidAmount),0) as ActPaidAmount, isnull(sum(p.Discount),0) as Discount, isnull(sum(p.TDS),0) as TDS from customer c left join payment p on c.custno = p.custno group by c.custno) as cp
                                        where cd.custno = cb.custno and cd.custno = cp.custno order by cd.compname asc", dbConnection)
        End If

        Dim billSummaryAdapter = New SqlDataAdapter()
        billSummaryAdapter.SelectCommand = billSummarySelectQuery
        Dim billSummaryDataSet = New DataSet
        billSummaryAdapter.Fill(billSummaryDataSet, "BillSummary")
        Return billSummaryDataSet.Tables(0)
    End Function

    Function getTotalBillSummary() As DataTable

        Dim totalBillSummarySelectQuery As SqlCommand
        totalBillSummarySelectQuery = New SqlCommand("select isnull(count(custno),0) as CustCount, isnull(sum(designCount),0) as TotDesignCount,isnull(sum(billCount),0) as TotBillCount, isnull(round(sum(unbilledDesAmtNoGST),0),0) as TotUnbilledDesAmt,
                                        round(isnull(sum(ActPaidAmount),0),0) as TotActualPaidAmount, round(isnull(sum(Discount),0),0) as TotDiscount, round(isnull(sum(TDS),0),0) as TotTDS, round(isnull(sum(billedDesAmtWithGST),0),0) as TotBilledAmt, 
                                        round(isnull(sum(FinalPaidAmount),0),0) as TotPaidAmount, round(isnull(sum(NetBalance),0),0) as TotUnPaidBillAmount
                                        from(
                                        select cd.custno, cd.compname, cd.designCount, cd.billedDesAmtNoGST, cd.unbilledDesAmtNoGST, cd.TotDeisgnAmtNoGST, cb.billedDesAmtWithGST,
                                        cp.ActPaidAmount, cp.Discount, cp.TDS, cb.billCount,
                                        cb.FinalPaidAmount, cb.billedDesAmtWithGST - cb.FinalPaidAmount as NetBalance
                                        from 
                                        (select c.custno, c.compname, count(d.DesignNo) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesAmtNoGST, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesAmtNoGST, isnull(sum(d.Price),0) as TotDeisgnAmtNoGST from customer c left join design d on c.custno = d.custno group by c.custno, c.compname) as cd,
                                        (select c.custno, count(b.BillNo) as billCount,  isnull(sum(b.DesignCost+((isnull(b.CGST,0)+isnull(b.SGST,0)+isnull(b.IGST,0))*b.DesignCost/100)),0) AS billedDesAmtWithGST, isnull(sum(b.PaidAmount),0) as FinalPaidAmount from customer c left join bill b on c.custno = b.custno group by c.custno) as cb,
                                        (select c.custno, isnull(sum(p.ActualPaidAmount),0) as ActPaidAmount, isnull(sum(p.Discount),0) as Discount, isnull(sum(p.TDS),0) as TDS from customer c left join payment p on c.custno = p.custno group by c.custno) as cp
                                        where cd.custno = cb.custno and cd.custno = cp.custno) as billSumary", dbConnection)

        Dim totalBillSummaryAdapter = New SqlDataAdapter()
        totalBillSummaryAdapter.SelectCommand = totalBillSummarySelectQuery
        Dim totalBillSummaryDataSet = New DataSet
        totalBillSummaryAdapter.Fill(totalBillSummaryDataSet, "TotalBillSummary")
        Return totalBillSummaryDataSet.Tables(0)
    End Function

    Private Sub bwtCustomerSummaryLoadThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtCustomerSummaryLoadThread.DoWork
        e.Result = getCustomerBillSummary()
    End Sub

    Private Sub bwtCustomerSummaryLoadThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtCustomerSummaryLoadThread.RunWorkerCompleted
        Dim customerBillSummaryTable As DataTable = e.Result
        dgCustomerBillSummary.DataSource = customerBillSummaryTable
    End Sub

    Private Sub CustomerBillSummary_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
    End Sub

    Private Sub bwtTotalBillSummaryLoadThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtTotalBillSummaryLoadThread.DoWork
        e.Result = getTotalBillSummary()
    End Sub

    Private Sub bwtTotalBillSummaryLoadThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtTotalBillSummaryLoadThread.RunWorkerCompleted
        Dim totalBillSummaryTable As DataTable = e.Result

        If (totalBillSummaryTable.Rows.Count = 0) Then
            Return
        End If

        Dim dataRow As DataRow = totalBillSummaryTable.Rows(0)
        lblNumberOfCustomers.Text = dataRow.Item("CustCount")
        lblDesignsCount.Text = dataRow.Item("TotDesignCount")
        lblBillsCount.Text = dataRow.Item("TotBillCount")
        lblUnBilledAmount.Text = Format(Math.Round(dataRow.Item("TotUnbilledDesAmt")), "0.00")
        lblActualPaidAmount.Text = Format(Math.Round(dataRow.Item("TotActualPaidAmount")), "0.00")
        lblDiscountAmount.Text = Format(Math.Round(dataRow.Item("TotDiscount")), "0.00")
        lblTDSAmount.Text = Format(Math.Round(dataRow.Item("TotTDS")), "0.00")
        lblTotalBilledAmount.Text = Format(Math.Round(dataRow.Item("TotBilledAmt")), "0.00")
        lblTotalPaidAmount.Text = Format(Math.Round(dataRow.Item("TotPaidAmount")), "0.00")
        lblUnpaidBillAmount.Text = Format(Math.Round(dataRow.Item("TotUnPaidBillAmount")), "0.00")
        lblOutstandingBalance.Text = Format(Math.Round(dataRow.Item("TotUnPaidBillAmount") + dataRow.Item("TotUnbilledDesAmt")), "0.00")

    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles btnPrintOutstandingReport.ClickButtonArea
        OutstandingCrystalReportHolder.ShowDialog()
    End Sub

    Private Sub GroupBox5_Enter(sender As Object, e As EventArgs) Handles GroupBox5.Enter

    End Sub
End Class