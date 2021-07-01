<?php
    // #0 -> todo ha ido BIEN
    // error #1 -> conexión fallida
    // error #7 -> consulta mejores tiempos fallida
    // error #8 -> el usuario no ha completado ninguna escape room
    // error #9 -> no se ha podido crear una partida nueva
    // error #10 -> no se ha podido insertar un tiempo
    // error #11 -> no se ha podido insertar una relación usuario-partida

    
    $URL = "localhost";
    $USUARIO = "root";
    $CONTRASEÑA = "154e";
    $DB = "TFG_escape_room";
    $PUERTO = "3306";
    

    //establecemos la conexión
	$con = mysqli_connect($URL,$USUARIO,$CONTRASEÑA,$DB,$PUERTO);

    //miramos si se ha podido establecer la conexión
    if(mysqli_connect_errno()){
        echo("ERROR 1: conexión fallida"); 
        exit();
    }

    //Consulta #1 -> devolver mejor tiempo
    //Consulta #2 -> añadir una nueva partida y devolver el ID
    //Consulta #3 -> añadir un tiempo a una partida
    //Consulta #3 -> añadir una relación usuario-partida

    switch($_SERVER['REQUEST_METHOD']){
        case 'GET': 
            $request = &$_GET; 
            break;
        case 'POST': 
            $request = &$_POST; 
            break;
        default:
            break;
    }

    $opcion = $request['opcion'];

    switch($opcion){
        case 1: //dado un jugador devolver para todas las escape room su mejor tiempo y su cantidad de intentos
            $usuario = $request['usuario'];

            $consultaQuery ="SELECT escaperoom.id, escaperoom.nombre escaperoomNombre, escaperoom.dificultad escaperoomDificultad, aux.mejorTiempo, aux.numIntentos
                FROM escaperoom LEFT JOIN 
                (SELECT escaperoom.id idEsc, escaperoom.nombre, escaperoom.dificultad, MIN(tiempo.tiempo) mejorTiempo, COUNT(partida.id) numIntentos
                    FROM escaperoom JOIN partida ON escaperoom.id=partida.idEscaperoom
                    JOIN tiempo ON partida.id=tiempo.idPartida
                    JOIN usuario_partida ON usuario_partida.idPartida = partida.id
                    JOIN usuario ON usuario_partida.idUsuario = usuario.id
                    JOIN prueba ON tiempo.idPrueba=prueba.id
                    WHERE prueba.nombre='FINAL' AND usuario.nombre = '".$usuario."' 
                    GROUP BY escaperoom.id) aux
                ON escaperoom.id = aux.idEsc";

            $consultaResultado = mysqli_query($con,$consultaQuery) or die("ERROR 7: consulta de los mejores tiempos fallida");

            if(mysqli_num_rows($consultaResultado) > 0) {
                $jsonDevuelto = '{"cantSalas":"'.mysqli_num_rows($consultaResultado).'", "partidas":[';
                    $reg = mysqli_fetch_array($consultaResultado);
                    $jsonDevuelto .= '{"escaperoom_nombre":"'.$reg["escaperoomNombre"].'",
                        "escaperoom_dificultad":"'.$reg["escaperoomDificultad"].'",
                        "mejor_tiempo":"'.$reg["mejorTiempo"].'",
                        "num_intentos":"'.$reg["numIntentos"].'"}';

                while($reg = mysqli_fetch_array($consultaResultado)){
                    $jsonDevuelto .= ',{"escaperoom_nombre":"'.$reg["escaperoomNombre"].'",
                        "escaperoom_dificultad":"'.$reg["escaperoomDificultad"].'",
                        "mejor_tiempo":"'.$reg["mejorTiempo"].'",
                        "num_intentos":"'.$reg["numIntentos"].'"}';
                }
                $jsonDevuelto .= ']}';
                echo($jsonDevuelto);
            }
            break;

        case 2: //añadir una nueva partida y devolver el ID
            $idEscaperoom = $request['idEscRoom'];
            $consultaQuery = "INSERT INTO partida(fecha, idEscaperoom) VALUES (CURDATE(),".$idEscaperoom.")";
            mysqli_query($con,$consultaQuery) or die("9: no se ha podido crear una partida nueva");
            $idPartida = mysqli_insert_id($con);
            echo($idPartida);
            break;

        case 3: //dado un id de partida, un id de prueba y un tiempo, inserta un tiempo
            $idPartida = $request['idPartida'];
            $idPrueba = $request['idPrueba'];
            $tiempo = floatval($request['tiempo']);
            $tiempoFormateado = sprintf('%02d:%02d:%02d', ($tiempo/3600),($tiempo/60%60), $tiempo%60);
            $consultaQuery = "INSERT INTO tiempo(idPartida, idPrueba, tiempo) VALUES (".$idPartida.",".$idPrueba.",TIME('".$tiempoFormateado."'));";
            mysqli_query($con,$consultaQuery) or die ("10: no se ha podido insertar un tiempo");
            break;

        case 4: // dado un id de usuario y un id de partida, inserta un usario_partida
            $idPartida = $request['idPartida'];
            $usuario = $request['usuario'];
            $consultaQuery = "INSERT INTO usuario_partida(idUsuario, idPartida) VALUES (".$usuario.",".$idPartida.");";
            mysqli_query($con,$consultaQuery) or die ("11: no se ha podido insertar una relación usuario-partida");
            echo("La consulta 4 ha ido bien");
            break;
    }    

    


?>