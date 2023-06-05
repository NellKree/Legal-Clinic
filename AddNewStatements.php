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

$id = $_POST["NewForm0"];
$date = $_POST["NewForm1"];
$problem = $_POST["NewForm2"];


$sql = "INSERT INTO `statements`( `IDCustomer`, `ApplicationDate`, `DescriptionOfTheProblem`) VALUES ('" . $id."','" . $date."','" . $problem."')";


$result = $conn->query($sql);


$conn->close();
?>