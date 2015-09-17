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
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Translate_Helper
{
   public partial class TranslateHelper : Form
   {
      List<TagString> mAllData = new List<TagString>();    //mAllData contains all data (tag+org+trans)
      XElement mXElement = null;
      string m_lastFolder = "";
      bool haveSaved = true;
      bool duplicationFound = false;
      int nrOfTags = 0;                                   //Keeps track of how many tags we have
      int current_selected = 0;                           //This part is for the find-next function
      int translations_left = 0;                          //Keeps track of how many orange lines you have left
      int search_column = 1;                              //Default search column (org)
      Encoding inputEncoding;
      Encoding outputEncoding;
      bool isHtml = false;

      public TranslateHelper()
      {
         InitializeComponent();
      }

      private void TranslateHelper_Load(object sender, EventArgs e)
      {
         cb_input_encoding.SelectedIndex = 1;
         cb_output_encoding.SelectedIndex = 2;
      }

      //Use this function to append new text to the log window.
      private void writeLog(string logText)
      {
         tb_log.AppendText(Environment.NewLine);                         //Always begin with a new row
         tb_log.AppendText(logText);
      }

      //Open input/original file (perhaps the English file).
      private void btn_openOrg_Click(object sender, EventArgs e)
      {
         OpenFileDialog dlg = new OpenFileDialog();
         dlg.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
         dlg.InitialDirectory = m_lastFolder;
         if (dlg.ShowDialog() == DialogResult.OK)                        //If file is opened
         {
            openFile(dlg.FileName, "org");                              //Open original file
            writeLog("Finished work on the original file.");
         }
      }

      //Open translation file (perhaps the last version).
      private void btn_openTrans_Click(object sender, EventArgs e)
      {
         OpenFileDialog dlg = new OpenFileDialog();
         dlg.InitialDirectory = m_lastFolder;
         dlg.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
         if (dlg.ShowDialog() == DialogResult.OK)                        //If file is opened
         {
            openFile(dlg.FileName, "trans");                            //Open translation file
            writeLog("Finished work on the translation file.");
            haveSaved = true;
         }
      }

      //Used for open_org, open_trans and after file_saved, send box = "org" or "trans"
      private void openFile(string filename, string box)
      {
         m_lastFolder = Path.GetFullPath(filename);

         dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;   //Disable the changed value event while opening a file

         if (box == "org")
         {
            tb_org.Text = filename;
            tb_trans.Text = "Translation file. This is probably a previous version of it.";
            mAllData.Clear();                                            //Always clear list in memory when opening new input file
            FillList(filename, box);                                    //Fill list with original data.
            FillAllData();                                              //Refresh datagridview with new data
            writeLog("Imported nr of tags: " + countTags().ToString());
            if (duplicationFound == true)
            {
               System.Windows.Forms.MessageBox.Show("Duplicated tag(s) found, see log for more information.", "Translate Helper Information");
               duplicationFound = false;
            }
            //Change a lot of layout things after opening original file
            btn_openTrans.Enabled = true;
            btn_save.Enabled = true;
            btn_addRow.Enabled = true;
            btn_delRow.Enabled = true;
            checkBox1.Enabled = true;
            btn_next_item.Enabled = true;
            btn_prev_item.Enabled = true;
            btn_search_prev.Enabled = true;
            btn_search_next.Enabled = true;
            tb_search.ReadOnly = false;
         }
         else if (box == "trans")
         {
            tb_trans.Text = filename;
            for (int i = 0; i < mAllData.Count - 1; i++)         //Clear all old translation fields first, in case other file was opened first
            {
               mAllData[i].tagTrans = "";                     //Set (old) content to null, will be done regardless of if this is first or later opening of translation file
            }
            FillList(filename, box);                            //Fill list with translation data.
            FillAllData();                                      //Fill datagrid with current list data
         }

         dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;   //Enable the changed value event while opening a file
      }

      //Updates the read/write 
      private void checkReadWrite()
      {
         if (checkBox1.Checked == true)
            dataGridView1.Columns[0].ReadOnly = false;                      //Tag name is readonly, can be changed
         else
            dataGridView1.Columns[0].ReadOnly = true;                       //Original tag value cannot be changed

         dataGridView1.Columns[1].ReadOnly = true;                           //Tag Value is always readonly
         dataGridView1.Columns[2].ReadOnly = false;                          //Translation value can always be changed
      }

      //Refresh dataGridView with current data (clear + fill).
      private void FillAllData()
      {
         dataGridView1.DataSource = null;                                    //I don't know if these two lines...
         dataGridView1.DataSource = mAllData;                                 //...are really needed
         writeLog("Filled datagrid from list in memory.");
         adjustColumns();                                                    //Set the format of the grid
         translations_left = 0;                                              //Number of rows to translate, will be counted in "MatchRowColor"
         MatchColors();                                                      //Always do color check after running FillAllData
         checkReadWrite();                                                   //Always after fill, check read/write of columns
      }

      //Event for value change in dataGridView
      private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         int changedRow = dataGridView1.CurrentCell.RowIndex;
         int changedCol = dataGridView1.CurrentCell.ColumnIndex;

         if (changedCol == 2)                            //If changed value is the translated value
         {
            if (mAllData[changedRow].tagName == "")      //If no tagName, clear value
            {
               mAllData[changedRow].tagTrans = "";
               writeLog("Row,column: " + changedRow.ToString() + "," + changedCol.ToString() + " is not a valid cell to edit (no tag name), cell cleared.");
            }
            else
               writeLog("Updated value in row,column: " + changedRow.ToString() + "," + changedCol.ToString() + ".");

            if (string.IsNullOrEmpty(mAllData[changedRow].tagTrans)) //If value is cleared
               mAllData[changedRow].tagTrans = "";                  //Change "null string" to ""

            haveSaved = false;                          //Any value is changed - set value to false
         }
         else if (changedCol == 0)                                   //If changed value is the tag name
         {
            if (string.IsNullOrEmpty(mAllData[changedRow].tagName))  //If tag name is cleared
               mAllData[changedRow].tagName = "";                   //Change "null string" to ""
            writeLog("Updated tag value in row,column: " + changedRow.ToString() + "," + changedCol.ToString() + ".");
         }

         MatchRowColor(changedRow);
      }

      //When value is updated in dataGridView, run this to refresh colors.
      //Matches up colors in dataGridView according to:
      //White = Value is different, probably translated and ok.
      //Yellow = Value is the same in original and translation file.
      //Orange = Value is missing, update required!
      private void MatchRowColor(int row)
      {
         if (string.IsNullOrEmpty(mAllData[row].tagName) == false && mAllData[row].tagValue == mAllData[row].tagTrans)
         {
            if (dataGridView1.Rows[row].DefaultCellStyle.BackColor == Color.Orange)
            {
               translations_left--;
            }
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.Yellow; //If values are the same - Yellow
         }
         else if (string.IsNullOrEmpty(mAllData[row].tagName) == false &&
                 (string.IsNullOrEmpty(mAllData[row].tagValue) || string.IsNullOrEmpty(mAllData[row].tagTrans)))
         {
            if (dataGridView1.Rows[row].DefaultCellStyle.BackColor != Color.Orange)
            {
               translations_left++;
            }
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.Orange; //If any value is different - orange
         }
         else if (string.IsNullOrEmpty(mAllData[row].tagName) == false && mAllData[row].tagValue != mAllData[row].tagTrans)
         {
            if (dataGridView1.Rows[row].DefaultCellStyle.BackColor == Color.Orange)
            {
               translations_left--;
            }
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.White; //If different values - White
         }
         else if (mAllData[row].tagName == "" && mAllData[row].tagValue == "" && mAllData[row].tagTrans == "")
         {
            if (dataGridView1.Rows[row].DefaultCellStyle.BackColor == Color.Orange)
            {
               translations_left--;
            }
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.White; //If all three values are blank, set white color
         }
         lbl_translations_left.Text = translations_left.ToString();
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

      //Fills list with new input from file, both from original and translation.
      private void FillList(string FileName, string box)
      {
         bool isTranslation = (box == "trans");
         try
         {

            // load translation document so we can edit it
            XmlDictionaryReaderQuotas quota = new XmlDictionaryReaderQuotas();
            quota.MaxNameTableCharCount = 500000;
            if (isTranslation)
            {
               using (StreamReader sr = new StreamReader(FileName, inputEncoding))
               {
                  XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(sr.BaseStream, quota);
                  mXElement = XElement.Load(reader);
               }
            }

            // Always load into tags
            using (StreamReader sr = new StreamReader(FileName, inputEncoding))
            {
               XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(sr.BaseStream, quota);
               List<string> currentPath = new List<string>();
               while (reader.Read())
               {
                  if (reader.NodeType == XmlNodeType.Element)
                  {
                     string name = reader.Name;
                     if (name.Equals("a:item"))
                     {
                        writeLog("encountered a:item");
                     }
                     else if (!name.Equals("root"))
                     {
                        currentPath.Add(name);
                     }
                  }
                  else if (reader.NodeType == XmlNodeType.EndElement)
                  {
                     currentPath.RemoveAt(currentPath.Count - 1);
                  }
                  else if (reader.NodeType == XmlNodeType.Text)
                  {
                     TagString newItem = new TagString();
                     string path = String.Join(".", currentPath.ToArray());
                     newItem.tagName = path;
                     newItem.tagValue = reader.Value;
                     if (box == "org")
                     {
                        mAllData.Add(newItem);                               //Fill list with new item.
                     }

                     else if (isTranslation)
                     {
                        int tagIndex = -1;                                                 //This will sort translation according to original.
                        tagIndex = mAllData.FindIndex(x => x.tagName.Equals(path));
                        if (tagIndex >= 0)                              //This means that item exists
                        {
                           mAllData[tagIndex].tagTrans = reader.Value;   //Add value to list in correct place. [Used to be 4]
                        }
                        else                                            //Tag do not exist in original, add only in translation kolumn
                        {
                           newItem.tagValue = "";                      //Tag did not exist, value has to be nothing!
                           newItem.tagTrans = reader.Value;             //Item 3 is value. [Used to be 4]
                           mAllData.Add(newItem);                       //Fill list with new item.
                        }
                     }
                  }
               }

               /*
            writeLog("Reading data from " + FileName);
                 string line;

                 while ((line = sr.ReadLine()) != null)              //Read until end of file.
                 {
                     string[] strSplit = splitRow(line);
                     TagString NewItem = new TagString();

                     if (strSplit.Length == 2)                       //This follows the JSON format. [Used to be 6]
                     {
                         NewItem.tagName = strSplit[0];              //Item 0 is tag name.
                         NewItem.tagValue = strSplit[1];             //Item 1 is value. [Used to be 4]

                         if (box == "org")   
                         {
                             //If row = valid, then search for duplicate tag
                             //But only when opening original file

                             if (searchForDuplicate(NewItem.tagName))    //True means duplicate found, consider printing the row numbers...
                             {
                                 writeLog("Duplicate tag found: " + NewItem.tagName);
                                 duplicationFound = true;
                             }    
                         }
                     }
                     else                                            //Row is perhaps blank? 
                         NewItem.tagName = NewItem.tagValue = NewItem.tagTrans = "";

                     if (box == "org")
                     {
                         mAllData.Add(NewItem);                               //Fill list with new item.
                     }

                     else if (box == "trans")
                     {
                         int tagIndex = -1;
                         if (strSplit.Length == 2)                           //Search list to find tag index. [Used to be 6]
                         {                                                   //This will sort translation according to original.
                             tagIndex = mAllData.FindIndex(x => x.tagName == strSplit[0]);
                             if (tagIndex >= 0)                              //This means that item exists
                             {
                                 mAllData[tagIndex].tagTrans = strSplit[1];   //Add value to list in correct place. [Used to be 4]
                             }

                             else                                            //Tag do not exist in original, add only in translation kolumn
                             {
                                 NewItem.tagValue = "";                      //Tag did not exist, value has to be nothing!
                                 NewItem.tagTrans = strSplit[1];             //Item 3 is value. [Used to be 4]
                                 mAllData.Add(NewItem);                       //Fill list with new item.
                                 writeLog("Tag not found, added tag: " + NewItem.tagName + " with value:" + NewItem.tagTrans);
                             }
                         }
                     }
                 }

                 writeLog("Data is put into memory.");
                                                         //If there are several empty rows in the beginning
                 while (string.IsNullOrEmpty(mAllData[0].tagName))            //they should be removed
                 {
                     mAllData.RemoveAt(0);                                    //Loop and remove as long as row 0 is empty
                 }*/

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
         if (dlg.ShowDialog() == DialogResult.OK)                                //If filename and everything is ok to be saved...
         {
            saveAs(dlg.FileName);                                               //Run the save file function
            DialogResult dialogResult = MessageBox.Show("File saved, do you want to open it?\n\nThis will reload the grid with data\nfrom both original and the new file.", "Translate Helper Question", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)                               //Ask user if the saved file should be opened
            {
               openFile(tb_org.Text, "org");                                   //Open original file again
               writeLog("Opened Original file again.");
               openFile(dlg.FileName, "trans");                                //Open translation file again (using the new file)
               writeLog("Opened the new translation file.");
            }
            haveSaved = true;                                                   //File saved, value = true
         }
      }

      //The function to save all data to a new file.
      private void saveAs(string newFilename)
      {
         //            using (StreamWriter wr = new StreamWriter(newFilename, false, System.Text.Encoding.Default))
         using (StreamWriter wr = new StreamWriter(newFilename, false, outputEncoding)) //To support more characters?
         {
            for (int i = 0; i < mAllData.Count; i++)
            {
               string path = mAllData[i].tagName;
               if (path != "")
               {
                  string xPath = "//" + path.Replace('.', '/');
                  XElement attribute = mXElement.XPathSelectElement(xPath);
                  string raw = mAllData[i].tagTrans;
                  string content = isHtml ? TagString.myHTMLconverter(raw) : raw;
                  attribute.Value = content;
                  writeLog(content);
               }
            }
            mXElement.Save(newFilename);
            /*
                  //XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(wr.BaseStream);
                  wr.WriteLine("{");                                                  //Start tag
            int currentTag = 0;                                                 //Keep track on which tag we're working on
            nrOfTags = countTags();                                             //Count tags in mAllData
            writeLog("Starting export, nr of tags: " + nrOfTags.ToString());    //Print number of tags to export in log

            for (int i = 0; i < mAllData.Count; i++)
            {
               string path = mAllData[i].tagName;
               if (path != "")
               {
                  string xPath = "//" + path.Replace('.', '/');
                  XAttribute attribute = mXElement.Attribute(xPath);

                  currentTag++;
                  //fix this temporary solution with encoding....
                  //string printStr;

                  if (currentTag != nrOfTags)
                  {
                     if (isHtml)
                        wr.WriteLine("    " + mAllData[i].PrintJsonRowTransHTML());  //Print row with HTML formatting
                     else
                        wr.WriteLine("    " + mAllData[i].PrintJsonRowTrans());      //Print row according to JSON format    
                  }
                  else
                      if (isHtml)
                     wr.WriteLine("    " + mAllData[i].PrintLastJsonRowTransHTML());  //Print row with HTML formatting
                  else
                     wr.WriteLine("    " + mAllData[i].PrintLastJsonRowTrans());  //Print last row according to JSON format (without the comma)   

                  if (currentTag == nrOfTags)                             //If all tags are saved, jump out of the foor loop
                     break;                                              //If there are several empty rows at the end they will be removed
               }
               else
                  wr.WriteLine("");                                       //Print blank row
            }
            wr.WriteLine("}");                                              //End tag
            */
         }
         writeLog("Saved to file: " + newFilename);
      }

      //Run this to adjust column after new files are loaded. Most could be set from design view.
      private void adjustColumns()
      {
         dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
         dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
         dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

         dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
         dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

         dataGridView1.Columns[0].HeaderText = "Tag name";
         dataGridView1.Columns[1].HeaderText = "Original text";
         dataGridView1.Columns[2].HeaderText = "Translated text";
      }

      //This adds a new row to the bottom of the workspace, and highlights it.
      private void btn_addRow_Click(object sender, EventArgs e)
      {
         dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;   //Disable the changed value event while deleting file

         DialogResult dialogResult = MessageBox.Show("Do you want to insert a row at selected position? \nNo means to add at the bottom.", "Translate Helper Question", MessageBoxButtons.YesNoCancel);
         if (dialogResult == DialogResult.Yes)
         {
            mAllData.Insert(dataGridView1.CurrentCell.RowIndex, new TagString("", "", ""));
            FillAllData();
            writeLog("A new (empty) row was inserted at row " + dataGridView1.CurrentCell.RowIndex.ToString() + ".");
         }
         else if (dialogResult == DialogResult.No)
         {
            mAllData.Add(new TagString("", "", ""));                         //Else, insert on last row
            FillAllData();
            dataGridView1.CurrentCell = dataGridView1.Rows[mAllData.Count - 1].Cells[1];     //highlight the added cell
            writeLog("A new (empty) row was added at the bottom.");
         }

         dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;   //Re-enable the event again
      }

      //This will remove a complete tag item with values.
      private void btn_delRow_Click(object sender, EventArgs e)
      {
         dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;   //Disable the changed value event while deleting file
         int rowToDelete = dataGridView1.CurrentCell.RowIndex;
         int activeColumn = dataGridView1.CurrentCell.ColumnIndex;
         DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this row?", "Translate Helper Warning", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.Yes)
         {
            mAllData.RemoveAt(rowToDelete);
            FillAllData();
            if (mAllData.Count == rowToDelete)       //Perform a check to highlight correct cell after delete.
               dataGridView1.CurrentCell = dataGridView1.Rows[rowToDelete - 1].Cells[activeColumn];     //highlight the new active cell    
            else
               dataGridView1.CurrentCell = dataGridView1.Rows[rowToDelete].Cells[activeColumn];     //highlight the new active cell

            writeLog("Deleted row " + rowToDelete + ".");
         }
         dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;   //Re-enable the event again
      }

      //Do we want to change name on tags?
      private void checkBox1_CheckedChanged(object sender, EventArgs e)
      {
         checkReadWrite();
      }


      private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
      {

      }

      //Used to warn the user of unsaved work
      private void TranslateHelper_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (haveSaved == false)         //You have not saved, do you still want to exit?
         {
            if (MessageBox.Show("You have not saved! Do you still want to exit program?",
                "Translate Helper Warning", MessageBoxButtons.YesNo) == DialogResult.No)
               e.Cancel = true;        //If answer is No - Abort the exit!
                                       //Consider adding YesNoCancel as SaveExitCancel...
         }
      }

      //Search through the tags for a duplicate (true means a duplicate found)
      private bool searchForDuplicate(string tagName)
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
      private string[] splitRow(string line)
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

      //Count the number of tags in the mAllData list
      private int countTags()
      {
         return mAllData.Where(x => x.tagName != "").ToList().Count();    //Return number of lines that are not ""
      }

      //Will try to locate the next "non-white" item. That means, missing or same translation
      private void btn_next_item_Click(object sender, EventArgs e)
      {
         for (int i = current_selected + 1; i <= mAllData.Count - 1; i++)                      //From current position to end
         {
            if (mAllData[i].tagTrans == "" || mAllData[i].tagValue == "" || mAllData[i].tagValue == mAllData[i].tagTrans)    //No translation or same
            {
               if (mAllData[i].tagName != "")                                               //Skip blank lines
               {
                  dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[2];             //Highlight cell
                  current_selected = i;                                                   //Remember where we are now (for next find or search)
                  break;
               }
            }
         }
      }

      //Same as previous function, but locate previous missing translation instead
      private void btn_prev_item_Click(object sender, EventArgs e)
      {
         for (int i = current_selected - 1; i >= 0; i--)                      //From current position to beginning
         {
            if (mAllData[i].tagTrans == "" || mAllData[i].tagValue == "" || mAllData[i].tagValue == mAllData[i].tagTrans)    //No translation or same
            {
               if (mAllData[i].tagName != "")                                               //Skip blank lines
               {
                  dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[2];             //Highlight cell
                  current_selected = i;                                                   //Remember where we are now (for next find or search)
                  break;
               }
            }
         }
      }

      //Support function to "find-next" to keep track of where we are in the dataset
      private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
      {
         current_selected = dataGridView1.CurrentRow.Index;
      }

      //Keeps track of what column to search in
      private void rb_tag_CheckedChanged(object sender, EventArgs e)
      {
         search_column = 0;
      }

      //Keeps track of what column to search in
      private void rb_org_CheckedChanged(object sender, EventArgs e)
      {
         search_column = 1;
      }

      //Keeps track of what column to search in
      private void rb_trans_CheckedChanged(object sender, EventArgs e)
      {
         search_column = 2;
      }

      //Search for next occurance of what is written in the search textbox
      private void btn_search_next_Click(object sender, EventArgs e)
      {
         if (tb_search.Text != "")                   //Only start search if anything is written
         {
            //Used to have "i <= mAllData.Count - 1", but changed it together with the i-checks to be able to search forward from last row...
            for (int i = current_selected + 1; i <= mAllData.Count; i++)  //Run to end of dataset length or when "run" is set to false (match found)
            {
               if (i <= mAllData.Count - 1 && search_result(i, search_column))   //If result is found, end loop (search)
                  break;
               if (i == mAllData.Count - 1) //Reached end with nothing found
               {
                  DialogResult dialogResult = MessageBox.Show("Reached end of data, do you want to continue from top?", "Translate Helper Question", MessageBoxButtons.YesNo);
                  if (dialogResult == DialogResult.Yes)
                  {
                     current_selected = -1;               //Start position where to begin next search
                     btn_search_next_Click(sender, e);    //Call function from within function, ok..?
                  }
                  else if (dialogResult == DialogResult.No)
                  {
                     current_selected = dataGridView1.CurrentRow.Index;   //If clicked yes before, this must be changed back to where we are
                  }
               }
            }
         }
      }

      //Help function to search next/prev to highlight match and stop loop
      private bool search_result(int i, int column)
      {
         //Tag should not be empty I guess? And compare search text to value in current cell
         if (mAllData[i].tagName != "" && dataGridView1.Rows[i].Cells[column].Value.ToString().ToLower().Contains(tb_search.Text.ToLower()))
         {
            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[column]; //Highlight what was found
            current_selected = i;   //Remember what row we have selected
            return true;            //Match found, return true
         }

         return false;               //Match not found, continue search
      }

      //Search for previous occurance of what is written in the search textbox
      private void btn_search_prev_Click(object sender, EventArgs e)
      {
         if (tb_search.Text != "")                   //Only start search if anything is written
         {
            //Used to have "i >= 0", but changed it together with the i-checks to be able to search backwards from first row...
            for (int i = current_selected - 1; i >= -1; i--)     //Run to beginning of dataset or when "run" is set to false (match found)
            {
               if (i >= 0 && search_result(i, search_column))   //If result is found, end loop (search)
                  break;
               if (i < 0) //Reached start with nothing found
               {
                  DialogResult dialogResult = MessageBox.Show("Reached beginning of data, do you want to continue from end?", "Translate Helper Question", MessageBoxButtons.YesNo);
                  if (dialogResult == DialogResult.Yes)
                  {
                     current_selected = mAllData.Count;                   //Start position where to begin next search
                     btn_search_prev_Click(sender, e);                   //Call function from within function, ok..?
                  }
                  else if (dialogResult == DialogResult.No)
                  {
                     current_selected = dataGridView1.CurrentRow.Index;  //If clicked yes before, this must be changed back to where we are
                  }
               }
            }
         }
      }

      private void cb_input_encoding_SelectedIndexChanged(object sender, EventArgs e)
      {
         switch (cb_input_encoding.SelectedIndex)
         {
            case 0:
               inputEncoding = System.Text.Encoding.Default;
               break;
            case 1:
               inputEncoding = Encoding.UTF8;
               break;
            case 2:
               inputEncoding = new System.Text.UTF8Encoding(false);
               break;
         }
         writeLog("Input encoding set to: " + inputEncoding.EncodingName);

      }

      private void cb_output_encoding_SelectedIndexChanged(object sender, EventArgs e)
      {
         switch (cb_output_encoding.SelectedIndex)
         {
            case 0:
               outputEncoding = System.Text.Encoding.Default;
               break;
            case 1:
               outputEncoding = Encoding.UTF8;
               break;
            case 2:
               outputEncoding = new System.Text.UTF8Encoding(false);
               break;
         }
         writeLog("Output encoding set to: " + outputEncoding.EncodingName);
      }


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

   }
}
