/****** TOURS MODULE ******/

DELETE FROM tours."TourExecution";
DELETE FROM tours."TourReview";
DELETE FROM tours."KeyPoints";
DELETE FROM tours."TourPreferences";
DELETE FROM tours."Tour";
DELETE FROM tours."Positions";
DELETE FROM tours."Equipment";
DELETE FROM tours."Objects";

/* Objects */

/* Equipment */


/* Positions */

INSERT INTO tours."Positions"(
    "Id", "Latitude", "Longitude", "TouristId")
VALUES 
(111, 45.244648392133605, 19.847638305710497, 222),
(112, 45.244648392133605, 19.847638305710497, 223);

/* Tour */

INSERT INTO tours."Tour" ("Id", 
    "Name", "Description", "Difficulty", "Tags", "Status", "Price", "UserId", "EquipmentIds", "LengthInKm", "PublishedTime", "ArchiveTime"
)
VALUES 
    (222, 
    'Mountain Adventure', 
    'A thrilling hike through the scenic mountain trails.', 
    'Moderate', 
    ARRAY[2, 4, 8]::integer[],  
    1, 
    50.00, 
    225,    --ovde treba da bude id autora ture 
    ARRAY[]::integer[],  
    15.5, 
    '2024-01-15 10:00:00', 
    NULL
    );
INSERT INTO tours."Tour" ("Id", 
    "Name", "Description", "Difficulty", "Tags", "Status", "Price", "UserId", "EquipmentIds", "LengthInKm", "PublishedTime", "ArchiveTime"
)
VALUES 
    (223, 
    'Desert Adventure', 
    'A thrilling hike through the desert.', 
    'Hard', 
    ARRAY[2, 4, 8]::integer[],  
    1, 
    70.00, 
    225,    --ovde treba da bude id autora ture 
    ARRAY[]::integer[],  
    15.5, 
    '2024-01-15 10:00:00', 
    NULL
    );

INSERT INTO tours."Tour" ("Id", 
    "Name", "Description", "Difficulty", "Tags", "Status", "Price", "UserId", "EquipmentIds", "LengthInKm", "PublishedTime", "ArchiveTime"
)
VALUES 
     (224, 
    'Forest Adventure', 
    'A thrilling hike through the forest.', 
    'Hard', 
    ARRAY[2, 4, 8]::integer[],  
    1, 
    40.00, 
    225,    --ovde treba da bude id autora ture 
    ARRAY[]::integer[],  
    15.5, 
    '2024-01-15 10:00:00', 
    NULL
    );

INSERT INTO tours."Tour" ("Id", 
    "Name", "Description", "Difficulty", "Tags", "Status", "Price", "UserId", "EquipmentIds", "LengthInKm", "PublishedTime", "ArchiveTime"
)
VALUES 
     (225, 
    'Forest Adventure 2', 
    'A thrilling hike through the forest.', 
    'Hard', 
    ARRAY[2, 4, 8]::integer[],  
    1, 
    40.00, 
    227,    --ovde treba da bude id autora ture 
    ARRAY[]::integer[],  
    15.5, 
    '2024-01-15 10:00:00', 
    NULL
    );

INSERT INTO tours."Tour" ("Id", 
    "Name", "Description", "Difficulty", "Tags", "Status", "Price", "UserId", "EquipmentIds", "LengthInKm", "PublishedTime", "ArchiveTime"
)
VALUES 
    (226, 
    'Desert Adventure 2', 
    'A thrilling hike through the desert.', 
    'Hard', 
    ARRAY[2, 4, 8]::integer[],  
    1, 
    70.00, 
    227,    --ovde treba da bude id autora ture 
    ARRAY[]::integer[],  
    15.5, 
    '2024-01-15 10:00:00', 
    NULL
    );


/* TourPreferences */



/* KeyPoints */

INSERT INTO tours."KeyPoints" ("Id", "Name", "Longitude", "Latitude", "Description", "Image", "PublicStatus", "UserId", "TourId") VALUES
(111, 'Random KP', 19.84494832522442, 45.255041845831364, 'Random Keypoint', 'image1.jpg', 0, 60, 222),
(112, 'Random KP', 19.84494832522442, 45.3, 'Random Keypoint', 'image1.jpg', 0, 60, 222),
(113, 'Random KP', 19.84494832522442, 45.255041845831364, 'Random Keypoint', 'image1.jpg', 0, 60, 223),
(114, 'Random KP', 19.84494832522442, 45.3, 'Random Keypoint', 'image1.jpg', 0, 60, 223),
(115, 'Random KP', 19.84494832522442, 45.255041845831364, 'Random Keypoint', 'image1.jpg', 0, 60, 224),
(116, 'Random KP', 19.84494832522442, 45.3, 'Random Keypoint', 'image1.jpg', 0, 60, 224),
(117, 'Random KP', 19.84494832522442, 45.255041845831364, 'Random Keypoint', 'image1.jpg', 0, 60, 225),
(118, 'Random KP', 19.84494832522442, 45.3, 'Random Keypoint', 'image1.jpg', 0, 60, 225),
(119, 'Random KP', 19.84494832522442, 45.255041845831364, 'Random Keypoint', 'image1.jpg', 0, 60, 226),
(120, 'Random KP', 19.84494832522442, 45.3, 'Random Keypoint', 'image1.jpg', 0, 60, 226);


/* PublicKeypointRequests */



/* TourReviews */



/* TourExecutions */
INSERT INTO tours."TourExecution" ("Id", "TourId", "TouristId", "LocationId", "LastActivity", "Status", "CompletedKeys") VALUES
(111, 222, 222, 111, '2024-11-03 10:00:00', 0, '[]'),
(112, 222, 223, 112, '2024-11-03 10:00:00', 0, '[]');


