<?php
require_once('../db_connection.php');

$idc = $_POST["NewForm0"];


$sql = "SELECT 
    (SELECT Surname FROM consultants WHERE IDConsultant = '$idc') AS ConsultantSurname,
    (SELECT Name FROM consultants WHERE IDConsultant = '$idc') AS ConsultantName,
    (SELECT Patronymic FROM consultants WHERE IDConsultant = '$idc') AS ConsultantPatronymic,
    (SELECT PhoneNumber FROM consultants WHERE IDConsultant = '$idc') AS ConsultantPhoneNumber,
    (SELECT Mail FROM consultants WHERE IDConsultant = '$idc') AS ConsultantMail,
    (SELECT StudyGroup FROM consultants WHERE IDConsultant ='$idc') AS ConsultantStudyGroup,
    (SELECT COUNT(*) FROM LegalIssues WHERE IDConsultant = '$idc') AS TotalIssues,
    (SELECT COUNT(*) FROM LegalIssues WHERE IDConsultant = '$idc' AND CaseStatus != 'Закрыто') AS ActiveIssues,
    (SELECT COUNT(*) FROM LegalIssues WHERE IDConsultant = '$idc'AND CaseStatus = 'Закрыто') AS CloseIssues,
    (SELECT COUNT(*) FROM Consultations JOIN LegalIssues ON Consultations.IDLegalIssues = 		LegalIssues.IDLegalIssues WHERE LegalIssues.IDConsultant = '$idc') AS CountConsultations;";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
        echo "" .$row["ConsultantSurname"]."/".$row["ConsultantName"]."/".$row["ConsultantPatronymic"]."/".$row["ConsultantPhoneNumber"]."/".$row["ConsultantMail"]."/".$row["ConsultantStudyGroup"]."/".$row["TotalIssues"]."/".$row["ActiveIssues"]."/".$row["CloseIssues"]."/".$row["CountConsultations"]."/";
    }
} else {
    echo "0 results";
}
$conn->close();
?>