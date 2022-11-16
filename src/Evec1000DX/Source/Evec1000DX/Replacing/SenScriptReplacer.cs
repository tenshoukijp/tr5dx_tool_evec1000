using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Evec1000DX.Replacing.Config;

namespace Evec1000DX.Replacing
{
    /// <summary>
    /// 千階堂スクリプトのリプレイスクラス
    /// </summary>
    public class SenScriptReplacer
    {
        #region 定数
        /// <summary>
        /// 一時ファイルの置き場
        /// </summary>
        private static readonly string _TempDirectory = @"TMP\";

        /// <summary>
        /// ダミースクリプトファイル
        /// </summary>
        private static readonly string _DummyFileName = _TempDirectory + @"DummyEvec1000Script";

        /// <summary>
        /// オリジナル千階堂
        /// </summary>
        private static readonly string _Evec1000Exe = @"Evec1000\Evec1000.exe";

        /// <summary>
        /// 千階堂記述のパターン
        /// </summary>
        private static readonly string _SenCommandPattern = @"(文字列設定:\(Ｅｍｐｔｙ\)|ひとりごと:)\[\[(?<cmd>＠(千階堂|千|万階堂|万)＠[^\[\]\r\n]*)\]\]";

        /// <summary>
        /// 千階堂コマンドの開始アドレス
        /// </summary>
        private static readonly int _SenCommandAddress = 0x44;

        /// <summary>
        /// コマンド終了後のバイト列
        /// </summary>
        private static readonly byte[] _CommandEndBytes = new byte[] { 0xF0, 0x00, 0x00, 0x00, 0xF0, 0x00, 0x00, 0x00 };

        /// <summary>
        /// コマンド終了後のバイト列2
        /// </summary>
        private static readonly byte[] _CommandEndBytes2 = new byte[] { 0xF0, 0x00, 0x00, 0x00 };

        #endregion

        #region フィールド
        /// <summary>
        /// 置き換え情報
        /// </summary>
        private List<ReplacingInfo> _ReplacingInfo = new List<ReplacingInfo>();

        /// <summary>
        /// ダミーコンフィグ
        /// </summary>
        private List<DummyConfig> _DummyConfig;

        /// <summary>
        /// NOPコマンドのコンフィグ
        /// </summary>
        private NOPConfig _NOPConfig;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// 千階堂スクリプトのリプレイスクラスのコンストラクタ
        /// </summary>
        /// <param name="dummyConfig">ダミーコンフィグ</param>
        /// <param name="nopConfig">NOPコマンドのコンフィグ</param>
        public SenScriptReplacer(List<DummyConfig> dummyConfig, NOPConfig nopConfig)
        {
            _DummyConfig = dummyConfig;
            _NOPConfig = nopConfig;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// 置き換え
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="script">置き換え前のスクリプト</param>
        /// <returns>置き換え後のスクリプト</returns>
        public string Replace(string fileName, string script)
        {
            // コメント除去
            string scriptWithoutComment = Regex.Replace(script, @"//[^\r\n]*", "");
            // アナライズ
            AnalyzeSenScript(fileName, scriptWithoutComment);
            // 千階堂記述の置き換えと置き換え情報の更新
            string replacedText = ReplaceScriptAndUpdateInfo(fileName, scriptWithoutComment);
            // 置き換え結果を返す
            return replacedText;
        }

        /// <summary>
        /// JSon形式の置き換え情報を取得
        /// </summary>
        /// <returns>置き換え情報</returns>
        public string GetReplacingInfoText()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(_ReplacingInfo, options);
            return json;
        }

