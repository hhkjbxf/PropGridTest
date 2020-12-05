
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;

namespace HHTech.CSMMSS.Framework.Util
{
    public partial class PropGrid : UserControl
    {
        #region Var define

        const string groupName = "GroupName";
        const string fieldName = "FieldName";
        const string fieldValue = "FieldValue";
        const string fieldType = "FieldType";
        const string fieldTag = "FieldTag";
        const string fieldDesp = "FieldDesp";

        DataTable groupTable = new DataTable("GroupTable");
        DataTable dataTable = new DataTable("PropDataTable");
        DataTable subOptTable = new DataTable("SubOptTable");
        DataSet dataSet = new DataSet();

        bool isDisplayInGroup = false;
        public bool IsDisplayInGroup
        {
            get { return this.isDisplayInGroup; }
            set { this.isDisplayInGroup = value; }
        }

        const string DefGroupName = "杂项";

        int GroupBandIdx { get { return IsDisplayInGroup ? 0 : -1; } }
        int PropBandIdx { get { return IsDisplayInGroup ? 1 : 0; } }
        int SubOptBandIdx{ get { return IsDisplayInGroup ? 2 : 1; } }

        #endregion

        #region PropGrid, OnLoad

        public PropGrid()
        {
            InitializeComponent();

            DataTable dt = this.groupTable;

            //属性组
            dt.Columns.Add(groupName, typeof(string));
            dt.Columns.Add("SpaceCol", typeof(string));
            dt.Columns[0].ReadOnly = true;
            dt.Columns[1].ReadOnly = true;

            // 属性表
            dt = this.dataTable;
            dt.Columns.Add(fieldName, typeof(string));
            dt.Columns.Add(fieldValue, typeof(string));
            dt.Columns.Add(fieldType, typeof(string));
            dt.Columns.Add(fieldTag, typeof(object));
            dt.Columns.Add(fieldDesp, typeof(string));
            dt.Columns.Add(groupName, typeof(string));
            dt.Columns[0].ReadOnly = true;

            // 属性为多选项时，提供多选Checkbox列表
            dt = this.subOptTable;
            dt.Columns.Add("PropName", typeof(string));
            dt.Columns.Add("PropOptName", typeof(string));
            dt.Columns.Add("PropOptVal", typeof(string));
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.DesignMode)
            {
                this.SetValue("Text", "This is a test string", "Text");
                this.SetValue("Color", Color.Red, "Color");
                this.SetValue("Font", "宋体", "Font");
                this.SetValue("ServerIP", "172.168.9.9", "IP");
                this.SetValue("ServerPort", 8888, "Int");
                this.SetValue("IsRight", true, "YesNoCheckBox");
                this.SetValue("Percent", 80, "Progress");

                List<string> slist = new List<string>();
                slist.Add("男");
                slist.Add("女");
                this.SetValue("person", "女", "CustomEnum", slist, "");
            }

            if (IsDisplayInGroup)
            {
                this.dataSet.Tables.Add(this.groupTable);
                this.dataSet.Tables.Add(this.dataTable);
                this.dataSet.Tables.Add(this.subOptTable);
                this.dataSet.Relations.Add(new DataRelation(
                    "groupRelation", this.groupTable.Columns[groupName], this.dataTable.Columns[groupName]));
                this.dataSet.Relations.Add(new DataRelation(
                    "propOptRelation", this.dataTable.Columns[fieldName], this.subOptTable.Columns["PropName"]));

                this.ulGrid.DataSource = this.dataSet;
                this.ulGrid.DisplayLayout.Bands[0].ColHeadersVisible = false;
                this.ulGrid.DisplayLayout.Bands[1].ColHeadersVisible = false;
                this.ulGrid.DisplayLayout.Bands[2].ColHeadersVisible = false;
            }
            else
            {
                this.dataSet.Tables.Add(this.dataTable);
                this.dataSet.Tables.Add(this.subOptTable);
                this.dataSet.Relations.Add(new DataRelation(
                    "propOptRelation", this.dataTable.Columns[fieldName], this.subOptTable.Columns["PropName"]));

                this.ulGrid.DataSource = this.dataSet;
                this.ulGrid.DisplayLayout.Bands[0].ColHeadersVisible = false;
                this.ulGrid.DisplayLayout.Bands[1].ColHeadersVisible = false;
            }
        }

        #endregion

        #region ulGrid_InitializeLayout, ulGrid_InitializeRow

        /// <summary>
        /// 表格的初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.GroupByBox.Hidden = true;
            e.Layout.CaptionVisible = DefaultableBoolean.True;

