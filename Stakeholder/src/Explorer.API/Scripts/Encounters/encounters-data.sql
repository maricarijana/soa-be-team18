DELETE FROM encounter."Encounters";

INSERT INTO encounter."Encounters"(
    "Id", "Title", "Description", "Longitude", "Latitude", "XP", "Status", "Type", "RequestStatus", "SocialData_RequiredParticipants", "SocialData_Radius", "HiddenLocationData_ImageUrl", "HiddenLocationData_ActivationRadius", "MiscData_ActionDescription", "Instances", "IsRequired")
    VALUES 
    (222, 'Random Social Encounter', 'Description Social Encounter', 19.84221427641111, 45.25532506717675, 5, 0, 0, 2, 2, 1, NULL, NULL, NULL, '[]', False),
    (223, 'Sa KP', 'opis sa kp kod snp', 19.843038341977376, 45.25532506717675, 5, 0, 1, 2, NULL, NULL, NULL, NULL, 'Uradi 10 sklekova', '[]', False),
    (224, 'Bez KP', 'opis bez kp', 19.84221427641111, 45.25532506717675, 5, 0, 1, 2, NULL, NULL, NULL, NULL, 'Uradi stoj na rukama', '[]', False);