using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evec1000DX.Replacing.Config;

namespace Evec1000DX.Replacing
{
    /// <summary>
    /// evmファイル置き換えクラス
    /// </summary>
    public class EvmReplacer
    {
        #region フィールド
        /// <summary>
        /// NOPコマンドのコンフィグ
        /// </summary>
        private NOPConfig _NOPConfig;

        /// <summary>
        /// IDコンフィグ
        /// </summary>
        private IDConfig _IDConfig;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// evmファイル置き換えクラスのコンストラクタ
        /// </summary>
        /// <param name="nopConfig">NOPコマンドのコンフィグ</param>
        /// <param name="idConfig">IDコンフィグ</param>
        public EvmReplacer(NOPConfig nopConfig, IDConfig idConfig)
        {
            _NOPConfig = nopConfig;
            _IDConfig = idConfig;
        }

        #endregion

        #region メソッド
        /// <summary>
        /// Evmファイルの置き換え
        /// </summary>
        /// <param name="bytes">バイト列</param>
        /// <param name="replacingInfo">置き換え情報</param>
        /// <returns>置き換え後のバイト列</returns>
        public byte[] Replace(byte[] bytes, IEnumerable<ReplacingInfo> replacingInfo)
        {
            // コピー作成
            byte[] result = new byte[bytes.Length];
            bytes.CopyTo(result, 0);

            // セーブナンバーを使えるようにする
            result[0x1C] = (byte)_IDConfig.LastSaveNumID;

            // 置き換え情報を基に置き換えを行う
            int currentIndex = 0;
            foreach (var info in replacingInfo)
            {
                // セーブナンバーを使っていれば、DX対応のためにID部分を書き換える
                if (info.UsedSaveNumFlag)
                {
                    int originalID = ((((info.ReplacingBinary[6] & 0x3F) << 1) | ((info.ReplacingBinary[5] & 0x80) >> 7)) << 8) | (((info.ReplacingBinary[5] & 0x7F) << 1) | ((info.ReplacingBinary[4] & 0x80) >> 7));
                    int dxID = originalID - _IDConfig.OffsetOfEvc1000SaveNumID + _IDConfig.FirstSaveNumID;
                    if (dxID > _IDConfig.LastSaveNumID)
                        throw new Exception("セーブナンバーのIDが最高値を超えています。");
                    info.ReplacingBinary[4] = (byte)(((dxID & 0x0001) << 7) | (info.ReplacingBinary[4] & 0x7F));
                    info.ReplacingBinary[5] = (byte)(((dxID & 0x0100) >> 1) | ((dxID & 0x00FE) >> 1));
                    info.ReplacingBinary[6] = (byte)((info.ReplacingBinary[6] & 0xC0) | ((dxID & 0x7E00) >> 1));
                }
                // 置き換え処理
                for (int i = currentIndex, n = bytes.Length; i < n; ++i)
                {
                    int checkSuccessCounter = 0;
                    for (int j = 0; j < info.ByteSize; ++j)
                    {
                        byte b = (byte)(bytes[i + j] & info.BitMask[j]);
                        if (b != info.DummyBinary[j])
                            break;
                        ++checkSuccessCounter;
                    }
                    if (checkSuccessCounter == info.ByteSize)
                    {
                        for (int j = 0; j < info.ByteSize; ++j)
                        {
                            result[i + j] = info.ReplacingBinary[j];
                        }
                        int nopSize = _NOPConfig.NumOfNOPCommands * _NOPConfig.ByteSize;
                        for (int j = 0; j < nopSize; ++j)
                        {
                            int index = i + info.ByteSize + j;
                            result[index] = _NOPConfig.NOPCommandBinary[j % _NOPConfig.ByteSize];
                        }
                        currentIndex = i + info.ByteSize + nopSize;
                        break;
                    }
                }
            }

            // 結果を返す
            return result;
        }

        #endregion
    }
}