            UltraGridBand band;
            if (GroupBandIdx >= 0)
            {
                band = e.Layout.Bands[GroupBandIdx];
                band.Columns[groupName].Width = 150;
                band.Columns[groupName].CellClickAction = CellClickAction.RowSelect;

                band.Columns["SpaceCol"].Width = 200;
                band.Columns["SpaceCol"].CellClickAction = CellClickAction.RowSelect;
                band.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.CheckOnDisplay;
            }

            band = e.Layout.Bands[PropBandIdx];
            band.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.CheckOnDisplay;

            band.Columns[fieldName].CellClickAction = CellClickAction.RowSelect;
            band.Columns[fieldName].Width = 150;
            band.Columns[fieldName].Header.Caption = "名称";

            band.Columns[fieldValue].Width = 200;
            band.Columns[fieldValue].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            band.Columns[fieldValue].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
            band.Columns[fieldValue].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            band.Columns[fieldValue].Header.Caption = "数值";

            band.Columns[fieldType].Hidden = true;
            band.Columns[fieldTag].Hidden = true;
            band.Columns[fieldDesp].Hidden = true;
            band.Columns[groupName].Hidden = true;

            band = e.Layout.Bands[SubOptBandIdx];
            band.Columns["PropName"].Hidden = true;
            band.Columns["PropOptName"].CellClickAction = CellClickAction.RowSelect;
        }

        public void SetGridCaption(string colName, string colVal)
        {
            this.ulGrid.DisplayLayout.Bands[PropBandIdx].Columns[fieldName].Header.Caption = colName;
            this.ulGrid.DisplayLayout.Bands[PropBandIdx].Columns[fieldValue].Header.Caption = colVal;
        }

        /// <summary>
        /// 加入新行时的初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulGrid_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Band.Index == GroupBandIdx)
            {
                e.Row.ExpandAll();
                e.Row.Appearance.BackColor = Color.DarkGray;
            }
            else if (e.Row.Band.Index == PropBandIdx)
            {
                e.Row.ExpandAll();
                string strType = e.Row.Cells[fieldType].Text;
                e.Row.Cells[fieldValue].Editor = this.GetEmbeddableEditor(
                    strType,
                    e.Row.Cells[fieldTag].Value);

                if (strType == "Enum" || strType == "CustomEnum" || strType == "Color" || strType == "Font" || strType == "YesNoDropDown")
                    e.Row.Cells[1].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            }
            else if (e.Row.Band.Index == SubOptBandIdx)
            {
                e.Row.Cells["PropOptVal"].Editor = this.GetEmbeddableEditor("YesNoCheckBox", null);
                e.Row.Cells["PropOptVal"].Editor.ValueChanged += new EventHandler(Editor_ValueChanged);
                e.Row.Cells["PropOptVal"].Editor.Tag = e.Row;
            }
        }

        #endregion

        #region Clear

        public void Clear()
        {
            this.subOptTable.Rows.Clear();
            this.dataTable.Rows.Clear();
            this.lbDesp.Text = "";
        }

        #endregion

        #region SetValue

        public bool SetValue(string propName, object propValue, string propType)
        {
            return this.SetValue(DefGroupName, propName, propValue, propType, null, "");
        }

        public bool SetValue(string grpName, string propName, object propValue, string propType)
        {
            return this.SetValue(grpName, propName, propValue, propType, null, "");
        }

        public bool SetValue(string propName, object propValue, string type, object tag, string desp)
        {
            return this.SetValue(DefGroupName, propName, propValue, type, tag, desp);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        public bool SetValue(string groupName, string propName, object propValue, string type, object tag, string desp)
        {
            foreach (DataRow r in this.dataTable.Rows)
            {
                if ((string)r[fieldName] == propName)
                {
                    if ((string)r[fieldType] == type)
                    {
                        r[fieldValue] = propValue;
                        r[fieldDesp] = desp;
                        if (type == "Option")
                            SetPropOptList(propName, propValue as string, tag as List<string>);
                        return  true;
                    }
                    else
                        return false;
                }
            }

            //添加数据组行
            if(this.groupTable.Select(string.Format("GroupName = '{0}'", groupName)).Length == 0)
            {
                this.groupTable.Rows.Add(new object[]{groupName, ""});
            }

            this.dataTable.Rows.Add(new object[] { propName, propValue, type, tag, desp, groupName });
            if (type == "Option")
                SetPropOptList(propName, propValue as string, tag as List<string>);

            return true;
        }

        static bool Exist(string[] strs, string str)
        {
            if (strs == null)
                return false;
            foreach (string s in strs)
            {
                if (s == str)
                    return true;
            }
            return false;
        }

        void SetPropOptList(string propName, string propVal, List<string> optList)
        {
            if (optList == null || string.IsNullOrEmpty(propName))
            {
                return;
            }

            for (int i = 0; i < this.subOptTable.Rows.Count;)
            {
                if ((string)this.subOptTable.Rows[i]["PropName"] == propName)
                {
                    this.subOptTable.Rows.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            string[] vals = (propVal as string).Split(new char[] { ',', '，' });
            foreach (string opt in optList)
            {
                this.subOptTable.Rows.Add(new object[] { propName, opt, Exist(vals, opt) });
            }
        }

        #endregion

        #region GetValue, GetProGridList

        /// <summary>
        /// 获取相应属性名称的属性值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string propName)
        {
            foreach (DataRow r in this.dataTable.Rows)
            {
                if ((string)r[fieldName] == propName)
                {
                    if ((string)r[fieldType] == "Option")
                    {
                        return this.GetOptString(propName);
                    }
                    else
                    {
                        if (r[fieldValue] == DBNull.Value)
                        {
                            return "";
                        }
                        else
                        {
                            return (string)r[fieldValue];
                        }
                    }
                }
            }
            return "";
        }

        string GetOptString(string propName)
        {
            string ret = "";
            foreach (DataRow r in this.subOptTable.Rows)
            {
                if ((string)r["PropName"] == propName)
                {
                    if ((string)r["PropOptVal"] == "True")
                    {
                        if (ret != "")
                            ret += ",";
                        ret += r["PropOptName"];
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetProGridList()
        {
            List<string> uGridList = new List<string>();
            for (int i = 0; i < this.ulGrid.Rows.Count; i++)
            {
                uGridList.Add(ulGrid.Rows[i].Cells[fieldName].Value.ToString());
            }

            return uGridList;
        }

        public object GetPropTag(string propName)
        {
            foreach (DataRow r in this.dataTable.Rows)
            {
                if ((string)r[fieldName] == propName)
                {
                    if (r[fieldTag] == DBNull.Value)
                    {
                        return null;
                    }
                    else
                    {
                        return r[fieldTag];
                    }
                }
            }
            return null;

        }

        #endregion

        #region SetPropReadOnly

        public void SetPropReadOnly(string propName, bool isReadOnly)
        {
            foreach (UltraGridRow r in this.ulGrid.DisplayLayout.Bands[PropBandIdx].Layout.Rows)
            {
                if (r.Cells[0].Text == propName)
                {
                    r.Cells[1].Activation = isReadOnly ? Activation.NoEdit : Activation.AllowEdit;
                    break;
                }
            }
        }

        #endregion

        public void UpdateData()
        {
            this.ulGrid.UpdateData();
        }

        /// <summary>
        /// 是否显示表格标题
        /// </summary>
        public bool CaptionVisible
        {
            get 
            {
                return this.ulGrid.DisplayLayout.CaptionVisible == 
                    DefaultableBoolean.True; 
            }

            set
            {
                this.ulGrid.DisplayLayout.CaptionVisible = 
                    (value ? DefaultableBoolean.True : DefaultableBoolean.False);
            }
        }

        /// <summary>
        /// 表格标题
        /// </summary>
        public string Caption
        {
            get { return this.ulGrid.Text; }
            set { this.ulGrid.Text = value; }
        }

        /// <summary>
        /// 设置某行是否显示
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="hidden"></param>
        public void SetRowHidden(string propName, bool hidden)
        {
            foreach (UltraGridRow r in this.ulGrid.DisplayLayout.Bands[PropBandIdx].Layout.Rows)
            {
                if (r.Cells[0].Text == propName)
                {
                    r.Hidden = hidden;
                    break;
                }
            }
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="cindex"></param>
        /// <param name="width"></param>
        public void SetColumnWidth(int cindex,int width)
        {
            if (cindex < this.ulGrid.DisplayLayout.Bands[PropBandIdx].Columns.Count)
            {
                this.ulGrid.DisplayLayout.Bands[PropBandIdx].Columns[cindex].Width = width;
            }
        }

        #region Editor_ValueChanged, ulGrid_AfterSelectChange

        void Editor_ValueChanged(object sender, EventArgs e)
        {
            EmbeddableEditorBase editor = sender as EmbeddableEditorBase;
            UltraGridRow r = editor.Tag as UltraGridRow;
            string propName = r.ParentRow.Cells[fieldName].Value as string;
            string optName = r.Cells["PropOptName"].Value as string;

            // 更新选择表内容
            foreach (DataRow row in this.subOptTable.Rows)
            {
                if (((string)row["PropName"] == propName) && 
                    ((string)row["PropOptName"] == optName))
                {
                    row["PropOptVal"] = editor.Value;
                    break;
                }
            }

            foreach (DataRow r2 in this.dataTable.Rows)
            {
                if ((string)r2[fieldName] == propName)
                {
                    r2[fieldValue] = this.GetOptString(propName);
                    break;
                }
            }

            r.Selected = true;
            r.Activate();
        }

        private void ulGrid_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            if (this.ulGrid.Selected.Rows.Count > 0)
            {
                if (this.ulGrid.Selected.Rows[0].Band.Index == PropBandIdx)
                {
                    this.lbDesp.Text = this.ulGrid.Selected.Rows[0].Cells[fieldDesp].Text;
                }
            }
        }

        #endregion

        #region GetEmbeddableEditor

        /// <summary>
        /// 获得表格单元所使用的内嵌编辑器
        /// </summary>
        /// <param name="editorType"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        EmbeddableEditorBase GetEmbeddableEditor(string editorType, object tag)
        {
            EmbeddableEditorBase editor = null;
            DefaultEditorOwnerSettings editorSettings = new DefaultEditorOwnerSettings();

            switch (editorType)
            {
                default:
                case "String":
                case "Text":
                    editorSettings.DataType = typeof(string);
                    return new EditorWithText(new DefaultEditorOwner(editorSettings));

                case "Color":
                    editorSettings.DataType = typeof(Color);
                    return new ColorPickerEditor(new DefaultEditorOwner(editorSettings));

                case "Font":
                    ValueList valueList = new ValueList();
                    editorSettings.DataType = typeof(string);
                    for (int i = 0; i < System.Drawing.FontFamily.Families.Length; i++)
                        valueList.ValueListItems.Add(System.Drawing.FontFamily.Families[i].Name);
                    editorSettings.ValueList = valueList;
                    return new FontNameEditor(new DefaultEditorOwner(editorSettings));

                case "Int":
                    editorSettings.DataType = typeof(int);
                    editor = new EditorWithMask(new DefaultEditorOwner(editorSettings));
                    editorSettings.MaskInput = "-nnnnnnnn";
                    return editor;

                case "Progress":
                    editorSettings.DataType = typeof(int);
                    return new Infragistics.Win.UltraWinProgressBar.ProgressBarEditor(new DefaultEditorOwner(editorSettings));

                case "Date":
                    editorSettings.DataType = typeof(DateTime);
                    editorSettings.MaskInput = "yyyy-mm-dd";
                    return new DateTimeEditor(new DefaultEditorOwner(editorSettings));

                case "Time":
                    editorSettings.DataType = typeof(DateTime);
                    editorSettings.MaskInput = "hh:mm:ss";
                    return new EditorWithMask(new DefaultEditorOwner(editorSettings));

                case "DateTime":
                    editorSettings.DataType = typeof(DateTime);
                    editorSettings.MaskInput = "yyyy-mm-dd hh:mm:ss";
                    return new DateTimeEditor(new DefaultEditorOwner(editorSettings));

                case "Double":
                case "Float":
                    editorSettings.DataType = typeof(double);
                    editorSettings.MaskInput = "-nnnnnnnn.nnn";
                    return new EditorWithMask(new DefaultEditorOwner(editorSettings));

                case "IP":
                    editorSettings.DataType = typeof(string);
                    editor = new EditorWithMask(new DefaultEditorOwner(editorSettings));
                    editorSettings.MaskInput = "nnn\\.nnn\\.nnn\\.nnn";
                    return editor;

                case "YesNoDropDown":
                    editorSettings.DataType = typeof(bool);
                    valueList = new ValueList();
                    valueList.ValueListItems.Add(true, "是");
                    valueList.ValueListItems.Add(false, "否");
                    editorSettings.ValueList = valueList;
                    return new EditorWithCombo(new DefaultEditorOwner(editorSettings));

                case "YesNoCheckBox":
                    editorSettings.DataType = typeof(bool);
                    return new CheckEditor(new DefaultEditorOwner(editorSettings));

                case "Enum":
                case "CustomEnum":
                    List<string> slist = tag as List<string>;
                    ValueList vlist = new ValueList();
                    if (slist != null)
                    {
                        foreach (string str in slist)
                            vlist.ValueListItems.Add(str);
                    }
                    editorSettings.ValueList = vlist;
                    editorSettings.DataType = typeof(string);
                    editor = new EditorWithCombo(new DefaultEditorOwner(editorSettings));
                    return editor;
            }
        }

        #endregion

        private void PropGrid_Resize(object sender, EventArgs e)
        {
            if (this.Height < this.lbDesp.Height * 3)
                this.lbDesp.Visible = false;
            else
                this.lbDesp.Visible = true;
        }

    }
}


