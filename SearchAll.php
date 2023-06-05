<?php
require_once('../db_connection.php');

$Surname = $_POST["NewForm0"];
$Name = $_POST["NewForm1"];
$TableName = $_POST["NewForm2"];

$sql = "SELECT * FROM `$TableName` WHERE `$Surname`='$Name'";
$result = $conn->query($sql);


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

// выводим данные для каждой строки таблицы
if ($result->num_rows > 0) {
    while($row = $result->fetch_assoc()) {
        // создаем пустую строку для хранения значений столбцов
        $output = "";

        // проходим по каждому столбцу и добавляем его значение в строку вывода
        foreach($columns as $column) {
            $output .= $row[$column] . "!";
        }
        // выводим строку значений столбцов
        echo $output . "<br>/";
    }
} else {
    echo "0 results";
}


$conn->close();
?>