<?php
require_once('../db_connection.php');

$TableName = $_POST["TableName"];// название таблицы
$values = $_POST["values"];
$first_values = array_shift($values);

// получаем список столбцов таблицы
$sql_columns = "SHOW COLUMNS FROM `$TableName`";
$result_columns = $conn->query($sql_columns);
// создаем массив для хранения имен столбцов
$columns = array();
if ($result_columns->num_rows > 0) {
    while($row = $result_columns->fetch_assoc()) {
        $columns[] = $row["Field"];
    }
}

$first_column = array_shift($columns);
// формируем список столбцов для выборки в SQL-запросе
$select_columns = implode(",", $columns);

// формируем список значений для вставки в SQL-запрос
$set_values = array();
foreach ($columns as $col) {
    $val = array_shift($values);
    $set_values[] = "`$col`='$val'";
}

$sql = "UPDATE `$TableName` SET " . implode(",", $set_values) . " WHERE `$first_column`='$first_values'";

$result = $conn->query($sql);

$conn->close();
?>
