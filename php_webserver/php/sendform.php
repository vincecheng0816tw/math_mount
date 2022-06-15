<?php
define("MAX_X_INPUT", 20);
define("MAX_Z_INPUT", 20);
function debugecho($text) {
  //echo $text;
}
function initalArray() {
    for ($i=0; $i<MAX_Z_INPUT; $i++) {
        for($j=0; $j<MAX_X_INPUT; $j++) {
           $numbers[$i][$j]= 0;
        }
    }
    return $numbers;
}
function printfArray($array) {
    for ($i=0; $i<MAX_Z_INPUT; $i++) {
        for($j=0; $j<MAX_X_INPUT; $j++) {
           echo $array[$i][$j].", ";
        }
        echo "<br>";
    }
}
	$text1 = $_POST["command"];
	$text2 = $_POST["input"];
	$text3 = $_POST["x"];
	$text4 = $_POST["y"];
	$text5 = $_POST["function"];
	
	if($text1 != "") 
	{
		#echo"Message successfully sent!";
		#echo"Field 1: $text1";
		#echo"Field 2:  $text2";
		#echo"Field 3:  $text3";
		#echo"Field 4:  $text4";
		#echo"Field 5:  $text5";
		$input = $text2;
		if($input ==0){
		    echo "Number is 0...No change";
		    return;
		}
	}
	else
	{
		echo "Message delivery failed...";
		return;
	}
	
    #To Read CSV File
    $csvfile = "maths_mountain.csv";
    $csvfile1 = "maths_mountain-1.csv";
    $numbersArr = initalArray();
    
    $row = 0;
    if (($handle = fopen($csvfile, "r")) !== FALSE) {
        while (($data = fgetcsv($handle, 1000, ",")) !== FALSE) {
            $num = count($data);
            #echo "<p> $num fields in line $row: <br /></p>\n";
            $col = 0;
            for ($c=0; $c < $num; $c++) {
                if($data[$c] != null){
                    $floatN = ($data[$c]);
                    $numbersArr[$row][$col] = $floatN ;
                	#if($c<($num-2))
                	#    debugecho ("".$floatN . ",");
                	#else
                	#    debugecho ("".$floatN);
                    $col++;    
                }
            }
            #debugecho(" <br>");
            $row++;
        }
        #echo " ";
        fclose($handle);
    }
    $numbersArr = math_mountain_algorithm($numbersArr, $input,$text3,$text4,$text5 );

    #echo "Read from csv:<br>";
    //printfArray($numbersArr);
    
    #To Write CSV File
    
    $file = fopen($csvfile,"w");
    foreach ($numbersArr as $fields) {
        fputcsv($file, $fields);
    }
    fclose($file);
    echo "update done...";
    
