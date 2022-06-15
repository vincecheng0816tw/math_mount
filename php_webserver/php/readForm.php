<?php	
    define("MAX_X_INPUT", 20);
    define("MAX_Z_INPUT", 20);
    $csvfile = "maths_mountain.csv";
    $csvfile1 = "maths_mountain-1.csv";
    
	$row = 0;
    if (($handle = fopen($csvfile, "r")) !== FALSE) {
        while (($data = fgetcsv($handle, 1000, ",")) !== FALSE) {
            $num = count($data);
            #echo "<p> $num fields in line $row: <br /></p>\n";
            $row++;
            for ($c=0; ($c < $num) && ($c < MAX_X_INPUT) ; $c++) {
                if($data[$c] != null){
                    #$floatN = ($data[$c]);
                    $intN = ((int)$data[$c]);
                    if($intN>=0)
                    {
                	    $floatN =  dechex($intN);
                    }
                	else
                	{
                	    $floatN = substr(dechex($intN),-8);
                	}
                	if($c<(MAX_X_INPUT-1))
                	    echo $floatN . ",";
                	else{
                	    echo $floatN;
                	    break;
                	}
                    
                }
            }

            
            echo nl2br("*");
            #echo "<br />";
            if($row >= MAX_Z_INPUT)
                break;
        }
        echo " ";
        fclose($handle);
    }
?>