        /// <summary>
        /// オリジナル千階堂のアナライズ操作を行う
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="script">スクリプト</param>
        /// <exception cref="Exception">千階堂記述エラー</exception>
        private void AnalyzeSenScript(string fileName, string script)
        {
            string replacedText = script;
            var match = Regex.Match(replacedText, _SenCommandPattern);
            while (match.Success)
            {
                try
                {
                    // 千階堂記述からコマンドの内容が書かれたテキストを取得
                    string cmdText = match.Groups["cmd"].Value;
                    // 千階堂コマンドからダミーファイルを生成
                    CreateDummyFiles(cmdText);
                    // ダミーファイルに対してアナライズを実行する
                    RunEvec1000("/a " + _DummyFileName + ".txt");
                    // 次の千階堂記述を探す
                    replacedText = replacedText.Substring(0, match.Index) + replacedText.Substring(match.Index + match.Length);
                    match = Regex.Match(replacedText, _SenCommandPattern);
                }
                catch (Exception ex)
                {
                    int line = replacedText.Substring(0, match.Index).Count((ch) => ch == '\n') + 1;
                    throw new Exception(@"千階堂記述エラー：「" + fileName + @".ev1000」の" + line + @"行目", ex);
                }
            }
        }

        /// <summary>
        /// 千階堂記述の置き換えと置き換え情報の更新
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="script">置き換え前のスクリプト</param>
        /// <returns>置き換え後のスクリプト</returns>
        /// <exception cref="Exception">千階堂記述エラー</exception>
        private string ReplaceScriptAndUpdateInfo(string fileName, string script)
        {
            string replacedText = script;
            int index = 0;
            var match = Regex.Match(replacedText, _SenCommandPattern);
            while (match.Success)
            {
                try
                {
                    // 千階堂記述からコマンドの内容が書かれたテキストを取得
                    string cmdText = match.Groups["cmd"].Value;
                    // 千階堂コマンドからダミーファイルを生成
                    CreateDummyFiles(cmdText);
                    // ダミーファイルに対して書き換えを実行する
                    RunEvec1000("/u " + _DummyFileName + ".txt");
                    // オリジナル千階堂で書き換えたファイルからリプレイス用のコマンドを取得
                    byte[] bytes = null;
                    using (FileStream fileStream = new FileStream(_DummyFileName + ".evm", FileMode.Open, FileAccess.Read))
                    {
                        // ファイルの読み込み
                        bytes = new byte[fileStream.Length];
                        fileStream.Read(bytes, 0, (int)fileStream.Length);
                    }
                    // リプレイス用のコマンドのサイズを取得
                    int byteSize = CalcSenCommandByteSize(bytes);
                    // セーブナンバー使用の確認
                    string[] cmdArgs = cmdText.Split(new char[] { '＠' }, StringSplitOptions.RemoveEmptyEntries);
                    bool usedSaveNumFlag = false;
                    if ((cmdArgs[1] == @"更新") && (cmdArgs[2].StartsWith(@"セ：：") || cmdArgs[2].StartsWith(@"セーブナンバー：：")))
                        usedSaveNumFlag = true;
                    else if ((cmdArgs[1] == @"代入") && (cmdArgs[3].StartsWith(@"セ：：") || cmdArgs[3].StartsWith(@"セーブナンバー：：")))
                        usedSaveNumFlag = true;
                    // 置き換え情報の更新
                    ReplacingInfo info = UpdateReplacingInfo(fileName, index, bytes, byteSize, usedSaveNumFlag);
                    // ダミーコマンドを置き換え
                    string insertText = info.DummyText;
                    for (int i = 0; i < _NOPConfig.NumOfNOPCommands; ++i)
                        insertText += "\r\n" + _NOPConfig.DummyText;
                    replacedText = replacedText.Substring(0, match.Index) + insertText + replacedText.Substring(match.Index + match.Length);
                    // 次の千階堂記述を探す
                    ++index;
                    match = Regex.Match(replacedText, _SenCommandPattern);
                }
                catch (Exception ex)
                {
                    int line = replacedText.Substring(0, match.Index).Count((ch) => ch == '\n') + 1;
                    line -= index * _NOPConfig.NumOfNOPCommands;
                    throw new Exception(@"千階堂記述エラー：「" + fileName + @".ev1000」の" + line + @"行目", ex);
                }
            }
            // 置き換え結果を返す
            return replacedText;
        }

