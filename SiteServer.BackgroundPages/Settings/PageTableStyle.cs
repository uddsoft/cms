using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Model;
using BaiRong.Core.Table;
using SiteServer.BackgroundPages.Cms;

namespace SiteServer.BackgroundPages.Settings
{
    public class PageTableStyle : BasePage
    {
        public DataGrid DgContents;

        public Button BtnAddStyle;
        public Button BtnAddStyles;
        public Button BtnImport;
        public Button BtnExport;

        private string _tableName;
        private string _redirectUrl;

        public static string GetRedirectUrl(string tableName)
        {
            return PageUtils.GetSettingsUrl(nameof(PageTableStyle), new NameValueCollection
            {
                {"tableName", tableName}
            });
        }

        public string GetReturnUrl()
        {
            return PageAuxiliaryTable.GetRedirectUrl();
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            _tableName = Body.GetQueryString("tableName");
            _redirectUrl = GetRedirectUrl(_tableName);

            if (IsPostBack) return;

            VerifyAdministratorPermissions(AppManager.Permissions.Settings.SiteManagement);

            //删除样式
            if (Body.IsQueryExists("DeleteStyle"))
            {
                var attributeName = Body.GetQueryString("AttributeName");
                if (TableStyleManager.IsExists(0, _tableName, attributeName))
                {
                    try
                    {
                        TableStyleManager.Delete(0, _tableName, attributeName);
                        Body.AddAdminLog("删除数据表单样式", $"表单:{_tableName},字段:{attributeName}");
                        SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        FailDeleteMessage(ex);
                    }
                }
            }

            var styleInfoList = TableStyleManager.GetTableStyleInfoList(_tableName, new List<int> {0});

            DgContents.DataSource = styleInfoList;
            DgContents.ItemDataBound += DgContents_ItemDataBound;
            DgContents.DataBind();

            BtnAddStyle.Attributes.Add("onclick", ModalTableStyleAdd.GetOpenWindowString(0, 0, new List<int> { 0 }, _tableName, string.Empty, _redirectUrl));
            BtnAddStyles.Attributes.Add("onclick",
                ModalTableStylesAdd.GetOpenWindowString(0, new List<int> {0}, _tableName, _redirectUrl));
            BtnImport.Attributes.Add("onclick", ModalTableStyleImport.GetOpenWindowString(_tableName, 0, 0));
            BtnExport.Attributes.Add("onclick", ModalExportMessage.GetOpenWindowStringToSingleTableStyle(_tableName));
        }

        public void Redirect(object sender, EventArgs e)
        {
            PageUtils.Redirect(GetRedirectUrl(_tableName));
        }

        private void DgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var styleInfo = (TableStyleInfo)e.Item.DataItem;

            var ltlAttributeName = (Literal)e.Item.FindControl("ltlAttributeName");
            var ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
            var ltlInputType = (Literal)e.Item.FindControl("ltlInputType");
            var ltlFieldType = (Literal)e.Item.FindControl("ltlFieldType");
            var ltlValidate = (Literal)e.Item.FindControl("ltlValidate");
            var ltlTaxis = (Literal)e.Item.FindControl("ltlTaxis");
            var ltlEditStyle = (Literal)e.Item.FindControl("ltlEditStyle");
            var ltlEditValidate = (Literal)e.Item.FindControl("ltlEditValidate");

            var showPopWinString = ModalTableMetadataView.GetOpenWindowString(_tableName, styleInfo.AttributeName);
            ltlAttributeName.Text = $"<a href=\"javascript:void 0;\" onClick=\"{showPopWinString}\">{styleInfo.AttributeName}</a>";

            ltlDisplayName.Text = styleInfo.DisplayName;
            ltlInputType.Text = InputTypeUtils.GetText(InputTypeUtils.GetEnumType(styleInfo.InputType));
            ltlFieldType.Text = TableMetadataManager.IsAttributeNameExists(_tableName, styleInfo.AttributeName) ? $"真实 {TableMetadataManager.GetTableMetadataDataType(_tableName, styleInfo.AttributeName)}" : "虚拟字段";

            ltlValidate.Text = ValidateTypeUtils.GetValidateInfo(styleInfo);

            showPopWinString = ModalTableStyleAdd.GetOpenWindowString(0, styleInfo.TableStyleId, new List<int>{0}, _tableName, styleInfo.AttributeName, _redirectUrl);
            var editText = styleInfo.TableStyleId != 0 ? "修改" : "添加";
            ltlEditStyle.Text = $@"<a href=""javascript:;"" onclick=""{showPopWinString}"">{editText}</a>";

            showPopWinString = ModalTableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleId, new List<int> { 0 }, _tableName, styleInfo.AttributeName, _redirectUrl);
            ltlEditValidate.Text = $@"<a href=""javascript:;"" onclick=""{showPopWinString}"">设置</a>";

            ltlTaxis.Text = styleInfo.Taxis.ToString();

            if (styleInfo.TableStyleId == 0) return;

            var urlStyle = PageUtils.GetSettingsUrl(nameof(PageTableStyle), new NameValueCollection
            {
                {"tableName", _tableName},
                {"DeleteStyle", true.ToString()},
                {"AttributeName", styleInfo.AttributeName}
            });
            ltlEditStyle.Text +=
                $@"&nbsp;&nbsp;<a href=""{urlStyle}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>";
        }
    }
}
