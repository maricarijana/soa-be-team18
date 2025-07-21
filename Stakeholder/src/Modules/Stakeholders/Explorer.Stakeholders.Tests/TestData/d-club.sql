INSERT INTO stakeholders."Clubs"(
    "Id", "Name", "Description", "Image", "UserId", "UserIds","Tags")
VALUES 
(-1, 'Mountaineering Society of Vojvodina', 'A group for everyone who wants to try new activities', 'image', -1,ARRAY[21, 23]::integer[],ARRAY[2, 3, 5]::integer[]);


INSERT INTO stakeholders."Clubs"(
    "Id", "Name", "Description", "Image", "UserId", "UserIds","Tags")
VALUES 
(-2, 'Cycling Across Austria', 'A group for experienced cyclists, ready for long routes through beautiful Austria', 'image2', -11,ARRAY[21, 23]::integer[], ARRAY[2, 3, 5]::integer[] );