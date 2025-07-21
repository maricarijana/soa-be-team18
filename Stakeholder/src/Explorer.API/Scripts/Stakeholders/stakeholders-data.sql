/****** STAKEHOLDERS MODULE ******/

DELETE FROM stakeholders."Notification";
DELETE FROM stakeholders."Problem";
DELETE FROM stakeholders."ClubJoinRequests";
DELETE FROM stakeholders."ClubInvitations";
DELETE FROM stakeholders."Clubs";
DELETE FROM stakeholders."AppReviews";
DELETE FROM stakeholders."People";
DELETE FROM stakeholders."Users";

/* Users */
INSERT INTO stakeholders."Users"("Id", "Username", "Password", "Role", "IsActive")
VALUES 
    (222, 'turista1', '12345678', 2, true),
    (223, 'turista2', '12345678', 2, true),
    (224, 'admin', '12345678', 0, true),
    (225, 'autor', '12345678', 1, true),
    (226, 'turista', '12345678', 2, true),
    (227, 'autor2', '12345678', 1, true);




/* People */

INSERT INTO stakeholders."People"(
    "Id", "UserId", "Name", "Surname", "Email", "ImageUrl", "Biography", "Motto", "Equipment", "Wallet", "XP", "Level")
    VALUES (222, 222, 'Turista', 'Prvi', 'turista@gmail.com', '', NULL, NULL, NULL, 0, 0, 1),
        (223, 223, 'Turista', 'Drugi', 'turista@gmail.com', '', NULL, NULL, NULL, 0, 0, 1),
         (226, 226, 'Turista', 'Treci', 'turista@gmail.com', '', NULL, NULL, NULL, 0, 0, 11);

/* AppReviews */


/* Clubs */




/* ClubInvitations */



/* ClubJoinRequests */




/* Problems */





/* Notifications */