function math_mountain_algorithm($nArr, $num, $indx, $indz, $operation)
{
    #$nArr[0][0] = 0;
    $smallFloatB = 100000;
    $original = $nArr[$indx][$indz] ;
    $num2 = $num;
    
    if ($num> $smallFloatB)
        $num2 = ((int)$num %(int)$smallFloatB);
    if ($num< -$smallFloatB)
        $num2 = ((int)$num %(int)$smallFloatB);    
        
    //algorithm: 1. decide a location (i, j) (row, column)
    //           2. generate a new number depend on input value and its neighbours 
    // Random location
    
    $operation = ((int)$operation) % 4;
    $noise = (float)(sin($indz*$indx))*0.01;  // 0.00~0.09
    
    #echo "<br> Debug:math_mountain_algorithm: input: ". $num." num2: ".$num2." z: ".$indz." x: ".$indx." op: ". $operation ."<br>";
    /* 8 neighbours:
     * [z-1, x-1], [z-1, x], [z-1, x+1]
     * [z, x-1],   [z, x],   [z, x+1]
     * [z+1, x-1], [z+1, x], [z+1, x+1]
    */

    // Math Mathod

    
    
    if ($operation == 0)
    {
        // 1. addition:
        
        $nArr[$indz][$indx] = $original + $noise + $num2;
            // neighbours
        if(($indx!=0)&&($indz!=0)){    
            $nArr[$indz - 1][$indx - 1] = $nArr[$indz - 1][$indx - 1] + ($noise * $num2 *0.2);
            $nArr[$indz - 1][$indx] = $nArr[$indz - 1][$indx] + ($noise * $num2 * 0.3);
            $nArr[$indz - 1][ $indx + 1] = $nArr[$indz - 1][$indx + 1] + ($noise * $num2 * 0.2);
            $nArr[$indz][$indx - 1] = $nArr[$indz][$indx - 1] + ($noise * $num2 * 0.3);
            $nArr[$indz][$indx + 1] = $nArr[$indz][$indx + 1] + ($noise * $num2 * 0.3);
            $nArr[$indz + 1][$indx - 1] = $nArr[$indz + 1][$indx - 1] + ($noise * $num2 * 0.2);
            $nArr[$indz + 1][$indx] = $nArr[$indz + 1][$indx] + ($noise * $num2 * 0.3);
            $nArr[$indz + 1][$indx + 1] = $nArr[$indz + 1][$indx + 1] + ($noise * $num2 * 0.2);
        }
    }
    else if ($operation == 1)
    {
        // 2. substraction
        $nArr[$indz][$indx] = $original - $noise - $num2;
        // neighbours
        if(($indx!=0)&&($indz!=0)){
            $nArr[$indz - 1][$indx - 1] = $nArr[$indz - 1][$indx - 1] - ($noise * $num2 *0.2);
            $nArr[$indz - 1][$indx] = $nArr[$indz - 1][$indx] - ($noise * $num2 * 0.3);
            $nArr[$indz - 1][ $indx + 1] = $nArr[$indz - 1][$indx + 1] - ($noise * $num2 * 0.2);
            $nArr[$indz][$indx - 1] = $nArr[$indz][$indx - 1] - ($noise * $num2 * 0.3);
            $nArr[$indz][$indx + 1] = $nArr[$indz][$indx + 1] - ($noise * $num2 * 0.3);
            $nArr[$indz + 1][$indx - 1] = $nArr[$indz + 1][$indx - 1] - ($noise * $num2 * 0.2);
            $nArr[$indz + 1][$indx] = $nArr[$indz + 1][$indx] - ($noise * $num2 * 0.3);
            $nArr[$indz + 1][$indx + 1] = $nArr[$indz + 1][$indx + 1] - ($noise * $num2 * 0.2);
        }
    }
    else if ($operation == 2)
    {
        // 3. multiple

        
    }
    else if ($operation == 3)
    {

        // 4. division (average)
        if(($indx!=0)&&($indz!=0)){
            $average = (($nArr[$indz][$indx] + $nArr[$indz - 1][$indx - 1] + $nArr[$indz - 1][$indx] + $nArr[$indz - 1][ $indx + 1] + $nArr[$indz][$indx - 1] + $nArr[$indz][$indx + 1] +  $nArr[$indz + 1][$indx - 1] + $nArr[$indz + 1][$indx] + $nArr[$indz + 1][$indx + 1])/9);
            if($nArr[$indz][$indx] > $average )
                $nArr[$indz][$indx] = $original - $noise - $num2;
            else
                $nArr[$indz][$indx] = $original + $noise + $num2;
            // neighbours
            if($nArr[$indz - 1][$indx - 1] > $average)
                $nArr[$indz - 1][$indx - 1] = $nArr[$indz - 1][$indx - 1] - ($noise * $num2 *0.5);
            else
                $nArr[$indz - 1][$indx - 1] = $nArr[$indz - 1][$indx - 1] + ($noise * $num2 *0.5);
            if($nArr[$indz - 1][$indx] > $average)
                $nArr[$indz - 1][$indx] = $nArr[$indz - 1][$indx] - ($noise * $num2 * 0.5);
            else
                $nArr[$indz - 1][$indx] = $nArr[$indz - 1][$indx] + ($noise * $num2 * 0.5);
            if($nArr[$indz - 1][ $indx + 1]>$average)
                $nArr[$indz - 1][ $indx + 1] = $nArr[$indz - 1][$indx + 1] - ($noise * $num2 * 0.5);
            else
                $nArr[$indz - 1][ $indx + 1] = $nArr[$indz - 1][$indx + 1] + ($noise * $num2 * 0.5);
            
            if($nArr[$indz][$indx - 1]>$average)
                $nArr[$indz][$indx - 1] = $nArr[$indz][$indx - 1] - ($noise * $num2 * 0.5);
            else
                $nArr[$indz][$indx - 1] = $nArr[$indz][$indx - 1] + ($noise * $num2 * 0.5);
            if($nArr[$indz][$indx + 1]>$average)
                $nArr[$indz][$indx + 1] = $nArr[$indz][$indx + 1] - ($noise * $num2 * 0.5);
            else
                $nArr[$indz][$indx + 1] = $nArr[$indz][$indx + 1] + ($noise * $num2 * 0.5);
            if($nArr[$indz + 1][$indx - 1]>$average)
                $nArr[$indz + 1][$indx - 1] = $nArr[$indz + 1][$indx - 1] - ($noise * $num2 * 0.5);
            else
                $nArr[$indz + 1][$indx - 1] = $nArr[$indz + 1][$indx - 1] + ($noise * $num2 * 0.5);
            if($nArr[$indz + 1][$indx]>$average)
                $nArr[$indz + 1][$indx] = $nArr[$indz + 1][$indx] - ($noise * $num2 * 0.5);
            else
                $nArr[$indz + 1][$indx] = $nArr[$indz + 1][$indx] + ($noise * $num2 * 0.5);
            if($nArr[$indz + 1][$indx + 1]>$average)
                $nArr[$indz + 1][$indx + 1] = $nArr[$indz + 1][$indx + 1] - ($noise * $num2 * 0.5);
            else
                $nArr[$indz + 1][$indx + 1] = $nArr[$indz + 1][$indx + 1] + ($noise * $num2 * 0.5);
        }
        else
            $nArr[$indz][$indx] = $original/$num2 + $noise;
    }
    else {
        debugecho("algorithm failed: unknown opertion". $operation); 
    }
    
    debugecho("<br>org: ".$original." noise: ".$noise."new: ".$nArr[$indz][$indx]."<br>");
    
    return $nArr; 
}

?>