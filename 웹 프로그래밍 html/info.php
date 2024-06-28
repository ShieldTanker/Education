<?PHP
$db = mysqli_connect("localhost", "root", "1234", "madang");
if($db)
	echo "connect : success<br>";
else
	echo "connect : failure<br>";

?>