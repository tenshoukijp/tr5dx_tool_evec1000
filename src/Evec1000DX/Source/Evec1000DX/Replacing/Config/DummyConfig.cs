using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evec1000DX.Replacing.Config
{
    /// <summary>
    /// ダミー設定
    /// </summary>
    [Serializable]
    public class DummyConfig
    {
        #region プロパティ
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

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ダミー設定のコンストラクタ
        /// </summary>
        public DummyConfig()
        {
        }

        #endregion

    }
}
