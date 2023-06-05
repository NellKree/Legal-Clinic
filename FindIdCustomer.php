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



if ($resultCustomer )
{
    // Создайте пустой массив для хранения названий столбцов
    $columnNamesCustomer = array();


    // Извлеките названия столбцов из результата запроса для таблицы customer
    while ($row = $resultCustomer->fetch_assoc()) {
        $columnNamesCustomer[] = $row['Field'];
    }
    // Запомните название первого столбца таблицы customer и удалите его из массива
    $firstColumnNameCustomer = $columnNamesCustomer[0];
    array_shift($columnNamesCustomer);

    $values = array();

    // Извлеките первые 7 значений из массива $_POST
    for ($i = 0; $i < 7; $i++) {
        $values[] = $conn->real_escape_string($_POST["NewForm" . $i]);
    }
    // Создайте ассоциативный массив, соединяя названия столбцов и значения
    $columnValuePairs = array_combine($columnNamesCustomer, $values);

    // Создайте список условий для SQL-запроса
    $conditions = array();
    foreach ($columnValuePairs as $column => $value)
    {
        $conditions[] = "$column = '$value'";
    }

    // Создайте динамический SQL-запрос для выборки значения первого столбца
    $selectFirstColumnValueQuery = "SELECT $firstColumnNameCustomer FROM customer WHERE " . implode(" AND ", $conditions);

    // Выполните запрос и получите результат
    $resultCustomer = $conn->query($selectFirstColumnValueQuery);

    if ($resultCustomer && $row = $resultCustomer->fetch_assoc())
    {
        $firstColumnValueCustomer = $row[$firstColumnNameCustomer];
        echo  $firstColumnValueCustomer."/";
    } else
    {
        echo "Error" . $conn->error;
    }
}

$conn->close();
?>