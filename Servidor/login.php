<?php
    // #0 -> todo ha ido BIEN
    // error #1 -> conexión fallida
    // error #5 -> nombre de usuario repetido
    // error #6 -> contraseña incorrecta

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

    $user = $_POST["usuario"];
    $pass = $_POST["contraseña"];

    //miramos si el nombre existe
    $nameCheckQuery = "SELECT id, nombre, hash FROM usuario WHERE nombre = '" . $user . "';";
    $nameCheck = mysqli_query($con,$nameCheckQuery) or die("ERROR 2: query fallida");

    if(mysqli_num_rows($nameCheck) == 0){
        echo("ERROR 7: el nombre de usuario no existe");
        exit();
    }

    if(mysqli_num_rows($nameCheck) > 1){
        echo("ERROR 5: el nombre de usuario parece estar repetido");
        exit();
    }

    //cogemos la información de la consulta
    $loginInfo = mysqli_fetch_array($nameCheck);
    $hash = $loginInfo["hash"];

    if(!password_verify($pass,$hash)){ //si el hash aplicado a la contraseña NO coincide con el hash guardado
        echo("ERROR 6: contraseña incorrecta");
        exit();
    }

    echo($loginInfo["id"]); //se ha iniciado sesión correctamente
?>