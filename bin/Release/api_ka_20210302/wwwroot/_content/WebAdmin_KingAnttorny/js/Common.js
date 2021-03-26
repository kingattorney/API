/*-----------------------------------------------------------------
Description: This libary is for common control ASP.Net
Creadted by : GoldGreen
Created on  : 2018/08/11
-------------------------------------------------------------------*/

/**********Variable ***********/
var ccyPlaDec = new Object();
var intDBMaxRec = 2000;
var strErrorValidMaxRecords = ""
var strDecPla = "18,2"

/********** END variable ***********/

/********** START Public Function ***********/

//
// Show dialog message 
//
function DisplayMessage(strmsg, fnCallback) {
    $("<div title='Alert'>" + strmsg + "</div>").dialog({
        modal: true,
        width: 400,
        resizable: false,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
                if (fnCallback != undefined) {
                    fnCallback();
                }
            }
        }
    });
}

//
// Show dialog message with title
//
function DisplayMsgWithTitle(strmsg, title, fnCallback) {
    var msgTitle = (title == undefined || title == "") ? "Alert" : title;
    $("<div title='" + msgTitle + "'>" + strmsg + "</div>").dialog({
        modal: true,
        width: 400,
        resizable: false,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
                if (fnCallback != undefined) {
                    fnCallback();
                }
            }
        }
    });
}

