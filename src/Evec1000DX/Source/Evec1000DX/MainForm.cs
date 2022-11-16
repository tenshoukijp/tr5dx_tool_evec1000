using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Evec1000DX.Replacing;
using Evec1000DX.Replacing.Config;

namespace Evec1000DX
{
    /// <summary>
    /// メインフォーム
    /// </summary>
    public partial class MainForm : Form
    {
        #region 定数
        /// <summary>
        /// 一時ファイルの置き場
        /// </summary>
        private static readonly string _TempDirectory = @"TMP\";

        /// <summary>
        /// オリジナル千階堂
        /// </summary>
        private static readonly string _Evec1000Exe = @"Evec1000\Evec1000.exe";

        #endregion

        #region コンストラクタ
        /// <summary>
        /// メインフォームのコンストラクタ
        /// </summary>
        public MainForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
        }

        #endregion

        #region イベントハンドラ
        /// <summary>
        /// フォーム読み込み時のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント情報</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 前回のウィンドウ位置、サイズを復元
            WindowState = Properties.Settings.Default.FormState;
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            Location = Properties.Settings.Default.FormLocation;
            Size = Properties.Settings.Default.FormSize;
            // ソースディレクトリ
            _SourceDirectoryTextBox.Text = Properties.Settings.Default.SourceDirectory;

            // 一時ファイルを全消去
            if (Directory.Exists(_TempDirectory))
            {
                var paths = Directory.GetFiles(_TempDirectory);
                foreach (string path in paths)
                {
                    File.Delete(path);
                }
            }
            else
            {
                Directory.CreateDirectory(_TempDirectory);
            }

