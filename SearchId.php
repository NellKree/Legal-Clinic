<?php
require_once('../db_connection.php');

$Surname = $_POST["NewForm0"];
$Name = $_POST["NewForm1"];
$Table= $_POST["NewForm2"];
$NameC = $_POST["NewForm3"];


$sql = "SELECT `$NameC` FROM `$Table` WHERE `$Surname`='$Name' ORDER BY `$NameC`";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        echo "" . $row["$NameC"]."/";
    }
} else {
    echo "0 results";
}
$conn->close();
?>