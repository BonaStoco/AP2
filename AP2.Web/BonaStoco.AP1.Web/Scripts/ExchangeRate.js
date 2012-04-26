var MataUangID = document.getElementById("MataUang").options[0].value;
var MataUangCode;
var kurs = $("#Kurs");
var kursEdit = $("#kurs-edit");
var tips = $(".validateTips");
var allFields = $( [] ).add( MataUangID ).add( kurs ).add(tips);
var listExchange = new Array();

$(function () {
    var dates = $("#dari, #sampai").datepicker({ dateFormat: 'yy-mm-dd',
        defaultDate: "+1w",
        changeMonth: true,
        numberOfMonths: 1,
        onSelect: function (selectedDate) {
            var option = this.id == "dari" ? "minDate" : "maxDate",
			instance = $(this).data("datepicker"),
			date = $.datepicker.parseDate(
				instance.settings.dateFormat ||
				$.datepicker._defaults.dateFormat,
				selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });
});

function updateTips( t ) {
	tips
		.text( t )
		.addClass( "ui-state-highlight" );
	setTimeout(function() {
		tips.removeClass( "ui-state-highlight", 500 );
	}, 500 );
}

function checkRegexp( o, regexp, n ) {
	if ( !( regexp.test( o.val() ) ) ) {
		o.addClass( "ui-state-error" );
		updateTips( n );
		return false;
	} else 
		return true;
			
}
function checkRate(o, nilai, n)
{   
    if(nilai < 1)
    {
        o.addClass("ui-state-error");
        updateTips(n);
        return false;
    }
    else
        return true;
}
function IsExistData()
{
    var found = false;
    for (var e in listExchange) {
        var value = listExchange[e];
	    if(value.MataUangId == MataUangID)
        {
            found = true;
        }
    }
    return found;
}
function IsIDRExistInListExchange()
{
    var found = false;
    for (var e in listExchange) {
        var value = listExchange[e];
	    if(value.CcyCode == "IDR")
        {
            found = true;
        }
    }
    return found;
}
function AddViewListExchange() {
    document.getElementById("updateBtn").disabled = false;
    for (var e in listExchange) {
        value = listExchange[e];
        $("tbody#bodyExchange").append("<tr class='bodyTable'>" +
			"<td>" + value.CcyCode + "</td>" +
			"<td class='right'>" + value.Rate + "</td>" +
			"<td class='center' id='Edit' onclick='dialogEdit(" + value.MataUangId +")'></td>" +
			"</tr>");
    }
}
function ClearListExchangeView()
{
    var table = document.getElementById("bodyExchange");
    while(table.rows.length > 1)
    {
            table.deleteRow(1);
    }
}
function EditExchange()
{
    var idMataUang = $("#idmatauang").val();
    for(var e in listExchange) {
	    var value = listExchange[e];
	    if(value.MataUangId == idMataUang)
        {
            value.Rate = kursEdit.val();
        }
    }
}
function UpdateItem() {
    for (var e in listExchange) {
        var value = listExchange[e];
        if (value.MataUangId == MataUangID) {
            value.Rate = kurs.val();
        }
    }
}

$("#Dialog-Add").dialog({
    autoOpen: false,
    height: 300,
    width: 350,
    modal: true,
    buttons: {
        "Add":
                function () {
                    var bValid1 = checkRegexp(kurs, /^([0-9])+$/i, "Kurs Harus Di isi dan Angka.");
                    var bValid2 = checkRate(kurs, kurs.val(), "Nilai Kurs Harus Besar Nol.");

                    if (!(bValid1 && bValid2))
                        return;

                    if (MataUangID == 00)
                        return;

                    if (!IsExistData()) {
                        ClearListExchangeView();
                        var _kurs = new Object();
                        _kurs.MataUangId = MataUangID;
                        _kurs.CcyCode = MataUangCode;
                        _kurs.Rate = kurs.val();
                        listExchange.push(_kurs)
                        AddViewListExchange();
                        $(this).dialog("close");
                    }
                    else {
                        ClearListExchangeView();
                        UpdateItem();
                        AddViewListExchange();
                        $(this).dialog("close");
                    }
                },
        Cancel:
                function () {
                    $(this).dialog("close");
                }
    },
    close:
                function () {
                    allFields.val("").removeClass("ui-state-error");
                    tips.text('');
                }
});

$("#Dialog-Edit").dialog({
    autoOpen: false,
    height: 300,
    width: 350,
    modal: true,
    buttons:
            {
                "Edit":
                    function () {
                        var bValid1 = checkRegexp(kursEdit, /^([0-9])+$/i, "Kurs Harus Di isi dan Angka.");
                        var bValid2 = checkRate(kursEdit, kursEdit.val(), "Nilai Kurs Harus Besar Nol.");

                        if (!(bValid1 && bValid2))
                            return;

                        ClearListExchangeView();
                        EditExchange();
                        AddViewListExchange();
                        $(this).dialog("close");
                    },
                Cancel:
                    function () {
                        $(this).dialog("close");
                    }
            }
});

$("#Add").click(function(){
    $("#Dialog-Add").dialog("open");
    });
function dialogEdit(id) {
    for (var e in listExchange) {
        var value = listExchange[e];
        if (value.MataUangId == id) {
            document.getElementById("idmatauang").value = value.MataUangId;
            document.getElementById("matauang").value = value.CcyCode;
            document.getElementById("kurs-edit").value = value.Rate;
            $("#Dialog-Edit").dialog("open");
        }
    }
}
function Change() {
    selectedindex = document.getElementById("MataUang").selectedIndex;
        MataUangID = document.getElementById("MataUang").options[selectedindex].value;
        MataUangCode = document.getElementById("MataUang").options[selectedindex].text;
}

$("#updateBtn").click(function () {
    var dari = document.getElementById("dari").value;
    var sampai = document.getElementById("sampai").value;
    if (dari == "") {
        alert("Tanggal Dari Harus Di isi.");
    }
    else if (sampai == "") {
        alert("Tanggal Sampai Harus Di isi.");
    }
    else {
        InsertIDR();
        var exchangeRate = new Object();
        exchangeRate.StartDate = dari;
        exchangeRate.EndDate = sampai;
        exchangeRate.Items = listExchange;
        var test = "data=" + JSON.stringify(exchangeRate);
        $.ajax({
            type: "POST",
            url: "/ExchangeRate/UpdateExchangeRate",
            data: { 'data': JSON.stringify(exchangeRate) },
            dataType: "json",
            success: ClearList
        });
    }
});
function ClearList() {
    
    document.getElementById("updateBtn").disabled = true;
    ClearListExchangeView();
    listExchange = new Array();
    alert("Update Data Exchange Rate Berhasil");
}

function InsertIDR() {
    if (IsIDRExistInListExchange())
        return;

    var idr = new Object();
    idr.CcyCode = "IDR";
    idr.Rate = 1;
    listExchange.push(idr);
}

$(document).ready(function () {
    $("#Kurs, #kurs-edit").keydown(function (event) {
        if (event.keyCode == 46 || event.keyCode == 8) {
        }
        else {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.preventDefault();
            }
        }
    });
});
