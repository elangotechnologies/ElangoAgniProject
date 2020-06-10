Public Class Loading

    Public gImplicitHandling As Boolean = True

    Private Sub Loading_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If gImplicitHandling Then
            Dim random As New Random()

            Dim resourceName As String = "loader" + Convert.ToString(random.Next(1, 12))
            pbLoadingImage.Image = My.Resources.ResourceManager.GetObject(resourceName)

            Timer1.Interval = 1000
            Timer1.Start()
        End If

    End Sub

    Private Sub Loading_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        AgniMainForm.btnLogOff.Text = "Log Off " + Login.cmbLoginUserName.Text.Trim
        AgniMainForm.tabAllTabsHolder.SelectedIndex = 0
        AgniMainForm.Show()
        Login.Hide()

        Me.Close()
    End Sub
End Class