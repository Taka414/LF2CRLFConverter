//
// Copyright (C) 2018 Taka All Rights Reserved.
//

using System;
using System.IO;
using System.Reflection;

namespace Takap.Utility.Convertion
{
    public class ShowHelpFunction
    {
        public void ShowHelpMessage()
        {
            foreach (string msg in this.GetHelpResource())
            {
                Console.WriteLine(msg);
            }
        }

        public string[] GetHelpResource()
        {
            // namespace + "." + sub-dir + "." + resource-name
            using (var strema = Assembly.GetExecutingAssembly().GetManifestResourceStream("Takap.Utility.Convertion.Assets.help-message.txt"))
            using (var sr = new StreamReader(strema))
            {
                return sr.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }
        }
    }
}