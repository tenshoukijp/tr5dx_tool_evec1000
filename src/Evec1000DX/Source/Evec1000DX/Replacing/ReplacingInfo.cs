using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evec1000DX.Replacing
{
    /// <summary>
    /// リプレイス情報
    /// </summary>
    [Serializable]
    public class ReplacingInfo
    {
        #region プロパティ
        /// <summary>
        /// 対象のファイル名
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// インデックス (同一ファイル内での登場の順番)
        /// </summary>
        public int Index { get; set; } = 0;

        /// <summary>
        /// バイトサイズ
        /// </summary>
        public int ByteSize { get; set; } = 0;

        /// <summary>
        /// ダミーテキスト
        /// </summary>
        public string DummyText { get; set; } = "";

        /// <summary>
        /// ダミーバイナリ
        /// </summary>
        public byte[] DummyBinary { get; set; } = null;

        /// <summary>
        /// ビットマスク
        /// </summary>
        public byte[] BitMask { get; set; } = null;

        /// <summary>
        /// 置き換えバイナリ
        /// </summary>
        public byte[] ReplacingBinary { get; set; } = null;

        /// <summary>
        /// セーブナンバー使用フラグ
        /// </summary>
        public bool UsedSaveNumFlag { get; set; } = false;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// リプレイス情報のコンストラクタ
        /// </summary>
        public ReplacingInfo()
        {
        }

        #endregion

    }
}
