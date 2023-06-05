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
echo "Connection successfully, now we will show the users! <br>";


$sql = "SELECT Surname, Name FROM Customer";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        echo "Surname: " . $row["Surname"]. " - Name: " . $row["Name"]. "<br>";
    }
} else {
    echo "0 results";
}
$conn->close();
?>