<?php

require_once('db_connection.php');

$sql = "SELECT COUNT(*) FROM LegalIssues WHERE CaseStatus != 'Закрыто'";

$result = mysqli_query($conn, $sql);
$count_items_return = mysqli_fetch_all($result, 1);

echo $count_items_return[0]['COUNT(*)'];

$conn->close();
?>