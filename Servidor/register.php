<?php
    // #0 -> todo ha ido BIEN
    // error #1 -> conexión fallida
    // error #2 -> query checkName fallida
    // error #3 -> el nombre ya existe
    // error #4 -> no se ha podido registrar el usuario

    $URL = "localhost";
    $USUARIO = "root";
    $CONTRASEÑA = "154e";
    $DB = "TFG_escape_room";
    $PUERTO = "3306";

    //establecemos la conexión
    $con = mysqli_connect($URL,$USUARIO,$CONTRASEÑA,$DB,$PUERTO);

    //miramos si se ha podido establecer la conexión
    if(mysqli_connect_errno()){
        echo("1: conexión fallida"); 
        exit();
    }

    $user = $_POST["usuario"];
    $pass = $_POST["contraseña"];

    //miramos si el nombre existe
    $nameCheckQuery = "SELECT nombre FROM usuario WHERE nombre = '" . $user . "';";
    $nameCheck = mysqli_query($con,$nameCheckQuery) or die("2: query fallida");

    if(mysqli_num_rows($nameCheck) > 0){
        echo("3: el nombre ya existe");
        exit();
    }

    //añadir el usuario a la BD
    $hash = password_hash($pass,PASSWORD_DEFAULT);

    $insertUserQuery = "INSERT INTO usuario(nombre, hash) VALUES ('".$user."','".$hash."');";
    mysqli_query($con,$insertUserQuery) or die("4: error al insertar usuario");

    echo("0");
?>