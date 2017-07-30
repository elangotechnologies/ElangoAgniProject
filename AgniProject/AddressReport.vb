Imports System.Data.SqlClient
Imports System.IO
Public Class AddressReport
    Dim Con As SqlConnection
    Dim cmd1 As SqlCommand
    Dim Sda1 As SqlDataAdapter
    Dim Ds1, ds8 As DataSet
    Dim Dt1, dt8 As DataTable
    Dim Dr1, dr8, dr4, dr5, dr6 As DataRow
    Dim Dc1, dc8 As DataColumn
    Dim Scb1 As SqlCommandBuilder
    Dim objrpt As New Address

    Private Sub AddressReport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        objrpt.Dispose()
    End Sub
    
    Private Sub AddressReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim j As Int16
            ds8 = New DataSet
            dt8 = New DataTable
            Dim addr As String
            ds8.Tables.Add(dt8)
            dc8 = New DataColumn("Address", Type.GetType("System.String"))
            dt8.Columns.Add(dc8)
            addr = AgnimainForm.cmbCustCompanyList.Text + vbNewLine + AgnimainForm.txtAddress.Text + vbNewLine + "Mobile : " + AgnimainForm.txtMobile.Text
            For j = 1 To AgnimainForm.addrcount
                dr8 = dt8.NewRow
                dr8.Item(0) = addr
                dt8.Rows.Add(dr8)
            Next
            objrpt.SetDataSource(ds8.Tables(0))
            CrystalReportViewer1.ReportSource = objrpt
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
End Class