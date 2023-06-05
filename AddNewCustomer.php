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
$DateOfBirth = $_POST["NewForm5"];
$IDCategory = $_POST["NewForm6"];

$sql = "INSERT INTO `customer`( `Surname`, `Name`, `Patronymic`, `PhoneNumber`, `Mail`, `DateOfBirth`, `IDCategory`) VALUES ('$Surname','$Name','$Patronymic','$PhoneNumber','$Mail','$DateOfBirth','$IDCategory')";

$result = $conn->query($sql);


$conn->close();
?>