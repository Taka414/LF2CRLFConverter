//
// Copyright (C) 2018 Taka All Rights Reserved.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Takap.Utility.Convertion
{
    /// <summary>
    /// メインクラス
    /// </summary>
    internal class AppMain
    {
        /// <summary>
        /// メイン・エントリポイント
        /// </summary>
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            string option = args[0].ToLower();
            switch (option)
            {
                case "/check":
                {
                    var func = new CheckModeFunction();
                    func.Check(args[1]);
                    func.ShowResultToConsole();

                    break;
                }
                case "/exec":
                {
                    var func = new ConvertFunction();
                    func.Convert(args[1], ConvertFunction.LoadExtensionList(args[2]));

                    break;
                }
                case "/?":
                {
                    ShowFormat();
                    break;
                }
                default:
                {
                    // オプション指定のないディレクトリだけの指定の場合も変換
                    if (Directory.Exists(args[0]))
                    {
                        var func = new ConvertFunction();
                        func.Convert(args[0], ConvertFunction.LoadExtensionList(AppDef.GetExtensionListPath(Assembly.GetExecutingAssembly())));
                    }
                    else
                    {
                        Console.WriteLine("<<書式エラー>>");
                        ShowFormat();
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 未チェックエラーの表示
        /// </summary>
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(File.ReadAllText(AppDef.GetNoteTextPath(Assembly.GetExecutingAssembly())));
        }

        /// <summary>
        /// note.txtの内容をコンソールに表示します。
        /// </summary>
        public static void ShowFormat()
        {
            Console.WriteLine(File.ReadAllText(AppDef.GetNoteTextPath(Assembly.GetExecutingAssembly())));
        }
    }

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