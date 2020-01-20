//
// (c) 2020 Takap.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Takap.Utility.Convertion
{
    /// <summary>
    /// 変換機能を表すクラス
    /// </summary>
    public class ConvertFunction
    {
        /// <summary>
        /// 指定したディレクトリを再帰的に検索して拡張子リストに一致するファイルの改行コードを変換します。
        /// </summary>
        public void Convert(string dir, IList<string> targetExtensions)
        {
            // 集計用のテーブル
            var enc_table = new Dictionary<string, int>();
            var ext_table = new Dictionary<string, int>();

            void func(string path)
            {
                string name = Path.GetFileName(path);
                string ext = Path.GetExtension(path);

                Encoding enc = EncodeUtil.GetCode(File.ReadAllBytes(path));
                if (enc == null)
                {
                    Console.WriteLine($"Unknown Encode = {path.Replace("dir", "")}");
                }

                if (!ext_table.ContainsKey(ext))
                {
                    ext_table[ext] = 0;
                }
                ext_table[ext]++;

                if (enc == null)
                {
                    if (!enc_table.ContainsKey("null"))
                    {
                        enc_table["null"] = 0;
                    }
                    enc_table["null"]++;
                }
                else
                {
                    if (!enc_table.ContainsKey(enc?.HeaderName))
                    {
                        enc_table[enc?.HeaderName] = 0;
                    }
                    enc_table[enc?.HeaderName]++;
                }

                foreach (string e in targetExtensions) // 拡張子に一致したら変換
                {
                    if (string.Compare(e, ext, true) == 0)
                    {
                        ConvertFile(path);
                    }
                }
            }

            if (Directory.Exists(dir))
            {
                string[] entries = Directory.GetFileSystemEntries(dir, "*", SearchOption.AllDirectories);
                foreach (string entry in entries)
                {
                    if (!File.Exists(entry))
                    {
                        continue;
                    }

                    func(entry);
                }
            }

            Console.WriteLine("");
            Console.WriteLine("--[info]--");
            Console.WriteLine("--encode--");
            foreach (var e in enc_table)
            {
                Console.WriteLine($"{e.Key} = {e.Value}");
            }

            Console.WriteLine("");
            Console.WriteLine("--extension--");
            foreach (var e in ext_table)
            {
                Console.WriteLine($"{e.Key} = {e.Value}");
            }
        }

        public static void ConvertFile(string path)
        {
            string name = Path.GetFileName(path);
            string ext = Path.GetExtension(path);

            Encoding enc = EncodeUtil.GetCode(File.ReadAllBytes(path));
            if (enc == null)
            {
                Console.WriteLine($"Unknwon encode, {name}");
                return;
            }

            string body = File.ReadAllText(path, enc);
            if (body.Contains("\r\n"))
            {
                Console.WriteLine("[skiped] " + Path.GetFileName(path));
                return; // 既に変換済み
            }

            body = body.Replace("\n", "\r\n");

            File.WriteAllText(path, body, enc);
            Console.WriteLine($"Converted : {name} [{enc.HeaderName}]");
        }

        /// <summary>
        /// 指定したパスから拡張子リストを取得します。
        /// </summary>
        public static IList<string> LoadExtensionList(string path)
        {
            IEnumerable<string> f()
            {
                foreach (string item in File.ReadLines(path))
                {
                    yield return item;
                }
            }

            return f().ToList();
        }
    }
}