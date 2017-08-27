Public Class ImagePopup
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles pbImageBox.Click

    End Sub

    Public Sub loadImage(image As Image)
        pbImageBox.Image = image
    End Sub

    Public Sub clearImage(image As Image)
        pbImageBox.Image = Nothing
    End Sub

End Class