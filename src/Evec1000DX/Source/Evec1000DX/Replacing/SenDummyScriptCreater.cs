using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evec1000DX.Replacing
{
    /// <summary>
    /// 千階堂に読み込ませるダミースクリプトを生成するクラス
    /// </summary>
    public class SenDummyScriptCreater
    {
        #region 定数
        /// <summary>
        /// 千階堂コマンド前のテキスト
        /// </summary>
        private static readonly string _PreText =
@"太閤立志伝５イベントソース
チャプター: {
イベント:千階堂用ダミー {
属性:一度だけ
発生契機:ゲームスタート時
発生条件: {
}
スクリプト: {
";

        /// <summary>
        /// 千階堂コマンド後のテキスト
        /// </summary>
        private static readonly string _PostText =
@"}
}
}
";

        /// <summary>
        /// 千階堂コマンド前のバイナリ
        /// </summary>
        private static readonly byte[] _PreBinary = new byte[] { 0x54, 0x35, 0x45, 0x4D, 0x09, 0x00, 0x13, 0x00, 0x01, 0x00, 0x00, 0x00, 0x20, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x17, 0x08, 0x38, 0x00, 0x00, 0x00, 0x12, 0x34, 0x56, 0x78, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x20, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 千階堂コマンド後のバイナリ
        /// </summary>
        private static readonly byte[] _PostBinary = new byte[] { 0x00, 0x07, 0x00, 0x00 };

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ダミーソースを生成するクラスのコンストラクタ
        /// </summary>
        public SenDummyScriptCreater()
        {
        }

        #endregion

        #region メソッド
        /// <summary>
        /// ダミーテキストの生成
        /// </summary>
        /// <param name="command">千階堂コマンド</param>
        /// <returns>ダミーテキスト</returns>
        public string CreateDummyText(string command)
        {
            string result = _PreText;
            result += @"文字列設定:(Ｅｍｐｔｙ)[[";
            result += command;
            result += "]]\r\n";
            result += _PostText;
            return result;
        }

        /// <summary>
        /// ダミーバイナリの生成
        /// </summary>
        /// <param name="command">千階堂コマンド</param>
        /// <returns>ダミーバイナリ</returns>
        public byte[] CreateDummyBinary(string command)
        {
            List<byte> result = new List<byte>();
            result.AddRange(_PreBinary);
            result.Add(0x1E);
            result.Add(0x00);
            result.Add(0x00);
            result.Add(0x00);
            // コマンドをShiftJisのバイト列に変換して突っ込む
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            byte[] data = sjisEnc.GetBytes(command);
            result.AddRange(data);
            // 文字列の半端なところを0で埋める
            int byteSize = data.Length / 4 * 4 + 4;
            for (int i = 0, n = byteSize - data.Length; i < n; ++i)
            {
                result.Add(0);
            }
            result.AddRange(_PostBinary);
            return result.ToArray();
        }

        #endregion

    }
}
