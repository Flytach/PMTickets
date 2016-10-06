
var price = 10; //price
var firstSeatLabel = 1;
var rowA = 'A';
//$(document).ready(function () {
function getSeatData(movieID, date, time) {
    
    //alert(movieID + ' ' + date + ' ' + time)
    var $cart = $('#selected-seats'), //Sitting Area
    $counter = $('#counter'), //Votes
    $total = $('#total'); //Total money

    var sc = $('#seat-map').seatCharts({
        map: [  //Seating chart
            '___aaaaaaaaa___',
            'aaaaaaaaaaaaaaa',
            'aaaaaaaaaaaaaaa',
            'aaaaaaaaaaaaaaa',
            '__aaaaaaaaaaa__',
        ],
        naming: {
            top: false,
            rows: ['A', 'B', 'C', 'D','E'],
            getLabel: function (character, row, column) {
                if (rowA != row) {
                    firstSeatLabel = 1;
                    rowA = row;
                    return firstSeatLabel++;
                }
                else {
                    return firstSeatLabel++;
                }
            }
        },
        legend: { //Definition legend
            node: $('#legend'),
            items: [
                ['a', 'available', 'Option'],
                ['a', 'unavailable', 'Sold']
            ]
        },
        click: function () { //Click event
            if (this.status() == 'available') { //optional seat
                var t1;
                switch ((this.settings.row + 1)) {
                    case 1:
                        t1 = 'A';
                        break;
                    case 2:
                        t1 = 'B';
                        break;
                    case 3:
                        t1 = 'C';
                        break;
                    case 4:
                        t1 = 'D';
                    case 5:
                        t1 = 'E';
                        break;
                }

                $('<li>' + t1 + '_' + this.settings.label + '</li>')
                    .attr('id', 'cart-item-' + this.settings.id)
                    .data('seatId', this.settings.id)
                    .appendTo($cart);

                $counter.text(sc.find('selected').length + 1);

                $total.text(recalculateTotal(sc) + price);

                return 'selected';
            } else if (this.status() == 'selected') { //Checked
                //Update Number
                $counter.text(sc.find('selected').length - 1);
                //update totalnum
                $total.text(recalculateTotal(sc) - price);

                //Delete reservation
                $('#cart-item-' + this.settings.id).remove();
                //optional
                return 'available';
            } else if (this.status() == 'unavailable') { //sold
                return 'unavailable';
            } else {
                return this.style();
            }
        }
    });
    //sold seat
    //sc.get(['1_1', '1_2']).status('unavailable');
    getBookedSeats(sc, movieID, date, time);

}
//);
//sum total money
function recalculateTotal(sc) {
    var total = 0;
    sc.find('selected').each(function () {
        total += price;
    });

    return total;
}

//CHECKOUT 
function checkOut() {
    var $selectedSeats = '';

    var $selectedSeat = $('#selected-seats li').each(function () {        
            $selectedSeats = $(this).text() + '$' + $selectedSeats;        
    });

    var ddlMoviesName = document.getElementById("ddlMovies");
    var ddlMoviesNameOTxt = ddlMoviesName.options[ddlMoviesName.selectedIndex].text;
    $("#lblmovieName").html(ddlMoviesNameOTxt);

    var ddlDateOTxt = $("#txtShowDate").val();
    $("#lblmovieDate").html(ddlDateOTxt);

    var ddlMoviesTim = document.getElementById("ddlMovieTimings");
    var ddlMoviesTimeOTxt = ddlMoviesTim.options[ddlMoviesTim.selectedIndex].text;

    $("#lblmovieTime").html(ddlMoviesTimeOTxt);
    var myUrl = $("#AppUrl").val();
    
    $.ajax({
        //url: "./home/updateMovieSeats",
        url: myUrl + 'home/updateMovieSeats',        
        type: 'POST',
        data: {
            "movieID": ddlMoviesNameOTxt,
            "movieDate": ddlDateOTxt,
            "movieTime": ddlMoviesTimeOTxt,
            "bookedSeats": $selectedSeats
        },
        success: function (data) {
            alert('Tickets Booked Successfully...!!!')
            $('#selected-seats').empty();
            $('#counter').empty(); 
            getSeatData(ddlMoviesNameOTxt, ddlDateOTxt, ddlMoviesTimeOTxt)
        }
    });
   
}


function getBookedSeats(sc, movieID, date, time) {
    var myUrl = $("#AppUrl").val();
    
    sc.find('unavailable').status('available');
    //Sold seats    
    
    $.ajax({        
        url: $("#AppUrl").val() + 'home/getbookedseats',
        type: 'GET',
        data: {
            "movieID": movieID,
            "movieDate": date,
            "movieTime": time
        },
        success: function (data) {
            sc.get(data).status('unavailable');
            $('#seat-map').fadeIn();
            $('#booking-details').fadeIn();            
        },
        error: function (err) {
            alert('Selected Movie Data Not being entered...!!!');            
            $('#seat-map').fadeOut();
            $('#booking-details').fadeOut();
        }

    });
}

function CreateClick() {
    var ddlMoviesTim = document.getElementById("ddlMovieTimings");
    var ddlMoviesTimeOTxt = ddlMoviesTim.options[ddlMoviesTim.selectedIndex].text;
    
    $.ajax({
        //url: 'createMovieINFile',
        url: $("#AppUrl").val() + 'home/createMovieINFile',
        type: 'POST',
        data: {
            "movieID": $("#ddlmoviename").val(),
            "movieDate": $("#txtShowDate").val(),
            "movieTime": ddlMoviesTimeOTxt
        },
        success: function (data) {
            alert('Movie Successfully Created...!!!')
            //getSeatData($('#ddlMovies').val(), ddlDateOTxt, ddlMoviesTimeOTxt)
        },
        error: function (data) {
            alert('Movie data already exists...!!!')
        }

    });
}

function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    var CSV = '';
    //Set Report title in first row or line

    CSV += ReportTitle + '\r\n\n';

    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";

        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {

            //Now convert each value to string and comma-seprated
            row += index + ',';
        }

        row = row.slice(0, -1);

        //append Label row with line break
        CSV += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";

        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }

        row.slice(0, row.length - 1);

        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "BookingReport";
    //this will remove the blank-spaces from the title and replace it with an underscore
    //fileName += ReportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function getMoviesList() {

    var myUrl = $("#AppUrl").val();

    $.ajax({
        url: $("#AppUrl").val() + 'home/getMovieData',
        type: 'GET',        
        success: function (data) {
            JSONToCSVConvertor(data, 'Booking Report', true)
        },
        error: function (err) {
            alert('Error Occured...!!!');           
        }

    });
}