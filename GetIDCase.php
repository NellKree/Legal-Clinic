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

$sql = "SELECT IDLegalIssues FROM `legalissues` ORDER BY `IDLegalIssues`";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        echo "" . $row["IDLegalIssues"]."/";
    }
} else {
    echo "0 results";
}
$conn->close();
?>