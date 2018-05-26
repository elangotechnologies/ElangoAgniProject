Imports System.Data.SqlClient
Imports System.Threading

Public Class ChangeAddress

    Dim dbConnection As SqlConnection

    Private Sub ChangeAddress_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dbConnection = getDBConnection()

        loadAddress()
    End Sub

    Sub loadAddress()
        Dim thread As Thread = New Thread(AddressOf loadAddressdInThread)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub loadAddressdInThread()
        Dim addressLine1 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_1)
        Dim addressLine2 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_2)
        Dim addressLine3 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_3)
        Dim addressLine4 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_4)
        Dim addressLine5 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_5)

        Dim setAddressLinesInvoker As New setAddressLinesDelegate(AddressOf Me.setAddressLines)
        Me.BeginInvoke(setAddressLinesInvoker, addressLine1, addressLine2, addressLine3, addressLine4, addressLine5)

    End Sub

    Delegate Sub setAddressLinesDelegate(addressLine1 As String, addressLine2 As String, addressLine3 As String, addressLine4 As String, addressLine5 As String)

    Sub setAddressLines(addressLine1 As String, addressLine2 As String, addressLine3 As String, addressLine4 As String, addressLine5 As String)
        txtChangeAddressLine1.Text = addressLine1
        txtChangeAddressLine2.Text = addressLine2
        txtChangeAddressLine3.Text = addressLine3
        txtChangeAddressLine4.Text = addressLine4
        txtChangeAddressLine5.Text = addressLine5
    End Sub

    Private Sub btnChangeAddressCancel_Click(sender As Object, e As EventArgs) Handles btnChangeAddressCancel.ClickButtonArea
        Me.Close()
    End Sub

    Private Sub btnChangeAddressConfirm_Click(sender As Object, e As EventArgs) Handles btnChangeAddressConfirm.ClickButtonArea
        Dim addressLine1 As String = If(String.IsNullOrEmpty(txtChangeAddressLine1.Text), "", txtChangeAddressLine1.Text)
        Dim addressLine2 As String = If(String.IsNullOrEmpty(txtChangeAddressLine2.Text), "", txtChangeAddressLine2.Text)
        Dim addressLine3 As String = If(String.IsNullOrEmpty(txtChangeAddressLine3.Text), "", txtChangeAddressLine3.Text)
        Dim addressLine4 As String = If(String.IsNullOrEmpty(txtChangeAddressLine4.Text), "", txtChangeAddressLine4.Text)
        Dim addressLine5 As String = If(String.IsNullOrEmpty(txtChangeAddressLine5.Text), "", txtChangeAddressLine5.Text)

        insertOrReplaceAttribute(ATTRIBUTE_ADDRESS_LINE_1, addressLine1)
        insertOrReplaceAttribute(ATTRIBUTE_ADDRESS_LINE_2, addressLine2)
        insertOrReplaceAttribute(ATTRIBUTE_ADDRESS_LINE_3, addressLine3)
        insertOrReplaceAttribute(ATTRIBUTE_ADDRESS_LINE_4, addressLine4)
        insertOrReplaceAttribute(ATTRIBUTE_ADDRESS_LINE_5, addressLine5)

        MsgBox("Your address is successfully updated !")
    End Sub
End Class