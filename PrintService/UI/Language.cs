using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace PrintService.UI
{
    /// <summary>
    /// Load lanaguage
    /// </summary>
    public class Language
    {
        private Dictionary<string, string> languages = null;
        private const string DefaultLanguageHint = "Default(En_US)";
        private string languageHint = "";

        private static Language instance = null;

        /// <summary>
        /// single instance mode
        /// </summary>
        /// <returns></returns>
        public static Language Instance()
        {
            if (instance == null)
            {
                instance = new Language();
            }
            return instance;
        }

        public Language()
        {
        }

        /// <summary>
        /// 更改语言
        /// </summary>
        /// <param name="languageFile"></param>
        public void ChangLanguage(string languageFilePath)
        {
            try
            {
                if (File.Exists(languageFilePath))
                {
                    var json = File.ReadAllText(languageFilePath);
                    var serializer = new JavaScriptSerializer();
                    this.languages = (Dictionary<string, string>)serializer.Deserialize<Dictionary<string, string>>(json);
                    this.languageHint = this.languages.ContainsKey("name") ? this.languages["name"] : "Unkonwn Language";
                }
            }
            catch
            {
             
            }
        }

        /// <summary>
        /// Get the lanaguage name
        /// </summary>
        /// <returns></returns>
        public string GetHint()
        {
            return this.languages == null ? DefaultLanguageHint : this.languageHint;
        }

        /// <summary>
        /// Get UI text with giving
        /// </summary>
        /// <param name="textKey"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public string GetText(string textKey, string defaultText)
        {
            if (this.languages != null && this.languages.ContainsKey(textKey))
            {
                return this.languages[textKey];
            }
            return defaultText;
        }
    }
}
