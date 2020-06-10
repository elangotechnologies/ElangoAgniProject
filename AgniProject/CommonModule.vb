Imports System.Data.SqlClient
Imports VB = Microsoft.VisualBasic

Module CommonModule

    Private dbConnection As SqlConnection = Nothing
    Private dbOpenSyncLock As New Object

    Public USER_TYPE_ADMINISTRATOR As Integer = 1
    Public USER_TYPE_GUEST As Integer = 2

    Public SEARCH_BY_CUSTOMER As Integer = 1
    Public SEARCH_BY_BILL_NO As Integer = 2
    Public SEARCH_BY_DESIGN_SELECTION As Integer = 4
    Public SEARCH_BY_DESIGN_NO As Integer = 8
    Public SEARCH_BY_DATE_RANGE As Integer = 16

    Public ATTRIBUTE_ADDRESS_LINE_1 As String = "company_address_line_1"
    Public ATTRIBUTE_ADDRESS_LINE_2 As String = "company_address_line_2"
    Public ATTRIBUTE_ADDRESS_LINE_3 As String = "company_address_line_3"
    Public ATTRIBUTE_ADDRESS_LINE_4 As String = "company_address_line_4"
    Public ATTRIBUTE_ADDRESS_LINE_5 As String = "company_address_line_5"

    Private valueToConvert As String
    Private n, intpart, realpart, numchar, intword, realword, spltval, spltword As String
    Private flag As Boolean

    Public Function getDBConnection() As SqlConnection
        SyncLock dbOpenSyncLock
            If (dbConnection Is Nothing OrElse dbConnection.State <> ConnectionState.Open) Then
                'dbConnection = New SqlConnection("server=DESKTOP-EHEMD7K\ELASQLEXPRESS;Database=agnidatabase;Integrated Security=true; MultipleActiveResultSets=True;")
                dbConnection = New SqlConnection("server=AGNI\SQLEXPRESS;Database=agnidatabase;Integrated Security=true; MultipleActiveResultSets=True;")
                dbConnection.Open()
            End If
            Return dbConnection
        End SyncLock
    End Function

    Public Sub closeDBConnection()
        SyncLock dbOpenSyncLock
            If dbConnection IsNot Nothing Then
                dbConnection.Close()
                dbConnection = Nothing
            End If
        End SyncLock
    End Sub

    Function getAttribute(attributeName As String) As String
        Dim attributeQuery = New SqlCommand("select * from attributes where AttributeName='" + attributeName + "'", getDBConnection())
        Dim attributeAdapter = New SqlDataAdapter()
        attributeAdapter.SelectCommand = attributeQuery
        Dim attributeDataSet = New DataSet
        attributeAdapter.Fill(attributeDataSet, "attributes")
        Dim attributeTable As DataTable = attributeDataSet.Tables(0)

        If attributeTable.Rows.Count > 0 Then
            Return attributeTable.Rows(0).Item("AttributeValue")
        End If
        Return Nothing
    End Function

    Public Sub insertOrReplaceAttribute(attributeName As String, attributeValue As String)

        Dim query As String = String.Empty
        query &= "begin tran
           update attributes with (serializable) set AttributeValue =  @attributeValue
           where AttributeName = @attributeName
           if @@rowcount = 0
           begin
              insert into attributes (AttributeName, AttributeValue) values (@attributeName, @attributeValue)
           end
        commit tran"

        Using comm As New SqlCommand()
            With comm
                .Connection = getDBConnection()
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@attributeName", attributeName)
                .Parameters.AddWithValue("@attributeValue", attributeValue)
            End With
            comm.ExecuteNonQuery()
        End Using
    End Sub


    Public Function getAmountInWords(valueToConvert As String) As String
        n = ""
        intpart = ""
        realpart = ""
        numchar = ""
        intword = ""
        realword = ""
        spltval = ""
        spltword = ""
        Dim amountInWords = ""
        If valueToConvert = "" Then
            Return "None"
        End If

        If valueToConvert = "." Then
            valueToConvert = "0.00"
        End If

        If Not valueToConvert.Contains(".") Then
            valueToConvert = valueToConvert + ".00"
        End If

        intpart = Format(Int(valueToConvert), "000000000")
        realpart = VB.Right(valueToConvert, 2)

        spltval = realpart
        Call ValFind()
        If spltword <> "" Then realword = spltword
        spltval = Mid(intpart, 1, 2)
        Call ValFind()
        If spltword <> "" Then intword = spltword + "Crore "
        spltval = Mid(intpart, 3, 2)
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword + "Lakh "
        spltval = Mid(intpart, 5, 2)
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword + "Thousand "
        n = Mid(intpart, 7, 1)
        Call ONES()
        If numchar <> "" Then intword = intword + numchar + "Hundred "
        spltval = Mid(intpart, 8, 2)
        If intword <> "" And Val(spltval) > 0 And realword = "" Then intword = intword + "and "
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword
        If intword <> "" And realword <> "" Then amountInWords = intword + "and " + realword + " Paise Only"
        If intword <> "" And realword = "" Then amountInWords = intword + "Only"
        If intword = "" And realword <> "" Then amountInWords = "Paise: " + realword + "Only"

        If amountInWords Is String.Empty Then
            amountInWords = "Zero Only"
        End If

        Return amountInWords
    End Function

    Private Sub ValFind()
        n = ""
        spltword = ""
        If Val(spltval) = 0 Then Exit Sub
        n = VB.Left(spltval, 1)
        Call TENS()
        spltword = numchar
        If flag = False Then n = VB.Right(spltval, 1) : Call ONES() : spltword = spltword + numchar
    End Sub

    Private Sub ONES()
        numchar = ""
        If n = 0 Then numchar = ""
        If n = 1 Then numchar = "One "
        If n = 2 Then numchar = "Two "
        If n = 3 Then numchar = "Three "
        If n = 4 Then numchar = "Four "
        If n = 5 Then numchar = "Five "
        If n = 6 Then numchar = "Six "
        If n = 7 Then numchar = "Seven "
        If n = 8 Then numchar = "Eight "
        If n = 9 Then numchar = "Nine "
    End Sub

    Private Sub TENS()
        numchar = ""
        If n = 1 Then n = VB.Right(spltval, 1) : Call TEENS() : flag = True : Exit Sub Else flag = False
        If n = 0 Then numchar = ""
        If n = 2 Then numchar = "Twenty "
        If n = 3 Then numchar = "Thirty "
        If n = 4 Then numchar = "Fourty "
        If n = 5 Then numchar = "Fifty "
        If n = 6 Then numchar = "Sixty "
        If n = 7 Then numchar = "Seventy "
        If n = 8 Then numchar = "Eighty "
        If n = 9 Then numchar = "Ninety "
    End Sub

    Private Sub TEENS()
        numchar = ""
        If n = 0 Then numchar = "Ten "
        If n = 1 Then numchar = "Eleven "
        If n = 2 Then numchar = "Twelve "
        If n = 3 Then numchar = "Thirteen "
        If n = 4 Then numchar = "Fourteen "
        If n = 5 Then numchar = "Fifteen "
        If n = 6 Then numchar = "Sixteen "
        If n = 7 Then numchar = "Seventeen "
        If n = 8 Then numchar = "Eighteen "
        If n = 9 Then numchar = "Nineten "
    End Sub

End Module

