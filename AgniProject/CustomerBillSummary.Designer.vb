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
        Me.lblBillsCount = New System.Windows.Forms.Label()
        Me.dgCustomerBillSummary = New System.Windows.Forms.DataGrid()
        Me.lblDesignsCount = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button21 = New System.Windows.Forms.Button()
        Me.lblUnBilledAmount = New System.Windows.Forms.Label()
        Me.bwtCustomerSummaryLoadThread = New System.ComponentModel.BackgroundWorker()
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn2 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn3 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn4 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn5 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn6 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn7 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn8 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn9 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn10 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.DataGridTextBoxColumn11 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.bwtTotalBillSummaryLoadThread = New System.ComponentModel.BackgroundWorker()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblActualPaidAmount = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblTaxDeductedAmount = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblDiscountAmount = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTotalBilledAmount = New System.Windows.Forms.Label()
        Me.lblTotalPaidAmount = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblNetBalance = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        CType(Me.dgCustomerBillSummary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBillsCount
        '
        Me.lblBillsCount.AutoSize = True
        Me.lblBillsCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblBillsCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillsCount.ForeColor = System.Drawing.Color.White
        Me.lblBillsCount.Location = New System.Drawing.Point(291, 68)
        Me.lblBillsCount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBillsCount.Name = "lblBillsCount"
        Me.lblBillsCount.Size = New System.Drawing.Size(24, 25)
        Me.lblBillsCount.TabIndex = 122
        Me.lblBillsCount.Text = "0"
        '
        'dgCustomerBillSummary
        '
        Me.dgCustomerBillSummary.AlternatingBackColor = System.Drawing.Color.GhostWhite
        Me.dgCustomerBillSummary.BackColor = System.Drawing.Color.GhostWhite
        Me.dgCustomerBillSummary.BackgroundColor = System.Drawing.Color.Lavender
        Me.dgCustomerBillSummary.CaptionBackColor = System.Drawing.Color.RoyalBlue
        Me.dgCustomerBillSummary.CaptionForeColor = System.Drawing.Color.White
        Me.dgCustomerBillSummary.CaptionText = "All Companies Outstanding Balance"
        Me.dgCustomerBillSummary.DataMember = ""
        Me.dgCustomerBillSummary.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgCustomerBillSummary.FlatMode = True
        Me.dgCustomerBillSummary.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgCustomerBillSummary.ForeColor = System.Drawing.Color.MidnightBlue
        Me.dgCustomerBillSummary.GridLineColor = System.Drawing.Color.RoyalBlue
        Me.dgCustomerBillSummary.HeaderBackColor = System.Drawing.Color.MidnightBlue
        Me.dgCustomerBillSummary.HeaderFont = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.dgCustomerBillSummary.HeaderForeColor = System.Drawing.Color.Lavender
        Me.dgCustomerBillSummary.LinkColor = System.Drawing.Color.Teal
        Me.dgCustomerBillSummary.Location = New System.Drawing.Point(0, 0)
        Me.dgCustomerBillSummary.Name = "dgCustomerBillSummary"
        Me.dgCustomerBillSummary.ParentRowsBackColor = System.Drawing.Color.Lavender
        Me.dgCustomerBillSummary.ParentRowsForeColor = System.Drawing.Color.MidnightBlue
        Me.dgCustomerBillSummary.ReadOnly = True
        Me.dgCustomerBillSummary.SelectionBackColor = System.Drawing.Color.Teal
        Me.dgCustomerBillSummary.SelectionForeColor = System.Drawing.Color.PaleGreen
        Me.dgCustomerBillSummary.Size = New System.Drawing.Size(1152, 656)
        Me.dgCustomerBillSummary.TabIndex = 123
        Me.dgCustomerBillSummary.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        '
        'lblDesignsCount
        '
        Me.lblDesignsCount.AutoSize = True
        Me.lblDesignsCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblDesignsCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesignsCount.ForeColor = System.Drawing.Color.White
        Me.lblDesignsCount.Location = New System.Drawing.Point(291, 30)
        Me.lblDesignsCount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDesignsCount.Name = "lblDesignsCount"
        Me.lblDesignsCount.Size = New System.Drawing.Size(24, 25)
        Me.lblDesignsCount.TabIndex = 125
        Me.lblDesignsCount.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(56, 30)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(158, 25)
        Me.Label2.TabIndex = 124
        Me.Label2.Text = " No.Of Designs"
        '
        'Button21
        '
        Me.Button21.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Button21.Location = New System.Drawing.Point(761, 254)
        Me.Button21.Margin = New System.Windows.Forms.Padding(4)
        Me.Button21.Name = "Button21"
        Me.Button21.Size = New System.Drawing.Size(384, 41)
        Me.Button21.TabIndex = 126
        Me.Button21.Text = "Print Report"
        Me.Button21.UseVisualStyleBackColor = True
        '
        'lblUnBilledAmount
        '
        Me.lblUnBilledAmount.AutoSize = True
        Me.lblUnBilledAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblUnBilledAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnBilledAmount.ForeColor = System.Drawing.Color.White
        Me.lblUnBilledAmount.Location = New System.Drawing.Point(673, 25)
        Me.lblUnBilledAmount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUnBilledAmount.Name = "lblUnBilledAmount"
        Me.lblUnBilledAmount.Size = New System.Drawing.Size(24, 25)
        Me.lblUnBilledAmount.TabIndex = 128
        Me.lblUnBilledAmount.Text = "0"
        '
        'bwtCustomerSummaryLoadThread
        '
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.dgCustomerBillSummary
        Me.DataGridTableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn1, Me.DataGridTextBoxColumn2, Me.DataGridTextBoxColumn3, Me.DataGridTextBoxColumn4, Me.DataGridTextBoxColumn5, Me.DataGridTextBoxColumn6, Me.DataGridTextBoxColumn7, Me.DataGridTextBoxColumn8, Me.DataGridTextBoxColumn9, Me.DataGridTextBoxColumn10, Me.DataGridTextBoxColumn11})
        Me.DataGridTableStyle1.GridLineColor = System.Drawing.Color.Indigo
        Me.DataGridTableStyle1.HeaderBackColor = System.Drawing.Color.Navy
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.Color.White
        Me.DataGridTableStyle1.LinkColor = System.Drawing.SystemColors.InactiveBorder
        Me.DataGridTableStyle1.MappingName = "BillSummary"
        Me.DataGridTableStyle1.PreferredColumnWidth = 200
        Me.DataGridTableStyle1.RowHeaderWidth = 200
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.HeaderText = "CustomerName"
        Me.DataGridTextBoxColumn1.MappingName = "CompName"
        Me.DataGridTextBoxColumn1.Width = 180
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Format = "0"
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.HeaderText = "DesignCount"
        Me.DataGridTextBoxColumn2.MappingName = "designCount"
        '
        'DataGridTextBoxColumn3
        '
        Me.DataGridTextBoxColumn3.Format = "0.00"
        Me.DataGridTextBoxColumn3.FormatInfo = Nothing
        Me.DataGridTextBoxColumn3.HeaderText = "BilledDesAmtNoGST"
        Me.DataGridTextBoxColumn3.MappingName = "billedDesAmtNoGST"
        Me.DataGridTextBoxColumn3.Width = 140
        '
        'DataGridTextBoxColumn4
        '
        Me.DataGridTextBoxColumn4.Format = "0.00"
        Me.DataGridTextBoxColumn4.FormatInfo = Nothing
        Me.DataGridTextBoxColumn4.HeaderText = "UnBilledDesAmtNoGST"
        Me.DataGridTextBoxColumn4.MappingName = "unbilledDesAmtNoGST"
        Me.DataGridTextBoxColumn4.Width = 150
        '
        'DataGridTextBoxColumn5
        '
        Me.DataGridTextBoxColumn5.Format = "0.00"
        Me.DataGridTextBoxColumn5.FormatInfo = Nothing
        Me.DataGridTextBoxColumn5.HeaderText = "TotDeisgnAmtNoGST"
        Me.DataGridTextBoxColumn5.MappingName = "TotDeisgnAmtNoGST"
        Me.DataGridTextBoxColumn5.Width = 140
        '
        'DataGridTextBoxColumn6
        '
        Me.DataGridTextBoxColumn6.Format = "0.00"
        Me.DataGridTextBoxColumn6.FormatInfo = Nothing
        Me.DataGridTextBoxColumn6.HeaderText = "BilledDesAmtWithGST"
        Me.DataGridTextBoxColumn6.MappingName = "billedDesAmtWithGST"
        Me.DataGridTextBoxColumn6.Width = 140
        '
        'DataGridTextBoxColumn7
        '
        Me.DataGridTextBoxColumn7.Format = "0.00"
        Me.DataGridTextBoxColumn7.FormatInfo = Nothing
        Me.DataGridTextBoxColumn7.HeaderText = "ActualPaidAmount"
        Me.DataGridTextBoxColumn7.MappingName = "ActPaidAmount"
        Me.DataGridTextBoxColumn7.Width = 120
        '
        'DataGridTextBoxColumn8
        '
        Me.DataGridTextBoxColumn8.Format = "0.00"
        Me.DataGridTextBoxColumn8.FormatInfo = Nothing
        Me.DataGridTextBoxColumn8.HeaderText = "Discounts"
        Me.DataGridTextBoxColumn8.MappingName = "Discount"
        '
        'DataGridTextBoxColumn9
        '
        Me.DataGridTextBoxColumn9.Format = "0.00"
        Me.DataGridTextBoxColumn9.FormatInfo = Nothing
        Me.DataGridTextBoxColumn9.HeaderText = "TaxDeductions"
        Me.DataGridTextBoxColumn9.MappingName = "TaxDeduction"
        '
        'DataGridTextBoxColumn10
        '
        Me.DataGridTextBoxColumn10.Format = "0.00"
        Me.DataGridTextBoxColumn10.FormatInfo = Nothing
        Me.DataGridTextBoxColumn10.HeaderText = "FinalPaidAmount"
        Me.DataGridTextBoxColumn10.MappingName = "FinalPaidAmount"
        Me.DataGridTextBoxColumn10.Width = 110
        '
        'DataGridTextBoxColumn11
        '
        Me.DataGridTextBoxColumn11.Format = "0.00"
        Me.DataGridTextBoxColumn11.FormatInfo = Nothing
        Me.DataGridTextBoxColumn11.HeaderText = "NetBalance"
        Me.DataGridTextBoxColumn11.MappingName = "NetBalance"
        '
        'bwtTotalBillSummaryLoadThread
        '
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblNetBalance)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.lblTotalPaidAmount)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.lblTotalBilledAmount)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.lblDiscountAmount)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.lblTaxDeductedAmount)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.lblActualPaidAmount)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.lblUnBilledAmount)
        Me.GroupBox1.Controls.Add(Me.Button21)
        Me.GroupBox1.Controls.Add(Me.lblBillsCount)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.lblDesignsCount)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox1.Location = New System.Drawing.Point(0, 681)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1152, 308)
        Me.GroupBox1.TabIndex = 136
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(56, 68)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(120, 25)
        Me.Label11.TabIndex = 136
        Me.Label11.Text = " No.Of Bills"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Black
        Me.Label12.Location = New System.Drawing.Point(464, 25)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(178, 25)
        Me.Label12.TabIndex = 137
        Me.Label12.Text = "Un Billed Amount"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(805, 25)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(202, 25)
        Me.Label5.TabIndex = 139
        Me.Label5.Text = "Actual Paid Amount"
        '
        'lblActualPaidAmount
        '
        Me.lblActualPaidAmount.AutoSize = True
        Me.lblActualPaidAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblActualPaidAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActualPaidAmount.ForeColor = System.Drawing.Color.White
        Me.lblActualPaidAmount.Location = New System.Drawing.Point(1052, 25)
        Me.lblActualPaidAmount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblActualPaidAmount.Name = "lblActualPaidAmount"
        Me.lblActualPaidAmount.Size = New System.Drawing.Size(24, 25)
        Me.lblActualPaidAmount.TabIndex = 138
        Me.lblActualPaidAmount.Text = "0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(805, 114)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(227, 25)
        Me.Label7.TabIndex = 141
        Me.Label7.Text = "Tax Deducted Amount"
        '
        'lblTaxDeductedAmount
        '
        Me.lblTaxDeductedAmount.AutoSize = True
        Me.lblTaxDeductedAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTaxDeductedAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxDeductedAmount.ForeColor = System.Drawing.Color.White
        Me.lblTaxDeductedAmount.Location = New System.Drawing.Point(1052, 114)
        Me.lblTaxDeductedAmount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTaxDeductedAmount.Name = "lblTaxDeductedAmount"
        Me.lblTaxDeductedAmount.Size = New System.Drawing.Size(24, 25)
        Me.lblTaxDeductedAmount.TabIndex = 140
        Me.lblTaxDeductedAmount.Text = "0"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(805, 68)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(200, 25)
        Me.Label9.TabIndex = 143
        Me.Label9.Text = "Discounted Amount"
        '
        'lblDiscountAmount
        '
        Me.lblDiscountAmount.AutoSize = True
        Me.lblDiscountAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblDiscountAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscountAmount.ForeColor = System.Drawing.Color.White
        Me.lblDiscountAmount.Location = New System.Drawing.Point(1052, 68)
        Me.lblDiscountAmount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDiscountAmount.Name = "lblDiscountAmount"
        Me.lblDiscountAmount.Size = New System.Drawing.Size(24, 25)
        Me.lblDiscountAmount.TabIndex = 142
        Me.lblDiscountAmount.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(56, 195)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(200, 25)
        Me.Label4.TabIndex = 144
        Me.Label4.Text = "Total Billed Amount"
        '
        'lblTotalBilledAmount
        '
        Me.lblTotalBilledAmount.AutoSize = True
        Me.lblTotalBilledAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTotalBilledAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalBilledAmount.ForeColor = System.Drawing.Color.White
        Me.lblTotalBilledAmount.Location = New System.Drawing.Point(309, 195)
        Me.lblTotalBilledAmount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalBilledAmount.Name = "lblTotalBilledAmount"
        Me.lblTotalBilledAmount.Size = New System.Drawing.Size(24, 25)
        Me.lblTotalBilledAmount.TabIndex = 145
        Me.lblTotalBilledAmount.Text = "0"
        '
        'lblTotalPaidAmount
        '
        Me.lblTotalPaidAmount.AutoSize = True
        Me.lblTotalPaidAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTotalPaidAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalPaidAmount.ForeColor = System.Drawing.Color.White
        Me.lblTotalPaidAmount.Location = New System.Drawing.Point(695, 195)
        Me.lblTotalPaidAmount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalPaidAmount.Name = "lblTotalPaidAmount"
        Me.lblTotalPaidAmount.Size = New System.Drawing.Size(24, 25)
        Me.lblTotalPaidAmount.TabIndex = 147
        Me.lblTotalPaidAmount.Text = "0"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.Black
        Me.Label15.Location = New System.Drawing.Point(442, 195)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(190, 25)
        Me.Label15.TabIndex = 146
        Me.Label15.Text = "Total Paid Amount"
        '
        'lblNetBalance
        '
        Me.lblNetBalance.AutoSize = True
        Me.lblNetBalance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblNetBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetBalance.ForeColor = System.Drawing.Color.White
        Me.lblNetBalance.Location = New System.Drawing.Point(1052, 195)
        Me.lblNetBalance.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNetBalance.Name = "lblNetBalance"
        Me.lblNetBalance.Size = New System.Drawing.Size(24, 25)
        Me.lblNetBalance.TabIndex = 149
        Me.lblNetBalance.Text = "0"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.Black
        Me.Label17.Location = New System.Drawing.Point(807, 195)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(184, 25)
        Me.Label17.TabIndex = 148
        Me.Label17.Text = "Total Net Balance"
        '
        'CustomerBillSummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1152, 989)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.dgCustomerBillSummary)
        Me.Location = New System.Drawing.Point(250, 30)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "CustomerBillSummary"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Agni Designs - Outstanding Balance Details"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgCustomerBillSummary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBillsCount As System.Windows.Forms.Label
    Friend WithEvents dgCustomerBillSummary As System.Windows.Forms.DataGrid
    Friend WithEvents lblDesignsCount As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button21 As System.Windows.Forms.Button
    Friend WithEvents lblUnBilledAmount As System.Windows.Forms.Label
    Friend WithEvents bwtCustomerSummaryLoadThread As System.ComponentModel.BackgroundWorker
    Friend WithEvents DataGridTableStyle1 As DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn3 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn4 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn5 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn6 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn7 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn8 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn9 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn10 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn11 As DataGridTextBoxColumn
    Friend WithEvents bwtTotalBillSummaryLoadThread As System.ComponentModel.BackgroundWorker
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblNetBalance As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents lblTotalPaidAmount As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents lblTotalBilledAmount As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblDiscountAmount As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents lblTaxDeductedAmount As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblActualPaidAmount As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
End Class
