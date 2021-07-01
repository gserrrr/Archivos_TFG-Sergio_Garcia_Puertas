var dimensionesMapas = [];
dimensionesMapas[1] = {
    "anchoJuego":20,
    "altoJuego":10
};

var anchoJuego = 20;
var altoJuego = 10;

var anchoCanvas = 1000;
var altoCanvas = 500;

$(document).ready(function() {
    $("#e2_form").css("display","none");
    $("#p2").css("display","none");
    $("#e3_form").css("display","none");
    $("#p3").css("display","none");


    let f = new FormData();
    f.append('opcion',2);
    $.ajax({
        url: 'https://alumnes-ltim.uib.es/escaperoom/apiAnalisis.php',
        data: f,
        method: 'post',
        processData: false,  // tell jQuery not to process the data
        contentType: false,   // tell jQuery not to set contentType
        enctype: 'multipart/form-data',
        success: function(result){
            var jsonEscapeRooms = JSON.parse(result);

            $("#e1_form").find("option").remove();
            $("#e1_form").append("<option value='-'>-</option>");
            for (let i = 0; i < jsonEscapeRooms.length; i++) {
                $("#e1_form").append("<option value="+ jsonEscapeRooms[i].id +">"+ jsonEscapeRooms[i].nombre +"</option>");
            }
        }
    });

    //si se cambia el primer combobox
    $("#e1_form").change(function() { 
        cambioCombobox1();
    });

    //si se cambia el segundo combobox
    $("#e2_form").change(function() {
        cambioCombobox2();
    });

    //si se cambia el tercer combobox
    $("#e3_form").change(function() {
        cambioCombobox3();
    });
});

function pintarMapaCalor(jsonPosiciones){
        var jsonBueno = [];
    for (let i = 0; i < jsonPosiciones.length; i++) {
        var coordenadas = jsonPosiciones[i];
        var aux = {
            "x":coordenadas.y,
            "y":coordenadas.x
        }
        
        aux.x = (aux.x/dimensionesMapas[$("#e1_form").val()].anchoJuego) * anchoCanvas;
        aux.y = (aux.y/dimensionesMapas[$("#e1_form").val()].altoJuego) * altoCanvas;

        var existe = false;
        for(let j=0; j < jsonBueno.length; j++){
            var aux2 = jsonBueno[j];
            if(aux2.x == aux.x && aux2.y == aux.y){ //si ya está esa posicion guardada
                existe = true;
                jsonBueno[j].value = jsonBueno[j].value + 1;
                break;
            }
        }
        if(!existe){
            var nuevo = {
                "x":aux.x,
                "y":aux.y,
                "value":1
            }
            jsonBueno.push(nuevo);
        }
    }    

    //creamos el mapa de calor
    if( $('#heatmapContainer').length){
        $('#heatmapContainer').remove();
    }
    $("body").append("<div id='heatmapContainer'></div>");
    var config = {
        container: document.getElementById('heatmapContainer'),
        radius: 10,
        maxOpacity: .5,
        minOpacity: .15,
        blur: .75,
        gradient: {
            '.5': 'blue',
            '.8': 'red',
            '.95': 'white'
        }
    };
    var heatmapInstance = h337.create(config);
    var data = {
        max:50,
        min:0,
        data:jsonBueno
    };
    heatmapInstance.setData(data);
}

function cambioCombobox1(){
    var escaperoomSeleccionado = false;
    if($("#e1_form").val() != '-') escaperoomSeleccionado = true;
    if(!escaperoomSeleccionado){
        $("#e3_form").css("display","none");
        $("#p3").css("display","none");

        $("#e3_form").find("option").remove();
        $("#e3_form").append("<option value='-'>-</option>");
        
        $("#e2_form").css("display","none");
        $("#p2").css("display","none");

        $("#e2_form").find("option").remove();
        $("#e2_form").append("<option value='-'>-</option>");
    }else{
        $("#e2_form").css("display","block");
        $("#p2").css("display","block");
    }

    if($("#e1_form").val() == '-'){
        pintarMapaCalor(JSON.parse("[]"));
        return;
    }

    var form = new FormData();    
    form.append('opcion', 3);
    form.append('idEscRoom', $("#e1_form").val());

    $.ajax({
        url: 'https://alumnes-ltim.uib.es/escaperoom/apiAnalisis.php',
        data: form,
        method: 'post',
        processData: false,  // tell jQuery not to process the data
        contentType: false,   // tell jQuery not to set contentType
        enctype: 'multipart/form-data',
        success: function(result){
            var json = JSON.parse(result);
            var display = json.display;
            var posiciones = json.posiciones;
            if(display.length > 0){
                $("#e2_form").find("option").remove();
                $("#e2_form").append("<option value='-'>-</option>");
                for(let i=0; i < display.length; i++){
                    $("#e2_form").append("<option value="+ display[i].id +">"+ display[i].tiempo + " - " + display[i].fecha + "</option>");
                }
            }
            
            pintarMapaCalor(posiciones);
        }
    });
}

function cambioCombobox2(){
    var partidaSeleccionada = false;
    if($("#e2_form").val() != '-') partidaSeleccionada = true;
    if(!partidaSeleccionada){
        $("#e3_form").css("display","none");
        $("#p3").css("display","none");

        $("#e3_form").find("option").remove();
        $("#e3_form").append("<option value='-'>-</option>");
    }else{
        $("#e3_form").css("display","block");
        $("#p3").css("display","block");
    }
    if($("#e2_form").val() == '-'){
        cambioCombobox1();
        return;
    };
    var form = new FormData();    
    form.append('opcion', 4);
    form.append('idPartida', $("#e2_form").val());

    $.ajax({
        url: 'https://alumnes-ltim.uib.es/escaperoom/apiAnalisis.php',
        data: form,
        method: 'post',
        processData: false,  // tell jQuery not to process the data
        contentType: false,   // tell jQuery not to set contentType
        enctype: 'multipart/form-data',
        success: function(result){
            var json = JSON.parse(result);
            var display = json.display;
            var posiciones = json.posiciones;

            if(display.length > 0){
                $("#e3_form").find("option").remove();
                $("#e3_form").append("<option value='-'>-</option>");
                for(let i=0; i < display.length; i++){
                    $("#e3_form").append("<option value="+ display[i].id +">"+ display[i].nombre + "</option>");
                }
            }
            
            pintarMapaCalor(posiciones);
        }
    });
}

function cambioCombobox3(){
    if($("#e3_form").val() == '-'){
        cambioCombobox2();
        return;
    };

    var form = new FormData();    
    form.append('opcion', 5);
    form.append('idPartida', $("#e2_form").val());
    form.append('idUsuario', $("#e3_form").val());

    $.ajax({
        url: 'https://alumnes-ltim.uib.es/escaperoom/apiAnalisis.php',
        data: form,
        method: 'post',
        processData: false,  // tell jQuery not to process the data
        contentType: false,   // tell jQuery not to set contentType
        enctype: 'multipart/form-data',
        success: function(result){
            var json = JSON.parse(result);
            
            pintarMapaCalor(json);
        }
    });
}


