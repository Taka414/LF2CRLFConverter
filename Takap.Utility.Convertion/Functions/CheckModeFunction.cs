//
// (c) 2020 Takap.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Takap.Utility.Convertion
{
    /// <summary>
    /// チェックモードの機能を表すクラス
    /// </summary>
    public class CheckModeFunction
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 拡張子のテーブル
        /// TKey   : 検出した拡張子の名前
        /// TValue : 検出回数
        /// </summary>
        public Dictionary<string, int> ExtensionTable { get; private set; } = new Dictionary<string, int>();

        /// <summary>
        /// エンコードのテーブル
        /// TKey   : 検出したエンコード
        /// TValue : 検出回数
        /// </summary>
        public Dictionary<Encoding, int> EncodingTable { get; private set; } = new Dictionary<Encoding, int>();

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 検索開始フォルダを指定して再帰的にチェックを行います。
        /// </summary>
        public void Check(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Console.WriteLine($"Directory not found = [{dir}]");
                return;
            }

            foreach (string path in Directory.EnumerateFileSystemEntries(dir, "*", SearchOption.AllDirectories))
            {
                if (Directory.Exists(path))
                {
                    continue;
                }

                if (!File.Exists(path))
                {
                    continue;
                }

                // 拡張子のカウント
                string ext = Path.GetExtension(path);
                if (!this.ExtensionTable.ContainsKey(ext))
                {
                    this.ExtensionTable[ext] = 0;
                }
                this.ExtensionTable[ext]++;

                // エンコードのカウント
                byte[] body = File.ReadAllBytes(path);
                Encoding enc = EncodeUtil.GetCode(body);
                if (enc == null)
                {
                    Console.WriteLine($"Unknown encode = {path.Replace(dir, "").Trim('\\')}");
                    continue;
                }

                if (!this.EncodingTable.ContainsKey(enc))
                {
                    this.EncodingTable[enc] = 0;
                }
                this.EncodingTable[enc]++;
            }
        }

        /// <summary>
        /// 結果をコンソール上に表示します。
        /// </summary>
        public void ShowResultToConsole()
        {
            Console.WriteLine("");
            Console.WriteLine("---- Extensions ----");
            foreach (var item in this.ExtensionTable)
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }

            Console.WriteLine("");
            Console.WriteLine("---- Encoding ----");
            foreach (var item in this.EncodingTable)
            {
                Console.WriteLine($"{item.Key.EncodingName} = {item.Value}");
            }
        }
    }
}