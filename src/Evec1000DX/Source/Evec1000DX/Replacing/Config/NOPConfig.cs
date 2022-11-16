using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evec1000DX.Replacing.Config
{
    /// <summary>
    /// NOPコマンドについての設定
    /// </summary>
    public class NOPConfig
    {
        #region プロパティ
        /// <summary>
        /// 千階堂コマンドの後のNOPコマンドの数
        /// </summary>
        public int NumOfNOPCommands { get; set; }

        /// <summary>
        /// ダミーテキスト
        /// </summary>
        public string DummyText { get; set; }

        /// <summary>
        /// バイトサイズ
        /// </summary>
        public int ByteSize { get; set; } = 0;

        /// <summary>
        /// NOPコマンドのバイナリ
        /// </summary>
        public byte[] NOPCommandBinary { get; set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// NOPコマンドについての設定のコンストラクタ
        /// </summary>
        public NOPConfig()
        {
        }

        #endregion

    }
}
