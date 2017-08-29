<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomerBillSummary
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
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
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
        Me.bwtCustomerSummaryLoadThread = New System.ComponentModel.BackgroundWorker()
        Me.bwtTotalBillSummaryLoadThread = New System.ComponentModel.BackgroundWorker()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.dgCustomerBillSummary = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblTaxDeductedAmount = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblActualPaidAmount = New System.Windows.Forms.Label()
        Me.lblDiscountAmount = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblUnBilledAmount = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblDesignsCount = New System.Windows.Forms.Label()
        Me.lblBillsCount = New System.Windows.Forms.Label()
        Me.lblTotalBilledAmount = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblNumberOfCustomers = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblNetBalance = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblTotalPaidAmount = New System.Windows.Forms.Label()
        Me.Button21 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CompName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CustNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.designCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BilledDesAmtNoGST = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.unbilledDesAmtNoGST = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotDeisgnAmtNoGST = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BilledDesAmtWithGST = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActPaidAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Discount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TaxDeduction = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FinalPaidAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NetBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.GroupBox5.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgCustomerBillSummary.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgCustomerBillSummary.ColumnHeadersHeight = 51
        Me.dgCustomerBillSummary.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CompName, Me.CustNo, Me.designCount, Me.BilledDesAmtNoGST, Me.unbilledDesAmtNoGST, Me.TotDeisgnAmtNoGST, Me.BilledDesAmtWithGST, Me.ActPaidAmount, Me.Discount, Me.TaxDeduction, Me.FinalPaidAmount, Me.NetBalance})
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.LightSteelBlue
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgCustomerBillSummary.DefaultCellStyle = DataGridViewCellStyle13
        Me.dgCustomerBillSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgCustomerBillSummary.EnableHeadersVisualStyles = False
        Me.dgCustomerBillSummary.GridColor = System.Drawing.Color.Maroon
        Me.dgCustomerBillSummary.Location = New System.Drawing.Point(3, 26)
        Me.dgCustomerBillSummary.MultiSelect = False
        Me.dgCustomerBillSummary.Name = "dgCustomerBillSummary"
        Me.dgCustomerBillSummary.ReadOnly = True
        Me.dgCustomerBillSummary.RowHeadersVisible = False
        DataGridViewCellStyle14.BackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Comic Sans MS", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgCustomerBillSummary.RowsDefaultCellStyle = DataGridViewCellStyle14
        Me.dgCustomerBillSummary.RowTemplate.Height = 25
        Me.dgCustomerBillSummary.RowTemplate.ReadOnly = True
        Me.dgCustomerBillSummary.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgCustomerBillSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgCustomerBillSummary.Size = New System.Drawing.Size(1910, 808)
        Me.dgCustomerBillSummary.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblTotalPaidAmount)
        Me.GroupBox2.Controls.Add(Me.Label17)
        Me.GroupBox2.Controls.Add(Me.lblNetBalance)
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
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.lblDiscountAmount)
        Me.GroupBox2.Controls.Add(Me.lblActualPaidAmount)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.lblTaxDeductedAmount)
        Me.GroupBox2.Location = New System.Drawing.Point(11, 21)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox2.Size = New System.Drawing.Size(1431, 160)
        Me.GroupBox2.TabIndex = 150
        Me.GroupBox2.TabStop = False
        '
        'lblTaxDeductedAmount
        '
        Me.lblTaxDeductedAmount.AutoSize = True
        Me.lblTaxDeductedAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTaxDeductedAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxDeductedAmount.ForeColor = System.Drawing.Color.White
        Me.lblTaxDeductedAmount.Location = New System.Drawing.Point(555, 89)
        Me.lblTaxDeductedAmount.MinimumSize = New System.Drawing.Size(135, 0)
        Me.lblTaxDeductedAmount.Name = "lblTaxDeductedAmount"
        Me.lblTaxDeductedAmount.Size = New System.Drawing.Size(135, 20)
        Me.lblTaxDeductedAmount.TabIndex = 140
        Me.lblTaxDeductedAmount.Text = "0"
        Me.lblTaxDeductedAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(344, 89)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(187, 20)
        Me.Label7.TabIndex = 141
        Me.Label7.Text = "Tax Deducted Amount"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label12.ForeColor = System.Drawing.Color.Crimson
        Me.Label12.Location = New System.Drawing.Point(980, 21)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(172, 24)
        Me.Label12.TabIndex = 137
        Me.Label12.Text = "Un Billed Amount"
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
        'lblUnBilledAmount
        '
        Me.lblUnBilledAmount.AutoSize = True
        Me.lblUnBilledAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblUnBilledAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblUnBilledAmount.ForeColor = System.Drawing.Color.White
        Me.lblUnBilledAmount.Location = New System.Drawing.Point(1178, 21)
        Me.lblUnBilledAmount.MinimumSize = New System.Drawing.Size(180, 20)
        Me.lblUnBilledAmount.Name = "lblUnBilledAmount"
        Me.lblUnBilledAmount.Size = New System.Drawing.Size(180, 24)
        Me.lblUnBilledAmount.TabIndex = 128
        Me.lblUnBilledAmount.Text = "0"
        Me.lblUnBilledAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        'lblTotalBilledAmount
        '
        Me.lblTotalBilledAmount.AutoSize = True
        Me.lblTotalBilledAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTotalBilledAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblTotalBilledAmount.ForeColor = System.Drawing.Color.White
        Me.lblTotalBilledAmount.Location = New System.Drawing.Point(1178, 55)
        Me.lblTotalBilledAmount.MinimumSize = New System.Drawing.Size(180, 20)
        Me.lblTotalBilledAmount.Name = "lblTotalBilledAmount"
        Me.lblTotalBilledAmount.Size = New System.Drawing.Size(180, 24)
        Me.lblTotalBilledAmount.TabIndex = 145
        Me.lblTotalBilledAmount.Text = "0"
        Me.lblTotalBilledAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.Crimson
        Me.Label4.Location = New System.Drawing.Point(980, 55)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(140, 24)
        Me.Label4.TabIndex = 144
        Me.Label4.Text = "Billed Amount"
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
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label15.ForeColor = System.Drawing.Color.Crimson
        Me.Label15.Location = New System.Drawing.Point(980, 89)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(181, 24)
        Me.Label15.TabIndex = 146
        Me.Label15.Text = "Total Paid Amount"
        '
        'lblNetBalance
        '
        Me.lblNetBalance.AutoSize = True
        Me.lblNetBalance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblNetBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetBalance.ForeColor = System.Drawing.Color.White
        Me.lblNetBalance.Location = New System.Drawing.Point(1178, 123)
        Me.lblNetBalance.MinimumSize = New System.Drawing.Size(180, 20)
        Me.lblNetBalance.Name = "lblNetBalance"
        Me.lblNetBalance.Size = New System.Drawing.Size(180, 24)
        Me.lblNetBalance.TabIndex = 149
        Me.lblNetBalance.Text = "0"
        Me.lblNetBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.Crimson
        Me.Label17.Location = New System.Drawing.Point(980, 123)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(175, 24)
        Me.Label17.TabIndex = 148
        Me.Label17.Text = "Total Net Balance"
        '
        'lblTotalPaidAmount
        '
        Me.lblTotalPaidAmount.AutoSize = True
        Me.lblTotalPaidAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTotalPaidAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblTotalPaidAmount.ForeColor = System.Drawing.Color.White
        Me.lblTotalPaidAmount.Location = New System.Drawing.Point(1178, 89)
        Me.lblTotalPaidAmount.MinimumSize = New System.Drawing.Size(180, 20)
        Me.lblTotalPaidAmount.Name = "lblTotalPaidAmount"
        Me.lblTotalPaidAmount.Size = New System.Drawing.Size(180, 24)
        Me.lblTotalPaidAmount.TabIndex = 147
        Me.lblTotalPaidAmount.Text = "0"
        Me.lblTotalPaidAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button21
        '
        Me.Button21.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Button21.Location = New System.Drawing.Point(1680, 148)
        Me.Button21.Name = "Button21"
        Me.Button21.Size = New System.Drawing.Size(201, 33)
        Me.Button21.TabIndex = 126
        Me.Button21.Text = "Print This Report"
        Me.Button21.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button21)
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
        'CompName
        '
        Me.CompName.DataPropertyName = "CompName"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
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
        Me.CustNo.HeaderText = "Cust No"
        Me.CustNo.Name = "CustNo"
        Me.CustNo.ReadOnly = True
        Me.CustNo.Visible = False
        '
        'designCount
        '
        Me.designCount.DataPropertyName = "designCount"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.designCount.DefaultCellStyle = DataGridViewCellStyle4
        Me.designCount.HeaderText = "Design Count"
        Me.designCount.Name = "designCount"
        Me.designCount.ReadOnly = True
        Me.designCount.Width = 150
        '
        'BilledDesAmtNoGST
        '
        Me.BilledDesAmtNoGST.DataPropertyName = "BilledDesAmtNoGST"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "0.00"
        Me.BilledDesAmtNoGST.DefaultCellStyle = DataGridViewCellStyle5
        Me.BilledDesAmtNoGST.HeaderText = "Billed Amt No GST"
        Me.BilledDesAmtNoGST.Name = "BilledDesAmtNoGST"
        Me.BilledDesAmtNoGST.ReadOnly = True
        Me.BilledDesAmtNoGST.Visible = False
        Me.BilledDesAmtNoGST.Width = 190
        '
        'unbilledDesAmtNoGST
        '
        Me.unbilledDesAmtNoGST.DataPropertyName = "unbilledDesAmtNoGST"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "0.00"
        Me.unbilledDesAmtNoGST.DefaultCellStyle = DataGridViewCellStyle6
        Me.unbilledDesAmtNoGST.HeaderText = "UnBilled Amount"
        Me.unbilledDesAmtNoGST.Name = "unbilledDesAmtNoGST"
        Me.unbilledDesAmtNoGST.ReadOnly = True
        Me.unbilledDesAmtNoGST.Width = 190
        '
        'TotDeisgnAmtNoGST
        '
        Me.TotDeisgnAmtNoGST.DataPropertyName = "TotDeisgnAmtNoGST"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Format = "0.00"
        Me.TotDeisgnAmtNoGST.DefaultCellStyle = DataGridViewCellStyle7
        Me.TotDeisgnAmtNoGST.HeaderText = "Tot Deisgn Amt No GST"
        Me.TotDeisgnAmtNoGST.Name = "TotDeisgnAmtNoGST"
        Me.TotDeisgnAmtNoGST.ReadOnly = True
        Me.TotDeisgnAmtNoGST.Visible = False
        Me.TotDeisgnAmtNoGST.Width = 190
        '
        'BilledDesAmtWithGST
        '
        Me.BilledDesAmtWithGST.DataPropertyName = "BilledDesAmtWithGST"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle8.Format = "0.00"
        Me.BilledDesAmtWithGST.DefaultCellStyle = DataGridViewCellStyle8
        Me.BilledDesAmtWithGST.HeaderText = "Billed Amount"
        Me.BilledDesAmtWithGST.Name = "BilledDesAmtWithGST"
        Me.BilledDesAmtWithGST.ReadOnly = True
        Me.BilledDesAmtWithGST.Width = 190
        '
        'ActPaidAmount
        '
        Me.ActPaidAmount.DataPropertyName = "ActPaidAmount"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Format = "0.00"
        Me.ActPaidAmount.DefaultCellStyle = DataGridViewCellStyle9
        Me.ActPaidAmount.HeaderText = "Actual Paid Amount"
        Me.ActPaidAmount.Name = "ActPaidAmount"
        Me.ActPaidAmount.ReadOnly = True
        Me.ActPaidAmount.Width = 190
        '
        'Discount
        '
        Me.Discount.DataPropertyName = "Discount"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle10.Format = "0.00"
        Me.Discount.DefaultCellStyle = DataGridViewCellStyle10
        Me.Discount.HeaderText = "Discount"
        Me.Discount.Name = "Discount"
        Me.Discount.ReadOnly = True
        Me.Discount.Width = 170
        '
        'TaxDeduction
        '
        Me.TaxDeduction.DataPropertyName = "TaxDeduction"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle11.Format = "0.00"
        Me.TaxDeduction.DefaultCellStyle = DataGridViewCellStyle11
        Me.TaxDeduction.HeaderText = "Tax Deduction"
        Me.TaxDeduction.Name = "TaxDeduction"
        Me.TaxDeduction.ReadOnly = True
        Me.TaxDeduction.Width = 170
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
        'NetBalance
        '
        Me.NetBalance.DataPropertyName = "NetBalance"
        Me.NetBalance.HeaderText = "Net Balance"
        Me.NetBalance.Name = "NetBalance"
        Me.NetBalance.ReadOnly = True
        Me.NetBalance.Width = 150
        '
        'CustomerBillSummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1916, 1053)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox1)
        Me.Location = New System.Drawing.Point(250, 30)
        Me.Name = "CustomerBillSummary"
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
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents dgCustomerBillSummary As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblTotalPaidAmount As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents lblNetBalance As Label
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
    Friend WithEvents Label7 As Label
    Friend WithEvents lblDiscountAmount As Label
    Friend WithEvents lblActualPaidAmount As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblTaxDeductedAmount As Label
    Friend WithEvents Button21 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents CompName As DataGridViewTextBoxColumn
    Friend WithEvents CustNo As DataGridViewTextBoxColumn
    Friend WithEvents designCount As DataGridViewTextBoxColumn
    Friend WithEvents BilledDesAmtNoGST As DataGridViewTextBoxColumn
    Friend WithEvents unbilledDesAmtNoGST As DataGridViewTextBoxColumn
    Friend WithEvents TotDeisgnAmtNoGST As DataGridViewTextBoxColumn
    Friend WithEvents BilledDesAmtWithGST As DataGridViewTextBoxColumn
    Friend WithEvents ActPaidAmount As DataGridViewTextBoxColumn
    Friend WithEvents Discount As DataGridViewTextBoxColumn
    Friend WithEvents TaxDeduction As DataGridViewTextBoxColumn
    Friend WithEvents FinalPaidAmount As DataGridViewTextBoxColumn
    Friend WithEvents NetBalance As DataGridViewTextBoxColumn
End Class
