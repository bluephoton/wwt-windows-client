using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace TerraViewer
{
    public class Language
    {
        public string Url;
        public string Name;
        public string Code;
        public string RootUrl;
        public string ImageSetsUrl;
        public string Proxy;
        public Language (string name, string url, string code, string rootUrl, string imageSetsUrl, string proxy)
        {
            Name = name;
            Url = url;
            Code = code;
            RootUrl = rootUrl;
            ImageSetsUrl = imageSetsUrl;
            Proxy = proxy;
        }

        public Language (string line)
        {
            var split = line.Split(new char[1] { '\t' });
            Code = split[0];
            Name = split[1];
            Url = split[2];
            RootUrl = split[3];
        }

        public Language(XmlNode node)
        {
            Code = node.Attributes["Code"].Value;
            Name = node.Attributes["Name"].Value;
            Url = node.Attributes["Url"].Value;
            RootUrl = node.Attributes["Root"].Value;
            ImageSetsUrl = node.Attributes["ImageSetsUrl"].Value;
            if (node.Attributes["Proxy"] != null)
            {
                Proxy = node.Attributes["Proxy"].Value;
            }

        }

        public override string ToString()
        {
            return Name;
        }

        static Dictionary<int, string> localizedStrings;

        static Language currentLanguage;

        public static Language CurrentLanguage
        {
            get
            { 
                return currentLanguage;
            }
            set 
            {
                                
                currentLanguage = value; 
                LoadLocalizedStrings(value);                
            }
        }

        static bool psudoLoc = false;
        public static string GetLocalizedText(int id, string defaultText)
        {
            if (localizedStrings != null && localizedStrings.ContainsKey(id))
            {
                if (psudoLoc)
                {
                    return "@@" + localizedStrings[id];
                }
                return localizedStrings[id];
            }
            if (psudoLoc)
            {
                return "@@" + defaultText;
            }
            return defaultText;
        }


        public static void LoadLocalizedStrings(Language l)
        {
            var path = string.Format(@"{0}\Data\Localization", Properties.Settings.Default.CahceDirectory);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filename = string.Format(@"{0}\Data\Localization\lang_{1}.tdf", Properties.Settings.Default.CahceDirectory, Math.Abs(l.Url.GetHashCode32()));
            DataSetManager.DownloadFile(l.Url, filename, false, false);

            //Stream fs = (Stream)Assembly.GetExecutingAssembly().GetManifestResourceStream("IMDFrame.Localization." + string.Format("lang_{0}.tdf", l.Code));
            try
            {
                Stream fs = File.Open(filename, FileMode.Open);
                var sr = new StreamReader(fs, Encoding.UTF8);


                localizedStrings = new Dictionary<int, string>();
                try
                {
                    while (!sr.EndOfStream)
                    {
                        try
                        {
                            var line = sr.ReadLine();

                            var split = line.Split(new char[1] { '\t' });
                            var text = split[1];
                            text = text.Replace("\\n", "\n");

                            localizedStrings.Add(Convert.ToInt32(split[0]), text);
                        }
                        catch
                        {
                        }
                    }
                }
                finally
                {
                    sr.Close();
                    fs.Close();
                }
            }
            catch
            {
                return;
            }
        }
       
        public static Language[] GetAvailableLanguages()
        {
            var filename = string.Format(@"{0}\Data\Localization\Languages.xml", Properties.Settings.Default.CahceDirectory);
            DataSetManager.DownloadFile("http://www.worldwidetelescope.org/wwtweb/catalog.aspx?X=Languages", filename, false, false);
            var list = new List<Language>();
            var doc = new XmlDocument();
            doc.Load(filename);

            XmlNode root = doc["LanguagePacks"];

            if (root != null)
            {
                foreach (XmlNode child in root.ChildNodes)
                {
                    list.Add(new Language(child));
                }
            }

            // Add Local Langues Pack

            var local = new Language("Load Local Language Pack", "", "ZZZZ", Properties.Settings.Default.ExploreRootUrl,
                             Properties.Settings.Default.ImageSetUrl,"");
            list.Add(local);


            return list.ToArray();
        }
    }
    //public class LanguageItem
    //{
    //    public string Url;
    //    public string Name;
    //    public string Code;
    //    public LanguageItem (string name,string url, string code)
    //    {
    //        Name = name;
    //        Url = url;
    //        Code = code;
    //    }

    //    public LanguageItem (string line)
    //    {
    //        string[] split = line.Split(new char[1] { '\t' });
    //        Code = split[0];
    //        Name = split[1];
    //        Url = split[2];
            
    //    }    

    //    public override string ToString()
    //    {
    //        return Name;
    //    }
    //}
}
