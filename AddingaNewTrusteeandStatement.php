<?php
require_once('../db_connection.php');

$Surname = $_POST["NewForm0"];
$Name = $_POST["NewForm1"];
$Patronymic = $_POST["NewForm2"];
$PhoneNumber = $_POST["NewForm3"];
$Mail = $_POST["NewForm4"];
$DateOfBirth = $_POST["NewForm5"];
$IDCategory = $_POST["NewForm6"];
$Date = $_POST["NewForm7"];
$Problem = $_POST["NewForm8"];




// Выполните запрос, чтобы получить названия столбцов таблицы customer
$queryCustomer = "SHOW COLUMNS FROM customer";

$resultCustomer = $conn->query($queryCustomer);

// Выполните запрос, чтобы получить названия столбцов таблицы statements
$queryStatements = "SHOW COLUMNS FROM statements";

$resultStatements = $conn->query($queryStatements);

if ($resultCustomer && $resultStatements) {
    // Создайте пустой массив для хранения названий столбцов
    $columnNamesCustomer = array();
    $columnNamesStatements = array();

    // Извлеките названия столбцов из результата запроса для таблицы customer
    while ($row = $resultCustomer->fetch_assoc()) {
        $columnNamesCustomer[] = $row['Field'];
    }

    // Извлеките названия столбцов из результата запроса для таблицы statements
    while ($row = $resultStatements->fetch_assoc()) {
        $columnNamesStatements[] = $row['Field'];
    }

    // Запомните название первого столбца таблицы customer и удалите его из массива
    $firstColumnNameCustomer = $columnNamesCustomer[0];
    array_shift($columnNamesCustomer);

    // Теперь у вас есть массив $columnNamesCustomer, содержащий остальные названия столбцов таблицы customer
    // Можете использовать этот массив для добавления новой строки в таблицу customer

    // Здесь пример кода, который использует первые 7 значений из массива $_POST
    // для добавления новой строки в таблицу customer

    $values = array();

    // Извлеките первые 7 значений из массива $_POST
    for ($i = 0; $i < 7; $i++) {
        $values[] = $conn->real_escape_string($_POST["NewForm" . $i]);
    }

    // Создайте SQL-запрос для вставки новой строки в таблицу customer
    $sqlCustomer = "INSERT INTO customer (" . implode(", ", $columnNamesCustomer) . ")
                    VALUES ('" . implode("', '", $values) . "')";

    // Выполните запрос на вставку в таблицу customer
    if ($conn->query($sqlCustomer) === TRUE) {
        // Создайте ассоциативный массив, соединяя названия столбцов и значения
        $columnValuePairs = array_combine($columnNamesCustomer, $values);

        // Создайте список условий для SQL-запроса
        $conditions = array();
        foreach ($columnValuePairs as $column => $value) {
            $conditions[] = "$column = '$value'";
        }

        // Создайте динамический SQL-запрос для выборки значения первого столбца
        $selectFirstColumnValueQuery = "SELECT $firstColumnNameCustomer FROM customer WHERE " . implode(" AND ", $conditions);

        // Выполните запрос и получите результат
        $resultCustomer = $conn->query($selectFirstColumnValueQuery);

        if ($resultCustomer && $row = $resultCustomer->fetch_assoc())
        {
            $firstColumnValueCustomer = $row[$firstColumnNameCustomer];
            echo "Значение первого столбца у только что созданной строки в таблице customer: " . $firstColumnValueCustomer;
        }
        else
        {
            echo "Ошибка при получении значения первого столбца таблицы customer: " . $conn->error;
        }
    }
    else
    {
        echo "Ошибка при добавлении новой строки в таблицу customer: " . $conn->error;
    }

    array_shift($columnNamesStatements);

    // Создайте SQL-запрос для вставки новой строки в таблицу statements
    $sqlStatements = "INSERT INTO statements (" . implode(", ", $columnNamesStatements) . ")
                      VALUES ('" . $firstColumnValueCustomer."','" . $Date."','" . $Problem."')";

    // Выполните запрос на вставку в таблицу statements
    if ($conn->query($sqlStatements) === TRUE)
    {

    }
    else
    {
        echo "Ошибка при добавлении новой строки в таблицу statements: " . $conn->error;
    }
}
else
{
    echo "Ошибка при получении названий столбцов: " . $conn->error;
}

$conn->close();
?>