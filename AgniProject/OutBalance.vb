Public Class OutBalance

    Private Sub OutBalance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            DataGrid4.DataSource = AgnimainForm.ds9.Tables(0)
            AgnimainForm.ds9.Tables(0).TableName = "bill"
            Dim gridstyle4 As New DataGridTableStyle
            gridstyle4.MappingName = "bill"
            Dim colStyle3(8) As DataGridTextBoxColumn
            Dim i As Int16
            For i = 0 To 7
                colStyle3(i) = New DataGridTextBoxColumn
            Next
            With colStyle3(0)
                .MappingName = "CompName"
                .HeaderText = "Company Name"
                .Width = 250
            End With
            With colStyle3(1)
                .MappingName = "BillNo"
                .HeaderText = "Bill No."
                .Width = 110
            End With
            With colStyle3(2)
                .MappingName = "BillDate"
                .Format = "MMM dd, yyyy"
                .HeaderText = "Bill Date"
                .Width = 150
            End With
            With colStyle3(3)
                .MappingName = "prebalance"
                .HeaderText = "Previous Balance"
                .Width = 140
            End With
            With colStyle3(4)
                .MappingName = "Deduction"
                .HeaderText = "Deduction"
                .Alignment = HorizontalAlignment.Right
                .Width = 140
            End With
            With colStyle3(5)
                .MappingName = "TaxDeduction"
                .HeaderText = "Tax Deduction"
                .Alignment = HorizontalAlignment.Right
                .Width = 160
            End With
            With colStyle3(6)
                .MappingName = "TotUnPaid"
                .HeaderText = "UnBilled"
                .Width = 140
                .Alignment = HorizontalAlignment.Right
            End With
            With colStyle3(7)
                .MappingName = "curBalance"
                .HeaderText = "Current Balance"
                .Width = 130
                .Alignment = HorizontalAlignment.Right
            End With
            With gridstyle4.GridColumnStyles
                .Add(colStyle3(0))
                '.Add(colStyle3(1))
                '.Add(colStyle3(2))
                '.Add(colStyle3(3))
                .Add(colStyle3(6))
                .Add(colStyle3(7))
                .Add(colStyle3(4))
                .Add(colStyle3(5))
            End With
            DataGrid4.TableStyles.Clear()
            DataGrid4.TableStyles.Add(gridstyle4)
            gridstyle4.HeaderBackColor = Color.MidnightBlue
            gridstyle4.HeaderForeColor = Color.White
            gridstyle4.GridLineColor = Color.RoyalBlue
            gridstyle4.AllowSorting = False

            Label6.Text = AgnimainForm.totDeduction
            Label9.Text = AgnimainForm.tottaxDeduction
            Label70.Text = AgnimainForm.outbal
            Label1.Text = AgnimainForm.unbilled
            Label3.Text = AgnimainForm.outbal + AgnimainForm.unbilled
            Label10.Text = AgnimainForm.totDeduction + AgnimainForm.tottaxDeduction
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Balance.Show()
    End Sub

    Private Sub DataGrid4_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid4.Navigate

    End Sub
End Class