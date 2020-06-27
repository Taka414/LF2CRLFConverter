//
// Copyright (C) 2018 Taka All Rights Reserved.
//

using System;
using System.IO;
using System.Reflection;

namespace Takap.Utility.Convertion
{
    internal class AppMain
    {
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
                    new ShowHelpFunction().ShowHelpMessage();
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
                        new ShowHelpFunction().ShowHelpMessage();
                    }

                    break;
                }
            }
        }

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