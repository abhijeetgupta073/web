/**
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

// validation function use every where 
function validate(group) {
    var val = Page_ClientValidate(group);
    if (!val) {
        var i = 0;
        for (; i < Page_Validators.length; i++) {
            if (!Page_Validators[i].isvalid) {
                $("#" + Page_Validators[i].controltovalidate).addClass('w3-pale-red w3-border-red  rdf-inupt-glow');
                var ctr = $("#" + Page_Validators[i].controltovalidate);
                if (ctr[0].classList[0] == 'rdfd_') {
                    $(ctr.selector + '_wrapper>table').addClass('w3-pale-red w3-border-red  rdf-inupt-glow');
                }
                $('#' + Page_Validators[i].id).addClass('rdf-text');
            }
        }
    }
    return val;
}
function checkvalue(txt) {
    if (txt.value != "") {
        var ctr = $("#" + txt.id);
        if (ctr[0].classList[0] == 'RadPicker') {
            //alert(ctr.selector + '_wrapper>table');
            $(ctr.selector + '>table').removeClass('w3-pale-red w3-border-red rdf-inupt-glow');
        }
        else {
            $('#' + txt.id).removeClass('w3-pale-red w3-border-red rdf-inupt-glow').addClass('w3-pale-green  ');
            $('#rdf' + txt.id).removeClass('rdf-text');
        }
    }
    else {
        $('#' + txt.id).addClass(' w3-pale-red w3-border-red rdf-inupt-glow').removeClass('w3-pale-green');
        var txtid = (txt.id).split('_');
        var id = $('#' + txtid[0] + '_rdf' + txtid[1]);
        $('#' + txtid[0] + '_rdf' + txtid[1]).addClass('rdf-text');
    }
}
function RowDblClick(sender, eventArgs) {
    var grid = $find(sender.ClientID);
    var masterTable = grid.get_masterTableView();
    var row = masterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
    var button = row.findElement("lnkEdit");
    button.click();
}
//For combobox value get at clientEvent
function getSelectedItem(sender, args) {

    var combo = $find(sender._element.id);
    var id = combo.get_id();
    var value = combo.get_value();
    var txtBox = id.replace('ddl', 'txt');
    $('#' + txtBox).val('$' + parseFloat(value).toFixed(2));
    TotalGroomingCal();

}

function disableAjax(sender, args) {

    if (args.get_eventTarget().indexOf("ExportTo") >= 0) {
        args.set_enableAjax(false);
    }
    else if (args.get_eventTarget().indexOf("btnViewLog") >= 0) {
        args.set_enableAjax(false);
    }
    else if (args.get_eventTarget().indexOf("btnViewSchedule") >= 0) {
        args.set_enableAjax(false);
    }
    else if (args.get_eventTarget().indexOf("btnContactView") >= 0) {
        args.set_enableAjax(false);


    }
    else if (args.get_eventTarget().indexOf("ViewIncidentReport") >= 0) {
        args.set_enableAjax(false);
    }
}


var URLSITE = window.location;
    $BODY = $('body'),
    $MENU_TOGGLE = $('#menu_toggle'),
    $SIDEBAR_MENU = $('#sidebar-menu'),
    $SIDEBAR_FOOTER = $('.sidebar-footer'),
    $LEFT_COL = $('.left_col'),
    $RIGHT_COL = $('.right_col'),
    $NAV_MENU = $('.nav_menu'),
    $FOOTER = $('footer');

// Sidebar
$(function () {

//    // TODO: This is some kind of easy fix, maybe we can improve this
    var setContentHeight = function () {
        // reset height
        $RIGHT_COL.css('min-height', $(window).height());

        var bodyHeight = $BODY.height(),
            leftColHeight = $LEFT_COL.eq(1).height() + $SIDEBAR_FOOTER.height(),
            contentHeight = bodyHeight < leftColHeight ? leftColHeight : bodyHeight;

        // normalize content
        contentHeight -= $NAV_MENU.height() + $FOOTER.height();

        $RIGHT_COL.css('min-height', contentHeight);
    };

    $SIDEBAR_MENU.find('a').on('click', function (ev) {
        var $li = $(this).parent();

        if ($li.is('.active')) {
            $li.removeClass('active');
            $('ul:first', $li).slideUp(function () {
                setContentHeight();
            });
        } else {
            // prevent closing menu if we are on child menu
            if (!$li.parent().is('.child_menu')) {
                $SIDEBAR_MENU.find('li').removeClass('active');
                $SIDEBAR_MENU.find('li ul').slideUp();
            }
            $('#setup').removeClass('active open');
            $('#fReporting').removeClass('active');
            $('#tReports').removeClass('active');
            $('#bReports').removeClass('active');
            $('#single').removeClass('active');
            $li.addClass('active');

            $('ul:first', $li).slideDown(function () {
                setContentHeight();
            });
        }
    });
    $('#res').on('click', function (ev) {
        var $li = $(this).parent();

        if ($li.is('.active')) {
            $li.removeClass('active');
            $('ul:first', $li).slideUp(function () {
                setContentHeight();
            });
        } else {
            // prevent closing menu if we are on child menu
            if (!$li.parent().is('.child_menu')) {
                $SIDEBAR_MENU.find('li').removeClass('active');
                $SIDEBAR_MENU.find('li ul').slideUp();
            }

            $li.addClass('active');

            $('ul:first', $li).slideDown(function () {
                setContentHeight();
            });
        }
    });
    // toggle small or large menu
    $MENU_TOGGLE.on('click', function () {
        if ($BODY.hasClass('nav-md')) {
            $('#CreateNewDropDown').css("margin-top", "2px");
            $BODY.removeClass('nav-md').addClass('nav-sm');
            $LEFT_COL.removeClass('scroll-view');
            //$('#BlackSection').css('display', 'block');
            $('#imgLogo').parent().css('display', 'none');
            $('#imgLogo1').css('display', 'block');
            $('#leftbarsection').css('margin-top', '0px');

            //$('.logo').removeClass('show').addClass('hide')
            //$('.logoIcon').removeClass('hide');
        } else {
            $('#CreateNewDropDown').css("margin-top", "6px");
            $BODY.removeClass('nav-sm').addClass('nav-md');
            //$('#BlackSection').css('display', 'none');
            var screensize = $(window).width();
            if (screensize < 650) {
                $('#imgLogo').parent().css('display', 'none');
                $('#imgLogo1').css('display', 'none');
                $('.imgdisplay').css('display', 'none');
            }
            else {
                $('#imgLogo').parent().css('display', 'block');
                $('#imgLogo1').css('display', 'none');
            }

            $('#leftbarsection').css('margin-top', '40px');
            //$('.logo').removeClass('hide').addClass('show');
            //$('.logoIcon').addClass('hide');
        }
        setContentHeight();
    });

    // check active menu
    $SIDEBAR_MENU.find('a[href="' + URLSITE + '"]').parent('li').addClass('current-page');

    $SIDEBAR_MENU.find('a').filter(function () {
        return this.href == URLSITE;
    }).parent('li').addClass('current-page').parents('ul').slideDown(function () {
        setContentHeight();
    }).parent().addClass('active');

    // recompute content when resizing
    $(window).smartresize(function () {
        setContentHeight();
    });

});

// Panel toolbox
$(function () {
    $('.collapse-link').on('click', function () {
        var $BOX_PANEL = $(this).closest('.x_panel'),
            $ICON = $(this).find('i'),
            $BOX_CONTENT = $BOX_PANEL.find('.x_content');

        // fix for some div with hardcoded fix class
        if ($BOX_PANEL.attr('style')) {
            $BOX_CONTENT.slideToggle(200, function () {
                $BOX_PANEL.removeAttr('style');
            });
        } else {
            $BOX_CONTENT.slideToggle(200);
            $BOX_PANEL.css('height', 'auto');
        }

        $ICON.toggleClass('fa-chevron-up fa-chevron-down');
    });

    $('.close-link').click(function () {
        var $BOX_PANEL = $(this).closest('.x_panel');

        $BOX_PANEL.remove();
    });
});

// Tooltip
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

// Progressbar
if ($(".progress .progress-bar")[0]) {
    $('.progress .progress-bar').progressbar(); // bootstrap 3
}

// Switchery
if ($(".js-switch")[0]) {
    var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
    elems.forEach(function (html) {
        var switchery = new Switchery(html, {
            color: '#26B99A'
        });
    });
}

// iCheck
if ($("input.flat")[0]) {
    $(document).ready(function () {
        $('input.flat').iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
        });
    });
}

//document.onkeydown = function (e) {
//    e = e || window.event;//Get event
//    if (e.ctrlKey) {
//        var c = e.which || e.keyCode;//Get key code
//        switch (c) {
//            case 83://Block Ctrl+S
//                // __doPostBack('ctl00$ContentPlaceHolder1$btnSave', '');

//                e.preventDefault();
//                e.stopPropagation();
//                break;
//        }
//    }
//};
// Starrr
var __slice = [].slice;

(function ($, window) {
    var Starrr;

    Starrr = (function () {
        Starrr.prototype.defaults = {
            rating: void 0,
            numStars: 5,
            change: function (e, value) {
            }
        };

        function Starrr($el, options) {
            var i, _, _ref,
                    _this = this;

            this.options = $.extend({}, this.defaults, options);
            this.$el = $el;
            _ref = this.defaults;
            for (i in _ref) {
                _ = _ref[i];
                if (this.$el.data(i) !== null) {
                    this.options[i] = this.$el.data(i);
                }
            }
            this.createStars();
            this.syncRating();
            this.$el.on('mouseover.starrr', 'span', function (e) {
                return _this.syncRating(_this.$el.find('span').index(e.currentTarget) + 1);
            });
            this.$el.on('mouseout.starrr', function () {
                return _this.syncRating();
            });
            this.$el.on('click.starrr', 'span', function (e) {
                return _this.setRating(_this.$el.find('span').index(e.currentTarget) + 1);
            });
            this.$el.on('starrr:change', this.options.change);
        }

        Starrr.prototype.createStars = function () {
            var _i, _ref, _results;

            _results = [];
            for (_i = 1, _ref = this.options.numStars; 1 <= _ref ? _i <= _ref : _i >= _ref; 1 <= _ref ? _i++ : _i--) {
                _results.push(this.$el.append("<span class='glyphicon .glyphicon-star-empty'></span>"));
            }
            return _results;
        };

        Starrr.prototype.setRating = function (rating) {
            if (this.options.rating === rating) {
                rating = void 0;
            }
            this.options.rating = rating;
            this.syncRating();
            return this.$el.trigger('starrr:change', rating);
        };

        Starrr.prototype.syncRating = function (rating) {
            var i, _i, _j, _ref;

            rating || (rating = this.options.rating);
            if (rating) {
                for (i = _i = 0, _ref = rating - 1; 0 <= _ref ? _i <= _ref : _i >= _ref; i = 0 <= _ref ? ++_i : --_i) {
                    this.$el.find('span').eq(i).removeClass('glyphicon-star-empty').addClass('glyphicon-star');
                }
            }
            if (rating && rating < 5) {
                for (i = _j = rating; rating <= 4 ? _j <= 4 : _j >= 4; i = rating <= 4 ? ++_j : --_j) {
                    this.$el.find('span').eq(i).removeClass('glyphicon-star').addClass('glyphicon-star-empty');
                }
            }
            if (!rating) {
                return this.$el.find('span').removeClass('glyphicon-star').addClass('glyphicon-star-empty');
            }
        };

        return Starrr;

    })();
    return $.fn.extend({
        starrr: function () {
            var args, option;

            option = arguments[0], args = 2 <= arguments.length ? __slice.call(arguments, 1) : [];
            return this.each(function () {
                var data;

                data = $(this).data('star-rating');
                if (!data) {
                    $(this).data('star-rating', (data = new Starrr($(this), option)));
                }
                if (typeof option === 'string') {
                    return data[option].apply(data, args);
                }
            });
        }
    });
})(window.jQuery, window);

$(function () {
    return $(".starrr").starrr();
});
// by jay decline client function
function declineMsg() {
    //if (chk.checked == true) {

    $('#ContentPlaceHolder1_art1').css("display", "block").addClass(" w3-section w3-card-12 fade in j-alertBox-Failed w3-pale-blue");
    $('#ContentPlaceHolder1_alertlabel').html('<Strong>Info ! </strong> Please Add Detailed comments as to why this change is being made..');
    //}
}
function showGridSavebtn() {
    $('#pnlSaveCancel').addClass('show').removeClass('hide');
}
$(document).ready(function () {
    // checkbox checked uncheckedby jay
    $('[data-toggle="tooltip"]').tooltip();
    $('.slideThree').on('click', function () {

        var chk = this.firstElementChild;
        //if (chk.checked == true)
        //    chk.checked = false;
        //else
        //    chk.checked = true;
        chk.click();

        if (chk.id == "ContentPlaceHolder1_chkvetBill")
            veterMsg(chk);

    });


    // get current URL path and assign 'active' class
    var pathname = window.location.pathname;
    var setContentHeight = function () {
        // reset height
        $RIGHT_COL.css('min-height', $(window).height());

        var bodyHeight = $BODY.height(),
            leftColHeight = $LEFT_COL.eq(1).height() + $SIDEBAR_FOOTER.height(),
            contentHeight = bodyHeight < leftColHeight ? leftColHeight : bodyHeight;

        // normalize content
        contentHeight -= $NAV_MENU.height() + $FOOTER.height();

        $RIGHT_COL.css('min-height', contentHeight);
    };
    var path = (pathname.substr(0, pathname.lastIndexOf('.')) || pathname).replace('/', '');
    if (path == 'Default')
        $('.side-menu > li > a[href="/Default.aspx"]').parent().addClass('active');
    else if (path == 'Clients' || path == '../EditClient')
        $('.side-menu > li > a[href="/Clients.aspx"]').parent().addClass('active');
    else if (path == 'PetDetails' || path == 'findPet' || path == 'Boarding')
        $('.side-menu > li > a[href="/findPet.aspx"]').parent().addClass('active');
    else if (path == 'ClientsManagement')
        $('.side-menu > li > a[href="/ClientsManagement.aspx"]').parent().addClass('active');
    else if (path == 'EditInvoice')
        $('.side-menu > li > a[class="quick"]').parent().addClass('active');
    else if (path == 'HR/StaffList' || path == 'Master/Employees' || path == 'Master/Pricing' || path == 'Breed' || path == 'Master/AgreementLiability' || path == 'DayLightSaving' || path == 'HRemployee' || path == 'EditHRemployee') {
        $('#setup').addClass('active');
        $('ul:first', $('#setup')).slideDown(function () {
            setContentHeight();
        });
    }
    else if (path == 'Reports/PaymentPlan' || path == 'Reports/DayTrainPayment' || path == 'Submit_Daily_Reports' || path == 'RegisterFunction') {
        $('#fReporting').addClass('active');
        $('ul:first', $('#fReporting')).slideDown(function () {
            setContentHeight();
        });

    }
    else if (path == 'Reports/DayTrainerPerformanceLog' || path == 'Reports/TrainingFollowUps') {
        $('#tReports').addClass('active');
        $('ul:first', $('#tReports')).slideDown(function () {
            setContentHeight();
        });
    }
    else if (path == 'Reports/BoardingAvailability' || path == 'Reports/BoardingNoShows') {
        $('#bReports').addClass('active');
        $('ul:first', $('#bReports')).slideDown(function () {
            setContentHeight();
        });
    }
    else if (path == 'Reports/DayCampRefundCalculator' || path == 'Reports/Daycampwithbalance') {
        $('#single').addClass('active');
        $('ul:first', $('#single')).slideDown(function () {
            setContentHeight();
        });
    }
    else if (path == 'Client/DayCamp' || path == 'Client/BookBoardingReservation' || path == 'Client/ReservationHistory') {
        $('#ress').addClass('active');
        $('ul:first', $('#ress')).slideDown(function () {
            setContentHeight();
        });
    }
    else if (path == 'Client/ClientDashBoard')
        $('.side-menu > li > a[href="/Client/ClientDashBoard.aspx"]').parent().addClass('active');
    else if (path == 'Client/ClientInfoSetting')
        $('.side-menu > li > a[href="/Client/ClientInfoSetting.aspx"]').parent().addClass('active');
    else if (path == 'Client/ClientPets.aspx')
        $('.side-menu > li > a[href="/Client/ClientPets.aspx.aspx"]').parent().addClass('active');






    $('#stars').on('starrr:change', function (e, value) {
        $('#count').html(value);
    });


    $('#stars-existing').on('starrr:change', function (e, value) {
        $('#count-existing').html(value);
    });

});

// Table
$('table input').on('ifChecked', function () {
    check_state = '';
    $(this).parent().parent().parent().addClass('selected');
    countChecked();
});
$('table input').on('ifUnchecked', function () {
    check_state = '';
    $(this).parent().parent().parent().removeClass('selected');
    countChecked();
});

var check_state = '';
$('.bulk_action input').on('ifChecked', function () {
    check_state = '';
    $(this).parent().parent().parent().addClass('selected');
    countChecked();
});
$('.bulk_action input').on('ifUnchecked', function () {
    check_state = '';
    $(this).parent().parent().parent().removeClass('selected');
    countChecked();
});
$('.bulk_action input#check-all').on('ifChecked', function () {
    check_state = 'check_all';
    countChecked();
});
$('.bulk_action input#check-all').on('ifUnchecked', function () {
    check_state = 'uncheck_all';
    countChecked();
});

function countChecked() {
    if (check_state == 'check_all') {
        $(".bulk_action input[name='table_records']").iCheck('check');
    }
    if (check_state == 'uncheck_all') {
        $(".bulk_action input[name='table_records']").iCheck('uncheck');
    }
    var n = $(".bulk_action input[name='table_records']:checked").length;
    if (n > 0) {
        $('.column-title').hide();
        $('.bulk-actions').show();
        $('.action-cnt').html(n + ' Records Selected');
    } else {
        $('.column-title').show();
        $('.bulk-actions').hide();
    }
}

// Accordion
$(function () {
    $(".expand").on("click", function () {
        $(this).next().slideToggle(200);
        $expand = $(this).find(">:first-child");

        if ($expand.text() == "+") {
            $expand.text("-");
        } else {
            $expand.text("+");
        }
    });
});


// NProgress
if (typeof NProgress != 'undefined') {
    $(document).ready(function () {
        NProgress.start();
    });

    $(window).load(function () {
        NProgress.done();
    });
}

/**
 * Resize function without multiple trigger
 * 
 * Usage:
 * $(window).smartresize(function(){  
 *     // code here
 * });
 */
(function ($, sr) {
    // debouncing function from John Hann
    // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
    var debounce = function (func, threshold, execAsap) {
        var timeout;

        return function debounced() {
            var obj = this, args = arguments;
            function delayed() {
                if (!execAsap)
                    func.apply(obj, args);
                timeout = null;
            }

            if (timeout)
                clearTimeout(timeout);
            else if (execAsap)
                func.apply(obj, args);

            timeout = setTimeout(delayed, threshold || 100);
        };
    };

    // smartresize 
    jQuery.fn[sr] = function (fn) { return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

})(jQuery, 'smartresize');