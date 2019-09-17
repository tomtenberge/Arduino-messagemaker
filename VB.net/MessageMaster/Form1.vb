Public Class Form1
    Public data As New MMdata
    Public currentslideshow As Integer = 0
    Public currentslide As Integer = 0
    Private workfolder As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolStripProgressBar1.Visible = False
        DataGridView1.ShowEditingIcon = False
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersVisible = False
        For a As Integer = 0 To 127
            DataGridView1.Columns.Add(a, a)
        Next
        For Each collum As DataGridViewColumn In DataGridView1.Columns
            collum.ReadOnly = True
            collum.MinimumWidth = 10
            collum.Width = 10
            collum.Resizable = DataGridViewTriState.False

        Next
        For a As Integer = 0 To 14
            DataGridView1.Rows.Add()
        Next
        For Each row As DataGridViewRow In DataGridView1.Rows
            row.Resizable = DataGridViewTriState.False
            row.Height = 10
            For Each cell As DataGridViewCell In row.Cells
                cell.Style.BackColor = Color.Black
            Next
        Next
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        DataGridView1.ClearSelection()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If (DataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Style.BackColor = Color.Black) Then
            DataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Style.BackColor = Color.Red
        ElseIf (DataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Style.BackColor = Color.Red) Then
            DataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Style.BackColor = Color.Green
        ElseIf (DataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Style.BackColor = Color.Green) Then
            DataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Style.BackColor = Color.Orange
        ElseIf (DataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Style.BackColor = Color.Orange) Then
            DataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Style.BackColor = Color.Black
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        Dim a As DialogResult = MessageBox.Show("Do you want to save any unsaved changes?\n All changes will be lost", "Save file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
        Select Case a
            Case DialogResult.Yes
                Savefile()
                Newfile()
            Case DialogResult.No
                Newfile()
        End Select
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer
        FolderBrowserDialog1.ShowNewFolderButton = True
        FolderBrowserDialog1.Description = "Please select the working folder."
        FolderBrowserDialog1.ShowDialog()
        If FolderBrowserDialog1.SelectedPath IsNot "" Then
            workfolder = FolderBrowserDialog1.SelectedPath
        End If
        Loadfile()
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer
        FolderBrowserDialog1.ShowNewFolderButton = True
        FolderBrowserDialog1.Description = "Please select the working folder." + vbNewLine + "Warning, all files in current folder will be deleted"
        FolderBrowserDialog1.ShowDialog()
        If FolderBrowserDialog1.SelectedPath = "" Then
            Exit Sub
        End If
        workfolder = FolderBrowserDialog1.SelectedPath
        Savefile()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Savefile()
    End Sub

    Private Sub Go_btn1_Click(sender As Object, e As EventArgs) Handles go_btn1.Click

    End Sub
    Private Sub Savefile()
        ToolStripProgressBar1.Visible = True
        ToolStripProgressBar1.Value = 0
        ToolStripProgressBar1.Maximum = 1000
        My.Computer.FileSystem.CurrentDirectory = workfolder
        Reloadscreen(True)
        Dim infodata As New List(Of Byte)
        For Each c As MMslideshow In data.slideshows
            infodata.Add(Convert.ToByte(c.slideshownr))
            infodata.Add(Convert.ToByte(c.slidesactive))
            infodata.Add(Convert.ToByte(c.seconds_slide))
            For Each d As MMslide In c.slides
                Dim rowdata As New List(Of Byte)
                For Each e As MMrow In d.rows
                    For Each f As MMpixel In e.pixels
                        rowdata.Add(Convert.ToByte(f.Colornumber))
                    Next
                Next
                My.Computer.FileSystem.WriteAllBytes(Makefilename(c.slideshownr, d.slidenr), rowdata.ToArray, False)
                ToolStripProgressBar1.Value += 1
            Next
        Next
        My.Computer.FileSystem.WriteAllBytes(workfolder + "\info.mmi", infodata.ToArray, False) 'write main info file
        ToolStripProgressBar1.Visible = False
    End Sub
    Private Function Makefilename(ByVal slideshownr As Integer, ByVal slidenr As Integer) As String
        Dim filename As String = ""
        filename += workfolder
        filename += "\"
        If slideshownr < 10 Then
            filename = filename + "0" + slideshownr.ToString()
        Else
            filename += slideshownr.ToString()
        End If
        If slidenr < 10 Then
            filename = filename + "0" + slidenr.ToString()
        Else
            filename += slidenr.ToString()
        End If
        filename += ".mms"
        Return filename
    End Function
    Private Sub Loadfile()
        Newfile()
        ToolStripProgressBar1.Visible = True
        ToolStripProgressBar1.Value = 0
        ToolStripProgressBar1.Maximum = 1000
        My.Computer.FileSystem.CurrentDirectory = workfolder
        Dim infodata() = My.Computer.FileSystem.ReadAllBytes(workfolder + "\info.mmi")
        Dim i As Integer = 0
        While (i < 10)
            data.slideshows(i).slidesactive = CInt(infodata((i * 3) + 1))
            data.slideshows(i).seconds_slide = CInt(infodata((i * 3) + 2))
            Dim j As Integer = 0
            While (j < 100)
                Dim realdata() = My.Computer.FileSystem.ReadAllBytes(Makefilename(i, j))
                Dim n As Integer = 0
                Dim k As Integer = 0
                While (k < 16)
                    Dim l As Integer = 0
                    While (l < 128)
                        data.slideshows(i).slides(j).rows(k).pixels(l).Colornumber = CInt(realdata(n))
                        n += 1
                        l += 1
                    End While
                    k += 1
                End While
                j += 1
            End While
            i += 1
        End While
        'reload the screen
        seconds_slide.Value = data.slideshows(CInt(slideshowselect.Value)).seconds_slide
        Slidescount.Value = data.slideshows(CInt(slideshowselect.Value)).slidesactive
        For b As Integer = 0 To 15
            For a As Integer = 0 To 127
                Select Case data.slideshows(CInt(slideshowselect.Value)).slides(CInt(slideselect.Value)).rows(b).pixels(a).Colornumber
                    Case 0
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Black
                    Case 1
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Green
                    Case 2
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Red
                    Case 3
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Orange
                    Case Else
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Black
                End Select
            Next
        Next
        currentslideshow = slideshowselect.Value
        currentslide = slideselect.Value
        ToolStripProgressBar1.Visible = False
    End Sub
    Private Sub Newfile()
        slideshowselect.Value = 0
        slideselect.Value = 0
        data = New MMdata
        currentslideshow = slideshowselect.Value
        currentslide = slideselect.Value
        seconds_slide.Value = 5
        Slidescount.Value = 1
        For b As Integer = 0 To 15
            For a As Integer = 0 To 127
                Select Case data.slideshows(CInt(slideshowselect.Value)).slides(CInt(slideselect.Value)).rows(b).pixels(a).Colornumber
                    Case 0
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Black
                    Case 1
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Green
                    Case 2
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Red
                    Case 3
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Orange
                    Case Else
                        DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Black
                End Select
            Next
        Next
    End Sub

    Private Sub Slideselect_ValueChanged(sender As Object, e As EventArgs) Handles slideselect.ValueChanged
        Reloadscreen()
    End Sub
    Private Sub Reloadscreen(Optional ByVal force As Boolean = False)
        'Dim goo As Boolean = False
        If slideshowselect.Value.ToString IsNot currentslideshow.ToString Then
            force = True
        End If
        If slideselect.Value.ToString IsNot currentslide.ToString Then
            force = True
        End If
        If force Then
            data.slideshows(currentslideshow).seconds_slide = seconds_slide.Value
            data.slideshows(currentslideshow).slidesactive = Slidescount.Value
            seconds_slide.Value = data.slideshows(CInt(slideshowselect.Value)).seconds_slide
            Slidescount.Value = data.slideshows(CInt(slideshowselect.Value)).slidesactive
            For b As Integer = 0 To 15
                For a As Integer = 0 To 127
                    Select Case DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor
                        Case Color.Black
                            data.slideshows(currentslideshow).slides(currentslide).rows(b).pixels(a).Colornumber = 0
                        Case Color.Red
                            data.slideshows(currentslideshow).slides(currentslide).rows(b).pixels(a).Colornumber = 2
                        Case Color.Green
                            data.slideshows(currentslideshow).slides(currentslide).rows(b).pixels(a).Colornumber = 1
                        Case Color.Orange
                            data.slideshows(currentslideshow).slides(currentslide).rows(b).pixels(a).Colornumber = 3
                        Case Else
                            data.slideshows(currentslideshow).slides(currentslide).rows(b).pixels(a).Colornumber = 0
                    End Select
                    Select Case data.slideshows(CInt(slideshowselect.Value)).slides(CInt(slideselect.Value)).rows(b).pixels(a).Colornumber
                        Case 0
                            DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Black
                        Case 1
                            DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Green
                        Case 2
                            DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Red
                        Case 3
                            DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Orange
                        Case Else
                            DataGridView1.Rows.Item(b).Cells.Item(a).Style.BackColor = Color.Black
                    End Select
                Next
            Next
            currentslideshow = slideshowselect.Value
            currentslide = slideselect.Value
        End If
    End Sub

    Private Sub Slideshowselect_ValueChanged(sender As Object, e As EventArgs) Handles slideshowselect.ValueChanged
        Reloadscreen()
    End Sub

    Private Sub ToolStripProgressBar1_Click(sender As Object, e As EventArgs) Handles ToolStripProgressBar1.Click

    End Sub
End Class
