namespace FileCompare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";
                // 현재 텍스트박스에 있는 경로를 초기 선택 폴더로 설정
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) &&
                Directory.Exists(txtLeftDir.Text))
                {
                    dlg.SelectedPath = txtLeftDir.Text;
                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtLeftDir.Text = dlg.SelectedPath;
                    PopulateListView(lvwLeftDir, dlg.SelectedPath);
                    CompareListViews();
                }
                
            }
        }

        private void btnRightDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";
                // 현재 텍스트박스에 있는 경로를 초기 선택 폴더로 설정
                if (!string.IsNullOrWhiteSpace(txtRightDir.Text) &&
                Directory.Exists(txtRightDir.Text))
                {
                    dlg.SelectedPath = txtRightDir.Text;
                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtRightDir.Text = dlg.SelectedPath;
                    PopulateListView(lvwrightDir, dlg.SelectedPath);
                    CompareListViews();
                }

            }
        }
        private void PopulateListView(ListView lv, string folderPath)
        {
            lv.BeginUpdate(); // 화면 깜빡임 방지
            lv.Items.Clear(); // 기존 목록 초기화

            try
            {
                // [1] 하위 폴더 목록 가져오기 및 추가
                var dirs = Directory.EnumerateDirectories(folderPath)
                            .Select(p => new DirectoryInfo(p))
                            .OrderBy(d => d.Name);

                foreach (var d in dirs)
                {
                    var item = new ListViewItem(d.Name);
                    item.SubItems.Add("<DIR>"); // 폴더는 크기 대신 <DIR> 표시
                    item.SubItems.Add(d.LastWriteTime.ToString("g"));
                    lv.Items.Add(item);
                }

                // [2] 파일 목록 가져오기 및 추가
                var files = Directory.EnumerateFiles(folderPath)
                            .Select(p => new FileInfo(p))
                            .OrderBy(f => f.Name);

                foreach (var f in files)
                {
                    var item = new ListViewItem(f.Name);
                    item.SubItems.Add(f.Length.ToString("N0") + " 바이트");
                    item.SubItems.Add(f.LastWriteTime.ToString("g"));
                    lv.Items.Add(item);
                }

                // [3] 리스트뷰 컬럼 너비 자동 조정
                for (int i = 0; i < lv.Columns.Count; i++)
                {
                    lv.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(this, "폴더를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "입출력 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lv.EndUpdate(); // 업데이트 완료 후 화면 갱신
            }

        }
        private void CompareListViews()
        {
            // 1. 왼쪽 리스트 기준으로 오른쪽과 비교
            foreach (ListViewItem leftItem in lvwLeftDir.Items)
            {
                // 오른쪽 리스트에서 같은 이름의 파일을 찾음
                ListViewItem rightItem = null;
                foreach (ListViewItem item in lvwrightDir.Items)
                {
                    if (item.Text == leftItem.Text)
                    {
                        rightItem = item;
                        break;
                    }
                }

                if (rightItem != null) // 양쪽에 이름이 같은 파일이 있는 경우
                {
                    DateTime leftDate = DateTime.Parse(leftItem.SubItems[2].Text);
                    DateTime rightDate = DateTime.Parse(rightItem.SubItems[2].Text);

                    if (leftDate > rightDate) // 왼쪽이 더 최신 (New)
                    {
                        leftItem.ForeColor = Color.Red;
                        rightItem.ForeColor = Color.Gray;
                    }
                    else if (leftDate < rightDate) // 오른쪽이 더 최신 (New)
                    {
                        leftItem.ForeColor = Color.Gray;
                        rightItem.ForeColor = Color.Red;
                    }
                    else // 날짜까지 동일한 파일
                    {
                        leftItem.ForeColor = Color.Black;
                        rightItem.ForeColor = Color.Black;
                    }
                }
                else // 왼쪽 폴더에만 존재하는 단독 파일
                {
                    leftItem.ForeColor = Color.Purple;
                }
            }

            // 2. 오른쪽 리스트에만 있는 단독 파일 처리
            foreach (ListViewItem rightItem in lvwrightDir.Items)
            {
                bool existsOnLeft = false;
                foreach (ListViewItem leftItem in lvwLeftDir.Items)
                {
                    if (leftItem.Text == rightItem.Text)
                    {
                        existsOnLeft = true;
                        break;
                    }
                }

                if (!existsOnLeft)
                {
                    rightItem.ForeColor = Color.Purple;
                }
            }
        }

    }

}
