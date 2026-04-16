namespace FileCompare
{
    public partial class Form1 : Form
    {
        // 파일 이름을 키로 해서 파일 정보를 저장하는 사전(Dictionary)
        Dictionary<string, FileInfo> leftFiles = new Dictionary<string, FileInfo>();
        Dictionary<string, FileInfo> rightFiles = new Dictionary<string, FileInfo>();

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

                // [2] 파일 목록 가져오기 및 추가 부분 수정
                var files = Directory.EnumerateFiles(folderPath)
                            .Select(p => new FileInfo(p))
                            .OrderBy(f => f.Name);

                // 저장소 초기화 (함수 시작 부분에 넣거나 여기서 처리)
                if (lv == lvwLeftDir) leftFiles.Clear();
                else rightFiles.Clear();

                foreach (var f in files)
                {
                    var item = new ListViewItem(f.Name);
                    item.SubItems.Add(f.Length.ToString("N0") + " 바이트");
                    item.SubItems.Add(f.LastWriteTime.ToString("g"));
                    lv.Items.Add(item);

                    // 저장소에 파일 정보 추가
                    if (lv == lvwLeftDir) leftFiles[f.Name] = f;
                    else rightFiles[f.Name] = f;
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
        private void CopyFileWithConfirmation(FileInfo src, string destPath)
        {
            if (File.Exists(destPath))
            {
                FileInfo dest = new FileInfo(destPath);

                // 원본이 대상보다 '오래된' 경우에만 경고 창 표시
                if (src.LastWriteTime < dest.LastWriteTime)
                {
                    string message = "대상에 동일한 이름의 파일이 이미 있습니다.\n" +
                                     "대상 파일이 더 신규 파일입니다. 덮어쓰시겠습니까?\n\n" +
                                     "원본: " + src.FullName + " (" + src.LastWriteTime + ")\n" +
                                     "대상: " + dest.FullName + " (" + dest.LastWriteTime + ")";

                    var result = MessageBox.Show(message, "덮어쓰기 확인",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No) return; // '아니요' 누르면 복사 안 함
                }
            }

            // 조건에 안 걸리거나 '예'를 누르면 조용히 복사 (완료 알림 없음)
            File.Copy(src.FullName, destPath, true);
        }

        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {
            // 1. 왼쪽 리스트에서 선택된 항목들을 가져옵니다.
            var selectedItems = lvwLeftDir.SelectedItems;

            // 2. 선택된 게 없으면 아무것도 안 하고 끝냅니다.
            if (selectedItems.Count == 0) return;

            // 3. 하나씩 꺼내서 복사 작업을 진행합니다.
            foreach (ListViewItem item in selectedItems)
            {
                string fileName = item.Text;

                // 주머니(Dictionary)에서 원본 파일 정보를 찾습니다.
                if (leftFiles.TryGetValue(fileName, out var srcFile))
                {
                    // 오른쪽 폴더 경로와 파일 이름을 합쳐서 목적지 주소를 만듭니다.
                    string destPath = Path.Combine(txtRightDir.Text, srcFile.Name);

                    // 3단계에서 만든 '똑똑한 복사 함수'를 실행합니다.
                    CopyFileWithConfirmation(srcFile, destPath);
                }
            }

            // 4. 복사가 다 끝났으면 화면을 새로고침해서 색깔을 맞춥니다.
            PopulateListView(lvwLeftDir, txtLeftDir.Text);
            PopulateListView(lvwrightDir, txtRightDir.Text);
            CompareListViews();
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            // 1. 오른쪽 리스트에서 선택된 항목들을 가져옵니다.
            var selectedItems = lvwrightDir.SelectedItems;

            // 2. 선택된 게 없으면 아무것도 안 하고 끝냅니다.
            if (selectedItems.Count == 0) return;

            // 3. 하나씩 꺼내서 복사 작업을 진행합니다.
            foreach (ListViewItem item in selectedItems)
            {
                string fileName = item.Text;

                // 오른쪽 주머니(rightFiles) 에서 원본 파일 정보를 찾습니다.
                if (rightFiles.TryGetValue(fileName, out var srcFile))
                {
                    // 왼쪽 폴더 경로(txtLeftDir) 와 파일 이름을 합쳐서 목적지 주소를 만듭니다.
                    string destPath = Path.Combine(txtLeftDir.Text, srcFile.Name);

                    // 날짜 비교 확인 창이 포함된 똑똑한 복사 함수 실행
                    CopyFileWithConfirmation(srcFile, destPath);
                }
            }

            // 4. 복사가 다 끝났으면 양쪽 리스트를 새로고침해서 색깔을 다시 맞춥니다.
            PopulateListView(lvwLeftDir, txtLeftDir.Text);
            PopulateListView(lvwrightDir, txtRightDir.Text);
            CompareListViews();
        }
    }

}
