<?php
// join.php
$servername = "localhost";
$username = "root";
$password = "1234";
$dbname = "fps_game";

$user_username = $_POST["usernamePost"];
$user_password = $_POST["passwordPost"];

$conn = new mysqli ($servername, $username, $password, $dbname);

if($conn -> connect_errno)
{
	echo "Failed to connect to MySQL : " + Smysqli -> connect_error;
	exit();
}

$sql = "SELECT id FROM user WHERE username = '".$user_username."' ";
$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0)
{
	echo "already exist";
	return;
}

$sql = "INSERT INTO user(username, password) 
		VALUES ('".$user_username."', PASSWORD('".$user_password."'))";

$result = mysqli_query($conn, $sql);

if ($result === true)
{
echo "success";
}
else
{
	echo "fail";
}
?>