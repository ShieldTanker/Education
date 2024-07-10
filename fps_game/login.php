<?php
// login.php
$servername = "localhost";
$username = "root";
$password = "1234";
$dbname = "fps_game";

$user_username = $_POST["usernamePost"];
$user_password = $_POST["passwordPost"];
$user_date = $_POST["datePost"];

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn -> connect_error)
{
	echo "Failed to connect to MySQL : " + $mysqli -> coonect_error;
	exit();
}

$sql = "SELECT password FROM user WHERE username = '".$user_username."' ";
$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0)
{
	if($row = mysqli_fetch_assoc($result))
	{
		if ($row['password'] == get_enc_str($conn, $user_password))
		{	
			// 마지막 접속시간 저장
			$sql = "UPDATE user
					SET lastdate = date
					WHERE username = '".$user_username."'";
			$result = mysqli_query($conn, $sql);
			
			$sql = "UPDATE user 
					SET    date = '".$user_date."' 
					WHERE  username = (SELECT username 
									   FROM user 
									   WHERE username = '".$user_username."')";
			$result = mysqli_query($conn, $sql);
			echo "login success";
		}
		else
			echo "password incorrect";
	}
}
else
{
	echo "user not found";
}
return;	

function get_enc_str($conn, $str)
{
	// 결과를 enc_str 라는 이름으로 받는 쿼리문 작성
	$sql = "SELECT PASSWORD('".$str."') AS 'enc_str'";
	
	// 쿼리문을 $conn 이 접속한 데이터베이스 에서 실행
	$result = mysqli_query($conn, $sql);
	// 쿼리문 의 조건에 맞는 레코드 를 저장
	$row = mysqli_fetch_assoc($result);
	
	// 레코드 의 변환된 문자열을 리턴
	return $row['enc_str'];
}
?>