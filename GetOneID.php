<?php

$servername = "localhost";
$user = "root";
$password = "";
$dbname = "legalclinic";

// Create connection
$conn = new mysqli($servername, $user, $password, $dbname);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$Surname = $_POST["NewForm0"];
$Name = $_POST["NewForm1"];
$Patronymic = $_POST["NewForm2"];
$PhoneNumber = $_POST["NewForm3"];
$Mail = $_POST["NewForm4"];

$sql = "SELECT `IDCustomer` FROM `customer` WHERE `Surname` = '$Surname' AND`Name`='$Name' AND `Patronymic`='$Patronymic' AND `PhoneNumber`='$PhoneNumber' AND `Mail`='$Mail'";

$result = mysqli_query($conn, $sql);
$count_items_return = mysqli_fetch_all($result, 1);

echo $count_items_return[0]['IDCustomer'];

$conn->close();
?>