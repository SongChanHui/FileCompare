namespace FileCompare
{
    public partial class Form1 : Form
    {
      
        Dictionary<string, FileSystemInfo> leftFiles = new Dictionary<string, FileSystemInfo>();
        Dictionary<string, FileSystemInfo> rightFiles = new Dictionary<string, FileSystemInfo>();

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
            lv.BeginUpdate();
            lv.Items.Clear();

            // 함수 시작하자마자 주머니부터 깨끗하게 비웁니다.
            if (lv == lvwLeftDir) leftFiles.Clear();
            else rightFiles.Clear();

            try
            {
                // [1] 하위 폴더 목록 가져오기 및 추가
                var dirs = Directory.EnumerateDirectories(folderPath)
                            .Select(p => new DirectoryInfo(p))
                            .OrderBy(d => d.Name);

                foreach (var d in dirs)
                {
                    var item = new ListViewItem(d.Name);

                    // 다시 <DIR> 로 변경 (용량 계산 안 함)
                    item.SubItems.Add("<DIR>");

                    item.SubItems.Add(d.LastWriteTime.ToString("g"));
                    lv.Items.Add(item);

                    if (lv == lvwLeftDir) leftFiles[d.Name] = d;
                    else rightFiles[d.Name] = d;
                }

                // [2] 파일 목록 가져오기 및 추가 부분 수정
                var files = Directory.EnumerateFiles(folderPath)
                            .Select(p => new FileInfo(p))
                            .OrderBy(f => f.Name);

                

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
            foreach (ListViewItem leftItem in lvwLeftDir.Items)
            {
                ListViewItem rightItem = null;
                foreach (ListViewItem item in lvwrightDir.Items)
                {
                    if (item.Text == leftItem.Text) { rightItem = item; break; }
                }

                if (rightItem != null)
                {
                    DateTime leftDate = DateTime.Parse(leftItem.SubItems[2].Text);
                    DateTime rightDate = DateTime.Parse(rightItem.SubItems[2].Text);
                    bool isFolder = leftItem.SubItems[1].Text == "<DIR>";

                    if (isFolder) // [폴더인 경우] 날짜만 같으면 검은색
                    {
                        if (leftDate == rightDate)
                        {
                            leftItem.ForeColor = Color.Black;
                            rightItem.ForeColor = Color.Black;
                        }
                        else
                        {
                            // 날짜 다르면 New/Old 판별 (빨간색/회색)
                            if (leftDate > rightDate) { leftItem.ForeColor = Color.Red; rightItem.ForeColor = Color.Gray; }
                            else { leftItem.ForeColor = Color.Gray; rightItem.ForeColor = Color.Red; }
                        }
                    }
                    else // [파일인 경우] 크기와 날짜 모두 같아야 검은색
                    {
                        string leftSize = leftItem.SubItems[1].Text;
                        string rightSize = rightItem.SubItems[1].Text;

                        if (leftSize == rightSize && leftDate == rightDate)
                        {
                            leftItem.ForeColor = Color.Black;
                            rightItem.ForeColor = Color.Black;
                        }
                        else
                        {
                            if (leftDate > rightDate) { leftItem.ForeColor = Color.Red; rightItem.ForeColor = Color.Gray; }
                            else if (leftDate < rightDate) { leftItem.ForeColor = Color.Gray; rightItem.ForeColor = Color.Red; }
                            else { leftItem.ForeColor = Color.Red; rightItem.ForeColor = Color.Red; }
                        }
                    }
                }
                else { leftItem.ForeColor = Color.Purple; }
            }
            // (오른쪽 단독 파일 보라색 로직 생략 - 기존과 동일)
        }
        private void CopyFileWithConfirmation(FileSystemInfo src, string destPath)
        {
            // [1] 대상이 폴더(DirectoryInfo) 인 경우
            if (src is DirectoryInfo)
            {
                if (Directory.Exists(destPath))
                {
                    DirectoryInfo dest = new DirectoryInfo(destPath);

                    // 원본이 대상보다 오래된(옛날) 폴더일 때만 경고 창 표시
                    if (src.LastWriteTime < dest.LastWriteTime)
                    {
                        string message = "대상에 동일한 이름의 폴더가 이미 있습니다.\n" +
                                         "대상 폴더가 더 신규 폴더입니다. 덮어쓰시겠습니까?\n\n" +
                                         "원본: " + src.FullName + " (" + src.LastWriteTime + ")\n" +
                                         "대상: " + dest.FullName + " (" + dest.LastWriteTime + ")";

                        var result = MessageBox.Show(message, "덮어쓰기 확인",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No) return;
                    }
                }
                // 폴더 복사 실행 (재귀 함수 호출)
                // 폴더 동기화 실행
                CopyDirectory(src.FullName, destPath);

                // 최종 시간 동기화
                Directory.SetLastWriteTime(destPath, src.LastWriteTime);
            }
            // [2] 대상이 파일(FileInfo) 인 경우
            else if (src is FileInfo)
            {
                if (File.Exists(destPath))
                {
                    FileInfo dest = new FileInfo(destPath);

                    // 원본이 대상보다 오래된(옛날) 파일일 때만 경고 창 표시
                    if (src.LastWriteTime < dest.LastWriteTime)
                    {
                        string message = "대상에 동일한 이름의 파일이 이미 있습니다.\n" +
                                         "대상 파일이 더 신규 파일입니다. 덮어쓰시겠습니까?\n\n" +
                                         "원본: " + src.FullName + " (" + src.LastWriteTime + ")\n" +
                                         "대상: " + dest.FullName + " (" + dest.LastWriteTime + ")";

                        var result = MessageBox.Show(message, "덮어쓰기 확인",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No) return;
                    }
                }
                // 파일 복사 실행 (덮어쓰기 허용)
                File.Copy(src.FullName, destPath, true);
            }
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
        private long GetDirectorySize(DirectoryInfo d)
        {
            try
            {
                // 하위 모든 파일의 합계를 구함
                return d.EnumerateFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
            }
            catch
            {
                return 0; // 접근 권한 없는 폴더 등 예외 처리
            }
        }
        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string dest = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, dest, true);
            }
            foreach (string folder in Directory.GetDirectories(sourceDir))
            {
                string dest = Path.Combine(destDir, Path.GetFileName(folder));
                CopyDirectory(folder, dest);
            }
            // 복사 후 폴더 날짜 강제 일치 (검은색 뜨게 하는 핵심)
            Directory.SetLastWriteTime(destDir, Directory.GetLastWriteTime(sourceDir));
        }

    }

}
