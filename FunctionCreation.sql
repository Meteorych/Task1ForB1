CREATE or REPLACE FUNCTION calculate_sum_and_median()
RETURNS TABLE(sum_of_integers numeric, median_of_floats FLOAT)
AS $$
DECLARE 
	summ_of_int numeric;
	median FLOAT;
BEGIN 
	-- Calculate the median of float values
    SELECT PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY replace(value5, ',', '.')::numeric) INTO median
    FROM table_strings;

    -- Return sum of integers and median of floats
    RETURN QUERY SELECT SUM(value4::numeric) AS summ_of_int, median FROM table_strings;
END;
$$ LANGUAGE plpgsql;