using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evec1000DX.Replacing.Config
{
    /// <summary>
    /// 千階堂のID関連の設定
    /// </summary>
    public class IDConfig
    {
        #region プロパティ
        /// <summary>
        /// ユーザのセーブナンバーの最大数
        /// </summary>
        public int MaxNumOfSaveNumIDs { get; set; }

        /// <summary>
        /// ユーザの最初のセーブナンバーのID
        /// </summary>
        public int FirstSaveNumID { get; set; }

        /// <summary>
        /// ユーザの最後のセーブナンバーのID
        /// </summary>
        public int LastSaveNumID { get; set; }

        /// <summary>
        /// オリジナル千階堂のセーブナンバーIDとのオフセット
        /// </summary>
        public int OffsetOfEvc1000SaveNumID { get; set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// 千階堂のID関連の設定のコンストラクタ
        /// </summary>
        public IDConfig()
        {
        }

        #endregion
    }
}
