<?php
	$text1 = $_POST["data"];
		
	if($text1 != "")
	{
		echo("Message successfully sent!");
		echo("Field 1" . $text1);
		$file = fopen("data.txt", "a");
		fwrite($file, $text1);
		fclose($file);
	} else
	{
		$file = fopen("data.txt", "a");
		fwrite($file, "fail");
		fclose($file);
		echo("Message delivery failed...");
	}
?>