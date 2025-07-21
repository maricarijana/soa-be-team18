INSERT INTO stakeholders."Problem"(
    "Id", "UserId", "TourId", "Category", "Description", "Priority", "Time","IsActive","Deadline","Comments")
	VALUES (-2, 2, 2, 'promena dana', 'pomeranje termina sa 3.10 na 4.10', 7, '2024-10-16 12:00:00', true,0,'[]'::jsonb);

INSERT INTO stakeholders."Problem"(
    "Id", "UserId", "TourId", "Category", "Description", "Priority", "Time", "IsActive","Deadline","Comments")
	VALUES (-3, 2, 2, 'promena satnice', 'odlaganje pokreta za 2 sata', 2, '2024-11-16 15:00:00', true,4,'[]'::jsonb);

INSERT INTO stakeholders."Problem"(
    "Id", "UserId", "TourId", "Category", "Description", "Priority", "Time","IsActive","Deadline","Comments")
	VALUES (-4, 1, 1, 'promena trajanja ture', 'skratiti turu za 1 dan', 9, '2024-12-16 11:00:00', true,3,'[]'::jsonb);