            // オリジナル千階堂の実行
            Process[] processes = Process.GetProcessesByName("Evec1000");
            if (processes.Length == 0)
            {
                Process process = new Process();
                process.StartInfo.FileName = _Evec1000Exe;
                process.Start();
                _LogTextBox.Text += "オリジナル千階堂を起動します。\r\n";
            }
            else
            {
                _LogTextBox.Text += "オリジナル千階堂は起動済みです。\r\n";
            }
            _LogTextBox.Text += "オリジナル千階堂の起動後に千階堂ソースコードの変換を行ってください。\r\n";
        }

        /// <summary>
        /// フォームが閉じられる直前のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント情報</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ソースディレクトリ
            Properties.Settings.Default.SourceDirectory = _SourceDirectoryTextBox.Text;
            // ウィンドウ位置、サイズの保存
            Properties.Settings.Default.FormState = WindowState;
            // ウインドウステートがNormalな場合には位置（location）とサイズ（size）を記憶する
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.FormLocation = Location;
                Properties.Settings.Default.FormSize = Size;
            }
            // 最小化（minimized）や最大化（maximized）の場合には、RestoreBoundsを記憶する
            else
            {
                Properties.Settings.Default.FormLocation = RestoreBounds.Location;
                Properties.Settings.Default.FormSize = RestoreBounds.Size;
            }

            // 設定を保存する
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// ソースフォルダ指定のテキストボックスのテキストが変化した際のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント情報</param>
        private void _SourceDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            // まずはクリア
            _SenScriptsCheckedListBox.Items.Clear();
            // ディレクトリが無ければ抜ける
            string directoryPath = _SourceDirectoryTextBox.Text;
            if (!Directory.Exists(directoryPath))
            {
                return;
            }
            // 拡張子が一致するファイルを一覧に表示
            var paths = Directory.GetFiles(directoryPath, "*.ev1000");
            var files = from path in paths
                        orderby path ascending
                        select Path.GetFileName(path);
            _SenScriptsCheckedListBox.Items.AddRange(files.ToArray());
            // 全てチェックを入れておく
            for (int i = 0; i < _SenScriptsCheckedListBox.Items.Count; ++i)
            {
                _SenScriptsCheckedListBox.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// フォルダ選択ボタンが押された際のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント情報</param>
        private void _DirectorySelectButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = @"ソースフォルダを指定してください。";
            dialog.SelectedPath = _SourceDirectoryTextBox.Text;
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _SourceDirectoryTextBox.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// フォルダ選択ボタンが押された際のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント情報</param>
        private void _LogTextBox_TextChanged(object sender, EventArgs e)
        {
            //カレット位置を末尾に移動
            _LogTextBox.SelectionStart = _LogTextBox.Text.Length;
            //テキストボックスにフォーカスを移動
            _LogTextBox.Focus();
            //カレット位置までスクロール
            _LogTextBox.ScrollToCaret();
        }

        /// <summary>
        /// 千階堂ソースコードの変換ボタンが押された際のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント情報</param>
        private void _ReplaceSenScriptButton_Click(object sender, EventArgs e)
        {
            try
            {
                // オリジナル千階堂が立ち上がってなければ、立ち上げる
                Process[] processes = Process.GetProcessesByName("Evec1000");
                if (processes.Length == 0)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = _Evec1000Exe;
                    process.Start();
                    _LogTextBox.Text += "千階堂ソースコードの変換にはオリジナル千階堂が必要です。\r\n";
                    _LogTextBox.Text += "オリジナル千階堂を起動後、再度ボタンを押してください。\r\n";
                    return;
                }

                // ソースコードのディレクトリが存在するか確認する
                string directoryPath = _SourceDirectoryTextBox.Text;
                if (!Directory.Exists(directoryPath))
                {
                    _LogTextBox.Text += "フォルダ「" + directoryPath + "」は存在しません。\r\n";
                    return;
                }
                directoryPath = directoryPath[directoryPath.Length - 1] == '\\' ? directoryPath : directoryPath + "\\";

                // ダミー設定の読み込み
                string jsonText = "";
                using (StreamReader streamReader = new StreamReader(@"Config\DummyConfig.json", Encoding.UTF8))
                {
                    jsonText = streamReader.ReadToEnd();
                }
                var dummyConfig = JsonSerializer.Deserialize<List<DummyConfig>>(jsonText);
                // NOPコマンドの設定の読み込み
                using (StreamReader streamReader = new StreamReader(@"Config\NOPConfig.json", Encoding.UTF8))
                {
                    jsonText = streamReader.ReadToEnd();
                }
                var nopConfig = JsonSerializer.Deserialize<NOPConfig>(jsonText);

                // 変換開始
                _LogTextBox.Text += "千階堂ソースコードの変換を開始します。\r\n";
                var replacer = new SenScriptReplacer(dummyConfig, nopConfig);
                for (int i = 0, n = _SenScriptsCheckedListBox.Items.Count; i < n; ++i)
                {
                    if (_SenScriptsCheckedListBox.GetItemChecked(i))
                    {
                        Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                        string fileName = _SenScriptsCheckedListBox.Items[i].ToString();
                        _LogTextBox.Text += "「" + fileName + "」の変換。\r\n";
                        // ファイル読み込み
                        string text = "";
                        using (StreamReader streamReader = new StreamReader(directoryPath + fileName, sjisEnc))
                        {
                            text = streamReader.ReadToEnd();
                        }
                        // 変換
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                        string newText = replacer.Replace(fileNameWithoutExtension, text);
                        // ファイル書き込み
                        using (StreamWriter fileStream = new StreamWriter(directoryPath + fileNameWithoutExtension + ".txt", false, sjisEnc))
                        {
                            fileStream.Write(newText);
                        }
                    }
                }
                // 置き換え情報の出力
                string jsonTextOutput = replacer.GetReplacingInfoText();
                using (StreamWriter fileStream = new StreamWriter(_TempDirectory + "ReplacingInfo.json", false, Encoding.UTF8))
                {
                    fileStream.Write(jsonTextOutput);
                }
                // 終了メッセージ
                _LogTextBox.Text += "千階堂ソースコードの変換が終了しました。\r\n";
                _LogTextBox.Text += "出力されたテキストを公式のイベコンでコンバートした後、evm変換ボタンを押してください。\r\n";
            }
            catch (Exception ex)
            {
                _LogTextBox.Text += "千階堂ソースコードの変換に失敗しました。\r\n";
                _LogTextBox.Text += ex.Message + "\r\n";
                if (ex.InnerException != null)
                {
                    _LogTextBox.Text += ex.InnerException.Message + "\r\n";
                    _LogTextBox.Text += ex.InnerException.StackTrace + "\r\n";
                }
                else
                {
                    _LogTextBox.Text += ex.StackTrace + "\r\n";
                }
            }
        }

        /// <summary>
        /// evm変換ボタンが押された際のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント情報</param>
        private void _ReplaceEvmButton_Click(object sender, EventArgs e)
        {
            try
            {
                // ソースコードのディレクトリが存在するか確認する
                string directoryPath = _SourceDirectoryTextBox.Text;
                if (!Directory.Exists(directoryPath))
                {
                    _LogTextBox.Text += "フォルダ「" + directoryPath + "」は存在しません。\r\n";
                    return;
                }
                directoryPath = directoryPath[directoryPath.Length - 1] == '\\' ? directoryPath : directoryPath + "\\";

                // 置き換え情報の読み込み
                string infoFilePath = _TempDirectory + "ReplacingInfo.json";
                if (!File.Exists(infoFilePath))
                {
                    _LogTextBox.Text += "千階堂ソースコードの変換が未実行です。先に千階堂ソースコードの変換を実行してください。\r\n";
                    return;
                }
                string jsonText = "";
                using (StreamReader streamReader = new StreamReader(infoFilePath, Encoding.UTF8))
                {
                    jsonText = streamReader.ReadToEnd();
                }
                var replacingInfo = JsonSerializer.Deserialize<List<ReplacingInfo>>(jsonText);

                // NOPコマンドの設定の読み込み
                using (StreamReader streamReader = new StreamReader(@"Config\NOPConfig.json", Encoding.UTF8))
                {
                    jsonText = streamReader.ReadToEnd();
                }
                var nopConfig = JsonSerializer.Deserialize<NOPConfig>(jsonText);
                // ID設定の読み込み
                using (StreamReader streamReader = new StreamReader(@"Config\IDConfig.json", Encoding.UTF8))
                {
                    jsonText = streamReader.ReadToEnd();
                }
                var idConfig = JsonSerializer.Deserialize<IDConfig>(jsonText);

                // 変換開始
                _LogTextBox.Text += "evmファイルの変換を開始します。\r\n";
                var replacer = new EvmReplacer(nopConfig, idConfig);
                for (int i = 0, n = _SenScriptsCheckedListBox.Items.Count; i < n; ++i)
                {
                    if (_SenScriptsCheckedListBox.GetItemChecked(i))
                    {
                        string fileName = _SenScriptsCheckedListBox.Items[i].ToString();
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                        _LogTextBox.Text += "「" + fileNameWithoutExtension + ".evm」の変換。\r\n";
                        // 変換対象のファイルに関する置き換え情報を取得
                        var replacingInfoForEvmFile = from info in replacingInfo
                                                      where info.FileName == fileNameWithoutExtension
                                                      orderby info.Index ascending
                                                      select info;
                        // ファイルの読み込み
                        byte[] bytes = null;
                        string filePath = directoryPath + fileNameWithoutExtension + ".evm";
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            bytes = new byte[fileStream.Length];
                            fileStream.Read(bytes, 0, bytes.Length);
                        }
                        // 変換処理
                        byte[] result = replacer.Replace(bytes, replacingInfoForEvmFile);
                        // ファイルの書き込み
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            fileStream.Write(result, 0, result.Length);
                        }
                    }
                }
                // 終了メッセージ
                _LogTextBox.Text += "evmファイルの変換が終了しました。\r\n";
                _LogTextBox.Text += "最後に公式イベコンで「__千階堂後にこれだけをコンバート.txt」だけをコンバートしてください。\r\n";
            }
            catch (Exception ex)
            {
                _LogTextBox.Text += "evmの変換に失敗しました。\r\n";
                _LogTextBox.Text += ex.Message + "\r\n";
                if (ex.InnerException != null)
                {
                    _LogTextBox.Text += ex.InnerException.Message + "\r\n";
                    _LogTextBox.Text += ex.InnerException.StackTrace + "\r\n";
                }
                else
                {
                    _LogTextBox.Text += ex.StackTrace + "\r\n";
                }
            }
        }

        #endregion

    }
}
