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
$form = $_POST["NewForm1"];
$date = $_POST["NewForm2"];
$time1= $_POST["NewForm3"];
$time2 = $_POST["NewForm4"];

$sql = "INSERT INTO `consultations`( `IDLegalIssues`, `MeetingFormat`, `DateOfConsultation`, `ConsultationStartTime`, `EndTimeOfTheConsultation`) VALUES ('$id','$form','$date','$time1','$time2')";
#$sql = "INSERT INTO `statements`( `IDCustomer`, `ApplicationDate`, `DescriptionOfTheProblem`) VALUES ('" . $id."','" . $date."','" . $problem."')";


$result = $conn->query($sql);


$conn->close();
?>