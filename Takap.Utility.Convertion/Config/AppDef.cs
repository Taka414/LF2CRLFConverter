//
// (c) 2020 Takap.
//

using System.IO;
using System.Reflection;

namespace Takap.Utility.Convertion
{
    /// <summary>
    /// アプリケーションの定義を取得します。
    /// </summary>
    public static class AppDef
    {
        public static string GetExtensionListPath(Assembly entry)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "extensions.txt");
        }

        public static string GetNoteTextPath(Assembly entry)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "note.txt");
        }
    }
}