Class MMdata
    Public slideshows(9) As MMslideshow
    Public Sub New()
        For a As Integer = 0 To 9
            slideshows(a) = New MMslideshow
            slideshows(a).slideshownr = a
        Next
    End Sub
End Class
Class MMslideshow
    Public slides(99) As MMslide
    Public slidesactive As Integer = 1
    Public seconds_slide As Integer = 5
    Public slideshownr As Integer = 0
    Public Sub New()
        For a As Integer = 0 To 99
            slides(a) = New MMslide
            slides(a).slidenr = a
        Next
    End Sub
End Class

Class MMslide
    Public rows(15) As MMrow
    Public slidenr As Integer = 0
    Public Sub New()
        For a As Integer = 0 To 15
            rows(a) = New MMrow
            rows(a).rownr = a
        Next
    End Sub
End Class
Class MMrow
    Public pixels(127) As MMpixel
    Public rownr As Integer = 0
    Public Sub New()
        For a As Integer = 0 To 127
            pixels(a) = New MMpixel
            pixels(a).pixelnr = a
        Next
    End Sub
End Class
Class MMpixel
    Public green As Boolean
    Public red As Boolean
    Public pixelnr As Integer = 0
    Public Sub New()
        green = False
        red = False
    End Sub
    Public Property Colornumber As Integer
        Get
            Dim a As Integer = 0
            If green Then
                a = a + 1
            End If
            If red Then
                a = a + 2
            End If
            Return a
        End Get
        Set(value As Integer)
            Select Case value
                Case 0
                    green = False
                    red = False
                Case 1
                    green = True
                    red = False
                Case 2
                    green = False
                    red = True
                Case 3
                    green = True
                    red = True
                Case Else
                    green = False
                    red = False
            End Select
        End Set
    End Property
End Class