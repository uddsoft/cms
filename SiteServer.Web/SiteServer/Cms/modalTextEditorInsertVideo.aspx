﻿<%@ Page Language="C#" Inherits="SiteServer.BackgroundPages.Cms.ModalTextEditorInsertVideo" %>
  <%@ Register TagPrefix="ctrl" Namespace="SiteServer.BackgroundPages.Controls" Assembly="SiteServer.BackgroundPages" %>
    <!DOCTYPE html>
    <html class="modalPage">

    <head>
      <meta charset="utf-8">
      <!--#include file="../inc/head.html"-->
    </head>

    <body>
      <!--#include file="../inc/openWindow.html"-->

      <form runat="server">
        <ctrl:alerts text="请选择插入视频的方式" runat="server" />
        <input type="hidden" id="fileNames" name="fileNames" value="" />
        <ctrl:code type="ajaxUpload" runat="server" />
        <script type="text/javascript" language="javascript">
          $(document).ready(function () {
            new AjaxUpload('upload_file', {
              action: '<%=UploadUrl%>',
              name: "filedata",
              data: {},
              onSubmit: function (file, ext) {
                var reg = /^(<%=TypeCollection%>)$/i;
                if (ext && reg.test(ext)) {
                  $('#img_upload_txt').text('上传中... ');
                } else {
                  $('#img_upload_txt').text('系统不允许上传指定的格式');
                  return false;
                }
              },
              onComplete: function (file, response) {
                $('#img_upload_txt').text('');
                if (response) {
                  response = eval("(" + response + ")");
                  if (response.success == 'true') {
                    $('#TbPlayUrl').val(response.playUrl);
                    $('#column1').hide();
                    $('#column2').show();
                    $('.nav-tabs li').removeClass('active');
                    $('#tab2').addClass('active');
                  } else {
                    $('#img_upload_txt').text(response.message);
                  }
                }
              }
            });
          });
        </script>

        <div class="raw">
          <ul class="nav nav-tabs tabs m-b-10">
            <li id="tab1" class="active tab">
              <a href="javascript:;" onclick="$('#column1').show();$('#column2').hide();$('.nav-tabs li').removeClass('active');$('#tab1').addClass('active');">上传视频</a>
            </li>
            <li id="tab2" class="tab">
              <a href="javascript:;" onclick="$('#column1').hide();$('#column2').show();$('.nav-tabs li').removeClass('active');$('#tab2').addClass('active');">输入地址</a>
            </li>
          </ul>
        </div>

        <div class="form-horizontal">

          <div class="form-group" id="column1">
            <label class="col-xs-3 text-right control-label">请选择视频文件</label>
            <div class="col-xs-8">
              <div id="fileSelect">
                <a id="upload_file" href="javascript:;" class="btn btn-success">上 传</a>
                <span id="img_upload_txt" style="clear:both; font-size:12px; color:#FF3737;"></span>
              </div>
            </div>
            <div class="col-xs-1"></div>
          </div>

          <div class="form-group" id="column2" style="display: none">
            <label class="col-xs-3 text-right control-label">请输入视频地址</label>
            <div class="col-xs-8">
              <asp:TextBox ID="TbPlayUrl" class="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-xs-1">
              <asp:RequiredFieldValidator ControlToValidate="TbPlayUrl" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server"
              />
            </div>
          </div>

          <div class="form-group">
            <label class="col-xs-3 text-right control-label">宽度</label>
            <div class="col-xs-8">
              <asp:TextBox ID="TbWidth" class="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-xs-1">
              <asp:RegularExpressionValidator ControlToValidate="TbWidth" ValidationExpression="\d+" Display="Dynamic" ErrorMessage=" *"
                foreColor="red" runat="server" />
            </div>
          </div>
          <div class="form-group">
            <label class="col-xs-3 text-right control-label">高度</label>
            <div class="col-xs-8">
              <asp:TextBox ID="TbHeight" class="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-xs-1">
              <asp:RegularExpressionValidator ControlToValidate="TbHeight" ValidationExpression="\d+" Display="Dynamic" ErrorMessage=" *"
                foreColor="red" runat="server" />
            </div>
          </div>
          <div class="form-group">
            <label class="col-xs-3 text-right control-label"></label>
            <div class="col-xs-8">
              <div class="checkbox checkbox-primary">
                <asp:CheckBox id="CbIsAutoPlay" Checked="true" runat="server" Text="自动播放" />
              </div>
            </div>
            <div class="col-xs-1"></div>
          </div>

          <hr />

          <div class="form-group m-b-0">
            <div class="col-xs-11 text-right">
              <asp:Button class="btn btn-primary m-l-10" text="确 定" runat="server" onClick="Submit_OnClick" />
              <button type="button" class="btn btn-default m-l-10" onclick="window.parent.layer.closeAll()">取 消</button>
            </div>
            <div class="col-xs-1"></div>
          </div>

        </div>

      </form>
    </body>

    </html>