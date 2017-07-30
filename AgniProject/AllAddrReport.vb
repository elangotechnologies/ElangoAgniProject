Imports System.Data.SqlClient
Imports System.IO
Imports System.Math
Public Class AllAddrReport
    Dim Con As SqlConnection
    Dim cmd1 As SqlCommand
    Dim Sda1 As SqlDataAdapter
    Dim Ds1, ds8 As DataSet
    Dim Dt1, dt8 As DataTable
    Dim Dr1, dr8, dr4, dr5, dr6 As DataRow
    Dim Dc1, dc8(3) As DataColumn
    Dim Scb1 As SqlCommandBuilder
    Dim i As Int16
    Dim objrpt As New AllAddress

    Private Sub AllAddrReport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        objrpt.Dispose()
    End Sub
End Class