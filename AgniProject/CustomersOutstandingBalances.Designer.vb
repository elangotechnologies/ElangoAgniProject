<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomersOutstandingBalances
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.bwtCustomerSummaryLoadThread = New System.ComponentModel.BackgroundWorker()
        Me.bwtTotalBillSummaryLoadThread = New System.ComponentModel.BackgroundWorker()
        Me.GroupBox5 = New ElaCustomGroupBoxControl.ElaCustomGroupBox()
        Me.dgCustomerBillSummary = New System.Windows.Forms.DataGridView()
        Me.CompName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CustNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.designCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BilledDesAmtNoGST = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.unbilledDesAmtNoGST = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotDeisgnAmtNoGST = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BilledDesAmtWithGST = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActPaidAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Discount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FinalPaidAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnPaidBilledAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OutstandingBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox2 = New ElaCustomGroupBoxControl.ElaCustomGroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblOutstandingBalance = New System.Windows.Forms.Label()
        Me.lblTotalPaidAmount = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblUnpaidBillAmount = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblNumberOfCustomers = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTotalBilledAmount = New System.Windows.Forms.Label()
        Me.lblBillsCount = New System.Windows.Forms.Label()
        Me.lblDesignsCount = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblUnBilledAmount = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblDiscountAmount = New System.Windows.Forms.Label()
        Me.lblActualPaidAmount = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnPrintOutstandingReport = New CButtonLib.CButton()
        Me.GroupBox1 = New ElaCustomGroupBoxControl.ElaCustomGroupBox()
        Me.GroupBox5.SuspendLayout()
        CType(Me.dgCustomerBillSummary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'bwtCustomerSummaryLoadThread
        '
        '
        'bwtTotalBillSummaryLoadThread
        '
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.dgCustomerBillSummary)
        Me.GroupBox5.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox5.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(1916, 837)
        Me.GroupBox5.TabIndex = 137
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "All Companies Outstanding Balance"
        '
        'dgCustomerBillSummary
        '
        Me.dgCustomerBillSummary.AllowUserToAddRows = False
        Me.dgCustomerBillSummary.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.dgCustomerBillSummary.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgCustomerBillSummary.BackgroundColor = System.Drawing.Color.Lavender
        Me.dgCustomerBillSummary.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgCustomerBillSummary.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical
        Me.dgCustomerBillSummary.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.RoyalBlue
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgCustomerBillSummary.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgCustomerBillSummary.ColumnHeadersHeight = 51
        Me.dgCustomerBillSummary.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CompName, Me.CustNo, Me.designCount, Me.BilledDesAmtNoGST, Me.unbilledDesAmtNoGST, Me.TotDeisgnAmtNoGST, Me.BilledDesAmtWithGST, Me.ActPaidAmount, Me.Discount, Me.FinalPaidAmount, Me.UnPaidBilledAmount, Me.OutstandingBalance})
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.Color.LightSteelBlue
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgCustomerBillSummary.DefaultCellStyle = DataGridViewCellStyle15
        Me.dgCustomerBillSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgCustomerBillSummary.EnableHeadersVisualStyles = False
        Me.dgCustomerBillSummary.GridColor = System.Drawing.Color.Maroon
        Me.dgCustomerBillSummary.Location = New System.Drawing.Point(3, 22)
        Me.dgCustomerBillSummary.MultiSelect = False
        Me.dgCustomerBillSummary.Name = "dgCustomerBillSummary"
        Me.dgCustomerBillSummary.ReadOnly = True
        Me.dgCustomerBillSummary.RowHeadersVisible = False
        DataGridViewCellStyle16.BackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgCustomerBillSummary.RowsDefaultCellStyle = DataGridViewCellStyle16
        Me.dgCustomerBillSummary.RowTemplate.Height = 25
        Me.dgCustomerBillSummary.RowTemplate.ReadOnly = True
        Me.dgCustomerBillSummary.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgCustomerBillSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgCustomerBillSummary.Size = New System.Drawing.Size(1910, 812)
        Me.dgCustomerBillSummary.TabIndex = 0
        '
        'CompName
        '
        Me.CompName.DataPropertyName = "CompName"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.CompName.DefaultCellStyle = DataGridViewCellStyle3
        Me.CompName.Frozen = True
        Me.CompName.HeaderText = "Customer Name"
        Me.CompName.Name = "CompName"
        Me.CompName.ReadOnly = True
        Me.CompName.Width = 200
        '
        'CustNo
        '
        Me.CustNo.DataPropertyName = "CustNo"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.CustNo.DefaultCellStyle = DataGridViewCellStyle4
        Me.CustNo.HeaderText = "Cust No"
        Me.CustNo.Name = "CustNo"
        Me.CustNo.ReadOnly = True
        Me.CustNo.Visible = False
        '
        'designCount
        '
        Me.designCount.DataPropertyName = "designCount"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.designCount.DefaultCellStyle = DataGridViewCellStyle5
        Me.designCount.HeaderText = "Design Count"
        Me.designCount.Name = "designCount"
        Me.designCount.ReadOnly = True
        Me.designCount.Width = 150
        '
        'BilledDesAmtNoGST
        '
        Me.BilledDesAmtNoGST.DataPropertyName = "BilledDesAmtNoGST"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "0.00"
        Me.BilledDesAmtNoGST.DefaultCellStyle = DataGridViewCellStyle6
        Me.BilledDesAmtNoGST.HeaderText = "Billed Amt No GST"
        Me.BilledDesAmtNoGST.Name = "BilledDesAmtNoGST"
        Me.BilledDesAmtNoGST.ReadOnly = True
        Me.BilledDesAmtNoGST.Visible = False
        Me.BilledDesAmtNoGST.Width = 190
        '
        'unbilledDesAmtNoGST
        '
        Me.unbilledDesAmtNoGST.DataPropertyName = "unbilledDesAmtNoGST"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "0.00"
        Me.unbilledDesAmtNoGST.DefaultCellStyle = DataGridViewCellStyle7
        Me.unbilledDesAmtNoGST.HeaderText = "UnBilled Amount"
        Me.unbilledDesAmtNoGST.Name = "unbilledDesAmtNoGST"
        Me.unbilledDesAmtNoGST.ReadOnly = True
        Me.unbilledDesAmtNoGST.Width = 190
        '
        'TotDeisgnAmtNoGST
        '
        Me.TotDeisgnAmtNoGST.DataPropertyName = "TotDeisgnAmtNoGST"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Format = "0.00"
        Me.TotDeisgnAmtNoGST.DefaultCellStyle = DataGridViewCellStyle8
        Me.TotDeisgnAmtNoGST.HeaderText = "Tot Deisgn Amt No GST"
        Me.TotDeisgnAmtNoGST.Name = "TotDeisgnAmtNoGST"
        Me.TotDeisgnAmtNoGST.ReadOnly = True
        Me.TotDeisgnAmtNoGST.Visible = False
        Me.TotDeisgnAmtNoGST.Width = 190
        '
        'BilledDesAmtWithGST
        '
        Me.BilledDesAmtWithGST.DataPropertyName = "BilledDesAmtWithGST"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle9.Format = "0.00"
        Me.BilledDesAmtWithGST.DefaultCellStyle = DataGridViewCellStyle9
        Me.BilledDesAmtWithGST.HeaderText = "Billed Amount"
        Me.BilledDesAmtWithGST.Name = "BilledDesAmtWithGST"
        Me.BilledDesAmtWithGST.ReadOnly = True
        Me.BilledDesAmtWithGST.Width = 190
        '
        'ActPaidAmount
        '
        Me.ActPaidAmount.DataPropertyName = "ActPaidAmount"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle10.Format = "0.00"
        Me.ActPaidAmount.DefaultCellStyle = DataGridViewCellStyle10
        Me.ActPaidAmount.HeaderText = "Actual Paid Amount"
        Me.ActPaidAmount.Name = "ActPaidAmount"
        Me.ActPaidAmount.ReadOnly = True
        Me.ActPaidAmount.Width = 190
        '
        'Discount
        '
        Me.Discount.DataPropertyName = "Discount"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle11.Format = "0.00"
        Me.Discount.DefaultCellStyle = DataGridViewCellStyle11
        Me.Discount.HeaderText = "Discount"
        Me.Discount.Name = "Discount"
        Me.Discount.ReadOnly = True
        Me.Discount.Width = 170
        '
        'FinalPaidAmount
        '
        Me.FinalPaidAmount.DataPropertyName = "FinalPaidAmount"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle12.Format = "0.00"
        Me.FinalPaidAmount.DefaultCellStyle = DataGridViewCellStyle12
        Me.FinalPaidAmount.HeaderText = "Final Paid Amount"
        Me.FinalPaidAmount.Name = "FinalPaidAmount"
        Me.FinalPaidAmount.ReadOnly = True
        Me.FinalPaidAmount.Width = 190
        '
        'UnPaidBilledAmount
        '
        Me.UnPaidBilledAmount.DataPropertyName = "UnPaidBilledAmount"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle13.Format = "0.00"
        Me.UnPaidBilledAmount.DefaultCellStyle = DataGridViewCellStyle13
        Me.UnPaidBilledAmount.HeaderText = "UnPaid Bill Amount"
        Me.UnPaidBilledAmount.Name = "UnPaidBilledAmount"
        Me.UnPaidBilledAmount.ReadOnly = True
        Me.UnPaidBilledAmount.Width = 190
        '
        'OutstandingBalance
        '
        Me.OutstandingBalance.DataPropertyName = "OutstandingBalance"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle14.Format = "0.00"
        Me.OutstandingBalance.DefaultCellStyle = DataGridViewCellStyle14
        Me.OutstandingBalance.HeaderText = "Outstanding Balance"
        Me.OutstandingBalance.Name = "OutstandingBalance"
        Me.OutstandingBalance.ReadOnly = True
        Me.OutstandingBalance.Width = 200
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.lblOutstandingBalance)
        Me.GroupBox2.Controls.Add(Me.lblTotalPaidAmount)
        Me.GroupBox2.Controls.Add(Me.Label17)
        Me.GroupBox2.Controls.Add(Me.lblUnpaidBillAmount)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.lblNumberOfCustomers)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.lblTotalBilledAmount)
        Me.GroupBox2.Controls.Add(Me.lblBillsCount)
        Me.GroupBox2.Controls.Add(Me.lblDesignsCount)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.lblUnBilledAmount)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.lblDiscountAmount)
        Me.GroupBox2.Controls.Add(Me.lblActualPaidAmount)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Location = New System.Drawing.Point(11, 21)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox2.Size = New System.Drawing.Size(1643, 160)
        Me.GroupBox2.TabIndex = 150
        Me.GroupBox2.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Crimson
        Me.Label1.Location = New System.Drawing.Point(1295, 52)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(321, 29)
        Me.Label1.TabIndex = 150
        Me.Label1.Text = "Total Outstanding Balance"
        '
        'lblOutstandingBalance
        '
        Me.lblOutstandingBalance.AutoSize = True
        Me.lblOutstandingBalance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblOutstandingBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblOutstandingBalance.ForeColor = System.Drawing.Color.White
        Me.lblOutstandingBalance.Location = New System.Drawing.Point(1340, 91)
        Me.lblOutstandingBalance.MinimumSize = New System.Drawing.Size(220, 20)
        Me.lblOutstandingBalance.Name = "lblOutstandingBalance"
        Me.lblOutstandingBalance.Size = New System.Drawing.Size(220, 26)
        Me.lblOutstandingBalance.TabIndex = 151
        Me.lblOutstandingBalance.Text = "0"
        Me.lblOutstandingBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTotalPaidAmount
        '
        Me.lblTotalPaidAmount.AutoSize = True
        Me.lblTotalPaidAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTotalPaidAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblTotalPaidAmount.ForeColor = System.Drawing.Color.White
        Me.lblTotalPaidAmount.Location = New System.Drawing.Point(1001, 91)
        Me.lblTotalPaidAmount.MinimumSize = New System.Drawing.Size(180, 20)
        Me.lblTotalPaidAmount.Name = "lblTotalPaidAmount"
        Me.lblTotalPaidAmount.Size = New System.Drawing.Size(180, 24)
        Me.lblTotalPaidAmount.TabIndex = 147
        Me.lblTotalPaidAmount.Text = "0"
        Me.lblTotalPaidAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.Crimson
        Me.Label17.Location = New System.Drawing.Point(742, 125)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(240, 24)
        Me.Label17.TabIndex = 148
        Me.Label17.Text = "Total Unpaid Bill Amount"
        '
        'lblUnpaidBillAmount
        '
        Me.lblUnpaidBillAmount.AutoSize = True
        Me.lblUnpaidBillAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblUnpaidBillAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnpaidBillAmount.ForeColor = System.Drawing.Color.White
        Me.lblUnpaidBillAmount.Location = New System.Drawing.Point(1001, 125)
        Me.lblUnpaidBillAmount.MinimumSize = New System.Drawing.Size(180, 20)
        Me.lblUnpaidBillAmount.Name = "lblUnpaidBillAmount"
        Me.lblUnpaidBillAmount.Size = New System.Drawing.Size(180, 24)
        Me.lblUnpaidBillAmount.TabIndex = 149
        Me.lblUnpaidBillAmount.Text = "0"
        Me.lblUnpaidBillAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label15.ForeColor = System.Drawing.Color.Crimson
        Me.Label15.Location = New System.Drawing.Point(742, 91)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(181, 24)
        Me.Label15.TabIndex = 146
        Me.Label15.Text = "Total Paid Amount"
        '
        'lblNumberOfCustomers
        '
        Me.lblNumberOfCustomers.AutoSize = True
        Me.lblNumberOfCustomers.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblNumberOfCustomers.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumberOfCustomers.ForeColor = System.Drawing.Color.White
        Me.lblNumberOfCustomers.Location = New System.Drawing.Point(188, 23)
        Me.lblNumberOfCustomers.MinimumSize = New System.Drawing.Size(52, 0)
        Me.lblNumberOfCustomers.Name = "lblNumberOfCustomers"
        Me.lblNumberOfCustomers.Size = New System.Drawing.Size(52, 20)
        Me.lblNumberOfCustomers.TabIndex = 145
        Me.lblNumberOfCustomers.Text = "0"
        Me.lblNumberOfCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(28, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(151, 20)
        Me.Label3.TabIndex = 144
        Me.Label3.Text = " No.Of Customers"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.Crimson
        Me.Label4.Location = New System.Drawing.Point(742, 57)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(140, 24)
        Me.Label4.TabIndex = 144
        Me.Label4.Text = "Billed Amount"
        '
        'lblTotalBilledAmount
        '
        Me.lblTotalBilledAmount.AutoSize = True
        Me.lblTotalBilledAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTotalBilledAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblTotalBilledAmount.ForeColor = System.Drawing.Color.White
        Me.lblTotalBilledAmount.Location = New System.Drawing.Point(1001, 57)
        Me.lblTotalBilledAmount.MinimumSize = New System.Drawing.Size(180, 20)
        Me.lblTotalBilledAmount.Name = "lblTotalBilledAmount"
        Me.lblTotalBilledAmount.Size = New System.Drawing.Size(180, 24)
        Me.lblTotalBilledAmount.TabIndex = 145
        Me.lblTotalBilledAmount.Text = "0"
        Me.lblTotalBilledAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblBillsCount
        '
        Me.lblBillsCount.AutoSize = True
        Me.lblBillsCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblBillsCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillsCount.ForeColor = System.Drawing.Color.White
        Me.lblBillsCount.Location = New System.Drawing.Point(188, 89)
        Me.lblBillsCount.MinimumSize = New System.Drawing.Size(52, 0)
        Me.lblBillsCount.Name = "lblBillsCount"
        Me.lblBillsCount.Size = New System.Drawing.Size(52, 20)
        Me.lblBillsCount.TabIndex = 122
        Me.lblBillsCount.Text = "0"
        Me.lblBillsCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDesignsCount
        '
        Me.lblDesignsCount.AutoSize = True
        Me.lblDesignsCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblDesignsCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesignsCount.ForeColor = System.Drawing.Color.White
        Me.lblDesignsCount.Location = New System.Drawing.Point(188, 56)
        Me.lblDesignsCount.MinimumSize = New System.Drawing.Size(52, 0)
        Me.lblDesignsCount.Name = "lblDesignsCount"
        Me.lblDesignsCount.Size = New System.Drawing.Size(52, 20)
        Me.lblDesignsCount.TabIndex = 125
        Me.lblDesignsCount.Text = "0"
        Me.lblDesignsCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(28, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(130, 20)
        Me.Label2.TabIndex = 124
        Me.Label2.Text = " No.Of Designs"
        '
        'lblUnBilledAmount
        '
        Me.lblUnBilledAmount.AutoSize = True
        Me.lblUnBilledAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblUnBilledAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblUnBilledAmount.ForeColor = System.Drawing.Color.White
        Me.lblUnBilledAmount.Location = New System.Drawing.Point(1001, 23)
        Me.lblUnBilledAmount.MinimumSize = New System.Drawing.Size(180, 20)
        Me.lblUnBilledAmount.Name = "lblUnBilledAmount"
        Me.lblUnBilledAmount.Size = New System.Drawing.Size(180, 24)
        Me.lblUnBilledAmount.TabIndex = 128
        Me.lblUnBilledAmount.Text = "0"
        Me.lblUnBilledAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(28, 89)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(98, 20)
        Me.Label11.TabIndex = 136
        Me.Label11.Text = " No.Of Bills"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label12.ForeColor = System.Drawing.Color.Crimson
        Me.Label12.Location = New System.Drawing.Point(742, 23)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(172, 24)
        Me.Label12.TabIndex = 137
        Me.Label12.Text = "Un Billed Amount"
        '
        'lblDiscountAmount
        '
        Me.lblDiscountAmount.AutoSize = True
        Me.lblDiscountAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblDiscountAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscountAmount.ForeColor = System.Drawing.Color.White
        Me.lblDiscountAmount.Location = New System.Drawing.Point(555, 56)
        Me.lblDiscountAmount.MinimumSize = New System.Drawing.Size(135, 0)
        Me.lblDiscountAmount.Name = "lblDiscountAmount"
        Me.lblDiscountAmount.Size = New System.Drawing.Size(135, 20)
        Me.lblDiscountAmount.TabIndex = 142
        Me.lblDiscountAmount.Text = "0"
        Me.lblDiscountAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblActualPaidAmount
        '
        Me.lblActualPaidAmount.AutoSize = True
        Me.lblActualPaidAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblActualPaidAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActualPaidAmount.ForeColor = System.Drawing.Color.White
        Me.lblActualPaidAmount.Location = New System.Drawing.Point(555, 23)
        Me.lblActualPaidAmount.MinimumSize = New System.Drawing.Size(135, 0)
        Me.lblActualPaidAmount.Name = "lblActualPaidAmount"
        Me.lblActualPaidAmount.Size = New System.Drawing.Size(135, 20)
        Me.lblActualPaidAmount.TabIndex = 138
        Me.lblActualPaidAmount.Text = "0"
        Me.lblActualPaidAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(344, 56)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(167, 20)
        Me.Label9.TabIndex = 143
        Me.Label9.Text = "Discounted Amount"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(344, 23)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(167, 20)
        Me.Label5.TabIndex = 139
        Me.Label5.Text = "Actual Paid Amount"
        '
        'btnPrintOutstandingReport
        '
        Me.btnPrintOutstandingReport.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.btnPrintOutstandingReport.Location = New System.Drawing.Point(1683, 112)
        Me.btnPrintOutstandingReport.Name = "btnPrintOutstandingReport"
        Me.btnPrintOutstandingReport.Size = New System.Drawing.Size(228, 49)
        Me.btnPrintOutstandingReport.TabIndex = 126
        Me.btnPrintOutstandingReport.Text = "Print This Report"
        Me.btnPrintOutstandingReport.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnPrintOutstandingReport)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 855)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Size = New System.Drawing.Size(1916, 198)
        Me.GroupBox1.TabIndex = 136
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Total Bill Summary"
        '
        'CustomersOutstandingBalances
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1916, 1053)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox1)
        Me.Location = New System.Drawing.Point(250, 30)
        Me.Name = "CustomersOutstandingBalances"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Agni Designs - Outstanding Balance Details"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox5.ResumeLayout(False)
        CType(Me.dgCustomerBillSummary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bwtCustomerSummaryLoadThread As System.ComponentModel.BackgroundWorker
    Friend WithEvents bwtTotalBillSummaryLoadThread As System.ComponentModel.BackgroundWorker
    Friend WithEvents GroupBox5 As ElaCustomGroupBoxControl.ElaCustomGroupBox
    Friend WithEvents dgCustomerBillSummary As DataGridView
    Friend WithEvents GroupBox2 As ElaCustomGroupBoxControl.ElaCustomGroupBox
    Friend WithEvents lblTotalPaidAmount As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents lblUnpaidBillAmount As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents lblNumberOfCustomers As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblTotalBilledAmount As Label
    Friend WithEvents lblBillsCount As Label
    Friend WithEvents lblDesignsCount As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblUnBilledAmount As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents lblDiscountAmount As Label
    Friend WithEvents lblActualPaidAmount As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents btnPrintOutstandingReport As CButtonLib.CButton
    Friend WithEvents GroupBox1 As ElaCustomGroupBoxControl.ElaCustomGroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblOutstandingBalance As Label
    Friend WithEvents CompName As DataGridViewTextBoxColumn
    Friend WithEvents CustNo As DataGridViewTextBoxColumn
    Friend WithEvents designCount As DataGridViewTextBoxColumn
    Friend WithEvents BilledDesAmtNoGST As DataGridViewTextBoxColumn
    Friend WithEvents unbilledDesAmtNoGST As DataGridViewTextBoxColumn
    Friend WithEvents TotDeisgnAmtNoGST As DataGridViewTextBoxColumn
    Friend WithEvents BilledDesAmtWithGST As DataGridViewTextBoxColumn
    Friend WithEvents ActPaidAmount As DataGridViewTextBoxColumn
    Friend WithEvents Discount As DataGridViewTextBoxColumn
    Friend WithEvents FinalPaidAmount As DataGridViewTextBoxColumn
    Friend WithEvents UnPaidBilledAmount As DataGridViewTextBoxColumn
    Friend WithEvents OutstandingBalance As DataGridViewTextBoxColumn
End Class
