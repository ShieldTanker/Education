<?php
// user_list.php
$servername = "localhost";
$username = "root";
$password = "1234";
$dbname = "fps_game";

$conn = new mysqli ($servername, $username, $password, $dbname);

if($conn -> connect_errno)
{
	echo "Failed to connect to MySQL : " + Smysqli -> connect_error;
	exit();
}

$sql = "SELECT id, username FROM user";
$result = mysqli_query($conn, $sql);

if(mysqli_num_rows($result) > 0)
{
	$users = array();
	while ($row = mysqli_fetch_assoc($result))
	{
		$users[] = array(
			'id' => $row['id'], 
			'username' => $row['username']);
	}
	
	$users_json_string = json_encode($users);
	echo $users_json_string;
}
else
{
	echo "no user";
}
?>