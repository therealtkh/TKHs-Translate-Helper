using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//using System.Runtime.Serialization;                       // Use with the old XML method - to be removed
//using System.Xml;                                         // Use with the old XML method - to be removed
//using System.Runtime.Serialization.Json;                  // Use with the old XML method - to be removed
//using System.Xml.Linq;                                    // Use with the old XML method - to be removed
//using System.Xml.XPath;                                   // Use with the old XML method - to be removed
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Translate_Helper
{
    public partial class TranslateHelper : Form
    {
        List<TagString> mAllData = new List<TagString>();   // mAllData contains all data (tag+org+trans)
        JObject output = new JObject();                     // A "virtual" JSON file - to be saved to file
        string m_lastFolder = "";                           // Remember where we opened a file, will be default for saving later
        bool haveSaved = true;                              // If user has not saved, give warning before exit application
        bool duplicationFound = false;                      // Should not happen, but who knows?
        int nrOfTags = 0;                                   // Keeps track of how many tags we have
        int search_column = 1;                              // Default search column (org)
        int current_selected = 0;                           // This part is for the find-next function
        int translations_orange_left = 0;                   // Keeps track of how many orange lines are left
        int translations_yellow_left = 0;                   // Keeps track of how many yellow lines are left
        Encoding inputEncoding;                             // What encoding should be used to read a file
        Encoding outputEncoding;                            // What encoding should be used when saving a file
        bool isHtml = false;                                // Set to true if HTML export is activated

        public TranslateHelper()
        {
            InitializeComponent();
        }

        private void TranslateHelper_Load(object sender, EventArgs e)
        {
            cb_input_encoding.SelectedIndex = 1;            // Default input encoding, 1 = UTF-8
            cb_output_encoding.SelectedIndex = 2;           // Default output encoding, 2 = UTF-8 w/o BOM
            btn_reset_tag.Enabled = false;                  
            btn_reset_tag_all.Enabled = false;
            cb_countBoth.Enabled = false;
            btn_export.Enabled = false;
            cb_export_row_numbers.Enabled = false;
            chkbx_html.Visible = false;                     // Suggestion to remove HTML export. Hidden until further notice.

            // Add a lot of tooltips
            toolTip1.SetToolTip(btn_openOrg, "Browse for original file, probably the English file.");
            toolTip1.SetToolTip(btn_openTrans, "Browse for a previous translation file.");
            toolTip1.SetToolTip(btn_save, "Save your work into a new file.");
            toolTip1.SetToolTip(rb_tag, "Search within the tag name column.");
            toolTip1.SetToolTip(tb_org, "Search within the original text column.");
            toolTip1.SetToolTip(tb_trans, "Search within the translated text column.");
            toolTip1.SetToolTip(btn_search_prev, "Search for previous occurance (upwards).");
            toolTip1.SetToolTip(btn_search_next, "Search for next occurance (downwards).");
            toolTip1.SetToolTip(btn_reset_tag, "Reset translated text on current row to original text.");
            toolTip1.SetToolTip(btn_reset_tag_all, "Set all missing translations to original texts.");
            toolTip1.SetToolTip(btn_prev_item, "Jump to previous yellow or orange row.");
            toolTip1.SetToolTip(btn_next_item, "Jump to next yellow or orange row.");
            toolTip1.SetToolTip(lbl_translations_left, "Number of orange or yellow + orange rows left.");
            toolTip1.SetToolTip(lbl_encoding_read, "Experimental - What encoding to expect from input file(s).");
            toolTip1.SetToolTip(lbl_encoding_write, "Experimental - What encoding output file(s) shall have.");
            toolTip1.SetToolTip(chkbx_html, "Experimental - Outputs (more) special characters in HTML format.");
            toolTip1.SetToolTip(btn_export, "Export file to a tabbed text file (no JSON formatting).");
            toolTip1.SetToolTip(cb_export_row_numbers, "Adds the row number in the exported text file.");
            toolTip1.SetToolTip(cb_showRowHeaders, "Shows row numbers in the translation matrix.");
        }

        // Use this function to append new text to the log window.
        private void writeLog(string logText)
        {
            tb_log.AppendText(Environment.NewLine);                         // Always begin with a new row
            tb_log.AppendText(logText);                                     // Append new log text
        }

        // Open original file (probably the English file).
        private void btn_openOrg_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";  // .json or All Files
            dlg.InitialDirectory = m_lastFolder;
            if (dlg.ShowDialog() == DialogResult.OK)                        
            {
                openFile(dlg.FileName, "org");                              // Open original file
                writeLog("Finished work on the original file.");
            }
        }

        // Open file to compare with (probably a translation file).
        private void btn_openTrans_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = m_lastFolder;
            dlg.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";  // .json or All Files
            if (dlg.ShowDialog() == DialogResult.OK)                        // If file is opened
            {
                openFile(dlg.FileName, "trans");                            // Open translation file
                writeLog("Finished work on the translation file.");
                haveSaved = true;                                           // When a file is opened, it is per definition already saved
            }
        }

        // Used for open_org, open_trans and after file_saved, send box = "org" or "trans" to select original or translation mode
        private void openFile(string filename, string box)
        {
            m_lastFolder = Path.GetFullPath(filename);                          // Remember folder path (working folder)

            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;   // Disable the changed value event while opening a file
            if (dataGridView1.DataSource != null)                               // Disable auto-size when opening another file, this saves a lot of time
            {
                dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                //dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                //dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            }

            if (box == "org")
            {
                tb_org.Text = filename;
                tb_trans.Text = "Translation file. This is probably a previous version of it.";
                mAllData.Clear();                                           // Always clear list in memory when opening new input file
                FillList(filename, box);                                    // Fill list with original data.
                FillAllData();                                              // Refresh datagridview with new data
                nrOfTags = CountTags();                                     
                writeLog("Imported nr of tags: " + nrOfTags.ToString());    // Mostly for debug purposes
                if (duplicationFound == true)
                {
                    System.Windows.Forms.MessageBox.Show("Duplicated tag(s) found, see log for more information.", "Translate Helper Information");
                    duplicationFound = false;
                }
                // Change a lot of layout things after opening original file
                btn_openTrans.Enabled = true;
                btn_save.Enabled = true;
                btn_next_item.Enabled = true;
                btn_prev_item.Enabled = true;
                btn_search_prev.Enabled = true;
                btn_search_next.Enabled = true;
                tb_search.ReadOnly = false;
                btn_reset_tag.Enabled = true;
                btn_reset_tag_all.Enabled = true;
                cb_countBoth.Enabled = true;
                btn_export.Enabled = true;
                cb_export_row_numbers.Enabled = true;
                cb_showRowHeaders.Enabled = true;
            }
            else if (box == "trans")                                            // Opening translation file
            {
                tb_trans.Text = filename;
                for (int i = 0; i < mAllData.Count - 1; i++)                    // Clear all old translation fields first, in case some other file was opened before
                {
                    mAllData[i].tagTrans = "";                                  // Set (old) content to null, will be done regardless of if this is first or later opening of translation file
                }
                FillList(filename, box);                                        // Fill list with translation data.
                FillAllData();                                                  // Fill datagrid with current list data
                // dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                // dataGridView1.Columns[0].Width = 2 * dataGridView1.Width / 10;
                // dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                // dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                // dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            }

            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;   // Enable the changed value event while opening a file
        }

        // Updates the read/write, a leftover from before and might be useless
        private void CheckReadWrite()
        {
            dataGridView1.Columns[0].ReadOnly = true;                           // Original tag value cannot be changed
            dataGridView1.Columns[1].ReadOnly = true;                           // Tag Value is always readonly
            dataGridView1.Columns[2].ReadOnly = false;                          // Translation value can always be changed
        }

        //Refresh dataGridView with current data (clear + fill).
        private void FillAllData()
        {
            dataGridView1.DataSource = null;                                    // I don't know if these two lines...
            dataGridView1.DataSource = mAllData;                                // ...are really needed.
            AdjustColumns();                                                    // Set the format of the grid
            translations_orange_left = 0;                                       // Number of rows left to translate...
            translations_yellow_left = 0;                                       // ...will be counted in "MatchRowColor".
            MatchColors();                                                      // Always do color check after running FillAllData
            CheckReadWrite();                                                   // Always after fill, check read/write of columns
        }

        //Event for value change in dataGridView
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int changedRow = dataGridView1.CurrentCell.RowIndex;
            int changedCol = dataGridView1.CurrentCell.ColumnIndex;

            if (changedCol == 2)                                                // If changed value is the translated value (can't be anything else...)
            {
                if (mAllData[changedRow].tagName == "")                         // If no tagName, clear value
                {
                    mAllData[changedRow].tagTrans = "";
                    writeLog("Row,column: " + changedRow.ToString() + "," + changedCol.ToString() + " is not a valid cell to edit (no tag name), cell cleared.");
                }
                else
                    writeLog("Updated value in row,column: " + (changedRow + 1).ToString() + "," + (changedCol + 1).ToString() + ".");

                if (string.IsNullOrEmpty(mAllData[changedRow].tagTrans))        // If value is cleared...
                    mAllData[changedRow].tagTrans = "";                         // ...hange "null string" to ""

                haveSaved = false;                                              // Any value is changed, remind user upon closing if not saved
            }
            MatchRowColor(changedRow);
        }

        // When value is updated in dataGridView, run this to refresh colors.
        // Matches up colors in dataGridView according to:
        // White = Value is different, probably translated and ok.
        // Yellow = Value is the same in original and translation file.
        // Orange = Value is missing, update required!
        private void MatchRowColor(int row)
        {
            if (string.IsNullOrEmpty(mAllData[row].tagName) == false && mAllData[row].tagValue == mAllData[row].tagTrans)  // If the change turned equal
            {
                if (dataGridView1.Rows[row].DefaultCellStyle.BackColor == Color.Orange) //This is if it had a previous value
                {
                    translations_orange_left--;
                }
                dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.Yellow; //If values are the same - Yellow
                translations_yellow_left++;
            }
            else if (string.IsNullOrEmpty(mAllData[row].tagName) == false &&
                    (string.IsNullOrEmpty(mAllData[row].tagValue) || string.IsNullOrEmpty(mAllData[row].tagTrans))) // not equal, but one of them empty
            {
                if (dataGridView1.Rows[row].DefaultCellStyle.BackColor != Color.Orange) // if it wasn't already orange
                {
                    translations_orange_left++;
                    if (dataGridView1.Rows[row].DefaultCellStyle.BackColor == Color.Yellow)  // if it was yellow but not any longer
                        translations_yellow_left--;
                }
                dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.Orange; //If any value is different - orange
            }
            else if (string.IsNullOrEmpty(mAllData[row].tagName) == false && mAllData[row].tagValue != mAllData[row].tagTrans) // simply not equal
            {
                if (dataGridView1.Rows[row].DefaultCellStyle.BackColor == Color.Orange) // if it was orange before turning white
                {
                    translations_orange_left--;
                }
                else if (dataGridView1.Rows[row].DefaultCellStyle.BackColor == Color.Yellow) // if it was yellow before turning white
                {
                    translations_yellow_left--;
                }
                dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.White; //If different values - White
            }
            else if (mAllData[row].tagName == "" && mAllData[row].tagValue == "" && mAllData[row].tagTrans == "") // should not happen any longer?
            {
                if (dataGridView1.Rows[row].DefaultCellStyle.BackColor == Color.Orange)
                {
                    translations_orange_left--;
                }
                dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.White; //If all three values are blank, set white color
            }

            if (cb_countBoth.Checked == true)
                lbl_translations_left.Text = (translations_orange_left + translations_yellow_left).ToString();
            else
                lbl_translations_left.Text = translations_orange_left.ToString();
        }

        //Same as MatchRowColor, but for all data
        private void MatchColors()
        {
            for (int i = 0; i < mAllData.Count; i++)
            {
                MatchRowColor(i);
            }
            writeLog("Performed complete color matching.");
        }
        /*
                //Checks a string for &quot; and returns a formatted string for display purpose, to be converted back later!
                private string changeFromQuot(string inStr)
                {
                    return inStr.Replace("&quot;", "\"");
                    //return inStr.Replace("\"", "&quot;");
                }

               //Checks a string for quotes and returns a formatted string for save purpose
                private string changeToQuot(string inStr)
                {
                    return inStr.Replace("\"", "&quot;");
                }
        */
        //Checks a string for &quot; and returns a formatted string for display purpose, to be converted back later!
        /*       private string changeFromSpecial(string str)
               {
                   if (str.Contains("&quot;"))
                       str = str.Replace("&quot;", "\"");
                   if (str.Contains("&frasl;"))
                       str = str.Replace("&frasl;", "/");

                   return str;
               }
               */
        //Checks a string for quotes and returns a formatted string for save purpose
        /*       private string changeToSpecial(string str)
               {
                   if (str.Contains("\""))
                       str = str.Replace("\"", "&quot;");
                   if (str.Contains("/"))
                       str = str.Replace("/", "&frasl;");

                   return str;
               }
        */
        //Check if first character in a string is a letter
        // Probably obsolete now when json.net is used everywhere
        private bool IsLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        //Fills list with new input from file, both from original and translation.
        private void FillList(string FileName, string box)
        {
            try
            {

                // Load translation document so we can edit it
/*                XmlDictionaryReaderQuotas quota = new XmlDictionaryReaderQuotas();
                quota.MaxNameTableCharCount = 1000000;
// Removed when json.net was used for saving as well                
                if (box == "org")
                {
                    using (StreamReader sr = new StreamReader(FileName, inputEncoding))
                    {
                        XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(sr.BaseStream, quota);
                        mXElement = XDocument.Load(reader);
                        mNameTable = reader.NameTable;
                    }
                }
*/                
                // Always load into tags
                using (StreamReader sr = new StreamReader(FileName, inputEncoding))
                {
                    JsonTextReader reader = new JsonTextReader(sr);
                    List<string> currentPath = new List<string>();                  // A list where every new PropertyName is saved (the "path", and depth of the text)
                    int pathDepth = -1;                                             // Start from -1 because it will become 0 with the first StartObject
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)              // StartObject should mean '{'
                        {
                            pathDepth++;                                            // Go one step deeper in the path
                            currentPath.Add("");                                    // Add another item in the list
                        }
                        else if (reader.TokenType == JsonToken.PropertyName)        // PropertyName means a new "key", "tag" or "item"
                        {
                            currentPath[pathDepth] = reader.Value.ToString();
/*                          These rows were only needed for special naming when using the XML library, should not be needed with the new json.net library
                            string name = reader.Value.ToString();                  // This is the name of the key
                            if (!IsLetter(name[0]))                                     // If key begins with a number or special character (like #)...
                                currentPath[pathDepth] = ("*[@item='" + name + "']");   // ...add a prefix. The prefix will not be saved.
                            else
                                currentPath[pathDepth] = name;                      // Save the key name to the list. 
*/
                        }
                        else if (reader.TokenType == JsonToken.EndObject)           // EndObject should mean '}'
                        {
                            currentPath.RemoveAt(pathDepth);                        // Remove last item in the list, could also use (currentPath.Count - 1)
                            pathDepth--;                                            // Depth is 1 smaller for every EndObject
                        }
                        else if (reader.TokenType == JsonToken.String)              // Should mean the text or value of the key
                        {
                            TagString newItem = new TagString();                    // One item consists of a tag + value
                            string path = String.Join(".", currentPath.ToArray());  // The full path is saved as one long string, with '.' as separator
                            newItem.tagName = path;                                 // Full path in the first column...
                            newItem.tagValue = reader.Value.ToString();             // ...and full string in the second

                            if (box == "org")
                            {
                                mAllData.Add(newItem);                                  // Add new item to list
                            }
                            else if (box == "trans")
                            {
                                int tagIndex = -1;                                              // This will sort translation according to original.
                                tagIndex = mAllData.FindIndex(x => x.tagName.Equals(path));
                                if (tagIndex >= 0)                                              // This means that item exists
                                {
                                    mAllData[tagIndex].tagTrans = reader.Value.ToString();      // Add value to list in correct place.
                                }
                                else                                                            // Tag do not exist in original, add only in translation kolumn
                                {
                                    //newItem.tagValue = "";                                      // Tag did not exist, value has to be nothing!
                                    //newItem.tagTrans = (reader.Value.ToString());             
                                    //mAllData.Add(newItem);                                      // Fill list with new item anyway
                                    writeLog("Not found or added, tag: [" + path + "] value: " + reader.Value.ToString());
                                }
                            }
                        }
                    }

// This is the previous reading method, using XML 
/* --------------------
                    //XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(sr.BaseStream, quota);
                    //List<string> currentPath = new List<string>();
                    //while (reader.Read())
                    //{
                    //   if (reader.NodeType == XmlNodeType.Element)
                    //   {
                    //      string name = reader.Name;
                    //      if (name.Equals("a:item"))
                    //      {
                    //         string id = reader.GetAttribute("item");
                    //         currentPath.Add("*[@item='" + id +"']");
                    //      }
                    //      else if (!name.Equals("root"))
                    //      {
                    //         currentPath.Add(name);
                    //      }
                    //   }
                    //   else if (reader.NodeType == XmlNodeType.EndElement)
                    //   {
                    //       if (currentPath.Count > 0)                            // Don't do this for the last item in the file where depth is 0
                    //           currentPath.RemoveAt(currentPath.Count - 1);    
                    //   }
                    //   else if (reader.NodeType == XmlNodeType.Text)
                    //   {
                    //      TagString newItem = new TagString();
                    //      string path = String.Join(".", currentPath.ToArray());
                    //      newItem.tagName = path;
                    //      newItem.tagValue = changeFromSpecial(reader.Value);               //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    //      if (box == "org")
                    //      {
                    //         mAllData.Add(newItem);                               //Fill list with new item.
                    //      }

                    //      else if (box == "trans")
                    //      {
                    //         int tagIndex = -1;                                                 //This will sort translation according to original.
                    //         tagIndex = mAllData.FindIndex(x => x.tagName.Equals(path));
                    //         if (tagIndex >= 0)                              //This means that item exists
                    //         {
                    //            mAllData[tagIndex].tagTrans = changeFromSpecial(reader.Value);   //Add value to list in correct place. [Used to be 4]           !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    //         }
                    //         else                                            //Tag do not exist in original, add only in translation kolumn
                    //         {
                    //            newItem.tagValue = "";                      //Tag did not exist, value has to be nothing!
                    //            newItem.tagTrans = changeFromSpecial(reader.Value);             //Item 3 is value. [Used to be 4]               !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    //            mAllData.Add(newItem);                       //Fill list with new item.
                    //         }
                    //      }
                    //   }
                    //}
--------------------*/
                }
            }
            catch (Exception e)
            {
                if (box == "org")
                    writeLog("Something went wrong when opening original file...");
                if (box == "trans")
                    writeLog("Something went wrong when opening translation file...");
                writeLog("Error message:");
                writeLog(Convert.ToString(e));
            }
        }

        //Save translate column into new file with tag.
        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)                                // If filename and everything is ok to be saved...
            {
                output = new JObject();
                SaveAs(dlg.FileName);                                               // Run the save file function
                DialogResult dialogResult = MessageBox.Show("File saved, do you want to open it?\n\n" +
                    "This will reload the grid with data\nfrom both original and the new file.", "Translate Helper Question", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)                               // Ask user if the saved file should be opened
                {
                    openFile(tb_org.Text, "org");                                   // Open original file again
                    writeLog("Opened Original file again.");
                    openFile(dlg.FileName, "trans");                                // Open translation file again (using the new file)
                    writeLog("Opened the new translation file.");
                }
                haveSaved = true;                                                   // File saved, value = true
            }
        }

        //The function to save all data to a new file.
        private void SaveAs(string newFilename)
        {
            for (int i = 0; i < mAllData.Count; i++)
            {
                if (mAllData[i].tagName != "")                            // Only save rows with both name and value
                    AddProperty(mAllData[i].tagName, mAllData[i].tagTrans);
                else                                                                                    // Might happen if a tag is removed in a new file...
                    writeLog("mAllData[" + i.ToString() + "] was empty, line not added.");              // ...then "value" will be blank, but not "name" and "trans"...
            }                                                                                           // ...so row should not be saved

            var jsonFile = new JsonSerializer();
            using (StreamWriter file = File.CreateText(newFilename))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    jsonFile.Formatting = Newtonsoft.Json.Formatting.Indented;
                    jsonFile.Serialize(writer, output);      
                    writer.Flush();
                }
            }

            /* THE PREVIOUS METHOD
                        XmlNamespaceManager manager = new XmlNamespaceManager(mNameTable);              // I don't know exactly...
                        manager.AddNamespace("a", manager.DefaultNamespace);                            // how this works.

                        using (StreamWriter wr = new StreamWriter(newFilename, false, outputEncoding)) 
                        {
                            for (int i = 0; i < mAllData.Count; i++)
                            {
                                string path = mAllData[i].tagName;                                      // Input path as xxx.yyy.zzz or similar
                                if (path != "")
                                {
                                    string xPath = "//" + path.Replace('.', '/');                       // Change path to xPath structure
                                    XElement element = mXElement.XPathSelectElement(xPath, manager);    // New element according to xPath
                                    string raw = mAllData[i].tagTrans;                                  // Used to be ChangeToSpecial
                                    string content = isHtml ? TagString.myHTMLconverter(raw) : raw;     // If HTML output is selected, convert data to HTML
                                    if (element != null)
                                    {
                                        element.Value = content;  
                                    }
                                    else
                                    {
                                        writeLog(content);                                              // This should hopefully never happen
                                    }
                                }
                            }
                            XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(wr.BaseStream, outputEncoding, true, true, "   ");        
                            mXElement.Save(writer);
                            writer.Flush();
                        }
            */
            writeLog("Saved to file: " + newFilename);
        }

        // This function adds the row of "path.path.path.key" with "value" to the global output object
        private void AddProperty(string path, string value)
        {
            List<string> pathList = new List<string>();
            pathList = path.Split('.').ToList();
            string searchPath = "";
            string addPath = "";

            // Result of a few trial/errors, it looks strange but works. Can probably be made a bit cleaner...
            for (int i = 0; i < pathList.Count; i++)                        // Traverse through the list of items (the full path)
            {
                if (i == 0)
                {
                    searchPath = pathList[i];                               // For root level, only update search path
                }
                else
                {
                    searchPath = searchPath + "." + pathList[i];            // Add next item we we get aaa.bbb.ccc 
                    if (i == pathList.Count - 1)                            // Last item in list = key value
                    {
                        addPath = addPath + "." + pathList[i - 1];          // addPath should always be previous value...
                    }
                    else
                    {
                        if (i > 1)
                            addPath = addPath + "." + pathList[i - 1];
                        else
                            addPath = pathList[i - 1];                      // ...except for when i == 0 or 1
                    }
                }

                if (output.SelectToken(searchPath) == null)                                     // Means not found - [i] must be added
                {
                    JObject tmpObj = output.SelectToken(addPath) as JObject;                    // Used to add a new key-pair or group
                    if (i == pathList.Count - 1)                                                // Last value in pathList is the key name
                    {
                        string content = isHtml ? TagString.myHTMLconverter(value) : value;     // If HTML output is selected, convert data to HTML
                        tmpObj.Add(new JProperty(pathList[i], content));                        // Add tag with its value
                    }
                    else
                    {
                        tmpObj.Add(new JProperty(pathList[i], new JObject()));                  // Add new empty property (a new group)
                    }
                }                                                                               
            }
        }

        //Run this to adjust column after new files are loaded. Most could be set from design view.
        private void AdjustColumns()
        {
            dataGridView1.Columns[0].Width = 2 * dataGridView1.Width / 10;
            //dataGridView1.Columns[1].Width = 4 * dataGridView1.Width / 10;
            //dataGridView1.Columns[2].Width = 4 * dataGridView1.Width / 10;
            //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Columns[0].HeaderText = "Tag name";
            dataGridView1.Columns[1].HeaderText = "Original text";
            dataGridView1.Columns[2].HeaderText = "Translated text";

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = String.Format("{0}", i + 1);
            }

        }

        // Used to warn the user of unsaved work
        private void TranslateHelper_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (haveSaved == false)                 // You have not saved, do you still want to exit?
            {
                if (MessageBox.Show("You have not saved! Do you still want to exit program?",
                    "Translate Helper Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;                // If answer is No - Abort the exit!
                                                    // Consider changing to YesNoCancel as SaveExitCancel...
            }
        }

// Osolete functions
/* ----------------
        // Search through the tags for a duplicate (true means a duplicate found)
        private bool SearchForDuplicate(string tagName)
        {
            int count = 0;                              //Number of occurances of tagName in original file
            for (int i = 0; i < mAllData.Count(); i++)
            {
                if (mAllData[i].tagName == tagName)      //see if new tag name already exists in mAllData
                    count++;
            }

            if (count > 0)                              // count > 0 means that it's already in the mAllData list
                return true;

            return false;
        }

        //Improved reading algorithm to be able to manage apostrophes within text. 
        private string[] SplitRow(string line)
        {
            string[] strSplit = line.Split('"');                //Split line on all apostrophes (there should be 4 on a normal row, but perhaps more)
            string[] returnString;

            if (strSplit.Length >= 4)                           //A row with valid content
            {
                returnString = new string[2];                   //Length = 2 in returnstring for content rows
                returnString[0] = strSplit[1];                  //Index 0 has tag name, index 1 has data (value)
                for (int i = 3; i < strSplit.Length - 1; i++)   //Value begins at index 3
                {
                    returnString[1] += strSplit[i];             //Rebuild the rest of the line if it contains any "
                    if (i != strSplit.Length - 2)               //Don't want anything after last apostrophe, that means after lenght - 2
                    {
                        returnString[1] += '"';                 //Add apostrophe if row had more than 4 in total
                    }
                }
            }
            else
                returnString = new string[1];                   //No valid content, return smaller array in that case
            return returnString;
        }
--------------*/
        //Count the number of tags in the mAllData list
        private int CountTags()
        {
            return mAllData.Where(x => x.tagName != "").ToList().Count();    // Return number of lines that are not ""
        }

        // Will try to locate the next "non-white" item. That means, missing or same translation
        private void btn_next_item_Click(object sender, EventArgs e)
        {
            for (int i = current_selected + 1; i <= mAllData.Count - 1; i++)            // From current position to end
            {
                if (mAllData[i].tagTrans == "" || mAllData[i].tagValue == "" || mAllData[i].tagValue == mAllData[i].tagTrans)  // No translation or same
                {
                    if (mAllData[i].tagName != "")                                      // Skip blank lines - should not happen any longer
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[2];     // Highlight cell
                        current_selected = i;                                           // Remember where we are now (for next find or search)
                        break;
                    }
                }
            }
        }

        // Same as previous function, but locate previous missing translation instead
        private void btn_prev_item_Click(object sender, EventArgs e)
        {
            for (int i = current_selected - 1; i >= 0; i--)                             // From current position to beginning
            {
                if (mAllData[i].tagTrans == "" || mAllData[i].tagValue == "" || mAllData[i].tagValue == mAllData[i].tagTrans)    //No translation or same
                {
                    if (mAllData[i].tagName != "")                                      // Skip blank lines
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[2];     // Highlight cell
                        current_selected = i;                                           // Remember where we are now (for next find or search)
                        break;
                    }
                }
            }
        }

        // Support function to "find-next" to keep track of where we are in the dataset
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            current_selected = dataGridView1.CurrentRow.Index;
        }

        // Keeps track of what column to search in, 0 = tag name
        private void rb_tag_CheckedChanged(object sender, EventArgs e)
        {
            search_column = 0;
        }

        // Keeps track of what column to search in, 1 = original text
        private void rb_org_CheckedChanged(object sender, EventArgs e)
        {
            search_column = 1;
        }

        // Keeps track of what column to search in, 2 = translation text
        private void rb_trans_CheckedChanged(object sender, EventArgs e)
        {
            search_column = 2;
        }

        // Search for next occurance of what is written in the search textbox
        private void btn_search_next_Click(object sender, EventArgs e)
        {
            if (tb_search.Text != "")                                           // Only start search if anything is written
            {
                // Used to have "i <= mAllData.Count - 1", but changed it together with the i-checks to be able to search forward from last row...
                for (int i = current_selected + 1; i <= mAllData.Count; i++)    // Run to end of dataset length or when "run" is set to false (match found)
                {
                    if (i <= mAllData.Count - 1 && search_result(i, search_column))   // If result is found, end loop (search)
                        break;
                    if (i == mAllData.Count - 1)                                // Reached end with nothing found
                    {
                        DialogResult dialogResult = MessageBox.Show("Reached end of data, do you want to continue from top?", "Translate Helper Question", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            current_selected = -1;                              // Start position where to begin next search
                            btn_search_next_Click(sender, e);                   // Call function from within function, ok..?
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            current_selected = dataGridView1.CurrentRow.Index;   // If clicked yes before, this must be changed back to where we are
                        }
                    }
                }
            }
        }

        // Help function to search next/prev to highlight match and stop loop
        private bool search_result(int i, int column)
        {
            //Tag should not be empty I guess? And compare search text to value in current cell
            if (mAllData[i].tagName != "" && dataGridView1.Rows[i].Cells[column].Value.ToString().ToLower().Contains(tb_search.Text.ToLower()))
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[column];    // Highlight what was found
                current_selected = i;                                               // Remember what row we have selected
                return true;                                                        // Match found, return true
            }
            return false;                                                           // Match not found, continue search
        }

        // Search for previous occurance of what is written in the search textbox
        private void btn_search_prev_Click(object sender, EventArgs e)
        {
            if (tb_search.Text != "")                                   // Only start search if anything is written
            {
                // Used to have "i >= 0", but changed it together with the i-checks to be able to search backwards from first row...
                for (int i = current_selected - 1; i >= -1; i--)        // Run to beginning of dataset or when "run" is set to false (match found)
                {
                    if (i >= 0 && search_result(i, search_column))      // If result is found, end loop (search)
                        break;
                    if (i < 0)                                          // Reached start with nothing found
                    {
                        DialogResult dialogResult = MessageBox.Show("Reached beginning of data, do you want to continue from end?", "Translate Helper Question", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            current_selected = mAllData.Count;          // Start position where to begin next search
                            btn_search_prev_Click(sender, e);           // Call function from within function, ok..?
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            current_selected = dataGridView1.CurrentRow.Index;  //If clicked yes before, this must be changed back to where we are
                        }
                    }
                }
            }
        }

        // Change input encoding (when reading file) if needed for some reason
        private void cb_input_encoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_input_encoding.SelectedIndex)
            {
                case 0:
                    inputEncoding = System.Text.Encoding.Default;
                    break;
                case 1:
                    inputEncoding = UTF8Encoding.UTF8;
                    break;
                case 2:
                    inputEncoding = new System.Text.UTF8Encoding(false);
                    break;
                case 3:
                    inputEncoding = new System.Text.UnicodeEncoding();
                    break;
            }
            writeLog("Input encoding set to: " + inputEncoding.EncodingName);
        }

        // Change output encoding (when writing file) preset if needed for some reason
        private void cb_output_encoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_output_encoding.SelectedIndex)
            {
                case 0:
                    outputEncoding = System.Text.Encoding.Default;
                    break;
                case 1:
                    outputEncoding = UTF8Encoding.UTF8;
                    break;
                case 2:
                    outputEncoding = new System.Text.UTF8Encoding(false);
                    break;
                case 3:
                    outputEncoding = new System.Text.UnicodeEncoding();
                    break;
            }
            writeLog("Output encoding set to: " + outputEncoding.EncodingName);
        }

        // Experimental - Outputs data HTML coded or not
        private void chkbx_html_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbx_html.Checked)
            {
                isHtml = true;
                writeLog("Will use HTML in export.");
            }
            else
            {
                isHtml = false;
                writeLog("Will not use HTML in export.");
            }
        }

        // The "remaining items to translate" value can show either yellow+orange or only orange
        private void cb_countBoth_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_countBoth.Checked == true)
                lbl_translations_left.Text = (translations_orange_left + translations_yellow_left).ToString();
            else
                lbl_translations_left.Text = translations_orange_left.ToString();
        }

        // If user clicks on reset button, the selected row will get translated value copied from original value
        private void btn_reset_tag_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[2];
            if (dataGridView1.Rows[row].Cells[1].Value != null)
            {
                dataGridView1.Rows[row].Cells[2].Value = dataGridView1.Rows[row].Cells[1].Value.ToString();
            }
        }

        // If user clicks "Reset All" button, all blank values are filled with the original text
        private void btn_reset_tag_all_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[2];
                if (!string.IsNullOrEmpty(mAllData[i].tagValue) && string.IsNullOrEmpty(mAllData[i].tagTrans))
                {
                    dataGridView1.Rows[i].Cells[2].Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                }
            }
        }

        // Function to export entire datagrid to a textfile
        private void btn_export_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";            // txt file is preferred
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter wr = new StreamWriter(dlg.FileName, false, System.Text.Encoding.UTF8))
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (cb_export_row_numbers.Checked == true)                  // If row numbers should be included
                            wr.WriteLine((i + 1).ToString() + "\t" + dataGridView1.Rows[i].Cells[0].Value.ToString()
                                    + "\t" + dataGridView1.Rows[i].Cells[1].Value.ToString()
                                    + "\t" + dataGridView1.Rows[i].Cells[2].Value.ToString());
                        else
                            wr.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString()
                                    + "\t" + dataGridView1.Rows[i].Cells[1].Value.ToString()
                                    + "\t" + dataGridView1.Rows[i].Cells[2].Value.ToString());
                    }
                }
                DialogResult dialogResult = MessageBox.Show("File saved, do you want to open it?", "Translate Helper Question", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)                               // Ask user if the saved file should be opened (using system preferred program)
                {
                    System.Diagnostics.Process.Start(dlg.FileName);
                }
            }
        }

        // Checkbox to show/hide row numbers
        private void cb_showRowHeaders_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_showRowHeaders.Checked)
                dataGridView1.RowHeadersVisible = true;
            else
                dataGridView1.RowHeadersVisible = false;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

    }
}
