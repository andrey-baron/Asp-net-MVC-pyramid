;(function () {
    var maxVal=$("#MaxPrice").val();
    var minVal = $("#MinPrice").val();

    var curmaxVal = $("#CurrentMaxPrice").val();
    var curminVal = $("#CurrentMinPrice").val();
    $(".sliderUI").slider({
        
        max:maxVal ,
        //min: minVal,
        range: true,
        values: [curminVal, curmaxVal],
        step:1,
        start: function (event, ui) {
            var values= $( ".sliderUI" ).slider( "values" );
            $(".spinnerUI1").spinner("value", values[0]);
            $(".spinnerUI2").spinner("value", values[1]);
        },
        change: function (event, ui) {
            var values = $(".sliderUI").slider("values");
            $(".spinnerUI1").spinner("value", values[0]);
            $(".spinnerUI2").spinner("value", values[1]);
        }
    });
     
    $(".spinnerUI1").spinner({
        incremental: true,
               
        numberFormat: "n",
        change: function (event, ui) {
           
            $(".sliderUI").slider("values", 0, this.value);
        }
    });
    $(".spinnerUI2").spinner({
        incremental: false,
        
        
        numberFormat: "n",
        change: function (event, ui) {
            $(".sliderUI").slider("values", 1, this.value);
        }
    }); 
})();