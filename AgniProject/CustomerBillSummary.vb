Imports System.Data.SqlClient

Public Class CustomerBillSummary

    Dim dbConnection As SqlConnection

    Private Sub OutBalance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        dbConnection = New SqlConnection("server=agni\SQLEXPRESS;Database=agnidatabase;Integrated Security=true; MultipleActiveResultSets=True;")
        dbConnection.Open()

        bwtCustomerSummaryLoadThread.RunWorkerAsync()
        bwtTotalBillSummaryLoadThread.RunWorkerAsync()

        'Catch ex As Exception
        '    MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End Try
    End Sub



    Function getCustomerBillSummary(Optional custNo As Integer = Nothing) As DataTable

        Dim billSummarySelectQuery As SqlCommand
        If (custNo <> Nothing) Then
            billSummarySelectQuery = New SqlCommand("select cd.compname, cd.designCount, cd.billedDesAmtNoGST, cd.unbilledDesAmtNoGST, cd.TotDeisgnAmtNoGST, cb.billedDesAmtWithGST,
                                        cp.ActPaidAmount, cp.Discount, cp.TaxDeduction,
                                        cb.FinalPaidAmount, cb.billedDesAmtWithGST - cb.FinalPaidAmount as NetBalance
                                        from 
                                        (select c.custno, c.compname, count(d.DesignNo) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesAmtNoGST, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesAmtNoGST, isnull(sum(d.Price),0) as TotDeisgnAmtNoGST from customer c left join design d on c.custno = d.custno where c.custno=" + custNo.ToString + " group by c.custno, c.compname) as cd,
                                        (select c.custno, isnull(sum(b.DesignCost+((isnull(b.CGST,0)+isnull(b.SGST,0)+isnull(b.IGST,0))*b.DesignCost/100)),0) AS billedDesAmtWithGST, isnull(sum(b.PaidAmount),0) as FinalPaidAmount from customer c left join bill b on c.custno = b.custno  where c.custno=" + custNo.ToString + " group by c.custno) as cb,
                                        (select c.custno, isnull(sum(p.ActualPaidAmount),0) as ActPaidAmount, isnull(sum(p.Discount),0) as Discount, isnull(sum(p.TaxDeduction),0) as TaxDeduction  from customer c left join payment p on c.custno = p.custno  where c.custno=" + custNo.ToString + " group by c.custno) as cp
                                        where cd.custno = cb.custno and cd.custno = cp.custno", dbConnection)
        Else
            billSummarySelectQuery = New SqlCommand("select cd.custno, cd.compname, cd.designCount, cd.billedDesAmtNoGST, cd.unbilledDesAmtNoGST, cd.TotDeisgnAmtNoGST, cb.billedDesAmtWithGST,
                                        cp.ActPaidAmount, cp.Discount, cp.TaxDeduction,
                                        cb.FinalPaidAmount, cb.billedDesAmtWithGST - cb.FinalPaidAmount as NetBalance
                                        from 
                                        (select c.custno, c.compname, count(d.DesignNo) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesAmtNoGST, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesAmtNoGST, isnull(sum(d.Price),0) as TotDeisgnAmtNoGST from customer c left join design d on c.custno = d.custno group by c.custno, c.compname) as cd,
                                        (select c.custno, isnull(sum(b.DesignCost+((isnull(b.CGST,0)+isnull(b.SGST,0)+isnull(b.IGST,0))*b.DesignCost/100)),0) AS billedDesAmtWithGST, isnull(sum(b.PaidAmount),0) as FinalPaidAmount from customer c left join bill b on c.custno = b.custno group by c.custno) as cb,
                                        (select c.custno, isnull(sum(p.ActualPaidAmount),0) as ActPaidAmount, isnull(sum(p.Discount),0) as Discount, isnull(sum(p.TaxDeduction),0) as TaxDeduction  from customer c left join payment p on c.custno = p.custno group by c.custno) as cp
                                        where cd.custno = cb.custno and cd.custno = cp.custno", dbConnection)
        End If

        Dim billSummaryAdapter = New SqlDataAdapter()
        billSummaryAdapter.SelectCommand = billSummarySelectQuery
        Dim billSummaryDataSet = New DataSet
        billSummaryAdapter.Fill(billSummaryDataSet, "BillSummary")
        Return billSummaryDataSet.Tables(0)
    End Function

    Function getTotalBillSummary() As DataTable

        Dim totalBillSummarySelectQuery As SqlCommand
        totalBillSummarySelectQuery = New SqlCommand("select sum(designCount) as TotDesignCount,sum(billCount) as TotBillCount, sum(unbilledDesAmtNoGST) as TotUnbilledDesAmt,
                                        sum(ActPaidAmount) as TotActualPaidAmount, sum(Discount) as TotDiscount, sum(TaxDeduction) as TotTaxDeduction, sum(billedDesAmtWithGST) as TotBilledAmt, 
                                        sum(FinalPaidAmount) as TotPaidAmount, sum(NetBalance) as TotNetBalance
                                        from(
                                        select cd.custno, cd.compname, cd.designCount, cd.billedDesAmtNoGST, cd.unbilledDesAmtNoGST, cd.TotDeisgnAmtNoGST, cb.billedDesAmtWithGST,
                                        cp.ActPaidAmount, cp.Discount, cp.TaxDeduction, cb.billCount,
                                        cb.FinalPaidAmount, cb.billedDesAmtWithGST - cb.FinalPaidAmount as NetBalance
                                        from 
                                        (select c.custno, c.compname, count(d.DesignNo) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesAmtNoGST, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesAmtNoGST, isnull(sum(d.Price),0) as TotDeisgnAmtNoGST from customer c left join design d on c.custno = d.custno group by c.custno, c.compname) as cd,
                                        (select c.custno, count(b.BillNo) as billCount,  isnull(sum(b.DesignCost+((isnull(b.CGST,0)+isnull(b.SGST,0)+isnull(b.IGST,0))*b.DesignCost/100)),0) AS billedDesAmtWithGST, isnull(sum(b.PaidAmount),0) as FinalPaidAmount from customer c left join bill b on c.custno = b.custno group by c.custno) as cb,
                                        (select c.custno, isnull(sum(p.ActualPaidAmount),0) as ActPaidAmount, isnull(sum(p.Discount),0) as Discount, isnull(sum(p.TaxDeduction),0) as TaxDeduction  from customer c left join payment p on c.custno = p.custno group by c.custno) as cp
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
        dbConnection.Close()
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
        lblDesignsCount.Text = dataRow.Item("TotDesignCount")
        lblBillsCount.Text = dataRow.Item("TotBillCount")
        lblUnBilledAmount.Text = dataRow.Item("TotUnbilledDesAmt")
        lblActualPaidAmount.Text = dataRow.Item("TotActualPaidAmount")
        lblDiscountAmount.Text = dataRow.Item("TotDiscount")
        lblTaxDeductedAmount.Text = dataRow.Item("TotTaxDeduction")
        lblTotalBilledAmount.Text = dataRow.Item("TotBilledAmt")
        lblTotalPaidAmount.Text = dataRow.Item("TotPaidAmount")
        lblNetBalance.Text = dataRow.Item("TotNetBalance")

    End Sub
End Class