        /// <summary>
        /// 千階堂コマンドからダミーファイルを生成
        /// </summary>
        /// <param name="cmdText">千階堂コマンド</param>
        private void CreateDummyFiles(string cmdText)
        {
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            var dummyCreater = new SenDummyScriptCreater();
            if (!Directory.Exists(_TempDirectory))
            {
                Directory.CreateDirectory(_TempDirectory);
            }
            string dummyText = dummyCreater.CreateDummyText(cmdText);
            using (StreamWriter fileStream = new StreamWriter(_DummyFileName + ".txt", false, sjisEnc))
            {
                fileStream.Write(dummyText);
            }
            byte[] dummyBinary = dummyCreater.CreateDummyBinary(cmdText);
            using (FileStream fileStream = new FileStream(_DummyFileName + ".evm", FileMode.Create, FileAccess.ReadWrite))
            {
                fileStream.Write(dummyBinary, 0, dummyBinary.Length);
            }
        }

        /// <summary>
        /// オリジナル千階堂ツールの実行
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        private void RunEvec1000(string args)
        {
            Process process = new Process();
            process.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.CreateNoWindow = true;
            string exePath = Path.GetFullPath(_Evec1000Exe);
            process.StartInfo.Arguments = "/c \"" + exePath + "\" " + args;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// リプレイス用のコマンドのサイズを取得
        /// </summary>
        /// <param name="bytes">オリジナル千階堂で置き換えたダミーevmの中身</param>
        /// <returns>コマンドのサイズ</returns>
        private int CalcSenCommandByteSize(byte[] bytes)
        {
            int indexOfBytes = -1;
            for (int i = _SenCommandAddress; i < (bytes.Length - 4); ++i)
            {
                int checkSuccessCounter = 0;
                for (int j = 0; j < _CommandEndBytes.Length; ++j)
                {
                    if (bytes[i + j] != _CommandEndBytes[j])
                        break;
                    ++checkSuccessCounter;
                }
                if (checkSuccessCounter == _CommandEndBytes.Length)
                {
                    indexOfBytes = i;
                    break;
                }
            }
            // 失敗したら小さいほうで試してみる
            if (indexOfBytes == -1)
            {
                for (int i = _SenCommandAddress; i < (bytes.Length - 4); ++i)
                {
                    int checkSuccessCounter = 0;
                    for (int j = 0; j < _CommandEndBytes2.Length; ++j)
                    {
                        if (bytes[i + j] != _CommandEndBytes2[j])
                            break;
                        ++checkSuccessCounter;
                    }
                    if (checkSuccessCounter == _CommandEndBytes2.Length)
                    {
                        indexOfBytes = i;
                        break;
                    }
                }
            }
            if (indexOfBytes == -1)
                throw new Exception("終端サーチエラー");

            // 結果を返す
            int byteSize = indexOfBytes - _SenCommandAddress;
            return byteSize;
        }

        /// <summary>
        /// 置き換え情報の更新
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="index">置き換え情報インデックス</param>
        /// <param name="bytes">オリジナル千階堂で置き換えたダミーevmの中身</param>
        /// <param name="byteSize">コマンドのサイズ</param>
        /// <param name="usedSaveNumFlag">セーブナンバー使用フラグ</param>
        /// <returns>追加された置き換え情報</returns>
        private ReplacingInfo UpdateReplacingInfo(string fileName, int index, byte[] bytes, int byteSize, bool usedSaveNumFlag)
        {
            // 置き換え情報の更新
            ReplacingInfo info = new ReplacingInfo();
            info.FileName = fileName;
            info.Index = index;
            info.ByteSize = byteSize;
            var dummyConfig = _DummyConfig.Find((config) => config.ByteSize == byteSize);
            info.DummyText = dummyConfig.DummyText;
            info.DummyBinary = dummyConfig.DummyBinary;
            info.BitMask = dummyConfig.BitMask;
            byte[] data = new byte[byteSize];
            Array.Copy(bytes, _SenCommandAddress, data, 0, byteSize);
            info.ReplacingBinary = data;
            info.UsedSaveNumFlag = usedSaveNumFlag;
            _ReplacingInfo.Add(info);

            // 結果を返す
            return info;
        }

        #endregion
    }
}
