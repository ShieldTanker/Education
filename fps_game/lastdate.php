<?php
// login.php
$servername = "localhost";
$username = "root";
$password = "1234";
$dbname = "fps_game";

$user_username = $_POST["usernamePost"];

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn -> connect_error)
{
	echo "Failed to connect to MySQL : " + $mysqli -> coonect_error;
	exit();
}

$sql = "SELECT lastdate FROM user WHERE username = '".$user_username."' ";
$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0)
{
	if($row = mysqli_fetch_assoc($result))
	{
		echo $row['lastdate'];
	}
}
?>