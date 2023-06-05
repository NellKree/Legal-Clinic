<?php
require_once('../db_connection.php');

$TableName = $_POST["NewForm0"];

// Подготовленный запрос для выборки данных из таблицы
$stmt = $conn->prepare("SELECT * FROM `$TableName`");
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // Получаем имена столбцов таблицы
    $columns = $result->fetch_fields();
    $columnNames = array_column($columns, 'name');

    // Получаем все строки выборки в виде массива
    $rows = $result->fetch_all(MYSQLI_ASSOC);

    foreach ($rows as $row) {
        // Обработка каждой строки
        $output = "";
        foreach ($columnNames as $columnName) {
            $output .= $row[$columnName] . "!";
        }
        echo $output . "<br>/";
    }
} else {
    echo "0 results";
}

$stmt->close();
$conn->close();
?>