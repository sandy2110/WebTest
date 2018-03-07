
$(document).ready(function () {
    $(".loader").hide();
});

function executeResult() {
    $('#outputresult').html("");
    var name = $('#name').val();
    var amount = $('#amount').val();
    if (IsFormValid(name, amount)) {
        $.ajax({
            url: 'api/demoapi',
            type: 'POST',
            // Switch off caching
            cache: false,
            contentType: "application/json",
            data: JSON.stringify({ "Name": name, "Amount": amount }),
            dataType: 'json',
            beforeSend: function () {
                // loading starts
                $(".loader").show();
            },
            success: function (data) {
                var str_array = data.split(',');
                setTimeout(function () {
                    $(".loader").hide();
                }, 2000);
                $('#outputresult').html("Name : " + str_array[0] + " | Amount : <b>" + str_array[1].toUpperCase() + "</b>");
            },
            failure: function (data) {
                $(".loader").hide();
                $('#outputresult').html('<p class="alert alert-danger"><strong>Oops!</strong> There is some error.</p>');
            },
            error: function () {
                // failed request; give feedback to user
                $(".loader").hide();
                $('#outputresult').html('<p class="alert alert-danger"><strong>Oops!</strong> There is some error.</p>');
            },
            timeout: 10000 // sets timeout to 10 seconds
        });

    }
}

//To check Form Validations
function IsFormValid(name, amount) {
    if (name == "") {
        $('#outputresult').html('<p class="alert alert-warning"><strong>Oops!</strong> Name Is Required.</p>');
        return false;
    }
    else if ($.isNumeric(name)) {
        $('#outputresult').html('<p class="alert alert-warning"><strong>Oops!</strong> Numeric Characters not allowed as Name.</p>');
        return false;
    }
    else if (amount == "") {
        $('#outputresult').html('<p class="alert alert-warning"><strong>Oops!</strong> Amount Is Required.</p>');
        return false;
    }
    else { return true; }
}