//
// Show dialog Confirm OK/Cancel message 
//
function ConfirmMessage(strmsg, fnCallback) {
    $("<div title='Alert'>" + strmsg + "</div>").dialog({
        modal: true,
        width: 400,
        resizable: false,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
                if (fnCallback != undefined) {
                    fnCallback();
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
}

//
// Show dialog Confirm Yes/No Message
//
function YesNoMessage(strmsg, strTitle, fnYesCallback, fnNoCallback) {
    if (strTitle == undefined) {
        strTitle = "Alert";
    }
    $("<div title='" + strTitle + "'>" + strmsg + "</div>").dialog({
        modal: true,
        width: 400,
        resizable: false,
        buttons: {
            Yes: function () {
                $(this).dialog("close");
                if (fnYesCallback != undefined) {
                    fnYesCallback();
                }
            },
            No: function () {
                $(this).dialog("close");
                if (fnNoCallback != undefined) {
                    fnNoCallback();
                }
            }
        }
    });
}

function ShowErrorMessage(controlID, strMessage) {
    var html = "Error occured, please contact System Admin. " +
        "<a onClick='alert(\"" + strMessage + "\")'" +
        " style='cursor:pointer;color:blue'>" + "click here</a> to get more info for System Admin."
    $(controlID).html(html)
}
//
// format amount with decimal places
//
function FormatAmount(strAmount, strDecimal) {
    var formarCurrent = /[^0-9\.]+/g;
    strAmount = (strAmount + "").replace(formarCurrent, "");

    if (!$.isNumeric(strAmount)) {
        strAmount = 0;
    }
    else {
        strAmount = parseFloat(strAmount);
    }
    if (strDecimal !== undefined && $.isNumeric(strDecimal)) {
        strDecimal = parseInt(strDecimal);
    }
    else {
        strDecimal = 0;
    }

    strAmount = strAmount.toLocaleString(undefined, { minimumFractionDigits: strDecimal, maximumFractionDigits: strDecimal });

    return strAmount;
}

function initButton() {
    //Need to apply this class for a input type button if need to be used as <button>
    $(".clsbutton").button();

    //Need to apply this class for every button which searches for content. 
    $(".clssearch").button({
        icon: "ui-icon-search", showLabel: false
    });
    $('.clsfind').button({
        icon: "ui-icon-search", showLabel: true
    });
    $('.clsclear').button({
        icon: "ui-icon-refresh", showLabel: true
    });

    $(".clsedit").button({
        icon: "ui-icon-pencil", showLabel: false
    });

    $(".clsdelete").button({
        icon: "ui-icon-trash", showLabel: false
    });

    $('.clscheck').button({
        icon: "ui-icon-check", showLabel: false
    });

    $('.clsapprove').button({
        icon: "ui-icon-person", showLabel: false
    });
}

//
// init table with page using GridView
//
function initGridViewDatatable(idGridview, isColumnFilter) {

    if (isColumnFilter == true && $(idGridview + " tr").length > 1) {
        var headFilter = "<thead><tr>"
        if ($(idGridview + " tr:first th").length > 0) {
            $(idGridview + " tr:first th").each(function (q, a) {
                headFilter += "<td></td>"
            });
        }
        else {
            $(idGridview + " tr:first td").each(function (q, a) {
                headFilter += "<td></td>"
            });
        }

        headFilter += "</tr></thead>";

        $(idGridview).prepend($(headFilter)
            .append($(idGridview)
                .find("tr:first")));
    }
    else {
        $(idGridview).prepend($("<thead></thead>")
            .append($(idGridview)
                .find("tr:first")));
    }

    if ($(idGridview + " thead tr:first td").length == 1) {
        $(idGridview + " thead tr:first td").each(function (q, a) {
            $(this).replaceWith('<th>' + $(this).text() + '</th>');
        });
    }

    $(idGridview + " tbody td").each(function () {
        this.innerHTML = $.trim(this.innerHTML.replace(/&nbsp;/g, ' '))
    })


}

//
// format amount with currency code
// 
function FormatAmountByCCY(strAmount, strCurrency) {
    var strDecimal = ccyPlaDec[strCurrency];
    return FormatAmount(strAmount, strDecimal)
}

//
// function disable/enable buttons with ui-button and asp.net
//
function DisableButton(idControl, isDisable) {
    if (isDisable == true) {
        $(idControl).attr('disabled', true);
        $(idControl).addClass('aspNetDisabled').addClass('ui-button-disabled').addClass('ui-state-disabled');
    }
    else {
        $(idControl).removeAttr('disabled');
        $(idControl).removeClass('aspNetDisabled').removeClass('ui-button-disabled').removeClass('ui-state-disabled');
    }
    $(idControl).attr('ui-disabled', isDisable);

}


//
// function to initialize a jQuery object to a dialog
//
jQuery.fn.InitDialog = function (size, title) {
    return this.each(function () {
        var dialogSize = getSize(size);

        var $container = $(this);
        // Prevent reseting iframe when called from code behind
        if ($container.find("iframe").length == 0) {
            $container.empty();
            var $frame = $('<iframe>', {
                frameborder: 0
                
            });
            $container.append($frame);
        }
        $container.dialog({
            autoOpen: false,
            resizable: true,
            height: dialogSize.height,
            width: dialogSize.width,
            modal: true,
            buttons: {
                Close: function () {
                    $(this).dialog("close");
                }
            }
        });

        if (title != undefined) {
            $container.dialog('option', 'title', title);
        }
    });
}

//
// Common function to show dialog if the respective div is in the same page
//
jQuery.fn.ShowDialogOfSamePage = function (size) {
    return this.each(function () {
        $(this).find('.ui-invalid').remove();
        var dialogSize = getSize(size);
        var $container = $(this);
        $container.dialog({
            autoOpen: false,
            resizable: true,
            height: dialogSize.height,
            width: dialogSize.width,
            modal: true,
            buttons: {
                Close: function () {
                    $(this).dialog("close");
                }
            }
        }).parent().appendTo($("form:first"));;
        $(this).dialog("open");
    });
}



// Common function to show dialog.
// In normal circumstance, one page should have one frame only.
// Parameters:
//      src: URL of child screen to be displayed in frame
jQuery.fn.ShowDialog = function (src, size, title) {

    return this.each(function () {
        if (title == undefined) {
            if ($(this).find("iframe").length == 0 || size != undefined) {
                $(this).InitDialog(size);
            }
        }
        else {
            if ($(this).find("iframe").length == 0 || size != undefined) {
                $(this).InitDialog(size, title);
            }
        }
        var $frame = $(this).find("iframe");
        $frame.attr("src", src);
        $(this).dialog("open");
    });
}


function ShowProgress() {
    setTimeout(function () {
        var modal = "<div><img src='../Images/loading.gif'/></div>"; //  $('<div />');
        modal.addClass("modal");
        $('body').append(modal);
        var loading = $(".loading");
        loading.show();
        var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
        var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
        loading.css({
            top: top, left: left
        });
    }, 200);
}

/********** START Private Function ***********/
//
// Get size dialog
//
function getSize(size) {
    var width, height;
    if (size == undefined) {
        width = $(window).width() * 0.8
        height = $(window).height() * 0.8
    }
    else if (size == 's') {
        width = $(window).width() * 0.4
        height = $(window).height() * 0.4
    }
    else if (size == 'm') {
        width = $(window).width() * 0.6
        height = $(window).height() * 0.8
    }
    else if (size == 'mw') {
        width = $(window).width() * 0.8
        height = $(window).height() * 0.6
    }
    else if (size == 'l') {
        width = $(window).width() * 0.9
        height = $(window).height() * 0.9
    }
    else if (size == 'f') { // full
        width = $(window).width() * 0.98
        height = $(window).height() * 0.9
    }
    else if (size.width != undefined) {
        width = size.width
        height = size.height
    }
    var retVal = {
        width: width, height: height
    };
    retVal.width = width;
    retVal.height = height;
    return retVal;
}

//
// clone function
//
function __fnCloneFunc(src) {
    function mixin(dest, source, copyFunc) {
        var name, s, i, empty = {};
        for (name in source) {
            // the (!(name in empty) || empty[name] !== s) condition avoids copying properties in "source"
            // inherited from Object.prototype.	 For example, if dest has a custom toString() method,
            // don't overwrite it with the toString() method that source inherited from Object.prototype
            s = source[name];
            if (!(name in dest) || (dest[name] !== s && (!(name in empty) || empty[name] !== s))) {
                dest[name] = copyFunc ? copyFunc(s) : s;
            }
        }
        return dest;
    }

    if (!src || typeof src != "object" || Object.prototype.toString.call(src) === "[object Function]") {
        // null, undefined, any non-object, or function
        return src;	// anything
    }
    if (src.nodeType && "cloneNode" in src) {
        // DOM Node
        return src.cloneNode(true); // Node
    }
    if (src instanceof Date) {
        // Date
        return new Date(src.getTime());	// Date
    }
    if (src instanceof RegExp) {
        // RegExp
        return new RegExp(src);   // RegExp
    }
    var r, i, l;
    if (src instanceof Array) {
        // array
        r = [];
        for (i = 0, l = src.length; i < l; ++i) {
            if (i in src) {
                r.push(__fnCloneFunc(src[i]));
            }
        }
    } else {
        r = src.constructor ? new src.constructor() : {};
    }
    return mixin(r, src, clone);

}
