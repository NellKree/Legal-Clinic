<?php
require_once('../db_connection.php');

$TableName = $_POST["TableName"];
$values = $_POST["values"];
// формируем список значений для вставки в SQL-запрос
$values_sql = "'" . implode("','", $values) . "'";

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

// формируем список столбцов для выборки в SQL-запросе
$select_columns = implode(",", $columns);



// формируем SQL-запрос
$sql = "INSERT INTO `$TableName` ($select_columns) VALUES ($values_sql)";


$result = $conn->query($sql);

$conn->close();
?>