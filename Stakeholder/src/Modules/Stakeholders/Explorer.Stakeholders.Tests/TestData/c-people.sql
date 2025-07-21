INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "ImageUrl", "Biography", "Motto", "Equipment", "Wallet", "XP", "Level")
VALUES 
(-11, -11, 'Ana', 'Anić', 'autor1@gmail.com', 'https1', 'KaoJa', 'Samo Jako Bro', ARRAY[1, 2]::integer[], 1000, 50, 3),
(-12, -12, 'Lena', 'Lenić', 'autor2@gmail.com', 'profilepic2.jpg', 'Lena loves to write about travel.', 'Explore the world.', ARRAY[1, 2]::integer[], 1000, 100, 4),
(-13, -13, 'Sara', 'Sarić', 'autor3@gmail.com', 'profilepic3.jpg', 'Sara is a creative writer.', 'Imagination is key.', ARRAY[1, 2]::integer[], 1000, 70, 3),
(-21, -21, 'Pera', 'Perić', 'turista1@gmail.com', 'profilepic4.jpg', 'Pera enjoys discovering new places.', 'Adventure awaits.', ARRAY[1, 2]::integer[], 1000, 30, 2),
(-22, -22, 'Mika', 'Mikić', 'turista2@gmail.com', 'profilepic5.jpg', 'Mika is an avid traveler.', 'Travel more, worry less.', ARRAY[1, 2]::integer[], 1000, 40, 2),
(-23, -23, 'Steva', 'Stević', 'turista3@gmail.com', 'profilepic6.jpg', 'Steva enjoys nature and hiking.', 'Nature is the best teacher.', ARRAY[1, 2]::integer[], 1000, 60, 3);
