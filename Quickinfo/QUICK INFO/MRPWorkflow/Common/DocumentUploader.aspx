<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentUploader.aspx.cs" Inherits="MRPWorkflow_Common_DocumentUploader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
        <script type="text/javascript" src="../../JQuery/jquery-1.8.2.min.js"></script>


    <script src="../../JS/drop_zone/dropzone-amd-module.js"></script>
    <script src="../../JS/drop_zone/dropzone.js"></script>
    

    <link href="../../Styles/drop_zone/dropzone.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/drop_zone/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">




        Dropzone.options.myDropzone = {

            url: "DocumentUploader.aspx?tid=<%=Request.QueryString["tid"]%>",
            // Prevents Dropzone from uploading dropped files immediately
            autoProcessQueue: false,

            init: function () {
                var submitButton = document.querySelector("#submit-all")
                myDropzone = this; // closure

                submitButton.addEventListener("click", function () {

                    myDropzone.processQueue(); // Tell Dropzone to process all queued files.
                });

                // You might want to show the submit button only when 
                // files are dropped here:
                this.on("addedfile", function () {
                    // Show submit button here and/or inform user to click it.
                });
                this.on("processing", function () {
                    this.options.autoProcessQueue = true;
                });


            }

        };
    </script>
</head>
<body>
     <div>
        <form action="/target" class="dropzone" id="my-dropzone" style="height: 550px;">
        </form>

        <button id="submit-all" class="btn btn-apps">
            Upload Documents</button>



    </div>
</body>
</html>
