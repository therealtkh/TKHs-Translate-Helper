using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translate_Helper
{
    class TagString
    {
        // Two values in TagString: "tag name" and "tag value"
        public string tagName { get; set; }     // tag name
        public string tagValue { get; set; }    // original text
        public string tagTrans { get; set; }    // translated text

        // If an empty TagString is created, default value is "" = Nothing
        public TagString()
        {
            tagName = tagValue = tagTrans= "";
        }

        // Makes it possible to create an already filled TagString
        public TagString(string _tag, string _value, string _trans)
        {
            tagName = _tag;
            tagValue = _value;
            tagTrans = _trans;
        }

        // Obsolete since we're using a JSON library
        // Returns a JSON formatted row with both tag name and value ready for export
        public string PrintJsonRowOrg()
        {
            return "\"" + tagName + "\"" + " : " + "\"" + tagValue + "\",";
        }

        // Obsolete since we're using a JSON library
        // Returns a JSON formatted row but without the comma at the end
        public string PrintLastJsonRowOrg()
        {
            return "\"" + tagName + "\"" + " : " + "\"" + tagValue + "\"";
        }

        // Obsolete since we're using a JSON library
        // Returns a JSON formatted row with both tag name and translated value ready for export
        public string PrintJsonRowTrans()
        {
            return "\"" + tagName + "\"" + " : " + "\"" + tagTrans + "\",";
        }

        // Obsolete since we're using a JSON library
        // Returns a JSON formatted row but without the comma at the end
        public string PrintLastJsonRowTrans()
        {
            return "\"" + tagName + "\"" + " : " + "\"" + tagTrans + "\"";
        }

        // Obsolete since we're using a JSON library
        // Returns a JSON formatted row with both tag name and translated value ready for export but in HTML
        public string PrintJsonRowTransHTML()
        {
            return "\"" + tagName + "\"" + " : " + "\"" + myHTMLconverter(tagTrans) + "\",";
        }

        // Obsolete since we're using a JSON library
        // Returns a JSON formatted row but without the comma at the end in HTML
        public string PrintLastJsonRowTransHTML()
        {
            return "\"" + tagName + "\"" + " : " + "\"" + myHTMLconverter(tagTrans) + "\"";
        }

        // Temporary function to convert a string to HTML
        // Works with Chinese characters as well since UTF8 doesn't seem to work
        public static string myHTMLconverter(string inputStr)
        {
            StringBuilder returnStr = new StringBuilder();

            foreach (char c in inputStr)
            {
                int value = Convert.ToInt32(c);
                if (c > 127)
                    returnStr.AppendFormat("&#{0:0000};", value);
                else
                    returnStr.Append(c);
            }

            return returnStr.ToString();
        }

    }

}
