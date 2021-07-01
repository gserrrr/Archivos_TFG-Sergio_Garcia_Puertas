<?php
    // #0 -> todo ha ido BIEN
    // error #1 -> conexión fallida


    
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

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    //Consulta #1 -> Añadir JSON de posiciones a la BD

    //Consulta #2 -> Obtener el id y nombre de todos los escape rooms [esta se llama al cargar la página]

    //Consulta #3 -> Dado un escape room ----> obtener un json con todas las posiciones de todas las partidas (para pintar el mapa)
    //               &&
    //               obtener el id, tiempo y fecha de todas las partidas de ese escape room
    
    //Consulta #4 -> Dado un escape room y una partida ----> obtener un json con todas las posiciones de todos los jugadores en esa partida (para pintar el mapa)
    //               &&
    //               obtener el id y nombre de usuario de todos los jugadores de esa partida

    //Consulta #5 -> Dado un escape room, una partida y un usuario ----> obtener los datos de las posiciones (para pintar el mapa)

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        case 1: 
            $jsonDatos = $request['datos'];
            $idPartida = $request['idPartida'];
            $idUsuario = $request['idUsuario'];
            $tipoDatos = $request['tipoDatos'];
            $consulta = "INSERT INTO datos(json, idPartida, idUsuario, idTipodatos) VALUES ('".$jsonDatos."',".$idPartida.",".$idUsuario.",".$tipoDatos.");";
            mysqli_query($con,$consulta) or die ("ERROR 2: fallo en la consulta #1");
            echo("La consulta 1 ha ido bien");
            break;
        
        case 2:
            $consulta = "SELECT id, nombre FROM escaperoom;";
            $resultado = mysqli_query($con,$consulta) or die ("ERROR 3: fallo en la consulta #2");
            $reg = mysqli_fetch_array($resultado);
            $jsonRespuesta = '[{"id":"'.$reg["id"].'",
                                "nombre":"'.$reg["nombre"].'"}';

            while($reg = mysqli_fetch_array($resultado)){
                $jsonRespuesta .= ',{"id":"'.$reg["id"].'",
                    "nombre":"'.$reg["nombre"].'"}';
            }

            $jsonRespuesta .= ']';
            echo($jsonRespuesta);
            break;
        
        case 3:
            $idEscRoom = $request['idEscRoom'];

            //primera consulta para obtener los datos que se utilizarán en el combobox
            $consulta1 = "SELECT partida.id id, partida.fecha fecha, tiempo.tiempo tiempo FROM
                        partida JOIN tiempo ON partida.id=tiempo.idPartida
                        JOIN prueba ON tiempo.idPrueba=prueba.id
                        WHERE partida.idEscaperoom='".$idEscRoom."' AND prueba.nombre='FINAL';";
            $resultado1 = mysqli_query($con,$consulta1) or die ("ERROR 4: fallo en la consulta #3");  

            //segunda consulta para obtener los datos del json
            $consulta2 = "SELECT datos.json json FROM
                        partida JOIN usuario_partida ON partida.id = usuario_partida.idPartida
                        JOIN datos ON usuario_partida.idPartida = datos.idPartida
                        WHERE partida.idEscaperoom='".$idEscRoom."' AND datos.idTipodatos=1";
            $resultado2 = mysqli_query($con,$consulta2) or die ("ERROR 4: fallo en la consulta #3"); 

            //construir primer bloque del json
            $reg1 = mysqli_fetch_array($resultado1);
            $jsonRespuesta = '{"display":[';
            if(mysqli_num_rows($resultado1) > 0){
                $jsonRespuesta .= '{
                                        "id":"'.$reg1["id"].'",
                                        "fecha":"'.$reg1["fecha"].'",
                                        "tiempo":"'.$reg1["tiempo"].'"
                                    }';
            }
            while($reg1 = mysqli_fetch_array($resultado1)){
                $jsonRespuesta .= ',{
                                        "id":"'.$reg1["id"].'",
                                        "fecha":"'.$reg1["fecha"].'",
                                        "tiempo":"'.$reg1["tiempo"].'"
                                    }';
            }
            $jsonRespuesta .= '],';

            //construir segundo bloque del json
            $reg2 = mysqli_fetch_array($resultado2);
            $jsonRespuesta .= '"posiciones":[';
            if(mysqli_num_rows($resultado2) > 0){
                $jsonRespuesta .= substr($reg2['json'],1,-1);
            }

            while($reg2 = mysqli_fetch_array($resultado2)){
                $jsonRespuesta .= ',';
                $jsonRespuesta .= substr($reg2['json'],1,-1);
            }
            $jsonRespuesta .= ']}';
            echo($jsonRespuesta);
            break;

        case 4:
            $idPartida = $request['idPartida'];

            //consulta para obtener los datos del siguiente combobox
            $consulta1 = "SELECT usuario.id id, usuario.nombre nombre FROM
                        usuario_partida JOIN usuario ON usuario.id = usuario_partida.idUsuario
                        WHERE usuario_partida.idPartida = ".$idPartida.";";
            $resultado1 = mysqli_query($con,$consulta1) or die ("ERROR 5: fallo en la consulta #4");

            //consulta para obtener los datos del posicionamiento de todos los jugadores en una partida
            $consulta2 = "SELECT datos.json json FROM
                        usuario_partida JOIN datos ON usuario_partida.idUsuario = datos.idUsuario AND usuario_partida.idPartida = datos.idPartida
                        WHERE datos.idTipodatos = 1 AND usuario_partida.idPartida = ".$idPartida.";";
            $resultado2 = mysqli_query($con,$consulta2) or die ("ERROR 5: fallo en la consulta #4");

            //construir el primer bloque del json
            $reg1 = mysqli_fetch_array($resultado1);
            $jsonRespuesta = '{"display":[';
            if(mysqli_num_rows($resultado1) > 0){
                $jsonRespuesta .= '{
                                        "id":"'.$reg1["id"].'",
                                        "nombre":"'.$reg1["nombre"].'"
                                    }';
            }
            while($reg1 = mysqli_fetch_array($resultado1)){
                $jsonRespuesta .= ',{
                                        "id":"'.$reg1["id"].'",
                                        "nombre":"'.$reg1["nombre"].'"
                                    }';
            }
            $jsonRespuesta .= '],';
            
            //construir segundo bloque del json
            $reg2 = mysqli_fetch_array($resultado2);
            $jsonRespuesta .= '"posiciones":[';
            if(mysqli_num_rows($resultado2) > 0){
                $jsonRespuesta .= substr($reg2['json'],1,-1);
            }

            while($reg2 = mysqli_fetch_array($resultado2)){
                $jsonRespuesta .= ',';
                $jsonRespuesta .= substr($reg2['json'],1,-1);
            }
            $jsonRespuesta .= ']}';
            echo($jsonRespuesta);
            break;

        case 5:
            $idPartida = $request['idPartida'];
            $idUsuario = $request['idUsuario'];

            $consulta = "SELECT json FROM datos WHERE idTipodatos=1 AND idPartida=".$idPartida." AND idUsuario=".$idUsuario.";";
            $resultado = mysqli_query($con,$consulta) or die ("ERROR 6: fallo en la consulta #5");
            $reg = mysqli_fetch_array($resultado);
            echo($reg['json']);
            break;
    }    

    


?>