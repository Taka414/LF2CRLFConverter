//
// (c) 2020 Takap.
//

using System;
using System.IO;
using System.Reflection;

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
}