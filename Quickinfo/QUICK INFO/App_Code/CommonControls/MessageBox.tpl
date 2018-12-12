<style>
    #dimmer {
    position:fixed; 
    top:0px; 
    right:0px; 
    bottom:0px; 
    left:0px; 
    background-color: rgb(33, 33, 33); 
    opacity: 0.5;
    }

    #message-box {
    font-family:"Helvetica Neue",Helvetica,Arial,sans-serif;
    position: fixed;
    top: 10%;
    left: 50%;
    z-index: 1050;
    width: 560px;
    margin-left: -280px;
    background-color: #fff;
    border: 1px solid rgba(0,0,0,0.3);
    border-radius: 4px;
    box-shadow: 0 3px 7px rgba(0,0,0,0.3);
    }

    #message-box .header { margin:0px; padding:8px; background-color:rgb(245, 245, 245);
    border-top-left-radius: 4px;
    border-top-right-radius: 4px;
    border-bottom:1px solid #DDD;
    }

    #message-box .header #close { float:right; font-weight:bold; color:#444; cursor:pointer; }
    #message-box .header .close:hover { color:#660000;  }
    #message-box .header h3 { color:#555; font-weight:normal; margin:0px;  }
    #message-box .body { padding:15px; color:#888; font-weight:normal;  }
    #message-box .body .icon img {  float:left;  width:100px; height:100px; }
    #message-box .body p { float:left; margin-left:10px;  }

    #message-box .footer {
    clear:both;
    margin:0px; padding:8px; background-color:rgb(245, 245, 245); color :#FFF; font-weight:normal;
    border-bottom-left-radius: 4px;
    border-bottom-right-radius: 4px;
    }

    #message-box .footer input {
    float:right;
    background-color: rgb(245, 245, 245);
    background-image: linear-gradient(rgb(255, 255, 255), rgb(230, 230, 230));
    background-repeat: repeat-x;
    border-bottom-color: rgb(179, 179, 179);
    border-bottom-left-radius: 4px;
    border-bottom-right-radius: 4px;
    border-bottom-style: solid;
    border-bottom-width: 1px;
    border-image-outset: 0 0 0 0;
    border-image-repeat: stretch stretch;
    border-image-slice: 100% 100% 100% 100%;
    border-image-source: none;
    border-image-width: 1 1 1 1;
    border-left-color: rgba(0, 0, 0, 0.1);
    border-left-style: solid;
    border-left-width: 1px;
    border-right-color: rgba(0, 0, 0, 0.1);
    border-right-style: solid;
    border-right-width: 1px;
    border-top-color: rgba(0, 0, 0, 0.1);
    border-top-left-radius: 4px;
    border-top-right-radius: 4px;
    border-top-style: solid;
    border-top-width: 1px;
    box-shadow: rgba(255, 255, 255, 0.2) 0px 1px 0px 0px inset, rgba(0, 0, 0, 0.05) 0px 1px 2px 0px;
    color: rgb(51, 51, 51);
    cursor: pointer;
    display: inline-block;
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
    font-size: 14px;
    line-height: 20px;
    margin-bottom: 0px;
    padding-bottom: 4px;
    padding-left: 12px;
    padding-right: 12px;
    padding-top: 4px;
    text-align: center;
    text-decoration: none;
    text-shadow: rgba(255, 255, 255, 0.75) 0px 1px 1px;
    vertical-align: middle;
    -moz-border-bottom-colors: none;
    -moz-border-left-colors: none;
    -moz-border-right-colors: none;
    -moz-border-top-colors: none;
    -moz-text-blink: none;
    -moz-text-decoration-color: rgb(51, 51, 51);
    -moz-text-decoration-line: none;
    -moz-text-decoration-style: solid;
    }

    #message-box .footer .btn-primary{
    background-color: rgb(0, 68, 204);
    background-image: linear-gradient(rgb(0, 136, 204), rgb(0, 68, 204));
    background-position: 0px -15px;
    background-repeat: repeat-x;
    border-bottom-color: rgba(0, 0, 0, 0.25);
    border-bottom-left-radius: 4px;
    border-bottom-right-radius: 4px;
    border-bottom-style: solid;
    border-bottom-width: 1px;
    border-image-outset: 0 0 0 0;
    border-image-repeat: stretch stretch;
    border-image-slice: 100% 100% 100% 100%;
    border-image-source: none;
    border-image-width: 1 1 1 1;
    border-left-color: rgba(0, 0, 0, 0.1);
    border-left-style: solid;
    border-left-width: 1px;
    border-right-color: rgba(0, 0, 0, 0.1);
    border-right-style: solid;
    border-right-width: 1px;
    border-top-color: rgba(0, 0, 0, 0.1);
    border-top-left-radius: 4px;
    border-top-right-radius: 4px;
    border-top-style: solid;
    border-top-width: 1px;
    box-shadow: rgba(255, 255, 255, 0.2) 0px 1px 0px 0px inset, rgba(0, 0, 0, 0.05) 0px 1px 2px 0px;
    color: rgb(255, 255, 255);
    cursor: pointer;
    display: inline-block;
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
    font-size: 14px;
    line-height: 20px;
    margin-bottom: 0px;
    margin-left: 5px;
    outline-color: rgb(255, 255, 255);
    outline-style: none;
    outline-width: 0px;
    padding-bottom: 4px;
    padding-left: 12px;
    padding-right: 12px;
    padding-top: 4px;
    text-align: center;
    text-decoration: none;
    text-shadow: rgba(0, 0, 0, 0.25) 0px -1px 0px;
    transition-delay: 0s;
    transition-duration: 0.1s;
    transition-property: background-position;
    transition-timing-function: cubic-bezier(0, 0, 1, 1);
    vertical-align: middle;
    -moz-border-bottom-colors: none;
    -moz-border-left-colors: none;
    -moz-border-right-colors: none;
    -moz-border-top-colors: none;
    -moz-text-blink: none;
    -moz-text-decoration-color: rgb(255, 255, 255);
    -moz-text-decoration-line: none;
    -moz-text-decoration-style: solid;
    }
</style>

<script type="text/javascript">

    function Cancel(){
    document.getElementById('dimmer').style.display = 'none';
    document.getElementById('message-box').style.display = 'none';
    }

    function Ok(){
        alert('Ok was clicked');
    }
</script>

<div id="message-box">
	<div class="header">
        <span id="close" onclick="Cancel()">&times;</span>
        <h3>{@TITLE}</h3>
    </div>
    <div class="body">
        <div class="icon">
            <img src="{@ICON}" />
        </div>
        <p>{@MESSAGE}</p>
    </div>
    <div class="footer">
        {@BUTTONS}    
        <div style="clear:both;"></div>
    </div>
</div>
<div id="dimmer"></div>