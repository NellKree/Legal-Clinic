<?php
require_once('../db_connection.php');

$name = $_POST["NewForm0"];
$TableName = $_POST["NewForm1"];

// Получаем список столбцов таблицы
$sql_columns = "DESCRIBE `$TableName`";
$result_columns = $conn->query($sql_columns);

// Создаем массив для хранения имен столбцов
$columns = array();
if ($result_columns->num_rows > 0) {
    while($row = $result_columns->fetch_assoc()) {
        $columns[] = $row["Field"];
    }
}

// Формируем подготовленный SQL-запрос
$sql = "SELECT " . $columns[0] . " FROM " . $TableName . " WHERE " . $columns[1] . " = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $name);
$stmt->execute();
$result = $stmt->get_result();

// Обработка результатов
if ($result->num_rows > 0) {
    while($row = $result->fetch_assoc()) {
        // Выводим значения первого столбца
        echo $row[$columns[0]] . "/";
    }
} else {
    echo "10";
}

$stmt->close();
$conn->close();


?>
