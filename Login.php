<?php
require_once('../db_connection.php');

$TableName = $_POST["TableName"];
$values = $_POST["values"];


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

// Проверяем, что массив $values содержит как минимум два элемента
if (count($values) >= 2)
{
    $email = $values[0];     // Первый элемент массива
    $password = $values[1];    // Второй элемент массива

    // Подготавливаем SQL-запрос с использованием подготовленных выражений для предотвращения атак SQL-инъекций
    $stmt = $conn->prepare("SELECT * FROM `$TableName` WHERE Mail = ? AND Password = ?");
    $stmt->bind_param("ss", $email, $password);
    $stmt->execute();

    // Получаем результаты запроса в виде ассоциативного массива
    $result = $stmt->get_result();

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
        // Пользователь не найден или неправильные данные
        echo "Authentication Failed";
    }

    $stmt->close();
}

$conn->close();
?>
