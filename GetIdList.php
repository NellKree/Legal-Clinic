<?php
require_once('../db_connection.php');

$Table= $_POST["NewForm0"];
$NameC = $_POST["NewForm1"];

$sql = "SELECT `$NameC` FROM `$Table` ORDER BY `$NameC`";
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