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
$Date = $_POST["NewForm7"];
$Problem = $_POST["NewForm8"];

$sql = "INSERT INTO `customer`( `Surname`, `Name`, `Patronymic`, `PhoneNumber`, `Mail`, `DateOfBirth`, `IDCategory`) VALUES ('$Surname','$Name','$Patronymic','$PhoneNumber','$Mail','$DateOfBirth','$IDCategory')";

$result = $conn->query($sql);

$sql2 = "SELECT `IDCustomer` FROM `customer` WHERE `Surname` = '$Surname' AND`Name`='$Name' AND `Patronymic`='$Patronymic' AND `PhoneNumber`='$PhoneNumber' AND `Mail`='$Mail'";

$result2 = mysqli_query($conn, $sql2);
$count_items_return = mysqli_fetch_all($result2, 1);


$id =  $count_items_return[0]['IDCustomer'];

$sql = "INSERT INTO `statements`( `IDCustomer`, `ApplicationDate`, `DescriptionOfTheProblem`) VALUES ('" . $id."','" . $Date."','" . $Problem."')";


$result = $conn->query($sql);

$conn->close();
?>