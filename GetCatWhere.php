<?php
require_once('../db_connection.php');

$name = $_POST["NewForm0"];
$TableName = $_POST["NewForm1"];

// Получаем список столбцов таблицы
$sql_columns = "DESCRIBE $TableName";
$result_columns = $conn->query($sql_columns);

// Создаем массив для хранения имен столбцов
$columns = array();
if ($result_columns->num_rows > 0) {
    while($row = $result_columns->fetch_assoc()) {
        $columns[] = $row["Field"];
    }
}

// Формируем подготовленный SQL-запрос
$sql = "SELECT " . $columns[1] . " FROM " . $TableName . " WHERE " . $columns[0] . " = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $name);
$stmt->execute();
$result = $stmt->get_result();

// Обработка результатов
if ($result->num_rows > 0) {
    while($row = $result->fetch_assoc()) {
// Выводим значения второго столбца
        echo $row[$columns[1]] . "/";
    }
} else {
    echo "0 results";
}

$stmt->close();
$conn->close();


?>
