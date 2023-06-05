<?php
require_once('../db_connection.php');
$id = $_POST["NewForm0"];
$Table= $_POST["NewForm1"];
$NameC = $_POST["NewForm2"];

// Формируем подготовленный SQL-запрос
$sql = "DELETE FROM `$Table` WHERE `$NameC`=?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $id);

if ($stmt->execute() === TRUE) {
    echo "Запись удалена успешно";
} else {
    echo "Ошибка удаления записи: " . $stmt->error;
}

$stmt->close();
$conn->close();

?>