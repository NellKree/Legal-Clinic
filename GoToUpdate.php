<?php
require_once('../db_connection.php');

$id = $_POST["NewForm0"];
$TableName = $_POST["NewForm1"];

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

// сохраняем первый элемент массива, который содержит имя столбца для условия WHERE
$first_column = array_shift($columns);

// формируем список столбцов для выборки в SQL-запросе
$select_columns = implode(",", $columns);

$sql = "SELECT $select_columns FROM `$TableName` WHERE `$first_column`= '". $id."'";


$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        // формируем строку с данными из выбранных столбцов
        $data = "";
        foreach ($columns as $column) {
            $data .= $row[$column] . "/";
        }
        echo $data;
    }
} else {
    echo "0 results";
}
$conn->close();

?>