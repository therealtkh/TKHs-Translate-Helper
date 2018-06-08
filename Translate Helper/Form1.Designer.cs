namespace Translate_Helper
{
    partial class TranslateHelper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslateHelper));
            this.chkbx_countBoth = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cb_countBoth = new System.Windows.Forms.CheckBox();
            this.btn_reset_tag_all = new System.Windows.Forms.Button();
            this.btn_reset_tag = new System.Windows.Forms.Button();
            this.btn_search_prev = new System.Windows.Forms.Button();
            this.rb_trans = new System.Windows.Forms.RadioButton();
            this.rb_org = new System.Windows.Forms.RadioButton();
            this.rb_tag = new System.Windows.Forms.RadioButton();
            this.tb_search = new System.Windows.Forms.TextBox();
            this.btn_search_next = new System.Windows.Forms.Button();
            this.lbl_translations_left = new System.Windows.Forms.Label();
            this.btn_prev_item = new System.Windows.Forms.Button();
            this.btn_next_item = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_openTrans = new System.Windows.Forms.Button();
            this.btn_openOrg = new System.Windows.Forms.Button();
            this.tb_trans = new System.Windows.Forms.TextBox();
            this.tb_org = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tb_log = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cb_showRowHeaders = new System.Windows.Forms.CheckBox();
            this.cb_export_row_numbers = new System.Windows.Forms.CheckBox();
            this.btn_export = new System.Windows.Forms.Button();
            this.chkbx_html = new System.Windows.Forms.CheckBox();
            this.lbl_encoding_write = new System.Windows.Forms.Label();
            this.lbl_encoding_read = new System.Windows.Forms.Label();
            this.cb_output_encoding = new System.Windows.Forms.ComboBox();
            this.cb_input_encoding = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkbx_countBoth.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkbx_countBoth
            // 
            this.chkbx_countBoth.Controls.Add(this.tabPage1);
            this.chkbx_countBoth.Controls.Add(this.tabPage2);
            this.chkbx_countBoth.Controls.Add(this.tabPage3);
            this.chkbx_countBoth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkbx_countBoth.Location = new System.Drawing.Point(0, 0);
            this.chkbx_countBoth.Name = "chkbx_countBoth";
            this.chkbx_countBoth.SelectedIndex = 0;
            this.chkbx_countBoth.Size = new System.Drawing.Size(783, 446);
            this.chkbx_countBoth.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cb_countBoth);
            this.tabPage1.Controls.Add(this.btn_reset_tag_all);
            this.tabPage1.Controls.Add(this.btn_reset_tag);
            this.tabPage1.Controls.Add(this.btn_search_prev);
            this.tabPage1.Controls.Add(this.rb_trans);
            this.tabPage1.Controls.Add(this.rb_org);
            this.tabPage1.Controls.Add(this.rb_tag);
            this.tabPage1.Controls.Add(this.tb_search);
            this.tabPage1.Controls.Add(this.btn_search_next);
            this.tabPage1.Controls.Add(this.lbl_translations_left);
            this.tabPage1.Controls.Add(this.btn_prev_item);
            this.tabPage1.Controls.Add(this.btn_next_item);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.btn_save);
            this.tabPage1.Controls.Add(this.btn_openTrans);
            this.tabPage1.Controls.Add(this.btn_openOrg);
            this.tabPage1.Controls.Add(this.tb_trans);
            this.tabPage1.Controls.Add(this.tb_org);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(775, 420);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Translate";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // cb_countBoth
            // 
            this.cb_countBoth.AutoSize = true;
            this.cb_countBoth.Enabled = false;
            this.cb_countBoth.Location = new System.Drawing.Point(600, 39);
            this.cb_countBoth.Name = "cb_countBoth";
            this.cb_countBoth.Size = new System.Drawing.Size(167, 17);
            this.cb_countBoth.TabIndex = 21;
            this.cb_countBoth.Text = "Count both yellow and orange";
            this.cb_countBoth.UseVisualStyleBackColor = true;
            this.cb_countBoth.CheckedChanged += new System.EventHandler(this.cb_countBoth_CheckedChanged);
            // 
            // btn_reset_tag_all
            // 
            this.btn_reset_tag_all.Location = new System.Drawing.Point(507, 64);
            this.btn_reset_tag_all.Name = "btn_reset_tag_all";
            this.btn_reset_tag_all.Size = new System.Drawing.Size(26, 23);
            this.btn_reset_tag_all.TabIndex = 20;
            this.btn_reset_tag_all.Text = "All";
            this.btn_reset_tag_all.UseVisualStyleBackColor = true;
            this.btn_reset_tag_all.Click += new System.EventHandler(this.btn_reset_tag_all_Click);
            // 
            // btn_reset_tag
            // 
            this.btn_reset_tag.Location = new System.Drawing.Point(458, 64);
            this.btn_reset_tag.Name = "btn_reset_tag";
            this.btn_reset_tag.Size = new System.Drawing.Size(43, 23);
            this.btn_reset_tag.TabIndex = 19;
            this.btn_reset_tag.Text = "Reset";
            this.btn_reset_tag.UseVisualStyleBackColor = true;
            this.btn_reset_tag.Click += new System.EventHandler(this.btn_reset_tag_Click);
            // 
            // btn_search_prev
            // 
            this.btn_search_prev.Enabled = false;
            this.btn_search_prev.Location = new System.Drawing.Point(164, 64);
            this.btn_search_prev.Name = "btn_search_prev";
            this.btn_search_prev.Size = new System.Drawing.Size(75, 23);
            this.btn_search_prev.TabIndex = 17;
            this.btn_search_prev.Text = "Find Prev";
            this.btn_search_prev.UseVisualStyleBackColor = true;
            this.btn_search_prev.Click += new System.EventHandler(this.btn_search_prev_Click);
            // 
            // rb_trans
            // 
            this.rb_trans.AutoSize = true;
            this.rb_trans.Location = new System.Drawing.Point(106, 66);
            this.rb_trans.Name = "rb_trans";
            this.rb_trans.Size = new System.Drawing.Size(52, 17);
            this.rb_trans.TabIndex = 16;
            this.rb_trans.TabStop = true;
            this.rb_trans.Text = "Trans";
            this.rb_trans.UseVisualStyleBackColor = true;
            this.rb_trans.CheckedChanged += new System.EventHandler(this.rb_trans_CheckedChanged);
            // 
            // rb_org
            // 
            this.rb_org.AutoSize = true;
            this.rb_org.Checked = true;
            this.rb_org.Location = new System.Drawing.Point(58, 66);
            this.rb_org.Name = "rb_org";
            this.rb_org.Size = new System.Drawing.Size(42, 17);
            this.rb_org.TabIndex = 15;
            this.rb_org.TabStop = true;
            this.rb_org.Text = "Org";
            this.rb_org.UseVisualStyleBackColor = true;
            this.rb_org.CheckedChanged += new System.EventHandler(this.rb_org_CheckedChanged);
            // 
            // rb_tag
            // 
            this.rb_tag.AutoSize = true;
            this.rb_tag.Location = new System.Drawing.Point(8, 66);
            this.rb_tag.Name = "rb_tag";
            this.rb_tag.Size = new System.Drawing.Size(44, 17);
            this.rb_tag.TabIndex = 14;
            this.rb_tag.TabStop = true;
            this.rb_tag.Text = "Tag";
            this.rb_tag.UseVisualStyleBackColor = true;
            this.rb_tag.CheckedChanged += new System.EventHandler(this.rb_tag_CheckedChanged);
            // 
            // tb_search
            // 
            this.tb_search.Location = new System.Drawing.Point(245, 66);
            this.tb_search.Name = "tb_search";
            this.tb_search.ReadOnly = true;
            this.tb_search.Size = new System.Drawing.Size(126, 20);
            this.tb_search.TabIndex = 13;
            // 
            // btn_search_next
            // 
            this.btn_search_next.Enabled = false;
            this.btn_search_next.Location = new System.Drawing.Point(377, 64);
            this.btn_search_next.Name = "btn_search_next";
            this.btn_search_next.Size = new System.Drawing.Size(75, 23);
            this.btn_search_next.TabIndex = 12;
            this.btn_search_next.Text = "Find Next";
            this.btn_search_next.UseVisualStyleBackColor = true;
            this.btn_search_next.Click += new System.EventHandler(this.btn_search_next_Click);
            // 
            // lbl_translations_left
            // 
            this.lbl_translations_left.AutoSize = true;
            this.lbl_translations_left.Location = new System.Drawing.Point(539, 42);
            this.lbl_translations_left.Name = "lbl_translations_left";
            this.lbl_translations_left.Size = new System.Drawing.Size(0, 13);
            this.lbl_translations_left.TabIndex = 11;
            // 
            // btn_prev_item
            // 
            this.btn_prev_item.Enabled = false;
            this.btn_prev_item.Location = new System.Drawing.Point(539, 64);
            this.btn_prev_item.Name = "btn_prev_item";
            this.btn_prev_item.Size = new System.Drawing.Size(75, 23);
            this.btn_prev_item.TabIndex = 10;
            this.btn_prev_item.Text = "Prev Item";
            this.btn_prev_item.UseVisualStyleBackColor = true;
            this.btn_prev_item.Click += new System.EventHandler(this.btn_prev_item_Click);
            // 
            // btn_next_item
            // 
            this.btn_next_item.Enabled = false;
            this.btn_next_item.Location = new System.Drawing.Point(620, 64);
            this.btn_next_item.Name = "btn_next_item";
            this.btn_next_item.Size = new System.Drawing.Size(75, 23);
            this.btn_next_item.TabIndex = 9;
            this.btn_next_item.Text = "Next Item";
            this.btn_next_item.UseVisualStyleBackColor = true;
            this.btn_next_item.Click += new System.EventHandler(this.btn_next_item_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 93);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(769, 324);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            // 
            // btn_save
            // 
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(458, 35);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 5;
            this.btn_save.Text = "Save As...";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_openTrans
            // 
            this.btn_openTrans.Enabled = false;
            this.btn_openTrans.Location = new System.Drawing.Point(377, 35);
            this.btn_openTrans.Name = "btn_openTrans";
            this.btn_openTrans.Size = new System.Drawing.Size(75, 23);
            this.btn_openTrans.TabIndex = 3;
            this.btn_openTrans.Text = "Browse";
            this.btn_openTrans.UseVisualStyleBackColor = true;
            this.btn_openTrans.Click += new System.EventHandler(this.btn_openTrans_Click);
            // 
            // btn_openOrg
            // 
            this.btn_openOrg.Location = new System.Drawing.Point(377, 6);
            this.btn_openOrg.Name = "btn_openOrg";
            this.btn_openOrg.Size = new System.Drawing.Size(75, 23);
            this.btn_openOrg.TabIndex = 2;
            this.btn_openOrg.Text = "Browse";
            this.btn_openOrg.UseVisualStyleBackColor = true;
            this.btn_openOrg.Click += new System.EventHandler(this.btn_openOrg_Click);
            // 
            // tb_trans
            // 
            this.tb_trans.Location = new System.Drawing.Point(8, 35);
            this.tb_trans.Name = "tb_trans";
            this.tb_trans.ReadOnly = true;
            this.tb_trans.Size = new System.Drawing.Size(363, 20);
            this.tb_trans.TabIndex = 1;
            this.tb_trans.Text = "Translation file. This is probably a previous version of it.";
            // 
            // tb_org
            // 
            this.tb_org.Location = new System.Drawing.Point(8, 6);
            this.tb_org.Name = "tb_org";
            this.tb_org.ReadOnly = true;
            this.tb_org.Size = new System.Drawing.Size(363, 20);
            this.tb_org.TabIndex = 0;
            this.tb_org.Text = "Input file. This is probably the English (original) file.";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tb_log);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(775, 420);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tb_log
            // 
            this.tb_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_log.Location = new System.Drawing.Point(3, 3);
            this.tb_log.Multiline = true;
            this.tb_log.Name = "tb_log";
            this.tb_log.ReadOnly = true;
            this.tb_log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_log.Size = new System.Drawing.Size(769, 414);
            this.tb_log.TabIndex = 0;
            this.tb_log.Text = "Welcome to the Translate Helper, current version is alpha 0.7.1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cb_showRowHeaders);
            this.tabPage3.Controls.Add(this.cb_export_row_numbers);
            this.tabPage3.Controls.Add(this.btn_export);
            this.tabPage3.Controls.Add(this.chkbx_html);
            this.tabPage3.Controls.Add(this.lbl_encoding_write);
            this.tabPage3.Controls.Add(this.lbl_encoding_read);
            this.tabPage3.Controls.Add(this.cb_output_encoding);
            this.tabPage3.Controls.Add(this.cb_input_encoding);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(775, 420);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Options";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cb_showRowHeaders
            // 
            this.cb_showRowHeaders.AutoSize = true;
            this.cb_showRowHeaders.Enabled = false;
            this.cb_showRowHeaders.Location = new System.Drawing.Point(11, 112);
            this.cb_showRowHeaders.Name = "cb_showRowHeaders";
            this.cb_showRowHeaders.Size = new System.Drawing.Size(123, 17);
            this.cb_showRowHeaders.TabIndex = 23;
            this.cb_showRowHeaders.Text = "Show Row Numbers";
            this.cb_showRowHeaders.UseVisualStyleBackColor = true;
            this.cb_showRowHeaders.CheckedChanged += new System.EventHandler(this.cb_showRowHeaders_CheckedChanged);
            // 
            // cb_export_row_numbers
            // 
            this.cb_export_row_numbers.AutoSize = true;
            this.cb_export_row_numbers.Enabled = false;
            this.cb_export_row_numbers.Location = new System.Drawing.Point(92, 87);
            this.cb_export_row_numbers.Name = "cb_export_row_numbers";
            this.cb_export_row_numbers.Size = new System.Drawing.Size(108, 17);
            this.cb_export_row_numbers.TabIndex = 22;
            this.cb_export_row_numbers.Text = "Add row numbers";
            this.cb_export_row_numbers.UseVisualStyleBackColor = true;
            // 
            // btn_export
            // 
            this.btn_export.Enabled = false;
            this.btn_export.Location = new System.Drawing.Point(11, 83);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(75, 23);
            this.btn_export.TabIndex = 21;
            this.btn_export.Text = "Export to .txt";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // chkbx_html
            // 
            this.chkbx_html.AutoSize = true;
            this.chkbx_html.Location = new System.Drawing.Point(11, 60);
            this.chkbx_html.Name = "chkbx_html";
            this.chkbx_html.Size = new System.Drawing.Size(95, 17);
            this.chkbx_html.TabIndex = 19;
            this.chkbx_html.Text = "Save in HTML";
            this.chkbx_html.UseVisualStyleBackColor = true;
            // 
            // lbl_encoding_write
            // 
            this.lbl_encoding_write.AutoSize = true;
            this.lbl_encoding_write.Location = new System.Drawing.Point(8, 36);
            this.lbl_encoding_write.Name = "lbl_encoding_write";
            this.lbl_encoding_write.Size = new System.Drawing.Size(87, 13);
            this.lbl_encoding_write.TabIndex = 3;
            this.lbl_encoding_write.Text = "Output Encoding";
            // 
            // lbl_encoding_read
            // 
            this.lbl_encoding_read.AutoSize = true;
            this.lbl_encoding_read.Location = new System.Drawing.Point(8, 9);
            this.lbl_encoding_read.Name = "lbl_encoding_read";
            this.lbl_encoding_read.Size = new System.Drawing.Size(79, 13);
            this.lbl_encoding_read.TabIndex = 2;
            this.lbl_encoding_read.Text = "Input Encoding";
            // 
            // cb_output_encoding
            // 
            this.cb_output_encoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_output_encoding.FormattingEnabled = true;
            this.cb_output_encoding.Items.AddRange(new object[] {
            "Default",
            "UTF-8",
            "UTF-8 (w/o BOM)",
            "Unicode"});
            this.cb_output_encoding.Location = new System.Drawing.Point(101, 33);
            this.cb_output_encoding.Name = "cb_output_encoding";
            this.cb_output_encoding.Size = new System.Drawing.Size(121, 21);
            this.cb_output_encoding.TabIndex = 1;
            this.cb_output_encoding.SelectedIndexChanged += new System.EventHandler(this.cb_output_encoding_SelectedIndexChanged);
            // 
            // cb_input_encoding
            // 
            this.cb_input_encoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_input_encoding.FormattingEnabled = true;
            this.cb_input_encoding.Items.AddRange(new object[] {
            "Default",
            "UTF-8",
            "UTF-8 (w/o BOM)",
            "Unicode"});
            this.cb_input_encoding.Location = new System.Drawing.Point(101, 6);
            this.cb_input_encoding.Name = "cb_input_encoding";
            this.cb_input_encoding.Size = new System.Drawing.Size(121, 21);
            this.cb_input_encoding.TabIndex = 0;
            this.cb_input_encoding.SelectedIndexChanged += new System.EventHandler(this.cb_input_encoding_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "\"JSON Files (*.json)|*.json|All Files (*.*)|*.*\"";
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // TranslateHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 446);
            this.Controls.Add(this.chkbx_countBoth);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(735, 484);
            this.Name = "TranslateHelper";
            this.Text = "TKH\'s JSON Translate Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TranslateHelper_FormClosing);
            this.Load += new System.EventHandler(this.TranslateHelper_Load);
            this.chkbx_countBoth.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl chkbx_countBoth;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tb_log;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.DataGridView dataGridView1;
        //private JonasDatagrid dataGridView1;
        private System.Windows.Forms.Button btn_openTrans;
        private System.Windows.Forms.Button btn_openOrg;
        private System.Windows.Forms.TextBox tb_trans;
        private System.Windows.Forms.TextBox tb_org;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btn_next_item;
        private System.Windows.Forms.Button btn_prev_item;
        private System.Windows.Forms.Label lbl_translations_left;
        private System.Windows.Forms.Button btn_search_prev;
        private System.Windows.Forms.RadioButton rb_trans;
        private System.Windows.Forms.RadioButton rb_org;
        private System.Windows.Forms.RadioButton rb_tag;
        private System.Windows.Forms.TextBox tb_search;
        private System.Windows.Forms.Button btn_search_next;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lbl_encoding_write;
        private System.Windows.Forms.Label lbl_encoding_read;
        private System.Windows.Forms.ComboBox cb_output_encoding;
        private System.Windows.Forms.ComboBox cb_input_encoding;
        private System.Windows.Forms.Button btn_reset_tag;
        private System.Windows.Forms.Button btn_reset_tag_all;
        private System.Windows.Forms.CheckBox cb_countBoth;
        private System.Windows.Forms.CheckBox chkbx_html;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.CheckBox cb_export_row_numbers;
        private System.Windows.Forms.CheckBox cb_showRowHeaders;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

