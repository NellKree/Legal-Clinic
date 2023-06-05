<?php
// Параметры подключения к базе данных
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "legalclinic";

// Создаем соединение
$conn = new mysqli($servername, $username, $password, $dbname);

// Проверяем соединение на ошибки
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
?>
